using System;
using System.Collections.Generic;

namespace ZombieEstate2
{
	// Token: 0x02000096 RID: 150
	public class Shootable : GameObject
	{
		// Token: 0x060003E6 RID: 998 RVA: 0x0001BFD4 File Offset: 0x0001A1D4
		public Shootable()
		{
			this.BuffManager = new BuffManager(this);
			this.SpecialProperties = new SpecialProperties();
			this.InitBaseSpecialProperties();
			this.FireUpdateProperties();
			this.Health = this.SpecialProperties.MaxHealth;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001C038 File Offset: 0x0001A238
		public Shootable(bool player)
		{
			if (player)
			{
				this.BuffManager = new PlayerBuffManager(this as Player);
			}
			else
			{
				this.BuffManager = new BuffManager(this);
			}
			this.InitBaseSpecialProperties();
			this.SpecialProperties = new SpecialProperties();
			this.FireUpdateProperties();
			this.Health = this.SpecialProperties.MaxHealth;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001C0B2 File Offset: 0x0001A2B2
		public virtual void InitBaseSpecialProperties()
		{
			this.BaseSpecialProperties = new SpecialProperties();
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001C0BF File Offset: 0x0001A2BF
		public void ReInitHealth()
		{
			this.FireUpdateProperties();
			this.Health = this.SpecialProperties.MaxHealth;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001C0D8 File Offset: 0x0001A2D8
		public override void Update(float elapsed)
		{
			if (this.SomethingChanged)
			{
				this.FireUpdateProperties();
			}
			this.BuffManager.Update(elapsed);
			if (this.BuffManager.UpdateSpec(ref this.BuffSpecialProps))
			{
				this.FireUpdateProperties();
			}
			this.HealTimer += elapsed;
			if (this.HealTimer >= 1f)
			{
				if (this.HEALER_OVER_TIME != null)
				{
					this.Heal(this.SpecialProperties.HealOverTime, this.HEALER_OVER_TIME, true);
				}
				else
				{
					this.Heal(this.SpecialProperties.HealOverTime, this, true);
				}
				this.HealTimer = 0f;
			}
			base.Update(elapsed);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001C17C File Offset: 0x0001A37C
		public virtual void FireUpdateProperties()
		{
			SpecialProperties.ClearProps(ref this.SpecialProperties);
			SpecialProperties.AddUpProps(ref this.SpecialProperties, this.BaseSpecialProperties);
			foreach (SpecialProperties two in this.PropertiesToAddUp)
			{
				SpecialProperties.AddUpProps(ref this.SpecialProperties, two);
			}
			SpecialProperties.AddUpProps(ref this.SpecialProperties, this.BuffSpecialProps);
			this.SomethingChanged = false;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001C208 File Offset: 0x0001A408
		public virtual void Heal(float amount, Shootable healer, bool ignoreHealBonus = false)
		{
			if (amount <= 0f)
			{
				return;
			}
			if (healer != null && !ignoreHealBonus)
			{
				amount *= healer.SpecialProperties.HealingDoneMod / 100f + 1f;
			}
			if (this.Health < this.SpecialProperties.MaxHealth && healer is Player)
			{
				Player player = healer as Player;
				float num = Math.Min(amount, this.SpecialProperties.MaxHealth - this.Health);
				player.Talleys.HealingDone += num;
				if (player.PlayerInfo != null && player.PlayerInfo.Local && !player.PlayerInfo.Guest)
				{
					SteamHelper.mHeal += (int)Math.Ceiling((double)num);
					SteamHelper.UpdateAllStats();
				}
			}
			this.Health += amount;
			if (this.Health > this.SpecialProperties.MaxHealth)
			{
				this.Health = this.SpecialProperties.MaxHealth;
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001C2FC File Offset: 0x0001A4FC
		public virtual void Damage(Shootable attacker, float amount, DamageType damageType, bool noGore, bool AOE, List<BulletModifier> mods = null)
		{
			if (mods != null && mods.Contains(BulletModifier.Crit))
			{
				Global.MasterCache.particleSystem.AddCombatText(CombatTextType.HealCrit, Shootable.CritString, this.Position);
			}
			amount = this.GetTrueAmount(amount, damageType, attacker);
			if (attacker != null)
			{
				this.LifeSteal(attacker, amount, AOE);
			}
			if (amount > 0f)
			{
				float num = Math.Min(this.Health, amount);
				this.Health -= amount;
				this.TakenDamage(amount);
				this.BuffManager.TakenDamage(attacker);
				if (attacker != null)
				{
					attacker.BuffManager.DeltDamage(this);
					if (attacker is Player)
					{
						Player player = attacker as Player;
						player.Talleys.DamageDealt += num;
						if (player.PlayerInfo != null && player.PlayerInfo.Local && !player.PlayerInfo.Guest)
						{
							if (this.HACK_AboutToBeMeleed)
							{
								SteamHelper.mMeleeDmg += (int)Math.Ceiling((double)num);
							}
							else
							{
								SteamHelper.mGunDmg += (int)Math.Ceiling((double)num);
							}
							SteamHelper.UpdateAllStats();
						}
					}
					if (attacker is Minion)
					{
						Player player2 = (attacker as Minion).parent as Player;
						if (player2 != null)
						{
							player2.Talleys.MinionDamageDealt += amount;
							if (player2.PlayerInfo != null && player2.PlayerInfo.Local && !player2.PlayerInfo.Guest)
							{
								SteamHelper.mMinionDmg += (int)Math.Ceiling((double)num);
								SteamHelper.UpdateAllStats();
							}
						}
					}
				}
				this.HealOnHit();
			}
			if (this.Health < 1f)
			{
				this.Health = 0f;
				this.Killed(attacker, false);
				if (attacker != null)
				{
					attacker.BuffManager.KilledTarget(this);
				}
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0001C4AC File Offset: 0x0001A6AC
		public override void DestroyObject()
		{
			DynamicShadows.Shootables.Remove(this);
			base.DestroyObject();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001C4C0 File Offset: 0x0001A6C0
		public override void BaseDestroyObject()
		{
			DynamicShadows.Shootables.Remove(this);
			base.BaseDestroyObject();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001C4D4 File Offset: 0x0001A6D4
		private void LifeSteal(Shootable attacker, float amount, bool AOE)
		{
			float num = amount * (attacker.SpecialProperties.LifeStealPercent / 100f);
			if (AOE)
			{
				num = 0f;
			}
			if (num > 0f)
			{
				attacker.Heal(num, null, false);
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001C510 File Offset: 0x0001A710
		public float GetTrueAmount(float amount, DamageType damageType, Shootable attacker)
		{
			switch (damageType)
			{
			case DamageType.Physical:
			{
				float num = 1f - (float)this.SpecialProperties.Armor / 100f;
				amount *= num;
				amount = Math.Max(1f, amount);
				break;
			}
			case DamageType.Fire:
			{
				if (attacker != null)
				{
					float num2 = 1f + attacker.SpecialProperties.FireDmg / 100f;
					amount *= num2;
				}
				float num3 = 1f - (float)this.SpecialProperties.FireResist / 100f;
				amount *= num3;
				amount = Math.Max(1f, amount);
				break;
			}
			case DamageType.Water:
			{
				if (attacker != null)
				{
					float num4 = 1f + attacker.SpecialProperties.WaterDmg / 100f;
					amount *= num4;
				}
				float num5 = 1f - (float)this.SpecialProperties.WaterResist / 100f;
				amount *= num5;
				amount = Math.Max(1f, amount);
				break;
			}
			case DamageType.Earth:
			{
				if (attacker != null)
				{
					float num6 = 1f + attacker.SpecialProperties.EarthDmg / 100f;
					amount *= num6;
				}
				float num7 = 1f - (float)this.SpecialProperties.EarthResist / 100f;
				amount *= num7;
				amount = Math.Max(1f, amount);
				break;
			}
			default:
				Console.WriteLine("ERROR -- unimplemented dmg type: " + damageType.ToString());
				break;
			}
			return amount;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001C680 File Offset: 0x0001A880
		private void HealOnHit()
		{
			if (this.SpecialProperties.HealOnHitPercent != 0f && this.SpecialProperties.HealOnHitAmount > 0f && Global.RandomFloat(0f, 1f) < this.SpecialProperties.HealOnHitPercent)
			{
				this.Heal(this.SpecialProperties.HealOnHitAmount, this, false);
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Killed(Shootable attacker, bool fromNet)
		{
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void TakenDamage(float amount)
		{
		}

		// Token: 0x040003AB RID: 939
		public BuffManager BuffManager;

		// Token: 0x040003AC RID: 940
		public float Health;

		// Token: 0x040003AD RID: 941
		public SpecialProperties SpecialProperties;

		// Token: 0x040003AE RID: 942
		public SpecialProperties BaseSpecialProperties;

		// Token: 0x040003AF RID: 943
		public List<SpecialProperties> PropertiesToAddUp = new List<SpecialProperties>();

		// Token: 0x040003B0 RID: 944
		public SpecialProperties BuffSpecialProps = new SpecialProperties();

		// Token: 0x040003B1 RID: 945
		private float HealTimer;

		// Token: 0x040003B2 RID: 946
		public bool SomethingChanged = true;

		// Token: 0x040003B3 RID: 947
		public bool IgnoreBullets;

		// Token: 0x040003B4 RID: 948
		public bool IgnoreAggro;

		// Token: 0x040003B5 RID: 949
		private static string CritString = "*CRIT*";

		// Token: 0x040003B6 RID: 950
		public Shootable HEALER_OVER_TIME;

		// Token: 0x040003B7 RID: 951
		public bool HACK_AboutToBeMeleed;
	}
}
