using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZombieEstate2.Buffs.Types;

namespace ZombieEstate2
{
	// Token: 0x0200010C RID: 268
	public static class Explosion
	{
		// Token: 0x06000736 RID: 1846 RVA: 0x00037020 File Offset: 0x00035220
		public static void CreateExplosion(float range, float damage, float pushBackMod, string type, Vector3 Position, GameObject parent, bool COSMETIC = false)
		{
			List<Zombie> zombieList = Global.ZombieList;
			if (parent is Shootable)
			{
				Shootable shootable = parent as Shootable;
				range *= 1f + (1f + shootable.SpecialProperties.ExplosionRadiusMod / 100f);
				if (type != "StinkOoze")
				{
					damage *= 1f + (1f + shootable.SpecialProperties.ExplosionDamageMod / 100f);
				}
			}
			if (!COSMETIC)
			{
				Camera.ShakeCamera(range * 0.1f, range * 0.01f);
			}
			bool flag = false;
			DamageType dmgType = DamageType.Fire;
			if (type == "SlowFire")
			{
				Camera.SlowTime(1f, 0.08f);
				Camera.ShakeCamera(0.6f, 0.1f);
				SoundEngine.PlaySound(Explosion.sFire, Position, -0.3f, 0.3f, 0.7f);
			}
			if (type == "Fire")
			{
				dmgType = DamageType.Fire;
				SoundEngine.PlaySound(Explosion.sFire, Position, -0.3f, 0.4f, 0.7f);
			}
			if (type == "DemonFire")
			{
				dmgType = DamageType.Fire;
				SoundEngine.PlaySound(Explosion.sFire, Position, -0.6f, 0.1f, 0.5f);
			}
			if (type == "MiniFire")
			{
				dmgType = DamageType.Fire;
				SoundEngine.PlaySound(Explosion.sFire, Position, -0.3f, 0.4f, 0.35f);
				type = "Fire";
			}
			if (type == "DemonSpawn")
			{
				Camera.ShakeCamera(0.2f, 0.05f);
				flag = true;
			}
			if (type == "Goo")
			{
				flag = true;
				dmgType = DamageType.Earth;
				SoundEngine.PlaySound(Explosion.sGoo, Position, 0.3f, 0.6f, 0.25f);
			}
			if (type == "Blood")
			{
				flag = false;
				dmgType = DamageType.Physical;
				SoundEngine.PlaySound(Explosion.sGoo, Position, -0.3f, 0.3f);
			}
			if (type == "BloodCow")
			{
				flag = false;
				dmgType = DamageType.Physical;
				Camera.ShakeCamera(0.3f, 0.07f);
				type = "Blood";
				SoundEngine.PlaySound(Explosion.sGoo, Position, -0.8f, -0.3f);
			}
			if (type == "GooPlayer")
			{
				flag = false;
				dmgType = DamageType.Earth;
				type = "Goo";
				AOE obj = new AOE(AOEType.Bio, 24f, 1.5f, Position, 6f, null, parent);
				Global.MasterCache.CreateObject(obj);
			}
			if (type == "Tractor")
			{
				flag = true;
			}
			if (type == "FreezeEnemy")
			{
				dmgType = DamageType.Water;
				SoundEngine.PlaySound(Explosion.sFire, Position, 0.3f, 0.6f, 0.5f);
				flag = true;
			}
			if (type == "StinkOoze")
			{
				dmgType = DamageType.Physical;
				SoundEngine.PlaySound(Explosion.sFire, Position, 1.3f, 1.6f, 0.5f);
				flag = false;
			}
			if (type == "Freeze")
			{
				dmgType = DamageType.Water;
				SoundEngine.PlaySound(Explosion.sFire, Position, 0.3f, 0.6f, 0.5f);
			}
			if (type == "MiniFrost")
			{
				dmgType = DamageType.Water;
				SoundEngine.PlaySound(Explosion.sFire, Position, 0.3f, 0.6f, 0.3f);
				type = "Freeze";
			}
			if (type == "Popcorn")
			{
				SoundEngine.PlaySound(Explosion.sPop, Position, -0.3f, 0.3f, 0.8f);
			}
			if (type == "Potato")
			{
				SoundEngine.PlaySound(Explosion.sGoo, Position, -0.4f, -0.2f);
			}
			if (type == "Suds")
			{
				dmgType = DamageType.Water;
				SoundEngine.PlaySound(Explosion.sSuds, Position, -0.3f, 0.3f, 0.8f);
			}
			if (type == "Egg")
			{
				Global.MasterCache.explosionPartSystem.AddParticle(Position, type, range, Position, 0.5f);
				SoundEngine.PlaySound(Explosion.sGoo, Position, -0.2f, 0.3f);
			}
			else
			{
				int num = Math.Max((int)(range * range * 2f), 5);
				num = Math.Min(num, 100);
				for (int i = 0; i < num; i++)
				{
					Vector3 pos = new Vector3(Position.X, Position.Y, Position.Z);
					float num2 = Global.RandomFloat(0f, range);
					float num3 = Global.RandomFloat(0f, 6.2831855f);
					Global.RandomFloat(0f, 3.1415927f);
					pos.X += (float)Math.Cos((double)num3) * num2;
					pos.Z += (float)Math.Sin((double)num3) * num2;
					pos.Y += Global.RandomFloat(0.55f, num2 / 2f);
					float num4 = 1f;
					if (range != 0f)
					{
						num4 = 1f + (1f - num2 / range) * Math.Min(range / 4f, 2f);
					}
					num4 = Math.Min(num4, 3f);
					Global.MasterCache.explosionPartSystem.AddParticle(pos, type, range, Position, num4);
				}
			}
			Vector2 twoDPos = new Vector2(Position.X, Position.Z);
			if (!COSMETIC)
			{
				Explosion.DoDamage(range, damage, pushBackMod, type, ref Position, parent, zombieList, twoDPos, dmgType);
				if (flag)
				{
					foreach (Player player in Global.PlayerList)
					{
						Explosion.HurtPlayer(range, damage, pushBackMod, type, parent, dmgType, ref twoDPos, player);
						for (int j = 0; j < player.GetMinionList.Count; j++)
						{
							Minion player2 = player.GetMinionList[j];
							Explosion.HurtPlayer(range, damage, pushBackMod, type, parent, dmgType, ref twoDPos, player2);
						}
					}
				}
			}
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x000375C0 File Offset: 0x000357C0
		private static void HurtPlayer(float range, float damage, float pushBackMod, string type, GameObject parent, DamageType dmgType, ref Vector2 twoDPos, Player player)
		{
			if (!VerchickMath.WithinDistance(twoDPos, player.TwoDPosition(), range))
			{
				return;
			}
			float num = 1f;
			int num2 = (int)(damage * num);
			Vector2 vector = VerchickMath.DirectionToVector2(twoDPos, player.TwoDPosition());
			player.Velocity.X = player.Velocity.X + vector.X * pushBackMod * num;
			player.Velocity.Z = player.Velocity.Z + vector.Y * pushBackMod * num;
			player.Velocity.Y = player.Velocity.Y + num * pushBackMod;
			player.Damage(parent as Shootable, (float)num2, dmgType, false, true, null);
			if (type == "FreezeEnemy")
			{
				player.BuffManager.AddBuff("Debuff_Ice", parent as Shootable, "2.0, 0.5");
			}
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0003768C File Offset: 0x0003588C
		private static void DoDamage(float range, float damage, float pushBackMod, string type, ref Vector3 Position, GameObject parent, List<Zombie> list, Vector2 twoDPos, DamageType dmgType)
		{
			if (damage != 0f || pushBackMod != 0f)
			{
				foreach (Tile tile in Explosion.GetTiles(range, twoDPos))
				{
					for (int i = 0; i < tile.ContainedObjects.Count; i++)
					{
						Zombie zombie = tile.ContainedObjects[i] as Zombie;
						if (zombie != null && parent as Zombie != zombie && VerchickMath.WithinDistance(twoDPos, zombie.TwoDPosition(), range))
						{
							Zombie zombie2 = zombie;
							if (zombie2.Health > 0f)
							{
								if (type == "StinkOoze")
								{
									float num = 1f;
									if (parent is Shootable)
									{
										num = (parent as Shootable).SpecialProperties.HealingDoneMod / 100f + 1f;
									}
									Debuff_Ooze debuff_Ooze = new Debuff_Ooze((int)(damage * num), 12f);
									debuff_Ooze.Init(zombie2, parent as Shootable);
									zombie2.BuffManager.AddBuff(debuff_Ooze);
								}
								else
								{
									float num2 = 1f;
									Vector2 vector = VerchickMath.DirectionToVector2(twoDPos, zombie2.TwoDPosition());
									Zombie zombie3 = zombie2;
									zombie3.Velocity.X = zombie3.Velocity.X + vector.X * pushBackMod * num2;
									Zombie zombie4 = zombie2;
									zombie4.Velocity.Z = zombie4.Velocity.Z + vector.Y * pushBackMod * num2;
									Zombie zombie5 = zombie2;
									zombie5.Velocity.Y = zombie5.Velocity.Y + num2 * pushBackMod;
									if (zombie2.BuffManager != null)
									{
										if (type == "Fire" || type == "SlowFire")
										{
											zombie2.BuffManager.AddBuff("Debuff_Fire", parent as Shootable, "3.0, 8");
										}
										if (type == "Freeze")
										{
											zombie2.BuffManager.AddBuff("Debuff_Ice", parent as Shootable, "2.0, 0.5");
										}
									}
									zombie2.Damage(parent as Shootable, damage, dmgType, true, true, null);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x000378B8 File Offset: 0x00035AB8
		private static List<Tile> GetTiles(float range, Vector2 pos)
		{
			List<Tile> list = new List<Tile>();
			Tile tileAtLocation = Global.Level.GetTileAtLocation(new Vector3(pos.X, 0f, pos.Y));
			if (tileAtLocation == null)
			{
				return list;
			}
			for (int i = tileAtLocation.x - (int)Math.Max(1f, range); i <= tileAtLocation.x + (int)Math.Max(1f, range); i++)
			{
				for (int j = tileAtLocation.y - (int)Math.Max(1f, range); j <= tileAtLocation.y + (int)Math.Max(1f, range); j++)
				{
					Tile tile = Global.Level.GetTile(i, j);
					if (tile != null)
					{
						list.Add(tile);
					}
				}
			}
			return list;
		}

		// Token: 0x0400072C RID: 1836
		private static string sGoo = "ze2_goo";

		// Token: 0x0400072D RID: 1837
		private static string sFire = "ze2_explo1";

		// Token: 0x0400072E RID: 1838
		private static string sPop = "ze2_popcornpop";

		// Token: 0x0400072F RID: 1839
		private static string sSuds = "ze2_bubblepop";
	}
}
