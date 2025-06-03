using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001B5 RID: 437
	[ProtoContract]
	public class Msg_PlayerUpdate : Msg_GameObjectUpdate
	{
		// Token: 0x04000C8E RID: 3214
		[ProtoMember(1)]
		public bool HealthChanged;

		// Token: 0x04000C8F RID: 3215
		[ProtoMember(2)]
		public ushort Health;
	}
}
