using System;
using System.Collections.Generic;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001F8 RID: 504
	internal class PlayerAura : Buff
	{
		// Token: 0x06000D77 RID: 3447 RVA: 0x0006AE18 File Offset: 0x00069018
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 0f;
			this.Src = Global.GetTexRectange(66, 50);
			this.Positive = true;
			this.Name = "-";
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0006AE50 File Offset: 0x00069050
		public override void Update(float elapsed)
		{
			this.EffectTick -= elapsed;
			if (this.EffectTick <= 0f)
			{
				this.FireAuraEffect();
				this.EffectTick = 2f;
			}
			foreach (Player player in Global.PlayerList)
			{
				if (player.GetMinionList != null)
				{
					foreach (Minion minion in player.GetMinionList)
					{
						if (minion != this.Parent && minion != null && VerchickMath.WithinDistance(minion.TwoDPosition(), this.Parent.TwoDPosition(), this.Range))
						{
							minion.AddBuff(this.Buff, this.Parent, this.Args);
						}
					}
				}
				if (player != this.Parent && VerchickMath.WithinDistance(player.TwoDPosition(), this.Parent.TwoDPosition(), this.Range))
				{
					player.AddBuff(this.Buff, this.Parent, this.Args);
				}
			}
			base.Update(elapsed);
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void FireAuraEffect()
		{
		}

		// Token: 0x04000DB9 RID: 3513
		public string Buff = "";

		// Token: 0x04000DBA RID: 3514
		public string Args = "";

		// Token: 0x04000DBB RID: 3515
		public float Range = 5f;

		// Token: 0x04000DBC RID: 3516
		private float EffectTick;
	}
}
