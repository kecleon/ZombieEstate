using System;
using ProtoBuf;

namespace ZombieEstate2
{
	// Token: 0x02000126 RID: 294
	[ProtoContract]
	public struct VertexWrapper
	{
		// Token: 0x04000901 RID: 2305
		[ProtoMember(1)]
		public Vector3Wrapper vertexPosition;

		// Token: 0x04000902 RID: 2306
		[ProtoMember(2)]
		public Vector2Wrapper vertexTexCoord;

		// Token: 0x04000903 RID: 2307
		[ProtoMember(3)]
		public Vector2Wrapper vertexLightTexCoord;
	}
}
