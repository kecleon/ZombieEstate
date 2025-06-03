using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000176 RID: 374
	internal class Talent_AmmoHeavy : TalentNode
	{
		// Token: 0x06000B45 RID: 2885 RVA: 0x0005D6E0 File Offset: 0x0005B8E0
		public Talent_AmmoHeavy(Point loc, Point topLeft, int level, int scale) : base(new Point(63, 52), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increase Heavy Ammo storage capacity by 100.");
			this.Descriptions.Add("Increase Heavy Ammo storage capacity by 200.");
			this.Descriptions.Add("Increase Heavy Ammo storage capacity by 300.");
			this.Name = "Heavy Ammo";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0005D74C File Offset: 0x0005B94C
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.Ammo_Heavy += 100;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.Ammo_Heavy += 200;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.Ammo_Heavy += 300;
			}
		}
	}
}
