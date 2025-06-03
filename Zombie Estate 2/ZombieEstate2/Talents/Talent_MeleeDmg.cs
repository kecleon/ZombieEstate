using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000183 RID: 387
	internal class Talent_MeleeDmg : TalentNode
	{
		// Token: 0x06000B5F RID: 2911 RVA: 0x0005E140 File Offset: 0x0005C340
		public Talent_MeleeDmg(Point loc, Point topLeft, int level, int scale) : base(new Point(66, 49), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increase damage done with melee weapons by 5%.");
			this.Descriptions.Add("Increase damage done with melee weapons by 10%.");
			this.Descriptions.Add("Increase damage done with melee weapons by 15%.");
			this.Name = "Weight Lifting";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0005E1AC File Offset: 0x0005C3AC
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.MeleeDamageMod += 5f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.MeleeDamageMod += 10f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.MeleeDamageMod += 15f;
			}
		}
	}
}
