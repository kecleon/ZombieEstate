using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;
using ZombieEstate2.Weapons.Bullets;

namespace ZombieEstate2
{
	// Token: 0x02000105 RID: 261
	public class Bullet : DualGameObject
	{
		// Token: 0x06000707 RID: 1799 RVA: 0x00033E00 File Offset: 0x00032000
		public Bullet() : base(new Point(63, 63), new Point(63, 63), new Vector3(0f, 0f, 0f), true)
		{
			this.Active = false;
			this.SpecialProperties = new SpecialProperties();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00033E9C File Offset: 0x0003209C
		public void InitBullet(BulletStats bull, GunStats gun, SpecialProperties ownerProps, SpecialProperties gunProps, Vector2 dir, Vector3 pos, ushort UID, Shootable parent, int lvl, List<BulletModifier> mods, int seed)
		{
			base.UID = UID;
			this.Rand = new Random(seed);
			this.Velocity.X = 0f;
			this.Velocity.Y = 0f;
			this.Velocity.Z = 0f;
			this.Level = lvl;
			this.BullStats = bull;
			this.GunStats = gun;
			this.dontBounceTime = 0.01f;
			this.timeAlive = 0f;
			this.landed = false;
			this.killNextFrame = false;
			this.sideMod = 1f;
			this.sinMod = 0f;
			this.launched = false;
			this.oscillation = 0f;
			this.positive = true;
			this.bounceCount = 0;
			this.healing = false;
			this.Mods.Clear();
			SpecialProperties.ClearProps(ref this.SpecialProperties);
			SpecialProperties.AddUpProps(ref this.SpecialProperties, ownerProps);
			SpecialProperties.AddUpProps(ref this.SpecialProperties, gunProps);
			this.Parent = parent;
			int num = 0;
			if (this.BullStats.RandomFour)
			{
				num = Global.rand.Next(4);
			}
			Point point = new Point(this.BullStats.HorizTexCoord.X, this.BullStats.HorizTexCoord.Y);
			if (point.X != 63 && point.Y != 63)
			{
				point.X += num;
			}
			Point point2 = new Point(this.BullStats.VertTexCoord.X, this.BullStats.VertTexCoord.Y);
			if (point2.X != 63 && point2.Y != 63)
			{
				point2.X += num;
			}
			base.InitSecondary(point2, point, pos);
			this.Speed = this.BullStats.Speed;
			this.Direction = dir;
			if (!gun.FireEvenly)
			{
				float radians = MathHelper.ToRadians(Global.RandomFloat(this.Rand, -gun.GunProperties[this.Level].Accuracy, gun.GunProperties[this.Level].Accuracy));
				Vector3 vector = new Vector3(dir.X, 0f, dir.Y);
				vector = Vector3.Transform(vector, Matrix.CreateRotationY(radians));
				this.Direction.X = vector.X;
				this.Direction.Y = vector.Z;
			}
			this.StartDirection = this.Direction;
			this.Position = pos;
			if (this.BullStats.Scale == 0f)
			{
				this.secondObject.scale = 0.45f;
				this.scale = 0.45f;
			}
			else
			{
				this.secondObject.scale = this.BullStats.Scale;
				this.scale = this.BullStats.Scale;
			}
			if (this.BullStats.StickToOwner)
			{
				this.xScale = 0f;
			}
			this.Active = true;
			this.secondObject.Active = true;
			this.Behaviors = BehaviorParser.LoadBehaviorFromStringList(bull.BehaviorsStrings);
			this.XRotation = 0f;
			this.YRotation = 0f;
			this.ZRotation = 0f;
			this.AffectedByGravity = false;
			this.secondObject.floorHeight = -20f;
			this.secondObject.AffectedByGravity = false;
			this.floorHeight = 0.2f;
			this.sideMod = (float)(this.Rand.NextDouble() - 0.5) / 4f;
			if (this.BullStats.RandomSpeedPercent != 0)
			{
				float num2 = Global.RandomFloat(this.Rand, 0f, (float)this.BullStats.RandomSpeedPercent / 100f);
				if (this.Rand.Next(0, 100) < 50)
				{
					num2 = 1f + num2;
				}
				else
				{
					num2 = 1f - num2;
				}
				this.Speed *= num2;
			}
			this.UpdateRotation();
			this.PrevPosition = this.Position;
			this.Position.X = this.Position.X + this.Direction.X * 0.66f;
			this.Position.Z = this.Position.Z + this.Direction.Y * 0.66f;
			this.Position.Y = 0.3f;
			this.secondObject.Position = this.Position;
			base.UpdateTile();
			this.UpdateLevelCollisions(0.016666668f);
			this.UpdateEnemyCollisions();
			base.SetHorizantleRotation(this.YRotation);
			int num3 = -1;
			if (parent != null && this.tile != null)
			{
				num3 = this.tile.CollidedFromParent(parent.Position);
			}
			if (num3 != -1)
			{
				this.JustHitAWall(num3);
			}
			if (this.GunStats.GunProperties[this.Level].MinDamage < 0)
			{
				this.healing = true;
				this.ActualDamage = this.Rand.Next(-this.GunStats.GunProperties[this.Level].MinDamage, -this.GunStats.GunProperties[this.Level].MaxDamage);
			}
			else if (this.GunStats.GunProperties[this.Level].MinDamage != this.GunStats.GunProperties[this.Level].MaxDamage)
			{
				this.ActualDamage = this.Rand.Next(this.GunStats.GunProperties[this.Level].MinDamage, this.GunStats.GunProperties[this.Level].MaxDamage);
			}
			else
			{
				this.ActualDamage = this.GunStats.GunProperties[this.Level].MaxDamage;
			}
			if (mods.Contains(BulletModifier.Crit))
			{
				this.ActualDamage *= 2;
			}
			this.Mods.AddRange(mods);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00034480 File Offset: 0x00032680
		public override void Update(float elapsed)
		{
			if (elapsed == 0f)
			{
				return;
			}
			this.BoundingRect.X = (int)this.Position.X;
			this.BoundingRect.Y = (int)this.Position.Z;
			if (this.dontBounceTime > 0f)
			{
				this.dontBounceTime -= elapsed;
			}
			this.timeAlive += elapsed;
			if (this.BullStats.LiveTime > 0f && this.timeAlive >= this.BullStats.LiveTime)
			{
				this.DestroyObject();
			}
			if (this.BullStats.StickToOwner)
			{
				Vector3 position = this.Parent.Position;
				this.xScale = this.BullStats.StickToOwnerXScale;
				this.secondObject.xScale = this.BullStats.StickToOwnerXScale;
				if (this.Parent is Player)
				{
					Player player = this.Parent as Player;
					position.X += player.mAimDir.X * (this.BullStats.StickToOwnerXScale / 2f + 0.4f);
					position.Z += player.mAimDir.Y * (this.BullStats.StickToOwnerXScale / 2f + 0.4f);
					this.YRotation = MathHelper.WrapAngle(-VerchickMath.AngleFromVector2(player.mAimDir));
					this.secondObject.YRotation = this.YRotation;
				}
				position.Y = 0.25f;
				this.Position = position;
			}
			else
			{
				this.Movement(elapsed);
			}
			if (base.OnFloor())
			{
				if (this.tile != null && !this.tile.HasFloor())
				{
					this.CollideWithWall();
				}
				if (this.BullStats.LandOnFloor)
				{
					if (!this.landed)
					{
						this.Velocity.X = this.Direction.X * this.Speed;
						this.Velocity.Z = this.Direction.Y * this.Speed;
						this.Speed = 0f;
						this.groundFriction = 30f;
						this.landed = true;
					}
				}
				else
				{
					this.CollideWithWall();
				}
			}
			this.XRotation += this.BullStats.XRotationOverTime * elapsed;
			this.YRotation += this.BullStats.YRotationOverTime * elapsed;
			this.ZRotation += this.BullStats.ZRotationOverTime * elapsed;
			this.secondObject.XRotation += this.BullStats.XRotationOverTime * elapsed;
			this.secondObject.YRotation += this.BullStats.YRotationOverTime * elapsed;
			this.secondObject.ZRotation += this.BullStats.ZRotationOverTime * elapsed;
			base.UpdateTile();
			this.UpdateEnemyCollisions();
			this.UpdatePlayerCollisions();
			if (this.killNextFrame)
			{
				this.DestroyObject();
				return;
			}
			this.UpdateLevelCollisions(elapsed);
			this.UpdateEmitParticles(elapsed);
			this.secondObject.Position = this.Position;
			base.Update(elapsed);
			this.secondObject.Position = this.Position;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000347B4 File Offset: 0x000329B4
		private void Movement(float elapsed)
		{
			this.Behaviors.UpdateBehaviors(elapsed);
			BulletBehavior[] behaviors = this.Behaviors.GetBehaviors();
			float[] percents = this.Behaviors.GetPercents();
			for (int i = 0; i < 4; i++)
			{
				BulletBehavior bulletBehavior = behaviors[i];
				if (bulletBehavior != BulletBehavior.NONE)
				{
					float num = percents[i];
					if (bulletBehavior == BulletBehavior.Straight)
					{
						this.Position.X = this.Position.X + this.Direction.X * elapsed * this.Speed * num;
						this.Position.Z = this.Position.Z + this.Direction.Y * elapsed * this.Speed * num;
					}
					if (bulletBehavior == BulletBehavior.Wiggle)
					{
						float num2 = (float)(this.Rand.NextDouble() * 2.0 - 1.0);
						float num3 = (float)(this.Rand.NextDouble() * 2.0 - 1.0);
						this.Direction.X = this.Direction.X + num2 * num;
						this.Direction.Y = this.Direction.Y + num3 * num;
						this.Direction.Normalize();
						if (this.BullStats.RotatesInDirection)
						{
							this.UpdateRotation();
						}
					}
					if (bulletBehavior == BulletBehavior.AngleSide)
					{
						this.sideMod *= 1.02f;
						Vector3 vector = Vector3.Transform(new Vector3(this.Direction.X, 0f, this.Direction.Y), Matrix.CreateRotationY(this.sideMod * 60f * elapsed));
						this.Direction.X = vector.X;
						this.Direction.Y = vector.Z;
						this.Position.X = this.Position.X + this.Direction.X * elapsed * num * this.Speed;
						this.Position.Z = this.Position.Z + this.Direction.Y * elapsed * num * this.Speed;
					}
					if (bulletBehavior == BulletBehavior.Launch && !this.launched)
					{
						this.launched = true;
						float num4 = 1f;
						if (this.BullStats.RandomSpeedPercent != 0)
						{
							num4 = Global.RandomFloat(this.Rand, 0f, (float)this.BullStats.RandomSpeedPercent / 100f);
							if (Global.Probability(this.Rand, 50))
							{
								num4 = 1f + num4;
							}
							else
							{
								num4 = 1f - num4;
							}
							this.Speed *= num4;
						}
						this.Velocity.Y = num * 4f * num4;
						this.AffectedByGravity = true;
					}
					if (bulletBehavior == BulletBehavior.Wave)
					{
						this.Oscillation(elapsed, num);
						Vector3 vector2 = new Vector3(this.Direction.X, 0f, this.Direction.Y);
						vector2 = Vector3.Transform(new Vector3(this.StartDirection.X, 0f, this.StartDirection.Y), Matrix.CreateRotationY(this.oscillation * num));
						this.Direction.X = vector2.X;
						this.Direction.Y = vector2.Z;
						base.SetHorizantleRotation((float)(-(float)Math.Atan2((double)this.Direction.Y, (double)this.Direction.X) + 3.141592653589793));
						this.Direction.Normalize();
					}
				}
			}
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00034B1C File Offset: 0x00032D1C
		private void UpdateLevelCollisions(float elapsed)
		{
			Vector3 position = this.Position;
			Tile tileAtLocation = Global.Level.GetTileAtLocation(position);
			if (tileAtLocation == null)
			{
				this.DestroyObject();
				return;
			}
			if (!tileAtLocation.HasAnyWalls())
			{
				return;
			}
			int num = tileAtLocation.CollidedBullet(position, this.Direction, this.Speed, elapsed);
			if (num == -1)
			{
				num = tileAtLocation.ShouldIHaveCollidedBulletPreviously(position, this.Direction, this.Speed, elapsed);
				if (num != -1)
				{
					this.Position = this.PrevPosition;
				}
			}
			if (num != -1)
			{
				this.JustHitAWall(num);
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00034B98 File Offset: 0x00032D98
		private void JustHitAWall(int collision)
		{
			if (this.Position.Y > 1f)
			{
				return;
			}
			if (Global.Probability(this.Rand, this.GunStats.GunProperties[this.Level].BounceChance) && this.bounceCount < this.GunStats.GunProperties[this.Level].BounceCount && this.dontBounceTime <= 0f)
			{
				this.bounceCount++;
				if (collision == 0 || collision == 2)
				{
					this.Direction = Vector2.Reflect(this.Direction, new Vector2(1f, 0f));
					this.DeathParticles();
					if (this.hitObjects != null)
					{
						this.hitObjects.Clear();
					}
				}
				else
				{
					this.Direction = Vector2.Reflect(this.Direction, new Vector2(0f, -1f));
					this.DeathParticles();
					if (this.hitObjects != null)
					{
						this.hitObjects.Clear();
					}
				}
				this.Direction.Normalize();
				this.UpdateRotation();
				return;
			}
			this.CollideWithWall();
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00034CBC File Offset: 0x00032EBC
		private void UpdateEnemyCollisions()
		{
			if (this.BullStats.IgnoreEnemies)
			{
				return;
			}
			if (this.tile == null)
			{
				return;
			}
			if (this.Position.Y > 1.75f)
			{
				return;
			}
			List<GameObject> adjacentTileContainedList = this.tile.GetAdjacentTileContainedList();
			this.hitObjs.Clear();
			for (int i = 0; i < adjacentTileContainedList.Count; i++)
			{
				if (this.Active && adjacentTileContainedList[i] != this)
				{
					GameObject gameObject = adjacentTileContainedList[i];
					if (gameObject != null && gameObject is Shootable && !(gameObject is Player))
					{
						GameObject gameObject2 = gameObject;
						float distance = 1f;
						if (this.BullStats.StickToOwner && this.BullStats.StickToOwnerXScale > 3.25f)
						{
							distance = 1.5f;
						}
						if (gameObject2.Active && VerchickMath.WithinDistance(base.TwoDPosition(), gameObject2.TwoDPosition(), distance))
						{
							if (this.BullStats.StickToOwner)
							{
								if (Global.Level.InLineOfSightMelee(this.Parent.Position, gameObject2.Position))
								{
									this.hitObjs.Add(gameObject2);
								}
								else if (VerchickMath.WithinDistance(this.Parent.TwoDPosition(), gameObject2.TwoDPosition(), 0.2f))
								{
									this.hitObjs.Add(gameObject2);
								}
							}
							else if (Global.Level.InLineOfSightMelee(this.Position, gameObject2.Position))
							{
								this.hitObjs.Add(gameObject2);
							}
						}
					}
				}
			}
			GameObject gameObject3 = null;
			float num = float.MaxValue;
			for (int j = 0; j < this.hitObjs.Count; j++)
			{
				if (gameObject3 == null)
				{
					gameObject3 = this.hitObjs[j];
					num = Vector2.DistanceSquared(gameObject3.TwoDPosition(), new Vector2(this.PrevPosition.X, this.PrevPosition.Z));
				}
				else
				{
					float num2 = Vector2.DistanceSquared(this.hitObjs[j].TwoDPosition(), new Vector2(this.PrevPosition.X, this.PrevPosition.Z));
					if (num2 < num)
					{
						gameObject3 = this.hitObjs[j];
						num = num2;
					}
				}
			}
			if (gameObject3 != null)
			{
				if (this.GunStats.GunProperties[this.Level].PenetrationChance > 0)
				{
					if (this.hitObjects == null)
					{
						this.hitObjects = new List<GameObject>();
					}
					if (this.hitObjects.Contains(gameObject3))
					{
						return;
					}
					this.hitObjects.Add(gameObject3);
				}
				Shootable shootable = (Shootable)gameObject3;
				if (!shootable.IgnoreBullets)
				{
					float num3 = 1f;
					if (this.GunStats.GunProperties[this.Level].DamageType == DamageType.Physical)
					{
						num3 = 1f + this.SpecialProperties.BulletDamageMod / 100f;
					}
					if (this.Parent is Minion)
					{
						num3 -= 0.3f;
						if (num3 < 0f)
						{
							num3 = 0f;
						}
					}
					shootable.Damage(this.Parent, (float)this.ActualDamage * num3, this.GunStats.GunProperties[this.Level].DamageType, this.BullStats.NoGore, false, this.Mods);
					if (!string.IsNullOrEmpty(this.GunStats.GunProperties[this.Level].Buff))
					{
						shootable.BuffManager.AddBuff(this.GunStats.GunProperties[this.Level].Buff, this.Parent, this.GunStats.GunProperties[this.Level].BuffArgs);
					}
					Shootable shootable2 = shootable;
					shootable2.Velocity.X = shootable2.Velocity.X + this.Direction.X * this.GunStats.GunProperties[this.Level].PushBack;
					Shootable shootable3 = shootable;
					shootable3.Velocity.Z = shootable3.Velocity.Z + this.Direction.Y * this.GunStats.GunProperties[this.Level].PushBack;
					this.CollideWithEnemy(shootable);
				}
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x000350E0 File Offset: 0x000332E0
		private void UpdatePlayerCollisions()
		{
			if (!this.BullStats.HurtsPlayers)
			{
				return;
			}
			foreach (Player player in Global.PlayerList)
			{
				if (VerchickMath.WithinDistance(base.TwoDPosition(), player.TwoDPosition(), 1f) && this.Position.Y < 1f)
				{
					this.CollideWithPlayer(player);
				}
				for (int i = 0; i < player.GetMinionList.Count; i++)
				{
					Minion minion = player.GetMinionList[i];
					if (VerchickMath.WithinDistance(base.TwoDPosition(), minion.TwoDPosition(), 1f) && this.Position.Y < 1f)
					{
						this.CollideWithPlayer(minion);
					}
				}
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x000351C4 File Offset: 0x000333C4
		private void UpdateEmitParticles(float elapsed)
		{
			if (this.BullStats.EmitPartType != "None" || this.BullStats.EmitPartType != string.Empty)
			{
				this.timeSinceLastEmit += elapsed;
				if (this.timeSinceLastEmit >= this.BullStats.EmitPartTime)
				{
					this.timeSinceLastEmit = 0f;
					if (this.BullStats.EmitPartType == "Smoke")
					{
						Vector3 pos = new Vector3(this.Position.X - this.Direction.X * this.Speed * elapsed, this.Position.Y, this.Position.Z - this.Direction.Y * this.Speed * elapsed);
						Global.MasterCache.CreateParticle(ParticleType.Smoke, pos, Vector3.Zero);
					}
					if (this.BullStats.EmitPartType == "Flare")
					{
						Global.MasterCache.CreateParticle(ParticleType.Flare, new Vector3(this.Position.X + 0.1f, this.Position.Y + 0.5f, this.Position.Z), Vector3.Zero);
					}
					if (this.BullStats.EmitPartType == "BigSmoke")
					{
						Vector3 pos2 = new Vector3(this.Position.X - this.Direction.X * this.Speed * elapsed, this.Position.Y, this.Position.Z - this.Direction.Y * this.Speed * elapsed);
						Global.MasterCache.CreateParticle(ParticleType.BigSmoke, pos2, Vector3.Zero);
					}
					if (this.BullStats.EmitPartType == "MiniDemonSpawn" && Global.Probability(this.Rand, 20))
					{
						Global.MasterCache.CreateParticle(ParticleType.DemonFire, this.Position, Vector3.Zero);
					}
					if (this.BullStats.EmitPartType == "Rainbow")
					{
						Global.MasterCache.CreateParticle(ParticleType.Rainbow, this.Position, Vector3.Zero);
					}
					if (this.BullStats.EmitPartType == "Magic")
					{
						Global.MasterCache.CreateParticle(ParticleType.Magic, this.Position, -this.Velocity);
					}
					if (this.BullStats.EmitPartType == "Confetti")
					{
						Global.MasterCache.CreateParticle(ParticleType.Confetti, this.Position, -this.Velocity);
					}
					if (this.BullStats.EmitPartType == "Heal")
					{
						Global.MasterCache.CreateParticle(ParticleType.Heal, this.Position, -this.Velocity);
					}
				}
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00035488 File Offset: 0x00033688
		public virtual void CollideWithWall()
		{
			this.DeathParticles();
			this.killNextFrame = true;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00035498 File Offset: 0x00033698
		private void DeathParticles()
		{
			if (this.BullStats.DeathParticleType == " " || string.IsNullOrEmpty(this.BullStats.DeathParticleType) || this.BullStats.DeathParticleType == "None")
			{
				return;
			}
			if (this.BullStats.DeathParticleType == "Sparks")
			{
				for (int i = 0; i < 5; i++)
				{
					Global.MasterCache.CreateParticle(ParticleType.Spark, this.Position, this.Velocity);
				}
				return;
			}
			if (this.BullStats.DeathParticleType == "Snow")
			{
				for (int j = 0; j < 5; j++)
				{
					Global.MasterCache.CreateParticle(ParticleType.Snow, this.Position, this.Velocity);
				}
				return;
			}
			if (this.BullStats.DeathParticleType == "Rainbow")
			{
				for (int k = 0; k < 5; k++)
				{
					Global.MasterCache.CreateParticle(ParticleType.Rainbow, VerchickMath.GetRandomPosition(this.Position, 1f), this.Velocity);
				}
				return;
			}
			if (this.BullStats.DeathParticleType == "LaserSparks")
			{
				for (int l = 0; l < 5; l++)
				{
					Global.MasterCache.CreateParticle(ParticleType.LaserSpark, VerchickMath.GetRandomPosition(this.Position, 0.2f), this.Velocity);
				}
				return;
			}
			if (this.BullStats.DeathParticleType == "Magic")
			{
				for (int m = 0; m < 5; m++)
				{
					Global.MasterCache.CreateParticle(ParticleType.Magic, VerchickMath.GetRandomPosition(this.Position, 0.2f), this.Velocity);
				}
				return;
			}
			Terminal.WriteMessage("Error: Unknown death part type |" + this.BullStats.DeathParticleType + "|", MessageType.ERROR);
			Console.WriteLine("Error: Unknown death part type |" + this.BullStats.DeathParticleType + "|");
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00035671 File Offset: 0x00033871
		public virtual void CollideWithEnemy(Shootable target)
		{
			if (!Global.Probability(this.Rand, this.GunStats.GunProperties[this.Level].PenetrationChance))
			{
				this.DestroyObject();
			}
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x000356A4 File Offset: 0x000338A4
		public virtual void CollideWithPlayer(Player player)
		{
			if (!this.BullStats.HurtsPlayers)
			{
				return;
			}
			if (this.healing)
			{
				player.Heal((float)this.ActualDamage, this.Parent, false);
				if (this.Mods.Contains(BulletModifier.Crit))
				{
					Global.MasterCache.particleSystem.AddCombatText(CombatTextType.HealCrit, Bullet.healCrit, new Vector3(player.Position.X, player.Position.Y + 1f, player.Position.Z));
				}
				if (!string.IsNullOrEmpty(this.GunStats.GunProperties[this.Level].Buff))
				{
					player.BuffManager.AddBuff(this.GunStats.GunProperties[this.Level].Buff, this.Parent, this.GunStats.GunProperties[this.Level].BuffArgs);
				}
			}
			else
			{
				if (this.ActualDamage != 0)
				{
					player.Damage(this.Parent, (float)this.ActualDamage, this.GunStats.GunProperties[this.Level].DamageType, this.BullStats.NoGore, false, null);
				}
				if (!string.IsNullOrEmpty(this.GunStats.GunProperties[this.Level].Buff))
				{
					player.BuffManager.AddBuff(this.GunStats.GunProperties[this.Level].Buff, this.Parent, this.GunStats.GunProperties[this.Level].BuffArgs);
				}
			}
			this.DestroyObject();
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00035850 File Offset: 0x00033A50
		public override void DestroyObject()
		{
			if (this.hitObjects != null)
			{
				this.hitObjects.Clear();
				this.hitObjects = null;
			}
			if (this.GunStats != null)
			{
				if (!string.IsNullOrEmpty(this.GunStats.GunProperties[this.Level].Explosion) && this.GunStats.GunProperties[this.Level].Explosion != "None")
				{
					Explosion.CreateExplosion(this.GunStats.GunProperties[this.Level].ExplosionRadius, (float)this.GunStats.GunProperties[this.Level].ExplosionDamage, (float)this.GunStats.GunProperties[this.Level].ExplosionPushBack, this.GunStats.GunProperties[this.Level].Explosion, this.Position, this.Parent, false);
					if (base.IAmOwnedByLocalPlayer)
					{
						NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.BulletDestroyed);
						netMessage.Payload = new Msg_BulletDestroyed
						{
							BulletUID = base.UID,
							BulletX = this.Position.X,
							BulletZ = this.Position.Z
						};
						NetworkManager.SendMessage(netMessage, SendType.Reliable);
					}
				}
				this.OnDestroySPECIAL();
			}
			base.DestroyObject();
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x000359B0 File Offset: 0x00033BB0
		private void OnDestroySPECIAL()
		{
			if (this.GunStats == null)
			{
				return;
			}
			if (this.GunStats.GunName == "Nuke Gun")
			{
				Nuke obj = new Nuke(this.Position + new Vector3(0f, 25f, 0f), this.Position, this.Parent);
				Global.MasterCache.CreateObject(obj);
				SoundEngine.PlaySound("ze2_airraid", this.Position, -0.3f, -0.25f);
				return;
			}
			if (this.GunStats.GunName == "Orbital Strike Cannon")
			{
				Orbital obj2 = new Orbital(this.Position, this.Direction, this.Parent);
				Global.MasterCache.CreateObject(obj2);
				return;
			}
			if (this.GunStats.GunName == "Demon Hand")
			{
				for (int i = 0; i < 100; i++)
				{
					Vector3 vector;
					vector.X = Global.RandomFloat(this.Rand, 1f, 31f);
					vector.Y = Global.RandomFloat(this.Rand, 100f, 200f);
					vector.Z = Global.RandomFloat(this.Rand, 1f, 31f);
					float hangTime = Global.RandomFloat(this.Rand, 0f, 15f);
					Vector3 endPos = new Vector3(vector.X, 0f, vector.Z);
					Meteor obj3 = new Meteor(vector, endPos, this.Parent, hangTime);
					Global.MasterCache.CreateObject(obj3);
				}
				Camera.ZoomOut(20f, 1.75f);
				return;
			}
			if (this.GunStats.GunName == "Mega Bubble")
			{
				for (int j = 0; j < 100; j++)
				{
					Vector3 vector2;
					vector2.X = Global.RandomFloat(this.Rand, 1f, 31f);
					vector2.Y = Global.RandomFloat(this.Rand, 10f, 30f);
					vector2.Z = Global.RandomFloat(this.Rand, 1f, 31f);
					Vector3 endPos2 = new Vector3(vector2.X, 0f, vector2.Z);
					MegaBubble obj4 = new MegaBubble(vector2, endPos2, this.Parent);
					Global.MasterCache.CreateObject(obj4);
				}
				Camera.ZoomOut(12f, 1.75f);
				return;
			}
			if (this.GunStats.GunName == "Comet Staff")
			{
				float num = 0.75f;
				for (int k = 0; k < 3; k++)
				{
					Vector3 vector3 = new Vector3(this.Parent.Position.X, 10f + (float)(k * 2), this.Parent.Position.Z);
					vector3.X += this.StartDirection.X * (2f * (float)(k + 1));
					vector3.Z += this.StartDirection.Y * (2f * (float)(k + 1));
					Vector3 vector4 = new Vector3(vector3.X, 0f, vector3.Z);
					for (int l = 0; l < 3; l++)
					{
						Global.MasterCache.CreateParticle(ParticleType.Magic, VerchickMath.GetRandomPosition(vector4 + new Vector3(0f, 0.25f, 0f), 0.25f), new Vector3(Global.RandomFloat(-num, num), Global.RandomFloat(0f, num), Global.RandomFloat(-num, num)));
					}
					Comet obj5 = new Comet(vector3, vector4, this.Parent, this.GunStats.GunProperties[this.Level].ExplosionRadius, (float)this.GunStats.GunProperties[this.Level].ExplosionDamage, this.Level);
					Global.MasterCache.CreateObject(obj5);
				}
				return;
			}
			if (this.GunStats.GunName == "Air Strike")
			{
				int num2 = (this.Level >= 2) ? 5 : 3;
				for (int m = 0; m < num2; m++)
				{
					Vector3 vector5 = new Vector3(this.Parent.Position.X, 20f + (float)(m * 3), this.Parent.Position.Z);
					vector5.X += this.StartDirection.X * (2f * (float)(m + 1));
					vector5.Z += this.StartDirection.Y * (2f * (float)(m + 1));
					Vector3 vector6 = new Vector3(vector5.X, 0f, vector5.Z);
					Global.MasterCache.CreateParticle(ParticleType.Target, vector6 + new Vector3(0f, 0.25f, 0f), Vector3.Zero);
					Missile obj6 = new Missile(vector5, vector6, this.Parent, this.GunStats.GunProperties[this.Level].ExplosionRadius, (float)this.GunStats.GunProperties[this.Level].ExplosionDamage, this.Level);
					Global.MasterCache.CreateObject(obj6);
				}
				return;
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00035F00 File Offset: 0x00034100
		private void Oscillation(float elapsed, float mod)
		{
			if (this.positive)
			{
				this.oscillation += 3f * elapsed / Math.Max(mod, 1f);
				if (this.oscillation >= 0.5f)
				{
					this.positive = false;
					return;
				}
			}
			else
			{
				this.oscillation -= 3f * elapsed / Math.Max(mod, 1f);
				if (this.oscillation <= -0.5f)
				{
					this.positive = true;
				}
			}
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00035F80 File Offset: 0x00034180
		private void UpdateRotation()
		{
			if (this.BullStats.RotatesInDirection)
			{
				this.YRotation = (float)(-(float)Math.Atan2((double)this.Direction.Y, (double)this.Direction.X) + 3.141592653589793);
			}
			else
			{
				this.YRotation = 0f;
			}
			if (this.BullStats.RandomZRotation)
			{
				this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
				this.secondObject.ZRotation = this.ZRotation;
			}
			base.SetHorizantleRotation(this.YRotation);
		}

		// Token: 0x040006EC RID: 1772
		private BulletStats BullStats;

		// Token: 0x040006ED RID: 1773
		private SpecialProperties SpecialProperties;

		// Token: 0x040006EE RID: 1774
		private GunStats GunStats;

		// Token: 0x040006EF RID: 1775
		private Vector2 Direction;

		// Token: 0x040006F0 RID: 1776
		private Vector2 StartDirection;

		// Token: 0x040006F1 RID: 1777
		private Random Rand = new Random();

		// Token: 0x040006F2 RID: 1778
		private Shootable Parent;

		// Token: 0x040006F3 RID: 1779
		private Behavior Behaviors;

		// Token: 0x040006F4 RID: 1780
		private Rectangle BoundingRect = new Rectangle(0, 0, 1, 1);

		// Token: 0x040006F5 RID: 1781
		private float Speed;

		// Token: 0x040006F6 RID: 1782
		private int ActualDamage;

		// Token: 0x040006F7 RID: 1783
		private int Level;

		// Token: 0x040006F8 RID: 1784
		private float dontBounceTime = 0.01f;

		// Token: 0x040006F9 RID: 1785
		private float timeAlive;

		// Token: 0x040006FA RID: 1786
		private bool landed;

		// Token: 0x040006FB RID: 1787
		private bool killNextFrame;

		// Token: 0x040006FC RID: 1788
		private float sideMod = 1f;

		// Token: 0x040006FD RID: 1789
		private float sinMod;

		// Token: 0x040006FE RID: 1790
		private bool launched;

		// Token: 0x040006FF RID: 1791
		private bool positive = true;

		// Token: 0x04000700 RID: 1792
		private float oscillation;

		// Token: 0x04000701 RID: 1793
		private float timeSinceLastEmit;

		// Token: 0x04000702 RID: 1794
		private int bounceCount;

		// Token: 0x04000703 RID: 1795
		private bool healing;

		// Token: 0x04000704 RID: 1796
		private List<GameObject> hitObjects;

		// Token: 0x04000705 RID: 1797
		private List<BulletModifier> Mods = new List<BulletModifier>();

		// Token: 0x04000706 RID: 1798
		private List<GameObject> hitObjs = new List<GameObject>();

		// Token: 0x04000707 RID: 1799
		private static string healCrit = "*HEAL CRIT*";
	}
}
