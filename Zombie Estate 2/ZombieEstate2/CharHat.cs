using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000B5 RID: 181
	internal class CharHat : StoreElement
	{
		// Token: 0x06000499 RID: 1177 RVA: 0x00021EF8 File Offset: 0x000200F8
		public CharHat(int xPos, int yPos, int size, Point hatCoord)
		{
			this.itemDest = new Rectangle(xPos, yPos + 12, size, size);
			this.bgDest = new Rectangle(xPos - 1, yPos - 1 + 12, size + 1, size + 1);
			this.SetHat(hatCoord);
			this.border = new Rectangle(xPos - 6, yPos - 6, 212, size + 32);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00021F60 File Offset: 0x00020160
		public void SetHat(Point hatCoord)
		{
			this.itemRect = Global.GetTexRectange(hatCoord.X, hatCoord.Y);
			if (hatCoord.X == 63 && hatCoord.Y == 63)
			{
				this.none = true;
				return;
			}
			this.none = false;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00021F9C File Offset: 0x0002019C
		public override void ItemSelected(PCItem item)
		{
			base.ItemSelected(item);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00021FA8 File Offset: 0x000201A8
		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2((float)(this.bgDest.X + 80), (float)(this.bgDest.Y - 14));
			spriteBatch.Draw(Global.Pixel, this.border, new Color(40, 40, 40));
			Shadow.DrawString("Equipped Hat", Global.StoreFont, vector, 1, Color.White, spriteBatch);
			spriteBatch.Draw(PCItem.Background, this.bgDest, Color.White);
			spriteBatch.Draw(Global.MasterTexture, this.itemDest, new Rectangle?(this.itemRect), Color.White);
			if (this.none)
			{
				vector = new Vector2((float)(this.bgDest.X + 32), (float)(this.bgDest.Y + 32));
				vector = VerchickMath.CenterText(Global.StoreFont, vector, "None");
				Shadow.DrawString("None", Global.StoreFont, vector, 1, Color.White, spriteBatch);
			}
			base.Draw(spriteBatch);
		}

		// Token: 0x04000492 RID: 1170
		private Rectangle itemRect;

		// Token: 0x04000493 RID: 1171
		private Rectangle itemDest;

		// Token: 0x04000494 RID: 1172
		private Rectangle bgDest;

		// Token: 0x04000495 RID: 1173
		private Rectangle border;

		// Token: 0x04000496 RID: 1174
		private bool none = true;
	}
}
