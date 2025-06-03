using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000039 RID: 57
	internal class Hazmat : Zombie
	{
		// Token: 0x0600015F RID: 351 RVA: 0x0000A638 File Offset: 0x00008838
		public Hazmat() : base(ZombieType.Hazmat)
		{
			base.InitSpeed(1.2f);
			this.TextureCoord = new Point(56, 12);
			this.startingTex = this.TextureCoord;
			this.scale = 0.4f;
			this.ProgressiveDamage = false;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.HazmatGibblet;
			this.AffectedByGravity = true;
			this.range = 1f;
			this.leapSpeed = 12f;
			this.topAttackCooldown = 3f;
			this.attackDamage = 15;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.Worth = 5f;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000A6E0 File Offset: 0x000088E0
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 80f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 0;
			this.BaseSpecialProperties.LifeStealPercent = 0f;
			this.SomethingChanged = true;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000A744 File Offset: 0x00008944
		public override bool ShouldIAttack()
		{
			return this.Target != null && VerchickMath.WithinDistance(base.TwoDPosition(), this.Target.TwoDPosition(), this.range) && Global.Level.InLineOfSight(this.Position, this.Target.Position);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000A797 File Offset: 0x00008997
		public override void Attack()
		{
			this.Damage(this, 1000000f, DamageType.Physical, false, false, null);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000A7A9 File Offset: 0x000089A9
		public override void Killed(Shootable attacker, bool fromNet)
		{
			if (!this.mKilled)
			{
				Explosion.CreateExplosion(0.7f, 8f, 2f, "Goo", this.Position, this, false);
			}
			base.Killed(attacker, fromNet);
		}
	}
}
