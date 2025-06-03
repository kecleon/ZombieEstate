using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000199 RID: 409
	internal class Talent_HealthI : TalentNode
	{
		// Token: 0x06000B97 RID: 2967 RVA: 0x0005F532 File Offset: 0x0005D732
		public Talent_HealthI(Point loc, Point topLeft, int level, int scale) : base(new Point(62, 49), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases health by 25.");
			this.Name = "Health Boost I";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0005F571 File Offset: 0x0005D771
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.MaxHealth += 25f;
			}
		}
	}
}
