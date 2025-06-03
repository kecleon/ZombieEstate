using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Enemies.EnemyAbilities
{
	// Token: 0x020001D4 RID: 468
	internal class IcePillar : GameObject
	{
		// Token: 0x06000C80 RID: 3200 RVA: 0x00067804 File Offset: 0x00065A04
		public IcePillar(Vector3 pos, Shootable att)
		{
			this.TexScale = 2f;
			this.ActivateObject(new Vector3(pos.X, pos.Y, pos.Z), new Point(72, 62));
			this.XRotation = 1.5707964f;
			this.scale = 1f;
			this.attacker = att;
			this.AffectedByGravity = false;
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0006786C File Offset: 0x00065A6C
		public override void Update(float elapsed)
		{
			this.scale -= elapsed * 0.68f;
			if (this.scale <= 0.1f)
			{
				this.DestroyObject();
				Explosion.CreateExplosion(0.9f, 13f, 1f, "FreezeEnemy", this.Position, this.attacker, false);
			}
			base.Update(elapsed);
		}

		// Token: 0x04000D4E RID: 3406
		private Shootable attacker;
	}
}
