using System;
using System.Collections.Generic;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001E6 RID: 486
	internal class Buff_MoveQuickAura : PlayerAura
	{
		// Token: 0x06000D04 RID: 3332 RVA: 0x00069B4C File Offset: 0x00067D4C
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 0f;
			this.Src = Global.GetTexRectange(68, 51);
			this.Positive = true;
			this.Name = "Move Quick Aura";
			this.Buff = "Buff_MoveQuick";
			this.Args = "";
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00069BA3 File Offset: 0x00067DA3
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.Speed += 0.1f;
			return true;
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00069BB9 File Offset: 0x00067DB9
		public override string Description
		{
			get
			{
				return string.Format("Target and nearby allies gain 0.1 movement speed.", new object[0]);
			}
		}
	}
}
