using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Enemies.EnemyAbilities
{
	// Token: 0x020001D5 RID: 469
	internal class FirePillar : GameObject
	{
		// Token: 0x06000C82 RID: 3202 RVA: 0x000678D0 File Offset: 0x00065AD0
		public FirePillar(Vector3 pos, Shootable att)
		{
			this.TexScale = 2f;
			this.ActivateObject(new Vector3(pos.X, pos.Y, pos.Z), new Point(72, 60));
			this.XRotation = 1.5707964f;
			this.scale = 2f;
			this.attacker = att;
			this.AffectedByGravity = false;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00067938 File Offset: 0x00065B38
		public override void Update(float elapsed)
		{
			this.scale -= elapsed * 1.5f;
			if (this.scale <= 0.1f)
			{
				this.DestroyObject();
				foreach (Player player in Global.PlayerList)
				{
					if (VerchickMath.WithinDistance(player.TwoDPosition(), this.twoDPosition, 1.5f))
					{
						player.Damage(this.attacker, 15f, DamageType.Fire, false, false, null);
						player.BuffManager.AddBuff("Debuff_Fire", this.attacker, "3.5, 4");
					}
					for (int i = 0; i < player.GetMinionList.Count; i++)
					{
						Minion minion = player.GetMinionList[i];
						if (VerchickMath.WithinDistance(minion.TwoDPosition(), this.twoDPosition, 1.5f))
						{
							minion.Damage(this.attacker, 15f, DamageType.Fire, false, false, null);
							minion.BuffManager.AddBuff("Debuff_Fire", this.attacker, "3.5, 4");
						}
					}
				}
				for (int j = 0; j < 20; j++)
				{
					Vector3 randomPosition = VerchickMath.GetRandomPosition(this.Position, 1.5f);
					randomPosition.Y = 0.5f;
					Global.MasterCache.CreateParticle(ParticleType.Fire, randomPosition, new Vector3(Global.RandomFloat(-0.5f, 0.5f), Global.RandomFloat(0f, 1f), Global.RandomFloat(-0.5f, 0.5f)));
				}
			}
			base.Update(elapsed);
		}

		// Token: 0x04000D4F RID: 3407
		private Shootable attacker;
	}
}
