using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200017C RID: 380
	internal class Talent_MinionBezerk : TalentNode
	{
		// Token: 0x06000B51 RID: 2897 RVA: 0x0005DC06 File Offset: 0x0005BE06
		public Talent_MinionBezerk(Point loc, Point topLeft, int level, int scale) : base(new Point(65, 54), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Right click to refill all active minions' ammo and health and cause them to bezerk for 15 seconds.");
			this.Name = "Minion Bezerk";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0005DC45 File Offset: 0x0005BE45
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.Ability = new AbilityMinionBezerk(player);
			}
		}
	}
}
