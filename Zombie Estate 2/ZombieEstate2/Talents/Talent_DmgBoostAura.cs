using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200019F RID: 415
	internal class Talent_DmgBoostAura : TalentNode
	{
		// Token: 0x06000BA3 RID: 2979 RVA: 0x0005F83E File Offset: 0x0005DA3E
		public Talent_DmgBoostAura(Point loc, Point topLeft, int level, int scale) : base(new Point(69, 51), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("You and your close by allies deal 5% more damage with guns, melee weapons, minions, and explosives.");
			this.Name = "Damage Aura";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0005F87D File Offset: 0x0005DA7D
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.AddBuff("Buff_DmgAura", player, "");
			}
		}
	}
}
