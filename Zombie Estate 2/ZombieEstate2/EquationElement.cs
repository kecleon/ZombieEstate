using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200005C RID: 92
	internal class EquationElement
	{
		// Token: 0x0600020E RID: 526 RVA: 0x0000F1D8 File Offset: 0x0000D3D8
		public static void Init()
		{
			EquationElement.Rects = new Dictionary<WaveObjectiveType, Rectangle>();
			EquationElement.Rects.Add(WaveObjectiveType.CarePackage, Global.GetTexRectange(4, 46));
			EquationElement.Rects.Add(WaveObjectiveType.Money, Global.GetTexRectange(4, 44));
			EquationElement.Rects.Add(WaveObjectiveType.Kills, Global.GetTexRectange(5, 38));
			EquationElement.Rects.Add(WaveObjectiveType.Time, Global.GetTexRectange(1, 39));
			EquationElement.Rects.Add(WaveObjectiveType.ParTime, Global.GetTexRectange(3, 39));
			EquationElement.Rects.Add(WaveObjectiveType.Seconds, Global.GetTexRectange(2, 39));
			EquationElement.Rects.Add(WaveObjectiveType.UpgradeTokens, Global.GetTexRectange(6, 37));
			EquationElement.Rects.Add(WaveObjectiveType.Talents, Global.GetTexRectange(8, 37));
			EquationElement.Rects.Add(WaveObjectiveType.ZombieHeads, Global.GetTexRectange(10, 37));
			EquationElement.Rects.Add(WaveObjectiveType.MoneyPickups, Global.GetTexRectange(4, 43));
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000F2AF File Offset: 0x0000D4AF
		public EquationElement(WaveObjectiveType type, int final, EquationPosition loc, Vector2 LeftPos) : this(type, final, loc, LeftPos, 0)
		{
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000F2BD File Offset: 0x0000D4BD
		public EquationElement(WaveObjectiveType type, int final, EquationPosition loc, Vector2 LeftPos, int startValue) : this(type, final, loc, LeftPos, startValue, Color.White)
		{
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000F2D4 File Offset: 0x0000D4D4
		public EquationElement(WaveObjectiveType type, int final, EquationPosition loc, Vector2 LeftPos, int startValue, Color col)
		{
			if (EquationElement.font == null)
			{
				EquationElement.font = Global.EquationFont;
			}
			this.currentValue = startValue;
			this.Position = LeftPos;
			this.type = type;
			this.finalValue = final;
			this.location = loc;
			this.color = col;
			if (type == WaveObjectiveType.Money)
			{
				this.color = Color.LightGreen;
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000F340 File Offset: 0x0000D540
		public void Start(float seconds)
		{
			this.timer = new Timer(seconds);
			if (this.type != WaveObjectiveType.Time && this.type != WaveObjectiveType.ParTime)
			{
				this.incrementValue = (int)((float)this.finalValue / (seconds * 60f));
			}
			else
			{
				this.incrementValue = (int)((float)this.finalValue * 1.6667f / (seconds * 60f));
			}
			if (this.finalValue != 0)
			{
				this.incrementValue = Math.Max(this.incrementValue, 1);
			}
			this.currentValue = 0;
			this.timer.IndependentOfTime = true;
			this.timer.DeltaDelegate = new Timer.TimerDelegate(this.Delta);
			this.timer.Start();
			this.Started = true;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000F3F8 File Offset: 0x0000D5F8
		private void Delta(float d)
		{
			if (d < EquationElement.AlphaFlash)
			{
				this.alpha = d / EquationElement.AlphaFlash;
			}
			this.currentValue += this.incrementValue;
			if (this.currentValue > this.finalValue || d == 1f)
			{
				this.currentValue = this.finalValue;
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000F450 File Offset: 0x0000D650
		public void Draw(SpriteBatch spriteBatch)
		{
			string @string = this.GetString();
			Vector2 pos = new Vector2(this.Position.X + (float)EquationElement.WIDTH - EquationElement.font.MeasureString(@string).X - 2f, this.Position.Y);
			Shadow.DrawString(@string, EquationElement.font, pos, 1, this.color * this.alpha, Color.Black * this.alpha, spriteBatch);
			Rectangle destinationRectangle = new Rectangle((int)this.Position.X + EquationElement.WIDTH + 2, (int)this.Position.Y, 32, 32);
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(EquationElement.Rects[this.type]), Color.White * this.alpha);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000F52C File Offset: 0x0000D72C
		private string GetString()
		{
			if (this.type == WaveObjectiveType.Time || this.type == WaveObjectiveType.ParTime)
			{
				int num = this.currentValue / 60;
				int num2 = this.currentValue % 60;
				string str = num.ToString();
				string str2;
				if (num2 >= 10)
				{
					str2 = num2.ToString();
				}
				else
				{
					str2 = "0" + num2.ToString();
				}
				return str + ":" + str2;
			}
			return this.currentValue.ToString();
		}

		// Token: 0x040001BF RID: 447
		public static Dictionary<WaveObjectiveType, Rectangle> Rects;

		// Token: 0x040001C0 RID: 448
		public bool Started;

		// Token: 0x040001C1 RID: 449
		private int finalValue;

		// Token: 0x040001C2 RID: 450
		private int currentValue;

		// Token: 0x040001C3 RID: 451
		private Vector2 Position;

		// Token: 0x040001C4 RID: 452
		private WaveObjectiveType type;

		// Token: 0x040001C5 RID: 453
		private EquationPosition location;

		// Token: 0x040001C6 RID: 454
		private Timer timer;

		// Token: 0x040001C7 RID: 455
		private int incrementValue;

		// Token: 0x040001C8 RID: 456
		private static SpriteFont font;

		// Token: 0x040001C9 RID: 457
		private Color color = Color.White;

		// Token: 0x040001CA RID: 458
		private float alpha;

		// Token: 0x040001CB RID: 459
		private static float AlphaFlash = 0.2f;

		// Token: 0x040001CC RID: 460
		private static int WIDTH = 128;
	}
}
