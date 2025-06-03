using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Cutscene
{
	// Token: 0x020001DA RID: 474
	internal class ParticleLine : CutSceneLine
	{
		// Token: 0x06000C9E RID: 3230 RVA: 0x00068004 File Offset: 0x00066204
		public ParticleLine(string type, Vector3 pos, float radius, int countPerTick, float seconds)
		{
			this.Duration = new Timer(seconds);
			this.position = pos;
			this.radius = radius;
			this.partType = (ParticleType)Enum.Parse(typeof(ParticleType), type);
			this.countPerTick = countPerTick;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00068055 File Offset: 0x00066255
		public override void Run()
		{
			this.Duration.Start();
			this.SpawnParticles();
			base.Run();
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0006806E File Offset: 0x0006626E
		public override void Update(float elapsed)
		{
			this.tickCount++;
			if (this.tickCount >= 10)
			{
				this.SpawnParticles();
				this.tickCount = 0;
			}
			base.Update(elapsed);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0006809C File Offset: 0x0006629C
		private void SpawnParticles()
		{
			for (int i = 0; i < this.countPerTick; i++)
			{
				Vector3 randomPosition = VerchickMath.GetRandomPosition(this.position, this.radius);
				CutSceneMaster.CutSceneCache.CreateParticle(this.partType, randomPosition, Vector3.Zero);
			}
		}

		// Token: 0x04000D62 RID: 3426
		private Vector3 position;

		// Token: 0x04000D63 RID: 3427
		private float radius;

		// Token: 0x04000D64 RID: 3428
		private int countPerTick;

		// Token: 0x04000D65 RID: 3429
		private ParticleType partType;

		// Token: 0x04000D66 RID: 3430
		private int tickCount;
	}
}
