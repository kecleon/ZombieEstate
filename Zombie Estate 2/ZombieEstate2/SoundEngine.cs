using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ZombieEstate2
{
	// Token: 0x020000A6 RID: 166
	public static class SoundEngine
	{
		// Token: 0x06000443 RID: 1091 RVA: 0x0001FA84 File Offset: 0x0001DC84
		public static void Init()
		{
			SoundEngine.Sounds = new Dictionary<string, SoundWrapper>();
			foreach (string text in SoundEngine.GetFiles())
			{
				SoundEngine.Sounds.Add(text, SoundEngine.LoadSound(text));
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0001FAEC File Offset: 0x0001DCEC
		public static void Update()
		{
			if (SoundEngine.TEMP_NO_SOUND_TIMER > 0f)
			{
				SoundEngine.TEMP_NO_SOUND_TIMER -= Global.REAL_GAME_TIME;
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0001FB0A File Offset: 0x0001DD0A
		public static void PlaySound(string name)
		{
			SoundEngine.PlaySound(name, Vector3.Zero);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001FB17 File Offset: 0x0001DD17
		public static void PlaySound(string name, float volMod)
		{
			if (!SoundEngine.NOSOUND && SoundEngine.TEMP_NO_SOUND_TIMER <= 0f)
			{
				SoundEngine.Sounds[name].PlaySound(Vector3.Zero, volMod);
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0001FB42 File Offset: 0x0001DD42
		public static void PlaySound(string name, Vector3 pos)
		{
			if (!SoundEngine.NOSOUND && SoundEngine.TEMP_NO_SOUND_TIMER <= 0f)
			{
				SoundEngine.Sounds[name].PlaySound(pos);
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001FB68 File Offset: 0x0001DD68
		public static void PlaySound(string name, Vector3 pos, float low, float high)
		{
			if (!SoundEngine.NOSOUND && SoundEngine.TEMP_NO_SOUND_TIMER <= 0f)
			{
				SoundEngine.Sounds[name].PlaySound(pos, low, high);
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0001FB90 File Offset: 0x0001DD90
		public static void PlaySound(string name, Vector3 pos, float low, float high, float vol)
		{
			if (!SoundEngine.NOSOUND && SoundEngine.TEMP_NO_SOUND_TIMER <= 0f)
			{
				SoundEngine.Sounds[name].PlaySound(pos, low, high, vol);
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0001FBBA File Offset: 0x0001DDBA
		private static SoundWrapper LoadSound(string filename)
		{
			return new SoundWrapper(filename);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001FBC4 File Offset: 0x0001DDC4
		public static Dictionary<string, T> LoadContent<T>(this ContentManager contentManager, string contentFolder)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException();
			}
			Dictionary<string, T> dictionary = new Dictionary<string, T>();
			FileInfo[] files = directoryInfo.GetFiles("*.*");
			for (int i = 0; i < files.Length; i++)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(files[i].Name);
				dictionary[fileNameWithoutExtension] = contentManager.Load<T>(string.Concat(new string[]
				{
					contentManager.RootDirectory,
					"/",
					contentFolder,
					"/",
					fileNameWithoutExtension
				}));
			}
			return dictionary;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0001FC5C File Offset: 0x0001DE5C
		public static List<string> GetFiles()
		{
			List<string> list = new List<string>();
			DirectoryInfo directoryInfo = new DirectoryInfo(Global.Content.RootDirectory + "\\Sounds");
			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException();
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*").ToList<FileInfo>())
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
				list.Add(fileNameWithoutExtension);
			}
			return list;
		}

		// Token: 0x04000422 RID: 1058
		private static Dictionary<string, SoundWrapper> Sounds;

		// Token: 0x04000423 RID: 1059
		private static bool NOSOUND;

		// Token: 0x04000424 RID: 1060
		public static float TEMP_NO_SOUND_TIMER;
	}
}
