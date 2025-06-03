using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000189 RID: 393
	internal class Talent_AmountArmorOnHeal : TalentNode
	{
		// Token: 0x06000B6B RID: 2923 RVA: 0x0005E61C File Offset: 0x0005C81C
		public Talent_AmountArmorOnHeal(Point loc, Point topLeft, int level, int scale) : base(new Point(69, 48), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases Raise Morale's armor buff to 7 armor.");
			this.Descriptions.Add("Increases Raise Morale's armor buff to 9 armor.");
			this.Descriptions.Add("Increases Raise Morale's armor buff to 12 armor.");
			this.Name = "Raise Morale II";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0005E688 File Offset: 0x0005C888
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.ArmorGainedOnChanceOnHeal += 2;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.ArmorGainedOnChanceOnHeal += 4;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.ArmorGainedOnChanceOnHeal += 7;
			}
		}
	}
}
