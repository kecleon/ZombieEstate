using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200009C RID: 156
	internal class SaveConfigChangesMenu : Menu
	{
		// Token: 0x0600040D RID: 1037 RVA: 0x0001D840 File Offset: 0x0001BA40
		public SaveConfigChangesMenu(bool blackout, int newScreenWidth, int newScreenHeight, bool fullscreen, bool vSync) : base(false, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2)))
		{
			this.mFull = fullscreen;
			this.mW = newScreenWidth;
			this.mH = newScreenHeight;
			this.mVSync = vSync;
			this.DrawBGPixel = blackout;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001D898 File Offset: 0x0001BA98
		public override void Setup()
		{
			base.AddToMenu("Yes", new MenuItem.SelectedDelegate(this.Yes), false);
			base.AddToMenu("No", new MenuItem.SelectedDelegate(this.No), false);
			this.title = "Apply Resolution Changes?";
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001D8D8 File Offset: 0x0001BAD8
		public void Yes()
		{
			Config.Instance.ScreenWidth = this.mW;
			Config.Instance.ScreenHeight = this.mH;
			Config.Instance.VSync = this.mVSync;
			if (this.mFull)
			{
				Config.Instance.ScreenMode = ScreenMode.FullScreen;
			}
			else
			{
				Config.Instance.ScreenMode = ScreenMode.Windowed;
			}
			Global.ScreenRect = new Rectangle(0, 0, Config.Instance.ScreenWidth, Config.Instance.ScreenHeight);
			Global.Graphics.PreferredBackBufferWidth = Config.Instance.ScreenWidth;
			Global.Graphics.PreferredBackBufferHeight = Config.Instance.ScreenHeight;
			Global.Graphics.IsFullScreen = (Config.Instance.ScreenMode == ScreenMode.FullScreen);
			Global.Graphics.SynchronizeWithVerticalRetrace = this.mVSync;
			Global.Graphics.ApplyChanges();
			Config.Save();
			MenuManager.CLOSEALL();
			Global.GameManager.GotoMainMenu();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001D9C2 File Offset: 0x0001BBC2
		public void No()
		{
			MenuManager.MenuClosed();
		}

		// Token: 0x040003E2 RID: 994
		private int mW;

		// Token: 0x040003E3 RID: 995
		private int mH;

		// Token: 0x040003E4 RID: 996
		private bool mFull;

		// Token: 0x040003E5 RID: 997
		private bool mVSync;
	}
}
