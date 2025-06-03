using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieEstate2.Networking;

namespace ZombieEstate2.UI.Menus
{
	// Token: 0x02000161 RID: 353
	internal class BrowseLobbiesMenu : Menu
	{
		// Token: 0x06000AD1 RID: 2769 RVA: 0x00059C98 File Offset: 0x00057E98
		public BrowseLobbiesMenu() : base(false)
		{
			this.MenuBG = Global.MenuBG;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00059CAC File Offset: 0x00057EAC
		public override void Setup()
		{
			this.DrawBGPixel = false;
			this.title = "Lobby Browser";
			this.mBrowser = new LobbyBrowser();
			this.mBackButton = new ZEButton(new Rectangle((int)Global.GetScreenCenter().X - 120, Global.ScreenRect.Bottom - 40, 240, 32), "Back [Esc]", ButtonPress.Negative, 0);
			this.mBackButton.OnPressed += this.MBackButton_OnPressed;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00059D26 File Offset: 0x00057F26
		private void MBackButton_OnPressed(object sender, EventArgs e)
		{
			this.BackPressed();
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00059D30 File Offset: 0x00057F30
		public override void UpdateMenu()
		{
			base.UpdateMenu();
			this.mBrowser.Update();
			for (int i = 0; i < 4; i++)
			{
				if (InputManager.ButtonPressed(ButtonPress.Negative, i, true))
				{
					this.BackPressed();
					return;
				}
			}
			if (InputManager.ButtonPressed(Keys.Escape, 0))
			{
				this.BackPressed();
				return;
			}
			this.mBackButton.Update();
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00059D87 File Offset: 0x00057F87
		public override void BackPressed()
		{
			SoundEngine.PlaySound("ze2_navdown", 0.3f);
			this.mBrowser.DESTROY();
			base.BackPressed();
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00059DAC File Offset: 0x00057FAC
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
			this.mBrowser.Draw(spriteBatch);
			int num = (int)((float)Global.ScreenRect.Height * 0.1f + 60f);
			string text = "Showing first " + LobbyBrowser.LOBBY_COUNT + ", non-full lobbies.";
			Vector2 pos = VerchickMath.CenterText(Global.StoreFontBig, new Vector2(Global.GetScreenCenter().X, (float)num), text);
			Shadow.DrawString(text, Global.StoreFontBig, pos, 1, Color.LightGray, spriteBatch);
			text = "Refreshes every " + LobbyBrowser.POLL_TIME + " seconds.";
			num += 20;
			pos = VerchickMath.CenterText(Global.StoreFont, new Vector2(Global.GetScreenCenter().X, (float)num), text);
			Shadow.DrawString(text, Global.StoreFont, pos, 1, Color.LightGray, spriteBatch);
			this.mBackButton.Draw(spriteBatch);
		}

		// Token: 0x04000B9E RID: 2974
		private LobbyBrowser mBrowser;

		// Token: 0x04000B9F RID: 2975
		private ZEButton mBackButton;
	}
}
