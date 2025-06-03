using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000B7 RID: 183
	internal class CheckBox : StoreElement
	{
		// Token: 0x060004A0 RID: 1184 RVA: 0x00022254 File Offset: 0x00020454
		public CheckBox(Point iconTex, Point pos, string tooltip, PCStore store)
		{
			this.BoxDest = new Rectangle(pos.X + 34, pos.Y, 32, 32);
			this.IconSrc = Global.GetTexRectange(iconTex.X, iconTex.Y);
			this.IconDest = new Rectangle(pos.X, pos.Y, 32, 32);
			if (CheckBox.TexUnchecked == null)
			{
				CheckBox.TexUnchecked = Global.Content.Load<Texture2D>("Store\\PCStore\\CheckBox_Unchecked");
				CheckBox.TexChecked = Global.Content.Load<Texture2D>("Store\\PCStore\\CheckBox_Checked");
			}
			this.tooltip = tooltip;
			this.store = store;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000222F8 File Offset: 0x000204F8
		public override void Update()
		{
			if (this.BoxDest.Contains(InputManager.GetMousePosition()))
			{
				ToolTip.SetText(this.tooltip);
				if (InputManager.LeftMouseClicked())
				{
					this.Checked = !this.Checked;
					this.store.FilterChanged();
				}
			}
			base.Update();
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0002234C File Offset: 0x0002054C
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (this.Checked)
			{
				spriteBatch.Draw(CheckBox.TexChecked, this.BoxDest, Color.White);
			}
			else
			{
				spriteBatch.Draw(CheckBox.TexUnchecked, this.BoxDest, Color.White);
			}
			spriteBatch.Draw(Global.MasterTexture, this.IconDest, new Rectangle?(this.IconSrc), Color.White);
			base.Draw(spriteBatch);
		}

		// Token: 0x0400049C RID: 1180
		public static Texture2D TexUnchecked;

		// Token: 0x0400049D RID: 1181
		public static Texture2D TexChecked;

		// Token: 0x0400049E RID: 1182
		private Rectangle IconSrc;

		// Token: 0x0400049F RID: 1183
		private Rectangle IconDest;

		// Token: 0x040004A0 RID: 1184
		private Rectangle BoxDest;

		// Token: 0x040004A1 RID: 1185
		public bool Checked;

		// Token: 0x040004A2 RID: 1186
		private string tooltip;

		// Token: 0x040004A3 RID: 1187
		private PCStore store;
	}
}
