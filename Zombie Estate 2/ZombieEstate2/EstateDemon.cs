using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000035 RID: 53
	internal class EstateDemon : Boss
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000946C File Offset: 0x0000766C
		public float Mod
		{
			get
			{
				float num = 1f;
				num *= num;
				return Math.Max(0.3f, num);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00009490 File Offset: 0x00007690
		public EstateDemon()
		{
			this.Position = new Vector3(15f, 0f, 8f);
			this.PreciseDirection = true;
			this.scale = 3f;
			this.floorHeight = 2.8f;
			this.ProgressiveDamage = false;
			this.NODAMAGE = true;
			this.Mass = 2500f;
			this.BounceEnabled = true;
			base.InitSpeed(2.4f);
			this.attackDamage = 15;
			this.topAttackCooldown = 4f;
			this.spawning = false;
			this.AffectedByGravity = false;
			this.startingTex = new Point(64, 58);
			this.TextureCoord = new Point(64, 58);
			this.ActivateObject(this.Position, this.TextureCoord);
			this.BloodType = ParticleType.DemonBlood;
			this.NoTurretTarget = true;
			this.EngageDistance = float.MaxValue;
			this.BounceEnabled = false;
			this.range = 3f;
			this.leapSpeed = 12f;
			this.Worth = 10000f;
			this.TexScale = 2f;
			this.GibbletChance = 0;
			this.state = EstateDemon.DemonState.Spawning;
			this.CameraOffset = new Vector3(0f, 0f, 4f);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00009620 File Offset: 0x00007820
		private void UpdateMovementAI(float elapsed)
		{
			if (this.state == EstateDemon.DemonState.Idle)
			{
				this.stoppedTime -= elapsed;
				if (this.stoppedTime <= 0f)
				{
					this.stoppedTime = Global.RandomFloat(8f, 16f);
					this.movingTime = Global.RandomFloat(1f, 4f);
					if (this.Position.X <= (float)(this.maxLeft + 2))
					{
						this.state = EstateDemon.DemonState.MovingRight;
					}
					else if (this.Position.X >= (float)(this.maxRight - 2))
					{
						this.state = EstateDemon.DemonState.MovingLeft;
					}
					else if (Global.Probability(50))
					{
						this.state = EstateDemon.DemonState.MovingLeft;
					}
					else
					{
						this.state = EstateDemon.DemonState.MovingRight;
					}
					Camera.ShakeCamera(this.movingTime, 0.05f);
				}
				return;
			}
			if (this.state == EstateDemon.DemonState.MovingLeft || this.state == EstateDemon.DemonState.MovingRight)
			{
				this.movingTime -= elapsed;
				if (this.movingTime <= 0f)
				{
					this.state = EstateDemon.DemonState.Idle;
					this.movement.X = 0f;
					return;
				}
				if (this.state == EstateDemon.DemonState.MovingLeft)
				{
					this.movement.X = -1f;
				}
				if (this.state == EstateDemon.DemonState.MovingRight)
				{
					this.movement.X = 1f;
				}
				if (this.Position.X < (float)this.maxLeft)
				{
					this.Position.X = (float)this.maxLeft;
					this.movement.X = 0f;
					this.state = EstateDemon.DemonState.Idle;
					Camera.ShakeCamera(0f, 0f);
				}
				if (this.Position.X > (float)this.maxRight)
				{
					this.Position.X = (float)this.maxRight;
					this.movement.X = 0f;
					this.state = EstateDemon.DemonState.Idle;
					Camera.ShakeCamera(0f, 0f);
				}
				this.CreateParticle(ParticleType.Dirt, 1);
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00009804 File Offset: 0x00007A04
		private void CreateParticle(ParticleType type, int count)
		{
			for (int i = 0; i < count; i++)
			{
				Vector3 pos = new Vector3(0f, 0.4f, 0f);
				pos.X = this.Position.X + Global.RandomFloat(-3.5f, 3.5f);
				pos.Z = this.Position.Z + Global.RandomFloat(0f, 0.2f) - 0.1f;
				Global.MasterCache.CreateParticle(type, pos, Vector3.Zero);
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00009890 File Offset: 0x00007A90
		public override void Update(float elapsed)
		{
			if (this.LeftHand == null)
			{
				this.LeftHand = new DemonHand(true, this);
				this.RightHand = new DemonHand(false, this);
				Global.MasterCache.CreateObject(this.LeftHand);
				Global.MasterCache.CreateObject(this.RightHand);
			}
			if (this.state == EstateDemon.DemonState.Spawning)
			{
				this.Spawning(elapsed);
				base.Update(elapsed);
				return;
			}
			this.particleTime -= elapsed;
			if (this.particleTime <= 0f)
			{
				this.CreateParticle(ParticleType.DemonFire, 2);
				this.particleTime = 0.05f;
			}
			this.angle += elapsed * 0.75f;
			if (this.angle > 6.2831855f)
			{
				this.angle -= 6.2831855f;
			}
			if (this.state != EstateDemon.DemonState.Spawning)
			{
				this.Position.Y = 2.6f + (float)(Math.Sin((double)this.angle) * 0.25);
			}
			this.Hurting(elapsed);
			this.UpdateAttacks(elapsed);
			this.DemonFire -= elapsed;
			this.MiniDemon -= elapsed;
			this.Fire -= elapsed;
			if (this.elapsedHurt > 0f)
			{
				this.elapsedHurt -= elapsed;
			}
			base.Update(elapsed);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000099E8 File Offset: 0x00007BE8
		public void Hurt(bool left)
		{
			if (this.state == EstateDemon.DemonState.Hurt)
			{
				return;
			}
			this.NoTurretTarget = false;
			if (left)
			{
				this.LeftHand.Shake(6f);
				this.RightHand.ShakeIdle(6f);
			}
			else
			{
				this.LeftHand.ShakeIdle(6f);
				this.RightHand.Shake(6f);
			}
			this.hurtTime = 6f;
			this.mainCooldown = 2f;
			this.state = EstateDemon.DemonState.Hurt;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00009A68 File Offset: 0x00007C68
		private void UpdateAttacks(float elapsed)
		{
			this.mainZombieCooldown -= elapsed;
			if (this.state == EstateDemon.DemonState.Hurt)
			{
				return;
			}
			if (this.state == EstateDemon.DemonState.Idle)
			{
				this.mainCooldown -= elapsed;
				if (this.mainCooldown <= 0f)
				{
					int num;
					if (this.mainZombieCooldown <= 0f)
					{
						num = 3;
					}
					else
					{
						num = Global.rand.Next(0, 3);
					}
					if (num == 0)
					{
						this.Punch();
					}
					if (num == 1)
					{
						this.Slam();
					}
					if (num == 2)
					{
						this.LeftHand.Split(1f / this.Mod);
						this.RightHand.Split(1f / this.Mod);
						this.state = EstateDemon.DemonState.AboutToBreathe;
					}
					if (num == 3)
					{
						this.BeginSummon();
					}
				}
			}
			if (this.state == EstateDemon.DemonState.Breathing)
			{
				this.meteorTime += elapsed;
				if (this.meteorTime > 0.4f)
				{
					this.meteorTime = 0f;
					for (int i = 0; i < 2; i++)
					{
					}
				}
				this.fireTime -= elapsed;
				if (this.fireTime < 0f)
				{
					this.GoIdle();
					this.TextureCoord = new Point(64, 58);
				}
			}
			EstateDemon.DemonState demonState = this.state;
			if (this.state == EstateDemon.DemonState.Summoning)
			{
				this.zombieTime -= elapsed;
				this.zombieCooldown -= elapsed;
				if (this.zombieCooldown <= 0f)
				{
					this.zombieCooldown = 0.5f;
					for (int j = 0; j < 4; j++)
					{
						Vector3 randomPosition = new Vector3(this.Position.X, this.Position.Y, this.Position.Z + 8f);
						randomPosition = VerchickMath.GetRandomPosition(randomPosition, 8f);
						if (Global.Level.GetTile((int)randomPosition.X, (int)randomPosition.Y) != null)
						{
							Zombie zombie = new Zombie(ZombieType.NormalZombie);
							zombie.Position.X = randomPosition.X;
							zombie.Position.Z = randomPosition.Z;
							zombie.Position.Y = -1f;
							zombie.ActivateObject(zombie.Position, zombie.TextureCoord);
							Global.MasterCache.CreateObject(zombie);
						}
					}
				}
				if (this.zombieTime <= 0f)
				{
					this.state = EstateDemon.DemonState.EndSummon;
					this.TextureCoord = new Point(64, 58);
				}
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00009CCC File Offset: 0x00007ECC
		private void Hurting(float elapsed)
		{
			if (this.state == EstateDemon.DemonState.Hurt)
			{
				this.hurtTime -= elapsed;
				this.TextureCoord = new Point(64, 60);
				if (this.prevHurtFrame)
				{
					this.TextureCoord = new Point(64, 62);
					this.prevHurtFrame = false;
				}
				if (this.hurtTime <= 0f)
				{
					this.state = EstateDemon.DemonState.Idle;
					this.TextureCoord = new Point(64, 58);
					this.NoTurretTarget = true;
				}
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00009D47 File Offset: 0x00007F47
		private void Punch()
		{
			this.GetRandomHand().Punch(Global.Player, 2f * this.Mod);
			this.state = EstateDemon.DemonState.Punching;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00009D6C File Offset: 0x00007F6C
		private void Slam()
		{
			this.state = EstateDemon.DemonState.Slamming;
			this.GetRandomHand().Slam();
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00009D80 File Offset: 0x00007F80
		public void GoIdle()
		{
			if (this.state != EstateDemon.DemonState.Hurt && this.state != EstateDemon.DemonState.Idle)
			{
				this.state = EstateDemon.DemonState.Idle;
			}
			this.mainCooldown = Global.RandomFloat(4f, 6f) * this.Mod;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00009DB7 File Offset: 0x00007FB7
		public void Breathe()
		{
			if (this.state != EstateDemon.DemonState.Breathing)
			{
				this.TextureCoord = new Point(64, 56);
				this.fireTime = 1f / this.Mod;
				this.state = EstateDemon.DemonState.Breathing;
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00009DEA File Offset: 0x00007FEA
		public void Summon()
		{
			if (this.state != EstateDemon.DemonState.Summoning)
			{
				this.state = EstateDemon.DemonState.Summoning;
				this.TextureCoord = new Point(66, 56);
				Camera.ShakeCamera(this.zombieTime, 0.04f);
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00009E20 File Offset: 0x00008020
		public void BeginSummon()
		{
			if (this.state != EstateDemon.DemonState.AboutToSummon)
			{
				this.zombieTime = 4f / Math.Max(this.Mod, 0.5f);
				this.state = EstateDemon.DemonState.AboutToSummon;
				this.LeftHand.UpToSummon(this.zombieTime);
				this.RightHand.UpToSummon(this.zombieTime);
				this.mainZombieCooldown = 55f * Math.Min(1f, this.Mod + 0.5f);
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00009E9D File Offset: 0x0000809D
		private void Spawning(float elapsed)
		{
			this.Position.Y = this.Position.Y + 0.5f * elapsed;
			this.CreateParticle(ParticleType.Dirt, 4);
			if (this.Position.Y > 2.6f)
			{
				this.state = EstateDemon.DemonState.Idle;
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00009ED8 File Offset: 0x000080D8
		public override void DestroyObject()
		{
			Global.BossActive = false;
			Global.Boss = null;
			GameManager.level.BossDeactivated();
			Camera.SlowTime(3f, 0.2f);
			for (int i = 0; i < 100; i++)
			{
				Global.MasterCache.CreateParticle(ParticleType.Blood, VerchickMath.GetRandomPosition(this.Position, 1f), Vector3.Zero);
			}
			base.DestroyObject();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00009F3D File Offset: 0x0000813D
		public override void BaseDestroyObject()
		{
			Global.BossActive = false;
			Global.Boss = null;
			GameManager.level.BossDeactivated();
			base.BaseDestroyObject();
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Drops()
		{
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00009F5B File Offset: 0x0000815B
		public override void UpdateDirection()
		{
			this.Velocity = Vector3.Zero;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00009F68 File Offset: 0x00008168
		public override void UpdateFacing()
		{
			this.facing = Facing.Down;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Attack()
		{
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00009F71 File Offset: 0x00008171
		private DemonHand GetRandomHand()
		{
			if (Global.rand.Next(0, 2) == 0)
			{
				return this.LeftHand;
			}
			return this.RightHand;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void IncreaseStrength(float mod)
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void ChangeMainTex(Point tex, bool scrambleDmg)
		{
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00009F8E File Offset: 0x0000818E
		public override string GetBossName()
		{
			return "Some Name";
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00009F95 File Offset: 0x00008195
		public override string GetBossSubtitle()
		{
			return "Estate Demon";
		}

		// Token: 0x040000D5 RID: 213
		private float DemonFire = 2f;

		// Token: 0x040000D6 RID: 214
		private float MiniDemon = 2f;

		// Token: 0x040000D7 RID: 215
		private float Fire = 1f;

		// Token: 0x040000D8 RID: 216
		public EstateDemon.DemonState state;

		// Token: 0x040000D9 RID: 217
		private DemonHand LeftHand;

		// Token: 0x040000DA RID: 218
		private DemonHand RightHand;

		// Token: 0x040000DB RID: 219
		private float stoppedTime = 1f;

		// Token: 0x040000DC RID: 220
		private float movingTime;

		// Token: 0x040000DD RID: 221
		private int maxLeft = 8;

		// Token: 0x040000DE RID: 222
		private int maxRight = 18;

		// Token: 0x040000DF RID: 223
		private float angle;

		// Token: 0x040000E0 RID: 224
		private float hurtTime;

		// Token: 0x040000E1 RID: 225
		private float particleTime;

		// Token: 0x040000E2 RID: 226
		private float fireTime;

		// Token: 0x040000E3 RID: 227
		private float meteorTime;

		// Token: 0x040000E4 RID: 228
		private float zombieTime;

		// Token: 0x040000E5 RID: 229
		private float zombieCooldown;

		// Token: 0x040000E6 RID: 230
		private float mainZombieCooldown = 20f;

		// Token: 0x040000E7 RID: 231
		private float mainCooldown = 6f;

		// Token: 0x040000E8 RID: 232
		private bool prevHurtFrame;

		// Token: 0x040000E9 RID: 233
		private float elapsedHurt;

		// Token: 0x02000205 RID: 517
		public enum DemonState
		{
			// Token: 0x04000DE7 RID: 3559
			MovingLeft,
			// Token: 0x04000DE8 RID: 3560
			MovingRight,
			// Token: 0x04000DE9 RID: 3561
			Idle,
			// Token: 0x04000DEA RID: 3562
			Hurt,
			// Token: 0x04000DEB RID: 3563
			Punching,
			// Token: 0x04000DEC RID: 3564
			Slamming,
			// Token: 0x04000DED RID: 3565
			Breathing,
			// Token: 0x04000DEE RID: 3566
			AboutToBreathe,
			// Token: 0x04000DEF RID: 3567
			AboutToSummon,
			// Token: 0x04000DF0 RID: 3568
			Summoning,
			// Token: 0x04000DF1 RID: 3569
			EndSummon,
			// Token: 0x04000DF2 RID: 3570
			Spawning
		}
	}
}
