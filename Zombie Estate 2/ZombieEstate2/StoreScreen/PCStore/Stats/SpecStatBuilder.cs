using System;

namespace ZombieEstate2.StoreScreen.PCStore.Stats
{
	// Token: 0x02000149 RID: 329
	public static class SpecStatBuilder
	{
		// Token: 0x060009FB RID: 2555 RVA: 0x00051CD4 File Offset: 0x0004FED4
		public static Cell AllResist(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.AllResist == p2.AllResist)
				{
					high = CellState.NA;
				}
				if (p1.AllResist < p2.AllResist)
				{
					high = CellState.Negative;
				}
				if (p1.AllResist > p2.AllResist)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(71, 50, high, string.Format("{0}", p1.AllResist), "Resistance to all elements.", false);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00051D3C File Offset: 0x0004FF3C
		public static Cell AssaultAmmo(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.Ammo_Assault == p2.Ammo_Assault)
				{
					high = CellState.NA;
				}
				if (p1.Ammo_Assault < p2.Ammo_Assault)
				{
					high = CellState.Negative;
				}
				if (p1.Ammo_Assault > p2.Ammo_Assault)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(62, 52, high, string.Format("{0}", p1.Ammo_Assault), "Bonus Assault Ammo storage.", false);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00051DA4 File Offset: 0x0004FFA4
		public static Cell ShellAmmo(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.Ammo_Shells == p2.Ammo_Shells)
				{
					high = CellState.NA;
				}
				if (p1.Ammo_Shells < p2.Ammo_Shells)
				{
					high = CellState.Negative;
				}
				if (p1.Ammo_Shells > p2.Ammo_Shells)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(62, 53, high, string.Format("{0}", p1.Ammo_Shells), "Bonus Shells storage.", false);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00051E0C File Offset: 0x0005000C
		public static Cell HeavyAmmo(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.Ammo_Heavy == p2.Ammo_Heavy)
				{
					high = CellState.NA;
				}
				if (p1.Ammo_Heavy < p2.Ammo_Heavy)
				{
					high = CellState.Negative;
				}
				if (p1.Ammo_Heavy > p2.Ammo_Heavy)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(63, 52, high, string.Format("{0}", p1.Ammo_Heavy), "Bonus Heavy Ammo storage.", false);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00051E74 File Offset: 0x00050074
		public static Cell ExplosiveAmmo(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.Ammo_Explosive == p2.Ammo_Explosive)
				{
					high = CellState.NA;
				}
				if (p1.Ammo_Explosive < p2.Ammo_Explosive)
				{
					high = CellState.Negative;
				}
				if (p1.Ammo_Explosive > p2.Ammo_Explosive)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(63, 53, high, string.Format("{0}", p1.Ammo_Explosive), "Bonus Explosive Ammo storage.", false);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00051EDC File Offset: 0x000500DC
		public static Cell MaxHealth(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.MaxHealth < p2.MaxHealth)
				{
					high = CellState.Negative;
				}
				if (p1.MaxHealth > p2.MaxHealth)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(62, 49, high, string.Format("{0}", p1.MaxHealth), "Maximum Health.", false);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00051F34 File Offset: 0x00050134
		public static Cell Speed(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			float num = 0f;
			if (p2 != null)
			{
				num = (p1.Speed / p2.Speed - 1f) * 100f;
				if (num < 0f)
				{
					high = CellState.Negative;
				}
				if (num > 0f)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(62, 48, high, string.Format("{0:0.#}%", num), "Bonus run speed.", false);
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00051F9C File Offset: 0x0005019C
		public static Cell MinionDmg(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.MinionDmgMod == p2.MinionDmgMod)
				{
					high = CellState.NA;
				}
				if (p1.MinionDmgMod < p2.MinionDmgMod)
				{
					high = CellState.Negative;
				}
				if (p1.MinionDmgMod > p2.MinionDmgMod)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(64, 53, high, string.Format("{0}%", p1.MinionDmgMod), "Bonus Minion damage.", false);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00052004 File Offset: 0x00050204
		public static Cell MinionRateOfFire(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.MinionFireRateMod == p2.MinionFireRateMod)
				{
					high = CellState.NA;
				}
				if (p1.MinionFireRateMod < p2.MinionFireRateMod)
				{
					high = CellState.Negative;
				}
				if (p1.MinionFireRateMod > p2.MinionFireRateMod)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(65, 53, high, string.Format("{0}%", p1.MinionFireRateMod), "Bonus Minion rate of fire.", false);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0005206C File Offset: 0x0005026C
		public static Cell MinionAmmo(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.MinionAmmoMod == p2.MinionAmmoMod)
				{
					high = CellState.NA;
				}
				if (p1.MinionAmmoMod < p2.MinionAmmoMod)
				{
					high = CellState.Negative;
				}
				if (p1.MinionAmmoMod > p2.MinionAmmoMod)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(65, 52, high, string.Format("{0}%", p1.MinionAmmoMod), "Bonus duration of Minions.", false);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x000520D4 File Offset: 0x000502D4
		public static Cell BulletDmg(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.BulletDamageMod == p2.BulletDamageMod)
				{
					high = CellState.NA;
				}
				if (p1.BulletDamageMod < p2.BulletDamageMod)
				{
					high = CellState.Negative;
				}
				if (p1.BulletDamageMod > p2.BulletDamageMod)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(67, 49, high, string.Format("{0}%", p1.BulletDamageMod), "Modifies damage done with Guns.", false);
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0005213C File Offset: 0x0005033C
		public static Cell MeleeDmg(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.MeleeDamageMod == p2.MeleeDamageMod)
				{
					high = CellState.NA;
				}
				if (p1.MeleeDamageMod < p2.MeleeDamageMod)
				{
					high = CellState.Negative;
				}
				if (p1.MeleeDamageMod > p2.MeleeDamageMod)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(66, 49, high, string.Format("{0}%", p1.MeleeDamageMod), "Modifies damage done with Melee Weapons.", false);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x000521A4 File Offset: 0x000503A4
		public static Cell Armor(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.Armor < p2.Armor)
				{
					high = CellState.Negative;
				}
				if (p1.Armor > p2.Armor)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(65, 50, high, string.Format("{0}", p1.Armor), "Armor reduces incoming physical damage.", false);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x000521FC File Offset: 0x000503FC
		public static Cell CritChance(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.CritChance == p2.CritChance)
				{
					high = CellState.NA;
				}
				if (p1.CritChance < p2.CritChance)
				{
					high = CellState.Negative;
				}
				if (p1.CritChance > p2.CritChance)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(72, 52, high, string.Format("{0}%", p1.CritChance), "Critical Strike chance. Crits cause double damage.", false);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00052264 File Offset: 0x00050464
		public static Cell ReloadSpeed(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.ReloadTimeMod == p2.ReloadTimeMod)
				{
					high = CellState.NA;
				}
				if (p1.ReloadTimeMod < p2.ReloadTimeMod)
				{
					high = CellState.Negative;
				}
				if (p1.ReloadTimeMod > p2.ReloadTimeMod)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(62, 50, high, string.Format("{0}%", p1.ReloadTimeMod), "Gun Reload speed bonus.", false);
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x000522CC File Offset: 0x000504CC
		public static Cell SwingSpeed(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.SwingTimeMod == p2.SwingTimeMod)
				{
					high = CellState.NA;
				}
				if (p1.SwingTimeMod < p2.SwingTimeMod)
				{
					high = CellState.Negative;
				}
				if (p1.SwingTimeMod > p2.SwingTimeMod)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(66, 51, high, string.Format("{0}%", p1.SwingTimeMod), "Bonus melee weapon swing speed.", false);
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00052334 File Offset: 0x00050534
		public static Cell ShotSpeed(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.ShotTimeMod == p2.ShotTimeMod)
				{
					high = CellState.NA;
				}
				if (p1.ShotTimeMod < p2.ShotTimeMod)
				{
					high = CellState.Negative;
				}
				if (p1.ShotTimeMod > p2.ShotTimeMod)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(69, 50, high, string.Format("{0}%", p1.ShotTimeMod), "Bonus gun fire rate.", false);
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0005239C File Offset: 0x0005059C
		public static Cell LifeSteal(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.LifeStealPercent == p2.LifeStealPercent)
				{
					high = CellState.NA;
				}
				if (p1.LifeStealPercent < p2.LifeStealPercent)
				{
					high = CellState.Negative;
				}
				if (p1.LifeStealPercent > p2.LifeStealPercent)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(67, 48, high, string.Format("{0}%", p1.LifeStealPercent), "Percentage of damage dealt returned as health.", false);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00052404 File Offset: 0x00050604
		public static Cell HealingDone(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.HealingDoneMod == p2.HealingDoneMod)
				{
					high = CellState.NA;
				}
				if (p1.HealingDoneMod < p2.HealingDoneMod)
				{
					high = CellState.Negative;
				}
				if (p1.HealingDoneMod > p2.HealingDoneMod)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(67, 50, high, string.Format("{0}%", p1.HealingDoneMod), "Healing done percent bonus.", false);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0005246C File Offset: 0x0005066C
		public static Cell HealingRecieved(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.HealRecievedMod == p2.HealRecievedMod)
				{
					high = CellState.NA;
				}
				if (p1.HealRecievedMod < p2.HealRecievedMod)
				{
					high = CellState.Negative;
				}
				if (p1.HealRecievedMod > p2.HealRecievedMod)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(69, 49, high, string.Format("{0}%", p1.HealRecievedMod), "Bonus amount healing does to you.", false);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x000524D4 File Offset: 0x000506D4
		public static Cell HealOverTime(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.HealOverTime == p2.HealOverTime)
				{
					high = CellState.NA;
				}
				if (p1.HealOverTime < p2.HealOverTime)
				{
					high = CellState.Negative;
				}
				if (p1.HealOverTime > p2.HealOverTime)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(68, 49, high, string.Format("{0}", p1.HealOverTime), "Heal over time. (Amount healed per second.)", false);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0005253C File Offset: 0x0005073C
		public static Cell FireBonus(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.FireDmg == p2.FireDmg)
				{
					high = CellState.NA;
				}
				if (p1.FireDmg < p2.FireDmg)
				{
					high = CellState.Negative;
				}
				if (p1.FireDmg > p2.FireDmg)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(72, 50, high, string.Format("{0}%", p1.FireDmg), "Bonus damage dealt with Fire.", false);
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x000525A4 File Offset: 0x000507A4
		public static Cell WaterBonus(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.WaterDmg == p2.WaterDmg)
				{
					high = CellState.NA;
				}
				if (p1.WaterDmg < p2.WaterDmg)
				{
					high = CellState.Negative;
				}
				if (p1.WaterDmg > p2.WaterDmg)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(73, 51, high, string.Format("{0}%", p1.WaterDmg), "Bonus damage dealt with Water and Ice.", false);
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0005260C File Offset: 0x0005080C
		public static Cell EarthBonus(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.EarthDmg == p2.EarthDmg)
				{
					high = CellState.NA;
				}
				if (p1.EarthDmg < p2.EarthDmg)
				{
					high = CellState.Negative;
				}
				if (p1.EarthDmg > p2.EarthDmg)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(73, 50, high, string.Format("{0}%", p1.EarthDmg), "Bonus damage dealt with Earth and Bio.", false);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00052674 File Offset: 0x00050874
		public static Cell FireResist(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.FireResist == p2.FireResist)
				{
					high = CellState.NA;
				}
				if (p1.FireResist < p2.FireResist)
				{
					high = CellState.Negative;
				}
				if (p1.FireResist > p2.FireResist)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(74, 50, high, string.Format("{0}", p1.FireResist), "Reduces Fire damage dealt to you.", false);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x000526DC File Offset: 0x000508DC
		public static Cell WaterResist(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.WaterResist == p2.WaterResist)
				{
					high = CellState.NA;
				}
				if (p1.WaterResist < p2.WaterResist)
				{
					high = CellState.Negative;
				}
				if (p1.WaterResist > p2.WaterResist)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(75, 50, high, string.Format("{0}", p1.WaterResist), "Reduces Water and Ice damage dealt to you.", false);
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x00052744 File Offset: 0x00050944
		public static Cell EarthResist(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.EarthResist == p2.EarthResist)
				{
					high = CellState.NA;
				}
				if (p1.EarthResist < p2.EarthResist)
				{
					high = CellState.Negative;
				}
				if (p1.EarthResist > p2.EarthResist)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(74, 51, high, string.Format("{0}", p1.EarthResist), "Reduces Earth and Bio damage dealt to you.", false);
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x000527AC File Offset: 0x000509AC
		public static Cell MoneyEarned(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p2 != null)
			{
				if (p1.MoneyBonus == p2.MoneyBonus)
				{
					high = CellState.NA;
				}
				if (p1.MoneyBonus < p2.MoneyBonus)
				{
					high = CellState.Negative;
				}
				if (p1.MoneyBonus > p2.MoneyBonus)
				{
					high = CellState.Positive;
				}
			}
			return new Cell(72, 51, high, string.Format("{0}%", p1.MoneyBonus), "Bonus money earned.", false);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00052814 File Offset: 0x00050A14
		public static Cell GUNLifeSteal(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.LifeStealPercent == 0f)
			{
				high = CellState.NA;
			}
			if (p2 != null)
			{
				if (p1.LifeStealPercent > p2.LifeStealPercent)
				{
					high = CellState.Negative;
				}
				if (p1.LifeStealPercent < p2.LifeStealPercent)
				{
					high = CellState.Positive;
				}
				return new Cell(67, 48, high, string.Format("{0}%", p2.LifeStealPercent), "Percentage of damage dealt returned as health.", false);
			}
			return new Cell(67, 48, high, string.Format("{0}%", p1.LifeStealPercent), "Percentage of damage dealt returned as health.", false);
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x000528A0 File Offset: 0x00050AA0
		public static Cell GUNFireResist(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.FireResist == 0)
			{
				high = CellState.NA;
			}
			if (p2 != null)
			{
				if (p1.FireResist > p2.FireResist)
				{
					high = CellState.Negative;
				}
				if (p1.FireResist < p2.FireResist)
				{
					high = CellState.Positive;
				}
				return new Cell(74, 50, high, string.Format("{0}", p2.FireResist), "Reduces Fire damage dealt to you.", false);
			}
			return new Cell(74, 50, high, string.Format("{0}", p1.FireResist), "Reduces Fire damage dealt to you.", false);
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00052928 File Offset: 0x00050B28
		public static Cell GUNWaterResist(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.WaterResist == 0)
			{
				high = CellState.NA;
			}
			if (p2 != null)
			{
				if (p1.WaterResist > p2.WaterResist)
				{
					high = CellState.Negative;
				}
				if (p1.WaterResist < p2.WaterResist)
				{
					high = CellState.Positive;
				}
				return new Cell(75, 50, high, string.Format("{0}", p2.WaterResist), "Reduces Water and Ice damage dealt to you.", false);
			}
			return new Cell(75, 50, high, string.Format("{0}", p1.WaterResist), "Reduces Water and Ice damage dealt to you.", false);
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x000529B0 File Offset: 0x00050BB0
		public static Cell GUNEarthResist(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.EarthResist == 0)
			{
				high = CellState.NA;
			}
			if (p2 != null)
			{
				if (p1.EarthResist > p2.EarthResist)
				{
					high = CellState.Negative;
				}
				if (p1.EarthResist < p2.EarthResist)
				{
					high = CellState.Positive;
				}
				return new Cell(74, 51, high, string.Format("{0}", p2.EarthResist), "Reduces Earth and Bio damage dealt to you.", false);
			}
			return new Cell(74, 51, high, string.Format("{0}", p1.EarthResist), "Reduces Earth and Bio damage dealt to you.", false);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00052A38 File Offset: 0x00050C38
		public static Cell GUNArmor(SpecialProperties p1, SpecialProperties p2 = null)
		{
			CellState high = CellState.Neutral;
			if (p1.Armor == 0)
			{
				high = CellState.NA;
			}
			if (p2 != null)
			{
				if (p1.Armor > p2.Armor)
				{
					high = CellState.Negative;
				}
				if (p1.Armor < p2.Armor)
				{
					high = CellState.Positive;
				}
				return new Cell(65, 50, high, string.Format("{0}", p2.Armor), "Armor reduces incoming physical damage.", false);
			}
			return new Cell(65, 50, high, string.Format("{0}", p1.Armor), "Armor reduces incoming physical damage.", false);
		}
	}
}
