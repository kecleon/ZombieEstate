using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001F2 RID: 498
	internal class Buff_HealingTotemAura : PlayerAura
	{
		// Token: 0x06000D4E RID: 3406 RVA: 0x0006A5C8 File Offset: 0x000687C8
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 0f;
			this.Src = Global.GetTexRectange(16, 43);
			this.Positive = true;
			this.Name = "Healing Totem ";
			this.Buff = "Buff_HealingTotem";
			this.Args = this.amount.ToString();
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0006A625 File Offset: 0x00068825
		public override void Destroyed()
		{
			base.Destroyed();
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0006A62D File Offset: 0x0006882D
		public override void Removed()
		{
			base.Removed();
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0006A638 File Offset: 0x00068838
		public override void Arguments(List<string> args)
		{
			this.amount = int.Parse(args[0], CultureInfo.InvariantCulture);
			this.Range = float.Parse(args[1], CultureInfo.InvariantCulture);
			this.Args = this.amount.ToString();
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0006A684 File Offset: 0x00068884
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.HealOverTime += (float)this.amount;
			return true;
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000D53 RID: 3411 RVA: 0x0006A69C File Offset: 0x0006889C
		public override string Description
		{
			get
			{
				return string.Format("Target and nearby allies gain {0} health per 1 seconds.", this.amount);
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0006A6B4 File Offset: 0x000688B4
		public override void FireAuraEffect()
		{
			RadialParticle radialParticle = new RadialParticle(new Point(76, 62), 0.1f, this.Range, 0.1f, new Vector3(this.Parent.Position.X, 0.025f, this.Parent.Position.Z));
			radialParticle.TexScale = 2f;
			Global.MasterCache.CreateObject(radialParticle);
			base.FireAuraEffect();
		}

		// Token: 0x04000DAC RID: 3500
		private int amount = 10;
	}
}
