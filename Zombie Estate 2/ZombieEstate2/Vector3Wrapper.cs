using System;
using ProtoBuf;

namespace ZombieEstate2
{
	// Token: 0x02000127 RID: 295
	[ProtoContract]
	public struct Vector3Wrapper
	{
		// Token: 0x04000904 RID: 2308
		[ProtoMember(1)]
		public float X;

		// Token: 0x04000905 RID: 2309
		[ProtoMember(2)]
		public float Y;

		// Token: 0x04000906 RID: 2310
		[ProtoMember(3)]
		public float Z;
	}
}
