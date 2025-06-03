using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000052 RID: 82
	public class AlarmMessage
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x0000DB2C File Offset: 0x0000BD2C
		public AlarmMessage(Vector2 center, GameObject parent)
		{
			this.mCenter = center;
			this.mParent = parent;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000DB44 File Offset: 0x0000BD44
		public void Update(float elapsed)
		{
			if (MasterStore.Active)
			{
				return;
			}
			if (this.RemainingTime > 0f)
			{
				this.RemainingTime -= elapsed;
				if (this.RemainingTime <= 0f)
				{
					this.RemainingTime = 0f;
					this.CurrentMessage = "";
				}
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000DB97 File Offset: 0x0000BD97
		public void ClearMessages()
		{
			this.CurrentMessage = "";
			this.RemainingTime = 0f;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000DBB0 File Offset: 0x0000BDB0
		public void Draw(SpriteBatch spriteBatch)
		{
			if (string.IsNullOrEmpty(this.CurrentMessage))
			{
				return;
			}
			Vector2 vector;
			if (this.mParent != null)
			{
				vector = VerchickMath.GetScreenPosition(this.mParent.Position + new Vector3(0f, 0.5f, 0f));
			}
			else
			{
				vector = this.mCenter;
			}
			vector = VerchickMath.CenterText(this.mFont, vector, this.CurrentMessage);
			vector.X = Math.Max(vector.X, 0f);
			if (this.mFont.MeasureString(this.CurrentMessage).X + vector.X > (float)Global.ScreenRect.Width)
			{
				vector.X = (float)Global.ScreenRect.Width - this.mFont.MeasureString(this.CurrentMessage).X;
			}
			float scale = Math.Min(1f, this.RemainingTime);
			Color frontColor = this.Color * scale;
			Shadow.DrawOutlinedString(spriteBatch, this.mFont, this.CurrentMessage, Color.Black * scale, frontColor, 1f, 0f, vector);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000DCCA File Offset: 0x0000BECA
		public void ShowMessage(string msg, Color col, float time)
		{
			this.Color = col;
			this.CurrentMessage = msg;
			this.RemainingTime = time;
			if (msg.Length > 20)
			{
				this.mFont = Global.StoreFont;
				return;
			}
			this.mFont = Global.StoreFontBig;
		}

		// Token: 0x0400015D RID: 349
		private string CurrentMessage;

		// Token: 0x0400015E RID: 350
		private float RemainingTime;

		// Token: 0x0400015F RID: 351
		private Color Color;

		// Token: 0x04000160 RID: 352
		private Vector2 mCenter;

		// Token: 0x04000161 RID: 353
		private SpriteFont mFont;

		// Token: 0x04000162 RID: 354
		private GameObject mParent;

		// Token: 0x04000163 RID: 355
		public static string HEAL = "Press Y To Heal!";

		// Token: 0x04000164 RID: 356
		public static string LOWHEALTH = "LOW HEALTH!!";

		// Token: 0x04000165 RID: 357
		public static string AMMO = "Requires ";

		// Token: 0x04000166 RID: 358
		public static string AMMO2 = " to fire!";

		// Token: 0x04000167 RID: 359
		public static string OUTOFAMMO = "Out of ammo!";
	}
}
