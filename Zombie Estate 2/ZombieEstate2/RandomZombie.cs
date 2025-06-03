using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000EC RID: 236
	internal class RandomZombie
	{
		// Token: 0x06000648 RID: 1608 RVA: 0x0002E8FC File Offset: 0x0002CAFC
		public RandomZombie(bool type)
		{
			this.position = new Rectangle(Global.rand.Next(320, Global.ScreenRect.Width - 320), Global.ScreenRect.Height + 132, 128, 128);
			this.y = (float)this.position.Y;
			if (type)
			{
				this.src = Global.GetTexRectange(56, 0);
				return;
			}
			this.src = Global.GetTexRectange(60, 6);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0002E98C File Offset: 0x0002CB8C
		public void Update()
		{
			if (this.active)
			{
				if (this.goingUp)
				{
					this.y -= 1f;
					if (this.position.Y <= 620)
					{
						this.goingUp = false;
						return;
					}
				}
				else
				{
					this.y += 1f;
					if (this.position.Y >= 852)
					{
						this.goingUp = true;
						this.active = false;
						return;
					}
				}
			}
			else if (Global.rand.Next(0, 300) < 2)
			{
				this.position = new Rectangle(Global.rand.Next(320, Global.ScreenRect.Width - 320), Global.ScreenRect.Height + 132, 128, 128);
				this.y = (float)this.position.Y;
				this.active = true;
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0002EA7B File Offset: 0x0002CC7B
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.active)
			{
				this.position.Y = (int)this.y;
				spriteBatch.Draw(Global.MasterTexture, this.position, new Rectangle?(this.src), Color.White);
			}
		}

		// Token: 0x04000603 RID: 1539
		private Rectangle src;

		// Token: 0x04000604 RID: 1540
		private Rectangle position;

		// Token: 0x04000605 RID: 1541
		private bool active;

		// Token: 0x04000606 RID: 1542
		private bool goingUp = true;

		// Token: 0x04000607 RID: 1543
		private float y;
	}
}
