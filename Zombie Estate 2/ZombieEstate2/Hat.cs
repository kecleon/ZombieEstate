using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x020000C4 RID: 196
	public struct Hat
	{
		// Token: 0x04000504 RID: 1284
		public Point Tex;

		// Token: 0x04000505 RID: 1285
		public int ZHCost;

		// Token: 0x04000506 RID: 1286
		public bool Unlocked;

		// Token: 0x04000507 RID: 1287
		public string Name;

		// Token: 0x04000508 RID: 1288
		public SpecialProperties Properties;

		// Token: 0x04000509 RID: 1289
		public List<string> PropDesc;
	}
}
