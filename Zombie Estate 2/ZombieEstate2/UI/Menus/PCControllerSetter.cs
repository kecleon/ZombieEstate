using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2.UI.Menus
{
	// Token: 0x02000165 RID: 357
	public class PCControllerSetter
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x0005A2CD File Offset: 0x000584CD
		public string Name
		{
			get
			{
				return this.mName;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x0005A2D5 File Offset: 0x000584D5
		public ButtonPress Action
		{
			get
			{
				return this.mAction;
			}
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0005A2E0 File Offset: 0x000584E0
		public PCControllerSetter(ButtonPress action, string name, Vector2 pos)
		{
			this.mName = name;
			this.mAction = action;
			this.mPosition = pos;
			this.mCurrent = InputManager.GetKeyForButtonPress(action);
			this.UpdateString();
			this.mSetButton = new Rectangle((int)this.mPosition.X + 700, (int)this.mPosition.Y, 80, 32);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0005A348 File Offset: 0x00058548
		private void UpdateString()
		{
			if (this.mCurrent == Keys.F14)
			{
				this.mKey = "Left Mouse";
				return;
			}
			if (this.mCurrent == Keys.F15)
			{
				this.mKey = "Right Mouse";
				return;
			}
			this.mKey = this.mCurrent.ToString();
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0005A398 File Offset: 0x00058598
		public void UpdateStuff()
		{
			this.mCurrent = InputManager.GetKeyForButtonPress(this.mAction);
			this.UpdateString();
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0005A3B1 File Offset: 0x000585B1
		public bool Clicked()
		{
			return InputManager.LeftMouseClicked() && this.mSetButton.Contains(InputManager.GetMousePosition());
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0005A3D0 File Offset: 0x000585D0
		public void Draw(SpriteBatch spritebatch, bool flash)
		{
			Shadow.DrawString(this.mName + ":", Global.StoreFontBig, this.mPosition, 1, Color.LightGray, spritebatch);
			Vector2 pos = new Vector2(this.mPosition.X + 400f, this.mPosition.Y);
			Shadow.DrawString(this.mKey.ToString(), Global.StoreFontBig, pos, 1, Color.White, spritebatch);
			pos.X += 300f;
			if (this.mSetButton.Contains(InputManager.GetMousePosition()) && flash)
			{
				Color color = Color.Lerp(Color.LightGreen, Color.White, Global.Pulse);
				Shadow.DrawString("Set", Global.StoreFontBig, pos, 1, color, spritebatch);
				return;
			}
			Shadow.DrawString("Set", Global.StoreFontBig, pos, 1, Color.Green, spritebatch);
		}

		// Token: 0x04000BAE RID: 2990
		private ButtonPress mAction;

		// Token: 0x04000BAF RID: 2991
		private string mName;

		// Token: 0x04000BB0 RID: 2992
		private Vector2 mPosition;

		// Token: 0x04000BB1 RID: 2993
		private string mKey;

		// Token: 0x04000BB2 RID: 2994
		private Keys mCurrent;

		// Token: 0x04000BB3 RID: 2995
		private Rectangle mSetButton;
	}
}
