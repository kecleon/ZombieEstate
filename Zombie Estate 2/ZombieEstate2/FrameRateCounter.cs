using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000111 RID: 273
	public class FrameRateCounter : DrawableGameComponent
	{
		// Token: 0x06000762 RID: 1890 RVA: 0x0003A585 File Offset: 0x00038785
		public FrameRateCounter(Game game) : base(game)
		{
			this.content = new ContentManager(game.Services);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0003A5AA File Offset: 0x000387AA
		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch(base.GraphicsDevice);
			this.spriteFont = this.content.Load<SpriteFont>("Content\\Font");
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0003A5D3 File Offset: 0x000387D3
		protected override void UnloadContent()
		{
			this.content.Unload();
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0003A5E0 File Offset: 0x000387E0
		public override void Update(GameTime gameTime)
		{
			this.elapsedTime += gameTime.ElapsedGameTime;
			if (this.elapsedTime > TimeSpan.FromSeconds(1.0))
			{
				this.elapsedTime -= TimeSpan.FromSeconds(1.0);
				this.frameRate = this.frameCounter;
				this.frameCounter = 0;
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0003A654 File Offset: 0x00038854
		public override void Draw(GameTime gameTime)
		{
			this.frameCounter++;
			string text = string.Format("fps: {0}", this.frameRate);
			this.spriteBatch.Begin();
			this.spriteBatch.DrawString(this.spriteFont, text, new Vector2(33f, 33f), Color.Black);
			this.spriteBatch.DrawString(this.spriteFont, text, new Vector2(32f, 32f), Color.White);
			this.spriteBatch.End();
		}

		// Token: 0x04000771 RID: 1905
		private ContentManager content;

		// Token: 0x04000772 RID: 1906
		private SpriteBatch spriteBatch;

		// Token: 0x04000773 RID: 1907
		private SpriteFont spriteFont;

		// Token: 0x04000774 RID: 1908
		private int frameRate;

		// Token: 0x04000775 RID: 1909
		private int frameCounter;

		// Token: 0x04000776 RID: 1910
		private TimeSpan elapsedTime = TimeSpan.Zero;
	}
}
