using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.UI.Menus;

namespace ZombieEstate2
{
	// Token: 0x020000E9 RID: 233
	internal class OptionsMenu : Menu
	{
		// Token: 0x0600062B RID: 1579 RVA: 0x0002DD8C File Offset: 0x0002BF8C
		public OptionsMenu() : base(true, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2 - 210)))
		{
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0002DDBC File Offset: 0x0002BFBC
		public override void Setup()
		{
			OptionsMenu.SetupRes();
			this.mResIndex = this.GetResIndex();
			this.mMusicVolume = base.AddToMenu("Music Volume - " + Config.Instance.MusicVolume + "%", null, true);
			this.mMusicVolume.SelectedFunction_Picker = new MenuItem.SelectedDelegatePicker(this.UpdateMusic);
			this.mSFXVolume = base.AddToMenu("Sound Effect Volume - " + Config.Instance.SfxVolume + "%", null, true);
			this.mSFXVolume.SelectedFunction_Picker = new MenuItem.SelectedDelegatePicker(this.UpdateSoundVolume);
			base.AddToMenu("Controls - PC", new MenuItem.SelectedDelegate(this.HelpMenuPC), "Configure the controls of Zombie Estate 2.");
			base.AddToMenu("Controls - Controller", new MenuItem.SelectedDelegate(this.HelpMenu), "See the controls of Zombie Estate 2.");
			this.mFullScreenToggle = (Config.Instance.ScreenMode == ScreenMode.FullScreen);
			this.mVSyncToggle = Config.Instance.VSync;
			this.mScreenShakeToggle = Config.Instance.ScreenShakeEnabled;
			if (Global.GameState != GameState.Playing)
			{
				this.mResolution = base.AddToMenu("Resolution " + OptionsMenu.GetResText(OptionsMenu.Resolutions[this.mResIndex]), null, true);
				this.mResolution.SelectedFunction_Picker = new MenuItem.SelectedDelegatePicker(this.ToggleRes);
				this.mWindowMode = base.AddToMenu((!this.mFullScreenToggle) ? "Windowed Mode" : "Full Screen", new MenuItem.SelectedDelegate(this.ToggleWindow), false);
				this.mVSync = base.AddToMenu(this.mVSyncToggle ? "VSync On" : "VSync Off", new MenuItem.SelectedDelegate(this.ToggleVSync), false);
			}
			this.mScreenShake = base.AddToMenu(this.mScreenShakeToggle ? "Screen Shake Enabled" : "Screen Shake Disabled", new MenuItem.SelectedDelegate(this.UpdateScreenShake), false);
			this.title = "Options";
			this.MenuBG = Global.MenuBG;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0002DFB8 File Offset: 0x0002C1B8
		private void UpdateMusic(bool pos)
		{
			if (pos)
			{
				Config.Instance.MusicVolume += 10;
				if (Config.Instance.MusicVolume > 100)
				{
					Config.Instance.MusicVolume = 100;
				}
			}
			else
			{
				Config.Instance.MusicVolume -= 10;
				if (Config.Instance.MusicVolume < 0)
				{
					Config.Instance.MusicVolume = 0;
				}
			}
			this.mMusicVolume.Text = "Music Volume - " + Config.Instance.MusicVolume + "%";
			Config.Save();
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0002E050 File Offset: 0x0002C250
		private void UpdateScreenShake()
		{
			this.mScreenShakeToggle = !this.mScreenShakeToggle;
			this.mScreenShake.Text = (this.mScreenShakeToggle ? "Screen Shake Enabled" : "Screen Shake Disabled");
			Config.Instance.ScreenShakeEnabled = this.mScreenShakeToggle;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0002E090 File Offset: 0x0002C290
		private void Delete()
		{
			MenuManager.PushMenu(new DeleteDataMenu());
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0002E09C File Offset: 0x0002C29C
		private void ToggleRes(bool pos)
		{
			if (pos)
			{
				this.mResIndex++;
				if (this.mResIndex >= OptionsMenu.Resolutions.Count)
				{
					this.mResIndex = OptionsMenu.Resolutions.Count - 1;
				}
			}
			else
			{
				this.mResIndex--;
				if (this.mResIndex < 0)
				{
					this.mResIndex = 0;
				}
			}
			this.mResolution.Text = "Resolution " + OptionsMenu.GetResText(OptionsMenu.Resolutions[this.mResIndex]);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0002E128 File Offset: 0x0002C328
		private void HelpMenu()
		{
			ScreenFader.Fade(delegate()
			{
				MenuManager.PushMenu(new HelpMenu());
			});
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0002E14E File Offset: 0x0002C34E
		private void HelpMenuPC()
		{
			ScreenFader.Fade(delegate()
			{
				MenuManager.PushMenu(new PCControlsMenu());
			});
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0002E174 File Offset: 0x0002C374
		private void ToggleWindow()
		{
			this.mFullScreenToggle = !this.mFullScreenToggle;
			this.mWindowMode.Text = ((!this.mFullScreenToggle) ? "Windowed Mode" : "Full Screen");
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0002E1A4 File Offset: 0x0002C3A4
		private void UpdateSoundVolume(bool pos)
		{
			if (pos)
			{
				Config.Instance.SfxVolume += 10;
				if (Config.Instance.SfxVolume > 100)
				{
					Config.Instance.SfxVolume = 100;
				}
			}
			else
			{
				Config.Instance.SfxVolume -= 10;
				if (Config.Instance.SfxVolume < 0)
				{
					Config.Instance.SfxVolume = 0;
				}
			}
			this.mSFXVolume.Text = "Sound Effect Volume - " + Config.Instance.SfxVolume + "%";
			Config.Save();
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0002E23C File Offset: 0x0002C43C
		private void ToggleVSync()
		{
			this.mVSyncToggle = !this.mVSyncToggle;
			this.mVSync.Text = (this.mVSyncToggle ? "VSync On" : "VSync Off");
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0002E26C File Offset: 0x0002C46C
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0002E275 File Offset: 0x0002C475
		private static string GetResText(int w, int h)
		{
			return w.ToString() + "x" + h.ToString();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0002E28F File Offset: 0x0002C48F
		private static string GetResText(Tuple<int, int> res)
		{
			return OptionsMenu.GetResText(res.Item1, res.Item2);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0002E2A4 File Offset: 0x0002C4A4
		private int GetResIndex()
		{
			for (int i = 0; i < OptionsMenu.Resolutions.Count; i++)
			{
				if (OptionsMenu.Resolutions[i].Item1 == Config.Instance.ScreenWidth && OptionsMenu.Resolutions[i].Item2 == Config.Instance.ScreenHeight)
				{
					return i;
				}
			}
			Terminal.WriteMessage(string.Concat(new object[]
			{
				"Error, didnt find res in list: ",
				Config.Instance.ScreenWidth,
				"x",
				Config.Instance.ScreenHeight
			}), MessageType.ERROR);
			return 0;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0002E348 File Offset: 0x0002C548
		private static void SetupRes()
		{
			if (OptionsMenu.Resolutions == null)
			{
				OptionsMenu.Resolutions = new List<Tuple<int, int>>();
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1024, 768));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1280, 720));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1280, 768));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1280, 800));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1280, 960));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1280, 1024));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1360, 768));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1366, 768));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1440, 900));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1536, 864));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1600, 900));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1600, 1200));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1680, 1050));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1920, 1080));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(1920, 1200));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(2560, 1080));
				OptionsMenu.Resolutions.Add(new Tuple<int, int>(2560, 1440));
				foreach (DisplayMode displayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
				{
					Tuple<int, int> item = new Tuple<int, int>(displayMode.Width, displayMode.Height);
					if (!OptionsMenu.Resolutions.Contains(item) && displayMode.Width >= 1280)
					{
						OptionsMenu.Resolutions.Add(item);
					}
				}
				OptionsMenu.Resolutions.Sort(delegate(Tuple<int, int> x, Tuple<int, int> y)
				{
					if (x.Item1 == y.Item1)
					{
						return y.Item2.CompareTo(x.Item2);
					}
					return y.Item1.CompareTo(x.Item1);
				});
				OptionsMenu.Resolutions.Reverse();
			}
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0002E5BC File Offset: 0x0002C7BC
		public override void BackPressed()
		{
			Config.Save();
			if (Config.Instance.ScreenWidth != OptionsMenu.Resolutions[this.mResIndex].Item1 || Config.Instance.ScreenHeight != OptionsMenu.Resolutions[this.mResIndex].Item2 || (Config.Instance.ScreenMode == ScreenMode.FullScreen && !this.mFullScreenToggle) || (Config.Instance.ScreenMode != ScreenMode.FullScreen && this.mFullScreenToggle) || Config.Instance.VSync != this.mVSyncToggle || Config.Instance.ScreenShakeEnabled != this.mScreenShakeToggle)
			{
				base.BackPressed();
				MenuManager.PushMenu(new SaveConfigChangesMenu(true, OptionsMenu.Resolutions[this.mResIndex].Item1, OptionsMenu.Resolutions[this.mResIndex].Item2, this.mFullScreenToggle, this.mVSyncToggle));
				return;
			}
			base.BackPressed();
		}

		// Token: 0x040005F8 RID: 1528
		private MenuItem mResolution;

		// Token: 0x040005F9 RID: 1529
		private MenuItem mWindowMode;

		// Token: 0x040005FA RID: 1530
		private MenuItem mVSync;

		// Token: 0x040005FB RID: 1531
		private MenuItem mSFXVolume;

		// Token: 0x040005FC RID: 1532
		private MenuItem mMusicVolume;

		// Token: 0x040005FD RID: 1533
		private MenuItem mScreenShake;

		// Token: 0x040005FE RID: 1534
		private int mResIndex;

		// Token: 0x040005FF RID: 1535
		private bool mFullScreenToggle;

		// Token: 0x04000600 RID: 1536
		private bool mVSyncToggle;

		// Token: 0x04000601 RID: 1537
		private bool mScreenShakeToggle;

		// Token: 0x04000602 RID: 1538
		public static List<Tuple<int, int>> Resolutions;
	}
}
