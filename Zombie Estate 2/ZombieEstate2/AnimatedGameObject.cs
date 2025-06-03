using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000045 RID: 69
	internal class AnimatedGameObject : GameObject
	{
		// Token: 0x060001AA RID: 426 RVA: 0x0000BF50 File Offset: 0x0000A150
		public AnimatedGameObject(List<Point> texs, float tickSpeed, Vector3 pos)
		{
			this.Texs = texs;
			this.TickSpeed = tickSpeed;
			this.index = 0;
			this.TextureCoord = this.Texs[0];
			this.ActivateObject(pos, this.TextureCoord);
			this.AffectedByGravity = false;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000BF9E File Offset: 0x0000A19E
		public AnimatedGameObject(List<Point> texs, float tickSpeed, Vector3 pos, GameObject parent) : this(texs, tickSpeed, pos)
		{
			this.Parent = parent;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000BFB4 File Offset: 0x0000A1B4
		public override void Update(float elapsed)
		{
			this.tick += elapsed;
			if (this.tick >= this.TickSpeed)
			{
				this.tick = 0f;
				this.index++;
				if (this.index >= this.Texs.Count)
				{
					this.index = 0;
				}
				this.TextureCoord = this.Texs[this.index];
			}
			if (this.Parent != null && !this.Parent.Active)
			{
				this.DestroyObject();
			}
			if (this.Parent is Player && (this.Parent as Player).DEAD)
			{
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x04000112 RID: 274
		private List<Point> Texs;

		// Token: 0x04000113 RID: 275
		private float TickSpeed;

		// Token: 0x04000114 RID: 276
		private float tick;

		// Token: 0x04000115 RID: 277
		private int index;

		// Token: 0x04000116 RID: 278
		private GameObject Parent;
	}
}
