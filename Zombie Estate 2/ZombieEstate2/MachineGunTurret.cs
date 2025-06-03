using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200008C RID: 140
	public class MachineGunTurret : Minion
	{
		// Token: 0x0600039C RID: 924 RVA: 0x00019FED File Offset: 0x000181ED
		public MachineGunTurret(Player owner, Vector3 pos, int level) : base(owner, pos)
		{
			this.level = level;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001A000 File Offset: 0x00018200
		public override void Init()
		{
			this.startGun = "Minigun Infinite";
			if (this.level == 0)
			{
				this.TotalShotCount = 240;
			}
			else if (this.level == 1)
			{
				this.TotalShotCount = 300;
			}
			else
			{
				this.TotalShotCount = 360;
			}
			this.StartTextureCoord = new Point(20, 37);
			this.TextureCoord = this.StartTextureCoord;
			this.Range = 6f;
			this.MuzzleFlash = true;
			base.Init();
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001A081 File Offset: 0x00018281
		public override void Update(float elapsed)
		{
			base.Update(elapsed);
			this.BounceEnabled = false;
			if (this.mGunObject != null)
			{
				this.mGunObject.TextureCoord.X = 63;
				this.mGunObject.TextureCoord.Y = 63;
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0001A0BD File Offset: 0x000182BD
		public override void DestroyObject()
		{
			Explosion.CreateExplosion(1f, 20f, 3f, "Fire", this.Position, this.parent, false);
			base.DestroyObject();
		}

		// Token: 0x0400035F RID: 863
		private int level;
	}
}
