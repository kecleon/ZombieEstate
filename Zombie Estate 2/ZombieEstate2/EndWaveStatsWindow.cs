using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000F1 RID: 241
	public class EndWaveStatsWindow : WaveTip
	{
		// Token: 0x0600065F RID: 1631 RVA: 0x0002F6E7 File Offset: 0x0002D8E7
		public EndWaveStatsWindow(List<PlayerDataBeforeWave> pre, List<PlayerDataBeforeWave> post) : base("EndWaveStatsNote")
		{
			this.PreWaveData = pre;
			this.PostWaveData = post;
			this.font = Global.StoreFontBig;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0002F710 File Offset: 0x0002D910
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			Vector2 centerPos = new Vector2(this.Position.X + 300f, this.Position.Y + 128f);
			for (int i = 0; i < this.PreWaveData.Count; i++)
			{
				this.DrawPlayer(centerPos, this.PreWaveData[i], this.PostWaveData[i], spriteBatch);
				centerPos.Y += (float)(i * 64);
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0002F794 File Offset: 0x0002D994
		private void DrawPlayer(Vector2 centerPos, PlayerDataBeforeWave pre, PlayerDataBeforeWave post, SpriteBatch spriteBatch)
		{
			int num = (int)centerPos.X - 200;
			Vector2 pos = VerchickMath.CenterText(this.font, centerPos, pre.Player.Stats.CharSettings.name);
			Shadow.DrawString(pre.Player.Stats.CharSettings.name, this.font, pos, 1, pre.Player.HUDColor * base.GetAlpha, spriteBatch);
			pos.Y += (float)this.font.LineSpacing;
			pos.X -= (float)num;
			int num2 = post.TotalKills - pre.TotalKills;
			int num3 = (int)this.font.MeasureString("Wave Kills: ").X;
			Shadow.DrawString("Wave Kills: ", this.font, pos, 1, Color.Red * base.GetAlpha, spriteBatch);
			pos.X += (float)num3;
			Shadow.DrawString(VerchickMath.AddCommas(num2), this.font, pos, 1, Color.White * base.GetAlpha, spriteBatch);
			pos.Y += (float)this.font.LineSpacing;
			pos.X = (float)num;
		}

		// Token: 0x04000634 RID: 1588
		private List<PlayerDataBeforeWave> PreWaveData;

		// Token: 0x04000635 RID: 1589
		private List<PlayerDataBeforeWave> PostWaveData;

		// Token: 0x04000636 RID: 1590
		private SpriteFont font;
	}
}
