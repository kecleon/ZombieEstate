using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200017F RID: 383
	internal class Talent_MinionDmg : TalentNode
	{
		// Token: 0x06000B57 RID: 2903 RVA: 0x0005DE30 File Offset: 0x0005C030
		public Talent_MinionDmg(Point loc, Point topLeft, int level, int scale) : base(new Point(64, 53), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases damage delt by minions and turrets by 5%.");
			this.Descriptions.Add("Increases damage delt by minions and turrets by 10%.");
			this.Descriptions.Add("Increases damage delt by minions and turrets by 20%.");
			this.Name = "Minion Damage";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0005DE9C File Offset: 0x0005C09C
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.MinionDmgMod += 5f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.MinionDmgMod += 10f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.MinionDmgMod += 20f;
			}
		}
	}
}
