using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000ED RID: 237
	internal class UIArrowControl
	{
		// Token: 0x0600064B RID: 1611 RVA: 0x0002EAB8 File Offset: 0x0002CCB8
		public UIArrowControl(string label, List<string> values, Vector2 topLeft, bool smallFont)
		{
			if (UIArrowControl.LeftArrow == null)
			{
				UIArrowControl.LeftArrow = Global.Content.Load<Texture2D>("Menus\\LeftArrow");
				UIArrowControl.RightArrow = Global.Content.Load<Texture2D>("Menus\\RightArrow");
				UIArrowControl.Middle = Global.Content.Load<Texture2D>("Menus\\ArrowCenter");
			}
			this.Font = Global.StoreFontXtraLarge;
			this.Label = label;
			this.Values = values;
			this.LeftPos = new Rectangle((int)topLeft.X, (int)topLeft.Y, 33, 66);
			this.MiddlePos = new Vector2(topLeft.X + 60f, topLeft.Y);
			this.RightPos = new Rectangle((int)topLeft.X + 60 + 300 + 28, (int)topLeft.Y, 33, 66);
			this.CenterPos = new Vector2(topLeft.X + 60f + 150f, topLeft.Y + 33f);
			this.LabelPos = VerchickMath.CenterText(this.Font, new Vector2(this.CenterPos.X, this.CenterPos.Y - 64f), this.Label);
			this.SmallFont = smallFont;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0002EC09 File Offset: 0x0002CE09
		public void Update()
		{
			this.Inputs();
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0002EC14 File Offset: 0x0002CE14
		private void Inputs()
		{
			this.LeftFlash = false;
			this.RightFlash = false;
			if (this.LeftPos.Contains(InputManager.GetMousePosition()))
			{
				if (InputManager.LeftMouseClicked())
				{
					this.currentIndex--;
					if (this.currentIndex < 0)
					{
						this.currentIndex = this.Values.Count - 1;
					}
				}
				this.LeftFlash = true;
			}
			if (this.RightPos.Contains(InputManager.GetMousePosition()))
			{
				if (InputManager.LeftMouseClicked())
				{
					this.currentIndex++;
					if (this.currentIndex > this.Values.Count - 1)
					{
						this.currentIndex = 0;
					}
				}
				this.RightFlash = true;
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0002ECC4 File Offset: 0x0002CEC4
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.LeftFlash)
			{
				spriteBatch.Draw(UIArrowControl.LeftArrow, this.LeftPos, Color.White);
			}
			else
			{
				spriteBatch.Draw(UIArrowControl.LeftArrow, this.LeftPos, Color.DarkGray);
			}
			spriteBatch.Draw(UIArrowControl.Middle, this.MiddlePos, Color.White);
			if (this.RightFlash)
			{
				spriteBatch.Draw(UIArrowControl.RightArrow, this.RightPos, Color.White);
			}
			else
			{
				spriteBatch.Draw(UIArrowControl.RightArrow, this.RightPos, Color.DarkGray);
			}
			SpriteFont font = this.Font;
			if (this.SmallFont)
			{
				font = Global.StoreFontBig;
			}
			Shadow.DrawString(this.Label, this.Font, this.LabelPos, 1, Color.White, spriteBatch);
			string text = this.Values[this.currentIndex];
			Vector2 pos = VerchickMath.CenterText(font, this.CenterPos, text);
			Shadow.DrawString(text, font, pos, 1, Color.White, spriteBatch);
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x0002EDB5 File Offset: 0x0002CFB5
		public string Value
		{
			get
			{
				return this.Values[this.currentIndex];
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0002EDC8 File Offset: 0x0002CFC8
		public void Select(string val)
		{
			foreach (string b in this.Values)
			{
				if (val == b)
				{
					this.currentIndex = this.Values.IndexOf(val);
				}
			}
		}

		// Token: 0x04000608 RID: 1544
		private static Texture2D LeftArrow;

		// Token: 0x04000609 RID: 1545
		private static Texture2D RightArrow;

		// Token: 0x0400060A RID: 1546
		private static Texture2D Middle;

		// Token: 0x0400060B RID: 1547
		private List<string> Values = new List<string>();

		// Token: 0x0400060C RID: 1548
		private string Label = "";

		// Token: 0x0400060D RID: 1549
		private Rectangle LeftPos;

		// Token: 0x0400060E RID: 1550
		private Rectangle RightPos;

		// Token: 0x0400060F RID: 1551
		private Vector2 MiddlePos;

		// Token: 0x04000610 RID: 1552
		private Vector2 LabelPos;

		// Token: 0x04000611 RID: 1553
		private Vector2 CenterPos;

		// Token: 0x04000612 RID: 1554
		private SpriteFont Font;

		// Token: 0x04000613 RID: 1555
		private bool LeftFlash;

		// Token: 0x04000614 RID: 1556
		private bool RightFlash;

		// Token: 0x04000615 RID: 1557
		private int currentIndex;

		// Token: 0x04000616 RID: 1558
		private bool SmallFont;
	}
}
