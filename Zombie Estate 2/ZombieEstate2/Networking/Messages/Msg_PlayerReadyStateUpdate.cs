using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001BD RID: 445
	[ProtoContract]
	public class Msg_PlayerReadyStateUpdate : NetPayload
	{
		// Token: 0x04000CB4 RID: 3252
		[ProtoMember(1)]
		public bool READY;

		// Token: 0x04000CB5 RID: 3253
		[ProtoMember(2)]
		public byte Index;
	}
}
