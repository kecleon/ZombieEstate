using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ZombieEstate2.Wave
{
	// Token: 0x02000156 RID: 342
	public class SpawnStats
	{
		// Token: 0x06000A60 RID: 2656 RVA: 0x0005443C File Offset: 0x0005263C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("WaveType: " + this.WaveType.ToString());
			stringBuilder.AppendLine("Weights: ");
			foreach (ZombieType key in this.Weights.Keys)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"  ----> |",
					key.ToString(),
					": ",
					this.Weights[key]
				}));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Special Count: " + this.SpecialCount.ToString());
			stringBuilder.AppendLine("Seconds: " + this.Seconds.ToString());
			stringBuilder.AppendLine("Max: " + this.ZombiesOnScreen.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x04000AEF RID: 2799
		public int ZombiesMaxNumber;

		// Token: 0x04000AF0 RID: 2800
		public int ZombiesOnScreen;

		// Token: 0x04000AF1 RID: 2801
		public int ZombiesPerSecond;

		// Token: 0x04000AF2 RID: 2802
		public float MinDistance;

		// Token: 0x04000AF3 RID: 2803
		public float MaxDistance;

		// Token: 0x04000AF4 RID: 2804
		[XmlIgnore]
		public Dictionary<ZombieType, int> Weights = new Dictionary<ZombieType, int>();

		// Token: 0x04000AF5 RID: 2805
		public string WeightString = "NONE";

		// Token: 0x04000AF6 RID: 2806
		public int SpecialCount;

		// Token: 0x04000AF7 RID: 2807
		public string WaveType = "NONE";

		// Token: 0x04000AF8 RID: 2808
		public int Seconds = -1;

		// Token: 0x04000AF9 RID: 2809
		public int TipIconX;

		// Token: 0x04000AFA RID: 2810
		public int TipIconY;

		// Token: 0x04000AFB RID: 2811
		public string TipText;

		// Token: 0x04000AFC RID: 2812
		public string TipTitle;

		// Token: 0x04000AFD RID: 2813
		public string WaveTitle;

		// Token: 0x04000AFE RID: 2814
		public string WaveObjective;
	}
}
