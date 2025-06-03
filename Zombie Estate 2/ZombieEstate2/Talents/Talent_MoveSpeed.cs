using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Talents
{
	// Token: 0x020001A2 RID: 418
	internal class Talent_MoveSpeed : TalentNode
	{
		// Token: 0x06000BA9 RID: 2985 RVA: 0x0005F990 File Offset: 0x0005DB90
		public Talent_MoveSpeed(Point loc, Point topLeft, int level, int scale) : base(new Point(62, 48), loc, topLeft, level, scale)
		{
			this.Descriptions.Add("Increases move speed by 0.1.");
			this.Descriptions.Add("Increases move speed by 0.2.");
			this.Descriptions.Add("Increases move speed by 0.4.");
			this.Name = "Move Speed";
			this.ParentNode = null;
			this.MaxLevel = 3;
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0005FA14 File Offset: 0x0005DC14
		protected override void ApplyTalent(Player player)
		{
			if (this.Level == 0)
			{
				return;
			}
			if (this.Level == 1)
			{
				player.TalentSpecProps.Speed += 0.1f;
			}
			if (this.Level == 2)
			{
				player.TalentSpecProps.Speed += 0.2f;
			}
			if (this.Level == 3)
			{
				player.TalentSpecProps.Speed += 0.4f;
			}
		}

		// Token: 0x04000C1B RID: 3099
		private float[] mod = new float[]
		{
			102.5f,
			105f,
			110f
		};
	}
}
