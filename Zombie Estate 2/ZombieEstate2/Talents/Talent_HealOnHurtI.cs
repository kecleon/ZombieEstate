using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000194 RID: 404
	internal class Talent_HealOnHurtI : TalentNode
	{
		// Token: 0x06000B81 RID: 2945 RVA: 0x0005EE04 File Offset: 0x0005D004
		public Talent_HealOnHurtI(Point loc, Point topLeft, int level, int scale) : base(new Point(63, 50), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("When hit, you will have a 1% chance to be healed by 5.");
			this.Descriptions.Add("When hit, you will have a 2.5% chance to be healed by 5.");
			this.Descriptions.Add("When hit, you will have a 5% chance to be healed by 5.");
			this.Name = "Absorb";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0005EE70 File Offset: 0x0005D070
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.HealOnHitAmount += 5f;
				player.TalentSpecProps.HealOnHitPercent += 0.01f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.HealOnHitAmount += 5f;
				player.TalentSpecProps.HealOnHitPercent += 0.025f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.HealOnHitAmount += 5f;
				player.TalentSpecProps.HealOnHitPercent += 0.05f;
			}
		}
	}
}
