using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000109 RID: 265
	internal class Chicken : Zombie
	{
		// Token: 0x06000729 RID: 1833 RVA: 0x00036C31 File Offset: 0x00034E31
		public Chicken(Vector3 pos, GameObject parent) : base(ZombieType.NOTHING)
		{
			this.Position = pos;
			this.parent = parent;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00036C60 File Offset: 0x00034E60
		public override void Setup()
		{
			this.TextureCoord = new Point(44, 0);
			this.startingTex = new Point(44, 0);
			this.spawning = false;
			this.AffectedByGravity = true;
			this.ActivateObject(this.Position, this.startingTex);
			base.InitSpeed(3f);
			this.bombTime = Global.RandomFloat(1f, 8f);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00036CCC File Offset: 0x00034ECC
		private void LayEgg()
		{
			this.bombTime = Global.RandomFloat(5f, 8f);
			Bomb obj = new Bomb(this.Position, new Point(10, 33), 4f, 800, 2f, "Cotton", 2f, this.parent);
			Global.MasterCache.CreateObject(obj);
			this.Velocity.Y = 6f;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Attack()
		{
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00036D40 File Offset: 0x00034F40
		public override void Update(float elapsed)
		{
			this.bombTime -= elapsed;
			this.LiveTime -= elapsed;
			if (this.bombTime < 0f)
			{
				this.LayEgg();
			}
			if (this.LiveTime < 0f)
			{
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00036D98 File Offset: 0x00034F98
		public override void UpdateDirection()
		{
			if (this.targTile == null)
			{
				int x = Global.rand.Next(0, 31);
				int y = Global.rand.Next(0, 31);
				this.targTile = Global.Level.GetTile(x, y);
			}
			if (this.tile == this.targTile)
			{
				int x2 = Global.rand.Next(0, 31);
				int y2 = Global.rand.Next(0, 31);
				this.targTile = Global.Level.GetTile(x2, y2);
			}
			base.UpdateDirection();
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void PickTarget()
		{
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Drops()
		{
		}

		// Token: 0x04000726 RID: 1830
		private float LiveTime = 20f;

		// Token: 0x04000727 RID: 1831
		private float bombTime = 1f;

		// Token: 0x04000728 RID: 1832
		private GameObject parent;
	}
}
