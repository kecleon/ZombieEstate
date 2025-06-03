using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Wave;

namespace ZombieEstate2
{
	// Token: 0x02000065 RID: 101
	internal class WaveDescription
	{
		// Token: 0x06000244 RID: 580 RVA: 0x00011BEC File Offset: 0x0000FDEC
		public WaveDescription(SpawnStats stats, int waveNum)
		{
			if (WaveDescription.ZombieTex == null)
			{
				this.SetupTex();
			}
			if (WaveDescription.BG == null)
			{
				WaveDescription.BG = Global.Content.Load<Texture2D>("HUD\\HUD_WaveStart");
			}
			this.Stats = stats;
			this.WaveNum = "Wave " + waveNum.ToString();
			this.Position = new Vector2((float)(Global.ScreenRect.Width / 2 - WaveDescription.BG.Width / 2), (float)(Global.ScreenRect.Height / 2 - WaveDescription.BG.Height / 2));
			this.Title = stats.WaveTitle;
			this.Desc = VerchickMath.WordWrapWidth(this.ParseDesc(stats), 340, Global.StoreFont);
			this.Dest = new Rectangle((int)this.Position.X + 23, (int)this.Position.Y + 185, 128, 128);
			this.Src = Global.GetTexRectange(stats.TipIconX, stats.TipIconY);
			this.Hide = new PCButton("Continue", Color.LightGray, new Vector2(this.Position.X + 580f, this.Position.Y + 292f));
			this.Hide.Pressed = new PCButton.PressedDelegate(this.HideWindow);
			this.TopLeftDesc = new Vector2(this.Position.X + 170f, this.Position.Y + 260f);
			this.TitlePos = VerchickMath.CenterText(Global.StoreFontBig, new Vector2(this.Position.X + 326f, this.Position.Y + 208f), this.Title);
			this.WaveNumPos = VerchickMath.CenterText(Global.BloodGutterXtraLarge, new Vector2(this.Position.X + 400f, this.Position.Y + 80f), this.WaveNum);
			this.alpha = 0f;
			Point pos = new Point((int)this.Position.X + 240, (int)this.Position.Y + 240);
			foreach (ZombieType zombieType in this.Stats.Weights.Keys)
			{
				AmmoMeter ammoMeter = new AmmoMeter(pos, Color.DarkGreen, WaveDescription.ZombieTex[zombieType]);
				ammoMeter.Zombie = zombieType;
				ammoMeter.ModColor = Color.Lime;
				this.meters.Add(ammoMeter);
				pos.Y += 40;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00011EC0 File Offset: 0x000100C0
		public void Update(float elapsed)
		{
			if (this.DEAD)
			{
				return;
			}
			this.Hide.Update();
			if (!this.hidden && this.alpha < 1f)
			{
				this.alpha += 0.05f;
			}
			if (this.hidden)
			{
				if (this.alpha > 0f)
				{
					this.alpha -= 0.05f;
					return;
				}
				this.DEAD = true;
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00011F38 File Offset: 0x00010138
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.DEAD)
			{
				return;
			}
			spriteBatch.Draw(WaveDescription.BG, this.Position, Color.White * this.alpha);
			Shadow.DrawString(this.Title, Global.StoreFontBig, this.TitlePos, 1, Color.White * this.alpha, Color.Black * this.alpha, spriteBatch);
			Shadow.DrawString(this.Desc, Global.StoreFont, this.TopLeftDesc, 1, Color.LightGray * this.alpha, Color.Black * this.alpha, spriteBatch);
			spriteBatch.Draw(Global.MasterTexture, this.Dest, new Rectangle?(this.Src), Color.White * this.alpha);
			Shadow.DrawString(this.WaveNum, Global.BloodGutterXtraLarge, this.WaveNumPos, 2, Color.Red * this.alpha, Color.Black * this.alpha, spriteBatch);
			foreach (AmmoMeter ammoMeter in this.meters)
			{
				ammoMeter.DrawMeter(spriteBatch, this.Stats.Weights[ammoMeter.Zombie], 100, false);
			}
			this.Hide.Draw(spriteBatch, this.alpha);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x000120B4 File Offset: 0x000102B4
		private void HideWindow()
		{
			this.hidden = true;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000120C0 File Offset: 0x000102C0
		private string ParseDesc(SpawnStats stats)
		{
			string text = stats.WaveObjective;
			if (text == null)
			{
				text = "NULL";
			}
			text = text.Replace("[K]", stats.SpecialCount.ToString());
			return text.Replace("[T]", stats.Seconds.ToString());
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0001210C File Offset: 0x0001030C
		private void SetupTex()
		{
			WaveDescription.ZombieTex = new Dictionary<ZombieType, Point>();
			WaveDescription.ZombieTex.Add(ZombieType.NormalZombie, new Point(56, 0));
			WaveDescription.ZombieTex.Add(ZombieType.Skeleton, new Point(56, 6));
			WaveDescription.ZombieTex.Add(ZombieType.Hazmat, new Point(56, 12));
			WaveDescription.ZombieTex.Add(ZombieType.Goliath, new Point(56, 18));
			WaveDescription.ZombieTex.Add(ZombieType.Banshee, new Point(52, 12));
		}

		// Token: 0x04000239 RID: 569
		private static Texture2D BG;

		// Token: 0x0400023A RID: 570
		private string Title;

		// Token: 0x0400023B RID: 571
		private string Desc;

		// Token: 0x0400023C RID: 572
		private Rectangle Src;

		// Token: 0x0400023D RID: 573
		private Rectangle Dest;

		// Token: 0x0400023E RID: 574
		private Vector2 Position;

		// Token: 0x0400023F RID: 575
		private Vector2 TitlePos;

		// Token: 0x04000240 RID: 576
		private Vector2 TopLeftDesc;

		// Token: 0x04000241 RID: 577
		private PCButton Hide;

		// Token: 0x04000242 RID: 578
		public bool hidden;

		// Token: 0x04000243 RID: 579
		private float alpha;

		// Token: 0x04000244 RID: 580
		private string WaveNum;

		// Token: 0x04000245 RID: 581
		private Vector2 WaveNumPos;

		// Token: 0x04000246 RID: 582
		private bool DEAD;

		// Token: 0x04000247 RID: 583
		private SpawnStats Stats;

		// Token: 0x04000248 RID: 584
		private List<AmmoMeter> meters = new List<AmmoMeter>();

		// Token: 0x04000249 RID: 585
		private static Dictionary<ZombieType, Point> ZombieTex;
	}
}
