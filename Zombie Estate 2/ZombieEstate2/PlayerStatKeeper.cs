using System;
using System.Collections.Generic;

namespace ZombieEstate2
{
	// Token: 0x02000090 RID: 144
	public static class PlayerStatKeeper
	{
		// Token: 0x060003BE RID: 958 RVA: 0x0001B41C File Offset: 0x0001961C
		public static void InitCharSettings()
		{
			if (PlayerStatKeeper.CharacterSettings.Count != 0)
			{
				return;
			}
			PlayerStatKeeper.CharacterSettings = new List<CharacterSettings>();
			PlayerStatKeeper.LoadChar("Jerry");
			PlayerStatKeeper.LoadChar("Jess");
			PlayerStatKeeper.LoadChar("Dave");
			PlayerStatKeeper.LoadChar("Specimen Shawn");
			PlayerStatKeeper.LoadChar("Snomis Gerg");
			PlayerStatKeeper.LoadChar("John");
			PlayerStatKeeper.LoadChar("Linda");
			PlayerStatKeeper.LoadChar("Jack");
			PlayerStatKeeper.LoadChar("Cooper");
			PlayerStatKeeper.LoadChar("Cherry");
			PlayerStatKeeper.LoadChar("Smiley");
			PlayerStatKeeper.LoadChar("Mrs. Smiley");
			PlayerStatKeeper.LoadChar("George the Wizard");
			PlayerStatKeeper.LoadChar("Madam Puff Puff");
			PlayerStatKeeper.LoadChar("Ducky");
			PlayerStatKeeper.LoadChar("Ninja");
			PlayerStatKeeper.LoadChar("Doc");
			PlayerStatKeeper.LoadChar("Andre");
			PlayerStatKeeper.LoadChar("Colonel Popcorn");
			PlayerStatKeeper.LoadChar("HotCheese");
			PlayerStatKeeper.LoadChar("Dawg");
			PlayerStatKeeper.LoadChar("Pup");
			PlayerStatKeeper.LoadChar("Rob");
			PlayerStatKeeper.LoadChar("Mr. Soap");
			PlayerStatKeeper.LoadChar("Bawnd");
			PlayerStatKeeper.LoadChar("Nick");
			PlayerStatKeeper.LoadChar("Doug");
			PlayerStatKeeper.LoadChar("Sheep");
			PlayerStatKeeper.LoadChar("Bubba");
			PlayerStatKeeper.LoadChar("Pizza Dude");
			PlayerStatKeeper.LoadChar("Dominick");
			PlayerStatKeeper.LoadChar("Iris Twinklespice");
			PlayerStatKeeper.LoadChar("Madam Broccoli");
			PlayerStatKeeper.LoadChar("DJ McButtButt");
			PlayerStatKeeper.LoadChar("Super Bixanna");
			PlayerStatKeeper.LoadChar("DEFAULT");
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0001B5A8 File Offset: 0x000197A8
		private static void LoadChar(string name)
		{
			CharacterSettings characterSettings;
			XMLSaverLoader.LoadObject<CharacterSettings>(".\\Data\\Characters\\" + name + ".chr", out characterSettings);
			characterSettings.Properties.HealRecievedMod = 0f;
			if (characterSettings.name == "DEFAULT")
			{
				PlayerStatKeeper.DEFAULT_CHAR = characterSettings;
				return;
			}
			PlayerStatKeeper.CharacterSettings.Add(characterSettings);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0001B604 File Offset: 0x00019804
		public static Dictionary<CharacterSettings, CharacterStats> LoadPlayerStatDictionary()
		{
			Dictionary<CharacterSettings, CharacterStats> dictionary = new Dictionary<CharacterSettings, CharacterStats>();
			new List<CharacterStats>();
			foreach (CharacterSettings characterSettings in PlayerStatKeeper.CharacterSettings)
			{
				default(CharacterStats).CharacterName = characterSettings.name;
				dictionary.Add(characterSettings, PlayerStatKeeper.InitStats(characterSettings));
			}
			return dictionary;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00002EF9 File Offset: 0x000010F9
		public static void SaveCharacterStats(Player player, CharacterStats statsToSave)
		{
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001B680 File Offset: 0x00019880
		public static CharacterSettings GetSettings(string name)
		{
			for (int i = 0; i < PlayerStatKeeper.CharacterSettings.Count; i++)
			{
				if (PlayerStatKeeper.CharacterSettings[i].name == name)
				{
					return PlayerStatKeeper.CharacterSettings[i];
				}
			}
			Terminal.WriteMessage("ERROR: Character settings |" + name + "| not found!", MessageType.ERROR);
			return PlayerStatKeeper.CharacterSettings[0];
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001B6E8 File Offset: 0x000198E8
		public static CharacterStats InitStats(CharacterSettings settings)
		{
			CharacterStats characterStats = new CharacterStats
			{
				CharacterName = settings.name,
				GamerName = "Bob",
				Ammo = new int[6],
				MaxAmmo = new int[6]
			};
			characterStats.Ammo[0] = 0;
			characterStats.MaxAmmo[0] = 200;
			characterStats.Ammo[1] = 0;
			characterStats.MaxAmmo[1] = 600;
			characterStats.Ammo[2] = 0;
			characterStats.MaxAmmo[2] = 80;
			characterStats.Ammo[3] = 0;
			characterStats.MaxAmmo[3] = 20;
			characterStats.Ammo[4] = 0;
			characterStats.MaxAmmo[4] = 4;
			TalentManager.AddGenericTalents();
			characterStats.Talents = TalentManager.GetTalents(settings);
			return characterStats;
		}

		// Token: 0x04000384 RID: 900
		public static List<CharacterSettings> CharacterSettings = new List<CharacterSettings>();

		// Token: 0x04000385 RID: 901
		public static CharacterSettings DEFAULT_CHAR;
	}
}
