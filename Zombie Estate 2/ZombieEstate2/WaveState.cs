using System;

namespace ZombieEstate2
{
	// Token: 0x020000FC RID: 252
	public enum WaveState
	{
		// Token: 0x04000682 RID: 1666
		BetweenWave,
		// Token: 0x04000683 RID: 1667
		PreWaveDelay,
		// Token: 0x04000684 RID: 1668
		WaveRunning,
		// Token: 0x04000685 RID: 1669
		WaveCompleteDelay,
		// Token: 0x04000686 RID: 1670
		WaveComplete,
		// Token: 0x04000687 RID: 1671
		WaveJustEnded,
		// Token: 0x04000688 RID: 1672
		WaveFailed,
		// Token: 0x04000689 RID: 1673
		NONE
	}
}
