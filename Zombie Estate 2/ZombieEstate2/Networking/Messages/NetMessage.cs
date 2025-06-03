using System;
using System.IO;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001B2 RID: 434
	public class NetMessage
	{
		// Token: 0x06000C2F RID: 3119 RVA: 0x0006432B File Offset: 0x0006252B
		protected NetMessage()
		{
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00064340 File Offset: 0x00062540
		private byte[] SerializePayload()
		{
			if (this.Payload == null)
			{
				return null;
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Serializer.Serialize<NetPayload>(memoryStream, this.Payload);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00064390 File Offset: 0x00062590
		public byte[] ToByteArray()
		{
			byte[] array = this.SerializePayload();
			int num = (array == null) ? 0 : array.Length;
			int num2 = (this.Type == NetMessageType.Data || this.Type == NetMessageType.PlayerJump) ? 2 : 0;
			byte[] array2 = new byte[1 + num2 + num];
			array2[0] = (byte)this.Type;
			int destinationIndex = 1;
			if (this.Type == NetMessageType.Data || this.Type == NetMessageType.PlayerJump)
			{
				byte[] bytes = BitConverter.GetBytes(this.UID);
				array2[1] = bytes[0];
				array2[2] = bytes[1];
				destinationIndex = 3;
			}
			if (array != null)
			{
				Array.Copy(array, 0, array2, destinationIndex, num);
			}
			return array2;
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x0006441D File Offset: 0x0006261D
		public static NetMessage GetNetMessage(NetMessageType type)
		{
			return new NetMessage
			{
				Type = type
			};
		}

		// Token: 0x04000C84 RID: 3204
		public static readonly ushort NULL_UID = ushort.MaxValue;

		// Token: 0x04000C85 RID: 3205
		public NetMessageType Type;

		// Token: 0x04000C86 RID: 3206
		public ushort UID = NetMessage.NULL_UID;

		// Token: 0x04000C87 RID: 3207
		public NetPayload Payload;
	}
}
