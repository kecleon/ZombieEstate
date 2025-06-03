using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200001E RID: 30
	internal class StatLine
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x00005DA4 File Offset: 0x00003FA4
		public StatLine(string label, string data, Vector2 pos, SpriteBatch spriteBatch)
		{
			SpriteFont storeFont = Global.StoreFont;
			int num = (int)storeFont.MeasureString(label).X;
			Shadow.DrawString(label, storeFont, pos, 1, Color.LightGray, spriteBatch);
			pos.X += (float)num;
			Shadow.DrawString(data, storeFont, pos, 1, Color.White, spriteBatch);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005DF8 File Offset: 0x00003FF8
		public StatLine(string label, string data, Vector2 pos, SpriteBatch spriteBatch, Color DataColor, int lineWidth, int spacing)
		{
			SpriteFont storeFont = Global.StoreFont;
			int num = (int)storeFont.MeasureString(label).X;
			Shadow.DrawString(label, storeFont, pos, 1, Color.LightGray, spriteBatch);
			pos.X += (float)num;
			Shadow.DrawString(data, storeFont, pos, 1, DataColor, spriteBatch);
			Rectangle destinationRectangle = new Rectangle((int)pos.X - num, (int)pos.Y + spacing, lineWidth, 1);
			Rectangle destinationRectangle2 = new Rectangle((int)pos.X - num, (int)pos.Y + spacing + 1, lineWidth, 1);
			spriteBatch.Draw(Global.Pixel, destinationRectangle, Color.White);
			spriteBatch.Draw(Global.Pixel, destinationRectangle2, Color.Gray);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005EAC File Offset: 0x000040AC
		public StatLine(string label, string data, Vector2 pos, SpriteBatch spriteBatch, Color DataColor, int lineWidth, int spacing, bool twoLine)
		{
			SpriteFont storeFont = Global.StoreFont;
			int num = (int)storeFont.MeasureString(label).X;
			Shadow.DrawString(label, storeFont, pos, 1, Color.LightGray, spriteBatch);
			if (twoLine)
			{
				pos.X += 16f;
				pos.Y += (float)spacing;
				Shadow.DrawString(data, storeFont, pos, 1, DataColor, spriteBatch);
				pos.X += (float)(num - 16);
			}
			else
			{
				pos.X += (float)num;
				Shadow.DrawString(data, storeFont, pos, 1, DataColor, spriteBatch);
			}
			Rectangle destinationRectangle = new Rectangle((int)pos.X - num, (int)pos.Y + spacing, lineWidth, 1);
			Rectangle destinationRectangle2 = new Rectangle((int)pos.X - num, (int)pos.Y + spacing + 1, lineWidth, 1);
			spriteBatch.Draw(Global.Pixel, destinationRectangle, Color.White);
			spriteBatch.Draw(Global.Pixel, destinationRectangle2, Color.Gray);
		}
	}
}
