using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Weapons.Bullets
{
	// Token: 0x02000151 RID: 337
	internal class Meteor : GameObject
	{
		// Token: 0x06000A35 RID: 2613 RVA: 0x00053960 File Offset: 0x00051B60
		public Meteor(Vector3 startPos, Vector3 endPos, GameObject parent, float hangTime)
		{
			this.mHangTime = hangTime;
			this.TextureCoord = new Point(5, 29);
			this.AffectedByGravity = true;
			this.Position = startPos;
			this.speed = Vector3.Distance(startPos, endPos);
			this.speed *= 0.25f;
			this.direction = endPos - this.Position;
			this.direction.Normalize();
			this.mParent = parent;
			this.ActivateObject(this.Position, this.TextureCoord);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00053A04 File Offset: 0x00051C04
		public override void Update(float elapsed)
		{
			this.Velocity = Vector3.Zero;
			this.mHangTime -= elapsed;
			if (this.mHangTime > 0f)
			{
				this.AffectedByGravity = false;
				return;
			}
			this.AffectedByGravity = true;
			if (!this.landed)
			{
				this.Position += this.direction * elapsed * this.speed;
			}
			else
			{
				this.time -= elapsed;
				if (this.time < 0f)
				{
					this.DestroyObject();
				}
				this.Position.Y = 0.1f;
			}
			base.Update(elapsed);
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x00053AB0 File Offset: 0x00051CB0
		public override void Landed()
		{
			if (!this.landed)
			{
				this.AffectedByGravity = false;
				this.landed = true;
				Camera.ShakeCamera(0.2f, 0.1f);
				Explosion.CreateExplosion(2f, 300f, 5f, "DemonFire", this.Position, this.mParent, false);
			}
			base.Landed();
		}

		// Token: 0x04000AC4 RID: 2756
		private Vector3 direction;

		// Token: 0x04000AC5 RID: 2757
		private float speed = 10f;

		// Token: 0x04000AC6 RID: 2758
		private bool landed;

		// Token: 0x04000AC7 RID: 2759
		private static float TOTAL_TIME = 3f;

		// Token: 0x04000AC8 RID: 2760
		private float time = Meteor.TOTAL_TIME;

		// Token: 0x04000AC9 RID: 2761
		private GameObject mParent;

		// Token: 0x04000ACA RID: 2762
		private float mHangTime;
	}
}
