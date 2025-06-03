using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000186 RID: 390
	internal class Talent_ExplosionDmg : TalentNode
	{
		// Token: 0x06000B65 RID: 2917 RVA: 0x0005E3EC File Offset: 0x0005C5EC
		public Talent_ExplosionDmg(Point loc, Point topLeft, int level, int scale) : base(new Point(70, 48), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases radius of all explosion damage by 5%.");
			this.Descriptions.Add("Increases radius of all explosion damage by 10%.");
			this.Descriptions.Add("Increases radius of all explosion damage by 20%.");
			this.Name = "High Explosive";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0005E458 File Offset: 0x0005C658
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.ExplosionDamageMod += 0.05f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.ExplosionDamageMod += 0.1f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.ExplosionDamageMod += 0.2f;
			}
		}
	}
}
