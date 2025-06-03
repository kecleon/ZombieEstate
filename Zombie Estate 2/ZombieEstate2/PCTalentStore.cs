using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000C5 RID: 197
	internal class PCTalentStore : PCStore
	{
		// Token: 0x060004EB RID: 1259 RVA: 0x00025E10 File Offset: 0x00024010
		public PCTalentStore(Player player) : base(player, 4, 4)
		{
			base.Title = "Talents";
			this.DrawTitle = false;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00025E30 File Offset: 0x00024030
		public override void AddItems()
		{
			for (int i = 0; i < this.Player.Stats.GetTalents().Count; i++)
			{
				base.ItemScreen.AddItem(new PCItem(this.Player.Stats.GetTalents()[i]));
			}
			base.AddItems();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00025E8C File Offset: 0x0002408C
		public override void AddStoreElements()
		{
			this.BackButton = new PCButton("EXIT", Color.LightGray, new Vector2((float)(base.StoreOffsetX + 1058), (float)(base.StoreOffsetY + 620)), "Exit the store.");
			this.BackButton.Pressed = new PCButton.PressedDelegate(this.BackDelegate);
			base.AddStoreElement(this.BackButton);
			this.BuyButton = new PCButton("BUY", Color.Pink, new Vector2((float)(base.StoreOffsetX + 1058), (float)(base.StoreOffsetY + 540)), "Spend 1 Talent Point to activate the selected Talent.");
			this.BuyButton.Pressed = new PCButton.PressedDelegate(this.BuyDelegate);
			base.AddStoreElement(this.BuyButton);
			this.BuyButton.Enabled = false;
			this.port = new Portrait(new Vector2((float)(32 + base.StoreOffsetX), (float)(38 + base.StoreOffsetY)), 192, false);
			this.port.SelectCharacter(this.Player.StartTextureCoord);
			base.AddStoreElement(this.port);
			base.AddStoreElement(new AmmoDisplay(new Vector2((float)(65 + base.StoreOffsetX), (float)(270 + base.StoreOffsetY)), this.Player));
			base.AddStoreElement(new CurrencyDisplay(CurrencyType.Money, new Vector2((float)(40 + base.StoreOffsetX), (float)(465 + base.StoreOffsetY)), this.Player));
			base.AddStoreElement(new CurrencyDisplay(CurrencyType.UpgradeTokens, new Vector2((float)(40 + base.StoreOffsetX), (float)(500 + base.StoreOffsetY)), this.Player));
			this.talentPoints = new CurrencyDisplay(CurrencyType.TalentPoints, new Vector2((float)(40 + base.StoreOffsetX), (float)(535 + base.StoreOffsetY)), this.Player);
			base.AddStoreElement(this.talentPoints);
			base.AddStoreElement(new CurrencyDisplay(CurrencyType.Points, new Vector2((float)(40 + base.StoreOffsetX), (float)(570 + base.StoreOffsetY)), this.Player));
			base.AddStoreElement(new ItemPortrait(372 + base.StoreOffsetX, 512 + base.StoreOffsetY, 128));
			base.AddStoreElement(new ItemDescription(this));
			base.AddStoreElements();
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000260D4 File Offset: 0x000242D4
		private void BackDelegate()
		{
			this.Back();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00024051 File Offset: 0x00022251
		private void Back()
		{
			MasterStore.Deactivate();
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00024125 File Offset: 0x00022325
		private void OkClicked()
		{
			base.CloseMessage();
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0002412D File Offset: 0x0002232D
		public override void LoadStoreBackground()
		{
			this.StoreBackground = Global.Content.Load<Texture2D>("Store\\PCStore\\GunStore");
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000260DC File Offset: 0x000242DC
		public override void SelectionChange(PCItem highlight)
		{
			if (highlight == null)
			{
				return;
			}
			Talent talent = (Talent)highlight.Tag;
			if (talent == null)
			{
				this.BuyButton.Enabled = false;
				base.SelectionChange(highlight);
				return;
			}
			if (talent.CurrentLevel >= 3)
			{
				this.BuyButton.Enabled = false;
				base.SelectionChange(highlight);
				return;
			}
			this.talentPoints.Highlighted = true;
			this.BuyButton.Enabled = true;
			base.SelectionChange(highlight);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0002614C File Offset: 0x0002434C
		private void BuyDelegate()
		{
			if (this.CurrentItem == null)
			{
				return;
			}
			if (this.Player.Stats.GetTalentPoints() <= 0)
			{
				base.ShowMessage("You don't have enough Talent Points!", false, new PCButton.PressedDelegate(base.CloseMessage), null);
				return;
			}
			Talent talent = (Talent)this.CurrentItem.Tag;
			this.Player.Stats.AddTalentPoints(-1);
			TalentManager.ApplyTalent(talent, this.Player);
			base.ReInit();
		}

		// Token: 0x0400050A RID: 1290
		private PCButton BackButton;

		// Token: 0x0400050B RID: 1291
		private Portrait port;

		// Token: 0x0400050C RID: 1292
		private PCButton BuyButton;

		// Token: 0x0400050D RID: 1293
		private CurrencyDisplay talentPoints;
	}
}
