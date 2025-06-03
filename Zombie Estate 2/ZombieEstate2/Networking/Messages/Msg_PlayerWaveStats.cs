using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001BE RID: 446
	[ProtoContract]
	public class Msg_PlayerWaveStats : NetPayload
	{
		// Token: 0x04000CB6 RID: 3254
		[ProtoMember(1)]
		public float DamageDealt;

		// Token: 0x04000CB7 RID: 3255
		[ProtoMember(2)]
		public float DamageTaken;

		// Token: 0x04000CB8 RID: 3256
		[ProtoMember(3)]
		public float MinionDamageDealt;

		// Token: 0x04000CB9 RID: 3257
		[ProtoMember(4)]
		public float HealingDone;

		// Token: 0x04000CBA RID: 3258
		[ProtoMember(5)]
		public byte PlayerIndex;
	}
}
