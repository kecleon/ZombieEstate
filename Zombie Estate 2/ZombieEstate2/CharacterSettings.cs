using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000093 RID: 147
	public struct CharacterSettings
	{
		// Token: 0x0400039C RID: 924
		public string name;

		// Token: 0x0400039D RID: 925
		public Vector2 hatOffset;

		// Token: 0x0400039E RID: 926
		public Point texCoord;

		// Token: 0x0400039F RID: 927
		public string description;

		// Token: 0x040003A0 RID: 928
		public string startingGun;

		// Token: 0x040003A1 RID: 929
		public string superWeapon;

		// Token: 0x040003A2 RID: 930
		public List<Talent> Talents;

		// Token: 0x040003A3 RID: 931
		public SpecialProperties Properties;

		// Token: 0x040003A4 RID: 932
		public int PointsToUnlock;
	}
}
