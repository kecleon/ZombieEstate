using System;
using System.Collections.Generic;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001EC RID: 492
	internal class Buff_CritAura : PlayerAura
	{
		// Token: 0x06000D27 RID: 3367 RVA: 0x0006A064 File Offset: 0x00068264
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 0f;
			this.Src = Global.GetTexRectange(72, 52);
			this.Positive = true;
			this.Name = "Crit Aura";
			this.Buff = "Buff_Crit";
			this.Args = "";
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0006A0BB File Offset: 0x000682BB
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.CritChance += 5f;
			return true;
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0006A0D1 File Offset: 0x000682D1
		public override string Description
		{
			get
			{
				return string.Format("Target and nearby allies gain 5% crit chance.", new object[0]);
			}
		}
	}
}
