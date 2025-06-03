using System;

namespace EventInputNamespace
{
	// Token: 0x02000007 RID: 7
	public class CharacterEventArgs : EventArgs
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000026C1 File Offset: 0x000008C1
		public CharacterEventArgs(char character, int lParam)
		{
			this.character = character;
			this.lParam = lParam;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000026D7 File Offset: 0x000008D7
		public char Character
		{
			get
			{
				return this.character;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000026DF File Offset: 0x000008DF
		public int Param
		{
			get
			{
				return this.lParam;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000026E7 File Offset: 0x000008E7
		public int RepeatCount
		{
			get
			{
				return this.lParam & 65535;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000026F5 File Offset: 0x000008F5
		public bool ExtendedKey
		{
			get
			{
				return (this.lParam & 16777216) > 0;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002706 File Offset: 0x00000906
		public bool AltPressed
		{
			get
			{
				return (this.lParam & 536870912) > 0;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002717 File Offset: 0x00000917
		public bool PreviousState
		{
			get
			{
				return (this.lParam & 1073741824) > 0;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002728 File Offset: 0x00000928
		public bool TransitionState
		{
			get
			{
				return (this.lParam & int.MinValue) > 0;
			}
		}

		// Token: 0x04000009 RID: 9
		private readonly char character;

		// Token: 0x0400000A RID: 10
		private readonly int lParam;
	}
}
