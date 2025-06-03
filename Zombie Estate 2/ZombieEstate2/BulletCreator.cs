using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using ZombieEstate2.Networking;

namespace ZombieEstate2
{
	// Token: 0x02000100 RID: 256
	public static class BulletCreator
	{
		// Token: 0x060006C8 RID: 1736 RVA: 0x00032EA0 File Offset: 0x000310A0
		public static void Init(MasterCache master)
		{
			BulletCreator.objList = master.gameObjectCaches[0].gameObjects;
			BulletCreator.startIndex = BulletCreator.objList.Count - BulletCreator.COUNT - 250;
			BulletCreator.Bullets = new Queue<GameObject>();
			for (int i = BulletCreator.startIndex; i < BulletCreator.startIndex + BulletCreator.COUNT; i++)
			{
				BulletCreator.objList[i] = new Bullet();
				BulletCreator.objList[i].DestroyObject();
				BulletCreator.Bullets.Enqueue(BulletCreator.objList[i]);
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00032F38 File Offset: 0x00031138
		public static Bullet BulletsFired(GunStats stats, int level, ref Vector3 pos, ref Vector3 dir, float chargeTime, Shootable parent, Gun gun, List<BulletModifier> mods, ref ushort startUID, int seed)
		{
			bool flag = startUID == ushort.MaxValue;
			if (flag)
			{
				startUID = NetworkManager.GetMachineUID();
			}
			if (stats.Minion)
			{
				Player player = parent as Player;
				if (player != null && player.GetMinionList.Count >= 2)
				{
					float num = 1.1f;
					Minion minion = player.GetMinionList[0];
					foreach (Minion minion2 in player.GetMinionList)
					{
						if (num > minion2.GetPercentage())
						{
							minion = minion2;
							num = minion2.GetPercentage();
						}
					}
					if (minion != null)
					{
						minion.DestroyObject();
					}
				}
				Terminal.WriteMessage(string.Concat(new object[]
				{
					"Minion created. UID: ",
					startUID,
					" | Parent OwnerIndex: ",
					parent.OwnerIndex
				}), MessageType.AI);
				BaseMinion obj = new BaseMinion(parent, stats.GunProperties[level].MinionGunStats, stats.GunProperties[level].MinionAmmo, stats.GunProperties[level].MinionMoveSpeed, pos, new Point(stats.GunProperties[level].MinionTexture.X, stats.GunProperties[level].MinionTexture.Y), stats.GunProperties[level].MinionMuzzleFlash, level, stats.GunProperties[level].MinionHealth_AmmoCount, stats.GunProperties[level].MinionBuff, stats.GunProperties[level].MinionBuffArgs, startUID);
				Global.MasterCache.CreateObject(obj);
				return null;
			}
			Random random = new Random(seed);
			for (int i = 0; i < stats.GunProperties[level].NumberOfBulletsFired; i++)
			{
				if (i != 0 && flag)
				{
					NetworkManager.IncrementUID();
				}
				float num2 = (float)stats.GunProperties[level].NumberOfBulletsFired;
				float accuracy = stats.GunProperties[level].Accuracy;
				Vector3 vector = new Vector3(dir.X, dir.Y, dir.Z);
				if (stats.FireEvenly)
				{
					float num3 = ((float)i - num2 / 2f) * (accuracy / num2);
					num3 += accuracy / num2 / 2f;
					num3 = MathHelper.ToRadians(num3);
					vector = Vector3.Transform(vector, Matrix.CreateRotationY(num3));
				}
				Vector3 pos2 = pos;
				Bullet bullet = (Bullet)BulletCreator.Bullets.Dequeue();
				if (bullet.Active)
				{
					bullet.DestroyObject();
				}
				bullet.InitBullet(BulletCreator.GetBullet(stats.GunProperties[level].BulletType), stats, parent.SpecialProperties, stats.SpecialProperties[level], new Vector2(vector.X, vector.Z), pos2, (ushort)((int)startUID + i), parent, level, mods, random.Next());
				BulletCreator.Bullets.Enqueue(bullet);
			}
			return null;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002036B File Offset: 0x0001E56B
		public static Bullet CreateBulletNoNet(string bullName, ref Vector3 pos, ref Vector3 dir, float chargeMod, int parentID, int gunID, int UID)
		{
			return null;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0003324C File Offset: 0x0003144C
		public static void LoadBullets()
		{
			Terminal.WriteMessage("Loading Bullets", MessageType.SAVELOAD);
			Terminal.WriteMessage("---------------------------------------------", MessageType.SAVELOAD);
			BulletCreator.bulletStats.Add("Normal", BulletCreator.LoadBull("Normal"));
			BulletCreator.bulletStats.Add("Arrow", BulletCreator.LoadBull("Arrow"));
			BulletCreator.bulletStats.Add("HeavyNormal", BulletCreator.LoadBull("HeavyNormal"));
			BulletCreator.bulletStats.Add("SnowBall", BulletCreator.LoadBull("SnowBall"));
			BulletCreator.bulletStats.Add("SnowBallRange", BulletCreator.LoadBull("SnowBallRange"));
			BulletCreator.bulletStats.Add("Dart", BulletCreator.LoadBull("Dart"));
			BulletCreator.bulletStats.Add("HeavyDart", BulletCreator.LoadBull("HeavyDart"));
			BulletCreator.bulletStats.Add("Card", BulletCreator.LoadBull("Card"));
			BulletCreator.bulletStats.Add("HeavyCard", BulletCreator.LoadBull("HeavyCard"));
			BulletCreator.bulletStats.Add("VacuumGust", BulletCreator.LoadBull("VacuumGust"));
			BulletCreator.bulletStats.Add("Bubble", BulletCreator.LoadBull("Bubble"));
			BulletCreator.bulletStats.Add("BubbleLarge", BulletCreator.LoadBull("BubbleLarge"));
			BulletCreator.bulletStats.Add("HeavyBubble", BulletCreator.LoadBull("HeavyBubble"));
			BulletCreator.bulletStats.Add("Laser", BulletCreator.LoadBull("Laser"));
			BulletCreator.bulletStats.Add("HeavyLaser", BulletCreator.LoadBull("HeavyLaser"));
			BulletCreator.bulletStats.Add("StuffedAnimal", BulletCreator.LoadBull("StuffedAnimal"));
			BulletCreator.bulletStats.Add("LargeSoap", BulletCreator.LoadBull("LargeSoap"));
			BulletCreator.bulletStats.Add("SmallSoap", BulletCreator.LoadBull("SmallSoap"));
			BulletCreator.bulletStats.Add("Cereal1", BulletCreator.LoadBull("Cereal1"));
			BulletCreator.bulletStats.Add("Cereal2", BulletCreator.LoadBull("Cereal2"));
			BulletCreator.bulletStats.Add("Cereal3", BulletCreator.LoadBull("Cereal3"));
			BulletCreator.bulletStats.Add("BinaryGreen", BulletCreator.LoadBull("BinaryGreen"));
			BulletCreator.bulletStats.Add("BinaryRed", BulletCreator.LoadBull("BinaryRed"));
			BulletCreator.bulletStats.Add("Rocket", BulletCreator.LoadBull("Rocket"));
			BulletCreator.bulletStats.Add("MiniRocket", BulletCreator.LoadBull("MiniRocket"));
			BulletCreator.bulletStats.Add("HeavyRocket", BulletCreator.LoadBull("HeavyRocket"));
			BulletCreator.bulletStats.Add("Flare", BulletCreator.LoadBull("Flare"));
			BulletCreator.bulletStats.Add("Nuke", BulletCreator.LoadBull("Nuke"));
			BulletCreator.bulletStats.Add("DemonMeteor", BulletCreator.LoadBull("DemonMeteor"));
			BulletCreator.bulletStats.Add("MiniDemonMeteor", BulletCreator.LoadBull("MiniDemonMeteor"));
			BulletCreator.bulletStats.Add("Kernal", BulletCreator.LoadBull("Kernal"));
			BulletCreator.bulletStats.Add("ButteredKernal", BulletCreator.LoadBull("ButteredKernal"));
			BulletCreator.bulletStats.Add("Flame", BulletCreator.LoadBull("Flame"));
			BulletCreator.bulletStats.Add("FlameLong", BulletCreator.LoadBull("FlameLong"));
			BulletCreator.bulletStats.Add("Bacon", BulletCreator.LoadBull("Bacon"));
			BulletCreator.bulletStats.Add("CookedBacon", BulletCreator.LoadBull("CookedBacon"));
			BulletCreator.bulletStats.Add("SpinningBlade", BulletCreator.LoadBull("SpinningBlade"));
			BulletCreator.bulletStats.Add("SpinningBladeLong", BulletCreator.LoadBull("SpinningBladeLong"));
			BulletCreator.bulletStats.Add("SpinningBladeLarge", BulletCreator.LoadBull("SpinningBladeLarge"));
			BulletCreator.bulletStats.Add("Unicorn", BulletCreator.LoadBull("Unicorn"));
			BulletCreator.bulletStats.Add("NormalShort", BulletCreator.LoadBull("NormalShort"));
			BulletCreator.bulletStats.Add("FrostArrow", BulletCreator.LoadBull("FrostArrow"));
			BulletCreator.bulletStats.Add("Potato", BulletCreator.LoadBull("Potato"));
			BulletCreator.bulletStats.Add("OrbitalStrike", BulletCreator.LoadBull("OrbitalStrike"));
			BulletCreator.bulletStats.Add("CottonBall", BulletCreator.LoadBull("CottonBall"));
			BulletCreator.bulletStats.Add("NormalExplosive", BulletCreator.LoadBull("NormalExplosive"));
			BulletCreator.bulletStats.Add("MachineTurret", BulletCreator.LoadBull("MachineTurret"));
			BulletCreator.bulletStats.Add("Rubber", BulletCreator.LoadBull("Rubber"));
			BulletCreator.bulletStats.Add("HeavyRubber", BulletCreator.LoadBull("HeavyRubber"));
			BulletCreator.bulletStats.Add("Text", BulletCreator.LoadBull("Text"));
			BulletCreator.bulletStats.Add("Magic", BulletCreator.LoadBull("Magic"));
			BulletCreator.bulletStats.Add("Spit", BulletCreator.LoadBull("Spit"));
			BulletCreator.bulletStats.Add("Bomb", BulletCreator.LoadBull("Bomb"));
			Terminal.WriteMessage("---------------------------------------------", MessageType.SAVELOAD);
			Terminal.WriteMessage("Loading Bullets Completed", MessageType.SAVELOAD);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x000337B4 File Offset: 0x000319B4
		public static void LoadProtoBullets()
		{
			List<BulletStats> list;
			XMLSaverLoader.LoadProto<List<BulletStats>>(".\\Data\\BulletList.bin", out list);
			BulletCreator.bulletStats = new Dictionary<string, BulletStats>();
			foreach (BulletStats bulletStats in list)
			{
				string text = bulletStats.Name.Replace(".bul", "");
				BulletCreator.bulletStats.Add(text, bulletStats);
				BulletCreator.UID_Bullets.Add(BulletCreator.uid, text);
				BulletCreator.UID_BulletsRev.Add(text, BulletCreator.uid);
				BulletCreator.uid++;
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00033860 File Offset: 0x00031A60
		public static void LoadAllBulletsInFolder()
		{
			if (BulletCreator.PROTO)
			{
				BulletCreator.LoadProtoBullets();
				return;
			}
			BulletCreator.bulletStats = new Dictionary<string, BulletStats>();
			List<string> list = new List<string>();
			foreach (FileInfo fileInfo in new DirectoryInfo(".\\Data\\Bullets\\").GetFiles())
			{
				list.Add(fileInfo.Name);
			}
			Console.WriteLine("Loading bullets...");
			foreach (string text in list)
			{
				string text2 = text.Replace(".bul", "");
				Console.WriteLine("Loading bullet: " + text2);
				BulletCreator.bulletStats.Add(text2, BulletCreator.LoadBull(text2));
				BulletCreator.UID_Bullets.Add(BulletCreator.uid, text2);
				BulletCreator.UID_BulletsRev.Add(text2, BulletCreator.uid);
				BulletCreator.uid++;
			}
			Console.WriteLine("Done Loading bullets.");
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0003396C File Offset: 0x00031B6C
		public static BulletStats GetBullet(string type)
		{
			return BulletCreator.bulletStats[type];
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0003397C File Offset: 0x00031B7C
		private static BulletStats LoadBull(string bullName)
		{
			BulletStats result;
			XMLSaverLoader.LoadObject<BulletStats>(".\\Data\\Bullets\\" + bullName + ".bul", out result);
			Terminal.WriteMessage("BULLET: |" + bullName + "| Loaded.", MessageType.SAVELOAD);
			return result;
		}

		// Token: 0x040006B3 RID: 1715
		public static Queue<GameObject> Bullets = new Queue<GameObject>();

		// Token: 0x040006B4 RID: 1716
		public static Dictionary<int, string> UID_Bullets = new Dictionary<int, string>();

		// Token: 0x040006B5 RID: 1717
		public static Dictionary<string, int> UID_BulletsRev = new Dictionary<string, int>();

		// Token: 0x040006B6 RID: 1718
		private static int uid = 0;

		// Token: 0x040006B7 RID: 1719
		private static int COUNT = 250;

		// Token: 0x040006B8 RID: 1720
		private static List<GameObject> objList;

		// Token: 0x040006B9 RID: 1721
		private static int startIndex = 0;

		// Token: 0x040006BA RID: 1722
		private static Dictionary<string, BulletStats> bulletStats = new Dictionary<string, BulletStats>();

		// Token: 0x040006BB RID: 1723
		private static bool PROTO = true;

		// Token: 0x040006BC RID: 1724
		private static List<int> PendingBulletUIDs = new List<int>();
	}
}
