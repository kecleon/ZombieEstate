using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000056 RID: 86
	public class DataString
	{
		// Token: 0x060001F6 RID: 502 RVA: 0x0000E438 File Offset: 0x0000C638
		public DataString(int iconX, int iconY, Vector2 loc, Color col, string value, bool badgeSlot = false, int badgeX = -1, int badgeY = -1, Player p = null) : this(iconX, iconY, loc, col, false)
		{
			if (p != null)
			{
				this.mBGColor = p.HUDColor;
			}
			this.mBadgeSlot = badgeSlot;
			if (badgeX != -1)
			{
				this.mBadgeEarned = true;
				this.mBadgeSrc = Global.GetTexRectange(badgeX, badgeY);
			}
			this.value = value;
			if (DataString.mBG == null)
			{
				DataString.mBG_Badge = Global.Content.Load<Texture2D>("XboxHUD\\StatLine");
				DataString.mBG = Global.Content.Load<Texture2D>("XboxHUD\\StatLine_NoBadge");
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000E4BC File Offset: 0x0000C6BC
		public DataString(int iconX, int iconY, Vector2 loc, Color col) : this(iconX, iconY, loc, col, false)
		{
			if (DataString.mBG == null)
			{
				DataString.mBG_Badge = Global.Content.Load<Texture2D>("XboxHUD\\StatLine");
				DataString.mBG = Global.Content.Load<Texture2D>("XboxHUD\\StatLine_NoBadge");
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000E4FC File Offset: 0x0000C6FC
		public DataString(int iconX, int iconY, Vector2 loc, Color col, bool time)
		{
			this.color = col;
			this.iconSrc = Global.GetTexRectange(iconX, iconY);
			this.Position = loc;
			this.iconDest = new Rectangle((int)this.Position.X - 64, (int)this.Position.Y + 8, 64, 64);
			this.Time = time;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000E57C File Offset: 0x0000C77C
		public void Update()
		{
			if (this.LerpValue > 0f)
			{
				this.LerpValue -= Global.REAL_GAME_TIME;
				if (this.LerpValue < 0f)
				{
					this.LerpValue = 0f;
				}
			}
			if (this.mInflateTime > 0f)
			{
				this.mInflateTime -= Global.REAL_GAME_TIME * 12f;
				if (this.mInflateTime < 0f)
				{
					this.mInflateTime = 0f;
				}
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000E600 File Offset: 0x0000C800
		public void Draw(SpriteBatch spriteBatch, int value)
		{
			if (this.Time && value < 10 && this.LerpValue == 0f)
			{
				this.Flash(Color.Red);
			}
			Color frontColor = Color.Lerp(this.color, this.lerpColor, this.LerpValue);
			Color backColor = Color.Lerp(Color.Black, Color.Lerp(Color.Black, this.lerpColor, 0.5f), this.LerpValue);
			spriteBatch.Draw(Global.MasterTexture, this.iconDest, new Rectangle?(this.iconSrc), Color.White);
			if (!this.Time)
			{
				Shadow.DrawOutlinedString(spriteBatch, Global.EquationFontSmall, VerchickMath.AddCommas(value), backColor, frontColor, 1f, 0f, this.Position);
				return;
			}
			Shadow.DrawOutlinedString(spriteBatch, Global.EquationFontSmall, Global.GetTimeString(value), backColor, frontColor, 1f, 0f, this.Position);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000E6E0 File Offset: 0x0000C8E0
		public void SetPosition(Vector2 pos)
		{
			this.Position = pos;
			this.mCenter = new Vector2(pos.X + 24f, pos.Y + 5f);
			this.iconDest = new Rectangle((int)pos.X + 5, (int)pos.Y + 7, 16, 16);
			this.mBadgeDest = new Rectangle((int)pos.X + 206, (int)pos.Y + 7, 16, 16);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000E760 File Offset: 0x0000C960
		public void Draw(SpriteBatch spriteBatch)
		{
			if (!this.Visible)
			{
				return;
			}
			Rectangle destinationRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, DataString.mBG.Width, DataString.mBG.Height);
			destinationRectangle.Inflate((int)(16f * this.mInflateTime), (int)(16f * this.mInflateTime));
			spriteBatch.Draw(this.mBadgeSlot ? DataString.mBG_Badge : DataString.mBG, destinationRectangle, this.mBGColor);
			Color color = Color.Lerp(this.color, this.lerpColor, this.LerpValue);
			Color shadowColor = Color.Lerp(Color.Black, Color.Lerp(Color.Black, this.lerpColor, 0.5f), this.LerpValue);
			spriteBatch.Draw(Global.MasterTexture, this.iconDest, new Rectangle?(this.iconSrc), Color.White);
			Shadow.DrawString(this.value, Global.StoreFontSmall, this.mCenter, 1, color, shadowColor, spriteBatch);
			if (this.mBadgeEarned)
			{
				spriteBatch.Draw(Global.MasterTexture, this.mBadgeDest, new Rectangle?(this.mBadgeSrc), color);
				if (this.LerpValue > 0f)
				{
					spriteBatch.Draw(Global.MasterTexture, this.mBadgeDest, new Rectangle?(Global.GetTexRectange(74, 47)), Color.White * this.LerpValue);
				}
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000E8C5 File Offset: 0x0000CAC5
		public void Flash(Color col)
		{
			this.lerpColor = col;
			this.LerpValue = 1f;
			this.mInflateTime = 2f;
		}

		// Token: 0x0400017D RID: 381
		private Color color;

		// Token: 0x0400017E RID: 382
		private Color lerpColor;

		// Token: 0x0400017F RID: 383
		private Rectangle iconSrc;

		// Token: 0x04000180 RID: 384
		private Rectangle iconDest;

		// Token: 0x04000181 RID: 385
		private float LerpValue;

		// Token: 0x04000182 RID: 386
		private Vector2 Position;

		// Token: 0x04000183 RID: 387
		private bool Time;

		// Token: 0x04000184 RID: 388
		private static Texture2D mBG;

		// Token: 0x04000185 RID: 389
		private static Texture2D mBG_Badge;

		// Token: 0x04000186 RID: 390
		public bool Visible = true;

		// Token: 0x04000187 RID: 391
		private string value;

		// Token: 0x04000188 RID: 392
		private Rectangle mBadgeSrc;

		// Token: 0x04000189 RID: 393
		private Rectangle mBadgeDest;

		// Token: 0x0400018A RID: 394
		private bool mBadgeEarned;

		// Token: 0x0400018B RID: 395
		private bool mBadgeSlot;

		// Token: 0x0400018C RID: 396
		private Color mBGColor = Color.White;

		// Token: 0x0400018D RID: 397
		private float mInflateTime = 1f;

		// Token: 0x0400018E RID: 398
		private Vector2 mCenter;
	}
}
