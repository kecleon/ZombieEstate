using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000053 RID: 83
	internal class AmmoMeter
	{
		// Token: 0x060001EB RID: 491 RVA: 0x0000DD38 File Offset: 0x0000BF38
		public AmmoMeter(Point pos, Color color, Point tex)
		{
			if (AmmoMeter.AmmoEmpty == null)
			{
				AmmoMeter.AmmoEmpty = Global.Content.Load<Texture2D>("AmmoEmpty");
			}
			if (AmmoMeter.AmmoFull == null)
			{
				AmmoMeter.AmmoFull = Global.Content.Load<Texture2D>("AmmoFull");
			}
			this.Origin = pos;
			this.Position = new Rectangle(pos.X, pos.Y, 20, 20);
			this.TexSource = new Rectangle(tex.X * 16, tex.Y * 16, 16, 16);
			this.ActualPosition = new Rectangle(this.Position.X, this.Position.Y, this.Position.Width, this.Position.Height);
			this.hudColor = color;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000DE10 File Offset: 0x0000C010
		public void DrawMeter(SpriteBatch spriteBatch, int actual, int max, bool selected)
		{
			Rectangle destinationRectangle = new Rectangle(this.ActualPosition.X - 20, this.ActualPosition.Y, this.ActualPosition.Width * 2, this.ActualPosition.Height * 2);
			spriteBatch.Draw(AmmoMeter.AmmoEmpty, destinationRectangle, this.hudColor);
			if (max == 0)
			{
				Terminal.WriteMessage("ERROR: Max was 0. Can't divide by zero! [AmmoMeter]", MessageType.ERROR);
				return;
			}
			float num = (float)actual / (float)max;
			this.Position.X = this.ActualPosition.X - 20;
			this.Position.Y = this.ActualPosition.Y;
			this.Position.Width = (int)(num * (float)(this.ActualPosition.Width * 2));
			this.Position.Height = 40;
			int width = (int)(20f * num);
			Rectangle value = new Rectangle(0, 0, width, 20);
			spriteBatch.Draw(AmmoMeter.AmmoFull, this.Position, new Rectangle?(value), this.ModColor);
			Rectangle destinationRectangle2 = new Rectangle(this.ActualPosition.X - 20 - 32, this.ActualPosition.Y + 2, 32, 32);
			Color color = new Color(1f, 1f, 1f, 1f);
			if (selected)
			{
				this.TexSource.X = this.TexSource.X + 48;
			}
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle2, new Rectangle?(this.TexSource), color);
			if (selected)
			{
				this.TexSource.X = this.TexSource.X - 48;
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000DF90 File Offset: 0x0000C190
		public void DrawMeterWithText(SpriteBatch spriteBatch, int actual, int max, bool selected)
		{
			Rectangle destinationRectangle = new Rectangle(this.ActualPosition.X, this.ActualPosition.Y, this.ActualPosition.Width * 2, this.ActualPosition.Height * 2);
			spriteBatch.Draw(AmmoMeter.AmmoEmpty, destinationRectangle, this.hudColor);
			if (max == 0)
			{
				Terminal.WriteMessage("ERROR: Max was 0. Can't divide by zero! [AmmoMeter]", MessageType.ERROR);
				return;
			}
			float num = (float)actual / (float)max;
			Rectangle actualPosition = this.ActualPosition;
			Rectangle actualPosition2 = this.ActualPosition;
			Rectangle actualPosition3 = this.ActualPosition;
			int num2 = (int)(num * (float)this.ActualPosition.Height);
			num2 = this.ActualPosition.Y - num2 * 2 + this.ActualPosition.Height * 2;
			this.Position.X = this.ActualPosition.X;
			this.Position.Y = num2;
			this.Position.Height = (int)(num * (float)this.ActualPosition.Height) * 2;
			this.Position.Width = 40;
			int num3 = (int)(20f * num);
			Rectangle value = new Rectangle(0, 20 - num3, 20, num3);
			spriteBatch.Draw(AmmoMeter.AmmoFull, this.Position, new Rectangle?(value), Color.White);
			Rectangle destinationRectangle2 = new Rectangle(this.ActualPosition.X + 3, this.ActualPosition.Y + 2, 32, 32);
			Color color = new Color(1f, 1f, 1f, 0.8f);
			if (selected)
			{
				this.TexSource.X = this.TexSource.X + 48;
			}
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle2, new Rectangle?(this.TexSource), color);
			if (selected)
			{
				this.TexSource.X = this.TexSource.X - 48;
			}
			string text = actual + "/" + max;
			Vector2 pos = VerchickMath.CenterText(Global.StoreFont, new Vector2((float)(this.Origin.X + 80), (float)(this.Origin.Y + 18)), text);
			Shadow.DrawString(text, Global.StoreFont, pos, 1, AmmoMeter.AmmoColor, spriteBatch);
		}

		// Token: 0x04000168 RID: 360
		public static Texture2D AmmoFull;

		// Token: 0x04000169 RID: 361
		public static Texture2D AmmoEmpty;

		// Token: 0x0400016A RID: 362
		private Rectangle Position;

		// Token: 0x0400016B RID: 363
		private Rectangle TexSource;

		// Token: 0x0400016C RID: 364
		private Rectangle ActualPosition;

		// Token: 0x0400016D RID: 365
		private Color hudColor;

		// Token: 0x0400016E RID: 366
		private Point Origin;

		// Token: 0x0400016F RID: 367
		public static Color AmmoColor = new Color(203, 198, 133);

		// Token: 0x04000170 RID: 368
		public Color ModColor = Color.White;

		// Token: 0x04000171 RID: 369
		public ZombieType Zombie;
	}
}
