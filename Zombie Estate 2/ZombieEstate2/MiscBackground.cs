using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000BB RID: 187
	internal class MiscBackground
	{
		// Token: 0x060004AA RID: 1194 RVA: 0x000227A3 File Offset: 0x000209A3
		public MiscBackground(Rectangle rect, Color col)
		{
			this.rectangle = rect;
			this.color = col;
			if (MiscBackground.Texture == null)
			{
				MiscBackground.Texture = Global.Content.Load<Texture2D>("ButtonBG");
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000227D4 File Offset: 0x000209D4
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(MiscBackground.Texture, this.rectangle, this.color);
		}

		// Token: 0x040004B6 RID: 1206
		public static Texture2D Texture;

		// Token: 0x040004B7 RID: 1207
		public Rectangle rectangle;

		// Token: 0x040004B8 RID: 1208
		public Color color;
	}
}
