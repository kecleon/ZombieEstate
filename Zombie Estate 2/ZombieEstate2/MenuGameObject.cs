using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000048 RID: 72
	internal class MenuGameObject : GameObject
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x0000C340 File Offset: 0x0000A540
		public MenuGameObject(Point tex, Vector3 pos)
		{
			this.StartTex = tex;
			this.TexScale = 2f;
			this.AffectedByGravity = false;
			this.ActivateObject(pos, tex);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000C36C File Offset: 0x0000A56C
		public override void Update(float elapsed)
		{
			if (this.Highlighted)
			{
				this.TextureCoord = new Point(this.StartTex.X + 2, this.StartTex.Y);
			}
			else
			{
				this.TextureCoord = new Point(this.StartTex.X, this.StartTex.Y);
			}
			base.Update(elapsed);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000C3D0 File Offset: 0x0000A5D0
		public bool MouseOver(Ray ray, out float? distance)
		{
			BoundingSphere boundingSphere = Global.instancedModel.Meshes[0].BoundingSphere;
			boundingSphere.Transform(base.GetTransform());
			boundingSphere.Center = this.Position;
			boundingSphere.Radius = this.scale;
			ray.Intersects(ref boundingSphere, out distance);
			return distance != null;
		}

		// Token: 0x0400011D RID: 285
		public bool Highlighted;

		// Token: 0x0400011E RID: 286
		private Point StartTex;
	}
}
