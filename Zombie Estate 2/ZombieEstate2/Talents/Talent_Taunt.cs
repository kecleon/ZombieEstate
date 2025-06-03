using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000174 RID: 372
	internal class Talent_Taunt : TalentNode
	{
		// Token: 0x06000B41 RID: 2881 RVA: 0x0005D5A4 File Offset: 0x0005B7A4
		public Talent_Taunt(Point loc, Point topLeft, int level, int scale) : base(new Point(70, 50), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Right click to taunt enemies in a large range into focusing on you. Has a 5 second cooldown.");
			this.Name = "Taunt";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0005D5E3 File Offset: 0x0005B7E3
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.Ability = new AbilityTaunt(player);
			}
		}
	}
}
