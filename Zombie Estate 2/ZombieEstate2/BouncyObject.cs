using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000DB RID: 219
	internal class BouncyObject
	{
		// Token: 0x060005B6 RID: 1462 RVA: 0x0002B79C File Offset: 0x0002999C
		public BouncyObject(Texture2D tex, int startX, int startY, int endY, int scaleX, int scaleY, Rectangle src, float delay)
		{
			this.dest = new Rectangle(startX, startY, scaleX, scaleY);
			this.EndY = endY;
			this.tex = tex;
			this.scaleX = scaleX;
			this.scaleY = scaleY;
			this.src = src;
			this.delay = delay;
			this.x = (float)startX;
			this.y = (float)startY;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0002B82C File Offset: 0x00029A2C
		public BouncyObject(Texture2D tex, int startX, int startY, int endY, int scaleX, int scaleY, float delay) : this(tex, startX, startY, endY, scaleX, scaleY, new Rectangle(0, 0, tex.Height, tex.Width), delay)
		{
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0002B860 File Offset: 0x00029A60
		public BouncyObject(Texture2D tex, int startX, int startY, int endY, int scaleX, int scaleY, float delay, Rectangle src, Rectangle crop, float squish) : this(tex, startX, startY, endY, scaleX, scaleY, src, delay)
		{
			this.Crop = crop;
			this.CropTex = new Texture2D(Global.GraphicsDevice, this.Crop.Width, this.Crop.Height);
			this.RastState = new RasterizerState();
			this.RastState.ScissorTestEnable = true;
			this.Squishyness = squish;
			this.Cropped = true;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0002B8D4 File Offset: 0x00029AD4
		public void Update(float elapsed)
		{
			if (this.state == BouncyObject.SquishState.Delay)
			{
				this.delay -= elapsed;
				if (this.delay <= 0f)
				{
					this.state = BouncyObject.SquishState.Falling;
				}
			}
			if (this.state == BouncyObject.SquishState.Falling)
			{
				this.Fall(elapsed);
				return;
			}
			if (this.state == BouncyObject.SquishState.Squish)
			{
				this.Squish(elapsed);
				return;
			}
			if (this.state == BouncyObject.SquishState.Rebound)
			{
				this.Rebound(elapsed);
				return;
			}
			if (this.state == BouncyObject.SquishState.Set)
			{
				this.Set(elapsed);
				return;
			}
			BouncyObject.SquishState squishState = this.state;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0002B958 File Offset: 0x00029B58
		public void Draw(SpriteBatch spriteBatch)
		{
			this.GetRect();
			if (this.Cropped)
			{
				this.DrawCrop(spriteBatch);
				return;
			}
			spriteBatch.Draw(this.tex, this.dest, new Rectangle?(this.src), Color.White);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0002B994 File Offset: 0x00029B94
		private void GetRect()
		{
			this.dest.X = (int)this.x;
			this.dest.Y = (int)this.y;
			int num = (int)((float)this.scaleX * this.squishFactor);
			int num2 = (int)((float)this.scaleY / this.squishFactor);
			this.dest.X = this.dest.X + (this.scaleX - num) / 2;
			this.dest.Y = this.dest.Y + (this.scaleY - num2);
			this.dest.Width = num;
			this.dest.Height = num2;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0002BA30 File Offset: 0x00029C30
		private void DrawCrop(SpriteBatch spriteBatch)
		{
			if (!Global.Game.IsActive)
			{
				return;
			}
			Global.GraphicsDevice.ScissorRectangle = this.Crop;
			RasterizerState rasterizerState = Global.GraphicsDevice.RasterizerState;
			Global.GraphicsDevice.RasterizerState = this.RastState;
			spriteBatch.Draw(this.tex, this.dest, new Rectangle?(this.src), Color.White);
			try
			{
				Global.GraphicsDevice.ScissorRectangle = Global.ScreenRect;
			}
			catch
			{
			}
			Global.GraphicsDevice.RasterizerState = rasterizerState;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0002BAC8 File Offset: 0x00029CC8
		private void Fall(float elapsed)
		{
			if (this.yVel < 32f)
			{
				this.yVel += BouncyObject.Gravity * elapsed;
			}
			if (this.y + (float)this.dest.Height < (float)this.EndY)
			{
				this.y += this.yVel;
			}
			if (this.y + (float)this.dest.Height > (float)this.EndY)
			{
				this.y = (float)(this.EndY - this.dest.Height);
				this.state = BouncyObject.SquishState.Squish;
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0002BB64 File Offset: 0x00029D64
		private void Squish(float elapsed)
		{
			if (this.squishFactor >= 1f + 0.8f * this.Squishyness)
			{
				this.squishFactor = 1f + 0.8f * this.Squishyness;
				this.state = BouncyObject.SquishState.Rebound;
				return;
			}
			this.squishFactor += elapsed * 7f * this.Squishyness;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0002BBC8 File Offset: 0x00029DC8
		private void Rebound(float elapsed)
		{
			if (this.squishFactor <= 1f - 0.15f * this.Squishyness)
			{
				this.squishFactor = 1f - 0.15f * this.Squishyness;
				this.state = BouncyObject.SquishState.Set;
				return;
			}
			this.squishFactor -= elapsed * 5f * this.Squishyness;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0002BC2A File Offset: 0x00029E2A
		private void Set(float elapsed)
		{
			if (this.squishFactor > 1f)
			{
				this.squishFactor = 1f;
				this.state = BouncyObject.SquishState.Settled;
				return;
			}
			this.squishFactor += elapsed * 0.85f * this.Squishyness;
		}

		// Token: 0x040005A1 RID: 1441
		private Texture2D tex;

		// Token: 0x040005A2 RID: 1442
		private float squishFactor = 1f;

		// Token: 0x040005A3 RID: 1443
		private Rectangle src;

		// Token: 0x040005A4 RID: 1444
		private Rectangle dest;

		// Token: 0x040005A5 RID: 1445
		private int scaleX = 1;

		// Token: 0x040005A6 RID: 1446
		private int scaleY = 1;

		// Token: 0x040005A7 RID: 1447
		private int EndY;

		// Token: 0x040005A8 RID: 1448
		private float x;

		// Token: 0x040005A9 RID: 1449
		private float y;

		// Token: 0x040005AA RID: 1450
		private float yVel;

		// Token: 0x040005AB RID: 1451
		private static float Gravity = 25f;

		// Token: 0x040005AC RID: 1452
		private BouncyObject.SquishState state = BouncyObject.SquishState.Delay;

		// Token: 0x040005AD RID: 1453
		private float delay;

		// Token: 0x040005AE RID: 1454
		private Rectangle Crop;

		// Token: 0x040005AF RID: 1455
		private bool Cropped;

		// Token: 0x040005B0 RID: 1456
		private Texture2D CropTex;

		// Token: 0x040005B1 RID: 1457
		private RasterizerState RastState;

		// Token: 0x040005B2 RID: 1458
		private float Squishyness = 1f;

		// Token: 0x02000210 RID: 528
		public enum SquishState
		{
			// Token: 0x04000E15 RID: 3605
			Falling,
			// Token: 0x04000E16 RID: 3606
			Squish,
			// Token: 0x04000E17 RID: 3607
			Rebound,
			// Token: 0x04000E18 RID: 3608
			Set,
			// Token: 0x04000E19 RID: 3609
			Settled,
			// Token: 0x04000E1A RID: 3610
			Delay
		}
	}
}
