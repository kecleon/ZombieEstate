using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001C6 RID: 454
	[ProtoContract]
	public class Msg_Dance : NetPayload
	{
		// Token: 0x04000CD7 RID: 3287
		[ProtoMember(1)]
		public byte State;

		// Token: 0x04000CD8 RID: 3288
		[ProtoMember(2)]
		public byte Index;
	}
}
