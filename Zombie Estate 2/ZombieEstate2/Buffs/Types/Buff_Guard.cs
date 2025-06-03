using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001FA RID: 506
	internal class Buff_Guard : Buff
	{
		// Token: 0x06000D81 RID: 3457 RVA: 0x0006B040 File Offset: 0x00069240
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 30f;
			this.Src = Global.GetTexRectange(67, 38);
			this.Positive = true;
			this.TickLength = 0.1f;
			this.Name = "Guard";
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00069B1E File Offset: 0x00067D1E
		public override void Update(float elapsed)
		{
			base.Update(elapsed);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0006B08C File Offset: 0x0006928C
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.Armor += this.amount;
			spec.FireResist += this.amount;
			spec.WaterResist += this.amount;
			spec.EarthResist += this.amount;
			return true;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0006B0EC File Offset: 0x000692EC
		public override void Tick()
		{
			Vector3 randomPosition = VerchickMath.GetRandomPosition(this.Parent.Position, 0.15f);
			Global.MasterCache.CreateParticle(ParticleType.GuardBoost, randomPosition, Vector3.Zero);
			base.Tick();
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x0006B127 File Offset: 0x00069327
		public override string Description
		{
			get
			{
				return string.Format("Increases player's armor and elemental resistances significantly.", new object[0]);
			}
		}

		// Token: 0x04000DBE RID: 3518
		private int amount = 70;
	}
}
