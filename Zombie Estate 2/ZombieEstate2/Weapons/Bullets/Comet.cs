using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Weapons.Bullets
{
	// Token: 0x0200014D RID: 333
	internal class Comet : GameObject
	{
		// Token: 0x06000A29 RID: 2601 RVA: 0x00053078 File Offset: 0x00051278
		public Comet(Vector3 startPos, Vector3 endPos, GameObject parent, float explSize, float dmg, int lvl)
		{
			this.mParent = parent;
			this.mExplosionRange = explSize;
			this.mExplosionDmg = dmg;
			this.TextureCoord = new Point(15, 26);
			if (lvl >= 2)
			{
				this.TextureCoord.X = 16;
			}
			this.AffectedByGravity = true;
			startPos.X -= 6f;
			startPos.Z -= 6f;
			this.Position = startPos;
			this.speed = 12f;
			this.direction = endPos - this.Position;
			this.direction.Normalize();
			this.ActivateObject(this.Position, this.TextureCoord);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00053138 File Offset: 0x00051338
		public override void Update(float elapsed)
		{
			this.Velocity = Vector3.Zero;
			if (!this.landed)
			{
				this.Position += this.direction * elapsed * this.speed;
				Global.MasterCache.CreateParticle(ParticleType.Magic, this.Position + new Vector3(0f, 1.25f, 0f), new Vector3(Global.RandomFloat(-0.25f, 0.25f), 1f, Global.RandomFloat(-0.25f, 0.25f)));
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

		// Token: 0x06000A2B RID: 2603 RVA: 0x0005321C File Offset: 0x0005141C
		public override void Landed()
		{
			if (!this.landed)
			{
				this.AffectedByGravity = false;
				this.landed = true;
				Explosion.CreateExplosion(this.mExplosionRange, this.mExplosionDmg, 1f, "MiniFrost", this.Position, this.mParent, false);
			}
			base.Landed();
		}

		// Token: 0x04000AA9 RID: 2729
		private Vector3 direction;

		// Token: 0x04000AAA RID: 2730
		private float speed = 10f;

		// Token: 0x04000AAB RID: 2731
		private bool landed;

		// Token: 0x04000AAC RID: 2732
		private float time;

		// Token: 0x04000AAD RID: 2733
		private GameObject mParent;

		// Token: 0x04000AAE RID: 2734
		private Vector3 mExplosionPosition;

		// Token: 0x04000AAF RID: 2735
		private float mExplosionRange;

		// Token: 0x04000AB0 RID: 2736
		private float mExplosionDmg;
	}
}
