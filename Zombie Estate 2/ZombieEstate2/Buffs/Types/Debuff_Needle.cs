using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001F5 RID: 501
	internal class Debuff_Needle : Buff
	{
		// Token: 0x06000D60 RID: 3424 RVA: 0x0006A8E8 File Offset: 0x00068AE8
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = this.total;
			this.Src = Global.GetTexRectange(13, 33);
			if (parent is Player)
			{
				this.Positive = true;
			}
			else
			{
				this.Positive = false;
			}
			this.Name = "Needle";
			if (this.mLevelTwo)
			{
				this.Effect = new AnimatedGameObject(Debuff_Needle.LevelOne, 0.3f, this.Parent.Position, this.Parent);
				this.Src = Global.GetTexRectange(6, 5);
			}
			else
			{
				this.Effect = new AnimatedGameObject(Debuff_Needle.LevelOne, 0.3f, this.Parent.Position, this.Parent);
				this.Src = Global.GetTexRectange(7, 5);
			}
			this.Effect.Position.Y = 0f;
			this.Effect.YRotation = VerchickMath.AngleFromVectors(attacker.TwoDPosition(), parent.TwoDPosition());
			this.Effect.ZRotation = 3.1415927f;
			Global.MasterCache.CreateObject(this.Effect);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0006AA00 File Offset: 0x00068C00
		public override void Tick()
		{
			if (this.Positive)
			{
				this.Parent.Heal(this.Damage / this.total, this.Attacker, false);
			}
			else
			{
				this.Parent.Damage(this.Attacker, this.Damage / this.total, DamageType.Physical, false, false, null);
			}
			base.Tick();
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0006AA5E File Offset: 0x00068C5E
		public override void Update(float elapsed)
		{
			base.FrontOfParent(out this.Effect.Position);
			this.Effect.Position.Y = 0f;
			base.Update(elapsed);
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00005D3F File Offset: 0x00003F3F
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			return true;
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0006AA90 File Offset: 0x00068C90
		public override void Arguments(List<string> args)
		{
			this.Time = float.Parse(args[0], CultureInfo.InvariantCulture);
			this.Damage = float.Parse(args[1], CultureInfo.InvariantCulture);
			if (int.Parse(args[2], CultureInfo.InvariantCulture) == 2)
			{
				this.mLevelTwo = true;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000D65 RID: 3429 RVA: 0x0006AAE6 File Offset: 0x00068CE6
		public override string Description
		{
			get
			{
				return string.Format("Target is slowed for {0} seconds.", this.total);
			}
		}

		// Token: 0x04000DAF RID: 3503
		private float total = 5f;

		// Token: 0x04000DB0 RID: 3504
		private float Damage = 0.5f;

		// Token: 0x04000DB1 RID: 3505
		private static List<Point> LevelOne = new List<Point>
		{
			new Point(6, 5)
		};

		// Token: 0x04000DB2 RID: 3506
		private static List<Point> LevelTwo = new List<Point>
		{
			new Point(7, 5)
		};

		// Token: 0x04000DB3 RID: 3507
		private bool mLevelTwo;
	}
}
