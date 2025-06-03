using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Cutscene
{
	// Token: 0x020001D9 RID: 473
	internal class MoveCamera : CutSceneLine
	{
		// Token: 0x06000C9B RID: 3227 RVA: 0x00067F94 File Offset: 0x00066194
		public MoveCamera(Vector3 pos, string look, float seconds)
		{
			this.lookName = look;
			this.movePos = pos;
			this.seconds = seconds;
			this.Duration = null;
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00067FB8 File Offset: 0x000661B8
		public override void Run()
		{
			this.lookAt = CutSceneMaster.GetCineObjectByID(this.lookName).Position;
			Camera.MoveTo(this.movePos, this.lookAt, this.seconds);
			base.Run();
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00067FED File Offset: 0x000661ED
		public override void Update(float elapsed)
		{
			if (!Camera.CineMoving())
			{
				base.TerminateLine();
			}
			base.Update(elapsed);
		}

		// Token: 0x04000D5E RID: 3422
		private Vector3 movePos;

		// Token: 0x04000D5F RID: 3423
		private Vector3 lookAt;

		// Token: 0x04000D60 RID: 3424
		private float seconds;

		// Token: 0x04000D61 RID: 3425
		private string lookName;
	}
}
