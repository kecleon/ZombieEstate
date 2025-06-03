using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000192 RID: 402
	internal class Talent_HealthyHeals : TalentNode
	{
		// Token: 0x06000B7D RID: 2941 RVA: 0x0005ECB8 File Offset: 0x0005CEB8
		public Talent_HealthyHeals(Point loc, Point topLeft, int level, int scale) : base(new Point(68, 50), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases all healing effects done to you by 3%.");
			this.Descriptions.Add("Increases all healing effects done to you by 7%.");
			this.Descriptions.Add("Increases all healing effects done to you by 10%.");
			this.Name = "Healthy Tank Heals";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0005ED24 File Offset: 0x0005CF24
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.HealRecievedMod += 0.03f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.HealRecievedMod += 0.07f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.HealRecievedMod += 0.1f;
			}
		}
	}
}
