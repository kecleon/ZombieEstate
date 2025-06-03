using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200019C RID: 412
	internal class Talent_CritAura : TalentNode
	{
		// Token: 0x06000B9D RID: 2973 RVA: 0x0005F68E File Offset: 0x0005D88E
		public Talent_CritAura(Point loc, Point topLeft, int level, int scale) : base(new Point(72, 52), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("You and your close by allies gain 5% bonus critical strike chance. Does not stack with other Crit Auras.");
			this.Name = "Crit Aura";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0005F6CD File Offset: 0x0005D8CD
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.AddBuff("Buff_CritAura", player, "");
			}
		}
	}
}
