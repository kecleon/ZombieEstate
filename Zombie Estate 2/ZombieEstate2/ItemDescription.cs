using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000BF RID: 191
	internal class ItemDescription : StoreElement
	{
		// Token: 0x060004B7 RID: 1207 RVA: 0x00022F61 File Offset: 0x00021161
		public ItemDescription(PCStore store) : this(store, 920, 155)
		{
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00022F74 File Offset: 0x00021174
		public ItemDescription(PCStore store, int x, int y)
		{
			this.startX = x;
			this.startY = y;
			this.offX = store.StoreOffsetX;
			this.offY = store.StoreOffsetY;
			this.ScrollBoxRect = new Rectangle(x + store.StoreOffsetX, y + store.StoreOffsetY, 360, 128);
			this.ScrollBox = new ScrollBox("", this.ScrollBoxRect, Global.StoreFont, store.Player, store.Player.HUDColor);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00022FFE File Offset: 0x000211FE
		public override void Update()
		{
			this.ScrollBox.Update();
			base.Update();
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00023011 File Offset: 0x00021211
		public override void Draw(SpriteBatch spriteBatch)
		{
			this.ScrollBox.Draw(spriteBatch);
			if (this.title != null)
			{
				Shadow.DrawString(this.title, Global.StoreFontBig, this.titlePos, 2, Color.White, spriteBatch);
			}
			base.Draw(spriteBatch);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0002304B File Offset: 0x0002124B
		public override void ItemSelected(PCItem item)
		{
			this.ScrollBox.SetText(item.Description);
			this.SetTitle(item.Title);
			base.ItemSelected(item);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00023074 File Offset: 0x00021274
		private void SetTitle(string text)
		{
			this.titlePos = VerchickMath.CenterText(Global.StoreFontBig, new Vector2((float)(138 + this.startX + this.offX), (float)(this.startY - 18 + this.offY)), text);
			this.title = text;
		}

		// Token: 0x040004D9 RID: 1241
		private Rectangle ScrollBoxRect;

		// Token: 0x040004DA RID: 1242
		private ScrollBox ScrollBox;

		// Token: 0x040004DB RID: 1243
		private string title;

		// Token: 0x040004DC RID: 1244
		private Vector2 titlePos;

		// Token: 0x040004DD RID: 1245
		private int offX;

		// Token: 0x040004DE RID: 1246
		private int offY;

		// Token: 0x040004DF RID: 1247
		private int startX;

		// Token: 0x040004E0 RID: 1248
		private int startY;
	}
}
