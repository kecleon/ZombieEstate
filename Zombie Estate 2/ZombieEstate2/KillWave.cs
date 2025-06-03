using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.Wave;

namespace ZombieEstate2
{
	// Token: 0x020000FA RID: 250
	internal class KillWave : WaveBase
	{
		// Token: 0x0600069A RID: 1690 RVA: 0x00030F26 File Offset: 0x0002F126
		public override void StartWaveRunning()
		{
			base.StartWaveRunning();
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00030F2E File Offset: 0x0002F12E
		public override WaveCompletionState UpdateWaveRunning(float elapsed)
		{
			base.UpdateWaveRunning(elapsed);
			if (this.Kills < this.KillsToWin)
			{
				return WaveCompletionState.Running;
			}
			return WaveCompletionState.Completed;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00030F4C File Offset: 0x0002F14C
		public override ObjectiveHUD GetHUD()
		{
			return new ObjectiveHUD(string.Format("Kill {0} Zombies!", this.KillsToWin), 5, 38, WaveBase.HUDLocation, Global.Player.HUDColor, new Color(0.9f, 0.2f, 0.1f));
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00030F99 File Offset: 0x0002F199
		public override int GetHUDActual()
		{
			return this.Kills;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00030FA1 File Offset: 0x0002F1A1
		public override int GetHUDTotal()
		{
			return this.KillsToWin;
		}
	}
}
