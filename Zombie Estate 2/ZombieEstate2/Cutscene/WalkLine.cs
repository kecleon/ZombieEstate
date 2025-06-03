using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Cutscene
{
	// Token: 0x020001DC RID: 476
	internal class WalkLine : CutSceneLine
	{
		// Token: 0x06000CA6 RID: 3238 RVA: 0x000681D8 File Offset: 0x000663D8
		public WalkLine(string walker, Vector2 direction, float time)
		{
			this.seconds = time;
			this.walkDir = direction;
			this.WalkID = walker;
			this.Duration = new Timer(this.seconds);
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x00068227 File Offset: 0x00066427
		public override void Run()
		{
			this.Walker = CutSceneMaster.GetCineObjectByID(this.WalkID);
			this.Duration.Start();
			this.Walker.Walk(this.Duration, this.walkDir);
			base.Run();
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00068262 File Offset: 0x00066462
		public override object[] GetArguments()
		{
			return new object[]
			{
				this.WalkID,
				this.seconds,
				this.walkDir
			};
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0006828F File Offset: 0x0006648F
		public override string[] GetArgumentDescriptions()
		{
			return new string[]
			{
				"Walker ID",
				"Walk Duration",
				"Walk Direction"
			};
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x000682AF File Offset: 0x000664AF
		public override void SetArguments(object[] args)
		{
			this.WalkID = (string)args[0];
			this.seconds = (float)args[1];
			this.walkDir = (Vector2)args[2];
		}

		// Token: 0x04000D6C RID: 3436
		private string WalkID = "NULL";

		// Token: 0x04000D6D RID: 3437
		private float seconds;

		// Token: 0x04000D6E RID: 3438
		private Vector2 walkDir = Vector2.Zero;

		// Token: 0x04000D6F RID: 3439
		private CineObject Walker;
	}
}
