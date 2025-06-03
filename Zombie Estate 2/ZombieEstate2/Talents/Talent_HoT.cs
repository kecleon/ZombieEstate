using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200018C RID: 396
	internal class Talent_HoT : TalentNode
	{
		// Token: 0x06000B71 RID: 2929 RVA: 0x0005E8F4 File Offset: 0x0005CAF4
		public Talent_HoT(Point loc, Point topLeft, int level, int scale) : base(new Point(68, 49), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Heal 0.5 health every 3 seconds.");
			this.Descriptions.Add("Heal 0.75 health every 3 seconds.");
			this.Descriptions.Add("Heal 1.0 health every 3 seconds.");
			this.Name = "Heal Over Time";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0005E960 File Offset: 0x0005CB60
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.HealOverTime += 0.5f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.HealOverTime += 0.75f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.HealOverTime += 1f;
			}
		}
	}
}
