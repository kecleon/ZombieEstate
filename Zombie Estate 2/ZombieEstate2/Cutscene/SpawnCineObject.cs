using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Cutscene
{
	// Token: 0x020001DB RID: 475
	internal class SpawnCineObject : CutSceneLine
	{
		// Token: 0x06000CA2 RID: 3234 RVA: 0x000680E2 File Offset: 0x000662E2
		public SpawnCineObject(string type, string name, Vector3 pos, Point tex) : this(type, name, pos, tex, Color.Black)
		{
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x000680F4 File Offset: 0x000662F4
		public SpawnCineObject(string type, string name, Vector3 pos, Point tex, Color textColor)
		{
			this.objName = name;
			this.objType = type;
			this.texCoord = tex;
			this.position = pos;
			this.textColor = textColor;
			this.Duration = new Timer(1E-05f);
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00068134 File Offset: 0x00066334
		public override void Run()
		{
			this.Duration.Start();
			if (this.objType.ToUpper() == "CINEOBJECT")
			{
				CineObject obj = new CineObject(this.objName, this.texCoord, this.position, this.textColor);
				CutSceneMaster.CutSceneCache.CreateObject(obj);
				base.Run();
				return;
			}
			Terminal.WriteMessage("Error: Unknown spawn type |" + this.objType + "|", MessageType.ERROR);
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x000681AF File Offset: 0x000663AF
		public override string[] GetArgumentDescriptions()
		{
			return new string[]
			{
				"Object Type",
				"Object ID",
				"Position",
				"TexCoord"
			};
		}

		// Token: 0x04000D67 RID: 3431
		private string objName;

		// Token: 0x04000D68 RID: 3432
		private string objType;

		// Token: 0x04000D69 RID: 3433
		private Point texCoord;

		// Token: 0x04000D6A RID: 3434
		private Vector3 position;

		// Token: 0x04000D6B RID: 3435
		private Color textColor;
	}
}
