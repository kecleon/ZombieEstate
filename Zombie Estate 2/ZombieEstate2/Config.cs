using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x0200004A RID: 74
	public class Config
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000C430 File Offset: 0x0000A630
		public static Config Instance
		{
			get
			{
				if (Config.mInstance == null)
				{
					try
					{
						XMLSaverLoader.LoadObject<Config>(Config.Filename, out Config.mInstance);
						InputManager.mPCControls = Config.mInstance.ConvertFromConfig();
					}
					catch
					{
						Terminal.WriteMessage("No config found. Making new one.", MessageType.DEBUG);
						Config.mInstance = new Config();
						DisplayMode currentDisplayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
						Config.mInstance.ScreenWidth = currentDisplayMode.Width;
						Config.mInstance.ScreenHeight = currentDisplayMode.Height;
						Config.mInstance.Controls = Config.mInstance.ConvertControls(InputManager.mPCControls);
					}
				}
				return Config.mInstance;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000C4D8 File Offset: 0x0000A6D8
		public static void Save()
		{
			try
			{
				if (Config.mInstance != null)
				{
					Config.mInstance.Controls = Config.mInstance.ConvertControls(InputManager.mPCControls);
					XMLSaverLoader.SaveObject<Config>(Config.Filename, Config.mInstance);
				}
			}
			catch (Exception ex)
			{
				Terminal.WriteMessage("Error saving config: " + ex.ToString(), MessageType.ERROR);
				Terminal.WriteMessage(ex.StackTrace.ToString(), MessageType.ERROR);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000C554 File Offset: 0x0000A754
		public bool MusicOn
		{
			get
			{
				return this.MusicVolume != 0;
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000C55F File Offset: 0x0000A75F
		public List<ConfigControls> ConvertControls(Dictionary<ButtonPress, List<Keys>> controls)
		{
			return (from kv in controls
			select new ConfigControls
			{
				Button = kv.Key,
				Keys = kv.Value
			}).ToList<ConfigControls>();
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000C58C File Offset: 0x0000A78C
		public Dictionary<ButtonPress, List<Keys>> ConvertFromConfig()
		{
			Dictionary<ButtonPress, List<Keys>> dictionary = this.Controls.ToDictionary((ConfigControls i) => i.Button, (ConfigControls i) => i.Keys);
			if (!dictionary.ContainsKey(ButtonPress.OpenStore))
			{
				Terminal.WriteMessage("Missing OpenStore button - adding.");
				dictionary.Add(ButtonPress.OpenStore, new List<Keys>
				{
					Keys.Space
				});
			}
			return dictionary;
		}

		// Token: 0x04000122 RID: 290
		private static string Filename = "Config.xml";

		// Token: 0x04000123 RID: 291
		private static Config mInstance;

		// Token: 0x04000124 RID: 292
		public int ScreenWidth = 1980;

		// Token: 0x04000125 RID: 293
		public int ScreenHeight = 1200;

		// Token: 0x04000126 RID: 294
		public int SfxVolume = 40;

		// Token: 0x04000127 RID: 295
		public int MusicVolume = 50;

		// Token: 0x04000128 RID: 296
		public ScreenMode ScreenMode = ScreenMode.FullScreen;

		// Token: 0x04000129 RID: 297
		public List<ConfigControls> Controls = new List<ConfigControls>();

		// Token: 0x0400012A RID: 298
		public bool VSync = true;

		// Token: 0x0400012B RID: 299
		public bool ScreenShakeEnabled = true;

		// Token: 0x0400012C RID: 300
		public bool DrawNetDiag;

		// Token: 0x0400012D RID: 301
		public bool DrawTerminal;

		// Token: 0x0400012E RID: 302
		public bool Log;
	}
}
