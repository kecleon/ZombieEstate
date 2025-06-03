using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001B7 RID: 439
	[ProtoContract]
	public class Msg_DropPickedUp : NetPayload
	{
		// Token: 0x04000C9D RID: 3229
		[ProtoMember(1)]
		public ushort DropUID;

		// Token: 0x04000C9E RID: 3230
		[ProtoMember(2)]
		public ushort PickUpperUID;
	}
}
