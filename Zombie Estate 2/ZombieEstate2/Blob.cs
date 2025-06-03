using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000025 RID: 37
	internal class Blob : Zombie
	{
		// Token: 0x060000DB RID: 219 RVA: 0x00006908 File Offset: 0x00004B08
		public Blob() : base(ZombieType.Blob)
		{
			base.InitSpeed(1.2f);
			this.TextureCoord = new Point(68, 24);
			this.startingTex = this.TextureCoord;
			this.scale = 0.35f;
			this.floorHeight = 0.35f;
			this.ProgressiveDamage = false;
			this.BloodType = ParticleType.BlobBlood;
			this.GibbletType = ParticleType.BlobGibblet;
			this.AffectedByGravity = true;
			this.range = 1f;
			this.leapSpeed = 6f;
			this.topAttackCooldown = 3f;
			this.attackDamage = 5;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.Worth = 1f;
			this.BounceEnabled = false;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000069C0 File Offset: 0x00004BC0
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 50f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 0;
			this.BaseSpecialProperties.EarthResist = 20;
			this.BaseSpecialProperties.WaterResist = 20;
			this.BaseSpecialProperties.FireResist = 70;
			this.BaseSpecialProperties.LifeStealPercent = 0f;
			this.SomethingChanged = true;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006A48 File Offset: 0x00004C48
		public override void Update(float elapsed)
		{
			base.UnSquishMe(0.2f, true);
			base.Update(elapsed);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006A60 File Offset: 0x00004C60
		public override void DestroyObject()
		{
			AOE obj = new AOE(AOEType.BlobGoo, 6f, 1f, this.Position, 4f, null, this);
			Global.MasterCache.CreateObject(obj);
			base.DestroyObject();
		}
	}
}
