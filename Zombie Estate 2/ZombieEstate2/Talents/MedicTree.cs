using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x020001A6 RID: 422
	internal class MedicTree : TalentTree
	{
		// Token: 0x06000BB5 RID: 2997 RVA: 0x0005FB67 File Offset: 0x0005DD67
		public MedicTree(Point pos, int scale) : base(pos, scale)
		{
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0005FE3C File Offset: 0x0005E03C
		public override void AddNodes(int scale)
		{
			this.Nodes[0, 0] = new Talent_AllRes(new Point(0, 0), this.TopLeft, 0, scale);
			this.Nodes[1, 0] = new Talent_HealthyHealsMedic(new Point(1, 0), this.TopLeft, 0, scale);
			this.Nodes[2, 0] = new Talent_HealthPack(new Point(2, 0), this.TopLeft, 0, scale);
			this.Nodes[0, 1] = new Talent_ArmorOnHeal(new Point(0, 1), this.TopLeft, 0, scale);
			this.Nodes[2, 1] = new Talent_HoT(new Point(2, 1), this.TopLeft, 0, scale);
			this.Nodes[0, 2] = new Talent_AmountArmorOnHeal(new Point(0, 2), this.TopLeft, 0, scale);
			this.Nodes[1, 2] = new Talent_HealDone(new Point(1, 2), this.TopLeft, 0, scale);
			TalentNode.LinkNodes(this.Nodes[0, 1], this.Nodes[0, 2]);
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0005FF4F File Offset: 0x0005E14F
		public override string Name
		{
			get
			{
				return "Medic";
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x0005FF56 File Offset: 0x0005E156
		public override Color BGColor
		{
			get
			{
				return new Color(0.4f, 0.8f, 0.5f);
			}
		}
	}
}
