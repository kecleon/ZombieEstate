using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.StoreScreen.PCStore.Stats;

namespace ZombieEstate2
{
	// Token: 0x020000CF RID: 207
	internal class StatComparer : StoreElement
	{
		// Token: 0x0600055D RID: 1373 RVA: 0x0002921A File Offset: 0x0002741A
		public void AddCell(Cell c)
		{
			if (c != null)
			{
				this.Cells.Add(c);
			}
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0002922C File Offset: 0x0002742C
		public StatComparer(Rectangle pos, PCStore store, Player player)
		{
			this.Position = pos;
			this.Border = new Rectangle(pos.X - 2, pos.Y - 2, pos.Width + 4, pos.Height + 4);
			this.Cells = new List<Cell>();
			this.mTextPos = new Vector2((float)(pos.X + 8), (float)(pos.Bottom - 80));
			this.mPlayer = player;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x000292A2 File Offset: 0x000274A2
		public virtual void BuildList()
		{
			this.SetLocations();
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x000292AC File Offset: 0x000274AC
		private void SetLocations()
		{
			int num = Math.Max(1, this.Position.Width / Cell.WIDTH);
			int num2 = 0;
			int num3 = 0;
			this.WIDTH = num;
			this.HEIGHT = this.Cells.Count / num;
			this.mArray = new Cell[num, this.Cells.Count / num];
			foreach (Cell cell in this.Cells)
			{
				cell.SetLocation(this.Position.X + num2 * Cell.WIDTH, this.Position.Y + num3 * Cell.HEIGHT);
				this.mArray[num2, num3] = cell;
				cell.X = num2;
				cell.Y = num3;
				num2++;
				if (num2 >= num)
				{
					num2 = 0;
					num3++;
				}
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x000293A4 File Offset: 0x000275A4
		public override void Update()
		{
			foreach (Cell cell in this.Cells)
			{
				cell.Update();
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveSouth, this.mPlayer.Index, false))
			{
				SoundEngine.PlaySound("ze2_menunav", 0.25f);
				this.mY++;
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveNorth, this.mPlayer.Index, false))
			{
				SoundEngine.PlaySound("ze2_menunav", 0.25f);
				this.mY--;
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveEast, this.mPlayer.Index, false))
			{
				SoundEngine.PlaySound("ze2_menunav", 0.25f);
				this.mX++;
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveWest, this.mPlayer.Index, false))
			{
				SoundEngine.PlaySound("ze2_menunav", 0.25f);
				this.mX--;
			}
			this.mX = VerchickMath.Wrap(this.mX, this.WIDTH);
			this.mY = VerchickMath.Wrap(this.mY, this.HEIGHT);
			Point mousePosition = InputManager.GetMousePosition();
			if (this.mPrevMousePos != mousePosition)
			{
				Cell cell2 = null;
				foreach (Cell cell3 in this.Cells)
				{
					if (cell3.BG.Contains(mousePosition))
					{
						cell2 = cell3;
						break;
					}
				}
				if (cell2 != null)
				{
					this.mX = cell2.X;
					this.mY = cell2.Y;
				}
			}
			this.mPrevMousePos = InputManager.GetMousePosition();
			base.Update();
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0002957C File Offset: 0x0002777C
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Global.Pixel, this.Border, Color.White);
			spriteBatch.Draw(Global.Pixel, this.Position, Color.Black);
			Cell cell = null;
			foreach (Cell cell2 in this.Cells)
			{
				cell2.Hover = false;
				if (this.mArray[this.mX, this.mY] == cell2)
				{
					cell2.Hover = true;
					cell = cell2;
				}
				cell2.Draw(spriteBatch);
			}
			if (cell != null)
			{
				Shadow.DrawString(cell.ToolTip, Global.StoreFont, this.mTextPos, 1, Color.White, spriteBatch);
			}
			base.Draw(spriteBatch);
		}

		// Token: 0x04000557 RID: 1367
		private Rectangle Position;

		// Token: 0x04000558 RID: 1368
		private Rectangle Border;

		// Token: 0x04000559 RID: 1369
		private List<Cell> Cells;

		// Token: 0x0400055A RID: 1370
		private Cell[,] mArray;

		// Token: 0x0400055B RID: 1371
		private int mX;

		// Token: 0x0400055C RID: 1372
		private int mY;

		// Token: 0x0400055D RID: 1373
		private int WIDTH;

		// Token: 0x0400055E RID: 1374
		private int HEIGHT;

		// Token: 0x0400055F RID: 1375
		private Player mPlayer;

		// Token: 0x04000560 RID: 1376
		private Vector2 mTextPos;

		// Token: 0x04000561 RID: 1377
		private Point mPrevMousePos;
	}
}
