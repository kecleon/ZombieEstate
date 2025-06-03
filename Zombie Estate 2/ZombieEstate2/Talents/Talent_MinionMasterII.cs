using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000196 RID: 406
	internal class Talent_MinionMasterII : TalentNode
	{
		// Token: 0x06000B85 RID: 2949 RVA: 0x0005EF7F File Offset: 0x0005D17F
		public Talent_MinionMasterII(Point loc, Point topLeft, int level, int scale) : base(new Point(64, 51), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases the number of active minions to 3.");
			this.Name = "Minion Master II";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0005EF6A File Offset: 0x0005D16A
		protected override void ApplyTalent(Player player)
		{
			player.TalentSpecProps.MinionCount++;
		}
	}
}
