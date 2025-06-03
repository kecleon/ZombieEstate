using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.UI;

namespace ZombieEstate2.StoreScreen.XboxStore
{
	// Token: 0x02000146 RID: 326
	internal class XboxStoreDialog
	{
		// Token: 0x060009EB RID: 2539 RVA: 0x000511F4 File Offset: 0x0004F3F4
		public XboxStoreDialog(string text, Vector2 topLeft, Player player, XboxStoreDialog.XboxDialogType type)
		{
			this.mBorder = new Rectangle((int)topLeft.X, (int)topLeft.Y, 500, 280);
			this.mBG = new Rectangle((int)topLeft.X + 2, (int)topLeft.Y + 2, 496, 276);
			this.mIndex = player.Index;
			Rectangle rect = new Rectangle(this.mBG.X + 30, this.mBG.Y + 30, this.mBG.Width - 60, this.mBG.Height - 60);
			if (text != null)
			{
				this.mText = new ScrollBox(text, rect, Global.StoreFontBig, null, Color.White);
			}
			this.mType = type;
			this.Active = true;
			this.mFirstFrame = true;
			this.SetupButtons(topLeft);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x000512EC File Offset: 0x0004F4EC
		public XboxStoreDialog(string text, Vector2 topLeft, int index, XboxStoreDialog.XboxDialogType type)
		{
			this.mIndex = index;
			this.mBorder = new Rectangle((int)topLeft.X, (int)topLeft.Y, 500, 280);
			this.mBG = new Rectangle((int)topLeft.X + 2, (int)topLeft.Y + 2, 496, 276);
			Rectangle rect = new Rectangle(this.mBG.X + 30, this.mBG.Y + 30, this.mBG.Width - 60, this.mBG.Height - 60);
			if (text != null)
			{
				this.mText = new ScrollBox(text, rect, Global.StoreFontBig, null, Color.White);
			}
			this.mType = type;
			this.Active = true;
			this.mFirstFrame = true;
			this.SetupButtons(topLeft);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x000513E0 File Offset: 0x0004F5E0
		private void SetupButtons(Vector2 topLeft)
		{
			this.mOkLoc = new Vector2(topLeft.X + (float)this.mBorder.Width - (float)this.BUTTON_WIDTH - 32f, topLeft.Y + 220f);
			this.mYesLoc = new Vector2(topLeft.X + 32f, topLeft.Y + 220f);
			this.mNoLoc = new Vector2(topLeft.X + (float)this.mBorder.Width - 32f - (float)this.BUTTON_WIDTH, topLeft.Y + 220f);
			this.mYesButton = new ZEButton(new Rectangle((int)this.mYesLoc.X, (int)this.mYesLoc.Y, this.BUTTON_WIDTH, this.BUTTON_HEIGHT), XboxStoreDialog.mYesText, ButtonPress.Affirmative, this.mIndex, ZEButton.POSITIVE_COLOR);
			this.mNoButton = new ZEButton(new Rectangle((int)this.mNoLoc.X, (int)this.mNoLoc.Y, this.BUTTON_WIDTH, this.BUTTON_HEIGHT), XboxStoreDialog.mNoText, ButtonPress.Negative, this.mIndex, ZEButton.NEGATIVE_COLOR);
			this.mOkButton = new ZEButton(new Rectangle((int)this.mOkLoc.X, (int)this.mOkLoc.Y, this.BUTTON_WIDTH, this.BUTTON_HEIGHT), XboxStoreDialog.mOkText, ButtonPress.Affirmative, this.mIndex);
			this.mYesButton.OnPressed += this.MYesButton_OnPressed;
			this.mNoButton.OnPressed += this.MNoButton_OnPressed;
			this.mOkButton.OnPressed += this.MOkButton_OnPressed;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0005158F File Offset: 0x0004F78F
		private void MOkButton_OnPressed(object sender, EventArgs e)
		{
			this.Active = false;
			this.YesPressed = true;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0005159F File Offset: 0x0004F79F
		private void MNoButton_OnPressed(object sender, EventArgs e)
		{
			this.Active = false;
			this.YesPressed = false;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0005158F File Offset: 0x0004F78F
		private void MYesButton_OnPressed(object sender, EventArgs e)
		{
			this.Active = false;
			this.YesPressed = true;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x000515B0 File Offset: 0x0004F7B0
		public virtual void Update()
		{
			if (this.mFirstFrame)
			{
				this.mFirstFrame = false;
				return;
			}
			if (this.mType == XboxStoreDialog.XboxDialogType.Ok)
			{
				this.mOkButton.Update();
				return;
			}
			if (this.mType == XboxStoreDialog.XboxDialogType.YesNo)
			{
				this.mYesButton.Update();
				this.mNoButton.Update();
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00051600 File Offset: 0x0004F800
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Global.Pixel, this.mBorder, Color.Black);
			spriteBatch.Draw(Global.Pixel, this.mBG, Color.Lerp(this.GetColor(), Color.Black, 0.5f));
			if (this.mText != null)
			{
				this.mText.Draw(spriteBatch);
			}
			this.DrawExtra(spriteBatch);
			if (this.mType == XboxStoreDialog.XboxDialogType.Ok)
			{
				this.mOkButton.Draw(spriteBatch);
				return;
			}
			if (this.mType == XboxStoreDialog.XboxDialogType.YesNo)
			{
				this.mYesButton.Draw(spriteBatch);
				this.mNoButton.Draw(spriteBatch);
			}
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void DrawExtra(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0005169C File Offset: 0x0004F89C
		private Color GetColor()
		{
			switch (this.mIndex)
			{
			case 0:
				return new Color(136, 178, 229);
			case 1:
				return new Color(255, 140, 140);
			case 2:
				return new Color(136, 229, 178);
			case 3:
				return new Color(229, 229, 136);
			default:
				return Color.White;
			}
		}

		// Token: 0x04000A70 RID: 2672
		private ScrollBox mText;

		// Token: 0x04000A71 RID: 2673
		private Rectangle mBG;

		// Token: 0x04000A72 RID: 2674
		private Rectangle mBorder;

		// Token: 0x04000A73 RID: 2675
		private XboxStoreDialog.XboxDialogType mType;

		// Token: 0x04000A74 RID: 2676
		private static string mYesText = "Yes";

		// Token: 0x04000A75 RID: 2677
		private static string mNoText = "No";

		// Token: 0x04000A76 RID: 2678
		private static string mOkText = "Ok";

		// Token: 0x04000A77 RID: 2679
		private Vector2 mOkLoc;

		// Token: 0x04000A78 RID: 2680
		private Vector2 mYesLoc;

		// Token: 0x04000A79 RID: 2681
		private Vector2 mNoLoc;

		// Token: 0x04000A7A RID: 2682
		private ZEButton mYesButton;

		// Token: 0x04000A7B RID: 2683
		private ZEButton mNoButton;

		// Token: 0x04000A7C RID: 2684
		private ZEButton mOkButton;

		// Token: 0x04000A7D RID: 2685
		private int mIndex;

		// Token: 0x04000A7E RID: 2686
		private int BUTTON_WIDTH = 100;

		// Token: 0x04000A7F RID: 2687
		private int BUTTON_HEIGHT = 48;

		// Token: 0x04000A80 RID: 2688
		public bool Active;

		// Token: 0x04000A81 RID: 2689
		public bool YesPressed;

		// Token: 0x04000A82 RID: 2690
		private bool mFirstFrame = true;

		// Token: 0x02000223 RID: 547
		public enum XboxDialogType
		{
			// Token: 0x04000E5C RID: 3676
			YesNo,
			// Token: 0x04000E5D RID: 3677
			Ok
		}
	}
}
