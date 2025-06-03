using System;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000D1 RID: 209
	internal class StoreElement
	{
		// Token: 0x06000568 RID: 1384 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Update()
		{
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Draw(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Destroy()
		{
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void ItemSelected(PCItem item)
		{
		}
	}
}
