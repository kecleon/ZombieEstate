using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200008D RID: 141
	internal class MassiveTurret : MachineGunTurret
	{
		// Token: 0x060003A0 RID: 928 RVA: 0x0001A0EC File Offset: 0x000182EC
		public MassiveTurret(Vector3 pos) : base(null, pos, 0)
		{
			this.TexScale = 2f;
			this.speedMod = 0f;
			this.startGun = "Minigun Infinite";
			this.StartTextureCoord = new Point(64, 48);
			this.TextureCoord = this.StartTextureCoord;
			this.scale = 1f;
			this.floorHeight = 1f;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0001A154 File Offset: 0x00018354
		public override void Init()
		{
			this.TotalShotCount = 800;
			base.MinionInit();
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void UpdateFacing()
		{
		}
	}
}
