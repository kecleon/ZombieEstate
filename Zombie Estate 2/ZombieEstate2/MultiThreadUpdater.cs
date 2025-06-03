using System;
using System.Collections.Generic;
using System.Threading;

namespace ZombieEstate2
{
	// Token: 0x02000119 RID: 281
	internal class MultiThreadUpdater
	{
		// Token: 0x060007B5 RID: 1973 RVA: 0x0003C84B File Offset: 0x0003AA4B
		public MultiThreadUpdater(List<GameObject> list)
		{
			if (list.Count % 2 != 0)
			{
				Terminal.WriteMessage("WARNING NOT EVEN!", MessageType.ERROR);
			}
			this.oneLower = 0;
			this.oneUpper = list.Count;
			this.objs1 = list;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0003C884 File Offset: 0x0003AA84
		public void UpdateGameObjects(ref List<GameObject> list, float elapsed)
		{
			this.objs1 = list;
			this.elapsed = elapsed;
			for (int i = 0; i < list.Count; i++)
			{
				if (this.objs1[i].Active)
				{
					this.objs1[i].Update(elapsed);
					this.objs1[i].UpdateTransform();
				}
			}
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0003C8E8 File Offset: 0x0003AAE8
		public void UpdateTransforms()
		{
			do
			{
				if (this.objs1 != null)
				{
					for (int i = 0; i < this.objs1.Count; i++)
					{
						if (this.objs1[i].Active)
						{
							this.objs1[i].UpdateTransform();
						}
					}
				}
			}
			while (!Global.GameEnded);
			Terminal.WriteMessage("Thread: |" + Thread.CurrentThread.Name + "| exiting.");
		}

		// Token: 0x0400082C RID: 2092
		private float elapsed;

		// Token: 0x0400082D RID: 2093
		private int oneLower;

		// Token: 0x0400082E RID: 2094
		private int oneUpper;

		// Token: 0x0400082F RID: 2095
		private int twoLower;

		// Token: 0x04000830 RID: 2096
		private int twoUpper;

		// Token: 0x04000831 RID: 2097
		private int count;

		// Token: 0x04000832 RID: 2098
		private List<GameObject> objs1;

		// Token: 0x04000833 RID: 2099
		private long IterNumber;

		// Token: 0x04000834 RID: 2100
		private float average;

		// Token: 0x04000835 RID: 2101
		private double SUM;

		// Token: 0x04000836 RID: 2102
		private Thread UpdateTransformThread;
	}
}
