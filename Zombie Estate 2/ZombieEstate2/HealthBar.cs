using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200005D RID: 93
	public class HealthBar
	{
		// Token: 0x06000217 RID: 535 RVA: 0x0000F5B4 File Offset: 0x0000D7B4
		public HealthBar(Vector2 topLeft, int portX, int portY, bool flipped, Color FG, Color BG, Color myColor)
		{
			this.PortaitSrc = Global.GetTexRectange(portX, portY);
			this.Position = topLeft;
			this.MyColor = myColor;
			if (HealthBar.BGBig == null)
			{
				HealthBar.BGBig = Global.Content.Load<Texture2D>("HUD\\NewHud\\BG_Big");
				HealthBar.BGFlipped = Global.Content.Load<Texture2D>("HUD\\NewHud\\BG_Big_Flipped");
			}
			if (!flipped)
			{
				this.SetupBig();
			}
			else
			{
				this.SetupFlipped();
			}
			this.PortriatDest.X = this.PortriatDest.X + (int)this.Position.X;
			this.PortriatDest.Y = this.PortriatDest.Y + (int)this.Position.Y;
			this.BarPosition.X = this.BarPosition.X + this.Position.X;
			this.BarPosition.Y = this.BarPosition.Y + this.Position.Y;
			this.Bar = new Bar(flipped, this.BarPosition, FG, BG);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000F6A5 File Offset: 0x0000D8A5
		private void SetupBig()
		{
			this.BG = HealthBar.BGBig;
			this.PortriatDest = new Rectangle(16, 16, 64, 64);
			this.BarPosition = new Vector2(68f, 4f);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000F6DA File Offset: 0x0000D8DA
		private void SetupFlipped()
		{
			this.BG = HealthBar.BGFlipped;
			this.PortriatDest = new Rectangle(259, 10, 64, 64);
			this.BarPosition = new Vector2(14f, 51f);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000F714 File Offset: 0x0000D914
		public void Draw(SpriteBatch spriteBatch, int actual, int total)
		{
			spriteBatch.Draw(this.BG, this.Position, this.MyColor);
			this.Bar.Draw(spriteBatch, actual, total, true);
			spriteBatch.Draw(Global.MasterTexture, this.PortriatDest, new Rectangle?(this.PortaitSrc), Color.White);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000F769 File Offset: 0x0000D969
		public void SetPortTex(int x, int y)
		{
			this.PortaitSrc = Global.GetTexRectange(x, y);
		}

		// Token: 0x040001CD RID: 461
		private static Texture2D BGBig;

		// Token: 0x040001CE RID: 462
		private static Texture2D BGFlipped;

		// Token: 0x040001CF RID: 463
		private Vector2 Position;

		// Token: 0x040001D0 RID: 464
		private Rectangle PortaitSrc;

		// Token: 0x040001D1 RID: 465
		private Rectangle PortriatDest;

		// Token: 0x040001D2 RID: 466
		private Texture2D BG;

		// Token: 0x040001D3 RID: 467
		private Vector2 BarPosition;

		// Token: 0x040001D4 RID: 468
		private Bar Bar;

		// Token: 0x040001D5 RID: 469
		private Color MyColor;
	}
}
