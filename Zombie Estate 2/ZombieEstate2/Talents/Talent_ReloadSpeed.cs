using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x020001A3 RID: 419
	internal class Talent_ReloadSpeed : TalentNode
	{
		// Token: 0x06000BAB RID: 2987 RVA: 0x0005FA8C File Offset: 0x0005DC8C
		public Talent_ReloadSpeed(Point loc, Point topLeft, int level, int scale) : base(new Point(62, 50), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Decrease reload time by 5%.");
			this.Descriptions.Add("Decrease reload time by 10%.");
			this.Descriptions.Add("Decrease reload time by 20%.");
			this.Name = "Reload Speed";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0005FAF8 File Offset: 0x0005DCF8
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 1)
			{
				player.TalentSpecProps.ReloadTimeMod += 5f;
				return;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.ReloadTimeMod += 10f;
				return;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.ReloadTimeMod += 20f;
			}
		}
	}
}
