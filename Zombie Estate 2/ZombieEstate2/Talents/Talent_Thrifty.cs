using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000175 RID: 373
	internal class Talent_Thrifty : TalentNode
	{
		// Token: 0x06000B43 RID: 2883 RVA: 0x0005D604 File Offset: 0x0005B804
		public Talent_Thrifty(Point loc, Point topLeft, int level, int scale) : base(new Point(71, 51), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Decreases costs of guns by 2.5%.");
			this.Descriptions.Add("Decreases costs of guns by 5%.");
			this.Descriptions.Add("Decreases costs of guns by 7.5%.");
			this.Name = "Thrifty Buyer";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0005D670 File Offset: 0x0005B870
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 1)
			{
				player.TalentSpecProps.GunCostMod += 2.5f;
				return;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.GunCostMod += 5f;
				return;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.GunCostMod += 7.5f;
			}
		}
	}
}
