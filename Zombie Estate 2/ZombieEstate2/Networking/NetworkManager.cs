using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProtoBuf;
using Steamworks;
using ZombieEstate2.Networking.Messages;
using ZombieEstate2.Wave;

namespace ZombieEstate2.Networking
{
	// Token: 0x020001AE RID: 430
	public static class NetworkManager
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x00062DAA File Offset: 0x00060FAA
		public static int Index
		{
			get
			{
				return NetworkManager.mIndex;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x00062DB1 File Offset: 0x00060FB1
		public static bool AmIHost
		{
			get
			{
				return NetworkManager.mHost;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00062DB8 File Offset: 0x00060FB8
		public static Dictionary<ushort, SyncedObject> NetObjects
		{
			get
			{
				return NetworkManager.mNetObjects;
			}
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00062DC0 File Offset: 0x00060FC0
		public static void AddNetObject(SyncedObject who)
		{
			if (NetworkManager.mNetObjects.ContainsKey(who.UID))
			{
				Terminal.WriteMessage(string.Concat(new object[]
				{
					"UID exists! UID: ",
					who.UID,
					"| Trying to add type: ",
					who.GetType(),
					" | Type there: ",
					NetworkManager.mNetObjects[who.UID].GetType()
				}), MessageType.ERROR);
				return;
			}
			NetworkManager.mNetObjects.Add(who.UID, who);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00062E49 File Offset: 0x00061049
		public static void RemoveNetObject(SyncedObject who)
		{
			NetworkManager.mRemoveList.Add(who);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00062E58 File Offset: 0x00061058
		public static void SetHost(bool amIHost)
		{
			if (amIHost)
			{
				Terminal.WriteMessage("I am now the host. Index: " + NetworkManager.mIndex);
			}
			else
			{
				Terminal.WriteMessage("I am now NOT the host. Index: " + NetworkManager.mIndex);
			}
			NetworkManager.mHost = amIHost;
			NetworkManager.SetIndex(NetworkManager.mIndex);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x00062EAC File Offset: 0x000610AC
		public static void SetIndex(int index)
		{
			NetworkManager.mIndex = index;
			NetworkManager.LOCAL_UID_MIN = (ushort)((int)NetworkManager.UID_START + index * (int)NetworkManager.UID_RANGE);
			NetworkManager.LOCAL_UID_MAX = (ushort)(NetworkManager.LOCAL_UID_MIN + NetworkManager.UID_RANGE - 25);
			NetworkManager.mCurrentUID = NetworkManager.LOCAL_UID_MIN;
			Terminal.WriteMessage(string.Concat(new object[]
			{
				"Local UID Min: ",
				NetworkManager.LOCAL_UID_MIN,
				" | Local UID Max: ",
				NetworkManager.LOCAL_UID_MAX
			}));
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00062F2C File Offset: 0x0006112C
		public static void Init(int index, bool host)
		{
			if (NetworkManager.mInited)
			{
				Terminal.WriteMessage("Error: NetManager init'ed twice.", MessageType.ERROR);
				return;
			}
			NetworkManager.mHost = host;
			NetworkManager.SetIndex(index);
			NetworkManager.mP2PSessionRequest = Callback<P2PSessionRequest_t>.Create(new Callback<P2PSessionRequest_t>.DispatchDelegate(NetworkManager.OnP2PSessionRequest));
			NetworkManager.mP2PSessionConnectFail = Callback<P2PSessionConnectFail_t>.Create(new Callback<P2PSessionConnectFail_t>.DispatchDelegate(NetworkManager.OnP2PSessionConnectFail));
			NetworkManager.mSocketStatusCallback_t = Callback<SocketStatusCallback_t>.Create(new Callback<SocketStatusCallback_t>.DispatchDelegate(NetworkManager.OnSocketStatusCallback));
			NetworkManager.mGameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(NetworkManager.OnLobbyJoinRequested));
			NetworkManager.mNetObjects = new Dictionary<ushort, SyncedObject>();
			PingManager.Init();
			NetworkManager.mInited = true;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00062FC8 File Offset: 0x000611C8
		public static void Update(float elapsed)
		{
			try
			{
				NetworkManager.UpdateIncomingMessages();
				NetworkManager.UpdateSyncObjects(elapsed);
				PingManager.Update(elapsed);
				NetworkManager.mTimeSinceLastSecond += elapsed;
				if (NetworkManager.mTimeSinceLastSecond >= 1f)
				{
					NetworkManager.mBytesSentLastSecond = NetworkManager.mBytesSentThisSecond;
					NetworkManager.mBytesSentThisSecond = 0;
					NetworkManager.mTimeSinceLastSecond = 0f;
				}
				if (NetworkManager.mMessage != null)
				{
					NetworkManager.mMessage.Update();
					if (!NetworkManager.mMessage.ACTIVE)
					{
						NetworkManager.mMessage = null;
					}
				}
			}
			catch (Exception ex)
			{
				Terminal.WriteMessage("EXCEPTION THROWN IN NETWORKMANAGER UPDATE. " + ex.ToString());
				throw ex;
			}
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00063068 File Offset: 0x00061268
		public static void Draw(SpriteBatch spritebatch)
		{
			if (NetworkManager.mMessage != null && Global.GameState == GameState.Playing)
			{
				NetworkManager.mMessage.Draw(spritebatch);
			}
			if (NetworkManager.DrawDiag)
			{
				Shadow.DrawString("Bytes: " + NetworkManager.mBytesSentLastSecond, Global.StoreFontBig, new Vector2(100f, (float)(Global.ScreenRect.Height - 44)), 1, Color.White, spritebatch);
				if (Global.GameState == GameState.Playing)
				{
					Shadow.DrawString("Pings: " + PingManager.GetPings(), Global.StoreFontBig, new Vector2(400f, (float)(Global.ScreenRect.Height - 44)), 1, Color.White, spritebatch);
				}
			}
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00063114 File Offset: 0x00061314
		private static void UpdateIncomingMessages()
		{
			bool flag = true;
			while (flag)
			{
				uint num;
				flag = SteamNetworking.IsP2PPacketAvailable(out num, 0);
				if (!flag)
				{
					return;
				}
				byte[] array = new byte[num];
				uint num2;
				CSteamID id;
				flag = SteamNetworking.ReadP2PPacket(array, num, out num2, out id, 0);
				if (!flag)
				{
					return;
				}
				if (PlayerManager.GetIndex(id) == -1)
				{
					Terminal.WriteMessage("Got p2p packet from unknown dude. Bailing for now...", MessageType.ERROR);
				}
				else
				{
					NetMessageType netMessageType = (NetMessageType)array[0];
					try
					{
						if (netMessageType == NetMessageType.Ping)
						{
							PingManager.GotPing(id);
						}
						else if (netMessageType == NetMessageType.Pong)
						{
							PingManager.GotPong(id);
						}
						else
						{
							if (netMessageType == NetMessageType.WeaponList)
							{
								NetPayload netPayload = NetworkManager.ExtractPayload(array, false);
								Msg_GunListUpdate msg_GunListUpdate = netPayload as Msg_GunListUpdate;
								if (msg_GunListUpdate.PlayerIndex >= 4)
								{
									Minion minion = PlayerManager.TryToGetMinion((int)msg_GunListUpdate.PlayerIndex);
									if (minion == null)
									{
										continue;
									}
									minion.RetrieveGunList(netPayload as Msg_GunListUpdate);
									continue;
								}
								else if (PlayerManager.GetPlayer((int)msg_GunListUpdate.PlayerIndex) != null && PlayerManager.GetPlayer((int)msg_GunListUpdate.PlayerIndex).PlayerObject != null)
								{
									PlayerManager.GetPlayer((int)msg_GunListUpdate.PlayerIndex).PlayerObject.RetrieveGunList(netPayload as Msg_GunListUpdate);
								}
							}
							if (netMessageType == NetMessageType.WeaponFired)
							{
								NetPayload netPayload2 = NetworkManager.ExtractPayload(array, false);
								Msg_GunFired msg_GunFired = netPayload2 as Msg_GunFired;
								if (msg_GunFired.PlayerIndex >= 4)
								{
									Minion minion2 = PlayerManager.TryToGetMinion((int)msg_GunFired.PlayerIndex);
									if (minion2 == null)
									{
										continue;
									}
									minion2.FireBulletFromNet(netPayload2 as Msg_GunFired);
									continue;
								}
								else if (PlayerManager.GetPlayer((int)msg_GunFired.PlayerIndex) != null && PlayerManager.GetPlayer((int)msg_GunFired.PlayerIndex).PlayerObject != null)
								{
									PlayerManager.GetPlayer((int)msg_GunFired.PlayerIndex).PlayerObject.FireBulletFromNet(netPayload2 as Msg_GunFired);
								}
							}
							if (netMessageType == NetMessageType.WaveStateUpdate)
							{
								if (Global.WaveMaster == null)
								{
									continue;
								}
								NetPayload netPayload3 = NetworkManager.ExtractPayload(array, false);
								Global.WaveMaster.ReceiveNetWaveState(netPayload3 as Msg_WaveStateUpdate);
							}
							if (netMessageType == NetMessageType.PlayerWaveReadyUpdate)
							{
								if (Global.WaveMaster == null)
								{
									continue;
								}
								Msg_PlayerReadyStateUpdate msg_PlayerReadyStateUpdate = NetworkManager.ExtractPayload(array, false) as Msg_PlayerReadyStateUpdate;
								if (PlayerManager.GetPlayer((int)msg_PlayerReadyStateUpdate.Index) != null)
								{
									PlayerManager.GetPlayer((int)msg_PlayerReadyStateUpdate.Index).PlayerObject.READY = msg_PlayerReadyStateUpdate.READY;
									if (PlayerManager.GetPlayer((int)msg_PlayerReadyStateUpdate.Index).PlayerObject.XboxHUD != null)
									{
										PlayerManager.GetPlayer((int)msg_PlayerReadyStateUpdate.Index).PlayerObject.XboxHUD.TriggerReady(msg_PlayerReadyStateUpdate.READY);
									}
									if (msg_PlayerReadyStateUpdate.READY)
									{
										SoundEngine.PlaySound("ze2_navup", 0.15f);
									}
									else
									{
										SoundEngine.PlaySound("ze2_navdown", 0.15f);
									}
								}
							}
							if (netMessageType == NetMessageType.SpawnZombie)
							{
								if (Global.GameState != GameState.Playing)
								{
									Terminal.WriteMessage("Warning: Got spawn zombie message while in gamestate: " + Global.GameState, MessageType.ERROR);
									continue;
								}
								Msg_SpawnZombie msg_SpawnZombie = NetworkManager.ExtractPayload(array, false) as Msg_SpawnZombie;
								Zombie zombie = ZombieSpawner.GenerateZombie(msg_SpawnZombie.DiffMod, new Vector2(msg_SpawnZombie.XPosition, msg_SpawnZombie.ZPosition), (ZombieType)msg_SpawnZombie.ZombieType, msg_SpawnZombie.UID, msg_SpawnZombie.DropUID, msg_SpawnZombie.Seed);
								if (zombie is BrainZombie)
								{
									(zombie as BrainZombie).InitiateBrianUIDs(msg_SpawnZombie.Special[0], msg_SpawnZombie.Special[1], msg_SpawnZombie.Special[2]);
								}
							}
							if (netMessageType == NetMessageType.ZombieKilled)
							{
								if (Global.GameState != GameState.Playing)
								{
									Terminal.WriteMessage("Warning: Got kill zombie message while in gamestate: " + Global.GameState, MessageType.ERROR);
									continue;
								}
								Msg_ZombieKilled msg_ZombieKilled = NetworkManager.ExtractPayload(array, false) as Msg_ZombieKilled;
								if (!NetworkManager.mNetObjects.ContainsKey(msg_ZombieKilled.UID))
								{
									continue;
								}
								Zombie zombie2 = NetworkManager.mNetObjects[msg_ZombieKilled.UID] as Zombie;
								if (zombie2 == null)
								{
									continue;
								}
								if (zombie2.Health <= 0f || !zombie2.Active)
								{
									continue;
								}
								GameObject gameObject = null;
								if (NetworkManager.mNetObjects.ContainsKey(msg_ZombieKilled.KillerUID))
								{
									gameObject = (NetworkManager.mNetObjects[msg_ZombieKilled.KillerUID] as GameObject);
								}
								zombie2.Killed(gameObject as Shootable, true);
							}
							if (netMessageType == NetMessageType.BulletDestroyed)
							{
								Msg_BulletDestroyed msg_BulletDestroyed = NetworkManager.ExtractPayload(array, false) as Msg_BulletDestroyed;
								if (!NetworkManager.mNetObjects.ContainsKey(msg_BulletDestroyed.BulletUID))
								{
									continue;
								}
								Bullet bullet = NetworkManager.mNetObjects[msg_BulletDestroyed.BulletUID] as Bullet;
								if (bullet == null)
								{
									Terminal.WriteMessage("BulletDestroyed: Got a non-bullet obj with UID: " + msg_BulletDestroyed.BulletUID, MessageType.ERROR);
									continue;
								}
								bullet.Position.X = msg_BulletDestroyed.BulletX;
								bullet.Position.Z = msg_BulletDestroyed.BulletZ;
								bullet.DestroyObject();
							}
							if (netMessageType == NetMessageType.PlayerJump)
							{
								ushort num3 = BitConverter.ToUInt16(array, 1);
								if (!NetworkManager.mNetObjects.ContainsKey(num3))
								{
									Terminal.WriteMessage("Received JUMP for unknown UID: " + num3, MessageType.AI);
									continue;
								}
								Player player = NetworkManager.mNetObjects[num3] as Player;
								if (player != null)
								{
									player.Jump();
								}
							}
							if (netMessageType == NetMessageType.PlayerWaveStats)
							{
								if (Global.WaveMaster == null)
								{
									continue;
								}
								Msg_PlayerWaveStats msg_PlayerWaveStats = NetworkManager.ExtractPayload(array, false) as Msg_PlayerWaveStats;
								if (PlayerManager.GetPlayer((int)msg_PlayerWaveStats.PlayerIndex) != null)
								{
									PlayerManager.GetPlayer((int)msg_PlayerWaveStats.PlayerIndex).PlayerObject.GetWaveStats(msg_PlayerWaveStats);
								}
							}
							if (netMessageType == NetMessageType.DropPickedUp)
							{
								Msg_DropPickedUp msg_DropPickedUp = NetworkManager.ExtractPayload(array, false) as Msg_DropPickedUp;
								if (NetworkManager.mNetObjects.ContainsKey(msg_DropPickedUp.PickUpperUID) && NetworkManager.mNetObjects.ContainsKey(msg_DropPickedUp.DropUID))
								{
									Player player2 = NetworkManager.mNetObjects[msg_DropPickedUp.PickUpperUID] as Player;
									Drop drop = NetworkManager.mNetObjects[msg_DropPickedUp.DropUID] as Drop;
									if (player2 != null && drop != null)
									{
										drop.GiveDrop(player2);
									}
								}
							}
							if (netMessageType == NetMessageType.PlayerReloaded)
							{
								Msg_GenericUID msg_GenericUID = NetworkManager.ExtractPayload(array, false) as Msg_GenericUID;
								if (NetworkManager.mNetObjects.ContainsKey(msg_GenericUID.UID))
								{
									Player player3 = NetworkManager.mNetObjects[msg_GenericUID.UID] as Player;
									if (player3 != null)
									{
										player3.mGun.VisualReload();
									}
								}
							}
							if (netMessageType == NetMessageType.PlayerHUDUpdate)
							{
								Msg_PlayerHUDUpdate msg_PlayerHUDUpdate = NetworkManager.ExtractPayload(array, false) as Msg_PlayerHUDUpdate;
								if (NetworkManager.mNetObjects.ContainsKey(msg_PlayerHUDUpdate.UID))
								{
									Player player4 = NetworkManager.mNetObjects[msg_PlayerHUDUpdate.UID] as Player;
									if (player4 != null)
									{
										player4.ReceiveHUDData(msg_PlayerHUDUpdate);
									}
								}
							}
							if (netMessageType == NetMessageType.SpawnEffect)
							{
								SpawnEffectManager.Spawn(NetworkManager.ExtractPayload(array, false) as Msg_SpawnEffect);
							}
							if (netMessageType == NetMessageType.ChatMsg)
							{
								Msg_ChatMsg msg_ChatMsg = NetworkManager.ExtractPayload(array, false) as Msg_ChatMsg;
								if (Global.GameChat != null)
								{
									Global.GameChat.TextEntered(msg_ChatMsg.Message, id);
								}
							}
							if (netMessageType == NetMessageType.PlayerLeft)
							{
								Msg_PlayerLeft msg_PlayerLeft = NetworkManager.ExtractPayload(array, false) as Msg_PlayerLeft;
								PlayerInfo player5 = PlayerManager.GetPlayer(msg_PlayerLeft.Index);
								if (player5 == null)
								{
									Terminal.WriteMessage("Player disconnected, but I don't see him. Might have left already.", MessageType.ERROR);
									continue;
								}
								Terminal.WriteMessage("Player left message! Index: " + msg_PlayerLeft.Index, MessageType.IMPORTANTEVENT);
								PlayerManager.PlayerDisconnected(player5.SteamID, true);
							}
							if (netMessageType == NetMessageType.Dance)
							{
								Msg_Dance msg_Dance = NetworkManager.ExtractPayload(array, false) as Msg_Dance;
								PlayerInfo player6 = PlayerManager.GetPlayer((int)msg_Dance.Index);
								if (player6 != null)
								{
									Player playerObject = player6.PlayerObject;
									if (playerObject != null)
									{
										playerObject.SetDanceState(msg_Dance.State);
									}
								}
							}
							if (netMessageType == NetMessageType.Data)
							{
								ushort num4 = BitConverter.ToUInt16(array, 1);
								if (!NetworkManager.mNetObjects.ContainsKey(num4))
								{
									Terminal.WriteMessage("Received update for unknown UID: " + num4, MessageType.AI);
								}
								else
								{
									NetPayload netPayload4 = NetworkManager.ExtractPayload(array, true);
									if (netPayload4 == null)
									{
										Terminal.WriteMessage("Null payload in net msg", MessageType.ERROR);
										break;
									}
									NetworkManager.mNetObjects[num4].ReceiveSyncMsg(netPayload4);
								}
							}
						}
					}
					catch (Exception ex)
					{
						Terminal.WriteMessage(string.Concat(new object[]
						{
							"Error processing incoming msg of type ",
							netMessageType.ToString(),
							" ",
							ex.GetType()
						}), MessageType.ERROR);
						Terminal.WriteMessage(ex.ToString(), MessageType.ERROR);
					}
				}
			}
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00063900 File Offset: 0x00061B00
		private static NetPayload ExtractPayload(byte[] msg, bool containsUID)
		{
			int num = 3;
			if (!containsUID)
			{
				num = 1;
			}
			int num2 = msg.Length - num;
			if (num2 <= 0)
			{
				return null;
			}
			byte[] array = new byte[num2];
			Array.Copy(msg, num, array, 0, num2);
			return Serializer.Deserialize<NetPayload>(new MemoryStream(array));
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0006393C File Offset: 0x00061B3C
		private static void UpdateSyncObjects(float elapsed)
		{
			foreach (SyncedObject syncedObject in NetworkManager.mNetObjects.Values)
			{
				if (PlayerManager.IsIndexLocal(syncedObject.OwnerIndex, syncedObject))
				{
					syncedObject.UpdateNet(elapsed);
					if (!(syncedObject as GameObject).Active)
					{
						NetworkManager.mRemoveList.Add(syncedObject);
					}
				}
			}
			foreach (SyncedObject syncedObject2 in NetworkManager.mRemoveList)
			{
				if (NetworkManager.mNetObjects.ContainsKey(syncedObject2.UID))
				{
					NetworkManager.mNetObjects.Remove(syncedObject2.UID);
				}
			}
			NetworkManager.mRemoveList.Clear();
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00063A20 File Offset: 0x00061C20
		public static void MigrateOwnership(int fromIndex, int toIndex)
		{
			foreach (SyncedObject syncedObject in NetworkManager.mNetObjects.Values)
			{
				if (syncedObject.OwnerIndex == fromIndex)
				{
					syncedObject.OwnerIndex = toIndex;
				}
			}
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00063A80 File Offset: 0x00061C80
		public static void SetMessage(string msg, bool persistent)
		{
			NetworkManager.mMessage = new MessageOverlay(msg, persistent);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00063A8E File Offset: 0x00061C8E
		public static void ClearMessage()
		{
			NetworkManager.mMessage = null;
			Global.Paused = false;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00063A9C File Offset: 0x00061C9C
		public static void SendMessage(NetMessage msg, SendType method)
		{
			byte[] array = msg.ToByteArray();
			int num = array.Length;
			for (int i = 0; i < 4; i++)
			{
				PlayerInfo player = PlayerManager.GetPlayer(i);
				if (player != null && !player.Local && !player.Guest)
				{
					if (!player.SteamID.IsValid())
					{
						Terminal.WriteMessage("Invalid SteamID on Send index " + i, MessageType.ERROR);
					}
					else
					{
						NetworkManager.mBytesSentThisSecond += num;
						bool flag;
						switch (method)
						{
						case SendType.Reliable:
							flag = SteamNetworking.SendP2PPacket(player.SteamID, array, (uint)array.Length, EP2PSend.k_EP2PSendReliable, 0);
							break;
						case SendType.ReliableBuffered:
							flag = SteamNetworking.SendP2PPacket(player.SteamID, array, (uint)array.Length, EP2PSend.k_EP2PSendReliableWithBuffering, 0);
							break;
						case SendType.Unreliable:
							flag = SteamNetworking.SendP2PPacket(player.SteamID, array, (uint)array.Length, EP2PSend.k_EP2PSendUnreliable, 0);
							break;
						default:
							flag = SteamNetworking.SendP2PPacket(player.SteamID, array, (uint)array.Length, EP2PSend.k_EP2PSendReliable, 0);
							break;
						}
						if (!flag)
						{
							Terminal.WriteMessage("NetworkManager: Error sending msg to player index: " + i, MessageType.ERROR);
						}
					}
				}
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00063BA0 File Offset: 0x00061DA0
		public static void CloseAllConnections()
		{
			for (int i = 0; i < 4; i++)
			{
				NetworkManager.CloseConnection(PlayerManager.GetPlayer(i));
			}
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00063BC4 File Offset: 0x00061DC4
		public static void CloseConnection(PlayerInfo info)
		{
			if (info == null || info.Local || info.Guest)
			{
				return;
			}
			bool flag = SteamNetworking.CloseP2PSessionWithUser(info.SteamID);
			Terminal.WriteMessage(string.Concat(new object[]
			{
				"Closing P2P Session with ",
				info.SteamID,
				" returned ",
				flag.ToString()
			}));
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00063C2F File Offset: 0x00061E2F
		public static void ClearAllObjects()
		{
			NetworkManager.mNetObjects.Clear();
			NetworkManager.mRemoveList.Clear();
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00063C48 File Offset: 0x00061E48
		private static void OnP2PSessionRequest(P2PSessionRequest_t pCallback)
		{
			Terminal.WriteMessage("OnP2PSessionRequest");
			Terminal.WriteMessage("     ID: " + pCallback.m_steamIDRemote);
			Terminal.WriteMessage("     Name: " + SteamFriends.GetFriendPersonaName(pCallback.m_steamIDRemote));
			if (NetworkManager.REJECT_INCOMING_MSGS)
			{
				return;
			}
			Terminal.WriteMessage("     AcceptP2PSession returned: " + SteamNetworking.AcceptP2PSessionWithUser(pCallback.m_steamIDRemote).ToString());
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00063CC3 File Offset: 0x00061EC3
		private static void OnP2PSessionConnectFail(P2PSessionConnectFail_t pCallback)
		{
			Terminal.WriteMessage("OnP2PSessionConnectFail:");
			Terminal.WriteMessage("     Error: " + pCallback.m_eP2PSessionError);
			Terminal.WriteMessage("     ID: " + pCallback.m_steamIDRemote);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00063D03 File Offset: 0x00061F03
		private static void OnSocketStatusCallback(SocketStatusCallback_t pCallback)
		{
			Terminal.WriteMessage("OnSocketStatusCallback");
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00063D10 File Offset: 0x00061F10
		private static void OnLobbyJoinRequested(GameLobbyJoinRequested_t pCallback)
		{
			Terminal.WriteMessage(string.Concat(new object[]
			{
				"Lobby invite received from friend: ",
				pCallback.m_steamIDFriend,
				" for lobby: ",
				pCallback.m_steamIDLobby
			}));
			if (Global.GameState == GameState.MainMenu || Global.GameState == GameState.WaitingForOtherPlayers || Global.GameState == GameState.LevelSelect || Global.GameState == GameState.CharacterSelect || Global.GameState == GameState.PressStart)
			{
				Terminal.WriteMessage("Attempting to join lobby from invite.");
				MainMenu.HOSTING_GAME = false;
				MainMenu.LOBBY_TO_JOIN = pCallback.m_steamIDLobby;
				MenuManager.CLOSEALL();
				Global.GameManager.GotoCharSelect();
				return;
			}
			Terminal.WriteMessage("Not joining - mid game.");
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00063DBC File Offset: 0x00061FBC
		private static void Test()
		{
			int num = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < 50000; i++)
			{
				byte[] array = NetworkManager.TestSend();
				num += array.Length;
				NetworkManager.TestReceive(array);
			}
			Console.WriteLine(string.Concat(new object[]
			{
				"ALL DONE: ",
				stopwatch.Elapsed.TotalSeconds,
				" Total Bytes: ",
				num
			}));
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00063E34 File Offset: 0x00062034
		public static void SendPing(CSteamID id)
		{
			NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.Ping);
			netMessage.Payload = null;
			NetworkManager.SendMessage(netMessage, SendType.Reliable);
			byte[] array = netMessage.ToByteArray();
			int num = array.Length;
			NetworkManager.mBytesSentThisSecond += num;
			SteamNetworking.SendP2PPacket(id, array, (uint)array.Length, EP2PSend.k_EP2PSendUnreliable, 0);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00063E78 File Offset: 0x00062078
		public static void SendPong(CSteamID id)
		{
			NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.Pong);
			netMessage.Payload = null;
			NetworkManager.SendMessage(netMessage, SendType.Reliable);
			byte[] array = netMessage.ToByteArray();
			int num = array.Length;
			NetworkManager.mBytesSentThisSecond += num;
			SteamNetworking.SendP2PPacket(id, array, (uint)array.Length, EP2PSend.k_EP2PSendUnreliable, 0);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00063EBC File Offset: 0x000620BC
		public static byte[] TestSend()
		{
			Random random = new Random();
			NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.Data);
			netMessage.UID = 1234;
			Msg_GameObjectUpdate msg_GameObjectUpdate = new Msg_GameObjectUpdate();
			if (random.Next(0, 100) < 50)
			{
				msg_GameObjectUpdate.PositionX = 123.4f;
				msg_GameObjectUpdate.PositionXChanged = true;
			}
			if (random.Next(0, 100) < 50)
			{
				msg_GameObjectUpdate.PositionZ = 456f;
				msg_GameObjectUpdate.PositionZChanged = true;
			}
			netMessage.Payload = msg_GameObjectUpdate;
			return netMessage.ToByteArray();
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00063F30 File Offset: 0x00062130
		public static void TestReceive(byte[] msg)
		{
			if (msg[0] == 3)
			{
				BitConverter.ToUInt16(msg, 1);
				int num = msg.Length - 3;
				if (num <= 0)
				{
					return;
				}
				byte[] array = new byte[num];
				Array.Copy(msg, 3, array, 0, num);
				Serializer.Deserialize<Msg_GameObjectUpdate>(new MemoryStream(array));
			}
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00002EF9 File Offset: 0x000010F9
		public static void HandleWeaponListUpdate(NetMessage msg)
		{
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00063F73 File Offset: 0x00062173
		public static ushort GetMachineUID()
		{
			NetworkManager.IncrementUID();
			return NetworkManager.mCurrentUID;
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00063F80 File Offset: 0x00062180
		public static void IncrementUID()
		{
			NetworkManager.mCurrentUID += 1;
			if (NetworkManager.mCurrentUID >= NetworkManager.LOCAL_UID_MAX)
			{
				NetworkManager.mCurrentUID = NetworkManager.LOCAL_UID_MIN;
				Terminal.WriteMessage("UID Rolled", MessageType.IMPORTANTEVENT);
			}
			while (NetworkManager.NetObjects.ContainsKey(NetworkManager.mCurrentUID))
			{
				NetworkManager.mCurrentUID += 1;
				if (NetworkManager.mCurrentUID >= NetworkManager.LOCAL_UID_MAX)
				{
					NetworkManager.mCurrentUID = NetworkManager.LOCAL_UID_MIN;
					Terminal.WriteMessage("UID Rolled in while", MessageType.IMPORTANTEVENT);
				}
			}
		}

		// Token: 0x04000C50 RID: 3152
		public static bool DrawDiag = false;

		// Token: 0x04000C51 RID: 3153
		private static ushort UID_RANGE = 15000;

		// Token: 0x04000C52 RID: 3154
		private static ushort UID_START = 100;

		// Token: 0x04000C53 RID: 3155
		private static ushort LOCAL_UID_MIN;

		// Token: 0x04000C54 RID: 3156
		private static ushort LOCAL_UID_MAX;

		// Token: 0x04000C55 RID: 3157
		private static int mIndex = 0;

		// Token: 0x04000C56 RID: 3158
		private static int mBytesSentThisSecond = 0;

		// Token: 0x04000C57 RID: 3159
		private static int mBytesSentLastSecond = 0;

		// Token: 0x04000C58 RID: 3160
		private static float mTimeSinceLastSecond = 0f;

		// Token: 0x04000C59 RID: 3161
		public static bool REJECT_INCOMING_MSGS = false;

		// Token: 0x04000C5A RID: 3162
		private static bool mHost = false;

		// Token: 0x04000C5B RID: 3163
		private static Dictionary<ushort, SyncedObject> mNetObjects;

		// Token: 0x04000C5C RID: 3164
		private static List<SyncedObject> mRemoveList = new List<SyncedObject>();

		// Token: 0x04000C5D RID: 3165
		private static ushort mCurrentUID = 0;

		// Token: 0x04000C5E RID: 3166
		private static Callback<P2PSessionRequest_t> mP2PSessionRequest;

		// Token: 0x04000C5F RID: 3167
		private static Callback<P2PSessionConnectFail_t> mP2PSessionConnectFail;

		// Token: 0x04000C60 RID: 3168
		private static Callback<SocketStatusCallback_t> mSocketStatusCallback_t;

		// Token: 0x04000C61 RID: 3169
		private static Callback<GameLobbyJoinRequested_t> mGameLobbyJoinRequested;

		// Token: 0x04000C62 RID: 3170
		private static MessageOverlay mMessage;

		// Token: 0x04000C63 RID: 3171
		private static bool mInited = false;
	}
}
