using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001FD RID: 509
	internal class Debuff_Stunned : Buff
	{
		// Token: 0x06000D96 RID: 3478 RVA: 0x0006B3F0 File Offset: 0x000695F0
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = this.total;
			this.Src = Global.GetTexRectange(0, 26);
			this.Positive = false;
			this.Name = "Stunned";
			this.Effect = new AnimatedGameObject(Debuff_Stunned.StunAnim, 0.3f, this.Parent.Position, this.Parent);
			Global.MasterCache.CreateObject(this.Effect);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0006A23B File Offset: 0x0006843B
		public override void Update(float elapsed)
		{
			base.TopOfParent(out this.Effect.Position);
			base.Update(elapsed);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0006B468 File Offset: 0x00069668
		public override float ApplySpeedMod(float start)
		{
			return 0f;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0006B46F File Offset: 0x0006966F
		public override void Arguments(List<string> args)
		{
			this.Time = float.Parse(args[0], CultureInfo.InvariantCulture);
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x0006B488 File Offset: 0x00069688
		public override string Description
		{
			get
			{
				return string.Format("Target is stunned for {0} seconds.", this.total);
			}
		}

		// Token: 0x04000DC6 RID: 3526
		private float total = 5f;

		// Token: 0x04000DC7 RID: 3527
		private static List<Point> StunAnim = new List<Point>
		{
			new Point(0, 26),
			new Point(1, 26),
			new Point(2, 26),
			new Point(3, 26)
		};
	}
}
