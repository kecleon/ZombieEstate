using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001EA RID: 490
	internal class Buff_DmgAura : PlayerAura
	{
		// Token: 0x06000D1B RID: 3355 RVA: 0x00069D64 File Offset: 0x00067F64
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 0f;
			this.Src = Global.GetTexRectange(69, 51);
			this.Positive = true;
			this.Name = "Damage Aura ";
			this.Buff = "Buff_DmgBoost";
			this.Args = "";
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00069DBC File Offset: 0x00067FBC
		public override void Arguments(List<string> args)
		{
			this.dmg = float.Parse(args[0], CultureInfo.InvariantCulture);
			this.Range = float.Parse(args[1], CultureInfo.InvariantCulture);
			this.Args = this.dmg.ToString();
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00069E08 File Offset: 0x00068008
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.BulletDamageMod += this.dmg;
			spec.FireDmg += this.dmg;
			spec.EarthDmg += this.dmg;
			spec.WaterDmg += this.dmg;
			spec.ExplosionDamageMod += this.dmg / 2f;
			spec.MeleeDamageMod += this.dmg;
			return true;
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x00069E94 File Offset: 0x00068094
		public override string Description
		{
			get
			{
				return string.Format("", new object[0]);
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00069EA8 File Offset: 0x000680A8
		public override void FireAuraEffect()
		{
			RadialParticle radialParticle = new RadialParticle(new Point(74, 60), 0.1f, this.Range, 0.1f, new Vector3(this.Parent.Position.X, 0.025f, this.Parent.Position.Z));
			radialParticle.TexScale = 2f;
			Global.MasterCache.CreateObject(radialParticle);
			base.FireAuraEffect();
		}

		// Token: 0x04000DA5 RID: 3493
		private float dmg;
	}
}
