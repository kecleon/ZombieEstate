using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000E6 RID: 230
	public class MenuItem
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x0002D5F4 File Offset: 0x0002B7F4
		public bool Picker
		{
			get
			{
				return this.mPicker;
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0002D5FC File Offset: 0x0002B7FC
		public MenuItem(Vector2 center, string text, int index, bool picker = false)
		{
			this.mIndex = index;
			this.mPicker = picker;
			this.Text = text;
			center.Y += (float)(index * 64);
			this.position = VerchickMath.CenterText(Global.StoreFontBig, center, this.Text);
			this.Collision = new Rectangle((int)center.X - 256, (int)this.position.Y - 2, 512, Global.StoreFontBig.LineSpacing + 4);
			this.bg = new MiscBackground(this.Collision, Color.White * 0.25f);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0002D6A6 File Offset: 0x0002B8A6
		public void Highlight()
		{
			this.highlighted = true;
			SoundEngine.PlaySound("ze2_menuselect", 0.1f);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0002D6BE File Offset: 0x0002B8BE
		public void Unhighlight()
		{
			this.highlighted = false;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0002D6C7 File Offset: 0x0002B8C7
		public void Selected()
		{
			if (this.SelectedFunction != null)
			{
				this.SelectedFunction();
			}
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0002D6DC File Offset: 0x0002B8DC
		public void Selected_Pick(bool positive)
		{
			if (this.mPicker && this.SelectedFunction_Picker != null)
			{
				this.SelectedFunction_Picker(positive);
			}
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0002D6FA File Offset: 0x0002B8FA
		public void Draw(SpriteBatch spriteBatch)
		{
			this.Draw(spriteBatch, 1f);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0002D708 File Offset: 0x0002B908
		public void Draw(SpriteBatch spriteBatch, float alpha)
		{
			this.bg.color = Color.Black * alpha;
			this.bg.Draw(spriteBatch);
			if (this.highlighted)
			{
				this.bg.color = Color.White * 0.95f * alpha;
			}
			else
			{
				this.bg.color = Color.White * 0.25f * alpha;
			}
			this.bg.Draw(spriteBatch);
			Color value = Color.LightGray;
			if (this.highlighted)
			{
				value = Color.Lerp(Color.Red, Color.Pink, Global.Pulse);
			}
			Shadow.DrawString(this.Text, Global.StoreFontBig, this.position, MenuManager.ShadowDistance, value * alpha, Color.Black * alpha, spriteBatch);
			if (this.Picker)
			{
				Color color = Color.Gray;
				Color color2 = Color.Gray;
				if (this.Collision.Contains(InputManager.GetMousePosition()))
				{
					if (this.Collision.X + (int)((float)this.Collision.Width / 2f) > InputManager.GetMousePosition().X)
					{
						color = Color.White * Global.Pulse;
					}
					else
					{
						color2 = Color.White * Global.Pulse;
					}
				}
				Rectangle texRectange = Global.GetTexRectange(4, 20);
				Rectangle destinationRectangle = new Rectangle(this.Collision.X + 4, this.Collision.Y + 4, 32, 32);
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(texRectange), color);
				texRectange = Global.GetTexRectange(5, 20);
				destinationRectangle = new Rectangle(this.Collision.Right - 4 - 32, this.Collision.Y + 4, 32, 32);
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(texRectange), color2);
			}
		}

		// Token: 0x040005E7 RID: 1511
		private Vector2 position;

		// Token: 0x040005E8 RID: 1512
		public string Text;

		// Token: 0x040005E9 RID: 1513
		public Rectangle Collision;

		// Token: 0x040005EA RID: 1514
		private MiscBackground bg;

		// Token: 0x040005EB RID: 1515
		private bool highlighted;

		// Token: 0x040005EC RID: 1516
		public MenuItem.SelectedDelegate SelectedFunction;

		// Token: 0x040005ED RID: 1517
		public MenuItem.SelectedDelegatePicker SelectedFunction_Picker;

		// Token: 0x040005EE RID: 1518
		public string Description;

		// Token: 0x040005EF RID: 1519
		private int mIndex;

		// Token: 0x040005F0 RID: 1520
		private bool mPicker;

		// Token: 0x02000215 RID: 533
		// (Invoke) Token: 0x06000DD5 RID: 3541
		public delegate void SelectedDelegate();

		// Token: 0x02000216 RID: 534
		// (Invoke) Token: 0x06000DD9 RID: 3545
		public delegate void SelectedDelegatePicker(bool positive);
	}
}
