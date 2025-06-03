using System;

namespace ZombieEstate2
{
	// Token: 0x02000027 RID: 39
	internal class BossWithState<T> : Boss
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x00006AF1 File Offset: 0x00004CF1
		public BossWithState()
		{
			this.StateMachine = new StateMachineEngine<T>();
			this.AddStates();
			this.StateMachine.Switched = new StateSwitched(this.StateSwitched);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006B22 File Offset: 0x00004D22
		public override void Update(float elapsed)
		{
			base.Update(elapsed);
			if (!this.spawning)
			{
				this.StateMachine.Update(elapsed);
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void AddStates()
		{
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void StateSwitched()
		{
		}

		// Token: 0x0400009D RID: 157
		public StateMachineEngine<T> StateMachine;
	}
}
