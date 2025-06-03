using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Enemies.EnemyAbilities
{
	// Token: 0x020001D3 RID: 467
	public class Confetti : GameObject
	{
		// Token: 0x06000C7E RID: 3198 RVA: 0x00067618 File Offset: 0x00065818
		public Confetti(float angle, Vector3 pos, float delay, Shootable attacker)
		{
			this.ActivateObject(pos, new Point(Global.rand.Next(0, 4), 30));
			this.mDir = VerchickMath.AngleToVector(angle);
			this.mDir.Normalize();
			this.mDelay = delay;
			this.mAtt = attacker;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00067678 File Offset: 0x00065878
		public override void Update(float elapsed)
		{
			this.mDelay -= elapsed;
			if (this.mDelay < 0f)
			{
				this.mSpeed += 0.001f;
				if (this.mSpeed > 1f)
				{
					this.mSpeed = 1f;
				}
				this.Position.X = this.Position.X + this.mDir.X * this.mSpeed;
				this.Position.Z = this.Position.Z + this.mDir.Y * this.mSpeed;
				this.mLiveTime -= elapsed;
				if (this.mLiveTime <= 0f)
				{
					this.DestroyObject();
				}
			}
			foreach (Player player in Global.PlayerList)
			{
				if (VerchickMath.WithinDistance(player.TwoDPosition(), base.TwoDPosition(), 0.5f))
				{
					player.Damage(this.mAtt, 5f, DamageType.Physical, false, false, null);
					this.DestroyObject();
				}
				for (int i = 0; i < player.GetMinionList.Count; i++)
				{
					Minion minion = player.GetMinionList[i];
					if (VerchickMath.WithinDistance(minion.TwoDPosition(), base.TwoDPosition(), 0.5f))
					{
						minion.Damage(this.mAtt, 5f, DamageType.Physical, false, false, null);
						this.DestroyObject();
					}
				}
			}
			base.Update(elapsed);
		}

		// Token: 0x04000D49 RID: 3401
		private Vector2 mDir;

		// Token: 0x04000D4A RID: 3402
		private float mDelay;

		// Token: 0x04000D4B RID: 3403
		private float mLiveTime = 2f;

		// Token: 0x04000D4C RID: 3404
		private float mSpeed;

		// Token: 0x04000D4D RID: 3405
		private Shootable mAtt;
	}
}
