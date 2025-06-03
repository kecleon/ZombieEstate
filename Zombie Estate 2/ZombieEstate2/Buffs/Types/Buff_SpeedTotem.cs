using System;
using System.Collections.Generic;
using System.Globalization;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001F3 RID: 499
	internal class Buff_SpeedTotem : Buff
	{
		// Token: 0x06000D56 RID: 3414 RVA: 0x0006A727 File Offset: 0x00068927
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 1f;
			this.Src = Global.GetTexRectange(77, 18);
			this.Positive = true;
			this.Name = "Speed Totem";
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0006A75D File Offset: 0x0006895D
		public override void Arguments(List<string> args)
		{
			this.speed = float.Parse(args[0], CultureInfo.InvariantCulture);
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0006A776 File Offset: 0x00068976
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.SwingTimeMod += this.speed;
			spec.ShotTimeMod += this.speed;
			return true;
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00069E94 File Offset: 0x00068094
		public override string Description
		{
			get
			{
				return string.Format("", new object[0]);
			}
		}

		// Token: 0x04000DAD RID: 3501
		private float speed;
	}
}
