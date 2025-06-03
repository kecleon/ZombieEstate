using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000B9 RID: 185
	internal class CurrencyDisplay : StoreElement
	{
		// Token: 0x060004A3 RID: 1187 RVA: 0x000223B8 File Offset: 0x000205B8
		public CurrencyDisplay(CurrencyType type, Vector2 pos, Player parent)
		{
			this.Position = pos;
			this.Type = type;
			this.player = parent;
			this.bgColor = parent.HUDColor * 0.5f;
			this.bg = new MiscBackground(new Rectangle((int)pos.X - 2, (int)pos.Y, 180, 34), this.bgColor);
			this.currencyDest = new Rectangle((int)pos.X, (int)pos.Y, 32, 32);
			switch (type)
			{
			case CurrencyType.Money:
				this.currencyImageSource = Global.GetTexRectange(4, 44);
				return;
			case CurrencyType.UpgradeTokens:
				this.currencyImageSource = Global.GetTexRectange(6, 37);
				return;
			case CurrencyType.TalentPoints:
				this.currencyImageSource = Global.GetTexRectange(8, 37);
				return;
			case CurrencyType.Points:
				this.currencyImageSource = Global.GetTexRectange(10, 37);
				return;
			default:
				return;
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00022494 File Offset: 0x00020694
		public override void Update()
		{
			if (this.bg.rectangle.Contains(InputManager.GetMousePosition()))
			{
				switch (this.Type)
				{
				case CurrencyType.Money:
					ToolTip.SetText("The amount of money you currently have.");
					break;
				case CurrencyType.UpgradeTokens:
					ToolTip.SetText("The number of Upgrade Tokens you have earned.");
					break;
				case CurrencyType.TalentPoints:
					ToolTip.SetText("One Talent Point is gained each time your character levels up.");
					break;
				case CurrencyType.Points:
					ToolTip.SetText("The amount of Zombie Heads you have earned.");
					break;
				}
			}
			base.Update();
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0002250C File Offset: 0x0002070C
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (this.Highlighted)
			{
				this.bg.color = Color.Lerp(this.bgColor, Color.White, Global.Pulse);
			}
			else
			{
				this.bg.color = this.bgColor;
			}
			this.bg.Draw(spriteBatch);
			spriteBatch.Draw(Global.MasterTexture, this.currencyDest, new Rectangle?(this.currencyImageSource), Color.White);
			string text = "";
			Color color = Color.White;
			switch (this.Type)
			{
			case CurrencyType.Money:
				text = this.player.Stats.GetMoney().ToString();
				color = Color.LightGreen;
				break;
			case CurrencyType.UpgradeTokens:
				text = "x " + this.player.Stats.UpgradeTokens.ToString();
				color = Color.LightYellow;
				break;
			case CurrencyType.TalentPoints:
				text = "x " + this.player.Stats.GetTalentPoints().ToString();
				color = Color.Pink;
				break;
			case CurrencyType.Points:
				text = "x " + this.player.Points.ToString();
				color = Color.LightBlue;
				break;
			}
			Shadow.DrawString(text, Global.StoreFontBig, new Vector2(this.Position.X + 36f, this.Position.Y - 4f), 1, color, spriteBatch);
			base.Draw(spriteBatch);
		}

		// Token: 0x040004A9 RID: 1193
		private CurrencyType Type;

		// Token: 0x040004AA RID: 1194
		private Vector2 Position;

		// Token: 0x040004AB RID: 1195
		private Rectangle currencyImageSource;

		// Token: 0x040004AC RID: 1196
		private Rectangle currencyDest;

		// Token: 0x040004AD RID: 1197
		private Player player;

		// Token: 0x040004AE RID: 1198
		public bool Highlighted;

		// Token: 0x040004AF RID: 1199
		private MiscBackground bg;

		// Token: 0x040004B0 RID: 1200
		private Color bgColor;
	}
}
