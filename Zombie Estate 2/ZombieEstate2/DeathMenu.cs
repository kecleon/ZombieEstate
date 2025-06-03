using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Networking;

namespace ZombieEstate2
{
	// Token: 0x020000E0 RID: 224
	internal class DeathMenu : Menu
	{
		// Token: 0x060005E0 RID: 1504 RVA: 0x0002C44C File Offset: 0x0002A64C
		public DeathMenu() : base(false, new Vector2((float)(2 * Global.ScreenRect.Width / 3 - 64), (float)(Global.ScreenRect.Height / 2 + 200)))
		{
			this.MenuBG = Global.Content.Load<Texture2D>("Menus//GameOver");
			this.ScaleBG = false;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0002C4B4 File Offset: 0x0002A6B4
		public override void Setup()
		{
			MusicEngine.Play(ZE2Songs.AllDead);
			this.mTimer = 9f;
			DeathMenu.CONTINUES--;
			if (DeathMenu.CONTINUES >= 0)
			{
				if (DeathMenu.CONTINUES == 1)
				{
					this.mContinuesText = string.Format("{0} Continue Remaining", DeathMenu.CONTINUES);
				}
				else
				{
					this.mContinuesText = string.Format("{0} Continues Remaining", DeathMenu.CONTINUES);
				}
			}
			else
			{
				base.AddToMenu("Admit Defeat", new MenuItem.SelectedDelegate(this.Defeat), false);
				PingManager.DISABLE_PINGS = true;
			}
			this.DrawBGPixel = true;
			this.title = "";
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0002C557 File Offset: 0x0002A757
		public override void UpdateMenu()
		{
			base.UpdateMenu();
			if (DeathMenu.CONTINUES >= 0)
			{
				this.mTimer -= Global.REAL_GAME_TIME;
				if (this.mTimer <= 0f)
				{
					this.Continue();
				}
			}
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0002C58C File Offset: 0x0002A78C
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
			if (DeathMenu.CONTINUES >= 0)
			{
				Vector2 pos = VerchickMath.CenterText(Global.StoreFontXtraLarge, Global.GetScreenCenter() + new Vector2(0f, -100f), this.mContinuesText);
				Shadow.DrawString(this.mContinuesText, Global.StoreFontXtraLarge, pos, 1, Color.Red, spriteBatch);
				string text = "Continuing in " + ((int)this.mTimer).ToString();
				pos = VerchickMath.CenterText(Global.StoreFontBig, Global.GetScreenCenter() + new Vector2(0f, 20f), text);
				Shadow.DrawString(text, Global.StoreFontBig, pos, 1, Color.White, spriteBatch);
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0002C63F File Offset: 0x0002A83F
		private void Continue()
		{
			Global.WaveMaster.ResetWave();
			Global.WaveMaster.RespawnAllPlayers();
			MenuManager.MenuClosed();
			MusicEngine.Play(ZE2Songs.Wave);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0002C661 File Offset: 0x0002A861
		private void End()
		{
			MenuManager.PushMenu(new QuitAreYouSureMenu(true));
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0002C66E File Offset: 0x0002A86E
		private void Defeat()
		{
			MenuManager.CLOSEALL();
			Global.WaveMaster.EndGameDefeat();
		}

		// Token: 0x040005C7 RID: 1479
		public static int CONTINUES;

		// Token: 0x040005C8 RID: 1480
		private string mContinuesText = "";

		// Token: 0x040005C9 RID: 1481
		private float mTimer;
	}
}
