using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200018E RID: 398
	internal class Talent_HealthIII : TalentNode
	{
		// Token: 0x06000B75 RID: 2933 RVA: 0x0005EABA File Offset: 0x0005CCBA
		public Talent_HealthIII(Point loc, Point topLeft, int level, int scale) : base(new Point(63, 48), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases health by 100.");
			this.Name = "Health Boost III";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0005EAF9 File Offset: 0x0005CCF9
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.MaxHealth += 100f;
			}
		}
	}
}
