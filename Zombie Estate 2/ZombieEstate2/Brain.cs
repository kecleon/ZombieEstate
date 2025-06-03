using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000028 RID: 40
	internal class Brain : Zombie
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00006B40 File Offset: 0x00004D40
		public Brain() : base(ZombieType.Brain)
		{
			base.InitSpeed(1.9f);
			this.TextureCoord = new Point(60, 24);
			this.startingTex = this.TextureCoord;
			this.scale = 0.38f;
			this.ProgressiveDamage = false;
			this.NODAMAGE = true;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.None;
			this.AffectedByGravity = true;
			this.range = 0.5f;
			this.leapSpeed = 3f;
			this.topAttackCooldown = 3f;
			this.attackDamage = 7;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.Worth = 5f;
			this.DontCountAsKill = true;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Drops()
		{
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006BF4 File Offset: 0x00004DF4
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 20f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 0;
			this.BaseSpecialProperties.LifeStealPercent = 0f;
			this.SomethingChanged = true;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006C55 File Offset: 0x00004E55
		public override void Killed(Shootable attacker, bool fromNet)
		{
			base.Killed(attacker, fromNet);
		}
	}
}
