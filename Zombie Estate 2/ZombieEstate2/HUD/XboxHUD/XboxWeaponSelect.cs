using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.HUD.XboxHUD
{
	// Token: 0x020001CD RID: 461
	public class XboxWeaponSelect
	{
		// Token: 0x06000C67 RID: 3175 RVA: 0x00066944 File Offset: 0x00064B44
		public XboxWeaponSelect(Player player, Vector2 topLeft)
		{
			this.mTopLeft = topLeft;
			this.mPlayer = player;
			this.mCurrentDraw = this.mTopLeft;
			this.mCurrentWeaponDraw = default(Rectangle);
			if (XboxWeaponSelect.mTexSelected == null)
			{
				XboxWeaponSelect.mTexSelected = Global.Content.Load<Texture2D>("XboxHUD//WeaponWheelSelected");
				XboxWeaponSelect.mTexUnselected = Global.Content.Load<Texture2D>("XboxHUD//WeaponWheelUnselected");
			}
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x000669AC File Offset: 0x00064BAC
		public bool Draw(SpriteBatch spriteBatch)
		{
			bool result = this.DrawName(spriteBatch);
			this.mCurrentDraw.X = this.mTopLeft.X;
			this.mCurrentDraw.Y = this.mTopLeft.Y;
			Gun mGun = this.mPlayer.mGun;
			for (int i = 0; i < Player.MAXGUNS; i++)
			{
				Gun gun = this.mPlayer.Guns[i];
				if (gun == null)
				{
					spriteBatch.Draw(XboxWeaponSelect.mTexUnselected, this.mCurrentDraw, this.mPlayer.HUDColor);
					this.mCurrentDraw.X = this.mCurrentDraw.X + (float)(XboxWeaponSelect.mTexUnselected.Width + 1);
				}
				else if (gun == mGun)
				{
					this.mCurrentDraw.Y = this.mCurrentDraw.Y - 16f;
					spriteBatch.Draw(XboxWeaponSelect.mTexSelected, this.mCurrentDraw, this.mPlayer.HUDColor);
					this.mCurrentWeaponDraw = new Rectangle((int)this.mCurrentDraw.X, (int)this.mCurrentDraw.Y, 32, 32);
					spriteBatch.Draw(Global.MasterTexture, this.mCurrentWeaponDraw, new Rectangle?(Global.GetTexRectange(gun.OriginTex.X, gun.OriginTex.Y + 1)), Color.White);
					this.mCurrentDraw.Y = this.mCurrentDraw.Y + 16f;
					this.mCurrentDraw.X = this.mCurrentDraw.X + (float)(XboxWeaponSelect.mTexSelected.Width + 1);
				}
				else
				{
					spriteBatch.Draw(XboxWeaponSelect.mTexUnselected, this.mCurrentDraw, this.mPlayer.HUDColor);
					this.mCurrentWeaponDraw = new Rectangle((int)this.mCurrentDraw.X, (int)this.mCurrentDraw.Y, 16, 16);
					spriteBatch.Draw(Global.MasterTexture, this.mCurrentWeaponDraw, new Rectangle?(Global.GetTexRectange(gun.OriginTex.X, gun.OriginTex.Y + 1)), Color.White);
					this.mCurrentDraw.X = this.mCurrentDraw.X + (float)(XboxWeaponSelect.mTexUnselected.Width + 1);
				}
			}
			return result;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00066BC0 File Offset: 0x00064DC0
		private bool DrawName(SpriteBatch spriteBatch)
		{
			Gun mGun = this.mPlayer.mGun;
			if (mGun != this.mPrevGun)
			{
				this.mFadeTime = 2f;
				this.mNamePos = VerchickMath.CenterText(Global.StoreFont, new Vector2(this.mTopLeft.X + 120f - 51f, this.mTopLeft.Y - 24f), mGun.Name);
			}
			if (this.mFadeTime > 0f)
			{
				this.mFadeTime -= 0.016666668f;
				float scale = 1f;
				if ((double)this.mFadeTime < 1.0)
				{
					scale = this.mFadeTime / 1f;
				}
				this.mPrevGun = mGun;
				Shadow.DrawOutlinedString(spriteBatch, Global.StoreFont, mGun.Name, Color.Black * scale, Color.White * scale, 1f, 0f, this.mNamePos);
				return true;
			}
			this.mPrevGun = mGun;
			return false;
		}

		// Token: 0x04000D1F RID: 3359
		private Player mPlayer;

		// Token: 0x04000D20 RID: 3360
		private Vector2 mTopLeft;

		// Token: 0x04000D21 RID: 3361
		private Vector2 mCurrentDraw;

		// Token: 0x04000D22 RID: 3362
		private Rectangle mCurrentWeaponDraw;

		// Token: 0x04000D23 RID: 3363
		private float mFadeTime;

		// Token: 0x04000D24 RID: 3364
		private Vector2 mNamePos;

		// Token: 0x04000D25 RID: 3365
		private Gun mPrevGun;

		// Token: 0x04000D26 RID: 3366
		private static Texture2D mTexSelected;

		// Token: 0x04000D27 RID: 3367
		private static Texture2D mTexUnselected;
	}
}
