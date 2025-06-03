using System;
using System.ComponentModel;
using ProtoBuf;

namespace ZombieEstate2
{
	// Token: 0x0200008A RID: 138
	[ProtoContract]
	public class SpecialProperties
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00019883 File Offset: 0x00017A83
		// (set) Token: 0x06000349 RID: 841 RVA: 0x0001988B File Offset: 0x00017A8B
		[Category("Movement")]
		[ProtoMember(1)]
		public float Speed { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00019894 File Offset: 0x00017A94
		// (set) Token: 0x0600034B RID: 843 RVA: 0x0001989C File Offset: 0x00017A9C
		[Category("Offense")]
		[ProtoMember(2)]
		public float BulletDamageMod { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600034C RID: 844 RVA: 0x000198A5 File Offset: 0x00017AA5
		// (set) Token: 0x0600034D RID: 845 RVA: 0x000198AD File Offset: 0x00017AAD
		[Category("Offense")]
		[ProtoMember(3)]
		public float MeleeDamageMod { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600034E RID: 846 RVA: 0x000198B6 File Offset: 0x00017AB6
		// (set) Token: 0x0600034F RID: 847 RVA: 0x000198BE File Offset: 0x00017ABE
		[Category("Offense")]
		[ProtoMember(4)]
		public float LifeStealPercent { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000350 RID: 848 RVA: 0x000198C7 File Offset: 0x00017AC7
		// (set) Token: 0x06000351 RID: 849 RVA: 0x000198CF File Offset: 0x00017ACF
		[Category("Offense")]
		[ProtoMember(5)]
		public float CritChance { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000352 RID: 850 RVA: 0x000198D8 File Offset: 0x00017AD8
		// (set) Token: 0x06000353 RID: 851 RVA: 0x000198E0 File Offset: 0x00017AE0
		[Category("Offense_Magic")]
		[ProtoMember(6)]
		public float FireDmg { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000354 RID: 852 RVA: 0x000198E9 File Offset: 0x00017AE9
		// (set) Token: 0x06000355 RID: 853 RVA: 0x000198F1 File Offset: 0x00017AF1
		[Category("Offense_Magic")]
		[ProtoMember(7)]
		public float WaterDmg { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000356 RID: 854 RVA: 0x000198FA File Offset: 0x00017AFA
		// (set) Token: 0x06000357 RID: 855 RVA: 0x00019902 File Offset: 0x00017B02
		[Category("Offense_Magic")]
		[ProtoMember(8)]
		public float EarthDmg { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0001990B File Offset: 0x00017B0B
		// (set) Token: 0x06000359 RID: 857 RVA: 0x00019913 File Offset: 0x00017B13
		[Category("Defense")]
		[ProtoMember(9)]
		public float MaxHealth { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0001991C File Offset: 0x00017B1C
		// (set) Token: 0x0600035B RID: 859 RVA: 0x00019924 File Offset: 0x00017B24
		[Category("Defense")]
		[ProtoMember(10)]
		public int Armor { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0001992D File Offset: 0x00017B2D
		// (set) Token: 0x0600035D RID: 861 RVA: 0x00019935 File Offset: 0x00017B35
		[Category("Defense")]
		[ProtoMember(11)]
		public int FireResist { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0001993E File Offset: 0x00017B3E
		// (set) Token: 0x0600035F RID: 863 RVA: 0x00019946 File Offset: 0x00017B46
		[Category("Defense")]
		[ProtoMember(12)]
		public int WaterResist { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0001994F File Offset: 0x00017B4F
		// (set) Token: 0x06000361 RID: 865 RVA: 0x00019957 File Offset: 0x00017B57
		[Category("Defense")]
		[ProtoMember(13)]
		public int EarthResist { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00019960 File Offset: 0x00017B60
		// (set) Token: 0x06000363 RID: 867 RVA: 0x00019968 File Offset: 0x00017B68
		[Category("Defense")]
		[ProtoMember(14)]
		public int AllResist { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00019971 File Offset: 0x00017B71
		// (set) Token: 0x06000365 RID: 869 RVA: 0x00019979 File Offset: 0x00017B79
		[Category("Heal")]
		[ProtoMember(15)]
		public float HealRecievedMod { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00019982 File Offset: 0x00017B82
		// (set) Token: 0x06000367 RID: 871 RVA: 0x0001998A File Offset: 0x00017B8A
		[Category("Heal")]
		[ProtoMember(16)]
		public float HealingDoneMod { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00019993 File Offset: 0x00017B93
		// (set) Token: 0x06000369 RID: 873 RVA: 0x0001999B File Offset: 0x00017B9B
		[Category("Heal")]
		[ProtoMember(17)]
		public float HealOverTime { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600036A RID: 874 RVA: 0x000199A4 File Offset: 0x00017BA4
		// (set) Token: 0x0600036B RID: 875 RVA: 0x000199AC File Offset: 0x00017BAC
		[Category("Heal")]
		[ProtoMember(18)]
		public float HealOnHitPercent { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000199B5 File Offset: 0x00017BB5
		// (set) Token: 0x0600036D RID: 877 RVA: 0x000199BD File Offset: 0x00017BBD
		[Category("Heal")]
		[ProtoMember(19)]
		public float HealOnHitAmount { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600036E RID: 878 RVA: 0x000199C6 File Offset: 0x00017BC6
		// (set) Token: 0x0600036F RID: 879 RVA: 0x000199CE File Offset: 0x00017BCE
		[Category("OnHit")]
		[ProtoMember(20)]
		public float ExplosionRadiusMod { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000370 RID: 880 RVA: 0x000199D7 File Offset: 0x00017BD7
		// (set) Token: 0x06000371 RID: 881 RVA: 0x000199DF File Offset: 0x00017BDF
		[Category("OnHit")]
		[ProtoMember(21)]
		public float ExplosionDamageMod { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000372 RID: 882 RVA: 0x000199E8 File Offset: 0x00017BE8
		// (set) Token: 0x06000373 RID: 883 RVA: 0x000199F0 File Offset: 0x00017BF0
		[Category("Guns")]
		[ProtoMember(22)]
		public float ReloadTimeMod { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000374 RID: 884 RVA: 0x000199F9 File Offset: 0x00017BF9
		// (set) Token: 0x06000375 RID: 885 RVA: 0x00019A01 File Offset: 0x00017C01
		[Category("Guns")]
		[ProtoMember(23)]
		public float ShotTimeMod { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00019A0A File Offset: 0x00017C0A
		// (set) Token: 0x06000377 RID: 887 RVA: 0x00019A12 File Offset: 0x00017C12
		[Category("Guns")]
		[ProtoMember(24)]
		public float SwingTimeMod { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00019A1B File Offset: 0x00017C1B
		// (set) Token: 0x06000379 RID: 889 RVA: 0x00019A23 File Offset: 0x00017C23
		[Category("Guns")]
		[ProtoMember(25)]
		public float NumBulletsMod { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00019A2C File Offset: 0x00017C2C
		// (set) Token: 0x0600037B RID: 891 RVA: 0x00019A34 File Offset: 0x00017C34
		[Category("Guns")]
		[ProtoMember(26)]
		public float GunCostMod { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00019A3D File Offset: 0x00017C3D
		// (set) Token: 0x0600037D RID: 893 RVA: 0x00019A45 File Offset: 0x00017C45
		[Category("Minion")]
		[ProtoMember(27)]
		public float MinionDmgMod { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00019A4E File Offset: 0x00017C4E
		// (set) Token: 0x0600037F RID: 895 RVA: 0x00019A56 File Offset: 0x00017C56
		[Category("Minion")]
		[ProtoMember(28)]
		public float MinionFireRateMod { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00019A5F File Offset: 0x00017C5F
		// (set) Token: 0x06000381 RID: 897 RVA: 0x00019A67 File Offset: 0x00017C67
		[Category("Minion")]
		[ProtoMember(29)]
		public float MinionAmmoMod { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00019A70 File Offset: 0x00017C70
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00019A78 File Offset: 0x00017C78
		[Category("Minion")]
		[ProtoMember(30)]
		public int MinionCount { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00019A81 File Offset: 0x00017C81
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00019A89 File Offset: 0x00017C89
		[Category("Ammo")]
		[ProtoMember(31)]
		public int Ammo_Shells { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00019A92 File Offset: 0x00017C92
		// (set) Token: 0x06000387 RID: 903 RVA: 0x00019A9A File Offset: 0x00017C9A
		[Category("Ammo")]
		[ProtoMember(32)]
		public int Ammo_Assault { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00019AA3 File Offset: 0x00017CA3
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00019AAB File Offset: 0x00017CAB
		[Category("Ammo")]
		[ProtoMember(33)]
		public int Ammo_Explosive { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00019AB4 File Offset: 0x00017CB4
		// (set) Token: 0x0600038B RID: 907 RVA: 0x00019ABC File Offset: 0x00017CBC
		[Category("Ammo")]
		[ProtoMember(34)]
		public int Ammo_Heavy { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00019AC5 File Offset: 0x00017CC5
		// (set) Token: 0x0600038D RID: 909 RVA: 0x00019ACD File Offset: 0x00017CCD
		[Category("Misc")]
		[ProtoMember(35)]
		public float ChanceOfArmorOnHeal { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00019AD6 File Offset: 0x00017CD6
		// (set) Token: 0x0600038F RID: 911 RVA: 0x00019ADE File Offset: 0x00017CDE
		[Category("Misc")]
		[ProtoMember(36)]
		public int ArmorGainedOnChanceOnHeal { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00019AE7 File Offset: 0x00017CE7
		// (set) Token: 0x06000391 RID: 913 RVA: 0x00019AEF File Offset: 0x00017CEF
		[Category("Misc")]
		[ProtoMember(37)]
		public int HealthPackStorageBonus { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000392 RID: 914 RVA: 0x00019AF8 File Offset: 0x00017CF8
		// (set) Token: 0x06000393 RID: 915 RVA: 0x00019B00 File Offset: 0x00017D00
		[Category("Misc")]
		[ProtoMember(38)]
		public float MoneyBonus { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00019B09 File Offset: 0x00017D09
		// (set) Token: 0x06000395 RID: 917 RVA: 0x00019B11 File Offset: 0x00017D11
		[Category("Misc")]
		[ProtoMember(39)]
		public float MagnetBonus { get; set; }

		// Token: 0x06000396 RID: 918 RVA: 0x00019B1C File Offset: 0x00017D1C
		public static void AddUpProps(ref SpecialProperties one, SpecialProperties two)
		{
			if (two == null)
			{
				return;
			}
			one.LifeStealPercent += two.LifeStealPercent;
			one.MaxHealth += two.MaxHealth;
			one.Speed += two.Speed;
			one.Armor += two.Armor;
			one.EarthResist += two.EarthResist;
			one.FireResist += two.FireResist;
			one.WaterResist += two.WaterResist;
			one.ExplosionDamageMod += two.ExplosionDamageMod;
			one.ExplosionRadiusMod += two.ExplosionRadiusMod;
			one.ReloadTimeMod += two.ReloadTimeMod;
			one.ShotTimeMod += two.ShotTimeMod;
			one.HealingDoneMod += two.HealingDoneMod;
			one.HealRecievedMod += two.HealRecievedMod;
			one.BulletDamageMod += two.BulletDamageMod;
			one.MeleeDamageMod += two.MeleeDamageMod;
			one.HealOverTime += two.HealOverTime;
			one.ChanceOfArmorOnHeal += two.ChanceOfArmorOnHeal;
			one.ArmorGainedOnChanceOnHeal += two.ArmorGainedOnChanceOnHeal;
			one.AllResist += two.AllResist;
			one.NumBulletsMod += two.NumBulletsMod;
			one.HealOnHitAmount += two.HealOnHitAmount;
			one.HealOnHitPercent += two.HealOnHitPercent;
			one.SwingTimeMod += two.SwingTimeMod;
			one.MinionAmmoMod += two.MinionAmmoMod;
			one.MinionDmgMod += two.MinionDmgMod;
			one.MinionFireRateMod += two.MinionFireRateMod;
			one.HealthPackStorageBonus += two.HealthPackStorageBonus;
			one.Ammo_Assault += two.Ammo_Assault;
			one.Ammo_Explosive += two.Ammo_Explosive;
			one.Ammo_Heavy += two.Ammo_Heavy;
			one.Ammo_Shells += two.Ammo_Shells;
			one.GunCostMod += two.GunCostMod;
			one.CritChance += two.CritChance;
			one.MoneyBonus += two.MoneyBonus;
			one.MagnetBonus += two.MagnetBonus;
			one.FireDmg += two.FireDmg;
			one.WaterDmg += two.WaterDmg;
			one.EarthDmg += two.EarthDmg;
			one.MinionCount += two.MinionCount;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00019E3C File Offset: 0x0001803C
		public static void ClearProps(ref SpecialProperties one)
		{
			one.LifeStealPercent = 0f;
			one.MaxHealth = 0f;
			one.Speed = 0f;
			one.Armor = 0;
			one.ExplosionDamageMod = 0f;
			one.ExplosionRadiusMod = 0f;
			one.ReloadTimeMod = 0f;
			one.ShotTimeMod = 0f;
			one.HealingDoneMod = 0f;
			one.HealRecievedMod = 0f;
			one.BulletDamageMod = 0f;
			one.MeleeDamageMod = 0f;
			one.FireResist = 0;
			one.WaterResist = 0;
			one.EarthResist = 0;
			one.HealOverTime = 0f;
			one.ChanceOfArmorOnHeal = 0f;
			one.ArmorGainedOnChanceOnHeal = 0;
			one.AllResist = 0;
			one.NumBulletsMod = 0f;
			one.HealOnHitPercent = 0f;
			one.HealOnHitAmount = 0f;
			one.SwingTimeMod = 0f;
			one.MinionAmmoMod = 0f;
			one.MinionDmgMod = 0f;
			one.MinionFireRateMod = 0f;
			one.HealthPackStorageBonus = 0;
			one.Ammo_Assault = 0;
			one.Ammo_Explosive = 0;
			one.Ammo_Heavy = 0;
			one.Ammo_Shells = 0;
			one.GunCostMod = 0f;
			one.CritChance = 0f;
			one.MoneyBonus = 0f;
			one.MagnetBonus = 0f;
			one.FireDmg = 0f;
			one.WaterDmg = 0f;
			one.EarthDmg = 0f;
			one.MinionCount = 0;
		}
	}
}
