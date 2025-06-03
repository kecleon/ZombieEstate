using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001BC RID: 444
	[ProtoContract]
	public class Msg_WaveStateUpdate : NetPayload
	{
		// Token: 0x04000CB1 RID: 3249
		[ProtoMember(1)]
		public int WaveNumber;

		// Token: 0x04000CB2 RID: 3250
		[ProtoMember(2)]
		public byte WaveState;

		// Token: 0x04000CB3 RID: 3251
		[ProtoMember(3)]
		public int PreSecondsLeft;
	}
}
