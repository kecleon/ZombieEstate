using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000179 RID: 377
	internal class Talent_AmmoAssault : TalentNode
	{
		// Token: 0x06000B4B RID: 2891 RVA: 0x0005D974 File Offset: 0x0005BB74
		public Talent_AmmoAssault(Point loc, Point topLeft, int level, int scale) : base(new Point(62, 52), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increase Assault Ammo storage capacity by 50.");
			this.Descriptions.Add("Increase Assault Ammo storage capacity by 100.");
			this.Descriptions.Add("Increase Assault Ammo storage capacity by 200.");
			this.Name = "Assault Ammo";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0005D9E0 File Offset: 0x0005BBE0
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.Ammo_Assault += 50;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.Ammo_Assault += 100;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.Ammo_Assault += 200;
			}
		}
	}
}
