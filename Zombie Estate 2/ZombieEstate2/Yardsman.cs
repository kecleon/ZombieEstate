using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000040 RID: 64
	internal class Yardsman : Boss
	{
		// Token: 0x06000187 RID: 391 RVA: 0x0000B258 File Offset: 0x00009458
		public Yardsman()
		{
			this.Position = new Vector3(16f, 0f, 12f);
			this.scale = 0.8f;
			this.PreciseDirection = true;
			this.floorHeight = 0.8f;
			this.ProgressiveDamage = true;
			this.Mass = 2500f;
			this.BounceEnabled = true;
			base.InitSpeed(1.9f);
			this.attackDamage = 15;
			this.topAttackCooldown = 4f;
			this.spawning = true;
			this.AffectedByGravity = true;
			this.startingTex = new Point(64, 6);
			this.TextureCoord = new Point(64, 6);
			this.ActivateObject(this.Position, this.TextureCoord);
			this.spawnSpeed = 0.25f;
			this.EngageDistance = float.MaxValue;
			this.range = 3f;
			this.leapSpeed = 12f;
			this.Worth = 1000f;
			this.GibbletChance = 0;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000B360 File Offset: 0x00009560
		public override void Update(float elapsed)
		{
			this.SpawnTractor -= elapsed;
			if (this.SpawnTractor <= 0f)
			{
				this.SpawnTractor = Global.RandomFloat(4f, 8f);
				Tractor obj = new Tractor(this.Position);
				Global.MasterCache.CreateObject(obj);
			}
			base.Update(elapsed);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000B3BC File Offset: 0x000095BC
		public override void DestroyObject()
		{
			Camera.SlowTime(3f, 0.2f);
			for (int i = 0; i < 100; i++)
			{
				Global.MasterCache.CreateParticle(ParticleType.Blood, VerchickMath.GetRandomPosition(this.Position, 1f), Vector3.Zero);
			}
			base.DestroyObject();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000B40C File Offset: 0x0000960C
		public override void Drops()
		{
			foreach (Player player in Global.PlayerList)
			{
				new SuperWeaponDrop(player, this.Position);
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void IncreaseStrength(float mod)
		{
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void ChangeMainTex(Point tex, bool scrambleDmg)
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000B464 File Offset: 0x00009664
		public override string GetBossName()
		{
			return "Billy Billington";
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000B46B File Offset: 0x0000966B
		public override string GetBossSubtitle()
		{
			return "The Yardsman";
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000B472 File Offset: 0x00009672
		public override bool ActivateBossLevelStuff()
		{
			return false;
		}

		// Token: 0x040000FC RID: 252
		private float SpawnTractor = 5f;
	}
}
