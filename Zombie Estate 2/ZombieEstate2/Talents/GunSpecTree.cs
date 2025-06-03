using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x020001A5 RID: 421
	internal class GunSpecTree : TalentTree
	{
		// Token: 0x06000BB1 RID: 2993 RVA: 0x0005FD44 File Offset: 0x0005DF44
		public GunSpecTree(Point pos, int scale) : base(pos, scale)
		{
			this.Nodes[0, 0] = new Talent_AmmoAssault(new Point(0, 0), this.TopLeft, 0, scale);
			this.Nodes[1, 0] = new Talent_AmmoHeavy(new Point(1, 0), this.TopLeft, 0, scale);
			this.Nodes[2, 0] = new Talent_AmmoShells(new Point(2, 0), this.TopLeft, 0, scale);
			this.Nodes[0, 1] = new Talent_ReloadSpeed(new Point(0, 1), this.TopLeft, 0, scale);
			this.Nodes[1, 1] = new Talent_Thrifty(new Point(1, 1), this.TopLeft, 0, scale);
			this.Nodes[2, 1] = new Talent_AmmoExplosive(new Point(2, 1), this.TopLeft, 0, scale);
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void AddNodes(int scale)
		{
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x0005FE1F File Offset: 0x0005E01F
		public override string Name
		{
			get
			{
				return "Gun Specialization";
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x0005FE26 File Offset: 0x0005E026
		public override Color BGColor
		{
			get
			{
				return new Color(0.8f, 0.8f, 0.5f);
			}
		}
	}
}
