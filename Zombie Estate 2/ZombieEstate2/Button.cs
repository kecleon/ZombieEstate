using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x0200006A RID: 106
	internal class Button
	{
		// Token: 0x06000289 RID: 649 RVA: 0x000141E0 File Offset: 0x000123E0
		public Button(Vector2 pos, List<string> states)
		{
			this.rect = new Rectangle((int)pos.X, (int)pos.Y, 80, 48);
			this.labels = new string[states.Count];
			this.stateCount = states.Count;
			for (int i = 0; i < states.Count; i++)
			{
				this.labels[i] = states[i];
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00014268 File Offset: 0x00012468
		public void Update(MouseState prevMouseState)
		{
			this.color = Color.Lerp(this.color, new Color(0.2f, 0.2f, 0.2f), 0.1f);
			if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
			{
				Point value = new Point(Mouse.GetState().X, Mouse.GetState().Y);
				if (this.rect.Contains(value))
				{
					this.currentState++;
					if (this.currentState >= this.stateCount)
					{
						this.currentState = 0;
					}
					this.color = new Color(0.1f, 0.4f, 0.8f);
				}
			}
			if (Mouse.GetState().RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
			{
				Point value2 = new Point(Mouse.GetState().X, Mouse.GetState().Y);
				if (this.rect.Contains(value2))
				{
					this.currentState--;
					if (this.currentState < 0)
					{
						this.currentState = this.stateCount - 1;
					}
					this.color = new Color(0.1f, 0.4f, 0.8f);
				}
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000143AC File Offset: 0x000125AC
		public void Draw(SpriteBatch spriteBatch)
		{
			this.rect.Width = (int)Global.StoreFont.MeasureString(this.labels[this.currentState]).X;
			this.rect.Height = (int)Global.StoreFont.MeasureString(this.labels[this.currentState]).Y;
			this.rect.Width = Math.Max(this.rect.Width, 32);
			spriteBatch.Draw(Global.Pixel, this.rect, this.color);
			Shadow.DrawString(this.labels[this.currentState], Global.StoreFont, new Vector2((float)this.rect.X, (float)this.rect.Y), 1, Color.White, spriteBatch);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00014478 File Offset: 0x00012678
		public string CurrentValue()
		{
			return this.labels[this.currentState];
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00014488 File Offset: 0x00012688
		public void SetValue(string label)
		{
			for (int i = 0; i < this.labels.Length; i++)
			{
				if (this.labels[i] == label)
				{
					this.currentState = i;
					return;
				}
			}
		}

		// Token: 0x0400027D RID: 637
		private string[] labels;

		// Token: 0x0400027E RID: 638
		private Rectangle rect;

		// Token: 0x0400027F RID: 639
		private int currentState;

		// Token: 0x04000280 RID: 640
		private int stateCount;

		// Token: 0x04000281 RID: 641
		private Color color = new Color(0.2f, 0.2f, 0.2f);
	}
}
