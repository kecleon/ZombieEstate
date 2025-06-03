using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000047 RID: 71
	public class Indicator2D
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x0000C1E8 File Offset: 0x0000A3E8
		public static void DrawInd(SpriteBatch spriteBatch)
		{
			foreach (Indicator2D indicator2D in Indicator2D.Indicators)
			{
				indicator2D.Draw(spriteBatch);
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000C238 File Offset: 0x0000A438
		public Indicator2D(GameObject parent)
		{
			this.Parent = parent;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000C248 File Offset: 0x0000A448
		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 screenPosition = VerchickMath.GetScreenPosition(this.Parent.Position);
			if (Global.ScreenRect.Contains((int)screenPosition.X, (int)screenPosition.Y))
			{
				return;
			}
			screenPosition.X = MathHelper.Clamp(screenPosition.X, 0f, (float)(Global.ScreenRect.Width - Indicator2D.SIZE));
			screenPosition.Y = MathHelper.Clamp(screenPosition.Y, 0f, (float)(Global.ScreenRect.Height - Indicator2D.SIZE));
			Rectangle texRectange = Global.GetTexRectange(this.Parent.TextureCoord.X, this.Parent.TextureCoord.Y);
			Rectangle destinationRectangle = new Rectangle((int)screenPosition.X, (int)screenPosition.Y, Indicator2D.SIZE, Indicator2D.SIZE);
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(texRectange), Color.White);
		}

		// Token: 0x0400011A RID: 282
		private static int SIZE = 64;

		// Token: 0x0400011B RID: 283
		public static List<Indicator2D> Indicators = new List<Indicator2D>();

		// Token: 0x0400011C RID: 284
		public GameObject Parent;
	}
}
