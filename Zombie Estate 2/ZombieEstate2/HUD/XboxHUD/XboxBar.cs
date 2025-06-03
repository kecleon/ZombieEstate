using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.HUD.XboxHUD
{
	// Token: 0x020001C9 RID: 457
	internal class XboxBar
	{
		// Token: 0x06000C4C RID: 3148 RVA: 0x0006477C File Offset: 0x0006297C
		public XboxBar(int x, int y, bool vertical, Texture2D foreground, int iconX, int iconY)
		{
			this.mOriginalX = x;
			this.mOriginalY = y;
			this.mVertical = vertical;
			this.mForeground = foreground;
			this.mPosition = default(Rectangle);
			this.mPosition.X = x;
			this.mPosition.Y = y;
			this.mPosition.Width = foreground.Width;
			this.mPosition.Height = foreground.Height;
			this.mSource = default(Rectangle);
			this.mSource.Width = foreground.Width;
			this.mSource.Height = foreground.Height;
			this.mIcon = Global.GetTexRectange(iconX, iconY);
			this.mIconPosition = default(Rectangle);
			this.mIconPosition.X = foreground.Width / 2 - 8 + x;
			this.mIconPosition.Y = foreground.Height / 2 - 8 + y;
			this.mIconPosition.Width = 16;
			this.mIconPosition.Height = 16;
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x0006488C File Offset: 0x00062A8C
		public void Draw(SpriteBatch spriteBatch, int total, int current)
		{
			if (current > total)
			{
				current = total;
			}
			float num = (float)current / (float)total;
			if (this.mVertical)
			{
				this.mPosition.Y = this.mOriginalY + (int)((1f - num) * (float)this.mForeground.Height);
				this.mPosition.Height = (int)((float)this.mForeground.Height * num);
				this.mSource.Y = (int)((1f - num) * (float)this.mForeground.Height);
				this.mSource.Height = (int)((float)this.mForeground.Height * num);
			}
			else
			{
				this.mPosition.X = this.mOriginalX;
				this.mPosition.Width = (int)((float)this.mForeground.Width * num);
				this.mSource.Width = (int)((float)this.mForeground.Width * num);
			}
			spriteBatch.Draw(this.mForeground, this.mPosition, new Rectangle?(this.mSource), Color.White);
			spriteBatch.Draw(Global.MasterTexture, this.mIconPosition, new Rectangle?(this.mIcon), Color.White);
		}

		// Token: 0x04000CDE RID: 3294
		private bool mVertical;

		// Token: 0x04000CDF RID: 3295
		private Texture2D mForeground;

		// Token: 0x04000CE0 RID: 3296
		public Rectangle mPosition;

		// Token: 0x04000CE1 RID: 3297
		private Rectangle mSource;

		// Token: 0x04000CE2 RID: 3298
		private Rectangle mIcon;

		// Token: 0x04000CE3 RID: 3299
		private Rectangle mIconPosition;

		// Token: 0x04000CE4 RID: 3300
		private int mOriginalX;

		// Token: 0x04000CE5 RID: 3301
		private int mOriginalY;
	}
}
