using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001EF RID: 495
	internal class Buff_ArmorTotemAura : PlayerAura
	{
		// Token: 0x06000D39 RID: 3385 RVA: 0x0006A2D0 File Offset: 0x000684D0
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 0f;
			this.Src = Global.GetTexRectange(16, 39);
			this.Positive = true;
			this.Name = "Armor Totem ";
			this.Buff = "Buff_ArmorTotem";
			this.Args = "";
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0006A328 File Offset: 0x00068528
		public override void Arguments(List<string> args)
		{
			this.armor = int.Parse(args[0], CultureInfo.InvariantCulture);
			this.Range = float.Parse(args[1], CultureInfo.InvariantCulture);
			this.Args = this.armor.ToString();
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0006A374 File Offset: 0x00068574
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.Armor += this.armor;
			return true;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x0006A38B File Offset: 0x0006858B
		public override string Description
		{
			get
			{
				return string.Format("Target and nearby allies gain {0} armor.", this.armor);
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0006A3A4 File Offset: 0x000685A4
		public override void FireAuraEffect()
		{
			RadialParticle radialParticle = new RadialParticle(new Point(75, 62), 0.1f, this.Range, 0.1f, new Vector3(this.Parent.Position.X, 0.025f, this.Parent.Position.Z));
			radialParticle.TexScale = 2f;
			Global.MasterCache.CreateObject(radialParticle);
			base.FireAuraEffect();
		}

		// Token: 0x04000DA9 RID: 3497
		private int armor = 10;
	}
}
