using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000B1 RID: 177
	internal class Alert
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x00021700 File Offset: 0x0001F900
		public Alert(Rectangle position, Point tex, string text)
		{
			this.Position = position;
			this.TexSrc = Global.GetTexRectange(tex.X, tex.Y);
			this.Text = text;
			this.TexDest = new Rectangle(this.Position.X + this.Position.Width / 4, this.Position.Y + this.Position.Height / 4, position.Width / 2, position.Height / 2);
			this.TextPos = VerchickMath.CenterText(Global.StoreFontBig, new Vector2((float)(position.X + position.Width / 2), (float)(position.Y + position.Height)), this.Text);
			if (Alert.Burst == null)
			{
				Alert.Burst = Global.Content.Load<Texture2D>("Store\\PCStore\\Burst");
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000217D9 File Offset: 0x0001F9D9
		public void Activate()
		{
			this.Active = true;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000217E2 File Offset: 0x0001F9E2
		public void Deactivate()
		{
			this.Active = false;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000217EC File Offset: 0x0001F9EC
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.Active)
			{
				float scale = Global.Pulse * 0.5f + 0.5f;
				spriteBatch.Draw(Alert.Burst, this.Position, Color.White * scale);
				spriteBatch.Draw(Global.MasterTexture, this.TexDest, new Rectangle?(this.TexSrc), Color.White * scale);
				Shadow.DrawString(this.Text, Global.StoreFontBig, this.TextPos, 1, Color.White * scale, spriteBatch);
			}
		}

		// Token: 0x04000471 RID: 1137
		private Rectangle Position;

		// Token: 0x04000472 RID: 1138
		private Rectangle TexSrc;

		// Token: 0x04000473 RID: 1139
		private Rectangle TexDest;

		// Token: 0x04000474 RID: 1140
		private string Text;

		// Token: 0x04000475 RID: 1141
		private Vector2 TextPos;

		// Token: 0x04000476 RID: 1142
		private bool Active;

		// Token: 0x04000477 RID: 1143
		private static Texture2D Burst;
	}
}
