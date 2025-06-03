using System;
using System.Collections.Generic;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001F9 RID: 505
	internal class Buff_GuardAura : PlayerAura
	{
		// Token: 0x06000D7C RID: 3452 RVA: 0x0006AFA8 File Offset: 0x000691A8
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 0f;
			this.Src = Global.GetTexRectange(66, 50);
			this.Positive = true;
			this.Name = "Guard Aura";
			this.Buff = "Buff_Guard";
			this.Args = "";
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0006AFFF File Offset: 0x000691FF
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.Armor += this.armor;
			return true;
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0006B016 File Offset: 0x00069216
		public override string Description
		{
			get
			{
				return string.Format("Target and nearby allies gain {0} armor.", this.armor);
			}
		}

		// Token: 0x04000DBD RID: 3517
		private int armor = 10;
	}
}
