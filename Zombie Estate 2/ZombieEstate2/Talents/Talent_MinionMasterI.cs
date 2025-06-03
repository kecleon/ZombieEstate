using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200019A RID: 410
	internal class Talent_MinionMasterI : TalentNode
	{
		// Token: 0x06000B99 RID: 2969 RVA: 0x0005F59C File Offset: 0x0005D79C
		public Talent_MinionMasterI(Point loc, Point topLeft, int level, int scale) : base(new Point(63, 51), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases the number of active minions to 2.");
			this.Name = "Minion Master I";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0005EF6A File Offset: 0x0005D16A
		protected override void ApplyTalent(Player player)
		{
			player.TalentSpecProps.MinionCount++;
		}
	}
}
