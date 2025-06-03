using System;
using ProtoBuf;

namespace ZombieEstate2
{
	// Token: 0x02000128 RID: 296
	[ProtoContract]
	public struct Vector2Wrapper
	{
		// Token: 0x04000907 RID: 2311
		[ProtoMember(1)]
		public float X;

		// Token: 0x04000908 RID: 2312
		[ProtoMember(2)]
		public float Y;
	}
}
