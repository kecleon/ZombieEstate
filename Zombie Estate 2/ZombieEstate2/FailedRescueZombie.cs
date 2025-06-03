using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000036 RID: 54
	internal class FailedRescueZombie : Zombie
	{
		// Token: 0x06000154 RID: 340 RVA: 0x00009F9C File Offset: 0x0000819C
		public FailedRescueZombie() : base(ZombieType.FailedRescueZombie)
		{
			base.InitSpeed(1.6f);
			this.TextureCoord = new Point(52, 24);
			this.startingTex = this.TextureCoord;
			this.scale = 0.45f;
			this.ProgressiveDamage = true;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.HazmatGibblet;
			this.AffectedByGravity = true;
			this.range = 1.5f;
			this.leapSpeed = 6f;
			this.topAttackCooldown = 4f;
			this.attackDamage = 20;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.Worth = 5f;
		}
	}
}
