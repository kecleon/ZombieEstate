using System;

namespace WG
{
	// Token: 0x02000005 RID: 5
	public class WGException : Exception
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000026A8 File Offset: 0x000008A8
		public WGException()
		{
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000026B0 File Offset: 0x000008B0
		public WGException(string message) : base(message)
		{
		}
	}
}
