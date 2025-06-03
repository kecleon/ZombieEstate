using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200019E RID: 414
	internal class Talent_MoneyBonus : TalentNode
	{
		// Token: 0x06000BA1 RID: 2977 RVA: 0x0005F75C File Offset: 0x0005D95C
		public Talent_MoneyBonus(Point loc, Point topLeft, int level, int scale) : base(new Point(62, 51), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases money gained by 2.5%.");
			this.Descriptions.Add("Increases money gained by 5%.");
			this.Descriptions.Add("Increases money gained by 8%.");
			this.Name = "Money Boost";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0005F7C8 File Offset: 0x0005D9C8
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.MoneyBonus += 2.5f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.MoneyBonus += 5f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.MoneyBonus += 8f;
			}
		}
	}
}
