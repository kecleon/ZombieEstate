using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x02000069 RID: 105
	public static class InputManager
	{
		// Token: 0x06000255 RID: 597 RVA: 0x00012858 File Offset: 0x00010A58
		public static bool InPCMode(int playerIndex)
		{
			return InputManager.mInPCMode[playerIndex];
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00012861 File Offset: 0x00010A61
		public static Dictionary<ButtonPress, List<Keys>> PCControls
		{
			get
			{
				return InputManager.mPCControls;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00012868 File Offset: 0x00010A68
		public static Dictionary<ButtonPress, List<Buttons>> XboxControls
		{
			get
			{
				return InputManager.mXboxControls;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00012870 File Offset: 0x00010A70
		public static void SetUsingKeyboard(int index)
		{
			for (int i = 0; i < 4; i++)
			{
				InputManager.mInPCMode[i] = false;
			}
			InputManager.mInPCMode[index] = true;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0001289C File Offset: 0x00010A9C
		static InputManager()
		{
			InputManager.mInPCMode = new bool[4];
			InputManager.mInPCMode[0] = false;
			InputManager.mInPCMode[1] = false;
			InputManager.mInPCMode[2] = false;
			InputManager.mInPCMode[3] = false;
			InputManager.mPCControls = new Dictionary<ButtonPress, List<Keys>>();
			InputManager.mXboxControls = new Dictionary<ButtonPress, List<Buttons>>();
			foreach (object obj in Enum.GetValues(typeof(ButtonPress)))
			{
				ButtonPress key = (ButtonPress)obj;
				InputManager.mPCControls.Add(key, new List<Keys>());
			}
			foreach (object obj2 in Enum.GetValues(typeof(ButtonPress)))
			{
				ButtonPress key2 = (ButtonPress)obj2;
				InputManager.mXboxControls.Add(key2, new List<Buttons>());
			}
			InputManager.mPCControls[ButtonPress.Affirmative].Add(Keys.Enter);
			InputManager.mPCControls[ButtonPress.Affirmative].Add(Keys.Space);
			InputManager.mPCControls[ButtonPress.Negative].Add(Keys.Escape);
			InputManager.mPCControls[ButtonPress.Negative].Add(Keys.Back);
			InputManager.mPCControls[ButtonPress.Fire].Add(Keys.F14);
			InputManager.mPCControls[ButtonPress.Jump].Add(Keys.Space);
			InputManager.mPCControls[ButtonPress.Ready].Add(Keys.Enter);
			InputManager.mPCControls[ButtonPress.Pause].Add(Keys.Escape);
			InputManager.mPCControls[ButtonPress.SwapWeaponsNegative].Add(Keys.Q);
			InputManager.mPCControls[ButtonPress.SwapWeaponsPositive].Add(Keys.E);
			InputManager.mPCControls[ButtonPress.HealthPack].Add(Keys.F);
			InputManager.mPCControls[ButtonPress.ViewStats].Add(Keys.LeftShift);
			InputManager.mPCControls[ButtonPress.MoveNorth].Add(Keys.W);
			InputManager.mPCControls[ButtonPress.MoveSouth].Add(Keys.S);
			InputManager.mPCControls[ButtonPress.MoveEast].Add(Keys.D);
			InputManager.mPCControls[ButtonPress.MoveWest].Add(Keys.A);
			InputManager.mPCControls[ButtonPress.MoveNorth].Add(Keys.Up);
			InputManager.mPCControls[ButtonPress.MoveSouth].Add(Keys.Down);
			InputManager.mPCControls[ButtonPress.MoveEast].Add(Keys.Right);
			InputManager.mPCControls[ButtonPress.MoveWest].Add(Keys.Left);
			InputManager.mPCControls[ButtonPress.Reload].Add(Keys.R);
			InputManager.mPCControls[ButtonPress.Inventory].Add(Keys.E);
			InputManager.mPCControls[ButtonPress.OpenStore].Add(Keys.Space);
			InputManager.mXboxControls[ButtonPress.Affirmative].Add(Buttons.Start);
			InputManager.mXboxControls[ButtonPress.Affirmative].Add(Buttons.A);
			InputManager.mXboxControls[ButtonPress.Negative].Add(Buttons.B);
			InputManager.mXboxControls[ButtonPress.Fire].Add(Buttons.RightTrigger);
			InputManager.mXboxControls[ButtonPress.Jump].Add(Buttons.A);
			InputManager.mXboxControls[ButtonPress.Ready].Add(Buttons.Back);
			InputManager.mXboxControls[ButtonPress.Pause].Add(Buttons.Start);
			InputManager.mXboxControls[ButtonPress.SwapWeaponsNegative].Add(Buttons.LeftShoulder);
			InputManager.mXboxControls[ButtonPress.SwapWeaponsPositive].Add(Buttons.RightShoulder);
			InputManager.mXboxControls[ButtonPress.HealthPack].Add(Buttons.Y);
			InputManager.mXboxControls[ButtonPress.ViewStats].Add(Buttons.LeftTrigger);
			InputManager.mXboxControls[ButtonPress.Reload].Add(Buttons.X);
			InputManager.mXboxControls[ButtonPress.Inventory].Add(Buttons.Y);
			InputManager.mXboxControls[ButtonPress.OpenStore].Add(Buttons.A);
			InputManager.mXboxControls[ButtonPress.MoveNorth].Add(Buttons.LeftThumbstickUp);
			InputManager.mXboxControls[ButtonPress.MoveSouth].Add(Buttons.LeftThumbstickDown);
			InputManager.mXboxControls[ButtonPress.MoveEast].Add(Buttons.LeftThumbstickRight);
			InputManager.mXboxControls[ButtonPress.MoveWest].Add(Buttons.LeftThumbstickLeft);
			InputManager.mXboxControls[ButtonPress.MoveNorth].Add(Buttons.DPadUp);
			InputManager.mXboxControls[ButtonPress.MoveSouth].Add(Buttons.DPadDown);
			InputManager.mXboxControls[ButtonPress.MoveEast].Add(Buttons.DPadRight);
			InputManager.mXboxControls[ButtonPress.MoveWest].Add(Buttons.DPadLeft);
			InputManager.mXboxControls[ButtonPress.XboxMoveNorth].Add(Buttons.LeftThumbstickUp);
			InputManager.mXboxControls[ButtonPress.XboxMoveSouth].Add(Buttons.LeftThumbstickDown);
			InputManager.mXboxControls[ButtonPress.XboxMoveEast].Add(Buttons.LeftThumbstickRight);
			InputManager.mXboxControls[ButtonPress.XboxMoveWest].Add(Buttons.LeftThumbstickLeft);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00012D90 File Offset: 0x00010F90
		public static bool LeftHeld(int index)
		{
			return InputManager.mKeyState.IsKeyDown(Keys.Left) || InputManager.mKeyState.IsKeyDown(Keys.A);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00012DB1 File Offset: 0x00010FB1
		public static bool RightHeld(int index)
		{
			return InputManager.mKeyState.IsKeyDown(Keys.Right) || InputManager.mKeyState.IsKeyDown(Keys.D);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00012DD2 File Offset: 0x00010FD2
		public static bool DownHeld(int index)
		{
			return InputManager.mKeyState.IsKeyDown(Keys.Down) || InputManager.mKeyState.IsKeyDown(Keys.S);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00012DF3 File Offset: 0x00010FF3
		public static bool UpHeld(int index)
		{
			return InputManager.mKeyState.IsKeyDown(Keys.Up) || InputManager.mKeyState.IsKeyDown(Keys.W);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00012E14 File Offset: 0x00011014
		public static bool LeftHeldRightThmb(int index)
		{
			return InputManager.mGamePadStates[index].ThumbSticks.Right.X < -0.1f;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00012E48 File Offset: 0x00011048
		public static bool RightHeldRightThmb(int index)
		{
			return InputManager.mGamePadStates[index].ThumbSticks.Right.X > 0.1f;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00012E7C File Offset: 0x0001107C
		public static bool DownHeldRightThmb(int index)
		{
			return InputManager.mGamePadStates[index].ThumbSticks.Right.Y < -0.1f;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00012EB0 File Offset: 0x000110B0
		public static bool UpHeldRightThmb(int index)
		{
			return InputManager.mGamePadStates[index].ThumbSticks.Right.Y > 0.1f;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00012EE4 File Offset: 0x000110E4
		public static bool ButtonHeld(ButtonPress button, int playerIndex, bool OVERRIDE_PLAYER_INDEX = false)
		{
			if (InputManager.LOCK_INPUT)
			{
				return false;
			}
			PlayerInfo player = PlayerManager.GetPlayer(playerIndex);
			if (player == null && !OVERRIDE_PLAYER_INDEX)
			{
				return false;
			}
			if (!OVERRIDE_PLAYER_INDEX && player.UsingController)
			{
				playerIndex = player.ControllerIndex;
			}
			if (!OVERRIDE_PLAYER_INDEX && !player.UsingController)
			{
				using (List<Keys>.Enumerator enumerator = InputManager.mPCControls[button].GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Keys keys = enumerator.Current;
						if (keys == Keys.F14)
						{
							if (InputManager.mMouseState.LeftButton == ButtonState.Pressed)
							{
								return true;
							}
						}
						else if (keys == Keys.F15)
						{
							if (InputManager.mMouseState.RightButton == ButtonState.Pressed)
							{
								return true;
							}
						}
						else if (InputManager.mKeyState.IsKeyDown(keys))
						{
							return true;
						}
					}
					return false;
				}
			}
			foreach (Buttons buttons in InputManager.mXboxControls[button])
			{
				if (buttons == Buttons.RightTrigger)
				{
					if (InputManager.mGamePadStates[playerIndex].Triggers.Right > InputManager.TRIGGERBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.LeftTrigger)
				{
					if (InputManager.mGamePadStates[playerIndex].Triggers.Left > InputManager.TRIGGERBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.LeftThumbstickLeft)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Left.X < -InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.LeftThumbstickRight)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Left.X > InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.LeftThumbstickUp)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Left.Y > InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.LeftThumbstickDown)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Left.Y < -InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.RightThumbstickLeft)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Right.X < -InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.RightThumbstickRight)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Right.X > InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.RightThumbstickUp)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Right.Y > InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.RightThumbstickDown)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Right.Y < -InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (InputManager.mGamePadStates[playerIndex].IsButtonDown(buttons))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00013250 File Offset: 0x00011450
		public static bool ButtonPressed(ButtonPress button, int playerIndex, bool OVERRIDE_PLAYER_INDEX = false)
		{
			if (InputManager.LOCK_INPUT)
			{
				return false;
			}
			PlayerInfo player = PlayerManager.GetPlayer(playerIndex);
			if (player == null && !OVERRIDE_PLAYER_INDEX)
			{
				return false;
			}
			if (!OVERRIDE_PLAYER_INDEX && player.UsingController)
			{
				playerIndex = player.ControllerIndex;
			}
			if (!OVERRIDE_PLAYER_INDEX && !player.UsingController)
			{
				using (List<Keys>.Enumerator enumerator = InputManager.mPCControls[button].GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Keys keys = enumerator.Current;
						if (keys == Keys.F14)
						{
							if (InputManager.mMouseState.LeftButton == ButtonState.Pressed && InputManager.mPrevMouseState.LeftButton == ButtonState.Released)
							{
								return true;
							}
						}
						else if (keys == Keys.F15)
						{
							if (InputManager.mMouseState.RightButton == ButtonState.Pressed && InputManager.mPrevMouseState.RightButton == ButtonState.Released)
							{
								return true;
							}
						}
						else if (InputManager.mKeyState.IsKeyDown(keys) && InputManager.mPrevKeyState.IsKeyUp(keys))
						{
							return true;
						}
					}
					return false;
				}
			}
			foreach (Buttons buttons in InputManager.mXboxControls[button])
			{
				if (buttons == Buttons.RightTrigger)
				{
					if (InputManager.mGamePadStates[playerIndex].Triggers.Right > InputManager.TRIGGERBUFFER && InputManager.mPrevGamePadStates[playerIndex].Triggers.Right < InputManager.TRIGGERBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.LeftTrigger)
				{
					if (InputManager.mGamePadStates[playerIndex].Triggers.Left > InputManager.TRIGGERBUFFER && InputManager.mPrevGamePadStates[playerIndex].Triggers.Left < InputManager.TRIGGERBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.LeftThumbstickLeft)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Left.X < -InputManager.STICKBUFFER && InputManager.mPrevGamePadStates[playerIndex].ThumbSticks.Left.X > -InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.LeftThumbstickRight)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Left.X > InputManager.STICKBUFFER && InputManager.mPrevGamePadStates[playerIndex].ThumbSticks.Left.X < InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.LeftThumbstickUp)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Left.Y > InputManager.STICKBUFFER && InputManager.mPrevGamePadStates[playerIndex].ThumbSticks.Left.Y < InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.LeftThumbstickDown)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Left.Y < -InputManager.STICKBUFFER && InputManager.mPrevGamePadStates[playerIndex].ThumbSticks.Left.Y > -InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.RightThumbstickLeft)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Right.X < -InputManager.STICKBUFFER && InputManager.mPrevGamePadStates[playerIndex].ThumbSticks.Right.X > -InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.RightThumbstickRight)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Right.X > InputManager.STICKBUFFER && InputManager.mPrevGamePadStates[playerIndex].ThumbSticks.Right.X < InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.RightThumbstickUp)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Right.Y > InputManager.STICKBUFFER && InputManager.mPrevGamePadStates[playerIndex].ThumbSticks.Right.Y < InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (buttons == Buttons.RightThumbstickDown)
				{
					if (InputManager.mGamePadStates[playerIndex].ThumbSticks.Right.Y < -InputManager.STICKBUFFER && InputManager.mPrevGamePadStates[playerIndex].ThumbSticks.Right.Y > -InputManager.STICKBUFFER)
					{
						return true;
					}
				}
				else if (InputManager.mGamePadStates[playerIndex].IsButtonDown(buttons) && InputManager.mPrevGamePadStates[playerIndex].IsButtonUp(buttons))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00013788 File Offset: 0x00011988
		public static bool ALL_ButtonPressed(ButtonPress bp)
		{
			foreach (Keys key in InputManager.mPCControls[bp])
			{
				if (InputManager.mKeyState.IsKeyDown(key) && InputManager.mPrevKeyState.IsKeyUp(key))
				{
					return true;
				}
			}
			for (int i = 0; i < 4; i++)
			{
				if (InputManager.mGamePadStates.Length >= i)
				{
					foreach (Buttons button in InputManager.mXboxControls[bp])
					{
						if (InputManager.mGamePadStates[i].IsButtonDown(button) && InputManager.mPrevGamePadStates[i].IsButtonUp(button))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00013884 File Offset: 0x00011A84
		public static bool New_DPadUp_Pressed(int cIndex)
		{
			return InputManager.mGamePadStates[cIndex].DPad.Up == ButtonState.Pressed && InputManager.mPrevGamePadStates[cIndex].DPad.Up == ButtonState.Released;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000138CC File Offset: 0x00011ACC
		public static bool New_DPadLeft_Pressed(int cIndex)
		{
			return InputManager.mGamePadStates[cIndex].DPad.Left == ButtonState.Pressed && InputManager.mPrevGamePadStates[cIndex].DPad.Left == ButtonState.Released;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00013914 File Offset: 0x00011B14
		public static bool New_DPadRight_Pressed(int cIndex)
		{
			return InputManager.mGamePadStates[cIndex].DPad.Right == ButtonState.Pressed && InputManager.mPrevGamePadStates[cIndex].DPad.Right == ButtonState.Released;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00013959 File Offset: 0x00011B59
		public static Keys GetKeyForButtonPress(ButtonPress bp)
		{
			if (!InputManager.mPCControls.ContainsKey(bp))
			{
				return Keys.PrintScreen;
			}
			if (InputManager.mPCControls[bp].Count == 0)
			{
				return Keys.PrintScreen;
			}
			return InputManager.mPCControls[bp].First<Keys>();
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00013990 File Offset: 0x00011B90
		public static void SetKeybind(ButtonPress bp, Keys k)
		{
			InputManager.mPCControls[bp] = new List<Keys>();
			InputManager.mPCControls[bp].Add(k);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000139B4 File Offset: 0x00011BB4
		public static void RESET_TO_DEFAULTS()
		{
			InputManager.mPCControls = new Dictionary<ButtonPress, List<Keys>>();
			foreach (object obj in Enum.GetValues(typeof(ButtonPress)))
			{
				ButtonPress key = (ButtonPress)obj;
				InputManager.mPCControls.Add(key, new List<Keys>());
			}
			InputManager.mPCControls[ButtonPress.Affirmative].Add(Keys.Enter);
			InputManager.mPCControls[ButtonPress.Affirmative].Add(Keys.Space);
			InputManager.mPCControls[ButtonPress.Negative].Add(Keys.Escape);
			InputManager.mPCControls[ButtonPress.Negative].Add(Keys.Back);
			InputManager.mPCControls[ButtonPress.Fire].Add(Keys.F14);
			InputManager.mPCControls[ButtonPress.Jump].Add(Keys.Space);
			InputManager.mPCControls[ButtonPress.Ready].Add(Keys.Enter);
			InputManager.mPCControls[ButtonPress.Pause].Add(Keys.Escape);
			InputManager.mPCControls[ButtonPress.SwapWeaponsNegative].Add(Keys.Q);
			InputManager.mPCControls[ButtonPress.SwapWeaponsPositive].Add(Keys.E);
			InputManager.mPCControls[ButtonPress.HealthPack].Add(Keys.F);
			InputManager.mPCControls[ButtonPress.ViewStats].Add(Keys.LeftShift);
			InputManager.mPCControls[ButtonPress.MoveNorth].Add(Keys.W);
			InputManager.mPCControls[ButtonPress.MoveSouth].Add(Keys.S);
			InputManager.mPCControls[ButtonPress.MoveEast].Add(Keys.D);
			InputManager.mPCControls[ButtonPress.MoveWest].Add(Keys.A);
			InputManager.mPCControls[ButtonPress.MoveNorth].Add(Keys.Up);
			InputManager.mPCControls[ButtonPress.MoveSouth].Add(Keys.Down);
			InputManager.mPCControls[ButtonPress.MoveEast].Add(Keys.Right);
			InputManager.mPCControls[ButtonPress.MoveWest].Add(Keys.Left);
			InputManager.mPCControls[ButtonPress.Reload].Add(Keys.R);
			InputManager.mPCControls[ButtonPress.Inventory].Add(Keys.E);
			InputManager.mPCControls[ButtonPress.OpenStore].Add(Keys.Space);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00013BD8 File Offset: 0x00011DD8
		public static Point GetMousePosition()
		{
			return new Point(InputManager.mMouseState.X, InputManager.mMouseState.Y);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00013BF3 File Offset: 0x00011DF3
		public static bool LeftMouseHeld()
		{
			return Global.Game.IsActive && !InputManager.LOCK_INPUT && InputManager.mMouseState.LeftButton == ButtonState.Pressed;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00013C1A File Offset: 0x00011E1A
		public static bool LeftMouseClicked()
		{
			return Global.Game.IsActive && !InputManager.LOCK_INPUT && (InputManager.mMouseState.LeftButton == ButtonState.Pressed && InputManager.mPrevMouseState.LeftButton == ButtonState.Released);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00013C4D File Offset: 0x00011E4D
		public static bool RightMouseClicked()
		{
			return Global.Game.IsActive && (InputManager.mMouseState.RightButton == ButtonState.Pressed && InputManager.mPrevMouseState.RightButton == ButtonState.Released);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00013C79 File Offset: 0x00011E79
		public static bool RightMouseHeld()
		{
			return InputManager.mMouseState.RightButton == ButtonState.Pressed;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00013C8B File Offset: 0x00011E8B
		public static bool ButtonPressed(Keys key, int index)
		{
			return InputManager.mKeyState.IsKeyDown(key) && !InputManager.mPrevKeyState.IsKeyDown(key);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00013CAA File Offset: 0x00011EAA
		public static bool ButtonHeld(Keys key, int index)
		{
			return InputManager.mKeyState.IsKeyDown(key);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00013CBC File Offset: 0x00011EBC
		public static bool RTriggerPressed(int index)
		{
			return InputManager.mMouseState.LeftButton == ButtonState.Pressed && InputManager.mPrevMouseState.LeftButton == ButtonState.Released;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00013CDA File Offset: 0x00011EDA
		public static bool RShoulderPressed(int index)
		{
			return InputManager.mKeyState.IsKeyDown(Keys.E) && InputManager.mPrevKeyState.IsKeyUp(Keys.E);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00013CFB File Offset: 0x00011EFB
		public static bool RShoulderHeld(int index)
		{
			return InputManager.mKeyState.IsKeyDown(Keys.E);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00013D0E File Offset: 0x00011F0E
		public static bool LShoulderPressed(int index)
		{
			return InputManager.mKeyState.IsKeyDown(Keys.Q) && InputManager.mPrevKeyState.IsKeyUp(Keys.Q);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00013D30 File Offset: 0x00011F30
		public static bool APressed(int index)
		{
			return (InputManager.mKeyState.IsKeyDown(Keys.Space) && InputManager.mPrevKeyState.IsKeyUp(Keys.Space)) || (InputManager.mGamePadStates[index].Buttons.A == ButtonState.Pressed && InputManager.mPrevGamePadStates[index].Buttons.A == ButtonState.Released);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00013D93 File Offset: 0x00011F93
		public static bool BPressed(int index)
		{
			return InputManager.mKeyState.IsKeyDown(Keys.B) && InputManager.mPrevKeyState.IsKeyUp(Keys.B);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00013DB4 File Offset: 0x00011FB4
		public static bool StartPressed(int index)
		{
			return InputManager.mKeyState.IsKeyDown(Keys.N) && InputManager.mPrevKeyState.IsKeyUp(Keys.N);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00013DD5 File Offset: 0x00011FD5
		public static bool BackPressed(int index)
		{
			return InputManager.mKeyState.IsKeyDown(Keys.Enter) && InputManager.mPrevKeyState.IsKeyUp(Keys.Enter);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00013DF8 File Offset: 0x00011FF8
		public static bool XPressed(int index)
		{
			return (InputManager.mKeyState.IsKeyDown(Keys.R) && InputManager.mPrevKeyState.IsKeyUp(Keys.R)) || (InputManager.mGamePadStates[index].Buttons.X == ButtonState.Pressed && InputManager.mPrevGamePadStates[index].Buttons.X == ButtonState.Released);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00013E5C File Offset: 0x0001205C
		public static bool PrevRTriggerPressed(int index)
		{
			return InputManager.mPrevGamePadStates[index].Triggers.Right > 0.1f;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00013E88 File Offset: 0x00012088
		public static void Init()
		{
			InputManager.mPrevGamePadStates = new GamePadState[4];
			InputManager.mGamePadStates = new GamePadState[4];
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00013EA0 File Offset: 0x000120A0
		public static void UpdateCurrent()
		{
			if (InputManager.mLockoutTimer.Running())
			{
				InputManager.mMouseState = default(MouseState);
				InputManager.mKeyState = default(KeyboardState);
				return;
			}
			InputManager.mMouseState = Mouse.GetState();
			InputManager.mKeyState = Keyboard.GetState();
			InputManager.mGamePadStates[0] = GamePad.GetState(PlayerIndex.One);
			InputManager.mGamePadStates[1] = GamePad.GetState(PlayerIndex.Two);
			InputManager.mGamePadStates[2] = GamePad.GetState(PlayerIndex.Three);
			InputManager.mGamePadStates[3] = GamePad.GetState(PlayerIndex.Four);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00013F28 File Offset: 0x00012128
		public static void UpdatePrevs()
		{
			InputManager.mPrevMouseState = InputManager.mMouseState;
			InputManager.mPrevKeyState = InputManager.mKeyState;
			for (int i = 0; i < 4; i++)
			{
				InputManager.mPrevGamePadStates[i] = InputManager.mGamePadStates[i];
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00013F6C File Offset: 0x0001216C
		public static float RightStickAngle(int index)
		{
			Vector2 vector = new Vector2(InputManager.mGamePadStates[index].ThumbSticks.Right.X, InputManager.mGamePadStates[index].ThumbSticks.Right.Y);
			vector.Normalize();
			return (float)Math.Atan2((double)vector.Y, (double)vector.X);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00013FD8 File Offset: 0x000121D8
		public static Vector2 RightStickDirection(int index)
		{
			Vector2 result = new Vector2(InputManager.mGamePadStates[index].ThumbSticks.Right.X, -InputManager.mGamePadStates[index].ThumbSticks.Right.Y);
			result.Normalize();
			return result;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0001402F File Offset: 0x0001222F
		public static void LockInput()
		{
			InputManager.mLockoutTimer.IndependentOfTime = true;
			InputManager.mLockoutTimer.Reset();
			InputManager.mLockoutTimer.Start();
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00014050 File Offset: 0x00012250
		public static Vector3 GetTargetPositionXbox(Player player)
		{
			float num = InputManager.RightStickAngle(player.Index);
			float x = (float)Math.Cos((double)num);
			float z = -(float)Math.Sin((double)num);
			return new Vector3(x, 0f, z);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00014086 File Offset: 0x00012286
		public static bool MouseWheelUp()
		{
			return InputManager.mPrevMouseState.ScrollWheelValue < InputManager.mMouseState.ScrollWheelValue;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x000140A1 File Offset: 0x000122A1
		public static bool MouseWheelDown()
		{
			return InputManager.mPrevMouseState.ScrollWheelValue > InputManager.mMouseState.ScrollWheelValue;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x000140BC File Offset: 0x000122BC
		public static string GetKeyString(Keys k)
		{
			if (k == Keys.Enter)
			{
				return "Enter";
			}
			if (k == Keys.Escape)
			{
				return "Esc";
			}
			switch (k)
			{
			case Keys.LeftShift:
			case Keys.RightShift:
				return "Shift";
			case Keys.LeftControl:
			case Keys.RightControl:
				return "Ctrl";
			case Keys.LeftAlt:
			case Keys.RightAlt:
				return "Alt";
			default:
				return k.ToString();
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00014124 File Offset: 0x00012324
		public static string GetKeyString(ButtonPress button, int index)
		{
			if (index == -1)
			{
				return InputManager.GetKeyString(InputManager.mPCControls[button][0]);
			}
			PlayerInfo player = PlayerManager.GetPlayer(index);
			if (player == null)
			{
				return "";
			}
			if (player.UsingController)
			{
				return InputManager.mXboxControls[button][0].ToString();
			}
			return InputManager.GetKeyString(InputManager.mPCControls[button][0]);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0001419A File Offset: 0x0001239A
		public static bool HasMouseMoved()
		{
			return InputManager.mPrevMouseState.X != InputManager.mMouseState.X || InputManager.mPrevMouseState.Y != InputManager.mMouseState.Y;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x000141CB File Offset: 0x000123CB
		public static bool ContollerConnected(int index)
		{
			return InputManager.mGamePadStates[index].IsConnected;
		}

		// Token: 0x04000270 RID: 624
		private static bool[] mInPCMode;

		// Token: 0x04000271 RID: 625
		private static KeyboardState mPrevKeyState;

		// Token: 0x04000272 RID: 626
		private static GamePadState[] mPrevGamePadStates;

		// Token: 0x04000273 RID: 627
		private static MouseState mPrevMouseState;

		// Token: 0x04000274 RID: 628
		private static KeyboardState mKeyState;

		// Token: 0x04000275 RID: 629
		private static GamePadState[] mGamePadStates;

		// Token: 0x04000276 RID: 630
		private static MouseState mMouseState;

		// Token: 0x04000277 RID: 631
		private static Timer mLockoutTimer = new Timer(1f);

		// Token: 0x04000278 RID: 632
		public static Dictionary<ButtonPress, List<Keys>> mPCControls;

		// Token: 0x04000279 RID: 633
		private static Dictionary<ButtonPress, List<Buttons>> mXboxControls;

		// Token: 0x0400027A RID: 634
		private static float TRIGGERBUFFER = 0.1f;

		// Token: 0x0400027B RID: 635
		private static float STICKBUFFER = 0.1f;

		// Token: 0x0400027C RID: 636
		public static bool LOCK_INPUT = false;
	}
}
