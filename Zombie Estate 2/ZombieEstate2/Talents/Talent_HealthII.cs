using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200018F RID: 399
	internal class Talent_HealthII : TalentNode
	{
		// Token: 0x06000B77 RID: 2935 RVA: 0x0005EB24 File Offset: 0x0005CD24
		public Talent_HealthII(Point loc, Point topLeft, int level, int scale) : base(new Point(63, 49), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases health by 50.");
			this.Name = "Health Boost II";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0005EB63 File Offset: 0x0005CD63
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.MaxHealth += 50f;
			}
		}
	}
}
