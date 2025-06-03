using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000AD RID: 173
	public class ItemScreen
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x000205FC File Offset: 0x0001E7FC
		public ItemScreen(ItemScreenController controller, Rectangle rectangle, Player parent)
		{
			this.Controller = controller;
			this.items = new Item[this.width, this.height];
			this.Position = rectangle;
			this.parent = parent;
			this.DrawToX = rectangle.Width / (this.itemSize + this.itemSpacing);
			this.timer.IndependentOfTime = true;
			this.flashTimer.IndependentOfTime = true;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000206D1 File Offset: 0x0001E8D1
		public ItemScreen(ItemScreenController controller, Rectangle rectangle, Player parent, int width) : this(controller, rectangle, parent)
		{
			this.width = width;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000206E4 File Offset: 0x0001E8E4
		public ItemScreen(ItemScreenController controller, Rectangle rectangle, Player parent, int width, int height, int size, int spacing)
		{
			this.width = width;
			this.height = height;
			this.itemSize = size;
			this.itemSpacing = spacing;
			this.Controller = controller;
			this.items = new Item[width, height];
			this.Position = rectangle;
			this.parent = parent;
			this.DrawToX = rectangle.Width / (this.itemSize + this.itemSpacing);
			this.timer.IndependentOfTime = true;
			this.flashTimer.IndependentOfTime = true;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000207D1 File Offset: 0x0001E9D1
		public void Update()
		{
			this.Inputs();
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000207DC File Offset: 0x0001E9DC
		private void Inputs()
		{
			if (this.Locked)
			{
				if (InputManager.BPressed(this.parent.Index))
				{
					this.Locked = false;
				}
				return;
			}
			this.Movement();
			if (InputManager.APressed(this.parent.Index) && this.items[this.selectedIndex.X, this.selectedIndex.Y] != null)
			{
				this.flashIntensity = 2f;
				this.flashTimer.DeltaDelegate = new Timer.TimerDelegate(this.FlashDelta);
				this.flashTimer.Reset();
				this.flashTimer.Start();
				this.Controller.SelectedObject(this.items[this.selectedIndex.X, this.selectedIndex.Y]);
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000208B0 File Offset: 0x0001EAB0
		private void Movement()
		{
			if (this.timer.Ready())
			{
				bool flag = false;
				if (InputManager.LeftHeld(this.parent.Index))
				{
					this.selectedIndex.X = this.selectedIndex.X - 1;
					flag = true;
					this.timer.Start();
				}
				if (InputManager.RightHeld(this.parent.Index))
				{
					this.selectedIndex.X = this.selectedIndex.X + 1;
					flag = true;
					this.timer.Start();
				}
				if (InputManager.UpHeld(this.parent.Index))
				{
					this.selectedIndex.Y = this.selectedIndex.Y - 1;
					flag = true;
					this.timer.Start();
				}
				if (InputManager.DownHeld(this.parent.Index))
				{
					this.selectedIndex.Y = this.selectedIndex.Y + 1;
					flag = true;
					this.timer.Start();
				}
				if (this.selectedIndex.X < 0)
				{
					this.selectedIndex.X = 0;
				}
				if (this.selectedIndex.X == this.width)
				{
					this.selectedIndex.X = this.width - 1;
				}
				if (this.selectedIndex.Y < 0)
				{
					this.selectedIndex.Y = 0;
				}
				if (this.selectedIndex.Y == this.height)
				{
					this.selectedIndex.Y = this.height - 1;
				}
				if (this.ViewingX < this.selectedIndex.X - 2)
				{
					this.ViewingX++;
				}
				if (this.ViewingX > this.selectedIndex.X)
				{
					this.ViewingX--;
				}
				this.ViewingX = (int)MathHelper.Clamp((float)this.ViewingX, 0f, (float)(this.width - this.DrawToX));
				if (flag)
				{
					this.Controller.SelectionChange(this.GetSelectedItem());
				}
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00020A8C File Offset: 0x0001EC8C
		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 pos = new Vector2((float)this.Position.X, (float)this.Position.Y);
			float scaleFactor = 1.2f;
			Color color = new Color(this.parent.HUDColor.ToVector4() * scaleFactor);
			for (int i = this.ViewingX; i < Math.Min(this.ViewingX + this.DrawToX, this.width); i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					pos.X = (float)(this.Position.X + this.itemSpacing * (i - this.ViewingX) + (i - this.ViewingX) * this.itemSize);
					pos.Y = (float)(this.Position.Y + this.itemSpacing * j + j * this.itemSize);
					if (!this.DrawBGOver)
					{
						this.DrawItemBG(spriteBatch, ref pos, ref color, i, j);
					}
					if (this.items[i, j] != null)
					{
						if (this.selectedIndex.X == i && this.selectedIndex.Y == j)
						{
							this.items[i, j].DrawItem(spriteBatch, pos, this.itemSize, Color.White, true);
						}
						else if (this.DimOthers)
						{
							this.items[i, j].DrawItem(spriteBatch, pos, this.itemSize, Color.White * 0.5f, false);
						}
						else
						{
							this.items[i, j].DrawItem(spriteBatch, pos, this.itemSize, Color.White, false);
						}
					}
					if (this.DrawBGOver)
					{
						Color white = Color.White;
						this.DrawItemBG(spriteBatch, ref pos, ref white, i, j);
					}
				}
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00020C5C File Offset: 0x0001EE5C
		private void DrawItemBG(SpriteBatch spriteBatch, ref Vector2 pos, ref Color color, int i, int j)
		{
			if (this.selectedIndex.X == i && this.selectedIndex.Y == j)
			{
				if (this.items[i, j] != null)
				{
					spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)pos.X, (int)pos.Y, this.itemSize, this.itemSize), new Rectangle?(new Rectangle((this.items[i, j].OverlayTex.X + 1) * 16, this.items[i, j].OverlayTex.Y * 16, 16, 16)), color * this.flashIntensity);
					return;
				}
				spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)pos.X, (int)pos.Y, this.itemSize, this.itemSize), new Rectangle?(new Rectangle(16, 592, 16, 16)), color * this.flashIntensity);
				return;
			}
			else if (this.Locked)
			{
				if (this.items[i, j] != null)
				{
					spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)pos.X, (int)pos.Y, this.itemSize, this.itemSize), new Rectangle?(new Rectangle(this.items[i, j].OverlayTex.X * 16, this.items[i, j].OverlayTex.Y * 16, 16, 16)), Color.DarkGray);
					return;
				}
				spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)pos.X, (int)pos.Y, this.itemSize, this.itemSize), new Rectangle?(new Rectangle(0, 592, 16, 16)), Color.DarkGray);
				return;
			}
			else
			{
				if (this.items[i, j] != null)
				{
					spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)pos.X, (int)pos.Y, this.itemSize, this.itemSize), new Rectangle?(new Rectangle(this.items[i, j].OverlayTex.X * 16, this.items[i, j].OverlayTex.Y * 16, 16, 16)), color);
					return;
				}
				spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)pos.X, (int)pos.Y, this.itemSize, this.itemSize), new Rectangle?(new Rectangle(0, 592, 16, 16)), color);
				return;
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00020F18 File Offset: 0x0001F118
		public void AddItem(Item item)
		{
			if (this.FillVertically)
			{
				for (int i = 0; i < this.width; i++)
				{
					for (int j = 0; j < this.height; j++)
					{
						if (this.items[i, j] == null)
						{
							this.items[i, j] = item;
							return;
						}
					}
				}
				return;
			}
			for (int k = 0; k < this.height; k++)
			{
				for (int l = 0; l < this.width; l++)
				{
					if (this.items[l, k] == null)
					{
						this.items[l, k] = item;
						return;
					}
				}
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00020FAE File Offset: 0x0001F1AE
		public Item GetSelectedItem()
		{
			return this.items[this.selectedIndex.X, this.selectedIndex.Y];
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00020FD4 File Offset: 0x0001F1D4
		public void ClearItems()
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					this.items[i, j] = null;
				}
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00021014 File Offset: 0x0001F214
		public void ResetItemColors()
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					if (this.items[i, j] != null)
					{
						float scaleFactor = 1.2f;
						Color colorModifier = new Color(this.parent.HUDColor.ToVector4() * scaleFactor);
						this.items[i, j].ColorModifier = colorModifier;
					}
				}
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00021088 File Offset: 0x0001F288
		private void FlashDelta(float delta)
		{
			if (delta >= 1f)
			{
				this.flashIntensity = 1f;
				return;
			}
			this.flashIntensity = 1f + 1f * (1f - delta);
		}

		// Token: 0x0400044F RID: 1103
		private Item[,] items;

		// Token: 0x04000450 RID: 1104
		private Timer timer = new Timer(0.15f);

		// Token: 0x04000451 RID: 1105
		private ItemScreenController Controller;

		// Token: 0x04000452 RID: 1106
		private int itemSize = 32;

		// Token: 0x04000453 RID: 1107
		private int itemSpacing = 4;

		// Token: 0x04000454 RID: 1108
		private int width = 10;

		// Token: 0x04000455 RID: 1109
		private int height = 4;

		// Token: 0x04000456 RID: 1110
		private Point selectedIndex = new Point(0, 0);

		// Token: 0x04000457 RID: 1111
		private Player parent;

		// Token: 0x04000458 RID: 1112
		private Rectangle Position;

		// Token: 0x04000459 RID: 1113
		private int ViewingX;

		// Token: 0x0400045A RID: 1114
		private int DrawToX;

		// Token: 0x0400045B RID: 1115
		public bool FillVertically = true;

		// Token: 0x0400045C RID: 1116
		public bool DrawBGOver;

		// Token: 0x0400045D RID: 1117
		public bool DimOthers = true;

		// Token: 0x0400045E RID: 1118
		public bool Locked;

		// Token: 0x0400045F RID: 1119
		private float flashIntensity = 1f;

		// Token: 0x04000460 RID: 1120
		private Timer flashTimer = new Timer(0.5f);
	}
}
