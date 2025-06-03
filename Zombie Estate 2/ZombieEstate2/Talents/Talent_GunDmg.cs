using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000184 RID: 388
	internal class Talent_GunDmg : TalentNode
	{
		// Token: 0x06000B61 RID: 2913 RVA: 0x0005E224 File Offset: 0x0005C424
		public Talent_GunDmg(Point loc, Point topLeft, int level, int scale) : base(new Point(67, 49), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increase damage done with guns by 5%.");
			this.Descriptions.Add("Increase damage done with guns by 10%.");
			this.Descriptions.Add("Increase damage done with guns by 15%.");
			this.Name = "Gun Boost";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0005E290 File Offset: 0x0005C490
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.BulletDamageMod += 5f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.BulletDamageMod += 10f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.BulletDamageMod += 15f;
			}
		}
	}
}
