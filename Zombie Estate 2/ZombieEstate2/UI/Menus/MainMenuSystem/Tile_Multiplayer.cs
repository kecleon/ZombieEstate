using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.UI.Menus.MainMenuSystem
{
	// Token: 0x0200016D RID: 365
	internal class Tile_Multiplayer : MenuTile
	{
		// Token: 0x06000B18 RID: 2840 RVA: 0x0005BCDC File Offset: 0x00059EDC
		public Tile_Multiplayer(Vector2 pos, Vector2 menuPos) : base("Multiplayer", pos, menuPos)
		{
			this.Host = new PCButton("Host Game", Color.CornflowerBlue, base.GetMenuPos(new Vector2((float)(-128 - PCButton.ButtonTex.Width / 2), 0f)));
			this.Join = new PCButton("Join Game", Color.LightGreen, base.GetMenuPos(new Vector2((float)(128 - PCButton.ButtonTex.Width / 2), 0f)));
			this.Host.Pressed = new PCButton.PressedDelegate(this.HostPressed);
			this.Join.Pressed = new PCButton.PressedDelegate(this.JoinPressed);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0005BD91 File Offset: 0x00059F91
		public override void UpdateMenu()
		{
			this.Host.Update();
			this.Join.Update();
			base.UpdateMenu();
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0005BDAF File Offset: 0x00059FAF
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			this.Host.Draw(spriteBatch);
			this.Join.Draw(spriteBatch);
			base.DrawCenteredTitle("Multiplayer", spriteBatch, Global.StoreFontXtraLarge);
			base.DrawMenu(spriteBatch);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void HostPressed()
		{
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void JoinPressed()
		{
		}

		// Token: 0x04000BE3 RID: 3043
		private PCButton Host;

		// Token: 0x04000BE4 RID: 3044
		private PCButton Join;
	}
}
