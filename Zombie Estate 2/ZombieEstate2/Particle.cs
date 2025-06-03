using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200011B RID: 283
	internal class Particle : GameObject
	{
		// Token: 0x060007B8 RID: 1976 RVA: 0x0003C95C File Offset: 0x0003AB5C
		public Particle(ParticleType partType, Vector3 pos)
		{
			this.type = partType;
			this.airFriction = 2f;
			this.floorHeight = (float)Global.rand.NextDouble() * 0.1f;
			if (this.type == ParticleType.None)
			{
				return;
			}
			switch (this.type)
			{
			case ParticleType.Blood:
			{
				Point texCoord = new Point(Global.rand.Next(0, 5), 31);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				this.ActivateObject(pos, texCoord);
				float num = (float)Global.rand.NextDouble() * 4f - 2f;
				float num2 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num3 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num2 * 1f;
				this.Velocity.X = num * 1f;
				this.Velocity.Z = num3 * 1f;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				break;
			}
			case ParticleType.Fire:
			{
				Point texCoord2 = new Point(Global.rand.Next(0, 5), 29);
				this.ActivateObject(pos, texCoord2);
				float x = (float)Global.rand.NextDouble() * 4f - 2f;
				float y = (float)Global.rand.NextDouble() * 6f + 3f;
				float z = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = y;
				this.Velocity.X = x;
				this.Velocity.Z = z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				break;
			}
			case ParticleType.NormalZombieGibblet:
			{
				Point texCoord3 = new Point(Global.rand.Next(0, 5), 29);
				this.scale = 1f;
				this.ActivateObject(pos, texCoord3);
				float x2 = (float)Global.rand.NextDouble() * 4f - 2f;
				float y2 = (float)Global.rand.NextDouble() * 6f + 3f;
				float z2 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = y2;
				this.Velocity.X = x2;
				this.Velocity.Z = z2;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				break;
			}
			}
			this.startScale = this.scale;
			this.totalTimer = (float)Global.rand.Next(6, 8);
			this.destroyTimer = -1f;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0003CC3C File Offset: 0x0003AE3C
		public override void Update(float elapsed)
		{
			if (this.destroyTimer >= this.totalTimer)
			{
				base.DestroyObject();
				base.Update(elapsed);
			}
			if (Global.OutsideMap(this.Position))
			{
				base.DestroyObject();
				base.Update(elapsed);
			}
			if (this.destroyTimer >= 0f)
			{
				this.destroyTimer += elapsed;
				if (this.destroyTimer > this.shrinkTime && this.totalTimer - this.shrinkTime != 0f)
				{
					this.scale = MathHelper.Lerp(this.scale, 0f, (this.destroyTimer - this.shrinkTime) / (this.totalTimer - this.shrinkTime));
				}
			}
			if (this.scale < 0f)
			{
				this.scale = 0f;
			}
			if (this.lands && this.destroyTimer == -1f && this.Position.Y <= this.floorHeight + 0.1f && this.Velocity.Y < 0f)
			{
				this.DestroyObject();
			}
			if ((this.type == ParticleType.AmmoPickup || this.type == ParticleType.Dirt_Short) && base.OnFloor())
			{
				base.DestroyObject();
			}
			if ((this.type == ParticleType.Spark || this.type == ParticleType.Snow) && (base.OnFloor() || this.Position.Y < -10f))
			{
				if (!this.bounced)
				{
					this.Position.Y = this.Position.Y + 0.02f;
					float num = (float)Global.rand.NextDouble() * 4f - 2f;
					float num2 = (float)Global.rand.NextDouble() * 6f + 3f;
					float num3 = (float)Global.rand.NextDouble() * 4f - 2f;
					this.Velocity.Y = num2 * 0.5f;
					this.Velocity.X = num * 0.8f;
					this.Velocity.Z = num3 * 0.8f;
					if (Global.Probability(85))
					{
						this.bounced = true;
					}
				}
				else
				{
					base.DestroyObject();
				}
			}
			if (this.type == ParticleType.Target)
			{
				this.YRotation += 3.5f * elapsed;
			}
			if (this.type == ParticleType.Confetti2 && !base.OnFloor())
			{
				this.ZRotation += this.ConfettiRotSpeed * elapsed;
			}
			if (this.type == ParticleType.HitEffect)
			{
				int x = (int)MathHelper.Lerp(15f, 19f, this.destroyTimer / this.totalTimer);
				this.TextureCoord.X = x;
			}
			ParticleType particleType = this.type;
			if ((this.type == ParticleType.Smoke || this.type == ParticleType.BigSmoke) && this.scale > 0.1f)
			{
				this.scale -= 1f * elapsed;
			}
			if (this.type == ParticleType.Flare && this.scale > 0.1f)
			{
				this.scale -= 0.15f * elapsed;
			}
			if ((this.type == ParticleType.GasPotent || this.type == ParticleType.Gas) && this.scale > 0.1f)
			{
				this.scale -= 0.3f * elapsed;
			}
			if (this.type == ParticleType.SkwugTele)
			{
				if (this.scale > 0.1f)
				{
					this.scale -= 0.1f * elapsed;
				}
				this.ZRotation += 0.18f;
			}
			base.Update(elapsed);
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0003CFB0 File Offset: 0x0003B1B0
		public override void DestroyObject()
		{
			this.destroyTimer = 0f;
			this.XRotation = -1.5707964f;
			if (this.type != ParticleType.NormalZombieGibblet || this.type != ParticleType.BrainGibblet || this.type != ParticleType.SkeletonGibblet || this.type != ParticleType.SecondNormalZombieGibblet)
			{
				this.groundFriction = 20f;
				return;
			}
			this.airFriction = 8f;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0003D010 File Offset: 0x0003B210
		public void SetUp(ParticleType partType, Vector3 pos, Vector3 vel)
		{
			this.ConfettiSin = 0f;
			this.ConfettiRotSpeed = 0f;
			this.maxFallSpeed = -12f;
			this.XRotation = 0f;
			this.destroyTimer = -1f;
			this.totalTimer = 1f;
			this.type = partType;
			this.floorHeight = (float)Global.rand.NextDouble() * 0.1f;
			if (this.type == ParticleType.None)
			{
				return;
			}
			this.bounced = false;
			this.AffectedByGravity = true;
			this.totalTimer = (float)Global.rand.Next(4, 8);
			this.shrinkTime = this.totalTimer * 0.75f;
			this.destroyTimer = -1f;
			this.XRotation = 0f;
			this.YRotation = 0f;
			this.ZRotation = 0f;
			this.Velocity.X = 0f;
			this.Velocity.Y = 0f;
			this.Velocity.Z = 0f;
			this.airFriction = 2f;
			switch (this.type)
			{
			case ParticleType.Blood:
			{
				Point texCoord = new Point(Global.rand.Next(0, 4), 31);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				this.ActivateObject(pos, texCoord);
				float num = (float)Global.rand.NextDouble() * 4f - 2f;
				float num2 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num3 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num2 * 1f + vel.Y;
				this.Velocity.X = num * 1f + vel.X;
				this.Velocity.Z = num3 * 1f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.Fire:
			{
				Point texCoord2 = new Point(13 + Global.rand.Next(0, 4), 27);
				this.ActivateObject(pos, texCoord2);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				float x = (float)Global.rand.NextDouble() * 2f - 1f;
				Global.rand.NextDouble();
				float z = (float)Global.rand.NextDouble() * 2f - 1f;
				this.Position.Y = this.Position.Y - 0.4f;
				this.floorHeight = 0f;
				this.Velocity.Y = Global.RandomFloat(1.4f, 1.8f);
				this.Velocity.X = x;
				this.Velocity.Z = z;
				this.Velocity += vel;
				this.AffectedByGravity = false;
				this.ZRotation = Global.RandomFloat(-0.7853982f, 0.7853982f);
				this.destroyTimer = 0.1f;
				this.totalTimer = 1f;
				this.airFriction = 0f;
				this.lands = false;
				break;
			}
			case ParticleType.NormalZombieGibblet:
			{
				Point texCoord3 = new Point(Global.rand.Next(56, 60), 5);
				this.scale = 0.4f;
				this.ActivateObject(pos, texCoord3);
				float num4 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num5 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num6 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num5 + vel.Y;
				this.Velocity.X = num4 + vel.X;
				this.Velocity.Z = num6 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.BrainGibblet:
			{
				Point texCoord4 = new Point(Global.rand.Next(60, 64), 23);
				this.scale = 0.5f;
				this.ActivateObject(pos, texCoord4);
				float num7 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num8 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num9 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num8 + vel.Y;
				this.Velocity.X = num7 + vel.X;
				this.Velocity.Z = num9 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.type = ParticleType.NormalZombieGibblet;
				this.lands = true;
				break;
			}
			case ParticleType.MummyBlood:
			{
				Point texCoord5 = new Point(Global.rand.Next(4, 8), 31);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				this.ActivateObject(pos, texCoord5);
				float num10 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num11 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num12 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num11 * 1f + vel.Y;
				this.Velocity.X = num10 * 1f + vel.X;
				this.Velocity.Z = num12 * 1f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.FemaleZombieGibblet:
			{
				Point texCoord6 = new Point(Global.rand.Next(60, 64), 11);
				this.scale = 0.4f;
				this.ActivateObject(pos, texCoord6);
				float num13 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num14 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num15 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num14 + vel.Y;
				this.Velocity.X = num13 + vel.X;
				this.Velocity.Z = num15 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.type = ParticleType.NormalZombieGibblet;
				this.lands = true;
				break;
			}
			case ParticleType.Spark:
			{
				Point texCoord7 = new Point(Global.rand.Next(0, 4), 25);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.15f;
				this.ActivateObject(pos, texCoord7);
				float num16 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num17 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num18 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num17 * 0.5f - vel.Y;
				this.Velocity.X = num16 * 3f - vel.X;
				this.Velocity.Z = num18 * 3f - vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				break;
			}
			case ParticleType.Snow:
			{
				pos.Y += 0.1f;
				Point texCoord8 = new Point(Global.rand.Next(0, 4), 24);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.15f;
				this.ActivateObject(pos, texCoord8);
				float num19 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num20 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num21 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num20 * 0.25f - vel.Y;
				this.Velocity.X = num19 * 1f - vel.X;
				this.Velocity.Z = num21 * 1f - vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				break;
			}
			case ParticleType.SecondNormalZombieGibblet:
			{
				Point texCoord9 = new Point(Global.rand.Next(60, 64), 5);
				this.scale = 0.4f;
				this.ActivateObject(pos, texCoord9);
				float num22 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num23 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num24 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num23 + vel.Y;
				this.Velocity.X = num22 + vel.X;
				this.Velocity.Z = num24 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.Dirt:
			{
				Point texCoord10 = new Point(Global.rand.Next(0, 4) + 4, 31);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				this.ActivateObject(pos, texCoord10);
				float num25 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num26 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num27 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num26 * 0.6f + vel.Y;
				this.Velocity.X = num25 * 1.2f + vel.X;
				this.Velocity.Z = num27 * 1.2f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.AmmoPickup:
			{
				Point texCoord11 = new Point(Global.rand.Next(4, 8), 30);
				this.scale = (float)Global.rand.NextDouble() * 0.15f + 0.25f;
				this.ActivateObject(pos, texCoord11);
				float num28 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num29 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num30 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num29 * 1f + vel.Y;
				this.Velocity.X = num28 * 1.3f + vel.X;
				this.Velocity.Z = num30 * 1.3f + vel.Z;
				break;
			}
			case ParticleType.Smoke:
			{
				Point texCoord12 = new Point(Global.rand.Next(1, 3), 51);
				this.ActivateObject(pos, texCoord12);
				this.scale = (float)Global.rand.NextDouble() * 0.15f + 0.35f;
				float x2 = (float)Global.rand.NextDouble() * 2f - 1f;
				Global.rand.NextDouble();
				float z2 = (float)Global.rand.NextDouble() * 2f - 1f;
				this.floorHeight = 0f;
				this.Velocity.Y = Global.RandomFloat(1.2f, 1.6f);
				this.Velocity.X = x2;
				this.Velocity.Z = z2;
				this.AffectedByGravity = false;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.destroyTimer = 0.15f + Global.RandomFloat(0f, 0.2f);
				this.totalTimer = 0.6f;
				this.lands = false;
				this.airFriction = 0f;
				break;
			}
			case ParticleType.SkeletonGibblet:
			{
				Point texCoord13 = new Point(Global.rand.Next(56, 60), 11);
				this.scale = 0.45f;
				this.ActivateObject(pos, texCoord13);
				float num31 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num32 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num33 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num32 + vel.Y;
				this.Velocity.X = num31 + vel.X;
				this.Velocity.Z = num33 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.SkeletonBlood:
			{
				Point texCoord14 = new Point(Global.rand.Next(8, 12), 30);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				this.ActivateObject(pos, texCoord14);
				float num34 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num35 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num36 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num35 * 1f + vel.Y;
				this.Velocity.X = num34 * 1f + vel.X;
				this.Velocity.Z = num36 * 1f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.BigSmoke:
			{
				Point texCoord15 = new Point(Global.rand.Next(1, 2), 51);
				this.ActivateObject(pos, texCoord15);
				this.scale = (float)Global.rand.NextDouble() * 0.25f + 0.45f;
				float x3 = (float)Global.rand.NextDouble() * 2f - 1f;
				Global.rand.NextDouble();
				float z3 = (float)Global.rand.NextDouble() * 2f - 1f;
				this.floorHeight = 0f;
				this.Velocity.Y = Global.RandomFloat(1.2f, 1.6f);
				this.Velocity.X = x3;
				this.Velocity.Z = z3;
				this.AffectedByGravity = false;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.destroyTimer = 0.15f + Global.RandomFloat(0f, 0.2f);
				this.lands = false;
				this.totalTimer = 0.6f;
				this.airFriction = 0f;
				break;
			}
			case ParticleType.Heal:
			{
				Point texCoord16 = new Point(Global.rand.Next(8, 12), 25);
				this.ActivateObject(pos, texCoord16);
				this.scale = 0.35f;
				Global.rand.NextDouble();
				Global.rand.NextDouble();
				Global.rand.NextDouble();
				this.floorHeight = 0f;
				this.Velocity.Y = Global.RandomFloat(1.2f, 1.8f);
				this.Velocity.X = 0f;
				this.Velocity.Z = 0f;
				this.ZRotation = 0f;
				this.AffectedByGravity = false;
				this.destroyTimer = 0.15f + Global.RandomFloat(0f, 0.2f);
				this.totalTimer = Global.RandomFloat(0.8f, 1.2f);
				this.lands = false;
				this.airFriction = 0f;
				break;
			}
			case ParticleType.Flare:
			{
				Point texCoord17 = new Point(Global.rand.Next(1, 3), 52);
				this.ActivateObject(pos, texCoord17);
				this.scale = (float)Global.rand.NextDouble() * 0.15f + 0.35f;
				float x4 = (float)Global.rand.NextDouble() * 2f - 1f + 1.5f;
				Global.rand.NextDouble();
				float z4 = (float)Global.rand.NextDouble() * 2f - 1f;
				this.floorHeight = 0f;
				this.Velocity.Y = Global.RandomFloat(1.2f, 1.6f);
				this.Velocity.X = x4;
				this.Velocity.Z = z4;
				this.AffectedByGravity = false;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.destroyTimer = 0.15f + Global.RandomFloat(0f, 0.2f);
				this.totalTimer = 4.6f;
				this.lands = false;
				this.airFriction = 0f;
				break;
			}
			case ParticleType.DemonBlood:
			{
				Point texCoord18 = new Point(Global.rand.Next(0, 4) + 8, 31);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				this.ActivateObject(pos, texCoord18);
				float num37 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num38 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num39 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num38 * 1f + vel.Y;
				this.Velocity.X = num37 * 1f + vel.X;
				this.Velocity.Z = num39 * 1f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.DemonGibblet:
			{
				Point texCoord19 = new Point(Global.rand.Next(52, 56), 11);
				this.scale = 0.6f;
				this.ActivateObject(pos, texCoord19);
				float num40 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num41 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num42 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num41 + vel.Y;
				this.Velocity.X = num40 + vel.X;
				this.Velocity.Z = num42 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.type = ParticleType.NormalZombieGibblet;
				this.lands = true;
				break;
			}
			case ParticleType.DemonFire:
			{
				Point texCoord20 = new Point(Global.rand.Next(8, 12), 29);
				this.ActivateObject(pos, texCoord20);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.35f;
				float x5 = (float)Global.rand.NextDouble() * 2f - 1f;
				Global.rand.NextDouble();
				float z5 = (float)Global.rand.NextDouble() * 2f - 1f;
				this.Position.Y = this.Position.Y - 0.4f;
				this.floorHeight = 0f;
				this.Velocity.Y = Global.RandomFloat(0.4f, 0.6f);
				this.Velocity.X = x5;
				this.Velocity.Z = z5;
				this.AffectedByGravity = false;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.destroyTimer = 0.1f;
				this.totalTimer = 1.2f;
				this.airFriction = 0f;
				this.lands = false;
				break;
			}
			case ParticleType.Sludge:
			{
				Point texCoord21 = new Point(Global.rand.Next(0, 3) + 9, 23);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.3f;
				this.ActivateObject(pos, texCoord21);
				float num43 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num44 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num45 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num44 * 1f + vel.Y;
				this.Velocity.X = num43 * 1f + vel.X;
				this.Velocity.Z = num45 * 1f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.SludgeGibblet:
			{
				Point texCoord22 = new Point(Global.rand.Next(60, 64), 17);
				this.scale = 0.7f;
				this.ActivateObject(pos, texCoord22);
				float num46 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num47 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num48 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num47 + vel.Y;
				this.Velocity.X = num46 + vel.X;
				this.Velocity.Z = num48 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.type = ParticleType.NormalZombieGibblet;
				this.lands = true;
				break;
			}
			case ParticleType.HazmatGibblet:
			{
				Point texCoord23 = new Point(Global.rand.Next(56, 60), 17);
				this.scale = 0.4f;
				this.ActivateObject(pos, texCoord23);
				float num49 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num50 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num51 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num50 + vel.Y;
				this.Velocity.X = num49 + vel.X;
				this.Velocity.Z = num51 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.GhostGoo:
			{
				Point texCoord24 = new Point(Global.rand.Next(0, 4) + 12, 31);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				this.ActivateObject(pos, texCoord24);
				float num52 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num53 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num54 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num53 * 1f + vel.Y;
				this.Velocity.X = num52 * 1f + vel.X;
				this.Velocity.Z = num54 * 1f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.GhostGibblet:
			{
				Point texCoord25 = new Point(Global.rand.Next(52, 56), 17);
				this.scale = 0.4f;
				this.ActivateObject(pos, texCoord25);
				float num55 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num56 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num57 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num56 + vel.Y;
				this.Velocity.X = num55 + vel.X;
				this.Velocity.Z = num57 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.GoliathGibblet:
			{
				Point texCoord26 = new Point(Global.rand.Next(56, 60), 23);
				this.scale = 0.8f;
				this.ActivateObject(pos, texCoord26);
				float num58 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num59 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num60 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num59 + vel.Y;
				this.Velocity.X = num58 + vel.X;
				this.Velocity.Z = num60 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.Rainbow:
			{
				Point texCoord27 = new Point(9, 32);
				this.ActivateObject(pos, texCoord27);
				this.scale = 0.5f;
				this.floorHeight = 0f;
				this.AffectedByGravity = false;
				this.destroyTimer = 0.85f + Global.RandomFloat(0f, 0.2f);
				this.totalTimer = 1.6f;
				this.airFriction = 0f;
				break;
			}
			case ParticleType.LaserSpark:
			{
				pos.Y += 0.02f;
				Point texCoord28 = new Point(Global.rand.Next(0, 2) + 12, 29);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.2f;
				this.ActivateObject(pos, texCoord28);
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.XRotation = Global.RandomFloat(0f, 6.2831855f);
				this.YRotation = Global.RandomFloat(0f, 6.2831855f);
				float x6 = (float)Global.rand.NextDouble() * 4f - 2f;
				float y = (float)Global.rand.NextDouble() * 4f - 2f;
				float z6 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = y;
				this.Velocity.X = x6;
				this.Velocity.Z = z6;
				this.AffectedByGravity = false;
				this.destroyTimer = 0f;
				this.totalTimer = 0.6f;
				break;
			}
			case ParticleType.MiniGunBullets:
			{
				Point texCoord29 = new Point(0, 30);
				this.scale = 0.25f;
				this.ActivateObject(pos, texCoord29);
				float num61 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num62 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num63 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num62 * 0.6f + vel.Y;
				this.Velocity.X = num61 * 1.2f + vel.X;
				this.Velocity.Z = num63 * 1.2f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.SkwugTele:
			{
				Point texCoord30 = new Point(8, 7);
				this.ActivateObject(pos, texCoord30);
				this.scale = 0.5f;
				this.floorHeight = 0f;
				this.ZRotation = 1.3f;
				this.AffectedByGravity = false;
				this.destroyTimer = 0.85f + Global.RandomFloat(0f, 0.2f);
				this.totalTimer = 2f;
				this.airFriction = 0f;
				break;
			}
			case ParticleType.SpentShells:
			{
				Point texCoord31 = new Point(0, 33);
				this.scale = 0.15f;
				this.ActivateObject(pos, texCoord31);
				float num64 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num65 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num66 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num65 * 0.6f + vel.Y;
				this.Velocity.X = num64 * 1.2f + vel.X;
				this.Velocity.Z = num66 * 1.2f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.HealWard:
			{
				Point texCoord32 = new Point(Global.rand.Next(8, 12), 25);
				this.ActivateObject(pos, texCoord32);
				this.scale = 0.35f;
				Global.rand.NextDouble();
				Global.rand.NextDouble();
				Global.rand.NextDouble();
				this.floorHeight = 0.01f;
				this.Position.Y = 0.01f;
				this.Velocity.Y = 0f;
				this.Velocity.X = 0f;
				this.Velocity.Z = 0f;
				this.XRotation = 1.5707964f;
				this.AffectedByGravity = false;
				this.destroyTimer = 0.15f + Global.RandomFloat(0f, 0.2f);
				this.totalTimer = Global.RandomFloat(0.8f, 1.2f);
				this.lands = true;
				this.airFriction = 0f;
				this.AffectedByGravity = true;
				break;
			}
			case ParticleType.Magic:
			{
				Point texCoord33 = new Point(Global.rand.Next(0, 4) + 2, 53);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.2f;
				this.ActivateObject(pos, texCoord33);
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.XRotation = Global.RandomFloat(0f, 6.2831855f);
				this.YRotation = Global.RandomFloat(0f, 6.2831855f);
				float x7 = (float)Global.rand.NextDouble() * 1f - 0.5f;
				float y2 = (float)Global.rand.NextDouble() * 1f - 0.5f;
				float z7 = (float)Global.rand.NextDouble() * 1f - 0.5f;
				this.Velocity.Y = y2;
				this.Velocity.X = x7;
				this.Velocity.Z = z7;
				this.AffectedByGravity = false;
				this.destroyTimer = 0f;
				this.totalTimer = 0.6f;
				break;
			}
			case ParticleType.DrZombieGibblet:
			{
				Point texCoord34 = new Point(Global.rand.Next(68, 72), 17);
				this.scale = 0.4f;
				this.ActivateObject(pos, texCoord34);
				float num67 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num68 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num69 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num68 + vel.Y;
				this.Velocity.X = num67 + vel.X;
				this.Velocity.Z = num69 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.BlobBlood:
			{
				Point texCoord35 = new Point(20 + Global.rand.Next(0, 4), 31);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				this.ActivateObject(pos, texCoord35);
				float num70 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num71 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num72 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num71 * 1f + vel.Y;
				this.Velocity.X = num70 * 1f + vel.X;
				this.Velocity.Z = num72 * 1f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.BlobGibblet:
			{
				Point texCoord36 = new Point(Global.rand.Next(68, 72), 29);
				this.scale = 0.25f;
				this.ActivateObject(pos, texCoord36);
				float num73 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num74 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num75 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num74 + vel.Y;
				this.Velocity.X = num73 + vel.X;
				this.Velocity.Z = num75 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.Confetti:
			{
				Point texCoord37 = new Point(Global.rand.Next(0, 4), 30);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				this.ActivateObject(pos, texCoord37);
				float num76 = (float)Global.rand.NextDouble() * 6f - 3f;
				float num77 = (float)Global.rand.NextDouble() * 8f + 3f;
				float num78 = (float)Global.rand.NextDouble() * 6f - 3f;
				this.Velocity.Y = num77 * 1f + vel.Y;
				this.Velocity.X = num76 * 1f + vel.X;
				this.Velocity.Z = num78 * 1f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.ClownGibblet:
			{
				Point texCoord38 = new Point(Global.rand.Next(52, 56), 5);
				this.scale = 0.4f;
				this.ActivateObject(pos, texCoord38);
				float num79 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num80 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num81 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num80 + vel.Y;
				this.Velocity.X = num79 + vel.X;
				this.Velocity.Z = num81 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.Gas:
			{
				Point texCoord39 = new Point(7 + Global.rand.Next(1, 3), 52);
				this.ActivateObject(pos, texCoord39);
				this.scale = (float)Global.rand.NextDouble() * 0.15f + 0.35f;
				float x8 = (float)Global.rand.NextDouble() * 2f - 1f + 1.5f;
				Global.rand.NextDouble();
				float z8 = (float)Global.rand.NextDouble() * 2f - 1f;
				this.floorHeight = 0f;
				this.Velocity.Y = Global.RandomFloat(1.2f, 1.6f);
				this.Velocity.X = x8;
				this.Velocity.Z = z8;
				this.AffectedByGravity = false;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.destroyTimer = 0.15f + Global.RandomFloat(0f, 0.2f);
				this.totalTimer = 1.2f;
				this.lands = false;
				this.airFriction = 0f;
				break;
			}
			case ParticleType.GasPotent:
			{
				Point texCoord40 = new Point(13 + Global.rand.Next(1, 3), 52);
				this.ActivateObject(pos, texCoord40);
				this.scale = (float)Global.rand.NextDouble() * 0.15f + 0.35f;
				float x9 = (float)Global.rand.NextDouble() * 2f - 1f + 1.5f;
				Global.rand.NextDouble();
				float z9 = (float)Global.rand.NextDouble() * 2f - 1f;
				this.floorHeight = 0f;
				this.Velocity.Y = Global.RandomFloat(1.2f, 1.6f);
				this.Velocity.X = x9;
				this.Velocity.Z = z9;
				this.AffectedByGravity = false;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.destroyTimer = 0.15f + Global.RandomFloat(0f, 0.2f);
				this.totalTimer = 1.2f;
				this.lands = false;
				this.airFriction = 0f;
				break;
			}
			case ParticleType.Bug:
			{
				Point texCoord41 = new Point(13, 26);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.45f;
				this.ActivateObject(pos, texCoord41);
				this.Velocity = vel;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.XRotation = Global.RandomFloat(0f, 6.2831855f);
				this.YRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = false;
				this.floorHeight = 0f;
				this.destroyTimer = 0.5f;
				this.totalTimer = 1.6f;
				this.airFriction = 0f;
				this.AffectedByGravity = false;
				break;
			}
			case ParticleType.Dirt_Short:
			{
				Point texCoord42 = new Point(Global.rand.Next(0, 4) + 4, 31);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				this.ActivateObject(pos, texCoord42);
				float num82 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num83 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num84 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num83 * 0.6f + vel.Y;
				this.Velocity.X = num82 * 1.2f + vel.X;
				this.Velocity.Z = num84 * 1.2f + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.FireWitchGibblet:
			{
				Point texCoord43 = new Point(Global.rand.Next(56, 60), 29);
				this.scale = 0.4f;
				this.ActivateObject(pos, texCoord43);
				float num85 = (float)Global.rand.NextDouble() * 4f - 2f;
				float num86 = (float)Global.rand.NextDouble() * 6f + 3f;
				float num87 = (float)Global.rand.NextDouble() * 4f - 2f;
				this.Velocity.Y = num86 + vel.Y;
				this.Velocity.X = num85 + vel.X;
				this.Velocity.Z = num87 + vel.Z;
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				break;
			}
			case ParticleType.Confetti2:
			{
				Point texCoord44 = new Point(Global.rand.Next(0, 4), 30);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.15f;
				this.ConfettiRotSpeed = (float)Global.rand.NextDouble() * 20f - 10f;
				this.ActivateObject(pos, texCoord44);
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.lands = true;
				this.totalTimer = 6f;
				this.shrinkTime = this.totalTimer * 0.75f;
				this.airFriction = 8f;
				this.maxFallSpeed = -1f;
				break;
			}
			case ParticleType.HitEffect:
			{
				pos.Y += 0.02f;
				Point texCoord45 = new Point(15, 8);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.25f;
				this.ActivateObject(pos, texCoord45);
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.AffectedByGravity = false;
				this.destroyTimer = 0f;
				this.totalTimer = 0.2f;
				break;
			}
			case ParticleType.SpeedBoost:
			{
				Point texCoord46 = new Point(Global.rand.Next(0, 4), 25);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.15f;
				this.ActivateObject(pos, texCoord46);
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.AffectedByGravity = false;
				this.destroyTimer = 0f;
				this.totalTimer = 1f;
				break;
			}
			case ParticleType.GuardBoost:
			{
				Point texCoord47 = new Point(67, 38);
				this.scale = 0.125f;
				this.ActivateObject(pos, texCoord47);
				this.AffectedByGravity = false;
				this.Velocity.Y = 1f;
				this.destroyTimer = 0f;
				this.totalTimer = 0.5f;
				break;
			}
			case ParticleType.DmgBoost:
			{
				Point texCoord48 = new Point(8 + Global.rand.Next(0, 4), 27);
				this.scale = (float)Global.rand.NextDouble() * 0.35f + 0.15f;
				this.ActivateObject(pos, texCoord48);
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.AffectedByGravity = false;
				this.Velocity.Y = 1f;
				this.destroyTimer = 0f;
				this.totalTimer = 1f;
				break;
			}
			case ParticleType.CritBoost:
			{
				Point texCoord49 = new Point(67, 40);
				this.scale = 0.125f;
				this.ActivateObject(pos, texCoord49);
				this.AffectedByGravity = false;
				this.Velocity.Y = 1f;
				this.destroyTimer = 0f;
				this.totalTimer = 0.5f;
				break;
			}
			case ParticleType.Target:
			{
				Point texCoord50 = new Point(10, 21);
				this.scale = 0.4f;
				this.ActivateObject(pos, texCoord50);
				this.XRotation = 1.5707964f;
				this.AffectedByGravity = false;
				this.totalTimer = 4f;
				this.destroyTimer = 0f;
				this.lands = false;
				break;
			}
			}
			this.startScale = this.scale;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0003FC08 File Offset: 0x0003DE08
		public override void Landed()
		{
			if (this.lands)
			{
				this.tile = Global.Level.GetTileAtLocation(this.Position);
				if (this.tile != null && !this.tile.HasFloor())
				{
					base.DestroyObject();
				}
			}
			base.Landed();
		}

		// Token: 0x0400086B RID: 2155
		private ParticleType type;

		// Token: 0x0400086C RID: 2156
		private float startScale;

		// Token: 0x0400086D RID: 2157
		private float destroyTimer = -1f;

		// Token: 0x0400086E RID: 2158
		private float totalTimer = 1f;

		// Token: 0x0400086F RID: 2159
		private bool bounced;

		// Token: 0x04000870 RID: 2160
		private bool lands;

		// Token: 0x04000871 RID: 2161
		private float shrinkTime = 0.5f;

		// Token: 0x04000872 RID: 2162
		private float ConfettiRotSpeed;

		// Token: 0x04000873 RID: 2163
		private float ConfettiSin;
	}
}
