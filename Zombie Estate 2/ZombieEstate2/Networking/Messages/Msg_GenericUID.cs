using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001B8 RID: 440
	[ProtoContract]
	public class Msg_GenericUID : NetPayload
	{
		// Token: 0x04000C9F RID: 3231
		[ProtoMember(1)]
		public ushort UID;
	}
}
