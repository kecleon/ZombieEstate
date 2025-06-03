using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.Enemies.EnemyAbilities;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x02000037 RID: 55
	internal class FireWitch : Zombie
	{
		// Token: 0x06000155 RID: 341 RVA: 0x0000A044 File Offset: 0x00008244
		public FireWitch() : base(ZombieType.FireWitch)
		{
			base.InitSpeed(0.9f);
			this.TextureCoord = new Point(56, 24);
			this.startingTex = this.TextureCoord;
			this.scale = 0.48f;
			this.ProgressiveDamage = false;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.FireWitchGibblet;
			this.AffectedByGravity = true;
			this.range = 2f;
			this.leapSpeed = 3f;
			this.topAttackCooldown = 15f;
			this.attackDamage = 4;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.Worth = 5f;
			this.teleCD = Global.RandomFloat(5f, 15f);
			this.fireCD = Global.RandomFloat(5f, 15f);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000A12C File Offset: 0x0000832C
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 300f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 10;
			this.BaseSpecialProperties.FireResist = 80;
			this.BaseSpecialProperties.WaterResist = -10;
			this.SomethingChanged = true;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000A198 File Offset: 0x00008398
		public override void Update(float elapsed)
		{
			this.Fire(elapsed);
			base.Update(elapsed);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000A1A8 File Offset: 0x000083A8
		private void Tele(float elapsed)
		{
			this.teleCD -= elapsed;
			if (this.teleCD <= 0f)
			{
				if (this.Target != null && VerchickMath.WithinDistance(this.twoDPosition, this.Target.TwoDPosition(), 8f))
				{
					Vector3 randomPosition = VerchickMath.GetRandomPosition(this.Target.Position, 3f);
					randomPosition.Y = 0.5f;
					Tile tileAtLocation = Global.Level.GetTileAtLocation(randomPosition);
					if (tileAtLocation != null && !tileAtLocation.TileProperties.Contains(TilePropertyType.NoSpawn) && !tileAtLocation.TileProperties.Contains(TilePropertyType.NoPath))
					{
						this.SpawnFire();
						this.Position = randomPosition;
						this.SpawnFire();
						foreach (Player player in Global.PlayerList)
						{
							if (VerchickMath.WithinDistance(this.twoDPosition, player.TwoDPosition(), 1f))
							{
								player.Damage(this, 5f, DamageType.Fire, false, false, null);
							}
						}
						if (this.targTile == null || this.targTile == tileAtLocation || this.prevTile != tileAtLocation || this.tile == null)
						{
							this.UpdateDirection();
						}
					}
				}
				this.teleCD = Global.RandomFloat(10f, 15f);
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000A30C File Offset: 0x0000850C
		private void Fire(float elapsed)
		{
			if (!NetworkManager.AmIHost)
			{
				return;
			}
			this.fireCD -= elapsed;
			if (this.fireCD <= 0f)
			{
				if (this.Target != null && VerchickMath.WithinDistance(this.twoDPosition, this.Target.TwoDPosition(), 5f))
				{
					FirePillar obj = new FirePillar(this.Target.Position, this);
					Global.MasterCache.CreateObject(obj);
					NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.SpawnEffect);
					netMessage.Payload = new Msg_SpawnEffect
					{
						AttackerUID = base.UID,
						PositionX = this.Target.Position.X,
						PositionZ = this.Target.Position.Z,
						Type = 0
					};
					NetworkManager.SendMessage(netMessage, SendType.Reliable);
				}
				this.fireCD = Global.RandomFloat(15f, 25f);
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000A3F4 File Offset: 0x000085F4
		private void SpawnFire()
		{
			for (int i = 0; i < 6; i++)
			{
				Global.MasterCache.CreateParticle(ParticleType.Fire, this.Position, Vector3.Zero);
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00008367 File Offset: 0x00006567
		public override void Attack()
		{
			base.Attack();
		}

		// Token: 0x040000EA RID: 234
		private float teleCD = 12f;

		// Token: 0x040000EB RID: 235
		private float fireCD = 12f;
	}
}
