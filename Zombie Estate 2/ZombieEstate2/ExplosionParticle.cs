using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200010F RID: 271
	internal class ExplosionParticle : DualGameObject
	{
		// Token: 0x0600075A RID: 1882 RVA: 0x000397DC File Offset: 0x000379DC
		public ExplosionParticle() : base(new Point(0, 0), new Point(0, 0), Vector3.Zero, false)
		{
			this.elapsedTime = 0f;
			this.speed = 0f;
			this.currentX = 0;
			this.Velocity = Vector3.Zero;
			this.Position = Vector3.Zero;
			this.AffectedByGravity = false;
			this.rotates = false;
			this.rotateSpeed = 0f;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00039858 File Offset: 0x00037A58
		public void Init(ref Vector3 position, string type, ref Vector3 origin, float partScale)
		{
			this.elapsedTime = 0f;
			this.speed = 0f;
			this.currentX = 0;
			this.Velocity = Vector3.Zero;
			this.Position = Vector3.Zero;
			this.AffectedByGravity = false;
			this.rotates = false;
			this.rotateSpeed = 0f;
			this.animated = true;
			this.dir = Vector3.Zero;
			this.scale = partScale;
			this.type = type;
			this.speed = Global.RandomFloat(0.05f, 0.1f) + 0.01f;
			this.TextureCoord = new Point(0, 28);
			base.SetHorizantleTextureCoord(this.TextureCoord);
			this.AffectedByGravity = false;
			this.secondObject.AffectedByGravity = false;
			this.YRotation = Global.RandomFloat(0f, 6.2831855f);
			this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
			this.XRotation = Global.RandomFloat(0f, 6.2831855f);
			this.secondObject.XRotation = this.XRotation - 1.5707964f;
			this.ActivateObject(position, this.TextureCoord);
			if (type == "Fire")
			{
				this.dir = this.Position - origin;
				float num = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.Normalize();
				this.dir *= 1f / (0.3f + num) * 0.1f;
				this.rotates = true;
			}
			if (type == "Blood")
			{
				this.dir = this.Position - origin;
				float num2 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.TextureCoord = new Point(6, 56);
				base.SetHorizantleTextureCoord(this.TextureCoord);
				this.dir.Normalize();
				this.dir *= 1f / (0.3f + num2) * 0.1f;
				this.rotates = true;
			}
			if (type == "StinkOoze")
			{
				this.scale = Global.RandomFloat(0.6f, 1.2f);
				this.TextureCoord = new Point(0, 49);
				base.SetHorizantleTextureCoord(this.TextureCoord);
				this.dir = this.Position - origin;
				float num3 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.Normalize();
				this.dir *= -(num3 * 0.05f);
				this.rotates = true;
			}
			if (type == "SlowFire")
			{
				this.dir = this.Position - origin;
				float num4 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.Normalize();
				this.dir *= 1f / (0.3f + num4) * 0.1f;
				this.rotates = true;
			}
			if (type == "Tractor")
			{
				this.dir = this.Position - origin;
				float num5 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.Normalize();
				this.dir *= 1f / (0.3f + num5) * 0.1f;
				this.rotates = true;
			}
			if (type == "BlackHole")
			{
				this.scale = Global.RandomFloat(0.2f, 0.8f);
				this.TextureCoord = new Point(0, 49);
				base.SetHorizantleTextureCoord(this.TextureCoord);
				this.dir = this.Position - origin;
				float num6 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.Normalize();
				this.dir *= -(num6 * 0.05f);
				this.rotates = true;
			}
			if (type == "Egg")
			{
				this.Position = origin;
				this.XRotation = 1.5707964f;
				this.YRotation = 0f;
				this.ZRotation = 0f;
				this.Position.Y = 0.3f;
				this.animated = false;
				if (Global.Probability(50))
				{
					this.TextureCoord = new Point(11, 34);
				}
				else
				{
					this.TextureCoord = new Point(12, 34);
				}
				base.SetHorizantleTextureCoord(new Point(63, 63));
				this.rotates = false;
			}
			if (type == "Suds")
			{
				this.TextureCoord = new Point(0, 50);
				base.SetHorizantleTextureCoord(this.TextureCoord);
				this.dir = this.Position - origin;
				float num7 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.Normalize();
				this.dir *= 1f / (0.3f + num7) * 0.01f;
				this.rotates = true;
			}
			if (type == "Freeze" || type == "FreezeEnemy")
			{
				this.scale = Global.RandomFloat(0.6f, 1.2f);
				this.TextureCoord = new Point(11, 33);
				base.SetHorizantleTextureCoord(this.TextureCoord);
				this.dir = this.Position - origin;
				float num8 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.Normalize();
				this.speed = Global.RandomFloat(0.05f, 0.1f) + 0.01f;
				this.dir *= 1f / (0.3f + num8) * 0.01f;
				this.rotates = false;
			}
			if (type == "Cotton")
			{
				this.scale = Global.RandomFloat(0.6f, 0.8f);
				this.TextureCoord = new Point(0, 51);
				base.SetHorizantleTextureCoord(this.TextureCoord);
				this.dir = this.Position - origin;
				float num9 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.X = this.dir.X * 0.2f;
				this.dir.Z = this.dir.Z * 0.2f;
				this.dir.Normalize();
				this.dir *= 1f / (0.3f + num9) * 0.05f;
			}
			if (type == "Potato")
			{
				this.TextureCoord = new Point(11, 32);
				base.SetHorizantleTextureCoord(this.TextureCoord);
				this.dir = this.Position - origin;
				float num10 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.Normalize();
				this.dir *= 1f / (0.3f + num10) * 0.01f;
				this.rotates = true;
			}
			if (type == "Goo")
			{
				this.scale = Global.RandomFloat(0.5f, 0.7f);
				this.TextureCoord = new Point(6, 48);
				base.SetHorizantleTextureCoord(this.TextureCoord);
				this.dir = this.Position - origin;
				float num11 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.X = this.dir.X * 0.2f;
				this.dir.Z = this.dir.Z * 0.2f;
				this.dir.Normalize();
				this.dir *= 1f / (0.3f + num11) * 0.05f;
			}
			if (type == "DemonSpawn")
			{
				this.dir = this.Position - origin;
				float num12 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.Normalize();
				this.dir *= 1f / (0.3f + num12) * 0.1f;
				this.TextureCoord = new Point(6, 28);
				this.speed = Global.RandomFloat(0.15f, 0.3f) + 0.04f;
				this.rotates = true;
			}
			if (type == "DemonFire")
			{
				this.dir = this.Position - origin;
				float num13 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.Normalize();
				this.dir *= 1f / (0.3f + num13) * 0.1f;
				this.TextureCoord = new Point(6, 28);
				this.speed = Global.RandomFloat(0.15f, 0.3f) + 0.04f;
				this.rotates = true;
			}
			if (type == "Popcorn")
			{
				this.scale = Global.RandomFloat(0.3f, 0.4f);
				this.TextureCoord = new Point(6, 49);
				base.SetHorizantleTextureCoord(new Point(6, 49));
				this.speed = Global.RandomFloat(0.15f, 0.3f) + 0.04f;
			}
			if (type == "ButteredPopcorn")
			{
				this.scale = Global.RandomFloat(0.3f, 0.4f);
				this.TextureCoord = new Point(6, 50);
				base.SetHorizantleTextureCoord(new Point(6, 50));
				this.speed = Global.RandomFloat(0.15f, 0.3f) + 0.04f;
			}
			if (type == "MagicExplosion")
			{
				this.scale = Global.RandomFloat(0.6f, 0.8f);
				this.TextureCoord = new Point(7, 53);
				base.SetHorizantleTextureCoord(this.TextureCoord);
				this.dir = this.Position - origin;
				float num14 = Math.Max(0.25f, Vector3.Distance(position, origin));
				this.dir.Normalize();
				this.dir *= 1f / (0.3f + num14) * 0.01f;
				this.rotates = true;
			}
			if (this.rotates)
			{
				this.rotateSpeed = Global.RandomFloat(0f, 5f) - 2.5f;
			}
			if (type == "Egg")
			{
				this.secondObject.ActivateObject(this.Position, this.secondObject.TextureCoord);
				return;
			}
			this.secondObject.ActivateObject(this.Position, this.TextureCoord);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0003A3E4 File Offset: 0x000385E4
		public override void Update(float elapsed)
		{
			this.elapsedTime += elapsed;
			if (this.elapsedTime > this.speed)
			{
				this.elapsedTime = 0f;
				this.currentX++;
				if (this.animated)
				{
					this.TextureCoord.X = this.TextureCoord.X + 1;
					this.secondObject.TextureCoord.X = this.TextureCoord.X;
				}
				if (this.currentX > 5)
				{
					this.DestroyObject();
				}
				if (this.type == "Fire" && this.currentX == 2)
				{
					Global.MasterCache.CreateParticle(ParticleType.BigSmoke, this.Position, this.Velocity);
				}
			}
			if (this.rotates)
			{
				this.ZRotation += this.rotateSpeed * elapsed;
				this.secondObject.ZRotation += this.rotateSpeed * elapsed;
			}
			if (this.Position.Y < 0.25f)
			{
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0003A4F4 File Offset: 0x000386F4
		public override void DestroyObject()
		{
			base.DestroyObject();
		}

		// Token: 0x0400075B RID: 1883
		private float elapsedTime;

		// Token: 0x0400075C RID: 1884
		private float speed;

		// Token: 0x0400075D RID: 1885
		private int currentX;

		// Token: 0x0400075E RID: 1886
		private string type;

		// Token: 0x0400075F RID: 1887
		private Vector3 dir;

		// Token: 0x04000760 RID: 1888
		private bool animated = true;

		// Token: 0x04000761 RID: 1889
		private bool rotates;

		// Token: 0x04000762 RID: 1890
		private float rotateSpeed;
	}
}
