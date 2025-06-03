using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000072 RID: 114
	internal class PathArrow : DualGameObject
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x000167D5 File Offset: 0x000149D5
		public PathArrow(Vector3 pos) : base(new Point(63, 63), new Point(4, 7), pos, false)
		{
			this.myTile = Global.Level.GetTileAtLocation(pos);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00016800 File Offset: 0x00014A00
		public override void Update(float elapsed)
		{
			if (this.myTile.Directions[PathViewer.targetX, PathViewer.targetY] == -1)
			{
				this.YRotation = 0f;
			}
			else
			{
				this.YRotation = 0.785398f * (float)this.myTile.Directions[PathViewer.targetX, PathViewer.targetY] + 3.1415927f;
			}
			this.secondObject.YRotation = this.YRotation;
			base.Update(elapsed);
		}

		// Token: 0x040002AC RID: 684
		private Tile myTile;
	}
}
