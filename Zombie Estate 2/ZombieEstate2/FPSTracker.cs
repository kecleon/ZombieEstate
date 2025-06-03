using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000110 RID: 272
	public static class FPSTracker
	{
		// Token: 0x0600075E RID: 1886 RVA: 0x0003A4FC File Offset: 0x000386FC
		public static void Initialize(Game game)
		{
			FPSTracker.pixel = game.Content.Load<Texture2D>("Pixel");
			FPSTracker.Ticker = false;
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00002EF9 File Offset: 0x000010F9
		public static void Update(GameTime gameTime)
		{
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0003A51C File Offset: 0x0003871C
		public static void Draw(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x04000763 RID: 1891
		private static Texture2D bg;

		// Token: 0x04000764 RID: 1892
		private static Texture2D pixel;

		// Token: 0x04000765 RID: 1893
		private static float[] FPSValues;

		// Token: 0x04000766 RID: 1894
		private static int index;

		// Token: 0x04000767 RID: 1895
		public static bool Ticker = true;

		// Token: 0x04000768 RID: 1896
		private static int Length = 240;

		// Token: 0x04000769 RID: 1897
		private static TimeSpan elapsedTime = TimeSpan.Zero;

		// Token: 0x0400076A RID: 1898
		private static float frameRate = 0f;

		// Token: 0x0400076B RID: 1899
		private static float prevFrameRate = 0f;

		// Token: 0x0400076C RID: 1900
		private static float frameCounter = 0f;

		// Token: 0x0400076D RID: 1901
		private static string frameString = "FPS: 0";

		// Token: 0x0400076E RID: 1902
		private static int objCount;

		// Token: 0x0400076F RID: 1903
		private static int cooldown = 6000;

		// Token: 0x04000770 RID: 1904
		private static long prevMem;
	}
}
