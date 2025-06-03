using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000117 RID: 279
	public class MasterCache
	{
		// Token: 0x060007A9 RID: 1961 RVA: 0x0003C2D4 File Offset: 0x0003A4D4
		public MasterCache(Game game, int numCaches, int numObjsPerCache, bool initInstanced, string name)
		{
			numObjsPerCache = 1500;
			this.gameObjectCaches = new List<GameObjectCache>();
			this.masterList = new List<GameObject>();
			this.gameObjectCaches.Add(new GameObjectCache(game, numObjsPerCache, initInstanced));
			for (int i = 0; i < numObjsPerCache; i++)
			{
				this.masterList.Add(this.gameObjectCaches[0].gameObjects[i]);
			}
			Terminal.WriteMessage("MasterCache: |" + name + "| initiated successfully. Max Obj Count = " + this.masterList.Count.ToString(), MessageType.IMPORTANTEVENT);
			this.multiUpdater = new MultiThreadUpdater(this.masterList);
			this.particleSystem = new ParticleSystem(this);
			Global.MasterCache = this;
			this.explosionPartSystem = new ExplosionPool(this);
			BulletCreator.Init(this);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0003C3A8 File Offset: 0x0003A5A8
		public void UpdateCaches(float elapsed)
		{
			this.masterList = this.gameObjectCaches[0].gameObjects;
			this.multiUpdater.UpdateGameObjects(ref this.masterList, elapsed);
			Global.ReadyToLoadTransforms = true;
			Global.ReadyToLoadTransformsTwo = true;
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0003C3DF File Offset: 0x0003A5DF
		public void LATE_UPDATE()
		{
			this.gameObjectCaches[0].SET_DATA();
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0003C3F2 File Offset: 0x0003A5F2
		public void DrawObjects()
		{
			this.gameObjectCaches[0].DrawObjects();
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0003C405 File Offset: 0x0003A605
		public bool CreateObject(GameObject obj)
		{
			return this.gameObjectCaches[0].CreateObject(obj);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0003C419 File Offset: 0x0003A619
		public void CreateParticle(ParticleType type, Vector3 pos, Vector3 vel)
		{
			this.particleSystem.AddParticle(type, pos, vel);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0003C429 File Offset: 0x0003A629
		public void ClearObjects()
		{
			SoundEngine.TEMP_NO_SOUND_TIMER = 2f;
			this.gameObjectCaches[0].ClearObjects();
		}

		// Token: 0x04000821 RID: 2081
		public List<GameObjectCache> gameObjectCaches;

		// Token: 0x04000822 RID: 2082
		private MultiThreadUpdater multiUpdater;

		// Token: 0x04000823 RID: 2083
		private List<GameObject> masterList;

		// Token: 0x04000824 RID: 2084
		public GameObjectCache testCache;

		// Token: 0x04000825 RID: 2085
		public ParticleSystem particleSystem;

		// Token: 0x04000826 RID: 2086
		public ExplosionPool explosionPartSystem;
	}
}
