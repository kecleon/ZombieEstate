using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Enemies.Weapons
{
	// Token: 0x020001D0 RID: 464
	internal class Shockwave
	{
		// Token: 0x06000C74 RID: 3188 RVA: 0x000670B8 File Offset: 0x000652B8
		public Shockwave(int dmg, float knockup, ParticleType partType, int density, Vector3 pos, float live, float distance, float width)
		{
			this.Damage = dmg;
			this.KnockUp = knockup;
			this.Width = width;
			this.Position = pos;
			this.Density = density;
			this.LiveTime = live;
			this.MaxDistance = distance;
			this.PartType = partType;
			this.currentDistance = 0f;
			this.currentLiveTime = this.LiveTime;
			this.ALIVE = true;
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00067134 File Offset: 0x00065334
		public void Update(float elapsed)
		{
			if (!this.ALIVE)
			{
				return;
			}
			this.currentDistance += this.MaxDistance / this.LiveTime * elapsed;
			this.currentLiveTime -= elapsed;
			if (this.currentLiveTime <= 0f)
			{
				this.ALIVE = false;
				return;
			}
			this.SpawnParticles(elapsed);
			this.UpdateCollisions();
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00067198 File Offset: 0x00065398
		private void SpawnParticles(float elapsed)
		{
			this.spawnTime -= elapsed;
			if (this.spawnTime < 0f)
			{
				for (int i = 0; i < this.Density; i++)
				{
					float min = this.currentDistance;
					float max = this.currentDistance + this.Width;
					float num = Global.RandomFloat(0f, 6.2831855f);
					float num2 = Global.RandomFloat(min, max);
					int num3 = (int)(Math.Cos((double)num) * (double)num2);
					int num4 = (int)(Math.Sin((double)num) * (double)num2);
					Vector3 vector = new Vector3(this.Position.X + (float)num3, 0.25f, this.Position.Z + (float)num4);
					if (vector.X >= 0f && vector.X <= 31f && vector.Z >= 0f && vector.Z <= 31f)
					{
						Global.MasterCache.CreateParticle(this.PartType, vector, Vector3.Zero);
					}
				}
				this.spawnTime = 0.016666668f;
			}
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x000672A4 File Offset: 0x000654A4
		private void UpdateCollisions()
		{
			foreach (Player player in Global.PlayerList)
			{
				if (player.OnFloor() && !this.HitPlayers.Contains(player))
				{
					float num = this.currentDistance;
					float num2 = this.currentDistance + this.Width;
					float num3 = Vector2.Distance(new Vector2(this.Position.X, this.Position.Z), player.TwoDPosition());
					if (num3 >= num && num3 <= num2)
					{
						this.HitPlayers.Add(player);
						player.Damage(null, (float)this.Damage, DamageType.Physical, false, true, null);
						player.Velocity.Y = 6f;
						player.AddBuff("Debuff_Stunned", null, "1.0");
					}
				}
			}
		}

		// Token: 0x04000D37 RID: 3383
		private float LiveTime;

		// Token: 0x04000D38 RID: 3384
		private float MaxDistance;

		// Token: 0x04000D39 RID: 3385
		private float Width;

		// Token: 0x04000D3A RID: 3386
		private float KnockUp;

		// Token: 0x04000D3B RID: 3387
		private int Density;

		// Token: 0x04000D3C RID: 3388
		private int Damage;

		// Token: 0x04000D3D RID: 3389
		private ParticleType PartType;

		// Token: 0x04000D3E RID: 3390
		private Vector3 Position;

		// Token: 0x04000D3F RID: 3391
		private float currentDistance;

		// Token: 0x04000D40 RID: 3392
		private float currentLiveTime;

		// Token: 0x04000D41 RID: 3393
		public bool ALIVE;

		// Token: 0x04000D42 RID: 3394
		private float spawnTime;

		// Token: 0x04000D43 RID: 3395
		private List<Player> HitPlayers = new List<Player>();
	}
}
