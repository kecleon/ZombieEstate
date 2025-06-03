using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using ProtoBuf;
using Steamworks;
using ZombieEstate2.UI.Xbox;

namespace ZombieEstate2.XboxSaving
{
	// Token: 0x0200014B RID: 331
	public static class XboxSaverLoader
	{
		// Token: 0x06000A1F RID: 2591 RVA: 0x00052B11 File Offset: 0x00050D11
		public static void AddStats(XboxGamerStats g)
		{
			XboxSaverLoader.mGamerStats.Add(g);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00052B20 File Offset: 0x00050D20
		public static void SaveData()
		{
			if (Global.GamerStats == null)
			{
				Terminal.WriteMessage("Null gamer stats on save?", MessageType.ERROR);
				return;
			}
			string text = Path.Combine(XboxSaverLoader.FOLDER, XboxSaverLoader.FILENAME + SteamHelper.GetLocalID().ToString() + ".sav");
			Terminal.WriteMessage("Saving save data: " + text, MessageType.SAVELOAD);
			try
			{
				int saveHash = XboxSaverLoader.GetSaveHash();
				Global.GamerStats.Hash = saveHash;
				using (StreamWriter streamWriter = new StreamWriter(text, false))
				{
					Serializer.Serialize<XboxGamerStats>(streamWriter.BaseStream, Global.GamerStats);
					streamWriter.Close();
					Terminal.WriteMessage("Done saving data.", MessageType.SAVELOAD);
				}
			}
			catch (Exception ex)
			{
				Terminal.WriteMessage("Error saving save file!", MessageType.ERROR);
				Terminal.WriteMessage(ex.ToString(), MessageType.ERROR);
				Terminal.WriteMessage(ex.StackTrace.ToString(), MessageType.ERROR);
			}
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00052C0C File Offset: 0x00050E0C
		public static void LoadData()
		{
			string text = Path.Combine(XboxSaverLoader.FOLDER, XboxSaverLoader.FILENAME + SteamHelper.GetLocalID().ToString() + ".sav");
			Terminal.WriteMessage("Loading save data: " + text, MessageType.SAVELOAD);
			if (File.Exists(text))
			{
				try
				{
					using (StreamReader streamReader = new StreamReader(text))
					{
						XboxGamerStats xboxGamerStats = Serializer.Deserialize<XboxGamerStats>(streamReader.BaseStream);
						streamReader.Close();
						Terminal.WriteMessage("Done loading save data.", MessageType.SAVELOAD);
						Global.GamerStats = xboxGamerStats;
						int saveHash = XboxSaverLoader.GetSaveHash();
						if (xboxGamerStats.Hash != saveHash)
						{
							Terminal.WriteMessage("Save file tampered with? Creating new.", MessageType.ERROR);
							Global.GamerStats = XboxSaverLoader.InitNew();
							XboxSaverLoader.SaveData();
						}
						return;
					}
				}
				catch (Exception ex)
				{
					Terminal.WriteMessage("Error loading save file!", MessageType.ERROR);
					Terminal.WriteMessage(ex.ToString(), MessageType.ERROR);
					Terminal.WriteMessage(ex.StackTrace.ToString(), MessageType.ERROR);
					Global.GamerStats = XboxSaverLoader.InitNew();
					return;
				}
			}
			Terminal.WriteMessage("No save data found. Creating new one!", MessageType.SAVELOAD);
			Global.GamerStats = XboxSaverLoader.InitNew();
			XboxSaverLoader.SaveData();
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00052D28 File Offset: 0x00050F28
		private static int GetSaveHash()
		{
			int num = 0;
			foreach (string text in Global.GamerStats.UnlockedCharacters)
			{
				num += text.GetHashCode();
			}
			foreach (string text2 in Global.GamerStats.UnlockedHats)
			{
				num += text2.GetHashCode();
			}
			num += Global.GamerStats.mPoints.GetHashCode();
			return num;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00052DE0 File Offset: 0x00050FE0
		public static XboxGamerStats InitNew()
		{
			return new XboxGamerStats
			{
				GamerName = SteamFriends.GetPersonaName(),
				UnlockedCharacters = new List<string>(),
				UnlockedHats = new List<string>(),
				Points = 0
			};
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00052E0F File Offset: 0x0005100F
		public static void ClearAllData()
		{
			Global.GamerStats = XboxSaverLoader.InitNew();
		}

		// Token: 0x04000A9D RID: 2717
		public static string FOLDER = "SaveData";

		// Token: 0x04000A9E RID: 2718
		private static string FILENAME = "save_";

		// Token: 0x04000A9F RID: 2719
		private static List<XboxGamerStats> mGamerStats = new List<XboxGamerStats>();

		// Token: 0x04000AA0 RID: 2720
		private static LoadIcon mSaveIcon = new LoadIcon(new Rectangle(Global.GetSafeScreenArea().Right - 32, Global.GetSafeScreenArea().Bottom - 32, 32, 32));

		// Token: 0x02000224 RID: 548
		// (Invoke) Token: 0x06000DFC RID: 3580
		public delegate void SelectedDelegate();
	}
}
