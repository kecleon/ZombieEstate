using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.StoreScreen.XboxStore
{
	// Token: 0x02000141 RID: 321
	public class XboxItem
	{
		// Token: 0x060009AD RID: 2477 RVA: 0x0004DDD6 File Offset: 0x0004BFD6
		public XboxItem(int iconX, int iconY, object tag, bool own = false, bool afford = true)
		{
			this.mSrc = Global.GetTexRectange(iconX, iconY);
			this.mTag = tag;
			this.mOwn = own;
			this.mAfford = afford;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void Update()
		{
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0004DE14 File Offset: 0x0004C014
		public void Draw(SpriteBatch spriteBatch, Rectangle pos)
		{
			if (this.Locked)
			{
				Rectangle destinationRectangle = new Rectangle(pos.X, pos.Bottom - 16, 16, 16);
				spriteBatch.Draw(Global.MasterTexture, pos, new Rectangle?(this.mSrc), new Color(0.2f, 0.2f, 0.2f));
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(XboxItem.mLockRect), Color.White);
				return;
			}
			if (!this.mAfford)
			{
				spriteBatch.Draw(Global.MasterTexture, pos, new Rectangle?(Global.GetTexRectange(2, 40)), Color.Red);
			}
			spriteBatch.Draw(Global.MasterTexture, pos, new Rectangle?(this.mSrc), Color.White);
			if (this.mOwn)
			{
				Vector2 pos2 = VerchickMath.CenterText(Global.StoreFont, new Vector2((float)(pos.X + 16), (float)(pos.Y + 32 - 8)), XboxItem.Own);
				Shadow.DrawString(XboxItem.Own, Global.StoreFont, pos2, 1, Color.White, spriteBatch);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0004DF17 File Offset: 0x0004C117
		public object Tag
		{
			get
			{
				return this.mTag;
			}
		}

		// Token: 0x04000A23 RID: 2595
		private Rectangle mSrc;

		// Token: 0x04000A24 RID: 2596
		private object mTag;

		// Token: 0x04000A25 RID: 2597
		public bool Locked;

		// Token: 0x04000A26 RID: 2598
		public string ID = "NONE";

		// Token: 0x04000A27 RID: 2599
		private static Rectangle mLockRect = Global.GetTexRectange(9, 37);

		// Token: 0x04000A28 RID: 2600
		private static string Own = "Own";

		// Token: 0x04000A29 RID: 2601
		private static string Afford = "$";

		// Token: 0x04000A2A RID: 2602
		public bool mOwn;

		// Token: 0x04000A2B RID: 2603
		public bool mAfford = true;
	}
}
