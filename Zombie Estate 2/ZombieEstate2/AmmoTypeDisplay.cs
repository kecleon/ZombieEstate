using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000B3 RID: 179
	internal class AmmoTypeDisplay : StoreElement
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x00021A88 File Offset: 0x0001FC88
		public AmmoTypeDisplay(Vector2 centerPos)
		{
			this.dest = new Rectangle((int)centerPos.X - 32, (int)centerPos.Y - 32, 64, 64);
			this.textCenterPos = new Vector2(centerPos.X, centerPos.Y + 64f);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00021ADB File Offset: 0x0001FCDB
		public override void Update()
		{
			base.Update();
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00021AE4 File Offset: 0x0001FCE4
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (!this.somethingSelected)
			{
				return;
			}
			Shadow.DrawString(this.text, Global.StoreFontBig, this.textPos, 1, AmmoTypeDisplay.ammoColor, spriteBatch);
			spriteBatch.Draw(Global.MasterTexture, this.dest, new Rectangle?(this.src), Color.White);
			base.Draw(spriteBatch);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00021B40 File Offset: 0x0001FD40
		public override void ItemSelected(PCItem item)
		{
			this.somethingSelected = true;
			GunStats gunStats = item.Tag as GunStats;
			if (gunStats == null)
			{
				this.somethingSelected = false;
				return;
			}
			this.text = gunStats.AmmoType.ToString();
			this.textPos = VerchickMath.CenterText(Global.StoreFontBig, this.textCenterPos, this.text);
			this.src = PCItem.GetAmmoRect(gunStats.AmmoType);
			base.ItemSelected(item);
		}

		// Token: 0x0400047E RID: 1150
		private Rectangle src;

		// Token: 0x0400047F RID: 1151
		private Rectangle dest;

		// Token: 0x04000480 RID: 1152
		private string text;

		// Token: 0x04000481 RID: 1153
		private Vector2 textCenterPos;

		// Token: 0x04000482 RID: 1154
		private Vector2 textPos;

		// Token: 0x04000483 RID: 1155
		private static Color ammoColor = new Color(203, 198, 133);

		// Token: 0x04000484 RID: 1156
		private bool somethingSelected;
	}
}
