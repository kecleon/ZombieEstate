using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.Enemies.BossMinions;
using ZombieEstate2.Enemies.Weapons;

namespace ZombieEstate2
{
	// Token: 0x02000042 RID: 66
	internal class YardsmanNew : BossWithState<YardsState>
	{
		// Token: 0x06000190 RID: 400 RVA: 0x0000B478 File Offset: 0x00009678
		public YardsmanNew()
		{
			this.Position = new Vector3(16f, 0f, 12f);
			this.scale = 0.8f;
			this.PreciseDirection = true;
			this.floorHeight = 0.8f;
			this.ProgressiveDamage = true;
			this.NODAMAGE = true;
			this.Mass = 2f;
			this.BounceEnabled = true;
			base.InitSpeed(1.9f);
			this.attackDamage = 15;
			this.topAttackCooldown = 4f;
			this.spawning = true;
			this.AffectedByGravity = true;
			this.startingTex = new Point(74, 2);
			this.TextureCoord = new Point(74, 2);
			this.TexScale = 2f;
			this.ActivateObject(this.Position, this.TextureCoord);
			this.spawnSpeed = 0.25f;
			this.EngageDistance = float.MaxValue;
			this.range = 3f;
			this.leapSpeed = 12f;
			this.Worth = 1000f;
			this.GibbletChance = 0;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000B590 File Offset: 0x00009790
		public override void AddStates()
		{
			this.StateMachine.AddState(YardsState.Chase, 8f, YardsState.BugsCharging, -1, -1, false, new UpdateStateDelegate(this.Chase));
			this.StateMachine.AddState(YardsState.BugsCharging, -1f, YardsState.BugsUnleashed, -1, -1, false, new UpdateStateDelegate(this.BugsCharging));
			this.StateMachine.AddState(YardsState.Chase2, 6f, YardsState.SpawnPlants, -1, -1, false, new UpdateStateDelegate(this.Chase));
			this.StateMachine.AddState(YardsState.SpawnPlants, -1f, YardsState.SpawnPlants, -1, -1, false, new UpdateStateDelegate(this.Plants));
			this.StateMachine.CurrentState = YardsState.Chase;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000B630 File Offset: 0x00009830
		public override void Update(float elapsed)
		{
			float num = 1f - this.Health / this.SpecialProperties.MaxHealth;
			int y = (int)((float)this.startingTex.Y + num * 4f * 2f);
			this.TextureCoord.Y = y;
			this.Thorns -= elapsed;
			if (this.Thorns <= 0f && (this.StateMachine.CurrentState == YardsState.Chase || this.StateMachine.CurrentState == YardsState.Chase2))
			{
				if (this.Target != null)
				{
					Vector2 vector = VerchickMath.DirectionToVector2(base.TwoDPosition(), this.Target.TwoDPosition());
					new Vector3(vector.X, 0f, vector.Y);
				}
				this.Thorns = 7f;
			}
			if (this.shock != null && this.shock.ALIVE)
			{
				this.shock.Update(elapsed);
			}
			base.Update(elapsed);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000B720 File Offset: 0x00009920
		public void Chase(float delta)
		{
			this.unleashed = false;
			this.AffectedByGravity = true;
			this.NOLOGIC = false;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000B737 File Offset: 0x00009937
		private void BugsCharging(float delta)
		{
			if (!this.charging)
			{
				this.Velocity.Y = 20f;
			}
			this.NOLOGIC = true;
			this.Bouncing = false;
			this.charging = true;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000B768 File Offset: 0x00009968
		private void Plants(float delta)
		{
			this.StateMachine.CurrentState = YardsState.Chase;
			for (int i = 0; i < 4; i++)
			{
				if (Global.ZombieList.Count < 200)
				{
					MiniPlant miniPlant = new MiniPlant();
					miniPlant.Position = new Vector3((float)this.Rand.Next(0, 31), 0.5f, (float)this.Rand.Next(0, 31));
					miniPlant.Active = true;
					Global.MasterCache.CreateObject(miniPlant);
				}
			}
			if (this.Target != null)
			{
				Vector2 vector = VerchickMath.DirectionToVector2(base.TwoDPosition(), this.Target.TwoDPosition());
				new Vector3(vector.X, 0f, vector.Y);
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000B81C File Offset: 0x00009A1C
		public override void Landed()
		{
			if (this.charging)
			{
				this.charging = false;
				this.shock = new Shockwave(20, 2f, ParticleType.Dirt_Short, 12, this.Position, 4f, 20f, 0.3f);
				this.NOLOGIC = false;
				this.StateMachine.CurrentState = YardsState.Chase2;
				Camera.ShakeCamera(0.2f, 0.1f);
			}
			base.Landed();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000B88B File Offset: 0x00009A8B
		public override void Attack()
		{
			if (this.StateMachine.CurrentState == YardsState.Chase || this.StateMachine.CurrentState == YardsState.Chase2)
			{
				base.Attack();
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000B8B0 File Offset: 0x00009AB0
		public override void DestroyObject()
		{
			Camera.SlowTime(3f, 0.2f);
			for (int i = 0; i < 100; i++)
			{
				Global.MasterCache.CreateParticle(ParticleType.Blood, VerchickMath.GetRandomPosition(this.Position, 1f), Vector3.Zero);
			}
			base.DestroyObject();
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000B900 File Offset: 0x00009B00
		public override void Drops()
		{
			int num = 0;
			foreach (Player player in Global.PlayerList)
			{
				new SuperWeaponDrop(player, new Vector3(this.Position.X - 1f + 0.25f * (float)num, this.Position.Y, this.Position.Z));
				num++;
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void IncreaseStrength(float mod)
		{
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void ChangeMainTex(Point tex, bool scrambleDmg)
		{
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000B464 File Offset: 0x00009664
		public override string GetBossName()
		{
			return "Billy Billington";
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000B98C File Offset: 0x00009B8C
		public override string GetBossSubtitle()
		{
			return "The Savage Gardner";
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000B472 File Offset: 0x00009672
		public override bool ActivateBossLevelStuff()
		{
			return false;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void StateSwitched()
		{
		}

		// Token: 0x04000104 RID: 260
		private bool unleashed;

		// Token: 0x04000105 RID: 261
		private Shockwave shock;

		// Token: 0x04000106 RID: 262
		private bool charging;

		// Token: 0x04000107 RID: 263
		private float Thorns = 7f;
	}
}
