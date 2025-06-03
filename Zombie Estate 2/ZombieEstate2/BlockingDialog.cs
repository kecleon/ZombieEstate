using System;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200001C RID: 28
	internal abstract class BlockingDialog
	{
		// Token: 0x060000BC RID: 188 RVA: 0x000026B9 File Offset: 0x000008B9
		public BlockingDialog()
		{
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Update()
		{
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Draw(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005D3F File Offset: 0x00003F3F
		public virtual bool DoneShowing()
		{
			return true;
		}
	}
}
