using System;
using EventInputNamespace;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Steamworks;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2.UI.Chat
{
	// Token: 0x02000171 RID: 369
	public class ChatUI
	{
		// Token: 0x06000B28 RID: 2856 RVA: 0x0005C164 File Offset: 0x0005A364
		public ChatUI(Rectangle pos, bool neverFade, Color bgColor, int bottomHeight)
		{
			this.BOTTOM_HEIGHT = bottomHeight;
			EventInput.CharEntered += this.EventInput_CharEntered;
			this.mNeverFade = neverFade;
			this.mPosition = pos;
			this.mBGColor = bgColor;
			int size = (this.mPosition.Height - this.BOTTOM_HEIGHT) / Global.StoreFont.LineSpacing - 4;
			this.mQueue = new FixedSizedQueue<ChatItem>(size);
			this.mAlpha = 1f;
			this.mQueue.Enqueue(new ChatItem("Press 'T' to Chat!", Color.White));
			this.mSendButton = new ZEButton(new Rectangle(this.mPosition.Right - 120, this.mPosition.Bottom + 2, 118, 32), "Send", "Enter");
			Global.GameChat = this;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0005C275 File Offset: 0x0005A475
		public void Destroy()
		{
			this.mUnlockNextFrame = false;
			InputManager.LOCK_INPUT = false;
			EventInput.CharEntered -= this.EventInput_CharEntered;
			Global.GameChat = null;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0005C29C File Offset: 0x0005A49C
		public void Update(float elapsed)
		{
			if (this.mUnlockNextFrame)
			{
				this.mUnlockNextFrame = false;
				InputManager.LOCK_INPUT = false;
			}
			if (InputManager.ButtonPressed(Keys.Escape, 0) && this.mTypingActive)
			{
				this.mTypingActive = false;
				this.mText = "";
				this.mUnlockNextFrame = true;
				return;
			}
			if (InputManager.ButtonPressed(Keys.Enter, 0) && this.mTypingActive)
			{
				this.mTypingActive = false;
				if (this.mText.Length == 0 || string.IsNullOrWhiteSpace(this.mText))
				{
					this.mUnlockNextFrame = true;
					return;
				}
				this.TextEntered(this.mText, SteamHelper.GetLocalID());
				NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.ChatMsg);
				netMessage.Payload = new Msg_ChatMsg
				{
					Message = this.mText
				};
				NetworkManager.SendMessage(netMessage, SendType.Reliable);
				this.mText = "";
				this.mUnlockNextFrame = true;
			}
			if (InputManager.ButtonPressed(Keys.T, 0) && !this.mTypingActive)
			{
				this.mTypingActive = true;
				this.mAlpha = 1f;
				this.mFadeTime = ChatUI.FADE_OUT_TIME;
				InputManager.LOCK_INPUT = true;
			}
			if (this.mTypingActive)
			{
				this.mAlpha = 1f;
				this.mFadeTime = ChatUI.FADE_OUT_TIME;
				return;
			}
			if (this.mNeverFade)
			{
				return;
			}
			if (this.mFadeTime <= 0f && this.mAlpha > 0f)
			{
				this.mAlpha -= ChatUI.FADE_OUT_SPEED;
			}
			if (this.mFadeTime > 0f)
			{
				this.mFadeTime -= elapsed;
			}
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0005C414 File Offset: 0x0005A614
		public void TextEntered(string s, CSteamID id)
		{
			this.mAlpha = 1f;
			this.mFadeTime = ChatUI.FADE_OUT_TIME;
			PlayerInfo player = PlayerManager.GetPlayer(id);
			s = ((player == null) ? "" : player.PersonaName) + ": " + s;
			s.Trim();
			s.Replace(Environment.NewLine, "");
			s.Replace("\n", "");
			foreach (string s2 in VerchickMath.WrapTextCHAT(Global.StoreFont, s, (float)this.mPosition.Width).Split(new char[]
			{
				'\n'
			}))
			{
				this.mQueue.Enqueue(new ChatItem(s2, Global.GetHUDColor((player == null) ? 0 : player.Index)));
			}
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0005C4E0 File Offset: 0x0005A6E0
		public void TextEntered_System(string s)
		{
			this.mAlpha = 1f;
			this.mFadeTime = ChatUI.FADE_OUT_TIME * 2f;
			s.Trim();
			s.Replace(Environment.NewLine, "");
			s.Replace("\n", "");
			foreach (string s2 in VerchickMath.WrapTextCHAT(Global.StoreFont, s, (float)this.mPosition.Width).Split(new char[]
			{
				'\n'
			}))
			{
				this.mQueue.Enqueue(new ChatItem(s2, Color.Yellow));
			}
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0005C584 File Offset: 0x0005A784
		public void Draw(SpriteBatch spritebatch)
		{
			if (this.mAlpha <= 0f)
			{
				return;
			}
			spritebatch.Draw(Global.Pixel, this.mPosition, this.mBGColor * this.mAlpha * 0.25f);
			int num = 0;
			foreach (ChatItem chatItem in this.mQueue)
			{
				string text = chatItem.Text;
				Shadow.DrawString(text, Global.StoreFont, new Vector2((float)this.mPosition.X, (float)(this.mPosition.Y + num)), 1, chatItem.Color * this.mAlpha, Color.Black * this.mAlpha, spritebatch);
				num += (int)Global.StoreFont.MeasureString(text).Y + 1;
			}
			if (this.mTypingActive)
			{
				spritebatch.Draw(Global.Pixel, new Rectangle(this.mPosition.X, this.mPosition.Bottom - this.BOTTOM_HEIGHT, this.mPosition.Width, this.BOTTOM_HEIGHT), Color.Black * this.mAlpha * 0.25f);
				Shadow.DrawString(VerchickMath.WrapTextCHAT(Global.StoreFont, this.mText, (float)this.mPosition.Width) + ((Global.Pulse < 0.25f || Global.Pulse > 0.75f) ? "|" : ""), Global.StoreFont, new Vector2((float)this.mPosition.X, (float)(this.mPosition.Bottom - this.BOTTOM_HEIGHT)), 1, Color.White * this.mAlpha, Color.Black * this.mAlpha, spritebatch);
				this.mSendButton.Draw(spritebatch);
			}
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0005C774 File Offset: 0x0005A974
		private void EventInput_CharEntered(object sender, CharacterEventArgs e)
		{
			if (!this.mTypingActive)
			{
				return;
			}
			if (e.Character != '\b' && e.Character != '\b')
			{
				try
				{
					string text = VerchickMath.WrapTextCHAT(Global.StoreFont, this.mText + e.Character.ToString(), (float)this.mPosition.Width);
					if (this.mText.Length < ChatUI.MAX_CHAR_LENGTH && Global.StoreFont.MeasureString(text).Y < (float)this.BOTTOM_HEIGHT)
					{
						this.mText += e.Character.ToString();
					}
				}
				catch
				{
				}
				return;
			}
			if (this.mText.Length == 0)
			{
				return;
			}
			this.mText = this.mText.Remove(this.mText.Length - 1);
		}

		// Token: 0x04000BEE RID: 3054
		private static float FADE_OUT_TIME = 5f;

		// Token: 0x04000BEF RID: 3055
		private static float FADE_OUT_SPEED = 0.1f;

		// Token: 0x04000BF0 RID: 3056
		private static int MAX_CHAR_LENGTH = 100;

		// Token: 0x04000BF1 RID: 3057
		private int BOTTOM_HEIGHT = 96;

		// Token: 0x04000BF2 RID: 3058
		private ZEButton mSendButton;

		// Token: 0x04000BF3 RID: 3059
		private Rectangle mPosition = new Rectangle(0, 0, 300, 500);

		// Token: 0x04000BF4 RID: 3060
		private FixedSizedQueue<ChatItem> mQueue;

		// Token: 0x04000BF5 RID: 3061
		private bool mTypingActive;

		// Token: 0x04000BF6 RID: 3062
		private string mText = "";

		// Token: 0x04000BF7 RID: 3063
		private float mAlpha;

		// Token: 0x04000BF8 RID: 3064
		private float mFadeTime = ChatUI.FADE_OUT_TIME;

		// Token: 0x04000BF9 RID: 3065
		private bool mNeverFade;

		// Token: 0x04000BFA RID: 3066
		private Color mBGColor = Color.Black;

		// Token: 0x04000BFB RID: 3067
		private bool mUnlockNextFrame;
	}
}
