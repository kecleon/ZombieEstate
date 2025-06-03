using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200007B RID: 123
	public class ExplosionPool
	{
		// Token: 0x060002FA RID: 762 RVA: 0x000175CC File Offset: 0x000157CC
		public ExplosionPool(MasterCache master)
		{
			List<GameObject> gameObjects = master.gameObjectCaches[0].gameObjects;
			this.startIndex = gameObjects.Count - this.count - 250 - 250;
			this.particles = new Queue<ExplosionParticle>();
			for (int i = this.startIndex; i < this.startIndex + this.count; i++)
			{
				gameObjects[i] = new ExplosionParticle();
				gameObjects[i].DestroyObject();
				this.particles.Enqueue(gameObjects[i] as ExplosionParticle);
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00017680 File Offset: 0x00015880
		public void AddParticle(Vector3 pos, string type, float range, Vector3 orig, float partScale)
		{
			ExplosionParticle explosionParticle = this.particles.Dequeue();
			explosionParticle.Init(ref pos, type, ref orig, partScale);
			explosionParticle.Active = true;
			explosionParticle.secondObject.Active = true;
			this.particles.Enqueue(explosionParticle);
		}

		// Token: 0x040002DF RID: 735
		private Queue<ExplosionParticle> particles;

		// Token: 0x040002E0 RID: 736
		private int startIndex;

		// Token: 0x040002E1 RID: 737
		private int count = 200;

		// Token: 0x040002E2 RID: 738
		private string temp = "Fire";
	}
}
