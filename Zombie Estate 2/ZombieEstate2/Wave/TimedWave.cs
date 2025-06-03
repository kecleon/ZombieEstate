using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.Wave
{
	// Token: 0x02000157 RID: 343
	internal class TimedWave : WaveBase
	{
		// Token: 0x06000A62 RID: 2658 RVA: 0x00054594 File Offset: 0x00052794
		public TimedWave()
		{
			this.TimeString = new DataString(1, 39, new Vector2((float)(Global.ScreenRect.Width / 2 - 64), 64f), Color.White, true);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x000545F8 File Offset: 0x000527F8
		public override void StartWaveRunning()
		{
			this.TimeLeft = (float)this.Seconds;
			base.StartWaveRunning();
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0005460D File Offset: 0x0005280D
		public override void ResetWave()
		{
			this.TimeLeft = (float)this.Seconds;
			base.ResetWave();
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00054624 File Offset: 0x00052824
		public override WaveCompletionState UpdateWaveRunning(float elapsed)
		{
			this.TimeLeft -= elapsed;
			this.TimeString.Update();
			if (this.TimeLeft <= 0f && !this.OverTime)
			{
				this.TimeLeft = 0f;
				this.OverTime = true;
				this.DiffIncreaseTimer.Reset();
				this.DiffIncreaseTimer.Start();
				this.DiffIncreaseTimer.ForceExpire();
			}
			this.DiffTimer();
			return base.UpdateWaveRunning(elapsed);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x000546A0 File Offset: 0x000528A0
		private void DiffTimer()
		{
			if (this.DiffIncreaseTimer.Expired())
			{
				this.iter++;
				this.DifficultyMod += (float)this.iter * 0.25f;
				this.DiffIncreaseTimer.Reset();
				this.DiffIncreaseTimer.Start();
			}
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x000546F8 File Offset: 0x000528F8
		public override void SpawnZombie()
		{
			ZombieSpawner.SpawnZombie(10f, 15f, this.Weights, this.DifficultyMod);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00054715 File Offset: 0x00052915
		public override void DrawRunning(SpriteBatch spriteBatch)
		{
			base.DrawRunning(spriteBatch);
		}

		// Token: 0x04000AFF RID: 2815
		public int Seconds = 60;

		// Token: 0x04000B00 RID: 2816
		private float TimeLeft;

		// Token: 0x04000B01 RID: 2817
		private Timer DiffIncreaseTimer = new Timer(30f);

		// Token: 0x04000B02 RID: 2818
		private float DifficultyMod = 1f;

		// Token: 0x04000B03 RID: 2819
		private int iter;

		// Token: 0x04000B04 RID: 2820
		private bool OverTime;

		// Token: 0x04000B05 RID: 2821
		private DataString TimeString;
	}
}
