using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001B3 RID: 435
	[ProtoContract]
	[ProtoInclude(400, typeof(Msg_GameObjectUpdate))]
	[ProtoInclude(402, typeof(Msg_GunListUpdate))]
	[ProtoInclude(403, typeof(Msg_GunFired))]
	[ProtoInclude(404, typeof(Msg_WaveStateUpdate))]
	[ProtoInclude(405, typeof(Msg_PlayerReadyStateUpdate))]
	[ProtoInclude(406, typeof(Msg_SpawnZombie))]
	[ProtoInclude(407, typeof(Msg_ZombieUpdate))]
	[ProtoInclude(408, typeof(Msg_ZombieKilled))]
	[ProtoInclude(409, typeof(Msg_BulletDestroyed))]
	[ProtoInclude(410, typeof(Msg_ZombieAttack))]
	[ProtoInclude(411, typeof(Msg_PlayerWaveStats))]
	[ProtoInclude(412, typeof(Msg_DropPickedUp))]
	[ProtoInclude(413, typeof(Msg_GenericUID))]
	[ProtoInclude(414, typeof(Msg_PlayerHUDUpdate))]
	[ProtoInclude(415, typeof(Msg_SpawnEffect))]
	[ProtoInclude(416, typeof(Msg_ChatMsg))]
	[ProtoInclude(417, typeof(Msg_PlayerLeft))]
	[ProtoInclude(418, typeof(Msg_Dance))]
	public class NetPayload
	{
	}
}
