using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000CA RID: 202
	internal class Portrait : StoreElement
	{
		// Token: 0x06000524 RID: 1316 RVA: 0x000278E0 File Offset: 0x00025AE0
		public Portrait(Vector2 origin, int size, bool autoSelect, string name)
		{
			this.Name = name;
			this.portraitDest = new Rectangle(0, 0, size, size);
			this.portraitDest.X = (int)origin.X;
			this.portraitDest.Y = (int)origin.Y;
			this.AutoSelect = autoSelect;
			if (this.AutoSelect)
			{
				this.characterSelected = true;
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0002794B File Offset: 0x00025B4B
		public Portrait(Vector2 origin, int size, bool autoSelect) : this(origin, size, autoSelect, "")
		{
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0002795B File Offset: 0x00025B5B
		public override void ItemSelected(PCItem item)
		{
			if (this.AutoSelect)
			{
				this.SelectCharacter(item);
				base.ItemSelected(item);
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00027974 File Offset: 0x00025B74
		public void SelectCharacter(PCItem item)
		{
			this.characterSelected = true;
			this.portraitTex = item.ItemTex;
			this.Bouncy = new BouncyObject(Global.MasterTexture, this.portraitDest.X, this.portraitDest.Y - this.portraitDest.Height - 32, this.portraitDest.Y + this.portraitDest.Height, this.portraitDest.Width, this.portraitDest.Height, 0f, this.portraitTex, this.portraitDest, 0.5f);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00027A0C File Offset: 0x00025C0C
		public void SelectCharacter(Point point)
		{
			this.characterSelected = true;
			this.portraitTex = Global.GetTexRectange(point.X, point.Y);
			this.Bouncy = new BouncyObject(Global.MasterTexture, this.portraitDest.X, this.portraitDest.Y - this.portraitDest.Height - 32, this.portraitDest.Y + this.portraitDest.Height, this.portraitDest.Width, this.portraitDest.Height, 0f, this.portraitTex, this.portraitDest, 0.5f);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00027AAF File Offset: 0x00025CAF
		public override void Update()
		{
			if (this.Bouncy != null)
			{
				this.Bouncy.Update(0.016666668f);
			}
			base.Update();
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00027ACF File Offset: 0x00025CCF
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (!this.characterSelected)
			{
				this.DrawName(spriteBatch);
				return;
			}
			if (this.Bouncy != null)
			{
				this.Bouncy.Draw(spriteBatch);
			}
			this.DrawName(spriteBatch);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00027AFC File Offset: 0x00025CFC
		private void DrawName(SpriteBatch spriteBatch)
		{
			if (!string.IsNullOrEmpty(this.Name))
			{
				Vector2 position = new Vector2((float)(this.portraitDest.X + this.portraitDest.Width / 2), (float)(this.portraitDest.Y + this.portraitDest.Height - 18));
				position = VerchickMath.CenterText(Global.StoreFont, position, this.Name);
				Shadow.DrawOutlinedString(spriteBatch, Global.StoreFont, this.Name, Color.Black, Color.White, 1f, 0f, position);
			}
		}

		// Token: 0x0400053B RID: 1339
		private Rectangle portraitTex;

		// Token: 0x0400053C RID: 1340
		private Rectangle portraitDest;

		// Token: 0x0400053D RID: 1341
		private bool characterSelected;

		// Token: 0x0400053E RID: 1342
		private bool AutoSelect = true;

		// Token: 0x0400053F RID: 1343
		private BouncyObject Bouncy;

		// Token: 0x04000540 RID: 1344
		public string Name;
	}
}
