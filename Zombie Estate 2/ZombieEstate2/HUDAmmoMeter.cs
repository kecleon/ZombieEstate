using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200005E RID: 94
	internal class HUDAmmoMeter
	{
		// Token: 0x0600021C RID: 540 RVA: 0x0000F778 File Offset: 0x0000D978
		public HUDAmmoMeter(Vector2 pos, Color color, int iconX, int iconY)
		{
			if (HUDAmmoMeter.BGTex == null)
			{
				HUDAmmoMeter.BGTex = Global.Content.Load<Texture2D>("HUD\\NewHud\\AmmoMeterBG");
				HUDAmmoMeter.BarTex = Global.Content.Load<Texture2D>("HUD\\NewHud\\AmmoMeterBar");
				HUDAmmoMeter.BGTexSel = Global.Content.Load<Texture2D>("HUD\\NewHud\\AmmoMeterBG_Sel");
			}
			this.hudColor = color;
			this.Position = pos;
			this.IconSource = Global.GetTexRectange(iconX, iconY);
			this.IconDest = new Rectangle((int)this.Position.X + 4, (int)this.Position.Y + HUDAmmoMeter.BGTex.Height + 2, 32, 32);
			this.BarFull = new Rectangle((int)this.Position.X, (int)this.Position.Y + 12, HUDAmmoMeter.BarTex.Width, HUDAmmoMeter.BarTex.Height);
			this.BarSrc = new Rectangle(0, 0, HUDAmmoMeter.BarTex.Width, HUDAmmoMeter.BarTex.Height);
			this.BarRect = new Rectangle((int)this.Position.X, (int)this.Position.Y + 12, HUDAmmoMeter.BarTex.Width, HUDAmmoMeter.BarTex.Height);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000F8C0 File Offset: 0x0000DAC0
		public void Draw(SpriteBatch spriteBatch, int max, int actual, bool selected)
		{
			float num = (float)actual / (float)max;
			this.BarRect.Y = this.BarFull.Y + (int)((1f - num) * (float)this.BarFull.Height);
			this.BarRect.Height = (int)((float)this.BarFull.Height * num);
			this.BarSrc.Y = (int)((1f - num) * (float)this.BarFull.Height);
			this.BarSrc.Height = (int)((float)this.BarFull.Height * num);
			if (selected)
			{
				spriteBatch.Draw(HUDAmmoMeter.BGTexSel, this.Position, this.hudColor);
			}
			else
			{
				spriteBatch.Draw(HUDAmmoMeter.BGTex, this.Position, this.hudColor);
			}
			spriteBatch.Draw(HUDAmmoMeter.BarTex, this.BarFull, new Color(0.1f, 0.1f, 0.1f));
			spriteBatch.Draw(HUDAmmoMeter.BarTex, this.BarRect, new Rectangle?(this.BarSrc), AmmoMeter.AmmoColor);
			if (selected)
			{
				this.IconSource.X = this.IconSource.X + 48;
			}
			spriteBatch.Draw(Global.MasterTexture, this.IconDest, new Rectangle?(this.IconSource), Color.White);
			if (actual == 0)
			{
				spriteBatch.Draw(Global.MasterTexture, this.IconDest, new Rectangle?(Global.RedX), Color.White * 0.5f);
			}
			if (selected)
			{
				this.IconSource.X = this.IconSource.X - 48;
			}
		}

		// Token: 0x040001D6 RID: 470
		private static Texture2D BGTex;

		// Token: 0x040001D7 RID: 471
		private static Texture2D BarTex;

		// Token: 0x040001D8 RID: 472
		private static Texture2D BGTexSel;

		// Token: 0x040001D9 RID: 473
		private Vector2 Position;

		// Token: 0x040001DA RID: 474
		private Rectangle IconSource;

		// Token: 0x040001DB RID: 475
		private Rectangle IconDest;

		// Token: 0x040001DC RID: 476
		private Color hudColor;

		// Token: 0x040001DD RID: 477
		private Rectangle BarFull;

		// Token: 0x040001DE RID: 478
		private Rectangle BarRect;

		// Token: 0x040001DF RID: 479
		private Rectangle BarSrc;

		// Token: 0x040001E0 RID: 480
		private Point Origin;

		// Token: 0x040001E1 RID: 481
		public Color ModColor = Color.White;
	}
}
