using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001EB RID: 491
	internal class Buff_DmgBoost : Buff
	{
		// Token: 0x06000D21 RID: 3361 RVA: 0x00069F30 File Offset: 0x00068130
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = PowerUpDrop.DURATION;
			this.Src = Global.GetTexRectange(67, 37);
			this.Positive = true;
			this.Name = "Damage Boost";
			this.TickLength = 0.05f;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00069F7C File Offset: 0x0006817C
		public override void Tick()
		{
			Vector3 randomPosition = VerchickMath.GetRandomPosition(this.Parent.Position, 0.15f);
			Global.MasterCache.CreateParticle(ParticleType.DmgBoost, randomPosition, Vector3.Zero);
			base.Tick();
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00069FB8 File Offset: 0x000681B8
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.BulletDamageMod += this.dmg;
			spec.FireDmg += this.dmg;
			spec.EarthDmg += this.dmg;
			spec.WaterDmg += this.dmg;
			spec.ExplosionDamageMod += this.dmg;
			spec.MeleeDamageMod += this.dmg;
			spec.MinionDmgMod += this.dmg;
			return true;
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x0006A052 File Offset: 0x00068252
		public override string Description
		{
			get
			{
				return string.Format("Player deals double damage with guns, elemental, melee weapons, minions, and explosives.", new object[0]);
			}
		}

		// Token: 0x04000DA6 RID: 3494
		private float dmg = 100f;
	}
}
