using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000083 RID: 131
	internal class AbilityMinionBezerk : Ability
	{
		// Token: 0x06000318 RID: 792 RVA: 0x00017DA6 File Offset: 0x00015FA6
		public AbilityMinionBezerk(Player player) : base(player, 1)
		{
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00018060 File Offset: 0x00016260
		public override void SetupCooldownsAndName()
		{
			this.AbilityName = "Minion Bezerk";
			this.CooldownType = AbilityCooldownType.Pressed;
			this.SecondsUntilCooldownBegins = 0f;
			this.SecondsToCompleteCooldown = 60f;
			this.MidActivationCooldown = 0f;
			this.PercentPerActivation = 1f;
			base.SetupCooldownsAndName();
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000180B4 File Offset: 0x000162B4
		public override void DoAbility()
		{
			RadialParticle radialParticle = new RadialParticle(new Point(75, 60), 0.2f, 10f, 0.1f, this.Parent.Position);
			radialParticle.TexScale = 2f;
			Global.MasterCache.CreateObject(radialParticle);
			foreach (Minion minion in this.Parent.GetMinionList)
			{
				minion.AddBuff("Buff_MinionBezerk", this.Parent, "15.0, 0.25");
				minion.ShotCount = minion.TotalShotCount;
				minion.Health = minion.SpecialProperties.MaxHealth;
			}
			base.DoAbility();
		}
	}
}
