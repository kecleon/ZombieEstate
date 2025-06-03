using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001F7 RID: 503
	internal class Debuff_Sludge : Buff
	{
		// Token: 0x06000D6F RID: 3439 RVA: 0x0006ACC0 File Offset: 0x00068EC0
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = this.total;
			this.Src = Global.GetTexRectange(11, 23);
			this.Positive = false;
			this.Name = "Sludge";
			this.Effect = new AnimatedGameObject(Debuff_Sludge.StunAnim, 0.3f, this.Parent.Position, this.Parent);
			this.Effect.Position.Y = 0.5f;
			Global.MasterCache.CreateObject(this.Effect);
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0006ABEE File Offset: 0x00068DEE
		public override void Update(float elapsed)
		{
			base.FrontOfParent(out this.Effect.Position);
			this.Effect.Position.Y = 0.5f;
			base.Update(elapsed);
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0006AD4E File Offset: 0x00068F4E
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.Speed = -(this.slowAmount * this.Parent.BaseSpecialProperties.Speed);
			return true;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0006AD70 File Offset: 0x00068F70
		public override void Arguments(List<string> args)
		{
			this.Time = float.Parse(args[0], CultureInfo.InvariantCulture);
			this.slowAmount = float.Parse(args[1], CultureInfo.InvariantCulture);
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x0006ADA0 File Offset: 0x00068FA0
		public override string Description
		{
			get
			{
				return string.Format("Target is slowed for {0} seconds.", this.total);
			}
		}

		// Token: 0x04000DB6 RID: 3510
		private float total = 5f;

		// Token: 0x04000DB7 RID: 3511
		private float slowAmount = 0.5f;

		// Token: 0x04000DB8 RID: 3512
		private static List<Point> StunAnim = new List<Point>
		{
			new Point(8, 23)
		};
	}
}
