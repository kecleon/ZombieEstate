using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000034 RID: 52
	internal class DrZombie : Zombie
	{
		// Token: 0x06000137 RID: 311 RVA: 0x0000925C File Offset: 0x0000745C
		public DrZombie() : base(ZombieType.DrZombie)
		{
			base.InitSpeed(1.2f);
			this.TextureCoord = new Point(68, 12);
			this.startingTex = this.TextureCoord;
			this.scale = 0.4f;
			this.ProgressiveDamage = false;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.DrZombieGibblet;
			this.AffectedByGravity = true;
			this.range = 1f;
			this.leapSpeed = 6f;
			this.topAttackCooldown = 3f;
			this.attackDamage = 10;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.Worth = 5f;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00009318 File Offset: 0x00007518
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 150f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 10;
			this.BaseSpecialProperties.EarthResist = 5;
			this.BaseSpecialProperties.FireResist = 5;
			this.BaseSpecialProperties.WaterResist = 5;
			this.BaseSpecialProperties.LifeStealPercent = 0f;
			this.BaseSpecialProperties.HealOverTime = 2.5f;
			this.SomethingChanged = true;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000093B0 File Offset: 0x000075B0
		public override void Update(float elapsed)
		{
			this.HealCD -= elapsed;
			if (this.HealCD < 0f)
			{
				this.HealCD = 2f;
				if (this.tile != null)
				{
					foreach (GameObject gameObject in this.tile.AdjacentObjects)
					{
						if (gameObject is Zombie && VerchickMath.WithinDistance(base.TwoDPosition(), gameObject.TwoDPosition(), this.healRange))
						{
							(gameObject as Zombie).Heal(20f, this, false);
						}
					}
				}
			}
			base.Update(elapsed);
		}

		// Token: 0x040000D3 RID: 211
		private float HealCD = 1f;

		// Token: 0x040000D4 RID: 212
		private float healRange = 2f;
	}
}
