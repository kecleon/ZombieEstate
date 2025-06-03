using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000E8 RID: 232
	internal class MenuWithDescription : Menu
	{
		// Token: 0x06000626 RID: 1574 RVA: 0x0002DB18 File Offset: 0x0002BD18
		public MenuWithDescription(bool backButton, int buttonsYOffset, bool lowerRect = false) : base(backButton, new Vector2((float)(Global.ScreenRect.Width / 3), (float)(Global.ScreenRect.Height / 2 + buttonsYOffset)))
		{
			if (lowerRect)
			{
				this.OuterRect.Y = this.OuterRect.Y + 100;
			}
			this.InnerRect = new Rectangle(this.OuterRect.X + MenuWithDescription.borderWidth, this.OuterRect.Y + MenuWithDescription.borderWidth, this.OuterRect.Width - MenuWithDescription.borderWidth * 2, this.OuterRect.Height - MenuWithDescription.borderWidth * 2);
			this.Box = new ScrollBox(" ", this.InnerRect, Global.StoreFont, null, Color.White);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0002DC0C File Offset: 0x0002BE0C
		public MenuWithDescription(bool backButton, bool loweredDesc) : base(backButton, new Vector2((float)(Global.ScreenRect.Width / 3), (float)(Global.ScreenRect.Height / 2 - 100)))
		{
			this.OuterRect.Y = this.OuterRect.Y + 64;
			this.InnerRect = new Rectangle(this.OuterRect.X + MenuWithDescription.borderWidth, this.OuterRect.Y + MenuWithDescription.borderWidth, this.OuterRect.Width - MenuWithDescription.borderWidth * 2, this.OuterRect.Height - MenuWithDescription.borderWidth * 2);
			this.Box = new ScrollBox(" ", this.InnerRect, Global.StoreFont, null, Color.White);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0002DCFD File Offset: 0x0002BEFD
		public override void UpdateMenu()
		{
			if (base.SelectedItem == null)
			{
				this.Box.SetText("");
			}
			else
			{
				this.Box.SetText(base.SelectedItem.Description);
			}
			base.UpdateMenu();
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0002DD38 File Offset: 0x0002BF38
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
			spriteBatch.Draw(Global.Pixel, this.OuterRect, Color.White);
			spriteBatch.Draw(Global.Pixel, this.InnerRect, Color.Black);
			this.Box.Draw(spriteBatch);
		}

		// Token: 0x040005F4 RID: 1524
		private static int borderWidth = 2;

		// Token: 0x040005F5 RID: 1525
		public Rectangle OuterRect = new Rectangle(Global.ScreenRect.Width / 2 + 180, Global.ScreenRect.Height / 2 + 32, 320, 180);

		// Token: 0x040005F6 RID: 1526
		public Rectangle InnerRect;

		// Token: 0x040005F7 RID: 1527
		private ScrollBox Box;
	}
}
