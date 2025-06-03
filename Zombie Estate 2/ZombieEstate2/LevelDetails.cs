using System;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000E2 RID: 226
	internal struct LevelDetails
	{
		// Token: 0x040005D0 RID: 1488
		public string levelName;

		// Token: 0x040005D1 RID: 1489
		public string levelDesc;

		// Token: 0x040005D2 RID: 1490
		public Texture2D texture;

		// Token: 0x040005D3 RID: 1491
		public MenuItem.SelectedDelegate SelectedFunction;
	}
}
