using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000C0 RID: 192
	internal class ItemPortrait : StoreElement
	{
		// Token: 0x060004BD RID: 1213 RVA: 0x000230C3 File Offset: 0x000212C3
		public ItemPortrait(int xPos, int yPos, int size)
		{
			this.itemDest = new Rectangle(xPos, yPos, size, size);
			this.bgDest = new Rectangle(xPos - 1, yPos - 1, size + 1, size + 1);
			this.itemRect = Global.GetTexRectange(63, 63);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00023100 File Offset: 0x00021300
		public ItemPortrait(int xPos, int yPos, int size, bool upgrade) : this(xPos, yPos, size)
		{
			this.Upgrade = upgrade;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00023114 File Offset: 0x00021314
		public override void ItemSelected(PCItem item)
		{
			if (this.Upgrade)
			{
				GunStats gunStats = item.Tag as GunStats;
				if (gunStats != null && gunStats.AmmoType == AmmoType.SPECIAL)
				{
					this.itemRect = Global.GetTexRectange(63, 63);
				}
				this.itemRect.X = item.ItemNextLevelTex.X;
				this.itemRect.Y = item.ItemNextLevelTex.Y;
			}
			else
			{
				this.itemRect.X = item.ItemTex.X;
				this.itemRect.Y = item.ItemTex.Y;
			}
			base.ItemSelected(item);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000231B1 File Offset: 0x000213B1
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(PCItem.Background, this.bgDest, Color.White);
			spriteBatch.Draw(Global.MasterTexture, this.itemDest, new Rectangle?(this.itemRect), Color.White);
			base.Draw(spriteBatch);
		}

		// Token: 0x040004E1 RID: 1249
		private Rectangle itemRect;

		// Token: 0x040004E2 RID: 1250
		private Rectangle itemDest;

		// Token: 0x040004E3 RID: 1251
		private Rectangle bgDest;

		// Token: 0x040004E4 RID: 1252
		private bool Upgrade;
	}
}
