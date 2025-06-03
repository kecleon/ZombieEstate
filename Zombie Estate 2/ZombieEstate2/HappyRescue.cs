using System;

namespace ZombieEstate2
{
	// Token: 0x020000F2 RID: 242
	internal class HappyRescue : GameObject
	{
		// Token: 0x06000662 RID: 1634 RVA: 0x0002F8C8 File Offset: 0x0002DAC8
		public override void Update(float elapsed)
		{
			this.scale = 0.4f;
			if (base.OnFloor())
			{
				base.UnSquishMe(0.1f);
				this.Velocity.Y = 4f;
			}
			this.liveTime -= elapsed;
			if (this.liveTime < 0f)
			{
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0002F92B File Offset: 0x0002DB2B
		public override void Landed()
		{
			base.SquishMe(1.2f);
			base.Landed();
		}

		// Token: 0x04000637 RID: 1591
		private float liveTime = 5f;
	}
}
