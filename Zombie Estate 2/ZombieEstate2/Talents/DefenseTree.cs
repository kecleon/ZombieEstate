using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x020001A4 RID: 420
	internal class DefenseTree : TalentTree
	{
		// Token: 0x06000BAD RID: 2989 RVA: 0x0005FB67 File Offset: 0x0005DD67
		public DefenseTree(Point pos, int scale) : base(pos, scale)
		{
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0005FB74 File Offset: 0x0005DD74
		public override void AddNodes(int scale)
		{
			this.Nodes[0, 0] = new Talent_HealthI(new Point(0, 0), this.TopLeft, 0, scale);
			this.Nodes[1, 0] = new Talent_HealOnHurtI(new Point(1, 0), this.TopLeft, 0, scale);
			this.Nodes[0, 1] = new Talent_HealthII(new Point(0, 1), this.TopLeft, 0, scale);
			this.Nodes[1, 1] = new Talent_HealOnHurtII(new Point(1, 1), this.TopLeft, 0, scale);
			this.Nodes[2, 1] = new Talent_DamageReduction(new Point(2, 1), this.TopLeft, 0, scale);
			this.Nodes[0, 2] = new Talent_HealthIII(new Point(0, 2), this.TopLeft, 0, scale);
			this.Nodes[1, 2] = new Talent_HealthyHeals(new Point(1, 2), this.TopLeft, 0, scale);
			this.Nodes[2, 2] = new Talent_DamageReductionAura(new Point(2, 2), this.TopLeft, 0, scale);
			this.Nodes[1, 3] = new Talent_Taunt(new Point(1, 3), this.TopLeft, 0, scale);
			TalentNode.LinkNodes(this.Nodes[1, 0], this.Nodes[1, 1]);
			TalentNode.LinkNodes(this.Nodes[2, 1], this.Nodes[2, 2]);
			TalentNode.LinkNodes(this.Nodes[0, 0], this.Nodes[0, 1]);
			TalentNode.LinkNodes(this.Nodes[0, 1], this.Nodes[0, 2]);
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0005FD26 File Offset: 0x0005DF26
		public override string Name
		{
			get
			{
				return "Defense";
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x0005FD2D File Offset: 0x0005DF2D
		public override Color BGColor
		{
			get
			{
				return new Color(0.3f, 0.5f, 0.8f);
			}
		}
	}
}
