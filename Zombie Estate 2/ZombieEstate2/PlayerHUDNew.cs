using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000064 RID: 100
	public class PlayerHUDNew
	{
		// Token: 0x06000234 RID: 564 RVA: 0x0001113C File Offset: 0x0000F33C
		public PlayerHUDNew(Player parent)
		{
			this.Parent = parent;
			this.HealthPosition = new Vector2(64f, (float)(Global.ScreenRect.Height - 168));
			this.GunPosition = new Vector2((float)(Global.ScreenRect.Width - 340 - 64), (float)(Global.ScreenRect.Height - 168));
			this.Health = new HealthBar(this.HealthPosition, parent.StartTextureCoord.X + 1, parent.StartTextureCoord.Y, false, Color.LightGreen, Color.DarkRed, parent.HUDColor);
			this.Gun = new HealthBar(this.GunPosition, 63, 63, true, AmmoMeter.AmmoColor, new Color(0.1f, 0.1f, 0.1f), parent.HUDColor);
			this.Money = new DataString(4, 44, PlayerHUDNew.MoneyLoc, Color.LightGreen);
			this.HealthPacks = Global.Content.Load<Texture2D>("HUD\\NewHud\\HealthPacks");
			this.HealthPackSrc = Global.GetTexRectange(0, 42);
			this.HealthPackLoc = new Vector2(this.HealthPosition.X + 234f, this.HealthPosition.Y + 38f);
			this.HealthPackDest = new Rectangle((int)this.HealthPackLoc.X + 38, (int)this.HealthPackLoc.Y + 1, 32, 32);
			this.HealthPackStringLoc = new Vector2(this.HealthPackLoc.X + 10f, this.HealthPackLoc.Y + 8f);
			this.HUDAbility = Global.Content.Load<Texture2D>("HUD_Ability");
			this.InitAmmo();
			this.Alarm = new AlarmMessage(new Vector2(0f, 0f), parent);
			this.Wheel = new WeaponWheel(parent);
			this.Minion = new MinionIndicator(parent, new Vector2(this.HealthPosition.X + 380f, this.HealthPosition.Y - 60f));
			this.Minis = new List<MiniHUD>();
			foreach (Player player in Global.PlayerList)
			{
				if (player.Index != parent.Index)
				{
					MiniHUD item = new MiniHUD(player);
					this.Minis.Add(item);
				}
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000113D0 File Offset: 0x0000F5D0
		private void InitAmmo()
		{
			int num = (int)this.GunPosition.X;
			int num2 = (int)this.GunPosition.Y - 70;
			this.Assault = new HUDAmmoMeter(new Vector2((float)num, (float)num2), this.Parent.HUDColor, 0, 43);
			this.Shells = new HUDAmmoMeter(new Vector2((float)(num + 44), (float)num2), this.Parent.HUDColor, 0, 45);
			this.Heavy = new HUDAmmoMeter(new Vector2((float)(num + 88), (float)num2), this.Parent.HUDColor, 0, 44);
			this.Explosive = new HUDAmmoMeter(new Vector2((float)(num + 132), (float)num2), this.Parent.HUDColor, 0, 46);
			this.Special = new HUDAmmoMeter(new Vector2((float)(num + 176), (float)num2), this.Parent.HUDColor, 0, 47);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000114B6 File Offset: 0x0000F6B6
		public void Update()
		{
			this.Money.Update();
			this.Alarm.Update(0.016666668f);
			this.Wheel.Update();
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000114E0 File Offset: 0x0000F6E0
		public void Draw(SpriteBatch spriteBatch)
		{
			this.Health.Draw(spriteBatch, (int)this.Parent.Health, (int)this.Parent.SpecialProperties.MaxHealth);
			this.Gun.Draw(spriteBatch, this.Parent.mGun.bulletsInClip, this.Parent.mGun.stats.GunProperties[this.Parent.mGun.GetLevel()].BulletsInClip);
			this.Money.Draw(spriteBatch, this.Parent.Stats.GetMoney());
			this.DrawAmmo(spriteBatch);
			this.DrawHealthPacks(spriteBatch);
			this.Minion.Draw(spriteBatch);
			this.DrawAbilityMeter(spriteBatch);
			foreach (MiniHUD miniHUD in this.Minis)
			{
				miniHUD.Draw(spriteBatch);
			}
			if (this.ControlActive && this.ControlTip != null)
			{
				if (this.ControlTipCycleTimer.Expired() || !this.ControlTipCycleTimer.Running())
				{
					this.LeftControlTip = !this.LeftControlTip;
					this.ControlTipCycleTimer.Reset();
					this.ControlTipCycleTimer.Start();
				}
				Vector2 screenPosition = VerchickMath.GetScreenPosition(this.Parent.Position);
				screenPosition.Y += 50f;
				screenPosition.X -= 50f;
				Rectangle value = new Rectangle(0, 0, 100, 100);
				if (!this.LeftControlTip)
				{
					value.X = 100;
				}
				spriteBatch.Draw(this.ControlTip, screenPosition, new Rectangle?(value), Color.White);
				this.ControlActive = false;
			}
			this.Alarm.Draw(spriteBatch);
			this.Wheel.Draw(spriteBatch);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000116C0 File Offset: 0x0000F8C0
		private void DrawAmmo(SpriteBatch spriteBatch)
		{
			this.Assault.Draw(spriteBatch, this.Parent.Stats.GetMaxAmmo(AmmoType.ASSAULT), this.Parent.Stats.GetAmmo(AmmoType.ASSAULT), this.Parent.mGun.ammoType == AmmoType.ASSAULT);
			this.Shells.Draw(spriteBatch, this.Parent.Stats.GetMaxAmmo(AmmoType.SHELLS), this.Parent.Stats.GetAmmo(AmmoType.SHELLS), this.Parent.mGun.ammoType == AmmoType.SHELLS);
			this.Heavy.Draw(spriteBatch, this.Parent.Stats.GetMaxAmmo(AmmoType.HEAVY), this.Parent.Stats.GetAmmo(AmmoType.HEAVY), this.Parent.mGun.ammoType == AmmoType.HEAVY);
			this.Explosive.Draw(spriteBatch, this.Parent.Stats.GetMaxAmmo(AmmoType.EXPLOSIVE), this.Parent.Stats.GetAmmo(AmmoType.EXPLOSIVE), this.Parent.mGun.ammoType == AmmoType.EXPLOSIVE);
			this.Special.Draw(spriteBatch, this.Parent.Stats.GetMaxAmmo(AmmoType.SPECIAL), this.Parent.Stats.GetAmmo(AmmoType.SPECIAL), this.Parent.mGun.ammoType == AmmoType.SPECIAL);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00011814 File Offset: 0x0000FA14
		private void DrawHealthPacks(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.HealthPacks, this.HealthPackLoc, this.Parent.HUDColor);
			spriteBatch.Draw(Global.MasterTexture, this.HealthPackDest, new Rectangle?(this.HealthPackSrc), Color.White);
			Shadow.DrawString(this.Parent.Stats.HealthPacks + "x", Global.StoreFontBig, this.HealthPackStringLoc, 1, Color.White, spriteBatch);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00011898 File Offset: 0x0000FA98
		public void SetGunTex(Gun gun)
		{
			int x = gun.OriginTex.X;
			int y = gun.OriginTex.Y + 2;
			this.Gun.SetPortTex(x, y);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000118CC File Offset: 0x0000FACC
		public void FlashMoney()
		{
			this.Money.Flash(Color.White);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000118DE File Offset: 0x0000FADE
		public void SetControlTip(Texture2D tex)
		{
			this.ControlTip = tex;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x000118E7 File Offset: 0x0000FAE7
		public void ActivateControlTip()
		{
			this.ControlActive = true;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x000118F0 File Offset: 0x0000FAF0
		public void ShowAlarmMessage(string msg, Color col, float time)
		{
			this.Alarm.ShowMessage(msg, col, time);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00011900 File Offset: 0x0000FB00
		public void ShowReady(bool show)
		{
			foreach (MiniHUD miniHUD in this.Minis)
			{
				miniHUD.ShowReady = show;
				miniHUD.Ready = false;
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00011958 File Offset: 0x0000FB58
		public void SetReady(int index)
		{
			foreach (MiniHUD miniHUD in this.Minis)
			{
				if (miniHUD.parent.Index == index)
				{
					miniHUD.Ready = true;
				}
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000119BC File Offset: 0x0000FBBC
		public bool AllReady()
		{
			bool result = true;
			using (List<MiniHUD>.Enumerator enumerator = this.Minis.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Ready)
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00011A14 File Offset: 0x0000FC14
		private void DrawAbilityMeter(SpriteBatch spriteBatch)
		{
			if (this.Parent.Ability == null)
			{
				return;
			}
			Rectangle hudrect = this.HUDRect;
			Rectangle rectangle = new Rectangle(Global.ScreenRect.Width - 256, Global.ScreenRect.Height - 256 + 32, 256, 64);
			spriteBatch.Draw(this.HUDAbility, new Rectangle(rectangle.X + 2, rectangle.Y + 2, 256, 64), new Color(40, 40, 40, 100));
			spriteBatch.Draw(this.HUDAbility, rectangle, this.Parent.HUDColor);
			int num = (int)(this.Parent.Ability.AbilityPercent * 104f);
			Rectangle rectangle2 = new Rectangle(76 + rectangle.X, 18 + rectangle.Y, num, 28);
			if (num > 2)
			{
				spriteBatch.Draw(Global.Pixel, rectangle2, Color.LightGray);
			}
			int num2 = (int)(1f / this.Parent.Ability.PercentPerActivation);
			if (num2 > 1)
			{
				for (int i = 0; i < num2; i++)
				{
					int num3 = (int)(this.Parent.Ability.PercentPerActivation * 52f * 2f) * i;
					if (this.Parent.Ability.PercentPerActivation * (float)i > this.Parent.Ability.AbilityPercent)
					{
						spriteBatch.Draw(Global.Pixel, new Rectangle(rectangle2.X + num3, rectangle2.Y, 2, rectangle2.Height), Color.Black);
					}
					else
					{
						spriteBatch.Draw(Global.Pixel, new Rectangle(rectangle2.X + num3, rectangle2.Y, 2, rectangle2.Height), Color.DarkGray);
					}
				}
			}
		}

		// Token: 0x0400021E RID: 542
		private Player Parent;

		// Token: 0x0400021F RID: 543
		private HealthBar Health;

		// Token: 0x04000220 RID: 544
		private HealthBar Gun;

		// Token: 0x04000221 RID: 545
		private Vector2 HealthPosition;

		// Token: 0x04000222 RID: 546
		private Vector2 GunPosition;

		// Token: 0x04000223 RID: 547
		private DataString Money;

		// Token: 0x04000224 RID: 548
		public static Vector2 MoneyLoc = new Vector2(96f, 64f);

		// Token: 0x04000225 RID: 549
		private HUDAmmoMeter Assault;

		// Token: 0x04000226 RID: 550
		private HUDAmmoMeter Shells;

		// Token: 0x04000227 RID: 551
		private HUDAmmoMeter Heavy;

		// Token: 0x04000228 RID: 552
		private HUDAmmoMeter Explosive;

		// Token: 0x04000229 RID: 553
		private HUDAmmoMeter Special;

		// Token: 0x0400022A RID: 554
		private Texture2D HealthPacks;

		// Token: 0x0400022B RID: 555
		private Rectangle HealthPackSrc;

		// Token: 0x0400022C RID: 556
		private Rectangle HealthPackDest;

		// Token: 0x0400022D RID: 557
		private Vector2 HealthPackStringLoc;

		// Token: 0x0400022E RID: 558
		private Vector2 HealthPackLoc;

		// Token: 0x0400022F RID: 559
		private Texture2D HUDAbility;

		// Token: 0x04000230 RID: 560
		private Rectangle HUDRect;

		// Token: 0x04000231 RID: 561
		private Texture2D ControlTip;

		// Token: 0x04000232 RID: 562
		private bool ControlActive;

		// Token: 0x04000233 RID: 563
		private bool LeftControlTip = true;

		// Token: 0x04000234 RID: 564
		private Timer ControlTipCycleTimer = new Timer(0.25f);

		// Token: 0x04000235 RID: 565
		private AlarmMessage Alarm;

		// Token: 0x04000236 RID: 566
		public WeaponWheel Wheel;

		// Token: 0x04000237 RID: 567
		private List<MiniHUD> Minis;

		// Token: 0x04000238 RID: 568
		public MinionIndicator Minion;
	}
}
