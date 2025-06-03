using System;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000D3 RID: 211
	internal interface ShopScreen
	{
		// Token: 0x06000571 RID: 1393
		void Update();

		// Token: 0x06000572 RID: 1394
		void Draw(SpriteBatch spriteBatch);

		// Token: 0x06000573 RID: 1395
		void ReInit();

		// Token: 0x06000574 RID: 1396
		bool MessageShown();
	}
}
