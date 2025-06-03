using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000D4 RID: 212
	public class Store : ItemScreenController, ShopScreen
	{
		// Token: 0x06000575 RID: 1397 RVA: 0x000299E8 File Offset: 0x00027BE8
		public Store(Player parent, StoreManager manager)
		{
			this.parent = parent;
			this.SetUpGuns();
			this.SetPosition();
			this.screen = new ItemScreen(this, new Rectangle((int)this.Position.X + 10, (int)this.Position.Y + 4, 242, 142), parent);
			this.AddItems();
			this.SetUpGraphics();
			this.StoreDialog = Global.Content.Load<Texture2D>("Store\\StoreDialog");
			this.manager = manager;
			this.DescriptionBox = new ScrollBox("", new Rectangle((int)this.Position.X + 423 - 82, (int)this.Position.Y + 42, 380, 84), Global.StoreFont, parent, this.descriptionColor);
			this.SelectionChange(this.screen.GetSelectedItem());
			this.screen.ResetItemColors();
			this.CurrentBGColor = Color.Transparent;
			this.AmmoAssault = new AmmoMeter(new Point(812 + (int)this.Position.X - 82, 104 + (int)this.Position.Y - 24), Color.White, new Point(0, 43));
			this.AmmoHeavy = new AmmoMeter(new Point(844 + (int)this.Position.X - 82, 104 + (int)this.Position.Y), Color.White, new Point(0, 44));
			this.AmmoShells = new AmmoMeter(new Point(876 + (int)this.Position.X - 82, 104 + (int)this.Position.Y - 24), Color.White, new Point(0, 45));
			this.AmmoExplosive = new AmmoMeter(new Point(908 + (int)this.Position.X - 82, 104 + (int)this.Position.Y), Color.White, new Point(0, 46));
			this.AmmoSpecial = new AmmoMeter(new Point(940 + (int)this.Position.X - 82, 104 + (int)this.Position.Y - 24), Color.White, new Point(0, 47));
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00029CC0 File Offset: 0x00027EC0
		public virtual void AddItems()
		{
			for (int i = 0; i < GunStatsLoader.GunStatsList.Count; i++)
			{
				this.screen.AddItem(new Item(GunStatsLoader.GunStatsList[i], i));
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00029CFE File Offset: 0x00027EFE
		public virtual void SetPosition()
		{
			this.Position = new Vector2(100f, (float)(60 + (148 * this.parent.PositionIndex + 2 * this.parent.PositionIndex)));
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00029D33 File Offset: 0x00027F33
		public virtual void SetUpGraphics()
		{
			this.StoreBackground = Global.Content.Load<Texture2D>("Store\\StoreMini2");
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00029D4C File Offset: 0x00027F4C
		public virtual void DrawPlayerData(SpriteBatch spriteBatch)
		{
			Vector2 pos = new Vector2(this.Position.X + 812f - 82f, this.Position.Y + 4f);
			Shadow.DrawString("Money:", Global.StoreFont, pos, 2, Color.LightGreen, spriteBatch);
			pos.Y += (float)Global.StoreFont.LineSpacing;
			int num = 172 - (int)Global.StoreFont.MeasureString("$ " + this.parent.Stats.GetMoney()).X;
			pos.X += (float)num;
			Shadow.DrawString("$ " + this.parent.Stats.GetMoney(), Global.StoreFont, pos, 2, this.CurrentCurrencyColor, spriteBatch);
			this.DrawAmmo(spriteBatch);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00029E34 File Offset: 0x00028034
		public virtual void DrawItemIcons(SpriteBatch spriteBatch)
		{
			Item selectedItem = this.screen.GetSelectedItem();
			if (selectedItem != null)
			{
				selectedItem.DrawItem(spriteBatch, new Vector2(263f, 43f) + this.Position, 64, Color.White, true);
			}
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00029E7C File Offset: 0x0002807C
		public virtual void DrawDescription(SpriteBatch spriteBatch)
		{
			this.DescriptionBox.Draw(spriteBatch);
			if (this.screen.GetSelectedItem() == null)
			{
				return;
			}
			Shadow.DrawString(this.GetDescriptionTitle(), Global.StoreFont, new Vector2(this.Position.X + 423f - 82f, this.Position.Y + 10f), 1, Color.White, spriteBatch);
			this.DrawDataNextToTitle(spriteBatch);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00029EF0 File Offset: 0x000280F0
		public virtual void DrawDataNextToTitle(SpriteBatch spriteBatch)
		{
			Item selectedItem = this.screen.GetSelectedItem();
			if (selectedItem == null)
			{
				return;
			}
			Vector2 pos = new Vector2(this.Position.X + 800f - 82f, this.Position.Y + 4f);
			pos.X -= Global.StoreFont.MeasureString("Cost: $" + selectedItem.cost).X;
			Shadow.DrawString("Cost: $" + selectedItem.cost, Global.StoreFont, pos, 2, Color.LightGreen, spriteBatch);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00029F94 File Offset: 0x00028194
		private void DrawMessage(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.StoreDialog, this.Position, Color.White);
			Vector2 vector = VerchickMath.CenterText(Global.StoreFontBig, new Vector2(this.Position.X + 535f, this.Position.Y + 48f), this.MessageText);
			Shadow.DrawString(this.MessageText, Global.StoreFontBig, vector, 1, Color.White, spriteBatch);
			if (this.MessageConfirmation)
			{
				vector = new Vector2(this.Position.X + 420f - 41f, this.Position.Y + 92f);
				Global.DrawTextWithIcon("Yes", 0, 38, (int)vector.X, (int)vector.Y, Global.StoreFontBig, spriteBatch, 32);
				Global.DrawTextWithIcon("No", 1, 38, (int)vector.X + 200, (int)vector.Y, Global.StoreFontBig, spriteBatch, 32);
				return;
			}
			vector = new Vector2(this.Position.X + 520f - 41f, this.Position.Y + 92f);
			Global.DrawTextWithIcon("Ok", 0, 38, (int)vector.X, (int)vector.Y, Global.StoreFontBig, spriteBatch, 32);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0002A0E0 File Offset: 0x000282E0
		private void DrawAmmo(SpriteBatch spriteBatch)
		{
			this.AmmoAssault.DrawMeter(spriteBatch, this.parent.Stats.GetAmmo(AmmoType.ASSAULT), this.parent.Stats.GetMaxAmmo(AmmoType.ASSAULT), false);
			this.AmmoHeavy.DrawMeter(spriteBatch, this.parent.Stats.GetAmmo(AmmoType.HEAVY), this.parent.Stats.GetMaxAmmo(AmmoType.HEAVY), false);
			this.AmmoShells.DrawMeter(spriteBatch, this.parent.Stats.GetAmmo(AmmoType.SHELLS), this.parent.Stats.GetMaxAmmo(AmmoType.SHELLS), false);
			this.AmmoExplosive.DrawMeter(spriteBatch, this.parent.Stats.GetAmmo(AmmoType.EXPLOSIVE), this.parent.Stats.GetMaxAmmo(AmmoType.EXPLOSIVE), false);
			this.AmmoSpecial.DrawMeter(spriteBatch, this.parent.Stats.GetAmmo(AmmoType.SPECIAL), this.parent.Stats.GetMaxAmmo(AmmoType.SPECIAL), false);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0002A1D8 File Offset: 0x000283D8
		public virtual void PurchaseItem(Item selectedItem)
		{
			if (this.parent.GetGunCount() >= Player.MAXGUNS)
			{
				this.FlashColorBG(Color.Red, 0.5f);
				this.ShowMessage("Inventory full! Sell your guns on the Inventory screen!", false);
				return;
			}
			foreach (Gun gun in this.parent.Guns)
			{
				if (gun != null && gun.Name == selectedItem.label)
				{
					this.FlashColorBG(Color.Red, 0.5f);
					this.ShowMessage("You already own this gun!", false);
					return;
				}
			}
			if (this.parent.Stats.GetMoney() < selectedItem.cost)
			{
				this.FlashColorBG(Color.Red, 0.5f);
				this.ShowMessage("You don't have enough money!", false);
				return;
			}
			this.parent.Stats.AddMoney(-selectedItem.cost);
			this.parent.AddGun(selectedItem);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0002A2C0 File Offset: 0x000284C0
		private void SetUpGuns()
		{
			Store.GunList = new List<Item>();
			for (int i = 0; i < GunStatsLoader.GunStatsList.Count; i++)
			{
				Store.GunList.Add(new Item(GunStatsLoader.GunStatsList[i], i));
			}
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0002A308 File Offset: 0x00028508
		public virtual string GetDescriptionTitle()
		{
			Item selectedItem = this.screen.GetSelectedItem();
			if (selectedItem == null)
			{
				return "";
			}
			return selectedItem.label;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0002A330 File Offset: 0x00028530
		public virtual string GetDescription()
		{
			Item selectedItem = this.screen.GetSelectedItem();
			if (selectedItem == null)
			{
				return "";
			}
			return selectedItem.Description;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0002A358 File Offset: 0x00028558
		public virtual string GetStoreName()
		{
			return "Store";
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0002A35F File Offset: 0x0002855F
		public Item GetCurrentItem()
		{
			return this.screen.GetSelectedItem();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0002A36C File Offset: 0x0002856C
		public void ShowMessage(string text, bool confirm)
		{
			this.MessageActive = true;
			this.MessageConfirmation = confirm;
			this.MessageText = text;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0002A384 File Offset: 0x00028584
		public void FlashErrorCurrency()
		{
			this.CurrencyFlash.DeltaDelegate = new Timer.TimerDelegate(this.FlashTimerDelegate);
			this.CurrencyFlash.IndependentOfTime = true;
			this.CurrencyFlash.Reset();
			this.CurrencyFlash.Start();
			this.FlashColorBG(new Color(1f, 0f, 0f), 0.5f);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0002A3E9 File Offset: 0x000285E9
		private void FlashTimerDelegate(float delta)
		{
			this.CurrentCurrencyColor = Color.Lerp(Color.Red, this.CurrencyColor, delta);
			if (delta >= 1f)
			{
				this.CurrentCurrencyColor = this.CurrencyColor;
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0002A418 File Offset: 0x00028618
		public void FlashColorBG(Color start, float alpha)
		{
			this.StartBGColor = start;
			this.StartBGColor *= alpha;
			this.BGFlash.DeltaDelegate = new Timer.TimerDelegate(this.BGFlashDelta);
			this.BGFlash.IndependentOfTime = true;
			this.BGFlash.Reset();
			this.BGFlash.Start();
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0002A477 File Offset: 0x00028677
		private void BGFlashDelta(float delta)
		{
			this.CurrentBGColor = Color.Lerp(this.StartBGColor, Color.Transparent, delta);
			if (delta >= 1f)
			{
				this.CurrentBGColor = Color.Transparent;
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0002A4A3 File Offset: 0x000286A3
		public void SelectedObject(Item selectedItem)
		{
			this.PurchaseItem(selectedItem);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0002A4AC File Offset: 0x000286AC
		public void SelectionChange(Item highlight)
		{
			if (highlight != null)
			{
				this.DescriptionBox.SetText(this.GetDescription());
				return;
			}
			this.DescriptionBox.SetText("");
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0002A4D4 File Offset: 0x000286D4
		public void Update()
		{
			if (this.JustActivated)
			{
				this.JustActivated = false;
				return;
			}
			if (this.WaitingForData)
			{
				return;
			}
			if (this.MessageActive)
			{
				if (this.MessageConfirmation)
				{
					if (InputManager.APressed(this.parent.Index))
					{
						this.MessageAccepted = true;
						this.MessageActive = false;
						this.JustActivated = true;
						return;
					}
					if (InputManager.BPressed(this.parent.Index))
					{
						this.MessageAccepted = false;
						this.MessageActive = false;
						this.JustActivated = true;
						return;
					}
				}
				else if (InputManager.APressed(this.parent.Index))
				{
					this.MessageAccepted = true;
					this.MessageActive = false;
					this.JustActivated = true;
					return;
				}
				return;
			}
			this.screen.Update();
			this.DescriptionBox.Update();
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0002A59C File Offset: 0x0002879C
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.StoreBackground, this.Position, this.parent.HUDColor);
			this.screen.Draw(spriteBatch);
			this.DrawItemIcons(spriteBatch);
			this.DrawDescription(spriteBatch);
			this.DrawPlayerData(spriteBatch);
			spriteBatch.Draw(Global.Pixel, new Rectangle((int)this.Position.X, (int)this.Position.Y, 1070, 150), this.CurrentBGColor);
			if (this.MessageActive)
			{
				this.DrawMessage(spriteBatch);
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0002A62E File Offset: 0x0002882E
		public void ReInit()
		{
			this.screen.ClearItems();
			this.AddItems();
			this.DescriptionBox.SetText(this.GetDescription());
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0002A652 File Offset: 0x00028852
		public bool MessageShown()
		{
			return this.MessageActive;
		}

		// Token: 0x04000570 RID: 1392
		public Player parent;

		// Token: 0x04000571 RID: 1393
		public ItemScreen screen;

		// Token: 0x04000572 RID: 1394
		private Vector2 ListPosition = new Vector2(168f, 96f);

		// Token: 0x04000573 RID: 1395
		private static List<Item> GunList;

		// Token: 0x04000574 RID: 1396
		public Texture2D StoreBackground;

		// Token: 0x04000575 RID: 1397
		public Texture2D StoreDialog;

		// Token: 0x04000576 RID: 1398
		private Color descriptionColor = new Color(200, 200, 200);

		// Token: 0x04000577 RID: 1399
		private Color ammoColor = Color.Yellow;

		// Token: 0x04000578 RID: 1400
		private StoreManager manager;

		// Token: 0x04000579 RID: 1401
		public Vector2 Position;

		// Token: 0x0400057A RID: 1402
		public Color CurrentCurrencyColor = Color.LightGreen;

		// Token: 0x0400057B RID: 1403
		public Color CurrencyColor = Color.LightGreen;

		// Token: 0x0400057C RID: 1404
		private Timer CurrencyFlash = new Timer(0.5f);

		// Token: 0x0400057D RID: 1405
		private Timer BGFlash = new Timer(0.8f);

		// Token: 0x0400057E RID: 1406
		public Color CurrentBGColor;

		// Token: 0x0400057F RID: 1407
		private Color StartBGColor = Color.Black;

		// Token: 0x04000580 RID: 1408
		private ScrollBox DescriptionBox;

		// Token: 0x04000581 RID: 1409
		private AmmoMeter AmmoAssault;

		// Token: 0x04000582 RID: 1410
		private AmmoMeter AmmoHeavy;

		// Token: 0x04000583 RID: 1411
		private AmmoMeter AmmoExplosive;

		// Token: 0x04000584 RID: 1412
		private AmmoMeter AmmoShells;

		// Token: 0x04000585 RID: 1413
		private AmmoMeter AmmoSpecial;

		// Token: 0x04000586 RID: 1414
		private bool JustActivated = true;

		// Token: 0x04000587 RID: 1415
		public bool MessageActive;

		// Token: 0x04000588 RID: 1416
		private string MessageText = "";

		// Token: 0x04000589 RID: 1417
		private bool MessageConfirmation;

		// Token: 0x0400058A RID: 1418
		private bool MessageAccepted;

		// Token: 0x0400058B RID: 1419
		public bool WaitingForData;
	}
}
