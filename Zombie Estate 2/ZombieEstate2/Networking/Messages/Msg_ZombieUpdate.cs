using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001C1 RID: 449
	[ProtoContract]
	public class Msg_ZombieUpdate : NetPayload
	{
		// Token: 0x04000CC7 RID: 3271
		[ProtoMember(1)]
		public bool HealthChanged;

		// Token: 0x04000CC8 RID: 3272
		[ProtoMember(2)]
		public ushort Health;

		// Token: 0x04000CC9 RID: 3273
		[ProtoMember(3)]
		public float MoveToPositionX;

		// Token: 0x04000CCA RID: 3274
		[ProtoMember(4)]
		public float MoveToPositionZ;

		// Token: 0x04000CCB RID: 3275
		[ProtoMember(5)]
		public ushort TargetUID;

		// Token: 0x04000CCC RID: 3276
		[ProtoMember(6)]
		public float PositionX;

		// Token: 0x04000CCD RID: 3277
		[ProtoMember(7)]
		public float PositionZ;

		// Token: 0x04000CCE RID: 3278
		[ProtoMember(9)]
		public bool Misc;
	}
}
