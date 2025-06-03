using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000EF RID: 239
	internal class SmallDataString
	{
		// Token: 0x06000657 RID: 1623 RVA: 0x0002F283 File Offset: 0x0002D483
		public SmallDataString(int iconX, int iconY, string val, string type, Color valC, Vector2 pos) : this(val, type, valC, pos)
		{
			this.IconTex = Global.GetTexRectange(iconX, iconY);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0002F29F File Offset: 0x0002D49F
		public SmallDataString(string val, string type, Color valC, Vector2 pos)
		{
			this.Value = val;
			this.Type = type;
			this.ValColor = valC;
			this.Position = pos;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0002F2C4 File Offset: 0x0002D4C4
		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2(this.Position.X, this.Position.Y);
			Rectangle iconTex = this.IconTex;
			Rectangle destinationRectangle = new Rectangle((int)vector.X, (int)vector.Y, 16, 16);
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(this.IconTex), Color.White);
			vector.X += 16f;
			int num = (int)Global.StoreFont.MeasureString(string.Format("{0}: ", this.Type)).X;
			Shadow.DrawString(string.Format("{0}: ", this.Type), Global.StoreFont, vector, 1, Color.White, spriteBatch);
			vector.X += (float)num;
			Shadow.DrawString(this.Value, Global.StoreFont, vector, 1, this.ValColor, spriteBatch);
		}

		// Token: 0x04000627 RID: 1575
		private string Value;

		// Token: 0x04000628 RID: 1576
		private string Type;

		// Token: 0x04000629 RID: 1577
		private Rectangle IconTex;

		// Token: 0x0400062A RID: 1578
		private Color ValColor;

		// Token: 0x0400062B RID: 1579
		private Vector2 Position;
	}
}
