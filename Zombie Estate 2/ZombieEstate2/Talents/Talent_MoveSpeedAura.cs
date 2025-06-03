using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x020001A1 RID: 417
	internal class Talent_MoveSpeedAura : TalentNode
	{
		// Token: 0x06000BA7 RID: 2983 RVA: 0x0005F92A File Offset: 0x0005DB2A
		public Talent_MoveSpeedAura(Point loc, Point topLeft, int level, int scale) : base(new Point(68, 51), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("You and your close by allies gain 0.1 move speed. Does not stack with other Move Quicks.");
			this.Name = "Move Quick";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0005F969 File Offset: 0x0005DB69
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.AddBuff("Buff_MoveQuickAura", player, "");
			}
		}
	}
}
