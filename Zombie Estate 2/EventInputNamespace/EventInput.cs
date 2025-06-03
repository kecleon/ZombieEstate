using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EventInputNamespace
{
	// Token: 0x0200000B RID: 11
	public static class EventInput
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000020 RID: 32 RVA: 0x00002750 File Offset: 0x00000950
		// (remove) Token: 0x06000021 RID: 33 RVA: 0x00002784 File Offset: 0x00000984
		public static event CharEnteredHandler CharEntered;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000022 RID: 34 RVA: 0x000027B8 File Offset: 0x000009B8
		// (remove) Token: 0x06000023 RID: 35 RVA: 0x000027EC File Offset: 0x000009EC
		public static event KeyEventHandler KeyDown;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000024 RID: 36 RVA: 0x00002820 File Offset: 0x00000A20
		// (remove) Token: 0x06000025 RID: 37 RVA: 0x00002854 File Offset: 0x00000A54
		public static event KeyEventHandler KeyUp;

		// Token: 0x06000026 RID: 38
		[DllImport("Imm32.dll")]
		private static extern IntPtr ImmGetContext(IntPtr hWnd);

		// Token: 0x06000027 RID: 39
		[DllImport("Imm32.dll")]
		private static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

		// Token: 0x06000028 RID: 40
		[DllImport("user32.dll")]
		private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000029 RID: 41
		[DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		// Token: 0x0600002A RID: 42 RVA: 0x00002888 File Offset: 0x00000A88
		public static void Initialize(GameWindow window)
		{
			if (EventInput.initialized)
			{
				throw new InvalidOperationException("TextInput.Initialize can only be called once!");
			}
			EventInput.hookProcDelegate = new EventInput.WndProc(EventInput.HookProc);
			EventInput.prevWndProc = (IntPtr)EventInput.SetWindowLong(window.Handle, -4, (int)Marshal.GetFunctionPointerForDelegate(EventInput.hookProcDelegate));
			EventInput.hIMC = EventInput.ImmGetContext(window.Handle);
			EventInput.initialized = true;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000028F4 File Offset: 0x00000AF4
		private static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
		{
			IntPtr result = EventInput.CallWindowProc(EventInput.prevWndProc, hWnd, msg, wParam, lParam);
			if (msg <= 135U)
			{
				if (msg != 81U)
				{
					if (msg == 135U)
					{
						result = (IntPtr)(result.ToInt32() | 4);
					}
				}
				else
				{
					EventInput.ImmAssociateContext(hWnd, EventInput.hIMC);
					result = (IntPtr)1;
				}
			}
			else
			{
				switch (msg)
				{
				case 256U:
					if (EventInput.KeyDown != null)
					{
						EventInput.KeyDown(null, new KeyEventArgs((Keys)((int)wParam)));
					}
					break;
				case 257U:
					if (EventInput.KeyUp != null)
					{
						EventInput.KeyUp(null, new KeyEventArgs((Keys)((int)wParam)));
					}
					break;
				case 258U:
					if (EventInput.CharEntered != null)
					{
						EventInput.CharEntered(null, new CharacterEventArgs((char)((int)wParam), lParam.ToInt32()));
					}
					break;
				default:
					if (msg == 641U)
					{
						if (wParam.ToInt32() == 1)
						{
							EventInput.ImmAssociateContext(hWnd, EventInput.hIMC);
						}
					}
					break;
				}
			}
			return result;
		}

		// Token: 0x0400000F RID: 15
		private static bool initialized;

		// Token: 0x04000010 RID: 16
		private static IntPtr prevWndProc;

		// Token: 0x04000011 RID: 17
		private static EventInput.WndProc hookProcDelegate;

		// Token: 0x04000012 RID: 18
		private static IntPtr hIMC;

		// Token: 0x04000013 RID: 19
		private const int GWL_WNDPROC = -4;

		// Token: 0x04000014 RID: 20
		private const int WM_KEYDOWN = 256;

		// Token: 0x04000015 RID: 21
		private const int WM_KEYUP = 257;

		// Token: 0x04000016 RID: 22
		private const int WM_CHAR = 258;

		// Token: 0x04000017 RID: 23
		private const int WM_IME_SETCONTEXT = 641;

		// Token: 0x04000018 RID: 24
		private const int WM_INPUTLANGCHANGE = 81;

		// Token: 0x04000019 RID: 25
		private const int WM_GETDLGCODE = 135;

		// Token: 0x0400001A RID: 26
		private const int WM_IME_COMPOSITION = 271;

		// Token: 0x0400001B RID: 27
		private const int DLGC_WANTALLKEYS = 4;

		// Token: 0x02000200 RID: 512
		// (Invoke) Token: 0x06000DA9 RID: 3497
		private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
	}
}
