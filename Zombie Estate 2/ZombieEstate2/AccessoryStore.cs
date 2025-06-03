using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000AC RID: 172
	internal class AccessoryStore : Store
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x000203B8 File Offset: 0x0001E5B8
		public AccessoryStore(Player parent, StoreManager manager) : base(parent, manager)
		{
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000203C4 File Offset: 0x0001E5C4
		public override void AddItems()
		{
			this.screen.AddItem(new Item(new Point(0, 62), 0, "Top Hat", "Only for the most prestigous gentleman."));
			this.screen.AddItem(new Item(new Point(1, 62), 0, "Party Hat", "PARTY HAT!!! WOO!"));
			this.screen.AddItem(new Item(new Point(2, 62), 0, "Bunny Ears", "?"));
			this.screen.AddItem(new Item(new Point(3, 62), 0, "Cat Ears", "?"));
			this.screen.AddItem(new Item(new Point(4, 62), 0, "Antena", "?"));
			this.screen.AddItem(new Item(new Point(5, 62), 0, "Beanie", "?"));
			this.screen.ResetItemColors();
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000204B0 File Offset: 0x0001E6B0
		public override void PurchaseItem(Item selectedItem)
		{
			if (this.parent.AccessoryName == selectedItem.label)
			{
				this.parent.AccessoryName = "None";
				this.parent.AccessoryTexCoord = new Point(63, 63);
				this.screen.ResetItemColors();
				return;
			}
			this.parent.AccessoryTexCoord = selectedItem.texCoord;
			this.parent.AccessoryName = selectedItem.label;
			this.screen.ResetItemColors();
			selectedItem.ColorModifier = Color.White;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00020540 File Offset: 0x0001E740
		public override void DrawPlayerData(SpriteBatch spriteBatch)
		{
			Vector2 pos = new Vector2(this.Position.X + 812f - 82f, this.Position.Y + 10f);
			Shadow.DrawString("Equipped:", Global.StoreFont, pos, 2, Color.Pink, spriteBatch);
			pos.Y += (float)Global.StoreFont.LineSpacing;
			int num = 172 - (int)Global.StoreFont.MeasureString(this.parent.AccessoryName).X;
			pos.X += (float)num;
			Shadow.DrawString(this.parent.AccessoryName, Global.StoreFont, pos, 2, Color.White, spriteBatch);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000205F5 File Offset: 0x0001E7F5
		public override string GetStoreName()
		{
			return "Accessories";
		}
	}
}
