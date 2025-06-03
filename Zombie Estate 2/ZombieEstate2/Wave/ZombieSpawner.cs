using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2.Wave
{
	// Token: 0x02000159 RID: 345
	public static class ZombieSpawner
	{
		// Token: 0x06000A70 RID: 2672 RVA: 0x00054C26 File Offset: 0x00052E26
		public static void SpawnZombie(float minDis, float maxDis, Dictionary<ZombieType, int> weights)
		{
			ZombieSpawner.SpawnZombie(minDis, maxDis, weights, 1f);
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00054C38 File Offset: 0x00052E38
		public static void SpawnZombie(float minDis, float maxDis, Dictionary<ZombieType, int> weights, float diffMod)
		{
			ZombieSpawner.Weights = weights;
			ZombieSpawner.MinDistance = minDis;
			ZombieSpawner.MaxDistance = maxDis;
			Vector2 spawnPosition = ZombieSpawner.GetSpawnPosition();
			if (spawnPosition == Vector2.Zero)
			{
				Terminal.WriteMessage("Error: Spawn Zombie failed to find valid location.", MessageType.ERROR);
				return;
			}
			ZombieType zombieType = ZombieSpawner.PickRandomZombieType();
			ushort machineUID = NetworkManager.GetMachineUID();
			ushort machineUID2 = NetworkManager.GetMachineUID();
			int seed = Global.rand.Next();
			Msg_SpawnZombie msg_SpawnZombie = null;
			if (NetworkManager.AmIHost)
			{
				NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.SpawnZombie);
				msg_SpawnZombie = new Msg_SpawnZombie();
				msg_SpawnZombie.DiffMod = diffMod;
				msg_SpawnZombie.XPosition = spawnPosition.X;
				msg_SpawnZombie.ZPosition = spawnPosition.Y;
				msg_SpawnZombie.ZombieType = (byte)zombieType;
				msg_SpawnZombie.UID = machineUID;
				msg_SpawnZombie.DropUID = machineUID2;
				msg_SpawnZombie.Seed = seed;
				if (zombieType == ZombieType.BrainZombie)
				{
					msg_SpawnZombie.Special = new ushort[3];
					msg_SpawnZombie.Special[0] = NetworkManager.GetMachineUID();
					msg_SpawnZombie.Special[1] = NetworkManager.GetMachineUID();
					msg_SpawnZombie.Special[2] = NetworkManager.GetMachineUID();
				}
				netMessage.Payload = msg_SpawnZombie;
				NetworkManager.SendMessage(netMessage, SendType.ReliableBuffered);
			}
			Zombie zombie = ZombieSpawner.GenerateZombie(diffMod, spawnPosition, zombieType, machineUID, machineUID2, seed);
			if (zombie is BrainZombie && msg_SpawnZombie != null)
			{
				(zombie as BrainZombie).InitiateBrianUIDs(msg_SpawnZombie.Special[0], msg_SpawnZombie.Special[1], msg_SpawnZombie.Special[2]);
			}
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00054D80 File Offset: 0x00052F80
		public static Zombie GenerateZombie(float diffMod, Vector2 pos, ZombieType type, ushort UID, ushort dropUID, int seed)
		{
			Zombie zombieClass = ZombieSpawner.GetZombieClass(type);
			zombieClass.Position.X = pos.X;
			zombieClass.Position.Z = pos.Y;
			zombieClass.Position.Y = -1f;
			zombieClass.InitiateNet(UID, dropUID, seed);
			zombieClass.ActivateObject(zombieClass.Position, zombieClass.TextureCoord);
			zombieClass.IncreaseStrength(diffMod);
			Global.MasterCache.CreateObject(zombieClass);
			return zombieClass;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00054DF8 File Offset: 0x00052FF8
		private static Zombie GetZombieClass(ZombieType type)
		{
			switch (type)
			{
			case ZombieType.NormalZombie:
				return new Zombie(ZombieType.NormalZombie);
			case ZombieType.Skeleton:
				return new Zombie(ZombieType.Skeleton);
			case ZombieType.SludgeMonster:
				return new SludgeMonster();
			case ZombieType.Hazmat:
				return new Hazmat();
			case ZombieType.Banshee:
				return new Banshee();
			case ZombieType.Goliath:
				return new Goliath();
			case ZombieType.RobBurglar:
				return new RobBurglar();
			case ZombieType.Yardsman:
				return new Yardsman();
			case ZombieType.EstateOwner:
				return new EstateDemon();
			case ZombieType.DrZombie:
				return new DrZombie();
			case ZombieType.Blob:
				return new Blob();
			case ZombieType.Plant:
				return new BigPlant();
			case ZombieType.Clown:
				return new Clown();
			case ZombieType.FireWitch:
				return new FireWitch();
			case ZombieType.IceGolem:
				return new IceGolem();
			case ZombieType.Glooper:
				return new Glooper();
			case ZombieType.BrainZombie:
				return new BrainZombie();
			case ZombieType.Gardner:
				return new Gardner();
			}
			throw new Exception("Unknown net zombie type: " + type.ToString());
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00054F2C File Offset: 0x0005312C
		private static Vector2 GetSpawnPosition()
		{
			Tile tileAtLocation = ZombieSpawner.sector.GetTileAtLocation(ZombieSpawner.GetPlayerAvgPosition());
			if (tileAtLocation == null)
			{
				Terminal.WriteMessage("Error: Player average position returned no tile!", MessageType.ERROR);
				return ZombieSpawner.spawnPos;
			}
			for (int i = 0; i < 100; i++)
			{
				int index = Global.rand.Next(0, Global.WaveMaster.SpawnTiles.Count);
				Tile tile = Global.WaveMaster.SpawnTiles[index];
				if (ZombieSpawner.WithinThreshold(tileAtLocation, tile))
				{
					ZombieSpawner.spawnPos.X = (float)tile.x;
					ZombieSpawner.spawnPos.Y = (float)tile.y;
					return ZombieSpawner.spawnPos;
				}
			}
			Terminal.WriteMessage("Error: Finding a tile failed! COUNT exceeded 100.", MessageType.ERROR);
			return ZombieSpawner.spawnPos;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00054FD7 File Offset: 0x000531D7
		private static Vector3 GetPlayerAvgPosition()
		{
			return Camera.AveragePlayerPos();
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00054FE0 File Offset: 0x000531E0
		private static bool WithinThreshold(Tile playerPos, Tile checkTile)
		{
			return !VerchickMath.WithinDistance(new Vector2((float)checkTile.x, (float)checkTile.y), new Vector2((float)playerPos.x, (float)playerPos.y), ZombieSpawner.MinDistance) && VerchickMath.WithinDistance(new Vector2((float)checkTile.x, (float)checkTile.y), new Vector2((float)playerPos.x, (float)playerPos.y), ZombieSpawner.MaxDistance);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00055058 File Offset: 0x00053258
		private static ZombieType PickRandomZombieType()
		{
			if (ZombieSpawner.Weights == null)
			{
				Terminal.WriteMessage("ERROR: Weights was null!", MessageType.ERROR);
				return ZombieType.NormalZombie;
			}
			int num = Global.rand.Next(0, 100);
			int num2 = 0;
			foreach (KeyValuePair<ZombieType, int> keyValuePair in from key in ZombieSpawner.Weights
			orderby key.Value
			select key)
			{
				num2 += keyValuePair.Value;
				if (num <= num2)
				{
					return keyValuePair.Key;
				}
			}
			Terminal.WriteMessage("ERROR: No zombie type was selected!", MessageType.ERROR);
			return ZombieType.NormalZombie;
		}

		// Token: 0x04000B25 RID: 2853
		public static Sector sector;

		// Token: 0x04000B26 RID: 2854
		private static float MinDistance = 0f;

		// Token: 0x04000B27 RID: 2855
		private static float MaxDistance = 1f;

		// Token: 0x04000B28 RID: 2856
		private static Vector2 spawnPos = new Vector2(0f, 0f);

		// Token: 0x04000B29 RID: 2857
		private static Dictionary<ZombieType, int> Weights;
	}
}
