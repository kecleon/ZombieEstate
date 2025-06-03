using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001FB RID: 507
	internal class Debuff_Fire : Buff
	{
		// Token: 0x06000D88 RID: 3464 RVA: 0x0006B148 File Offset: 0x00069348
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 10f;
			this.Src = Global.GetTexRectange(2, 27);
			this.Positive = false;
			this.Effect = new AnimatedGameObject(Debuff_Fire.Anim, 0.2f, this.Parent.Position, this.Parent);
			Global.MasterCache.CreateObject(this.Effect);
			this.Name = Debuff_Fire.NAME;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0006B1BF File Offset: 0x000693BF
		public override void Arguments(List<string> args)
		{
			this.Time = float.Parse(args[0], CultureInfo.InvariantCulture);
			this.dmg = int.Parse(args[1], CultureInfo.InvariantCulture);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0006B1EF File Offset: 0x000693EF
		public override void Update(float elapsed)
		{
			base.FrontOfParent(out this.Effect.Position);
			base.Update(elapsed);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0006B209 File Offset: 0x00069409
		public override void Tick()
		{
			this.Parent.Damage(this.Attacker, (float)this.dmg, DamageType.Fire, false, false, null);
			base.Tick();
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x0006B22D File Offset: 0x0006942D
		public override string Description
		{
			get
			{
				return string.Format("Target is burned for {0} damage per second.", this.dmg);
			}
		}

		// Token: 0x04000DBF RID: 3519
		private int dmg = 2;

		// Token: 0x04000DC0 RID: 3520
		private static List<Point> Anim = new List<Point>
		{
			new Point(12, 27),
			new Point(13, 27),
			new Point(14, 27),
			new Point(15, 27)
		};

		// Token: 0x04000DC1 RID: 3521
		private static string NAME = "Fire";
	}
}
