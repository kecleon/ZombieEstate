using System;

namespace ZombieEstate2
{
	// Token: 0x02000080 RID: 128
	public class Ability
	{
		// Token: 0x06000308 RID: 776 RVA: 0x00017C46 File Offset: 0x00015E46
		public Ability(Player player, int level)
		{
			this.Parent = player;
			this.Level = level;
			this.Setup();
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00017C78 File Offset: 0x00015E78
		public void Activate()
		{
			if (this.MidActTimer.Ready() && this.AbilityPercent >= this.PercentPerActivation)
			{
				this.FireOffAbility();
				this.DoAbility();
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void DoAbility()
		{
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00017CA1 File Offset: 0x00015EA1
		private void FireOffAbility()
		{
			this.AbilityPercent -= this.PercentPerActivation;
			this.MainCooldown.Reset();
			this.MainCooldown.SetTimeRemaining(this.AbilityPercent);
			this.MainCooldown.Start();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00017CDD File Offset: 0x00015EDD
		private void UntilCooldownBegins(float delta)
		{
			if (delta >= 1f)
			{
				this.UntilCooldownBeginsTimer.Stop();
				this.MainCooldown.Resume();
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00017CFD File Offset: 0x00015EFD
		private void MainCooldownDel(float delta)
		{
			this.AbilityPercent = delta;
			if (this.AbilityPercent >= 1f)
			{
				this.MainCooldown.Stop();
				this.MainCooldown.Reset();
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00017D29 File Offset: 0x00015F29
		public void Setup()
		{
			this.SetupCooldownsAndName();
			this.SetupTimers();
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void SetupCooldownsAndName()
		{
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00017D38 File Offset: 0x00015F38
		private void SetupTimers()
		{
			this.MidActTimer = new Timer(this.MidActivationCooldown);
			this.UntilCooldownBeginsTimer = new Timer(this.SecondsUntilCooldownBegins);
			this.MainCooldown = new Timer(this.SecondsToCompleteCooldown);
			this.UntilCooldownBeginsTimer.DeltaDelegate = new Timer.TimerDelegate(this.UntilCooldownBegins);
			this.MainCooldown.DeltaDelegate = new Timer.TimerDelegate(this.MainCooldownDel);
		}

		// Token: 0x040002FE RID: 766
		public float PercentPerActivation;

		// Token: 0x040002FF RID: 767
		public float SecondsUntilCooldownBegins;

		// Token: 0x04000300 RID: 768
		public float SecondsToCompleteCooldown;

		// Token: 0x04000301 RID: 769
		public float MidActivationCooldown;

		// Token: 0x04000302 RID: 770
		public AbilityCooldownType CooldownType;

		// Token: 0x04000303 RID: 771
		private Timer MidActTimer;

		// Token: 0x04000304 RID: 772
		private Timer UntilCooldownBeginsTimer;

		// Token: 0x04000305 RID: 773
		private Timer MainCooldown;

		// Token: 0x04000306 RID: 774
		public string AbilityName = "NONE";

		// Token: 0x04000307 RID: 775
		public float AbilityPercent = 1f;

		// Token: 0x04000308 RID: 776
		public Player Parent;

		// Token: 0x04000309 RID: 777
		public int Level;
	}
}
