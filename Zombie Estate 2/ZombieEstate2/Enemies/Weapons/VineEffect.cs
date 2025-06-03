using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Enemies.Weapons
{
	// Token: 0x020001D2 RID: 466
	internal class VineEffect : GameObject
	{
		// Token: 0x06000C7C RID: 3196 RVA: 0x00067520 File Offset: 0x00065720
		public VineEffect(Vector3 pos)
		{
			this.AffectedByGravity = false;
			pos.Y = -0.5f;
			this.scale = 0.5f;
			this.floorHeight = this.scale;
			this.ActivateObject(pos, new Point(76, 13));
			for (int i = 0; i < 3; i++)
			{
				Global.MasterCache.CreateParticle(ParticleType.Dirt, new Vector3(this.Position.X, 0f, this.Position.Z), Vector3.Zero);
			}
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x000675B8 File Offset: 0x000657B8
		public override void Update(float elapsed)
		{
			this.time -= elapsed;
			if (this.Position.Y < this.floorHeight)
			{
				this.Position.Y = this.Position.Y + 2.4f * elapsed;
			}
			if (this.time <= 0f)
			{
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x04000D48 RID: 3400
		private float time = 1f;
	}
}
