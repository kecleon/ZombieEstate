using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000190 RID: 400
	internal class Talent_DamageReductionAura : TalentNode
	{
		// Token: 0x06000B79 RID: 2937 RVA: 0x0005EB8E File Offset: 0x0005CD8E
		public Talent_DamageReductionAura(Point loc, Point topLeft, int level, int scale) : base(new Point(66, 50), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("You and allies close by to you gain 10 armor. Does not stack with other Guard Auras");
			this.Name = "Guard Aura";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0005EBCD File Offset: 0x0005CDCD
		protected override void ApplyTalent(Player player)
		{
			player.AddBuff("Buff_GuardAura", player, "");
		}
	}
}
