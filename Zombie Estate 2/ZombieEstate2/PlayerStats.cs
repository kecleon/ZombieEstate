using System;
using System.Collections.Generic;

namespace ZombieEstate2
{
	// Token: 0x02000095 RID: 149
	public class PlayerStats
	{
		// Token: 0x060003C5 RID: 965 RVA: 0x000026B9 File Offset: 0x000008B9
		public PlayerStats()
		{
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001B7B4 File Offset: 0x000199B4
		public PlayerStats(CharacterStats charStats, CharacterSettings settings, Player parent)
		{
			this.parent = parent;
			this.CharSettings = settings;
			this.stats = default(CharacterStats);
			this.stats.CharacterName = settings.name;
			this.stats.GamerName = "TestPC";
			this.stats.Ammo = new int[6];
			this.stats.MaxAmmo = new int[6];
			this.stats.Ammo[0] = 0;
			this.stats.MaxAmmo[0] = 350;
			this.stats.Ammo[1] = 0;
			this.stats.MaxAmmo[1] = 600;
			this.stats.Ammo[2] = 0;
			this.stats.MaxAmmo[2] = 120;
			this.stats.Ammo[3] = 0;
			this.stats.MaxAmmo[3] = 20;
			this.stats.Ammo[4] = 0;
			this.stats.MaxAmmo[4] = 4;
			this.stats.TalentPoints = 0;
			this.stats.Guns = new List<GunSaveStats>();
			TalentManager.GetTalents(settings);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001B8DD File Offset: 0x00019ADD
		public int GetMoney()
		{
			return (int)this.stats.Money;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001B8EB File Offset: 0x00019AEB
		public void AddMoney(float amount)
		{
			this.stats.Money = this.stats.Money + amount;
			if (this.stats.Money > 99999f)
			{
				this.stats.Money = 99999f;
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001B920 File Offset: 0x00019B20
		public void SetMoney(float amount)
		{
			this.stats.Money = amount;
			if (this.stats.Money > 99999f)
			{
				this.stats.Money = 99999f;
			}
			if (this.stats.Money < 0f)
			{
				this.stats.Money = 0f;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001B97D File Offset: 0x00019B7D
		public void AddMoney(int amount)
		{
			this.AddMoney((float)amount);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001B988 File Offset: 0x00019B88
		public bool GiveAmmo(AmmoType type, int amount)
		{
			if (this.parent is Minion || this.parent == null)
			{
				return false;
			}
			int num = this.stats.Ammo[(int)type];
			this.stats.Ammo[(int)type] = Math.Min(this.stats.Ammo[(int)type] + amount, this.stats.MaxAmmo[(int)type]);
			return this.stats.Ammo[(int)type] != num;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0001B9FB File Offset: 0x00019BFB
		public void SetAmmo(AmmoType type, int amount)
		{
			if (this.parent is Minion)
			{
				return;
			}
			this.stats.Ammo[(int)type] = Math.Min(amount, this.stats.MaxAmmo[(int)type]);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001BA2C File Offset: 0x00019C2C
		public int TakeAmmo(AmmoType type, int amount)
		{
			if (type == AmmoType.INFINITE || type == AmmoType.MELEE || this.parent is Minion || this.parent == null)
			{
				return amount;
			}
			int num = this.stats.Ammo[(int)type];
			this.stats.Ammo[(int)type] = Math.Max(this.stats.Ammo[(int)type] - amount, 0);
			return num - this.stats.Ammo[(int)type];
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001BA95 File Offset: 0x00019C95
		public bool HasABullet(AmmoType type)
		{
			return type == AmmoType.INFINITE || type == AmmoType.MELEE || this.parent is Minion || this.parent == null || this.stats.Ammo[(int)type] > 0;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001BAC6 File Offset: 0x00019CC6
		public bool HasBullets(AmmoType type, int amount)
		{
			return type == AmmoType.INFINITE || type == AmmoType.MELEE || this.parent is Minion || this.parent == null || this.stats.Ammo[(int)type] >= amount;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001BAFC File Offset: 0x00019CFC
		public int GetMaxClip(AmmoType type, int maxClipSize)
		{
			if (type == AmmoType.INFINITE || type == AmmoType.MELEE || this.parent is Minion || this.parent == null)
			{
				return maxClipSize;
			}
			int num = Math.Min(this.stats.Ammo[(int)type], maxClipSize);
			this.TakeAmmo(type, num);
			return num;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001BB48 File Offset: 0x00019D48
		public void DumpClip(AmmoType type, int remainingClip)
		{
			if (type == AmmoType.INFINITE || type == AmmoType.MELEE || this.parent is Minion || this.parent == null)
			{
				return;
			}
			this.stats.Ammo[(int)type] = Math.Min(this.stats.Ammo[(int)type] + remainingClip, this.stats.MaxAmmo[(int)type]);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001BBA1 File Offset: 0x00019DA1
		public int GetAmmo(AmmoType type)
		{
			if (type == AmmoType.INFINITE || type == AmmoType.MELEE || this.parent is Minion || this.parent == null)
			{
				return -1;
			}
			return this.stats.Ammo[(int)type];
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001BBD0 File Offset: 0x00019DD0
		public int GetAmmoIncludingClip(AmmoType type)
		{
			if (type == AmmoType.INFINITE || type == AmmoType.MELEE || this.parent is Minion || this.parent == null)
			{
				return -1;
			}
			if (this.parent.mGun.ammoType == type)
			{
				return this.stats.Ammo[(int)type] + this.parent.mGun.bulletsInClip;
			}
			return this.stats.Ammo[(int)type];
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001BC3B File Offset: 0x00019E3B
		public int GetMaxAmmo(AmmoType type)
		{
			if (type == AmmoType.INFINITE || type == AmmoType.MELEE || this.parent is Minion || this.parent == null)
			{
				return -1;
			}
			return this.stats.MaxAmmo[(int)type];
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001BC69 File Offset: 0x00019E69
		public void AddMaxAmmo(AmmoType type, int amount)
		{
			if (type == AmmoType.INFINITE || type == AmmoType.MELEE || this.parent is Minion || this.parent == null)
			{
				return;
			}
			this.stats.MaxAmmo[(int)type] += amount;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001BC9F File Offset: 0x00019E9F
		public void KilledZombie(Zombie zombie)
		{
			if (this.stats.totalZombiesKilled < 2147483647)
			{
				this.stats.totalZombiesKilled = this.stats.totalZombiesKilled + 1;
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001BCC3 File Offset: 0x00019EC3
		public int TotalZombiesKilled()
		{
			return this.stats.totalZombiesKilled;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001BCD0 File Offset: 0x00019ED0
		public int GetTalentPoints()
		{
			return this.stats.TalentPoints;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001BCDD File Offset: 0x00019EDD
		public void AddTalentPoints(int amount)
		{
			this.stats.TalentPoints = this.stats.TalentPoints + amount;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0001BCEF File Offset: 0x00019EEF
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0001BCFC File Offset: 0x00019EFC
		public int UpgradeTokens
		{
			get
			{
				return this.stats.UpgradeTokens;
			}
			set
			{
				this.stats.UpgradeTokens = value;
				this.stats.UpgradeTokens = Math.Min(100, this.stats.UpgradeTokens);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0001BD27 File Offset: 0x00019F27
		// (set) Token: 0x060003DD RID: 989 RVA: 0x0001BD34 File Offset: 0x00019F34
		public int HealthPacks
		{
			get
			{
				return this.stats.HealthPacks;
			}
			set
			{
				this.stats.HealthPacks = value;
				this.stats.HealthPacks = Math.Min(5, this.stats.HealthPacks);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0001BD5E File Offset: 0x00019F5E
		// (set) Token: 0x060003DF RID: 991 RVA: 0x0001BD66 File Offset: 0x00019F66
		public CharacterStats MyStats
		{
			get
			{
				return this.stats;
			}
			set
			{
				this.stats = value;
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0001BD6F File Offset: 0x00019F6F
		public List<GunSaveStats> GetSavedGuns()
		{
			return this.stats.Guns;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001BD7C File Offset: 0x00019F7C
		public void PrepForSaving(List<Gun> guns)
		{
			this.stats.AmmoInt = new List<int>();
			this.stats.MaxAmmoInt = new List<int>();
			this.stats.Guns = new List<GunSaveStats>();
			foreach (Gun gun in guns)
			{
				if (gun != null)
				{
					GunSaveStats item = default(GunSaveStats);
					item.gunName = gun.Name;
					item.kills = gun.Kills;
					item.level = gun.GetLevel();
					this.stats.Guns.Add(item);
				}
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void PostLoad()
		{
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001BE38 File Offset: 0x0001A038
		private void DefaultStats()
		{
			this.stats.AmmoInt = new List<int>();
			this.stats.MaxAmmoInt = new List<int>();
			this.stats.AmmoInt.Add(0);
			this.stats.MaxAmmoInt.Add(350);
			this.stats.AmmoInt.Add(0);
			this.stats.MaxAmmoInt.Add(20);
			this.stats.AmmoInt.Add(0);
			this.stats.MaxAmmoInt.Add(600);
			this.stats.AmmoInt.Add(0);
			this.stats.MaxAmmoInt.Add(120);
			this.stats.AmmoInt.Add(0);
			this.stats.MaxAmmoInt.Add(4);
			this.stats.Money = 0f;
			this.stats.Talents = TalentManager.GetTalents(this.CharSettings);
			this.stats.TalentPoints = 0;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001BF4C File Offset: 0x0001A14C
		private int getGun(string name)
		{
			foreach (GunSaveStats gunSaveStats in this.stats.Guns)
			{
				if (name == gunSaveStats.gunName)
				{
					return this.stats.Guns.IndexOf(gunSaveStats);
				}
			}
			return -1;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001BFC4 File Offset: 0x0001A1C4
		public List<Talent> GetTalents()
		{
			return this.stats.Talents;
		}

		// Token: 0x040003A8 RID: 936
		private CharacterStats stats;

		// Token: 0x040003A9 RID: 937
		public CharacterSettings CharSettings;

		// Token: 0x040003AA RID: 938
		public Player parent;
	}
}
