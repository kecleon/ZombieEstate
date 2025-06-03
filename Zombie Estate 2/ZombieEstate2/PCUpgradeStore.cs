using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000C9 RID: 201
	internal class PCUpgradeStore : PCStore
	{
		// Token: 0x06000518 RID: 1304 RVA: 0x00027138 File Offset: 0x00025338
		public PCUpgradeStore(Player player) : base(player, 4, 2, new Rectangle(580, 128, 400, 118), false)
		{
			base.Title = "Inventory";
			this.DrawTitle = false;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0002716C File Offset: 0x0002536C
		public override void AddItems()
		{
			for (int i = 0; i < this.Player.GetGunCount(); i++)
			{
				base.ItemScreen.AddItem(new PCItem(this.Player.Guns[i].stats, this.Player.Guns[i]));
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000271C0 File Offset: 0x000253C0
		public override void AddStoreElements()
		{
			this.UpgradeButton = new PCButton("UPGRADE", Color.LightBlue, new Vector2((float)(base.StoreOffsetX + 780 - PCButton.ButtonTex.Width / 2 - 138), (float)(base.StoreOffsetY + 620)), "Spend one Upgrade Token to increase the strength of the selected gun.");
			this.UpgradeButton.Pressed = new PCButton.PressedDelegate(this.BuyDelegate);
			this.UpgradeButton.Enabled = false;
			base.AddStoreElement(this.UpgradeButton);
			this.SellButton = new PCButton("SELL", Color.LightGreen, new Vector2((float)(base.StoreOffsetX + 780 - PCButton.ButtonTex.Width / 2), (float)(base.StoreOffsetY + 620)), "Sell the selected gun for half of what you paid for it.");
			this.SellButton.Pressed = new PCButton.PressedDelegate(this.SellDelegate);
			this.SellButton.Enabled = false;
			base.AddStoreElement(this.SellButton);
			this.BackButton = new PCButton("EXIT", Color.LightGray, new Vector2((float)(base.StoreOffsetX + 780 - PCButton.ButtonTex.Width / 2 + 138), (float)(base.StoreOffsetY + 620)), "Exit the store.");
			this.BackButton.Pressed = new PCButton.PressedDelegate(this.BackDelegate);
			base.AddStoreElement(this.BackButton);
			base.AddStoreElement(new BobbingPortrait(new Point(10, 6), new Point(9, 6), 12f, new Point(32 + base.StoreOffsetX, 38 + base.StoreOffsetY), 0.6f));
			base.AddStoreElement(new AmmoDisplay(new Vector2((float)(65 + base.StoreOffsetX), (float)(270 + base.StoreOffsetY)), this.Player));
			base.AddStoreElement(new CurrencyDisplay(CurrencyType.Money, new Vector2((float)(40 + base.StoreOffsetX), (float)(465 + base.StoreOffsetY)), this.Player));
			this.upgradeTokens = new CurrencyDisplay(CurrencyType.UpgradeTokens, new Vector2((float)(40 + base.StoreOffsetX), (float)(500 + base.StoreOffsetY)), this.Player);
			base.AddStoreElement(this.upgradeTokens);
			base.AddStoreElement(new CurrencyDisplay(CurrencyType.TalentPoints, new Vector2((float)(40 + base.StoreOffsetX), (float)(535 + base.StoreOffsetY)), this.Player));
			base.AddStoreElement(new CurrencyDisplay(CurrencyType.Points, new Vector2((float)(40 + base.StoreOffsetX), (float)(570 + base.StoreOffsetY)), this.Player));
			base.AddStoreElement(new ItemPortrait(780 + base.StoreOffsetX - 128 - 80, 320 + base.StoreOffsetY, 128));
			base.AddStoreElement(new ItemPortrait(780 + base.StoreOffsetX + 80, 320 + base.StoreOffsetY, 128, true));
			base.AddStoreElement(new ItemDescription(this, 641, 484));
			base.AddStoreElements();
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000274D4 File Offset: 0x000256D4
		private void BuyDelegate()
		{
			if (this.Player.Stats.UpgradeTokens <= 0)
			{
				base.ShowMessage("Not enough Upgrade Tokens!", false, new PCButton.PressedDelegate(base.CloseMessage), null);
				return;
			}
			for (int i = 0; i < this.Player.GetGunCount(); i++)
			{
				if (this.Player.Guns[i].Name == this.CurrentItem.Title && this.Player.Guns[i].GetLevel() < 3)
				{
					this.Player.Guns[i].LevelUpGun();
					PlayerStats stats = this.Player.Stats;
					int num = stats.UpgradeTokens;
					stats.UpgradeTokens = num - 1;
					base.ReInit();
				}
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0002758F File Offset: 0x0002578F
		private void BackDelegate()
		{
			this.Back();
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00024125 File Offset: 0x00022325
		private void OkClicked()
		{
			base.CloseMessage();
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00024051 File Offset: 0x00022251
		private void Back()
		{
			MasterStore.Deactivate();
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00027598 File Offset: 0x00025798
		private void SellDelegate()
		{
			if (this.CurrentItem == null)
			{
				return;
			}
			Gun gun = this.CurrentItem.Tag as Gun;
			if (gun == null)
			{
				Terminal.WriteMessage("ERROR: EXPECTED GUN IN TAG! -- Upgrade", MessageType.ERROR);
				return;
			}
			if (gun.ammoType == AmmoType.INFINITE || gun.ammoType == AmmoType.SPECIAL)
			{
				base.ShowMessage("You can not sell that gun!", false, new PCButton.PressedDelegate(base.CloseMessage), new PCButton.PressedDelegate(base.CloseMessage));
				return;
			}
			base.ShowMessage("Are you sure you want to sell this gun for $" + this.CurrentItem.Cost / 2 + "?", true, new PCButton.PressedDelegate(this.SellConfirmed), new PCButton.PressedDelegate(base.CloseMessage));
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00027645 File Offset: 0x00025845
		private void SellConfirmed()
		{
			base.CloseMessage();
			this.Player.Stats.AddMoney(this.CurrentItem.Cost / 2);
			base.ItemScreen.SelectFirstItem();
			base.ReInit();
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0002767B File Offset: 0x0002587B
		public override void LoadStoreBackground()
		{
			this.StoreBackground = Global.Content.Load<Texture2D>("Store\\PCStore\\UpgradeStore");
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00027694 File Offset: 0x00025894
		public override void AddEmptyItems()
		{
			base.AddEmptyItems();
			bool flag = false;
			Gun[] guns = this.Player.Guns;
			for (int i = 0; i < guns.Length; i++)
			{
				if (guns[i].Name == this.Player.Stats.CharSettings.superWeapon)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				base.ItemScreen.LockLastSpot();
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000276F8 File Offset: 0x000258F8
		public override void SelectionChange(PCItem highlight)
		{
			Gun gun = highlight.Tag as Gun;
			if (gun != null)
			{
				this.upgradeTokens.Highlighted = true;
				if (gun.GetLevel() == 3 || gun.ammoType == AmmoType.SPECIAL)
				{
					this.UpgradeButton.Enabled = false;
				}
				else
				{
					this.UpgradeButton.Enabled = true;
				}
				this.SellButton.Enabled = true;
			}
			else
			{
				this.UpgradeButton.Enabled = false;
				this.SellButton.Enabled = false;
			}
			base.RemoveStoreElement(this.Comp1);
			base.RemoveStoreElement(this.Comp2);
			this.Comp1 = null;
			this.Comp2 = null;
			if (highlight != null && highlight.Tag is Gun)
			{
				Gun gun2 = highlight.Tag as Gun;
				this.Comp1 = new GunStatsComparer(null, this, gun2.stats.SpecialProperties[gun2.GetLevel()], gun2.stats.GunProperties[gun2.GetLevel()], new Rectangle(base.StoreOffsetX + 300, base.StoreOffsetY + 120, 256, 500), null, null);
				base.AddStoreElement(this.Comp1);
				if (gun2.GetLevel() <= 2 && gun2.ammoType != AmmoType.SPECIAL)
				{
					this.Comp2 = new GunStatsComparer(null, this, gun2.stats.SpecialProperties[gun2.GetLevel()], gun2.stats.GunProperties[gun2.GetLevel()], new Rectangle(base.StoreOffsetX + 1000, base.StoreOffsetY + 120, 256, 500), gun2.stats.SpecialProperties[gun2.GetLevel() + 1], gun2.stats.GunProperties[gun2.GetLevel() + 1]);
					base.AddStoreElement(this.Comp2);
				}
			}
			base.SelectionChange(highlight);
		}

		// Token: 0x04000535 RID: 1333
		private PCButton UpgradeButton;

		// Token: 0x04000536 RID: 1334
		private PCButton BackButton;

		// Token: 0x04000537 RID: 1335
		private PCButton SellButton;

		// Token: 0x04000538 RID: 1336
		private CurrencyDisplay upgradeTokens;

		// Token: 0x04000539 RID: 1337
		private GunStatsComparer Comp1;

		// Token: 0x0400053A RID: 1338
		private GunStatsComparer Comp2;
	}
}
