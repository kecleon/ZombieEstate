using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000058 RID: 88
	public struct EqData
	{
		// Token: 0x04000197 RID: 407
		public int left;

		// Token: 0x04000198 RID: 408
		public int right;

		// Token: 0x04000199 RID: 409
		public int result;

		// Token: 0x0400019A RID: 410
		public WaveObjectiveType leftType;

		// Token: 0x0400019B RID: 411
		public WaveObjectiveType rightType;

		// Token: 0x0400019C RID: 412
		public WaveObjectiveType resultType;

		// Token: 0x0400019D RID: 413
		public EquationOperation op;

		// Token: 0x0400019E RID: 414
		public string header;

		// Token: 0x0400019F RID: 415
		public bool linked;

		// Token: 0x040001A0 RID: 416
		public Color color;
	}
}
