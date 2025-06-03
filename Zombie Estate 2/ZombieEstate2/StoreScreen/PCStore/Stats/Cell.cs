using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.StoreScreen.PCStore.Stats
{
	// Token: 0x02000148 RID: 328
	public class Cell
	{
		// Token: 0x060009F6 RID: 2550 RVA: 0x00051741 File Offset: 0x0004F941
		public Cell(int xTex, int yTex, CellState high, string text, string toolTip, bool type = false)
		{
			this.Src = Global.GetTexRectange(xTex, yTex);
			this.Highlight = high;
			this.Text = text;
			this.ToolTip = toolTip;
			this.mType = type;
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00051780 File Offset: 0x0004F980
		public void SetLocation(int x, int y)
		{
			this.Border = new Rectangle(x, y, Cell.WIDTH, Cell.HEIGHT);
			this.BG = new Rectangle(this.Border.X + 2, this.Border.Y + 2, Cell.WIDTH - 4, Cell.HEIGHT - 4);
			this.Dest = new Rectangle(x + 8, y + 8, 32, 32);
			this.TextPos = new Vector2((float)(x + 44), (float)(y + 12));
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void Update()
		{
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00051804 File Offset: 0x0004FA04
		public void Draw(SpriteBatch spriteBatch)
		{
			float scale = 1f;
			if (this.Highlight == CellState.NA && !this.Hover)
			{
				spriteBatch.Draw(Global.Pixel, this.Border, new Color(0.5f, 0.5f, 0.5f) * scale);
				spriteBatch.Draw(Global.Pixel, this.BG, new Color(0.2f, 0.2f, 0.2f) * scale);
				spriteBatch.Draw(Global.MasterTexture, this.Dest, new Rectangle?(this.Src), new Color(0.55f, 0.55f, 0.55f) * scale);
				Shadow.DrawString("-", Global.StoreFont, this.TextPos, 1, new Color(0.55f, 0.55f, 0.55f) * scale, spriteBatch);
				return;
			}
			if (!this.Hover)
			{
				if (this.Highlight == CellState.Positive)
				{
					spriteBatch.Draw(Global.Pixel, this.Border, new Color(0.5f, 0.7f, 0.5f) * scale);
					spriteBatch.Draw(Global.Pixel, this.BG, new Color(0.2f, 0.5f, 0.2f) * scale);
				}
				else if (this.Highlight == CellState.Negative)
				{
					spriteBatch.Draw(Global.Pixel, this.Border, new Color(0.9f, 0.6f, 0.6f) * scale);
					spriteBatch.Draw(Global.Pixel, this.BG, new Color(0.6f, 0.3f, 0.3f) * scale);
				}
				else
				{
					spriteBatch.Draw(Global.Pixel, this.Border, new Color(0.6f, 0.6f, 0.6f) * scale);
					spriteBatch.Draw(Global.Pixel, this.BG, new Color(0.3f, 0.3f, 0.3f) * scale);
				}
			}
			else if (this.Highlight == CellState.Positive)
			{
				spriteBatch.Draw(Global.Pixel, this.Border, new Color(0.6f, 0.8f, 0.6f) * scale);
				spriteBatch.Draw(Global.Pixel, this.BG, new Color(0.3f, 0.6f, 0.3f) * scale);
			}
			else if (this.Highlight == CellState.Negative)
			{
				spriteBatch.Draw(Global.Pixel, this.Border, new Color(1f, 0.8f, 0.8f) * scale);
				spriteBatch.Draw(Global.Pixel, this.BG, new Color(0.6f, 0.4f, 0.4f) * scale);
			}
			else
			{
				spriteBatch.Draw(Global.Pixel, this.Border, new Color(1f, 1f, 1f) * scale);
				spriteBatch.Draw(Global.Pixel, this.BG, new Color(0.6f, 0.6f, 0.6f) * scale);
			}
			spriteBatch.Draw(Global.MasterTexture, this.Dest, new Rectangle?(this.Src), Color.White * scale);
			Color value = Color.Yellow;
			if (this.Highlight == CellState.Negative)
			{
				value = Color.Red;
			}
			else if (this.Highlight == CellState.Positive)
			{
				value = Color.LightGreen;
			}
			else if (this.Highlight == CellState.NA)
			{
				value = Color.Gray;
			}
			else if (this.Highlight == CellState.Override)
			{
				value = this.OverrideColor;
			}
			if (this.Highlight == CellState.NA)
			{
				Shadow.DrawString("-", Global.StoreFont, this.TextPos, 1, new Color(0.5f, 0.5f, 0.5f) * scale, spriteBatch);
				return;
			}
			if (this.mType)
			{
				Rectangle texRectange = Global.GetTexRectange(65, 50);
				if (this.Text == "Physical")
				{
					texRectange = Global.GetTexRectange(65, 50);
				}
				if (this.Text == "Water")
				{
					texRectange = Global.GetTexRectange(73, 49);
				}
				if (this.Text == "Earth")
				{
					texRectange = Global.GetTexRectange(73, 48);
				}
				if (this.Text == "Fire")
				{
					texRectange = Global.GetTexRectange(72, 48);
				}
				spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)this.TextPos.X, (int)this.TextPos.Y - 4, 32, 32), new Rectangle?(texRectange), Color.White);
				return;
			}
			Shadow.DrawString(this.Text, Global.StoreFont, this.TextPos, 1, value * scale, spriteBatch);
		}

		// Token: 0x04000A89 RID: 2697
		public static int WIDTH = 98;

		// Token: 0x04000A8A RID: 2698
		public static int HEIGHT = 48;

		// Token: 0x04000A8B RID: 2699
		private Rectangle Border;

		// Token: 0x04000A8C RID: 2700
		public Rectangle BG;

		// Token: 0x04000A8D RID: 2701
		private Rectangle Src;

		// Token: 0x04000A8E RID: 2702
		private Rectangle Dest;

		// Token: 0x04000A8F RID: 2703
		private string Text;

		// Token: 0x04000A90 RID: 2704
		public string ToolTip;

		// Token: 0x04000A91 RID: 2705
		private CellState Highlight;

		// Token: 0x04000A92 RID: 2706
		private Vector2 TextPos;

		// Token: 0x04000A93 RID: 2707
		public bool Hover;

		// Token: 0x04000A94 RID: 2708
		public Color OverrideColor = Color.White;

		// Token: 0x04000A95 RID: 2709
		private bool mType;

		// Token: 0x04000A96 RID: 2710
		public int X;

		// Token: 0x04000A97 RID: 2711
		public int Y;
	}
}
