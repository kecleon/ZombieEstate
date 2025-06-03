using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001F6 RID: 502
	internal class Debuff_Ooze : Buff
	{
		// Token: 0x06000D68 RID: 3432 RVA: 0x0006AB4B File Offset: 0x00068D4B
		public Debuff_Ooze(int amount, float time)
		{
			this.mAmount = amount;
			this.Time = time;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0006AB6C File Offset: 0x00068D6C
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Src = Global.GetTexRectange(11, 23);
			this.Positive = false;
			this.Name = "Ooze";
			this.Effect = new AnimatedGameObject(Debuff_Ooze.StunAnim, 0.2f, this.Parent.Position, this.Parent);
			this.Effect.Position.Y = 0.5f;
			Global.MasterCache.CreateObject(this.Effect);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0006ABEE File Offset: 0x00068DEE
		public override void Update(float elapsed)
		{
			base.FrontOfParent(out this.Effect.Position);
			this.Effect.Position.Y = 0.5f;
			base.Update(elapsed);
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0006AC1D File Offset: 0x00068E1D
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.Armor = -this.mAmount;
			spec.FireResist = -this.mAmount;
			spec.WaterResist = -this.mAmount;
			spec.EarthResist = -this.mAmount;
			return true;
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Arguments(List<string> args)
		{
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x0006AC58 File Offset: 0x00068E58
		public override string Description
		{
			get
			{
				return string.Format("Target takes more damage.", new object[0]);
			}
		}

		// Token: 0x04000DB4 RID: 3508
		private static List<Point> StunAnim = new List<Point>
		{
			new Point(13, 50),
			new Point(14, 50),
			new Point(15, 50),
			new Point(16, 50)
		};

		// Token: 0x04000DB5 RID: 3509
		private int mAmount = 15;
	}
}
