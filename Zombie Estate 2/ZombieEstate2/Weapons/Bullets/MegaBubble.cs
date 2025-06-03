using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Weapons.Bullets
{
	// Token: 0x0200014E RID: 334
	internal class MegaBubble : GameObject
	{
		// Token: 0x06000A2C RID: 2604 RVA: 0x00053270 File Offset: 0x00051470
		public MegaBubble(Vector3 startPos, Vector3 endPos, GameObject parent)
		{
			this.mParent = parent;
			this.mOrigPosition = new Vector3(startPos.X, startPos.Y, startPos.Z);
			this.TextureCoord = new Point(6, 4);
			this.AffectedByGravity = true;
			this.Position = startPos;
			this.speed = 1f;
			this.direction = endPos - this.Position;
			this.direction.Normalize();
			this.time = startPos.X * startPos.Z;
			this.ActivateObject(this.Position, this.TextureCoord);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0005331C File Offset: 0x0005151C
		public override void Update(float elapsed)
		{
			this.time += elapsed * 2f;
			this.Velocity = Vector3.Zero;
			if (!this.landed)
			{
				this.Position += this.direction * elapsed * this.speed;
				this.Position.X = this.mOrigPosition.X + (float)Math.Sin((double)this.time);
			}
			else
			{
				this.mExplosionPosition = this.Position;
				this.DestroyObject();
				this.Position.Y = 0.1f;
			}
			base.Update(elapsed);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000533C8 File Offset: 0x000515C8
		public override void Landed()
		{
			if (!this.landed)
			{
				this.AffectedByGravity = false;
				this.landed = true;
				Explosion.CreateExplosion(2f, 500f, 2f, "Suds", this.Position, this.mParent, false);
			}
			base.Landed();
		}

		// Token: 0x04000AB1 RID: 2737
		private Vector3 direction;

		// Token: 0x04000AB2 RID: 2738
		private float speed = 10f;

		// Token: 0x04000AB3 RID: 2739
		private bool landed;

		// Token: 0x04000AB4 RID: 2740
		private float time;

		// Token: 0x04000AB5 RID: 2741
		private GameObject mParent;

		// Token: 0x04000AB6 RID: 2742
		private Vector3 mExplosionPosition;

		// Token: 0x04000AB7 RID: 2743
		private Vector3 mOrigPosition;
	}
}
