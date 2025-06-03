using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000198 RID: 408
	internal class Talent_COPY : TalentNode
	{
		// Token: 0x06000B95 RID: 2965 RVA: 0x0005F4CD File Offset: 0x0005D6CD
		public Talent_COPY(Point loc, Point topLeft, int level, int scale) : base(new Point(0, 0), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("hi");
			this.Name = "hi";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0005F50A File Offset: 0x0005D70A
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				return;
			}
			if (this.Level == 2)
			{
				return;
			}
			int level = this.Level;
		}
	}
}
