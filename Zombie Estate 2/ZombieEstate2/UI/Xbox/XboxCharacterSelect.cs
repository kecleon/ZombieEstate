using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using StringParseLibrary;
using ZombieEstate2.HUD.XboxHUD;
using ZombieEstate2.Networking;
using ZombieEstate2.StoreScreen.XboxStore;
using ZombieEstate2.XboxSaving;

namespace ZombieEstate2.UI.Xbox
{
	// Token: 0x0200015F RID: 351
	public class XboxCharacterSelect
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0005633E File Offset: 0x0005453E
		public bool DialogActive
		{
			get
			{
				return this.mUnlockCharacter != null || this.mNotEnoughPoints != null || this.mUnlockHat != null;
			}
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0005635C File Offset: 0x0005455C
		public XboxCharacterSelect(int index, LobbyManager lobby)
		{
			this.mLobbyManager = lobby;
			this.mGamerStats = Global.GamerStats;
			this.mIndex = index;
			this.Setup();
			this.SetupButtons();
			this.mScreen = new XboxItemScreen(5, 7, 5, 6, Vector2.Add(this.mTopLeft, new Vector2(11f, 35f)), this.mIndex);
			this.mScreen.ItemHighlighted += this.mScreen_ItemHighlighted;
			this.mHatScreen = new XboxItemScreen(5, 7, 5, 6, Vector2.Add(this.mTopLeft, new Vector2(11f, 35f)), this.mIndex);
			this.mHatScreen.ItemHighlighted += this.mHatScreen_ItemHighlighted;
			this.mScreen.PopulateCharacters(PlayerStatKeeper.CharacterSettings, this.mGamerStats);
			this.mHatScreen.PopulateHats(PCHatStore.GetHatList(), this.mGamerStats);
			this.mGunNamePos = new Vector2(this.mTopLeft.X + 304f, this.mTopLeft.Y + 43f);
			this.mLocBigTex = new Rectangle((int)this.mTopLeft.X + 195, (int)this.mTopLeft.Y + 7, 64, 64);
			this.mLocNameCenter = new Vector2(this.mTopLeft.X + 379f, this.mTopLeft.Y + 23f);
			this.mLocStartingGun = new Rectangle((int)this.mTopLeft.X + 264, (int)this.mTopLeft.Y + 38, 32, 32);
			this.mLocFinalSelectedHat = new Rectangle((int)this.mTopLeft.X + 264, (int)this.mTopLeft.Y + 2, 32, 32);
			this.mZombiePointsPos = new Vector2((float)((int)this.mTopLeft.X + 130), (float)((int)this.mTopLeft.Y + 248));
			this.mPointsToUnlockLoc = new Vector2(this.mTopLeft.X + 302f, this.mTopLeft.Y + 42f);
			this.mGamerLoc = new Vector2(this.mTopLeft.X + 90f, this.mTopLeft.Y + 16f);
			this.mWaitingIcon = new LoadIcon(new Rectangle((int)this.mTopLeft.X + 250 - 32, (int)this.mTopLeft.Y + 140 - 32, 64, 64));
			this.mGamer = Gamer.SignedInGamers[(PlayerIndex)index];
			SignedInGamer.SignedIn += this.SignedInGamer_SignedIn;
			SignedInGamer.SignedOut += this.SignedInGamer_SignedOut;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00056654 File Offset: 0x00054854
		public void SetupButtons()
		{
			this.mJoinBackButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 250 - 60, (int)this.mTopLeft.Y + 164, 120, 32), "Back", ButtonPress.Negative, this.mIndex);
			this.mJoinBackButton.OnPressed += delegate(object e, EventArgs s)
			{
				if (this.mState == XboxCharacterSelect.CharState.WaitingToJoin)
				{
					this.Close();
					SoundEngine.PlaySound("ze2_navdown", 0.3f);
					Global.GameManager.GotoMainMenu();
				}
			};
			this.mJoinButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 250 - 60, (int)this.mTopLeft.Y + 114, 120, 32), "Join!", ButtonPress.Affirmative, this.mIndex, ZEButton.POSITIVE_COLOR);
			this.mJoinButton.OnPressed += delegate(object e, EventArgs s)
			{
				if (this.mState == XboxCharacterSelect.CharState.WaitingToJoin)
				{
					this.JoinLocal();
					SoundEngine.PlaySound("ze2_navup", 0.3f);
				}
			};
			this.mBackButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 3, (int)this.mTopLeft.Y + 250, 100, 28), "Back", ButtonPress.Negative, this.mIndex);
			this.mBackButton.OnPressed += delegate(object e, EventArgs s)
			{
				if (this.mState == XboxCharacterSelect.CharState.Open)
				{
					PlayerManager.RemovePlayer(this.mIndex);
					this.mState = XboxCharacterSelect.CharState.WaitingToJoin;
					SoundEngine.PlaySound("ze2_navdown", 0.3f);
					this.mLobbyManager.ClearPlayer(this.mIndex);
					this.mLobbyManager.LocalPlayerClickedBack();
				}
			};
			this.mSelectButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 378, (int)this.mTopLeft.Y + 250, 120, 28), "Select", ButtonPress.Affirmative, this.mIndex);
			this.mStatsButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 191, (int)this.mTopLeft.Y + 250, 120, 28), "Stats", ButtonPress.ViewStats, this.mIndex);
			this.mStatsButton.OnPressed += delegate(object s, EventArgs e)
			{
				if (this.mState == XboxCharacterSelect.CharState.Open && this.mSelected.name != null && this.mSelected.name != "")
				{
					Player player = new Player();
					player.CHARSELECT = true;
					player.InitPlayer(PCCharacterSelect.Stats[this.mSelected], this.mSelected, -1);
					player.Index = this.mIndex;
					this.mStatsHUD = new XboxStatsHUD(player, this.mTopLeft, true);
					this.mStatsHUD.Update();
					this.mState = XboxCharacterSelect.CharState.Stats;
					SoundEngine.PlaySound("ze2_menunav", 0.45f);
				}
			};
			this.mSelectButton.OnPressed += this.mScreen_ItemSelected;
			this.mHatSelectButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 378, (int)this.mTopLeft.Y + 250, 120, 28), "Select", ButtonPress.Affirmative, this.mIndex);
			this.mHatBackButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 3, (int)this.mTopLeft.Y + 250, 100, 28), "Back", ButtonPress.Negative, this.mIndex);
			this.mHatSelectButton.OnPressed += this.mHatScreen_ItemSelected;
			this.mHatBackButton.OnPressed += delegate(object e, EventArgs s)
			{
				if (this.mState == XboxCharacterSelect.CharState.Hats)
				{
					PlayerManager.GetPlayer(this.mIndex).CharacterName = PlayerInfo.NO_SELECTION;
					this.mState = XboxCharacterSelect.CharState.Open;
					this.mScreen.PopulateCharacters(PlayerStatKeeper.CharacterSettings, this.mGamerStats);
					this.mScreen.ReselectItem(PlayerManager.GetPlayer(this.mIndex).CharacterName);
					this.mScreen.RefireEvent(false);
					SoundEngine.PlaySound("ze2_navdown", 0.3f);
					this.mLobbyManager.UpdateWhoIWantToBe(this.mIndex);
				}
			};
			this.mAllDoneBackButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 3, (int)this.mTopLeft.Y + 250, 100, 28), "Back", ButtonPress.Negative, this.mIndex);
			this.mAllDoneBackButton.OnPressed += delegate(object e, EventArgs s)
			{
				if (this.mState == XboxCharacterSelect.CharState.AllDone)
				{
					this.mState = XboxCharacterSelect.CharState.Hats;
					this.mHatScreen.PopulateHats(PCHatStore.GetHatList(), this.mGamerStats);
					this.mHatScreen.ReselectItem(PlayerManager.GetPlayer(this.mIndex).AccessoryName);
					PlayerManager.GetPlayer(this.mIndex).AccessoryName = PlayerInfo.NO_SELECTION;
					this.mHatScreen.RefireEvent(false);
					SoundEngine.PlaySound("ze2_navdown", 0.3f);
					this.mLobbyManager.UpdateWhoIWantToBe(this.mIndex);
				}
			};
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00056938 File Offset: 0x00054B38
		public static void LoadGfx()
		{
			if (XboxCharacterSelect.mBG == null)
			{
				XboxCharacterSelect.mBG = Global.Content.Load<Texture2D>("XboxStore\\CharBG");
				XboxCharacterSelect.mBG_Picking = Global.Content.Load<Texture2D>("XboxStore\\CharBG_Picking");
				XboxCharacterSelect.mBGJoin = Global.Content.Load<Texture2D>("XboxStore\\PressAJoin");
				XboxCharacterSelect.mBG_Inactive = Global.Content.Load<Texture2D>("XboxStore\\CharBG_Inactive");
				XboxCharacterSelect.mIcons = Global.Content.Load<Texture2D>("XboxStore\\CharIcons");
				XboxCharacterSelect.mIcons_PC = Global.Content.Load<Texture2D>("XboxStore\\CharIcons_PC");
				XboxCharacterSelect.mReady = Global.Content.Load<Texture2D>("XboxStore\\PlayerReady");
				XboxCharacterSelect.mLogin = Global.Content.Load<Texture2D>("XboxStore\\LoginFirst");
				XboxCharacterSelect.mHatIcons = Global.Content.Load<Texture2D>("XboxStore\\CharIcons_PC");
				XboxCharacterSelect.mPressStart = Global.Content.Load<Texture2D>("XboxStore\\AllReady");
			}
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00056A17 File Offset: 0x00054C17
		private void SignedInGamer_SignedOut(object sender, SignedOutEventArgs e)
		{
			if (this.mIndex == (int)e.Gamer.PlayerIndex)
			{
				this.mGamer = null;
				this.Active = false;
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00056A3C File Offset: 0x00054C3C
		private void SignedInGamer_SignedIn(object sender, SignedInEventArgs e)
		{
			if (this.mIndex == (int)e.Gamer.PlayerIndex)
			{
				this.mGamer = e.Gamer;
				this.mGamerStats = Global.GamerStats;
				if (this.mGamerStats == null)
				{
					this.mGamerStats = new XboxGamerStats();
					this.mGamerStats.GamerName = this.mGamer.Gamertag;
					XboxSaverLoader.AddStats(this.mGamerStats);
				}
				this.mScreen.PopulateCharacters(PlayerStatKeeper.CharacterSettings, this.mGamerStats);
				this.mHatScreen.PopulateHats(PCHatStore.GetHatList(), this.mGamerStats);
				this.mScreen.RefireEvent(true);
			}
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00056AE4 File Offset: 0x00054CE4
		public void Update(float elapsed)
		{
			if (this.mInflateTime > 0f)
			{
				this.mInflateTime -= Global.REAL_GAME_TIME;
			}
			if (this.mInflateTime < 0f)
			{
				this.mInflateTime = 0f;
			}
			this.UpdateButtonStates();
			if (this.mState != XboxCharacterSelect.CharState.WaitingToJoin && PlayerManager.GetPlayer(this.mIndex) == null)
			{
				this.mState = XboxCharacterSelect.CharState.WaitingToJoin;
			}
			if (this.mGamerStats != null && this.prevPoints != this.mGamerStats.Points)
			{
				this.mZombiePoints = this.mGamerStats.Points.ToString();
				this.prevPoints = this.mGamerStats.Points;
			}
			if (this.mUnlockCharacter != null)
			{
				this.mUnlockCharacter.Update();
				if (!this.mUnlockCharacter.Active)
				{
					if (this.mUnlockCharacter.YesPressed)
					{
						this.mGamerStats.UnlockedCharacters.Add(this.mSelected.name);
						this.mGamerStats.Points -= this.mSelected.PointsToUnlock;
						this.mScreen.PopulateCharacters(PlayerStatKeeper.CharacterSettings, this.mGamerStats);
						this.mScreen.RefireEvent(false);
						XboxSaverLoader.SaveData();
						SoundEngine.PlaySound("ze2_money", 0.7f);
						this.mLobbyManager.RefreshLocalUnlocks();
					}
					this.mUnlockCharacter = null;
				}
				return;
			}
			if (this.mUnlockHat != null)
			{
				this.mUnlockHat.Update();
				if (!this.mUnlockHat.Active)
				{
					if (this.mUnlockHat.YesPressed)
					{
						this.mGamerStats.UnlockedHats.Add(this.mSelectedHat.Name);
						this.mGamerStats.Points -= this.mSelectedHat.ZHCost;
						this.mHatScreen.PopulateHats(PCHatStore.GetHatList(), this.mGamerStats);
						this.mHatScreen.RefireEvent(false);
						XboxSaverLoader.SaveData();
						SoundEngine.PlaySound("ze2_money", 0.6f);
						this.mLobbyManager.RefreshLocalUnlocks();
					}
					this.mUnlockHat = null;
				}
				return;
			}
			if (this.mNotEnoughPoints != null)
			{
				this.mNotEnoughPoints.Update();
				if (!this.mNotEnoughPoints.Active)
				{
					this.mNotEnoughPoints = null;
				}
				return;
			}
			if (this.mState == XboxCharacterSelect.CharState.Open)
			{
				this.mScreen.Update();
				this.mBackButton.Update();
				this.mStatsButton.Update();
				this.mSelectButton.Update();
				this.UpdateKickButton();
				this.UpdateControllerButton();
				return;
			}
			if (this.mState == XboxCharacterSelect.CharState.NetDone)
			{
				this.UpdateKickButton();
			}
			if (this.mState == XboxCharacterSelect.CharState.LoginFirst)
			{
				this.LoginTime += 0.016666668f;
				if (this.LoginTime >= 3f)
				{
					this.mState = XboxCharacterSelect.CharState.Open;
					this.Active = false;
				}
				return;
			}
			if (this.mState == XboxCharacterSelect.CharState.Stats)
			{
				this.mStatsHUD.Update();
				if (this.mStatsHUD.CLOSED)
				{
					this.mStatsHUD = null;
					this.mState = XboxCharacterSelect.CharState.Open;
					this.mScreen.RefireEvent(false);
					SoundEngine.PlaySound("ze2_navdown", 0.3f);
				}
				return;
			}
			if (this.mState == XboxCharacterSelect.CharState.Hats)
			{
				this.mHatScreen.Update();
				this.mHatSelectButton.Update();
				this.mHatBackButton.Update();
				this.UpdateKickButton();
				this.UpdateControllerButton();
				return;
			}
			if (this.mState == XboxCharacterSelect.CharState.AllDone)
			{
				this.mAllDoneBackButton.Update();
				this.UpdateKickButton();
				this.UpdateControllerButton();
				return;
			}
			if (this.mState == XboxCharacterSelect.CharState.WaitingToJoin)
			{
				this.mWaitingIcon.Update(elapsed);
				if (PlayerManager.GetPlayer(this.mIndex) == null)
				{
					for (int i = 0; i < 4; i++)
					{
						if (!PlayerManager.ControllerAlreadyInUse(i) && InputManager.ButtonPressed(ButtonPress.Affirmative, i, true))
						{
							PlayerManager.AddPlayer(this.mIndex, true, SteamHelper.GetLocalID());
							PlayerManager.GetPlayer(this.mIndex).Guest = true;
							PlayerManager.GetPlayer(this.mIndex).ControllerIndex = i;
							this.mLobbyManager.UpdateWhoIWantToBe(this.mIndex);
							Terminal.WriteMessage("Local player joined in index: " + this.mIndex);
							this.mLobbyManager.SetLobbyData();
							break;
						}
					}
				}
				if (PlayerManager.GetPlayer(this.mIndex) != null)
				{
					PlayerInfo player = PlayerManager.GetPlayer(this.mIndex);
					if (player.Local)
					{
						if (player.AccessoryName == PlayerInfo.NO_SELECTION && player.CharacterName != PlayerInfo.NO_SELECTION)
						{
							this.mState = XboxCharacterSelect.CharState.Hats;
							return;
						}
						if (player.CharacterName == PlayerInfo.NO_SELECTION)
						{
							this.mState = XboxCharacterSelect.CharState.Open;
							return;
						}
						this.mState = XboxCharacterSelect.CharState.AllDone;
						return;
					}
					else
					{
						if (player.CharacterName == PlayerInfo.NO_SELECTION && player.AccessoryName == PlayerInfo.NO_SELECTION)
						{
							this.mState = XboxCharacterSelect.CharState.NetPicking;
							return;
						}
						this.mState = XboxCharacterSelect.CharState.NetPicking;
					}
				}
				return;
			}
			if (this.mState == XboxCharacterSelect.CharState.NetPicking)
			{
				this.UpdateKickButton();
				this.UpdateControllerButton();
			}
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00056FBC File Offset: 0x000551BC
		private void UpdateButtonStates()
		{
			PlayerInfo player = PlayerManager.GetPlayer(this.mIndex);
			if (player != null)
			{
				if (!player.Local)
				{
					this.mSelectButton.ENABLED = false;
					this.mStatsButton.ENABLED = false;
					this.mBackButton.ENABLED = false;
					this.mAllDoneBackButton.ENABLED = false;
					this.mHatSelectButton.ENABLED = false;
					this.mHatBackButton.ENABLED = false;
					this.mJoinBackButton.ENABLED = false;
					this.mJoinButton.ENABLED = false;
					return;
				}
				this.mSelectButton.ENABLED = true;
				this.mStatsButton.ENABLED = true;
				this.mBackButton.ENABLED = true;
				this.mAllDoneBackButton.ENABLED = true;
				this.mHatSelectButton.ENABLED = true;
				this.mHatBackButton.ENABLED = true;
				this.mJoinBackButton.ENABLED = true;
				this.mJoinButton.ENABLED = true;
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x000570A4 File Offset: 0x000552A4
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.mState != XboxCharacterSelect.CharState.Stats)
			{
				spriteBatch.Draw(XboxCharacterSelect.mBG_Inactive, this.mTopLeft, this.mHUDColor * 0.5f);
			}
			if (this.Active)
			{
				int num = (int)(16f * (this.mInflateTime / this.MAX_INFLATE));
				if (PlayerManager.GetPlayer(this.mIndex) != null && !PlayerManager.GetPlayer(this.mIndex).Local)
				{
					num = 0;
				}
				Rectangle destinationRectangle = new Rectangle(this.mLocBigTex.X, this.mLocBigTex.Y, this.mLocBigTex.Width, this.mLocBigTex.Height);
				destinationRectangle.Inflate(num, num);
				if (this.mUnlockCharacter != null)
				{
					this.mUnlockCharacter.Draw(spriteBatch);
					return;
				}
				if (this.mUnlockHat != null)
				{
					this.mUnlockHat.Draw(spriteBatch);
					return;
				}
				if (this.mNotEnoughPoints != null)
				{
					this.mNotEnoughPoints.Draw(spriteBatch);
					return;
				}
				if (this.mState == XboxCharacterSelect.CharState.Open)
				{
					spriteBatch.Draw(XboxCharacterSelect.mBG, this.mTopLeft, this.mHUDColor);
					this.mScreen.Draw(spriteBatch);
					if (this.mSelected.name != "" && this.mSelected.name != null)
					{
						if (this.mCurrentItemLocked)
						{
							spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(Global.GetTexRectange(this.mSelected.texCoord.X, this.mSelected.texCoord.Y)), new Color(0.2f, 0.2f, 0.2f));
							Shadow.DrawString(this.mPointsToUnlock, Global.EquationFontSmall, this.mPointsToUnlockLoc, 1, Color.White, spriteBatch);
						}
						else
						{
							spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(Global.GetTexRectange(this.mSelected.texCoord.X, this.mSelected.texCoord.Y)), Color.White);
							GunStats stats = GunStatsLoader.GetStats(this.mSelected.startingGun);
							spriteBatch.Draw(Global.MasterTexture, this.mLocStartingGun, new Rectangle?(Global.GetTexRectange(stats.GunXCoord, stats.GunYCoord + 1)), Color.White);
							Shadow.DrawString(stats.GunName, Global.EquationFontSmall, this.mGunNamePos, 1, Color.LightGray, spriteBatch);
						}
						Shadow.DrawString(this.mSelected.name, Global.EquationFontSmall, this.mNamePos, 1, Color.White, spriteBatch);
						this.mScroll.Draw(spriteBatch);
					}
					if (PlayerManager.GetPlayer(this.mIndex) != null)
					{
						string personaName = PlayerManager.GetPlayer(this.mIndex).PersonaName;
						Shadow.DrawString(personaName, Global.EquationFontSmall, VerchickMath.CenterText(Global.EquationFontSmall, this.mGamerLoc, personaName), 1, Color.White, spriteBatch);
					}
					else
					{
						Shadow.DrawString("Player " + this.mIndex.ToString(), Global.EquationFontSmall, VerchickMath.CenterText(Global.EquationFontSmall, this.mGamerLoc, "Player " + this.mIndex.ToString()), 1, Color.White, spriteBatch);
					}
					Shadow.DrawString(this.mZombiePoints, Global.EquationFontSmall, this.mZombiePointsPos, 1, Color.White, spriteBatch);
					this.DrawStats(spriteBatch);
					spriteBatch.Draw(XboxCharacterSelect.mIcons_PC, this.mTopLeft, Color.White);
					this.mBackButton.Draw(spriteBatch);
					this.mSelectButton.Draw(spriteBatch);
					this.mStatsButton.Draw(spriteBatch);
					this.DrawKick(spriteBatch);
					this.DrawControllerIcon(spriteBatch);
					return;
				}
				if (this.mState == XboxCharacterSelect.CharState.Stats)
				{
					this.mStatsHUD.Draw(spriteBatch);
					return;
				}
				if (this.mState == XboxCharacterSelect.CharState.LoginFirst)
				{
					spriteBatch.Draw(XboxCharacterSelect.mLogin, this.mTopLeft, Color.White);
					return;
				}
				if (this.mState == XboxCharacterSelect.CharState.Hats)
				{
					spriteBatch.Draw(XboxCharacterSelect.mBG, this.mTopLeft, this.mHUDColor);
					this.mHatScreen.Draw(spriteBatch);
					spriteBatch.Draw(XboxCharacterSelect.mHatIcons, this.mTopLeft, Color.White);
					if (this.mSelectedHat.Name != "" && this.mSelectedHat.Name != null)
					{
						if (this.mCurrentItemLocked)
						{
							Shadow.DrawString(this.mSelectedHat.Name, Global.EquationFontSmall, this.mNamePos, 1, Color.White, spriteBatch);
							spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(Global.GetTexRectange(this.mSelectedHat.Tex.X, this.mSelectedHat.Tex.Y)), new Color(0.2f, 0.2f, 0.2f));
							Shadow.DrawString(this.mPointsToUnlock, Global.EquationFontSmall, this.mPointsToUnlockLoc, 1, Color.White, spriteBatch);
							for (int i = 0; i < this.mStringParsers.Count; i++)
							{
								Vector2 drawPosition = new Vector2(this.mTopLeft.X + 210f, this.mTopLeft.Y + 100f + (float)(28 * i));
								this.mStringParsers[i].Draw(spriteBatch, Global.EquationFontSmall, Global.MasterTexture, drawPosition, 2);
							}
						}
						else
						{
							Shadow.DrawString(this.mSelectedHat.Name, Global.EquationFontSmall, this.mNamePos, 1, Color.White, spriteBatch);
							spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(Global.GetTexRectange(this.mSelectedHat.Tex.X, this.mSelectedHat.Tex.Y)), Color.White);
							for (int j = 0; j < this.mStringParsers.Count; j++)
							{
								Vector2 drawPosition2 = new Vector2(this.mTopLeft.X + 210f, this.mTopLeft.Y + 100f + (float)(28 * j));
								this.mStringParsers[j].Draw(spriteBatch, Global.EquationFontSmall, Global.MasterTexture, drawPosition2, 2);
							}
						}
					}
					Shadow.DrawString(this.mZombiePoints, Global.EquationFontSmall, this.mZombiePointsPos, 1, Color.White, spriteBatch);
					this.mHatSelectButton.Draw(spriteBatch);
					this.mHatBackButton.Draw(spriteBatch);
					this.DrawControllerIcon(spriteBatch);
					this.DrawKick(spriteBatch);
					return;
				}
				if (this.mState == XboxCharacterSelect.CharState.AllDone || this.mState == XboxCharacterSelect.CharState.NetDone || this.mState == XboxCharacterSelect.CharState.NetPicking)
				{
					spriteBatch.Draw(XboxCharacterSelect.mBG_Picking, this.mTopLeft, this.mHUDColor);
					if (this.mState != XboxCharacterSelect.CharState.NetPicking)
					{
						if (this.mSelected.name != "" && this.mSelected.name != null)
						{
							this.mNamePos = VerchickMath.CenterText(Global.EquationFontSmall, this.mLocNameCenter, this.mSelected.name);
							spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(Global.GetTexRectange(this.mSelected.texCoord.X, this.mSelected.texCoord.Y)), Color.White);
							GunStats stats2 = GunStatsLoader.GetStats(this.mSelected.startingGun);
							spriteBatch.Draw(Global.MasterTexture, this.mLocStartingGun, new Rectangle?(Global.GetTexRectange(stats2.GunXCoord, stats2.GunYCoord + 1)), Color.White);
							Shadow.DrawString(stats2.GunName, Global.EquationFontSmall, this.mGunNamePos, 1, Color.LightGray, spriteBatch);
							Shadow.DrawString(this.mSelected.name, Global.EquationFontSmall, this.mNamePos, 1, Color.White, spriteBatch);
							this.mScroll.Draw(spriteBatch);
						}
						if (this.mSelectedHat.Name != "" && this.mSelectedHat.Name != null)
						{
							spriteBatch.Draw(Global.MasterTexture, this.mLocFinalSelectedHat, new Rectangle?(Global.GetTexRectange(this.mSelectedHat.Tex.X, this.mSelectedHat.Tex.Y)), Color.White);
						}
					}
					if (PlayerManager.GetPlayer(this.mIndex) != null)
					{
						PlayerInfo player = PlayerManager.GetPlayer(this.mIndex);
						string personaName2 = PlayerManager.GetPlayer(this.mIndex).PersonaName;
						Shadow.DrawString(personaName2, Global.EquationFontSmall, VerchickMath.CenterText(Global.EquationFontSmall, this.mGamerLoc, personaName2), 1, Color.White, spriteBatch);
						spriteBatch.Draw(Global.Pixel, new Rectangle((int)this.mTopLeft.X + 63 - 2, (int)this.mTopLeft.Y + 93 - 2, 68, 68), Color.Black);
						if (player.SteamAvatar != null)
						{
							spriteBatch.Draw(player.SteamAvatar, new Rectangle((int)this.mTopLeft.X + 63, (int)this.mTopLeft.Y + 93, 64, 64), Color.White);
						}
						else
						{
							Rectangle texRectange = Global.GetTexRectange(73, 0);
							spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)this.mTopLeft.X + 63, (int)this.mTopLeft.Y + 93, 64, 64), new Rectangle?(texRectange), Color.White);
						}
					}
					else
					{
						Shadow.DrawString("Player " + this.mIndex.ToString(), Global.EquationFontSmall, VerchickMath.CenterText(Global.EquationFontSmall, this.mGamerLoc, "Player " + this.mIndex.ToString()), 1, Color.White, spriteBatch);
					}
					if (PlayerManager.GetPlayer(this.mIndex) != null && PlayerManager.GetPlayer(this.mIndex).Local)
					{
						this.mAllDoneBackButton.Draw(spriteBatch);
					}
					Vector2 vector = this.mTopLeft + new Vector2(338f, 266f);
					if (this.mState == XboxCharacterSelect.CharState.NetPicking)
					{
						vector = VerchickMath.CenterText(Global.StoreFontBig, vector, "Player Picking...");
						Shadow.DrawString("Player Picking...", Global.StoreFontBig, vector, 1, Color.White * Global.Pulse, Color.Black * Global.Pulse, spriteBatch);
					}
					else
					{
						vector = VerchickMath.CenterText(Global.StoreFontBig, vector, "Player Ready!");
						Shadow.DrawString("Player Ready!", Global.StoreFontBig, vector, 1, Color.White, spriteBatch);
					}
					this.DrawKick(spriteBatch);
					this.DrawControllerIcon(spriteBatch);
					this.DrawStats(spriteBatch);
					return;
				}
				if (this.mState == XboxCharacterSelect.CharState.WaitingToJoin)
				{
					if (this.mLobbyManager.PrivateLobby)
					{
						Vector2 pos = VerchickMath.CenterText(Global.StoreFontBig, new Vector2(this.mTopLeft.X + 250f, this.mTopLeft.Y + 44f), "Private Lobby");
						Shadow.DrawString("Private Lobby", Global.StoreFontBig, pos, 1, Color.WhiteSmoke, spriteBatch);
						pos = VerchickMath.CenterText(Global.StoreFont, new Vector2(this.mTopLeft.X + 250f, this.mTopLeft.Y + 74f), "Click Start Search to Find Players");
						Shadow.DrawString("Click Start Search to Find Players", Global.StoreFont, pos, 1, Color.LightGray, spriteBatch);
						pos = VerchickMath.CenterText(Global.StoreFont, new Vector2(this.mTopLeft.X + 250f, this.mTopLeft.Y + 104f), "...or Join Locally!");
						Shadow.DrawString("...or join locally!", Global.StoreFont, pos, 1, Color.LightGray, spriteBatch);
						return;
					}
					this.mWaitingIcon.Draw(spriteBatch);
					Vector2 pos2 = VerchickMath.CenterText(Global.StoreFontBig, new Vector2(this.mTopLeft.X + 250f, this.mTopLeft.Y + 44f), "Waiting For Player...");
					Shadow.DrawString("Waiting For Player...", Global.StoreFontBig, pos2, 1, Color.WhiteSmoke, spriteBatch);
					return;
				}
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00057C3C File Offset: 0x00055E3C
		private void DrawStats(SpriteBatch spriteBatch)
		{
			if (PlayerManager.GetPlayer(this.mIndex) != null && !PlayerManager.GetPlayer(this.mIndex).Local && this.mState != XboxCharacterSelect.CharState.NetDone)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			foreach (StringParser stringParser in this.mStringParsers)
			{
				Vector2 drawPosition = new Vector2(this.mTopLeft.X + 190f + (float)num, this.mTopLeft.Y + 174f + (float)num2);
				stringParser.Draw(spriteBatch, Global.EquationFontSmall, Global.MasterTexture, drawPosition, 1);
				num += 64;
				if (num >= 256)
				{
					num = 0;
					num2 += 18;
				}
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00057D0C File Offset: 0x00055F0C
		public void Close()
		{
			this.mScreen.ItemHighlighted -= this.mScreen_ItemHighlighted;
			this.mHatScreen.ItemHighlighted -= this.mHatScreen_ItemHighlighted;
			SignedInGamer.SignedIn -= this.SignedInGamer_SignedIn;
			SignedInGamer.SignedOut -= this.SignedInGamer_SignedOut;
			this.mSelectButton.OnPressed -= this.mScreen_ItemSelected;
			this.mHatSelectButton.OnPressed -= this.mHatScreen_ItemSelected;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00057D98 File Offset: 0x00055F98
		private void mScreen_ItemSelected(object sender, EventArgs args)
		{
			if (this.mCurrentlyHighlightedChar == null)
			{
				return;
			}
			XboxItem xboxItem = this.mCurrentlyHighlightedChar;
			if (xboxItem.Locked)
			{
				if (this.mGamerStats.Points < this.mSelected.PointsToUnlock)
				{
					SoundEngine.PlaySound("ze2_death", 1f);
					this.mNotEnoughPoints = new XboxStoreDialog("You don't have enough points to unlock this!", this.mTopLeft, this.mIndex, XboxStoreDialog.XboxDialogType.Ok);
					return;
				}
				SoundEngine.PlaySound("ze2_navup", 0.3f);
				this.mUnlockCharacter = new XboxStoreDialog("Do you want to unlock this character for " + this.mSelected.PointsToUnlock + " points?", this.mTopLeft, this.mIndex, XboxStoreDialog.XboxDialogType.YesNo);
				return;
			}
			else
			{
				SoundEngine.PlaySound("ze2_navup", 0.3f);
				XboxSaverLoader.SaveData();
				if (xboxItem == null)
				{
					return;
				}
				this.FinalSettings = (CharacterSettings)xboxItem.Tag;
				PlayerManager.GetPlayer(this.mIndex).CharacterName = this.FinalSettings.name;
				PlayerManager.GetPlayer(this.mIndex).AccessoryName = PlayerInfo.NO_SELECTION;
				this.mLobbyManager.UpdateWhoIWantToBe(this.mIndex);
				this.mState = XboxCharacterSelect.CharState.Hats;
				this.mHatScreen.RefireEvent(false);
				return;
			}
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00057ECC File Offset: 0x000560CC
		private void mScreen_ItemHighlighted(XboxItem item)
		{
			if (item == null)
			{
				this.mSelected.name = "";
				this.mCurrentlyHighlightedChar = null;
				this.mStringParsers.Clear();
				return;
			}
			this.mCurrentItemLocked = item.Locked;
			this.mInflateTime = this.MAX_INFLATE;
			if (this.mCurrentItemLocked)
			{
				this.mSelectButton.SetText("Unlock");
			}
			else
			{
				this.mSelectButton.SetText("Select");
			}
			this.mSelected = (CharacterSettings)item.Tag;
			this.mNamePos = VerchickMath.CenterText(Global.EquationFontSmall, this.mLocNameCenter, this.mSelected.name);
			this.mScroll = new ScrollBox(this.mSelected.description, new Rectangle((int)this.mTopLeft.X + 192, (int)this.mTopLeft.Y + 76, 304, 135), Global.StoreFontSmall, null, Color.LightBlue);
			this.mPointsToUnlock = "Points: " + this.mSelected.PointsToUnlock.ToString();
			if (PlayerManager.GetPlayer(this.mIndex) != null && PlayerManager.GetPlayer(this.mIndex).Local && this.mState == XboxCharacterSelect.CharState.Open)
			{
				this.UpdateStatsGUI(this.mSelected.Properties);
			}
			else
			{
				this.mStringParsers.Clear();
			}
			this.mCurrentlyHighlightedChar = item;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00058030 File Offset: 0x00056230
		private void Setup()
		{
			int num = 520;
			int num2 = 310;
			Global.GetSafeScreenArea();
			int num3 = (int)Global.GetScreenCenter().X - 100;
			int num4 = (int)Global.GetScreenCenter().Y - 48;
			switch (this.mIndex)
			{
			case 0:
				this.mHUDColor = new Color(136, 178, 229);
				this.mTopLeft = new Vector2((float)(num3 - num), (float)(num4 - num2));
				break;
			case 1:
				this.mHUDColor = new Color(255, 140, 140);
				this.mTopLeft = new Vector2((float)(num3 + 20), (float)(num4 - num2));
				break;
			case 2:
				this.mHUDColor = new Color(136, 229, 178);
				this.mTopLeft = new Vector2((float)(num3 - num), (float)(num4 + 0));
				break;
			case 3:
				this.mHUDColor = new Color(229, 229, 136);
				this.mTopLeft = new Vector2((float)(num3 + 20), (float)(num4 + 0));
				break;
			}
			this.mKickPosition = new Rectangle((int)this.mTopLeft.X + 500 - 20, (int)this.mTopLeft.Y + 4, 16, 16);
			this.mControllerIcon = new Rectangle((int)this.mTopLeft.X + 500 - 20, (int)this.mTopLeft.Y + 4, 16, 16);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x000581B4 File Offset: 0x000563B4
		private void UpdateKickButton()
		{
			if (!this.mLobbyManager.IAmLobbyCreator)
			{
				return;
			}
			PlayerInfo player = PlayerManager.GetPlayer(this.mIndex);
			if (player == null)
			{
				return;
			}
			if (player.Local)
			{
				return;
			}
			if (InputManager.LeftMouseClicked() && this.mKickPosition.Contains(InputManager.GetMousePosition()))
			{
				this.mLobbyManager.KickPlayer(player.SteamID);
			}
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00058214 File Offset: 0x00056414
		private void UpdateControllerButton()
		{
			PlayerInfo player = PlayerManager.GetPlayer(this.mIndex);
			if (player == null)
			{
				return;
			}
			if (!player.Local)
			{
				return;
			}
			if (InputManager.LeftMouseClicked() && this.mControllerIcon.Contains(InputManager.GetMousePosition()))
			{
				int num = player.ControllerIndex;
				num++;
				while (!this.IsContIndexAvail(num, player.Index))
				{
					num++;
					if (num >= 4)
					{
						num = -1;
					}
				}
				Terminal.WriteMessage(string.Concat(new object[]
				{
					"Swapping controller index of player ",
					player.Index,
					" to ",
					num
				}));
				player.ControllerIndex = num;
				this.mLobbyManager.LocalPlayerChangedContIndex(this.mIndex, num);
			}
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x000582CC File Offset: 0x000564CC
		private bool IsContIndexAvail(int checkIndex, int myPlayerIndex)
		{
			for (int i = 0; i < 4; i++)
			{
				if (i != myPlayerIndex)
				{
					PlayerInfo player = PlayerManager.GetPlayer(i);
					if (player != null && player.Local && checkIndex == player.ControllerIndex)
					{
						return false;
					}
				}
			}
			return checkIndex == -1 || InputManager.ContollerConnected(checkIndex);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00058314 File Offset: 0x00056514
		private void DrawKick(SpriteBatch spriteBatch)
		{
			if (!this.mLobbyManager.IAmLobbyCreator)
			{
				return;
			}
			PlayerInfo player = PlayerManager.GetPlayer(this.mIndex);
			if (player == null)
			{
				return;
			}
			if (player.Local)
			{
				return;
			}
			Rectangle texRectange = Global.GetTexRectange(13, 8);
			if (this.mKickPosition.Contains(InputManager.GetMousePosition()))
			{
				texRectange = Global.GetTexRectange(14, 8);
			}
			spriteBatch.Draw(Global.MasterTexture, this.mKickPosition, new Rectangle?(texRectange), Color.White);
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00058388 File Offset: 0x00056588
		private void DrawControllerIcon(SpriteBatch spriteBatch)
		{
			PlayerInfo player = PlayerManager.GetPlayer(this.mIndex);
			if (player == null)
			{
				return;
			}
			if (!player.Local)
			{
				return;
			}
			Rectangle texRectange = Global.GetTexRectange(11 + player.ControllerIndex, 42);
			if (this.mControllerIcon.Contains(InputManager.GetMousePosition()))
			{
				texRectange = Global.GetTexRectange(11 + player.ControllerIndex, 43);
			}
			spriteBatch.Draw(Global.MasterTexture, this.mKickPosition, new Rectangle?(texRectange), Color.White);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x000583FE File Offset: 0x000565FE
		public void FlashLoginThing()
		{
			this.Active = true;
			this.mState = XboxCharacterSelect.CharState.LoginFirst;
			SoundEngine.PlaySound("ze2_death", 1f);
			this.LoginTime = 0f;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00058428 File Offset: 0x00056628
		private void mHatScreen_ItemSelected(object sender, EventArgs args)
		{
			if (this.mCurrentlyHighlightedHat == null)
			{
				return;
			}
			if (!this.mCurrentlyHighlightedHat.Locked)
			{
				SoundEngine.PlaySound("ze2_navup", 0.3f);
				this.FinalHat = (Hat)this.mCurrentlyHighlightedHat.Tag;
				PlayerManager.GetPlayer(this.mIndex).AccessoryName = this.FinalHat.Name;
				this.mLobbyManager.UpdateWhoIWantToBe(this.mIndex);
				this.mState = XboxCharacterSelect.CharState.AllDone;
				if (PlayerManager.GetPlayer(this.mIndex) != null && PlayerManager.GetPlayer(this.mIndex).Local && this.mState == XboxCharacterSelect.CharState.AllDone)
				{
					if (this.FinalHat.Name != "None")
					{
						SpecialProperties stats = new SpecialProperties();
						SpecialProperties.AddUpProps(ref stats, this.mSelected.Properties);
						SpecialProperties.AddUpProps(ref stats, this.FinalHat.Properties);
						this.UpdateStatsGUI(stats);
						return;
					}
					this.UpdateStatsGUI(this.mSelected.Properties);
				}
				return;
			}
			if (this.mGamerStats.Points < this.mSelectedHat.ZHCost)
			{
				SoundEngine.PlaySound("ze2_death", 1f);
				this.mNotEnoughPoints = new XboxStoreDialog("You don't have enough points to unlock this!", this.mTopLeft, this.mIndex, XboxStoreDialog.XboxDialogType.Ok);
				return;
			}
			SoundEngine.PlaySound("ze2_navup", 0.3f);
			this.mUnlockHat = new XboxStoreDialog("Do you want to unlock this hat for " + this.mSelectedHat.ZHCost + " points?", this.mTopLeft, this.mIndex, XboxStoreDialog.XboxDialogType.YesNo);
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x000585B4 File Offset: 0x000567B4
		private void mHatScreen_ItemHighlighted(XboxItem item)
		{
			if (item != null)
			{
				this.mSelectedHat = (Hat)item.Tag;
				this.mInflateTime = this.MAX_INFLATE;
				this.mNamePos = VerchickMath.CenterText(Global.EquationFontSmall, this.mLocNameCenter, this.mSelectedHat.Name);
				this.mCurrentItemLocked = item.Locked;
				if (this.mCurrentItemLocked)
				{
					this.mHatSelectButton.SetText("Unlock");
				}
				else
				{
					this.mHatSelectButton.SetText("Select");
				}
				this.mPointsToUnlock = "Points: " + this.mSelectedHat.ZHCost.ToString();
				this.mStringParsers.Clear();
				if (PlayerManager.GetPlayer(this.mIndex) != null && PlayerManager.GetPlayer(this.mIndex).Local && this.mState == XboxCharacterSelect.CharState.Hats && this.mSelectedHat.PropDesc != null)
				{
					for (int i = 0; i < this.mSelectedHat.PropDesc.Count; i++)
					{
						this.mStringParsers.Add(new StringParser(this.mSelectedHat.PropDesc[i], StringAlignment.Centered));
					}
				}
				this.mCurrentlyHighlightedHat = item;
				return;
			}
			this.mSelectedHat.Name = "";
			this.mCurrentlyHighlightedHat = null;
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x000586F7 File Offset: 0x000568F7
		public bool AllDoneAndWaiting
		{
			get
			{
				return PlayerManager.GetPlayer(this.mIndex) != null && !(PlayerManager.GetPlayer(this.mIndex).AccessoryName == PlayerInfo.NO_SELECTION);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00005D3F File Offset: 0x00003F3F
		public bool GamerFound
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00058728 File Offset: 0x00056928
		public void AssertSelections()
		{
			PlayerInfo player = PlayerManager.GetPlayer(this.mIndex);
			if (player == null)
			{
				this.mState = XboxCharacterSelect.CharState.WaitingToJoin;
				return;
			}
			this.mScreen.PopulateCharacters(PlayerStatKeeper.CharacterSettings, this.mGamerStats);
			this.mHatScreen.PopulateHats(PCHatStore.GetHatList(), this.mGamerStats);
			if (this.mSelected.name != player.CharacterName && player.CharacterName != PlayerInfo.NO_SELECTION)
			{
				this.mScreen.ReselectItem(player.CharacterName);
			}
			if (this.mSelectedHat.Name != player.AccessoryName && player.AccessoryName != PlayerInfo.NO_SELECTION)
			{
				this.mHatScreen.ReselectItem(player.AccessoryName);
			}
			if (player.Local)
			{
				if (player.AccessoryName != PlayerInfo.NO_SELECTION)
				{
					this.mStringParsers.Clear();
					if (player.AccessoryName != "NONE")
					{
						SpecialProperties stats = new SpecialProperties();
						SpecialProperties.AddUpProps(ref stats, this.mSelected.Properties);
						SpecialProperties.AddUpProps(ref stats, this.FinalHat.Properties);
						this.UpdateStatsGUI(stats);
						return;
					}
					this.UpdateStatsGUI(this.mSelected.Properties);
					return;
				}
			}
			else
			{
				this.mStringParsers.Clear();
				if (player.AccessoryName != PlayerInfo.NO_SELECTION)
				{
					this.mState = XboxCharacterSelect.CharState.NetDone;
					this.mStringParsers.Clear();
					if (player.AccessoryName != "NONE")
					{
						SpecialProperties stats2 = new SpecialProperties();
						SpecialProperties.AddUpProps(ref stats2, this.mSelected.Properties);
						SpecialProperties.AddUpProps(ref stats2, this.FinalHat.Properties);
						this.UpdateStatsGUI(stats2);
						return;
					}
					this.UpdateStatsGUI(this.mSelected.Properties);
					return;
				}
				else
				{
					this.mState = XboxCharacterSelect.CharState.NetPicking;
					this.mStringParsers.Clear();
				}
			}
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00058903 File Offset: 0x00056B03
		public void ClearSelection()
		{
			this.mSelected.name = "";
			this.mSelectedHat.Name = "";
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00058925 File Offset: 0x00056B25
		internal void Activate()
		{
			this.Active = true;
			this.mState = XboxCharacterSelect.CharState.WaitingToJoin;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00058938 File Offset: 0x00056B38
		private void JoinLocal()
		{
			Terminal.WriteMessage("Local player joined index: " + this.mIndex);
			PlayerManager.AddPlayer(this.mIndex, true, SteamHelper.GetLocalID());
			this.mGamerStats = Global.GamerStats;
			this.mState = XboxCharacterSelect.CharState.Open;
			this.mLobbyManager.UpdateWhoIWantToBe(this.mIndex);
			this.mLobbyManager.SetLobbyData();
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x000589A0 File Offset: 0x00056BA0
		private void UpdateStatsGUI(SpecialProperties stats)
		{
			this.mStringParsers.Clear();
			SpecialProperties properties = PlayerStatKeeper.DEFAULT_CHAR.Properties;
			foreach (StringParser stringParser in new List<StringParser>
			{
				this.GetParser(stats.MaxHealth, properties.MaxHealth, 68, 42),
				this.GetParser((float)stats.Armor, (float)properties.Armor, 69, 43),
				this.GetParser(stats.Speed, properties.Speed, 69, 42),
				this.GetParser(stats.HealingDoneMod, properties.HealingDoneMod, 70, 42),
				this.GetParser(stats.LifeStealPercent, properties.LifeStealPercent, 70, 43),
				this.GetParser(stats.CritChance, properties.CritChance, 71, 42),
				this.GetParser(stats.BulletDamageMod, properties.BulletDamageMod, 68, 44),
				this.GetParser(stats.ShotTimeMod, properties.ShotTimeMod, 68, 45),
				this.GetParser(stats.ReloadTimeMod, properties.ReloadTimeMod, 68, 43),
				this.GetParser(stats.MeleeDamageMod, properties.MeleeDamageMod, 69, 44),
				this.GetParser(stats.SwingTimeMod, properties.SwingTimeMod, 69, 45),
				this.GetParser(stats.ExplosionDamageMod, properties.ExplosionDamageMod, 71, 43),
				this.GetParser(stats.ExplosionRadiusMod, properties.ExplosionRadiusMod, 72, 43),
				this.GetParser(stats.MinionDmgMod, properties.MinionDmgMod, 70, 44),
				this.GetParser(stats.MinionFireRateMod, properties.MinionFireRateMod, 70, 45),
				this.GetParser((float)stats.Ammo_Assault, (float)properties.Ammo_Assault, 71, 44),
				this.GetParser((float)stats.Ammo_Shells, (float)properties.Ammo_Shells, 71, 45),
				this.GetParser((float)stats.Ammo_Heavy, (float)properties.Ammo_Heavy, 71, 46),
				this.GetParser((float)stats.Ammo_Explosive, (float)properties.Ammo_Explosive, 71, 47),
				this.GetParser(stats.EarthDmg, properties.EarthDmg, 69, 46),
				this.GetParser(stats.FireDmg, properties.FireDmg, 68, 46),
				this.GetParser(stats.WaterDmg, properties.WaterDmg, 70, 46),
				this.GetParser((float)stats.EarthResist, (float)properties.EarthResist, 69, 47),
				this.GetParser((float)stats.FireResist, (float)properties.FireResist, 68, 47),
				this.GetParser((float)stats.WaterResist, (float)properties.WaterResist, 70, 47)
			})
			{
				if (stringParser != null)
				{
					this.mStringParsers.Add(stringParser);
				}
			}
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00058CDC File Offset: 0x00056EDC
		private StringParser GetParser(float stats, float def, int texX, int texY)
		{
			if (stats == def)
			{
				return null;
			}
			string arg = string.Format("[{0},{1}]", texX * 16, texY * 16);
			string text;
			if (def == 0f)
			{
				if (stats >= def + 50f)
				{
					text = "+++";
				}
				else if (stats >= def + 25f)
				{
					text = "++";
				}
				else if (stats > def)
				{
					text = "+";
				}
				else if (stats <= def - 50f)
				{
					text = "---";
				}
				else if (stats <= def - 25f)
				{
					text = "--";
				}
				else
				{
					text = "-";
				}
			}
			else if (stats >= def * XboxCharacterSelect.PLUSPLUSPLUS)
			{
				text = "+++";
			}
			else if (stats >= def * XboxCharacterSelect.PLUSPLUS)
			{
				text = "++";
			}
			else if (stats > def)
			{
				text = "+";
			}
			else if (stats <= def * XboxCharacterSelect.MINUSMINUSMINUS)
			{
				text = "---";
			}
			else if (stats <= def * XboxCharacterSelect.MINUSMINUS)
			{
				text = "--";
			}
			else
			{
				text = "-";
			}
			string arg2;
			if (text.Contains("+"))
			{
				arg2 = "LightGreen";
			}
			else
			{
				arg2 = "Red";
			}
			return new StringParser(string.Format("[{0}]{1}{2}", arg2, arg, text), StringAlignment.Centered);
		}

		// Token: 0x04000B52 RID: 2898
		private static Texture2D mBG;

		// Token: 0x04000B53 RID: 2899
		private static Texture2D mBGJoin;

		// Token: 0x04000B54 RID: 2900
		private static Texture2D mBG_Picking;

		// Token: 0x04000B55 RID: 2901
		private static Texture2D mIcons;

		// Token: 0x04000B56 RID: 2902
		private static Texture2D mIcons_PC;

		// Token: 0x04000B57 RID: 2903
		private static Texture2D mReady;

		// Token: 0x04000B58 RID: 2904
		private static Texture2D mLogin;

		// Token: 0x04000B59 RID: 2905
		private static Texture2D mHatIcons;

		// Token: 0x04000B5A RID: 2906
		private static Texture2D mBG_Inactive;

		// Token: 0x04000B5B RID: 2907
		public static Texture2D mPressStart;

		// Token: 0x04000B5C RID: 2908
		private LobbyManager mLobbyManager;

		// Token: 0x04000B5D RID: 2909
		private XboxItemScreen mScreen;

		// Token: 0x04000B5E RID: 2910
		private XboxItemScreen mHatScreen;

		// Token: 0x04000B5F RID: 2911
		private Color mHUDColor;

		// Token: 0x04000B60 RID: 2912
		public Vector2 mTopLeft;

		// Token: 0x04000B61 RID: 2913
		public int mIndex;

		// Token: 0x04000B62 RID: 2914
		private CharacterSettings mSelected;

		// Token: 0x04000B63 RID: 2915
		private Hat mSelectedHat;

		// Token: 0x04000B64 RID: 2916
		public bool Active;

		// Token: 0x04000B65 RID: 2917
		private Rectangle mLocBigTex;

		// Token: 0x04000B66 RID: 2918
		private Vector2 mLocNameCenter;

		// Token: 0x04000B67 RID: 2919
		private ScrollBox mScroll;

		// Token: 0x04000B68 RID: 2920
		private Vector2 mNamePos;

		// Token: 0x04000B69 RID: 2921
		private Vector2 mGunNamePos;

		// Token: 0x04000B6A RID: 2922
		private Vector2 mZombiePointsPos;

		// Token: 0x04000B6B RID: 2923
		private XboxStatsHUD mStatsHUD;

		// Token: 0x04000B6C RID: 2924
		private Rectangle mLocStartingGun;

		// Token: 0x04000B6D RID: 2925
		private Rectangle mLocFinalSelectedHat;

		// Token: 0x04000B6E RID: 2926
		private float mInflateTime;

		// Token: 0x04000B6F RID: 2927
		private float MAX_INFLATE = 0.15f;

		// Token: 0x04000B70 RID: 2928
		private ZEButton mSelectButton;

		// Token: 0x04000B71 RID: 2929
		private ZEButton mStatsButton;

		// Token: 0x04000B72 RID: 2930
		private ZEButton mBackButton;

		// Token: 0x04000B73 RID: 2931
		private ZEButton mAllDoneBackButton;

		// Token: 0x04000B74 RID: 2932
		private ZEButton mHatSelectButton;

		// Token: 0x04000B75 RID: 2933
		private ZEButton mHatBackButton;

		// Token: 0x04000B76 RID: 2934
		private ZEButton mJoinBackButton;

		// Token: 0x04000B77 RID: 2935
		private ZEButton mJoinButton;

		// Token: 0x04000B78 RID: 2936
		private XboxItem mCurrentlyHighlightedChar;

		// Token: 0x04000B79 RID: 2937
		private XboxItem mCurrentlyHighlightedHat;

		// Token: 0x04000B7A RID: 2938
		private LoadIcon mWaitingIcon;

		// Token: 0x04000B7B RID: 2939
		private Rectangle mKickPosition;

		// Token: 0x04000B7C RID: 2940
		private Rectangle mControllerIcon;

		// Token: 0x04000B7D RID: 2941
		private bool mCurrentItemLocked;

		// Token: 0x04000B7E RID: 2942
		public XboxCharacterSelect.CharState mState;

		// Token: 0x04000B7F RID: 2943
		public CharacterSettings FinalSettings;

		// Token: 0x04000B80 RID: 2944
		public Hat FinalHat;

		// Token: 0x04000B81 RID: 2945
		private float LoginTime;

		// Token: 0x04000B82 RID: 2946
		private List<StringParser> mStringParsers = new List<StringParser>();

		// Token: 0x04000B83 RID: 2947
		private string mZombiePoints = "0";

		// Token: 0x04000B84 RID: 2948
		private int prevPoints;

		// Token: 0x04000B85 RID: 2949
		public SignedInGamer mGamer;

		// Token: 0x04000B86 RID: 2950
		public XboxGamerStats mGamerStats;

		// Token: 0x04000B87 RID: 2951
		private Vector2 mGamerLoc;

		// Token: 0x04000B88 RID: 2952
		private Vector2 mPointsToUnlockLoc;

		// Token: 0x04000B89 RID: 2953
		private string mPointsToUnlock;

		// Token: 0x04000B8A RID: 2954
		private XboxStoreDialog mUnlockCharacter;

		// Token: 0x04000B8B RID: 2955
		private XboxStoreDialog mNotEnoughPoints;

		// Token: 0x04000B8C RID: 2956
		private XboxStoreDialog mUnlockHat;

		// Token: 0x04000B8D RID: 2957
		private static float PLUSPLUSPLUS = 1.5f;

		// Token: 0x04000B8E RID: 2958
		private static float PLUSPLUS = 1.25f;

		// Token: 0x04000B8F RID: 2959
		private static float MINUSMINUS = 0.75f;

		// Token: 0x04000B90 RID: 2960
		private static float MINUSMINUSMINUS = 0.5f;

		// Token: 0x02000227 RID: 551
		public enum CharState
		{
			// Token: 0x04000E63 RID: 3683
			Open,
			// Token: 0x04000E64 RID: 3684
			Hats,
			// Token: 0x04000E65 RID: 3685
			Stats,
			// Token: 0x04000E66 RID: 3686
			AllDone,
			// Token: 0x04000E67 RID: 3687
			LoginFirst,
			// Token: 0x04000E68 RID: 3688
			WaitingToJoin,
			// Token: 0x04000E69 RID: 3689
			NetPicking,
			// Token: 0x04000E6A RID: 3690
			NetDone
		}
	}
}
