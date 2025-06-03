using System;
using Steamworks;

namespace ZombieEstate2.Networking
{
	// Token: 0x020001AF RID: 431
	public static class PingManager
	{
		// Token: 0x06000C29 RID: 3113 RVA: 0x00002EF9 File Offset: 0x000010F9
		public static void Init()
		{
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00064060 File Offset: 0x00062260
		public static void Update(float elapsed)
		{
			if (NetworkManager.REJECT_INCOMING_MSGS || Global.GameState != GameState.Playing || PingManager.DISABLE_PINGS)
			{
				return;
			}
			PingManager.mNextPingTime += elapsed;
			for (int i = 0; i < 4; i++)
			{
				PlayerInfo player = PlayerManager.GetPlayer(i);
				if (player != null && player.WaitingOnPong)
				{
					player.TimeSinceLastPing += elapsed;
					if (player.HaveGottenOnePing && Global.GameState == GameState.Playing)
					{
						if (player.TimeSinceLastPing >= PingManager.DISCONNECTED_TIME)
						{
							Terminal.WriteMessage("!!! Player disconnected! " + player.SteamID, MessageType.ERROR);
							PlayerManager.PlayerDisconnected(player.SteamID, false);
							PingManager.HostAlert = false;
						}
						else if (!PingManager.HostAlert && player.Index == 0 && player.TimeSinceLastPing >= PingManager.HOST_DISCONNECT_ALERT)
						{
							NetworkManager.SetMessage("Host Disconnected. Waiting to try to reconnect...", true);
							PingManager.HostAlert = true;
						}
					}
				}
			}
			if (PingManager.mNextPingTime >= PingManager.TIME_BETWEEN_PINGS)
			{
				for (int j = 0; j < 4; j++)
				{
					PlayerInfo player2 = PlayerManager.GetPlayer(j);
					if (player2 != null && !player2.Local && !player2.Guest)
					{
						if (!player2.WaitingOnPong)
						{
							player2.TimeSinceLastPing = 0f;
						}
						player2.WaitingOnPong = true;
						NetworkManager.SendPing(player2.SteamID);
					}
				}
				PingManager.mNextPingTime = 0f;
			}
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x000641A0 File Offset: 0x000623A0
		public static void GotPing(CSteamID id)
		{
			if (NetworkManager.REJECT_INCOMING_MSGS)
			{
				return;
			}
			NetworkManager.SendPong(id);
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000641B0 File Offset: 0x000623B0
		public static void GotPong(CSteamID id)
		{
			PlayerInfo player = PlayerManager.GetPlayer(id);
			if (player == null)
			{
				Terminal.WriteMessage("Error: Got pong from unknown ID: " + id, MessageType.ERROR);
				return;
			}
			player.WaitingOnPong = false;
			player.HaveGottenOnePing = true;
			if (player.Index == 0 && PingManager.HostAlert)
			{
				NetworkManager.ClearMessage();
				PingManager.HostAlert = false;
			}
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00064208 File Offset: 0x00062408
		public static string GetPings()
		{
			return string.Format("0: {0,8} | 1: {1,8} | 2: {2,8} | 3: {3,8}", new object[]
			{
				(PlayerManager.GetPlayer(0) == null || PlayerManager.GetPlayer(0).Local) ? " " : PlayerManager.GetPlayer(0).TimeSinceLastPing.ToString("0.0"),
				(PlayerManager.GetPlayer(1) == null || PlayerManager.GetPlayer(1).Local) ? " " : PlayerManager.GetPlayer(1).TimeSinceLastPing.ToString("0.0"),
				(PlayerManager.GetPlayer(2) == null || PlayerManager.GetPlayer(2).Local) ? " " : PlayerManager.GetPlayer(2).TimeSinceLastPing.ToString("0.0"),
				(PlayerManager.GetPlayer(3) == null || PlayerManager.GetPlayer(3).Local) ? " " : PlayerManager.GetPlayer(3).TimeSinceLastPing.ToString("0.0")
			});
		}

		// Token: 0x04000C64 RID: 3172
		private static float mNextPingTime = 0f;

		// Token: 0x04000C65 RID: 3173
		private static float TIME_BETWEEN_PINGS = 2f;

		// Token: 0x04000C66 RID: 3174
		private static float DISCONNECTED_TIME = 10f;

		// Token: 0x04000C67 RID: 3175
		private static float HOST_DISCONNECT_ALERT = 3f;

		// Token: 0x04000C68 RID: 3176
		private static bool HostAlert = false;

		// Token: 0x04000C69 RID: 3177
		public static bool DISABLE_PINGS = false;
	}
}
