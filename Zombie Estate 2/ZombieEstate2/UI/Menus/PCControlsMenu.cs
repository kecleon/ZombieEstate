using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2.UI.Menus
{
	// Token: 0x02000166 RID: 358
	internal class PCControlsMenu : Menu
	{
		// Token: 0x06000AEE RID: 2798 RVA: 0x0005A4A8 File Offset: 0x000586A8
		public PCControlsMenu() : base(false, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height - 110)))
		{
			this.mReset = base.AddToMenu("Reset To Defaults", new MenuItem.SelectedDelegate(this.Reset), false);
			base.AddToMenu("Back", new MenuItem.SelectedDelegate(this.BackPressed), false);
			this.DrawBGPixel = true;
			this.MenuBG = Global.MenuBG;
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0005A534 File Offset: 0x00058734
		public override void Setup()
		{
			this.title = "";
			int num = (int)Global.GetScreenCenter().X - 400;
			Vector2 pos = new Vector2((float)num, 100f);
			int num2 = 32;
			this.mSetters = new List<PCControllerSetter>();
			this.mSetters.Add(new PCControllerSetter(ButtonPress.MoveNorth, "Move Up", pos));
			pos.Y += (float)num2;
			this.mSetters.Add(new PCControllerSetter(ButtonPress.MoveSouth, "Move Down", pos));
			pos.Y += (float)num2;
			this.mSetters.Add(new PCControllerSetter(ButtonPress.MoveWest, "Move Left", pos));
			pos.Y += (float)num2;
			this.mSetters.Add(new PCControllerSetter(ButtonPress.MoveEast, "Move Right", pos));
			pos.Y += (float)num2;
			pos.Y += (float)num2;
			this.mSetters.Add(new PCControllerSetter(ButtonPress.Fire, "Fire", pos));
			pos.Y += (float)num2;
			pos.Y += (float)num2;
			this.mSetters.Add(new PCControllerSetter(ButtonPress.Reload, "Reload", pos));
			pos.Y += (float)num2;
			this.mSetters.Add(new PCControllerSetter(ButtonPress.Jump, "Jump", pos));
			pos.Y += (float)num2;
			this.mSetters.Add(new PCControllerSetter(ButtonPress.HealthPack, "Use Health Pack", pos));
			pos.Y += (float)num2;
			this.mSetters.Add(new PCControllerSetter(ButtonPress.ViewStats, "View Stats", pos));
			pos.Y += (float)num2;
			pos.Y += (float)num2;
			this.mSetters.Add(new PCControllerSetter(ButtonPress.SwapWeaponsNegative, "Previous Weapon", pos));
			pos.Y += (float)num2;
			this.mSetters.Add(new PCControllerSetter(ButtonPress.SwapWeaponsPositive, "Next Weapon", pos));
			pos.Y += (float)num2;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0005A734 File Offset: 0x00058934
		public override void UpdateMenu()
		{
			base.UpdateMenu();
			if (this.mErrorAlpha > 0f)
			{
				this.mErrorAlpha -= 0.01f;
			}
			if (this.mSettingSetter != null)
			{
				if (InputManager.ButtonPressed(Keys.Escape, 0))
				{
					this.mSettingSetter = null;
					SoundEngine.PlaySound("ze2_navdown", 0.25f);
					return;
				}
				foreach (object obj in Enum.GetValues(typeof(Keys)))
				{
					Keys keys = (Keys)obj;
					if (InputManager.ButtonPressed(keys, 0))
					{
						if (keys == Keys.F12)
						{
							SoundEngine.PlaySound("ze2_navdown", 0.25f);
							this.mErrorAlpha = 1f;
						}
						else if (keys == Keys.Enter)
						{
							SoundEngine.PlaySound("ze2_navdown", 0.25f);
							this.mErrorAlpha = 1f;
						}
						else if (keys == Keys.T)
						{
							SoundEngine.PlaySound("ze2_navdown", 0.25f);
							this.mErrorAlpha = 1f;
						}
						else
						{
							Terminal.WriteMessage("Setting key: " + keys.ToString());
							SoundEngine.PlaySound("ze2_navup", 0.25f);
							this.SetNewKey(this.mSettingSetter.Action, keys);
						}
					}
				}
				if (InputManager.LeftMouseClicked())
				{
					Keys k = Keys.F14;
					Terminal.WriteMessage("Setting key: " + k.ToString());
					SoundEngine.PlaySound("ze2_navup", 0.25f);
					this.SetNewKey(this.mSettingSetter.Action, k);
					return;
				}
				if (InputManager.RightMouseClicked())
				{
					Keys k2 = Keys.F15;
					Terminal.WriteMessage("Setting key: " + k2.ToString());
					SoundEngine.PlaySound("ze2_navup", 0.25f);
					this.SetNewKey(this.mSettingSetter.Action, k2);
					return;
				}
			}
			else
			{
				foreach (PCControllerSetter pccontrollerSetter in this.mSetters)
				{
					if (pccontrollerSetter.Clicked())
					{
						SoundEngine.PlaySound("ze2_navup", 0.25f);
						this.mSettingSetter = pccontrollerSetter;
						break;
					}
				}
			}
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0005A988 File Offset: 0x00058B88
		private void Reset()
		{
			InputManager.RESET_TO_DEFAULTS();
			foreach (PCControllerSetter pccontrollerSetter in this.mSetters)
			{
				pccontrollerSetter.UpdateStuff();
			}
			SoundEngine.PlaySound("ze2_death", 1f);
			Config.Save();
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0005A9F4 File Offset: 0x00058BF4
		private void SetNewKey(ButtonPress action, Keys k)
		{
			if (this.mSettingSetter == null)
			{
				Terminal.WriteMessage("Setting setter = null!", MessageType.ERROR);
				return;
			}
			PCControllerSetter pccontrollerSetter = null;
			foreach (PCControllerSetter pccontrollerSetter2 in this.mSetters)
			{
				if (!(pccontrollerSetter2.Name == this.mSettingSetter.Name) && InputManager.GetKeyForButtonPress(pccontrollerSetter2.Action) == k)
				{
					pccontrollerSetter = pccontrollerSetter2;
					break;
				}
			}
			if (pccontrollerSetter != null)
			{
				Keys keyForButtonPress = InputManager.GetKeyForButtonPress(action);
				InputManager.SetKeybind(pccontrollerSetter.Action, keyForButtonPress);
				pccontrollerSetter.UpdateStuff();
			}
			InputManager.SetKeybind(action, k);
			this.mSettingSetter.UpdateStuff();
			this.mSettingSetter = null;
			Config.Save();
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0005AABC File Offset: 0x00058CBC
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
			foreach (PCControllerSetter pccontrollerSetter in this.mSetters)
			{
				pccontrollerSetter.Draw(spriteBatch, this.mSettingSetter == null);
			}
			if (this.mSettingSetter != null)
			{
				spriteBatch.Draw(Global.Pixel, Global.ScreenRect, Color.Black * 0.85f);
				string text = "Press key for: " + this.mSettingSetter.Name;
				Vector2 pos = VerchickMath.CenterText(Global.StoreFontBig, Global.GetScreenCenter(), text);
				Shadow.DrawString(text, Global.StoreFontBig, pos, 1, Color.White, spriteBatch);
				text = "Press [Esc] to cancel.";
				pos = VerchickMath.CenterText(Global.StoreFont, Global.GetScreenCenter() + new Vector2(0f, 72f), text);
				Shadow.DrawString(text, Global.StoreFont, pos, 1, Color.LightGray, spriteBatch);
				if (this.mErrorAlpha > 0f)
				{
					text = "Cannot bind to that key!";
					pos = VerchickMath.CenterText(Global.StoreFont, Global.GetScreenCenter() + new Vector2(0f, 96f), text);
					Shadow.DrawString(text, Global.StoreFont, pos, 1, Color.Red * this.mErrorAlpha, spriteBatch);
				}
			}
			Vector2 vector = new Vector2((float)(Global.ScreenRect.Width / 2), (float)(this.mReset.Collision.Top - 26));
			vector = VerchickMath.CenterText(Global.StoreFontBig, vector, this.mDance);
			Shadow.DrawString(this.mDance, Global.StoreFontBig, vector, 1, Color.CornflowerBlue, spriteBatch);
		}

		// Token: 0x04000BB4 RID: 2996
		private List<PCControllerSetter> mSetters;

		// Token: 0x04000BB5 RID: 2997
		private PCControllerSetter mSettingSetter;

		// Token: 0x04000BB6 RID: 2998
		private string mDance = "Press [Shift] + 1, 2, or 3 to dance!";

		// Token: 0x04000BB7 RID: 2999
		private float mErrorAlpha;

		// Token: 0x04000BB8 RID: 3000
		private MenuItem mReset;
	}
}
