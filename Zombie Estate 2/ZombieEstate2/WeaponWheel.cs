using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000067 RID: 103
	public class WeaponWheel
	{
		// Token: 0x0600024C RID: 588 RVA: 0x0001227C File Offset: 0x0001047C
		public WeaponWheel(Player player)
		{
			this.parent = player;
			this.xOrig = 420;
			this.yOrig = Global.ScreenRect.Height - 180;
			this.BGSrc = Global.GetTexRectange(0, 37);
			this.BGSrcSel = Global.GetTexRectange(1, 37);
			this.CurrWepDest = new Rectangle(Global.ScreenRect.Width / 2 - 32, Global.ScreenRect.Height / 2 + 0, 64, 64);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00012344 File Offset: 0x00010544
		public void Update()
		{
			InputManager.MouseWheelUp();
			InputManager.MouseWheelDown();
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00012352 File Offset: 0x00010552
		// (set) Token: 0x0600024F RID: 591 RVA: 0x0001235A File Offset: 0x0001055A
		public int SelectedIndex
		{
			get
			{
				return this.sel;
			}
			set
			{
				if (this.sel != value)
				{
					this.sel = value;
					this.hudWepTimer.Reset();
					this.hudWepTimer.Start();
				}
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00012384 File Offset: 0x00010584
		public void Draw(SpriteBatch spriteBatch)
		{
			int i;
			for (i = 0; i < this.parent.GetGunCount(); i++)
			{
				this.DrawBG(spriteBatch, i);
				this.DrawWeapon(spriteBatch, i);
				this.textPos.X = (float)(this.BGRect.X + 3);
				this.textPos.Y = (float)(this.BGRect.Y + 1);
				Shadow.DrawString((i + 1).ToString(), Global.StoreFont, this.textPos, 1, Color.White, spriteBatch);
			}
			while (i < Player.MAXGUNS)
			{
				this.DrawBG(spriteBatch, i);
				this.textPos.X = (float)(this.BGRect.X + 3);
				this.textPos.Y = (float)(this.BGRect.Y + 1);
				Shadow.DrawString((i + 1).ToString(), Global.StoreFont, this.textPos, 1, Color.White, spriteBatch);
				i++;
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00012478 File Offset: 0x00010678
		private void DrawBG(SpriteBatch spriteBatch, int i)
		{
			if (i < 4)
			{
				this.BGRect.X = this.xOrig + i * 50;
				this.BGRect.Y = this.yOrig;
			}
			else
			{
				this.BGRect.X = this.xOrig + (i - 4) * 50;
				this.BGRect.Y = this.yOrig + 50;
			}
			Color hudcolor = this.parent.HUDColor;
			if (i == this.sel)
			{
				spriteBatch.Draw(Global.MasterTexture, this.BGRect, new Rectangle?(this.BGSrcSel), hudcolor);
				return;
			}
			spriteBatch.Draw(Global.MasterTexture, this.BGRect, new Rectangle?(this.BGSrc), hudcolor);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00012530 File Offset: 0x00010730
		private void DrawWeapon(SpriteBatch spriteBatch, int i)
		{
			this.WeaponSrc = Global.GetTexRectange(this.Guns[i].OriginTex.X, this.Guns[i].OriginTex.Y + 1);
			if (i < 4)
			{
				this.BGRect.X = this.xOrig + i * 50;
				this.BGRect.Y = this.yOrig;
			}
			else
			{
				this.BGRect.X = this.xOrig + (i - 4) * 50;
				this.BGRect.Y = this.yOrig + 50;
			}
			spriteBatch.Draw(Global.MasterTexture, this.BGRect, new Rectangle?(this.WeaponSrc), Color.White);
			if (this.Guns[i].ammoType != AmmoType.INFINITE && this.parent.Stats.GetAmmo(this.Guns[i].ammoType) <= 0 && this.Guns[i].bulletsInClip <= 0)
			{
				spriteBatch.Draw(Global.MasterTexture, this.BGRect, new Rectangle?(this.OOARect), Color.White * 0.5f);
			}
			this.DrawWep(spriteBatch);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0001265C File Offset: 0x0001085C
		private void DrawWep(SpriteBatch spriteBatch)
		{
			if (this.hudWepTimer.Running())
			{
				float num = 1f - this.hudWepTimer.Delta();
				if (num > 0.5f)
				{
					num = 1f;
				}
				else
				{
					num /= 0.5f;
				}
				Rectangle texRectange = Global.GetTexRectange(this.Guns[this.sel].OriginTex.X, this.Guns[this.sel].OriginTex.Y + 1);
				spriteBatch.Draw(Global.MasterTexture, this.CurrWepDest, new Rectangle?(this.BGSrc), this.parent.HUDColor * num);
				spriteBatch.Draw(Global.MasterTexture, this.CurrWepDest, new Rectangle?(texRectange), Color.White * num);
				Vector2 vector = new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2 + 138));
				vector = VerchickMath.CenterText(Global.StoreFontBig, vector, this.Guns[this.sel].Name);
				Shadow.DrawString(this.Guns[this.sel].Name, Global.StoreFontBig, vector, 2, Color.White * num, Color.Black * num, spriteBatch);
				if (this.parent.mGun.ammoType != AmmoType.INFINITE && this.parent.Stats.GetAmmo(this.parent.mGun.ammoType) <= 0 && this.parent.mGun.bulletsInClip <= 0)
				{
					vector = new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2 + 54));
					vector = VerchickMath.CenterText(Global.StoreFontBig, vector, "Out of Ammo!");
					Shadow.DrawString("Out of Ammo!", Global.StoreFontBig, vector, 2, Color.White * num, Color.Red * num, spriteBatch);
				}
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0001284B File Offset: 0x00010A4B
		private Gun[] Guns
		{
			get
			{
				return this.parent.Guns;
			}
		}

		// Token: 0x0400024E RID: 590
		private Player parent;

		// Token: 0x0400024F RID: 591
		private int xOrig;

		// Token: 0x04000250 RID: 592
		private int yOrig;

		// Token: 0x04000251 RID: 593
		private int sel;

		// Token: 0x04000252 RID: 594
		private Timer hudWepTimer = new Timer(2f);

		// Token: 0x04000253 RID: 595
		private Vector2 textPos = new Vector2(0f, 0f);

		// Token: 0x04000254 RID: 596
		private Rectangle BGRect = new Rectangle(0, 0, 48, 48);

		// Token: 0x04000255 RID: 597
		private Rectangle BGSrc;

		// Token: 0x04000256 RID: 598
		private Rectangle BGSrcSel;

		// Token: 0x04000257 RID: 599
		private Rectangle WeaponSrc;

		// Token: 0x04000258 RID: 600
		private Rectangle OOARect = Global.GetTexRectange(62, 63);

		// Token: 0x04000259 RID: 601
		private Rectangle CurrWepDest;
	}
}
