using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.Enemies.EnemyAbilities;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x0200002A RID: 42
	internal class Clown : Zombie
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x00006E78 File Offset: 0x00005078
		public Clown() : base(ZombieType.Clown)
		{
			base.InitSpeed(1.4f);
			this.TextureCoord = new Point(52, 0);
			this.startingTex = this.TextureCoord;
			this.scale = 0.42f;
			this.ProgressiveDamage = false;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.ClownGibblet;
			this.AffectedByGravity = true;
			this.range = 1f;
			this.leapSpeed = 4f;
			this.topAttackCooldown = 6f;
			this.attackDamage = 8;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.BounceEnabled = true;
			this.Worth = 10f;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006F30 File Offset: 0x00005130
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 120f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 0;
			this.BaseSpecialProperties.LifeStealPercent = 0f;
			this.SomethingChanged = true;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006F91 File Offset: 0x00005191
		protected override void InitiateAllRandomness()
		{
			base.InitiateAllRandomness();
			this.hopCD = (float)this.Rand.Next(13, 20);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006FB0 File Offset: 0x000051B0
		public override void Update(float elapsed)
		{
			this.hopCD -= elapsed;
			if (this.Target != null && this.hopCD <= 0f)
			{
				if (VerchickMath.WithinDistance(this.Target.twoDPosition, this.twoDPosition, 5f))
				{
					this.Velocity.Y = 6f;
					this.justHopped = true;
					this.hopCD = 20f;
				}
				else
				{
					this.hopCD = 8f;
				}
			}
			base.Update(elapsed);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007034 File Offset: 0x00005234
		public override void Landed()
		{
			if (!NetworkManager.AmIHost)
			{
				this.justHopped = false;
				return;
			}
			if (this.justHopped)
			{
				Clown.SpawnConfetti(this.Position, this);
				NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.SpawnEffect);
				netMessage.Payload = new Msg_SpawnEffect
				{
					AttackerUID = base.UID,
					PositionX = this.Position.X,
					PositionZ = this.Position.Z,
					Type = 3
				};
				NetworkManager.SendMessage(netMessage, SendType.Reliable);
				this.justHopped = false;
			}
			base.Landed();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000070C0 File Offset: 0x000052C0
		public static void SpawnConfetti(Vector3 pos, Zombie z)
		{
			for (int i = 0; i < 9; i++)
			{
				float angle = (float)i * 0.6981317f;
				float delay = 0f;
				Confetti obj = new Confetti(angle, pos, delay, z);
				Global.MasterCache.CreateObject(obj);
			}
		}

		// Token: 0x0400009F RID: 159
		private float hopCD = 5f;

		// Token: 0x040000A0 RID: 160
		private bool justHopped;
	}
}
