using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000177 RID: 375
	internal class Talent_AmmoExplosive : TalentNode
	{
		// Token: 0x06000B47 RID: 2887 RVA: 0x0005D7C0 File Offset: 0x0005B9C0
		public Talent_AmmoExplosive(Point loc, Point topLeft, int level, int scale) : base(new Point(63, 53), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increase Explosive Ammo storage capacity by 5.");
			this.Descriptions.Add("Increase Explosive Ammo storage capacity by 10.");
			this.Descriptions.Add("Increase Explosive Ammo storage capacity by 20.");
			this.Name = "Explosive Ammo";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0005D82C File Offset: 0x0005BA2C
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.Ammo_Explosive += 5;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.Ammo_Explosive += 10;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.Ammo_Explosive += 20;
			}
		}
	}
}
