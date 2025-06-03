using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200012A RID: 298
	public static class Shadow
	{
		// Token: 0x06000868 RID: 2152 RVA: 0x00046084 File Offset: 0x00044284
		public static void DrawShadowed(Texture2D tex, Vector2 pos, int distance, Color color, Color shadowColor, SpriteBatch spriteBatch)
		{
			Vector2 position = new Vector2(pos.X + (float)distance, pos.Y + (float)distance);
			spriteBatch.Draw(tex, position, shadowColor);
			spriteBatch.Draw(tex, pos, color);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x000460BF File Offset: 0x000442BF
		public static void DrawShadowed(Texture2D tex, Vector2 pos, int distance, Color color, SpriteBatch spriteBatch)
		{
			Shadow.DrawShadowed(tex, pos, distance, color, Color.Black, spriteBatch);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x000460D4 File Offset: 0x000442D4
		public static void DrawString(string text, SpriteFont font, Vector2 pos, int distance, Color color, Color shadowColor, SpriteBatch spriteBatch)
		{
			Vector2 position = new Vector2(pos.X + (float)distance, pos.Y + (float)distance);
			spriteBatch.DrawString(font, text, position, shadowColor);
			spriteBatch.DrawString(font, text, pos, color);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00046112 File Offset: 0x00044312
		public static void DrawString(string text, SpriteFont font, Vector2 pos, int distance, Color color, SpriteBatch spriteBatch)
		{
			Shadow.DrawString(text, font, pos, distance, color, Color.Black, spriteBatch);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00046128 File Offset: 0x00044328
		public static void DrawOutlinedString(SpriteBatch sb, SpriteFont font, string text, Color backColor, Color frontColor, float scale, float rotation, Vector2 position)
		{
			float num = 2f;
			Vector2 zero = Vector2.Zero;
			sb.DrawString(font, text, position + new Vector2(num * scale, num * scale), backColor, rotation, zero, scale, SpriteEffects.None, 1f);
			sb.DrawString(font, text, position + new Vector2(-num * scale, -num * scale), backColor, rotation, zero, scale, SpriteEffects.None, 1f);
			sb.DrawString(font, text, position + new Vector2(-num * scale, num * scale), backColor, rotation, zero, scale, SpriteEffects.None, 1f);
			sb.DrawString(font, text, position + new Vector2(num * scale, -num * scale), backColor, rotation, zero, scale, SpriteEffects.None, 1f);
			sb.DrawString(font, text, position + new Vector2(0f, -num * scale), backColor, rotation, zero, scale, SpriteEffects.None, 1f);
			sb.DrawString(font, text, position + new Vector2(num * scale, 0f), backColor, rotation, zero, scale, SpriteEffects.None, 1f);
			sb.DrawString(font, text, position + new Vector2(-num * scale, 0f), backColor, rotation, zero, scale, SpriteEffects.None, 1f);
			sb.DrawString(font, text, position + new Vector2(0f, num * scale), backColor, rotation, zero, scale, SpriteEffects.None, 1f);
			sb.DrawString(font, text, position, frontColor, rotation, zero, scale, SpriteEffects.None, 1f);
		}
	}
}
