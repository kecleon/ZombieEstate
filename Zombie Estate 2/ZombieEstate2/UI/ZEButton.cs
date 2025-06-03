using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.UI
{
	// Token: 0x0200015B RID: 347
	public class ZEButton
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000A7F RID: 2687 RVA: 0x000557E0 File Offset: 0x000539E0
		// (remove) Token: 0x06000A80 RID: 2688 RVA: 0x00055818 File Offset: 0x00053A18
		public event EventHandler OnPressed;

		// Token: 0x06000A81 RID: 2689 RVA: 0x00055850 File Offset: 0x00053A50
		public static void Init(ContentManager content)
		{
			ZEButton.mKeyboardKey = content.Load<Texture2D>("XboxStore\\KeyboardButton");
			ZEButton.mKeyboardKeyLong = content.Load<Texture2D>("XboxStore\\KeyboardButtonLong");
			ZEButton.mXboxA = content.Load<Texture2D>("XboxStore\\AButton");
			ZEButton.mXboxB = content.Load<Texture2D>("XboxStore\\BButton");
			ZEButton.mIconFont = content.Load<SpriteFont>("TinyKootenay");
			ZEButton.mNameFont = Global.EquationFontSmall;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x000558B7 File Offset: 0x00053AB7
		public ZEButton(Rectangle rect, string name, ButtonPress button, int index, Color color) : this(rect, name, button, index)
		{
			this.mColor = color;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x000558CC File Offset: 0x00053ACC
		public ZEButton(Rectangle rect, string name, ButtonPress button, int index)
		{
			this.ENABLED = true;
			this.mIconWidth = ZEButton.ICON_WIDTH;
			base..ctor();
			this.mKeyIcon = ZEButton.mKeyboardKey;
			this.mIconText = InputManager.GetKeyString(button, this.mIndex);
			if (this.mIconText.Length > 3)
			{
				this.mKeyIcon = ZEButton.mKeyboardKeyLong;
				this.mIconWidth = ZEButton.ICON_WIDTH_LONG;
				this.mIconWidthFudge = -4;
			}
			this.mColor = new Color(60, 60, 60);
			this.mRectangle = rect;
			this.mName = name;
			this.mButton = button;
			this.mIndex = index;
			this.ComputePositions();
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00055970 File Offset: 0x00053B70
		public ZEButton(Rectangle rect, string name, string buttonText)
		{
			this.ENABLED = true;
			this.mIconWidth = ZEButton.ICON_WIDTH;
			base..ctor();
			this.mKeyIcon = ZEButton.mKeyboardKey;
			this.mIconText = buttonText;
			if (this.mIconText.Length > 3)
			{
				this.mKeyIcon = ZEButton.mKeyboardKeyLong;
				this.mIconWidth = ZEButton.ICON_WIDTH_LONG;
				this.mIconWidthFudge = -4;
			}
			this.mColor = new Color(60, 60, 60);
			this.mRectangle = rect;
			this.mName = name;
			this.mButton = ButtonPress.Affirmative;
			this.mIndex = 0;
			this.ComputePositions();
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00055A08 File Offset: 0x00053C08
		public void Update()
		{
			if (!this.ENABLED)
			{
				return;
			}
			if (SteamHelper.SteamOverlayVisible)
			{
				return;
			}
			if (VerchickMath.IsMouseInRect(this.mRectangle))
			{
				if (!this.mHighlighted)
				{
					SoundEngine.PlaySound("ze2_menuselect", 0.1f);
				}
				this.mHighlighted = true;
				if (InputManager.LeftMouseClicked() && this.OnPressed != null)
				{
					this.OnPressed(this, null);
				}
			}
			else
			{
				this.mHighlighted = false;
			}
			if (this.mIndex == -1)
			{
				for (int i = 0; i < 4; i++)
				{
					if (InputManager.ButtonPressed(this.mButton, i, false) && this.OnPressed != null)
					{
						this.OnPressed(this, null);
					}
				}
				return;
			}
			if (InputManager.ButtonPressed(this.mButton, this.mIndex, false) && this.OnPressed != null)
			{
				this.OnPressed(this, null);
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00055ADC File Offset: 0x00053CDC
		public void Draw(SpriteBatch spriteBatch)
		{
			this.mIconText = InputManager.GetKeyString(this.mButton, this.mIndex);
			if (this.mIconText.Length > 3)
			{
				this.mKeyIcon = ZEButton.mKeyboardKeyLong;
				this.mIconWidth = ZEButton.ICON_WIDTH_LONG;
				this.mIconWidthFudge = -4;
			}
			this.ComputePositions();
			float scale = this.ENABLED ? 1f : 0.5f;
			Color black = Color.Black;
			Color color = this.mColor * scale;
			if (this.mHighlighted)
			{
				color.R *= 2;
				color.G *= 2;
				color.B *= 2;
			}
			if (this.PULSE && this.ENABLED)
			{
				color = Color.Lerp(this.mColor, Color.White, Math.Min(0.75f, Global.Pulse));
			}
			spriteBatch.Draw(Global.Pixel, this.mRectangle, black);
			Rectangle rectangle = this.mRectangle;
			rectangle.Inflate(-2, -2);
			spriteBatch.Draw(Global.Pixel, rectangle, color);
			if (rectangle.Height > 32)
			{
				Rectangle destinationRectangle = new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - (int)((float)rectangle.Height * 0.15f), rectangle.Width, (int)((float)rectangle.Height * 0.15f));
				spriteBatch.Draw(Global.Pixel, destinationRectangle, Color.Lerp(color, Color.Black, 0.5f));
			}
			if (this.mIndex != -1)
			{
				PlayerInfo player = PlayerManager.GetPlayer(this.mIndex);
				if (player != null && !player.UsingController)
				{
					spriteBatch.Draw(this.mKeyIcon, this.mIconPosition, Color.White * scale);
					Shadow.DrawString(this.mIconText, ZEButton.mIconFont, this.mIconTextPosition, 1, Color.White * scale, Color.Black * scale, spriteBatch);
				}
				else
				{
					this.DrawXboxButton(spriteBatch);
				}
			}
			else
			{
				bool flag = true;
				for (int i = 0; i < 4; i++)
				{
					PlayerInfo player2 = PlayerManager.GetPlayer(i);
					if (player2 != null && !player2.UsingController && player2.Local)
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					spriteBatch.Draw(this.mKeyIcon, this.mIconPosition, Color.White * scale);
					Shadow.DrawString(this.mIconText, ZEButton.mIconFont, this.mIconTextPosition, 1, Color.White * scale, Color.Black * scale, spriteBatch);
				}
				else
				{
					this.DrawXboxButton(spriteBatch);
				}
			}
			Shadow.DrawString(this.mName, ZEButton.mNameFont, this.mNamePosition, 1, Color.White * scale, Color.Black * scale, spriteBatch);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00055D93 File Offset: 0x00053F93
		public void SetText(string s)
		{
			this.mName = s;
			this.ComputePositions();
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00055DA4 File Offset: 0x00053FA4
		private void DrawXboxButton(SpriteBatch spriteBatch)
		{
			Rectangle destinationRectangle = new Rectangle(this.mRectangle.X + 2, this.mRectangle.Y + (int)((float)(this.mRectangle.Height - 32) / 2f), 32, 32);
			Rectangle texRectange = Global.GetTexRectange(0, 38);
			ButtonPress buttonPress = this.mButton;
			switch (buttonPress)
			{
			case ButtonPress.Affirmative:
			case ButtonPress.Jump:
				texRectange = Global.GetTexRectange(0, 38);
				goto IL_CE;
			case ButtonPress.Negative:
				texRectange = Global.GetTexRectange(1, 38);
				goto IL_CE;
			case ButtonPress.Fire:
			case ButtonPress.SwapWeaponsNegative:
			case ButtonPress.SwapWeaponsPositive:
			case ButtonPress.Pause:
				goto IL_C6;
			case ButtonPress.Ready:
				texRectange = Global.GetTexRectange(4, 39);
				goto IL_CE;
			case ButtonPress.HealthPack:
				break;
			case ButtonPress.ViewStats:
				texRectange = Global.GetTexRectange(4, 40);
				goto IL_CE;
			default:
				if (buttonPress == ButtonPress.Reload)
				{
					texRectange = Global.GetTexRectange(2, 38);
					goto IL_CE;
				}
				if (buttonPress != ButtonPress.Inventory)
				{
					goto IL_C6;
				}
				break;
			}
			texRectange = Global.GetTexRectange(3, 38);
			goto IL_CE;
			IL_C6:
			texRectange = Global.GetTexRectange(0, 0);
			IL_CE:
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(texRectange), Color.White);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00055E98 File Offset: 0x00054098
		private void ComputePositions()
		{
			ZEButton.mNameFont.MeasureString(this.mName);
			int num = (int)ZEButton.mNameFont.MeasureString(this.mName).Y;
			int num2 = this.mRectangle.X + 4 + this.mIconWidthFudge;
			int num3 = this.mRectangle.Y + (this.mRectangle.Height - num) / 2;
			this.mIconPosition = new Vector2((float)num2, (float)(num3 + 4));
			this.mNamePosition = new Vector2((float)(num2 + this.mIconWidth + 4 + this.mIconWidthFudge), (float)num3);
			int num4 = (int)ZEButton.mIconFont.MeasureString(this.mIconText).X;
			num2 += (this.mIconWidth - num4) / 2;
			this.mIconTextPosition = new Vector2((float)num2, (float)(num3 + 4));
		}

		// Token: 0x04000B2F RID: 2863
		public static Color POSITIVE_COLOR = new Color(40, 80, 50);

		// Token: 0x04000B30 RID: 2864
		public static Color NEGATIVE_COLOR = new Color(80, 40, 40);

		// Token: 0x04000B31 RID: 2865
		private static SpriteFont mIconFont;

		// Token: 0x04000B32 RID: 2866
		private static SpriteFont mNameFont;

		// Token: 0x04000B33 RID: 2867
		private static Texture2D mKeyboardKey;

		// Token: 0x04000B34 RID: 2868
		private static Texture2D mKeyboardKeyLong;

		// Token: 0x04000B35 RID: 2869
		private static Texture2D mXboxA;

		// Token: 0x04000B36 RID: 2870
		private static Texture2D mXboxB;

		// Token: 0x04000B37 RID: 2871
		private ButtonPress mButton;

		// Token: 0x04000B38 RID: 2872
		private string mName;

		// Token: 0x04000B39 RID: 2873
		private Rectangle mRectangle;

		// Token: 0x04000B3A RID: 2874
		private bool mHighlighted;

		// Token: 0x04000B3B RID: 2875
		private int mIndex;

		// Token: 0x04000B3C RID: 2876
		private Vector2 mIconPosition;

		// Token: 0x04000B3D RID: 2877
		private Vector2 mIconTextPosition;

		// Token: 0x04000B3E RID: 2878
		private Vector2 mNamePosition;

		// Token: 0x04000B3F RID: 2879
		private string mIconText;

		// Token: 0x04000B40 RID: 2880
		private Texture2D mKeyIcon;

		// Token: 0x04000B41 RID: 2881
		public bool PULSE;

		// Token: 0x04000B42 RID: 2882
		public bool ENABLED;

		// Token: 0x04000B43 RID: 2883
		private Color mColor;

		// Token: 0x04000B44 RID: 2884
		private static int ICON_WIDTH = 24;

		// Token: 0x04000B45 RID: 2885
		private static int ICON_WIDTH_LONG = 48;

		// Token: 0x04000B47 RID: 2887
		private int mIconWidth;

		// Token: 0x04000B48 RID: 2888
		private int mIconWidthFudge;
	}
}
