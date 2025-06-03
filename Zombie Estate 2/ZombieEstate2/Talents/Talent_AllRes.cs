using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200017B RID: 379
	internal class Talent_AllRes : TalentNode
	{
		// Token: 0x06000B4F RID: 2895 RVA: 0x0005DAB8 File Offset: 0x0005BCB8
		public Talent_AllRes(Point loc, Point topLeft, int level, int scale) : base(new Point(71, 50), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases resitances against all elements by 5.");
			this.Descriptions.Add("Increases resitances against all elements by 10.");
			this.Descriptions.Add("Increases resitances against all elements by 15.");
			this.Name = "All Resistances";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0005DB24 File Offset: 0x0005BD24
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.FireResist += 5;
				player.TalentSpecProps.WaterResist += 5;
				player.TalentSpecProps.EarthResist += 5;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.FireResist += 10;
				player.TalentSpecProps.WaterResist += 10;
				player.TalentSpecProps.EarthResist += 10;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.FireResist += 15;
				player.TalentSpecProps.WaterResist += 15;
				player.TalentSpecProps.EarthResist += 15;
			}
		}
	}
}
