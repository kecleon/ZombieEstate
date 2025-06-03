using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200008F RID: 143
	public class PlantTurret : Minion
	{
		// Token: 0x060003BA RID: 954 RVA: 0x0001B37D File Offset: 0x0001957D
		public PlantTurret(Player owner, Vector3 pos, int level) : base(owner, pos)
		{
			this.level = level;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0001B390 File Offset: 0x00019590
		public override void Init()
		{
			this.startGun = "Plant Infinite";
			if (this.level == 0)
			{
				this.TotalShotCount = 100;
			}
			else if (this.level == 1)
			{
				this.TotalShotCount = 150;
			}
			else
			{
				this.TotalShotCount = 200;
			}
			this.StartTextureCoord = new Point(20, 45);
			this.TextureCoord = this.StartTextureCoord;
			this.Range = 3.5f;
			this.MuzzleFlash = true;
			base.Init();
			this.mGun.FireEndOfBarrel = false;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001A081 File Offset: 0x00018281
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

		// Token: 0x060003BD RID: 957 RVA: 0x0001A0BD File Offset: 0x000182BD
		public override void DestroyObject()
		{
			Explosion.CreateExplosion(1f, 20f, 3f, "Fire", this.Position, this.parent, false);
			base.DestroyObject();
		}

		// Token: 0x04000383 RID: 899
		private int level;
	}
}
