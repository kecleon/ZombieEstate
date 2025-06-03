using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.UI.Menus.MainMenuSystem
{
	// Token: 0x0200016E RID: 366
	internal class Tile_Singleplayer : MenuTile
	{
		// Token: 0x06000B1D RID: 2845 RVA: 0x0005BDE4 File Offset: 0x00059FE4
		public Tile_Singleplayer(Vector2 pos, Vector2 menuPos) : base("SinglePlayer", pos, menuPos)
		{
			this.Join = new PCButton("Play", Color.LightGreen, base.GetMenuPos(new Vector2((float)(-(float)PCButton.ButtonTex.Width / 2), 0f)));
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0005BE31 File Offset: 0x0005A031
		public override void UpdateMenu()
		{
			this.Join.Update();
			base.UpdateMenu();
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0005BE44 File Offset: 0x0005A044
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			this.Join.Draw(spriteBatch);
			base.DrawCenteredTitle("Single Player", spriteBatch, Global.StoreFontXtraLarge);
			base.DrawMenu(spriteBatch);
		}

		// Token: 0x04000BE5 RID: 3045
		private PCButton Join;
	}
}
