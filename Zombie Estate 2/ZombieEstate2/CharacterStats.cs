using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ZombieEstate2
{
	// Token: 0x02000092 RID: 146
	public struct CharacterStats
	{
		// Token: 0x0400038E RID: 910
		[XmlIgnore]
		public int[] Ammo;

		// Token: 0x0400038F RID: 911
		[XmlIgnore]
		public int[] MaxAmmo;

		// Token: 0x04000390 RID: 912
		public int totalZombiesKilled;

		// Token: 0x04000391 RID: 913
		[XmlIgnore]
		public List<Talent> Talents;

		// Token: 0x04000392 RID: 914
		public string CharacterName;

		// Token: 0x04000393 RID: 915
		public string GamerName;

		// Token: 0x04000394 RID: 916
		public List<int> AmmoInt;

		// Token: 0x04000395 RID: 917
		public List<int> MaxAmmoInt;

		// Token: 0x04000396 RID: 918
		public int TalentPoints;

		// Token: 0x04000397 RID: 919
		public int UpgradeTokens;

		// Token: 0x04000398 RID: 920
		public string EquippedHat;

		// Token: 0x04000399 RID: 921
		public int HealthPacks;

		// Token: 0x0400039A RID: 922
		public float Money;

		// Token: 0x0400039B RID: 923
		public List<GunSaveStats> Guns;
	}
}
