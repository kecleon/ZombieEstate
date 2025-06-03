using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Networking;

namespace ZombieEstate2.Wave
{
	// Token: 0x02000155 RID: 341
	public class WaveBase
	{
		// Token: 0x06000A46 RID: 2630 RVA: 0x000540E4 File Offset: 0x000522E4
		public WaveBase()
		{
			this.Weights = new Dictionary<ZombieType, int>();
			this.Weights.Add(ZombieType.NormalZombie, 100);
			this.CompleteText = new BouncyText(new List<string>
			{
				"WAVE",
				"COMPLETE"
			}, Global.ScreenRect.Width / 2, Global.ScreenRect.Height / 2, 2);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x000541A8 File Offset: 0x000523A8
		public void SET_GAME_COMPLETE()
		{
			this.CompleteText = new BouncyText(new List<string>
			{
				"GAME",
				"COMPLETE"
			}, Global.ScreenRect.Width / 2, Global.ScreenRect.Height / 2, 2);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x000541F4 File Offset: 0x000523F4
		public virtual void StartPreWaveDelay()
		{
			this.PreWaveStartDelay.Reset();
			this.PreWaveStartDelay.Start();
			this.mWaveStartText = string.Format("Wave {0} Start", Global.WaveMaster.WaveNumber);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0005422B File Offset: 0x0005242B
		public virtual bool UpdatePreWaveDelay(float elapsed)
		{
			if (Global.CHEAT && InputManager.BackPressed(0))
			{
				this.PreWaveStartDelay.ForceExpire();
			}
			WaveTip tip = this.Tip;
			return this.PreWaveStartDelay.Expired();
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0005425E File Offset: 0x0005245E
		public virtual void StartWaveRunning()
		{
			this.SpawnTimer.Start();
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0005426C File Offset: 0x0005246C
		public virtual WaveCompletionState UpdateWaveRunning(float elapsed)
		{
			if (NetworkManager.AmIHost && this.SpawnTimer.Expired())
			{
				this.SpawnTimer.Reset();
				this.SpawnTimer.Start();
				int num = Global.ZombieList.Count<Zombie>();
				if (num < this.MaxNumberOfZombiesOnScreen)
				{
					int num2 = this.MaxNumberOfZombiesOnScreen - num;
					num2 = Math.Min(this.ZombiesPerSecond, num2);
					for (int i = 0; i < num2; i++)
					{
						this.SpawnZombie();
					}
				}
			}
			return WaveCompletionState.Running;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x000542E0 File Offset: 0x000524E0
		public virtual void StartPreWaveCompleteDelay()
		{
			this.WaveCompleteDelay.Reset();
			this.WaveCompleteDelay.Start();
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x000542F8 File Offset: 0x000524F8
		public virtual bool UpdatePreCompleteDelay(float elapsed)
		{
			this.CompleteText.Update();
			return this.WaveCompleteDelay.Expired();
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00005D3F File Offset: 0x00003F3F
		public virtual bool UpdateWaveFailed(float elapsed)
		{
			return true;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00005D3F File Offset: 0x00003F3F
		public virtual bool UpdateWaveComplete(float elapsed)
		{
			return true;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void UpdateAfterWaveComplete()
		{
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00054315 File Offset: 0x00052515
		public virtual void SpawnZombie()
		{
			ZombieSpawner.SpawnZombie(10f, 15f, this.Weights);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void CompletedAnObjective()
		{
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void FailedAnObjective(GameObject obj)
		{
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0005432C File Offset: 0x0005252C
		public virtual void ResetWave()
		{
			this.Kills = 0;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void DrawRunning(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void DrawComplete(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00054335 File Offset: 0x00052535
		public virtual void DrawPreCompleteDelay(SpriteBatch spriteBatch)
		{
			this.CompleteText.Draw(spriteBatch);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00054344 File Offset: 0x00052544
		public virtual void DrawPreWaveDelay(SpriteBatch spriteBatch)
		{
			Shadow.DrawOutlinedString(spriteBatch, Global.StoreFontXtraLarge, this.mWaveStartText, Color.Black, Color.White, 1f, 0f, VerchickMath.CenterText(Global.StoreFontXtraLarge, new Vector2((float)(Global.ScreenRect.Width / 2), 100f), this.mWaveStartText));
			string text = this.PreWaveStartDelay.SecondsLeft().ToString();
			Shadow.DrawOutlinedString(spriteBatch, Global.StoreFontXtraLarge, text, Color.Black, Color.Red, 2f, 0f, VerchickMath.CenterText(Global.StoreFontXtraLarge, new Vector2((float)(Global.ScreenRect.Width / 2), 160f), text));
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void DrawAfterWaveComplete(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void AboutToCycleToNextWave()
		{
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void GivePreAndPostStats(List<PlayerDataBeforeWave> pre, List<PlayerDataBeforeWave> post)
		{
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x000543F3 File Offset: 0x000525F3
		public virtual ObjectiveHUD GetHUD()
		{
			return new ObjectiveHUD("NONE", 0, 0, WaveBase.HUDLocation, Global.Player.HUDColor, Color.CornflowerBlue);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0002DAE2 File Offset: 0x0002BCE2
		public virtual int GetHUDActual()
		{
			return 4;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00054415 File Offset: 0x00052615
		public virtual int GetHUDTotal()
		{
			return 8;
		}

		// Token: 0x04000ADD RID: 2781
		public int KillsToWin = 2;

		// Token: 0x04000ADE RID: 2782
		private Timer SpawnTimer = new Timer(1f);

		// Token: 0x04000ADF RID: 2783
		private Timer WaveCompleteDelay = new Timer(6f);

		// Token: 0x04000AE0 RID: 2784
		public Timer PreWaveStartDelay = new Timer(6f);

		// Token: 0x04000AE1 RID: 2785
		public Dictionary<ZombieType, int> Weights;

		// Token: 0x04000AE2 RID: 2786
		public static Vector2 HUDLocation = new Vector2((float)(Global.ScreenRect.Width - 340), 88f);

		// Token: 0x04000AE3 RID: 2787
		public int Kills;

		// Token: 0x04000AE4 RID: 2788
		public int MaxNumberOfZombiesOnScreen = 35;

		// Token: 0x04000AE5 RID: 2789
		public int ZombiesPerSecond = 5;

		// Token: 0x04000AE6 RID: 2790
		private BouncyText CompleteText;

		// Token: 0x04000AE7 RID: 2791
		public WaveTip Tip;

		// Token: 0x04000AE8 RID: 2792
		public EndWaveStatsWindow EndStatsWindow;

		// Token: 0x04000AE9 RID: 2793
		public List<ObjectiveObject> Objectives = new List<ObjectiveObject>();

		// Token: 0x04000AEA RID: 2794
		private List<PlayerDataBeforeWave> PreWaveData;

		// Token: 0x04000AEB RID: 2795
		private List<PlayerDataBeforeWave> PostWaveData;

		// Token: 0x04000AEC RID: 2796
		private EndWaveList EndWave;

		// Token: 0x04000AED RID: 2797
		public bool ZombiesDropMoney = true;

		// Token: 0x04000AEE RID: 2798
		private string mWaveStartText;
	}
}
