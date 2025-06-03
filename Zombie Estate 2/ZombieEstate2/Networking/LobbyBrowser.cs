using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using ZombieEstate2.UI.Menus;
using ZombieEstate2.UI.Xbox;

namespace ZombieEstate2.Networking
{
	// Token: 0x020001AA RID: 426
	public class LobbyBrowser
	{
		// Token: 0x06000BC5 RID: 3013 RVA: 0x0006037C File Offset: 0x0005E57C
		public LobbyBrowser()
		{
			int y = (int)((float)Global.ScreenRect.Height * 0.1f + 90f);
			this.mLoad = new LoadIcon(new Rectangle((int)Global.GetScreenCenter().X - 32, y, 64, 64));
			XboxLevelSelect.LoadIcons();
			this.mOnLobbyMatchList = CallResult<LobbyMatchList_t>.Create(new CallResult<LobbyMatchList_t>.APIDispatchDelegate(this.OnLobbyMatchList));
			this.UpdateList();
			this.mListHeader = string.Format("Lobby {0}:    {1,-15}{2, -15}{3, -20}", new object[]
			{
				"ID",
				"Level",
				"Slots",
				"Mode"
			});
			this.mHeaderLoc = VerchickMath.CenterText(Global.StoreFontBig, new Vector2(Global.GetScreenCenter().X, (float)((int)((float)Global.ScreenRect.Height * 0.1f + 160f))), this.mListHeader);
			this.SetUpScrollArrow();
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00060484 File Offset: 0x0005E684
		public void Update()
		{
			if (this.mTryingToJoin)
			{
				return;
			}
			if (this.mSearchingCooldown > 0f)
			{
				this.mSearchingCooldown -= Global.REAL_GAME_TIME;
			}
			this.mLoad.Update(Global.REAL_GAME_TIME);
			this.mPollTime -= Global.REAL_GAME_TIME;
			if (this.mPollTime < 0f)
			{
				this.UpdateList();
			}
			this.Inputs();
			this.ScrollInputs();
			if (this.mPrevIndex != this.mSelectedIndex)
			{
				SoundEngine.PlaySound("ze2_menuselect", 0.1f);
			}
			this.mPrevIndex = this.mSelectedIndex;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00060524 File Offset: 0x0005E724
		private void Inputs()
		{
			if (this.mLobbies.Count == 0)
			{
				this.mSelectedIndex = 0;
				return;
			}
			if (this.mSomeLobbiesFound)
			{
				if (InputManager.HasMouseMoved())
				{
					for (int i = 0; i < this.mLobbies.Count; i++)
					{
						if (i >= this.mCurrentWindowTop && i < this.mCurrentWindowTop + this.WINDOW_SIZE && this.mLobbies[i].GetRect(i - this.mCurrentWindowTop).Contains(InputManager.GetMousePosition()))
						{
							this.mSelectedIndex = i;
							break;
						}
					}
				}
				if (InputManager.LeftMouseClicked())
				{
					if (this.mSelectedIndex < 0 || this.mSelectedIndex >= this.mLobbies.Count)
					{
						return;
					}
					if (this.mLobbies[this.mSelectedIndex].GetRect(this.mSelectedIndex - this.mCurrentWindowTop).Contains(InputManager.GetMousePosition()))
					{
						SoundEngine.PlaySound("ze2_navup", 0.3f);
						this.TryJoinLobby(this.mLobbies[this.mSelectedIndex]);
						return;
					}
				}
			}
			int j = 0;
			while (j < 4)
			{
				if (InputManager.ButtonPressed(ButtonPress.Affirmative, j, true) && !ScreenFader.Active)
				{
					if (this.mSelectedIndex < 0 || this.mSelectedIndex >= this.mLobbies.Count)
					{
						return;
					}
					SoundEngine.PlaySound("ze2_navup", 0.3f);
					Global.GameManager.INITIATED_CONTROL_INDEX = j;
					this.TryJoinLobby(this.mLobbies[this.mSelectedIndex]);
					return;
				}
				else
				{
					if (InputManager.ButtonPressed(ButtonPress.MoveSouth, j, true))
					{
						SoundEngine.PlaySound("ze2_menunav", 0.3f);
						this.mSelectedIndex++;
						if (this.mSelectedIndex < 0)
						{
							this.mSelectedIndex = 0;
						}
						if (this.mSelectedIndex >= this.mLobbies.Count)
						{
							this.mSelectedIndex = this.mLobbies.Count - 1;
						}
						if (this.mCurrentWindowTop + this.mSelectedIndex > this.WINDOW_SIZE - 1)
						{
							this.mCurrentWindowTop = this.mSelectedIndex - this.WINDOW_SIZE + 1;
						}
						this.UpdateWindow();
					}
					if (InputManager.ButtonPressed(ButtonPress.MoveNorth, j, true))
					{
						SoundEngine.PlaySound("ze2_menunav", 0.3f);
						this.mSelectedIndex--;
						if (this.mSelectedIndex < 0)
						{
							this.mSelectedIndex = 0;
						}
						if (this.mSelectedIndex >= this.mLobbies.Count)
						{
							this.mSelectedIndex = this.mLobbies.Count - 1;
						}
						if (this.mCurrentWindowTop + this.mSelectedIndex > this.WINDOW_SIZE - 1)
						{
							this.mCurrentWindowTop = this.mSelectedIndex - this.WINDOW_SIZE + 1;
						}
						this.UpdateWindow();
					}
					j++;
				}
			}
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x000607D0 File Offset: 0x0005E9D0
		private void ScrollInputs()
		{
			if (InputManager.LeftMouseClicked())
			{
				if (this.mUpArrowPos.Contains(InputManager.GetMousePosition()))
				{
					this.mCurrentWindowTop--;
					this.UpdateWindow();
				}
				if (this.mDownArrowPos.Contains(InputManager.GetMousePosition()))
				{
					this.mCurrentWindowTop++;
					this.UpdateWindow();
				}
			}
			if (InputManager.MouseWheelUp())
			{
				this.mCurrentWindowTop--;
				this.UpdateWindow();
			}
			if (InputManager.MouseWheelDown())
			{
				this.mCurrentWindowTop++;
				this.UpdateWindow();
			}
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00060868 File Offset: 0x0005EA68
		private void TryJoinLobby(LobbyItem lobby)
		{
			this.Log("Attempting to join lobby: " + lobby.mLobbyID);
			this.mLobbyToJoin = lobby.mLobbyID;
			this.mTryingToJoin = true;
			this.Log("Forcing search...");
			if (!this.mSearching)
			{
				this.PerformSearch();
			}
			this.Log("Forced search...");
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000608C8 File Offset: 0x0005EAC8
		private void JoinLobby(LobbyItem lobby)
		{
			this.DESTROY();
			this.Log(string.Concat(new object[]
			{
				"Lobby selected: ",
				lobby.mListIndex,
				" ",
				lobby.mLobbyID
			}));
			this.Log("Attempting to join...");
			MainMenu.LOBBY_TO_JOIN = lobby.mLobbyID;
			MainMenu.HOSTING_GAME = false;
			MenuManager.CLOSEALL();
			Global.GameManager.GotoCharSelect();
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00060944 File Offset: 0x0005EB44
		private void UpdateWindow()
		{
			if (this.mLobbies.Count == 0)
			{
				return;
			}
			if (this.mSelectedIndex >= this.mLobbies.Count)
			{
				this.mSelectedIndex = this.mLobbies.Count - 1;
			}
			if (this.mCurrentWindowTop + this.WINDOW_SIZE > this.mLobbies.Count)
			{
				this.mCurrentWindowTop = this.mLobbies.Count - this.WINDOW_SIZE;
			}
			if (this.mCurrentWindowTop < 0)
			{
				this.mCurrentWindowTop = 0;
			}
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x000609C7 File Offset: 0x0005EBC7
		private void UpdateList()
		{
			if (this.mSearching)
			{
				return;
			}
			this.SetSearching(true);
			this.PerformSearch();
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x000609E0 File Offset: 0x0005EBE0
		private void PerformSearch()
		{
			this.mPollTime = LobbyBrowser.POLL_TIME;
			SteamMatchmaking.AddRequestLobbyListStringFilter("version", Global.VersionNumber, ELobbyComparison.k_ELobbyComparisonEqual);
			int nValueToMatch = 1;
			SteamMatchmaking.AddRequestLobbyListNumericalFilter("openSlots", nValueToMatch, ELobbyComparison.k_ELobbyComparisonEqualToOrGreaterThan);
			SteamAPICall_t hAPICall = SteamMatchmaking.RequestLobbyList();
			this.mOnLobbyMatchList.Set(hAPICall, null);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00060A2C File Offset: 0x0005EC2C
		private void OnLobbyMatchList(LobbyMatchList_t list, bool bIOFailure)
		{
			if (this.DESTROYED)
			{
				Terminal.WriteMessage("Destroyed lobbybrowser received OnLobbyMatchList", MessageType.ERROR);
				return;
			}
			this.SetSearching(false);
			this.UpdateWindow();
			this.mLobbies.Clear();
			if (list.m_nLobbiesMatching != 0U && !bIOFailure)
			{
				this.mSomeLobbiesFound = true;
				int num = 0;
				while ((long)num < (long)((ulong)list.m_nLobbiesMatching) && num < 10)
				{
					CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(num);
					string lobbyData = this.GetLobbyData("map", lobbyByIndex);
					int num2 = int.Parse(this.GetLobbyData("difficulty", lobbyByIndex), CultureInfo.InvariantCulture);
					int num3 = int.Parse(this.GetLobbyData("openSlots", lobbyByIndex), CultureInfo.InvariantCulture);
					string text = "";
					if (num2 == 1)
					{
						text = "Casual";
					}
					else if (num2 == 2)
					{
						text = "Hard";
					}
					else if (num2 == 3)
					{
						text = "Unlimited";
					}
					this.Log(string.Format("Lobby {0}: {1}, {2}, {3}", new object[]
					{
						num,
						lobbyData,
						text,
						num3
					}));
					this.mLobbies.Add(new LobbyItem(lobbyByIndex, lobbyData, text, num3, num));
					num++;
				}
				if (this.mTryingToJoin)
				{
					foreach (LobbyItem lobbyItem in this.mLobbies)
					{
						if (lobbyItem.mLobbyID == this.mLobbyToJoin)
						{
							this.Log("Lobby still there. Joining.");
							this.JoinLobby(lobbyItem);
							return;
						}
					}
					Terminal.WriteMessage("Lobby gone or private.", MessageType.ERROR);
					this.DESTROY();
					Global.GameManager.GotoMainMenu();
					MenuManager.PushMenu(new KickedMenu("The lobby no longer exists!"));
					return;
				}
				return;
			}
			this.Log("No lobbies found...");
			if (this.mTryingToJoin)
			{
				Terminal.WriteMessage("Lobby gone or private.", MessageType.ERROR);
				this.DESTROY();
				Global.GameManager.GotoMainMenu();
				MenuManager.PushMenu(new KickedMenu("The lobby no longer exists!"));
				return;
			}
			this.mSomeLobbiesFound = false;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00060C38 File Offset: 0x0005EE38
		public void Draw(SpriteBatch spritebatch)
		{
			if (this.mSearching || this.mSearchingCooldown > 0f || !this.mSomeLobbiesFound || this.mTryingToJoin)
			{
				this.mLoad.Draw(spritebatch);
			}
			if (this.mTryingToJoin)
			{
				Vector2 vector = Global.GetScreenCenter();
				string text = "Attempting to join...";
				vector = VerchickMath.CenterText(Global.StoreFontBig, vector, text);
				Shadow.DrawString(text, Global.StoreFontBig, vector, 1, Color.White, spritebatch);
				return;
			}
			Shadow.DrawString(this.mListHeader, Global.StoreFontBig, this.mHeaderLoc, 1, Color.White, spritebatch);
			if (this.mSomeLobbiesFound)
			{
				for (int i = 0; i < this.mLobbies.Count; i++)
				{
					if (i >= this.mCurrentWindowTop && i < this.mCurrentWindowTop + this.WINDOW_SIZE)
					{
						this.mLobbies[i].Draw(spritebatch, i - this.mCurrentWindowTop, i == this.mSelectedIndex);
					}
				}
				if (this.mUpArrowPos.Contains(InputManager.GetMousePosition()))
				{
					spritebatch.Draw(Global.MasterTexture, this.mUpArrowPos, new Rectangle?(Global.GetTexRectange(5, 21)), Color.Lerp(Color.Gray, Color.White, Global.Pulse));
				}
				else
				{
					spritebatch.Draw(Global.MasterTexture, this.mUpArrowPos, new Rectangle?(Global.GetTexRectange(5, 21)), Color.Gray);
				}
				if (this.mDownArrowPos.Contains(InputManager.GetMousePosition()))
				{
					spritebatch.Draw(Global.MasterTexture, this.mDownArrowPos, new Rectangle?(Global.GetTexRectange(4, 21)), Color.Lerp(Color.Gray, Color.White, Global.Pulse));
					return;
				}
				spritebatch.Draw(Global.MasterTexture, this.mDownArrowPos, new Rectangle?(Global.GetTexRectange(4, 21)), Color.Gray);
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00060DF6 File Offset: 0x0005EFF6
		private string GetLobbyData(string key, CSteamID lobbyID)
		{
			return SteamMatchmaking.GetLobbyData(lobbyID, key);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00060DFF File Offset: 0x0005EFFF
		public void DESTROY()
		{
			this.DESTROYED = true;
			this.Log("DESTROYED");
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00060E13 File Offset: 0x0005F013
		private void Log(string s)
		{
			Terminal.WriteMessage("LobbyBrowser: " + s, MessageType.SAVELOAD);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00060E26 File Offset: 0x0005F026
		private void SetSearching(bool searching)
		{
			this.mSearching = searching;
			if (!this.mSearching)
			{
				this.mSearchingCooldown = 1f;
			}
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00060E44 File Offset: 0x0005F044
		private void SetUpScrollArrow()
		{
			int x = (int)(Global.GetScreenCenter().X + (float)LobbyItem.WIDTH / 2f + 4f);
			int y = (int)((float)Global.ScreenRect.Height * 0.1f + 180f);
			this.mUpArrowPos = new Rectangle(x, y, 64, 64);
			y = (int)((float)(this.WINDOW_SIZE * LobbyItem.HEIGHT) + ((float)Global.ScreenRect.Height * 0.1f + 180f) + (float)(LobbyItem.PADDING * this.WINDOW_SIZE)) - 64;
			this.mDownArrowPos = new Rectangle(x, y, 64, 64);
		}

		// Token: 0x04000C1C RID: 3100
		public static float POLL_TIME = 10f;

		// Token: 0x04000C1D RID: 3101
		private int WINDOW_SIZE = 6;

		// Token: 0x04000C1E RID: 3102
		public static int LOBBY_COUNT = 15;

		// Token: 0x04000C1F RID: 3103
		private bool mSomeLobbiesFound;

		// Token: 0x04000C20 RID: 3104
		private CallResult<LobbyMatchList_t> mOnLobbyMatchList;

		// Token: 0x04000C21 RID: 3105
		private List<LobbyItem> mLobbies = new List<LobbyItem>();

		// Token: 0x04000C22 RID: 3106
		private float mPollTime;

		// Token: 0x04000C23 RID: 3107
		private bool mSearching;

		// Token: 0x04000C24 RID: 3108
		private LoadIcon mLoad;

		// Token: 0x04000C25 RID: 3109
		private float mSearchingCooldown;

		// Token: 0x04000C26 RID: 3110
		private int mCurrentWindowTop;

		// Token: 0x04000C27 RID: 3111
		private int mSelectedIndex;

		// Token: 0x04000C28 RID: 3112
		private string mListHeader;

		// Token: 0x04000C29 RID: 3113
		private Vector2 mHeaderLoc;

		// Token: 0x04000C2A RID: 3114
		private Rectangle mUpArrowPos;

		// Token: 0x04000C2B RID: 3115
		private Rectangle mDownArrowPos;

		// Token: 0x04000C2C RID: 3116
		private int mPrevIndex;

		// Token: 0x04000C2D RID: 3117
		private CSteamID mLobbyToJoin = CSteamID.Nil;

		// Token: 0x04000C2E RID: 3118
		private bool mTryingToJoin;

		// Token: 0x04000C2F RID: 3119
		private bool DESTROYED;
	}
}
