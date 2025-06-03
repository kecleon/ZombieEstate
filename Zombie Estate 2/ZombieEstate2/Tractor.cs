using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200010B RID: 267
	internal class Tractor : Zombie
	{
		// Token: 0x06000733 RID: 1843 RVA: 0x00036EA4 File Offset: 0x000350A4
		public Tractor(Vector3 pos) : base(ZombieType.NOTHING)
		{
			base.InitSpeed(4.5f);
			this.BounceEnabled = false;
			this.TextureCoord = new Point(64, 11);
			this.startingTex = new Point(64, 11);
			this.ActivateObject(pos, this.startingTex);
			this.Mass = 5f;
			this.BloodType = ParticleType.Sludge;
			this.timeAlive = Global.RandomFloat(5f, 10f);
			this.GibbletChance = 0;
			this.NODAMAGE = true;
			this.Worth = 0f;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00036F44 File Offset: 0x00035144
		public override void Update(float elapsed)
		{
			this.timeAlive -= elapsed;
			if (Global.Probability(5))
			{
				Global.MasterCache.CreateParticle(ParticleType.Smoke, this.Position, Vector3.Zero);
			}
			if (this.timeAlive < 0f)
			{
				this.DestroyObject();
			}
			using (List<Player>.Enumerator enumerator = Global.PlayerList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (VerchickMath.WithinDistance(enumerator.Current.TwoDPosition(), base.TwoDPosition(), 1f))
					{
						this.DestroyObject();
					}
				}
			}
			base.Update(elapsed);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00036FF4 File Offset: 0x000351F4
		public override void DestroyObject()
		{
			Explosion.CreateExplosion(1.5f, 40f, 1f, "Tractor", this.Position, this, false);
			base.DestroyObject();
		}

		// Token: 0x0400072B RID: 1835
		private float timeAlive = 10f;
	}
}
