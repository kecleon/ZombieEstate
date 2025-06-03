using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000029 RID: 41
	internal class BrainZombie : Zombie
	{
		// Token: 0x060000EC RID: 236 RVA: 0x00006C60 File Offset: 0x00004E60
		public BrainZombie() : base(ZombieType.BrainZombie)
		{
			base.InitSpeed(1.4f);
			this.TextureCoord = new Point(60, 18);
			this.startingTex = this.TextureCoord;
			this.scale = 0.5f;
			this.ProgressiveDamage = false;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.BrainGibblet;
			this.AffectedByGravity = true;
			this.range = 1f;
			this.leapSpeed = 6f;
			this.topAttackCooldown = 3f;
			this.attackDamage = 7;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.Worth = 5f;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006D04 File Offset: 0x00004F04
		public void InitiateBrianUIDs(ushort uid1, ushort uid2, ushort uid3)
		{
			this.mBrainUIDs = new ushort[3];
			this.mBrainUIDs[0] = uid1;
			this.mBrainUIDs[1] = uid2;
			this.mBrainUIDs[2] = uid3;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006D30 File Offset: 0x00004F30
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 80f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 0;
			this.BaseSpecialProperties.LifeStealPercent = 0f;
			this.SomethingChanged = true;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006D94 File Offset: 0x00004F94
		public override void Killed(Shootable attacker, bool fromNet)
		{
			if (!this.mKilled && Global.ZombieList.Count < 440)
			{
				Random random = new Random(this.mSeed);
				for (int i = 0; i < 3; i++)
				{
					Brain brain = new Brain();
					brain.Velocity.X = Global.RandomFloat(random, -3f, 3f);
					brain.Velocity.Y = Global.RandomFloat(random, 3f, 6f);
					brain.Velocity.Z = Global.RandomFloat(random, -3f, 3f);
					brain.InitiateNet(this.mBrainUIDs[i], this.mBrainUIDs[i], random.Next());
					brain.ActivateObject(this.Position, brain.TextureCoord);
					Global.MasterCache.CreateObject(brain);
				}
			}
			base.Killed(attacker, fromNet);
		}

		// Token: 0x0400009E RID: 158
		private ushort[] mBrainUIDs;
	}
}
