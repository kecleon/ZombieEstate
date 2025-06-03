using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200010D RID: 269
	internal class Grenade : GameObject
	{
		// Token: 0x0600073B RID: 1851 RVA: 0x00037998 File Offset: 0x00035B98
		public Grenade(Vector2 dir, Vector3 pos)
		{
			this.ActivateObject(pos, new Point(2, 4));
			this.direction = dir;
			this.timer = new Timer(2f);
			this.timer.Start();
			this.Velocity.X = dir.X * this.tossSpeed;
			this.Velocity.Z = dir.Y * this.tossSpeed;
			this.Velocity.Y = 6f;
			this.rotSpeed = Global.RandomFloat(0f, 8f) - 4f;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00037A44 File Offset: 0x00035C44
		public override void Update(float elapsed)
		{
			if (!base.OnFloor())
			{
				this.ZRotation += this.rotSpeed * elapsed;
			}
			if ((double)this.ZRotation >= 6.283185307179586)
			{
				this.ZRotation = 0f;
			}
			if (this.timer.Expired())
			{
				List<GameObject> gameObjects = Global.MasterCache.gameObjectCaches[0].gameObjects;
				for (int i = 0; i < gameObjects.Count; i++)
				{
					if (gameObjects[i].Active && gameObjects[i].GetType() == typeof(Zombie) && VerchickMath.WithinDistance(base.TwoDPosition(), gameObjects[i].TwoDPosition(), 4f))
					{
						Zombie zombie = (Zombie)gameObjects[i];
						float num = 1f - Vector2.Distance(base.TwoDPosition(), zombie.TwoDPosition()) / 4f;
						Vector2 vector = VerchickMath.DirectionToVector2(base.TwoDPosition(), zombie.TwoDPosition());
						num = Math.Min(0.12f, num);
						Zombie zombie2 = zombie;
						zombie2.Velocity.X = zombie2.Velocity.X + vector.X * 30f * num;
						Zombie zombie3 = zombie;
						zombie3.Velocity.Z = zombie3.Velocity.Z + vector.Y * 30f * num;
						Zombie zombie4 = zombie;
						zombie4.Velocity.Y = zombie4.Velocity.Y + num * 50f;
					}
				}
				for (int j = 0; j < 10; j++)
				{
					new Vector3(this.Position.X + Global.RandomFloat(0f, 4f) - 2f, this.Position.Y + Global.RandomFloat(0f, 2f), this.Position.Z + Global.RandomFloat(0f, 4f) - 2f);
				}
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x04000730 RID: 1840
		private Vector2 direction;

		// Token: 0x04000731 RID: 1841
		private Timer timer;

		// Token: 0x04000732 RID: 1842
		private float tossSpeed = 7.5f;

		// Token: 0x04000733 RID: 1843
		private float rotSpeed;
	}
}
