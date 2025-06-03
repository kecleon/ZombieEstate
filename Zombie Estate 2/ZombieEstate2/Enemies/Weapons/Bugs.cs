using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Enemies.Weapons
{
	// Token: 0x020001CE RID: 462
	internal class Bugs : EnemyBullet
	{
		// Token: 0x06000C6A RID: 3178 RVA: 0x00066CBC File Offset: 0x00064EBC
		public Bugs(Vector3 pos, Vector3 direction, int targIndex, bool slave) : base(pos, direction, 0.6f, false, new Point(13, 26), new Point(63, 63))
		{
			this.zRot = this.GetRot();
			this.ZRotation = Global.RandomFloat(0f, 6.2831855f);
			this.Targ = targIndex;
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x00066D27 File Offset: 0x00064F27
		private float GetRot()
		{
			return Global.RandomFloat(0.1f, 0.6f);
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00066D38 File Offset: 0x00064F38
		public override void Update(float elapsed)
		{
			this.flap -= elapsed;
			if (this.flap <= 0f)
			{
				if (this.TextureCoord.X == 13)
				{
					this.TextureCoord.X = 14;
				}
				else
				{
					this.TextureCoord.X = 13;
				}
				this.flap = 0.06f;
			}
			this.liveTime -= elapsed;
			if (this.liveTime <= 0f)
			{
				this.DestroyObject();
				return;
			}
			this.XRotation += this.xRot * elapsed;
			this.YRotation += this.yRot * elapsed;
			this.ZRotation += this.zRot * elapsed;
			this.secondObject.XRotation += this.xRot2 * elapsed;
			this.secondObject.YRotation += this.yRot2 * elapsed;
			this.secondObject.ZRotation += this.zRot2 * elapsed;
			if (this.SLAVE)
			{
				this.Position = this.Parent.Position;
				this.secondObject.Position = this.Position;
				return;
			}
			this.Direction = Vector3.Lerp(this.Direction, Vector3.Subtract(Global.PlayerList[this.Targ].Position, this.Position), 0.8f * elapsed);
			base.Update(elapsed);
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00066EAF File Offset: 0x000650AF
		public override void DestroyObject()
		{
			if (this.Child != null)
			{
				this.Child.DestroyObject();
			}
			base.DestroyObject();
		}

		// Token: 0x04000D28 RID: 3368
		private float xRot;

		// Token: 0x04000D29 RID: 3369
		private float yRot;

		// Token: 0x04000D2A RID: 3370
		private float zRot;

		// Token: 0x04000D2B RID: 3371
		private float xRot2;

		// Token: 0x04000D2C RID: 3372
		private float yRot2;

		// Token: 0x04000D2D RID: 3373
		private float zRot2;

		// Token: 0x04000D2E RID: 3374
		private int Targ;

		// Token: 0x04000D2F RID: 3375
		public bool SLAVE;

		// Token: 0x04000D30 RID: 3376
		public Bugs Parent;

		// Token: 0x04000D31 RID: 3377
		public Bugs Child;

		// Token: 0x04000D32 RID: 3378
		private float liveTime = 8f;

		// Token: 0x04000D33 RID: 3379
		private float flap = 0.06f;
	}
}
