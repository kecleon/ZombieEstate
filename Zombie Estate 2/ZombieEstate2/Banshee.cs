using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x0200002F RID: 47
	internal class Banshee : Zombie
	{
		// Token: 0x0600010C RID: 268 RVA: 0x000079E4 File Offset: 0x00005BE4
		public Banshee() : base(ZombieType.Banshee)
		{
			base.InitSpeed(1.4f);
			this.TextureCoord = new Point(52, 12);
			this.startingTex = this.TextureCoord;
			this.scale = 0.42f;
			this.ProgressiveDamage = false;
			this.BloodType = ParticleType.GhostGoo;
			this.GibbletType = ParticleType.GhostGibblet;
			this.AffectedByGravity = true;
			this.range = 1f;
			this.leapSpeed = 4f;
			this.topAttackCooldown = 6f;
			this.attackDamage = 8;
			this.PreciseDirection = false;
			this.GibbletChance = 75;
			this.BounceEnabled = false;
			this.Worth = 15f;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007AA8 File Offset: 0x00005CA8
		public override void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
			this.BaseSpecialProperties.MaxHealth = 80f * Global.ZombieHealthMod;
			this.BaseSpecialProperties.Speed = 0f;
			this.BaseSpecialProperties.Armor = 0;
			this.BaseSpecialProperties.EarthResist = 25;
			this.BaseSpecialProperties.FireResist = 25;
			this.BaseSpecialProperties.WaterResist = 25;
			this.BaseSpecialProperties.LifeStealPercent = 10f;
			this.SomethingChanged = true;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007B30 File Offset: 0x00005D30
		public override void Update(float elapsed)
		{
			this.IgnoreBullets = this.Phased;
			if (!this.Phased)
			{
				this.PhaseCooldown -= elapsed;
				if (this.PhaseCooldown <= 0f)
				{
					this.PhaseCooldown = 10f;
					if (base.OnFloor())
					{
						this.Phased = true;
						this.AffectedByGravity = false;
						this.Velocity.Y = 1.1f;
						this.PhaseTime = 3f;
						this.startingTex = new Point(52, 18);
						this.TextureCoord = new Point(52, 18);
					}
				}
			}
			else
			{
				if (this.Position.Y >= 1.2f)
				{
					this.Position.Y = 1.2f;
				}
				this.PhaseTime -= elapsed;
				if (this.PhaseTime <= 0f)
				{
					if (this.tile != null && !this.tile.TileProperties.Contains(TilePropertyType.NoPath))
					{
						this.Phased = false;
						this.AffectedByGravity = true;
						this.startingTex = new Point(52, 12);
						this.TextureCoord = new Point(52, 12);
						this.UpdateDirection();
					}
					if (this.tile == null)
					{
						this.DestroyObject();
					}
				}
			}
			base.Update(elapsed);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00007C73 File Offset: 0x00005E73
		public override void UpdateDirection()
		{
			if (this.Phased)
			{
				if (this.Target != null)
				{
					this.movement = VerchickMath.DirectionToVector2(base.TwoDPosition(), this.Target.TwoDPosition());
				}
				return;
			}
			base.UpdateDirection();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00007CA8 File Offset: 0x00005EA8
		public override void Damage(Shootable attacker, float amount, DamageType damageType, bool noGore, bool AOE, List<BulletModifier> mods = null)
		{
			if (!this.Phased)
			{
				base.Damage(attacker, amount, damageType, noGore, AOE, mods);
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00007CC1 File Offset: 0x00005EC1
		protected override void AdditionalReceive(Msg_ZombieUpdate zUpdate)
		{
			this.Phased = zUpdate.Misc;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00007CCF File Offset: 0x00005ECF
		protected override void AdditionalSync(Msg_ZombieUpdate zUpdate)
		{
			zUpdate.Misc = this.Phased;
		}

		// Token: 0x040000B8 RID: 184
		private float PhaseCooldown = 4f;

		// Token: 0x040000B9 RID: 185
		private bool Phased;

		// Token: 0x040000BA RID: 186
		private float PhaseTime = 3f;
	}
}
