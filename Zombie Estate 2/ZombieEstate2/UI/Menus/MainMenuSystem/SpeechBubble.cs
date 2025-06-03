using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.UI.Menus.MainMenuSystem
{
	// Token: 0x0200016C RID: 364
	internal class SpeechBubble
	{
		// Token: 0x06000B14 RID: 2836 RVA: 0x0005BA48 File Offset: 0x00059C48
		public SpeechBubble(Vector2 pos)
		{
			this.TexBubble = Global.Content.Load<Texture2D>("MenuTiles//Speech");
			this.Pos = pos;
			this.TextLoc = new Rectangle((int)pos.X + 214, (int)pos.Y + 18, 394, 144);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0005BAEC File Offset: 0x00059CEC
		public void Update()
		{
			this.tick -= 0.016666668f;
			if (this.tick <= 0f)
			{
				this.tick = this.speed;
				if (this.Text != "")
				{
					this.talkCount++;
					if (this.talkCount > 4)
					{
						this.xTex = Global.rand.Next(75, 79);
						this.talkCount = 0;
					}
					if (this.CurrentText != this.Text)
					{
						this.talking = true;
						this.charCount++;
						this.CurrentText = this.Text.Substring(0, this.charCount);
						return;
					}
					this.talking = false;
					this.xTex = 73;
				}
			}
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0005BBBE File Offset: 0x00059DBE
		public void GiveString(string s)
		{
			this.Text = VerchickMath.WordWrapWidth(s, this.TextLoc.Width, this.font);
			this.CurrentText = "";
			this.charCount = 0;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0005BBF0 File Offset: 0x00059DF0
		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 pos = new Vector2(this.Pos.X, this.Pos.Y);
			this.Pos += MenuTileManager.ShakeVectorOffset;
			spriteBatch.Draw(this.TexBubble, this.Pos, Color.White);
			string currentText = this.CurrentText;
			spriteBatch.DrawString(this.font, currentText, new Vector2((float)this.TextLoc.X, (float)this.TextLoc.Y), Color.Black);
			Rectangle texRectange = Global.GetTexRectange(this.xTex, this.yTex);
			Rectangle destinationRectangle = new Rectangle((int)this.Pos.X, (int)this.Pos.Y + 32, 128, 128);
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(texRectange), Color.White);
			this.Pos = pos;
		}

		// Token: 0x04000BD6 RID: 3030
		private Texture2D TexBubble;

		// Token: 0x04000BD7 RID: 3031
		private Vector2 Pos;

		// Token: 0x04000BD8 RID: 3032
		private Rectangle TextLoc;

		// Token: 0x04000BD9 RID: 3033
		private string CurrentText = "";

		// Token: 0x04000BDA RID: 3034
		private string Text = "";

		// Token: 0x04000BDB RID: 3035
		private int charCount;

		// Token: 0x04000BDC RID: 3036
		private float tick = 0.014f;

		// Token: 0x04000BDD RID: 3037
		private float speed = 0.014f;

		// Token: 0x04000BDE RID: 3038
		private int talkCount;

		// Token: 0x04000BDF RID: 3039
		private bool talking;

		// Token: 0x04000BE0 RID: 3040
		private SpriteFont font = Global.StoreFont;

		// Token: 0x04000BE1 RID: 3041
		private int xTex = 73;

		// Token: 0x04000BE2 RID: 3042
		private int yTex = 1;
	}
}
