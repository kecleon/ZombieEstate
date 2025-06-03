using System;
using Microsoft.Xna.Framework.Input;

namespace EventInputNamespace
{
	// Token: 0x02000008 RID: 8
	public class KeyEventArgs : EventArgs
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002739 File Offset: 0x00000939
		public KeyEventArgs(Keys keyCode)
		{
			this.keyCode = keyCode;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002748 File Offset: 0x00000948
		public Keys KeyCode
		{
			get
			{
				return this.keyCode;
			}
		}

		// Token: 0x0400000B RID: 11
		private Keys keyCode;
	}
}
