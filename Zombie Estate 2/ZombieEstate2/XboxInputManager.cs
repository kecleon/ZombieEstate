using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x02000140 RID: 320
	public static class XboxInputManager
	{
		// Token: 0x06000980 RID: 2432 RVA: 0x0004D25F File Offset: 0x0004B45F
		public static void Init()
		{
			XboxInputManager.mPrevGamePadStates = new GamePadState[4];
			XboxInputManager.mGamePadStates = new GamePadState[4];
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00002EF9 File Offset: 0x000010F9
		public static void Update()
		{
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0004D278 File Offset: 0x0004B478
		public static void EndUpdate()
		{
			XboxInputManager.mPrevGamePadStates[0] = XboxInputManager.mGamePadStates[0];
			XboxInputManager.mPrevGamePadStates[1] = XboxInputManager.mGamePadStates[1];
			XboxInputManager.mPrevGamePadStates[2] = XboxInputManager.mGamePadStates[2];
			XboxInputManager.mPrevGamePadStates[3] = XboxInputManager.mGamePadStates[3];
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0004D2E0 File Offset: 0x0004B4E0
		public static bool RightStickDownHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Right.Y < -XboxInputManager.BUFFER;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0004D318 File Offset: 0x0004B518
		public static bool RightStickUpHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Right.Y > XboxInputManager.BUFFER;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0004D34C File Offset: 0x0004B54C
		public static bool RightStickLeftHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Right.X < -XboxInputManager.BUFFER;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0004D384 File Offset: 0x0004B584
		public static bool RightStickRightHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Right.X > XboxInputManager.BUFFER;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0004D3B8 File Offset: 0x0004B5B8
		public static bool RightStickDownPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Right.Y < -XboxInputManager.BUFFER && XboxInputManager.mPrevGamePadStates[index].ThumbSticks.Right.Y >= -XboxInputManager.BUFFER;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0004D414 File Offset: 0x0004B614
		public static bool RightStickUpPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Right.Y > XboxInputManager.BUFFER && XboxInputManager.mPrevGamePadStates[index].ThumbSticks.Right.Y <= XboxInputManager.BUFFER;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0004D46C File Offset: 0x0004B66C
		public static bool RightStickLeftPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Right.X < -XboxInputManager.BUFFER && XboxInputManager.mPrevGamePadStates[index].ThumbSticks.Right.X >= -XboxInputManager.BUFFER;
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0004D4C8 File Offset: 0x0004B6C8
		public static bool RightStickRightPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Right.X > XboxInputManager.BUFFER && XboxInputManager.mPrevGamePadStates[index].ThumbSticks.Right.X <= XboxInputManager.BUFFER;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0004D520 File Offset: 0x0004B720
		public static bool LeftStickDownHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Left.Y < -XboxInputManager.BUFFER;
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0004D558 File Offset: 0x0004B758
		public static bool LeftStickUpHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Left.Y > XboxInputManager.BUFFER;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0004D58C File Offset: 0x0004B78C
		public static bool LeftStickLeftHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Left.X < -XboxInputManager.BUFFER;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0004D5C4 File Offset: 0x0004B7C4
		public static bool LeftStickRightHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Left.X > XboxInputManager.BUFFER;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0004D5F8 File Offset: 0x0004B7F8
		public static bool LeftStickDownPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Left.Y < -XboxInputManager.BUFFER && XboxInputManager.mPrevGamePadStates[index].ThumbSticks.Left.Y >= -XboxInputManager.BUFFER;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0004D654 File Offset: 0x0004B854
		public static bool LeftStickUpPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Left.Y > XboxInputManager.BUFFER && XboxInputManager.mPrevGamePadStates[index].ThumbSticks.Left.Y <= XboxInputManager.BUFFER;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0004D6AC File Offset: 0x0004B8AC
		public static bool LeftStickLeftPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Left.X < -XboxInputManager.BUFFER && XboxInputManager.mPrevGamePadStates[index].ThumbSticks.Left.X >= -XboxInputManager.BUFFER;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0004D708 File Offset: 0x0004B908
		public static bool LeftStickRightPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].ThumbSticks.Left.X > XboxInputManager.BUFFER && XboxInputManager.mPrevGamePadStates[index].ThumbSticks.Left.X <= XboxInputManager.BUFFER;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0004D760 File Offset: 0x0004B960
		public static bool RightTriggerHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].Triggers.Right > XboxInputManager.TRIGGERBUFFER;
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0004D790 File Offset: 0x0004B990
		public static bool RightTriggerPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].Triggers.Right > XboxInputManager.TRIGGERBUFFER && XboxInputManager.mPrevGamePadStates[index].Triggers.Right < XboxInputManager.TRIGGERBUFFER;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0004D7E0 File Offset: 0x0004B9E0
		public static bool LeftTriggerHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].Triggers.Left > XboxInputManager.TRIGGERBUFFER;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0004D810 File Offset: 0x0004BA10
		public static bool LeftTriggerPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].Triggers.Left > XboxInputManager.TRIGGERBUFFER && XboxInputManager.mPrevGamePadStates[index].Triggers.Left < XboxInputManager.TRIGGERBUFFER;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0004D860 File Offset: 0x0004BA60
		public static bool StartPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.Start == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].Buttons.Start == ButtonState.Released;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0004D8A8 File Offset: 0x0004BAA8
		public static bool APressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.A == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].Buttons.A == ButtonState.Released;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0004D8F0 File Offset: 0x0004BAF0
		public static bool AHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.A == ButtonState.Pressed;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0004D91C File Offset: 0x0004BB1C
		public static bool XPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.X == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].Buttons.X == ButtonState.Released;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0004D964 File Offset: 0x0004BB64
		public static bool XHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.X == ButtonState.Pressed;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0004D990 File Offset: 0x0004BB90
		public static bool BPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.B == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].Buttons.B == ButtonState.Released;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0004D9D8 File Offset: 0x0004BBD8
		public static bool BHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.B == ButtonState.Pressed;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0004DA04 File Offset: 0x0004BC04
		public static bool YPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.Y == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].Buttons.Y == ButtonState.Released;
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0004DA4C File Offset: 0x0004BC4C
		public static bool YHeld(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.Y == ButtonState.Pressed;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0004DA78 File Offset: 0x0004BC78
		public static bool RightShoulderPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.RightShoulder == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].Buttons.RightShoulder == ButtonState.Released;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0004DAC0 File Offset: 0x0004BCC0
		public static bool LeftShoulderPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.LeftShoulder == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].Buttons.LeftShoulder == ButtonState.Released;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0004DB08 File Offset: 0x0004BD08
		public static float RightStickAngle(int index)
		{
			Vector2 vector = new Vector2(XboxInputManager.mGamePadStates[index].ThumbSticks.Right.X, -XboxInputManager.mGamePadStates[index].ThumbSticks.Right.Y);
			vector.Normalize();
			return (float)Math.Atan2((double)vector.Y, (double)vector.X);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0004DB74 File Offset: 0x0004BD74
		public static Vector2 RightStickDirection(int index)
		{
			Vector2 result = new Vector2(XboxInputManager.mGamePadStates[index].ThumbSticks.Right.X, -XboxInputManager.mGamePadStates[index].ThumbSticks.Right.Y);
			result.Normalize();
			return result;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0004DBCC File Offset: 0x0004BDCC
		public static bool BackPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].Buttons.Back == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].Buttons.Back == ButtonState.Released;
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0004DC14 File Offset: 0x0004BE14
		public static bool DPadUpPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].DPad.Up == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].DPad.Up == ButtonState.Released;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0004DC5C File Offset: 0x0004BE5C
		public static bool DPadLeftPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].DPad.Left == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].DPad.Left == ButtonState.Released;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0004DCA4 File Offset: 0x0004BEA4
		public static bool DPadRightPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].DPad.Right == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].DPad.Right == ButtonState.Released;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0004DCEC File Offset: 0x0004BEEC
		public static bool DPadDownPressed(int index)
		{
			return XboxInputManager.mGamePadStates[index].DPad.Down == ButtonState.Pressed && XboxInputManager.mPrevGamePadStates[index].DPad.Down == ButtonState.Released;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0004DD31 File Offset: 0x0004BF31
		public static bool MenuDownPressed(int index)
		{
			return XboxInputManager.DPadDownPressed(index) || XboxInputManager.LeftStickDownPressed(index);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0004DD43 File Offset: 0x0004BF43
		public static bool MenuUpPressed(int index)
		{
			return XboxInputManager.DPadUpPressed(index) || XboxInputManager.LeftStickUpPressed(index);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0004DD58 File Offset: 0x0004BF58
		public static bool AnyControllersUnplugged(List<Player> players)
		{
			foreach (Player player in players)
			{
				if (!XboxInputManager.mGamePadStates[player.Index].IsConnected)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000A1F RID: 2591
		private static GamePadState[] mPrevGamePadStates;

		// Token: 0x04000A20 RID: 2592
		private static GamePadState[] mGamePadStates;

		// Token: 0x04000A21 RID: 2593
		private static float BUFFER = 0.1f;

		// Token: 0x04000A22 RID: 2594
		private static float TRIGGERBUFFER = 0.1f;
	}
}
