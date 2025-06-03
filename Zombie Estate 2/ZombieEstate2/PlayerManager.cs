using System;
using System.Threading;
using Steamworks;
using ZombieEstate2.Networking;

namespace ZombieEstate2
{
	// Token: 0x02000088 RID: 136
	public static class PlayerManager
	{
		// Token: 0x0600032E RID: 814 RVA: 0x00018E3C File Offset: 0x0001703C
		public static void AddPlayer(int index, bool local, CSteamID id)
		{
			Terminal.WriteMessage("Player Manager - Adding Player:");
			Terminal.WriteMessage("     Name: " + SteamFriends.GetFriendPersonaName(id));
			Terminal.WriteMessage("     Index: " + index);
			Terminal.WriteMessage("     Local: " + local.ToString());
			Terminal.WriteMessage("--------------------------------");
			if (PlayerManager.mPlayers[index] != null)
			{
				Terminal.WriteMessage("ERROR: Player index " + index + " already exists!", MessageType.ERROR);
				return;
			}
			PlayerInfo playerInfo = new PlayerInfo();
			playerInfo.Index = index;
			playerInfo.Local = local;
			playerInfo.SteamID = id;
			PlayerManager.mPlayers[index] = playerInfo;
			playerInfo.GetImage();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00018EF1 File Offset: 0x000170F1
		public static void MovePlayerIndex(PlayerInfo info, int toIndex)
		{
			PlayerManager.mPlayers[info.Index] = null;
			PlayerManager.mPlayers[toIndex] = info;
			info.Index = toIndex;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00018F10 File Offset: 0x00017110
		public static void RemovePlayer(int index)
		{
			if (PlayerManager.mPlayers[index] == null)
			{
				return;
			}
			Terminal.WriteMessage("Player Manager - Removing Player:");
			Terminal.WriteMessage("     Name: " + SteamFriends.GetFriendPersonaName(PlayerManager.mPlayers[index].SteamID));
			Terminal.WriteMessage("     Index: " + index);
			Terminal.WriteMessage("--------------------------------");
			PlayerInfo info = PlayerManager.mPlayers[index];
			CSteamID steamID = PlayerManager.mPlayers[index].SteamID;
			PlayerManager.mPlayers[index] = null;
			bool flag = true;
			for (int i = 0; i < 4; i++)
			{
				PlayerInfo player = PlayerManager.GetPlayer(i);
				if (player != null && player.SteamID == steamID)
				{
					flag = false;
				}
			}
			if (flag)
			{
				NetworkManager.CloseConnection(info);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000331 RID: 817 RVA: 0x00018FC4 File Offset: 0x000171C4
		public static bool GameIsFull
		{
			get
			{
				for (int i = 0; i < 4; i++)
				{
					if (PlayerManager.mPlayers[i] == null)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00018FEC File Offset: 0x000171EC
		public static bool IsIndexLocal(int index, SyncedObject so)
		{
			if (so is Minion)
			{
				index = (so as Minion).parent.OwnerIndex;
			}
			if (PlayerManager.mPlayers[index] == null)
			{
				Terminal.WriteMessage("ERROR: (is local test) - No player at index " + index + "!", MessageType.ERROR);
				return true;
			}
			return PlayerManager.mPlayers[index].Local;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00019045 File Offset: 0x00017245
		public static PlayerInfo GetPlayer(int index)
		{
			return PlayerManager.mPlayers[index];
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00019050 File Offset: 0x00017250
		public static Minion TryToGetMinion(int index)
		{
			for (int i = 0; i < 4; i++)
			{
				if (PlayerManager.mPlayers[i] != null)
				{
					foreach (Minion minion in PlayerManager.mPlayers[i].PlayerObject.GetMinionList)
					{
						if (index == minion.Index)
						{
							return minion;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000190CC File Offset: 0x000172CC
		public static PlayerInfo GetPlayer(CSteamID id)
		{
			for (int i = 0; i < 4; i++)
			{
				if (PlayerManager.mPlayers[i] != null && PlayerManager.mPlayers[i].SteamID.Equals(id))
				{
					return PlayerManager.mPlayers[i];
				}
			}
			return null;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0001910C File Offset: 0x0001730C
		public static PlayerInfo GetPlayer(CSteamID id, int controllerIndex)
		{
			for (int i = 0; i < 4; i++)
			{
				if (PlayerManager.mPlayers[i] != null && PlayerManager.mPlayers[i].SteamID.Equals(id) && PlayerManager.mPlayers[i].ControllerIndex == controllerIndex)
				{
					return PlayerManager.mPlayers[i];
				}
			}
			return null;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0001915C File Offset: 0x0001735C
		public static int GetFirstAvailIndex()
		{
			for (int i = 0; i < 4; i++)
			{
				if (PlayerManager.mPlayers[i] == null)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00019184 File Offset: 0x00017384
		public static int PlayerCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					if (PlayerManager.mPlayers[i] != null)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000191B0 File Offset: 0x000173B0
		public static int GetIndex(CSteamID id)
		{
			for (int i = 0; i < 4; i++)
			{
				if (PlayerManager.mPlayers[i] != null && PlayerManager.mPlayers[i].SteamID.Equals(id))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000191EC File Offset: 0x000173EC
		public static void PrintPlayers()
		{
			Terminal.WriteMessage("Current Players:", MessageType.SAVELOAD);
			for (int i = 0; i < 4; i++)
			{
				if (PlayerManager.mPlayers[i] != null)
				{
					Terminal.WriteMessage("     " + SteamFriends.GetFriendPersonaName(PlayerManager.mPlayers[i].SteamID) + " | " + PlayerManager.mPlayers[i].GetData(), MessageType.SAVELOAD);
				}
				else
				{
					Terminal.WriteMessage("     NULL", MessageType.SAVELOAD);
				}
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00019258 File Offset: 0x00017458
		public static void ClearPlayers()
		{
			NetworkManager.CloseAllConnections();
			for (int i = 0; i < 4; i++)
			{
				PlayerManager.mPlayers[i] = null;
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00019280 File Offset: 0x00017480
		public static bool AllLocalPlayers()
		{
			bool result = true;
			for (int i = 0; i < 4; i++)
			{
				if (PlayerManager.mPlayers[i] != null && !PlayerManager.mPlayers[i].Local)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000192B8 File Offset: 0x000174B8
		public static bool ControllerAlreadyInUse(int controllerIndex)
		{
			for (int i = 0; i < 4; i++)
			{
				if (PlayerManager.mPlayers[i] != null && PlayerManager.mPlayers[i].ControllerIndex == controllerIndex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000192EC File Offset: 0x000174EC
		public static int NumberOfTimesNameInUse(string name)
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				if (PlayerManager.mPlayers[i] != null && SteamFriends.GetFriendPersonaName(PlayerManager.mPlayers[i].SteamID) == name)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00019330 File Offset: 0x00017530
		public static void PlayerDisconnected(CSteamID id, bool left = false)
		{
			Terminal.WriteMessage("Disconnecting player: " + id, MessageType.IMPORTANTEVENT);
			if (id == SteamHelper.GetLocalID())
			{
				Terminal.WriteMessage("Error... 'I' disconnected??", MessageType.ERROR);
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				PlayerInfo player = PlayerManager.GetPlayer(i);
				if (player != null && !(player.SteamID != id))
				{
					string personaName = player.PersonaName;
					bool flag = false;
					if (player.PlayerObject != null)
					{
						Global.PlayerList.Remove(player.PlayerObject);
						player.PlayerObject.RemoveMinions();
						player.PlayerObject.DestroyObject();
						if (player.PlayerObject.mGunObject != null)
						{
							player.PlayerObject.mGunObject.DestroyObject();
						}
						if (player.PlayerObject.mAccessoryObject != null)
						{
							player.PlayerObject.mAccessoryObject.DestroyObject();
						}
						if (player.PlayerObject.mTargetReticule != null)
						{
							player.PlayerObject.mTargetReticule.DestroyObject();
						}
						if (player.Index == 0)
						{
							flag = true;
						}
						PlayerManager.RemovePlayer(player.Index);
						NetworkManager.MigrateOwnership(player.Index, 0);
					}
					if (flag)
					{
						PlayerInfo playerInfo = null;
						for (int j = 0; j < 4; j++)
						{
							playerInfo = PlayerManager.GetPlayer(j);
							if (playerInfo != null && !playerInfo.Guest)
							{
								break;
							}
						}
						Terminal.WriteMessage("New host: " + playerInfo.SteamID, MessageType.IMPORTANTEVENT);
						PlayerManager.mPlayers[playerInfo.Index] = null;
						NetworkManager.MigrateOwnership(playerInfo.Index, 0);
						playerInfo.Index = 0;
						PlayerManager.mPlayers[0] = playerInfo;
						playerInfo.PlayerObject.Index = 0;
						if (playerInfo.Local)
						{
							playerInfo.TimeSinceLastPing = -1f;
							playerInfo.WaitingOnPong = false;
							Terminal.WriteMessage("I'm the new host.", MessageType.IMPORTANTEVENT);
							NetworkManager.SetHost(true);
						}
					}
					Terminal.WriteMessage("Disconnect finished. Remaining players:", MessageType.IMPORTANTEVENT);
					PlayerManager.PrintPlayers();
					Thread.Sleep(100);
					NetworkManager.ClearMessage();
					if (left)
					{
						Global.GameChat.TextEntered_System(personaName + " - Player disconnected!");
					}
					else
					{
						NetworkManager.SetMessage("Player disconnected!", false);
					}
					if (Global.GameState == GameState.Playing && Global.AllPlayersDead())
					{
						Terminal.WriteMessage("No remaining players are alive. Death screen.");
						Global.Paused = true;
						MenuManager.PushMenu(new DeathMenu());
						SteamHelper.PushStats();
					}
				}
			}
		}

		// Token: 0x04000322 RID: 802
		private static PlayerInfo[] mPlayers = new PlayerInfo[4];
	}
}
