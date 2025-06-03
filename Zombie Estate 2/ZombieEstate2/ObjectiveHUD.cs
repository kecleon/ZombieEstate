using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000061 RID: 97
	public class ObjectiveHUD
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000FE70 File Offset: 0x0000E070
		public ObjectiveHUD(string text, int iconX, int iconY, Vector2 pos, Color color, Color barColor)
		{
			if (ObjectiveHUD.BG == null)
			{
				ObjectiveHUD.BG = Global.Content.Load<Texture2D>("HUD\\NewHud\\ObjectiveBG");
				ObjectiveHUD.Bar = Global.Content.Load<Texture2D>("HUD\\NewHud\\ObjectiveBar");
			}
			this.Text = text;
			this.TextPos = VerchickMath.CenterText(Global.StoreFont, new Vector2(pos.X + 160f, pos.Y + 24f), this.Text);
			this.HudColor = color;
			this.BarColor = barColor;
			this.Position = pos;
			this.IconSrc = Global.GetTexRectange(iconX, iconY);
			this.IconDest = new Rectangle((int)this.Position.X + 26, (int)this.Position.Y + 48, 32, 32);
			this.BarSrc = new Rectangle(0, 0, ObjectiveHUD.Bar.Width, ObjectiveHUD.Bar.Height);
			this.BarDest = new Rectangle((int)pos.X + 12, (int)pos.Y + 47, ObjectiveHUD.Bar.Width, ObjectiveHUD.Bar.Height);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000FF98 File Offset: 0x0000E198
		public void Draw(SpriteBatch spriteBatch, int actual, int total)
		{
			float num = (float)actual / (float)total;
			int width = (int)((float)ObjectiveHUD.BG.Width * num);
			this.BarSrc.Width = width;
			this.BarDest.Width = width;
			spriteBatch.Draw(ObjectiveHUD.BG, this.Position, this.HudColor);
			spriteBatch.Draw(ObjectiveHUD.Bar, this.BarDest, new Rectangle?(this.BarSrc), this.BarColor);
			Shadow.DrawString(this.Text, Global.StoreFont, this.TextPos, 1, Color.White, spriteBatch);
			Vector2 pos = new Vector2(this.Position.X + 64f, this.Position.Y + 52f);
			spriteBatch.Draw(Global.MasterTexture, this.IconDest, new Rectangle?(this.IconSrc), Color.White);
			Shadow.DrawString(string.Format("{0}/{1}", VerchickMath.AddCommas(actual), VerchickMath.AddCommas(total)), Global.StoreFont, pos, 1, Color.White, spriteBatch);
		}

		// Token: 0x040001EE RID: 494
		private Rectangle IconSrc;

		// Token: 0x040001EF RID: 495
		private Rectangle IconDest;

		// Token: 0x040001F0 RID: 496
		private Vector2 BarTex;

		// Token: 0x040001F1 RID: 497
		private Rectangle BarSrc;

		// Token: 0x040001F2 RID: 498
		private Rectangle BarDest;

		// Token: 0x040001F3 RID: 499
		private Vector2 Position;

		// Token: 0x040001F4 RID: 500
		private static Texture2D BG;

		// Token: 0x040001F5 RID: 501
		private static Texture2D Bar;

		// Token: 0x040001F6 RID: 502
		private string Text;

		// Token: 0x040001F7 RID: 503
		private Vector2 TextPos;

		// Token: 0x040001F8 RID: 504
		private Color HudColor;

		// Token: 0x040001F9 RID: 505
		private Color BarColor;
	}
}
