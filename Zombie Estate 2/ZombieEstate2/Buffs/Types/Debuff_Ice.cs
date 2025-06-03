using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001FC RID: 508
	internal class Debuff_Ice : Buff
	{
		// Token: 0x06000D8E RID: 3470 RVA: 0x0006B2A4 File Offset: 0x000694A4
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = this.total;
			this.Src = Global.GetTexRectange(13, 33);
			this.Positive = false;
			this.Name = Debuff_Ice.NAME;
			this.Effect = new AnimatedGameObject(Debuff_Ice.StunAnim, 0.3f, this.Parent.Position, this.Parent);
			this.Effect.Position.Y = 0.5f;
			Global.MasterCache.CreateObject(this.Effect);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0006ABEE File Offset: 0x00068DEE
		public override void Update(float elapsed)
		{
			base.FrontOfParent(out this.Effect.Position);
			this.Effect.Position.Y = 0.5f;
			base.Update(elapsed);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0006B332 File Offset: 0x00069532
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.Speed = -(this.slowAmount * this.Parent.BaseSpecialProperties.Speed);
			return true;
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0006B354 File Offset: 0x00069554
		public override void Arguments(List<string> args)
		{
			this.Time = float.Parse(args[0], CultureInfo.InvariantCulture);
			this.slowAmount = float.Parse(args[1], CultureInfo.InvariantCulture);
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x0006B384 File Offset: 0x00069584
		public override string Description
		{
			get
			{
				return string.Format("Target is slowed for {0} seconds.", this.total);
			}
		}

		// Token: 0x04000DC2 RID: 3522
		private float total = 5f;

		// Token: 0x04000DC3 RID: 3523
		private float slowAmount = 0.5f;

		// Token: 0x04000DC4 RID: 3524
		private static List<Point> StunAnim = new List<Point>
		{
			new Point(4, 29)
		};

		// Token: 0x04000DC5 RID: 3525
		private static string NAME = "Frozen";
	}
}
