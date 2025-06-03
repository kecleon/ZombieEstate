using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001E7 RID: 487
	internal class Buff_MoveQuick : Buff
	{
		// Token: 0x06000D09 RID: 3337 RVA: 0x00069BCC File Offset: 0x00067DCC
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = PowerUpDrop.DURATION;
			this.Src = Global.GetTexRectange(67, 41);
			this.Positive = true;
			this.TickLength = 0.05f;
			this.Name = "Move Quick";
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00069C18 File Offset: 0x00067E18
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.Speed += 1f;
			return true;
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00069C2E File Offset: 0x00067E2E
		public override void Tick()
		{
			Global.MasterCache.CreateParticle(ParticleType.SpeedBoost, this.Parent.Position + new Vector3(0f, 0f, -0.01f), Vector3.Zero);
			base.Tick();
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00069C6B File Offset: 0x00067E6B
		public override string Description
		{
			get
			{
				return string.Format("Player gains substantial movement speed.", new object[0]);
			}
		}
	}
}
