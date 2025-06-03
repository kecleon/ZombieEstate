using System;
using System.Collections.Generic;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001E9 RID: 489
	internal class Buff_AllRes : Buff
	{
		// Token: 0x06000D14 RID: 3348 RVA: 0x00069D1C File Offset: 0x00067F1C
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 1f;
			this.Src = Global.GetTexRectange(71, 50);
			this.Positive = true;
			this.Name = "Resistance";
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00069B1E File Offset: 0x00067D1E
		public override void Update(float elapsed)
		{
			base.Update(elapsed);
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00069CD7 File Offset: 0x00067ED7
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.FireResist += 10;
			spec.WaterResist += 10;
			spec.EarthResist += 10;
			return true;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00069B27 File Offset: 0x00067D27
		public override void Tick()
		{
			base.Tick();
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x00069D52 File Offset: 0x00067F52
		public override string Description
		{
			get
			{
				return string.Format("Target gains 10 resistance to all elements.", new object[0]);
			}
		}
	}
}
