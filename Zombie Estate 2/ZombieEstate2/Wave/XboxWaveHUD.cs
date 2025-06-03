using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.Wave
{
	// Token: 0x02000152 RID: 338
	public class XboxWaveHUD
	{
		// Token: 0x06000A39 RID: 2617 RVA: 0x00053B1C File Offset: 0x00051D1C
		public XboxWaveHUD()
		{
			int num = (int)Global.GetScreenCenter().X - XboxWaveHUD.mBG.Width / 2;
			int num2 = Global.GetSafeScreenArea().Bottom - XboxWaveHUD.mBG.Height;
			this.mPosition = new Vector2((float)num, (float)num2);
			this.mFullBar = new Rectangle(num + 4, num2 + 48, 234, 32);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00053B98 File Offset: 0x00051D98
		public void Draw(SpriteBatch spriteBatch, int current, int total, int waveNum)
		{
			float num = (float)current / (float)total;
			Rectangle destinationRectangle = new Rectangle(this.mFullBar.X, this.mFullBar.Y, (int)((float)this.mFullBar.Width * num), this.mFullBar.Height);
			Rectangle value = new Rectangle(0, 0, (int)((float)this.mFullBar.Width * num), this.mFullBar.Height);
			spriteBatch.Draw(XboxWaveHUD.mBG, this.mPosition, Color.White);
			spriteBatch.Draw(XboxWaveHUD.mBar, destinationRectangle, new Rectangle?(value), Color.White);
			if (this.mWaveNum != waveNum)
			{
				this.mWaveNum = waveNum;
				this.mWaveString = "Wave " + this.mWaveNum;
				this.mWaveCenter = VerchickMath.CenterText(Global.StoreFontBig, new Vector2(this.mPosition.X + 120f, this.mPosition.Y + 20f), this.mWaveString);
			}
			if (current != this.mCurrent)
			{
				this.mCurrent = current;
				this.mCurrentString = string.Format("{0}/{1}", current, total);
				this.mCurrentLoc = VerchickMath.CenterText(Global.StoreFont, new Vector2(this.mPosition.X + 120f, this.mPosition.Y + 66f), this.mCurrentString);
			}
			Shadow.DrawString(this.mCurrentString, Global.StoreFont, this.mCurrentLoc, 1, Color.White, spriteBatch);
			Shadow.DrawString(this.mWaveString, Global.StoreFontBig, this.mWaveCenter, 1, Color.White, spriteBatch);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00053D3F File Offset: 0x00051F3F
		public static void LoadGfx()
		{
			XboxWaveHUD.mBar = Global.Content.Load<Texture2D>("XboxHUD//WaveCountBar");
			XboxWaveHUD.mBG = Global.Content.Load<Texture2D>("XboxHUD//WaveCount");
		}

		// Token: 0x04000ACB RID: 2763
		private static Texture2D mBG;

		// Token: 0x04000ACC RID: 2764
		private static Texture2D mBar;

		// Token: 0x04000ACD RID: 2765
		private Vector2 mPosition;

		// Token: 0x04000ACE RID: 2766
		private Rectangle mFullBar;

		// Token: 0x04000ACF RID: 2767
		private string mWaveString;

		// Token: 0x04000AD0 RID: 2768
		private string mCurrentString;

		// Token: 0x04000AD1 RID: 2769
		private int mWaveNum = -1;

		// Token: 0x04000AD2 RID: 2770
		private int mCurrent = -1;

		// Token: 0x04000AD3 RID: 2771
		private Vector2 mWaveCenter;

		// Token: 0x04000AD4 RID: 2772
		private Vector2 mCurrentLoc;
	}
}
