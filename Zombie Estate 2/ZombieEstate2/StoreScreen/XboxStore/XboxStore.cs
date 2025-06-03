using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.HUD.XboxHUD;
using ZombieEstate2.UI;

namespace ZombieEstate2.StoreScreen.XboxStore
{
	// Token: 0x02000145 RID: 325
	internal class XboxStore
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x0004F0B5 File Offset: 0x0004D2B5
		public Player Player
		{
			get
			{
				return this.mPlayer;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x0004F0BD File Offset: 0x0004D2BD
		public XboxStore.StoreState State
		{
			get
			{
				return this.mState;
			}
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0004F0C8 File Offset: 0x0004D2C8
		public XboxStore(Player player)
		{
			this.mState = XboxStore.StoreState.Store;
			this.mPlayer = player;
			this.SetUpTopLeft();
			this.mScreen = new XboxItemScreen(5, 16, 5, 5, Vector2.Add(this.mTopLeft, new Vector2(12f, 16f)), player);
			this.mScreen.PopulateGuns(GunStatsLoader.GunStatsList);
			this.mScreen.ItemHighlighted += this.store_ItemHighlighted;
			this.mInvScreen = new XboxItemScreen(3, 3, 3, 3, new Vector2(this.mTopLeft.X + 45f, this.mTopLeft.Y + 46f), this.mPlayer);
			this.mInvScreen.ItemHighlighted += this.mInvScreen_ItemHighlighted;
			this.mInvScreen.INVENTORY = true;
			this.mLocBigTex = new Rectangle((int)this.mTopLeft.X + 195, (int)this.mTopLeft.Y + 7, 64, 64);
			this.mLocNameCenter = new Vector2(this.mTopLeft.X + 379f, this.mTopLeft.Y + 23f);
			this.mLocAmmoType = new Rectangle((int)this.mTopLeft.X + 264, (int)this.mTopLeft.Y + 38, 32, 32);
			this.mLocMoney = new Vector2(this.mTopLeft.X + 19f, this.mTopLeft.Y + 200f);
			this.mLocHealthPacks = new Vector2(this.mTopLeft.X + 103f, this.mTopLeft.Y + 200f);
			this.mLocUpgradeTokens = new Vector2(this.mTopLeft.X + 158f, this.mTopLeft.Y + 200f);
			this.mLocUpgradeLeft = new Rectangle((int)this.mTopLeft.X + 366, (int)this.mTopLeft.Y + 192, 32, 32);
			this.mLocUpgradeRight = new Rectangle((int)this.mTopLeft.X + 445, (int)this.mTopLeft.Y + 192, 32, 32);
			this.mLocUp1 = new Rectangle((int)this.mTopLeft.X + 196, (int)this.mTopLeft.Y + 180, 32, 32);
			this.mLocUp2 = new Rectangle((int)this.mTopLeft.X + 196 + 32, (int)this.mTopLeft.Y + 180, 32, 32);
			this.mLocUp3 = new Rectangle((int)this.mTopLeft.X + 196 + 64, (int)this.mTopLeft.Y + 180, 32, 32);
			this.mTypeLocation = new Rectangle((int)this.mTopLeft.X + 460, (int)this.mTopLeft.Y + 38, 16, 16);
			this.AmmoInit();
			this.mScreen.UpdateGunListOwnAfford();
			int num = 333;
			int num2 = 211;
			this.mAmmoBars = new List<XboxBar>();
			XboxBar item = new XboxBar(num + (int)this.mTopLeft.X, num2 + (int)this.mTopLeft.Y, true, XboxHUD.mAmmoTex, 0, 43);
			this.mAmmoBars.Add(item);
			item = new XboxBar(num + (int)this.mTopLeft.X + 38, num2 + (int)this.mTopLeft.Y, true, XboxHUD.mAmmoTex, 0, 45);
			this.mAmmoBars.Add(item);
			item = new XboxBar(num + (int)this.mTopLeft.X + 76, num2 + (int)this.mTopLeft.Y, true, XboxHUD.mAmmoTex, 0, 44);
			this.mAmmoBars.Add(item);
			item = new XboxBar(num + (int)this.mTopLeft.X + 114, num2 + (int)this.mTopLeft.Y, true, XboxHUD.mAmmoTex, 0, 46);
			this.mAmmoBars.Add(item);
			this.SetupButtons();
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0004F520 File Offset: 0x0004D720
		private void SetUpTopLeft()
		{
			Rectangle safeScreenArea = Global.GetSafeScreenArea();
			if (this.mPlayer.Index == 0)
			{
				this.mTopLeft = new Vector2((float)safeScreenArea.X, (float)safeScreenArea.Y);
				return;
			}
			if (this.mPlayer.Index == 1)
			{
				this.mTopLeft = new Vector2((float)(safeScreenArea.X + safeScreenArea.Width - 500), (float)safeScreenArea.Y);
				return;
			}
			if (this.mPlayer.Index == 2)
			{
				this.mTopLeft = new Vector2((float)safeScreenArea.X, (float)(safeScreenArea.Y + safeScreenArea.Height - 280));
				return;
			}
			if (this.mPlayer.Index == 3)
			{
				this.mTopLeft = new Vector2((float)(safeScreenArea.X + safeScreenArea.Width - 500), (float)(safeScreenArea.Y + safeScreenArea.Height - 280));
			}
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0004F608 File Offset: 0x0004D808
		private void SetupButtons()
		{
			this.mBackButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 191, (int)this.mTopLeft.Y + 250, 120, 28), "Back", ButtonPress.Negative, this.mPlayer.Index);
			this.mPurchaseButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 318, (int)this.mTopLeft.Y + 250, 180, 28), "Purchase", ButtonPress.Affirmative, this.mPlayer.Index);
			this.mInventoryButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 2, (int)this.mTopLeft.Y + 250, 186, 28), "Inventory", ButtonPress.Inventory, this.mPlayer.Index);
			this.mStoreStats = new ZEButton(new Rectangle((int)this.mTopLeft.X + 191, (int)this.mTopLeft.Y + 219, 120, 28), "Stats", ButtonPress.ViewStats, this.mPlayer.Index);
			this.mBackButton.OnPressed += this.Close;
			this.mPurchaseButton.OnPressed += this.PurchasePressed;
			this.mInventoryButton.OnPressed += delegate(object e, EventArgs s)
			{
				if (this.mState == XboxStore.StoreState.Store)
				{
					this.mState = XboxStore.StoreState.Inventory;
					this.mInvScreen.PopulateGuns(this.mPlayer.Guns);
					this.mSelectedItem = null;
					this.mInvScreen.RefireEvent(true);
					SoundEngine.PlaySound("ze2_menunav", 0.25f);
				}
			};
			this.mStoreStats.OnPressed += delegate(object e, EventArgs s)
			{
				if (this.mState == XboxStore.StoreState.Store && this.mSelectedItem != null && this.mSelectedItem.AMMOITEM == null)
				{
					this.mStatsDialog = new XboxStatsDialog("", this.mTopLeft, this.mPlayer, this.mSelectedItem.Stats, 0);
					SoundEngine.PlaySound("ze2_menunav", 0.45f);
				}
			};
			this.mUpgradeBackButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 191, (int)this.mTopLeft.Y + 250, 120, 28), "Back", ButtonPress.Negative, this.mPlayer.Index);
			this.mUpgradeButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 318, (int)this.mTopLeft.Y + 250, 180, 28), "Upgrade", ButtonPress.Affirmative, this.mPlayer.Index);
			this.mStoreButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 2, (int)this.mTopLeft.Y + 250, 186, 28), "Store", ButtonPress.Inventory, this.mPlayer.Index);
			this.mSellButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 22, (int)this.mTopLeft.Y + 154, 160, 28), "Sell Gun", ButtonPress.Reload, this.mPlayer.Index);
			this.mUpgradeStats = new ZEButton(new Rectangle((int)this.mTopLeft.X + 191, (int)this.mTopLeft.Y + 219, 120, 28), "Stats", ButtonPress.ViewStats, this.mPlayer.Index);
			this.mUpgradeBackButton.OnPressed += this.Close;
			this.mUpgradeButton.OnPressed += delegate(object e, EventArgs s)
			{
				XboxItem xboxItem = this.mHighlightedItem;
				if (xboxItem == null || this.mFirstFrame)
				{
					return;
				}
				GunStats gunStats = xboxItem.Tag as GunStats;
				Gun gun = this.mPlayer.GetGun(gunStats.GunName);
				if (gun.IsSuper)
				{
					this.mNotEnoughMoney = new XboxStoreDialog("This weapon can not be upgraded!", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.Ok);
					SoundEngine.PlaySound("ze2_death", 1f);
					return;
				}
				if (gun.GetLevel() >= 3)
				{
					this.mNotEnoughMoney = new XboxStoreDialog("This weapon is fully upgraded!", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.Ok);
					SoundEngine.PlaySound("ze2_death", 1f);
					return;
				}
				if (this.mPlayer.Stats.UpgradeTokens < 1)
				{
					this.mNotEnoughMoney = new XboxStoreDialog("You need at least 1 Upgrade Token!", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.Ok);
					SoundEngine.PlaySound("ze2_death", 1f);
					return;
				}
				this.mAreYouSureUpgrade = new XboxStoreDialog("Upgrade this weapon for 1 Upgrade Token?", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.YesNo);
				SoundEngine.PlaySound("ze2_menunav", 0.45f);
			};
			this.mUpgradeStats.OnPressed += delegate(object e, EventArgs s)
			{
				if (this.mSelectedItem != null)
				{
					SoundEngine.PlaySound("ze2_menunav", 0.45f);
					if (this.mSelectedItem.Level == 3)
					{
						this.mStatsDialog = new XboxStatsDialog("", this.mTopLeft, this.mPlayer, this.mSelectedItem.Stats, 3);
						return;
					}
					this.mStatsDialog = new XboxStatsDialog("", this.mTopLeft, this.mPlayer, this.mSelectedItem.Stats, this.mSelectedItem.Stats, this.mSelectedItem.Level);
				}
			};
			this.mStoreButton.OnPressed += delegate(object e, EventArgs s)
			{
				if (this.mState == XboxStore.StoreState.Inventory)
				{
					this.mState = XboxStore.StoreState.Store;
					this.mSelectedItem = null;
					this.mScreen.RefireEvent(false);
					SoundEngine.PlaySound("ze2_menunav", 0.25f);
				}
			};
			this.mSellButton.OnPressed += delegate(object e, EventArgs s)
			{
				if (this.mSelectedItem != null)
				{
					if (this.mSelectedItem.AmmoType == AmmoType.INFINITE || this.mSelectedItem.Name == "Rake" || this.mSelectedItem.Name == "Stick" || this.mSelectedItem.Name == "Cyber Baton")
					{
						this.mNotEnoughMoney = new XboxStoreDialog("You can't sell that gun!", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.Ok);
						SoundEngine.PlaySound("ze2_death", 1f);
						return;
					}
					this.mAreYouSureSell = new XboxStoreDialog("Do you want to sell this item for $" + this.mSelectedItem.Cost / 2 + "?", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.YesNo);
					SoundEngine.PlaySound("ze2_menunav", 0.45f);
				}
			};
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0004F980 File Offset: 0x0004DB80
		public static void LoadTex()
		{
			if (XboxStore.mTexBG == null)
			{
				XboxStore.mTexBG = Global.Content.Load<Texture2D>("XboxStore\\StoreBG");
				XboxStore.mTexUpgradeBG = Global.Content.Load<Texture2D>("XboxStore\\UpgradeBG");
				XboxStore.mTexIcons = Global.Content.Load<Texture2D>("XboxStore\\StoreIcons");
				XboxStore.mTexUpgradeIcons = Global.Content.Load<Texture2D>("XboxStore\\UpgradeIcons");
				XboxStore.mTexReopen = Global.Content.Load<Texture2D>("XboxStore\\ReopenStore");
				XboxStore.mTexUpInd = Global.Content.Load<Texture2D>("XboxStore\\UpgradeIndicator");
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0004FA0C File Offset: 0x0004DC0C
		private void AmmoInit()
		{
			this.mAmmoLocs = new List<Rectangle>();
			this.mAmmoLocs.Add(new Rectangle((int)this.mTopLeft.X + 45, (int)this.mTopLeft.Y + 7, 32, 32));
			this.mAmmoLocs.Add(new Rectangle((int)this.mTopLeft.X + 80, (int)this.mTopLeft.Y + 7, 32, 32));
			this.mAmmoLocs.Add(new Rectangle((int)this.mTopLeft.X + 115, (int)this.mTopLeft.Y + 7, 32, 32));
			this.mAmmoSrcs = new List<Rectangle>();
			this.mAmmoSrcs.Add(PCItem.GetAmmoRect(AmmoType.ASSAULT));
			this.mAmmoSrcs.Add(PCItem.GetAmmoRect(AmmoType.SHELLS));
			this.mAmmoSrcs.Add(PCItem.GetAmmoRect(AmmoType.HEAVY));
			this.mAmmoSrcs.Add(PCItem.GetAmmoRect(AmmoType.EXPLOSIVE));
			this.mAmmoSrcs.Add(PCItem.GetAmmoRect(AmmoType.MELEE));
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0004FB18 File Offset: 0x0004DD18
		public void Update()
		{
			if (this.mInflateTime > 0f)
			{
				this.mInflateTime -= Global.REAL_GAME_TIME;
			}
			if (this.mInflateTime < 0f)
			{
				this.mInflateTime = 0f;
			}
			if (this.mNotEnoughMoney != null && this.mNotEnoughMoney.Active)
			{
				this.mNotEnoughMoney.Update();
				return;
			}
			if (this.mAreYouSureSell != null && this.mAreYouSureSell.Active)
			{
				this.SellDialog();
				return;
			}
			if (this.mAreYouSureBuy != null && this.mAreYouSureBuy.Active)
			{
				this.BuyDialog();
				return;
			}
			if (this.mAreYouSureUpgrade != null && this.mAreYouSureUpgrade.Active)
			{
				this.UpgradeDialog();
				return;
			}
			if (this.mStatsDialog != null && this.mStatsDialog.Active)
			{
				this.mStatsDialog.Update();
				return;
			}
			if (this.mState == XboxStore.StoreState.Store)
			{
				this.UpdateStore();
				this.mInventoryButton.Update();
				this.mPurchaseButton.Update();
				this.mBackButton.Update();
				this.mStoreStats.Update();
				this.mFirstFrame = false;
				return;
			}
			if (this.mState == XboxStore.StoreState.Inventory)
			{
				this.UpdateUpgrade();
				this.mUpgradeButton.Update();
				this.mUpgradeBackButton.Update();
				this.mSellButton.Update();
				this.mStoreButton.Update();
				this.mUpgradeStats.Update();
				this.mFirstFrame = false;
				return;
			}
			if (this.mState == XboxStore.StoreState.Closed)
			{
				if (InputManager.ButtonPressed(ButtonPress.OpenStore, this.mPlayer.Index, false))
				{
					this.mState = XboxStore.StoreState.Store;
					this.mSelectedItem = null;
					this.mScreen.RefireEvent(false);
					SoundEngine.PlaySound("ze2_menuselect", 0.5f);
				}
				this.mFirstFrame = false;
				return;
			}
			this.mFirstFrame = false;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0004FCD8 File Offset: 0x0004DED8
		private void SellDialog()
		{
			this.mAreYouSureSell.Update();
			if (this.mAreYouSureSell.Active)
			{
				return;
			}
			if (this.mAreYouSureSell.YesPressed)
			{
				this.mPlayer.RemoveGun(this.mSelectedItem.Name);
				this.mPlayer.Stats.AddMoney(this.mSelectedItem.Cost / 2);
				this.mInvScreen.PopulateGuns(this.mPlayer.Guns);
				this.mInvScreen.RefireEvent(true);
				this.mScreen.UpdateGunListOwnAfford();
				SoundEngine.PlaySound("ze2_money", 0.6f);
				return;
			}
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0004FD7C File Offset: 0x0004DF7C
		private void BuyDialog()
		{
			this.mAreYouSureBuy.Update();
			if (!this.mAreYouSureBuy.Active && this.mAreYouSureBuy.YesPressed)
			{
				this.mPlayer.Stats.AddMoney(-this.mSelectedItem.Cost);
				if (this.mSelectedItem.AMMOITEM != null)
				{
					this.mSelectedItem.AMMOITEM.Purchased(this.mPlayer);
				}
				else
				{
					this.mPlayer.AddGun(this.mSelectedItem.Name, true);
				}
				this.mScreen.UpdateGunListOwnAfford();
				SoundEngine.PlaySound("ze2_money", 0.6f);
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0004FE24 File Offset: 0x0004E024
		private void UpgradeDialog()
		{
			this.mAreYouSureUpgrade.Update();
			if (!this.mAreYouSureUpgrade.Active && this.mAreYouSureUpgrade.YesPressed)
			{
				this.mPlayer.GetGun(this.mSelectedItem.Name).LevelUpGun();
				PlayerStats stats = this.mPlayer.Stats;
				int upgradeTokens = stats.UpgradeTokens;
				stats.UpgradeTokens = upgradeTokens - 1;
				if (this.mPlayer.IAmOwnedByLocalPlayer)
				{
					this.mPlayer.AssertNetGunList();
				}
				this.mInvScreen.PopulateGuns(this.mPlayer.Guns);
				this.mInvScreen.RefireEvent(false);
				SoundEngine.PlaySound("ze2_money", 0.6f);
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0004FED7 File Offset: 0x0004E0D7
		private void UpdateStore()
		{
			this.mScreen.Update();
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0004FEE4 File Offset: 0x0004E0E4
		private void UpdateUpgrade()
		{
			this.mInvScreen.Update();
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0004FEF1 File Offset: 0x0004E0F1
		private void Close(object sender, EventArgs e)
		{
			if (this.mState == XboxStore.StoreState.Closed)
			{
				this.mState = XboxStore.StoreState.Store;
				return;
			}
			this.mState = XboxStore.StoreState.Closed;
			this.mFirstFrame = true;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0004FF14 File Offset: 0x0004E114
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.mNotEnoughMoney != null && this.mNotEnoughMoney.Active)
			{
				this.mNotEnoughMoney.Draw(spriteBatch);
				return;
			}
			if (this.mAreYouSureSell != null && this.mAreYouSureSell.Active)
			{
				this.mAreYouSureSell.Draw(spriteBatch);
				return;
			}
			if (this.mAreYouSureBuy != null && this.mAreYouSureBuy.Active)
			{
				this.mAreYouSureBuy.Draw(spriteBatch);
				return;
			}
			if (this.mAreYouSureUpgrade != null && this.mAreYouSureUpgrade.Active)
			{
				this.mAreYouSureUpgrade.Draw(spriteBatch);
				return;
			}
			if (this.mStatsDialog != null && this.mStatsDialog.Active)
			{
				this.mStatsDialog.Draw(spriteBatch);
				return;
			}
			if (this.mState == XboxStore.StoreState.Store)
			{
				this.DrawStore(spriteBatch);
				this.DrawCurrencies(spriteBatch);
				this.mInventoryButton.Draw(spriteBatch);
				this.mBackButton.Draw(spriteBatch);
				this.mPurchaseButton.Draw(spriteBatch);
				this.mStoreStats.Draw(spriteBatch);
			}
			if (this.mState == XboxStore.StoreState.Inventory)
			{
				this.DrawUpgradeStore(spriteBatch);
				this.DrawCurrencies(spriteBatch);
				this.mUpgradeBackButton.Draw(spriteBatch);
				this.mUpgradeButton.Draw(spriteBatch);
				this.mUpgradeStats.Draw(spriteBatch);
				this.mStoreButton.Draw(spriteBatch);
				this.mSellButton.Draw(spriteBatch);
			}
			if (this.mState == XboxStore.StoreState.Closed)
			{
				spriteBatch.Draw(XboxStore.mTexReopen, this.mTopLeft, Color.White);
			}
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00050084 File Offset: 0x0004E284
		private void DrawStore(SpriteBatch spriteBatch)
		{
			int num = (int)(16f * (this.mInflateTime / this.MAX_INFLATE));
			Rectangle destinationRectangle = new Rectangle(this.mLocBigTex.X, this.mLocBigTex.Y, this.mLocBigTex.Width, this.mLocBigTex.Height);
			destinationRectangle.Inflate(num, num);
			spriteBatch.Draw(XboxStore.mTexBG, this.mTopLeft, this.mPlayer.HUDColor);
			this.mScreen.Draw(spriteBatch);
			if (this.mSelectedItem != null)
			{
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(this.mSelectedItem.Src), Color.White);
				Shadow.DrawString(this.mSelectedItem.Name, Global.EquationFontSmall, this.mSelectedItem.NameLocation, 1, Color.White, spriteBatch);
				this.mSelectedItem.Desc.Draw(spriteBatch);
				spriteBatch.Draw(Global.MasterTexture, this.mLocAmmoType, new Rectangle?(this.mSelectedItem.AmmoSrc), Color.White);
				Shadow.DrawString(this.mSelectedItem.CostString, Global.EquationFontSmall, this.mSelectedItem.CostLocation, 1, Color.LightGreen, spriteBatch);
				if (this.mSelectedItem.Stats != null)
				{
					Rectangle texRectange = Global.GetTexRectange(0, 0);
					string text;
					switch (this.mSelectedItem.Stats.GunProperties[0].DamageType)
					{
					case DamageType.Fire:
						texRectange = Global.GetTexRectange(67, 43);
						text = "Fire";
						break;
					case DamageType.Water:
						texRectange = Global.GetTexRectange(67, 45);
						text = "Water";
						break;
					case DamageType.Earth:
						texRectange = Global.GetTexRectange(67, 44);
						text = "Earth";
						break;
					default:
						texRectange = Global.GetTexRectange(67, 42);
						text = "Phys";
						break;
					}
					spriteBatch.Draw(Global.MasterTexture, this.mTypeLocation, new Rectangle?(texRectange), Color.White);
					Vector2 pos = VerchickMath.CenterText(Global.StoreFontSmall, new Vector2((float)(this.mTypeLocation.X + 8), (float)(this.mTypeLocation.Y + 20)), text);
					Shadow.DrawString(text, Global.StoreFontSmall, pos, 1, Color.White, spriteBatch);
				}
			}
			spriteBatch.Draw(XboxStore.mTexIcons, this.mTopLeft, Color.White);
			if (this.mPlayer.Stats.UpgradeTokens > 0)
			{
				float pulse = Global.Pulse;
				spriteBatch.Draw(XboxStore.mTexUpInd, this.mTopLeft, Color.White * pulse * 0.5f);
			}
			this.mAmmoBars[0].Draw(spriteBatch, this.mPlayer.Stats.GetMaxAmmo(AmmoType.ASSAULT), this.mPlayer.Stats.GetAmmoIncludingClip(AmmoType.ASSAULT));
			this.mAmmoBars[1].Draw(spriteBatch, this.mPlayer.Stats.GetMaxAmmo(AmmoType.SHELLS), this.mPlayer.Stats.GetAmmoIncludingClip(AmmoType.SHELLS));
			this.mAmmoBars[2].Draw(spriteBatch, this.mPlayer.Stats.GetMaxAmmo(AmmoType.HEAVY), this.mPlayer.Stats.GetAmmoIncludingClip(AmmoType.HEAVY));
			this.mAmmoBars[3].Draw(spriteBatch, this.mPlayer.Stats.GetMaxAmmo(AmmoType.EXPLOSIVE), this.mPlayer.Stats.GetAmmoIncludingClip(AmmoType.EXPLOSIVE));
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x000503DC File Offset: 0x0004E5DC
		private void DrawCurrencies(SpriteBatch spriteBatch)
		{
			Shadow.DrawString(this.mPlayer.Stats.GetMoney().ToString(), Global.EquationFontSmall, this.mLocMoney, 1, Color.LightGreen, spriteBatch);
			Shadow.DrawString(this.mPlayer.Stats.HealthPacks.ToString(), Global.EquationFontSmall, this.mLocHealthPacks, 1, Color.LightPink, spriteBatch);
			Shadow.DrawString(this.mPlayer.Stats.UpgradeTokens.ToString(), Global.EquationFontSmall, this.mLocUpgradeTokens, 1, Color.LightYellow, spriteBatch);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00050478 File Offset: 0x0004E678
		private void DrawUpgradeStore(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(XboxStore.mTexUpgradeBG, this.mTopLeft, this.mPlayer.HUDColor);
			this.mInvScreen.Draw(spriteBatch);
			if (this.mSelectedItem != null)
			{
				int num = (int)(16f * (this.mInflateTime / this.MAX_INFLATE));
				Rectangle destinationRectangle = new Rectangle(this.mLocBigTex.X, this.mLocBigTex.Y, this.mLocBigTex.Width, this.mLocBigTex.Height);
				destinationRectangle.Inflate(num, num);
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(this.mSelectedItem.Src), Color.White);
				Shadow.DrawString(this.mSelectedItem.Name, Global.EquationFontSmall, this.mSelectedItem.NameLocation, 1, Color.White, spriteBatch);
				this.mSelectedItem.LevelDescription.Draw(spriteBatch);
				spriteBatch.Draw(Global.MasterTexture, this.mLocUpgradeLeft, new Rectangle?(this.mSelectedItem.Src), Color.White);
				spriteBatch.Draw(Global.MasterTexture, this.mLocUpgradeRight, new Rectangle?(this.mSelectedItem.SrcLevelUp), Color.White);
				spriteBatch.Draw(Global.MasterTexture, this.mLocAmmoType, new Rectangle?(this.mSelectedItem.AmmoSrc), Color.White);
				spriteBatch.Draw(Global.MasterTexture, this.mLocUp1, new Rectangle?(Global.GetTexRectange(7, 37)), Color.White);
				spriteBatch.Draw(Global.MasterTexture, this.mLocUp2, new Rectangle?(Global.GetTexRectange(7, 37)), Color.White);
				spriteBatch.Draw(Global.MasterTexture, this.mLocUp3, new Rectangle?(Global.GetTexRectange(7, 37)), Color.White);
				if (this.mSelectedItem.Level > 0)
				{
					spriteBatch.Draw(Global.MasterTexture, this.mLocUp1, new Rectangle?(Global.GetTexRectange(6, 37)), Color.White);
				}
				if (this.mSelectedItem.Level > 1)
				{
					spriteBatch.Draw(Global.MasterTexture, this.mLocUp2, new Rectangle?(Global.GetTexRectange(6, 37)), Color.White);
				}
				if (this.mSelectedItem.Level > 2)
				{
					spriteBatch.Draw(Global.MasterTexture, this.mLocUp3, new Rectangle?(Global.GetTexRectange(6, 37)), Color.White);
				}
			}
			spriteBatch.Draw(XboxStore.mTexUpgradeIcons, this.mTopLeft, Color.White);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x000506E6 File Offset: 0x0004E8E6
		public void Close()
		{
			this.mScreen.ItemHighlighted -= this.store_ItemHighlighted;
			this.mInvScreen.ItemHighlighted -= this.mInvScreen_ItemHighlighted;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00050718 File Offset: 0x0004E918
		private void PurchasePressed(object sender, EventArgs e)
		{
			XboxItem xboxItem = this.mHighlightedItem;
			if (xboxItem == null || this.mFirstFrame)
			{
				return;
			}
			if (xboxItem is XboxAmmoItem)
			{
				this.AttemptPurchaseAmmo(xboxItem);
				return;
			}
			GunStats gunStats = xboxItem.Tag as GunStats;
			if (this.mPlayer.Stats.GetMoney() < gunStats.Cost)
			{
				this.mNotEnoughMoney = new XboxStoreDialog("Not enough money for this weapon!", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.Ok);
				SoundEngine.PlaySound("ze2_death", 1f);
				return;
			}
			if (this.mPlayer.GetGunCount() >= 7)
			{
				this.mNotEnoughMoney = new XboxStoreDialog("You own too many guns! Sell one from the inventory screen!", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.Ok);
				SoundEngine.PlaySound("ze2_death", 1f);
				return;
			}
			if (this.mPlayer.GetGun(gunStats.GunName) != null)
			{
				this.mNotEnoughMoney = new XboxStoreDialog("You already own this weapon!", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.Ok);
				SoundEngine.PlaySound("ze2_death", 1f);
				return;
			}
			this.mAreYouSureBuy = new XboxStoreDialog("Are you sure you want to buy this weapon for $" + this.mSelectedItem.Cost + "?", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.YesNo);
			SoundEngine.PlaySound("ze2_menunav", 0.45f);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0005085C File Offset: 0x0004EA5C
		private void AttemptPurchaseAmmo(XboxItem item)
		{
			XboxAmmoItem xboxAmmoItem = item as XboxAmmoItem;
			if (this.mPlayer.Stats.GetMoney() < xboxAmmoItem.mCost)
			{
				this.mNotEnoughMoney = new XboxStoreDialog("Not enough money for this item!", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.Ok);
				SoundEngine.PlaySound("ze2_death", 1f);
				return;
			}
			if (this.mPlayer.Stats.GetAmmoIncludingClip(xboxAmmoItem.mType) >= this.mPlayer.Stats.GetMaxAmmo(xboxAmmoItem.mType))
			{
				this.mNotEnoughMoney = new XboxStoreDialog("You are full on that ammo type!", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.Ok);
				SoundEngine.PlaySound("ze2_death", 1f);
				return;
			}
			this.mAreYouSureBuy = new XboxStoreDialog("Are you sure you want to buy this item for $" + this.mSelectedItem.Cost + "?", this.mTopLeft, this.mPlayer, XboxStoreDialog.XboxDialogType.YesNo);
			SoundEngine.PlaySound("ze2_menunav", 0.45f);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00050956 File Offset: 0x0004EB56
		private void store_ItemHighlighted(XboxItem item)
		{
			this.ItemHighlighted(item);
			this.mInflateTime = this.MAX_INFLATE;
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0005096C File Offset: 0x0004EB6C
		private void ItemHighlighted(XboxItem item)
		{
			if (item == null)
			{
				this.mSelectedItem = null;
				this.mHighlightedItem = null;
				return;
			}
			this.mSelectedItem = new XboxStore.SelectedItem();
			this.mHighlightedItem = item;
			if (item is XboxAmmoItem)
			{
				XboxAmmoItem xboxAmmoItem = item as XboxAmmoItem;
				this.mSelectedItem.Name = xboxAmmoItem.mType.ToString();
				this.mSelectedItem.AmmoSrc = Global.GetTexRectange(63, 63);
				this.mSelectedItem.Src = PCItem.GetAmmoRect(xboxAmmoItem.mType);
				this.mSelectedItem.AmmoType = AmmoType.INFINITE;
				this.mSelectedItem.Cost = xboxAmmoItem.mCost;
				this.mSelectedItem.Desc = new ScrollBox(string.Format("Purchase {0} {1} ammo.", xboxAmmoItem.mAmount, xboxAmmoItem.mType.ToString()), new Rectangle((int)this.mTopLeft.X + 192, (int)this.mTopLeft.Y + 76, 304, 135), Global.StoreFont, this.mPlayer, Color.LightBlue);
				this.mSelectedItem.NameLocation = VerchickMath.CenterText(Global.EquationFontSmall, this.mLocNameCenter, this.mSelectedItem.Name);
				this.mSelectedItem.CostLocation = new Vector2(this.mTopLeft.X + 302f, this.mTopLeft.Y + 38f);
				this.mSelectedItem.CostString = "Cost: $" + this.mSelectedItem.Cost.ToString();
				this.mSelectedItem.AMMOITEM = xboxAmmoItem;
				return;
			}
			GunStats gunStats = item.Tag as GunStats;
			this.mSelectedItem.Cost = gunStats.Cost;
			this.mSelectedItem.Name = gunStats.GunName;
			this.mSelectedItem.AmmoType = gunStats.AmmoType;
			this.mSelectedItem.AmmoSrc = PCItem.GetAmmoRect(gunStats.AmmoType);
			this.mSelectedItem.Desc = new ScrollBox(gunStats.StoreDescription, new Rectangle((int)this.mTopLeft.X + 192, (int)this.mTopLeft.Y + 76, 304, 135), Global.StoreFont, this.mPlayer, Color.LightBlue);
			this.mSelectedItem.Src = Global.GetTexRectange(gunStats.GunXCoord, gunStats.GunYCoord + 1);
			this.mSelectedItem.NameLocation = VerchickMath.CenterText(Global.EquationFontSmall, this.mLocNameCenter, this.mSelectedItem.Name);
			this.mSelectedItem.CostLocation = new Vector2(this.mTopLeft.X + 302f, this.mTopLeft.Y + 38f);
			this.mSelectedItem.CostString = "Cost: $" + this.mSelectedItem.Cost.ToString();
			this.mSelectedItem.Stats = gunStats;
			this.mSelectedItem.AMMOITEM = null;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00050C74 File Offset: 0x0004EE74
		private void mInvScreen_ItemHighlighted(XboxItem item)
		{
			this.ItemHighlighted(item);
			if (item == null)
			{
				return;
			}
			this.mInflateTime = this.MAX_INFLATE;
			GunStats gunStats = item.Tag as GunStats;
			Gun gun = this.mPlayer.GetGun(gunStats.GunName);
			this.mSelectedItem.Src = Global.GetTexRectange(gunStats.GunXCoord + 2 * gun.GetLevel(), gunStats.GunYCoord + 1);
			if (gun.GetLevel() < 3 && !gun.IsSuper)
			{
				this.mSelectedItem.SrcLevelUp = Global.GetTexRectange(gunStats.GunXCoord + 2 * (gun.GetLevel() + 1), gunStats.GunYCoord + 1);
			}
			else
			{
				this.mSelectedItem.SrcLevelUp = Global.GetTexRectange(4, 37);
			}
			this.mSelectedItem.Level = gun.GetLevel();
			if (this.mSelectedItem.Level >= 3)
			{
				this.mSelectedItem.LevelDescription = new ScrollBox("Gun fully upgraded!", new Rectangle((int)this.mTopLeft.X + 192, (int)this.mTopLeft.Y + 76, 304, 135), Global.StoreFont, this.mPlayer, Color.LightBlue);
				return;
			}
			if (gun.IsSuper)
			{
				this.mSelectedItem.LevelDescription = new ScrollBox("Gun cannot be upgraded!", new Rectangle((int)this.mTopLeft.X + 192, (int)this.mTopLeft.Y + 76, 304, 135), Global.StoreFont, this.mPlayer, Color.LightBlue);
				return;
			}
			this.mSelectedItem.LevelDescription = new ScrollBox(gun.stats.GunProperties[this.mSelectedItem.Level + 1].LevelDescription, new Rectangle((int)this.mTopLeft.X + 192, (int)this.mTopLeft.Y + 76, 304, 135), Global.StoreFont, this.mPlayer, Color.LightBlue);
		}

		// Token: 0x04000A42 RID: 2626
		private Vector2 mTopLeft = new Vector2(320f, 320f);

		// Token: 0x04000A43 RID: 2627
		private Player mPlayer;

		// Token: 0x04000A44 RID: 2628
		private XboxStore.StoreState mState;

		// Token: 0x04000A45 RID: 2629
		private List<XboxBar> mAmmoBars;

		// Token: 0x04000A46 RID: 2630
		private XboxItemScreen mScreen;

		// Token: 0x04000A47 RID: 2631
		private static Texture2D mTexBG;

		// Token: 0x04000A48 RID: 2632
		private static Texture2D mTexUpgradeBG;

		// Token: 0x04000A49 RID: 2633
		private static Texture2D mTexIcons;

		// Token: 0x04000A4A RID: 2634
		private static Texture2D mTexUpgradeIcons;

		// Token: 0x04000A4B RID: 2635
		private static Texture2D mTexReopen;

		// Token: 0x04000A4C RID: 2636
		private static Texture2D mTexUpInd;

		// Token: 0x04000A4D RID: 2637
		private XboxStore.SelectedItem mSelectedItem;

		// Token: 0x04000A4E RID: 2638
		private XboxItem mHighlightedItem;

		// Token: 0x04000A4F RID: 2639
		private bool mFirstFrame = true;

		// Token: 0x04000A50 RID: 2640
		private ZEButton mBackButton;

		// Token: 0x04000A51 RID: 2641
		private ZEButton mPurchaseButton;

		// Token: 0x04000A52 RID: 2642
		private ZEButton mInventoryButton;

		// Token: 0x04000A53 RID: 2643
		private ZEButton mStoreStats;

		// Token: 0x04000A54 RID: 2644
		private ZEButton mUpgradeBackButton;

		// Token: 0x04000A55 RID: 2645
		private ZEButton mUpgradeButton;

		// Token: 0x04000A56 RID: 2646
		private ZEButton mStoreButton;

		// Token: 0x04000A57 RID: 2647
		private ZEButton mSellButton;

		// Token: 0x04000A58 RID: 2648
		private ZEButton mUpgradeStats;

		// Token: 0x04000A59 RID: 2649
		private Rectangle mTypeLocation;

		// Token: 0x04000A5A RID: 2650
		private float mInflateTime;

		// Token: 0x04000A5B RID: 2651
		private float MAX_INFLATE = 0.15f;

		// Token: 0x04000A5C RID: 2652
		private List<Rectangle> mAmmoSrcs;

		// Token: 0x04000A5D RID: 2653
		private List<Rectangle> mAmmoLocs;

		// Token: 0x04000A5E RID: 2654
		private int mAmmoIndex;

		// Token: 0x04000A5F RID: 2655
		private Rectangle mLocBigTex;

		// Token: 0x04000A60 RID: 2656
		private Vector2 mLocNameCenter;

		// Token: 0x04000A61 RID: 2657
		private Rectangle mLocAmmoType;

		// Token: 0x04000A62 RID: 2658
		private Vector2 mLocMoney;

		// Token: 0x04000A63 RID: 2659
		private Vector2 mLocHealthPacks;

		// Token: 0x04000A64 RID: 2660
		private Vector2 mLocUpgradeTokens;

		// Token: 0x04000A65 RID: 2661
		private Rectangle mLocUpgradeLeft;

		// Token: 0x04000A66 RID: 2662
		private Rectangle mLocUpgradeRight;

		// Token: 0x04000A67 RID: 2663
		private Rectangle mLocUp1;

		// Token: 0x04000A68 RID: 2664
		private Rectangle mLocUp2;

		// Token: 0x04000A69 RID: 2665
		private Rectangle mLocUp3;

		// Token: 0x04000A6A RID: 2666
		private XboxItemScreen mInvScreen;

		// Token: 0x04000A6B RID: 2667
		private XboxStoreDialog mNotEnoughMoney;

		// Token: 0x04000A6C RID: 2668
		private XboxStoreDialog mAreYouSureSell;

		// Token: 0x04000A6D RID: 2669
		private XboxStoreDialog mAreYouSureBuy;

		// Token: 0x04000A6E RID: 2670
		private XboxStoreDialog mAreYouSureUpgrade;

		// Token: 0x04000A6F RID: 2671
		private XboxStatsDialog mStatsDialog;

		// Token: 0x02000221 RID: 545
		private class SelectedItem
		{
			// Token: 0x04000E47 RID: 3655
			public Rectangle Src;

			// Token: 0x04000E48 RID: 3656
			public Rectangle SrcLevelUp;

			// Token: 0x04000E49 RID: 3657
			public Rectangle AmmoSrc;

			// Token: 0x04000E4A RID: 3658
			public string Name;

			// Token: 0x04000E4B RID: 3659
			public string CostString;

			// Token: 0x04000E4C RID: 3660
			public int Cost;

			// Token: 0x04000E4D RID: 3661
			public ScrollBox Desc;

			// Token: 0x04000E4E RID: 3662
			public ScrollBox LevelDescription;

			// Token: 0x04000E4F RID: 3663
			public GunStats Stats;

			// Token: 0x04000E50 RID: 3664
			public AmmoType AmmoType;

			// Token: 0x04000E51 RID: 3665
			public Vector2 NameLocation;

			// Token: 0x04000E52 RID: 3666
			public Vector2 CostLocation;

			// Token: 0x04000E53 RID: 3667
			public XboxAmmoItem AMMOITEM;

			// Token: 0x04000E54 RID: 3668
			public int Level;
		}

		// Token: 0x02000222 RID: 546
		public enum StoreState
		{
			// Token: 0x04000E56 RID: 3670
			Store,
			// Token: 0x04000E57 RID: 3671
			Inventory,
			// Token: 0x04000E58 RID: 3672
			Stats_Store,
			// Token: 0x04000E59 RID: 3673
			Stats_Upgrade,
			// Token: 0x04000E5A RID: 3674
			Closed
		}
	}
}
