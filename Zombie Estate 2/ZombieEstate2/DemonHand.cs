using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000033 RID: 51
	internal class DemonHand : Zombie
	{
		// Token: 0x06000122 RID: 290 RVA: 0x00008370 File Offset: 0x00006570
		public DemonHand(bool left, EstateDemon parent) : base(ZombieType.NOTHING)
		{
			this.left = left;
			this.parent = parent;
			this.radius = 0.4f;
			this.Mass = 99999f;
			this.DMG = this.BaseHandDmg;
			this.NoTurretTarget = true;
			this.NOLOGIC = true;
			this.BounceEnabled = false;
			this.scale = 1.2f;
			if (left)
			{
				this.TextureCoord = new Point(66, 59);
			}
			else
			{
				this.TextureCoord = new Point(67, 59);
			}
			this.AffectedByGravity = false;
			if (left)
			{
				this.mod = -1;
			}
			this.ActivateObject(this.Position, this.TextureCoord);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008458 File Offset: 0x00006658
		public override void Update(float elapsed)
		{
			this.angle += elapsed * this.speedMod;
			if (this.angle > 6.2831855f)
			{
				this.angle -= 6.2831855f;
			}
			this.Velocity = Vector3.Zero;
			this.Idle(elapsed);
			this.Slamming(elapsed);
			this.Splitting(elapsed);
			this.Punching(elapsed);
			this.Shaking(elapsed);
			this.UpToSummoning(elapsed);
			this.Summoning(elapsed);
			base.Update(elapsed);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000084DC File Offset: 0x000066DC
		private void Idle(float elapsed)
		{
			if (this.parent.state == EstateDemon.DemonState.Spawning)
			{
				this.Position.Z = this.parent.Position.Z + 2f;
				this.Position.X = this.parent.Position.X + (float)this.mod * ((float)Math.Cos((double)this.angle) * (this.radius * 0.5f)) + (float)this.mod * 2.25f;
				this.Position.Y = this.parent.Position.Y - 1.5f;
				return;
			}
			if (this.state == DemonHand.HandState.Idle)
			{
				float x = this.parent.Position.X;
				float num = 1.5f;
				this.Position.X = x + (float)this.mod * ((float)Math.Cos((double)this.angle) * (this.radius * 0.5f)) + (float)this.mod * 2.25f;
				float num2 = (float)Math.Sin((double)this.angle);
				this.Position.Y = num + num2 * this.radius;
				if (this.Position.Y > 1.5f)
				{
					this.Position.Y = 1.5f - (this.Position.Y - 1.5f);
				}
				this.Position.Z = this.parent.Position.Z + 2f;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00008664 File Offset: 0x00006864
		private void Slamming(float elapsed)
		{
			if (this.state == DemonHand.HandState.Raising)
			{
				this.Position.Y = this.Position.Y + 1.2f * elapsed;
				if (this.Position.Y > 3f)
				{
					this.state = DemonHand.HandState.SlammingDown;
				}
				return;
			}
			if (this.state == DemonHand.HandState.SlammingDown)
			{
				this.Position.Y = this.Position.Y - 6.5f * elapsed;
				if (this.Position.Y < 1f)
				{
					this.state = DemonHand.HandState.Contact;
					this.contactTime = 3f;
					this.HitGround();
					return;
				}
			}
			if (this.state == DemonHand.HandState.Contact)
			{
				this.contactTime -= elapsed;
				if (this.contactTime <= 0f)
				{
					this.state = DemonHand.HandState.GoingIdle;
					return;
				}
			}
			if (this.state == DemonHand.HandState.GoingIdle)
			{
				this.Position.Y = this.Position.Y + 0.8f * elapsed;
				if (this.Position.Y > 1.4f)
				{
					this.state = DemonHand.HandState.Idle;
					this.parent.GoIdle();
				}
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00008764 File Offset: 0x00006964
		private void HitGround()
		{
			Camera.ShakeCamera(1f, 0.2f);
			for (int i = 0; i < 20; i++)
			{
				Vector3 randomPosition = VerchickMath.GetRandomPosition(this.Position, 0.4f);
				Global.MasterCache.CreateParticle(ParticleType.Dirt, randomPosition, Vector3.Zero);
			}
			AOE aoe = new AOE(AOEType.DemonFire, 12f, 6f, this.Position, 1f / this.parent.Mod, null, this.parent);
			AOE aoe2 = aoe;
			aoe2.Position.X = aoe2.Position.X + (float)(2 * this.mod);
			AOE aoe3 = aoe;
			aoe3.Position.Z = aoe3.Position.Z + 3f;
			Global.MasterCache.CreateObject(aoe);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00008818 File Offset: 0x00006A18
		private void Splitting(float elapsed)
		{
			if (this.state == DemonHand.HandState.SplittingOut)
			{
				this.Position.X = this.Position.X + 4f * (float)this.mod * elapsed;
				if (Math.Abs(this.Position.X - this.parent.Position.X) > 4f)
				{
					this.state = DemonHand.HandState.Split;
					this.parent.Breathe();
				}
				return;
			}
			if (this.state == DemonHand.HandState.Split)
			{
				this.splitTime -= elapsed;
				if (this.splitTime <= 0f)
				{
					this.state = DemonHand.HandState.SplittingIn;
				}
			}
			if (this.state == DemonHand.HandState.SplittingIn)
			{
				this.Position.X = this.Position.X - 3f * (float)this.mod * elapsed;
				if (Math.Abs(this.Position.X - this.parent.Position.X) < 2f)
				{
					this.state = DemonHand.HandState.Idle;
					this.parent.GoIdle();
				}
				return;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00008914 File Offset: 0x00006B14
		private void Punching(float elapsed)
		{
			if (this.state == DemonHand.HandState.PunchOut)
			{
				Vector3 randomPosition = VerchickMath.GetRandomPosition(this.Position, 0.4f);
				this.particleTime -= elapsed;
				if (this.particleTime <= 0f)
				{
					Global.MasterCache.CreateParticle(ParticleType.DemonFire, randomPosition, Vector3.Zero);
					this.particleTime = 0.05f;
				}
				this.punchTime -= elapsed;
				this.Position.X = this.Position.X + this.punchDir.X * elapsed * this.SpecialProperties.Speed * 60f;
				this.Position.Z = this.Position.Z + this.punchDir.Y * elapsed * this.SpecialProperties.Speed * 60f;
				if (this.punchTime <= 0f)
				{
					this.punchTime = 1.8f;
					this.state = DemonHand.HandState.PunchIn;
				}
				this.CheckCollisions();
				return;
			}
			if (this.state == DemonHand.HandState.PunchIn)
			{
				this.alreadyCollided = false;
				this.punchTime -= elapsed;
				this.Position.X = this.Position.X - this.punchDir.X * elapsed * this.SpecialProperties.Speed * 20f;
				this.Position.Z = this.Position.Z - this.punchDir.Y * elapsed * this.SpecialProperties.Speed * 20f;
				if (this.punchTime <= 0f)
				{
					this.state = DemonHand.HandState.Idle;
					this.parent.GoIdle();
				}
				return;
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00008AA8 File Offset: 0x00006CA8
		private void UpToSummoning(float elapsed)
		{
			if (this.state == DemonHand.HandState.UpToSummon)
			{
				Vector3 position = this.parent.Position;
				this.Position.Y = this.Position.Y + 3f * elapsed;
				if (this.Position.Y >= 4f)
				{
					this.parent.Summon();
					this.state = DemonHand.HandState.Summoning;
				}
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00008B08 File Offset: 0x00006D08
		private void Summoning(float elapsed)
		{
			if (this.state == DemonHand.HandState.Summoning)
			{
				if (this.mod == -1)
				{
					this.TextureCoord = new Point(66, 62);
				}
				else
				{
					this.TextureCoord = new Point(67, 62);
				}
				this.summonTime -= elapsed;
				if (this.summonTime <= 0f)
				{
					if (this.mod == -1)
					{
						this.TextureCoord = new Point(66, 59);
					}
					else
					{
						this.TextureCoord = new Point(67, 59);
					}
					this.state = DemonHand.HandState.EndSummon;
				}
			}
			if (this.state == DemonHand.HandState.EndSummon)
			{
				this.Position.Y = this.Position.Y - 4f * elapsed;
				if (this.Position.Y <= this.parent.Position.Y)
				{
					this.state = DemonHand.HandState.Idle;
					this.parent.GoIdle();
				}
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00008BE4 File Offset: 0x00006DE4
		private void Shaking(float elapsed)
		{
			if (this.state == DemonHand.HandState.Shaking)
			{
				this.shakeAngle += elapsed * this.speedMod * 1.5f * this.handShakeMod;
				if (this.shakeAngle > 1.0471976f || this.shakeAngle < -1.0471976f)
				{
					this.handShakeMod *= -1f;
				}
				float x = this.parent.Position.X;
				this.Position.X = x + (float)this.mod * ((float)Math.Cos(0.0) * (this.radius * 0.5f)) + (float)this.mod * 2.25f;
				this.Position.Y = 1.5f;
				this.ZRotation = this.shakeAngle;
				this.shakeTime -= elapsed;
				if (this.shakeTime <= 0f)
				{
					if (this.mod == -1)
					{
						this.TextureCoord = new Point(66, 59);
					}
					else
					{
						this.TextureCoord = new Point(67, 59);
					}
					this.ZRotation = 0f;
					this.state = DemonHand.HandState.Idle;
					this.SetDamage();
				}
			}
			if (this.state == DemonHand.HandState.ShakingIdle)
			{
				this.angle += elapsed * this.speedMod * 4f;
				if (this.angle > 6.2831855f)
				{
					this.angle -= 6.2831855f;
				}
				float x2 = this.parent.Position.X;
				this.Position.X = x2 + (float)this.mod * ((float)Math.Cos(0.0) * (this.radius * 0.5f)) + (float)this.mod * 2.25f;
				this.Position.Y = 1.2f;
				this.shakeTime -= elapsed;
				if (this.shakeTime <= 0f)
				{
					this.state = DemonHand.HandState.Idle;
					this.SetDamage();
				}
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008DE3 File Offset: 0x00006FE3
		public void Slam()
		{
			if (this.state == DemonHand.HandState.Idle)
			{
				this.state = DemonHand.HandState.Raising;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008DF5 File Offset: 0x00006FF5
		public void Split(float time)
		{
			if (this.state == DemonHand.HandState.Idle)
			{
				this.state = DemonHand.HandState.SplittingOut;
				this.splitTime = time;
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00008E0E File Offset: 0x0000700E
		public void Punch(GameObject target, float speed)
		{
			if (this.state == DemonHand.HandState.Idle)
			{
				this.punchSpeed = speed;
				this.punchTime = 0.6f;
				this.state = DemonHand.HandState.PunchOut;
				this.punchDir = VerchickMath.DirectionToVector2(base.TwoDPosition(), target.TwoDPosition());
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00008E4C File Offset: 0x0000704C
		public void ShakeIdle(float time)
		{
			float x = this.parent.Position.X;
			float num = 1.5f;
			this.Position.X = x + (float)this.mod * ((float)Math.Cos((double)this.angle) * (this.radius * 0.5f)) + (float)this.mod * 2.25f;
			float num2 = (float)Math.Sin((double)this.angle);
			this.Position.Y = num + num2 * this.radius;
			if (this.Position.Y > 1.5f)
			{
				this.Position.Y = 1.5f - (this.Position.Y - 1.5f);
			}
			this.state = DemonHand.HandState.ShakingIdle;
			this.Position.Z = this.parent.Position.Z + 2f;
			this.shakeTime = time;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00008F34 File Offset: 0x00007134
		public void Shake(float time)
		{
			float x = this.parent.Position.X;
			float num = 1.5f;
			this.Position.X = x + (float)this.mod * ((float)Math.Cos((double)this.angle) * (this.radius * 0.5f)) + (float)this.mod * 2.25f;
			float num2 = (float)Math.Sin((double)this.angle);
			this.Position.Y = num + num2 * this.radius;
			if (this.Position.Y > 1.5f)
			{
				this.Position.Y = 1.5f - (this.Position.Y - 1.5f);
			}
			if (this.mod == -1)
			{
				this.TextureCoord = new Point(66, 60);
			}
			else
			{
				this.TextureCoord = new Point(67, 60);
			}
			this.state = DemonHand.HandState.Shaking;
			this.Position.Z = this.parent.Position.Z + 2f;
			this.shakeTime = time;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00009048 File Offset: 0x00007248
		public void UpToSummon(float time)
		{
			if (this.state == DemonHand.HandState.Idle)
			{
				this.summonTime = time;
				this.state = DemonHand.HandState.UpToSummon;
				float x = this.parent.Position.X;
				float num = 1.5f;
				this.Position.X = x + (float)this.mod * ((float)Math.Cos((double)this.angle) * 0f) + (float)this.mod * 2.25f;
				float num2 = (float)Math.Sin((double)this.angle);
				this.Position.Y = num + num2 * 0f;
				if (this.Position.Y > 1.5f)
				{
					this.Position.Y = 1.5f - (this.Position.Y - 1.5f);
				}
				this.Position.Z = this.parent.Position.Z + 2f;
				if (this.mod == -1)
				{
					this.TextureCoord = new Point(66, 61);
					return;
				}
				this.TextureCoord = new Point(67, 61);
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000915C File Offset: 0x0000735C
		private void CheckCollisions()
		{
			using (List<Player>.Enumerator enumerator = Global.PlayerList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (VerchickMath.WithinDistance(enumerator.Current.TwoDPosition(), base.TwoDPosition(), 0.5f) && !this.alreadyCollided)
					{
						this.alreadyCollided = true;
					}
				}
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000091CC File Offset: 0x000073CC
		private void DoDamage(int amount)
		{
			if (amount > 0)
			{
				for (int i = 0; i < 5; i++)
				{
					Global.MasterCache.CreateParticle(ParticleType.DemonBlood, VerchickMath.GetRandomPosition(this.Position, 0.2f), Vector3.Zero);
				}
			}
			this.hurtTime = 0.15f;
			this.DMG -= amount;
			if (this.DMG <= 0)
			{
				this.parent.Hurt(this.mod == -1);
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00009240 File Offset: 0x00007440
		private void SetDamage()
		{
			this.DMG = (int)((float)this.BaseHandDmg / this.parent.Mod);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void IncreaseStrength(float mod)
		{
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void ChangeMainTex(Point tex, bool scrambleDmg)
		{
		}

		// Token: 0x040000BE RID: 190
		private bool left;

		// Token: 0x040000BF RID: 191
		private int mod = 1;

		// Token: 0x040000C0 RID: 192
		private EstateDemon parent;

		// Token: 0x040000C1 RID: 193
		private float radius;

		// Token: 0x040000C2 RID: 194
		private float angle;

		// Token: 0x040000C3 RID: 195
		private float speedMod = 3f;

		// Token: 0x040000C4 RID: 196
		private float contactTime;

		// Token: 0x040000C5 RID: 197
		private float splitTime;

		// Token: 0x040000C6 RID: 198
		private float punchTime;

		// Token: 0x040000C7 RID: 199
		private float punchSpeed;

		// Token: 0x040000C8 RID: 200
		private float shakeTime;

		// Token: 0x040000C9 RID: 201
		private float shakeAngle;

		// Token: 0x040000CA RID: 202
		private float summonTime;

		// Token: 0x040000CB RID: 203
		private float particleTime;

		// Token: 0x040000CC RID: 204
		private int DMG = 800;

		// Token: 0x040000CD RID: 205
		private int BaseHandDmg = 800;

		// Token: 0x040000CE RID: 206
		private float hurtTime;

		// Token: 0x040000CF RID: 207
		private Vector2 punchDir;

		// Token: 0x040000D0 RID: 208
		private DemonHand.HandState state = DemonHand.HandState.Idle;

		// Token: 0x040000D1 RID: 209
		private float handShakeMod = 1f;

		// Token: 0x040000D2 RID: 210
		private bool alreadyCollided;

		// Token: 0x02000204 RID: 516
		private enum HandState
		{
			// Token: 0x04000DD7 RID: 3543
			Raising,
			// Token: 0x04000DD8 RID: 3544
			SlammingDown,
			// Token: 0x04000DD9 RID: 3545
			Contact,
			// Token: 0x04000DDA RID: 3546
			Idle,
			// Token: 0x04000DDB RID: 3547
			GoingIdle,
			// Token: 0x04000DDC RID: 3548
			SplittingOut,
			// Token: 0x04000DDD RID: 3549
			Split,
			// Token: 0x04000DDE RID: 3550
			SplittingIn,
			// Token: 0x04000DDF RID: 3551
			PunchOut,
			// Token: 0x04000DE0 RID: 3552
			PunchIn,
			// Token: 0x04000DE1 RID: 3553
			Shaking,
			// Token: 0x04000DE2 RID: 3554
			ShakingIdle,
			// Token: 0x04000DE3 RID: 3555
			UpToSummon,
			// Token: 0x04000DE4 RID: 3556
			Summoning,
			// Token: 0x04000DE5 RID: 3557
			EndSummon
		}
	}
}
