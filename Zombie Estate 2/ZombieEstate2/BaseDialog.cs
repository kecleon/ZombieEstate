using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200001B RID: 27
	internal class BaseDialog
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x0000586C File Offset: 0x00003A6C
		public static bool AllDialogsClear()
		{
			return BaseDialog.DialogsActive == 0;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005878 File Offset: 0x00003A78
		public BaseDialog(Player parent, DialogLayout layout)
		{
			if (BaseDialog.BaseDialogTex == null)
			{
				BaseDialog.BaseDialogTex = Global.Content.Load<Texture2D>("Dialogs\\BaseDialog");
			}
			this.texture = BaseDialog.BaseDialogTex;
			this.parent = parent;
			this.SetFinalPos(layout);
			this.SetStartPos();
			this.position = new Vector2(this.startPos.X, this.startPos.Y);
			this.swoopTimer.IndependentOfTime = true;
			this.swoopTimer.DeltaDelegate = new Timer.TimerDelegate(this.SwoopIn);
			this.swoopTimer.Start();
			BaseDialog.DialogsActive++;
			this.Text = this.text;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000596C File Offset: 0x00003B6C
		public void Update()
		{
			if (this.parent == null)
			{
				return;
			}
			if (this.Active && this.swoopTimer.Expired() && InputManager.APressed(this.parent.Index))
			{
				this.swoopTimer.Reset();
				this.swoopTimer.DeltaDelegate = new Timer.TimerDelegate(this.SwoopOut);
				this.swoopTimer.Start();
				if (this.DialogResult != null)
				{
					this.DialogResult(DialogChoice.Accept);
				}
				return;
			}
			if (!this.Active && this.swoopTimer.Expired() && InputManager.APressed(this.parent.Index))
			{
				this.swoopTimer.Reset();
				this.swoopTimer.DeltaDelegate = new Timer.TimerDelegate(this.SwoopIn);
				this.swoopTimer.Start();
				this.Active = true;
				BaseDialog.DialogsActive++;
				return;
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005A54 File Offset: 0x00003C54
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.parent == null)
			{
				spriteBatch.Draw(BaseDialog.BaseDialogTex, this.position, Color.White * this.alpha);
				this.DrawText(spriteBatch);
				return;
			}
			spriteBatch.Draw(BaseDialog.BaseDialogTex, this.position, this.parent.HUDColor * this.alpha);
			this.DrawText(spriteBatch);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005AC0 File Offset: 0x00003CC0
		public virtual void DrawText(SpriteBatch spriteBatch)
		{
			Vector2 pos = new Vector2(this.position.X + (float)this.textOffsetX, this.position.Y + (float)this.textOffsetY);
			Shadow.DrawString(this.text, Global.StoreFont, pos, 1, this.textColor, spriteBatch);
		}

		// Token: 0x17000010 RID: 16
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00005B13 File Offset: 0x00003D13
		public string Text
		{
			set
			{
				this.text = VerchickMath.WordWrapWidth(value, this.texture.Width - this.textOffsetX * 2, Global.StoreFont);
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005B3C File Offset: 0x00003D3C
		private void SetFinalPos(DialogLayout layout)
		{
			int num = 464;
			int num2 = 324;
			int width = Global.ScreenRect.Width;
			int height = Global.ScreenRect.Height;
			if (layout == DialogLayout.Center)
			{
				this.finalPosition = default(Vector2);
				this.finalPosition.X = (float)((width - num) / 2);
				this.finalPosition.Y = (float)((height - num2) / 2);
				return;
			}
			int index = this.parent.Index;
			int num3 = 1280 - width;
			int num4 = 720 - height;
			num3 /= 2;
			num4 /= 2;
			this.finalPosition = default(Vector2);
			if (layout == DialogLayout.Corners)
			{
				int num5 = index % 2;
				int num6 = 0;
				if (index > 1)
				{
					num6 = 1;
				}
				this.finalPosition.X = (float)((width - num) / 2 - num / 2 + num * num5);
				this.finalPosition.Y = (float)((height - num2) / 2 - num2 / 2 + num2 * num6);
			}
			this.finalPosition.X = this.finalPosition.X + (float)(num3 + 32);
			this.finalPosition.Y = this.finalPosition.Y + (float)(num4 + 2);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005C48 File Offset: 0x00003E48
		private void SetStartPos()
		{
			if (this.finalPosition.X > 640f)
			{
				this.startPos.X = 1760f;
			}
			else
			{
				this.startPos.X = -480f;
			}
			if (this.finalPosition.Y > 360f)
			{
				this.startPos.Y = 1200f;
				return;
			}
			this.startPos.Y = -480f;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005CBC File Offset: 0x00003EBC
		private void SwoopIn(float delta)
		{
			this.position = Vector2.SmoothStep(this.startPos, this.finalPosition, delta);
			this.alpha = delta;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005CE0 File Offset: 0x00003EE0
		private void SwoopOut(float delta)
		{
			this.position = Vector2.SmoothStep(this.finalPosition, this.startPos, delta);
			this.alpha = 1f - delta;
			if (delta == 1f)
			{
				this.Active = false;
				BaseDialog.DialogsActive--;
			}
		}

		// Token: 0x04000080 RID: 128
		public static Texture2D BaseDialogTex;

		// Token: 0x04000081 RID: 129
		private static int DialogsActive = 0;

		// Token: 0x04000082 RID: 130
		private static float SECONDS = 0.5f;

		// Token: 0x04000083 RID: 131
		public BaseDialog.DialogDelegate DialogResult;

		// Token: 0x04000084 RID: 132
		public Player parent;

		// Token: 0x04000085 RID: 133
		public Vector2 finalPosition;

		// Token: 0x04000086 RID: 134
		private Vector2 startPos;

		// Token: 0x04000087 RID: 135
		public Vector2 position;

		// Token: 0x04000088 RID: 136
		private Timer swoopTimer = new Timer(BaseDialog.SECONDS);

		// Token: 0x04000089 RID: 137
		private bool Active = true;

		// Token: 0x0400008A RID: 138
		public float alpha;

		// Token: 0x0400008B RID: 139
		private int textOffsetX = 16;

		// Token: 0x0400008C RID: 140
		private int textOffsetY = 16;

		// Token: 0x0400008D RID: 141
		private Texture2D texture;

		// Token: 0x0400008E RID: 142
		private string text = "Here is some test text for the dialog box. It should be wrapped properly.";

		// Token: 0x0400008F RID: 143
		private Color textColor = Color.White;

		// Token: 0x02000201 RID: 513
		// (Invoke) Token: 0x06000DAD RID: 3501
		public delegate void DialogDelegate(DialogChoice result);
	}
}
