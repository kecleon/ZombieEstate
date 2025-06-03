using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000C3 RID: 195
	internal class PCHatStore : PCStore
	{
		// Token: 0x060004DE RID: 1246 RVA: 0x00024430 File Offset: 0x00022630
		public PCHatStore(Player player) : base(player, 10, 4)
		{
			base.Title = "Hats";
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00024448 File Offset: 0x00022648
		public override void AddItems()
		{
			this.InitHats();
			base.ItemScreen.AddItem(new PCItem(new Point(62, 63), "None", "", 0, "None"));
			foreach (Hat hat in this.Hats)
			{
				PCItem pcitem = new PCItem(hat);
				if (!hat.Unlocked)
				{
					pcitem.Lock();
				}
				pcitem.PointsToUnlock = hat.ZHCost;
				base.ItemScreen.AddItem(pcitem);
			}
			base.AddItems();
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000244F8 File Offset: 0x000226F8
		public override void AddStoreElements()
		{
			base.AddStoreElement(new ItemPortrait(372 + base.StoreOffsetX, 512 + base.StoreOffsetY, 128));
			this.UnlockButton = new PCButton("UNLOCK", Color.LightBlue, new Vector2((float)(base.StoreOffsetX + 1058), (float)(base.StoreOffsetY + 480)), "Unlock this hat permanently by spending Zombie Heads.");
			this.UnlockButton.Pressed = new PCButton.PressedDelegate(this.UnlockDelegate);
			base.AddStoreElement(this.UnlockButton);
			this.UnlockButton.Enabled = false;
			this.EquipButton = new PCButton("EQUIP", Color.LightGreen, new Vector2((float)(base.StoreOffsetX + 1058), (float)(base.StoreOffsetY + 550)), "Equip this hat on your character.");
			this.EquipButton.Pressed = new PCButton.PressedDelegate(this.EquipDelegate);
			base.AddStoreElement(this.EquipButton);
			this.EquipButton.Enabled = false;
			base.AddStoreElement(new ItemCost(864 + base.StoreOffsetX, 484 + base.StoreOffsetY, true));
			this.points = new CurrencyDisplay(CurrencyType.Points, new Vector2((float)(40 + base.StoreOffsetX), (float)(570 + base.StoreOffsetY)), this.Player);
			base.AddStoreElement(this.points);
			this.BackButton = new PCButton("CANCEL", Color.LightGray, new Vector2((float)(base.StoreOffsetX + 1058), (float)(base.StoreOffsetY + 620)), "Exit the store, equipping no hat.");
			this.BackButton.Pressed = new PCButton.PressedDelegate(this.Cancel);
			base.AddStoreElement(this.BackButton);
			base.AddStoreElements();
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000246C0 File Offset: 0x000228C0
		private void UnlockDelegate()
		{
			if (this.CurrentItem == null)
			{
				return;
			}
			if (this.Player.Points < this.CurrentItem.PointsToUnlock)
			{
				base.ShowMessage("You don't have enough Zombie Heads to unlock this hat!", false, new PCButton.PressedDelegate(base.CloseMessage), null);
				return;
			}
			base.ShowMessage("Are you sure you want to spend Zombie Heads to unlock this hat?", true, new PCButton.PressedDelegate(this.UnlockGo), new PCButton.PressedDelegate(base.CloseMessage));
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0002472C File Offset: 0x0002292C
		private void UnlockGo()
		{
			this.Player.Points = this.Player.Points - this.CurrentItem.PointsToUnlock;
			this.Player.UnlockedHats.Add(this.CurrentItem.Title);
			this.Player.SaveAllPlayerData();
			base.ReInit();
			base.CloseMessage();
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00024790 File Offset: 0x00022990
		private void EquipDelegate()
		{
			if (this.CurrentItem.Title == "None")
			{
				this.Player.AccessoryName = "None";
				this.Player.AccessoryTexCoord = new Point(63, 63);
				this.BackDelegate();
				return;
			}
			Hat hat = (Hat)this.CurrentItem.Tag;
			this.Player.AccessoryTexCoord = hat.Tex;
			this.Player.AccessoryName = hat.Name;
			this.BackDelegate();
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00024818 File Offset: 0x00022A18
		public override void SelectionChange(PCItem highlight)
		{
			if (highlight.Locked)
			{
				this.points.Highlighted = true;
			}
			else
			{
				this.points.Highlighted = false;
			}
			if (highlight.Locked)
			{
				this.UnlockButton.Enabled = true;
				this.EquipButton.Enabled = false;
			}
			else
			{
				this.UnlockButton.Enabled = false;
				this.EquipButton.Enabled = true;
			}
			base.SelectionChange(highlight);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void BackDelegate()
		{
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00024888 File Offset: 0x00022A88
		private void Cancel()
		{
			this.Player.AccessoryName = "None";
			this.Player.AccessoryTexCoord = new Point(63, 63);
			this.BackDelegate();
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000248B4 File Offset: 0x00022AB4
		public override void LoadStoreBackground()
		{
			this.StoreBackground = Global.Content.Load<Texture2D>("Store\\PCStore\\HatStore");
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000248CC File Offset: 0x00022ACC
		private void InitHats()
		{
			if (this.Hats == null)
			{
				this.Hats = PCHatStore.GetHatList();
				PCHatStore.GlobalHatList = this.Hats;
			}
			for (int i = 0; i < this.Hats.Count; i++)
			{
				if (this.Player.IsHatUnlocked(this.Hats[i].Name))
				{
					Hat value = default(Hat);
					value.Name = this.Hats[i].Name;
					value.Unlocked = true;
					value.ZHCost = this.Hats[i].ZHCost;
					value.Tex = this.Hats[i].Tex;
					this.Hats[i] = value;
				}
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00024998 File Offset: 0x00022B98
		public static List<Hat> GetHatList()
		{
			List<Hat> list = new List<Hat>();
			list.Add(new Hat
			{
				Name = "None",
				Tex = new Point(62, 63),
				Unlocked = true,
				ZHCost = 0
			});
			Hat hat = default(Hat);
			hat.Name = "Beanie";
			hat.Tex = new Point(6, 59);
			hat.Unlocked = false;
			hat.ZHCost = 5;
			hat.Properties = new SpecialProperties();
			hat.Properties.MaxHealth = 15f;
			hat.Properties.Armor = 5;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,672]+15 Health");
			hat.PropDesc.Add("[White][1104,688]+5 Armor");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Top Hat";
			hat.Tex = new Point(7, 59);
			hat.Unlocked = false;
			hat.ZHCost = 10;
			hat.Properties = new SpecialProperties();
			hat.Properties.MaxHealth = 15f;
			hat.Properties.CritChance = 2f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,672]+15 Health");
			hat.PropDesc.Add("[White][1136,672]+2% Crit Chance");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Blue Party Hat";
			hat.Tex = new Point(8, 59);
			hat.Unlocked = false;
			hat.ZHCost = 10;
			hat.Properties = new SpecialProperties();
			hat.Properties.WaterResist = 15;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1120,752]+15 Water Resist");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Red Party Hat";
			hat.Tex = new Point(10, 59);
			hat.Unlocked = false;
			hat.ZHCost = 10;
			hat.Properties = new SpecialProperties();
			hat.Properties.FireResist = 15;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,752]+15 Fire Resist");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Green Party Hat";
			hat.Tex = new Point(11, 59);
			hat.Unlocked = false;
			hat.ZHCost = 10;
			hat.Properties = new SpecialProperties();
			hat.Properties.EarthResist = 15;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1104,752]+15 Earth Resist");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Yellow Party Hat";
			hat.Tex = new Point(12, 59);
			hat.Unlocked = false;
			hat.ZHCost = 10;
			hat.Properties = new SpecialProperties();
			hat.Properties.MaxHealth = 20f;
			hat.Properties.Armor = -10;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,672]+20 Health");
			hat.PropDesc.Add("[Red][1104,688]-10 Armor");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Purple Party Hat";
			hat.Tex = new Point(9, 59);
			hat.Unlocked = false;
			hat.ZHCost = 10;
			hat.Properties = new SpecialProperties();
			hat.Properties.MaxHealth = -20f;
			hat.Properties.Armor = 10;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1104,688]+10 Armor");
			hat.PropDesc.Add("[Red][1088,672]-20 Health");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Red Antenna";
			hat.Tex = new Point(13, 59);
			hat.Unlocked = false;
			hat.ZHCost = 10;
			hat.Properties = new SpecialProperties();
			hat.Properties.MinionDmgMod = 10f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1120,704]+10% Minion Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Blue Antenna";
			hat.Tex = new Point(14, 59);
			hat.Unlocked = false;
			hat.ZHCost = 10;
			hat.Properties = new SpecialProperties();
			hat.Properties.MinionFireRateMod = 10f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1120,720]+10% Minion Att Spd");
			list.Add(hat);
			hat = default(Hat);
			hat.Properties = new SpecialProperties();
			hat.Properties.WaterDmg = 5f;
			hat.Properties.WaterResist = 5;
			hat.Name = "Wizard Cap";
			hat.Tex = new Point(16, 59);
			hat.Unlocked = false;
			hat.ZHCost = 10;
			hat.Properties = new SpecialProperties();
			hat.Properties.WaterDmg = 20f;
			hat.Properties.MinionDmgMod = -10f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1120,736]+20% Water Damage");
			hat.PropDesc.Add("[Red][1120,704]-10% Minion Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Cowboy's Hat";
			hat.Tex = new Point(17, 59);
			hat.Unlocked = false;
			hat.ZHCost = 20;
			hat.Properties = new SpecialProperties();
			hat.Properties.ShotTimeMod = 8f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,720]+8% Shot Speed");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Army Helmet";
			hat.Tex = new Point(75, 42);
			hat.Unlocked = false;
			hat.ZHCost = 30;
			hat.Properties = new SpecialProperties();
			hat.Properties.Ammo_Assault = 40;
			hat.Properties.Ammo_Shells = 20;
			hat.Properties.Ammo_Heavy = 80;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1136,704]+40 Assault Ammo");
			hat.PropDesc.Add("[White][1136,720]+20 Shells");
			hat.PropDesc.Add("[White][1136,736]+80 Heavy Ammo");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Nurse Hat";
			hat.Tex = new Point(79, 42);
			hat.Unlocked = false;
			hat.ZHCost = 40;
			hat.Properties = new SpecialProperties();
			hat.Properties.HealingDoneMod = 10f;
			hat.Properties.ShotTimeMod = -8f;
			hat.Properties.SwingTimeMod = -8f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1120,672]+10% Heal Bonus");
			hat.PropDesc.Add("[Red][1088,720]-8% Shot Speed");
			hat.PropDesc.Add("[Red][1104,720]-8% Swing Speed");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Black Cowboy's Hat";
			hat.Tex = new Point(18, 59);
			hat.Unlocked = false;
			hat.ZHCost = 50;
			hat.Properties = new SpecialProperties();
			hat.Properties.BulletDamageMod = 10f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,704]+10% Gun Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Arrow Cowboy's Hat";
			hat.Tex = new Point(19, 59);
			hat.Unlocked = false;
			hat.ZHCost = 50;
			hat.Properties = new SpecialProperties();
			hat.Properties.BulletDamageMod = 20f;
			hat.Properties.ReloadTimeMod = -10f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,704]+20% Gun Damage");
			hat.PropDesc.Add("[Red][1088,688]-10% Reload Speed");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Witches Hat";
			hat.Tex = new Point(21, 59);
			hat.Unlocked = false;
			hat.ZHCost = 60;
			hat.Properties = new SpecialProperties();
			hat.Properties.EarthDmg = 15f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1104,736]+15% Earth Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Fireman Hat";
			hat.Tex = new Point(82, 42);
			hat.Unlocked = false;
			hat.ZHCost = 70;
			hat.Properties = new SpecialProperties();
			hat.Properties.BulletDamageMod = -10f;
			hat.Properties.MeleeDamageMod = -10f;
			hat.Properties.FireDmg = 35f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,736]+35% Fire Damage");
			hat.PropDesc.Add("[Red][1088,704]-10% Gun Damage");
			hat.PropDesc.Add("[Red][1104,704]-10% Melee Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Cat Ears";
			hat.Tex = new Point(73, 42);
			hat.Unlocked = false;
			hat.ZHCost = 80;
			hat.Properties = new SpecialProperties();
			hat.Properties.SwingTimeMod = 15f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1104,720]+15% Swing Speed");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Black Cat Ears";
			hat.Tex = new Point(74, 42);
			hat.Unlocked = false;
			hat.ZHCost = 80;
			hat.Properties = new SpecialProperties();
			hat.Properties.MeleeDamageMod = 25f;
			hat.Properties.SwingTimeMod = -10f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1104,704]+25% Melee Damage");
			hat.PropDesc.Add("[Red][1104,720]-10% Swing Speed");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Mining Helmet";
			hat.Tex = new Point(76, 42);
			hat.Unlocked = false;
			hat.ZHCost = 90;
			hat.Properties = new SpecialProperties();
			hat.Properties.Ammo_Explosive = 8;
			hat.Properties.ExplosionDamageMod = 25f;
			hat.Properties.BulletDamageMod = -20f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1136,752]+8 Explosive Ammo");
			hat.PropDesc.Add("[White][1136,688]+25% Explosive Damage");
			hat.PropDesc.Add("[Red][1088,704]-20% Gun Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Blue Beanie";
			hat.Tex = new Point(15, 59);
			hat.Unlocked = false;
			hat.ZHCost = 100;
			hat.Properties = new SpecialProperties();
			hat.Properties.MaxHealth = 50f;
			hat.Properties.Armor = 10;
			hat.Properties.BulletDamageMod = -10f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,672]+50 Health");
			hat.PropDesc.Add("[White][1104,688]+10 Armor");
			hat.PropDesc.Add("[Red][1088,704]-10% Gun Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Biker Helmet";
			hat.Tex = new Point(81, 42);
			hat.Unlocked = false;
			hat.ZHCost = 100;
			hat.Properties = new SpecialProperties();
			hat.Properties.Armor = 10;
			hat.Properties.BulletDamageMod = -40f;
			hat.Properties.MaxHealth = 100f;
			hat.Properties.HealingDoneMod = -25f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,672]+100 Health");
			hat.PropDesc.Add("[White][1104,688]+10 Armor");
			hat.PropDesc.Add("[Red][1120,672]-25% Heal Bonus");
			hat.PropDesc.Add("[Red][1088,704]-40% Gun Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Green Baseball Cap";
			hat.Tex = new Point(20, 59);
			hat.Unlocked = false;
			hat.ZHCost = 100;
			hat.Properties = new SpecialProperties();
			hat.Properties.MaxHealth = -20f;
			hat.Properties.EarthDmg = 25f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1104,736]+25 Earth Damage");
			hat.PropDesc.Add("[Red][1088,672]-20 Health");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Blue Baseball Cap";
			hat.Tex = new Point(22, 59);
			hat.Unlocked = false;
			hat.ZHCost = 100;
			hat.Properties = new SpecialProperties();
			hat.Properties.Armor = -10;
			hat.Properties.WaterDmg = 25f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1120,736]+25% Water Damage");
			hat.PropDesc.Add("[Red][1104,688]-10 Armor");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Red Baseball Cap";
			hat.Tex = new Point(23, 59);
			hat.Unlocked = false;
			hat.ZHCost = 100;
			hat.Properties = new SpecialProperties();
			hat.Properties.ReloadTimeMod = -10f;
			hat.Properties.FireDmg = 25f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,736]+25% Fire Damage");
			hat.PropDesc.Add("[Red][1088,688]-10% Reload Speed");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Prestigious Top Hat";
			hat.Tex = new Point(77, 42);
			hat.Unlocked = false;
			hat.ZHCost = 120;
			hat.Properties = new SpecialProperties();
			hat.Properties.CritChance = 4f;
			hat.Properties.Armor = 4;
			hat.Properties.MaxHealth = 10f;
			hat.Properties.BulletDamageMod = -8f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1136,672]+4% Crit Chance");
			hat.PropDesc.Add("[White][1104,688]+4 Armor");
			hat.PropDesc.Add("[White][1088,672]+10 Health");
			hat.PropDesc.Add("[Red][1088,704]-8% Gun Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Deluxe Nurse Hat";
			hat.Tex = new Point(80, 42);
			hat.Unlocked = false;
			hat.ZHCost = 120;
			hat.Properties = new SpecialProperties();
			hat.Properties.HealingDoneMod = 25f;
			hat.Properties.MeleeDamageMod = -30f;
			hat.Properties.BulletDamageMod = -20f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1120,672]+25% Heal Bonus");
			hat.PropDesc.Add("[Red][1104,704]-30% Melee Damage");
			hat.PropDesc.Add("[Red][1088,704]-20% Gun Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Crown";
			hat.Tex = new Point(78, 42);
			hat.Unlocked = false;
			hat.ZHCost = 120;
			hat.Properties = new SpecialProperties();
			hat.Properties.Armor = 10;
			hat.Properties.CritChance = 1f;
			hat.Properties.BulletDamageMod = -40f;
			hat.Properties.MeleeDamageMod = 30f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1136,672]+1% Crit Chance");
			hat.PropDesc.Add("[White][1104,704]+30% Melee Damage");
			hat.PropDesc.Add("[White][1104,688]+10 Armor");
			hat.PropDesc.Add("[Red][1088,704]-40% Gun Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Super Antenna";
			hat.Tex = new Point(83, 42);
			hat.Unlocked = false;
			hat.ZHCost = 120;
			hat.Properties = new SpecialProperties();
			hat.Properties.MinionDmgMod = 40f;
			hat.Properties.MinionFireRateMod = 15f;
			hat.Properties.HealingDoneMod = -45f;
			hat.Properties.MeleeDamageMod = -30f;
			hat.Properties.BulletDamageMod = -20f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1120,704]+40% Minion Damage");
			hat.PropDesc.Add("[White][1120,720]+15% Minion Att Spd");
			hat.PropDesc.Add("[Red][1120,672]-45% Heal Bonus");
			hat.PropDesc.Add("[Red][1104,704]-30% Melee Damage");
			hat.PropDesc.Add("[Red][1088,704]-20% Gun Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Jester Hat";
			hat.Tex = new Point(100, 60);
			hat.Unlocked = false;
			hat.ZHCost = 130;
			hat.Properties = new SpecialProperties();
			hat.Properties.CritChance = 15f;
			hat.Properties.ReloadTimeMod = -100f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1136,672]+15% Crit Chance");
			hat.PropDesc.Add("[Red][1088,688]-100% Reload Speed");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Devil Horns";
			hat.Tex = new Point(84, 42);
			hat.Unlocked = false;
			hat.ZHCost = 140;
			hat.Properties = new SpecialProperties();
			hat.Properties.BulletDamageMod = 50f;
			hat.Properties.ExplosionDamageMod = -50f;
			hat.Properties.MeleeDamageMod = -50f;
			hat.Properties.ShotTimeMod = -80f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1088,704]+50% Gun Damage");
			hat.PropDesc.Add("[Red][1136,688]-50% Explosive Damage");
			hat.PropDesc.Add("[Red][1088,720]-80% Shot Speed");
			hat.PropDesc.Add("[Red][1104,704]-50% Melee Damage");
			list.Add(hat);
			hat = default(Hat);
			hat.Name = "Silver Crown";
			hat.Tex = new Point(85, 42);
			hat.Unlocked = false;
			hat.ZHCost = 140;
			hat.Properties = new SpecialProperties();
			hat.Properties.Armor = -20;
			hat.Properties.CritChance = 5f;
			hat.Properties.BulletDamageMod = 20f;
			hat.Properties.MeleeDamageMod = -30f;
			hat.PropDesc = new List<string>();
			hat.PropDesc.Add("[White][1136,672]+5% Crit Chance");
			hat.PropDesc.Add("[White][1088,704]+20% Gun Damage");
			hat.PropDesc.Add("[Red][1104,704]-30% Melee Damage");
			hat.PropDesc.Add("[Red][1104,688]-20 Armor");
			list.Add(hat);
			PCHatStore.GlobalHatList = list;
			return list;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00025DA8 File Offset: 0x00023FA8
		public static Hat GetHatByName(string name)
		{
			foreach (Hat hat in PCHatStore.GlobalHatList)
			{
				if (hat.Name == name)
				{
					return hat;
				}
			}
			return default(Hat);
		}

		// Token: 0x040004FE RID: 1278
		private List<Hat> Hats;

		// Token: 0x040004FF RID: 1279
		private PCButton UnlockButton;

		// Token: 0x04000500 RID: 1280
		private PCButton EquipButton;

		// Token: 0x04000501 RID: 1281
		private PCButton BackButton;

		// Token: 0x04000502 RID: 1282
		private CurrencyDisplay points;

		// Token: 0x04000503 RID: 1283
		public static List<Hat> GlobalHatList;
	}
}
