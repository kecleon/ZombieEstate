using System;
using System.Collections.Generic;
using System.IO;

namespace ZombieEstate2
{
	// Token: 0x0200013B RID: 315
	public static class GunStatsLoader
	{
		// Token: 0x0600094F RID: 2383 RVA: 0x0004AD74 File Offset: 0x00048F74
		public static GunStats LoadGun(string GunName)
		{
			string fileName;
			if (GunName.Contains(".gun"))
			{
				fileName = ".\\Data\\Guns\\" + GunName;
			}
			else
			{
				fileName = ".\\Data\\Guns\\" + GunName + ".gun";
			}
			GunStats result;
			XMLSaverLoader.LoadObject<GunStats>(fileName, out result);
			return result;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0004ADB8 File Offset: 0x00048FB8
		public static void LoadAllGuns()
		{
			List<string> list = new List<string>();
			list.Add("Assault Rifle");
			list.Add("Bow and Arrow");
			list.Add("Shotgun");
			list.Add("Heavy Shotgun");
			list.Add("Tundra Falcon");
			list.Add("Minigun");
			list.Add("Snow Launcher");
			list.Add("Card Shuffler");
			list.Add("Vacuum");
			list.Add("Bubble Launcher");
			list.Add("Laser Pistol");
			list.Add("Toy Launcher");
			list.Add("Soap Gun");
			list.Add("Cereal Bowl");
			list.Add("Heavy Assault Rifle");
			list.Add("The Compiler");
			list.Add("Rocket Launcher");
			list.Add("Rocket Shotgun");
			list.Add("Nuke Gun");
			list.Add("Popcorn Rifle");
			list.Add("Uzi");
			list.Add("Flame Thrower");
			list.Add("Bacon Slicer");
			list.Add("Blade Slinger");
			list.Add("Pistol");
			list.Add("Laser Rifle");
			list.Add("Minigun Infinite");
			list.Add("Frost Bow");
			list.Add("Tri-Shot Machine Gun");
			list.Add("Potato Cannon");
			list.Add("Orbital Strike Cannon");
			list.Add("Wool Gun");
			list.Add("Minibomb");
			list.Add("Turret Launcher");
			list.Add("Rubber Shotgun");
			list.Add("Text Pistol");
			list.Add("RM80");
			list.Add("Magic Wand");
			list.Add("Acid Spitter");
			list.Add("Basic Bomb");
			Terminal.WriteMessage("Loading Guns", MessageType.SAVELOAD);
			Terminal.WriteMessage("---------------------------------------------", MessageType.SAVELOAD);
			foreach (string gunName in list)
			{
				GunStats item = GunStatsLoader.LoadGun(gunName);
				GunStatsLoader.GunStatsList.Add(item);
			}
			GunStatsLoader.GunStatsList.Sort();
			Terminal.WriteMessage("---------------------------------------------", MessageType.SAVELOAD);
			Terminal.WriteMessage("Loading Guns Completed", MessageType.SAVELOAD);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0004B004 File Offset: 0x00049204
		public static void LoadAllGunsInFolder()
		{
			XMLSaverLoader.LoadProto<List<GunStats>>("Data\\GunList.bin", out GunStatsLoader.GunStatsList);
			GunStatsLoader.GunStatsList.Sort();
			foreach (GunStats gunStats in GunStatsLoader.GunStatsList)
			{
				GunStatsLoader.UID_Guns.Add(GunStatsLoader.UID, gunStats.GunName);
				GunStatsLoader.UID_GunsRev.Add(gunStats.GunName, GunStatsLoader.UID);
				GunStatsLoader.UID += 1;
				if (string.IsNullOrEmpty(gunStats.GunProperties[0].SoundName))
				{
					Terminal.WriteMessage("MISSING SOUND: " + gunStats.GunName, MessageType.ERROR);
				}
			}
			Terminal.WriteMessage("MISSING SOUND: LEAF ARM", MessageType.ERROR);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0004B0D8 File Offset: 0x000492D8
		private static void LoadIndividualFiles()
		{
			List<string> list = new List<string>();
			foreach (FileInfo fileInfo in new DirectoryInfo(".\\Data\\Guns\\").GetFiles())
			{
				list.Add(fileInfo.Name);
			}
			Console.WriteLine("Loading guns...");
			foreach (string gunName in list)
			{
				GunStats gunStats = GunStatsLoader.LoadGun(gunName);
				GunStatsLoader.GunStatsList.Add(gunStats);
				GunStatsLoader.UID_Guns.Add(GunStatsLoader.UID, gunStats.GunName);
				GunStatsLoader.UID_GunsRev.Add(gunStats.GunName, GunStatsLoader.UID);
				GunStatsLoader.UID += 1;
			}
			GunStatsLoader.GunStatsList.Sort();
			Console.WriteLine("Done Loading guns.");
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0004B1C0 File Offset: 0x000493C0
		public static void TESTSAVEALL()
		{
			XMLSaverLoader.SaveProto<List<GunStats>>("TESTDELETE", GunStatsLoader.GunStatsList);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0004B1D4 File Offset: 0x000493D4
		public static GunStats GetStats(string name)
		{
			for (int i = 0; i < GunStatsLoader.GunStatsList.Count; i++)
			{
				if (name == GunStatsLoader.GunStatsList[i].GunName)
				{
					return GunStatsLoader.GunStatsList[i];
				}
			}
			Terminal.WriteMessage("ERROR: Gun |" + name + "| not found!");
			return GunStatsLoader.GunStatsList[0];
		}

		// Token: 0x040009C5 RID: 2501
		public static List<GunStats> GunStatsList = new List<GunStats>();

		// Token: 0x040009C6 RID: 2502
		public static Dictionary<short, string> UID_Guns = new Dictionary<short, string>();

		// Token: 0x040009C7 RID: 2503
		public static Dictionary<string, short> UID_GunsRev = new Dictionary<string, short>();

		// Token: 0x040009C8 RID: 2504
		private static short UID = 1;

		// Token: 0x040009C9 RID: 2505
		public static short INVALID_UID = 0;
	}
}
