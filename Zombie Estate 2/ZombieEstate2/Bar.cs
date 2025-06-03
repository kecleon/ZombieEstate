using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000054 RID: 84
	public class Bar
	{
		// Token: 0x060001EF RID: 495 RVA: 0x0000E1BC File Offset: 0x0000C3BC
		public Bar(bool flipped, Vector2 pos, Color FGColor, Color BGColor)
		{
			this.Flipped = flipped;
			this.FGColor = FGColor;
			this.BGColor = BGColor;
			this.Position = pos;
			if (Bar.BG_Flipped == null)
			{
				Bar.BG_Big = Global.Content.Load<Texture2D>("HUD\\NewHud\\Bar_Big");
				Bar.BG_Flipped = Global.Content.Load<Texture2D>("HUD\\NewHud\\Bar_Flipped");
			}
			if (!flipped)
			{
				this.BG = Bar.BG_Big;
				this.CenterTextLoc = new Vector2(this.Position.X + 24f, this.Position.Y);
			}
			else
			{
				this.BG = Bar.BG_Flipped;
				this.CenterTextLoc = new Vector2(this.Position.X + 232f, this.Position.Y + 10f);
			}
			this.Src = new Rectangle(0, 0, this.BG.Width, this.BG.Height);
			this.Dest = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.BG.Width, this.BG.Height);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000E2E6 File Offset: 0x0000C4E6
		public void Draw(SpriteBatch spriteBatch, int actual, int total)
		{
			this.Draw(spriteBatch, actual, total, false);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000E2F4 File Offset: 0x0000C4F4
		public void Draw(SpriteBatch spriteBatch, int actual, int total, bool text)
		{
			float num = (float)actual / (float)total;
			int width = (int)((float)this.BG.Width * num);
			this.Src.Width = width;
			this.Dest.Width = width;
			spriteBatch.Draw(this.BG, this.Position, this.BGColor);
			spriteBatch.Draw(this.BG, this.Dest, new Rectangle?(this.Src), this.FGColor);
			if (text)
			{
				string text2 = string.Format("{0}/{1}", actual, total);
				Vector2 centerTextLoc = this.CenterTextLoc;
				if (this.Flipped)
				{
					centerTextLoc.X -= Global.StoreFont.MeasureString(text2).X;
				}
				Shadow.DrawString(text2, Global.StoreFont, centerTextLoc, 1, Color.White, spriteBatch);
			}
		}

		// Token: 0x04000172 RID: 370
		private static Texture2D BG_Big;

		// Token: 0x04000173 RID: 371
		private static Texture2D BG_Flipped;

		// Token: 0x04000174 RID: 372
		private Vector2 Position;

		// Token: 0x04000175 RID: 373
		private Color FGColor;

		// Token: 0x04000176 RID: 374
		private Color BGColor;

		// Token: 0x04000177 RID: 375
		private Texture2D BG;

		// Token: 0x04000178 RID: 376
		private Rectangle Src;

		// Token: 0x04000179 RID: 377
		private Rectangle Dest;

		// Token: 0x0400017A RID: 378
		private bool Flipped;

		// Token: 0x0400017B RID: 379
		private Vector2 CenterTextLoc;
	}
}
