using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.Enemies.Weapons;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x02000030 RID: 48
	internal class Gardner : Zombie
	{
		// Token: 0x06000113 RID: 275 RVA: 0x00007CE0 File Offset: 0x00005EE0
		public Gardner() : base(ZombieType.Gardner)
		{
			base.InitSpeed(1.4f);
			this.TextureCoord = new Point(74, 2);
			this.startingTex = this.TextureCoord;
			this.scale = 0.6f;
			this.floorHeight = 0.6f;
			this.TexScale = 2f;
			this.ProgressiveDamage = true;
			this.BloodType = ParticleType.Blood;
			this.GibbletType = ParticleType.None;
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

		// Token: 0x06000114 RID: 276 RVA: 0x00007DAC File Offset: 0x00005FAC
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 310f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 10;
			this.BaseSpecialProperties.LifeStealPercent = 0f;
			this.SomethingChanged = true;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00007E0E File Offset: 0x0000600E
		protected override void InitiateAllRandomness()
		{
			base.InitiateAllRandomness();
			this.hopCD = (float)this.Rand.Next(5, 10);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00007E2C File Offset: 0x0000602C
		public override void Update(float elapsed)
		{
			this.hopCD -= elapsed;
			if (this.Target != null && this.hopCD <= 0f)
			{
				if (VerchickMath.WithinDistance(this.Target.twoDPosition, this.twoDPosition, 5f))
				{
					this.Velocity.Y = 6f;
					this.justHopped = true;
					this.hopCD = 25f;
				}
				else
				{
					this.hopCD = 10f;
				}
			}
			base.Update(elapsed);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00007EB0 File Offset: 0x000060B0
		public override void Landed()
		{
			if (!NetworkManager.AmIHost)
			{
				this.justHopped = false;
				return;
			}
			if (this.justHopped)
			{
				Gardner.SpawnVines(this.Position, this);
				NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.SpawnEffect);
				netMessage.Payload = new Msg_SpawnEffect
				{
					AttackerUID = base.UID,
					PositionX = this.Position.X,
					PositionZ = this.Position.Z,
					Type = 2
				};
				NetworkManager.SendMessage(netMessage, SendType.Reliable);
				this.justHopped = false;
			}
			base.Landed();
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007F3C File Offset: 0x0000613C
		public static void SpawnVines(Vector3 pos, Zombie z)
		{
			float num = Global.RandomFloat(0f, 6.2831855f);
			for (int i = 0; i < 4; i++)
			{
				float angle = num + (float)i * 1.5707964f;
				VineAttack obj = new VineAttack(pos, VerchickMath.AngleToVector(angle), z);
				Global.MasterCache.CreateObject(obj);
			}
		}

		// Token: 0x040000BB RID: 187
		private float hopCD = 5f;

		// Token: 0x040000BC RID: 188
		private bool justHopped;
	}
}
