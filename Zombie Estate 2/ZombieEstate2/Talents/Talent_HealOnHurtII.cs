using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000193 RID: 403
	internal class Talent_HealOnHurtII : TalentNode
	{
		// Token: 0x06000B7F RID: 2943 RVA: 0x0005ED9A File Offset: 0x0005CF9A
		public Talent_HealOnHurtII(Point loc, Point topLeft, int level, int scale) : base(new Point(64, 50), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Amount healed by Absorb increased to 10.");
			this.Name = "Absorb II";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0005EDD9 File Offset: 0x0005CFD9
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.HealOnHitAmount += 5f;
			}
		}
	}
}
