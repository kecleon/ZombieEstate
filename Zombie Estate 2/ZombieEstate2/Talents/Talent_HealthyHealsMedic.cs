using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200018D RID: 397
	internal class Talent_HealthyHealsMedic : TalentNode
	{
		// Token: 0x06000B73 RID: 2931 RVA: 0x0005E9D8 File Offset: 0x0005CBD8
		public Talent_HealthyHealsMedic(Point loc, Point topLeft, int level, int scale) : base(new Point(69, 49), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases all healing effects done to you by 5%.");
			this.Descriptions.Add("Increases all healing effects done to you by 10%.");
			this.Descriptions.Add("Increases all healing effects done to you by 20%.");
			this.Name = "Healthy Medic Heals";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0005EA44 File Offset: 0x0005CC44
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.HealRecievedMod += 0.05f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.HealRecievedMod += 0.1f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.HealRecievedMod += 0.2f;
			}
		}
	}
}
