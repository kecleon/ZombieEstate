using System;

namespace ZombieEstate2
{
	// Token: 0x0200011F RID: 287
	public class WaveTalleys
	{
		// Token: 0x06000819 RID: 2073 RVA: 0x000437AE File Offset: 0x000419AE
		public void Clear()
		{
			this.DamageDealt = 0f;
			this.DamageTaken = 0f;
			this.MinionDamageDealt = 0f;
			this.HealingDone = 0f;
		}

		// Token: 0x040008CB RID: 2251
		public float DamageDealt;

		// Token: 0x040008CC RID: 2252
		public float DamageTaken;

		// Token: 0x040008CD RID: 2253
		public float MinionDamageDealt;

		// Token: 0x040008CE RID: 2254
		public float HealingDone;
	}
}
