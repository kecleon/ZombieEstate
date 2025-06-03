using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000D0 RID: 208
	internal class StoreDialog
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x00029650 File Offset: 0x00027850
		public StoreDialog(string message, bool yesOrNo)
		{
			if (StoreDialog.BaseDialogTex == null)
			{
				StoreDialog.BaseDialogTex = Global.Content.Load<Texture2D>("Dialogs\\BaseDialog");
			}
			int num = 0;
			if (PCButton.ButtonTex != null)
			{
				num = PCButton.ButtonTex.Width / 2;
			}
			this.YesOrNo = yesOrNo;
			this.Message = message;
			if (this.YesOrNo)
			{
				this.YesButton = new PCButton("Yes", Color.LightGreen, new Vector2((float)(Global.ScreenRect.Width / 2 - num - 92), (float)(Global.ScreenRect.Height / 2 + 64)));
				this.NoButton = new PCButton("No", Color.Red, new Vector2((float)(Global.ScreenRect.Width / 2 - num + 92), (float)(Global.ScreenRect.Height / 2 + 64)));
			}
			else
			{
				this.YesButton = new PCButton("Ok", Color.LightGray, new Vector2((float)(Global.ScreenRect.Width / 2 - num), (float)(Global.ScreenRect.Height / 2 + 64)));
			}
			this.Position = new Vector2((float)((Global.ScreenRect.Width - StoreDialog.BaseDialogTex.Width) / 2), (float)((Global.ScreenRect.Height - StoreDialog.BaseDialogTex.Height) / 2));
			this.box = new ScrollBox(message, new Rectangle((int)this.Position.X + 16, (int)this.Position.Y + 16, StoreDialog.BaseDialogTex.Width - 32, StoreDialog.BaseDialogTex.Height - 32), Global.StoreFontBig, null, Color.White);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x000297EC File Offset: 0x000279EC
		public void Update()
		{
			if (this.CLOSED)
			{
				return;
			}
			this.YesButton.Update();
			if (this.CLOSED)
			{
				return;
			}
			if (this.YesOrNo)
			{
				this.NoButton.Update();
			}
			if (this.CLOSED)
			{
				return;
			}
			this.box.Update();
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00029840 File Offset: 0x00027A40
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(StoreDialog.BaseDialogTex, this.Position, Color.White);
			this.YesButton.Draw(spriteBatch);
			if (this.YesOrNo)
			{
				this.NoButton.Draw(spriteBatch);
			}
			this.box.Draw(spriteBatch);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0002988F File Offset: 0x00027A8F
		public void CloseDialog()
		{
			this.YesButton = null;
			this.NoButton = null;
			this.CLOSED = true;
		}

		// Token: 0x04000562 RID: 1378
		private bool YesOrNo;

		// Token: 0x04000563 RID: 1379
		private string Message;

		// Token: 0x04000564 RID: 1380
		public PCButton YesButton;

		// Token: 0x04000565 RID: 1381
		public PCButton NoButton;

		// Token: 0x04000566 RID: 1382
		public static Texture2D BaseDialogTex;

		// Token: 0x04000567 RID: 1383
		private Vector2 Position;

		// Token: 0x04000568 RID: 1384
		private ScrollBox box;

		// Token: 0x04000569 RID: 1385
		public bool CLOSED;
	}
}
