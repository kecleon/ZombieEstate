using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200019B RID: 411
	internal class Talent_MagnetBonus : TalentNode
	{
		// Token: 0x06000B9B RID: 2971 RVA: 0x0005F5DC File Offset: 0x0005D7DC
		public Talent_MagnetBonus(Point loc, Point topLeft, int level, int scale) : base(new Point(71, 52), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases distance that drops will pull to you by 10%.");
			this.Descriptions.Add("Increases distance that drops will pull to you by 20%.");
			this.Name = "Magnet";
			this.ParentNode = null;
			this.MaxLevel = 2;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0005F638 File Offset: 0x0005D838
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.MagnetBonus += 10f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.MagnetBonus += 20f;
			}
		}
	}
}
