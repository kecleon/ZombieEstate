using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000031 RID: 49
	internal class Glooper : Zombie
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00007F8C File Offset: 0x0000618C
		public Glooper() : base(ZombieType.Glooper)
		{
			base.InitSpeed(0.8f);
			this.TextureCoord = new Point(60, 12);
			this.startingTex = this.TextureCoord;
			this.scale = 0.5f;
			this.ProgressiveDamage = false;
			this.BloodType = ParticleType.Sludge;
			this.GibbletType = ParticleType.SludgeGibblet;
			this.AffectedByGravity = true;
			this.range = 0.8f;
			this.leapSpeed = 5f;
			this.topAttackCooldown = 3f;
			this.attackDamage = 12;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.Worth = 5f;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00008034 File Offset: 0x00006234
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 140f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 10;
			this.BaseSpecialProperties.LifeStealPercent = 0f;
			this.SomethingChanged = true;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00008098 File Offset: 0x00006298
		public override void Killed(Shootable attacker, bool fromNet)
		{
			if (!this.mKilled)
			{
				AOE obj = new AOE(AOEType.Sludge, 0f, 1.4f, this.Position, 4f, null, this);
				Global.MasterCache.CreateObject(obj);
			}
			base.Killed(attacker, fromNet);
		}
	}
}
