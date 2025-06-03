using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x02000076 RID: 118
	public static class PathViewer
	{
		// Token: 0x060002D6 RID: 726 RVA: 0x00016CB4 File Offset: 0x00014EB4
		public static void Update()
		{
			if (InputManager.APressed(0) && InputManager.ButtonHeld(Keys.LeftShift, 0))
			{
				if (PathViewer.enabled)
				{
					PathViewer.Disable();
				}
				else
				{
					PathViewer.Enable();
				}
			}
			if (PathViewer.enabled)
			{
				Tile tileAtLocation = Global.Level.GetTileAtLocation(Global.Player.Position);
				PathViewer.targetX = tileAtLocation.x;
				PathViewer.targetY = tileAtLocation.y;
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00016D1C File Offset: 0x00014F1C
		private static void Enable()
		{
			PathViewer.enabled = true;
			PathViewer.arrows = new List<PathArrow>();
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					PathViewer.arrows.Add(new PathArrow(new Vector3((float)i + 0.5f, 0f, (float)j + 0.5f)));
				}
			}
			foreach (PathArrow obj in PathViewer.arrows)
			{
				Global.MasterCache.CreateObject(obj);
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00016DC8 File Offset: 0x00014FC8
		private static void Disable()
		{
			PathViewer.enabled = false;
			if (PathViewer.arrows == null)
			{
				return;
			}
			foreach (PathArrow pathArrow in PathViewer.arrows)
			{
				pathArrow.DestroyObject();
			}
			PathViewer.arrows = null;
		}

		// Token: 0x040002B5 RID: 693
		private static Sector sector;

		// Token: 0x040002B6 RID: 694
		public static int targetX = 16;

		// Token: 0x040002B7 RID: 695
		public static int targetY = 16;

		// Token: 0x040002B8 RID: 696
		private static List<PathArrow> arrows = new List<PathArrow>();

		// Token: 0x040002B9 RID: 697
		private static bool enabled;
	}
}
