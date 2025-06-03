using System;
using System.Collections.Generic;
using System.Globalization;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001F0 RID: 496
	internal class Buff_ArmorTotem : Buff
	{
		// Token: 0x06000D3F RID: 3391 RVA: 0x0006A427 File Offset: 0x00068627
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 1f;
			this.Src = Global.GetTexRectange(16, 39);
			this.Positive = true;
			this.Name = "Armor Totem";
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0006A45D File Offset: 0x0006865D
		public override void Arguments(List<string> args)
		{
			this.armor = int.Parse(args[0], CultureInfo.InvariantCulture);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00069B1E File Offset: 0x00067D1E
		public override void Update(float elapsed)
		{
			base.Update(elapsed);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0006A476 File Offset: 0x00068676
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.Armor += this.armor;
			return true;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00069B27 File Offset: 0x00067D27
		public override void Tick()
		{
			base.Tick();
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x0006A48D File Offset: 0x0006868D
		public override string Description
		{
			get
			{
				return string.Format("Target gains {0} armor.", this.armor);
			}
		}

		// Token: 0x04000DAA RID: 3498
		private int armor = 10;
	}
}
