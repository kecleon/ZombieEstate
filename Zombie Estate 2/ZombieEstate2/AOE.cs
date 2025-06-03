using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x020000FF RID: 255
	public class AOE : GameObject
	{
		// Token: 0x060006BD RID: 1725 RVA: 0x000325D0 File Offset: 0x000307D0
		public AOE(AOEType aoeType, float dmg, float distance, Vector3 pos, float liveTime, Bullet bullet, GameObject attacker)
		{
			this.Position = pos;
			this.type = aoeType;
			this.scale = distance;
			this.startScale = distance;
			this.startLiveTime = liveTime;
			this.Damage = dmg;
			this.XRotation = -1.5707964f;
			this.Position.Y = 0.1f + Global.RandomFloat(0.001f, 0.005f);
			this.liveTime = liveTime;
			this.bullet = bullet;
			this.effects = new List<string>();
			this.attacker = attacker;
			if (liveTime == -1f)
			{
				this.forever = true;
			}
			switch (this.type)
			{
			case AOEType.Molotov:
				this.TextureCoord = new Point(63, 63);
				this.Damage = 0f;
				this.effects.Add("Fire");
				this.damageEnemies = true;
				this.damagePlayers = false;
				this.emitParticles = true;
				break;
			case AOEType.DemonFire:
				this.TextureCoord = new Point(63, 63);
				this.Damage = 3f;
				this.effects.Add("Fire");
				this.damageEnemies = false;
				this.damagePlayers = true;
				this.emitParticles = true;
				break;
			case AOEType.Sludge:
				this.TextureCoord = new Point(8, 24);
				this.TextureCoord.X = this.TextureCoord.X + Global.rand.Next(0, 4);
				this.Damage = 0f;
				this.damageEnemies = false;
				this.damagePlayers = true;
				this.emitParticles = false;
				break;
			case AOEType.Shadow:
				this.TextureCoord = new Point(4, 38);
				this.Damage = 0f;
				this.damageEnemies = false;
				this.damagePlayers = false;
				this.emitParticles = true;
				this.shrink = true;
				break;
			case AOEType.HealWard:
				this.TextureCoord = new Point(63, 63);
				this.damageEnemies = false;
				this.damagePlayers = true;
				this.shrink = false;
				this.emitParticles = true;
				this.partEmitTimeMod = 3f;
				this.timeBetweenTicks = 1f;
				break;
			case AOEType.AcidSpit:
				this.TextureCoord = new Point(14, 34);
				this.TextureCoord.X = this.TextureCoord.X + Global.rand.Next(0, 3);
				this.Damage = 12f;
				this.damageEnemies = true;
				this.damagePlayers = false;
				this.emitParticles = false;
				AOE.SpitList.Add(this);
				break;
			case AOEType.BlobGoo:
				this.TextureCoord = new Point(17, 32);
				this.TextureCoord.X = this.TextureCoord.X + Global.rand.Next(0, 3);
				this.Damage = 4f;
				this.damageEnemies = false;
				this.damagePlayers = true;
				this.emitParticles = false;
				break;
			case AOEType.Gas:
				this.TextureCoord = new Point(63, 63);
				this.Damage = 20f;
				this.damageEnemies = true;
				this.damagePlayers = false;
				this.emitParticles = true;
				break;
			case AOEType.GasPotent:
				this.TextureCoord = new Point(63, 63);
				this.Damage = 30f;
				this.damageEnemies = true;
				this.damagePlayers = false;
				this.emitParticles = true;
				break;
			case AOEType.Bio:
				this.TextureCoord = new Point(17, 33);
				this.TextureCoord.X = this.TextureCoord.X + Global.rand.Next(0, 3);
				this.Damage = 24f;
				this.damageEnemies = true;
				this.damagePlayers = false;
				this.emitParticles = false;
				break;
			}
			this.AffectedByGravity = false;
			this.ActivateObject(this.Position, this.TextureCoord);
			this.MeldAcid();
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0003299C File Offset: 0x00030B9C
		private void MeldAcid()
		{
			if (this.type == AOEType.AcidSpit)
			{
				for (int i = 0; i < AOE.SpitList.Count; i++)
				{
					AOE aoe = AOE.SpitList[i];
					if (aoe != this && VerchickMath.WithinDistance(aoe.TwoDPosition(), base.TwoDPosition(), aoe.startScale * 1.5f))
					{
						aoe.liveTime = this.startLiveTime;
						aoe.scale = Math.Min(1f, aoe.startScale * 1.2f);
						this.DestroyObject();
						return;
					}
				}
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00032A25 File Offset: 0x00030C25
		public AOE(AOEType aoeType, float dmg, float distance, Vector3 pos, float liveTime, Bullet bullet, GameObject attacker, bool lowParts) : this(aoeType, dmg, distance, pos, liveTime, bullet, attacker)
		{
			this.LowParts = lowParts;
			if (this.LowParts)
			{
				this.partEmitTimeMod = 30f;
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00032A54 File Offset: 0x00030C54
		public override void Update(float elapsed)
		{
			this.tickTime += elapsed;
			this.partTime -= elapsed;
			if (!this.forever)
			{
				this.liveTime -= elapsed;
			}
			if (this.shrink)
			{
				this.scale = this.startScale * (this.liveTime / this.startLiveTime);
			}
			if (this.tickTime >= this.timeBetweenTicks)
			{
				this.tickTime = 0f;
				if (this.damageEnemies)
				{
					this.DamageEnemies();
				}
				if (this.damagePlayers)
				{
					this.DamagePlayers();
				}
			}
			if (this.partTime <= 0f)
			{
				this.partTime = 0.005f / this.startScale * this.partEmitTimeMod;
				this.EmitParticle();
			}
			if (!this.forever && this.liveTime <= 0f)
			{
				this.DestroyObject();
			}
			base.UpdateTile();
			base.Update(elapsed);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00032B3E File Offset: 0x00030D3E
		public override void DestroyObject()
		{
			if (this.type == AOEType.AcidSpit)
			{
				AOE.SpitList.Remove(this);
			}
			base.DestroyObject();
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00032B5C File Offset: 0x00030D5C
		private void EmitParticle()
		{
			if (!this.emitParticles)
			{
				return;
			}
			Vector3 position = this.Position;
			float num = 6.2831855f * Global.RandomFloat(0f, 1f);
			float num2 = Global.RandomFloat(0f, this.scale);
			position.X += (float)Math.Cos((double)num) * num2;
			position.Z += (float)Math.Sin((double)num) * num2;
			position.Y = 0.4f;
			switch (this.type)
			{
			case AOEType.Molotov:
				Global.MasterCache.CreateParticle(ParticleType.Fire, position, this.Velocity);
				return;
			case AOEType.DemonFire:
				Global.MasterCache.CreateParticle(ParticleType.DemonFire, position, this.Velocity);
				return;
			case AOEType.Sludge:
			case AOEType.Shadow:
			case AOEType.AcidSpit:
			case AOEType.BlobGoo:
				break;
			case AOEType.HealWard:
				Global.MasterCache.CreateParticle(ParticleType.HealWard, position, this.Velocity);
				return;
			case AOEType.Gas:
				Global.MasterCache.CreateParticle(ParticleType.Gas, position, this.Velocity);
				return;
			case AOEType.GasPotent:
				Global.MasterCache.CreateParticle(ParticleType.GasPotent, position, this.Velocity);
				break;
			default:
				return;
			}
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00032C6C File Offset: 0x00030E6C
		private void DamageEnemies()
		{
			if (this.tile == null)
			{
				return;
			}
			for (int i = 0; i < this.tile.AdjacentObjects.Count; i++)
			{
				Zombie zombie = this.tile.AdjacentObjects[i] as Zombie;
				if (zombie != null && VerchickMath.WithinDistance(base.TwoDPosition(), zombie.TwoDPosition(), this.scale))
				{
					this.DamageZombie(zombie);
				}
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00032CD8 File Offset: 0x00030ED8
		private void DamagePlayers()
		{
			foreach (Player player in Global.PlayerList)
			{
				if (VerchickMath.WithinDistance(base.TwoDPosition(), player.TwoDPosition(), this.scale))
				{
					this.DamagePlayer(player);
				}
				for (int i = 0; i < player.GetMinionList.Count; i++)
				{
					Minion minion = player.GetMinionList[i];
					if (VerchickMath.WithinDistance(minion.TwoDPosition(), this.twoDPosition, this.scale))
					{
						this.DamagePlayer(minion);
					}
				}
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00032D88 File Offset: 0x00030F88
		private void DamageZombie(Zombie zomb)
		{
			AOEType aoetype = this.type;
			if (aoetype == AOEType.Molotov)
			{
				zomb.Damage(this.attacker as Shootable, this.Damage, DamageType.Fire, false, true, null);
				return;
			}
			if (aoetype != AOEType.Bio)
			{
				return;
			}
			zomb.Damage(this.attacker as Shootable, this.Damage, DamageType.Earth, false, true, null);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00032DDC File Offset: 0x00030FDC
		private void DamagePlayer(Player player)
		{
			switch (this.type)
			{
			case AOEType.DemonFire:
				player.Damage(this.attacker as Shootable, this.Damage, DamageType.Fire, false, true, null);
				return;
			case AOEType.Sludge:
				player.AddBuff(AOE.SLUDGE, this.attacker as Shootable, AOE.SLUDGEARGS);
				break;
			case AOEType.Shadow:
			case AOEType.AcidSpit:
				break;
			case AOEType.HealWard:
				player.Heal((float)((int)this.Damage), this.attacker as Shootable, false);
				return;
			case AOEType.BlobGoo:
				player.Damage(this.attacker as Shootable, this.Damage, DamageType.Water, false, true, null);
				return;
			default:
				return;
			}
		}

		// Token: 0x0400069D RID: 1693
		public AOEType type;

		// Token: 0x0400069E RID: 1694
		public float Damage;

		// Token: 0x0400069F RID: 1695
		private float tickTime;

		// Token: 0x040006A0 RID: 1696
		private float partTime = 0.05f;

		// Token: 0x040006A1 RID: 1697
		private float liveTime;

		// Token: 0x040006A2 RID: 1698
		private float timeBetweenTicks = 0.5f;

		// Token: 0x040006A3 RID: 1699
		private Bullet bullet;

		// Token: 0x040006A4 RID: 1700
		private List<string> effects;

		// Token: 0x040006A5 RID: 1701
		private bool damagePlayers;

		// Token: 0x040006A6 RID: 1702
		private bool damageEnemies = true;

		// Token: 0x040006A7 RID: 1703
		private bool emitParticles;

		// Token: 0x040006A8 RID: 1704
		private GameObject attacker;

		// Token: 0x040006A9 RID: 1705
		private float startScale;

		// Token: 0x040006AA RID: 1706
		private float startLiveTime;

		// Token: 0x040006AB RID: 1707
		private float partEmitTimeMod = 1f;

		// Token: 0x040006AC RID: 1708
		private bool shrink;

		// Token: 0x040006AD RID: 1709
		private bool forever;

		// Token: 0x040006AE RID: 1710
		private bool LowParts;

		// Token: 0x040006AF RID: 1711
		private DamageType dmgType;

		// Token: 0x040006B0 RID: 1712
		public static List<AOE> SpitList = new List<AOE>();

		// Token: 0x040006B1 RID: 1713
		private static string SLUDGE = "Debuff_Sludge";

		// Token: 0x040006B2 RID: 1714
		private static string SLUDGEARGS = "3.0, 0.5";
	}
}
