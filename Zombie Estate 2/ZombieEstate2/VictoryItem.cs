using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200009E RID: 158
	public class VictoryItem
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x0001DE1C File Offset: 0x0001C01C
		public VictoryItem(string s, Point tex, Vector2 pos, float time, SpriteFont font, bool center = false)
		{
			this.mText = s;
			this.mTex = Global.GetTexRectange(tex.X, tex.Y);
			this.mTime = time / 4f;
			this.mFont = font;
			this.mPos = pos;
			this.FLASH = 0f;
			if (center)
			{
				this.mPos.X = this.mPos.X - (font.MeasureString(s).X / 2f + 18f);
			}
			this.mDest = new Rectangle((int)this.mPos.X - 36, (int)this.mPos.Y, 32, 32);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001DED8 File Offset: 0x0001C0D8
		public void Update()
		{
			if (this.mTime > 0f)
			{
				this.mTime -= 0.016666668f;
				return;
			}
			this.FLASH += Global.REAL_GAME_TIME;
			if (this.FLASH > this.TOTAL_FLASH)
			{
				this.FLASH = this.TOTAL_FLASH;
			}
			this.mDraw = true;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001DF38 File Offset: 0x0001C138
		public void Draw(SpriteBatch spriteBatch)
		{
			float scale = MathHelper.Lerp(0f, 1f, this.FLASH / this.TOTAL_FLASH);
			int num = -(int)(this.FLASH / this.TOTAL_FLASH * 32f);
			Rectangle destinationRectangle = new Rectangle(this.mDest.X - num, this.mDest.Y, this.mDest.Width, this.mDest.Height);
			Vector2 pos = new Vector2(this.mPos.X - (float)num, this.mPos.Y);
			if (this.mDraw)
			{
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(this.mTex), Color.White * scale);
				Shadow.DrawString(this.mText, this.mFont, pos, 1, Color.White * scale, Color.Black * scale, spriteBatch);
			}
		}

		// Token: 0x040003EF RID: 1007
		private string mText;

		// Token: 0x040003F0 RID: 1008
		private Vector2 mPos;

		// Token: 0x040003F1 RID: 1009
		private float mTime;

		// Token: 0x040003F2 RID: 1010
		private Rectangle mTex;

		// Token: 0x040003F3 RID: 1011
		private Rectangle mDest;

		// Token: 0x040003F4 RID: 1012
		private bool mDraw;

		// Token: 0x040003F5 RID: 1013
		private SpriteFont mFont;

		// Token: 0x040003F6 RID: 1014
		private float TOTAL_FLASH = 0.5f;

		// Token: 0x040003F7 RID: 1015
		private float FLASH;
	}
}
