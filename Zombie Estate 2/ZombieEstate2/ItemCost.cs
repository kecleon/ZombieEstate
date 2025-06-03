using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000BA RID: 186
	internal class ItemCost : StoreElement
	{
		// Token: 0x060004A6 RID: 1190 RVA: 0x00022686 File Offset: 0x00020886
		public ItemCost(int x, int y)
		{
			this.origin = new Vector2((float)x, (float)y);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x000226A8 File Offset: 0x000208A8
		public ItemCost(int x, int y, bool OnlyPoints)
		{
			this.origin = new Vector2((float)x, (float)y);
			this.OnlyPoints = OnlyPoints;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000226D4 File Offset: 0x000208D4
		public override void ItemSelected(PCItem item)
		{
			if (item.Locked)
			{
				this.amount = "Points: " + item.PointsToUnlock.ToString();
				this.color = Color.LightBlue;
				this.haveItem = true;
				base.ItemSelected(item);
				return;
			}
			if (item.Cost == 0)
			{
				this.amount = "";
				this.haveItem = true;
				base.ItemSelected(item);
				return;
			}
			this.amount = "Cost:\n$" + item.Cost.ToString();
			this.color = Color.LightGreen;
			this.haveItem = true;
			base.ItemSelected(item);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00022774 File Offset: 0x00020974
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (this.haveItem)
			{
				Shadow.DrawString(this.amount, Global.StoreFontBig, this.origin, 1, this.color, spriteBatch);
			}
			base.Draw(spriteBatch);
		}

		// Token: 0x040004B1 RID: 1201
		private Vector2 origin;

		// Token: 0x040004B2 RID: 1202
		private string amount;

		// Token: 0x040004B3 RID: 1203
		private bool haveItem;

		// Token: 0x040004B4 RID: 1204
		private Color color = Color.LightGreen;

		// Token: 0x040004B5 RID: 1205
		private bool OnlyPoints;
	}
}
