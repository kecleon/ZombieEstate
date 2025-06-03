using System;
using System.Collections.Generic;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001E8 RID: 488
	internal class Buff_AllResAura : PlayerAura
	{
		// Token: 0x06000D0F RID: 3343 RVA: 0x00069C80 File Offset: 0x00067E80
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 0f;
			this.Src = Global.GetTexRectange(70, 51);
			this.Positive = true;
			this.Name = "Resistance Aura";
			this.Buff = "Buff_AllRes";
			this.Args = "";
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00069CD7 File Offset: 0x00067ED7
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.FireResist += 10;
			spec.WaterResist += 10;
			spec.EarthResist += 10;
			return true;
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x00069D0A File Offset: 0x00067F0A
		public override string Description
		{
			get
			{
				return string.Format("Target and nearby allies gain 10 resistance to all elements.", new object[0]);
			}
		}
	}
}
