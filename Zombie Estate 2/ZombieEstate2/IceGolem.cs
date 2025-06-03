using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.Enemies.EnemyAbilities;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x02000032 RID: 50
	internal class IceGolem : Zombie
	{
		// Token: 0x0600011C RID: 284 RVA: 0x000080E0 File Offset: 0x000062E0
		public IceGolem() : base(ZombieType.IceGolem)
		{
			base.InitSpeed(0.9f);
			this.TextureCoord = new Point(56, 30);
			this.startingTex = this.TextureCoord;
			this.scale = 0.65f;
			this.floorHeight = 0.6f;
			this.ProgressiveDamage = false;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.None;
			this.AffectedByGravity = true;
			this.range = 2f;
			this.leapSpeed = 3f;
			this.topAttackCooldown = 15f;
			this.attackDamage = 4;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.Worth = 5f;
			this.fireCD = Global.RandomFloat(5f, 15f);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000081B0 File Offset: 0x000063B0
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 600f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 5;
			this.BaseSpecialProperties.WaterResist = 80;
			this.BaseSpecialProperties.FireResist = -30;
			this.SomethingChanged = true;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000821B File Offset: 0x0000641B
		public override void Update(float elapsed)
		{
			this.Fire(elapsed);
			base.Update(elapsed);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000822C File Offset: 0x0000642C
		private void Fire(float elapsed)
		{
			if (!NetworkManager.AmIHost)
			{
				return;
			}
			this.fireCD -= elapsed;
			if (this.fireCD <= 0f)
			{
				if (Global.Probability(30))
				{
					this.fireCD = Global.RandomFloat(15f, 20f);
					return;
				}
				if (this.Target != null && VerchickMath.WithinDistance(this.twoDPosition, this.Target.TwoDPosition(), 5f))
				{
					Vector3 randomPosition = VerchickMath.GetRandomPosition(this.Target.Position, 2f);
					randomPosition.Y = 0.3f;
					IcePillar obj = new IcePillar(randomPosition, this);
					Global.MasterCache.CreateObject(obj);
					NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.SpawnEffect);
					netMessage.Payload = new Msg_SpawnEffect
					{
						AttackerUID = base.UID,
						PositionX = randomPosition.X,
						PositionZ = randomPosition.Z,
						Type = 1
					};
					NetworkManager.SendMessage(netMessage, SendType.Reliable);
				}
				this.fireCD = Global.RandomFloat(15f, 20f);
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008338 File Offset: 0x00006538
		private void SpawnFire()
		{
			for (int i = 0; i < 6; i++)
			{
				Global.MasterCache.CreateParticle(ParticleType.Fire, this.Position, Vector3.Zero);
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00008367 File Offset: 0x00006567
		public override void Attack()
		{
			base.Attack();
		}

		// Token: 0x040000BD RID: 189
		private float fireCD = 12f;
	}
}
