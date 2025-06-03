using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000DC RID: 220
	internal class BouncyText
	{
		// Token: 0x060005C2 RID: 1474 RVA: 0x0002BC74 File Offset: 0x00029E74
		public BouncyText(List<string> lines, int centerX, int centerY, int scale)
		{
			int num = scale * 50;
			int num2 = scale * 100;
			this.Letters = new List<BouncyLetter>();
			int num3 = 0;
			int num4 = 0;
			foreach (string text in lines)
			{
				int endY = centerY + num2 * num3;
				char[] array = text.ToCharArray();
				int num5 = centerX - array.Length * num / 2;
				for (int i = 0; i < array.Length; i++)
				{
					BouncyLetter item;
					if (num3 > 0)
					{
						item = new BouncyLetter(array[i], num5 + i * num, -num2, endY, (float)num4 * 0.45f, scale);
					}
					else
					{
						item = new BouncyLetter(array[i], num5 + i * num, -num2, endY, (float)num4 * 0.25f + (float)i * 0.25f, scale);
					}
					this.Letters.Add(item);
				}
				num3++;
				num4 += array.Length;
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0002BD80 File Offset: 0x00029F80
		public void Update()
		{
			foreach (BouncyLetter bouncyLetter in this.Letters)
			{
				bouncyLetter.Update(Global.REAL_GAME_TIME);
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0002BDD8 File Offset: 0x00029FD8
		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (BouncyLetter bouncyLetter in this.Letters)
			{
				bouncyLetter.Draw(spriteBatch);
			}
		}

		// Token: 0x040005B3 RID: 1459
		private List<BouncyLetter> Letters;
	}
}
