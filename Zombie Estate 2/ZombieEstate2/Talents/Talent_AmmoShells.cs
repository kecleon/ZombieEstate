using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000178 RID: 376
	internal class Talent_AmmoShells : TalentNode
	{
		// Token: 0x06000B49 RID: 2889 RVA: 0x0005D898 File Offset: 0x0005BA98
		public Talent_AmmoShells(Point loc, Point topLeft, int level, int scale) : base(new Point(62, 53), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increase Shells storage capacity by 20.");
			this.Descriptions.Add("Increase Shells storage capacity by 40.");
			this.Descriptions.Add("Increase Shells storage capacity by 60.");
			this.Name = "Assault Ammo";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0005D904 File Offset: 0x0005BB04
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.Ammo_Shells += 20;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.Ammo_Shells += 40;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.Ammo_Shells += 60;
			}
		}
	}
}
