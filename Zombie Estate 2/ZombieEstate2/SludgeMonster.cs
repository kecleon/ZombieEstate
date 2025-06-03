using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200003C RID: 60
	internal class SludgeMonster : Zombie
	{
		// Token: 0x06000173 RID: 371 RVA: 0x0000AE38 File Offset: 0x00009038
		public SludgeMonster() : base(ZombieType.SludgeMonster)
		{
			this.TextureCoord = new Point(60, 12);
			this.startingTex = this.TextureCoord;
			this.scale = 0.55f;
			this.floorHeight = 0.55f;
			this.ProgressiveDamage = false;
			this.BloodType = ParticleType.Sludge;
			this.GibbletType = ParticleType.SludgeGibblet;
			this.Mass = 2f;
			this.AffectedByGravity = true;
			this.range = 1f;
			this.leapSpeed = 6f;
			this.topAttackCooldown = 2f;
			this.attackDamage = 6;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.sludgeTime += Global.RandomFloat(0f, 0.4f);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000AF04 File Offset: 0x00009104
		public override void Update(float elapsed)
		{
			this.sludgeTime -= elapsed;
			if (this.sludgeTime <= 0f)
			{
				this.sludgeTime = 1.5f;
				AOE obj = new AOE(AOEType.Sludge, 0f, 0.8f, this.Position, 10f, null, this);
				Global.MasterCache.CreateObject(obj);
			}
			base.Update(elapsed);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000AF68 File Offset: 0x00009168
		public override void DestroyObject()
		{
			for (int i = 0; i < 6; i++)
			{
				Vector3 randomPosition = VerchickMath.GetRandomPosition(this.Position, 1.5f);
				AOE obj = new AOE(AOEType.Sludge, 0f, 0.8f, randomPosition, 10f, null, this);
				Global.MasterCache.CreateObject(obj);
			}
			base.DestroyObject();
		}

		// Token: 0x040000F6 RID: 246
		private float sludgeTime = 1f;
	}
}
