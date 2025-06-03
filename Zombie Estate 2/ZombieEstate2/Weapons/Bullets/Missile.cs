using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Weapons.Bullets
{
	// Token: 0x0200014C RID: 332
	internal class Missile : GameObject
	{
		// Token: 0x06000A26 RID: 2598 RVA: 0x00052E7C File Offset: 0x0005107C
		public Missile(Vector3 startPos, Vector3 endPos, GameObject parent, float explSize, float dmg, int lvl)
		{
			this.mParent = parent;
			this.mExplosionRange = explSize;
			this.mExplosionDmg = dmg;
			this.TextureCoord = new Point(2, 34);
			this.AffectedByGravity = true;
			startPos.X -= 6f;
			startPos.Z -= 6f;
			this.scale = 0.25f;
			this.ZRotation = 2.0943952f;
			this.Position = startPos;
			this.speed = 12f;
			this.direction = endPos - this.Position;
			this.direction.Normalize();
			this.ActivateObject(this.Position, this.TextureCoord);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00052F40 File Offset: 0x00051140
		public override void Update(float elapsed)
		{
			this.Velocity = Vector3.Zero;
			if (!this.landed)
			{
				this.Position += this.direction * elapsed * this.speed;
				Global.MasterCache.CreateParticle(ParticleType.Smoke, this.Position + new Vector3(0f, 1.25f, 0f), new Vector3(Global.RandomFloat(-0.25f, 0.25f), 1f, Global.RandomFloat(-0.25f, 0.25f)));
			}
			else
			{
				this.time -= elapsed;
				this.mExplosionPosition = this.Position;
				if (this.time < 0f)
				{
					this.DestroyObject();
				}
				this.Position.Y = 0.1f;
			}
			base.Update(elapsed);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00053024 File Offset: 0x00051224
		public override void Landed()
		{
			if (!this.landed)
			{
				this.AffectedByGravity = false;
				this.landed = true;
				Explosion.CreateExplosion(this.mExplosionRange, this.mExplosionDmg, 2f, "Fire", this.Position, this.mParent, false);
			}
			base.Landed();
		}

		// Token: 0x04000AA1 RID: 2721
		private Vector3 direction;

		// Token: 0x04000AA2 RID: 2722
		private float speed = 10f;

		// Token: 0x04000AA3 RID: 2723
		private bool landed;

		// Token: 0x04000AA4 RID: 2724
		private float time;

		// Token: 0x04000AA5 RID: 2725
		private GameObject mParent;

		// Token: 0x04000AA6 RID: 2726
		private Vector3 mExplosionPosition;

		// Token: 0x04000AA7 RID: 2727
		private float mExplosionRange;

		// Token: 0x04000AA8 RID: 2728
		private float mExplosionDmg;
	}
}
