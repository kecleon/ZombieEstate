using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x0200012F RID: 303
	internal class TexturePicker
	{
		// Token: 0x06000880 RID: 2176 RVA: 0x0004722C File Offset: 0x0004542C
		public TexturePicker(Game game)
		{
			this.pixel = game.Content.Load<Texture2D>("Pixel");
			this.texture = Global.MasterEnvTex;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x000472C0 File Offset: 0x000454C0
		public TexturePicker(Game game, Texture2D tex)
		{
			this.pixel = game.Content.Load<Texture2D>("Pixel");
			this.texture = tex;
			this.masterTex = true;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00047358 File Offset: 0x00045558
		public void Update()
		{
			MouseState state = Mouse.GetState();
			this.updateView(state);
			if (state.LeftButton == ButtonState.Pressed && this.prevMouse.LeftButton == ButtonState.Released && this.RectLocation.Contains(state.X, state.Y))
			{
				int num = state.X - this.RectLocation.X + this.RectTexture.X;
				int num2 = state.Y - this.RectLocation.Y + this.RectTexture.Y;
				num = (int)((float)num / TileBuilder.TileSize);
				num2 = (int)((float)num2 / TileBuilder.TileSize);
				Terminal.WriteMessage(num + "," + num2, MessageType.COMMAND);
				Editor.SelectedTexCoords = new Point(num, num2);
			}
			this.prevMouse = state;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0004742D File Offset: 0x0004562D
		public Point GetPoint()
		{
			return Editor.SelectedTexCoords;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00047434 File Offset: 0x00045634
		public void SetPoint(Point point)
		{
			Editor.SelectedTexCoords = point;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0004743C File Offset: 0x0004563C
		private void updateView(MouseState mouse)
		{
			if (mouse.RightButton == ButtonState.Pressed && (this.RectLocation.Contains(mouse.X, mouse.Y) || this.prevMouse.RightButton == ButtonState.Pressed))
			{
				this.RectTexture.X = this.RectTexture.X - (mouse.X - this.prevMouse.X);
				this.RectTexture.Y = this.RectTexture.Y - (mouse.Y - this.prevMouse.Y);
				if (this.masterTex)
				{
					this.RectTexture.X = (int)MathHelper.Clamp((float)this.RectTexture.X, 0f, 897f);
					this.RectTexture.Y = (int)MathHelper.Clamp((float)this.RectTexture.Y, 0f, 897f);
					return;
				}
				this.RectTexture.X = (int)MathHelper.Clamp((float)this.RectTexture.X, 0f, TileBuilder.TileSetSize - 127f);
				this.RectTexture.Y = (int)MathHelper.Clamp((float)this.RectTexture.Y, 0f, TileBuilder.TileSetSize - 127f);
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00047578 File Offset: 0x00045778
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.pixel, this.RectLocation, Color.DarkBlue);
			spriteBatch.Draw(this.texture, this.RectLocation, new Rectangle?(this.RectTexture), Color.White);
			Rectangle rectangle = new Rectangle(this.RectLocation.X + Editor.SelectedTexCoords.X * (int)TileBuilder.TileSize - this.RectTexture.X, this.RectLocation.Y + Editor.SelectedTexCoords.Y * (int)TileBuilder.TileSize - this.RectTexture.Y, (int)TileBuilder.TileSize, (int)TileBuilder.TileSize);
			if (rectangle.X >= this.RectLocation.X && rectangle.Y >= this.RectLocation.Y && rectangle.X + rectangle.Width < this.RectLocation.X + this.RectLocation.Width && rectangle.Y + rectangle.Height < this.RectLocation.Y + this.RectLocation.Height)
			{
				spriteBatch.Draw(this.pixel, rectangle, this.highlightColor);
			}
		}

		// Token: 0x04000941 RID: 2369
		private Rectangle RectLocation = new Rectangle(Global.ScreenRect.Width - 128, 360, 128, 128);

		// Token: 0x04000942 RID: 2370
		private Rectangle RectTexture = new Rectangle(0, 0, 128, 128);

		// Token: 0x04000943 RID: 2371
		private Texture2D texture;

		// Token: 0x04000944 RID: 2372
		private Texture2D pixel;

		// Token: 0x04000945 RID: 2373
		private Color highlightColor = new Color(255, 255, 255, 150);

		// Token: 0x04000946 RID: 2374
		private MouseState prevMouse;

		// Token: 0x04000947 RID: 2375
		private bool masterTex;
	}
}
