using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.Enemies.EnemyAbilities;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001C8 RID: 456
	public static class SpawnEffectManager
	{
		// Token: 0x06000C4B RID: 3147 RVA: 0x000646BC File Offset: 0x000628BC
		public static void Spawn(Msg_SpawnEffect payload)
		{
			Vector3 pos = new Vector3(payload.PositionX, 0.2f, payload.PositionZ);
			if (NetworkManager.NetObjects.ContainsKey(payload.AttackerUID))
			{
				Shootable shootable = NetworkManager.NetObjects[payload.AttackerUID] as Shootable;
				if (payload.Type == 0)
				{
					FirePillar obj = new FirePillar(pos, shootable);
					Global.MasterCache.CreateObject(obj);
				}
				if (payload.Type == 1)
				{
					IcePillar obj2 = new IcePillar(pos, shootable);
					Global.MasterCache.CreateObject(obj2);
				}
				if (payload.Type == 2)
				{
					Gardner.SpawnVines(pos, shootable as Zombie);
				}
				if (payload.Type == 3)
				{
					Clown.SpawnConfetti(pos, shootable as Zombie);
				}
				return;
			}
			Terminal.WriteMessage("Spawned effect with no attacker found.", MessageType.ERROR);
		}
	}
}
