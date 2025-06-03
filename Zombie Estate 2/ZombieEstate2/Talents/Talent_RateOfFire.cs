using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000188 RID: 392
	internal class Talent_RateOfFire : TalentNode
	{
		// Token: 0x06000B69 RID: 2921 RVA: 0x0005E538 File Offset: 0x0005C738
		public Talent_RateOfFire(Point loc, Point topLeft, int level, int scale) : base(new Point(69, 50), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases rate of fire by 5% for all guns.");
			this.Descriptions.Add("Increases rate of fire by 10% for all guns.");
			this.Descriptions.Add("Increases rate of fire by 15% for all guns.");
			this.Name = "Rate of Fire";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0005E5A4 File Offset: 0x0005C7A4
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.ShotTimeMod += 5f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.ShotTimeMod += 10f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.ShotTimeMod += 15f;
			}
		}
	}
}
