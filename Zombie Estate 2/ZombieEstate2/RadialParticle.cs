using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200007E RID: 126
	internal class RadialParticle : GameObject
	{
		// Token: 0x06000304 RID: 772 RVA: 0x00017AE0 File Offset: 0x00015CE0
		public RadialParticle(Point tex, float speed, float maxSize, float startingSize, Vector3 position)
		{
			this.AffectedByGravity = false;
			this.floorHeight = 0f;
			this.XRotation = 1.5707964f;
			this.YRotation = 3.1415927f;
			position.X += RadialParticle.centerOffset;
			position.Z += RadialParticle.centerOffset;
			this.speed = speed;
			this.maxSize = maxSize;
			this.scale = startingSize;
			this.TextureCoord = tex;
			this.Position = position;
			float num = Global.RandomFloat(0.95f, 1.05f);
			this.Position.Y = this.Position.Y * num;
			this.ActivateObject(this.Position, this.TextureCoord);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00017B92 File Offset: 0x00015D92
		public RadialParticle(Point tex, float speed, float maxSize, float startingSize, Vector3 position, GameObject parent) : this(tex, speed, maxSize, startingSize, position)
		{
			this.parent = parent;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00017BAC File Offset: 0x00015DAC
		public override void Update(float elapsed)
		{
			if (this.scale >= this.maxSize)
			{
				this.DestroyObject();
				return;
			}
			if (this.parent != null)
			{
				this.Position.X = this.parent.Position.X + RadialParticle.centerOffset;
				this.Position.Z = this.parent.Position.Z + RadialParticle.centerOffset;
			}
			this.scale += this.speed * elapsed * 60f;
			base.Update(elapsed);
		}

		// Token: 0x040002F7 RID: 759
		private float maxSize;

		// Token: 0x040002F8 RID: 760
		private float speed;

		// Token: 0x040002F9 RID: 761
		private GameObject parent;

		// Token: 0x040002FA RID: 762
		private static float centerOffset = 0.05f;
	}
}
