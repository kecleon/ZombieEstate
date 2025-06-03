using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x020001A9 RID: 425
	internal class UtilityTree : TalentTree
	{
		// Token: 0x06000BC1 RID: 3009 RVA: 0x0005FB67 File Offset: 0x0005DD67
		public UtilityTree(Point pos, int scale) : base(pos, scale)
		{
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0006020C File Offset: 0x0005E40C
		public override void AddNodes(int scale)
		{
			this.Nodes[0, 0] = new Talent_MoveSpeed(new Point(0, 0), this.TopLeft, 0, scale);
			this.Nodes[2, 0] = new Talent_MagnetBonus(new Point(2, 0), this.TopLeft, 0, scale);
			this.Nodes[0, 1] = new Talent_MoveSpeedAura(new Point(0, 1), this.TopLeft, 0, scale);
			this.Nodes[1, 1] = new Talent_AllResAura(new Point(1, 1), this.TopLeft, 0, scale);
			this.Nodes[2, 1] = new Talent_MoneyBonus(new Point(2, 1), this.TopLeft, 0, scale);
			this.Nodes[0, 2] = new Talent_CritAura(new Point(0, 2), this.TopLeft, 0, scale);
			this.Nodes[1, 2] = new Talent_DmgBoostAura(new Point(1, 2), this.TopLeft, 0, scale);
			this.Nodes[2, 2] = new Talent_MoneyBonusII(new Point(2, 2), this.TopLeft, 0, scale);
			TalentNode.LinkNodes(this.Nodes[0, 0], this.Nodes[0, 1]);
			TalentNode.LinkNodes(this.Nodes[2, 1], this.Nodes[2, 2]);
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x0006035F File Offset: 0x0005E55F
		public override string Name
		{
			get
			{
				return "Utility";
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x00060366 File Offset: 0x0005E566
		public override Color BGColor
		{
			get
			{
				return new Color(0.8f, 0.5f, 0.1f);
			}
		}
	}
}
