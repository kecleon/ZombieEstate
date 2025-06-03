using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Weapons.Bullets
{
	// Token: 0x02000150 RID: 336
	internal class Orbital : GameObject
	{
		// Token: 0x06000A33 RID: 2611 RVA: 0x00053778 File Offset: 0x00051978
		public Orbital(Vector3 startPos, Vector2 dir, Shootable parent)
		{
			this.mDir = new Vector3(dir.X, 0f, dir.Y);
			this.mDir.Normalize();
			this.yScale = 1000f;
			this.mParent = parent;
			this.TextureCoord = new Point(0, 56);
			this.AffectedByGravity = false;
			this.Position = startPos;
			Camera.ZoomOut(this.mDuration, 1.65f);
			this.ActivateObject(this.Position, this.TextureCoord);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x00053818 File Offset: 0x00051A18
		public override void Update(float elapsed)
		{
			base.Update(elapsed);
			this.mDuration -= elapsed;
			this.mPulse -= elapsed;
			if (this.mPulse < 0f)
			{
				this.mPulse = 0.4f;
				Explosion.CreateExplosion(2f, 300f, 3f, "Fire", this.Position, this.mParent, false);
			}
			if (this.mDuration <= 0f)
			{
				this.DestroyObject();
				return;
			}
			this.yScale = 1000f;
			this.Position += this.mDir * elapsed * this.mSpeed;
			if (this.Position.Z <= 0f || this.Position.Z >= 32f)
			{
				this.mDir = Vector3.Reflect(this.mDir, new Vector3(0f, 0f, 1f));
			}
			if (this.Position.X <= 0f || this.Position.X >= 32f)
			{
				this.mDir = Vector3.Reflect(this.mDir, new Vector3(1f, 0f, 0f));
			}
		}

		// Token: 0x04000ABF RID: 2751
		private float mSpeed = 3f;

		// Token: 0x04000AC0 RID: 2752
		private Vector3 mDir;

		// Token: 0x04000AC1 RID: 2753
		private float mDuration = 20f;

		// Token: 0x04000AC2 RID: 2754
		private float mPulse;

		// Token: 0x04000AC3 RID: 2755
		private Shootable mParent;
	}
}
