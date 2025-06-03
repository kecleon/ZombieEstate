using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000191 RID: 401
	internal class Talent_DamageReduction : TalentNode
	{
		// Token: 0x06000B7B RID: 2939 RVA: 0x0005EBE0 File Offset: 0x0005CDE0
		public Talent_DamageReduction(Point loc, Point topLeft, int level, int scale) : base(new Point(65, 50), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases armor by 5.");
			this.Descriptions.Add("Increases armor by 10.");
			this.Descriptions.Add("Increases armor by 15.");
			this.Name = "Guard";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0005EC4C File Offset: 0x0005CE4C
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.Armor += 5;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.Armor += 10;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.Armor += 15;
			}
		}
	}
}
