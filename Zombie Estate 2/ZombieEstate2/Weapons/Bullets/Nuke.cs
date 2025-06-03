using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Weapons.Bullets
{
	// Token: 0x0200014F RID: 335
	internal class Nuke : GameObject
	{
		// Token: 0x06000A2F RID: 2607 RVA: 0x00053418 File Offset: 0x00051618
		public Nuke(Vector3 startPos, Vector3 endPos, GameObject parent)
		{
			this.mShadow = new GameObject();
			this.mShadow.TextureCoord = new Point(4, 38);
			this.mShadow.scale = 0f;
			this.mShadow.Position = new Vector3(endPos.X, 0.2f, endPos.Z);
			this.mShadow.XRotation = 1.5707964f;
			this.mShadow.AffectedByGravity = false;
			this.mShadow.ActivateObject(this.mShadow.Position, this.mShadow.TextureCoord);
			Global.MasterCache.CreateObject(this.mShadow);
			this.mParent = parent;
			this.TextureCoord = new Point(0, 46);
			this.AffectedByGravity = true;
			this.ZRotation = 3.1415927f;
			this.Position = startPos;
			this.speed = Vector3.Distance(startPos, endPos);
			this.speed *= 0.25f;
			this.direction = endPos - this.Position;
			this.direction.Normalize();
			Camera.ZoomOut(8f, 1.75f);
			Camera.ShakeCamera(5f, 0.025f);
			this.ActivateObject(this.Position, this.TextureCoord);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00053570 File Offset: 0x00051770
		public override void Update(float elapsed)
		{
			this.Velocity = Vector3.Zero;
			if (!this.landed)
			{
				this.mShadow.scale += elapsed * 0.1f;
				this.Position += this.direction * elapsed * this.speed;
				Global.MasterCache.CreateParticle(ParticleType.BigSmoke, this.Position + new Vector3(0f, 1f, 0f), new Vector3(Global.RandomFloat(-0.25f, 0.25f), 1f, Global.RandomFloat(-0.25f, 0.25f)));
			}
			else
			{
				this.mShadow.DestroyObject();
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

		// Token: 0x06000A31 RID: 2609 RVA: 0x00053678 File Offset: 0x00051878
		public override void Landed()
		{
			if (!this.landed)
			{
				this.AffectedByGravity = false;
				this.landed = true;
				Camera.ShakeCamera(1f, 0.15f);
				Explosion.CreateExplosion(8f, 3000f, 10f, "Fire", this.Position, this.mParent, false);
				for (int i = 0; i < 10; i++)
				{
					new Timer(0.1f * (float)i + 0.2f)
					{
						DeltaDelegate = new Timer.TimerDelegate(this.Delta)
					}.Start();
				}
			}
			base.Landed();
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00053710 File Offset: 0x00051910
		private void Delta(float d)
		{
			if (d >= 1f)
			{
				Vector3 randomPosition = VerchickMath.GetRandomPosition(this.mExplosionPosition, 10f);
				randomPosition.Y = Global.RandomFloat(0f, 1f);
				Explosion.CreateExplosion(Global.RandomFloat(2f, 3f), 0f, 0f, "Fire", randomPosition, this.mParent, true);
			}
		}

		// Token: 0x04000AB8 RID: 2744
		private Vector3 direction;

		// Token: 0x04000AB9 RID: 2745
		private float speed = 10f;

		// Token: 0x04000ABA RID: 2746
		private bool landed;

		// Token: 0x04000ABB RID: 2747
		private float time;

		// Token: 0x04000ABC RID: 2748
		private GameObject mParent;

		// Token: 0x04000ABD RID: 2749
		private Vector3 mExplosionPosition;

		// Token: 0x04000ABE RID: 2750
		private GameObject mShadow;
	}
}
