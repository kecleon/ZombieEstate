using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001BB RID: 443
	[ProtoContract]
	public class Msg_BulletDestroyed : NetPayload
	{
		// Token: 0x04000CAE RID: 3246
		[ProtoMember(1)]
		public ushort BulletUID;

		// Token: 0x04000CAF RID: 3247
		[ProtoMember(2)]
		public float BulletX;

		// Token: 0x04000CB0 RID: 3248
		[ProtoMember(3)]
		public float BulletZ;
	}
}
