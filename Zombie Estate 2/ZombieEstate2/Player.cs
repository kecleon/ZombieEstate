using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieEstate2.HUD.XboxHUD;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x0200011D RID: 285
	public class Player : Shootable
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0003FF0A File Offset: 0x0003E10A
		public PlayerInfo PlayerInfo
		{
			get
			{
				return this.mPlayerInfo;
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0003FF14 File Offset: 0x0003E114
		public Player()
		{
			this.TextureCoord = new Point(4, 0);
			this.StartTextureCoord = new Point(this.TextureCoord.X, this.TextureCoord.Y);
			this.Active = true;
			this.LockInput.IndependentOfTime = true;
			this.Position.Y = 0.5f;
			this.Position.X = 10f + (float)Global.rand.Next(0, 4);
			this.Position.Z = 10f + (float)Global.rand.Next(0, 4);
			this.scale = 0.4f;
			this.Guns = new Gun[Player.MAXGUNS];
			for (int i = 0; i < Player.MAXGUNS; i++)
			{
				this.Guns[i] = null;
			}
			this.BounceSpeed = 0.1f;
			this.HUDColor = new Color(136, 178, 229);
			this.Stats = new PlayerStats();
			this.mStatusEffects = new StatusEffect[8];
			this.LoadPlayerData();
			this.XboxHUD = new XboxHUD(this);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00040163 File Offset: 0x0003E363
		public override void InitBaseSpecialProperties()
		{
			if (this.Stats == null)
			{
				return;
			}
			this.BaseSpecialProperties = this.Stats.CharSettings.Properties;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00040184 File Offset: 0x0003E384
		public override void Update(float elapsed)
		{
			if (this.mReadyCooldown > 0f)
			{
				this.mReadyCooldown -= Global.REAL_GAME_TIME;
				if (this.mReadyCooldown < 0f)
				{
					this.mReadyCooldown = 0f;
				}
			}
			if (this.DEAD)
			{
				this.TextureCoord = this.mDeathTexture;
				if (this.mGunObject != null)
				{
					this.mGunObject.Position.Y = 10000f;
				}
				if (this.mTargetReticule != null)
				{
					this.mTargetReticule.Invisible = true;
				}
				return;
			}
			if (this.mTargetReticule != null && this.mPlayerInfo != null)
			{
				if (!base.IAmOwnedByLocalPlayer)
				{
					this.mTargetReticule.Invisible = true;
				}
				else if (this.mPlayerInfo.UsingController)
				{
					this.mTargetReticule.Invisible = false;
				}
				else
				{
					this.mTargetReticule.Invisible = true;
				}
			}
			if (Global.Paused)
			{
				return;
			}
			this.UpdateTargetObject();
			base.UpdateTile();
			this.UpdateDances(elapsed);
			if (this.HUMAN && base.IAmOwnedByLocalPlayer)
			{
				this.Input(elapsed);
				BloodEffect.Update();
			}
			else
			{
				if (this.mNetHomingPosition != Vector2.Zero && VerchickMath.WithinDistance(this.twoDPosition, this.mNetHomingPosition, 0.1f))
				{
					this.mNetHomingPosition = Vector2.Zero;
					this.mMovement = Vector2.Zero;
				}
				if (!this.PlayerInputLocked)
				{
					if (Math.Abs(this.mMovement.X) > Math.Abs(this.mMovement.Y))
					{
						if (this.mMovement.X > 0f)
						{
							this.FaceRight();
						}
						if (this.mMovement.X < 0f)
						{
							this.FaceLeft();
						}
					}
					else
					{
						if (this.mMovement.Y > 0f)
						{
							this.FaceDown();
						}
						if (this.mMovement.Y < 0f)
						{
							this.FaceUp();
						}
					}
				}
			}
			this.CheckCameraBounds();
			if (this.mMovement != Vector2.Zero && base.OnFloor() && this.SpecialProperties.Speed != 0f)
			{
				this.BounceEnabled = true;
			}
			else
			{
				this.BounceEnabled = false;
			}
			this.Position.X = this.Position.X + this.mMovement.X * this.SpecialProperties.Speed * elapsed;
			this.UpdateLevelCollisions();
			this.PrevPosition = this.Position;
			this.Position.Z = this.Position.Z + this.mMovement.Y * this.SpecialProperties.Speed * elapsed;
			this.UpdateFiringFacing(elapsed);
			this.UpdateGunObject();
			this.UpdateLevelCollisions();
			if (this.mGun != null)
			{
				this.mGun.UpdateGun(elapsed);
			}
			this.UpdateAccessoryObject();
			if (base.IAmOwnedByLocalPlayer && this.HUMAN && !this.DEAD)
			{
				this.UpdateReloadTip(elapsed);
			}
			bool human = this.HUMAN;
			base.Update(elapsed);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00040464 File Offset: 0x0003E664
		private void UpdateLevelCollisions()
		{
			Tile tileAtLocation = Global.Level.GetTileAtLocation(this.Position);
			if (tileAtLocation == null || (tileAtLocation != null && tileAtLocation.TileProperties.Contains(TilePropertyType.NoPath)))
			{
				this.Position = this.PrevPosition;
				this.mTimeInNoPath += 1f;
				if (this.mTimeInNoPath >= 60f)
				{
					this.Position = GameManager.level.PlayerSpawns[0];
				}
				return;
			}
			this.mTimeInNoPath = 0f;
			int num = tileAtLocation.CollidedWithWallPLAYER(this.Position, 1f);
			if (Global.BossActive && num != -1)
			{
				this.Position = this.PrevPosition;
				return;
			}
			if (!tileAtLocation.HasAnyWalls() || num == -1)
			{
				return;
			}
			bool flag = tileAtLocation.CollidedWithHalfWall(this.Position, 1f);
			if (flag && this.Position.Y > this.floorHeight && this.Velocity.Y > 0.1f)
			{
				if (tileAtLocation.CollidedWithFullOrWindowWall(this.Position, 1f) != -1)
				{
					this.Position.X = this.PrevPosition.X;
					this.Position.Z = this.PrevPosition.Z;
				}
				return;
			}
			if (flag && this.Position.Y > this.floorHeight && this.Velocity.Y < 0f)
			{
				float num2 = 0.25f;
				this.Position.Z = this.Position.Z + num2;
				this.Position.Y = this.PrevPosition.Y;
				this.Position.X = this.PrevPosition.X;
				return;
			}
			this.Position = this.PrevPosition;
			base.UnSquishMe(0.1f, false);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00040620 File Offset: 0x0003E820
		public void Input(float elapsed)
		{
			if (this.LockInput.Running() || MasterStore.Active || !Global.Game.IsActive)
			{
				return;
			}
			this.XboxHUD.Update(0.016666668f);
			if (SteamHelper.SteamOverlayVisible)
			{
				return;
			}
			this.FIRING = false;
			if (this.PlayerInputLocked)
			{
				this.mMovement = Vector2.Zero;
			}
			if (this.speedMod == 0f)
			{
				return;
			}
			if (!this.PlayerInputLocked)
			{
				if (!this.XboxHUD.mStatsMode)
				{
					if (this.mPlayerInfo.UsingController)
					{
						this.MovementXbox();
					}
					else
					{
						this.Movement();
					}
				}
				if (!this.mPlayerInfo.UsingController)
				{
					this.mAimDir = VerchickMath.DirectionToVector2(base.TwoDPosition(), new Vector2(MouseHandler.GetPickedPosition().X, MouseHandler.GetPickedPosition().Z));
				}
				else
				{
					this.mAimDir = InputManager.RightStickDirection(this.mPlayerInfo.ControllerIndex);
				}
				if (InputManager.ButtonPressed(ButtonPress.Jump, this.Index, false))
				{
					this.Jump();
				}
				if (InputManager.ButtonPressed(ButtonPress.HealthPack, this.Index, false) && this.Health < this.SpecialProperties.MaxHealth && this.Stats.HealthPacks > 0)
				{
					this.Heal(25f, this, false);
					PlayerStats stats = this.Stats;
					int healthPacks = stats.HealthPacks;
					stats.HealthPacks = healthPacks - 1;
				}
				if (InputManager.ButtonHeld(ButtonPress.Fire, this.Index, false) && elapsed != 0f)
				{
					this.FIRING = true;
					this.FireBullet();
				}
				this.Input_GunSwitch();
				if (InputManager.ButtonPressed(ButtonPress.Reload, this.Index, false))
				{
					this.mGun.ReloadIfNeeded();
				}
				if (InputManager.ButtonPressed(ButtonPress.Ready, this.Index, false) && this.mReadyCooldown <= 0f && !Global.WaveMaster.PreWaveActive && !Global.WaveMaster.WaveActive && !MasterStore.Active)
				{
					this.READY = !this.READY;
					this.mReadyCooldown = 1f;
					if (this.XboxHUD != null)
					{
						this.XboxHUD.TriggerReady(this.READY);
					}
					if (this.READY)
					{
						SoundEngine.PlaySound("ze2_navup", 0.25f);
					}
					else
					{
						SoundEngine.PlaySound("ze2_navdown", 0.25f);
					}
					this.SendREADYState();
				}
			}
			if (!Global.Paused && !MenuManager.MenuActive && !PingManager.DISABLE_PINGS && InputManager.ButtonPressed(ButtonPress.Pause, this.Index, false))
			{
				MenuManager.PushMenu(new PauseMenu());
			}
			if (Global.CHEAT)
			{
				if (InputManager.ButtonPressed(Keys.U, this.Index))
				{
					this.Stats.GiveAmmo(AmmoType.ASSAULT, 999);
					this.Stats.GiveAmmo(AmmoType.SHELLS, 999);
					this.Stats.GiveAmmo(AmmoType.HEAVY, 999);
					this.Stats.GiveAmmo(AmmoType.EXPLOSIVE, 999);
					this.Stats.GiveAmmo(AmmoType.SPECIAL, 999);
					this.Stats.AddTalentPoints(5);
					this.Stats.UpgradeTokens += 5;
					this.Stats.HealthPacks++;
					this.Heal(1000f, this, false);
					this.Stats.AddMoney(3000f);
				}
				if (InputManager.ButtonPressed(Keys.NumPad0, this.Index))
				{
					Terminal.WriteMessage("Current Zombie List Count: " + Global.ZombieList.Count, MessageType.IMPORTANTEVENT);
				}
				if (InputManager.ButtonPressed(Keys.L, this.Index) && !this.mGun.IsSuper)
				{
					this.mGun.LevelUpGun();
				}
				if (InputManager.ButtonPressed(Keys.NumPad1, this.Index))
				{
					MasterStore.Activate();
				}
				if (InputManager.ButtonPressed(Keys.E, this.Index) && InputManager.ButtonHeld(Keys.LeftControl, 0) && this.Index == 0)
				{
					Global.WaveMaster.EndWave();
				}
				if (Keyboard.GetState().IsKeyDown(Keys.I) && this.mPrevKeyState.IsKeyUp(Keys.I))
				{
					Global.Editor.EDITING = !Global.Editor.EDITING;
					Global.GameState = (Global.Editor.EDITING ? GameState.Editor : GameState.Playing);
				}
			}
			this.mPrevKeyState = Keyboard.GetState();
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00040A40 File Offset: 0x0003EC40
		private void Input_GunSwitch()
		{
			if (this.mGun.Swinging)
			{
				return;
			}
			this.mMousewheelTimer -= Global.REAL_GAME_TIME;
			if (this.mMousewheelTimer < 0f)
			{
				this.mMousewheelTimer = 0f;
			}
			if (this.mPlayerInfo != null && !this.mPlayerInfo.UsingController && this.mMousewheelTimer <= 0f)
			{
				if (InputManager.MouseWheelDown())
				{
					this.mSwappedWeaponsYet = true;
					this.mWeaponIndex--;
					this.mWeaponIndex = VerchickMath.Wrap(this.mWeaponIndex, this.GetGunCount());
					this.SwitchGun(this.Guns[this.mWeaponIndex].Name);
					this.mMousewheelTimer = 0.18f;
					return;
				}
				if (InputManager.MouseWheelUp())
				{
					this.mSwappedWeaponsYet = true;
					this.mWeaponIndex++;
					this.mWeaponIndex = VerchickMath.Wrap(this.mWeaponIndex, this.GetGunCount());
					this.SwitchGun(this.Guns[this.mWeaponIndex].Name);
					this.mMousewheelTimer = 0.18f;
					return;
				}
			}
			if (InputManager.ButtonPressed(ButtonPress.SwapWeaponsNegative, this.Index, false))
			{
				this.mSwappedWeaponsYet = true;
				this.mWeaponIndex--;
				this.mWeaponIndex = VerchickMath.Wrap(this.mWeaponIndex, this.GetGunCount());
				this.SwitchGun(this.Guns[this.mWeaponIndex].Name);
				return;
			}
			if (InputManager.ButtonPressed(ButtonPress.SwapWeaponsPositive, this.Index, false))
			{
				this.mSwappedWeaponsYet = true;
				this.mWeaponIndex++;
				this.mWeaponIndex = VerchickMath.Wrap(this.mWeaponIndex, this.GetGunCount());
				this.SwitchGun(this.Guns[this.mWeaponIndex].Name);
				return;
			}
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00040C08 File Offset: 0x0003EE08
		private void WeaponHotkeys()
		{
			if (InputManager.ButtonHeld(Keys.LeftShift, 0))
			{
				return;
			}
			if (InputManager.ButtonPressed(Keys.D1, this.Index))
			{
				this.AttemptToSwitchGuns(0);
			}
			if (InputManager.ButtonPressed(Keys.D2, this.Index))
			{
				this.AttemptToSwitchGuns(1);
			}
			if (InputManager.ButtonPressed(Keys.D3, this.Index))
			{
				this.AttemptToSwitchGuns(2);
			}
			if (InputManager.ButtonPressed(Keys.D4, this.Index))
			{
				this.AttemptToSwitchGuns(3);
			}
			if (InputManager.ButtonPressed(Keys.D5, this.Index))
			{
				this.AttemptToSwitchGuns(4);
			}
			if (InputManager.ButtonPressed(Keys.D6, this.Index))
			{
				this.AttemptToSwitchGuns(5);
			}
			if (InputManager.ButtonPressed(Keys.D7, this.Index))
			{
				this.AttemptToSwitchGuns(6);
			}
			if (InputManager.ButtonPressed(Keys.D8, this.Index))
			{
				this.AttemptToSwitchGuns(7);
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00040CD3 File Offset: 0x0003EED3
		private void AttemptToSwitchGuns(int i)
		{
			if (this.Guns[i] == null)
			{
				return;
			}
			this.SwitchGun(this.Guns[i].Name);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00040CF4 File Offset: 0x0003EEF4
		private void Movement()
		{
			this.mMovement = Vector2.Zero;
			if (InputManager.ButtonHeld(ButtonPress.MoveWest, this.Index, false))
			{
				if (InputManager.ButtonHeld(ButtonPress.MoveSouth, this.Index, false))
				{
					this.mMovement.X = this.mMovement.X - 0.8f;
					this.mMovement.Y = this.mMovement.Y + 0.8f;
					this.FaceLeft();
					return;
				}
				if (InputManager.ButtonHeld(ButtonPress.MoveNorth, this.Index, false))
				{
					this.mMovement.X = this.mMovement.X - 0.8f;
					this.mMovement.Y = this.mMovement.Y - 0.8f;
					this.FaceLeft();
					return;
				}
				this.mMovement.X = this.mMovement.X - 1f;
				this.FaceLeft();
				return;
			}
			else if (InputManager.ButtonHeld(ButtonPress.MoveEast, this.Index, false))
			{
				if (InputManager.ButtonHeld(ButtonPress.MoveSouth, this.Index, false))
				{
					this.mMovement.X = this.mMovement.X + 0.8f;
					this.mMovement.Y = this.mMovement.Y + 0.8f;
					this.FaceRight();
					return;
				}
				if (InputManager.ButtonHeld(ButtonPress.MoveNorth, this.Index, false))
				{
					this.mMovement.X = this.mMovement.X + 0.8f;
					this.mMovement.Y = this.mMovement.Y - 0.8f;
					this.FaceRight();
					return;
				}
				this.mMovement.X = this.mMovement.X + 1f;
				this.FaceRight();
				return;
			}
			else
			{
				if (InputManager.ButtonHeld(ButtonPress.MoveNorth, this.Index, false))
				{
					this.mMovement.Y = this.mMovement.Y - 1f;
					this.FaceUp();
					return;
				}
				if (InputManager.ButtonHeld(ButtonPress.MoveSouth, this.Index, false))
				{
					this.mMovement.Y = this.mMovement.Y + 1f;
					this.FaceDown();
					return;
				}
				return;
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00040EBC File Offset: 0x0003F0BC
		private void MovementXbox()
		{
			this.mMovement = Vector2.Zero;
			if (InputManager.ButtonHeld(ButtonPress.XboxMoveWest, this.Index, false))
			{
				if (InputManager.ButtonHeld(ButtonPress.XboxMoveSouth, this.Index, false))
				{
					this.mMovement.X = this.mMovement.X - 0.8f;
					this.mMovement.Y = this.mMovement.Y + 0.8f;
					this.FaceLeft();
					return;
				}
				if (InputManager.ButtonHeld(ButtonPress.XboxMoveNorth, this.Index, false))
				{
					this.mMovement.X = this.mMovement.X - 0.8f;
					this.mMovement.Y = this.mMovement.Y - 0.8f;
					this.FaceLeft();
					return;
				}
				this.mMovement.X = this.mMovement.X - 1f;
				this.FaceLeft();
				return;
			}
			else if (InputManager.ButtonHeld(ButtonPress.XboxMoveEast, this.Index, false))
			{
				if (InputManager.ButtonHeld(ButtonPress.XboxMoveSouth, this.Index, false))
				{
					this.mMovement.X = this.mMovement.X + 0.8f;
					this.mMovement.Y = this.mMovement.Y + 0.8f;
					this.FaceRight();
					return;
				}
				if (InputManager.ButtonHeld(ButtonPress.XboxMoveNorth, this.Index, false))
				{
					this.mMovement.X = this.mMovement.X + 0.8f;
					this.mMovement.Y = this.mMovement.Y - 0.8f;
					this.FaceRight();
					return;
				}
				this.mMovement.X = this.mMovement.X + 1f;
				this.FaceRight();
				return;
			}
			else
			{
				if (InputManager.ButtonHeld(ButtonPress.XboxMoveNorth, this.Index, false))
				{
					this.mMovement.Y = this.mMovement.Y - 1f;
					this.FaceUp();
					return;
				}
				if (InputManager.ButtonHeld(ButtonPress.XboxMoveSouth, this.Index, false))
				{
					this.mMovement.Y = this.mMovement.Y + 1f;
					this.FaceDown();
					return;
				}
				return;
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00041082 File Offset: 0x0003F282
		public void FaceDown()
		{
			this.TextureCoord.X = this.StartTextureCoord.X;
			this.mFacingOffset = 0;
			this.mFacing = Facing.Down;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x000410A8 File Offset: 0x0003F2A8
		private void FaceUp()
		{
			this.TextureCoord.X = this.StartTextureCoord.X + 3;
			this.mFacingOffset = 3;
			this.mFacing = Facing.Up;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x000410D0 File Offset: 0x0003F2D0
		private void FaceRight()
		{
			this.TextureCoord.X = this.StartTextureCoord.X + 1;
			this.mFacingOffset = 1;
			this.mFacing = Facing.Right;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x000410F8 File Offset: 0x0003F2F8
		private void FaceLeft()
		{
			this.TextureCoord.X = this.StartTextureCoord.X + 2;
			this.mFacingOffset = 2;
			this.mFacing = Facing.Left;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00041120 File Offset: 0x0003F320
		private void UpdateFiringFacing(float elapsed)
		{
			if (this.FIRING || (this.mRecentlyFired.Running() && (this.mPlayerInfo == null || !this.mPlayerInfo.UsingController) && !this.PlayerInputLocked))
			{
				float num = Math.Abs(this.mAimDir.X);
				float num2 = Math.Abs(this.mAimDir.Y);
				if (num < num2)
				{
					if (this.mAimDir.Y < 0f)
					{
						this.FaceUp();
						return;
					}
					this.FaceDown();
					return;
				}
				else
				{
					if (this.mAimDir.X < 0f)
					{
						this.FaceLeft();
						return;
					}
					this.FaceRight();
				}
			}
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000411C8 File Offset: 0x0003F3C8
		private void UpdateGunObject()
		{
			if (this.mGunObject == null)
			{
				this.mGunObject = new GameObject();
				this.mGunObject.ActivateObject(this.Position, new Point(63, 63));
				Global.MasterCache.CreateObject(this.mGunObject);
				this.mGunObject.AffectedByGravity = false;
				this.mGunObject.scale = 0.45f;
			}
			this.mGunObject.AffectedByGravity = false;
			if (this.InvisGun)
			{
				this.mGunObject.TextureCoord = new Point(63, 63);
				return;
			}
			float num = 0.2f;
			float num2 = 0.18f;
			float num3 = -0.2f;
			switch (this.mFacing)
			{
			case Facing.Up:
				this.mGunObject.TextureCoord = new Point(63, 63);
				this.mGunObject.Position = new Vector3(this.Position.X, this.Position.Y + num3, this.Position.Z + 0.01f);
				break;
			case Facing.Down:
				this.mGunObject.TextureCoord = new Point(this.mGunObject.TextureCoord.X, this.mGun.OriginTex.Y + 0);
				this.mGunObject.Position = new Vector3(this.Position.X - num2, this.Position.Y + num3, this.Position.Z + 0.08f);
				break;
			case Facing.Left:
				this.mGunObject.TextureCoord = new Point(this.mGunObject.TextureCoord.X, this.mGun.OriginTex.Y + 2);
				this.mGunObject.Position = new Vector3(this.Position.X - num - 0.1f, this.Position.Y + num3, this.Position.Z + 0.01f);
				break;
			case Facing.Right:
				this.mGunObject.TextureCoord = new Point(this.mGunObject.TextureCoord.X, this.mGun.OriginTex.Y + 1);
				this.mGunObject.Position = new Vector3(this.Position.X + num + 0.1f, this.Position.Y + num3, this.Position.Z + 0.01f);
				break;
			}
			this.mGunObject.Position.Y = this.Position.Y + 0f;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0004145C File Offset: 0x0003F65C
		private void UpdateAccessoryObject()
		{
			if (this.mAccessoryObject == null)
			{
				this.mAccessoryObject = new GameObject();
				this.mAccessoryObject.ActivateObject(this.Position, this.AccessoryTexCoord);
				Global.MasterCache.CreateObject(this.mAccessoryObject);
				this.mAccessoryObject.AffectedByGravity = false;
				this.mAccessoryObject.scale = 0.4f;
			}
			this.mAccessoryObject.TextureCoord = this.AccessoryTexCoord;
			if (this.AccessoryName != "None")
			{
				GameObject gameObject = this.mAccessoryObject;
				gameObject.TextureCoord.Y = gameObject.TextureCoord.Y + this.mFacingOffset;
			}
			float num = 0.6f + this.Stats.CharSettings.hatOffset.Y;
			float num2 = this.Stats.CharSettings.hatOffset.X;
			if (this.mFacing == Facing.Left)
			{
				num2 = -num2;
			}
			if (this.mFacing == Facing.Up || this.mFacing == Facing.Down)
			{
				num2 = 0f;
			}
			if (this.BounceEnabled && this.Bouncing)
			{
				this.mAccessoryObject.Position = new Vector3(this.Position.X + num2, this.Position.Y + num + this.BounceHeight - (1f - this.yScale) * this.scale, this.Position.Z + 0.01f);
				return;
			}
			this.mAccessoryObject.Position = new Vector3(this.Position.X + num2, this.Position.Y + num - (1f - this.yScale) * this.scale, this.Position.Z + 0.01f);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00041608 File Offset: 0x0003F808
		public virtual void UpdateTargetObject()
		{
			if (this.mTargetReticule == null)
			{
				this.mTargetReticule = new GameObject();
				this.mTargetReticule.ActivateObject(this.Position, new Point(this.Index, 8));
				Global.MasterCache.CreateObject(this.mTargetReticule);
				this.mTargetReticule.scale = 0.4f;
				this.mTargetReticule.floorHeight = -1f;
				this.mTargetReticule.AffectedByGravity = false;
			}
			float num = 2f;
			this.mTargetReticule.Position.X = this.Position.X + this.mAimDir.X * num - 0.1f;
			this.mTargetReticule.Position.Z = this.Position.Z + this.mAimDir.Y * num;
			this.mTargetReticule.Position.Y = this.Position.Y;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x000416FC File Offset: 0x0003F8FC
		private void UpdateEffects(float elapsed)
		{
			for (int i = 0; i < this.mStatusEffects.Length; i++)
			{
				if (this.mStatusEffects[i] != null)
				{
					this.mStatusEffects[i].Update(elapsed);
					if (this.mStatusEffects[i].Type == "None")
					{
						this.mStatusEffects[i] = null;
					}
				}
			}
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00041758 File Offset: 0x0003F958
		public override void FireUpdateProperties()
		{
			this.PropertiesToAddUp.Clear();
			if (this.mGun != null && !this.CHARSELECT)
			{
				this.PropertiesToAddUp.Add(this.mGun.stats.SpecialProperties[this.mGun.GetLevel()]);
			}
			this.PropertiesToAddUp.Add(this.TalentSpecProps);
			this.PropertiesToAddUp.Add(this.HatSpecProps);
			base.FireUpdateProperties();
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x000417D4 File Offset: 0x0003F9D4
		public void Jump()
		{
			if (base.OnFloor())
			{
				if (VerchickMath.WithinDistance(base.TwoDPosition(), new Vector2(Level.ShopKeepLocation.X, Level.ShopKeepLocation.Z), 1.5f) && !Global.WaveMaster.WaveActive)
				{
					return;
				}
				base.UnSquishMe(0.125f);
				this.Velocity.Y = 6f;
				if (this.mDanceState == DanceState.NONE)
				{
					SoundEngine.PlaySound(Player.mJumpSound, 0.3f);
				}
				if (base.IAmOwnedByLocalPlayer)
				{
					NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.PlayerJump);
					netMessage.UID = base.UID;
					NetworkManager.SendMessage(netMessage, SendType.Reliable);
				}
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00041878 File Offset: 0x0003FA78
		public void Draw(SpriteBatch spriteBatch)
		{
			if (!this.HUMAN)
			{
				Player player = Global.PlayerList[this.Index];
				return;
			}
			if (this.DEAD)
			{
				return;
			}
			this.DrawOverHead(spriteBatch);
			BloodEffect.Draw(spriteBatch);
			if (!Global.Paused)
			{
				this.XboxHUD.Draw(spriteBatch);
				if (!this.XboxHUD.mStatsMode)
				{
					this.BuffManager.Draw(spriteBatch);
				}
			}
			if (this.mGunNameTime > 0f)
			{
				Vector2 position = base.GetScreenPosBottom();
				position = VerchickMath.CenterText(Global.StoreFont, position, this.mGun.Name);
				position.Y += 6f;
				float scale = (this.mGunNameTime > 1f) ? 1f : this.mGunNameTime;
				Shadow.DrawOutlinedString(spriteBatch, Global.StoreFont, this.mGun.Name, Color.Black * scale, this.HUDColor * scale, 1f, 0f, position);
				this.mGunNameTime -= 0.016666668f;
			}
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00041984 File Offset: 0x0003FB84
		private void DrawOverHead(SpriteBatch spriteBatch)
		{
			bool flag = false;
			float num = 0.033333335f;
			if (this.tile != null && this.tile.HasBottomWall())
			{
				flag = true;
			}
			if (Camera.ZoomedOut || flag)
			{
				this.mNameAlpha += num;
				if (this.mNameAlpha > 1f)
				{
					this.mNameAlpha = 1f;
				}
			}
			else
			{
				this.mNameAlpha -= num;
				if (this.mNameAlpha < 0f)
				{
					this.mNameAlpha = 0f;
				}
			}
			if (this.mNameAlpha >= 0f)
			{
				Vector2 screenPosCenter = base.GetScreenPosCenter();
				string gamerName = this.GamerName;
				Vector2 pos = VerchickMath.CenterText(Global.StoreFont, screenPosCenter, gamerName);
				pos.Y -= 34f;
				Shadow.DrawString(gamerName, Global.StoreFont, pos, 2, this.HUDColor * this.mNameAlpha, Color.Black * this.mNameAlpha, spriteBatch);
			}
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00041A70 File Offset: 0x0003FC70
		private void UpdateReloadTip(float elapsed)
		{
			if (this.mPlayerInfo == null)
			{
				return;
			}
			if (!this.mPlayerInfo.Local)
			{
				return;
			}
			if (this.mReloadTipCount < 3 && this.mGun != null && this.mGun.ammoType != AmmoType.MELEE)
			{
				if (this.mGun.DeltaClip() < 1f)
				{
					this.mReloadTipTimer += elapsed;
					if (this.mReloadTipTimer > 8f)
					{
						this.mReloadTipCount++;
						this.mReloadTipTimer = 0f;
						this.XboxHUD.mAlarm.ShowMessage("(Press " + InputManager.GetKeyString(ButtonPress.Reload, this.mIndex) + " to reload)", Color.White, 2f);
						return;
					}
				}
				else
				{
					this.mReloadTipTimer = 0f;
				}
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00041B44 File Offset: 0x0003FD44
		public void HealthPackTip()
		{
			if (this.mShownHPTip > 3)
			{
				return;
			}
			if (this.mPlayerInfo == null)
			{
				return;
			}
			if (!this.mPlayerInfo.Local)
			{
				return;
			}
			this.mShownHPTip++;
			this.XboxHUD.mAlarm.ShowMessage("(Press " + InputManager.GetKeyString(ButtonPress.HealthPack, this.mIndex) + " to use Health Pack!)", Color.White, 2f);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00041BB8 File Offset: 0x0003FDB8
		private void DEBUGDrawAggros(SpriteBatch spriteBatch)
		{
			foreach (Zombie zombie in Global.ZombieList)
			{
				Vector2 screenPosCenter = zombie.GetScreenPosCenter();
				spriteBatch.DrawString(Global.StoreFont, zombie.GetHighestAggroDEBUG().ToString(), screenPosCenter, Color.White);
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00041C2C File Offset: 0x0003FE2C
		public override void Damage(Shootable attacker, float amount, DamageType damageType, bool noGore, bool AOE, List<BulletModifier> mods = null)
		{
			if (Global.WaveMaster != null && !Global.WaveMaster.WaveRUNNING)
			{
				return;
			}
			this.Talleys.DamageTaken += amount;
			if (base.IAmOwnedByLocalPlayer)
			{
				base.Damage(attacker, amount, damageType, noGore, AOE, null);
			}
			if (this.DEAD)
			{
				return;
			}
			if (this.InvincibilityTimer.Running())
			{
				return;
			}
			if (!this.HUMAN || !base.IAmOwnedByLocalPlayer)
			{
				Global.MasterCache.CreateParticle(ParticleType.Blood, this.Position, this.Velocity);
				return;
			}
			SoundEngine.PlaySound(Player.mHurtSound, this.Position);
			Global.MasterCache.CreateParticle(ParticleType.Blood, this.Position, this.Velocity);
			float trueAmount = base.GetTrueAmount(amount, damageType, attacker);
			Global.MasterCache.particleSystem.AddCombatText(CombatTextType.Damage, ((int)trueAmount).ToString(), this.Position);
			float num = this.Health / this.SpecialProperties.MaxHealth;
			num = 1f - num;
			num = MathHelper.Clamp(num, 0.2f, 0.75f);
			if (this.mPlayerInfo != null && this.mPlayerInfo.ControllerIndex != -1)
			{
				Vibrate.VibrateController(this.mPlayerInfo.ControllerIndex, 0.5f, num);
			}
			if (this.Health <= 0f)
			{
				this.Death();
			}
			if (this.Health <= 25f)
			{
				if (this.Stats.HealthPacks > 0)
				{
					this.XboxHUD.mAlarm.ShowMessage("Press " + InputManager.GetKeyString(ButtonPress.HealthPack, this.mIndex) + " to Heal!", Color.DeepPink, 3f);
					return;
				}
				this.XboxHUD.mAlarm.ShowMessage(AlarmMessage.LOWHEALTH, Color.Red, 3f);
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00041DE4 File Offset: 0x0003FFE4
		public override void Heal(float amount, Shootable healer, bool ignoreHealBonus = false)
		{
			if (this.DEAD)
			{
				return;
			}
			if (amount <= 0f)
			{
				return;
			}
			if (this.Health < this.SpecialProperties.MaxHealth)
			{
				Vector3 position = this.Position;
				int num = (int)(amount / 5f);
				num++;
				num = Math.Min(10, num);
				for (int i = 0; i < num; i++)
				{
					position.X = this.Position.X + Global.RandomFloat(0f, 1f) - 0.5f;
					position.Z = this.Position.Z + Global.RandomFloat(0f, 1f) - 0.5f;
					Global.MasterCache.CreateParticle(ParticleType.Heal, position, Vector3.Zero);
				}
				position = this.Position;
				position.Y = 0.1f;
				if (amount > 14f)
				{
					SoundEngine.PlaySound(Player.mHealSound);
					RadialParticle obj = new RadialParticle(new Point(7, 32), 0.05f, 1.4f, 0.1f, position, this);
					Global.MasterCache.CreateObject(obj);
				}
				float num2 = amount;
				if (healer != null && !ignoreHealBonus)
				{
					num2 = amount * (healer.SpecialProperties.HealingDoneMod / 100f + 1f);
				}
				if (num2 > 1f)
				{
					Global.MasterCache.particleSystem.AddCombatText(CombatTextType.Heal, ((int)num2).ToString(), this.Position);
				}
			}
			base.Heal(amount, healer, ignoreHealBonus);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00041F4A File Offset: 0x0004014A
		public void SlowDown(float percent)
		{
			this.speedMod = percent;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00041F54 File Offset: 0x00040154
		public void LevelUp()
		{
			Vector3 position = this.Position;
			int num = 6;
			for (int i = 0; i < num; i++)
			{
				float speed = 0.025f;
				position.Y = (float)i * (1f / (float)num) + 0.1f;
				RadialParticle obj = new RadialParticle(new Point(6, 32), speed, 1f, 0.1f, position, this);
				Global.MasterCache.CreateObject(obj);
			}
			this.Stats.AddTalentPoints(1);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002F92B File Offset: 0x0002DB2B
		public override void Landed()
		{
			base.SquishMe(1.2f);
			base.Landed();
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00041FC9 File Offset: 0x000401C9
		public float GetHealthDelta()
		{
			return this.Health / this.SpecialProperties.MaxHealth;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00041FE0 File Offset: 0x000401E0
		public void SwitchGun(string label, bool fromNet)
		{
			if (this.mGun == null)
			{
				Terminal.WriteMessage("Warning: Switched from no gun?", MessageType.IMPORTANTEVENT);
				return;
			}
			string name = this.mGun.Name;
			Gun gun = this.mGun;
			if (this.HUMAN)
			{
				Terminal.WriteMessage(string.Concat(new object[]
				{
					"Player ",
					this.Index,
					": Switching gun from: ",
					this.mGun.Name,
					" - to: ",
					label
				}));
			}
			for (int i = 0; i < this.GetGunCount(); i++)
			{
				if (this.Guns[i].Name == label)
				{
					this.mGun = this.Guns[i];
					this.mWeaponIndex = i;
					if (this.mGun.Name != name)
					{
						gun.DeactivateGun();
						this.mGun.ActivateGun();
						this.mGunNameTime = 2f;
						this.AssertNetGunList();
					}
					Terminal.WriteMessage(string.Concat(new object[]
					{
						"Player ",
						this.Index,
						": Switched to: ",
						this.mGun.Name
					}));
					return;
				}
			}
			Terminal.WriteMessage("Warning: Gun " + label + " was not found.", MessageType.ERROR);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00042130 File Offset: 0x00040330
		public void SwitchGun(string label)
		{
			this.SwitchGun(label, false);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0004213C File Offset: 0x0004033C
		public void AddGun(Item item)
		{
			this.Guns[this.GetNextOpenGunSlot()] = new Gun(this, item.label);
			if (this.mGun == null)
			{
				this.mGun = this.Guns[0];
				this.mGun.ActivateGun();
				this.mGun.ForceReload();
			}
			this.AssertNetGunList();
			if (this.mPlayerInfo != null && !this.mSwappedWeaponsYet && this.mPlayerInfo.Local && this.GetGunCount() > 1)
			{
				string msg = string.Format("Press [{0}] or [{1}] to swap weapons.", InputManager.GetKeyForButtonPress(ButtonPress.SwapWeaponsNegative), InputManager.GetKeyForButtonPress(ButtonPress.SwapWeaponsPositive));
				this.XboxHUD.mAlarm.ShowMessage(msg, Color.White, 3f);
				this.mSwappedWeaponsYet = true;
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00042200 File Offset: 0x00040400
		public int GetNextOpenGunSlot()
		{
			for (int i = 0; i < Player.MAXGUNS; i++)
			{
				if (this.Guns[i] == null)
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0004222C File Offset: 0x0004042C
		public int GetGunCount()
		{
			int num = 0;
			for (int i = 0; i < Player.MAXGUNS; i++)
			{
				if (this.Guns[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0004225C File Offset: 0x0004045C
		public Gun AddGun(string name, bool NETSYNC = true)
		{
			Gun gun = new Gun(this, name);
			this.Guns[this.GetNextOpenGunSlot()] = gun;
			if (this.mGun == null)
			{
				this.mGun = this.Guns[0];
				this.mGun.ActivateGun();
				this.mGun.ForceReload();
			}
			if (this.mPlayerInfo != null && !this.mSwappedWeaponsYet && this.mPlayerInfo.Local && this.GetGunCount() > 1)
			{
				string msg = string.Format("Press [{0}] or [{1}]to swap weapons.", InputManager.GetKeyString(ButtonPress.SwapWeaponsNegative, this.mIndex), InputManager.GetKeyString(ButtonPress.SwapWeaponsPositive, this.mIndex));
				this.XboxHUD.mAlarm.ShowMessage(msg, Color.White, 3f);
			}
			if (NETSYNC)
			{
				this.AssertNetGunList();
			}
			return gun;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0004231C File Offset: 0x0004051C
		public Gun ReplaceGun(string name, int index)
		{
			Gun gun = new Gun(this, name);
			this.Guns[index] = gun;
			if (this.mGun == null)
			{
				this.mGun = this.Guns[0];
				this.mGun.ActivateGun();
				this.mGun.ForceReload();
			}
			this.AssertNetGunList();
			return gun;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0004236D File Offset: 0x0004056D
		public float DeltaClip()
		{
			return this.mGun.DeltaClip();
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0004237C File Offset: 0x0004057C
		public void RemoveGun(string name)
		{
			int num = -1;
			int i = 0;
			while (i < Player.MAXGUNS)
			{
				if (this.Guns[i] != null && name == this.Guns[i].Name)
				{
					this.Guns[i] = null;
					num = i;
					if (this.mWeaponIndex == i)
					{
						this.SwitchGun(this.Guns[0].Name, false);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (num != -1)
			{
				for (int j = num; j < Player.MAXGUNS - 1; j++)
				{
					this.Guns[j] = this.Guns[j + 1];
				}
				this.Guns[Player.MAXGUNS - 1] = null;
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0004241C File Offset: 0x0004061C
		private void FireBullet()
		{
			this.mRecentlyFired.Restart();
			if (InputManager.ButtonPressed(ButtonPress.Fire, this.Index, false))
			{
				this.mGun.Fire(this.mAimDir, true);
				return;
			}
			this.mGun.Fire(this.mAimDir, false);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0004246C File Offset: 0x0004066C
		public void FireBulletFromNet(Msg_GunFired payload)
		{
			this.mRecentlyFired.Restart();
			List<BulletModifier> list = new List<BulletModifier>();
			if (payload.Crit)
			{
				list.Add(BulletModifier.Crit);
			}
			this.mAimDir.X = payload.AimDirX;
			this.mAimDir.Y = payload.AimDirY;
			this.mGun.FireBulletFromNet(new Vector2(payload.AimDirX, payload.AimDirY), list, new Vector3(payload.FiredPositionX, 0f, payload.FiredPositionZ), payload.StartUID, payload.Seed, payload.BulletsRemainingInClip);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00042501 File Offset: 0x00040701
		public override void AddVelocity(float elapsed)
		{
			base.AddVelocity(elapsed);
			this.UpdateLevelCollisions();
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00042510 File Offset: 0x00040710
		public virtual void InitPlayer(CharacterStats stats, CharacterSettings settings, int uid)
		{
			this.Index = this.mIndex;
			base.OwnerIndex = this.mIndex;
			PlayerManager.GetPlayer(this.mIndex).PlayerObject = this;
			this.mPlayerInfo = PlayerManager.GetPlayer(this.mIndex);
			if (this.mPlayerInfo != null)
			{
				this.GamerName = this.mPlayerInfo.PersonaName;
			}
			Terminal.WriteMessage(string.Format("Init Player Index {0}: ControllerIndex = {1}, UsingController = {2}", this.mIndex, this.mPlayerInfo.ControllerIndex, this.mPlayerInfo.UsingController), MessageType.IMPORTANTEVENT);
			this.Stats = new PlayerStats(stats, settings, this);
			this.Stats.parent = this;
			this.BuffManager = new PlayerBuffManager(this);
			this.Guns = new Gun[Player.MAXGUNS];
			this.mGun = null;
			this.AddGun(settings.startingGun, true);
			this.mGun = this.Guns[0];
			this.AssertNetGunList();
			this.StartTextureCoord = this.Stats.CharSettings.texCoord;
			this.TextureCoord = this.StartTextureCoord;
			this.Stats.PostLoad();
			this.InitBaseSpecialProperties();
			this.FireUpdateProperties();
			base.ReInitHealth();
			this.Stats.AddMaxAmmo(AmmoType.ASSAULT, this.SpecialProperties.Ammo_Assault);
			this.Stats.AddMaxAmmo(AmmoType.SHELLS, this.SpecialProperties.Ammo_Shells);
			this.Stats.AddMaxAmmo(AmmoType.HEAVY, this.SpecialProperties.Ammo_Heavy);
			this.Stats.AddMaxAmmo(AmmoType.EXPLOSIVE, this.SpecialProperties.Ammo_Explosive);
			if (uid != -1)
			{
				base.UID = (ushort)this.Index;
				Terminal.WriteMessage("Player Object Initialized. UID: " + base.UID);
				return;
			}
			Terminal.WriteMessage("Fake Player Object Initialized. No UID.");
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x000426DB File Offset: 0x000408DB
		public void SavePlayer()
		{
			PlayerStatKeeper.SaveCharacterStats(this, this.Stats.MyStats);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x000426F0 File Offset: 0x000408F0
		public void AddStatEffect(StatusEffect eff)
		{
			if (eff == null)
			{
				return;
			}
			for (int i = 0; i < this.mStatusEffects.Length; i++)
			{
				if (this.mStatusEffects[i] != null && !eff.Stackable && this.mStatusEffects[i].Type == eff.Type)
				{
					this.mStatusEffects[i].Clear();
					this.mStatusEffects[i] = null;
					this.mStatusEffects[i] = eff;
					return;
				}
				if (this.mStatusEffects[i] == null)
				{
					this.mStatusEffects[i] = eff;
					return;
				}
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00042774 File Offset: 0x00040974
		public virtual void Death()
		{
			if (this.HUMAN)
			{
				this.XboxHUD.mAlarm.ClearMessages();
			}
			this.BuffManager.ParentKilled();
			SpecialProperties.ClearProps(ref this.BuffSpecialProps);
			this.SomethingChanged = true;
			this.Health = 0f;
			this.DEAD = true;
			SoundEngine.PlaySound("ze2_death", this.Position);
			if (this.mAccessoryObject != null)
			{
				this.mAccessoryObject.Invisible = true;
			}
			this.Position.Y = this.floorHeight;
			for (int i = 0; i < this.MinionList.Count; i++)
			{
				this.MinionList[i].DestroyObject();
			}
			if (Global.AllPlayersDead())
			{
				Global.Paused = true;
				ScreenFader.Fade(new ScreenFader.FadeDone(this.DeathFadeDone), 0.0075f);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00042847 File Offset: 0x00040A47
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x00042850 File Offset: 0x00040A50
		public int Index
		{
			get
			{
				return this.mIndex;
			}
			set
			{
				this.mIndex = value;
				base.OwnerIndex = value;
				int num = 1280;
				int num2 = 720;
				int num3 = 256;
				int num4 = 180;
				switch (this.mIndex)
				{
				case 0:
					this.HUDColor = new Color(136, 178, 229);
					this.HUDPosition = new Vector2(64f, (float)(Global.ScreenRect.Height - 64 - num4));
					break;
				case 1:
					this.HUDColor = new Color(255, 140, 140);
					this.HUDPosition = new Vector2((float)(num - 64 - num3), 64f);
					break;
				case 2:
					this.HUDColor = new Color(136, 229, 178);
					this.HUDPosition = new Vector2(64f, (float)(num2 - 64 - num4));
					break;
				case 3:
					this.HUDColor = new Color(229, 229, 136);
					this.HUDPosition = new Vector2((float)(num - 64 - num3), (float)(num2 - 64 - num4));
					break;
				}
				this.mDeathTexture = new Point(8 + this.mIndex, 8);
				this.XboxHUD = new XboxHUD(this);
				this.BuffManager = new PlayerBuffManager(this);
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000429AB File Offset: 0x00040BAB
		private void DeathFadeDone()
		{
			MenuManager.PushMenu(new DeathMenu());
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x000429B8 File Offset: 0x00040BB8
		public void Revive(bool reviving)
		{
			this.DEAD = false;
			this.Health = this.SpecialProperties.MaxHealth;
			this.TextureCoord = this.StartTextureCoord;
			this.mAccessoryObject.Invisible = false;
			this.Velocity = Vector3.Zero;
			this.InvincibilityTimer.Reset();
			this.InvincibilityTimer.Start();
			this.BuffManager.ClearBuffs();
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00042A21 File Offset: 0x00040C21
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x00042A29 File Offset: 0x00040C29
		public int Points
		{
			get
			{
				return this.mPointsNOT_USED;
			}
			set
			{
				this.mPointsNOT_USED = value;
				this.SaveAllPlayerData();
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00042A38 File Offset: 0x00040C38
		public bool IsGunUnlocked(string name)
		{
			return this.UnlockedGuns.Contains(name);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00042A4B File Offset: 0x00040C4B
		public bool IsHatUnlocked(string name)
		{
			return this.UnlockedHats.Contains(name);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00042A60 File Offset: 0x00040C60
		public void SaveAllPlayerData()
		{
			XMLSaverLoader.SaveObject<Player.PlayerSaveData>("PlayerData.xml", new Player.PlayerSaveData
			{
				GunsUnlocked = this.UnlockedGuns,
				HatsUnlocked = this.UnlockedHats,
				Points = this.Points
			});
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void LoadPlayerData()
		{
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00042AA8 File Offset: 0x00040CA8
		public bool OwnsGun(GunStats stats)
		{
			foreach (Gun gun in this.Guns)
			{
				if (gun != null && gun.stats.GunName == stats.GunName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00042AEC File Offset: 0x00040CEC
		public bool OwnsGun(string name)
		{
			foreach (Gun gun in this.Guns)
			{
				if (gun != null && gun.stats.GunName == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00042B2C File Offset: 0x00040D2C
		public Gun GetGun(string name)
		{
			foreach (Gun gun in this.Guns)
			{
				if (gun != null && gun.stats.GunName == name)
				{
					return gun;
				}
			}
			return null;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00042B6B File Offset: 0x00040D6B
		public void AddBuff(string name, Shootable att, string args)
		{
			this.BuffManager.AddBuff(name, att, args);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00042B7C File Offset: 0x00040D7C
		private void CheckCameraBounds()
		{
			if (!this.HUMAN || GameManager.PLANONEDITING || !base.IAmOwnedByLocalPlayer)
			{
				return;
			}
			if (this.PlayerInputLocked)
			{
				return;
			}
			Vector2 screenPosition = VerchickMath.GetScreenPosition(this.Position);
			float num = 0.07333334f;
			int num2 = 30;
			if (screenPosition.X < (float)num2)
			{
				this.Position.X = this.Position.X + num;
				this.UpdateLevelCollisions();
			}
			if (screenPosition.X > (float)(Global.ScreenRect.Right - num2))
			{
				this.Position.X = this.Position.X - num;
				this.UpdateLevelCollisions();
			}
			if (screenPosition.Y < (float)num2)
			{
				this.Position.Z = this.Position.Z + num;
				this.UpdateLevelCollisions();
			}
			if (screenPosition.Y > (float)(Global.ScreenRect.Bottom - num2))
			{
				this.Position.Z = this.Position.Z - num;
				this.UpdateLevelCollisions();
			}
			if (screenPosition.X < (float)(-(float)num2) || screenPosition.X > (float)(Global.ScreenRect.Right + num2) || screenPosition.Y < (float)(-(float)num2) || screenPosition.Y > (float)(Global.ScreenRect.Bottom + num2))
			{
				this.mOffScreenTime += 0.016666668f;
				if (this.mOffScreenTime >= 1f)
				{
					List<Player> list = new List<Player>();
					foreach (Player player in Global.PlayerList)
					{
						if (player != this)
						{
							list.Add(player);
						}
					}
					int index = Global.rand.Next(0, list.Count);
					if (list.Count == 0)
					{
						return;
					}
					this.Position = list[index].Position;
					return;
				}
			}
			else
			{
				this.mOffScreenTime = 0f;
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00042D48 File Offset: 0x00040F48
		public void AddMinion(Minion m)
		{
			this.MinionList.Add(m);
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00042D56 File Offset: 0x00040F56
		public List<Minion> GetMinionList
		{
			get
			{
				return this.MinionList;
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00042D5E File Offset: 0x00040F5E
		public void RemoveMinion(Minion m)
		{
			if (this.MinionList.Contains(m))
			{
				this.MinionList.Remove(m);
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00042D7C File Offset: 0x00040F7C
		public int GetMinionIndex()
		{
			int num = this.Index * 10 + this.MinionCount + 10;
			using (List<Minion>.Enumerator enumerator = this.MinionList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Index == num)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00042DE8 File Offset: 0x00040FE8
		public void RemoveMinions()
		{
			foreach (Minion minion in this.MinionList)
			{
				minion.FULL_DESTROY();
			}
			this.MinionList.Clear();
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00042E44 File Offset: 0x00041044
		private void UpdateDances(float elapsed)
		{
			if (this.HUMAN && base.IAmOwnedByLocalPlayer && !this.PlayerInputLocked && !MasterStore.Active)
			{
				if (this.mPlayerInfo.UsingController)
				{
					if (InputManager.New_DPadUp_Pressed(this.mPlayerInfo.ControllerIndex))
					{
						this.mDanceState = DanceState.Squish;
						this.ResetDance();
					}
					if (InputManager.New_DPadLeft_Pressed(this.mPlayerInfo.ControllerIndex))
					{
						this.mDanceState = DanceState.Flip;
						this.ResetDance();
					}
					if (InputManager.New_DPadRight_Pressed(this.mPlayerInfo.ControllerIndex))
					{
						this.mDanceState = DanceState.Twirl;
						this.ResetDance();
					}
				}
				else
				{
					if (InputManager.ButtonHeld(Keys.LeftShift, 0) && InputManager.ButtonPressed(Keys.D1, 0))
					{
						this.mDanceState = DanceState.Squish;
						this.ResetDance();
					}
					if (InputManager.ButtonHeld(Keys.LeftShift, 0) && InputManager.ButtonPressed(Keys.D2, 0))
					{
						this.mDanceState = DanceState.Flip;
						this.ResetDance();
					}
					if (InputManager.ButtonHeld(Keys.LeftShift, 0) && InputManager.ButtonPressed(Keys.D3, 0))
					{
						this.mDanceState = DanceState.Twirl;
						this.ResetDance();
					}
				}
			}
			if (base.IAmOwnedByLocalPlayer && this.mDanceState != DanceState.NONE && this.HUMAN && (this.mMovement.X != 0f || this.mMovement.Y != 0f))
			{
				this.mDanceState = DanceState.NONE;
				this.ResetDance();
			}
			if (this.mDanceState == DanceState.Squish)
			{
				base.SquishMe(1.2f, false);
			}
			if (this.mDanceState == DanceState.Flip)
			{
				this.Jump();
				this.XRotation += elapsed * 8f;
				this.mGunObject.Invisible = true;
				this.mAccessoryObject.Invisible = true;
			}
			if (this.mDanceState == DanceState.Twirl)
			{
				this.Jump();
				this.YRotation += elapsed * 12f;
				this.mGunObject.Invisible = true;
				this.mAccessoryObject.Invisible = true;
			}
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00043028 File Offset: 0x00041228
		private void ResetDance()
		{
			this.XRotation = 0f;
			this.YRotation = 0f;
			if (this.mAccessoryObject != null)
			{
				this.mAccessoryObject.Invisible = false;
			}
			if (this.mGunObject != null)
			{
				this.mGunObject.Invisible = false;
			}
			if (base.IAmOwnedByLocalPlayer && this.HUMAN)
			{
				byte state = (byte)this.mDanceState;
				NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.Dance);
				netMessage.Payload = new Msg_Dance
				{
					State = state,
					Index = (byte)this.Index
				};
				NetworkManager.SendMessage(netMessage, SendType.Reliable);
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000430BC File Offset: 0x000412BC
		public void SetDanceState(byte state)
		{
			this.mDanceState = (DanceState)state;
			this.ResetDance();
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x000430D8 File Offset: 0x000412D8
		protected override void SetSyncTimes()
		{
			this.mTotalSyncTime = 0.1f;
			this.mTotalFullSyncTime = 1f;
			this.mDoISync = true;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000430F8 File Offset: 0x000412F8
		protected override void SendSync(NetMessage msg)
		{
			Msg_PlayerUpdate msg_PlayerUpdate = new Msg_PlayerUpdate();
			Msg_GameObjectUpdate.FillIn(msg_PlayerUpdate, this.Position, this.mLastNetPosition, false);
			if (this.mLastNetHealth != this.Health)
			{
				msg_PlayerUpdate.HealthChanged = true;
				msg_PlayerUpdate.Health = Convert.ToUInt16(this.Health);
			}
			msg.Payload = msg_PlayerUpdate;
			this.mLastNetPosition = this.Position;
			this.mLastNetHealth = this.Health;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00043164 File Offset: 0x00041364
		protected override void SendFullSync(NetMessage msg)
		{
			Msg_PlayerUpdate msg_PlayerUpdate = new Msg_PlayerUpdate();
			Msg_GameObjectUpdate.FillIn(msg_PlayerUpdate, this.Position, this.mLastNetPosition, true);
			msg_PlayerUpdate.HealthChanged = true;
			msg_PlayerUpdate.Health = Convert.ToUInt16(this.Health);
			msg.Payload = msg_PlayerUpdate;
			this.mLastNetPosition = this.Position;
			this.mLastNetHealth = this.Health;
			this.SendHUDData();
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000431C8 File Offset: 0x000413C8
		protected override void ReceiveSync(NetPayload incoming)
		{
			Msg_GameObjectUpdate msg_GameObjectUpdate = incoming as Msg_GameObjectUpdate;
			Vector2 vector = new Vector2(this.Position.X, this.Position.Z);
			if (msg_GameObjectUpdate.PositionXChanged)
			{
				vector.X = msg_GameObjectUpdate.PositionX;
			}
			if (msg_GameObjectUpdate.PositionZChanged)
			{
				vector.Y = msg_GameObjectUpdate.PositionZ;
			}
			if (!VerchickMath.WithinDistance(base.TwoDPosition(), vector, Player.TELEPORT_THRESHOLD))
			{
				this.Position.X = vector.X;
				this.Position.Z = vector.Y;
			}
			else
			{
				this.mNetHomingPosition = vector;
				this.mMovement = VerchickMath.DirectionToVector2(base.TwoDPosition(), vector);
			}
			Msg_PlayerUpdate msg_PlayerUpdate = incoming as Msg_PlayerUpdate;
			if (msg_PlayerUpdate.HealthChanged)
			{
				this.Health = Convert.ToSingle(msg_PlayerUpdate.Health);
				if (this.Health <= 0f && !this.DEAD)
				{
					this.Death();
				}
			}
			if (this.Health <= 0f && !this.DEAD)
			{
				this.Death();
			}
			if (this.DEAD && msg_PlayerUpdate.Health > 0)
			{
				this.Revive(true);
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x000432E4 File Offset: 0x000414E4
		public void AssertNetGunList()
		{
			if (!base.IAmOwnedByLocalPlayer || !this.HUMAN)
			{
				return;
			}
			NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.WeaponList);
			Msg_GunListUpdate msg_GunListUpdate = new Msg_GunListUpdate();
			msg_GunListUpdate.FillIn(this.Guns, this.mWeaponIndex);
			msg_GunListUpdate.PlayerIndex = (byte)this.Index;
			netMessage.Payload = msg_GunListUpdate;
			NetworkManager.SendMessage(netMessage, SendType.Reliable);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0004333A File Offset: 0x0004153A
		public void RetrieveGunList(Msg_GunListUpdate payload)
		{
			payload.Retrieve(this);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00043344 File Offset: 0x00041544
		public void SendREADYState()
		{
			NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.PlayerWaveReadyUpdate);
			netMessage.Payload = new Msg_PlayerReadyStateUpdate
			{
				READY = this.READY,
				Index = (byte)this.Index
			};
			NetworkManager.SendMessage(netMessage, SendType.Reliable);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00043384 File Offset: 0x00041584
		public void SendWaveStats()
		{
			NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.PlayerWaveStats);
			netMessage.Payload = new Msg_PlayerWaveStats
			{
				DamageDealt = this.Talleys.DamageDealt,
				DamageTaken = this.Talleys.DamageTaken,
				MinionDamageDealt = this.Talleys.MinionDamageDealt,
				HealingDone = this.Talleys.HealingDone,
				PlayerIndex = (byte)this.Index
			};
			NetworkManager.SendMessage(netMessage, SendType.Reliable);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x000433FC File Offset: 0x000415FC
		public void GetWaveStats(Msg_PlayerWaveStats stats)
		{
			this.Talleys.DamageDealt = stats.DamageDealt;
			this.Talleys.DamageTaken = stats.DamageTaken;
			this.Talleys.MinionDamageDealt = stats.MinionDamageDealt;
			this.Talleys.HealingDone = stats.HealingDone;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00043450 File Offset: 0x00041650
		public void SendHUDData()
		{
			if (!base.IAmOwnedByLocalPlayer || !this.HUMAN)
			{
				return;
			}
			Msg_PlayerHUDUpdate msg_PlayerHUDUpdate = new Msg_PlayerHUDUpdate();
			msg_PlayerHUDUpdate.UID = base.UID;
			msg_PlayerHUDUpdate.HealthPackChanged = (this.mLastHealthPack != this.Stats.HealthPacks);
			msg_PlayerHUDUpdate.AssaultChanged = (this.mLastAssault != this.Stats.GetAmmo(AmmoType.ASSAULT));
			msg_PlayerHUDUpdate.ShellsChanged = (this.mLastShells != this.Stats.GetAmmo(AmmoType.SHELLS));
			msg_PlayerHUDUpdate.HeavyChanged = (this.mLastHeavy != this.Stats.GetAmmo(AmmoType.HEAVY));
			msg_PlayerHUDUpdate.ExplosiveChanged = (this.mLastExplosive != this.Stats.GetAmmo(AmmoType.EXPLOSIVE));
			msg_PlayerHUDUpdate.MoneyChanged = (this.mLastMoney != (ushort)this.Stats.GetMoney());
			if (msg_PlayerHUDUpdate.HealthPackChanged)
			{
				msg_PlayerHUDUpdate.HealthPack = (byte)this.Stats.HealthPacks;
			}
			if (msg_PlayerHUDUpdate.AssaultChanged)
			{
				msg_PlayerHUDUpdate.Assault = (ushort)this.Stats.GetAmmo(AmmoType.ASSAULT);
			}
			if (msg_PlayerHUDUpdate.ShellsChanged)
			{
				msg_PlayerHUDUpdate.Shells = (ushort)this.Stats.GetAmmo(AmmoType.SHELLS);
			}
			if (msg_PlayerHUDUpdate.HeavyChanged)
			{
				msg_PlayerHUDUpdate.Heavy = (ushort)this.Stats.GetAmmo(AmmoType.HEAVY);
			}
			if (msg_PlayerHUDUpdate.ExplosiveChanged)
			{
				msg_PlayerHUDUpdate.Explosive = (ushort)this.Stats.GetAmmo(AmmoType.EXPLOSIVE);
			}
			if (msg_PlayerHUDUpdate.MoneyChanged)
			{
				msg_PlayerHUDUpdate.Money = (ushort)this.Stats.GetMoney();
			}
			this.mLastAssault = this.Stats.GetAmmo(AmmoType.ASSAULT);
			this.mLastShells = this.Stats.GetAmmo(AmmoType.SHELLS);
			this.mLastHeavy = this.Stats.GetAmmo(AmmoType.HEAVY);
			this.mLastExplosive = this.Stats.GetAmmo(AmmoType.EXPLOSIVE);
			this.mLastHealthPack = this.Stats.HealthPacks;
			this.mLastMoney = (ushort)this.Stats.GetMoney();
			NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.PlayerHUDUpdate);
			netMessage.Payload = msg_PlayerHUDUpdate;
			NetworkManager.SendMessage(netMessage, SendType.Unreliable);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0004364C File Offset: 0x0004184C
		public void ReceiveHUDData(Msg_PlayerHUDUpdate hud)
		{
			if (hud.HealthPackChanged)
			{
				this.Stats.HealthPacks = (int)hud.HealthPack;
			}
			if (hud.AssaultChanged)
			{
				this.Stats.SetAmmo(AmmoType.ASSAULT, (int)hud.Assault);
			}
			if (hud.ShellsChanged)
			{
				this.Stats.SetAmmo(AmmoType.SHELLS, (int)hud.Shells);
			}
			if (hud.HeavyChanged)
			{
				this.Stats.SetAmmo(AmmoType.HEAVY, (int)hud.Heavy);
			}
			if (hud.ExplosiveChanged)
			{
				this.Stats.SetAmmo(AmmoType.EXPLOSIVE, (int)hud.Explosive);
			}
			if (hud.MoneyChanged)
			{
				this.Stats.SetMoney((float)hud.Money);
			}
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x000436F4 File Offset: 0x000418F4
		public static Color IndexColor(int index)
		{
			Color white = Color.White;
			switch (index)
			{
			case 0:
				white = new Color(136, 178, 229);
				break;
			case 1:
				white = new Color(255, 140, 140);
				break;
			case 2:
				white = new Color(136, 229, 178);
				break;
			case 3:
				white = new Color(229, 229, 136);
				break;
			}
			return white;
		}

		// Token: 0x0400087A RID: 2170
		public Facing mFacing = Facing.Down;

		// Token: 0x0400087B RID: 2171
		public Vector2 mAimDir;

		// Token: 0x0400087C RID: 2172
		public Vector2 mMovement = new Vector2(0f, 0f);

		// Token: 0x0400087D RID: 2173
		public GameObject mGunObject;

		// Token: 0x0400087E RID: 2174
		public GameObject mAccessoryObject;

		// Token: 0x0400087F RID: 2175
		public GameObject mTargetReticule;

		// Token: 0x04000880 RID: 2176
		public SpecialProperties TalentSpecProps = new SpecialProperties();

		// Token: 0x04000881 RID: 2177
		public SpecialProperties HatSpecProps = new SpecialProperties();

		// Token: 0x04000882 RID: 2178
		private KeyboardState mPrevKeyState;

		// Token: 0x04000883 RID: 2179
		public Gun[] Guns;

		// Token: 0x04000884 RID: 2180
		public Gun mGun;

		// Token: 0x04000885 RID: 2181
		public bool InvisGun;

		// Token: 0x04000886 RID: 2182
		public PlayerStats Stats;

		// Token: 0x04000887 RID: 2183
		protected PlayerInfo mPlayerInfo;

		// Token: 0x04000888 RID: 2184
		public XboxHUD XboxHUD;

		// Token: 0x04000889 RID: 2185
		public StatusEffect[] mStatusEffects;

		// Token: 0x0400088A RID: 2186
		public Color HUDColor;

		// Token: 0x0400088B RID: 2187
		public int mIndex;

		// Token: 0x0400088C RID: 2188
		public int PositionIndex;

		// Token: 0x0400088D RID: 2189
		public Point StartTextureCoord;

		// Token: 0x0400088E RID: 2190
		private int mWeaponIndex;

		// Token: 0x0400088F RID: 2191
		private bool mSwappedWeaponsYet;

		// Token: 0x04000890 RID: 2192
		public bool READY;

		// Token: 0x04000891 RID: 2193
		public Point AccessoryTexCoord = new Point(63, 63);

		// Token: 0x04000892 RID: 2194
		public string AccessoryName = "None";

		// Token: 0x04000893 RID: 2195
		public float speedMod = 1f;

		// Token: 0x04000894 RID: 2196
		public bool HUMAN = true;

		// Token: 0x04000895 RID: 2197
		public Vector2 HUDPosition;

		// Token: 0x04000896 RID: 2198
		public string GamerName = "TestPC";

		// Token: 0x04000897 RID: 2199
		public Gamer Gamer;

		// Token: 0x04000898 RID: 2200
		public static int MAXGUNS = 7;

		// Token: 0x04000899 RID: 2201
		public bool DEAD;

		// Token: 0x0400089A RID: 2202
		public bool FIRING;

		// Token: 0x0400089B RID: 2203
		private Timer mRecentlyFired = new Timer(2f);

		// Token: 0x0400089C RID: 2204
		private Point mDeathTexture = new Point(0, 0);

		// Token: 0x0400089D RID: 2205
		public Timer InvincibilityTimer = new Timer(15f);

		// Token: 0x0400089E RID: 2206
		public GamerData PlayerData;

		// Token: 0x0400089F RID: 2207
		public Ability Ability;

		// Token: 0x040008A0 RID: 2208
		public float ReloadSpeedMod = 1f;

		// Token: 0x040008A1 RID: 2209
		public float ZombieKillMoneyMod = 1f;

		// Token: 0x040008A2 RID: 2210
		public List<string> UnlockedGuns = new List<string>();

		// Token: 0x040008A3 RID: 2211
		public List<string> UnlockedHats = new List<string>();

		// Token: 0x040008A4 RID: 2212
		public Timer LockInput = new Timer(0.3f);

		// Token: 0x040008A5 RID: 2213
		public bool PlayerInputLocked;

		// Token: 0x040008A6 RID: 2214
		private int mPointsNOT_USED;

		// Token: 0x040008A7 RID: 2215
		private int mFacingOffset;

		// Token: 0x040008A8 RID: 2216
		private float mReadyCooldown;

		// Token: 0x040008A9 RID: 2217
		private List<Minion> MinionList = new List<Minion>();

		// Token: 0x040008AA RID: 2218
		public int MinionCount = 1;

		// Token: 0x040008AB RID: 2219
		public bool LOADED;

		// Token: 0x040008AC RID: 2220
		public WaveTalleys Talleys = new WaveTalleys();

		// Token: 0x040008AD RID: 2221
		public GameTalleys GameTalleys = new GameTalleys();

		// Token: 0x040008AE RID: 2222
		public List<DataString> DataStrings = new List<DataString>();

		// Token: 0x040008AF RID: 2223
		private float mGunNameTime;

		// Token: 0x040008B0 RID: 2224
		private float mTimeInNoPath;

		// Token: 0x040008B1 RID: 2225
		private float mMousewheelTimer;

		// Token: 0x040008B2 RID: 2226
		public bool CHARSELECT;

		// Token: 0x040008B3 RID: 2227
		private static string mJumpSound = "ze2_jump";

		// Token: 0x040008B4 RID: 2228
		private static string mHealSound = "ze2_heal";

		// Token: 0x040008B5 RID: 2229
		private static string mHurtSound = "ze2_hurt";

		// Token: 0x040008B6 RID: 2230
		private float mNameAlpha;

		// Token: 0x040008B7 RID: 2231
		private float mReloadTipTimer;

		// Token: 0x040008B8 RID: 2232
		private int mReloadTipCount;

		// Token: 0x040008B9 RID: 2233
		private int mShownHPTip;

		// Token: 0x040008BA RID: 2234
		private float mOffScreenTime;

		// Token: 0x040008BB RID: 2235
		private DanceState mDanceState = DanceState.NONE;

		// Token: 0x040008BC RID: 2236
		private Vector3 mLastNetPosition = Vector3.Zero;

		// Token: 0x040008BD RID: 2237
		private Vector2 mNetHomingPosition = Vector2.Zero;

		// Token: 0x040008BE RID: 2238
		private int mLastHealthPack;

		// Token: 0x040008BF RID: 2239
		private int mLastAssault;

		// Token: 0x040008C0 RID: 2240
		private int mLastShells;

		// Token: 0x040008C1 RID: 2241
		private int mLastHeavy;

		// Token: 0x040008C2 RID: 2242
		private int mLastExplosive;

		// Token: 0x040008C3 RID: 2243
		private ushort mLastMoney;

		// Token: 0x040008C4 RID: 2244
		private static float TELEPORT_THRESHOLD = 2f;

		// Token: 0x040008C5 RID: 2245
		private float mLastNetHealth = 100f;

		// Token: 0x04000E83 RID: 3715
		public bool autofire;

		// Token: 0x0200021B RID: 539
		public struct PlayerSaveData
		{
			// Token: 0x04000E38 RID: 3640
			public List<string> GunsUnlocked;

			// Token: 0x04000E39 RID: 3641
			public List<string> HatsUnlocked;

			// Token: 0x04000E3A RID: 3642
			public int Points;
		}
	}
}
