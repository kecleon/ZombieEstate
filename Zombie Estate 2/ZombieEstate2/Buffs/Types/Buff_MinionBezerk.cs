using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001EE RID: 494
	internal class Buff_MinionBezerk : Buff
	{
		// Token: 0x06000D32 RID: 3378 RVA: 0x0006A194 File Offset: 0x00068394
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 1f;
			this.Src = Global.GetTexRectange(2, 33);
			this.Positive = true;
			this.Effect = new AnimatedGameObject(Buff_MinionBezerk.Anim, 0.2f, this.Parent.Position, this.Parent);
			Global.MasterCache.CreateObject(this.Effect);
			this.Name = "Bezerk";
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0006A20B File Offset: 0x0006840B
		public override void Arguments(List<string> args)
		{
			this.Time = float.Parse(args[0], CultureInfo.InvariantCulture);
			this.amount = float.Parse(args[1], CultureInfo.InvariantCulture);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0006A23B File Offset: 0x0006843B
		public override void Update(float elapsed)
		{
			base.TopOfParent(out this.Effect.Position);
			base.Update(elapsed);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0006A255 File Offset: 0x00068455
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.ShotTimeMod += this.amount;
			spec.Speed += this.amount * this.Parent.BaseSpecialProperties.Speed;
			return true;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000D36 RID: 3382 RVA: 0x0006A291 File Offset: 0x00068491
		public override string Description
		{
			get
			{
				return string.Format("Target is enraged, firing faster.", new object[0]);
			}
		}

		// Token: 0x04000DA7 RID: 3495
		private float amount;

		// Token: 0x04000DA8 RID: 3496
		private static List<Point> Anim = new List<Point>
		{
			new Point(62, 54)
		};
	}
}
