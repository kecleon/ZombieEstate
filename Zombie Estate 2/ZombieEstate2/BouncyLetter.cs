using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x020000DA RID: 218
	internal class BouncyLetter : BouncyObject
	{
		// Token: 0x060005B5 RID: 1461 RVA: 0x0002B75C File Offset: 0x0002995C
		public BouncyLetter(char c, int startX, int startY, int endY, float delay, int scale) : base(Global.Alphabet2, startX, startY, endY, 50 * scale, 80 * scale, new Rectangle((int)((float)(c - 'A') * 50.4f) + 1, 0, 50, 80), delay)
		{
		}
	}
}
