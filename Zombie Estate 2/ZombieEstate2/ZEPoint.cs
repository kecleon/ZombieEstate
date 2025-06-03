using System;
using System.ComponentModel;
using ProtoBuf;

namespace ZombieEstate2
{
	// Token: 0x02000138 RID: 312
	[TypeConverter(typeof(ZEPointConverter))]
	[ProtoContract]
	public class ZEPoint
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x0004AC18 File Offset: 0x00048E18
		// (set) Token: 0x06000945 RID: 2373 RVA: 0x0004AC20 File Offset: 0x00048E20
		[ProtoMember(1)]
		public int X { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x0004AC29 File Offset: 0x00048E29
		// (set) Token: 0x06000947 RID: 2375 RVA: 0x0004AC31 File Offset: 0x00048E31
		[ProtoMember(2)]
		public int Y { get; set; }

		// Token: 0x06000948 RID: 2376 RVA: 0x000026B9 File Offset: 0x000008B9
		public ZEPoint()
		{
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0004AC3A File Offset: 0x00048E3A
		public ZEPoint(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
