using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x020001A0 RID: 416
	internal class Talent_AllResAura : TalentNode
	{
		// Token: 0x06000BA5 RID: 2981 RVA: 0x0005F8A4 File Offset: 0x0005DAA4
		public Talent_AllResAura(Point loc, Point topLeft, int level, int scale) : base(new Point(70, 51), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("You and your close by allies gain 10 All Resistance. Does not stack with other Elemental Resistance Auras.");
			this.Name = "Resistance Aura";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0005F905 File Offset: 0x0005DB05
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.AddBuff("Buff_AllResAura", player, "");
			}
		}

		// Token: 0x04000C1A RID: 3098
		private float[] mod = new float[]
		{
			102.5f,
			105f,
			110f
		};
	}
}
