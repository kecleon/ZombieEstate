using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.UI.Menus
{
	// Token: 0x02000163 RID: 355
	internal class LogoMenu : Menu
	{
		// Token: 0x06000ADA RID: 2778 RVA: 0x00059F30 File Offset: 0x00058130
		public LogoMenu() : base(false, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2)))
		{
			this.MenuBG = Global.Content.Load<Texture2D>("Logo\\SSEBg");
			this.SSE1 = Global.Content.Load<Texture2D>("Logo\\SSE1");
			this.SSE2 = Global.Content.Load<Texture2D>("Logo\\SSE2");
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00059FA2 File Offset: 0x000581A2
		public override void Setup()
		{
			this.title = "";
			this.DrawBGPixel = true;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00059FB8 File Offset: 0x000581B8
		public override void UpdateMenu()
		{
			if (this.DONE)
			{
				return;
			}
			if (this.mShowingText)
			{
				this.mAlpha2 += Global.REAL_GAME_TIME * 0.75f;
			}
			else
			{
				this.mAlpha1 += Global.REAL_GAME_TIME * 0.75f;
			}
			if (this.mAlpha1 >= 1f)
			{
				this.mAlpha1 = 1f;
				this.mShowingText = true;
			}
			if (this.mAlpha2 >= 1f)
			{
				this.mAlpha2 = 1f;
			}
			this.mTotalTime += Global.REAL_GAME_TIME;
			if (this.mTotalTime >= LogoMenu.TOTAL_TIME)
			{
				this.DONE = true;
				ScreenFader.Fade(delegate()
				{
					MenuManager.CLOSEALL();
					MenuManager.PushMenu(new PressStart());
				}, 0.025f);
				return;
			}
			base.UpdateMenu();
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0005A098 File Offset: 0x00058298
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
			Vector2 position = new Vector2((float)((Global.ScreenRect.Width - this.SSE1.Width) / 2), (float)((Global.ScreenRect.Height - this.SSE1.Height) / 2));
			spriteBatch.Draw(this.SSE1, position, Color.White * this.mAlpha1);
			spriteBatch.Draw(this.SSE2, position, Color.White * this.mAlpha2);
		}

		// Token: 0x04000BA1 RID: 2977
		private float mAlpha1;

		// Token: 0x04000BA2 RID: 2978
		private float mAlpha2;

		// Token: 0x04000BA3 RID: 2979
		private bool DONE;

		// Token: 0x04000BA4 RID: 2980
		private float mTotalTime;

		// Token: 0x04000BA5 RID: 2981
		private static float TOTAL_TIME = 5f;

		// Token: 0x04000BA6 RID: 2982
		private bool mShowingText;

		// Token: 0x04000BA7 RID: 2983
		private Texture2D SSE1;

		// Token: 0x04000BA8 RID: 2984
		private Texture2D SSE2;
	}
}
