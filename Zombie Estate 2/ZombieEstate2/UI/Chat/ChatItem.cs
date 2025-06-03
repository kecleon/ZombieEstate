using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.UI.Chat
{
	// Token: 0x02000170 RID: 368
	internal class ChatItem
	{
		// Token: 0x06000B27 RID: 2855 RVA: 0x0005C14E File Offset: 0x0005A34E
		public ChatItem(string s, Color c)
		{
			this.Text = s;
			this.Color = c;
		}

		// Token: 0x04000BEC RID: 3052
		public string Text;

		// Token: 0x04000BED RID: 3053
		public Color Color;
	}
}
