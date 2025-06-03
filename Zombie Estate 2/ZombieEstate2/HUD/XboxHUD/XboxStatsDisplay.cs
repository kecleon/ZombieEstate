using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.HUD.XboxHUD
{
	// Token: 0x020001CB RID: 459
	internal class XboxStatsDisplay
	{
		// Token: 0x06000C5C RID: 3164 RVA: 0x000657F1 File Offset: 0x000639F1
		public XboxStatsDisplay(Vector2 textPosition)
		{
			this.mTextPosition = textPosition;
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x00065800 File Offset: 0x00063A00
		public void Update(IComparable current, IComparable starting, string toolTip, bool percent = false)
		{
			if (toolTip != this.mToolTip)
			{
				this.mToolTip = toolTip;
			}
			if (this.mValue != current)
			{
				this.mValue = current;
				this.mText = current.ToString();
				if (percent)
				{
					this.mText += "%";
				}
				int num = current.CompareTo(starting);
				if (num == -1)
				{
					this.mState = XboxStatsDisplay.State.Lower;
					return;
				}
				if (num == 0)
				{
					this.mState = XboxStatsDisplay.State.Neutral;
					return;
				}
				this.mState = XboxStatsDisplay.State.Higher;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x0006587E File Offset: 0x00063A7E
		public string ToolTip
		{
			get
			{
				return this.mToolTip;
			}
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00065888 File Offset: 0x00063A88
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.mText == null)
			{
				return;
			}
			Color color = Color.LightGray;
			if (this.mState == XboxStatsDisplay.State.Higher)
			{
				color = Color.LightGreen;
			}
			else if (this.mState == XboxStatsDisplay.State.Lower)
			{
				color = Color.Red;
			}
			Shadow.DrawString(this.mText, Global.EquationFontSmall, this.mTextPosition, 1, color, spriteBatch);
		}

		// Token: 0x04000D0B RID: 3339
		private string mText;

		// Token: 0x04000D0C RID: 3340
		private IComparable mValue;

		// Token: 0x04000D0D RID: 3341
		private XboxStatsDisplay.State mState;

		// Token: 0x04000D0E RID: 3342
		private Vector2 mTextPosition;

		// Token: 0x04000D0F RID: 3343
		private string mToolTip;

		// Token: 0x0200022C RID: 556
		private enum State
		{
			// Token: 0x04000E80 RID: 3712
			Neutral,
			// Token: 0x04000E81 RID: 3713
			Higher,
			// Token: 0x04000E82 RID: 3714
			Lower
		}
	}
}
