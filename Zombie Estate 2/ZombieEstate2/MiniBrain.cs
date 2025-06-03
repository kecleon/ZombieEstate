using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200003A RID: 58
	internal class MiniBrain : Zombie
	{
		// Token: 0x06000164 RID: 356 RVA: 0x0000A7DC File Offset: 0x000089DC
		public MiniBrain(Vector3 pos) : base(ZombieType.NOTHING)
		{
			this.startingTex = new Point(60, 24);
			this.TextureCoord = new Point(60, 24);
			this.Position = pos;
			this.EngageDistance = float.MaxValue;
			base.InitSpeed(4.1f);
			this.attackDamage = 1;
			this.leapSpeed = 12f;
			this.range = 1.5f;
			this.NODAMAGE = true;
			this.BloodType = ParticleType.Blood;
			this.GibbletChance = 0;
			this.ActivateObject(pos, this.startingTex);
			this.Worth = 0f;
		}
	}
}
