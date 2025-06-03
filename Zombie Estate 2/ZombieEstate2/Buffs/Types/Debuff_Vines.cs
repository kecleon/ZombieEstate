using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001FE RID: 510
	internal class Debuff_Vines : Buff
	{
		// Token: 0x06000D9C RID: 3484 RVA: 0x0006B4EF File Offset: 0x000696EF
		public Debuff_Vines()
		{
			this.CanNotBeOverwritten = true;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0006B50C File Offset: 0x0006970C
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = this.total;
			this.Src = Global.GetTexRectange(0, 26);
			this.Positive = false;
			this.Name = "Grabbing Vines";
			this.Effect = new AnimatedGameObject(Debuff_Vines.StunAnim, 0.3f, this.Parent.Position, this.Parent);
			Global.MasterCache.CreateObject(this.Effect);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0006B584 File Offset: 0x00069784
		public override void Update(float elapsed)
		{
			this.Parent.AffectedByGravity = false;
			base.FrontOfParent(out this.Effect.Position);
			this.Effect.Position.Y = this.Parent.floorHeight;
			this.Parent.Velocity = Vector3.Zero;
			base.Update(elapsed);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0006B468 File Offset: 0x00069668
		public override float ApplySpeedMod(float start)
		{
			return 0f;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0006B5E0 File Offset: 0x000697E0
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.Speed = -this.Parent.BaseSpecialProperties.Speed;
			return true;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0006B5FC File Offset: 0x000697FC
		public override void Arguments(List<string> args)
		{
			this.ToPos = default(Vector3);
			this.ToPos.X = float.Parse(args[0], CultureInfo.InvariantCulture);
			this.ToPos.Y = float.Parse(args[1], CultureInfo.InvariantCulture);
			this.ToPos.Z = float.Parse(args[2], CultureInfo.InvariantCulture);
			if (this.ToPos == Vector3.Zero)
			{
				this.mNoPart = true;
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0006B682 File Offset: 0x00069882
		public override void Applied()
		{
			this.Parent.Position.Y = this.Parent.floorHeight;
			base.Applied();
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0006B6A8 File Offset: 0x000698A8
		public override void Removed()
		{
			this.Parent.AffectedByGravity = true;
			if (!this.mNoPart)
			{
				for (int i = 0; i < 8; i++)
				{
					Global.MasterCache.CreateParticle(ParticleType.Dirt, this.ToPos, this.Parent.Velocity);
				}
			}
			base.Removed();
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0006B6F8 File Offset: 0x000698F8
		public override void Destroyed()
		{
			this.Parent.AffectedByGravity = true;
			base.Destroyed();
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x0006B70C File Offset: 0x0006990C
		public override string Description
		{
			get
			{
				return string.Format("Target is stunned for pulled underground and teleported to a new location.", new object[0]);
			}
		}

		// Token: 0x04000DC8 RID: 3528
		private float total = 1.2f;

		// Token: 0x04000DC9 RID: 3529
		private Vector3 ToPos;

		// Token: 0x04000DCA RID: 3530
		private static List<Point> StunAnim = new List<Point>
		{
			new Point(72, 13),
			new Point(73, 13),
			new Point(74, 13),
			new Point(75, 13)
		};

		// Token: 0x04000DCB RID: 3531
		private bool mNoPart;
	}
}
