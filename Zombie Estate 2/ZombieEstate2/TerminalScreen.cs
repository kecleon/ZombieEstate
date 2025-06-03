using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x0200012E RID: 302
	internal class TerminalScreen
	{
		// Token: 0x06000879 RID: 2169 RVA: 0x00046788 File Offset: 0x00044988
		public TerminalScreen(Game game, int w, int h)
		{
			this.Hidden = true;
			this.game = game;
			this.Width = w;
			this.Height = h / 2;
			this.currentHeight = (float)(128 - this.Height);
			this.pixel = game.Content.Load<Texture2D>(this.contentDir + "Pixel");
			this.downArrow = game.Content.Load<Texture2D>(this.contentDir + "DownArrow");
			this.upArrow = game.Content.Load<Texture2D>(this.contentDir + "UpArrow");
			this.downArrowEnd = game.Content.Load<Texture2D>(this.contentDir + "DownArrowEnd");
			this.lockIcon = game.Content.Load<Texture2D>(this.contentDir + "Lock");
			this.hideIcon = game.Content.Load<Texture2D>(this.contentDir + "Hid");
			this.DRAW = (Config.Instance.DrawTerminal || Global.CHEAT);
			this.Messages = new List<Message>();
			this.Font = game.Content.Load<SpriteFont>(this.contentDir + "Font");
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00046914 File Offset: 0x00044B14
		public void Update()
		{
			if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.T) && this.prevKeyboardState.IsKeyUp(Keys.T))
			{
				this.DRAW = !this.DRAW;
			}
			this.prevKeyboardState = Keyboard.GetState();
			if (!this.DRAW)
			{
				return;
			}
			MouseState state = Mouse.GetState();
			KeyboardState state2 = Keyboard.GetState();
			if (state.LeftButton == ButtonState.Pressed && !this.Locked && !this.Hidden)
			{
				if (this.dragging || ((float)state.Y <= this.currentHeight + (float)this.Height && (float)state.Y >= this.currentHeight + (float)this.Height - 16f && state.X < 1200))
				{
					this.dragging = true;
					this.currentHeight = MathHelper.Lerp(this.currentHeight, (float)(state.Y - this.Height + 16), 0.6f);
					this.currentHeight = MathHelper.Clamp(this.currentHeight, (float)(16 - this.Height), 0f);
				}
			}
			else
			{
				this.dragging = false;
			}
			this.alpha = 255f + 255f * (this.currentHeight / (float)this.Height);
			this.alpha = MathHelper.Clamp(this.alpha, 50f, 180f);
			if (this.Hidden)
			{
				this.currentHeight = (float)(32 - this.Height);
			}
			this.Messages = Terminal.RecentMessages();
			this.UpdateButtons(state);
			this.ToggleButton(state2);
			this.prevMouseState = Mouse.GetState();
			this.prevKeyboardState = Keyboard.GetState();
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00046AEC File Offset: 0x00044CEC
		public void Draw(SpriteBatch spriteBatch)
		{
			if (!this.DRAW)
			{
				return;
			}
			if (!this.Hidden)
			{
				Rectangle rectangle = new Rectangle(0, (int)this.currentHeight, this.Width, this.Height);
				spriteBatch.Draw(this.pixel, rectangle, new Color(0, 0, 0, (int)((byte)this.alpha)));
				Rectangle destinationRectangle = new Rectangle(rectangle.X, rectangle.Y + this.Height - 16, this.Width, 16);
				spriteBatch.Draw(this.pixel, destinationRectangle, new Color(0, 0, 0, (int)((byte)this.alpha)));
				Vector2 pos = new Vector2((float)(this.Width / 2 - 8), this.currentHeight + (float)this.Height - 16f);
				if (!this.dragging)
				{
					Shadow.DrawShadowed(this.downArrow, pos, 1, Color.Gray, spriteBatch);
				}
				else
				{
					Shadow.DrawShadowed(this.downArrow, pos, 1, Color.White, spriteBatch);
				}
				this.DrawMessages(spriteBatch);
			}
			this.DrawButtons(spriteBatch);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00046BEC File Offset: 0x00044DEC
		private void DrawMessages(SpriteBatch spriteBatch)
		{
			int num = 0;
			while ((float)num < (float)this.Messages.Count - this.line)
			{
				Message message = this.Messages[num];
				Vector2 pos = new Vector2(0f, (float)this.Height + this.currentHeight + ((float)num + this.line) * 16f - (float)(this.Messages.Count * 16) - 14f);
				if (message.text == null)
				{
					Shadow.DrawString("NULL MESSAGE TEXT", this.Font, pos, 1, message.color, spriteBatch);
				}
				else
				{
					Shadow.DrawString(message.text, this.Font, pos, 1, message.color, spriteBatch);
				}
				num++;
			}
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00046CA8 File Offset: 0x00044EA8
		private void UpdateButtons(MouseState mouse)
		{
			this.UpButtonAct = false;
			this.DownButtonAct = false;
			this.EndButtonAct = false;
			this.UpButton = new Rectangle(this.Width - 32, (int)(this.currentHeight + (float)this.Height) - 64, 16, 16);
			this.DownButton = new Rectangle(this.Width - 32, (int)(this.currentHeight + (float)this.Height) - 48, 16, 16);
			this.EndButton = new Rectangle(this.Width - 64, (int)(this.currentHeight + (float)this.Height) - 48, 16, 16);
			this.LockButton = new Rectangle(this.Width - 64, (int)(this.currentHeight + (float)this.Height) - 17, 16, 16);
			this.HideButton = new Rectangle(this.Width - 32, (int)(this.currentHeight + (float)this.Height) - 17, 16, 16);
			if (mouse.LeftButton == ButtonState.Pressed && this.UpButton.Contains(new Point(mouse.X, mouse.Y)))
			{
				this.UpButtonAct = true;
				this.line += 1f;
			}
			if (mouse.LeftButton == ButtonState.Pressed && this.DownButton.Contains(new Point(mouse.X, mouse.Y)))
			{
				this.DownButtonAct = true;
				this.line -= 1f;
			}
			if (mouse.LeftButton == ButtonState.Pressed && this.EndButton.Contains(new Point(mouse.X, mouse.Y)))
			{
				this.EndButtonAct = true;
				this.line = 0f;
			}
			if (mouse.LeftButton == ButtonState.Pressed && this.LockButton.Contains(new Point(mouse.X, mouse.Y)) && this.prevMouseState.LeftButton == ButtonState.Released)
			{
				this.Locked = !this.Locked;
			}
			if (mouse.LeftButton == ButtonState.Pressed && this.HideButton.Contains(new Point(mouse.X, mouse.Y)) && this.prevMouseState.LeftButton == ButtonState.Released)
			{
				this.Hidden = !this.Hidden;
				if (this.Hidden)
				{
					Terminal.WriteMessage("Terminal hidden.", MessageType.TERMINAL);
				}
				else
				{
					Terminal.WriteMessage("Terminal shown.", MessageType.TERMINAL);
				}
			}
			this.line = MathHelper.Clamp(this.line, 0f, 200f);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00046F28 File Offset: 0x00045128
		private void ToggleButton(KeyboardState keyboard)
		{
			if (keyboard.IsKeyDown(Keys.T) && this.prevKeyboardState.IsKeyUp(Keys.T) && !this.activateTimer.Running())
			{
				if (this.activateTimer.Expired())
				{
					this.activateTimer.Start();
					this.lowered = !this.lowered;
				}
				else
				{
					this.activateTimer.Start();
				}
			}
			if (this.activateTimer.Running())
			{
				if (!this.lowered)
				{
					this.currentHeight = 16f - (float)this.Height * (1f - this.activateTimer.Delta()) + 5f;
				}
				else
				{
					this.currentHeight = 16f - (float)this.Height * this.activateTimer.Delta() - 5f;
				}
			}
			this.currentHeight = MathHelper.Clamp(this.currentHeight, (float)(16 - this.Height), 0f);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00047018 File Offset: 0x00045218
		private void DrawButtons(SpriteBatch spriteBatch)
		{
			if (this.Hidden)
			{
				Shadow.DrawShadowed(this.hideIcon, new Vector2((float)this.HideButton.X, (float)this.HideButton.Y), 1, Color.White, spriteBatch);
				return;
			}
			Shadow.DrawShadowed(this.hideIcon, new Vector2((float)this.HideButton.X, (float)this.HideButton.Y), 1, Color.Gray, spriteBatch);
			if (this.UpButtonAct)
			{
				Shadow.DrawShadowed(this.upArrow, new Vector2((float)this.UpButton.X, (float)this.UpButton.Y), 1, Color.White, spriteBatch);
			}
			else
			{
				Shadow.DrawShadowed(this.upArrow, new Vector2((float)this.UpButton.X, (float)this.UpButton.Y), 1, Color.Gray, spriteBatch);
			}
			if (this.DownButtonAct)
			{
				Shadow.DrawShadowed(this.downArrow, new Vector2((float)this.DownButton.X, (float)this.DownButton.Y), 1, Color.White, spriteBatch);
			}
			else
			{
				Shadow.DrawShadowed(this.downArrow, new Vector2((float)this.DownButton.X, (float)this.DownButton.Y), 1, Color.Gray, spriteBatch);
			}
			if (this.EndButtonAct)
			{
				Shadow.DrawShadowed(this.downArrowEnd, new Vector2((float)this.EndButton.X, (float)this.EndButton.Y), 1, Color.White, spriteBatch);
			}
			else
			{
				Shadow.DrawShadowed(this.downArrowEnd, new Vector2((float)this.EndButton.X, (float)this.EndButton.Y), 1, Color.Gray, spriteBatch);
			}
			if (this.Locked)
			{
				Shadow.DrawShadowed(this.lockIcon, new Vector2((float)this.LockButton.X, (float)this.LockButton.Y), 1, Color.White, spriteBatch);
				return;
			}
			Shadow.DrawShadowed(this.lockIcon, new Vector2((float)this.LockButton.X, (float)this.LockButton.Y), 1, Color.Gray, spriteBatch);
		}

		// Token: 0x04000922 RID: 2338
		private int Width = 1280;

		// Token: 0x04000923 RID: 2339
		private int Height = 360;

		// Token: 0x04000924 RID: 2340
		private float alpha = 100f;

		// Token: 0x04000925 RID: 2341
		private float currentHeight;

		// Token: 0x04000926 RID: 2342
		private string contentDir = "Terminal/";

		// Token: 0x04000927 RID: 2343
		private Texture2D pixel;

		// Token: 0x04000928 RID: 2344
		private Texture2D downArrow;

		// Token: 0x04000929 RID: 2345
		private Texture2D upArrow;

		// Token: 0x0400092A RID: 2346
		private Texture2D downArrowEnd;

		// Token: 0x0400092B RID: 2347
		private Texture2D lockIcon;

		// Token: 0x0400092C RID: 2348
		private Texture2D hideIcon;

		// Token: 0x0400092D RID: 2349
		private SpriteFont Font;

		// Token: 0x0400092E RID: 2350
		private Game game;

		// Token: 0x0400092F RID: 2351
		private MouseState prevMouseState;

		// Token: 0x04000930 RID: 2352
		private KeyboardState prevKeyboardState;

		// Token: 0x04000931 RID: 2353
		private List<Message> Messages;

		// Token: 0x04000932 RID: 2354
		private Rectangle UpButton;

		// Token: 0x04000933 RID: 2355
		private Rectangle DownButton;

		// Token: 0x04000934 RID: 2356
		private Rectangle LockButton;

		// Token: 0x04000935 RID: 2357
		private Rectangle EndButton;

		// Token: 0x04000936 RID: 2358
		private Rectangle HideButton;

		// Token: 0x04000937 RID: 2359
		private bool UpButtonAct;

		// Token: 0x04000938 RID: 2360
		private bool DownButtonAct;

		// Token: 0x04000939 RID: 2361
		private bool EndButtonAct;

		// Token: 0x0400093A RID: 2362
		private bool Locked;

		// Token: 0x0400093B RID: 2363
		private bool Hidden;

		// Token: 0x0400093C RID: 2364
		private bool dragging;

		// Token: 0x0400093D RID: 2365
		private float line;

		// Token: 0x0400093E RID: 2366
		private Timer activateTimer = new Timer(0.25f);

		// Token: 0x0400093F RID: 2367
		private bool lowered;

		// Token: 0x04000940 RID: 2368
		private bool DRAW;
	}
}
