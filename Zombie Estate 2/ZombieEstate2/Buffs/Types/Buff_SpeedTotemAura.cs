using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Buffs.Types
{
	// Token: 0x020001F4 RID: 500
	internal class Buff_SpeedTotemAura : PlayerAura
	{
		// Token: 0x06000D5B RID: 3419 RVA: 0x0006A7A4 File Offset: 0x000689A4
		public override void Init(Shootable parent, Shootable attacker)
		{
			base.Init(parent, attacker);
			this.Time = 0f;
			this.Src = Global.GetTexRectange(16, 39);
			this.Positive = true;
			this.Name = "Speed Totem ";
			this.Buff = "Buff_SpeedTotem";
			this.Args = "";
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0006A7FC File Offset: 0x000689FC
		public override void Arguments(List<string> args)
		{
			this.speed = float.Parse(args[0], CultureInfo.InvariantCulture);
			this.Range = float.Parse(args[1], CultureInfo.InvariantCulture);
			this.Args = this.speed.ToString();
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0006A848 File Offset: 0x00068A48
		public override bool UpdateSpec(ref SpecialProperties spec)
		{
			spec.SwingTimeMod += this.speed;
			spec.ShotTimeMod += this.speed;
			return true;
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00069E94 File Offset: 0x00068094
		public override string Description
		{
			get
			{
				return string.Format("", new object[0]);
			}
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0006A874 File Offset: 0x00068A74
		public override void FireAuraEffect()
		{
			RadialParticle radialParticle = new RadialParticle(new Point(78, 62), 0.1f, this.Range, 0.1f, new Vector3(this.Parent.Position.X, 0.025f, this.Parent.Position.Z));
			radialParticle.TexScale = 2f;
			Global.MasterCache.CreateObject(radialParticle);
			base.FireAuraEffect();
		}

		// Token: 0x04000DAE RID: 3502
		private float speed;
	}
}
