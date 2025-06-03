using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200017E RID: 382
	internal class Talent_MinionAmmo : TalentNode
	{
		// Token: 0x06000B55 RID: 2901 RVA: 0x0005DD4C File Offset: 0x0005BF4C
		public Talent_MinionAmmo(Point loc, Point topLeft, int level, int scale) : base(new Point(65, 52), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases the starting ammo of all minions and turrets by 5%.");
			this.Descriptions.Add("Increases the starting ammo of all minions and turrets by 10%.");
			this.Descriptions.Add("Increases the starting ammo of all minions and turrets by 25%.");
			this.Name = "Ammo Reserves";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0005DDB8 File Offset: 0x0005BFB8
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.MinionAmmoMod += 5f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.MinionAmmoMod += 10f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.MinionAmmoMod += 25f;
			}
		}
	}
}
