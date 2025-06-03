using System;

namespace ZombieEstate2
{
	// Token: 0x02000133 RID: 307
	public class Timer
	{
		// Token: 0x060008C0 RID: 2240 RVA: 0x0004A0E7 File Offset: 0x000482E7
		public Timer(float seconds)
		{
			this.mTotalTime = seconds;
			this.mRunningTime = -1f;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0004A101 File Offset: 0x00048301
		public void Start()
		{
			this.mRunningTime = 0f;
			if (!TimerHandler.timers.Contains(this))
			{
				TimerHandler.timers.Add(this);
			}
			this.expired = false;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0004A12D File Offset: 0x0004832D
		public void Restart()
		{
			this.Reset();
			this.Start();
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0004A13B File Offset: 0x0004833B
		public bool Running()
		{
			return !this.expired && this.mRunningTime != -1f;
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0004A157 File Offset: 0x00048357
		public bool Ready()
		{
			return !this.Running() || this.Expired();
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0004A169 File Offset: 0x00048369
		public void Reset()
		{
			this.mRunningTime = -1f;
			this.expired = false;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0004A180 File Offset: 0x00048380
		public void Update(float elapsed)
		{
			if (this.mRunningTime == -1f)
			{
				return;
			}
			this.mRunningTime += elapsed;
			if (this.DeltaDelegate != null)
			{
				this.DeltaDelegate(this.Delta());
			}
			if (this.mRunningTime >= this.mTotalTime)
			{
				if (this.DeltaDelegate != null)
				{
					this.DeltaDelegate(1f);
				}
				this.Stop();
			}
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0004A1EE File Offset: 0x000483EE
		public void Stop()
		{
			this.expired = true;
			TimerHandler.removeList.Add(this);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0004A202 File Offset: 0x00048402
		public void Resume()
		{
			if (!TimerHandler.timers.Contains(this))
			{
				TimerHandler.timers.Add(this);
			}
			this.expired = false;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0004A223 File Offset: 0x00048423
		public void Pause()
		{
			TimerHandler.removeList.Add(this);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0004A230 File Offset: 0x00048430
		public bool Expired()
		{
			return this.expired;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0004A238 File Offset: 0x00048438
		public float Delta()
		{
			return this.mRunningTime / this.mTotalTime;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0004A247 File Offset: 0x00048447
		public void SetTimeRemaining(float delta)
		{
			this.mRunningTime = delta * this.mTotalTime;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0004A257 File Offset: 0x00048457
		public void SetSecondsLeft(int seconds)
		{
			this.mRunningTime = this.mTotalTime - (float)seconds;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0004A268 File Offset: 0x00048468
		public int SecondsLeft()
		{
			return (int)(this.mTotalTime - this.mRunningTime);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0004A278 File Offset: 0x00048478
		public void ForceExpire()
		{
			if (this.DeltaDelegate != null)
			{
				this.DeltaDelegate(1f);
			}
			this.Stop();
		}

		// Token: 0x04000988 RID: 2440
		public float mTotalTime;

		// Token: 0x04000989 RID: 2441
		private float mRunningTime;

		// Token: 0x0400098A RID: 2442
		private bool expired;

		// Token: 0x0400098B RID: 2443
		public bool IndependentOfTime;

		// Token: 0x0400098C RID: 2444
		public Timer.TimerDelegate DeltaDelegate;

		// Token: 0x0200021E RID: 542
		// (Invoke) Token: 0x06000DF2 RID: 3570
		public delegate void TimerDelegate(float delta);
	}
}
