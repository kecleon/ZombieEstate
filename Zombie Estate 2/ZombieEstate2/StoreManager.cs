using System;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000D6 RID: 214
	public class StoreManager
	{
		// Token: 0x06000593 RID: 1427 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void Update()
		{
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0002A904 File Offset: 0x00028B04
		public void DrawStores(SpriteBatch spriteBatch)
		{
			bool active = this.Active;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0002A90D File Offset: 0x00028B0D
		public void ToggleStore()
		{
			this.Active = !this.Active;
		}

		// Token: 0x0400058E RID: 1422
		public bool Active;
	}
}
