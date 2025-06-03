using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000066 RID: 102
	public class WeaponSelector
	{
		// Token: 0x0600024A RID: 586 RVA: 0x00012185 File Offset: 0x00010385
		public WeaponSelector(Player player)
		{
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000121A4 File Offset: 0x000103A4
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.parent.mGun == null)
			{
				return;
			}
			this.CurrentIndex = this.Guns.IndexOf(this.parent.mGun);
			int currentIndex = this.CurrentIndex;
			Rectangle destinationRectangle = new Rectangle((int)this.Position.X + 23, (int)this.Position.Y + 46, 32, 32);
			Rectangle value = new Rectangle(this.Guns[currentIndex].OriginTex.X * 16, this.Guns[currentIndex].OriginTex.Y * 16 + 16, 16, 16);
			Color color = new Color(255, 255, 255, 255);
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(value), color);
		}

		// Token: 0x0400024A RID: 586
		private Player parent;

		// Token: 0x0400024B RID: 587
		private Vector2 Position = new Vector2(320f, 64f);

		// Token: 0x0400024C RID: 588
		private List<Gun> Guns;

		// Token: 0x0400024D RID: 589
		private int CurrentIndex;
	}
}
