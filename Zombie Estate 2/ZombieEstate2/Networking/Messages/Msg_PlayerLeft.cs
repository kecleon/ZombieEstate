using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001C5 RID: 453
	[ProtoContract]
	public class Msg_PlayerLeft : NetPayload
	{
		// Token: 0x04000CD6 RID: 3286
		[ProtoMember(1)]
		public int Index;
	}
}
