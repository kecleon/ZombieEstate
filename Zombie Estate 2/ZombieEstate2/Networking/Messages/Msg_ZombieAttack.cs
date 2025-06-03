using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001C0 RID: 448
	[ProtoContract]
	public class Msg_ZombieAttack : NetPayload
	{
		// Token: 0x04000CC3 RID: 3267
		[ProtoMember(1)]
		public float PositionX;

		// Token: 0x04000CC4 RID: 3268
		[ProtoMember(2)]
		public float PositionZ;

		// Token: 0x04000CC5 RID: 3269
		[ProtoMember(3)]
		public ushort UID;

		// Token: 0x04000CC6 RID: 3270
		[ProtoMember(4)]
		public ushort TargetUID;
	}
}
