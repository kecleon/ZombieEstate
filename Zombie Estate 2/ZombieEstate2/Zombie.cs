using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x0200013F RID: 319
	public class Zombie : Shootable
	{
		// Token: 0x06000956 RID: 2390 RVA: 0x0004B268 File Offset: 0x00049468
		public Zombie(ZombieType type)
		{
			this.Rand = new Random();
			this.zombieType = type;
			this.groundFriction = 20f;
			if (!Global.NETWORKED)
			{
				this.InitiateZombie();
				this.startingTex = this.TextureCoord;
			}
			this.BounceEnabled = true;
			this.movement = new Vector2(0f, 0f);
			this.statusEffects = new StatusEffect[6];
			this.Setup();
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0004B3E5 File Offset: 0x000495E5
		public void InitiateNet(ushort uid, ushort dropUID, int seed)
		{
			this.Rand = new Random(seed);
			base.UID = uid;
			this.mSeed = seed;
			this.GenerateDropPercent(dropUID);
			this.InitiateAllRandomness();
			this.startAttack = this.attackDamage;
			this.SomethingChanged = true;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0004B421 File Offset: 0x00049621
		protected virtual void InitiateAllRandomness()
		{
			this.mDropRandomSeed = this.Rand.Next();
			this.InitiateZombie();
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0004B43C File Offset: 0x0004963C
		private void InitiateZombie()
		{
			this.EngageDistance = 6f;
			if (this.zombieType == ZombieType.NormalZombie)
			{
				this.scale = 0.4f;
				if (Global.Probability(this.Rand, 33))
				{
					this.TextureCoord.X = 56;
					this.TextureCoord.Y = 0;
					this.GibbletType = ParticleType.NormalZombieGibblet;
				}
				else if (Global.Probability(this.Rand, 50))
				{
					this.TextureCoord.X = 60;
					this.TextureCoord.Y = 0;
					this.GibbletType = ParticleType.SecondNormalZombieGibblet;
				}
				else
				{
					this.TextureCoord.X = 60;
					this.TextureCoord.Y = 6;
					this.GibbletType = ParticleType.FemaleZombieGibblet;
				}
				this.InitSpeed(Global.RandomFloat(this.Rand, 0.75f, 1.1f));
				this.SpecialProperties.MaxHealth = 100f * Global.ZombieHealthMod;
				this.startingTex = this.TextureCoord;
				this.Worth = 2f;
			}
			if (this.zombieType == ZombieType.Skeleton)
			{
				this.scale = 0.45f;
				this.TextureCoord.X = 56;
				this.TextureCoord.Y = 6;
				this.GibbletType = ParticleType.SkeletonGibblet;
				this.BloodType = ParticleType.SkeletonBlood;
				this.InitSpeed(Global.RandomFloat(this.Rand, 0.8f, 1.1f));
				this.attackDamage = 5;
				this.topAttackCooldown = 3f;
				this.leapSpeed = 5f;
				this.range = 2.85f;
				this.SpecialProperties.MaxHealth = 65f * Global.ZombieHealthMod;
				this.Health = 65f;
				this.Worth = 10f;
				this.startingTex = this.TextureCoord;
			}
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0004B5F0 File Offset: 0x000497F0
		public override void InitBaseSpecialProperties()
		{
			if (this.zombieType == ZombieType.Skeleton)
			{
				this.BaseSpecialProperties = new SpecialProperties();
				this.BaseSpecialProperties.MaxHealth = 100f;
				this.BaseSpecialProperties.Speed = 0f;
				this.BaseSpecialProperties.Armor = 0;
				this.BaseSpecialProperties.LifeStealPercent = 0f;
			}
			else
			{
				this.BaseSpecialProperties = new SpecialProperties();
				this.BaseSpecialProperties.MaxHealth = 90f;
				this.BaseSpecialProperties.Speed = 0f;
				this.BaseSpecialProperties.Armor = 0;
				this.BaseSpecialProperties.LifeStealPercent = 0f;
			}
			this.SomethingChanged = true;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0004B69D File Offset: 0x0004989D
		public virtual void Setup()
		{
			Global.ZombieList.Add(this);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0004B6AC File Offset: 0x000498AC
		public override void Update(float elapsed)
		{
			if (this.NOLOGIC)
			{
				this.BoundingRect.X = (int)this.Position.X;
				this.BoundingRect.Y = (int)this.Position.Z;
				base.UpdateTile();
				base.Update(elapsed);
				return;
			}
			if (this.Health <= 0f || this.mKilled)
			{
				this.DestroyObject();
			}
			if (this.LiveSpeedMod < this.MaxLiveSpeedMod)
			{
				this.LiveSpeedMod += this.LiveSpeedModStep * elapsed;
			}
			this.DeAggroTimer -= elapsed;
			if (this.DeAggroTimer <= 0f)
			{
				this.DeAggroTick();
				this.DeAggroTimer = this.TotalDeAggroTime;
			}
			this.BoundingRect.X = (int)this.Position.X;
			this.BoundingRect.Y = (int)this.Position.Z;
			if (this.spawning && !this.mKilled)
			{
				this.Spawn(elapsed);
				base.Update(elapsed);
				return;
			}
			if (this.TimeToEngage > 0f)
			{
				this.TimeToEngage -= elapsed;
			}
			else
			{
				this.TimeToEngage = 0f;
			}
			if (this.JustLeaped > 0f)
			{
				this.JustLeaped -= elapsed;
				if (this.JustLeaped < 0f)
				{
					this.JustLeaped = 0f;
				}
				if (this.Target != null && this.Target is Shootable && VerchickMath.WithinDistance(base.TwoDPosition(), this.Target.TwoDPosition(), this.range * 0.8f))
				{
					(this.Target as Shootable).Damage(this, (float)this.attackDamage * (1f + this.SpecialProperties.BulletDamageMod), DamageType.Physical, false, false, null);
					this.JustLeaped = 0f;
				}
			}
			if (this.attackCooldown > 0f)
			{
				this.attackCooldown -= elapsed;
			}
			if (this.attackCooldown < 0f)
			{
				this.attackCooldown = 0f;
				this.UpdateDirection();
			}
			if (this.targTile == null || this.targTile == this.tile || this.prevTile != this.tile || this.tile == null)
			{
				this.UpdateDirection();
			}
			this.UpdateEffects(elapsed);
			this.Position.X = this.Position.X + this.movement.X * elapsed * this.speedMod * this.SpecialProperties.Speed * this.LiveSpeedMod;
			this.Position.Z = this.Position.Z + this.movement.Y * elapsed * this.speedMod * this.SpecialProperties.Speed * this.LiveSpeedMod;
			this.speedMod = 1f;
			this.UpdateFacing();
			base.UpdateTile();
			this.PickTarget();
			if (this.ShouldIAttack())
			{
				this.Attack();
			}
			base.Update(elapsed);
			if (this.tile == null)
			{
				this.Position = this.PrevPosition;
				return;
			}
			if (this.tile.HasAnyWalls() && this.tile.CollidedWithWall(this.Position, 1f) != -1)
			{
				this.Position = this.PrevPosition;
				this.Velocity.X = -this.Velocity.X / 2f;
				this.Velocity.Z = -this.Velocity.Z / 2f;
				this.targTile = null;
				return;
			}
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0004BA20 File Offset: 0x00049C20
		private void UpdateEffects(float elapsed)
		{
			if (this.statusEffects != null)
			{
				for (int i = 0; i < this.statusEffects.Length; i++)
				{
					if (this.statusEffects[i] != null)
					{
						if (this.statusEffects[i].Type == "None")
						{
							this.statusEffects[i].Clear();
							this.statusEffects[i] = null;
						}
						else
						{
							this.statusEffects[i].Update(elapsed);
						}
					}
				}
			}
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0004BA94 File Offset: 0x00049C94
		private void ClearEffects()
		{
			if (this.statusEffects != null)
			{
				for (int i = 0; i < this.statusEffects.Length; i++)
				{
					if (this.statusEffects[i] != null)
					{
						this.statusEffects[i].Clear();
						this.statusEffects[i] = null;
					}
				}
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0004BADC File Offset: 0x00049CDC
		public override void DestroyObject()
		{
			this.BuffManager.ParentKilled();
			this.ClearEffects();
			Global.ZombieList.Remove(this);
			base.DestroyObject();
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0004BB01 File Offset: 0x00049D01
		public override void BaseDestroyObject()
		{
			this.BuffManager.ParentKilled();
			base.BaseDestroyObject();
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0004BB14 File Offset: 0x00049D14
		public override void Killed(Shootable attacker, bool fromNet)
		{
			this.Kill(attacker, fromNet);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0004BB20 File Offset: 0x00049D20
		private void Kill(Shootable attacker, bool fromNet)
		{
			if (this.mKilled)
			{
				return;
			}
			this.Health = 0f;
			this.mKilled = true;
			this.ClearEffects();
			this.Drops();
			this.BuffManager.ParentKilled();
			if (attacker != null && !attacker.IgnoreAggro && this.tile != null)
			{
				foreach (GameObject gameObject in this.tile.AdjacentObjects)
				{
					if (gameObject is Zombie)
					{
						Zombie zombie = gameObject as Zombie;
						if (attacker is Minion)
						{
							zombie.BuildAggro(attacker as Player, 40);
						}
						else
						{
							zombie.BuildAggro(attacker as Player, 10);
						}
					}
				}
			}
			if (Global.WaveMaster.CurrentWave != null && !this.DontCountAsKill)
			{
				Global.WaveMaster.CurrentWave.Kills++;
			}
			int num = 6 + Global.rand.Next(0, 4);
			for (int i = 0; i < num; i++)
			{
				Global.MasterCache.CreateParticle(this.BloodType, this.Position, this.Velocity);
			}
			if (Global.Probability(this.GibbletChance))
			{
				Global.MasterCache.CreateParticle(this.GibbletType, this.Position, this.Velocity);
			}
			if (!fromNet)
			{
				NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.ZombieKilled);
				Msg_ZombieKilled msg_ZombieKilled = new Msg_ZombieKilled();
				msg_ZombieKilled.UID = base.UID;
				if (attacker != null)
				{
					msg_ZombieKilled.KillerUID = attacker.UID;
				}
				netMessage.Payload = msg_ZombieKilled;
				NetworkManager.SendMessage(netMessage, SendType.Reliable);
			}
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0004BCB8 File Offset: 0x00049EB8
		public virtual void UpdateFacing()
		{
			if (this.spawning)
			{
				this.facing = Facing.Down;
			}
			else if (this.attackCooldown == 0f || base.OnFloor())
			{
				if (Math.Abs(this.movement.X) > Math.Abs(this.movement.Y))
				{
					if (this.movement.X < 0f)
					{
						this.facing = Facing.Left;
					}
					else
					{
						this.facing = Facing.Right;
					}
				}
				else if (this.movement.Y > 0f)
				{
					this.facing = Facing.Down;
				}
				else
				{
					this.facing = Facing.Up;
				}
			}
			if (this.facing == Facing.Left)
			{
				this.TextureCoord.X = this.startingTex.X + 2 * (int)this.TexScale;
			}
			if (this.facing == Facing.Down)
			{
				this.TextureCoord.X = this.startingTex.X + 0 * (int)this.TexScale;
			}
			if (this.facing == Facing.Up)
			{
				this.TextureCoord.X = this.startingTex.X + 3 * (int)this.TexScale;
			}
			if (this.facing == Facing.Right)
			{
				this.TextureCoord.X = this.startingTex.X + 1 * (int)this.TexScale;
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0004BDF8 File Offset: 0x00049FF8
		public override void Damage(Shootable attacker, float amount, DamageType damageType, bool noGore, bool AOE, List<BulletModifier> mods = null)
		{
			base.Damage(attacker, amount, damageType, noGore, AOE, mods);
			if (!noGore)
			{
				Global.MasterCache.CreateParticle(this.BloodType, this.Position, this.Velocity);
			}
			if (attacker != null && !attacker.IgnoreAggro)
			{
				if (attacker is Minion)
				{
					this.BuildAggro(attacker as Player, 15);
				}
				else
				{
					this.BuildAggro(attacker as Player, 5);
				}
			}
			if (!this.NODAMAGE)
			{
				if (!this.ProgressiveDamage)
				{
					if (this.mDmgTexOffset == 0 && this.Health < (float)((int)(this.SpecialProperties.MaxHealth * 0.75f)) && this.Health > 0f)
					{
						this.mDmgTexOffset = Global.rand.Next(1, 5);
						Global.MasterCache.CreateParticle(this.BloodType, this.Position, this.Velocity);
						Global.MasterCache.CreateParticle(this.BloodType, this.Position, this.Velocity);
						this.TextureCoord.Y = this.TextureCoord.Y + this.mDmgTexOffset;
						return;
					}
				}
				else
				{
					if (this.Health < (float)((int)(this.SpecialProperties.MaxHealth * 0.8f)) && this.Health > 0f)
					{
						this.mDmgTexOffset = 1;
						this.TextureCoord.Y = this.startingTex.Y + this.mDmgTexOffset;
					}
					if (this.Health < (float)((int)(this.SpecialProperties.MaxHealth * 0.6f)) && this.Health > 0f)
					{
						this.mDmgTexOffset = 2;
						this.TextureCoord.Y = this.startingTex.Y + this.mDmgTexOffset;
					}
					if (this.Health < (float)((int)(this.SpecialProperties.MaxHealth * 0.4f)) && this.Health > 0f)
					{
						this.mDmgTexOffset = 3;
						this.TextureCoord.Y = this.startingTex.Y + this.mDmgTexOffset;
					}
					if (this.Health < (float)((int)(this.SpecialProperties.MaxHealth * 0.2f)) && this.Health > 0f)
					{
						this.mDmgTexOffset = 4;
						this.TextureCoord.Y = this.startingTex.Y + this.mDmgTexOffset;
					}
				}
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0004C044 File Offset: 0x0004A244
		public virtual void ChangeMainTex(Point tex, bool scrambleDmg)
		{
			if (scrambleDmg)
			{
				this.mDmgTexOffset = Global.rand.Next(1, 5);
				this.NODAMAGE = true;
			}
			this.GibbletChance = 0;
			this.startingTex = tex;
			this.UpdateFacing();
			this.TextureCoord.Y = this.startingTex.Y + this.mDmgTexOffset;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0004C0A0 File Offset: 0x0004A2A0
		public virtual void UpdateDirection()
		{
			if (NetworkManager.AmIHost)
			{
				this.targTile = this.UpdatePathing();
				if (this.targTile != null)
				{
					if (this.targTile.TouchingTiles == null)
					{
						Terminal.WriteMessage("ERROR: TouchingTiles list was null... Zombie_UpdateDir", MessageType.ERROR);
						return;
					}
					this.targTile = this.targTile.TouchingTiles[this.Rand.Next(0, this.targTile.TouchingTiles.Count)];
					if (!this.PreciseDirection && Global.Probability(this.Rand, 50))
					{
						this.targTile = this.targTile.TouchingTiles[this.Rand.Next(0, this.targTile.TouchingTiles.Count)];
					}
					this.MoveToPosition = new Vector2((float)this.targTile.x + 0.5f, (float)this.targTile.y + 0.5f);
					this.movement = VerchickMath.DirectionToVector2(new Vector2(this.Position.X, this.Position.Z), this.MoveToPosition);
					this.movement.Normalize();
					if (this.mTimeSinceLastSentSync < this.mTotalSyncTime / 2f)
					{
						base.Sync();
					}
				}
			}
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0004C1E4 File Offset: 0x0004A3E4
		private Tile UpdatePathing()
		{
			if (this.Target == null)
			{
				return this.tile;
			}
			Tile tileAtLocation = Global.Level.GetTileAtLocation(this.Target.Position);
			if (this.tile != null)
			{
				int num = this.tile.Directions[tileAtLocation.x, tileAtLocation.y];
				if (num == 10)
				{
					return this.tile;
				}
				switch (num)
				{
				case 0:
					return Global.Level.GetTile(this.tile.x + 1, this.tile.y);
				case 1:
					return Global.Level.GetTile(this.tile.x + 1, this.tile.y - 1);
				case 2:
					return Global.Level.GetTile(this.tile.x, this.tile.y - 1);
				case 3:
					return Global.Level.GetTile(this.tile.x - 1, this.tile.y - 1);
				case 4:
					return Global.Level.GetTile(this.tile.x - 1, this.tile.y);
				case 5:
					return Global.Level.GetTile(this.tile.x - 1, this.tile.y + 1);
				case 6:
					return Global.Level.GetTile(this.tile.x, this.tile.y + 1);
				case 7:
					return Global.Level.GetTile(this.tile.x + 1, this.tile.y + 1);
				}
			}
			return this.tile;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0004C39C File Offset: 0x0004A59C
		public virtual void Drops()
		{
			Random random = new Random(this.mDropRandomSeed);
			if (Global.DropsList.Count > 100)
			{
				return;
			}
			int killsToWin = Global.WaveMaster.CurrentWave.KillsToWin;
			float num = MathHelper.Lerp(0.5f, 1.44f, (float)killsToWin / 300f);
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			if (num > 1.5f)
			{
				num = 1.5f;
			}
			float num2 = 0.8f * Zombie.PercentMod[this.zombieType] * num;
			if (num2 < 0.55f)
			{
				num2 = 0.55f;
			}
			if (num2 > 0.94f)
			{
				num2 = 0.94f;
			}
			if (this.mDropPercent <= num2)
			{
				return;
			}
			int num3 = random.Next(0, 1050);
			if (num3 <= 540)
			{
				int num4 = this.dropAmmoType;
				AmmoType type = AmmoType.ASSAULT;
				switch (num4)
				{
				case 0:
					type = AmmoType.ASSAULT;
					break;
				case 1:
					type = AmmoType.EXPLOSIVE;
					break;
				case 2:
					type = AmmoType.HEAVY;
					break;
				case 3:
					type = AmmoType.SHELLS;
					break;
				}
				new AmmoDrop(this.Position, type).UID = this.DropUID;
				return;
			}
			if (num3 <= 945)
			{
				new MoneyDrop(this.Position).UID = this.DropUID;
				return;
			}
			if (num3 <= 965)
			{
				Console.WriteLine("MEGA MONEY DROPPED");
				new MegaMoneyDrop(this.Position).UID = this.DropUID;
				return;
			}
			if (num3 <= 985)
			{
				Console.WriteLine("POWER UP DROPPED");
				new PowerUpDrop(this.Position, this.dropAmmoType).UID = this.DropUID;
				return;
			}
			if (num3 <= 1050)
			{
				new HealthDrop(this.Position).UID = this.DropUID;
				return;
			}
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0004C548 File Offset: 0x0004A748
		public virtual bool ShouldIAttack()
		{
			return this.attackCooldown <= 0f && this.SpecialProperties.Speed != 0f && this.BuffManager.SpeedMod() != 0f && this.Target != null && (VerchickMath.WithinDistance(this.Target.TwoDPosition(), base.TwoDPosition(), this.range) && Global.Level.InLineOfSight(this.Position, this.Target.Position));
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0004C5D4 File Offset: 0x0004A7D4
		public virtual void Attack()
		{
			if (base.OnFloor())
			{
				this.NormalLeap();
				if (this.Target == null)
				{
					return;
				}
				NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.ZombieAttack);
				netMessage.Payload = new Msg_ZombieAttack
				{
					PositionX = this.Position.X,
					PositionZ = this.Position.Z,
					UID = base.UID,
					TargetUID = this.Target.UID
				};
				NetworkManager.SendMessage(netMessage, SendType.Reliable);
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0004C654 File Offset: 0x0004A854
		public virtual void Spawn(float elapsed)
		{
			this.elapsedSpawnTime += elapsed;
			if (this.elapsedSpawnTime >= 0.1f)
			{
				if (Global.Probability(80))
				{
					Global.MasterCache.CreateParticle(ParticleType.Dirt, this.Position, Vector3.Zero);
				}
				this.elapsedSpawnTime = 0f;
			}
			this.AffectedByGravity = false;
			this.Position.Y = this.Position.Y + this.spawnSpeed * elapsed;
			if (this.Position.Y >= this.floorHeight)
			{
				this.spawning = false;
				this.AffectedByGravity = true;
				this.mDoISync = true;
			}
			this.UpdateFacing();
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0004C6F4 File Offset: 0x0004A8F4
		public void InitSpeed(float spd)
		{
			this.BaseSpecialProperties.Speed = spd;
			this.defSpeed = spd;
			this.defSpeed = this.BaseSpecialProperties.Speed;
			this.BounceSpeed = 1f / (this.BaseSpecialProperties.Speed * 8f);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0004C744 File Offset: 0x0004A944
		public virtual void PickTarget()
		{
			if (this.Target is Player && ((this.Target as Player).DEAD || !this.Target.Active))
			{
				this.Target = null;
			}
			Player highestAggro = this.GetHighestAggro();
			if (highestAggro != null)
			{
				this.Target = highestAggro;
				return;
			}
			if (this.Target == null || !this.Target.Active || !VerchickMath.WithinDistance(base.TwoDPosition(), this.Target.TwoDPosition(), this.dismissDistance))
			{
				float num = this.EngageDistance;
				if (this.TimeToEngage <= 0f)
				{
					num = float.MaxValue;
				}
				foreach (Player player in Global.PlayerList)
				{
					float num2 = Vector2.Distance(player.TwoDPosition(), base.TwoDPosition());
					if (num2 < num && !player.DEAD)
					{
						this.Target = player;
						num = num2;
						this.TimeToEngage = 0f;
					}
				}
				if (Global.WaveMaster.CurrentWave != null && Global.WaveMaster.WaveActive)
				{
					if (Global.WaveMaster.CurrentWave.Objectives.Count == 0)
					{
						return;
					}
					foreach (ObjectiveObject objectiveObject in Global.WaveMaster.CurrentWave.Objectives)
					{
						float num3 = Vector2.Distance(objectiveObject.TwoDPosition(), base.TwoDPosition());
						if (num3 < num && objectiveObject.Active)
						{
							this.Target = objectiveObject;
							num = num3;
							this.TimeToEngage = 0f;
						}
					}
				}
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0002F92B File Offset: 0x0002DB2B
		public override void Landed()
		{
			base.SquishMe(1.2f);
			base.Landed();
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0004C90C File Offset: 0x0004AB0C
		public virtual void IncreaseStrength(float mod)
		{
			this.InitSpeed(this.defSpeed * Math.Min((mod - 1f) * 0.25f + 1f, 1.75f));
			this.SpecialProperties.MaxHealth = (float)Math.Max(1, (int)(this.SpecialProperties.MaxHealth * mod));
			this.Health = this.SpecialProperties.MaxHealth;
			this.attackDamage = (int)((float)this.attackDamage * mod);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0004C984 File Offset: 0x0004AB84
		public override void Heal(float amount, Shootable healer, bool ignoreHealBonus = false)
		{
			base.Heal(amount, healer, ignoreHealBonus);
			if (this.Health > (float)((int)(this.SpecialProperties.MaxHealth * 0.75f)) && !this.ProgressiveDamage)
			{
				this.mDmgTexOffset = 0;
				this.TextureCoord.Y = this.startingTex.Y;
			}
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0004C9DC File Offset: 0x0004ABDC
		private void DeAggroTick()
		{
			for (int i = 0; i < this.AggroList.Count; i++)
			{
				if (this.AggroList[i].AggroValue > 0)
				{
					this.AggroList[i].AggroValue--;
				}
			}
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0004CA2C File Offset: 0x0004AC2C
		public void BuildAggro(Player p, int amount)
		{
			if (p == null)
			{
				return;
			}
			Zombie.Aggro aggro = null;
			foreach (Zombie.Aggro aggro2 in this.AggroList)
			{
				if (aggro2.Player == p)
				{
					aggro = aggro2;
					break;
				}
			}
			if (aggro == null)
			{
				aggro = new Zombie.Aggro();
				aggro.Player = p;
				aggro.AggroValue = amount;
				this.AggroList.Add(aggro);
				return;
			}
			aggro.AggroValue += amount;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0004CAC0 File Offset: 0x0004ACC0
		private Player GetHighestAggro()
		{
			int num = -1;
			Player result = null;
			foreach (Zombie.Aggro aggro in this.AggroList)
			{
				if (!aggro.Player.DEAD && aggro.AggroValue > num)
				{
					result = aggro.Player;
					num = aggro.AggroValue;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0004CB38 File Offset: 0x0004AD38
		public int GetHighestAggroDEBUG()
		{
			int num = -1;
			foreach (Zombie.Aggro aggro in this.AggroList)
			{
				if (aggro.AggroValue > num)
				{
					num = aggro.AggroValue;
					break;
				}
			}
			return num;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0004CB9C File Offset: 0x0004AD9C
		private void NormalLeap()
		{
			base.UnSquishMe(0.1f);
			this.Velocity.Y = 4f;
			Vector2 vector = VerchickMath.DirectionToVector2(base.TwoDPosition(), this.Target.TwoDPosition());
			this.Velocity.X = vector.X * this.leapSpeed;
			this.Velocity.Z = vector.Y * this.leapSpeed;
			this.JustLeaped = 1f;
			this.attackCooldown = this.topAttackCooldown;
			if (Math.Abs(this.Velocity.X) > Math.Abs(this.Velocity.Z))
			{
				if (this.Velocity.X < 0f)
				{
					this.facing = Facing.Left;
				}
				else
				{
					this.facing = Facing.Right;
				}
			}
			else if (this.Velocity.Z > 0f)
			{
				this.facing = Facing.Down;
			}
			else
			{
				this.facing = Facing.Up;
			}
			if (this.zombieType == ZombieType.Skeleton)
			{
				for (int i = 0; i < 10; i++)
				{
					Global.MasterCache.CreateParticle(ParticleType.Dirt, this.Position, -this.Velocity / 2f);
				}
			}
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0004CCC8 File Offset: 0x0004AEC8
		public void AttackFromNet(Msg_ZombieAttack att)
		{
			if (this.attackCooldown > 0f)
			{
				return;
			}
			if (this.SpecialProperties.Speed == 0f)
			{
				return;
			}
			if (this.BuffManager.SpeedMod() == 0f)
			{
				return;
			}
			if (this.JustLeaped > 0f)
			{
				return;
			}
			if (!NetworkManager.NetObjects.ContainsKey(att.TargetUID))
			{
				return;
			}
			this.Target = (NetworkManager.NetObjects[att.TargetUID] as GameObject);
			if (Vector2.Distance(new Vector2(att.PositionX, att.PositionZ), this.twoDPosition) > this.NET_POSITION_THRESHOLD / 2f)
			{
				this.Position.X = att.PositionX;
				this.Position.Z = att.PositionZ;
			}
			this.Attack();
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0004CD98 File Offset: 0x0004AF98
		private void AddStatusEffect(StatusEffect eff)
		{
			for (int i = 0; i < this.statusEffects.Length; i++)
			{
				if (this.statusEffects[i] != null && this.statusEffects[i].Type == eff.Type)
				{
					this.statusEffects[i].Clear();
					this.statusEffects[i] = null;
					this.statusEffects[i] = eff;
					return;
				}
				if (this.statusEffects[i] == null)
				{
					this.statusEffects[i] = eff;
					return;
				}
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0004CE10 File Offset: 0x0004B010
		public static void InitWorths()
		{
			Zombie.Worths = new Dictionary<ZombieType, int>();
			Zombie.PercentMod = new Dictionary<ZombieType, float>();
			Zombie.Worths.Add(ZombieType.NormalZombie, 1);
			Zombie.Worths.Add(ZombieType.Banshee, 5);
			Zombie.Worths.Add(ZombieType.Goliath, 15);
			Zombie.Worths.Add(ZombieType.Hazmat, 2);
			Zombie.Worths.Add(ZombieType.RobBurglar, 15);
			Zombie.Worths.Add(ZombieType.Skeleton, 3);
			Zombie.Worths.Add(ZombieType.SludgeMonster, 8);
			Zombie.Worths.Add(ZombieType.FireWitch, 15);
			Zombie.Worths.Add(ZombieType.IceGolem, 15);
			Zombie.Worths.Add(ZombieType.Glooper, 15);
			Zombie.PercentMod.Add(ZombieType.NormalZombie, 1f);
			Zombie.PercentMod.Add(ZombieType.Skeleton, 0.5f);
			Zombie.PercentMod.Add(ZombieType.Goliath, 0.2f);
			Zombie.PercentMod.Add(ZombieType.Hazmat, 1f);
			Zombie.PercentMod.Add(ZombieType.Banshee, 0.7f);
			Zombie.PercentMod.Add(ZombieType.DrZombie, 0.7f);
			Zombie.PercentMod.Add(ZombieType.Blob, 1.25f);
			Zombie.PercentMod.Add(ZombieType.FireWitch, 0.18f);
			Zombie.PercentMod.Add(ZombieType.IceGolem, 0.135f);
			Zombie.PercentMod.Add(ZombieType.Glooper, 0.28f);
			Zombie.PercentMod.Add(ZombieType.Clown, 0.6f);
			Zombie.PercentMod.Add(ZombieType.BrainZombie, 0.55f);
			Zombie.PercentMod.Add(ZombieType.Brain, 5f);
			Zombie.PercentMod.Add(ZombieType.Gardner, 0.13f);
			Zombie.PercentMod.Add(ZombieType.RobBurglar, 0.2f);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0004CFAA File Offset: 0x0004B1AA
		public void GenerateDropPercent(ushort dropUID)
		{
			this.DropUID = dropUID;
			this.mDropPercent = (float)this.Rand.NextDouble();
			this.dropAmmoType = this.Rand.Next(0, 4);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0004CFD8 File Offset: 0x0004B1D8
		protected override void SendSync(NetMessage msg)
		{
			Msg_ZombieUpdate msg_ZombieUpdate = new Msg_ZombieUpdate();
			msg_ZombieUpdate.PositionX = this.Position.X;
			msg_ZombieUpdate.PositionZ = this.Position.Z;
			if (this.mLastNetHealth != this.Health)
			{
				msg_ZombieUpdate.HealthChanged = true;
				msg_ZombieUpdate.Health = Convert.ToUInt16(this.Health);
			}
			msg_ZombieUpdate.MoveToPositionX = this.MoveToPosition.X;
			msg_ZombieUpdate.MoveToPositionZ = this.MoveToPosition.Y;
			if (this.Target != null)
			{
				msg_ZombieUpdate.TargetUID = this.Target.UID;
			}
			this.mLastNetPosition = this.Position;
			this.mLastNetHealth = this.Health;
			this.AdditionalSync(msg_ZombieUpdate);
			msg.Payload = msg_ZombieUpdate;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0004D094 File Offset: 0x0004B294
		protected override void SendFullSync(NetMessage msg)
		{
			Msg_ZombieUpdate msg_ZombieUpdate = new Msg_ZombieUpdate();
			msg_ZombieUpdate.PositionX = this.Position.X;
			msg_ZombieUpdate.PositionZ = this.Position.Z;
			msg_ZombieUpdate.HealthChanged = true;
			msg_ZombieUpdate.Health = Convert.ToUInt16(this.Health);
			msg_ZombieUpdate.MoveToPositionX = this.MoveToPosition.X;
			msg_ZombieUpdate.MoveToPositionZ = this.MoveToPosition.Y;
			if (this.Target != null)
			{
				msg_ZombieUpdate.TargetUID = this.Target.UID;
			}
			this.mLastNetPosition = this.Position;
			this.mLastNetHealth = this.Health;
			this.AdditionalSync(msg_ZombieUpdate);
			msg.Payload = msg_ZombieUpdate;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0004D144 File Offset: 0x0004B344
		protected override void ReceiveSync(NetPayload incoming)
		{
			Msg_ZombieUpdate msg_ZombieUpdate = incoming as Msg_ZombieUpdate;
			new Vector2(this.Position.X, this.Position.Z);
			if (Vector2.Distance(new Vector2(msg_ZombieUpdate.PositionX, msg_ZombieUpdate.PositionZ), this.twoDPosition) > this.NET_POSITION_THRESHOLD)
			{
				this.Position.X = msg_ZombieUpdate.PositionX;
				this.Position.Z = msg_ZombieUpdate.PositionZ;
			}
			this.MoveToPosition = new Vector2(msg_ZombieUpdate.MoveToPositionX, msg_ZombieUpdate.MoveToPositionZ);
			this.movement = VerchickMath.DirectionToVector2(base.TwoDPosition(), this.MoveToPosition);
			this.movement.Normalize();
			if (msg_ZombieUpdate.HealthChanged)
			{
				this.Health = Convert.ToSingle(msg_ZombieUpdate.Health);
			}
			if (NetworkManager.NetObjects.ContainsKey(msg_ZombieUpdate.TargetUID))
			{
				this.Target = (NetworkManager.NetObjects[msg_ZombieUpdate.TargetUID] as GameObject);
			}
			this.AdditionalReceive(msg_ZombieUpdate);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0004D240 File Offset: 0x0004B440
		protected override void SetSyncTimes()
		{
			this.mDoISync = false;
			this.mTotalSyncTime = 0.75f;
			this.mTotalFullSyncTime = 5f;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00002EF9 File Offset: 0x000010F9
		protected virtual void AdditionalSync(Msg_ZombieUpdate zUpdate)
		{
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00002EF9 File Offset: 0x000010F9
		protected virtual void AdditionalReceive(Msg_ZombieUpdate zUpdate)
		{
		}

		// Token: 0x040009EA RID: 2538
		public Rectangle BoundingRect = new Rectangle(0, 0, 1, 1);

		// Token: 0x040009EB RID: 2539
		public ZombieType zombieType;

		// Token: 0x040009EC RID: 2540
		public Vector2 movement;

		// Token: 0x040009ED RID: 2541
		public Facing facing = Facing.Down;

		// Token: 0x040009EE RID: 2542
		public float defSpeed = 0.2f;

		// Token: 0x040009EF RID: 2543
		private int mDmgTexOffset;

		// Token: 0x040009F0 RID: 2544
		public Point startingTex;

		// Token: 0x040009F1 RID: 2545
		public StatusEffect[] statusEffects;

		// Token: 0x040009F2 RID: 2546
		private float speedMod = 1f;

		// Token: 0x040009F3 RID: 2547
		public GameObject Target;

		// Token: 0x040009F4 RID: 2548
		public ParticleType BloodType = ParticleType.Blood;

		// Token: 0x040009F5 RID: 2549
		public ParticleType GibbletType = ParticleType.NormalZombieGibblet;

		// Token: 0x040009F6 RID: 2550
		public bool ProgressiveDamage;

		// Token: 0x040009F7 RID: 2551
		public int GibbletChance = 75;

		// Token: 0x040009F8 RID: 2552
		public bool PreciseDirection;

		// Token: 0x040009F9 RID: 2553
		private int mDropRandomSeed;

		// Token: 0x040009FA RID: 2554
		private float JustLeaped;

		// Token: 0x040009FB RID: 2555
		public float Worth = 1f;

		// Token: 0x040009FC RID: 2556
		private float dismissDistance = 10f;

		// Token: 0x040009FD RID: 2557
		public bool NOLOGIC;

		// Token: 0x040009FE RID: 2558
		public Tile targTile;

		// Token: 0x040009FF RID: 2559
		public bool spawning = true;

		// Token: 0x04000A00 RID: 2560
		public float spawnSpeed = 1f;

		// Token: 0x04000A01 RID: 2561
		public float elapsedSpawnTime;

		// Token: 0x04000A02 RID: 2562
		public float range = 1f;

		// Token: 0x04000A03 RID: 2563
		public int attackDamage = 5;

		// Token: 0x04000A04 RID: 2564
		private int startAttack = 5;

		// Token: 0x04000A05 RID: 2565
		public float topAttackCooldown = 1.4f;

		// Token: 0x04000A06 RID: 2566
		public float attackCooldown;

		// Token: 0x04000A07 RID: 2567
		public float leapSpeed = 2f;

		// Token: 0x04000A08 RID: 2568
		public bool NoTurretTarget;

		// Token: 0x04000A09 RID: 2569
		public bool NODAMAGE;

		// Token: 0x04000A0A RID: 2570
		public float LiveSpeedMod = 1f;

		// Token: 0x04000A0B RID: 2571
		public float MaxLiveSpeedMod = 1.25f;

		// Token: 0x04000A0C RID: 2572
		public float LiveSpeedModStep = 0.025f;

		// Token: 0x04000A0D RID: 2573
		public float EngageDistance = 6f;

		// Token: 0x04000A0E RID: 2574
		public float TimeToEngage;

		// Token: 0x04000A0F RID: 2575
		public Random Rand;

		// Token: 0x04000A10 RID: 2576
		private Vector2 MoveToPosition = new Vector2(0f, 0f);

		// Token: 0x04000A11 RID: 2577
		public ushort DropUID;

		// Token: 0x04000A12 RID: 2578
		private float mDropPercent;

		// Token: 0x04000A13 RID: 2579
		private int dropAmmoType;

		// Token: 0x04000A14 RID: 2580
		protected bool mKilled;

		// Token: 0x04000A15 RID: 2581
		protected int mSeed;

		// Token: 0x04000A16 RID: 2582
		public bool DontCountAsKill;

		// Token: 0x04000A17 RID: 2583
		private float DeAggroTimer;

		// Token: 0x04000A18 RID: 2584
		private float TotalDeAggroTime = 1f;

		// Token: 0x04000A19 RID: 2585
		private List<Zombie.Aggro> AggroList = new List<Zombie.Aggro>();

		// Token: 0x04000A1A RID: 2586
		public static Dictionary<ZombieType, int> Worths;

		// Token: 0x04000A1B RID: 2587
		public static Dictionary<ZombieType, float> PercentMod;

		// Token: 0x04000A1C RID: 2588
		private float mLastNetHealth;

		// Token: 0x04000A1D RID: 2589
		private Vector3 mLastNetPosition = Vector3.Zero;

		// Token: 0x04000A1E RID: 2590
		private float NET_POSITION_THRESHOLD = 1.25f;

		// Token: 0x0200021F RID: 543
		private class Aggro
		{
			// Token: 0x04000E45 RID: 3653
			public Player Player;

			// Token: 0x04000E46 RID: 3654
			public int AggroValue;
		}
	}
}
