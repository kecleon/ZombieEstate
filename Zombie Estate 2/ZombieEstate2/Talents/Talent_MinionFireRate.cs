using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200017D RID: 381
	internal class Talent_MinionFireRate : TalentNode
	{
		// Token: 0x06000B53 RID: 2899 RVA: 0x0005DC68 File Offset: 0x0005BE68
		public Talent_MinionFireRate(Point loc, Point topLeft, int level, int scale) : base(new Point(65, 53), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases the firing rate of minions by 5%");
			this.Descriptions.Add("Increases the firing rate of minions by 7%");
			this.Descriptions.Add("Increases the firing rate of minions by 10%");
			this.Name = "Minion Fire Rate";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0005DCD4 File Offset: 0x0005BED4
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.MinionFireRateMod += 5f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.MinionFireRateMod += 7f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.MinionFireRateMod += 10f;
			}
		}
	}
}
