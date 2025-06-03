using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000C2 RID: 194
	internal class PCGunStore : PCStore
	{
		// Token: 0x060004CE RID: 1230 RVA: 0x00023578 File Offset: 0x00021778
		public PCGunStore(Player player) : base(player, 20, 5, new Rectangle(292, 315, 465, 380), true)
		{
			base.Title = "Gun Shop";
			this.DrawTitle = false;
			this.TryToGiveSuperWeapon();
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00023600 File Offset: 0x00021800
		public override void AddItems()
		{
			if (this.ActiveButton == null)
			{
				return;
			}
			if (this.ActiveButton.Misc)
			{
				base.ItemScreen.AddItem(new PCItem(new Point(0, 43), "Assault Ammo", "Contains " + this.ammoAssaultCount + " Assault Ammo. Assault Ammo is often used for medium fire rate weapons.", this.ammoAssaultCost, "+" + this.ammoAssaultCount.ToString()));
				base.ItemScreen.AddItem(new PCItem(new Point(0, 45), "Shells", "Contains " + this.ammoShellsCount + " Shells. Shells are often used for shotguns and low fire rate weapons.", this.ammoShellsCost, "+" + this.ammoShellsCount.ToString()));
				base.ItemScreen.AddItem(new PCItem(new Point(0, 44), "Heavy Ammo", "Contains " + this.ammoHeavyCount + " Heavy Ammo. Heavy Ammo is used for light damage, but high rate of fire weapons.", this.ammoHeavyCost, "+" + this.ammoHeavyCount.ToString()));
				base.ItemScreen.AddItem(new PCItem(new Point(0, 46), "Explosive Ammo", "Contains " + this.ammoExplosiveCount + " Explosive Ammo. Explosive Ammo is used for heavy damage, low rate of fire weapons.", this.ammoExplosiveCost, "+" + this.ammoExplosiveCount.ToString()));
				base.ItemScreen.AddItem(this.GetAmmoBoxItem());
				base.ItemScreen.AddItem(new PCItem(new Point(0, 42), "Health Pack", "Buy one Health Pack to refill your health by 25.", 300, "+1"));
				return;
			}
			for (int i = 0; i < GunStatsLoader.GunStatsList.Count; i++)
			{
				if (this.PassesFilter(GunStatsLoader.GunStatsList[i]))
				{
					if (this.Player.OwnsGun(GunStatsLoader.GunStatsList[i]))
					{
						base.ItemScreen.AddItem(new PCItem(GunStatsLoader.GunStatsList[i], this.Player, "Own"));
					}
					else
					{
						base.ItemScreen.AddItem(new PCItem(GunStatsLoader.GunStatsList[i], this.Player));
					}
				}
			}
			base.AddItems();
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00023840 File Offset: 0x00021A40
		public override void AddStoreElements()
		{
			base.AddStoreElement(new ItemPortrait(802 + base.StoreOffsetX, 124 + base.StoreOffsetY, 96));
			base.AddStoreElement(new ItemDescription(this));
			this.BuyButton = new PCButton("BUY", Color.LightGreen, new Vector2((float)(base.StoreOffsetX + 1120), (float)(base.StoreOffsetY + 550)), "Purchase the selected weapon for the cost of the gun.");
			this.BuyButton.Pressed = new PCButton.PressedDelegate(this.BuyDelegate);
			base.AddStoreElement(this.BuyButton);
			this.BuyButton.PlaysSound = false;
			this.BuyButton.Enabled = false;
			this.BackButton = new PCButton("EXIT", Color.LightGray, new Vector2((float)(base.StoreOffsetX + 1120), (float)(base.StoreOffsetY + 620)), "Exit the store.");
			this.BackButton.Pressed = new PCButton.PressedDelegate(this.BackDelegate);
			base.AddStoreElement(this.BackButton);
			base.AddStoreElement(new ItemCost(1088 + base.StoreOffsetX, 400 + base.StoreOffsetY));
			base.AddStoreElement(new AmmoTypeDisplay(new Vector2((float)(1158 + base.StoreOffsetX), (float)(280 + base.StoreOffsetY))));
			base.AddStoreElement(new AmmoDisplay(new Vector2((float)(65 + base.StoreOffsetX), (float)(270 + base.StoreOffsetY)), this.Player));
			this.money = new CurrencyDisplay(CurrencyType.Money, new Vector2((float)(40 + base.StoreOffsetX), (float)(465 + base.StoreOffsetY)), this.Player);
			base.AddStoreElement(this.money);
			base.AddStoreElement(new CurrencyDisplay(CurrencyType.UpgradeTokens, new Vector2((float)(40 + base.StoreOffsetX), (float)(500 + base.StoreOffsetY)), this.Player));
			base.AddStoreElement(new CurrencyDisplay(CurrencyType.TalentPoints, new Vector2((float)(40 + base.StoreOffsetX), (float)(535 + base.StoreOffsetY)), this.Player));
			this.points = new CurrencyDisplay(CurrencyType.Points, new Vector2((float)(40 + base.StoreOffsetX), (float)(570 + base.StoreOffsetY)), this.Player);
			base.AddStoreElement(this.points);
			base.AddStoreElement(new BobbingPortrait(new Point(10, 6), new Point(9, 6), 12f, new Point(32 + base.StoreOffsetX, 38 + base.StoreOffsetY), 0.6f));
			this.AmmoButtons = new List<PCAmmoButtons>();
			this.AmmoButtons.Add(new PCAmmoButtons(AmmoType.ASSAULT, 330 + base.StoreOffsetX, 128 + base.StoreOffsetY, this));
			this.AmmoButtons.Add(new PCAmmoButtons(AmmoType.SHELLS, 330 + base.StoreOffsetX + 128, 128 + base.StoreOffsetY, this));
			this.AmmoButtons.Add(new PCAmmoButtons(AmmoType.HEAVY, 330 + base.StoreOffsetX + 256, 128 + base.StoreOffsetY, this));
			this.AmmoButtons.Add(new PCAmmoButtons(AmmoType.EXPLOSIVE, 330 + base.StoreOffsetX + 64, 128 + base.StoreOffsetY + 80, this));
			this.AmmoButtons.Add(new PCAmmoButtons(AmmoType.MELEE, 330 + base.StoreOffsetX + 128 + 64, 128 + base.StoreOffsetY + 80, this));
			this.AmmoButtons.Add(new PCAmmoButtons(330 + base.StoreOffsetX + 256 + 64, 128 + base.StoreOffsetY + 80, this));
			foreach (PCAmmoButtons element in this.AmmoButtons)
			{
				base.AddStoreElement(element);
			}
			base.AddStoreElements();
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00023C50 File Offset: 0x00021E50
		private void BuyDelegate()
		{
			if (this.CurrentItem == null)
			{
				return;
			}
			if (this.Player.Stats.GetMoney() < this.CurrentItem.Cost)
			{
				SoundEngine.PlaySound("ze2_death", 0.5f);
				base.ShowMessage("You don't have enough money!", false, new PCButton.PressedDelegate(this.OkClicked), null);
				return;
			}
			bool flag = false;
			if (this.CurrentItem.Title == "Assault Ammo")
			{
				if (this.Player.Stats.GetAmmo(AmmoType.ASSAULT) == this.Player.Stats.GetMaxAmmo(AmmoType.ASSAULT))
				{
					base.ShowMessage("You can not buy more of this ammo type!", false, new PCButton.PressedDelegate(this.OkClicked), null);
					return;
				}
				this.Player.Stats.GiveAmmo(AmmoType.ASSAULT, this.ammoAssaultCount);
				flag = true;
			}
			if (this.CurrentItem.Title == "Shells")
			{
				if (this.Player.Stats.GetAmmo(AmmoType.SHELLS) == this.Player.Stats.GetMaxAmmo(AmmoType.SHELLS))
				{
					base.ShowMessage("You can not buy more of this ammo type!", false, new PCButton.PressedDelegate(this.OkClicked), null);
					return;
				}
				this.Player.Stats.GiveAmmo(AmmoType.SHELLS, this.ammoShellsCount);
				flag = true;
			}
			if (this.CurrentItem.Title == "Heavy Ammo")
			{
				if (this.Player.Stats.GetAmmo(AmmoType.HEAVY) == this.Player.Stats.GetMaxAmmo(AmmoType.HEAVY))
				{
					base.ShowMessage("You can not buy more of this ammo type!", false, new PCButton.PressedDelegate(this.OkClicked), null);
					return;
				}
				this.Player.Stats.GiveAmmo(AmmoType.HEAVY, this.ammoHeavyCount);
				flag = true;
			}
			if (this.CurrentItem.Title == "Explosive Ammo")
			{
				if (this.Player.Stats.GetAmmo(AmmoType.EXPLOSIVE) == this.Player.Stats.GetMaxAmmo(AmmoType.EXPLOSIVE))
				{
					base.ShowMessage("You can not buy more of this ammo type!", false, new PCButton.PressedDelegate(this.OkClicked), null);
					return;
				}
				this.Player.Stats.GiveAmmo(AmmoType.EXPLOSIVE, this.ammoExplosiveCount);
				flag = true;
			}
			if (this.CurrentItem.Title == "Health Pack")
			{
				if (this.Player.Stats.HealthPacks >= 3)
				{
					base.ShowMessage("You can not buy any more Health Packs!", false, new PCButton.PressedDelegate(this.OkClicked), null);
					return;
				}
				PlayerStats stats = this.Player.Stats;
				int i = stats.HealthPacks;
				stats.HealthPacks = i + 1;
				flag = true;
			}
			if (this.CurrentItem.Title == "Ammo Box")
			{
				if (this.CurrentItem.Cost == 0)
				{
					base.ShowMessage("You are full on ammo!", false, new PCButton.PressedDelegate(this.OkClicked), null);
				}
				this.Player.Stats.GiveAmmo(AmmoType.ASSAULT, 100000);
				this.Player.Stats.GiveAmmo(AmmoType.SHELLS, 100000);
				this.Player.Stats.GiveAmmo(AmmoType.HEAVY, 100000);
				this.Player.Stats.GiveAmmo(AmmoType.EXPLOSIVE, 100000);
				base.ReInit();
				flag = true;
			}
			if (!flag)
			{
				int num = Player.MAXGUNS - 1;
				if (this.Player.OwnsGun(this.Player.Stats.CharSettings.superWeapon))
				{
					num++;
				}
				if (this.Player.GetGunCount() >= num)
				{
					base.ShowMessage("Inventory full! Sell your guns on the Inventory screen!", false, new PCButton.PressedDelegate(this.OkClicked), null);
					return;
				}
				Gun[] guns = this.Player.Guns;
				for (int i = 0; i < guns.Length; i++)
				{
					if (guns[i].Name == this.CurrentItem.Title)
					{
						base.ShowMessage("You already own this gun!", false, new PCButton.PressedDelegate(this.OkClicked), null);
						return;
					}
				}
			}
			SoundEngine.PlaySound("ze2_money", 0.7f);
			this.Player.Stats.AddMoney(-this.CurrentItem.Cost);
			base.ReInit();
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00024049 File Offset: 0x00022249
		private void BackDelegate()
		{
			this.Back();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00024051 File Offset: 0x00022251
		private void Back()
		{
			MasterStore.Deactivate();
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00024058 File Offset: 0x00022258
		private void UnlockDelegate()
		{
			if (this.CurrentItem == null)
			{
				return;
			}
			if (this.Player.Points < this.CurrentItem.PointsToUnlock)
			{
				base.ShowMessage("You don't have enough Zombie Heads to unlock this weapon!", false, new PCButton.PressedDelegate(this.OkClicked), null);
				return;
			}
			base.ShowMessage("Are you sure you want to spend Zombie Heads to unlock this weapon?", true, new PCButton.PressedDelegate(this.UnlockGo), new PCButton.PressedDelegate(this.OkClicked));
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x000240C4 File Offset: 0x000222C4
		private void UnlockGo()
		{
			this.Player.Points = this.Player.Points - this.CurrentItem.PointsToUnlock;
			this.Player.UnlockedGuns.Add(this.CurrentItem.Title);
			this.Player.SaveAllPlayerData();
			base.ReInit();
			base.CloseMessage();
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00024125 File Offset: 0x00022325
		private void OkClicked()
		{
			base.CloseMessage();
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0002412D File Offset: 0x0002232D
		public override void LoadStoreBackground()
		{
			this.StoreBackground = Global.Content.Load<Texture2D>("Store\\PCStore\\GunStore");
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00024144 File Offset: 0x00022344
		public override void SelectionChange(PCItem highlight)
		{
			if (highlight.Locked)
			{
				this.BuyButton.Enabled = false;
				this.points.Highlighted = true;
				this.money.Highlighted = false;
			}
			else
			{
				this.BuyButton.Enabled = true;
				this.points.Highlighted = false;
				this.money.Highlighted = true;
			}
			if (highlight != null && highlight.Tag is GunStats)
			{
				this.Comp = new GunStatsComparer(null, this, (highlight.Tag as GunStats).SpecialProperties[0], (highlight.Tag as GunStats).GunProperties[0], new Rectangle(base.StoreOffsetX + 802, base.StoreOffsetY + 260, 256, 421), null, null);
				base.AddStoreElement(this.Comp);
			}
			else
			{
				base.RemoveStoreElement(this.Comp);
				this.Comp = null;
			}
			base.SelectionChange(highlight);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00024240 File Offset: 0x00022440
		public PCItem GetAmmoBoxItem()
		{
			int num = 0;
			int ammo = this.Player.Stats.GetAmmo(AmmoType.ASSAULT);
			int maxAmmo = this.Player.Stats.GetMaxAmmo(AmmoType.ASSAULT);
			int ammo2 = this.Player.Stats.GetAmmo(AmmoType.HEAVY);
			int maxAmmo2 = this.Player.Stats.GetMaxAmmo(AmmoType.HEAVY);
			int ammo3 = this.Player.Stats.GetAmmo(AmmoType.SHELLS);
			int maxAmmo3 = this.Player.Stats.GetMaxAmmo(AmmoType.SHELLS);
			int ammo4 = this.Player.Stats.GetAmmo(AmmoType.EXPLOSIVE);
			int maxAmmo4 = this.Player.Stats.GetMaxAmmo(AmmoType.EXPLOSIVE);
			num += (int)((float)(maxAmmo - ammo) / (float)this.ammoAssaultCount * (float)this.ammoAssaultCost);
			num += (int)((float)(maxAmmo2 - ammo2) / (float)this.ammoHeavyCount * (float)this.ammoHeavyCost);
			num += (int)((float)(maxAmmo3 - ammo3) / (float)this.ammoShellsCount * (float)this.ammoShellsCost);
			num += (int)((float)(maxAmmo4 - ammo4) / (float)this.ammoExplosiveCount * (float)this.ammoExplosiveCost);
			return new PCItem(new Point(4, 47), "Ammo Box", "Purchase this box to refill all your ammo types except Special.", num);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00024368 File Offset: 0x00022568
		private bool PassesFilter(GunStats gun)
		{
			return this.ActiveButton != null && !this.ActiveButton.Misc && gun.Cost != -1 && gun.AmmoType == this.ActiveButton.MyType && gun.AmmoType != AmmoType.INFINITE && gun.AmmoType != AmmoType.SPECIAL;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void TryToGiveSuperWeapon()
		{
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000243C5 File Offset: 0x000225C5
		public override void DrawMisc(SpriteBatch spriteBatch)
		{
			base.DrawMisc(spriteBatch);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x000243D0 File Offset: 0x000225D0
		public void FireNewScreen(PCAmmoButtons active)
		{
			foreach (PCAmmoButtons pcammoButtons in this.AmmoButtons)
			{
				pcammoButtons.Active = false;
			}
			this.ActiveButton = active;
			base.ReInit();
		}

		// Token: 0x040004EF RID: 1263
		private PCButton BuyButton;

		// Token: 0x040004F0 RID: 1264
		private PCButton BackButton;

		// Token: 0x040004F1 RID: 1265
		private int ammoAssaultCost = 25;

		// Token: 0x040004F2 RID: 1266
		private int ammoShellsCost = 35;

		// Token: 0x040004F3 RID: 1267
		private int ammoHeavyCost = 40;

		// Token: 0x040004F4 RID: 1268
		private int ammoExplosiveCost = 100;

		// Token: 0x040004F5 RID: 1269
		private int ammoAssaultCount = 50;

		// Token: 0x040004F6 RID: 1270
		private int ammoShellsCount = 12;

		// Token: 0x040004F7 RID: 1271
		private int ammoHeavyCount = 50;

		// Token: 0x040004F8 RID: 1272
		private int ammoExplosiveCount = 4;

		// Token: 0x040004F9 RID: 1273
		private CurrencyDisplay money;

		// Token: 0x040004FA RID: 1274
		private CurrencyDisplay points;

		// Token: 0x040004FB RID: 1275
		private GunStatsComparer Comp;

		// Token: 0x040004FC RID: 1276
		private List<PCAmmoButtons> AmmoButtons;

		// Token: 0x040004FD RID: 1277
		private PCAmmoButtons ActiveButton;
	}
}
