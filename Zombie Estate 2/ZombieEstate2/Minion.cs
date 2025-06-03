using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x0200008E RID: 142
	public class Minion : Player
	{
		// Token: 0x060003A3 RID: 931 RVA: 0x0001A168 File Offset: 0x00018368
		public Minion(Shootable owner, Vector3 pos)
		{
			if (this.parent != null && this.parent is Player)
			{
				Player player = this.parent as Player;
				base.Index = player.GetMinionIndex();
			}
			this.parent = owner;
			this.TextureCoord = new Point(48, 0);
			this.StartTextureCoord = new Point(this.TextureCoord.X, this.TextureCoord.Y);
			this.Position = pos;
			this.Position.X = this.Position.X + 0.01f;
			this.HUMAN = false;
			this.Init();
			this.mPlayerInfo = new PlayerInfo();
			this.mPlayerInfo.Local = true;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001A2D0 File Offset: 0x000184D0
		public Minion(Shootable owner, Vector3 pos, string gun, int ammo, float movespeed, Point tex, bool muzzleFlash, int lvl, int health_count, string buff, string buffArgs, ushort startUID)
		{
			this.parent = owner;
			if (this.parent != null && this.parent is Player)
			{
				Player player = this.parent as Player;
				base.Index = player.GetMinionIndex();
			}
			this.InitBaseSpecialProperties();
			base.UID = startUID;
			int num = 0;
			if (owner is Player)
			{
				num = (owner as Player).Index;
			}
			if (muzzleFlash)
			{
				num *= 2;
			}
			Point point = new Point(tex.X, tex.Y + num);
			this.TextureCoord = point;
			this.StartTextureCoord = point;
			this.Position = pos;
			this.Position.X = this.Position.X + 0.01f;
			this.BaseSpecialProperties.Speed = movespeed;
			this.startGun = gun;
			this.MuzzleFlash = muzzleFlash;
			if (movespeed == 0f)
			{
				this.Moving = false;
				this.TotalShotCount = (int)((float)ammo * (1f + this.parent.SpecialProperties.MinionAmmoMod / 100f));
				this.ShotCount = this.TotalShotCount;
				this.IgnoreAggro = true;
			}
			else
			{
				this.Moving = true;
				this.BaseSpecialProperties.MaxHealth = (float)health_count;
				this.Health = this.BaseSpecialProperties.MaxHealth;
				this.IgnoreAggro = false;
			}
			if (!string.IsNullOrEmpty(buff))
			{
				base.AddBuff(buff, this.parent, buffArgs);
			}
			this.HUMAN = false;
			this.SomethingChanged = true;
			this.Level = lvl;
			this.Init();
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0001A500 File Offset: 0x00018700
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			if (this.parent == null)
			{
				return;
			}
			this.BaseSpecialProperties.ShotTimeMod += this.parent.SpecialProperties.MinionFireRateMod;
			this.BaseSpecialProperties.BulletDamageMod += this.parent.SpecialProperties.MinionDmgMod;
			this.SomethingChanged = true;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001A56C File Offset: 0x0001876C
		public virtual void Init()
		{
			this.MinionInit();
			base.SquishMe(1.5f);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001A580 File Offset: 0x00018780
		public void MinionInit()
		{
			if (this.parent != null && this.parent is Player)
			{
				(this.parent as Player).AddMinion(this);
			}
			this.Guns = new Gun[Player.MAXGUNS];
			if (this.startGun != "NONE")
			{
				base.AddGun(this.startGun, true);
				for (int i = 0; i < this.Level; i++)
				{
					this.Guns[0].LevelUpGun();
				}
			}
			else
			{
				this.Passive = true;
				this.InvisGun = true;
			}
			this.ShotCount = this.TotalShotCount;
			if (!this.Moving)
			{
				this.TimeInd = new FloatingIndicator(this, "DurationMeter");
				Global.MasterCache.CreateObject(this.TimeInd);
				this.TimeInd.UpdateData((float)this.ShotCount / (float)this.TotalShotCount);
			}
			else
			{
				this.TimeInd = new FloatingIndicator(this, "HealthMeter");
				Global.MasterCache.CreateObject(this.TimeInd);
				this.TimeInd.UpdateData(this.Health / this.SpecialProperties.MaxHealth);
			}
			if (this.BaseSpecialProperties.Speed == 0f)
			{
				this.BounceEnabled = false;
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001A6BF File Offset: 0x000188BF
		public override void Update(float elapsed)
		{
			this.UpdateAI(elapsed);
			base.Update(elapsed);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001A6D0 File Offset: 0x000188D0
		private void UpdateAI(float elapsed)
		{
			if (Global.Paused)
			{
				return;
			}
			if (!base.IAmOwnedByLocalPlayer)
			{
				if (this.Passive)
				{
					this.tickTimer -= elapsed;
					if (this.tickTimer <= 0f)
					{
						this.tickTimer = 1f;
						this.ShotCount--;
						if (this.ShotCount <= 0)
						{
							Terminal.WriteMessage("Passive Minion Destroyed - Out of Shots");
							this.DestroyObject();
						}
					}
				}
				this.PickTarget();
				this.UpdateAngleToTarget();
				if (this.Moving && this.TimeInd != null)
				{
					this.TimeInd.UpdateData(this.Health / this.SpecialProperties.MaxHealth);
				}
				if (this.TotalShotCount != 0 && !this.Moving && this.TimeInd != null)
				{
					this.TimeInd.UpdateData((float)this.ShotCount / (float)this.TotalShotCount);
				}
				this.UpdateFacing();
				return;
			}
			if (this.muzzleTimer > 0f)
			{
				this.muzzleTimer -= elapsed;
				this.TextureCoord.Y = this.StartTextureCoord.Y + 1;
			}
			else
			{
				this.muzzleTimer = 0f;
				this.TextureCoord.Y = this.StartTextureCoord.Y;
			}
			if (this.TotalShotCount != 0 && !this.Moving && this.TimeInd != null)
			{
				this.TimeInd.UpdateData((float)this.ShotCount / (float)this.TotalShotCount);
			}
			if (this.Moving && this.TimeInd != null)
			{
				this.TimeInd.UpdateData(this.Health / this.SpecialProperties.MaxHealth);
			}
			this.UpdateMovement();
			this.PickTarget();
			this.UpdateAngleToTarget();
			if (!this.Passive)
			{
				if (this.ZombieTarget != null && this.ZombieTarget.Active && Global.Level.InLineOfSight(this.Position, this.ZombieTarget.Position))
				{
					this.mAimDir.Normalize();
					if (this.mGun.Fire(this.mAimDir, false))
					{
						this.ShotCount--;
						if (this.MuzzleFlash)
						{
							this.muzzleTimer = 0.1f;
						}
					}
				}
			}
			else
			{
				this.tickTimer -= elapsed;
				if (this.tickTimer <= 0f)
				{
					this.tickTimer = 1f;
					this.ShotCount--;
					if (this.ShotCount <= 0)
					{
						this.DestroyObject();
						Terminal.WriteMessage("Passive (LOCAL) Minion Destroyed - Out of Shots");
					}
				}
			}
			this.UpdateFacing();
			this.ReEngageTime -= elapsed;
			if (this.ReEngageTime <= 0f)
			{
				this.ZombieTarget = null;
			}
			if (this.ZombieTarget != null && this.ZombieTarget.NoTurretTarget)
			{
				this.ZombieTarget = null;
			}
			if (this.ShotCount <= 0 && !this.Moving)
			{
				Terminal.WriteMessage("Passive (LOCAL?) Minion Destroyed - Out of Shots");
				this.DestroyObject();
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001A9C0 File Offset: 0x00018BC0
		public override void DestroyObject()
		{
			Terminal.WriteMessage("Minion DestroyObject()");
			if (this.parent != null && this.parent is Player)
			{
				(this.parent as Player).RemoveMinion(this);
			}
			if (this.TimeInd != null)
			{
				this.TimeInd.DestroyObject();
			}
			if (this.mGunObject != null)
			{
				this.mGunObject.DestroyObject();
			}
			if (Global.GameState == GameState.Playing)
			{
				Explosion.CreateExplosion(1f, 25f, 2f, "Fire", this.Position, this.parent, false);
			}
			this.Active = false;
			if (base.IAmOwnedByLocalPlayer)
			{
				Terminal.WriteMessage("Local minion detected to be dead. Sending sync.");
				base.FullSync();
			}
			base.DestroyObject();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001AA75 File Offset: 0x00018C75
		public void FULL_DESTROY()
		{
			if (this.TimeInd != null)
			{
				this.TimeInd.DestroyObject();
			}
			if (this.mGunObject != null)
			{
				this.mGunObject.DestroyObject();
			}
			this.Active = false;
			Terminal.WriteMessage("Minion FULL_DESTROY() called");
			base.DestroyObject();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001AAB4 File Offset: 0x00018CB4
		public override void Damage(Shootable attacker, float amount, DamageType damageType, bool noGore, bool AOE, List<BulletModifier> mods = null)
		{
			if (this.Moving)
			{
				if (this.DEAD)
				{
					return;
				}
				base.Damage(attacker, amount, damageType, noGore, AOE, mods);
				if (this.Health < 1f && base.IAmOwnedByLocalPlayer)
				{
					this.DestroyObject();
					if (this.TimeInd != null)
					{
						this.TimeInd.DestroyObject();
					}
					if (this.mGunObject != null)
					{
						this.mGunObject.DestroyObject();
					}
					this.DEAD = true;
				}
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001AB29 File Offset: 0x00018D29
		public float GetPercentage()
		{
			if (this.Moving)
			{
				return this.Health / this.SpecialProperties.MaxHealth;
			}
			return (float)this.ShotCount / (float)this.TotalShotCount;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001AB58 File Offset: 0x00018D58
		private void UpdateAngleToTarget()
		{
			if (this.ZombieTarget != null)
			{
				this.desiredAngle = VerchickMath.AngleFromVectors(base.TwoDPosition(), this.ZombieTarget.TwoDPosition());
			}
			else if (this.parent != null)
			{
				this.desiredAngle = VerchickMath.AngleFromVectors(base.TwoDPosition(), this.parent.TwoDPosition());
			}
			float num = Minion.WrapAngle(this.desiredAngle - this.angleToTarget);
			num = MathHelper.Clamp(num, -0.1f, 0.1f);
			this.angleToTarget = Minion.WrapAngle(this.angleToTarget + num);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0001ABE8 File Offset: 0x00018DE8
		private void PickTarget()
		{
			if (this.ZombieTarget == null || !this.ZombieTarget.Active || !VerchickMath.WithinDistance(base.TwoDPosition(), this.ZombieTarget.TwoDPosition(), this.dismissDistance) || !Global.Level.InLineOfSight(this.Position, this.ZombieTarget.Position) || this.ZombieTarget.spawning || this.ZombieTarget.NoTurretTarget)
			{
				float num = this.Range;
				foreach (Zombie zombie in Global.ZombieList)
				{
					float num2 = Vector2.Distance(zombie.TwoDPosition(), base.TwoDPosition());
					if (num2 < num && !zombie.spawning && !zombie.NoTurretTarget && Global.Level.InLineOfSight(this.Position, zombie.Position))
					{
						this.ZombieTarget = zombie;
						num = num2;
						this.ReEngageTime = 120f;
					}
				}
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0001ACFC File Offset: 0x00018EFC
		protected override void ReceiveSync(NetPayload incoming)
		{
			if (Global.WaveMaster != null && Global.WaveMaster.FinalWaveComplete)
			{
				return;
			}
			base.ReceiveSync(incoming);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001AD1C File Offset: 0x00018F1C
		private void UpdateMovement()
		{
			this.MoveToTarget = this.parent;
			if (this.tile == this.goalTile)
			{
				this.mAtTile = true;
			}
			if (this.moveState == AIMoveState.Following)
			{
				if (this.mAtTile)
				{
					this.mMovement = Vector2.Zero;
					this.moveState = AIMoveState.Stopped;
					this.targTile = null;
				}
				else if (this.parent.tile != this.prevParentTile || this.targTile == this.tile)
				{
					this.UpdateDirection();
				}
			}
			if (this.moveState == AIMoveState.Stopped)
			{
				this.mMovement = Vector2.Zero;
				this.targTile = null;
				if (this.MoveToTarget != null && !VerchickMath.WithinDistance(this.MoveToTarget.TwoDPosition(), base.TwoDPosition(), this.followingDistance + 0.1f))
				{
					this.moveState = AIMoveState.Following;
					this.followingDistance = Global.RandomFloat(this.minFollowingDistance, this.maxFollowingDistance);
					this.mAtTile = false;
					this.goalTile = null;
				}
			}
			this.prevParentTile = this.parent.tile;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0001AE20 File Offset: 0x00019020
		public override void UpdateTargetObject()
		{
			if (this.mTargetReticule == null)
			{
				this.mTargetReticule = new GameObject();
				this.mTargetReticule.ActivateObject(this.Position, new Point(1, 8));
				Global.MasterCache.CreateObject(this.mTargetReticule);
				this.mTargetReticule.scale = 0.4f;
				this.mTargetReticule.floorHeight = -1f;
				this.mTargetReticule.AffectedByGravity = false;
				this.mTargetReticule.TextureCoord = new Point(63, 63);
			}
			float num = 2f;
			this.mTargetReticule.Position.X = this.Position.X + (float)Math.Cos((double)this.angleToTarget) * num - 0.1f;
			this.mTargetReticule.Position.Z = this.Position.Z + (float)Math.Sin((double)this.angleToTarget) * num;
			this.mTargetReticule.Position.Y = this.Position.Y;
			this.mAimDir = this.mTargetReticule.TwoDPosition() - base.TwoDPosition();
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001AF44 File Offset: 0x00019144
		public void UpdateDirection()
		{
			this.targTile = this.UpdatePathing();
			if (this.targTile != null)
			{
				this.mMovement = VerchickMath.DirectionToVector2(new Vector2(this.Position.X, this.Position.Z), new Vector2((float)this.targTile.x + 0.5f, (float)this.targTile.y + 0.5f));
				this.mMovement.Normalize();
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001AFC0 File Offset: 0x000191C0
		public virtual void UpdateFacing()
		{
			if (base.OnFloor() && this.mTargetReticule != null)
			{
				Vector2 vector = VerchickMath.DirectionToVector2(base.TwoDPosition(), this.mTargetReticule.TwoDPosition());
				if (Math.Abs(vector.X) > Math.Abs(vector.Y))
				{
					if (vector.X < 0f)
					{
						this.mFacing = Facing.Left;
					}
					else
					{
						this.mFacing = Facing.Right;
					}
				}
				else if (vector.Y > 0f)
				{
					this.mFacing = Facing.Down;
				}
				else
				{
					this.mFacing = Facing.Up;
				}
			}
			if (this.mFacing == Facing.Left)
			{
				this.TextureCoord.X = this.StartTextureCoord.X + 2;
			}
			if (this.mFacing == Facing.Down)
			{
				this.TextureCoord.X = this.StartTextureCoord.X;
			}
			if (this.mFacing == Facing.Up)
			{
				this.TextureCoord.X = this.StartTextureCoord.X + 3;
			}
			if (this.mFacing == Facing.Right)
			{
				this.TextureCoord.X = this.StartTextureCoord.X + 1;
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0001B0CC File Offset: 0x000192CC
		private Tile UpdatePathing()
		{
			Vector3 loc = new Vector3(this.MoveToTarget.Position.X, this.MoveToTarget.Position.Y, this.MoveToTarget.Position.Z);
			Tile tile = Global.Level.GetTileAtLocation(loc);
			int index = Global.rand.Next(0, tile.AdjacentTiles.Count);
			tile = tile.AdjacentTiles[index];
			this.goalTile = tile;
			if (this.tile != null)
			{
				int num = this.tile.Directions[tile.x, tile.y];
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

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0001B2CC File Offset: 0x000194CC
		private string AICode
		{
			get
			{
				return "AI Minion " + base.Index.ToString() + ": ";
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0001B2F6 File Offset: 0x000194F6
		private void AITerm(string message)
		{
			Terminal.WriteMessage(this.AICode + message, MessageType.AI);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00018DE2 File Offset: 0x00016FE2
		private static float WrapAngle(float radians)
		{
			while (radians < -3.1415927f)
			{
				radians += 6.2831855f;
			}
			while (radians > 3.1415927f)
			{
				radians -= 6.2831855f;
			}
			return radians;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001B30C File Offset: 0x0001950C
		public override void Death()
		{
			if (this.ShotCount > 0 && (!this.Moving || this.Passive))
			{
				return;
			}
			if (this.Health < 1f)
			{
				Terminal.WriteMessage("DEATH() called for minion.");
				this.DestroyObject();
				if (this.TimeInd != null)
				{
					this.TimeInd.DestroyObject();
				}
				if (this.mGunObject != null)
				{
					this.mGunObject.DestroyObject();
				}
				this.DEAD = true;
			}
		}

		// Token: 0x04000360 RID: 864
		private float angleToTarget;

		// Token: 0x04000361 RID: 865
		private Tile targTile;

		// Token: 0x04000362 RID: 866
		private GameObject MoveToTarget;

		// Token: 0x04000363 RID: 867
		private Zombie ZombieTarget;

		// Token: 0x04000364 RID: 868
		private float minFollowingDistance = 2.5f;

		// Token: 0x04000365 RID: 869
		private float maxFollowingDistance = 3.5f;

		// Token: 0x04000366 RID: 870
		private float followingDistance = 2.5f;

		// Token: 0x04000367 RID: 871
		private AIMoveState moveState = AIMoveState.Following;

		// Token: 0x04000368 RID: 872
		private AICombatState combatState = AICombatState.Wandering;

		// Token: 0x04000369 RID: 873
		public float dismissDistance = 10f;

		// Token: 0x0400036A RID: 874
		private float desiredAngle;

		// Token: 0x0400036B RID: 875
		private float AmmoThreshold = 0.5f;

		// Token: 0x0400036C RID: 876
		private float LowerAmmoThreshold = 0.1f;

		// Token: 0x0400036D RID: 877
		private float HigherAmmoThreshold = 0.65f;

		// Token: 0x0400036E RID: 878
		private float DropDistance = 5f;

		// Token: 0x0400036F RID: 879
		private float ReEngageTime;

		// Token: 0x04000370 RID: 880
		private Timer GunSwitchTimer = new Timer(3f);

		// Token: 0x04000371 RID: 881
		public Shootable parent;

		// Token: 0x04000372 RID: 882
		public int ShotCount = 80;

		// Token: 0x04000373 RID: 883
		public int TotalShotCount = 80;

		// Token: 0x04000374 RID: 884
		public string startGun = "Soap Gun";

		// Token: 0x04000375 RID: 885
		private FloatingIndicator TimeInd;

		// Token: 0x04000376 RID: 886
		private float netUpdate = 0.1f;

		// Token: 0x04000377 RID: 887
		public float Range = 5f;

		// Token: 0x04000378 RID: 888
		public bool MuzzleFlash;

		// Token: 0x04000379 RID: 889
		private float muzzleTimer;

		// Token: 0x0400037A RID: 890
		private int Level;

		// Token: 0x0400037B RID: 891
		public bool Moving;

		// Token: 0x0400037C RID: 892
		public bool Passive;

		// Token: 0x0400037D RID: 893
		private float tickTimer = 1f;

		// Token: 0x0400037E RID: 894
		private Tile prevParentTile;

		// Token: 0x0400037F RID: 895
		private bool mAtTile;

		// Token: 0x04000380 RID: 896
		private Tile goalTile;

		// Token: 0x04000381 RID: 897
		private const byte MINION_DESTROYED = 4;

		// Token: 0x04000382 RID: 898
		private const byte MINION_AMMO_UPDATE = 5;
	}
}
