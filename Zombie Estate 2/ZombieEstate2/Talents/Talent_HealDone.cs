using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200018B RID: 395
	internal class Talent_HealDone : TalentNode
	{
		// Token: 0x06000B6F RID: 2927 RVA: 0x0005E810 File Offset: 0x0005CA10
		public Talent_HealDone(Point loc, Point topLeft, int level, int scale) : base(new Point(67, 50), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases healing done by 5%.");
			this.Descriptions.Add("Increases healing done by 10%.");
			this.Descriptions.Add("Increases healing done by 15%.");
			this.Name = "Heal Increase";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0005E87C File Offset: 0x0005CA7C
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.HealingDoneMod += 5f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.HealingDoneMod += 10f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.HealingDoneMod += 15f;
			}
		}
	}
}
