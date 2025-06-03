using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using ZombieEstate2.Networking;
using ZombieEstate2.UI.Chat;

namespace ZombieEstate2.UI.Xbox
{
	// Token: 0x02000160 RID: 352
	public class XboxMasterCharacterSelect
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x000590A3 File Offset: 0x000572A3
		public List<XboxCharacterSelect> Stores
		{
			get
			{
				return this.mStores;
			}
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x000590AC File Offset: 0x000572AC
		public XboxMasterCharacterSelect()
		{
			this.mLobbyManager = new LobbyManager(MainMenu.HOSTING_GAME, this, MainMenu.HOSTING_GAME, MainMenu.LOBBY_TO_JOIN);
			this.WaitingForLobbyUpdate.IndependentOfTime = true;
			MainMenu.LOBBY_TO_JOIN = default(CSteamID);
			this.mStores = new List<XboxCharacterSelect>();
			this.mStores.Add(new XboxCharacterSelect(0, this.mLobbyManager));
			this.mStores.Add(new XboxCharacterSelect(1, this.mLobbyManager));
			this.mStores.Add(new XboxCharacterSelect(2, this.mLobbyManager));
			this.mStores.Add(new XboxCharacterSelect(3, this.mLobbyManager));
			this.mChat = new ChatUI(new Rectangle((int)this.mStores[1].mTopLeft.X + 500 + 10, (int)this.mStores[1].mTopLeft.Y, 200, 590), true, Color.White, 120);
			int num = (int)Global.GetScreenCenter().X;
			int num2 = 100;
			this.mLobbyType = new ZEButton(new Rectangle(num - 160, Global.ScreenRect.Height - num2, 240, 44), "Public Lobby", ButtonPress.HealthPack, -1);
			this.mHardBackButton = new Rectangle(80, Global.ScreenRect.Height - num2, 80, 44);
			this.mInviteButton = new ZEButton(new Rectangle(180, Global.ScreenRect.Height - num2, 240, 44), "Invite a Friend", ButtonPress.Reload, -1);
			this.mStartGame = new ZEButton(new Rectangle(num + 160, Global.ScreenRect.Height - num2, 200, 44), "Start Game!", ButtonPress.Ready, -1, ZEButton.POSITIVE_COLOR);
			this.mStartGame.PULSE = true;
			this.mCancelSearch = new ZEButton(new Rectangle(num - 160, Global.ScreenRect.Height - num2, 240, 44), "Cancel...", ButtonPress.Negative, -1, ZEButton.NEGATIVE_COLOR);
			if (this.mLobbyManager.PrivateLobby)
			{
				this.mLobbyType.SetText("Start Search...");
			}
			else
			{
				this.mLobbyType.SetText("Stop Searching");
			}
			this.mStartGame.OnPressed += delegate(object s, EventArgs e)
			{
				if (this.READY_TO_START)
				{
					SoundEngine.PlaySound("ze2_navup", 0.3f);
					this.mLobbyManager.STARTGAME();
				}
			};
			this.mLobbyType.OnPressed += delegate(object s, EventArgs e)
			{
				this.mLobbyManager.ToggleLobbyType();
				if (this.mLobbyManager.PrivateLobby)
				{
					this.mLobbyType.SetText("Start Search...");
					return;
				}
				this.mLobbyType.SetText("Stop Searching");
			};
			this.mInviteButton.OnPressed += delegate(object s, EventArgs e)
			{
				this.mLobbyManager.InviteAFriend();
			};
			this.mCancelSearch.OnPressed += delegate(object s, EventArgs e)
			{
				Terminal.WriteMessage("Cancelling lobby search!", MessageType.IMPORTANTEVENT);
				Global.GameManager.GotoMainMenu();
				PlayerManager.ClearPlayers();
				this.mChat.Destroy();
			};
			for (int i = 0; i < 4; i++)
			{
				this.mStores[i].Activate();
			}
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00059384 File Offset: 0x00057584
		public void Update(float elapsed)
		{
			this.HandleRemoves();
			this.mLobbyManager.Update();
			this.mChat.Update(0.016666668f);
			if (!NetworkManager.AmIHost)
			{
				this.mStartGame.ENABLED = false;
			}
			if (this.mLobbyManager.HaveIFoundALobby)
			{
				this.mAllDone = true;
				this.mNoneActive = true;
				this.READY_TO_START = false;
				foreach (XboxCharacterSelect xboxCharacterSelect in this.mStores)
				{
					xboxCharacterSelect.Update(elapsed);
					if (xboxCharacterSelect.mState != XboxCharacterSelect.CharState.WaitingToJoin)
					{
						this.mNoneActive = false;
						if (xboxCharacterSelect.mState != XboxCharacterSelect.CharState.AllDone && xboxCharacterSelect.Active && !xboxCharacterSelect.AllDoneAndWaiting)
						{
							this.mAllDone = false;
						}
					}
				}
				if (this.mAllDone && !this.mNoneActive)
				{
					this.READY_TO_START = true;
					this.mStartGame.Update();
				}
				this.UpdateLobbyButton();
				this.mInviteButton.ENABLED = (!PlayerManager.GameIsFull && !this.mLobbyManager.OfflineMode);
				this.mInviteButton.Update();
				this.UpdateLeaving();
			}
			else
			{
				this.mCancelSearch.Update();
			}
			for (int i = 0; i < 4; i++)
			{
				PlayerInfo player = PlayerManager.GetPlayer(i);
				if (player != null && player.Local && player.UsingController && !player.Guest && InputManager.ButtonPressed(ButtonPress.Pause, i, false) && this.READY_TO_START)
				{
					SoundEngine.PlaySound("ze2_navup", 0.3f);
					this.mLobbyManager.STARTGAME();
				}
			}
			this.HandleAdds();
			if (InputManager.LeftMouseClicked() && this.mHardBackButton.Contains(InputManager.GetMousePosition()))
			{
				this.mLobbyManager.LocalPlayerClickedBack();
				SoundEngine.PlaySound("ze2_navdown", 0.3f);
				return;
			}
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00059560 File Offset: 0x00057760
		private void UpdateLobbyButton()
		{
			if (NetworkManager.AmIHost && !this.mLobbyManager.OfflineMode)
			{
				this.mLobbyType.ENABLED = true;
				this.mLobbyType.Update();
				return;
			}
			if (this.mLobbyManager.OfflineMode)
			{
				this.mLobbyType.ENABLED = false;
				this.mLobbyType.SetText("OFFLINE MODE");
				return;
			}
			this.mLobbyType.ENABLED = false;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void HandleAdds()
		{
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void HandleRemoves()
		{
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void UpdateLeaving()
		{
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x000595D0 File Offset: 0x000577D0
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null);
			spriteBatch.Draw(Global.MenuBG, Global.ScreenRect, Color.White);
			if (this.mLobbyManager.HaveIFoundALobby)
			{
				foreach (XboxCharacterSelect xboxCharacterSelect in this.mStores)
				{
					xboxCharacterSelect.Draw(spriteBatch);
				}
				if (this.mAllDone && !this.mNoneActive && NetworkManager.AmIHost)
				{
					this.mStartGame.ENABLED = true;
				}
				else
				{
					this.mStartGame.ENABLED = false;
				}
				this.mStartGame.Draw(spriteBatch);
				this.mLobbyType.Draw(spriteBatch);
				this.mInviteButton.Draw(spriteBatch);
			}
			else
			{
				Vector2 pos = VerchickMath.CenterText(Global.StoreFontBig, Global.GetScreenCenter(), "Searching for lobby...");
				Shadow.DrawString("Searching for lobby...", Global.StoreFontBig, pos, 1, Color.White, spriteBatch);
				this.mCancelSearch.Draw(spriteBatch);
			}
			this.mChat.Draw(spriteBatch);
			this.DrawBackButton(spriteBatch);
			spriteBatch.End();
			spriteBatch.Begin();
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0005970C File Offset: 0x0005790C
		public void Launch()
		{
			PingManager.DISABLE_PINGS = false;
			Camera.RESET_ANIM();
			Global.PlayerList = new List<Player>();
			Terminal.WriteMessage("Creating players...");
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				XboxCharacterSelect xboxCharacterSelect = this.mStores[i];
				if (xboxCharacterSelect.Active)
				{
					num++;
					CharacterSettings finalSettings = xboxCharacterSelect.FinalSettings;
					Hat finalHat = xboxCharacterSelect.FinalHat;
					if (finalSettings.name == PlayerInfo.NO_PLAYER || finalSettings.name == PlayerInfo.NO_SELECTION || string.IsNullOrEmpty(finalSettings.name))
					{
						Terminal.WriteMessage("     Index " + i + " is empty.");
					}
					else if (PlayerManager.GetPlayer(i) == null)
					{
						Terminal.WriteMessage("     Player Manager Index " + i + " is empty.");
					}
					else
					{
						Player player = new Player();
						player.TextureCoord = finalSettings.texCoord;
						player.StartTextureCoord = finalSettings.texCoord;
						player.Stats = new PlayerStats(PCCharacterSelect.Stats[finalSettings], finalSettings, player);
						player.Stats.CharSettings = finalSettings;
						player.Index = xboxCharacterSelect.mIndex;
						player.InitPlayer(PCCharacterSelect.Stats[finalSettings], finalSettings, xboxCharacterSelect.mIndex);
						if (finalHat.Name != "" && finalHat.Name != null && finalHat.Name != "None" && finalHat.Name != PlayerInfo.NO_SELECTION)
						{
							player.AccessoryName = finalHat.Name;
							player.AccessoryTexCoord = finalHat.Tex;
							player.HatSpecProps = finalHat.Properties;
						}
						Terminal.WriteMessage(string.Format("   Index: {0} - Selected Char {1} with Hat {2}", i, finalSettings.name, player.AccessoryName));
						player.SomethingChanged = true;
						player.ReInitHealth();
						player.Gamer = xboxCharacterSelect.mGamer;
						Global.PlayerList.Add(player);
					}
				}
			}
			this.mChat.Destroy();
			if (!this.mLobbyManager.OfflineMode)
			{
				this.mLobbyManager.LeaveLobby();
				this.mLobbyManager.RemoveCallbacks();
			}
			this.StartGame();
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00059946 File Offset: 0x00057B46
		public void StartGame()
		{
			ScreenFader.Fade(delegate()
			{
				foreach (XboxCharacterSelect xboxCharacterSelect in this.mStores)
				{
					xboxCharacterSelect.Close();
				}
				Global.Paused = false;
				Global.GameManager.StartGame(GameManager.LevelName);
			});
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0005995C File Offset: 0x00057B5C
		public void RefreshAllStores()
		{
			for (int i = 0; i < 4; i++)
			{
				XboxCharacterSelect xboxCharacterSelect = this.mStores[i];
				PlayerInfo player = PlayerManager.GetPlayer(xboxCharacterSelect.mIndex);
				if (player == null)
				{
					xboxCharacterSelect.mState = XboxCharacterSelect.CharState.WaitingToJoin;
				}
				else
				{
					if (player.CharacterName != PlayerInfo.NO_SELECTION)
					{
						xboxCharacterSelect.FinalSettings = PlayerStatKeeper.GetSettings(player.CharacterName);
						if (player.Local)
						{
							xboxCharacterSelect.mState = XboxCharacterSelect.CharState.Hats;
						}
					}
					if (player.AccessoryName != PlayerInfo.NO_SELECTION)
					{
						xboxCharacterSelect.FinalHat = PCHatStore.GetHatByName(player.AccessoryName);
						if (player.Local)
						{
							xboxCharacterSelect.mState = XboxCharacterSelect.CharState.AllDone;
						}
					}
					if (player.AccessoryName == PlayerInfo.NO_SELECTION && player.CharacterName == PlayerInfo.NO_SELECTION)
					{
						xboxCharacterSelect.mState = XboxCharacterSelect.CharState.Open;
					}
					if (!player.Local)
					{
						xboxCharacterSelect.mState = XboxCharacterSelect.CharState.NetPicking;
					}
					xboxCharacterSelect.AssertSelections();
				}
			}
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00059A48 File Offset: 0x00057C48
		public void RefreshLocalUnlocks()
		{
			for (int i = 0; i < 4; i++)
			{
				XboxCharacterSelect xboxCharacterSelect = this.mStores[i];
				PlayerInfo player = PlayerManager.GetPlayer(xboxCharacterSelect.mIndex);
				if (player != null && player.Local)
				{
					xboxCharacterSelect.AssertSelections();
				}
			}
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00059A8B File Offset: 0x00057C8B
		public void Destroy()
		{
			if (this.mLobbyManager != null)
			{
				this.mLobbyManager.RemoveCallbacks();
			}
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00059AA0 File Offset: 0x00057CA0
		private void DrawBackButton(SpriteBatch spritebatch)
		{
			Rectangle destinationRectangle = new Rectangle(this.mHardBackButton.X, this.mHardBackButton.Y, this.mHardBackButton.Width, this.mHardBackButton.Height);
			destinationRectangle.Inflate(2, 2);
			Color red = new Color(0.3f, 0f, 0f);
			if (this.mHardBackButton.Contains(InputManager.GetMousePosition()))
			{
				red = Color.Red;
			}
			spritebatch.Draw(Global.Pixel, destinationRectangle, Color.Black);
			spritebatch.Draw(Global.Pixel, this.mHardBackButton, red);
			Vector2 vector = new Vector2((float)(this.mHardBackButton.X + this.mHardBackButton.Width / 2), (float)(this.mHardBackButton.Y + this.mHardBackButton.Height / 2));
			vector = VerchickMath.CenterText(Global.StoreFontBig, vector, "Quit");
			Shadow.DrawString("Quit", Global.StoreFontBig, vector, 1, Color.White, spritebatch);
		}

		// Token: 0x04000B91 RID: 2961
		private List<XboxCharacterSelect> mStores;

		// Token: 0x04000B92 RID: 2962
		private bool mONEPLAYER = true;

		// Token: 0x04000B93 RID: 2963
		private ZEButton mStartGame;

		// Token: 0x04000B94 RID: 2964
		private ZEButton mLobbyType;

		// Token: 0x04000B95 RID: 2965
		private ZEButton mInviteButton;

		// Token: 0x04000B96 RID: 2966
		private ZEButton mCancelSearch;

		// Token: 0x04000B97 RID: 2967
		private LobbyManager mLobbyManager;

		// Token: 0x04000B98 RID: 2968
		private Rectangle mHardBackButton;

		// Token: 0x04000B99 RID: 2969
		private bool mAllDone = true;

		// Token: 0x04000B9A RID: 2970
		private bool mNoneActive = true;

		// Token: 0x04000B9B RID: 2971
		private bool READY_TO_START;

		// Token: 0x04000B9C RID: 2972
		public Timer WaitingForLobbyUpdate = new Timer(10f);

		// Token: 0x04000B9D RID: 2973
		public ChatUI mChat;
	}
}
