using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.UI.Menus
{
	// Token: 0x02000169 RID: 361
	internal class HelpMenu : Menu
	{
		// Token: 0x06000AFC RID: 2812 RVA: 0x0005AE14 File Offset: 0x00059014
		public HelpMenu() : base(false, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height - 110)))
		{
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0005AE3D File Offset: 0x0005903D
		public override void Setup()
		{
			this.title = "";
			this.MenuBG = Global.MenuBG;
			base.AddToMenu("Continue", new MenuItem.SelectedDelegate(this.BackPressed), false);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0005AE70 File Offset: 0x00059070
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
			Vector2 position = new Vector2((float)(Global.ScreenRect.Width - HelpMenu.mTex.Width) / 2f, (float)(Global.ScreenRect.Height - HelpMenu.mTex.Height) / 2f);
			spriteBatch.Draw(HelpMenu.mTex, position, Color.White);
		}

		// Token: 0x04000BBB RID: 3003
		public static Texture2D mTex;
	}
}
