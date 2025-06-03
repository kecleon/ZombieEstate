using System;
using System.Collections.Generic;
using System.Globalization;
using Steamworks;
using ZombieEstate2.UI.Menus;
using ZombieEstate2.UI.Xbox;

namespace ZombieEstate2.Networking
{
	// Token: 0x020001AC RID: 428
	public class LobbyManager
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0006123B File Offset: 0x0005F43B
		public bool IAmLobbyCreator
		{
			get
			{
				return this.mIAmLobbyCreator;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x00061243 File Offset: 0x0005F443
		public bool PrivateLobby
		{
			get
			{
				return this.mPrivateLobby;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0006124B File Offset: 0x0005F44B
		public bool HaveIFoundALobby
		{
			get
			{
				return this.mLobbyID.IsValid() || this.mOfflineMode;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x00061262 File Offset: 0x0005F462
		public bool OfflineMode
		{
			get
			{
				return this.mOfflineMode;
			}
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x0006126C File Offset: 0x0005F46C
		public LobbyManager(bool hostALobby, XboxMasterCharacterSelect sel, bool privateLobby, CSteamID lobbyToJoin)
		{
			Global.WAVE_GEN_SEED = Global.rand.Next();
			PingManager.DISABLE_PINGS = false;
			if (GameManager.LevelName == "Any")
			{
				this.mNoMapPreference = true;
			}
			this.mMasterCharSelect = sel;
			this.mPrivateLobby = privateLobby;
			this.mOnLobbyMatchList = CallResult<LobbyMatchList_t>.Create(new CallResult<LobbyMatchList_t>.APIDispatchDelegate(this.OnLobbyMatchList));
			this.mOnLobbyCreated = CallResult<LobbyCreated_t>.Create(new CallResult<LobbyCreated_t>.APIDispatchDelegate(this.OnLobbyCreated));
			this.mOnLobbyEntered = CallResult<LobbyEnter_t>.Create(new CallResult<LobbyEnter_t>.APIDispatchDelegate(this.OnLobbyEntered));
			this.mOnLobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(new Callback<LobbyChatUpdate_t>.DispatchDelegate(this.OnLobbyChatUpdate));
			this.mOnLobbyChatMsg = Callback<LobbyChatMsg_t>.Create(new Callback<LobbyChatMsg_t>.DispatchDelegate(this.OnLobbyChatMsg));
			this.mOnLobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(new Callback<LobbyDataUpdate_t>.DispatchDelegate(this.OnLobbyDataUpdate));
			if (hostALobby)
			{
				this.CreateLobby();
				return;
			}
			if (lobbyToJoin != CSteamID.Nil)
			{
				this.mPreviousLobbyPlayers = new PlayerInfo[4];
				PlayerInfo playerInfo = new PlayerInfo();
				playerInfo.SteamID = SteamHelper.GetLocalID();
				playerInfo.CharacterName = PlayerInfo.NO_SELECTION;
				playerInfo.AccessoryName = PlayerInfo.NO_SELECTION;
				playerInfo.Local = true;
				playerInfo.Guest = false;
				playerInfo.Index = 0;
				playerInfo.ControllerIndex = Global.GameManager.INITIATED_CONTROL_INDEX;
				Global.GameManager.INITIATED_CONTROL_INDEX = -1;
				this.mPreviousLobbyPlayers[0] = playerInfo;
				Terminal.WriteMessage("Joining a lobby from invite (or browser): " + lobbyToJoin);
				NetworkManager.SetHost(false);
				SteamAPICall_t hAPICall = SteamMatchmaking.JoinLobby(lobbyToJoin);
				this.mOnLobbyEntered.Set(hAPICall, null);
				return;
			}
			this.BeginSearchForLobby();
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00061424 File Offset: 0x0005F624
		private void BeginSearchForLobby()
		{
			if (!this.mNoMapPreference)
			{
				Terminal.WriteMessage("Filtering lobby choices to Map: " + GameManager.LevelName);
				SteamMatchmaking.AddRequestLobbyListStringFilter("map", GameManager.LevelName, ELobbyComparison.k_ELobbyComparisonEqual);
			}
			SteamMatchmaking.AddRequestLobbyListStringFilter("version", Global.VersionNumber, ELobbyComparison.k_ELobbyComparisonEqual);
			SteamMatchmaking.AddRequestLobbyListNumericalFilter("difficulty", Global.DIFFICULTY_LEVEL, ELobbyComparison.k_ELobbyComparisonEqual);
			int num = Math.Max(1, PlayerManager.PlayerCount);
			Terminal.WriteMessage("Setting requested player slots to: " + num);
			SteamMatchmaking.AddRequestLobbyListNumericalFilter("openSlots", num, ELobbyComparison.k_ELobbyComparisonEqualToOrGreaterThan);
			SteamAPICall_t hAPICall = SteamMatchmaking.RequestLobbyList();
			this.mOnLobbyMatchList.Set(hAPICall, null);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x000614C0 File Offset: 0x0005F6C0
		public void CreateLobby()
		{
			if (this.mNoMapPreference)
			{
				int index = Global.rand.Next(0, XboxLevelSelect.LevelNames.Count);
				GameManager.LevelName = XboxLevelSelect.LevelNames[index];
			}
			this.mIAmLobbyCreator = true;
			NetworkManager.SetHost(true);
			if (this.mPrivateLobby)
			{
				SteamAPICall_t hAPICall = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePrivate, 4);
				this.mOnLobbyCreated.Set(hAPICall, null);
				return;
			}
			SteamAPICall_t hAPICall2 = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);
			this.mOnLobbyCreated.Set(hAPICall2, null);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0006153C File Offset: 0x0005F73C
		public void Update()
		{
			if (!this.IAmLobbyCreator)
			{
				return;
			}
			if (this.mRefreshSelectionsSendTime > 0f)
			{
				this.mRefreshSelectionsSendTime -= Global.REAL_GAME_TIME;
				if (this.mRefreshSelectionsSendTime <= 0f)
				{
					Terminal.WriteMessage("Sending a refresh of host selections...");
					this.SetLobbyData();
				}
			}
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00061590 File Offset: 0x0005F790
		private void OnLobbyMatchList(LobbyMatchList_t list, bool bIOFailure)
		{
			Terminal.WriteMessage("OnLobbyMatchList");
			Terminal.WriteMessage("     Result: " + list.m_nLobbiesMatching);
			if (bIOFailure)
			{
				Terminal.WriteMessage("     IOFailure: " + bIOFailure.ToString(), MessageType.ERROR);
				return;
			}
			if (list.m_nLobbiesMatching != 0U)
			{
				Terminal.WriteMessage("At least one lobby found - connecting to first index.");
				NetworkManager.SetHost(false);
				CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(0);
				if (this.mLobbyID != CSteamID.Nil)
				{
					Terminal.WriteMessage("***Swapping to a new lobby!", MessageType.IMPORTANTEVENT);
					string text = "NEW LOBBY," + lobbyByIndex.ToString();
					Terminal.WriteMessage(text, MessageType.IMPORTANTEVENT);
					byte[] bytes = LobbyManager.GetBytes(text);
					SteamMatchmaking.SendLobbyChatMsg(this.mLobbyID, bytes, bytes.Length);
					this.JoinNewLobby(lobbyByIndex);
					NetworkManager.SetHost(false);
				}
				SteamAPICall_t hAPICall = SteamMatchmaking.JoinLobby(lobbyByIndex);
				this.mOnLobbyEntered.Set(hAPICall, null);
				return;
			}
			if (this.mLobbyID == CSteamID.Nil)
			{
				Terminal.WriteMessage("No lobby found, and we haven't created one yet - hosting one!");
				NetworkManager.SetHost(true);
				this.CreateLobby();
				this.SetLobbyData();
				return;
			}
			Terminal.WriteMessage("No lobby found - Staying in this one.");
			SteamMatchmaking.SetLobbyType(this.mLobbyID, ELobbyType.k_ELobbyTypePublic);
			NetworkManager.SetHost(true);
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x000616C0 File Offset: 0x0005F8C0
		private void OnLobbyCreated(LobbyCreated_t lobby, bool ioFailure)
		{
			if (lobby.m_eResult == EResult.k_EResultOK)
			{
				this.mLeavingLobby = false;
				Terminal.WriteMessage("Created Lobby");
				Terminal.WriteMessage("     Result: " + lobby.m_eResult);
				Terminal.WriteMessage("     ID: " + lobby.m_ulSteamIDLobby);
				Terminal.WriteMessage("     IOFailure: " + ioFailure.ToString());
				this.mLobbyID = new CSteamID(lobby.m_ulSteamIDLobby);
				NetworkManager.SetHost(true);
				this.mIAmLobbyCreator = true;
				this.SetInitialLobbyData();
				PlayerManager.AddPlayer(0, true, SteamHelper.GetLocalID());
				if (!Global.GameManager.INITIATED_CHAR_SEL_WITH_MOUSE)
				{
					PlayerManager.GetPlayer(0).ControllerIndex = Global.GameManager.INITIATED_CONTROL_INDEX;
				}
				else
				{
					PlayerManager.GetPlayer(0).ControllerIndex = -1;
				}
				Global.GameManager.INITIATED_CONTROL_INDEX = -1;
				return;
			}
			Terminal.WriteMessage("Failed to create Lobby!", MessageType.ERROR);
			Terminal.WriteMessage("     Result: " + lobby.m_eResult, MessageType.ERROR);
			Terminal.WriteMessage("     IOFailure: " + ioFailure.ToString(), MessageType.ERROR);
			this.mOfflineMode = true;
			PlayerManager.AddPlayer(0, true, SteamHelper.GetLocalID());
			if (!Global.GameManager.INITIATED_CHAR_SEL_WITH_MOUSE)
			{
				PlayerManager.GetPlayer(0).ControllerIndex = 0;
			}
			else
			{
				PlayerManager.GetPlayer(0).ControllerIndex = Global.GameManager.INITIATED_CONTROL_INDEX;
			}
			Global.GameManager.INITIATED_CONTROL_INDEX = -1;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00061838 File Offset: 0x0005FA38
		private void OnLobbyEntered(LobbyEnter_t lobby, bool bIOFailure)
		{
			this.CLEAR_STORES();
			this.mMasterCharSelect.WaitingForLobbyUpdate.Restart();
			this.mLeavingLobby = false;
			NetworkManager.SetHost(false);
			this.mIAmLobbyCreator = false;
			PlayerManager.ClearPlayers();
			this.mLobbyOccupantCount = -1;
			Terminal.WriteMessage("OnLobbyEntered");
			Terminal.WriteMessage("     Lobby ID Joined: " + lobby.m_ulSteamIDLobby);
			this.mLobbyID = new CSteamID(lobby.m_ulSteamIDLobby);
			this.mPrivateLobby = lobby.m_bLocked;
			this.UpdateCount();
			this.ConfirmVersions();
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x000618C8 File Offset: 0x0005FAC8
		private void OnLobbyChatUpdate(LobbyChatUpdate_t lobby)
		{
			Terminal.WriteMessage("OnLobbyChatUpdate");
			if (this.mLeavingLobby)
			{
				Terminal.WriteMessage("     Ignoring, currently leaving the lobby");
				return;
			}
			this.UpdateCount();
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x000618F0 File Offset: 0x0005FAF0
		private void OnLobbyChatMsg(LobbyChatMsg_t msg)
		{
			Terminal.WriteMessage("OnLobbyChatMsg");
			if (this.mLeavingLobby)
			{
				Terminal.WriteMessage("OnLobbyChatMsg - Ignoring, leaving lobby.");
				return;
			}
			byte[] array = new byte[4096];
			int iChatID = Convert.ToInt32(msg.m_iChatID);
			CSteamID id;
			EChatEntryType echatEntryType;
			int lobbyChatEntry = SteamMatchmaking.GetLobbyChatEntry(this.mLobbyID, iChatID, out id, array, 4096, out echatEntryType);
			if (echatEntryType == EChatEntryType.k_EChatEntryTypeInvalid || lobbyChatEntry == 0)
			{
				Terminal.WriteMessage("     OnLobbyChatMsg - TYPE INVALID, OR BYTE COUNT = 0", MessageType.ERROR);
				return;
			}
			string @string = LobbyManager.GetString(array, lobbyChatEntry);
			if (@string == "START GAME")
			{
				Terminal.WriteMessage("LAUNCHING GAME!", MessageType.IMPORTANTEVENT);
				if (!this.mOfflineMode)
				{
					this.RemoveCallbacks();
				}
				this.mLeavingLobby = true;
				PlayerManager.PrintPlayers();
				try
				{
					if (PlayerManager.GetPlayer(SteamHelper.GetLocalID()) == null)
					{
						PlayerManager.ClearPlayers();
						this.LeaveLobby();
						this.RemoveCallbacks();
						Global.GameManager.GotoMainMenu();
						MenuManager.PushMenu(new KickedMenu("Error joining lobby. Game may have started without you. :("));
						return;
					}
					this.mMasterCharSelect.Launch();
				}
				catch (Exception ex)
				{
					Terminal.WriteMessage("ERROR STARTING GAME: " + ex.ToString(), MessageType.ERROR);
					throw ex;
				}
				return;
			}
			if (@string.Contains("KICK PLAYER"))
			{
				Terminal.WriteMessage("Someone is being kicked... ooooo", MessageType.IMPORTANTEVENT);
				string[] array2 = @string.Split(new char[]
				{
					','
				});
				CSteamID y = new CSteamID(ulong.Parse(array2[1]));
				if (SteamHelper.GetLocalID() == y)
				{
					Terminal.WriteMessage("I have been kicked.", MessageType.IMPORTANTEVENT);
					PlayerManager.ClearPlayers();
					this.LeaveLobby();
					this.RemoveCallbacks();
					Global.GameManager.GotoMainMenu();
					MenuManager.PushMenu(new KickedMenu("You've been kicked from the lobby. :("));
				}
				return;
			}
			if (@string.Contains("NEW LOBBY"))
			{
				string[] array3 = @string.Split(new char[]
				{
					','
				});
				CSteamID newLobbyID = new CSteamID(ulong.Parse(array3[1]));
				this.JoinNewLobby(newLobbyID);
				return;
			}
			if (@string.Contains("CONT INDEX CHANGE"))
			{
				string[] array4 = @string.Split(new char[]
				{
					','
				});
				int num = int.Parse(array4[1]);
				int num2 = int.Parse(array4[2]);
				PlayerInfo player = PlayerManager.GetPlayer(num);
				if (player == null)
				{
					Terminal.WriteMessage("Error - no player found for index " + num, MessageType.ERROR);
					return;
				}
				Terminal.WriteMessage(string.Concat(new object[]
				{
					"Player index ",
					num,
					" changing controller index to: ",
					num2
				}), MessageType.IMPORTANTEVENT);
				player.ControllerIndex = num2;
			}
			if (!this.mIAmLobbyCreator)
			{
				Terminal.WriteMessage("     OnLobbyChatMsg - bailing (for now), not lobby owner.");
				Terminal.WriteMessage("     OnLobbyChatMsg - " + @string);
				return;
			}
			this.HandlePlayerSelection(id, @string);
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00061B94 File Offset: 0x0005FD94
		private void HandlePlayerSelection(CSteamID id, string selection)
		{
			Terminal.WriteMessage("Got player selection msg - " + selection);
			string[] array = selection.Split(new char[]
			{
				','
			});
			string text = array[0];
			string text2 = array[1];
			bool guest = bool.Parse(array[2]);
			int num = int.Parse(array[3]);
			CSteamID id2 = new CSteamID(ulong.Parse(array[4]));
			PlayerInfo playerInfo = PlayerManager.GetPlayer(id, num);
			if (playerInfo == null)
			{
				Terminal.WriteMessage("No player found for id: " + id);
				Terminal.WriteMessage("Adding player...");
				int firstAvailIndex = PlayerManager.GetFirstAvailIndex();
				if (firstAvailIndex == -1)
				{
					Terminal.WriteMessage("     OnLobbyChatMsg - Error: NO ROOM FOR PLAYER");
					this.KickPlayer(id2);
					return;
				}
				PlayerManager.AddPlayer(firstAvailIndex, false, id);
				PlayerInfo player = PlayerManager.GetPlayer(firstAvailIndex);
				player.Guest = guest;
				player.ControllerIndex = num;
				playerInfo = player;
			}
			Terminal.WriteMessage("     OnLobbyChatMsg Selection Request:");
			Terminal.WriteMessage("         Index: " + playerInfo.Index);
			Terminal.WriteMessage("         Desired Char: " + text);
			Terminal.WriteMessage("         Desired Hat: " + text2);
			Terminal.WriteMessage("         Guest: " + guest.ToString());
			Terminal.WriteMessage("         ControllerIndex: " + num);
			Terminal.WriteMessage("Accepting character selection: " + text + ", " + text2, MessageType.DEBUG);
			playerInfo.CharacterName = text;
			playerInfo.AccessoryName = text2;
			this.mMasterCharSelect.Stores[playerInfo.Index].FinalSettings = PlayerStatKeeper.GetSettings(text);
			this.mMasterCharSelect.Stores[playerInfo.Index].FinalHat = PCHatStore.GetHatByName(text2);
			this.mMasterCharSelect.Stores[playerInfo.Index].AssertSelections();
			this.SetLobbyData();
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00061D5E File Offset: 0x0005FF5E
		private void OnLobbyDataUpdate(LobbyDataUpdate_t data)
		{
			if (data.m_bSuccess == 1)
			{
				this.UpdateLobbyData();
				return;
			}
			Terminal.WriteMessage("Lobby data update call failed?", MessageType.ERROR);
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00061D7C File Offset: 0x0005FF7C
		private void UpdateCount()
		{
			int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(this.mLobbyID);
			this.mLobbyOccupantCount = numLobbyMembers;
			Terminal.WriteMessage("     Lobby Occupant Count: " + numLobbyMembers, MessageType.IMPORTANTEVENT);
			this.UpdateLobbyOccupants();
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00061DB8 File Offset: 0x0005FFB8
		private void UpdateLobbyOccupants()
		{
			Terminal.WriteMessage("Lobby Occupants:", MessageType.IMPORTANTEVENT);
			this.mPresentPlayers.Clear();
			for (int i = 0; i < this.mLobbyOccupantCount; i++)
			{
				CSteamID lobbyMemberByIndex = SteamMatchmaking.GetLobbyMemberByIndex(this.mLobbyID, i);
				this.mPresentPlayers.Add(lobbyMemberByIndex);
				Terminal.WriteMessage("     " + lobbyMemberByIndex.ToString(), MessageType.IMPORTANTEVENT);
			}
			for (int j = 0; j < 4; j++)
			{
				PlayerInfo player = PlayerManager.GetPlayer(j);
				if (player != null && !this.mPresentPlayers.Contains(player.SteamID))
				{
					PlayerManager.RemovePlayer(j);
					Terminal.WriteMessage("No one with SteamID: " + player.SteamID + " found. Removing player.");
				}
			}
			if (SteamHelper.GetLocalID() == SteamMatchmaking.GetLobbyOwner(this.mLobbyID))
			{
				this.mIAmLobbyCreator = true;
				Terminal.WriteMessage("I am lobby owner now.");
				PlayerInfo playerInfo = null;
				for (int k = 0; k < 4; k++)
				{
					playerInfo = PlayerManager.GetPlayer(k);
					if (playerInfo != null && !playerInfo.Guest && !(playerInfo.SteamID != SteamHelper.GetLocalID()))
					{
						break;
					}
				}
				if (playerInfo == null)
				{
					Terminal.WriteMessage("Error migrating host.", MessageType.ERROR);
					this.LeaveLobby();
					this.RemoveCallbacks();
					Global.GameManager.GotoMainMenu();
					return;
				}
				PlayerManager.MovePlayerIndex(playerInfo, 0);
				NetworkManager.SetIndex(0);
				this.mMasterCharSelect.RefreshAllStores();
			}
			if (this.mIAmLobbyCreator)
			{
				NetworkManager.SetHost(true);
				this.SetLobbyData();
				this.mRefreshSelectionsSendTime = LobbyManager.POLL_TIME;
			}
			PlayerManager.PrintPlayers();
			if (this.mPreviousLobbyPlayers != null)
			{
				this.CLEAR_STORES();
				for (int l = 0; l < 4; l++)
				{
					if (this.mPreviousLobbyPlayers[l] != null)
					{
						Terminal.WriteMessage("Updating lobby with data: " + this.mPreviousLobbyPlayers[l].GetData(), MessageType.IMPORTANTEVENT);
						PlayerInfo playerInfo2 = PlayerManager.GetPlayer(this.mPreviousLobbyPlayers[l].SteamID, this.mPreviousLobbyPlayers[l].ControllerIndex);
						if (playerInfo2 == null)
						{
							Terminal.WriteMessage("Couldn't find player from previous lobby. (This is expected)", MessageType.IMPORTANTEVENT);
							playerInfo2 = this.mPreviousLobbyPlayers[l];
							playerInfo2.Index = -1;
						}
						Terminal.WriteMessage("Setting local player selections back to previous lobby: ", MessageType.SAVELOAD);
						Terminal.WriteMessage("Name: " + this.mPreviousLobbyPlayers[l].PersonaName, MessageType.SAVELOAD);
						Terminal.WriteMessage("Char: " + this.mPreviousLobbyPlayers[l].CharacterName, MessageType.SAVELOAD);
						Terminal.WriteMessage("Hat: " + this.mPreviousLobbyPlayers[l].AccessoryName, MessageType.SAVELOAD);
						Terminal.WriteMessage("UsingController: " + this.mPreviousLobbyPlayers[l].UsingController.ToString(), MessageType.SAVELOAD);
						Terminal.WriteMessage("ControllerIndex: " + this.mPreviousLobbyPlayers[l].ControllerIndex, MessageType.SAVELOAD);
						Terminal.WriteMessage("Guest: " + this.mPreviousLobbyPlayers[l].Guest.ToString(), MessageType.SAVELOAD);
						playerInfo2.CharacterName = this.mPreviousLobbyPlayers[l].CharacterName;
						playerInfo2.AccessoryName = this.mPreviousLobbyPlayers[l].AccessoryName;
						playerInfo2.Guest = this.mPreviousLobbyPlayers[l].Guest;
						playerInfo2.ControllerIndex = this.mPreviousLobbyPlayers[l].ControllerIndex;
						playerInfo2.SteamID = this.mPreviousLobbyPlayers[l].SteamID;
						this.UpdateWhoIWantToBe(playerInfo2);
					}
					else
					{
						Terminal.WriteMessage("Info was null " + l, MessageType.ERROR);
					}
				}
				this.mPreviousLobbyPlayers = null;
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00062144 File Offset: 0x00060344
		private void CLEAR_STORES()
		{
			for (int i = 0; i < 4; i++)
			{
				this.mMasterCharSelect.Stores[i].FinalSettings.name = PlayerInfo.NO_SELECTION;
				this.mMasterCharSelect.Stores[i].FinalHat.Name = PlayerInfo.NO_SELECTION;
				this.mMasterCharSelect.Stores[i].ClearSelection();
				this.mMasterCharSelect.Stores[i].AssertSelections();
			}
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x000621CC File Offset: 0x000603CC
		private void SetInitialLobbyData()
		{
			this.SendLobbyData("map", GameManager.LevelName);
			this.SendLobbyData("version", Global.VersionNumber);
			this.SendLobbyData("difficulty", Global.DIFFICULTY_LEVEL.ToString());
			this.SendLobbyData("diff", Global.DifficultyModeMod.ToString());
			this.SendLobbyData("unlimited", Global.UnlimitedMode.ToString());
			this.SendLobbyData("zombieHealthMod", Global.ZombieHealthMod.ToString(CultureInfo.InvariantCulture));
			this.SetLobbyData();
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00062258 File Offset: 0x00060458
		public void SetLobbyData()
		{
			for (int i = 0; i < 4; i++)
			{
				this.BroadcastPlayerSelection(i);
			}
			int num = 4 - PlayerManager.PlayerCount;
			Terminal.WriteMessage("OPEN SLOTS: " + num);
			this.SendLobbyData("Private", this.mPrivateLobby.ToString());
			this.SendLobbyData("Random_Seed", Global.WAVE_GEN_SEED.ToString());
			this.SendLobbyData("openSlots", num.ToString());
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x000622D4 File Offset: 0x000604D4
		private void BroadcastPlayerSelection(int i)
		{
			PlayerInfo player = PlayerManager.GetPlayer(i);
			if (player == null)
			{
				this.SendLobbyData("Player_Selection" + i, PlayerInfo.NO_PLAYER);
				return;
			}
			this.SendLobbyData("Player_Selection" + i, player.GetData());
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00062323 File Offset: 0x00060523
		public void ClearPlayer(int i)
		{
			this.SendLobbyData("Player_Selection" + i, PlayerInfo.NO_PLAYER);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00062340 File Offset: 0x00060540
		public void LocalPlayerClickedBack()
		{
			this.mMasterCharSelect.mChat.Destroy();
			Terminal.WriteMessage("Local player wants to leave. Backing out.", MessageType.IMPORTANTEVENT);
			PlayerManager.ClearPlayers();
			this.LeaveLobby();
			this.RemoveCallbacks();
			Global.GameManager.GotoMainMenu();
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00062378 File Offset: 0x00060578
		public void LocalPlayerChangedContIndex(int pIndex, int cIndex)
		{
			string text = string.Format("CONT INDEX CHANGE,{0},{1}", pIndex, cIndex);
			Terminal.WriteMessage(text, MessageType.IMPORTANTEVENT);
			byte[] bytes = LobbyManager.GetBytes(text);
			SteamMatchmaking.SendLobbyChatMsg(this.mLobbyID, bytes, bytes.Length);
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x000623B8 File Offset: 0x000605B8
		private void UpdateLobbyData()
		{
			if (this.mIAmLobbyCreator)
			{
				return;
			}
			if (this.mLeavingLobby)
			{
				Terminal.WriteMessage("UpdateLobbyData - Ignoring, leaving lobby.");
				return;
			}
			this.UpdateCharacterSelectionsFromHost();
			Global.WAVE_GEN_SEED = int.Parse(this.GetLobbyData("Random_Seed"));
			Global.DifficultyModeMod = float.Parse(this.GetLobbyData("diff"), CultureInfo.InvariantCulture);
			Global.DIFFICULTY_LEVEL = int.Parse(this.GetLobbyData("difficulty"), CultureInfo.InvariantCulture);
			Global.UnlimitedMode = bool.Parse(this.GetLobbyData("unlimited"));
			Global.ZombieHealthMod = float.Parse(this.GetLobbyData("zombieHealthMod"), CultureInfo.InvariantCulture);
			if (!NetworkManager.AmIHost)
			{
				this.mPrivateLobby = bool.Parse(this.GetLobbyData("Private"));
			}
			if (Global.UnlimitedMode)
			{
				Global.WaveCount = 50;
			}
			else
			{
				Global.WaveCount = 15;
			}
			GameManager.LevelName = this.GetLobbyData("map");
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000624A4 File Offset: 0x000606A4
		private void UpdateCharacterSelectionsFromHost()
		{
			try
			{
				for (int i = 0; i < 4; i++)
				{
					PlayerInfo player = PlayerManager.GetPlayer(i);
					string[] array = this.GetLobbyData("Player_Selection" + i).Split(new char[]
					{
						','
					});
					string text = array[0];
					if (text == PlayerInfo.NO_PLAYER)
					{
						PlayerManager.RemovePlayer(i);
						this.mMasterCharSelect.Stores[i].FinalSettings.name = PlayerInfo.NO_SELECTION;
						this.mMasterCharSelect.Stores[i].FinalHat.Name = PlayerInfo.NO_SELECTION;
						this.mMasterCharSelect.Stores[i].AssertSelections();
					}
					else
					{
						string text2 = array[1];
						bool guest = bool.Parse(array[2]);
						int controllerIndex = int.Parse(array[3]);
						CSteamID csteamID = new CSteamID(ulong.Parse(array[4]));
						if (player == null)
						{
							bool local = csteamID == SteamHelper.GetLocalID();
							PlayerManager.AddPlayer(i, local, csteamID);
							player = PlayerManager.GetPlayer(i);
						}
						if (csteamID == SteamHelper.GetLocalID())
						{
							NetworkManager.SetIndex(i);
						}
						player.CharacterName = text;
						player.AccessoryName = text2;
						player.ControllerIndex = controllerIndex;
						player.Guest = guest;
						if (text == PlayerInfo.NO_SELECTION)
						{
							this.mMasterCharSelect.Stores[i].FinalSettings.name = PlayerInfo.NO_SELECTION;
							this.mMasterCharSelect.Stores[i].FinalHat.Name = PlayerInfo.NO_SELECTION;
							this.mMasterCharSelect.Stores[i].AssertSelections();
						}
						else if (text == PlayerInfo.NO_PLAYER)
						{
							this.mMasterCharSelect.Stores[i].FinalSettings.name = PlayerInfo.NO_SELECTION;
							this.mMasterCharSelect.Stores[i].FinalHat.Name = PlayerInfo.NO_SELECTION;
							this.mMasterCharSelect.Stores[i].AssertSelections();
						}
						else
						{
							this.mMasterCharSelect.Stores[i].FinalSettings = PlayerStatKeeper.GetSettings(text);
							this.mMasterCharSelect.Stores[i].FinalHat = PCHatStore.GetHatByName(text2);
							this.mMasterCharSelect.Stores[i].AssertSelections();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Terminal.WriteMessage("Exception occurred in UpdateCharacterSelectionsFromHost. " + ex.ToString(), MessageType.ERROR);
				Terminal.WriteMessage("Leaving lobby due to error.", MessageType.ERROR);
				PlayerManager.ClearPlayers();
				this.LeaveLobby();
				this.RemoveCallbacks();
				Global.GameManager.GotoMainMenu();
				MenuManager.PushMenu(new KickedMenu("An error occurred in the lobby!"));
			}
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0006276C File Offset: 0x0006096C
		private void SendLobbyData(string key, string value)
		{
			Terminal.WriteMessage("Setting lobby data - KEY: " + key + " | VALUE: " + value, MessageType.IMPORTANTEVENT);
			SteamMatchmaking.SetLobbyData(this.mLobbyID, key, value);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00062794 File Offset: 0x00060994
		private string GetLobbyData(string key)
		{
			string lobbyData = SteamMatchmaking.GetLobbyData(this.mLobbyID, key);
			Terminal.WriteMessage("Getting lobby data - KEY: " + key + " | VALUE: " + lobbyData);
			return lobbyData;
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x000627C8 File Offset: 0x000609C8
		public void UpdateWhoIWantToBe(int index)
		{
			PlayerInfo player = PlayerManager.GetPlayer(index);
			this.UpdateWhoIWantToBe(player);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x000627E4 File Offset: 0x000609E4
		public void UpdateWhoIWantToBe(PlayerInfo p)
		{
			if (this.mOfflineMode)
			{
				return;
			}
			if (p == null)
			{
				Terminal.WriteMessage("Error updating selection data - null player info", MessageType.ERROR);
				return;
			}
			if (this.mIAmLobbyCreator)
			{
				if (string.IsNullOrEmpty(p.CharacterName))
				{
					p.CharacterName = PlayerInfo.NO_SELECTION;
				}
				this.SendLobbyData("Player_Selection" + p.Index, p.GetData());
				return;
			}
			if (string.IsNullOrEmpty(p.CharacterName))
			{
				Terminal.WriteMessage("Char name was null/empty, reverting to NO_SELECTION for index " + p.Index, MessageType.DEBUG);
				p.CharacterName = PlayerInfo.NO_SELECTION;
			}
			Terminal.WriteMessage("Requesting to be: " + p.GetData(), MessageType.DEBUG);
			byte[] bytes = LobbyManager.GetBytes(p.GetData());
			SteamMatchmaking.SendLobbyChatMsg(this.mLobbyID, bytes, bytes.Length);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x000628B0 File Offset: 0x00060AB0
		public void RefreshLocalUnlocks()
		{
			this.mMasterCharSelect.RefreshLocalUnlocks();
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x000628C0 File Offset: 0x00060AC0
		public void STARTGAME()
		{
			if (this.mOfflineMode)
			{
				this.mLeavingLobby = true;
				Terminal.WriteMessage("Starting game in offline mode.");
				this.mMasterCharSelect.Launch();
			}
			if (!this.mIAmLobbyCreator)
			{
				Terminal.WriteMessage("Error, non lobby owner tried to start?", MessageType.ERROR);
				return;
			}
			byte[] bytes = LobbyManager.GetBytes("START GAME");
			SteamMatchmaking.SendLobbyChatMsg(this.mLobbyID, bytes, bytes.Length);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00062920 File Offset: 0x00060B20
		public void LeaveLobby()
		{
			if (this.mOfflineMode)
			{
				return;
			}
			this.mLeavingLobby = true;
			Terminal.WriteMessage("Leaving lobby: " + this.mLobbyID, MessageType.IMPORTANTEVENT);
			SteamMatchmaking.LeaveLobby(this.mLobbyID);
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x00062958 File Offset: 0x00060B58
		public void RemoveCallbacks()
		{
			Terminal.WriteMessage("Removing lobby callbacks.", MessageType.IMPORTANTEVENT);
			this.mOnLobbyChatMsg.Unregister();
			this.mOnLobbyMatchList.Cancel();
			this.mOnLobbyCreated.Cancel();
			this.mOnLobbyEntered.Cancel();
			this.mOnLobbyChatUpdate.Unregister();
			this.mOnLobbyChatMsg.Unregister();
			this.mOnLobbyDataUpdate.Unregister();
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x000629C0 File Offset: 0x00060BC0
		public void ToggleLobbyType()
		{
			this.mPrivateLobby = !this.mPrivateLobby;
			if (!this.mPrivateLobby)
			{
				Terminal.WriteMessage("Transitioning to public lobby.", MessageType.IMPORTANTEVENT);
				Terminal.WriteMessage("Searching for a lobby to merge with...", MessageType.IMPORTANTEVENT);
				this.BeginSearchForLobby();
				return;
			}
			SteamMatchmaking.SetLobbyType(this.mLobbyID, ELobbyType.k_ELobbyTypePrivate);
			this.SetLobbyData();
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00062A14 File Offset: 0x00060C14
		public void InviteAFriend()
		{
			SteamFriends.ActivateGameOverlayInviteDialog(this.mLobbyID);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00062A24 File Offset: 0x00060C24
		private void JoinNewLobby(CSteamID newLobbyID)
		{
			this.mMasterCharSelect.WaitingForLobbyUpdate.Restart();
			Terminal.WriteMessage("I was told to leave this lobby and join another: " + newLobbyID, MessageType.IMPORTANTEVENT);
			this.mPreviousLobbyPlayers = new PlayerInfo[4];
			for (int i = 0; i < 4; i++)
			{
				PlayerInfo player = PlayerManager.GetPlayer(i);
				if (player != null && player.Local)
				{
					this.mPreviousLobbyPlayers[i] = player.Clone();
					Terminal.WriteMessage("Copying player data: " + player.GetData(), MessageType.IMPORTANTEVENT);
				}
			}
			this.LeaveLobby();
			SteamAPICall_t hAPICall = SteamMatchmaking.JoinLobby(newLobbyID);
			this.mOnLobbyEntered.Set(hAPICall, null);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00062AC0 File Offset: 0x00060CC0
		public void KickPlayer(CSteamID id)
		{
			if (!this.mIAmLobbyCreator)
			{
				Terminal.WriteMessage("Tried to kick someone as non host?", MessageType.ERROR);
				return;
			}
			Terminal.WriteMessage("Kicking player: " + id.ToString(), MessageType.IMPORTANTEVENT);
			string text = "KICK PLAYER," + id.ToString();
			Terminal.WriteMessage(text, MessageType.IMPORTANTEVENT);
			byte[] bytes = LobbyManager.GetBytes(text);
			SteamMatchmaking.SendLobbyChatMsg(this.mLobbyID, bytes, bytes.Length);
			for (int i = 0; i < 4; i++)
			{
				PlayerInfo player = PlayerManager.GetPlayer(i);
				if (player != null && player.SteamID == id)
				{
					PlayerManager.RemovePlayer(i);
				}
			}
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00062B5C File Offset: 0x00060D5C
		private static byte[] GetBytes(string str)
		{
			byte[] array = new byte[str.Length * 2];
			Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00062B8C File Offset: 0x00060D8C
		private static string GetString(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return "ZeroLength";
			}
			char[] array = new char[bytes.Length / 2];
			Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
			return new string(array);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00062BC4 File Offset: 0x00060DC4
		private static string GetString(byte[] bytes, int actualLength)
		{
			if (bytes == null || actualLength == 0)
			{
				return "ZeroLength";
			}
			char[] array = new char[actualLength / 2];
			Buffer.BlockCopy(bytes, 0, array, 0, actualLength);
			return new string(array);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00062BF8 File Offset: 0x00060DF8
		private void ConfirmVersions()
		{
			Terminal.WriteMessage("Confirming matching versions.");
			string lobbyData = SteamMatchmaking.GetLobbyData(this.mLobbyID, "version");
			Terminal.WriteMessage("My version: " + Global.VersionNumber + " | Their version: " + lobbyData);
			if (string.IsNullOrEmpty(lobbyData) || string.IsNullOrWhiteSpace(lobbyData))
			{
				Terminal.WriteMessage("Lobby doesn't exist!", MessageType.ERROR);
				this.LeaveLobby();
				Global.GameManager.GotoMainMenu();
				MenuManager.PushMenu(new KickedMenu("The lobby no longer exists!"));
				return;
			}
			if (lobbyData != Global.VersionNumber)
			{
				Terminal.WriteMessage("Version mismatch!", MessageType.ERROR);
				this.LeaveLobby();
				Global.GameManager.GotoMainMenu();
				SteamApps.MarkContentCorrupt(false);
				MenuManager.PushMenu(new KickedMenu("Game version mismatch! Make sure both players have updated!"));
				return;
			}
		}

		// Token: 0x04000C3A RID: 3130
		private CSteamID mLobbyID = CSteamID.Nil;

		// Token: 0x04000C3B RID: 3131
		private static float POLL_TIME = 1.25f;

		// Token: 0x04000C3C RID: 3132
		private CallResult<LobbyMatchList_t> mOnLobbyMatchList;

		// Token: 0x04000C3D RID: 3133
		private CallResult<LobbyCreated_t> mOnLobbyCreated;

		// Token: 0x04000C3E RID: 3134
		private CallResult<LobbyEnter_t> mOnLobbyEntered;

		// Token: 0x04000C3F RID: 3135
		private Callback<LobbyChatUpdate_t> mOnLobbyChatUpdate;

		// Token: 0x04000C40 RID: 3136
		private Callback<LobbyChatMsg_t> mOnLobbyChatMsg;

		// Token: 0x04000C41 RID: 3137
		private Callback<LobbyDataUpdate_t> mOnLobbyDataUpdate;

		// Token: 0x04000C42 RID: 3138
		private List<CSteamID> mPresentPlayers = new List<CSteamID>();

		// Token: 0x04000C43 RID: 3139
		private int mLobbyOccupantCount;

		// Token: 0x04000C44 RID: 3140
		private bool mIAmLobbyCreator;

		// Token: 0x04000C45 RID: 3141
		private XboxMasterCharacterSelect mMasterCharSelect;

		// Token: 0x04000C46 RID: 3142
		private bool mLeavingLobby;

		// Token: 0x04000C47 RID: 3143
		private bool mNoMapPreference;

		// Token: 0x04000C48 RID: 3144
		private PlayerInfo[] mPreviousLobbyPlayers;

		// Token: 0x04000C49 RID: 3145
		private bool mPrivateLobby;

		// Token: 0x04000C4A RID: 3146
		private bool mOfflineMode;

		// Token: 0x04000C4B RID: 3147
		private float mRefreshSelectionsSendTime = LobbyManager.POLL_TIME;
	}
}
