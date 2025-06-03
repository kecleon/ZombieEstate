using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001C4 RID: 452
	[ProtoContract]
	public class Msg_ChatMsg : NetPayload
	{
		// Token: 0x04000CD5 RID: 3285
		[ProtoMember(1)]
		public string Message;
	}
}
