using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000F7 RID: 247
	public class WaveTip
	{
		// Token: 0x0600067A RID: 1658 RVA: 0x00030678 File Offset: 0x0002E878
		public WaveTip(string name)
		{
			if (WaveTip.Textures == null)
			{
				WaveTip.Textures = new Dictionary<string, Texture2D>();
			}
			if (!WaveTip.Textures.ContainsKey(name))
			{
				WaveTip.Textures.Add(name, Global.Content.Load<Texture2D>("Tips\\" + name));
			}
			this.myTex = WaveTip.Textures[name];
			this.StartPosition = new Vector2((float)(-(float)this.myTex.Width), (float)(Global.ScreenRect.Height / 2 - this.myTex.Height / 2));
			this.ShownPosition = new Vector2(0f, (float)(Global.ScreenRect.Height / 2 - this.myTex.Height / 2));
			this.Position = this.StartPosition;
			this.ShowTimer = new Timer(1f);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0003075B File Offset: 0x0002E95B
		public void Show()
		{
			this.ShowTimer.Reset();
			this.ShowTimer.Start();
			this.showing = true;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0003077A File Offset: 0x0002E97A
		public void Hide()
		{
			this.ShowTimer.Reset();
			this.ShowTimer.Start();
			this.showing = false;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0003079C File Offset: 0x0002E99C
		public void Update()
		{
			if (this.showing)
			{
				this.Position = Vector2.SmoothStep(this.StartPosition, this.ShownPosition, this.ShowTimer.Delta());
				return;
			}
			this.Position = Vector2.SmoothStep(this.ShownPosition, this.StartPosition, this.ShowTimer.Delta());
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000307F8 File Offset: 0x0002E9F8
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (this.showing)
			{
				spriteBatch.Draw(this.myTex, this.Position, Color.White * this.GetAlpha);
				return;
			}
			spriteBatch.Draw(this.myTex, this.Position, Color.White * this.GetAlpha);
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x00030852 File Offset: 0x0002EA52
		public float GetAlpha
		{
			get
			{
				if (this.showing)
				{
					return this.ShowTimer.Delta();
				}
				return 1f - this.ShowTimer.Delta();
			}
		}

		// Token: 0x04000658 RID: 1624
		private static Dictionary<string, Texture2D> Textures;

		// Token: 0x04000659 RID: 1625
		private Texture2D myTex;

		// Token: 0x0400065A RID: 1626
		private Vector2 StartPosition;

		// Token: 0x0400065B RID: 1627
		private Vector2 ShownPosition;

		// Token: 0x0400065C RID: 1628
		public Vector2 Position;

		// Token: 0x0400065D RID: 1629
		private Timer ShowTimer;

		// Token: 0x0400065E RID: 1630
		public bool showing = true;
	}
}
