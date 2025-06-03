using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.HUD.XboxHUD
{
	// Token: 0x020001CA RID: 458
	public class XboxHUD
	{
		// Token: 0x06000C4E RID: 3150 RVA: 0x000649B4 File Offset: 0x00062BB4
		public XboxHUD(Player player)
		{
			this.mAmmoBars = new List<XboxBar>();
			this.mPlayer = player;
			this.SetPosition();
			this.mStats = new XboxStatsHUD(player, this.mPosition, false);
			XboxHUD.LoadTex();
			this.SetUpAmmoBars();
			this.mPosHealthPacks.X = 70f + this.mPosition.X;
			this.mPosHealthPacks.Y = 91f + this.mPosition.Y;
			this.mHealthBar = new XboxBar(4 + (int)this.mPosition.X, 9 + (int)this.mPosition.Y, true, XboxHUD.mHealthTex, 63, 63);
			this.CenterHealth = new Vector2(this.mPosition.X + 25f, this.mPosition.Y + 52f);
			this.CenterMoney = new Vector2(this.mPosition.X + 172f, this.mPosition.Y + 91f);
			this.CenterClip = new Vector2(this.mPosition.X + 120f, this.mPosition.Y + 81f);
			this.CenterName = new Vector2(this.mPosition.X + 120f, this.mPosition.Y - 12f);
			this.mClipBar = new XboxBar(52 + (int)this.mPosition.X, 72 + (int)this.mPosition.Y, false, XboxHUD.mClipTex, 63, 63);
			this.mWeaponSelect = new XboxWeaponSelect(player, new Vector2(this.mPosition.X + 51f, this.mPosition.Y + 14f));
			new Rectangle((int)this.mPosition.X + 4, (int)this.mPosition.Y + 102, 48, 16);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00064C6C File Offset: 0x00062E6C
		private void SetPosition()
		{
			Rectangle safeScreenArea = Global.GetSafeScreenArea();
			switch (this.mPlayer.Index)
			{
			case 0:
				this.mPosition = new Vector2((float)safeScreenArea.X, (float)(safeScreenArea.Y + 20));
				break;
			case 1:
				this.mPosition = new Vector2((float)(safeScreenArea.Right - 192), (float)(safeScreenArea.Y + 20));
				break;
			case 2:
				this.mPosition = new Vector2((float)safeScreenArea.X, (float)(safeScreenArea.Bottom - 123 - 20));
				break;
			case 3:
				this.mPosition = new Vector2((float)(safeScreenArea.Right - 192), (float)(safeScreenArea.Bottom - 123 - 20));
				break;
			default:
				this.mPosition = new Vector2((float)safeScreenArea.X, (float)safeScreenArea.Y);
				break;
			}
			if (this.mPlayer.Index == 2 || this.mPlayer.Index == 3)
			{
				this.mAlarm = new AlarmMessage(new Vector2(this.mPosition.X + (float)(XboxHUD.mBackgroundTex.Width / 2), this.mPosition.Y - 24f), this.mPlayer);
				return;
			}
			this.mAlarm = new AlarmMessage(new Vector2(this.mPosition.X + (float)(XboxHUD.mBackgroundTex.Width / 2), this.mPosition.Y + 24f + (float)XboxHUD.mBackgroundTex.Height), this.mPlayer);
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00064DFC File Offset: 0x00062FFC
		private void SetUpAmmoBars()
		{
			XboxBar item = new XboxBar(53 + (int)this.mPosition.X, 36 + (int)this.mPosition.Y, true, XboxHUD.mAmmoTex, 0, 43);
			this.mAmmoBars.Add(item);
			item = new XboxBar(91 + (int)this.mPosition.X, 36 + (int)this.mPosition.Y, true, XboxHUD.mAmmoTex, 0, 45);
			this.mAmmoBars.Add(item);
			item = new XboxBar(129 + (int)this.mPosition.X, 36 + (int)this.mPosition.Y, true, XboxHUD.mAmmoTex, 0, 44);
			this.mAmmoBars.Add(item);
			item = new XboxBar(167 + (int)this.mPosition.X, 36 + (int)this.mPosition.Y, true, XboxHUD.mAmmoTex, 0, 46);
			this.mAmmoBars.Add(item);
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00064EF4 File Offset: 0x000630F4
		public void Update(float elapsed)
		{
			if (InputManager.ButtonHeld(ButtonPress.ViewStats, this.mPlayer.Index, false))
			{
				this.mStatsMode = true;
				this.mPlayer.mMovement = Vector2.Zero;
			}
			else
			{
				this.mStatsMode = false;
			}
			if (this.mStatsMode)
			{
				this.mStats.Update();
			}
			this.mAlarm.Update(elapsed);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00064F58 File Offset: 0x00063158
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.mPlayer.IAmOwnedByLocalPlayer)
			{
				this.mAlarm.Draw(spriteBatch);
			}
			if (this.mStatsMode)
			{
				this.mStats.Draw(spriteBatch);
				return;
			}
			if (PlayerManager.GetPlayer(this.mPlayer.Index).UsingController)
			{
				spriteBatch.Draw(XboxHUD.mBackgroundTex, this.mPosition, this.mPlayer.HUDColor);
				spriteBatch.Draw(XboxHUD.mHealthBGTex, this.mPosition, Color.White);
			}
			else
			{
				spriteBatch.Draw(XboxHUD.mBackgroundPCTex, this.mPosition, this.mPlayer.HUDColor);
				spriteBatch.Draw(XboxHUD.mHealthBGPCTex, this.mPosition, Color.White);
			}
			this.DrawClipBar(spriteBatch);
			this.DrawAmmoBars(spriteBatch);
			this.DrawHealthBar(spriteBatch);
			this.DrawMoney(spriteBatch);
			if (!this.mWeaponSelect.Draw(spriteBatch))
			{
				this.DrawName(spriteBatch);
			}
			this.DrawReady(spriteBatch);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00065048 File Offset: 0x00063248
		private void DrawHealthBar(SpriteBatch spriteBatch)
		{
			this.mHealthBar.Draw(spriteBatch, (int)this.mPlayer.SpecialProperties.MaxHealth, (int)this.mPlayer.Health);
			int num = (int)this.mPlayer.Health;
			if (num != this.mPrevHealth)
			{
				this.mPrevHealth = num;
				this.mStringHealth = num.ToString();
				this.mPosHealth = VerchickMath.CenterText(Global.EquationFontSmall, this.CenterHealth, this.mStringHealth);
			}
			int healthPacks = this.mPlayer.Stats.HealthPacks;
			if (healthPacks != this.mPrevHealthPack)
			{
				this.mPrevHealthPack = healthPacks;
				this.mStringHealthPacks = "x" + healthPacks.ToString();
			}
			Shadow.DrawString(this.mStringHealth, Global.EquationFontSmall, this.mPosHealth, 1, Color.White, spriteBatch);
			Shadow.DrawString(this.mStringHealthPacks, Global.EquationFontSmall, this.mPosHealthPacks, 1, Color.White, spriteBatch);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00065138 File Offset: 0x00063338
		private void DrawAmmoBars(SpriteBatch spriteBatch)
		{
			this.mAmmoBars[0].Draw(spriteBatch, this.mPlayer.Stats.GetMaxAmmo(AmmoType.ASSAULT), this.mPlayer.Stats.GetAmmo(AmmoType.ASSAULT));
			this.mAmmoBars[1].Draw(spriteBatch, this.mPlayer.Stats.GetMaxAmmo(AmmoType.SHELLS), this.mPlayer.Stats.GetAmmo(AmmoType.SHELLS));
			this.mAmmoBars[2].Draw(spriteBatch, this.mPlayer.Stats.GetMaxAmmo(AmmoType.HEAVY), this.mPlayer.Stats.GetAmmo(AmmoType.HEAVY));
			this.mAmmoBars[3].Draw(spriteBatch, this.mPlayer.Stats.GetMaxAmmo(AmmoType.EXPLOSIVE), this.mPlayer.Stats.GetAmmo(AmmoType.EXPLOSIVE));
			Vector2 zero = Vector2.Zero;
			if (this.mPlayer.mGun.ammoType == AmmoType.ASSAULT)
			{
				zero = new Vector2(this.mPosition.X + 53f - 3f, this.mPosition.Y + 33f);
			}
			if (this.mPlayer.mGun.ammoType == AmmoType.SHELLS)
			{
				zero = new Vector2(this.mPosition.X + 91f - 3f, this.mPosition.Y + 33f);
			}
			if (this.mPlayer.mGun.ammoType == AmmoType.HEAVY)
			{
				zero = new Vector2(this.mPosition.X + 129f - 3f, this.mPosition.Y + 33f);
			}
			if (this.mPlayer.mGun.ammoType == AmmoType.EXPLOSIVE)
			{
				zero = new Vector2(this.mPosition.X + 167f - 3f, this.mPosition.Y + 33f);
			}
			if (zero != Vector2.Zero)
			{
				spriteBatch.Draw(XboxHUD.mAmmoHighlight, zero, Color.White);
			}
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00065340 File Offset: 0x00063540
		private void DrawClipBar(SpriteBatch spriteBatch)
		{
			int bulletsInClip = this.mPlayer.mGun.stats.GunProperties[this.mPlayer.mGun.GetLevel()].BulletsInClip;
			int bulletsInClip2 = this.mPlayer.mGun.bulletsInClip;
			this.mClipBar.Draw(spriteBatch, bulletsInClip, bulletsInClip2);
			if (bulletsInClip != this.mPrevClipSize || bulletsInClip2 != this.mPrevClip)
			{
				this.mPrevClip = bulletsInClip2;
				this.mPrevClipSize = bulletsInClip;
				this.mStringClip = bulletsInClip2.ToString() + "/" + bulletsInClip.ToString();
				this.mPosClip = VerchickMath.CenterText(Global.EquationFontSmall, this.CenterClip, this.mStringClip);
			}
			Shadow.DrawString(this.mStringClip, Global.EquationFontSmall, this.mPosClip, 1, Color.White, spriteBatch);
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00065414 File Offset: 0x00063614
		private void DrawMoney(SpriteBatch spriteBatch)
		{
			int money = this.mPlayer.Stats.GetMoney();
			if (money != this.mPrevMoney)
			{
				this.mPrevMoney = money;
				this.mStringMoney = money.ToString();
				this.mPosMoney = new Vector2(this.CenterMoney.X - Global.EquationFontSmall.MeasureString(this.mStringMoney).X, this.CenterMoney.Y);
			}
			Shadow.DrawString(this.mStringMoney, Global.EquationFontSmall, this.mPosMoney, 1, Color.LightGreen, spriteBatch);
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x000654A4 File Offset: 0x000636A4
		private void DrawReady(SpriteBatch spriteBatch)
		{
			if (Global.WaveMaster.PreWaveActive || Global.WaveMaster.WaveActive)
			{
				return;
			}
			if (this.mReadyLerp > 0f)
			{
				this.mReadyLerp -= Global.REAL_GAME_TIME * 8f;
				if (this.mReadyLerp < 0f)
				{
					this.mReadyLerp = 0f;
				}
			}
			Vector2 vector = new Vector2(this.mPosition.X + 4f, this.mPosition.Y + (float)XboxHUD.mBackgroundTex.Height + 48f);
			if (this.mPlayer.Index == 2 || this.mPlayer.Index == 3)
			{
				vector.Y = this.mPosition.Y - 100f;
			}
			if (!this.mPlayer.READY)
			{
				string text;
				if (this.mPlayer.PlayerInfo.UsingController)
				{
					text = "Press [Back] when ready!";
				}
				else
				{
					text = "Press " + InputManager.GetKeyString(ButtonPress.Ready, this.mPlayer.Index) + " when ready!";
				}
				if (this.mPlayer.Index == 1 || this.mPlayer.Index == 3)
				{
					vector.X = (float)Global.ScreenRect.Width - Global.EquationFontSmall.MeasureString(text).X - 20f;
				}
				Shadow.DrawString(text, Global.EquationFontSmall, vector, 1, Color.White, spriteBatch);
				return;
			}
			if (this.mPlayer.Index == 1 || this.mPlayer.Index == 3)
			{
				vector.X = this.mPosition.X + 4f;
			}
			int num = -200;
			if (this.mPlayer.Index == 1 || this.mPlayer.Index == 3)
			{
				num = 200;
			}
			Vector2 value = new Vector2(vector.X + (float)num, vector.Y);
			Vector2 position = Vector2.Lerp(vector, value, this.mReadyLerp);
			Color frontColor = Color.Lerp(Color.LightGreen, Color.White, Global.Pulse);
			Shadow.DrawOutlinedString(spriteBatch, Global.StoreFontXtraLarge, XboxHUD.mReady, Color.Black, frontColor, 1f, 0f, position);
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x000656CE File Offset: 0x000638CE
		public void TriggerReady(bool ready)
		{
			this.mReadyLerp = 1f;
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x000656DC File Offset: 0x000638DC
		private void DrawName(SpriteBatch spriteBatch)
		{
			Vector2 pos = VerchickMath.CenterText(Global.StoreFont, this.CenterName, this.mPlayer.GamerName);
			Shadow.DrawString(this.mPlayer.GamerName, Global.StoreFont, pos, 1, Color.LightGray, spriteBatch);
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00065724 File Offset: 0x00063924
		public static void LoadTex()
		{
			if (XboxHUD.mHealthTex == null)
			{
				XboxHUD.mClipTex = Global.Content.Load<Texture2D>("XboxHUD//ClipFull");
				XboxHUD.mAmmoTex = Global.Content.Load<Texture2D>("XboxHUD//AmmoFull");
				XboxHUD.mHealthTex = Global.Content.Load<Texture2D>("XboxHUD//HealthFull");
				XboxHUD.mBackgroundTex = Global.Content.Load<Texture2D>("XboxHUD//Background");
				XboxHUD.mHealthBGTex = Global.Content.Load<Texture2D>("XboxHUD//HealthBG");
				XboxHUD.mAmmoHighlight = Global.Content.Load<Texture2D>("XboxHUD//AmmoHighlight");
				XboxHUD.mHealthBGPCTex = Global.Content.Load<Texture2D>("XboxHUD//HealthBG_PC");
				XboxHUD.mBackgroundPCTex = Global.Content.Load<Texture2D>("XboxHUD//Background_PC");
			}
		}

		// Token: 0x04000CE6 RID: 3302
		private Player mPlayer;

		// Token: 0x04000CE7 RID: 3303
		private List<XboxBar> mAmmoBars;

		// Token: 0x04000CE8 RID: 3304
		private XboxBar mHealthBar;

		// Token: 0x04000CE9 RID: 3305
		private XboxBar mClipBar;

		// Token: 0x04000CEA RID: 3306
		private XboxWeaponSelect mWeaponSelect;

		// Token: 0x04000CEB RID: 3307
		private XboxStatsHUD mStats;

		// Token: 0x04000CEC RID: 3308
		public AlarmMessage mAlarm;

		// Token: 0x04000CED RID: 3309
		private static Texture2D mHealthTex;

		// Token: 0x04000CEE RID: 3310
		public static Texture2D mAmmoTex;

		// Token: 0x04000CEF RID: 3311
		private static Texture2D mBackgroundTex;

		// Token: 0x04000CF0 RID: 3312
		private static Texture2D mBackgroundPCTex;

		// Token: 0x04000CF1 RID: 3313
		private static Texture2D mClipTex;

		// Token: 0x04000CF2 RID: 3314
		private static Texture2D mHealthBGTex;

		// Token: 0x04000CF3 RID: 3315
		private static Texture2D mHealthBGPCTex;

		// Token: 0x04000CF4 RID: 3316
		private static Texture2D mAmmoHighlight;

		// Token: 0x04000CF5 RID: 3317
		public Vector2 mPosition = new Vector2(64f, 64f);

		// Token: 0x04000CF6 RID: 3318
		public bool mStatsMode;

		// Token: 0x04000CF7 RID: 3319
		private int mPrevMoney = -1;

		// Token: 0x04000CF8 RID: 3320
		private int mPrevHealthPack = -1;

		// Token: 0x04000CF9 RID: 3321
		private int mPrevHealth = -1;

		// Token: 0x04000CFA RID: 3322
		private int mPrevClipSize = -1;

		// Token: 0x04000CFB RID: 3323
		private int mPrevClip = -1;

		// Token: 0x04000CFC RID: 3324
		private string mStringHealth = "0";

		// Token: 0x04000CFD RID: 3325
		private string mStringHealthPacks = "0";

		// Token: 0x04000CFE RID: 3326
		private string mStringMoney = "0";

		// Token: 0x04000CFF RID: 3327
		private string mStringClip = "-/-";

		// Token: 0x04000D00 RID: 3328
		private static string mPressBack = "Press [Back] when ready!";

		// Token: 0x04000D01 RID: 3329
		private static string mReady = "Ready!";

		// Token: 0x04000D02 RID: 3330
		private Vector2 mPosHealth = new Vector2(0f, 0f);

		// Token: 0x04000D03 RID: 3331
		private Vector2 mPosHealthPacks = new Vector2(0f, 0f);

		// Token: 0x04000D04 RID: 3332
		private Vector2 mPosMoney = new Vector2(0f, 0f);

		// Token: 0x04000D05 RID: 3333
		private Vector2 mPosClip = new Vector2(0f, 0f);

		// Token: 0x04000D06 RID: 3334
		private Vector2 CenterHealth;

		// Token: 0x04000D07 RID: 3335
		private Vector2 CenterMoney;

		// Token: 0x04000D08 RID: 3336
		private Vector2 CenterClip;

		// Token: 0x04000D09 RID: 3337
		private Vector2 CenterName;

		// Token: 0x04000D0A RID: 3338
		private float mReadyLerp = 1f;
	}
}
