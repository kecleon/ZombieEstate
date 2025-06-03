using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001C2 RID: 450
	[ProtoContract]
	public class Msg_ZombieKilled : NetPayload
	{
		// Token: 0x04000CCF RID: 3279
		[ProtoMember(1)]
		public ushort UID;

		// Token: 0x04000CD0 RID: 3280
		[ProtoMember(2)]
		public ushort KillerUID;
	}
}
