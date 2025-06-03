using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000BC RID: 188
	internal class PCAmmoButtons : StoreElement
	{
		// Token: 0x060004AC RID: 1196 RVA: 0x000227F0 File Offset: 0x000209F0
		public PCAmmoButtons(AmmoType myType, int x, int y, PCGunStore store)
		{
			this.GunStore = store;
			this.MyType = myType;
			if (myType == AmmoType.ASSAULT)
			{
				this.Src = Global.GetTexRectange(0, 43);
			}
			if (myType == AmmoType.SHELLS)
			{
				this.Src = Global.GetTexRectange(0, 45);
			}
			if (myType == AmmoType.HEAVY)
			{
				this.Src = Global.GetTexRectange(0, 44);
			}
			if (myType == AmmoType.EXPLOSIVE)
			{
				this.Src = Global.GetTexRectange(0, 46);
			}
			if (myType == AmmoType.MELEE)
			{
				this.Src = Global.GetTexRectange(8, 47);
			}
			this.Dest = new Rectangle(x, y, 64, 64);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0002287C File Offset: 0x00020A7C
		public PCAmmoButtons(int x, int y, PCGunStore store)
		{
			this.GunStore = store;
			this.Misc = true;
			this.Src = Global.GetTexRectange(8, 46);
			this.Dest = new Rectangle(x, y, 64, 64);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x000228B4 File Offset: 0x00020AB4
		public override void Update()
		{
			this.Highlighted = false;
			if (this.Dest.Contains(InputManager.GetMousePosition()))
			{
				this.Highlighted = true;
				if (InputManager.LeftMouseClicked())
				{
					this.GunStore.FireNewScreen(this);
					this.Active = true;
				}
			}
			base.Update();
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00022904 File Offset: 0x00020B04
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (this.Active)
			{
				Rectangle value = new Rectangle(this.Src.X + 48, this.Src.Y, 16, 16);
				spriteBatch.Draw(Global.MasterTexture, this.Dest, new Rectangle?(value), Color.White);
				string text = this.MyType.ToString();
				if (this.Misc)
				{
					text = "Miscellaneous";
				}
				Vector2 pos = VerchickMath.CenterText(Global.StoreFont, new Vector2((float)(this.Dest.X + this.Dest.Width / 2), (float)(this.Dest.Y + this.Dest.Height + 8)), text);
				Shadow.DrawString(text, Global.StoreFont, pos, 1, Color.White, spriteBatch);
				return;
			}
			if (this.Highlighted)
			{
				Rectangle value2 = new Rectangle(this.Src.X + 16, this.Src.Y, 16, 16);
				spriteBatch.Draw(Global.MasterTexture, this.Dest, new Rectangle?(value2), Color.White);
				string text2 = this.MyType.ToString();
				if (this.Misc)
				{
					text2 = "Miscellaneous";
				}
				Vector2 pos2 = VerchickMath.CenterText(Global.StoreFont, new Vector2((float)(this.Dest.X + this.Dest.Width / 2), (float)(this.Dest.Y + this.Dest.Height + 8)), text2);
				Shadow.DrawString(text2, Global.StoreFont, pos2, 1, Color.White, spriteBatch);
				return;
			}
			spriteBatch.Draw(Global.MasterTexture, this.Dest, new Rectangle?(this.Src), Color.White);
			base.Draw(spriteBatch);
		}

		// Token: 0x040004B9 RID: 1209
		public bool Active;

		// Token: 0x040004BA RID: 1210
		public bool Highlighted;

		// Token: 0x040004BB RID: 1211
		public bool Misc;

		// Token: 0x040004BC RID: 1212
		public AmmoType MyType;

		// Token: 0x040004BD RID: 1213
		private Rectangle Src;

		// Token: 0x040004BE RID: 1214
		private Rectangle Dest;

		// Token: 0x040004BF RID: 1215
		private PCGunStore GunStore;
	}
}
