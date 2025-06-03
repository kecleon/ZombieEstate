using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000D2 RID: 210
	internal static class ToolTip
	{
		// Token: 0x0600056C RID: 1388 RVA: 0x000298A8 File Offset: 0x00027AA8
		public static void Init(int xOff, int yOff)
		{
			ToolTip.Box = new ScrollBox("", new Rectangle(18 + xOff, 616 + yOff, 244, 94), Global.StoreFont, null, Color.White);
			ToolTip.rect = new Rectangle(16 + xOff, 616 + yOff, 224, 94);
			ToolTip.border = new Rectangle(14 + xOff, 614 + yOff, 228, 98);
			ToolTip.timer = new Timer(0.25f);
			ToolTip.timer.IndependentOfTime = true;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00029938 File Offset: 0x00027B38
		public static void Update()
		{
			if (ToolTip.timer.Expired() && ToolTip.hasText)
			{
				ToolTip.hasText = false;
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00029954 File Offset: 0x00027B54
		public static void Draw(SpriteBatch spriteBatch)
		{
			if (MiscBackground.Texture != null)
			{
				spriteBatch.Draw(Global.Pixel, ToolTip.border, Color.White);
				spriteBatch.Draw(MiscBackground.Texture, ToolTip.rect, ToolTip.bg);
			}
			if (ToolTip.hasText)
			{
				ToolTip.Box.Draw(spriteBatch);
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000299A4 File Offset: 0x00027BA4
		public static void SetText(string text)
		{
			ToolTip.Box.SetText(text);
			ToolTip.timer.Reset();
			ToolTip.timer.Start();
			ToolTip.hasText = true;
		}

		// Token: 0x0400056A RID: 1386
		private static ScrollBox Box;

		// Token: 0x0400056B RID: 1387
		private static Rectangle rect;

		// Token: 0x0400056C RID: 1388
		private static bool hasText;

		// Token: 0x0400056D RID: 1389
		private static Timer timer;

		// Token: 0x0400056E RID: 1390
		private static Rectangle border;

		// Token: 0x0400056F RID: 1391
		private static Color bg = new Color(0.2f, 0.2f, 0.2f);
	}
}
