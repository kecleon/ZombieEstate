using System;
using System.Collections.Generic;
using System.Globalization;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001F1 RID: 497
	internal class Buff_HealingTotem : Buff
	{
		// Token: 0x06000D46 RID: 3398 RVA: 0x0006A4B4 File Offset: 0x000686B4
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 1f;
			this.Src = Global.GetTexRectange(16, 43);
			this.Positive = true;
			this.Name = "Healing Totem";
			if (attacker is Minion && (attacker as Minion).parent != null)
			{
				parent.HEALER_OVER_TIME = attacker;
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0006A514 File Offset: 0x00068714
		public override void Arguments(List<string> args)
		{
			this.amount = int.Parse(args[0], CultureInfo.InvariantCulture);
			if (this.Attacker != null)
			{
				this.amount = (int)((float)this.amount * (this.Attacker.SpecialProperties.HealingDoneMod / 100f + 1f));
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00069B1E File Offset: 0x00067D1E
		public override void Update(float elapsed)
		{
			base.Update(elapsed);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0006A56B File Offset: 0x0006876B
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.HealOverTime += (float)this.amount;
			return true;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00069B27 File Offset: 0x00067D27
		public override void Tick()
		{
			base.Tick();
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0006A583 File Offset: 0x00068783
		public override void Destroyed()
		{
			base.Destroyed();
			if (this.Parent != null)
			{
				this.Parent.HEALER_OVER_TIME = null;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x0006A59F File Offset: 0x0006879F
		public override string Description
		{
			get
			{
				return string.Format("Target gains {0} health per 3 seconds.", this.amount);
			}
		}

		// Token: 0x04000DAB RID: 3499
		private int amount = 10;
	}
}
