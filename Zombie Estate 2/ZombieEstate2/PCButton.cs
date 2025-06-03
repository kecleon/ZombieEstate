using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000BD RID: 189
	internal class PCButton : StoreElement
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x00022AC8 File Offset: 0x00020CC8
		public PCButton(string text, Color color, Vector2 loc)
		{
			this.Text = text;
			this.Color = color;
			this.Position = loc;
			this.font = Global.StoreFontBig;
			if (this.font.MeasureString(text).X > (float)(PCButton.ButtonTex.Width - 8))
			{
				this.font = Global.StoreFont;
			}
			this.MyTex = PCButton.ButtonTex;
			this.MyPressedTex = PCButton.ButtonPressedTex;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00022B55 File Offset: 0x00020D55
		public PCButton(string text, Color color, Vector2 loc, ButtonType type) : this(text, color, loc)
		{
			if (type == ButtonType.Normal)
			{
				this.MyTex = PCButton.ButtonTex;
				this.MyPressedTex = PCButton.ButtonPressedTex;
			}
			if (type == ButtonType.Talent)
			{
				this.MyTex = PCButton.TalentButtonTex;
				this.MyPressedTex = PCButton.TalentButtonPressedTex;
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00022B98 File Offset: 0x00020D98
		public static void LOAD(ContentManager Content)
		{
			if (PCButton.ButtonTex == null)
			{
				PCButton.ButtonTex = Content.Load<Texture2D>("Store\\Button");
				PCButton.ButtonPressedTex = Content.Load<Texture2D>("Store\\Button_Selected");
				PCButton.LongButtonTex = Content.Load<Texture2D>("Store\\LongButton");
				PCButton.LongButtonPressedTex = Content.Load<Texture2D>("Store\\LongButton_Selected");
				PCButton.TalentButtonTex = Content.Load<Texture2D>("Store\\SmallButton");
				PCButton.TalentButtonPressedTex = Content.Load<Texture2D>("Store\\SmallButtonSel");
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00022C0C File Offset: 0x00020E0C
		public PCButton(string text, Color color, Vector2 loc, string tooltip) : this(text, color, loc)
		{
			this.hasToolTip = true;
			this.toolTip = tooltip;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00022C28 File Offset: 0x00020E28
		public override void Update()
		{
			if (!this.inited)
			{
				this.inited = true;
				this.Rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.MyTex.Width, this.MyTex.Height);
				this.TextPos = VerchickMath.CenterText(this.font, new Vector2(this.Position.X + (float)(this.Rect.Width / 2), this.Position.Y + (float)(this.Rect.Height / 2)), this.Text);
			}
			bool highlighted = this.Highlighted;
			if (this.Enabled)
			{
				Point mousePosition = InputManager.GetMousePosition();
				if (this.Rect.Contains(mousePosition))
				{
					if (InputManager.LeftMouseClicked())
					{
						if (this.Pressed != null)
						{
							this.Pressed();
						}
						if (Global.Player != null)
						{
							Global.Player.LockInput.Reset();
							Global.Player.LockInput.Start();
						}
						if (this.PlaysSound)
						{
							SoundEngine.PlaySound("ze2_menunav", 0.35f);
						}
						this.pressedTime -= 1f;
					}
					this.Highlighted = true;
					if (this.hasToolTip)
					{
						ToolTip.SetText(this.toolTip);
					}
				}
				else
				{
					this.Highlighted = false;
				}
			}
			if (this.pressedTime < 10f)
			{
				this.pressedTime -= 1f;
				if (this.pressedTime <= 0f)
				{
					this.pressedTime = 10f;
				}
			}
			if (this.Highlighted && !highlighted)
			{
				SoundEngine.PlaySound("ze2_menuselect", 0.25f);
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00022DD5 File Offset: 0x00020FD5
		public override void Draw(SpriteBatch spriteBatch)
		{
			this.Draw(spriteBatch, 1f);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00022DE4 File Offset: 0x00020FE4
		public void Draw(SpriteBatch spriteBatch, float alpha)
		{
			if (!this.inited)
			{
				return;
			}
			if (this.pressedTime < 10f)
			{
				spriteBatch.Draw(this.MyPressedTex, this.Position, this.Color * alpha);
				Shadow.DrawString(this.Text, this.font, this.TextPos, 2, Color.White * alpha, Color.Black * alpha, spriteBatch);
				return;
			}
			if (!this.Enabled)
			{
				spriteBatch.Draw(this.MyTex, this.Position, new Color(40, 40, 40) * alpha);
				Shadow.DrawString(this.Text, this.font, this.TextPos, 2, Color.Gray * alpha, Color.Black * alpha, spriteBatch);
				return;
			}
			if (this.Highlighted)
			{
				Color color = Color.Lerp(Color.White * alpha, this.Color * alpha, 0.5f);
				spriteBatch.Draw(this.MyTex, this.Position, color);
				Shadow.DrawString(this.Text, this.font, this.TextPos, 2, Color.White, Color.Black * alpha, spriteBatch);
				return;
			}
			spriteBatch.Draw(this.MyTex, this.Position, this.Color * alpha);
			Shadow.DrawString(this.Text, this.font, this.TextPos, 2, Color.LightGray, Color.Black * alpha, spriteBatch);
		}

		// Token: 0x040004C0 RID: 1216
		private string Text;

		// Token: 0x040004C1 RID: 1217
		private Color Color;

		// Token: 0x040004C2 RID: 1218
		public bool Enabled = true;

		// Token: 0x040004C3 RID: 1219
		public bool Highlighted;

		// Token: 0x040004C4 RID: 1220
		public static Texture2D ButtonTex;

		// Token: 0x040004C5 RID: 1221
		public static Texture2D ButtonPressedTex;

		// Token: 0x040004C6 RID: 1222
		public static Texture2D LongButtonTex;

		// Token: 0x040004C7 RID: 1223
		public static Texture2D LongButtonPressedTex;

		// Token: 0x040004C8 RID: 1224
		public static Texture2D TalentButtonTex;

		// Token: 0x040004C9 RID: 1225
		public static Texture2D TalentButtonPressedTex;

		// Token: 0x040004CA RID: 1226
		public PCButton.PressedDelegate Pressed;

		// Token: 0x040004CB RID: 1227
		private Vector2 Position;

		// Token: 0x040004CC RID: 1228
		private Rectangle Rect;

		// Token: 0x040004CD RID: 1229
		private Vector2 TextPos;

		// Token: 0x040004CE RID: 1230
		private string toolTip;

		// Token: 0x040004CF RID: 1231
		private bool hasToolTip;

		// Token: 0x040004D0 RID: 1232
		private float pressedTime = 10f;

		// Token: 0x040004D1 RID: 1233
		private SpriteFont font;

		// Token: 0x040004D2 RID: 1234
		public bool PlaysSound = true;

		// Token: 0x040004D3 RID: 1235
		private bool inited;

		// Token: 0x040004D4 RID: 1236
		private Texture2D MyTex;

		// Token: 0x040004D5 RID: 1237
		private Texture2D MyPressedTex;

		// Token: 0x0200020E RID: 526
		// (Invoke) Token: 0x06000DBD RID: 3517
		public delegate void PressedDelegate();
	}
}
