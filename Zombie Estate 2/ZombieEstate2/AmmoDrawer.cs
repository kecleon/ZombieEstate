using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000108 RID: 264
	public class AmmoDrawer
	{
		// Token: 0x06000726 RID: 1830 RVA: 0x00036C22 File Offset: 0x00034E22
		public AmmoDrawer(Vector2 pos)
		{
			this.Position = pos;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void Update(Gun gun)
		{
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void Draw(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x04000720 RID: 1824
		private Rectangle OnRect;

		// Token: 0x04000721 RID: 1825
		private Rectangle OffRect;

		// Token: 0x04000722 RID: 1826
		private Vector2 Position;

		// Token: 0x04000723 RID: 1827
		private int ammoWidth;

		// Token: 0x04000724 RID: 1828
		private int bulletsInClip;

		// Token: 0x04000725 RID: 1829
		private int clipSize;
	}
}
