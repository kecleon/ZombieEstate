using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000C7 RID: 199
	internal class PCItemScreen
	{
		// Token: 0x06000505 RID: 1285 RVA: 0x00026944 File Offset: 0x00024B44
		public PCItemScreen(int width, int height, Rectangle area, PCItemScreenController controller, Vector2 offset)
		{
			if (PCItemScreen.RightArrow == null)
			{
				PCItemScreen.RightArrow = Global.Content.Load<Texture2D>("Store\\PCStore\\RightArrow");
				PCItemScreen.LeftArrow = Global.Content.Load<Texture2D>("Store\\PCStore\\LeftArrow");
			}
			this.bounds = area;
			this.bounds.Width = this.bounds.Width - 64;
			this.bounds.X = this.bounds.X - 4;
			this.width = width;
			this.height = height;
			this.widthBound = (area.Width - 64) / (PCItem.Size + 8);
			this.Items = new PCItem[width, height];
			this.LeftArrowRect = new Rectangle(area.X + (int)offset.X, area.Y + (area.Height - 128) / 2 + (int)offset.Y, 32, 128);
			this.RightArrowRect = new Rectangle(area.X + area.Width - 32 + (int)offset.X, area.Y + (area.Height - 128) / 2 + (int)offset.Y, 32, 128);
			this.Controller = controller;
			this.Offset = offset;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00026A80 File Offset: 0x00024C80
		public void AddItem(PCItem item)
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					if (this.Items[i, j] == null)
					{
						this.Items[i, j] = item;
						return;
					}
				}
			}
			Terminal.WriteMessage("ERROR: NO SPACE LEFT IN PCITEMSCREEN!!!", MessageType.ERROR);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00026AD8 File Offset: 0x00024CD8
		public void FillEmptyItems()
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					if (this.Items[i, j] == null)
					{
						this.Items[i, j] = new PCItem(new Point(63, 63), false);
					}
				}
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00026B34 File Offset: 0x00024D34
		public void Update()
		{
			Point mousePosition = InputManager.GetMousePosition();
			if (InputManager.LeftMouseClicked())
			{
				int num = this.currentLeftBound;
				while (num < this.width && num < this.currentLeftBound + this.widthBound)
				{
					for (int i = 0; i < this.height; i++)
					{
						if (this.Items[num, i] != null && this.Items[num, i].CollisionRectangle.Contains(mousePosition))
						{
							this.SelectedItem = this.Items[num, i];
							SoundEngine.PlaySound("ze2_menuselect", 0.25f);
						}
					}
					num++;
				}
			}
			if (this.LeftArrowRect.Contains(InputManager.GetMousePosition()) && InputManager.LeftMouseClicked() && this.currentLeftBound > 0)
			{
				this.currentLeftBound--;
			}
			if (this.RightArrowRect.Contains(InputManager.GetMousePosition()) && InputManager.LeftMouseClicked() && this.currentLeftBound + this.widthBound < this.width)
			{
				this.currentLeftBound++;
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00026C44 File Offset: 0x00024E44
		public void Draw(SpriteBatch spriteBatch, Color color)
		{
			Vector2 position = new Vector2(0f, 0f);
			int num = this.currentLeftBound;
			while (num < this.width && num < this.currentLeftBound + this.widthBound)
			{
				for (int i = 0; i < this.height; i++)
				{
					if (this.Items[num, i] != null)
					{
						position.X = (float)(this.bounds.X + (num - this.currentLeftBound) * PCItem.Size + PCItemScreen.BorderSize * (num - this.currentLeftBound)) + this.Offset.X + 64f;
						position.Y = (float)(this.bounds.Y + i * PCItem.Size + PCItemScreen.BorderSize * i) + this.Offset.Y;
						this.Items[num, i].DrawItem(spriteBatch, position, color);
					}
				}
				num++;
			}
			this.DrawArrows(spriteBatch, color);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00026D48 File Offset: 0x00024F48
		private void DrawArrows(SpriteBatch spriteBatch, Color color)
		{
			Color color2 = color * (0.5f + 0.5f * Global.Pulse);
			if (this.LeftArrowRect.Contains(InputManager.GetMousePosition()))
			{
				color2 = Color.Lerp(color, Color.White, 0.5f + 0.5f * Global.Pulse);
			}
			if (this.currentLeftBound == 0)
			{
				color2 = Color.Gray;
			}
			spriteBatch.Draw(PCItemScreen.LeftArrow, this.LeftArrowRect, color2);
			color2 = color * (0.5f + 0.5f * Global.Pulse);
			if (this.RightArrowRect.Contains(InputManager.GetMousePosition()))
			{
				color2 = Color.Lerp(color, Color.White, 0.5f + 0.5f * Global.Pulse);
			}
			if (this.currentLeftBound + this.widthBound >= this.width || this.widthBound > this.width)
			{
				color2 = Color.Gray;
			}
			spriteBatch.Draw(PCItemScreen.RightArrow, this.RightArrowRect, color2);
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x00026E3E File Offset: 0x0002503E
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x00026E46 File Offset: 0x00025046
		public PCItem SelectedItem
		{
			get
			{
				return this.selectedItem;
			}
			set
			{
				this.DeselectAll();
				this.selectedItem = value;
				this.selectedItem.Selected = true;
				this.Controller.SelectionChange(this.selectedItem);
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00026E72 File Offset: 0x00025072
		public void SelectFirstItem()
		{
			this.SelectedItem = this.Items[0, 0];
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00026E88 File Offset: 0x00025088
		public void SelectName(string name)
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					if (this.Items[i, j] != null && this.Items[i, j].Title == name)
					{
						this.SelectedItem = this.Items[i, j];
						return;
					}
				}
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00026EF4 File Offset: 0x000250F4
		public PCItem GetFromName(string name)
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					if (this.Items[i, j] != null && this.Items[i, j].Title == name)
					{
						return this.Items[i, j];
					}
				}
			}
			return null;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00026F5C File Offset: 0x0002515C
		public void DisableName(string name)
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					if (this.Items[i, j] != null && this.Items[i, j].Title == name)
					{
						this.Items[i, j].Enabled = false;
						return;
					}
				}
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00026FC8 File Offset: 0x000251C8
		public void EnableName(string name)
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					if (this.Items[i, j] != null && this.Items[i, j].Title == name)
					{
						this.Items[i, j].Enabled = true;
						return;
					}
				}
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00027034 File Offset: 0x00025234
		public void EnableAll()
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					if (this.Items[i, j] != null)
					{
						this.Items[i, j].Enabled = true;
					}
				}
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00027088 File Offset: 0x00025288
		private void DeselectAll()
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					if (this.Items[i, j] != null)
					{
						this.Items[i, j].Selected = false;
					}
				}
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000270DC File Offset: 0x000252DC
		public void LockLastSpot()
		{
			PCItem pcitem = this.Items[this.width - 1, this.height - 1];
			if (pcitem != null && pcitem.Tag == null)
			{
				this.Items[this.width - 1, this.height - 1].Lock();
			}
		}

		// Token: 0x04000527 RID: 1319
		private PCItem[,] Items;

		// Token: 0x04000528 RID: 1320
		private Rectangle bounds;

		// Token: 0x04000529 RID: 1321
		private int currentLeftBound;

		// Token: 0x0400052A RID: 1322
		private int widthBound;

		// Token: 0x0400052B RID: 1323
		private static int BorderSize = 6;

		// Token: 0x0400052C RID: 1324
		private int width;

		// Token: 0x0400052D RID: 1325
		private int height;

		// Token: 0x0400052E RID: 1326
		private PCItemScreenController Controller;

		// Token: 0x0400052F RID: 1327
		private PCItem selectedItem;

		// Token: 0x04000530 RID: 1328
		private Vector2 Offset;

		// Token: 0x04000531 RID: 1329
		private Rectangle RightArrowRect;

		// Token: 0x04000532 RID: 1330
		private Rectangle LeftArrowRect;

		// Token: 0x04000533 RID: 1331
		private static Texture2D RightArrow;

		// Token: 0x04000534 RID: 1332
		private static Texture2D LeftArrow;
	}
}
