using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000AE RID: 174
	public class Item
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x000210B7 File Offset: 0x0001F2B7
		public Item(Point tex, int index, string label)
		{
			this.texCoord = tex;
			this.index = index;
			this.label = label;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000210ED File Offset: 0x0001F2ED
		public Item(Point tex, int index, string label, object tag) : this(tex, index, label)
		{
			this.Tag = tag;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00021100 File Offset: 0x0001F300
		public Item(GunStats stats, int index)
		{
			this.texCoord = new Point(stats.GunXCoord, stats.GunYCoord + 1);
			this.index = index;
			this.label = stats.GunName;
			this.cost = Item.RoundCost((float)stats.Cost);
			this.AmmoType = stats.AmmoType;
			this.Description = stats.StoreDescription;
			this.offset = -2;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0002118A File Offset: 0x0001F38A
		public Item(Point tex, int index, string label, string description)
		{
			this.texCoord = tex;
			this.index = index;
			this.label = label;
			this.Description = description;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x000211C8 File Offset: 0x0001F3C8
		public Item(GunStats stats, Gun gun)
		{
			this.offset = -2;
			this.upgrade = true;
			if (gun.GetLevel() < 3)
			{
				this.level = gun.GetLevel() + 1;
				this.texCoord = new Point(stats.GunXCoord + (this.level - 1) * 2, stats.GunYCoord + 1);
				this.texNextLevel = new Point(stats.GunXCoord + this.level * 2, stats.GunYCoord + 1);
				this.label = stats.GunName;
				this.cost = stats.Cost;
				this.AmmoType = stats.AmmoType;
				this.Description = stats.StoreDescription;
				return;
			}
			this.level = gun.GetLevel() + 1;
			this.texCoord = new Point(stats.GunXCoord + (this.level - 1) * 2, stats.GunYCoord + 1);
			this.texNextLevel = new Point(4, 37);
			this.label = stats.GunName;
			this.cost = stats.Cost;
			this.AmmoType = stats.AmmoType;
			this.Description = "Gun fully upgraded.";
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00021304 File Offset: 0x0001F504
		public void DrawItem(SpriteBatch spriteBatch, Vector2 pos, int sizeWidthHeight, Color color, bool selected)
		{
			spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)pos.X + this.offset, (int)pos.Y + this.offset, sizeWidthHeight, sizeWidthHeight), new Rectangle?(new Rectangle(this.texCoord.X * 16, this.texCoord.Y * 16, 16, 16)), color);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0002136C File Offset: 0x0001F56C
		public void DrawUpgradeTransition(SpriteBatch spriteBatch, Vector2 pos)
		{
			spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)pos.X + this.offset, (int)pos.Y + this.offset, 64, 64), new Rectangle?(new Rectangle(this.texCoord.X * 16, this.texCoord.Y * 16, 16, 16)), Color.White);
			spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)pos.X + this.offset + 93, (int)pos.Y + this.offset, 64, 64), new Rectangle?(new Rectangle(this.texNextLevel.X * 16, this.texNextLevel.Y * 16, 16, 16)), Color.White);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0002143A File Offset: 0x0001F63A
		public static int RoundCost(float cost)
		{
			return (int)(cost * (float)Global.UPPERGUNLIMIT) / 5 * 5;
		}

		// Token: 0x04000461 RID: 1121
		public Point texCoord;

		// Token: 0x04000462 RID: 1122
		private Point texNextLevel;

		// Token: 0x04000463 RID: 1123
		private int index;

		// Token: 0x04000464 RID: 1124
		public string label;

		// Token: 0x04000465 RID: 1125
		public int cost;

		// Token: 0x04000466 RID: 1126
		public string Description;

		// Token: 0x04000467 RID: 1127
		public AmmoType AmmoType;

		// Token: 0x04000468 RID: 1128
		public int level;

		// Token: 0x04000469 RID: 1129
		private bool upgrade;

		// Token: 0x0400046A RID: 1130
		public Color ColorModifier = Color.White;

		// Token: 0x0400046B RID: 1131
		public Point OverlayTex = new Point(0, 37);

		// Token: 0x0400046C RID: 1132
		public int offset;

		// Token: 0x0400046D RID: 1133
		public object Tag;
	}
}
