using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Buffs;

namespace ZombieEstate2
{
	// Token: 0x02000015 RID: 21
	public class PlayerBuffManager : BuffManager
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00003AE4 File Offset: 0x00001CE4
		public PlayerBuffManager(Player p) : base(p)
		{
			this.Player = p;
			this.HUDColor = p.HUDColor;
			Rectangle safeScreenArea = Global.GetSafeScreenArea();
			switch (this.Player.Index)
			{
			case 0:
				this.mPosition = new Vector2((float)safeScreenArea.X, (float)(safeScreenArea.Y + 20 + 125));
				return;
			case 1:
				this.mPosition = new Vector2((float)(safeScreenArea.Right - 192), (float)(safeScreenArea.Y + 20 + 125));
				return;
			case 2:
				this.mPosition = new Vector2((float)safeScreenArea.X, (float)(safeScreenArea.Bottom - 123 - 20 + 125));
				return;
			case 3:
				this.mPosition = new Vector2((float)(safeScreenArea.Right - 192), (float)(safeScreenArea.Bottom - 123 - 20 + 125));
				return;
			default:
				this.mPosition = new Vector2((float)safeScreenArea.X, (float)safeScreenArea.Y);
				return;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003BE8 File Offset: 0x00001DE8
		public override void Draw(SpriteBatch spriteBatch)
		{
			int num = 0;
			Buff buff = null;
			foreach (Buff buff2 in this.Buffs)
			{
				if (buff2.Drawable)
				{
					bool flag = buff2.Draw(spriteBatch, num, (int)this.mPosition.X, (int)this.mPosition.Y, this.HUDColor);
					num++;
					if (flag)
					{
						buff = buff2;
					}
				}
			}
			if (buff != null)
			{
				string name = buff.Name;
				Rectangle rectangle = new Rectangle(32, Global.ScreenRect.Height - 380, 256, 120);
				Vector2 pos = VerchickMath.CenterText(Global.StoreFontBig, new Vector2((float)(rectangle.X + 128), (float)(Global.ScreenRect.Height - 355)), name);
				Vector2 pos2 = new Vector2((float)(rectangle.X + 8), (float)(rectangle.Y + 40));
				spriteBatch.Draw(Global.Pixel, rectangle, Color.Black * 0.5f);
				int num2 = 6;
				rectangle.X -= num2 / 2;
				rectangle.Y -= num2 / 2;
				rectangle.Width += num2;
				rectangle.Height += num2;
				spriteBatch.Draw(Global.Pixel, rectangle, Color.Black * 0.5f);
				Shadow.DrawString(name, Global.StoreFontBig, pos, 1, Color.White, spriteBatch);
				Shadow.DrawString(VerchickMath.WordWrapWidth(buff.Description, 240, Global.StoreFont), Global.StoreFont, pos2, 1, buff.Positive ? Color.LightGreen : Color.Red, spriteBatch);
			}
		}

		// Token: 0x04000038 RID: 56
		private Color HUDColor;

		// Token: 0x04000039 RID: 57
		private Player Player;

		// Token: 0x0400003A RID: 58
		private Vector2 mPosition;
	}
}
