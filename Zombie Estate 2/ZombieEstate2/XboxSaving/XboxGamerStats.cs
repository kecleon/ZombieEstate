using System;
using System.Collections.Generic;
using ProtoBuf;

namespace ZombieEstate2.XboxSaving
{
	// Token: 0x0200014A RID: 330
	[ProtoContract]
	public class XboxGamerStats
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x00052ABF File Offset: 0x00050CBF
		// (set) Token: 0x06000A1D RID: 2589 RVA: 0x00052AC7 File Offset: 0x00050CC7
		public int Points
		{
			get
			{
				return this.mPoints;
			}
			set
			{
				this.mPoints = value;
				if (this.mPoints > 9999)
				{
					this.mPoints = 9999;
				}
			}
		}

		// Token: 0x04000A98 RID: 2712
		[ProtoMember(1)]
		public string GamerName = "";

		// Token: 0x04000A99 RID: 2713
		[ProtoMember(2)]
		public List<string> UnlockedCharacters = new List<string>();

		// Token: 0x04000A9A RID: 2714
		[ProtoMember(3)]
		public List<string> UnlockedHats = new List<string>();

		// Token: 0x04000A9B RID: 2715
		[ProtoMember(4)]
		public int mPoints;

		// Token: 0x04000A9C RID: 2716
		[ProtoMember(5)]
		public int Hash;
	}
}
