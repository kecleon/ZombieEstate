using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000F4 RID: 244
	public static class HUDWaveObjective
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x0002F951 File Offset: 0x0002DB51
		private static Dictionary<WaveObjectiveType, Rectangle> Rects
		{
			get
			{
				return EquationElement.Rects;
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0002F958 File Offset: 0x0002DB58
		public static void Init()
		{
			HUDWaveObjective.dest = default(Rectangle);
			HUDWaveObjective.textDest = default(Vector2);
			HUDWaveObjective.lineRect = new Rectangle(0, 0, 4, 2);
			HUDWaveObjective.dest.Width = 32;
			HUDWaveObjective.dest.Height = 32;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0002F996 File Offset: 0x0002DB96
		public static void DrawObjective(WaveObjectiveType type, int amount, int x, int y, SpriteBatch spriteBatch)
		{
			HUDWaveObjective.DrawObjective(type, amount, 0, x, y, spriteBatch);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0002F9A4 File Offset: 0x0002DBA4
		public static void DrawObjective(WaveObjectiveType type, int amount, int goalAmount, int x, int y, SpriteBatch spriteBatch)
		{
			HUDWaveObjective.dest.X = x + 118 + (int)PlayerHUD.HUD_Right_Pos.X;
			HUDWaveObjective.dest.Y = y + 86 + (int)PlayerHUD.HUD_Right_Pos.Y;
			spriteBatch.Draw(Global.MasterTexture, HUDWaveObjective.dest, new Rectangle?(HUDWaveObjective.Rects[type]), Color.White);
			if (goalAmount == 0)
			{
				HUDWaveObjective.textDest.X = (float)(x + 34 + 118 + (int)PlayerHUD.HUD_Right_Pos.X);
				HUDWaveObjective.textDest.Y = (float)(y + 82 + (int)PlayerHUD.HUD_Right_Pos.Y);
				string text = "x" + amount.ToString();
				if (type == WaveObjectiveType.Time || type == WaveObjectiveType.ParTime)
				{
					text = HUDWaveObjective.GetTimeString(amount);
				}
				Shadow.DrawString(text, Global.StoreFontBig, HUDWaveObjective.textDest, 1, Color.White, spriteBatch);
				return;
			}
			int num = (int)Global.StoreFontBig.MeasureString("x").X;
			if (type != WaveObjectiveType.Time && type != WaveObjectiveType.ParTime)
			{
				HUDWaveObjective.textDest.X = (float)(x + 34 + 118 + (int)PlayerHUD.HUD_Right_Pos.X);
				HUDWaveObjective.textDest.Y = (float)(y + 82 + (int)PlayerHUD.HUD_Right_Pos.Y);
				Shadow.DrawString("x", Global.StoreFontBig, HUDWaveObjective.textDest, 1, Color.White, spriteBatch);
			}
			else
			{
				num = 0;
			}
			HUDWaveObjective.textDest.X = (float)(x + 34 + 118 + (int)PlayerHUD.HUD_Right_Pos.X + num);
			HUDWaveObjective.textDest.Y = (float)(y + 68 + (int)PlayerHUD.HUD_Right_Pos.Y);
			string text2 = amount.ToString();
			if (type == WaveObjectiveType.Time || type == WaveObjectiveType.ParTime)
			{
				text2 = HUDWaveObjective.GetTimeString(amount);
			}
			Shadow.DrawString(text2, Global.StoreFontBig, HUDWaveObjective.textDest, 1, Color.White, spriteBatch);
			HUDWaveObjective.textDest.X = (float)(x + 34 + 118 + (int)PlayerHUD.HUD_Right_Pos.X + num);
			HUDWaveObjective.textDest.Y = (float)(y + 96 + (int)PlayerHUD.HUD_Right_Pos.Y);
			text2 = goalAmount.ToString();
			if (type == WaveObjectiveType.Time || type == WaveObjectiveType.ParTime)
			{
				text2 = HUDWaveObjective.GetTimeString(goalAmount);
			}
			Shadow.DrawString(text2, Global.StoreFontBig, HUDWaveObjective.textDest, 1, Color.Gray, spriteBatch);
			HUDWaveObjective.lineRect.X = x + 34 + 118 + (int)PlayerHUD.HUD_Right_Pos.X + num;
			HUDWaveObjective.lineRect.Y = y + 102 + (int)PlayerHUD.HUD_Right_Pos.Y;
			HUDWaveObjective.lineRect.Width = (int)Global.StoreFontBig.MeasureString(text2).X;
			spriteBatch.Draw(Global.Pixel, HUDWaveObjective.lineRect, Color.White);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0002FC3C File Offset: 0x0002DE3C
		private static string GetTimeString(int amount)
		{
			string str = (amount / 60).ToString();
			string text = (amount % 60).ToString();
			if (amount % 60 < 10)
			{
				text = "0" + text;
			}
			return str + ":" + text;
		}

		// Token: 0x04000643 RID: 1603
		private static Rectangle dest;

		// Token: 0x04000644 RID: 1604
		private static Vector2 textDest;

		// Token: 0x04000645 RID: 1605
		private static Rectangle lineRect;
	}
}
