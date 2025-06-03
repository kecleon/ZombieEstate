using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x0200009D RID: 157
	internal class PressStart : Menu
	{
		// Token: 0x06000411 RID: 1041 RVA: 0x0001D9CC File Offset: 0x0001BBCC
		public PressStart() : base(false, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2)))
		{
			this.MenuBG = Global.Content.Load<Texture2D>("Menus\\MainMenuBG_NoTitle");
			this.mTitle = Global.Content.Load<Texture2D>("ZETitle");
			this.mPressLoc = VerchickMath.CenterText(Global.StoreFontBig, Global.GetScreenCenter(), this.mPress);
			this.mPressLoc.Y = (float)Global.ScreenRect.Height * 0.84f;
			this.BGScale = 2;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001DA93 File Offset: 0x0001BC93
		public override void Setup()
		{
			this.title = "";
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001DAA0 File Offset: 0x0001BCA0
		public override void UpdateMenu()
		{
			this.mTitleAlpha += Global.REAL_GAME_TIME * 1.1f;
			if (this.mTitleAlpha >= 1f)
			{
				this.mTitleAlpha = 1f;
			}
			this.counter += 0.033333335f;
			if (!this.hitOne)
			{
				this.alpha += 0.016666668f;
			}
			if (this.alpha > 1f)
			{
				this.alpha = 1f;
				this.hitOne = true;
			}
			if (Global.CHEAT)
			{
				for (int i = 0; i < 4; i++)
				{
					using (IEnumerator enumerator = Enum.GetValues(typeof(Keys)).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (InputManager.ButtonPressed((Keys)enumerator.Current, 0))
							{
								SoundEngine.PlaySound("ze2_navup", 0.3f);
								this.mIndex = (PlayerIndex)i;
								this.Resume();
								return;
							}
						}
					}
				}
			}
			if (this.mTitleAlpha >= 1f)
			{
				this.mPressAlpha += Global.REAL_GAME_TIME * 0.5f;
				if (this.mPressAlpha >= 1f)
				{
					this.mPressAlpha = 1f;
				}
				for (int j = 0; j < 4; j++)
				{
					using (IEnumerator enumerator = Enum.GetValues(typeof(Keys)).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (InputManager.ButtonPressed((Keys)enumerator.Current, 0))
							{
								SoundEngine.PlaySound("ze2_navup", 0.3f);
								this.mIndex = (PlayerIndex)j;
								this.Resume();
								return;
							}
						}
					}
					using (IEnumerator enumerator = Enum.GetValues(typeof(ButtonPress)).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (InputManager.ButtonPressed((ButtonPress)enumerator.Current, j, true))
							{
								SoundEngine.PlaySound("ze2_navup", 0.3f);
								this.mIndex = (PlayerIndex)j;
								this.Resume();
								return;
							}
						}
					}
				}
			}
			base.UpdateMenu();
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001DCEC File Offset: 0x0001BEEC
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
			if (this.mTitleAlpha > 0f)
			{
				Vector2 position = new Vector2((float)(Global.ScreenRect.Width - this.mTitle.Width) / 2f, (float)(Global.ScreenRect.Height - this.mTitle.Height) / 2f);
				spriteBatch.Draw(this.mTitle, position, Color.White * this.mTitleAlpha);
			}
			if (this.mPressAlpha > 0f)
			{
				Shadow.DrawOutlinedString(spriteBatch, Global.StoreFontBig, this.mPress, Color.Black * this.mPressAlpha, Color.White * this.mPressAlpha, 1f, 0f, this.mPressLoc);
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001DDBC File Offset: 0x0001BFBC
		private void Resume()
		{
			if (Program.JOIN_LOBBY)
			{
				Terminal.WriteMessage("Press Start -> Joining lobby from invite.");
				MainMenu.HOSTING_GAME = false;
				MainMenu.LOBBY_TO_JOIN = Program.LOBBY_ID;
				MenuManager.CLOSEALL();
				Global.GameManager.GotoCharSelect();
				return;
			}
			Terminal.WriteMessage("Press Start -> Main Menu");
			MenuManager.CLOSEALL();
			MenuManager.PushMenu(new MainMenu());
			Global.GameState = GameState.MainMenu;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void ShowDeviceSelect()
		{
		}

		// Token: 0x040003E6 RID: 998
		private string mPress = "Press Any Key to Start";

		// Token: 0x040003E7 RID: 999
		private Vector2 mPressLoc;

		// Token: 0x040003E8 RID: 1000
		private float alpha = -1f;

		// Token: 0x040003E9 RID: 1001
		private bool hitOne;

		// Token: 0x040003EA RID: 1002
		private float counter;

		// Token: 0x040003EB RID: 1003
		private float mTitleAlpha = -0.5f;

		// Token: 0x040003EC RID: 1004
		private float mPressAlpha = -0.9f;

		// Token: 0x040003ED RID: 1005
		private Texture2D mTitle;

		// Token: 0x040003EE RID: 1006
		private PlayerIndex mIndex;
	}
}
