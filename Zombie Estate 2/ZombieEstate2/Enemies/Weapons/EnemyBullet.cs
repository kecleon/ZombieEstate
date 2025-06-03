using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Enemies.Weapons
{
	// Token: 0x020001CF RID: 463
	internal class EnemyBullet : DualGameObject
	{
		// Token: 0x06000C6E RID: 3182 RVA: 0x00066ECC File Offset: 0x000650CC
		public EnemyBullet(Vector3 pos, Vector3 direction, float speed, bool affGrav, Point vertTex, Point horizTex) : base(vertTex, horizTex, pos, false)
		{
			this.Direction = direction;
			this.Position = pos;
			this.Speed = speed;
			this.AffectedByGravity = affGrav;
			Global.MasterCache.CreateObject(this);
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00066F19 File Offset: 0x00065119
		public override void Update(float elapsed)
		{
			this.Position += this.Direction * this.Speed * elapsed;
			this.UpdateCollisions();
			this.UpdateLevelCollisions(elapsed);
			base.Update(elapsed);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00066F58 File Offset: 0x00065158
		private void UpdateCollisions()
		{
			foreach (Player player in Global.PlayerList)
			{
				if (VerchickMath.WithinDistance(player.TwoDPosition(), base.TwoDPosition(), this.Range))
				{
					this.HitTarget(player);
					break;
				}
				for (int i = 0; i < player.GetMinionList.Count; i++)
				{
					if (VerchickMath.WithinDistance(player.GetMinionList[i].TwoDPosition(), base.TwoDPosition(), this.Range))
					{
						this.HitTarget(player);
						return;
					}
				}
			}
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00067008 File Offset: 0x00065208
		private void UpdateLevelCollisions(float elapsed)
		{
			Vector3 position = this.Position;
			Tile tileAtLocation = Global.Level.GetTileAtLocation(position);
			if (tileAtLocation == null)
			{
				this.DestroyObject();
				return;
			}
			if (!tileAtLocation.HasAnyWalls())
			{
				return;
			}
			int num = tileAtLocation.CollidedBullet(position, new Vector2(this.Direction.X, this.Direction.Z), this.Speed, elapsed);
			if (num == -1)
			{
				num = tileAtLocation.ShouldIHaveCollidedBulletPreviously(position, new Vector2(this.Direction.X, this.Direction.Z), this.Speed, elapsed);
				if (num != -1)
				{
					this.Position = this.PrevPosition;
				}
			}
			if (num != -1)
			{
				this.HitWall();
			}
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void HitTarget(Player p)
		{
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x000670AD File Offset: 0x000652AD
		public virtual void HitWall()
		{
			this.DestroyObject();
		}

		// Token: 0x04000D34 RID: 3380
		public Vector3 Direction;

		// Token: 0x04000D35 RID: 3381
		public float Speed;

		// Token: 0x04000D36 RID: 3382
		public float Range = 0.5f;
	}
}
