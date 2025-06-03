using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001BA RID: 442
	[ProtoContract]
	public class Msg_GunFired : NetPayload
	{
		// Token: 0x04000CA4 RID: 3236
		[ProtoMember(1)]
		public short GunUID;

		// Token: 0x04000CA5 RID: 3237
		[ProtoMember(2)]
		public float AimDirX;

		// Token: 0x04000CA6 RID: 3238
		[ProtoMember(3)]
		public float AimDirY;

		// Token: 0x04000CA7 RID: 3239
		[ProtoMember(4)]
		public int Seed;

		// Token: 0x04000CA8 RID: 3240
		[ProtoMember(5)]
		public float FiredPositionX;

		// Token: 0x04000CA9 RID: 3241
		[ProtoMember(6)]
		public float FiredPositionZ;

		// Token: 0x04000CAA RID: 3242
		[ProtoMember(7)]
		public bool Crit;

		// Token: 0x04000CAB RID: 3243
		[ProtoMember(8)]
		public ushort StartUID;

		// Token: 0x04000CAC RID: 3244
		[ProtoMember(9)]
		public byte PlayerIndex;

		// Token: 0x04000CAD RID: 3245
		[ProtoMember(10)]
		public ushort BulletsRemainingInClip;
	}
}
