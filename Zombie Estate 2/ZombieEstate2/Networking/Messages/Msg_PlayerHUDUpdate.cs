using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001B6 RID: 438
	[ProtoContract]
	public class Msg_PlayerHUDUpdate : NetPayload
	{
		// Token: 0x04000C90 RID: 3216
		[ProtoMember(1)]
		public ushort UID;

		// Token: 0x04000C91 RID: 3217
		[ProtoMember(2)]
		public bool AssaultChanged;

		// Token: 0x04000C92 RID: 3218
		[ProtoMember(3)]
		public bool ShellsChanged;

		// Token: 0x04000C93 RID: 3219
		[ProtoMember(4)]
		public bool HeavyChanged;

		// Token: 0x04000C94 RID: 3220
		[ProtoMember(5)]
		public bool ExplosiveChanged;

		// Token: 0x04000C95 RID: 3221
		[ProtoMember(6)]
		public bool HealthPackChanged;

		// Token: 0x04000C96 RID: 3222
		[ProtoMember(7)]
		public bool MoneyChanged;

		// Token: 0x04000C97 RID: 3223
		[ProtoMember(8)]
		public ushort Assault;

		// Token: 0x04000C98 RID: 3224
		[ProtoMember(9)]
		public ushort Shells;

		// Token: 0x04000C99 RID: 3225
		[ProtoMember(10)]
		public ushort Heavy;

		// Token: 0x04000C9A RID: 3226
		[ProtoMember(11)]
		public ushort Explosive;

		// Token: 0x04000C9B RID: 3227
		[ProtoMember(12)]
		public byte HealthPack;

		// Token: 0x04000C9C RID: 3228
		[ProtoMember(13)]
		public ushort Money;
	}
}
