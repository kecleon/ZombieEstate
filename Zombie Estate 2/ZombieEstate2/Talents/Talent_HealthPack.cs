using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200017A RID: 378
	internal class Talent_HealthPack : TalentNode
	{
		// Token: 0x06000B4D RID: 2893 RVA: 0x0005DA50 File Offset: 0x0005BC50
		public Talent_HealthPack(Point loc, Point topLeft, int level, int scale) : base(new Point(67, 51), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increase how many health packs you can carry at once by 1.");
			this.Name = "Be Prepared";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0005DA8F File Offset: 0x0005BC8F
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.AllResist += 5;
			}
		}
	}
}
