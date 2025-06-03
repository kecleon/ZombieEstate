using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.UI.Menus
{
	// Token: 0x02000162 RID: 354
	internal class KickedMenu : Menu
	{
		// Token: 0x06000AD7 RID: 2775 RVA: 0x00059E8A File Offset: 0x0005808A
		public KickedMenu(string msg) : base(false, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height - 110)))
		{
			this.mMessage = msg;
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00059EBA File Offset: 0x000580BA
		public override void Setup()
		{
			this.title = "";
			this.MenuBG = Global.MenuBG;
			base.AddToMenu("Back", new MenuItem.SelectedDelegate(this.BackPressed), false);
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00059EEC File Offset: 0x000580EC
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
			Vector2 pos = VerchickMath.CenterText(Global.StoreFontBig, Global.GetScreenCenter(), this.mMessage);
			Shadow.DrawString(this.mMessage, Global.StoreFontBig, pos, 1, Color.White, spriteBatch);
		}

		// Token: 0x04000BA0 RID: 2976
		private string mMessage;
	}
}
