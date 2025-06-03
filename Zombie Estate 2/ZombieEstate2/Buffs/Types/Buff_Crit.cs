using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001ED RID: 493
	internal class Buff_Crit : Buff
	{
		// Token: 0x06000D2C RID: 3372 RVA: 0x0006A0E4 File Offset: 0x000682E4
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 30f;
			this.Src = Global.GetTexRectange(67, 40);
			this.Positive = true;
			this.TickLength = 0.1f;
			this.Name = "Crit Boost";
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0006A130 File Offset: 0x00068330
		public override void Tick()
		{
			Vector3 randomPosition = VerchickMath.GetRandomPosition(this.Parent.Position, 0.15f);
			Global.MasterCache.CreateParticle(ParticleType.CritBoost, randomPosition, Vector3.Zero);
			base.Tick();
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0006A16B File Offset: 0x0006836B
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.CritChance += 100f;
			return true;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x0006A181 File Offset: 0x00068381
		public override string Description
		{
			get
			{
				return string.Format("Player gains 100% crit chance.", new object[0]);
			}
		}
	}
}
