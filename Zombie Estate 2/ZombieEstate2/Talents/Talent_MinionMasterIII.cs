using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000195 RID: 405
	internal class Talent_MinionMasterIII : TalentNode
	{
		// Token: 0x06000B83 RID: 2947 RVA: 0x0005EF2B File Offset: 0x0005D12B
		public Talent_MinionMasterIII(Point loc, Point topLeft, int level, int scale) : base(new Point(65, 51), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases the number of active minions by 3.");
			this.Name = "Minion Master III";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0005EF6A File Offset: 0x0005D16A
		protected override void ApplyTalent(Player player)
		{
			player.TalentSpecProps.MinionCount++;
		}
	}
}
