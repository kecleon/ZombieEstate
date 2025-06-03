using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000187 RID: 391
	internal class Talent_ExplosionRadius : TalentNode
	{
		// Token: 0x06000B67 RID: 2919 RVA: 0x0005E4CE File Offset: 0x0005C6CE
		public Talent_ExplosionRadius(Point loc, Point topLeft, int level, int scale) : base(new Point(71, 48), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases radius of all explosions by 15%.");
			this.Name = "Explosion Radius";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0005E50D File Offset: 0x0005C70D
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.ExplosionRadiusMod += 0.15f;
			}
		}
	}
}
