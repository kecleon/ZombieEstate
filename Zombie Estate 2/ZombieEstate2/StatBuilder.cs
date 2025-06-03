using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.StoreScreen.PCStore.Stats;

namespace ZombieEstate2
{
	// Token: 0x020000CE RID: 206
	public static class StatBuilder
	{
		// Token: 0x0600054B RID: 1355 RVA: 0x00028708 File Offset: 0x00026908
		public static Cell Damage(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.MaxDamage < p2.MaxDamage || p1.MinDamage < p2.MinDamage)
				{
					high = CellState.Positive;
				}
				if (p1.MaxDamage > p2.MaxDamage || p1.MinDamage > p2.MinDamage)
				{
					high = CellState.Negative;
				}
				return new Cell(61, 41, high, string.Format("{0} - {1}", p2.MinDamage, p2.MaxDamage), "Damage range of this weapon.", false);
			}
			return new Cell(61, 41, high, string.Format("{0} - {1}", p1.MinDamage, p1.MaxDamage), "Damage range of this weapon.", false);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x000287B8 File Offset: 0x000269B8
		public static Cell LowDamage(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.MinDamage < 0)
			{
				if (p2 != null)
				{
					if (p1.MinDamage > p2.MinDamage)
					{
						high = CellState.Positive;
					}
					if (p1.MinDamage < p2.MinDamage)
					{
						high = CellState.Negative;
					}
					float num = (float)(-(float)p2.MinDamage);
					return new Cell(65, 40, high, string.Format("{0}", num), "Minimum amount this weapon can heal.", false);
				}
				float num2 = (float)(-(float)p1.MinDamage);
				return new Cell(65, 40, high, string.Format("{0}", num2), "Minimum amount this weapon can heal.", false);
			}
			else
			{
				if (p2 != null)
				{
					if (p1.MinDamage < p2.MinDamage)
					{
						high = CellState.Positive;
					}
					if (p1.MinDamage > p2.MinDamage)
					{
						high = CellState.Negative;
					}
					return new Cell(61, 40, high, string.Format("{0}", p2.MinDamage), "Minimum damage of this weapon.", false);
				}
				return new Cell(61, 40, high, string.Format("{0}", p1.MinDamage), "Minimum damage of this weapon.", false);
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000288B8 File Offset: 0x00026AB8
		public static Cell HighDamage(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.MaxDamage < 0)
			{
				if (p2 != null)
				{
					if (p1.MaxDamage > p2.MaxDamage)
					{
						high = CellState.Positive;
					}
					if (p1.MaxDamage < p2.MaxDamage)
					{
						high = CellState.Negative;
					}
					float num = (float)(-(float)p2.MaxDamage);
					return new Cell(65, 41, high, string.Format("{0}", num), "Maximum amount this weapon can heal.", false);
				}
				float num2 = (float)(-(float)p1.MaxDamage);
				return new Cell(65, 41, high, string.Format("{0}", num2), "Maximum amount this weapon can heal.", false);
			}
			else
			{
				if (p2 != null)
				{
					if (p1.MaxDamage < p2.MaxDamage)
					{
						high = CellState.Positive;
					}
					if (p1.MaxDamage > p2.MaxDamage)
					{
						high = CellState.Negative;
					}
					return new Cell(61, 41, high, string.Format("{0}", p2.MaxDamage), "Maximum damage of this weapon.", false);
				}
				return new Cell(61, 41, high, string.Format("{0}", p1.MaxDamage), "Maximum damage of this weapon.", false);
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x000289B8 File Offset: 0x00026BB8
		public static Cell ShotTime(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.ShotTime > p2.ShotTime)
				{
					high = CellState.Positive;
				}
				if (p1.ShotTime < p2.ShotTime)
				{
					high = CellState.Negative;
				}
				float num = 1f / p2.ShotTime;
				return new Cell(62, 41, high, string.Format("{0:0.0}", num), "Number of shots per second.", false);
			}
			float num2 = 1f / p1.ShotTime;
			return new Cell(62, 41, high, string.Format("{0:0.0}", num2), "Number of shots per second.", false);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00028A48 File Offset: 0x00026C48
		public static Cell KnockBack(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.PushBack < p2.PushBack)
				{
					high = CellState.Positive;
				}
				if (p1.PushBack > p2.PushBack)
				{
					high = CellState.Negative;
				}
				return new Cell(63, 41, high, string.Format("{0}", p2.PushBack), "Stopping power.", false);
			}
			return new Cell(63, 41, high, string.Format("{0}", p1.PushBack), "Stopping power.", false);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00028AC8 File Offset: 0x00026CC8
		public static Cell Penetration(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.PenetrationChance == StatBuilder.DEFAULTS.PenetrationChance)
			{
				high = CellState.NA;
			}
			if (p2 != null)
			{
				if (p1.PenetrationChance < p2.PenetrationChance)
				{
					high = CellState.Positive;
				}
				if (p1.PenetrationChance > p2.PenetrationChance)
				{
					high = CellState.Negative;
				}
				if (p2.PenetrationChance == StatBuilder.DEFAULTS.PenetrationChance)
				{
					high = CellState.NA;
				}
				return new Cell(63, 43, high, string.Format("{0}%", p2.PenetrationChance), "Bullet penetration chance.", false);
			}
			return new Cell(63, 43, high, string.Format("{0}%", p1.PenetrationChance), "Bullet penetration chance.", false);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00028B70 File Offset: 0x00026D70
		public static Cell Reload(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.ReloadTime == 0f)
			{
				high = CellState.NA;
			}
			if (p2 != null)
			{
				if (p1.ReloadTime > p2.ReloadTime)
				{
					high = CellState.Positive;
				}
				if (p1.ReloadTime < p2.ReloadTime)
				{
					high = CellState.Negative;
				}
				return new Cell(61, 42, high, string.Format("{0}", p2.ReloadTime), "Amount of time it takes to reload. (Seconds)", false);
			}
			return new Cell(61, 42, high, string.Format("{0}", p1.ReloadTime), "Amount of time it takes to reload. (Seconds)", false);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00028BFC File Offset: 0x00026DFC
		public static Cell ClipSize(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.BulletsInClip == 0)
			{
				high = CellState.NA;
			}
			if (p2 != null)
			{
				if (p1.BulletsInClip < p2.BulletsInClip)
				{
					high = CellState.Positive;
				}
				if (p1.BulletsInClip > p2.BulletsInClip)
				{
					high = CellState.Negative;
				}
				return new Cell(62, 42, high, string.Format("{0}", p2.BulletsInClip), "How many bullets before needing to reload.", false);
			}
			return new Cell(62, 42, high, string.Format("{0}", p1.BulletsInClip), "How many bullets before needing to reload.", false);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00028C84 File Offset: 0x00026E84
		public static Cell BulletsCostToFire(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.BulletsCostToFire > p2.BulletsCostToFire)
				{
					high = CellState.Positive;
				}
				if (p1.BulletsCostToFire < p2.BulletsCostToFire)
				{
					high = CellState.Negative;
				}
				if (p2.BulletsCostToFire == 0)
				{
					high = CellState.NA;
				}
				return new Cell(63, 42, high, string.Format("{0}", p2.BulletsCostToFire), "How many bullets it costs per shot.", false);
			}
			if (p1.BulletsCostToFire == 0)
			{
				high = CellState.NA;
			}
			return new Cell(63, 42, high, string.Format("{0}", p1.BulletsCostToFire), "How many bullets it costs per shot.", false);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00028D18 File Offset: 0x00026F18
		public static Cell BouncePercent(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.BounceChance < p2.BounceChance)
				{
					high = CellState.Positive;
				}
				if (p1.BounceChance > p2.BounceChance)
				{
					high = CellState.Negative;
				}
				if (p2.BounceChance == StatBuilder.DEFAULTS.BounceChance)
				{
					high = CellState.NA;
				}
				return new Cell(61, 43, high, string.Format("{0}%", p2.BounceChance), "Chance of a bullet ricocheting off a wall.", false);
			}
			if (p1.BounceChance == StatBuilder.DEFAULTS.BounceChance)
			{
				high = CellState.NA;
			}
			return new Cell(61, 43, high, string.Format("{0}%", p1.BounceChance), "Chance of a bullet ricocheting off a wall.", false);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00028DC0 File Offset: 0x00026FC0
		public static Cell BounceCount(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.BounceCount < p2.BounceCount)
				{
					high = CellState.Positive;
				}
				if (p1.BounceCount > p2.BounceCount)
				{
					high = CellState.Negative;
				}
				if (p2.BounceCount == StatBuilder.DEFAULTS.BounceCount)
				{
					high = CellState.NA;
				}
				return new Cell(62, 43, high, string.Format("{0}", p2.BounceCount), "Number of times a bullet can ricochet off a wall.", false);
			}
			if (p1.BounceCount == StatBuilder.DEFAULTS.BounceCount)
			{
				high = CellState.NA;
			}
			return new Cell(62, 43, high, string.Format("{0}", p1.BounceCount), "Number of times a bullet can ricochet off a wall.", false);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00028E68 File Offset: 0x00027068
		public static Cell ExplosionDmg(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.ExplosionDamage < p2.ExplosionDamage)
				{
					high = CellState.Positive;
				}
				if (p1.ExplosionDamage > p2.ExplosionDamage)
				{
					high = CellState.Negative;
				}
				if (p2.ExplosionDamage == StatBuilder.DEFAULTS.ExplosionDamage)
				{
					high = CellState.NA;
				}
				return new Cell(62, 44, high, string.Format("{0}", p2.ExplosionDamage), "Explosion damage.", false);
			}
			if (p1.ExplosionDamage == StatBuilder.DEFAULTS.ExplosionDamage)
			{
				high = CellState.NA;
			}
			return new Cell(62, 44, high, string.Format("{0}", p1.ExplosionDamage), "Explosion damage.", false);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00028F10 File Offset: 0x00027110
		public static Cell ExplosionRange(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.ExplosionRadius < p2.ExplosionRadius)
				{
					high = CellState.Positive;
				}
				if (p1.ExplosionRadius > p2.ExplosionRadius)
				{
					high = CellState.Negative;
				}
				if (p2.ExplosionRadius == StatBuilder.DEFAULTS.ExplosionRadius)
				{
					high = CellState.NA;
				}
				return new Cell(61, 44, high, string.Format("{0}", p2.ExplosionRadius), "Explosion radius.", false);
			}
			if (p1.ExplosionRadius == StatBuilder.DEFAULTS.ExplosionRadius)
			{
				high = CellState.NA;
			}
			return new Cell(61, 44, high, string.Format("{0}", p1.ExplosionRadius), "Explosion radius.", false);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00028FB8 File Offset: 0x000271B8
		public static Cell NumBulletsFired(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.NumberOfBulletsFired == 0)
			{
				high = CellState.NA;
			}
			if (p2 != null)
			{
				if (p1.NumberOfBulletsFired < p2.NumberOfBulletsFired)
				{
					high = CellState.Positive;
				}
				if (p1.NumberOfBulletsFired > p2.NumberOfBulletsFired)
				{
					high = CellState.Negative;
				}
				if (p2.NumberOfBulletsFired == 1)
				{
					high = CellState.NA;
				}
				return new Cell(63, 44, high, string.Format("{0}", p2.NumberOfBulletsFired), "Number of bullets fired per shot.", false);
			}
			if (p1.NumberOfBulletsFired == 1)
			{
				high = CellState.NA;
			}
			return new Cell(63, 44, high, string.Format("{0}", p1.NumberOfBulletsFired), "Number of bullets fired per shot.", false);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00029058 File Offset: 0x00027258
		public static Cell DmgType(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Override;
			Color overrideColor = Color.White;
			switch (p1.DamageType)
			{
			case DamageType.Physical:
				overrideColor = Color.LightGray;
				break;
			case DamageType.Fire:
				overrideColor = Color.OrangeRed;
				break;
			case DamageType.Water:
				overrideColor = Color.Blue;
				break;
			case DamageType.Earth:
				overrideColor = Color.Green;
				break;
			case DamageType.Dark:
				overrideColor = Color.Purple;
				break;
			}
			return new Cell(64, 42, high, string.Format("{0}", p1.DamageType.ToString()), "Type of damage this weapon deals.", true)
			{
				OverrideColor = overrideColor
			};
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x000290EC File Offset: 0x000272EC
		public static Cell Accuracy(GunProperties p1, GunProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.Accuracy == 0f)
			{
				high = CellState.NA;
			}
			if (p2 != null)
			{
				if (p1.Accuracy > p2.Accuracy)
				{
					high = CellState.Positive;
				}
				if (p1.Accuracy < p2.Accuracy)
				{
					high = CellState.Positive;
				}
				return new Cell(64, 41, high, string.Format("{0}", p2.Accuracy), "Accuracy range in degrees.", false);
			}
			return new Cell(64, 41, high, string.Format("{0}", p1.Accuracy), "Accuracy range in degrees.", false);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00029178 File Offset: 0x00027378
		public static Cell CritChance(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.CritChance == 0f)
			{
				high = CellState.NA;
			}
			if (p2 != null)
			{
				if (p1.CritChance < p2.CritChance)
				{
					high = CellState.Positive;
				}
				if (p1.CritChance > p2.CritChance)
				{
					high = CellState.Positive;
				}
				return new Cell(64, 43, high, string.Format("{0}%", p2.CritChance), "Critical Strike chance. Crits cause double damage.", false);
			}
			return new Cell(64, 43, high, string.Format("{0}%", p1.CritChance), "Critical Strike chance. Crits cause double damage.", false);
		}

		// Token: 0x04000555 RID: 1365
		public static GunProperties DEFAULTS = new GunProperties();

		// Token: 0x04000556 RID: 1366
		public static SpecialProperties DEFAULTS_SPEC = new SpecialProperties();
	}
}
