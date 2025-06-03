using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x0200003B RID: 59
	internal class RobBurglar : Zombie
	{
		// Token: 0x06000165 RID: 357 RVA: 0x0000A878 File Offset: 0x00008A78
		public RobBurglar() : base(ZombieType.RobBurglar)
		{
			base.InitSpeed(2.4f);
			this.TextureCoord = new Point(64, 0);
			this.startingTex = this.TextureCoord;
			this.scale = 0.45f;
			this.ProgressiveDamage = false;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.None;
			this.AffectedByGravity = true;
			this.range = 2f;
			this.leapSpeed = 6f;
			this.topAttackCooldown = 1f;
			this.attackDamage = 20;
			this.PreciseDirection = true;
			this.GibbletChance = 75;
			this.timer = new Timer(1f);
			this.Mass = 2.5f;
			this.State = RobBurglar.RobState.Waiting;
			this.Worth = 100f;
			this.mSyncTimer.Start();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000A968 File Offset: 0x00008B68
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 280f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 20;
			this.BaseSpecialProperties.LifeStealPercent = 0.5f;
			this.SomethingChanged = true;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000A9CC File Offset: 0x00008BCC
		public override void Update(float elapsed)
		{
			if (this.spawning)
			{
				this.Spawn(elapsed);
				this.TextureCoord.X = 68;
				this.TextureCoord.Y = 0;
				return;
			}
			this.BoundingRect.X = (int)this.Position.X;
			this.BoundingRect.Y = (int)this.Position.Z;
			base.UpdateTile();
			if (this.mState != RobBurglar.RobState.Angry)
			{
				using (List<Player>.Enumerator enumerator = Global.PlayerList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (VerchickMath.WithinDistance(enumerator.Current.twoDPosition, this.twoDPosition, 1.4f))
						{
							this.State = RobBurglar.RobState.Angry;
						}
					}
				}
			}
			if (this.mState == RobBurglar.RobState.Waiting)
			{
				this.facing = Facing.Down;
				this.Waiting();
			}
			if (this.mState == RobBurglar.RobState.Warping)
			{
				this.facing = Facing.Down;
				this.Warping();
			}
			if (this.mState == RobBurglar.RobState.Munching)
			{
				this.facing = Facing.Down;
				this.Munching();
			}
			if (this.mState == RobBurglar.RobState.Angry)
			{
				base.Update(elapsed);
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000AAEC File Offset: 0x00008CEC
		public override void Spawn(float elapsed)
		{
			if (!this.moved)
			{
				this.Position = VerchickMath.GetRandomPosition(this.Rand, this.Position, 0.4f);
				this.Position.Y = this.floorHeight;
				this.moved = true;
			}
			this.TextureCoord.X = 68;
			this.TextureCoord.Y = 0;
			this.facing = Facing.Down;
			base.Spawn(elapsed);
			if (!this.mSyncTimer.Expired())
			{
				this.mDoISync = false;
				return;
			}
			this.mDoISync = true;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000AB78 File Offset: 0x00008D78
		public override void Damage(Shootable attacker, float amount, DamageType damageType, bool noGore, bool AOE, List<BulletModifier> mods = null)
		{
			this.State = RobBurglar.RobState.Angry;
			base.Damage(attacker, amount, damageType, noGore, AOE, mods);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000AB90 File Offset: 0x00008D90
		private void Munching()
		{
			if (this.timer.Expired())
			{
				this.timer.Reset();
				this.timer.Start();
				this.frame++;
				if (this.frame % 2 == 0 && Global.Probability(10))
				{
					Vector3 randomPosition = VerchickMath.GetRandomPosition(this.Position, 0.3f);
					randomPosition.X += this.scale;
					Global.MasterCache.particleSystem.AddPaticle2D(RobBurglar.Part, randomPosition);
				}
				this.TextureCoord.Y = this.frame;
				if (this.frame == 12)
				{
					this.State = RobBurglar.RobState.Waiting;
				}
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000AC40 File Offset: 0x00008E40
		private void Warping()
		{
			if (this.timer.Expired())
			{
				this.timer.Reset();
				this.timer.Start();
				this.frame++;
				if (this.frame == 4)
				{
					this.State = RobBurglar.RobState.Munching;
				}
				this.TextureCoord.Y = this.frame;
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000AC9F File Offset: 0x00008E9F
		private void Waiting()
		{
			this.facing = Facing.Down;
			if (this.timer.Expired())
			{
				this.State = RobBurglar.RobState.Warping;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000ACBC File Offset: 0x00008EBC
		// (set) Token: 0x0600016E RID: 366 RVA: 0x0000ACC4 File Offset: 0x00008EC4
		private RobBurglar.RobState State
		{
			get
			{
				return this.mState;
			}
			set
			{
				RobBurglar.RobState robState = this.mState;
				this.mState = value;
				if (this.mState == RobBurglar.RobState.Waiting)
				{
					this.TextureCoord.X = 68;
					this.TextureCoord.Y = 0;
					this.frame = 0;
					this.timer.mTotalTime = 2f;
					this.timer.Start();
				}
				if (this.mState == RobBurglar.RobState.Warping)
				{
					this.TextureCoord.X = 68;
					this.TextureCoord.Y = 0;
					this.frame = 0;
					this.timer.mTotalTime = 0.5f;
					this.timer.Start();
				}
				if (this.mState == RobBurglar.RobState.Munching)
				{
					this.timer.mTotalTime = 0.5f;
					this.timer.Start();
				}
				if (this.mState == RobBurglar.RobState.Angry && robState != RobBurglar.RobState.Angry)
				{
					this.timer.Stop();
					this.TextureCoord.X = 64;
					this.TextureCoord.Y = 0;
					this.scaleTimer.Reset();
					this.scaleTimer.DeltaDelegate = new Timer.TimerDelegate(this.ScaleDel);
					this.scaleTimer.Start();
				}
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000ADE8 File Offset: 0x00008FE8
		protected override void AdditionalReceive(Msg_ZombieUpdate zUpdate)
		{
			if (zUpdate.Misc)
			{
				this.State = RobBurglar.RobState.Angry;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000ADF9 File Offset: 0x00008FF9
		protected override void AdditionalSync(Msg_ZombieUpdate zUpdate)
		{
			if (this.mState == RobBurglar.RobState.Angry)
			{
				zUpdate.Misc = true;
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000AE0B File Offset: 0x0000900B
		private void ScaleDel(float delta)
		{
			this.scale = 0.25f * delta + 0.45f;
			this.floorHeight = this.scale;
		}

		// Token: 0x040000EF RID: 239
		private RobBurglar.RobState mState;

		// Token: 0x040000F0 RID: 240
		private int frame;

		// Token: 0x040000F1 RID: 241
		private Timer timer;

		// Token: 0x040000F2 RID: 242
		private Timer scaleTimer = new Timer(1f);

		// Token: 0x040000F3 RID: 243
		private Timer mSyncTimer = new Timer(0.5f);

		// Token: 0x040000F4 RID: 244
		private bool moved;

		// Token: 0x040000F5 RID: 245
		private static string Part = "Munch";

		// Token: 0x02000206 RID: 518
		private enum RobState
		{
			// Token: 0x04000DF4 RID: 3572
			Waiting,
			// Token: 0x04000DF5 RID: 3573
			Warping,
			// Token: 0x04000DF6 RID: 3574
			Munching,
			// Token: 0x04000DF7 RID: 3575
			Angry
		}
	}
}
