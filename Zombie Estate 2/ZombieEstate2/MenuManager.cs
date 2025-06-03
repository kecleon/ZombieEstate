using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000E7 RID: 231
	public static class MenuManager
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0002D8DB File Offset: 0x0002BADB
		public static bool MenuActive
		{
			get
			{
				return MenuManager.Menus.Count != 0;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0002D8EA File Offset: 0x0002BAEA
		public static void Update()
		{
			if (MenuManager.Menus.Count == 0)
			{
				return;
			}
			if (MenuManager.BGTimer.Ready() || MenuManager.BGTimer.Delta() >= 0.45f)
			{
				MenuManager.Current.UpdateMenu();
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0002D920 File Offset: 0x0002BB20
		public static void Draw(SpriteBatch spriteBatch)
		{
			if (MenuManager.Menus.Count == 0)
			{
				return;
			}
			try
			{
				MenuManager.Current.Draw3DMenu();
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null);
				if (MenuManager.Current.DrawBGPixel)
				{
					spriteBatch.Draw(Global.Pixel, new Rectangle(0, 0, Global.ScreenRect.Width, Global.ScreenRect.Height), Color.Black * MenuManager.BGPulse);
				}
				MenuManager.Current.DrawMenu(spriteBatch);
				spriteBatch.End();
				spriteBatch.Begin();
			}
			catch
			{
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0002D9C8 File Offset: 0x0002BBC8
		public static void PushMenu(Menu menu)
		{
			if (PlayerManager.AllLocalPlayers())
			{
				Global.Paused = true;
			}
			MenuManager.Menus.Push(menu);
			if (MenuManager.Menus.Count == 1)
			{
				MenuManager.BGPulse = 0f;
				MenuManager.BGTimer.Reset();
				MenuManager.BGTimer.Start();
				MenuManager.BGTimer.IndependentOfTime = true;
				MenuManager.BGTimer.DeltaDelegate = new Timer.TimerDelegate(MenuManager.BGDelta);
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0002DA3C File Offset: 0x0002BC3C
		public static void MenuClosed()
		{
			if (MenuManager.Menus.Count == 0)
			{
				return;
			}
			MenuManager.Menus.Pop();
			if (MenuManager.Menus.Count == 0)
			{
				Global.Paused = false;
				foreach (Player player in Global.PlayerList)
				{
					player.LockInput.Reset();
					player.LockInput.Start();
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0002DAC8 File Offset: 0x0002BCC8
		public static Menu Current
		{
			get
			{
				return MenuManager.Menus.Peek();
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0002DAD4 File Offset: 0x0002BCD4
		private static void BGDelta(float d)
		{
			MenuManager.BGPulse = 0.74f * d;
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0002DAE2 File Offset: 0x0002BCE2
		public static int ShadowDistance
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0002DAE5 File Offset: 0x0002BCE5
		public static void CLOSEALL()
		{
			MenuManager.Menus.Clear();
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0002D8DB File Offset: 0x0002BADB
		public static bool MenuOpen()
		{
			return MenuManager.Menus.Count != 0;
		}

		// Token: 0x040005F1 RID: 1521
		private static Stack<Menu> Menus = new Stack<Menu>();

		// Token: 0x040005F2 RID: 1522
		private static float BGPulse = 0f;

		// Token: 0x040005F3 RID: 1523
		private static Timer BGTimer = new Timer(0.6f);
	}
}
