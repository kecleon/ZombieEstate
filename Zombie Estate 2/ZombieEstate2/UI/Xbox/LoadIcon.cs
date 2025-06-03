using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.UI.Xbox
{
	// Token: 0x0200015C RID: 348
	public class LoadIcon
	{
		// Token: 0x06000A8B RID: 2699 RVA: 0x00055F93 File Offset: 0x00054193
		public LoadIcon(Rectangle pos)
		{
			this.mPosition = pos;
			this.x = 0;
			this.origX = 0;
			this.y = 58;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00055FB8 File Offset: 0x000541B8
		public void Update(float elapsed)
		{
			this.mElapsed += Global.REAL_GAME_TIME;
			if (this.mElapsed > 0.1f)
			{
				this.mElapsed = 0f;
				this.x++;
				if (this.x == this.origX + 10)
				{
					this.x = this.origX;
				}
			}
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0005601C File Offset: 0x0005421C
		public void Draw(SpriteBatch spriteBatch)
		{
			Rectangle texRectange = Global.GetTexRectange(this.x, this.y);
			spriteBatch.Draw(Global.MasterTexture, this.mPosition, new Rectangle?(texRectange), Color.White);
		}

		// Token: 0x04000B49 RID: 2889
		private int origX;

		// Token: 0x04000B4A RID: 2890
		private int x;

		// Token: 0x04000B4B RID: 2891
		private int y;

		// Token: 0x04000B4C RID: 2892
		private Rectangle mPosition;

		// Token: 0x04000B4D RID: 2893
		private float mElapsed;
	}
}
