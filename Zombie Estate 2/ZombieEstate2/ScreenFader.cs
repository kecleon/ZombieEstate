using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200004D RID: 77
	public static class ScreenFader
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x0000D41C File Offset: 0x0000B61C
		public static void Update(float elapsed)
		{
			if (!ScreenFader.Active)
			{
				return;
			}
			if (ScreenFader.fadingOut)
			{
				ScreenFader.alpha -= ScreenFader.speed * (60f * elapsed);
				if (ScreenFader.alpha <= 0f)
				{
					ScreenFader.alpha = 0f;
					ScreenFader.Active = false;
					return;
				}
			}
			else
			{
				ScreenFader.alpha += ScreenFader.speed * (60f * elapsed);
				if (ScreenFader.alpha >= 1f)
				{
					ScreenFader.alpha = 1f;
					ScreenFader.fadingOut = true;
					if (ScreenFader.OnFadeDone != null)
					{
						ScreenFader.OnFadeDone();
					}
					ScreenFader.OnFadeDone = null;
				}
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000D4B8 File Offset: 0x0000B6B8
		public static void Draw(SpriteBatch spriteBatch)
		{
			if (!ScreenFader.Active)
			{
				return;
			}
			Color color = Color.Black * ScreenFader.alpha;
			spriteBatch.Draw(Global.Pixel, new Rectangle(0, 0, Global.ScreenRect.Width, Global.ScreenRect.Height), color);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000D504 File Offset: 0x0000B704
		public static void Fade(ScreenFader.FadeDone fadeDone, float spd)
		{
			ScreenFader.alpha = 0f;
			ScreenFader.Active = true;
			ScreenFader.fadingOut = false;
			ScreenFader.OnFadeDone = fadeDone;
			ScreenFader.speed = spd;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000D528 File Offset: 0x0000B728
		public static void Fade(ScreenFader.FadeDone fadeDone)
		{
			ScreenFader.Fade(fadeDone, 0.1f);
		}

		// Token: 0x04000146 RID: 326
		private static bool fadingOut;

		// Token: 0x04000147 RID: 327
		private static float alpha;

		// Token: 0x04000148 RID: 328
		public static bool Active;

		// Token: 0x04000149 RID: 329
		private static float speed = 0.2f;

		// Token: 0x0400014A RID: 330
		private static ScreenFader.FadeDone OnFadeDone;

		// Token: 0x0200020A RID: 522
		// (Invoke) Token: 0x06000DB9 RID: 3513
		public delegate void FadeDone();
	}
}
