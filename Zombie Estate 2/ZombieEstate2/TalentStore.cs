using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000D7 RID: 215
	internal class TalentStore : Store
	{
		// Token: 0x06000596 RID: 1430 RVA: 0x0002A920 File Offset: 0x00028B20
		public TalentStore(Player parent, StoreManager manager) : base(parent, manager)
		{
			this.screen = new ItemScreen(this, new Rectangle((int)this.Position.X + 26, (int)this.Position.Y + 42, 256, 84), parent, 4, 1, 64, 8);
			this.screen.DrawBGOver = false;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0002A97D File Offset: 0x00028B7D
		public override void SetUpGraphics()
		{
			this.StoreBackground = Global.Content.Load<Texture2D>("Store\\TalentMini");
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0002A994 File Offset: 0x00028B94
		public override void AddItems()
		{
			foreach (Talent talent in this.parent.Stats.GetTalents())
			{
				this.screen.AddItem(new Item(talent.TexCoord, 0, talent.Name, talent));
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0002AA08 File Offset: 0x00028C08
		public override void PurchaseItem(Item selectedItem)
		{
			int currentLevel = ((Talent)selectedItem.Tag).CurrentLevel;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0002AA1C File Offset: 0x00028C1C
		public override void DrawPlayerData(SpriteBatch spriteBatch)
		{
			Vector2 pos = new Vector2(this.Position.X + 812f, this.Position.Y + 10f);
			Shadow.DrawString("Points:", Global.StoreFont, pos, 2, Color.LightGreen, spriteBatch);
			pos.Y += (float)Global.StoreFont.LineSpacing;
			int num = 172 - (int)Global.StoreFontBig.MeasureString(this.parent.Stats.GetTalentPoints().ToString()).X;
			pos.X += (float)num;
			Shadow.DrawString(this.parent.Stats.GetTalentPoints().ToString(), Global.StoreFontBig, pos, 2, Color.Thistle, spriteBatch);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0002AAE8 File Offset: 0x00028CE8
		public override void DrawItemIcons(SpriteBatch spriteBatch)
		{
			Item selectedItem = this.screen.GetSelectedItem();
			if (selectedItem != null)
			{
				Vector2 vector = new Vector2(308f, 16f) + this.Position;
				selectedItem.DrawItem(spriteBatch, vector, 64, Color.White, true);
				spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)vector.X, (int)vector.Y, 64, 64), new Rectangle?(new Rectangle((selectedItem.OverlayTex.X + 1) * 16, selectedItem.OverlayTex.Y * 16, 16, 16)), Color.White);
			}
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0002AB84 File Offset: 0x00028D84
		public override void DrawDataNextToTitle(SpriteBatch spriteBatch)
		{
			Item selectedItem = this.screen.GetSelectedItem();
			if (selectedItem == null)
			{
				return;
			}
			if (selectedItem.cost == -1)
			{
				return;
			}
			int num = (int)Global.StoreFont.MeasureString("Cost: " + selectedItem.cost).X;
			int num2 = (int)Global.StoreFont.MeasureString("Level Required").X;
			Vector2 pos = new Vector2(this.Position.X + 800f, this.Position.Y + 10f);
			pos.X -= (float)num;
			Shadow.DrawString("Cost: " + selectedItem.cost, Global.StoreFont, pos, 2, Color.Thistle, spriteBatch);
			pos = new Vector2(this.Position.X + 335f - (float)(num2 / 2), this.Position.Y + 90f);
			Shadow.DrawString("Level Required", Global.StoreFont, pos, 2, Color.Yellow, spriteBatch);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void UpdateItem(Item item)
		{
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0002AC89 File Offset: 0x00028E89
		public override string GetStoreName()
		{
			return "Talents";
		}

		// Token: 0x0400058F RID: 1423
		private static Point RedBox = new Point(60, 48);

		// Token: 0x04000590 RID: 1424
		private static Point YellowBox = new Point(60, 49);

		// Token: 0x04000591 RID: 1425
		private static Point GreenOne = new Point(60, 50);

		// Token: 0x04000592 RID: 1426
		private static Point GreenTwo = new Point(60, 51);

		// Token: 0x04000593 RID: 1427
		private static Point GreenThree = new Point(60, 52);

		// Token: 0x04000594 RID: 1428
		private static Point GreenOneFinished = new Point(60, 53);
	}
}
