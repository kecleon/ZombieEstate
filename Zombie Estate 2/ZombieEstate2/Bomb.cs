using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000101 RID: 257
	internal class Bomb : GameObject
	{
		// Token: 0x060006D1 RID: 1745 RVA: 0x00033A14 File Offset: 0x00031C14
		public Bomb(Vector3 pos, Point tex, float fuse, int damage, float range, string type, float push, GameObject parent)
		{
			this.TextureCoord = tex;
			this.orig = tex;
			this.Position = pos;
			this.ActivateObject(pos, tex);
			this.Fuse = fuse;
			this.startFuse = fuse;
			this.Push = push;
			this.Range = range;
			this.Damage = damage;
			this.Type = type;
			this.Parent = parent;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00033A7C File Offset: 0x00031C7C
		public override void Update(float elapsed)
		{
			this.Fuse -= elapsed;
			this.maxFlash = (int)(60f * (this.Fuse / this.startFuse));
			this.flash++;
			if (this.flash > this.maxFlash)
			{
				this.flash = 0;
				if (this.offSet == 0)
				{
					this.offSet = 1;
					this.flash = this.maxFlash / 2;
				}
				else
				{
					this.offSet = 0;
				}
				this.TextureCoord.X = this.orig.X + this.offSet;
			}
			if (this.Fuse < 0f)
			{
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00033B31 File Offset: 0x00031D31
		public override void DestroyObject()
		{
			Explosion.CreateExplosion(this.Range, (float)this.Damage, this.Push, this.Type, this.Position, this.Parent, false);
			base.DestroyObject();
		}

		// Token: 0x040006BD RID: 1725
		private float Fuse;

		// Token: 0x040006BE RID: 1726
		private float Range;

		// Token: 0x040006BF RID: 1727
		private string Type;

		// Token: 0x040006C0 RID: 1728
		private int Damage;

		// Token: 0x040006C1 RID: 1729
		private float Push;

		// Token: 0x040006C2 RID: 1730
		private GameObject Parent;

		// Token: 0x040006C3 RID: 1731
		private int offSet;

		// Token: 0x040006C4 RID: 1732
		private int flash;

		// Token: 0x040006C5 RID: 1733
		private int maxFlash;

		// Token: 0x040006C6 RID: 1734
		private Point orig;

		// Token: 0x040006C7 RID: 1735
		private float startFuse;
	}
}
