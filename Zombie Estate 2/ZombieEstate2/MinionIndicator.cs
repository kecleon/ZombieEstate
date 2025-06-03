using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000060 RID: 96
	public class MinionIndicator
	{
		// Token: 0x06000220 RID: 544 RVA: 0x0000FC64 File Offset: 0x0000DE64
		public MinionIndicator(Player player, Vector2 pos)
		{
			this.Texs = new List<Point>();
			this.Texs.Add(this.EMPTY);
			this.Texs.Add(this.EMPTY);
			this.Texs.Add(this.EMPTY);
			this.Texs.Add(this.EMPTY);
			this.parent = player;
			this.position = pos;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000FCE4 File Offset: 0x0000DEE4
		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < this.parent.SpecialProperties.MinionCount + 1; i++)
			{
				spriteBatch.Draw(Global.MasterTexture, new Rectangle((int)this.position.X + i * 32, (int)this.position.Y, 32, 32), new Rectangle?(Global.GetTexRectange(this.Texs[i].X, this.Texs[i].Y)), Color.White);
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000FD70 File Offset: 0x0000DF70
		public void AddMinion(Minion m)
		{
			for (int i = 0; i < this.parent.MinionCount; i++)
			{
				if (this.Texs[i] == this.EMPTY)
				{
					this.Texs[i] = new Point(m.StartTextureCoord.X + 1, m.StartTextureCoord.Y);
					return;
				}
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000FDD8 File Offset: 0x0000DFD8
		public void RemoveMinion(Minion m)
		{
			int index = 0;
			foreach (Point point in this.Texs)
			{
				if (point.X == m.StartTextureCoord.X + 1 && point.Y == m.StartTextureCoord.Y)
				{
					index = this.Texs.IndexOf(point);
				}
			}
			this.Texs[index] = this.EMPTY;
		}

		// Token: 0x040001EA RID: 490
		public Player parent;

		// Token: 0x040001EB RID: 491
		private Vector2 position;

		// Token: 0x040001EC RID: 492
		private List<Point> Texs;

		// Token: 0x040001ED RID: 493
		private Point EMPTY = new Point(12, 37);
	}
}
