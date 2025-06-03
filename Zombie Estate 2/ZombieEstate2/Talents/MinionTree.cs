using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x020001A7 RID: 423
	internal class MinionTree : TalentTree
	{
		// Token: 0x06000BB9 RID: 3001 RVA: 0x0005FB67 File Offset: 0x0005DD67
		public MinionTree(Point pos, int scale) : base(pos, scale)
		{
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0005FF6C File Offset: 0x0005E16C
		public override void AddNodes(int scale)
		{
			this.Nodes[0, 0] = new Talent_MinionFireRate(new Point(0, 0), this.TopLeft, 0, scale);
			this.Nodes[2, 0] = new Talent_MinionAmmo(new Point(2, 0), this.TopLeft, 0, scale);
			this.Nodes[1, 1] = new Talent_MinionMasterI(new Point(1, 1), this.TopLeft, 0, scale);
			this.Nodes[0, 1] = new Talent_NumBullMod(new Point(0, 1), this.TopLeft, 0, scale);
			this.Nodes[0, 2] = new Talent_MinionDmg(new Point(0, 2), this.TopLeft, 0, scale);
			this.Nodes[1, 2] = new Talent_MinionMasterII(new Point(1, 2), this.TopLeft, 0, scale);
			this.Nodes[1, 3] = new Talent_MinionBezerk(new Point(1, 3), this.TopLeft, 0, scale);
			TalentNode.LinkNodes(this.Nodes[1, 1], this.Nodes[1, 2]);
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0006007F File Offset: 0x0005E27F
		public override string Name
		{
			get
			{
				return "Minion Master";
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00060086 File Offset: 0x0005E286
		public override Color BGColor
		{
			get
			{
				return new Color(0.8f, 0.3f, 0.8f);
			}
		}
	}
}
