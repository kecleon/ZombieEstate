using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001BF RID: 447
	[ProtoContract]
	public class Msg_SpawnZombie : NetPayload
	{
		// Token: 0x04000CBB RID: 3259
		[ProtoMember(1)]
		public float XPosition;

		// Token: 0x04000CBC RID: 3260
		[ProtoMember(2)]
		public float ZPosition;

		// Token: 0x04000CBD RID: 3261
		[ProtoMember(3)]
		public byte ZombieType;

		// Token: 0x04000CBE RID: 3262
		[ProtoMember(4)]
		public float DiffMod;

		// Token: 0x04000CBF RID: 3263
		[ProtoMember(5)]
		public ushort UID;

		// Token: 0x04000CC0 RID: 3264
		[ProtoMember(6)]
		public ushort DropUID;

		// Token: 0x04000CC1 RID: 3265
		[ProtoMember(7)]
		public int Seed;

		// Token: 0x04000CC2 RID: 3266
		[ProtoMember(8)]
		public ushort[] Special;
	}
}
