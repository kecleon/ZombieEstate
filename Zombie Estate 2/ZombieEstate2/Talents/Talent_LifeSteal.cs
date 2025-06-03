using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000185 RID: 389
	internal class Talent_LifeSteal : TalentNode
	{
		// Token: 0x06000B63 RID: 2915 RVA: 0x0005E308 File Offset: 0x0005C508
		public Talent_LifeSteal(Point loc, Point topLeft, int level, int scale) : base(new Point(67, 48), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Gain 0.5% of damage done as life.");
			this.Descriptions.Add("Gain 0.75% of damage done as life.");
			this.Descriptions.Add("Gain 1.0% of damage done as life.");
			this.Name = "Life Steal";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0005E374 File Offset: 0x0005C574
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.LifeStealPercent += 0.5f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.LifeStealPercent += 0.75f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.LifeStealPercent += 1f;
			}
		}
	}
}
