using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Wave;

namespace ZombieEstate2
{
	// Token: 0x020000F9 RID: 249
	internal class BossWave : WaveBase
	{
		// Token: 0x0600068B RID: 1675 RVA: 0x00030BC8 File Offset: 0x0002EDC8
		public BossWave(string bossName)
		{
			this.BossName = bossName;
			this.Font = Global.BloodGutterXtraLarge;
			this.SmallFont = Global.BloodGutterLarge;
			this.ZombiesDropMoney = false;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00030C1C File Offset: 0x0002EE1C
		private void GetBossClass()
		{
			if (this.BossName == "Yardsman")
			{
				this.Boss = new YardsmanNew();
			}
			if (this.BossName == "EstateDemon")
			{
				this.Boss = new EstateDemon();
			}
			bool networked = Global.NETWORKED;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00030C69 File Offset: 0x0002EE69
		public override ObjectiveHUD GetHUD()
		{
			return new ObjectiveHUD("Kill the boss!", 0, 42, WaveBase.HUDLocation, Global.Player.HUDColor, Color.Red);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00005D3F File Offset: 0x00003F3F
		public override int GetHUDTotal()
		{
			return 1;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00005D3F File Offset: 0x00003F3F
		public override int GetHUDActual()
		{
			return 1;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00030C8C File Offset: 0x0002EE8C
		public override void DrawPreWaveDelay(SpriteBatch spriteBatch)
		{
			this.DrawTitle(spriteBatch);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00030C95 File Offset: 0x0002EE95
		public override void DrawRunning(SpriteBatch spriteBatch)
		{
			this.DrawTitle(spriteBatch);
			base.DrawRunning(spriteBatch);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00030CA8 File Offset: 0x0002EEA8
		private void DrawTitle(SpriteBatch spriteBatch)
		{
			if (this.DrawTitleTimer.Expired())
			{
				return;
			}
			float scale = 1f;
			if (this.DrawTitleTimer.Delta() <= 0.1f)
			{
				scale = this.DrawTitleTimer.Delta() / 0.1f;
			}
			if (this.DrawTitleTimer.Delta() >= 0.8f)
			{
				scale = 1f - (this.DrawTitleTimer.Delta() - 0.8f) / 0.2f;
			}
			Vector2 position = VerchickMath.CenterText(this.Font, Global.GetScreenCenter() + new Vector2(0f, -64f), this.Boss.GetBossName());
			Shadow.DrawOutlinedString(spriteBatch, this.Font, this.Boss.GetBossName(), Color.Black * scale, Color.Red * scale, 1f, 0f, position);
			position = VerchickMath.CenterText(this.SmallFont, Global.GetScreenCenter() + new Vector2(0f, 32f), this.Boss.GetBossSubtitle());
			Shadow.DrawOutlinedString(spriteBatch, this.SmallFont, this.Boss.GetBossSubtitle(), Color.Black * scale, Color.White * scale, 1f, 0f, position);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00030DED File Offset: 0x0002EFED
		public override bool UpdatePreWaveDelay(float elapsed)
		{
			if (!this.Faded)
			{
				this.Faded = true;
				ScreenFader.Fade(new ScreenFader.FadeDone(this.FadeDone), 0.005f);
			}
			return base.UpdatePreWaveDelay(elapsed);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00030E1B File Offset: 0x0002F01B
		public override void StartPreWaveDelay()
		{
			this.PreWaveStartDelay.mTotalTime = 50f;
			this.GetBossClass();
			Global.Boss = this.Boss;
			base.StartPreWaveDelay();
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00030E44 File Offset: 0x0002F044
		private void FadeDone()
		{
			this.DrawTitleTimer.Reset();
			this.DrawTitleTimer.Start();
			if (this.Boss.ActivateBossLevelStuff())
			{
				GameManager.level.BossActivated();
			}
			this.PreWaveStartDelay.ForceExpire();
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00030E80 File Offset: 0x0002F080
		public override void StartWaveRunning()
		{
			Global.MasterCache.CreateObject(this.Boss);
			if (this.Boss.ActivateBossLevelStuff())
			{
				Global.BossActive = true;
			}
			foreach (Player player in Global.PlayerList)
			{
				player.Position = this.Boss.GetPlayerStartPos(player.Index);
			}
			base.StartWaveRunning();
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00030F0C File Offset: 0x0002F10C
		public override WaveCompletionState UpdateWaveRunning(float elapsed)
		{
			if (!this.Boss.Active)
			{
				return WaveCompletionState.Completed;
			}
			return WaveCompletionState.Running;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void SpawnZombie()
		{
		}

		// Token: 0x04000664 RID: 1636
		private Boss Boss;

		// Token: 0x04000665 RID: 1637
		private string BossName = "EstateDemon";

		// Token: 0x04000666 RID: 1638
		private SpriteFont Font;

		// Token: 0x04000667 RID: 1639
		private SpriteFont SmallFont;

		// Token: 0x04000668 RID: 1640
		private bool Faded;

		// Token: 0x04000669 RID: 1641
		private Timer DrawTitleTimer = new Timer(8f);
	}
}
