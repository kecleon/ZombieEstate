using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001C3 RID: 451
	[ProtoContract]
	public class Msg_SpawnEffect : NetPayload
	{
		// Token: 0x04000CD1 RID: 3281
		[ProtoMember(1)]
		public byte Type;

		// Token: 0x04000CD2 RID: 3282
		[ProtoMember(2)]
		public float PositionX;

		// Token: 0x04000CD3 RID: 3283
		[ProtoMember(3)]
		public float PositionZ;

		// Token: 0x04000CD4 RID: 3284
		[ProtoMember(4)]
		public ushort AttackerUID;
	}
}
