using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000D8 RID: 216
	public class UpgradeScreen : ItemScreenController, ShopScreen
	{
		// Token: 0x060005A0 RID: 1440 RVA: 0x0002ACF4 File Offset: 0x00028EF4
		public UpgradeScreen(Player parent, StoreManager manager)
		{
			this.parent = parent;
			this.SetUpGuns();
			this.Position = new Vector2(64f, 72f);
			this.screen = new ItemScreen(this, new Rectangle((int)this.Position.X + 10, (int)this.Position.Y + 10, 242, 142), parent);
			this.StoreBackground = Global.Content.Load<Texture2D>("Store\\UpgradeMini");
			this.AddAllGuns();
			this.manager = manager;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0002ADC0 File Offset: 0x00028FC0
		private void AddAllGuns()
		{
			this.screen = null;
			this.screen = new ItemScreen(this, new Rectangle((int)this.ListPosition.X, (int)this.ListPosition.Y, 128, 128), this.parent);
			for (int i = 0; i < this.parent.GetGunCount(); i++)
			{
				this.screen.AddItem(new Item(this.parent.Guns[i].stats, this.parent.Guns[i]));
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0002AE54 File Offset: 0x00029054
		private void DrawPlayerData(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2(940f, 190f);
			Rectangle destinationRectangle = new Rectangle((int)vector.X, (int)vector.Y, 48, 48);
			Rectangle value = new Rectangle(96, 592, 16, 16);
			if (this.parent.Stats.UpgradeTokens <= 0)
			{
				value.X = 112;
			}
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(value), Color.White);
			vector.X += 54f;
			vector.Y += 6f;
			Shadow.DrawString(this.parent.Stats.UpgradeTokens.ToString(), Global.StoreFont, vector, 2, Color.SkyBlue, spriteBatch);
			vector.X = 808f - Global.StoreFont.MeasureString("Player " + (this.parent.Index + 1)).X / 2f;
			vector.Y = 80f;
			Shadow.DrawString("Player " + (this.parent.Index + 1), Global.StoreFont, vector, 2, Color.White, spriteBatch);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0002AF94 File Offset: 0x00029194
		private void DrawData(SpriteBatch spriteBatch)
		{
			Item selectedItem = this.screen.GetSelectedItem();
			if (selectedItem != null)
			{
				Vector2 vector = VerchickMath.CenterText(Global.StoreFont, new Vector2(616f, 220f), selectedItem.label);
				Shadow.DrawString(selectedItem.label, Global.StoreFont, vector, 2, Color.White, spriteBatch);
				vector.X = 496f;
				vector.Y = 380f;
				Shadow.DrawString(selectedItem.Description, Global.StoreFont, vector, 2, this.descriptionColor, spriteBatch);
				vector.X = 1124f - Global.StoreFont.MeasureString("Cost: $" + selectedItem.cost).X;
				vector.Y = 310f;
				Shadow.DrawString("Cost: $" + selectedItem.cost, Global.StoreFont, vector, 2, Color.LightGreen, spriteBatch);
				vector.X = 508f;
				vector.Y = 316f;
				for (int i = 0; i < 3; i++)
				{
					if (selectedItem.level - 2 >= i)
					{
						Rectangle destinationRectangle = new Rectangle((int)vector.X + 48 * i, (int)vector.Y, 48, 48);
						Rectangle value = new Rectangle(96, 592, 16, 16);
						spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(value), Color.White);
					}
					else
					{
						Rectangle destinationRectangle2 = new Rectangle((int)vector.X + 48 * i, (int)vector.Y, 48, 48);
						Rectangle value2 = new Rectangle(112, 592, 16, 16);
						spriteBatch.Draw(Global.MasterTexture, destinationRectangle2, new Rectangle?(value2), Color.White);
					}
				}
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0002B148 File Offset: 0x00029348
		private void BuyWeapon(Item selectedItem)
		{
			if (this.parent.Stats.UpgradeTokens <= 0)
			{
				return;
			}
			for (int i = 0; i < this.parent.GetGunCount(); i++)
			{
				if (this.parent.Guns[i].Name == selectedItem.label)
				{
					this.parent.Guns[i].LevelUpGun();
				}
			}
			PlayerStats stats = this.parent.Stats;
			int upgradeTokens = stats.UpgradeTokens;
			stats.UpgradeTokens = upgradeTokens - 1;
			this.ReInit();
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0002B1D0 File Offset: 0x000293D0
		private void SetUpGuns()
		{
			this.GunList = new List<Item>();
			for (int i = 0; i < GunStatsLoader.GunStatsList.Count; i++)
			{
				this.GunList.Add(new Item(GunStatsLoader.GunStatsList[i], i));
			}
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0002B219 File Offset: 0x00029419
		public void SelectedObject(Item selectedItem)
		{
			this.BuyWeapon(selectedItem);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void SelectionChange(Item highlight)
		{
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0002B222 File Offset: 0x00029422
		public void Update()
		{
			this.screen.Update();
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0002B230 File Offset: 0x00029430
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.StoreBackground, new Rectangle(0, 0, Global.ScreenRect.Width, Global.ScreenRect.Height), this.parent.HUDColor);
			this.screen.Draw(spriteBatch);
			this.DrawData(spriteBatch);
			this.DrawPlayerData(spriteBatch);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0002B28C File Offset: 0x0002948C
		public void ReInit()
		{
			this.screen.ClearItems();
			for (int i = 0; i < this.parent.GetGunCount(); i++)
			{
				this.screen.AddItem(new Item(this.parent.Guns[i].stats, this.parent.Guns[i]));
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0000B472 File Offset: 0x00009672
		public bool MessageShown()
		{
			return false;
		}

		// Token: 0x04000595 RID: 1429
		private Player parent;

		// Token: 0x04000596 RID: 1430
		private ItemScreen screen;

		// Token: 0x04000597 RID: 1431
		private Vector2 ListPosition = new Vector2(168f, 96f);

		// Token: 0x04000598 RID: 1432
		private List<Item> GunList;

		// Token: 0x04000599 RID: 1433
		private Texture2D StoreBackground;

		// Token: 0x0400059A RID: 1434
		private Color descriptionColor = new Color(200, 200, 200);

		// Token: 0x0400059B RID: 1435
		private Color ammoColor = Color.Yellow;

		// Token: 0x0400059C RID: 1436
		private Vector2 Position;

		// Token: 0x0400059D RID: 1437
		private StoreManager manager;
	}
}
