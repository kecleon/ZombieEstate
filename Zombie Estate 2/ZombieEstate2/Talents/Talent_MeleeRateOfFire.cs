using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000182 RID: 386
	internal class Talent_MeleeRateOfFire : TalentNode
	{
		// Token: 0x06000B5D RID: 2909 RVA: 0x0005E05C File Offset: 0x0005C25C
		public Talent_MeleeRateOfFire(Point loc, Point topLeft, int level, int scale) : base(new Point(66, 51), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases rate of fire by 5% for all melee weapons.");
			this.Descriptions.Add("Increases rate of fire by 10% for all melee weapons.");
			this.Descriptions.Add("Increases rate of fire by 15% for all melee weapons.");
			this.Name = "Fast Swings";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0005E0C8 File Offset: 0x0005C2C8
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.SwingTimeMod += 5f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.SwingTimeMod += 10f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.SwingTimeMod += 15f;
			}
		}
	}
}
