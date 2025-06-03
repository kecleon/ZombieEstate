using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Enemies.BossMinions
{
	// Token: 0x020001D6 RID: 470
	internal class MiniPlant : Zombie
	{
		// Token: 0x06000C84 RID: 3204 RVA: 0x00067ADC File Offset: 0x00065CDC
		public MiniPlant() : base(ZombieType.NOTHING)
		{
			base.InitSpeed(1f);
			this.TextureCoord = new Point(72, 12);
			this.startingTex = this.TextureCoord;
			this.scale = 0.3f;
			this.floorHeight = 0.3f;
			this.ProgressiveDamage = false;
			this.NODAMAGE = true;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.None;
			this.AffectedByGravity = true;
			this.range = 6f;
			this.leapSpeed = 4f;
			this.topAttackCooldown = 6f;
			this.attackDamage = 8;
			this.PreciseDirection = false;
			this.GibbletChance = 0;
			this.BounceEnabled = true;
			this.Worth = 0f;
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00067BA3 File Offset: 0x00065DA3
		public override void Update(float elapsed)
		{
			if (this.cd > 0f)
			{
				this.cd -= elapsed;
			}
			base.Update(elapsed);
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Attack()
		{
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00067BC7 File Offset: 0x00065DC7
		public override bool ShouldIAttack()
		{
			return base.ShouldIAttack() && this.cd <= 0f;
		}

		// Token: 0x04000D50 RID: 3408
		private float cd = 2f;
	}
}
