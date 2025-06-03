using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000180 RID: 384
	internal class Talent_NumBullMod : TalentNode
	{
		// Token: 0x06000B59 RID: 2905 RVA: 0x0005DF14 File Offset: 0x0005C114
		public Talent_NumBullMod(Point loc, Point topLeft, int level, int scale) : base(new Point(64, 52), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Decreases the number of bullets required to shoot a gun by 10%.");
			this.Descriptions.Add("Decreases the number of bullets required to shoot a gun by 15%.");
			this.Descriptions.Add("Decreases the number of bullets required to shoot a gun by 25%.");
			this.Name = "High Efficiency";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0005DF80 File Offset: 0x0005C180
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.NumBulletsMod += 10f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.NumBulletsMod += 15f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.NumBulletsMod += 25f;
			}
		}
	}
}
