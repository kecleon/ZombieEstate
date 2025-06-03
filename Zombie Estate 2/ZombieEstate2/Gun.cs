using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x0200010E RID: 270
	public class Gun
	{
		// Token: 0x0600073D RID: 1853 RVA: 0x00037C38 File Offset: 0x00035E38
		public Gun(Player parent, string name)
		{
			this.Name = name;
			this.stats = GunStatsLoader.GetStats(name);
			this.currentTexCoord.X = this.stats.GunXCoord;
			this.currentTexCoord.Y = this.stats.GunYCoord;
			this.parent = parent;
			this.flashTime = new Timer(0.05f);
			this.shootTime = new Timer(0.1f);
			this.reloadTime = new Timer(this.stats.GunProperties[this.level].ReloadTime * parent.ReloadSpeedMod);
			this.semiTime = new Timer(0.1f);
			this.bulletsInClip = this.stats.GunProperties[this.level].BulletsInClip;
			this.prevBulletsInClip = this.stats.GunProperties[this.level].BulletsInClip;
			this.OriginTex = this.stats.GetOrigin(this.level);
			this.ammoType = this.stats.AmmoType;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00037DEC File Offset: 0x00035FEC
		public void UpdateGun(float elapsed)
		{
			if (this.mOutOfAmmo > 0f)
			{
				this.mOutOfAmmo -= 0.016666668f;
			}
			if (this.InvisTimer > 0f)
			{
				this.InvisTimer -= elapsed;
				if (this.InvisTimer <= 0f && !this.Swinging)
				{
					this.parent.InvisGun = false;
				}
			}
			this.OriginTex = this.stats.GetOrigin(this.level);
			if (this.parent.mFacing != Facing.Up)
			{
				this.parent.mGunObject.TextureCoord.X = this.currentTexCoord.X;
			}
			this.UpdateSwinging(elapsed);
			if (this.flashTime.Expired() || !this.flashTime.Running())
			{
				this.currentTexCoord.X = this.OriginTex.X;
			}
			if (this.reloadTime.Expired())
			{
				this.ForceReload();
			}
			this.UpdateReloadTimer();
			this.charging = false;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00037EF0 File Offset: 0x000360F0
		public void ForceReload()
		{
			this.bulletsInClip += this.parent.Stats.GetMaxClip(this.ammoType, this.stats.GunProperties[this.level].BulletsInClip - this.bulletsInClip);
			this.reloadTime.Reset();
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00037F50 File Offset: 0x00036150
		public bool Fire(Vector2 aimDir, bool pressed)
		{
			if (float.IsNaN(aimDir.X) || float.IsNaN(aimDir.Y))
			{
				if (!this.stats.Minion)
				{
					return false;
				}
				aimDir = Vector2.Zero;
			}
			if (this.bulletsInClip == 0 && !this.parent.Stats.HasABullet(this.ammoType))
			{
				if (this.mOutOfAmmo <= 0f)
				{
					FloatingIndicator obj = new FloatingIndicator(this.parent, this.GetOutOfAmmoTex(this.ammoType), 1f);
					Global.MasterCache.CreateObject(obj);
					this.mOutOfAmmo = 1f;
					this.parent.XboxHUD.mAlarm.ShowMessage(AlarmMessage.OUTOFAMMO, Color.Red, 1f);
					SoundEngine.PlaySound(Gun.clicky, 0.8f);
				}
				return false;
			}
			if (this.reloadTime.Running())
			{
				return false;
			}
			this.GetModsForAttack(Global.rand);
			ushort maxValue = ushort.MaxValue;
			int seed = Global.rand.Next();
			bool flag = this.FireBullet(aimDir, false, this.Mods, ref maxValue, seed);
			if (flag && this.parent.IAmOwnedByLocalPlayer)
			{
				this.SendGunFireOverNet(seed, maxValue, this.Mods.Contains(BulletModifier.Crit), aimDir);
			}
			return flag;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00038089 File Offset: 0x00036289
		private void GetModsForAttack(Random rand)
		{
			this.Mods.Clear();
			if (rand.NextDouble() < (double)(this.parent.SpecialProperties.CritChance / 100f))
			{
				this.Mods.Add(BulletModifier.Crit);
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000380C4 File Offset: 0x000362C4
		public bool FireBullet(Vector2 aimDir, bool semiPressed, List<BulletModifier> mods, ref ushort StartUID, int seed)
		{
			if (this.stats.GunProperties[this.level].NumberOfBulletsFired == 0 && this.ammoType != AmmoType.MELEE && !this.stats.Minion)
			{
				throw new Exception("HEY! 0 bullets!?");
			}
			if (this.chargeIndicator != null)
			{
				this.chargeIndicator.DestroyObject();
				this.chargeIndicator = null;
			}
			if ((this.ammoType == AmmoType.MELEE || this.bulletsInClip > 0 || this.stats.GunProperties[this.level].BulletsInClip == -1) && (this.shootTime.Expired() || !this.shootTime.Running()))
			{
				if (this.stats.GunProperties[this.level].BulletsCostToFire > 1)
				{
					int num = Math.Max(1, (int)((float)this.stats.GunProperties[this.level].BulletsCostToFire * (1f - this.parent.SpecialProperties.NumBulletsMod / 100f)));
					if (num > this.bulletsInClip)
					{
						if (this.parent.Stats.GetAmmoIncludingClip(this.ammoType) > num)
						{
							this.ReloadIfNeeded();
							return false;
						}
						this.parent.XboxHUD.mAlarm.ShowMessage(AlarmMessage.AMMO + num + AlarmMessage.AMMO2, Color.Red, 2f);
						return false;
					}
					else if (this.stats.AmmoType != AmmoType.MELEE)
					{
						this.bulletsInClip -= num - 1;
					}
				}
				this.shootdir.X = aimDir.X;
				this.shootdir.Z = aimDir.Y;
				Vector3 vector = this.parent.Position + new Vector3(0f, -0.2f, 0f);
				Vector3 vector2;
				if (this.FireEndOfBarrel)
				{
					vector2 = vector + this.shootdir / 2f;
				}
				else
				{
					vector2 = vector;
				}
				this.CreateBullet(ref this.shootdir, ref vector2, this.chargeTime, this.parent, mods, ref StartUID, seed);
				float num2 = 1f;
				if (this.parent is Minion)
				{
					num2 = 0.4f;
				}
				else if (!this.parent.IAmOwnedByLocalPlayer)
				{
					num2 = 0.5f;
				}
				if (string.IsNullOrEmpty(this.stats.GunProperties[this.level].SoundName))
				{
					SoundEngine.PlaySound("ze2_pistol1", num2);
				}
				else
				{
					SoundEngine.PlaySound(this.stats.GunProperties[this.level].SoundName, this.parent.Position, this.stats.GunProperties[this.level].PitchModLow, this.stats.GunProperties[this.level].PitchModHigh, num2);
				}
				this.currentTexCoord.X = this.OriginTex.X + 1;
				this.flashTime.Start();
				this.FireParticles();
				if (this.stats.AmmoType != AmmoType.MELEE)
				{
					this.bulletsInClip--;
				}
				this.shootTime.mTotalTime = this.stats.GunProperties[this.level].ShotTime * (1f - this.parent.SpecialProperties.ShotTimeMod / 100f);
				this.shootTime.Start();
				return true;
			}
			if (this.bulletsInClip <= 0 && this.ammoType != AmmoType.MELEE && this.stats.GunProperties[this.level].BulletsInClip != -1)
			{
				if (!this.reloadTime.Running() && !this.reloadTime.Expired() && this.parent.Stats.HasABullet(this.ammoType))
				{
					this.reloadTime.mTotalTime = this.stats.GunProperties[this.level].ReloadTime * (1f - this.parent.SpecialProperties.ReloadTimeMod / 100f);
					this.reloadTime.Start();
					if (this.reloadIndicator != null)
					{
						this.reloadIndicator.DestroyObject();
					}
					this.reloadIndicator = new FloatingIndicator(this.parent, "Reload");
					Global.MasterCache.CreateObject(this.reloadIndicator);
					if (this.parent.IAmOwnedByLocalPlayer)
					{
						NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.PlayerReloaded);
						netMessage.Payload = new Msg_GenericUID
						{
							UID = this.parent.UID
						};
						NetworkManager.SendMessage(netMessage, SendType.Reliable);
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00038570 File Offset: 0x00036770
		public bool FireBulletFromNet(Vector2 aimDir, List<BulletModifier> mods, Vector3 startPos, ushort startUID, int seed, ushort bulletsRemainingInClip)
		{
			this.shootdir.X = aimDir.X;
			this.shootdir.Z = aimDir.Y;
			this.shootdir.Y = 0f;
			int numberOfBulletsFired = this.stats.GunProperties[this.level].NumberOfBulletsFired;
			float accuracy = this.stats.GunProperties[this.level].Accuracy;
			Vector3 vector = startPos + new Vector3(0f, -0.2f, 0f);
			Vector3 vector2;
			if (this.FireEndOfBarrel)
			{
				vector2 = vector + this.shootdir / 2f;
			}
			else
			{
				vector2 = vector;
			}
			this.CreateBullet(ref this.shootdir, ref vector2, this.chargeTime, this.parent, mods, ref startUID, seed);
			float num = 1f;
			if (this.parent is Minion)
			{
				num = 0.4f;
			}
			else if (!this.parent.IAmOwnedByLocalPlayer)
			{
				num = 0.5f;
			}
			if (string.IsNullOrEmpty(this.stats.GunProperties[this.level].SoundName))
			{
				SoundEngine.PlaySound("ze2_pistol1", num);
			}
			else
			{
				SoundEngine.PlaySound(this.stats.GunProperties[this.level].SoundName, this.parent.Position, this.stats.GunProperties[this.level].PitchModLow, this.stats.GunProperties[this.level].PitchModHigh, num);
			}
			if (this.ammoType != AmmoType.MELEE)
			{
				this.bulletsInClip = (int)bulletsRemainingInClip;
			}
			this.currentTexCoord.X = this.OriginTex.X + 1;
			this.flashTime.Start();
			this.FireParticles();
			if (this.parent != null && this.parent is Minion)
			{
				Minion minion = this.parent as Minion;
				if (!minion.Moving)
				{
					minion.ShotCount--;
					if (minion.ShotCount <= 0)
					{
						minion.DestroyObject();
					}
				}
			}
			return true;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00038784 File Offset: 0x00036984
		public void ReloadIfNeeded()
		{
			if (this.bulletsInClip < this.stats.GunProperties[this.level].BulletsInClip)
			{
				if (!this.reloadTime.Running() && !this.reloadTime.Expired() && this.parent.Stats.HasABullet(this.ammoType))
				{
					if (this.parent.IAmOwnedByLocalPlayer)
					{
						NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.PlayerReloaded);
						netMessage.Payload = new Msg_GenericUID
						{
							UID = this.parent.UID
						};
						NetworkManager.SendMessage(netMessage, SendType.Reliable);
					}
					this.parent.SendHUDData();
					this.VisualReload();
				}
				return;
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00038830 File Offset: 0x00036A30
		public void VisualReload()
		{
			this.reloadTime.mTotalTime = this.stats.GunProperties[this.level].ReloadTime * (1f - this.parent.SpecialProperties.ReloadTimeMod / 100f);
			this.reloadTime.Start();
			if (this.reloadIndicator != null)
			{
				this.reloadIndicator.DestroyObject();
			}
			this.reloadIndicator = new FloatingIndicator(this.parent, "Reload");
			Global.MasterCache.CreateObject(this.reloadIndicator);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x000388C5 File Offset: 0x00036AC5
		public void ReloadIfEmpty()
		{
			if (this.bulletsInClip < 0)
			{
				this.ReloadIfNeeded();
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x000388D6 File Offset: 0x00036AD6
		public bool Reloading
		{
			get
			{
				return this.reloadTime.Running();
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x000388E4 File Offset: 0x00036AE4
		public void UpdateReloadTimer()
		{
			if (this.reloadTime.Running())
			{
				if (this.reloadIndicator != null)
				{
					this.reloadIndicator.UpdateData(this.reloadTime.Delta());
					return;
				}
			}
			else if (this.reloadIndicator != null)
			{
				this.reloadIndicator.DestroyObject();
				this.reloadIndicator = null;
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00038938 File Offset: 0x00036B38
		private void CreateBullet(ref Vector3 dir, ref Vector3 pos, float chargeTime, Shootable parent, List<BulletModifier> mods, ref ushort startUID, int seed)
		{
			if (this.ammoType == AmmoType.MELEE)
			{
				this.SwingWeapon(dir, seed);
				return;
			}
			BulletCreator.BulletsFired(this.stats, this.level, ref pos, ref dir, chargeTime, parent, this, mods, ref startUID, seed);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0003897B File Offset: 0x00036B7B
		public void LevelUpGun()
		{
			if (this.level < 3)
			{
				this.level++;
			}
			this.parent.SomethingChanged = true;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x000389A0 File Offset: 0x00036BA0
		public void LevelGunToNum(int lev)
		{
			if (lev == this.level)
			{
				return;
			}
			this.level = lev;
			this.parent.SomethingChanged = true;
			this.OriginTex = this.stats.GetOrigin(this.level);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x000389D6 File Offset: 0x00036BD6
		public int GetLevel()
		{
			return this.level;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x000389E0 File Offset: 0x00036BE0
		public void DeactivateGun()
		{
			this.parent.Stats.DumpClip(this.ammoType, this.bulletsInClip);
			this.prevBulletsInClip = this.bulletsInClip;
			this.bulletsInClip = 0;
			if (this.reloadIndicator != null)
			{
				this.reloadIndicator.DestroyObject();
			}
			if (this.reloadTime != null)
			{
				this.reloadTime.Reset();
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00038A44 File Offset: 0x00036C44
		public void ActivateGun()
		{
			this.parent.SomethingChanged = true;
			this.parent.InvisGun = false;
			this.bulletsInClip = this.parent.Stats.TakeAmmo(this.ammoType, this.prevBulletsInClip);
			if (this.bulletsInClip <= 0 && this.parent.Stats.HasABullet(this.ammoType) && this.ammoType != AmmoType.MELEE)
			{
				this.reloadTime.Start();
				if (this.reloadIndicator != null)
				{
					this.reloadIndicator.DestroyObject();
				}
				this.reloadIndicator = new FloatingIndicator(this.parent, "Reload");
				if (Global.MasterCache != null)
				{
					Global.MasterCache.CreateObject(this.reloadIndicator);
				}
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00038B04 File Offset: 0x00036D04
		public float DeltaClip()
		{
			if ((float)this.stats.GunProperties[this.level].BulletsInClip == 0f)
			{
				return 0f;
			}
			return (float)this.bulletsInClip / (float)this.stats.GunProperties[this.level].BulletsInClip;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00038B60 File Offset: 0x00036D60
		private void FireParticles()
		{
			string particleOnFire = this.stats.ParticleOnFire;
			if (particleOnFire == " " || particleOnFire == string.Empty || particleOnFire == "None")
			{
				return;
			}
			this.PartAimDir.X = this.parent.mAimDir.X;
			this.PartAimDir.Z = this.parent.mAimDir.Y;
			if (particleOnFire == "Sparks")
			{
				for (int i = 0; i < 5; i++)
				{
					Global.MasterCache.CreateParticle(ParticleType.Spark, this.parent.mGunObject.Position, this.PartAimDir);
				}
				return;
			}
			if (particleOnFire == "Smoke")
			{
				for (int j = 0; j < 5; j++)
				{
					Global.MasterCache.CreateParticle(ParticleType.Smoke, this.parent.mGunObject.Position, this.PartAimDir);
				}
				return;
			}
			if (particleOnFire == "Magic")
			{
				for (int k = 0; k < 5; k++)
				{
					Global.MasterCache.CreateParticle(ParticleType.Magic, this.parent.mGunObject.Position, Vector3.Zero);
				}
				return;
			}
			if (particleOnFire == "MiniGunBullets")
			{
				this.PartAimDir.X = this.PartAimDir.X * -2f;
				this.PartAimDir.Z = this.PartAimDir.Z * -2f;
				if (this.parent != null && this.parent.mGunObject != null)
				{
					Global.MasterCache.CreateParticle(ParticleType.MiniGunBullets, this.parent.mGunObject.Position, this.PartAimDir);
					return;
				}
				Terminal.WriteMessage("ERROR: Parent was null when trying to create minigunbullet particle!", MessageType.ERROR);
				return;
			}
			else
			{
				if (!(particleOnFire == "SpentShells"))
				{
					return;
				}
				this.PartAimDir.X = this.PartAimDir.X * -2f;
				this.PartAimDir.Z = this.PartAimDir.Z * -2f;
				if (this.parent != null && this.parent.mGunObject != null)
				{
					Global.MasterCache.CreateParticle(ParticleType.SpentShells, this.parent.mGunObject.Position, this.PartAimDir);
					return;
				}
				Terminal.WriteMessage("ERROR: Parent was null when trying to create SpentShells particle!", MessageType.ERROR);
				return;
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00038D9C File Offset: 0x00036F9C
		private void SwingWeapon(Vector3 dir, int seed)
		{
			if (this.shootTime.Running())
			{
				return;
			}
			this.SWING_DIR = dir;
			this.mExplodedWhileSwinging = false;
			if (this.SwingObject == null)
			{
				this.SwingObject = new GameObject();
				this.SwingObject.ActivateObject(this.parent.Position, new Point(63, 63));
				this.SwingObject.TexScale = 2f;
				this.SwingObject.scale = 1f;
				this.SwingObject.AffectedByGravity = false;
				Global.MasterCache.CreateObject(this.SwingObject);
				this.SwingEffect = new GameObject();
				this.SwingEffect.ActivateObject(this.parent.Position, new Point(63, 63));
				this.SwingEffect.TexScale = 2f;
				this.SwingEffect.scale = 1.4f;
				this.SwingEffect.AffectedByGravity = false;
				Global.MasterCache.CreateObject(this.SwingEffect);
			}
			Random random = new Random(seed);
			this.GetModsForAttack(random);
			this.MeleeSwing.mods = this.Mods;
			float num = Global.RandomFloat(random, (float)this.stats.GunProperties[this.level].MinDamage, (float)this.stats.GunProperties[this.level].MaxDamage);
			num *= 1f + this.parent.SpecialProperties.MeleeDamageMod / 100f;
			this.MeleeSwing.dmg = num;
			if (this.Mods.Contains(BulletModifier.Crit))
			{
				this.MeleeSwing.dmg = this.MeleeSwing.dmg * 2f;
			}
			this.InvisTimer = -1f;
			this.SwingDir = dir;
			this.parent.InvisGun = true;
			this.Swinging = true;
			float num2 = MathHelper.WrapAngle(-VerchickMath.AngleFromVector2(new Vector2(dir.X, dir.Z)));
			this.SwingAngle = num2;
			this.ToAngle = num2 + 1.5707964f;
			this.FromAngle = num2 - 1.5707964f;
			this.SwingOffset = new Vector2(dir.X, dir.Z);
			this.SwingOffset *= 0.25f;
			this.UpdateSwingCollisions();
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00038FE0 File Offset: 0x000371E0
		private void UpdateSwinging(float elapsed)
		{
			if (this.SwingObject == null || this.SwingEffect == null)
			{
				return;
			}
			if (this.Swinging)
			{
				this.SwingTime -= elapsed;
				this.UpdateAnglesAndStuff();
				if (this.SwingTime <= 0f)
				{
					this.Swinging = false;
					this.SwingObject.TextureCoord = new Point(126, 62);
					this.SwingEffect.TextureCoord = new Point(126, 62);
					this.SwingTime = this.TotalSwingTime;
					this.InvisTimer = this.stats.GunProperties[this.level].ShotTime * (1f + this.parent.SpecialProperties.SwingTimeMod / 100f) + 0.1f;
					this.HitZombies.Clear();
				}
				if (this.SwingTime <= this.TotalSwingTime && !this.mExplodedWhileSwinging)
				{
					this.mExplodedWhileSwinging = true;
					if (!string.IsNullOrEmpty(this.stats.GunProperties[this.level].Explosion) && this.stats.GunProperties[this.level].Explosion != "None")
					{
						float num = 1f;
						Vector3 position = new Vector3(this.parent.Position.X + this.SWING_DIR.X * num, this.parent.Position.Y, this.parent.Position.Z + this.SWING_DIR.Z * num);
						Explosion.CreateExplosion(this.stats.GunProperties[this.level].ExplosionRadius, (float)this.stats.GunProperties[this.level].ExplosionDamage, (float)this.stats.GunProperties[this.level].ExplosionPushBack, this.stats.GunProperties[this.level].Explosion, position, this.parent, false);
					}
				}
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00039200 File Offset: 0x00037400
		private void UpdateAnglesAndStuff()
		{
			float yrotation = MathHelper.Lerp(this.FromAngle, this.ToAngle, this.SwingTime / this.TotalSwingTime);
			this.SwingObject.YRotation = yrotation;
			this.SwingObject.XRotation = 1.5707964f;
			this.SwingObject.Position.X = this.parent.Position.X + this.SwingOffset.X;
			this.SwingObject.Position.Y = this.parent.Position.Y - 0.1f;
			this.SwingObject.Position.Z = this.parent.Position.Z + this.SwingOffset.Y;
			this.SwingObject.TextureCoord.X = this.stats.GunProperties[this.level].MeleeTexX;
			this.SwingObject.TextureCoord.Y = this.stats.GunProperties[this.level].MeleeTexY;
			this.SwingEffect.Position.X = this.parent.Position.X + this.SwingOffset.X * 1.5f;
			this.SwingEffect.Position.Y = this.parent.Position.Y - 0.2f;
			this.SwingEffect.Position.Z = this.parent.Position.Z + this.SwingOffset.Y * 1.5f;
			this.SwingEffect.XRotation = 1.5707964f;
			this.SwingEffect.YRotation = this.FromAngle;
			this.SwingEffect.ZRotation = -1.5707964f;
			int y = 66 - 2 * (int)(this.SwingTime / this.TotalSwingTime * 10f) - 2;
			this.SwingEffect.TextureCoord = new Point(94, y);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00039408 File Offset: 0x00037608
		private void UpdateSwingCollisions()
		{
			if (!this.Swinging)
			{
				return;
			}
			for (int i = 0; i < Global.ZombieList.Count; i++)
			{
				Zombie zombie = Global.ZombieList[i];
				if (VerchickMath.WithinDistance(this.parent.twoDPosition, zombie.twoDPosition, 2.3f) && this.InFrontOf(zombie.twoDPosition) && (Global.Level.InLineOfSightMelee(this.parent.Position, zombie.Position) || this.parent.tile == zombie.tile))
				{
					Vector3 vector = Vector3.Subtract(zombie.Position, this.parent.Position);
					vector.Normalize();
					vector.Y = 0.5f;
					vector *= this.stats.GunProperties[this.level].PushBack;
					zombie.Velocity += vector;
					zombie.HACK_AboutToBeMeleed = true;
					zombie.Damage(this.parent, this.MeleeSwing.dmg, this.stats.GunProperties[this.level].DamageType, false, false, this.MeleeSwing.mods);
					zombie.HACK_AboutToBeMeleed = false;
					if (!string.IsNullOrEmpty(this.stats.GunProperties[this.level].Buff))
					{
						zombie.BuffManager.AddBuff(this.stats.GunProperties[this.level].Buff, this.parent, this.stats.GunProperties[this.level].BuffArgs);
					}
					this.HitZombies.Add(zombie);
				}
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000395CC File Offset: 0x000377CC
		private bool InFrontOf(Vector2 pos)
		{
			float num = Vector2.Dot(new Vector2(this.SwingDir.X, this.SwingDir.Z), VerchickMath.DirectionToVector2(this.parent.TwoDPosition(), pos));
			float num2 = (float)Math.Acos((double)num);
			return num > 0f && (double)num2 <= 3.141592653589793;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0003962C File Offset: 0x0003782C
		private Point GetOutOfAmmoTex(AmmoType type)
		{
			if (type == AmmoType.ASSAULT)
			{
				return new Point(12, 46);
			}
			if (type == AmmoType.HEAVY)
			{
				return new Point(12, 47);
			}
			if (type == AmmoType.SHELLS)
			{
				return new Point(12, 48);
			}
			if (type == AmmoType.EXPLOSIVE)
			{
				return new Point(12, 49);
			}
			return new Point(63, 63);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0003967C File Offset: 0x0003787C
		public void SendGunFireOverNet(int seed, ushort StartUID, bool crit, Vector2 aimDir)
		{
			Msg_GunFired msg_GunFired = new Msg_GunFired();
			msg_GunFired.FiredPositionX = this.parent.Position.X;
			msg_GunFired.FiredPositionZ = this.parent.Position.Z;
			msg_GunFired.AimDirX = aimDir.X;
			msg_GunFired.AimDirY = aimDir.Y;
			msg_GunFired.GunUID = GunStatsLoader.UID_GunsRev[this.stats.GunName];
			msg_GunFired.StartUID = StartUID;
			msg_GunFired.Seed = seed;
			msg_GunFired.Crit = crit;
			msg_GunFired.PlayerIndex = (byte)this.parent.Index;
			if (this.ammoType != AmmoType.MELEE)
			{
				msg_GunFired.BulletsRemainingInClip = (ushort)this.bulletsInClip;
			}
			NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.WeaponFired);
			netMessage.Payload = msg_GunFired;
			netMessage.UID = this.parent.UID;
			NetworkManager.SendMessage(netMessage, SendType.Reliable);
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x00039754 File Offset: 0x00037954
		public bool IsSuper
		{
			get
			{
				return this.Name == "Nuke Gun" || this.Name == "Mega Heal Totem" || this.Name == "Mega Bubble" || this.Name == "Orbital Strike Cannon" || this.Name == "Mega Robot" || this.Name == "Demon Hand";
			}
		}

		// Token: 0x04000734 RID: 1844
		public string Name;

		// Token: 0x04000735 RID: 1845
		public Point OriginTex;

		// Token: 0x04000736 RID: 1846
		private Point currentTexCoord = new Point(63, 63);

		// Token: 0x04000737 RID: 1847
		public GunStats stats;

		// Token: 0x04000738 RID: 1848
		private int level;

		// Token: 0x04000739 RID: 1849
		private Player parent;

		// Token: 0x0400073A RID: 1850
		public AmmoType ammoType;

		// Token: 0x0400073B RID: 1851
		private Timer flashTime;

		// Token: 0x0400073C RID: 1852
		private Timer reloadTime;

		// Token: 0x0400073D RID: 1853
		private Timer shootTime;

		// Token: 0x0400073E RID: 1854
		private Timer semiTime;

		// Token: 0x0400073F RID: 1855
		public int bulletsInClip = 20;

		// Token: 0x04000740 RID: 1856
		private float chargeTime;

		// Token: 0x04000741 RID: 1857
		private bool charging;

		// Token: 0x04000742 RID: 1858
		public FloatingIndicator reloadIndicator;

		// Token: 0x04000743 RID: 1859
		public FloatingIndicator chargeIndicator;

		// Token: 0x04000744 RID: 1860
		public int Kills;

		// Token: 0x04000745 RID: 1861
		public GameObject SwingObject;

		// Token: 0x04000746 RID: 1862
		public GameObject SwingEffect;

		// Token: 0x04000747 RID: 1863
		private float SwingTime = 0.22f;

		// Token: 0x04000748 RID: 1864
		private float TotalSwingTime = 0.22f;

		// Token: 0x04000749 RID: 1865
		public bool Swinging;

		// Token: 0x0400074A RID: 1866
		private float InvisTimer;

		// Token: 0x0400074B RID: 1867
		private int prevBulletsInClip = 20;

		// Token: 0x0400074C RID: 1868
		public bool FireEndOfBarrel = true;

		// Token: 0x0400074D RID: 1869
		private List<BulletModifier> Mods = new List<BulletModifier>();

		// Token: 0x0400074E RID: 1870
		private float mOutOfAmmo;

		// Token: 0x0400074F RID: 1871
		private static string clicky = "ze2_bzz";

		// Token: 0x04000750 RID: 1872
		private Vector3 shootdir = new Vector3(0f, 0f, 0f);

		// Token: 0x04000751 RID: 1873
		private Vector3 PartAimDir = new Vector3(0f, 0f, 0f);

		// Token: 0x04000752 RID: 1874
		private bool mExplodedWhileSwinging;

		// Token: 0x04000753 RID: 1875
		private Vector3 SWING_DIR = Vector3.Zero;

		// Token: 0x04000754 RID: 1876
		private List<Zombie> HitZombies = new List<Zombie>();

		// Token: 0x04000755 RID: 1877
		private Vector3 SwingDir;

		// Token: 0x04000756 RID: 1878
		private float SwingAngle;

		// Token: 0x04000757 RID: 1879
		private float FromAngle;

		// Token: 0x04000758 RID: 1880
		private float ToAngle;

		// Token: 0x04000759 RID: 1881
		private Vector2 SwingOffset;

		// Token: 0x0400075A RID: 1882
		private Gun.MeleeAttack MeleeSwing;

		// Token: 0x02000219 RID: 537
		private struct MeleeAttack
		{
			// Token: 0x04000E2F RID: 3631
			public List<BulletModifier> mods;

			// Token: 0x04000E30 RID: 3632
			public float dmg;
		}
	}
}
