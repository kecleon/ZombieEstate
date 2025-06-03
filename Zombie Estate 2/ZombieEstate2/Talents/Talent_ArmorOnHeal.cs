using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x0200018A RID: 394
	internal class Talent_ArmorOnHeal : TalentNode
	{
		// Token: 0x06000B6D RID: 2925 RVA: 0x0005E6F4 File Offset: 0x0005C8F4
		public Talent_ArmorOnHeal(Point loc, Point topLeft, int level, int scale) : base(new Point(68, 48), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("When healing a target, they have a 25% chance to gain 5 armor for 5 seconds.");
			this.Descriptions.Add("When healing a target, they have a 50% chance to gain 5 armor for 5 seconds.");
			this.Descriptions.Add("When healing a target, they have a 100% chance to gain 5 armor for 5 seconds.");
			this.Name = "Raise Morale";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0005E760 File Offset: 0x0005C960
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.ArmorGainedOnChanceOnHeal += 5;
				player.TalentSpecProps.ChanceOfArmorOnHeal += 25f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.ArmorGainedOnChanceOnHeal += 5;
				player.TalentSpecProps.ChanceOfArmorOnHeal += 50f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.ArmorGainedOnChanceOnHeal += 5;
				player.TalentSpecProps.ChanceOfArmorOnHeal += 100f;
			}
		}
	}
}
