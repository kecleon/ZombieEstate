using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000A5 RID: 165
	public static class ScreenTransition
	{
		// Token: 0x0600043F RID: 1087 RVA: 0x0001F61E File Offset: 0x0001D81E
		public static void StartTransition(float time)
		{
			ScreenTransition.timer = new Timer(time);
			ScreenTransition.fadeTimer = new Timer(time);
			ScreenTransition.splatters = new List<Splatter>();
			ScreenTransition.timer.Start();
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001F64C File Offset: 0x0001D84C
		public static void Update()
		{
			if (ScreenTransition.timer.Running())
			{
				int num = Global.rand.Next(10, 30);
				for (int i = 0; i < num; i++)
				{
					float x = Global.RandomFloat(0f, 1280f) - 64f;
					float num2 = Global.RandomFloat(0f, ScreenTransition.messyness) - ScreenTransition.messyness / 2f + ScreenTransition.timer.Delta();
					num2 = num2 * 840f - 64f;
					Splatter item = default(Splatter);
					item.position = new Vector2(x, num2);
					item.scale = Global.rand.Next(ScreenTransition.minScale, ScreenTransition.maxScale);
					item.point = new Point(Global.rand.Next(0, 4) * 16, 496);
					item.alpha = 0f;
					ScreenTransition.splatters.Add(item);
				}
				for (int j = 0; j < ScreenTransition.splatters.Count; j++)
				{
					if (ScreenTransition.splatters[j].alpha < 140f)
					{
						Splatter value = ScreenTransition.splatters[j];
						value.alpha += ScreenTransition.fadeSpeed;
						ScreenTransition.splatters[j] = value;
					}
				}
				ScreenTransition.bgAlpha += ScreenTransition.fadeSpeed * 0.75f;
				if (ScreenTransition.bgAlpha > 255f)
				{
					ScreenTransition.bgAlpha = 255f;
				}
			}
			if (ScreenTransition.timer.Expired() && !ScreenTransition.fadeTimer.Running())
			{
				ScreenTransition.fadeTimer.Start();
			}
			if (ScreenTransition.fadeTimer.Running())
			{
				for (int k = 0; k < ScreenTransition.splatters.Count; k++)
				{
					if (ScreenTransition.splatters[k].alpha > 0f)
					{
						Splatter value2 = ScreenTransition.splatters[k];
						value2.alpha -= ScreenTransition.fadeSpeed;
						ScreenTransition.splatters[k] = value2;
					}
				}
				ScreenTransition.bgAlpha -= ScreenTransition.fadeSpeed;
				if (ScreenTransition.bgAlpha < 0f)
				{
					ScreenTransition.bgAlpha = 0f;
				}
			}
			if (ScreenTransition.fadeTimer.Expired() && ScreenTransition.splatters.Count != 0)
			{
				Terminal.WriteMessage(ScreenTransition.splatters.Count.ToString());
				ScreenTransition.splatters.Clear();
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001F8B4 File Offset: 0x0001DAB4
		public static void Draw(SpriteBatch spriteBatch)
		{
			Rectangle value = new Rectangle(0, 0, 16, 16);
			Rectangle destinationRectangle = new Rectangle(0, 0, 16, 16);
			Color white = Color.White;
			Color black = Color.Black;
			black.A = (byte)ScreenTransition.bgAlpha;
			spriteBatch.Draw(Global.Pixel, new Rectangle(0, 0, 1280, 720), black);
			if (ScreenTransition.timer.Running() || ScreenTransition.fadeTimer.Running())
			{
				for (int i = 0; i < ScreenTransition.splatters.Count; i++)
				{
					value.X = ScreenTransition.splatters[i].point.X;
					value.Y = ScreenTransition.splatters[i].point.Y;
					destinationRectangle.X = (int)ScreenTransition.splatters[i].position.X;
					destinationRectangle.Y = (int)ScreenTransition.splatters[i].position.Y;
					destinationRectangle.Width = ScreenTransition.splatters[i].scale;
					destinationRectangle.Height = ScreenTransition.splatters[i].scale;
					white.A = (byte)ScreenTransition.splatters[i].alpha;
					spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(value), white);
				}
			}
		}

		// Token: 0x0400041A RID: 1050
		private static Timer timer = new Timer(4f);

		// Token: 0x0400041B RID: 1051
		private static Timer fadeTimer = new Timer(4f);

		// Token: 0x0400041C RID: 1052
		private static List<Splatter> splatters = new List<Splatter>();

		// Token: 0x0400041D RID: 1053
		private static float messyness = 0.2f;

		// Token: 0x0400041E RID: 1054
		private static int maxScale = 256;

		// Token: 0x0400041F RID: 1055
		private static int minScale = 64;

		// Token: 0x04000420 RID: 1056
		private static float fadeSpeed = 4f;

		// Token: 0x04000421 RID: 1057
		private static float bgAlpha = 0f;
	}
}
