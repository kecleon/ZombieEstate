using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000086 RID: 134
	internal class AIPlayer : Player
	{
		// Token: 0x0600031B RID: 795 RVA: 0x0001817C File Offset: 0x0001637C
		public AIPlayer()
		{
			this.TextureCoord = new Point(0, 1);
			this.StartTextureCoord = new Point(this.TextureCoord.X, this.TextureCoord.Y);
			this.Position = new Vector3(8f, 0f, 8f);
			this.HUDColor = new Color(255, 140, 140);
			this.HUMAN = false;
			this.Guns[0] = new Gun(this, "Soap Gun");
			base.SwitchGun("Soap Gun");
			base.Index = 1;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00018293 File Offset: 0x00016493
		public override void Update(float elapsed)
		{
			this.UpdateAI(elapsed);
			base.Update(elapsed);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000182A4 File Offset: 0x000164A4
		private void UpdateAI(float elapsed)
		{
			if (Global.Paused)
			{
				return;
			}
			this.GoAfterAmmo();
			this.UpdateMovement();
			this.PickTarget();
			this.ShouldSwitchGuns();
			this.UpdateAngleToTarget();
			if (this.ZombieTarget != null && this.ZombieTarget.Active && Global.Level.InLineOfSight(this.Position, this.ZombieTarget.Position))
			{
				this.mGun.Fire(this.mAimDir, false);
			}
			this.UpdateFacing();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00018324 File Offset: 0x00016524
		private void UpdateAngleToTarget()
		{
			if (this.ZombieTarget != null)
			{
				this.desiredAngle = VerchickMath.AngleFromVectors(base.TwoDPosition(), this.ZombieTarget.TwoDPosition());
			}
			else
			{
				this.desiredAngle = VerchickMath.AngleFromVectors(base.TwoDPosition(), Global.Player.TwoDPosition());
			}
			float num = AIPlayer.WrapAngle(this.desiredAngle - this.angleToTarget);
			num = MathHelper.Clamp(num, -0.1f, 0.1f);
			this.angleToTarget = AIPlayer.WrapAngle(this.angleToTarget + num);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000183AC File Offset: 0x000165AC
		private void PickTarget()
		{
			if (this.ZombieTarget == null || !this.ZombieTarget.Active || !VerchickMath.WithinDistance(base.TwoDPosition(), this.ZombieTarget.TwoDPosition(), this.dismissDistance) || !Global.Level.InLineOfSight(this.Position, this.ZombieTarget.Position))
			{
				float num = float.MaxValue;
				foreach (Zombie zombie in Global.ZombieList)
				{
					float num2 = Vector2.Distance(zombie.TwoDPosition(), base.TwoDPosition());
					if (num2 < num && !zombie.spawning && Global.Level.InLineOfSight(this.Position, zombie.Position))
					{
						this.ZombieTarget = zombie;
						num = num2;
					}
				}
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0001848C File Offset: 0x0001668C
		private void UpdateMovement()
		{
			if (this.moveState != AIMoveState.Ammo)
			{
				this.MoveToTarget = Global.Player;
				if (this.moveState == AIMoveState.Following)
				{
					if (this.targTile != this.tile)
					{
						this.UpdateDirection();
					}
					else
					{
						this.mMovement = Vector2.Zero;
						this.moveState = AIMoveState.Stopped;
						this.targTile = null;
					}
				}
				if (this.moveState == AIMoveState.Stopped)
				{
					this.mMovement = Vector2.Zero;
					this.targTile = null;
					if (!VerchickMath.WithinDistance(this.MoveToTarget.TwoDPosition(), base.TwoDPosition(), this.followingDistance + 0.1f))
					{
						this.moveState = AIMoveState.Following;
						this.followingDistance = Global.RandomFloat(this.minFollowingDistance, this.maxFollowingDistance);
					}
				}
				return;
			}
			if (!this.MoveToTarget.Active)
			{
				this.moveState = AIMoveState.Following;
				this.targTile = null;
				return;
			}
			if (!VerchickMath.WithinDistance(this.MoveToTarget.TwoDPosition(), base.TwoDPosition(), 1f))
			{
				this.UpdateDirection();
				return;
			}
			this.mMovement = VerchickMath.DirectionToVector2(base.TwoDPosition(), this.MoveToTarget.TwoDPosition());
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000185A0 File Offset: 0x000167A0
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
			}
			float num = 2f;
			this.mTargetReticule.Position.X = this.Position.X + (float)Math.Cos((double)this.angleToTarget) * num - 0.1f;
			this.mTargetReticule.Position.Z = this.Position.Z + (float)Math.Sin((double)this.angleToTarget) * num;
			this.mTargetReticule.Position.Y = this.Position.Y;
			this.mAimDir = this.mTargetReticule.TwoDPosition() - base.TwoDPosition();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000186B0 File Offset: 0x000168B0
		private void ShouldSwitchGuns()
		{
			if (!this.GunSwitchTimer.Ready())
			{
				return;
			}
			AmmoType ammoType = this.mGun.ammoType;
			if (this.Stats.GetAmmo(ammoType) == 0 && ammoType != AmmoType.INFINITE)
			{
				this.PickNextBestGun(true);
			}
			if (ammoType == AmmoType.INFINITE)
			{
				this.PickNextBestGun(false);
				return;
			}
			if ((float)this.Stats.GetAmmo(ammoType) < (float)this.Stats.GetMaxAmmo(ammoType) * this.LowerAmmoThreshold)
			{
				this.PickNextBestGun(false);
				return;
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00018728 File Offset: 0x00016928
		private void PickNextBestGun(bool force)
		{
			this.GunSwitchTimer.Reset();
			this.GunSwitchTimer.Start();
			foreach (Gun gun in this.Guns)
			{
				if (gun.ammoType != AmmoType.INFINITE && gun != this.mGun && (float)this.Stats.GetAmmo(gun.ammoType) >= (float)this.Stats.GetMaxAmmo(gun.ammoType) * this.AmmoThreshold)
				{
					base.SwitchGun(gun.Name);
					this.AITerm("Switched to gun: |" + gun.Name + "|");
					return;
				}
			}
			if (force)
			{
				foreach (Gun gun2 in this.Guns)
				{
					if (gun2.ammoType != AmmoType.INFINITE && gun2 != this.mGun && this.Stats.GetAmmo(gun2.ammoType) > 0)
					{
						base.SwitchGun(gun2.Name);
						this.AITerm("Switched to gun: |" + gun2.Name + "|");
						return;
					}
				}
				foreach (Gun gun3 in this.Guns)
				{
					if (gun3.ammoType == AmmoType.INFINITE)
					{
						base.SwitchGun(gun3.Name);
					}
				}
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0001886C File Offset: 0x00016A6C
		public void UpdateDirection()
		{
			this.targTile = this.UpdatePathing();
			if (this.targTile != null)
			{
				this.mMovement = VerchickMath.DirectionToVector2(new Vector2(this.Position.X, this.Position.Z), new Vector2((float)this.targTile.x + 0.5f, (float)this.targTile.y + 0.5f));
				this.mMovement.Normalize();
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000188E8 File Offset: 0x00016AE8
		public void UpdateFacing()
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

		// Token: 0x06000326 RID: 806 RVA: 0x000189F4 File Offset: 0x00016BF4
		private void GoAfterAmmo()
		{
			if (this.moveState == AIMoveState.Ammo)
			{
				return;
			}
			if (this.tile == null)
			{
				return;
			}
			foreach (Drop drop in Global.DropsList)
			{
				if (VerchickMath.WithinDistance(drop.TwoDPosition(), base.TwoDPosition(), this.DropDistance))
				{
					AmmoDrop ammoDrop = drop as AmmoDrop;
					if (ammoDrop != null)
					{
						AmmoType ammoType = ammoDrop.ammoType;
						if (ammoType != AmmoType.SPECIAL && (float)this.Stats.GetAmmo(ammoType) < (float)this.Stats.GetMaxAmmo(ammoType) * this.HigherAmmoThreshold)
						{
							this.moveState = AIMoveState.Ammo;
							this.MoveToTarget = ammoDrop;
							this.AITerm("Going to pick up |" + ammoDrop.ammoType.ToString() + " Drop|");
							this.targTile = null;
							break;
						}
					}
					else
					{
						HealthDrop healthDrop = drop as HealthDrop;
						if (healthDrop != null && this.Health < this.SpecialProperties.MaxHealth * 0.5f)
						{
							this.moveState = AIMoveState.Ammo;
							this.MoveToTarget = healthDrop;
							this.AITerm("Going to pick up |Health Drop|");
							this.targTile = null;
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00018B38 File Offset: 0x00016D38
		private Tile UpdatePathing()
		{
			Vector3 loc = new Vector3(this.MoveToTarget.Position.X, this.MoveToTarget.Position.Y, this.MoveToTarget.Position.Z);
			Tile tileAtLocation = Global.Level.GetTileAtLocation(loc);
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

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00018D10 File Offset: 0x00016F10
		private string AICode
		{
			get
			{
				return "AI " + base.Index.ToString() + ": ";
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00018D3A File Offset: 0x00016F3A
		private void AITerm(string message)
		{
			Terminal.WriteMessage(this.AICode + message, MessageType.AI);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00018D50 File Offset: 0x00016F50
		public override void InitPlayer(CharacterStats stats, CharacterSettings settings, int uid)
		{
			this.Stats = new PlayerStats(stats, settings, this);
			this.Stats.parent = this;
			this.Guns = new Gun[Player.MAXGUNS];
			this.mGun = null;
			this.Stats.CharSettings = settings;
			base.AddGun(settings.startingGun, true);
			base.Index = this.mIndex;
			base.OwnerIndex = this.mIndex;
			this.StartTextureCoord = this.Stats.CharSettings.texCoord;
			this.TextureCoord = this.StartTextureCoord;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00018DE2 File Offset: 0x00016FE2
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

		// Token: 0x04000312 RID: 786
		private float angleToTarget;

		// Token: 0x04000313 RID: 787
		private Tile targTile;

		// Token: 0x04000314 RID: 788
		private GameObject MoveToTarget;

		// Token: 0x04000315 RID: 789
		private Zombie ZombieTarget;

		// Token: 0x04000316 RID: 790
		private float minFollowingDistance = 2.5f;

		// Token: 0x04000317 RID: 791
		private float maxFollowingDistance = 3.5f;

		// Token: 0x04000318 RID: 792
		private float followingDistance = 2.5f;

		// Token: 0x04000319 RID: 793
		private AIMoveState moveState = AIMoveState.Following;

		// Token: 0x0400031A RID: 794
		private AICombatState combatState = AICombatState.Wandering;

		// Token: 0x0400031B RID: 795
		private float dismissDistance = 10f;

		// Token: 0x0400031C RID: 796
		private float desiredAngle;

		// Token: 0x0400031D RID: 797
		private float AmmoThreshold = 0.5f;

		// Token: 0x0400031E RID: 798
		private float LowerAmmoThreshold = 0.1f;

		// Token: 0x0400031F RID: 799
		private float HigherAmmoThreshold = 0.65f;

		// Token: 0x04000320 RID: 800
		private float DropDistance = 5f;

		// Token: 0x04000321 RID: 801
		private Timer GunSwitchTimer = new Timer(3f);
	}
}
