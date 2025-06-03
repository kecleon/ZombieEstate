using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000EE RID: 238
	internal class ScrollBox
	{
		// Token: 0x06000651 RID: 1617 RVA: 0x0002EE30 File Offset: 0x0002D030
		public ScrollBox(string text, Rectangle rect, SpriteFont font, Player parent, Color color)
		{
			if (text == null)
			{
				text = "";
			}
			text = text.Replace('\n', ' ');
			this.linesVisible = rect.Height / font.LineSpacing + 1;
			this.text = VerchickMath.WordWrapWidth(text, rect.Width - 32, font);
			this.rectangle = rect;
			this.font = font;
			this.parent = parent;
			this.textColor = color;
			this.ScrollTimer.Reset();
			this.UpdateCurrentText();
			if (parent == null)
			{
				this.scrolls = false;
			}
			if (ScrollBox.ScrollIcon == null)
			{
				ScrollBox.ScrollIcon = Global.Content.Load<Texture2D>("Store\\RightThumbStick");
				ScrollBox.ArrowUp = Global.Content.Load<Texture2D>("Store\\ArrowUp");
				ScrollBox.ArrowDown = Global.Content.Load<Texture2D>("Store\\ArrowDown");
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0002EF24 File Offset: 0x0002D124
		public void Update()
		{
			if (this.text == string.Empty)
			{
				return;
			}
			this.Movement();
			this.UpdateCurrentText();
			if (this.scrolls)
			{
				if (this.pulsingUp)
				{
					this.pulse += 0.025f;
					if (this.pulse > 0.8f)
					{
						this.pulsingUp = false;
					}
				}
				else
				{
					this.pulse -= 0.025f;
					if (this.pulse < 0.2f)
					{
						this.pulsingUp = true;
					}
				}
			}
			if (this.ScrollTimer.Expired())
			{
				this.ScrollTimer.Reset();
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0002EFC8 File Offset: 0x0002D1C8
		private void Movement()
		{
			if (!this.scrolls)
			{
				return;
			}
			if (this.ScrollTimer.Ready())
			{
				if (this.currentTopLine < 0)
				{
					this.currentTopLine = 0;
				}
				if (this.currentTopLine >= this.totalLines - this.linesVisible - 1)
				{
					this.currentTopLine = Math.Max(0, this.totalLines - 2 - this.linesVisible);
				}
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0002F030 File Offset: 0x0002D230
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.text == string.Empty)
			{
				return;
			}
			if (this.totalLines != 0 && this.linesVisible < this.totalLines - 2)
			{
				Vector2 vector = new Vector2((float)(this.rectangle.X + this.rectangle.Width - 32), (float)(this.rectangle.Y + this.rectangle.Height / 2 - 16));
				spriteBatch.Draw(ScrollBox.ScrollIcon, vector, this.parent.HUDColor);
				if (this.currentTopLine > 0)
				{
					spriteBatch.Draw(ScrollBox.ArrowUp, new Vector2(vector.X, vector.Y - 14f), Color.White * this.pulse);
				}
				if (this.currentTopLine < this.totalLines - this.linesVisible - 2)
				{
					spriteBatch.Draw(ScrollBox.ArrowDown, new Vector2(vector.X, vector.Y + 14f), Color.White * this.pulse);
				}
			}
			Shadow.DrawString(this.currentText, this.font, new Vector2((float)this.rectangle.X, (float)this.rectangle.Y), 1, this.textColor, spriteBatch);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0002F180 File Offset: 0x0002D380
		public void SetText(string text)
		{
			if (text == null)
			{
				text = "";
			}
			text = text.Replace('\n', ' ');
			this.linesVisible = this.rectangle.Height / this.font.LineSpacing;
			this.text = VerchickMath.WordWrapWidth(text, this.rectangle.Width - 32, this.font);
			this.currentTopLine = 0;
			this.UpdateCurrentText();
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0002F1F0 File Offset: 0x0002D3F0
		private void UpdateCurrentText()
		{
			this.text.Trim();
			string[] array = this.text.Split(new char[]
			{
				'\n'
			});
			this.totalLines = array.Length;
			this.currentText = "";
			int num = this.currentTopLine;
			while (num < this.currentTopLine + this.linesVisible + 1 && num < this.totalLines)
			{
				array[num].Trim();
				this.currentText += array[num] + "\n";
				num++;
			}
		}

		// Token: 0x04000617 RID: 1559
		private string text;

		// Token: 0x04000618 RID: 1560
		private Rectangle rectangle;

		// Token: 0x04000619 RID: 1561
		private SpriteFont font;

		// Token: 0x0400061A RID: 1562
		private string currentText;

		// Token: 0x0400061B RID: 1563
		private int linesVisible;

		// Token: 0x0400061C RID: 1564
		private int currentTopLine;

		// Token: 0x0400061D RID: 1565
		private int totalLines;

		// Token: 0x0400061E RID: 1566
		private Player parent;

		// Token: 0x0400061F RID: 1567
		private Timer ScrollTimer = new Timer(0.15f);

		// Token: 0x04000620 RID: 1568
		private Color textColor;

		// Token: 0x04000621 RID: 1569
		private static Texture2D ScrollIcon;

		// Token: 0x04000622 RID: 1570
		private static Texture2D ArrowUp;

		// Token: 0x04000623 RID: 1571
		private static Texture2D ArrowDown;

		// Token: 0x04000624 RID: 1572
		private float pulse = 0.5f;

		// Token: 0x04000625 RID: 1573
		private bool pulsingUp;

		// Token: 0x04000626 RID: 1574
		private bool scrolls = true;
	}
}
