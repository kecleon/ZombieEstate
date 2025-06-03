using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200019D RID: 413
	internal class Talent_MoneyBonusII : TalentNode
	{
		// Token: 0x06000B9F RID: 2975 RVA: 0x0005F6F2 File Offset: 0x0005D8F2
		public Talent_MoneyBonusII(Point loc, Point topLeft, int level, int scale) : base(new Point(72, 51), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases money gained by another 5%.");
			this.Name = "Money Boost II";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0005F731 File Offset: 0x0005D931
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.MoneyBonus += 5f;
			}
		}
	}
}
