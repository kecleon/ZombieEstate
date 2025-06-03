using System;
using System.Collections.Generic;

namespace ZombieEstate2
{
	// Token: 0x0200000D RID: 13
	public static class AchievementSystem
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002B23 File Offset: 0x00000D23
		public static void Init()
		{
			AchievementSystem.Achievements = new List<Achievement>();
			Terminal.WriteMessage("Loading Achievements", MessageType.SAVELOAD);
			Terminal.WriteMessage("---------------------------------------------", MessageType.SAVELOAD);
			Terminal.WriteMessage("---------------------------------------------", MessageType.SAVELOAD);
			Terminal.WriteMessage("Loading Achievements Completed", MessageType.SAVELOAD);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002B5C File Offset: 0x00000D5C
		private static void LoadAchievement(string name)
		{
			Achievement item;
			XMLSaverLoader.LoadObject<Achievement>(AchievementSystem.folder + name + ".ach", out item);
			AchievementSystem.Achievements.Add(item);
			Terminal.WriteMessage("ACHIEVE: |" + name + "| Loaded.", MessageType.SAVELOAD);
		}

		// Token: 0x04000022 RID: 34
		private static string folder = "\\Achievements\\";

		// Token: 0x04000023 RID: 35
		public static List<Achievement> Achievements;
	}
}
