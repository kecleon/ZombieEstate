using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000081 RID: 129
	internal class AbilityTaunt : Ability
	{
		// Token: 0x06000311 RID: 785 RVA: 0x00017DA6 File Offset: 0x00015FA6
		public AbilityTaunt(Player player) : base(player, 1)
		{
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00017DB0 File Offset: 0x00015FB0
		public override void SetupCooldownsAndName()
		{
			this.AbilityName = "Taunt";
			this.CooldownType = AbilityCooldownType.Pressed;
			this.SecondsUntilCooldownBegins = 0f;
			this.SecondsToCompleteCooldown = 5f;
			this.MidActivationCooldown = 0f;
			this.PercentPerActivation = 1f;
			base.SetupCooldownsAndName();
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00017E04 File Offset: 0x00016004
		public override void DoAbility()
		{
			RadialParticle radialParticle = new RadialParticle(new Point(75, 60), 0.2f, 10f, 0.1f, this.Parent.Position);
			radialParticle.TexScale = 2f;
			Global.MasterCache.CreateObject(radialParticle);
			foreach (Zombie zombie in Global.ZombieList)
			{
				float num = Vector3.Distance(this.Parent.Position, zombie.Position);
				if (num < 10f)
				{
					float num2 = 1f - num / 10f;
					int amount = (int)(10f * num2);
					zombie.BuildAggro(this.Parent, amount);
				}
			}
			base.DoAbility();
		}
	}
}
