using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x020001A8 RID: 424
	internal class OffenseTree : TalentTree
	{
		// Token: 0x06000BBD RID: 3005 RVA: 0x0005FB67 File Offset: 0x0005DD67
		public OffenseTree(Point pos, int scale) : base(pos, scale)
		{
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0006009C File Offset: 0x0005E29C
		public override void AddNodes(int scale)
		{
			this.Nodes[0, 0] = new Talent_GunDmg(new Point(0, 0), this.TopLeft, 0, scale);
			this.Nodes[2, 0] = new Talent_MeleeDmg(new Point(2, 0), this.TopLeft, 0, scale);
			this.Nodes[0, 1] = new Talent_ExplosionDmg(new Point(0, 1), this.TopLeft, 0, scale);
			this.Nodes[1, 1] = new Talent_RateOfFire(new Point(1, 1), this.TopLeft, 0, scale);
			this.Nodes[2, 1] = new Talent_MeleeRateOfFire(new Point(2, 1), this.TopLeft, 0, scale);
			this.Nodes[0, 2] = new Talent_ExplosionRadius(new Point(0, 2), this.TopLeft, 0, scale);
			this.Nodes[2, 2] = new Talent_LifeSteal(new Point(2, 2), this.TopLeft, 0, scale);
			this.Nodes[1, 3] = new Talent_ExplosionShots(new Point(1, 3), this.TopLeft, 0, scale);
			TalentNode.LinkNodes(this.Nodes[0, 1], this.Nodes[0, 2]);
			TalentNode.LinkNodes(this.Nodes[2, 0], this.Nodes[2, 1]);
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x000601EF File Offset: 0x0005E3EF
		public override string Name
		{
			get
			{
				return "Offense";
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x000601F6 File Offset: 0x0005E3F6
		public override Color BGColor
		{
			get
			{
				return new Color(0.8f, 0.2f, 0.2f);
			}
		}
	}
}
