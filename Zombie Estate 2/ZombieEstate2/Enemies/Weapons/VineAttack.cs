using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Enemies.Weapons
{
	// Token: 0x020001D1 RID: 465
	internal class VineAttack : EnemyBullet
	{
		// Token: 0x06000C78 RID: 3192 RVA: 0x00067398 File Offset: 0x00065598
		public VineAttack(Vector3 pos, Vector2 direction, Zombie parent) : base(pos, new Vector3(direction.X, 0f, direction.Y), 4.4f, false, new Point(63, 63), new Point(63, 63))
		{
			this.zomb = parent;
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x000673EC File Offset: 0x000655EC
		public override void Update(float elapsed)
		{
			this.cd -= elapsed;
			this.liveTime += elapsed;
			if (this.liveTime >= 1f)
			{
				this.DestroyObject();
				return;
			}
			if (this.cd <= 0f)
			{
				this.cd = 0.3f;
				VineEffect obj = new VineEffect(this.Position);
				Global.MasterCache.CreateObject(obj);
			}
			this.tile = Global.Level.GetTileAtLocation(this.Position);
			if (this.tile != null && !this.tile.HasFloor())
			{
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00067494 File Offset: 0x00065694
		public override void HitTarget(Player p)
		{
			int num = Global.rand.Next(0, 31);
			int num2 = Global.rand.Next(0, 31);
			p.AddBuff(VineAttack.buff, null, string.Format("{0},{1},{2}", (float)num + 0.5f, 0.2f, (float)num2 + 0.5f));
			p.Damage(this.zomb, 10f, DamageType.Earth, false, false, null);
			this.DestroyObject();
		}

		// Token: 0x04000D44 RID: 3396
		private float cd = 0.3f;

		// Token: 0x04000D45 RID: 3397
		private Zombie zomb;

		// Token: 0x04000D46 RID: 3398
		private float liveTime;

		// Token: 0x04000D47 RID: 3399
		private static string buff = "Debuff_Vines";
	}
}
