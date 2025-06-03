using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000D9 RID: 217
	internal class UpgradeStore : Store
	{
		// Token: 0x060005AC RID: 1452 RVA: 0x0002B2EC File Offset: 0x000294EC
		public UpgradeStore(Player parent, StoreManager manager) : base(parent, manager)
		{
			this.CurrencyColor = Color.SkyBlue;
			this.CurrentCurrencyColor = Color.SkyBlue;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0002B358 File Offset: 0x00029558
		public override void AddItems()
		{
			this.screen = new ItemScreen(this, new Rectangle((int)this.Position.X + 10, (int)this.Position.Y + 4, 128, 142), this.parent, 2);
			for (int i = 0; i < this.parent.GetGunCount(); i++)
			{
				this.screen.AddItem(new Item(this.parent.Guns[i].stats, this.parent.Guns[i]));
			}
			this.screen.ResetItemColors();
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0002B3F4 File Offset: 0x000295F4
		public override void SetUpGraphics()
		{
			this.StoreBackground = Global.Content.Load<Texture2D>("Store\\UpgradeMini");
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0002B40C File Offset: 0x0002960C
		public override void PurchaseItem(Item selectedItem)
		{
			if (this.parent.Stats.UpgradeTokens <= 0)
			{
				base.FlashErrorCurrency();
				base.ShowMessage("You don't have enough Upgrade Tokens!", false);
				return;
			}
			for (int i = 0; i < this.parent.GetGunCount(); i++)
			{
				if (this.parent.Guns[i].Name == selectedItem.label && this.parent.Guns[i].GetLevel() < 3)
				{
					this.parent.Guns[i].LevelUpGun();
					PlayerStats stats = this.parent.Stats;
					int upgradeTokens = stats.UpgradeTokens;
					stats.UpgradeTokens = upgradeTokens - 1;
					base.ReInit();
				}
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0002B4BC File Offset: 0x000296BC
		public override void DrawPlayerData(SpriteBatch spriteBatch)
		{
			Vector2 vector = this.Position + this.upgradePlayerOffset;
			Rectangle destinationRectangle = new Rectangle((int)vector.X - 82, (int)vector.Y, 32, 32);
			Rectangle value = new Rectangle(96, 592, 16, 16);
			if (this.parent.Stats.UpgradeTokens <= 0)
			{
				value.X = 112;
			}
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(value), Color.White);
			vector.X += -38f;
			vector.Y += 6f;
			Shadow.DrawString("x " + this.parent.Stats.UpgradeTokens.ToString(), Global.StoreFont, vector, 2, this.CurrentCurrencyColor, spriteBatch);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0002B594 File Offset: 0x00029794
		public override void DrawItemIcons(SpriteBatch spriteBatch)
		{
			Item currentItem = base.GetCurrentItem();
			if (currentItem == null)
			{
				return;
			}
			currentItem.DrawUpgradeTransition(spriteBatch, this.Position + this.upgradeOffset);
			this.DrawUpgradeIcons(spriteBatch);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0002B5CC File Offset: 0x000297CC
		private void DrawUpgradeIcons(SpriteBatch spriteBatch)
		{
			Item currentItem = base.GetCurrentItem();
			if (currentItem == null)
			{
				return;
			}
			Vector2 vector = this.Position + this.upgradeIconOffset;
			for (int i = 0; i < 3; i++)
			{
				if (currentItem.level - 2 >= i)
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

		// Token: 0x060005B3 RID: 1459 RVA: 0x0002B6A8 File Offset: 0x000298A8
		public override void DrawDataNextToTitle(SpriteBatch spriteBatch)
		{
			Item selectedItem = this.screen.GetSelectedItem();
			if (selectedItem == null)
			{
				return;
			}
			if (selectedItem.level == 4)
			{
				return;
			}
			Vector2 pos = new Vector2(this.Position.X + 800f - 82f, this.Position.Y + 10f);
			pos.X -= Global.StoreFont.MeasureString("Level " + selectedItem.level).X;
			Shadow.DrawString("Level " + selectedItem.level, Global.StoreFont, pos, 2, Color.Pink, spriteBatch);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0002B754 File Offset: 0x00029954
		public override string GetStoreName()
		{
			return "Upgrades";
		}

		// Token: 0x0400059E RID: 1438
		private Vector2 upgradeOffset = new Vector2(176f, 86f);

		// Token: 0x0400059F RID: 1439
		private Vector2 upgradeIconOffset = new Vector2(181f, 12f);

		// Token: 0x040005A0 RID: 1440
		private Vector2 upgradePlayerOffset = new Vector2(813f, 10f);
	}
}
