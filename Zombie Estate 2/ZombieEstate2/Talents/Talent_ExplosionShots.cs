using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000181 RID: 385
	internal class Talent_ExplosionShots : TalentNode
	{
		// Token: 0x06000B5B RID: 2907 RVA: 0x0005DFF6 File Offset: 0x0005C1F6
		public Talent_ExplosionShots(Point loc, Point topLeft, int level, int scale) : base(new Point(70, 49), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("When a zombie is killed by you, there is a 5% chance that it will explode dealing area of effect damage around it.");
			this.Name = "Explosive Shots";
			this.ParentNode = null;
			this.MaxLevel = 1;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0005E035 File Offset: 0x0005C235
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.AddBuff("Buff_ExplosiveShots", player, "");
			}
		}
	}
}
