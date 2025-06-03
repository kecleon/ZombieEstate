using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000013 RID: 19
	public class Behavior
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00003244 File Offset: 0x00001444
		public Behavior()
		{
			this.behaviors = new BulletBehavior[4];
			for (int i = 0; i < 4; i++)
			{
				this.behaviors[i] = BulletBehavior.NONE;
			}
			this.percents = new float[4];
			this.changeOverTime = new float[4];
			this.minPercents = new float[4];
			this.maxPercents = new float[4];
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000032A8 File Offset: 0x000014A8
		public void UpdateBehaviors(float elapsed)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.changeOverTime[i] != 0f)
				{
					this.ChangePercentByValue(this.behaviors[i], this.changeOverTime[i] * elapsed);
				}
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000032E8 File Offset: 0x000014E8
		public BulletBehavior[] GetBehaviors()
		{
			return this.behaviors;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000032F0 File Offset: 0x000014F0
		public float[] GetPercents()
		{
			return this.percents;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000032F8 File Offset: 0x000014F8
		public BulletBehavior CurrentBehavior()
		{
			float num = 0f;
			for (int i = 0; i < 4; i++)
			{
				if (this.currentPercent < num + this.percents[i])
				{
					return this.behaviors[i];
				}
				num += this.percents[i];
			}
			Terminal.WriteMessage("ERROR: Behavior Percent invalid!", MessageType.ERROR);
			return this.behaviors[0];
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003350 File Offset: 0x00001550
		public void ChangeOverTimePercent(BulletBehavior behavior, float delta)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.behaviors[i] == behavior)
				{
					this.changeOverTime[i] = delta;
					return;
				}
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003380 File Offset: 0x00001580
		public void SetMinMaxPercent(BulletBehavior behavior, float min, float max)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.behaviors[i] == behavior)
				{
					this.minPercents[i] = min;
					this.maxPercents[i] = max;
					return;
				}
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000033B8 File Offset: 0x000015B8
		public void AddBehavior(BulletBehavior behavior)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.behaviors[i] == BulletBehavior.NONE)
				{
					this.behaviors[i] = behavior;
					if (i == 0)
					{
						this.percents[i] = 1f;
					}
					else
					{
						this.percents[i] = 0f;
					}
					this.minPercents[i] = 0f;
					this.maxPercents[i] = 0f;
					this.changeOverTime[i] = 0f;
					return;
				}
				if (i == 3)
				{
					Terminal.WriteMessage("ERROR: Added more than 4 behaviors!!!", MessageType.ERROR);
					return;
				}
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000343C File Offset: 0x0000163C
		public void ChangePercent(BulletBehavior behavior, float percent)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.behaviors[i] == behavior)
				{
					this.percents[i] = percent;
					return;
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000346C File Offset: 0x0000166C
		public void ChangePercentByValue(BulletBehavior behavior, float percent)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.behaviors[i] == behavior)
				{
					this.percents[i] += percent;
					this.percents[i] = MathHelper.Clamp(this.percents[i], this.minPercents[i], this.maxPercents[i]);
					return;
				}
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000034C8 File Offset: 0x000016C8
		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < 4; i++)
			{
				text = text + this.behaviors[i].ToString() + ", ";
				text = text + this.percents[i].ToString() + ", ";
				text = text + this.changeOverTime[i].ToString() + ", ";
				text = text + this.minPercents[i].ToString() + ", ";
				text = text + this.maxPercents[i].ToString() + "\n";
			}
			return text;
		}

		// Token: 0x0400002F RID: 47
		private BulletBehavior[] behaviors;

		// Token: 0x04000030 RID: 48
		private float[] percents;

		// Token: 0x04000031 RID: 49
		private float[] changeOverTime;

		// Token: 0x04000032 RID: 50
		private float[] minPercents;

		// Token: 0x04000033 RID: 51
		private float[] maxPercents;

		// Token: 0x04000034 RID: 52
		private float currentPercent;
	}
}
