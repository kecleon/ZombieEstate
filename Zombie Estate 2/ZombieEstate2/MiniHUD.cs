using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200005F RID: 95
	internal class MiniHUD
	{
		// Token: 0x0600021E RID: 542 RVA: 0x0000FA48 File Offset: 0x0000DC48
		public MiniHUD(Player p)
		{
			this.parent = p;
			this.dest = default(Rectangle);
			this.dest.X = 64;
			this.dest.Y = Global.ScreenRect.Height - 480 - 100 * p.Index;
			this.dest.Width = 64;
			this.dest.Height = 64;
			this.BG = Global.GetTexRectange(0, 37);
			this.src = Global.GetTexRectange(p.StartTextureCoord.X, p.StartTextureCoord.Y);
			this.Bar = new Rectangle(64, this.dest.Y + 68, 64, 12);
			this.BarBG = new Rectangle(63, this.dest.Y + 67, 66, 14);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000FB28 File Offset: 0x0000DD28
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Global.MasterTexture, this.dest, new Rectangle?(this.BG), this.parent.HUDColor);
			spriteBatch.Draw(Global.MasterTexture, this.dest, new Rectangle?(this.src), Color.White);
			int width = (int)(64f * this.parent.GetHealthDelta());
			this.Bar.Width = 64;
			spriteBatch.Draw(Global.Pixel, this.BarBG, Color.Black);
			spriteBatch.Draw(Global.Pixel, this.Bar, Color.Red);
			this.Bar.Width = width;
			spriteBatch.Draw(Global.Pixel, this.Bar, Color.LightGreen);
			if (this.ShowReady)
			{
				Vector2 position = new Vector2(132f, (float)(this.dest.Y + 12));
				if (this.Ready)
				{
					Shadow.DrawOutlinedString(spriteBatch, Global.StoreFontBig, "Ready!", Color.Black, Color.LightGreen, 1f, 0f, position);
					return;
				}
				Shadow.DrawOutlinedString(spriteBatch, Global.StoreFontBig, "Not Ready!", Color.Black, Color.Red, 1f, 0f, position);
			}
		}

		// Token: 0x040001E2 RID: 482
		public Player parent;

		// Token: 0x040001E3 RID: 483
		private Rectangle dest;

		// Token: 0x040001E4 RID: 484
		private Rectangle src;

		// Token: 0x040001E5 RID: 485
		private Rectangle BG;

		// Token: 0x040001E6 RID: 486
		private Rectangle Bar;

		// Token: 0x040001E7 RID: 487
		private Rectangle BarBG;

		// Token: 0x040001E8 RID: 488
		public bool Ready;

		// Token: 0x040001E9 RID: 489
		public bool ShowReady;
	}
}
