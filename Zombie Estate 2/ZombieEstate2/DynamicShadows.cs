using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000046 RID: 70
	public static class DynamicShadows
	{
		// Token: 0x060001AD RID: 429 RVA: 0x0000C06E File Offset: 0x0000A26E
		public static void Init()
		{
			DynamicShadows.mRT = new RenderTarget2D(Global.GraphicsDevice, 16 * Sector.width, 16 * Sector.height);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000C090 File Offset: 0x0000A290
		public static void DrawShadows(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin();
			Global.GraphicsDevice.SetRenderTarget(DynamicShadows.mRT);
			Global.GraphicsDevice.Clear(Color.Black);
			foreach (Shootable shootable in DynamicShadows.Shootables)
			{
				if (shootable.Position.Y >= 0f)
				{
					Vector2 vector = new Vector2((shootable.Position.X - 0.5f) * 16f, (shootable.Position.Z - 0.5f) * 16f);
					spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)vector.X, (int)vector.Y, 16, 16), new Rectangle?(DynamicShadows.mRect), Color.White);
				}
			}
			spriteBatch.End();
			Global.GraphicsDevice.SetRenderTarget(null);
			Global.GraphicsDevice.BlendState = BlendState.AlphaBlend;
			Global.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			spriteBatch.Begin();
			Global.GraphicsDevice.Clear(Color.Black);
		}

		// Token: 0x04000117 RID: 279
		public static List<Shootable> Shootables = new List<Shootable>(900);

		// Token: 0x04000118 RID: 280
		public static RenderTarget2D mRT;

		// Token: 0x04000119 RID: 281
		private static Rectangle mRect = Global.GetTexRectange(12, 21);
	}
}
