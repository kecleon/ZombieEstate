using System;
using Microsoft.Xna.Framework;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001B4 RID: 436
	[ProtoContract]
	public class Msg_GameObjectUpdate : NetPayload
	{
		// Token: 0x06000C35 RID: 3125 RVA: 0x00064438 File Offset: 0x00062638
		public static void FillIn(Msg_GameObjectUpdate payload, Vector3 pos, Vector3 prevPos, bool fullSync)
		{
			if (!fullSync)
			{
				if (prevPos.X != pos.X)
				{
					payload.PositionXChanged = true;
					payload.PositionX = pos.X;
				}
				if (prevPos.Z != pos.Z)
				{
					payload.PositionZChanged = true;
					payload.PositionZ = pos.Z;
					return;
				}
			}
			else
			{
				payload.PositionXChanged = true;
				payload.PositionX = pos.X;
				payload.PositionZChanged = true;
				payload.PositionZ = pos.Z;
			}
		}

		// Token: 0x04000C88 RID: 3208
		[ProtoMember(1)]
		public bool PositionXChanged;

		// Token: 0x04000C89 RID: 3209
		[ProtoMember(3)]
		public bool PositionZChanged;

		// Token: 0x04000C8A RID: 3210
		[ProtoMember(4)]
		public float PositionX;

		// Token: 0x04000C8B RID: 3211
		[ProtoMember(6)]
		public float PositionZ;

		// Token: 0x04000C8C RID: 3212
		[ProtoMember(7)]
		public bool MovementXChanged;

		// Token: 0x04000C8D RID: 3213
		[ProtoMember(8)]
		public bool MovementYChanged;
	}
}
