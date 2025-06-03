using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.Networking
{
	// Token: 0x020001AD RID: 429
	public class MessageOverlay
	{
		// Token: 0x06000C05 RID: 3077 RVA: 0x00062CC0 File Offset: 0x00060EC0
		public MessageOverlay(string msg, bool persistent)
		{
			this.mMessage = msg;
			this.mPersistent = persistent;
			if (!this.mPersistent)
			{
				this.mTimer = new Timer(4f);
				this.mTimer.IndependentOfTime = true;
				this.mTimer.Start();
			}
			Global.Paused = true;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00062D28 File Offset: 0x00060F28
		public void Update()
		{
			if (!this.mPersistent && this.mTimer.Expired())
			{
				this.ACTIVE = false;
				Global.Paused = false;
				return;
			}
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00062D50 File Offset: 0x00060F50
		public void Draw(SpriteBatch spritebatch)
		{
			spritebatch.Draw(Global.Pixel, Global.ScreenRect, Color.Black * 0.5f);
			Vector2 pos = VerchickMath.CenterText(Global.StoreFontBig, Global.GetScreenCenter(), this.mMessage);
			Shadow.DrawString(this.mMessage, Global.StoreFontBig, pos, 2, Color.White, spritebatch);
		}

		// Token: 0x04000C4C RID: 3148
		private string mMessage = "NULL";

		// Token: 0x04000C4D RID: 3149
		private Timer mTimer;

		// Token: 0x04000C4E RID: 3150
		private bool mPersistent;

		// Token: 0x04000C4F RID: 3151
		public bool ACTIVE = true;
	}
}
