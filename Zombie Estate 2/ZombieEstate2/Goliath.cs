using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000038 RID: 56
	internal class Goliath : Zombie
	{
		// Token: 0x0600015C RID: 348 RVA: 0x0000A424 File Offset: 0x00008624
		public Goliath() : base(ZombieType.Goliath)
		{
			base.InitSpeed(1.2f);
			this.TextureCoord = new Point(56, 18);
			this.startingTex = this.TextureCoord;
			this.scale = 0.7f;
			this.floorHeight = 0.7f;
			this.ProgressiveDamage = true;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.GoliathGibblet;
			this.AffectedByGravity = true;
			this.range = 2f;
			this.leapSpeed = 12f;
			this.topAttackCooldown = 4f;
			this.attackDamage = 20;
			this.PreciseDirection = false;
			this.GibbletChance = 100;
			base.InitSpeed(0.4f);
			this.Mass = 2f;
			this.ZRotation = 0f;
			this.Worth = 50f;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000A4FC File Offset: 0x000086FC
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 480f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 10;
			this.BaseSpecialProperties.EarthResist = 5;
			this.BaseSpecialProperties.FireResist = 5;
			this.BaseSpecialProperties.WaterResist = 5;
			this.BaseSpecialProperties.LifeStealPercent = 0f;
			this.SomethingChanged = true;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000A584 File Offset: 0x00008784
		public override void Update(float elapsed)
		{
			if (this.Bouncing)
			{
				if (this.movement != Vector2.Zero)
				{
					float num = 0.5f * elapsed;
					if (this.firstRotate)
					{
						num = 0.25f * elapsed;
					}
					if (this.rotatingLeft)
					{
						this.ZRotation += num;
					}
					else
					{
						this.ZRotation -= num;
					}
				}
				else
				{
					this.ZRotation = 0f;
					this.firstRotate = true;
				}
			}
			this.prevBounce = this.Bouncing;
			base.Update(elapsed);
			if (this.prevBounce && !this.Bouncing)
			{
				this.rotatingLeft = !this.rotatingLeft;
				this.firstRotate = false;
			}
		}

		// Token: 0x040000EC RID: 236
		private bool rotatingLeft;

		// Token: 0x040000ED RID: 237
		private bool prevBounce;

		// Token: 0x040000EE RID: 238
		private bool firstRotate = true;
	}
}
