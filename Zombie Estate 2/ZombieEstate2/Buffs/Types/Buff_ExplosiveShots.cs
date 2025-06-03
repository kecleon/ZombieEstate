using System;
using System.Collections.Generic;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001E5 RID: 485
	internal class Buff_ExplosiveShots : Buff
	{
		// Token: 0x06000CFC RID: 3324 RVA: 0x00069AB8 File Offset: 0x00067CB8
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 0f;
			this.Src = Global.GetTexRectange(70, 49);
			this.Positive = true;
			this.Name = "Explosive Shot";
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00069AEE File Offset: 0x00067CEE
		public override void KilledATarget(Shootable target)
		{
			if (Global.Probability(5))
			{
				Explosion.CreateExplosion(1.5f, 50f, 0.5f, "Fire", target.Position, this.Parent, false);
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00069B1E File Offset: 0x00067D1E
		public override void Update(float elapsed)
		{
			base.Update(elapsed);
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00005D3F File Offset: 0x00003F3F
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			return true;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x00069B27 File Offset: 0x00067D27
		public override void Tick()
		{
			base.Tick();
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00069B2F File Offset: 0x00067D2F
		public override string Description
		{
			get
			{
				return string.Format("When a zombie is killed, it has a 5% chance to explode.", new object[0]);
			}
		}
	}
}
