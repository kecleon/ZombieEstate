using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200007D RID: 125
	public class Particle2D
	{
		// Token: 0x060002FF RID: 767 RVA: 0x0001780C File Offset: 0x00015A0C
		public Particle2D(Vector3 startPos, string type)
		{
			this.Position = startPos;
			Vector3 vector = Global.GraphicsDevice.Viewport.Project(this.Position, Global.Projection, Global.View, Matrix.Identity);
			this.ScreenPos = new Vector2(vector.X, vector.Y);
			this.Velocity = new Vector3(0f, 0f, 0f);
			this.Type = type;
			if (this.Type == "Munch")
			{
				this.myTex = Particle2D.TexMunch;
				this.gravity = 0.01f;
			}
			if (this.Type == "Zzz")
			{
				this.myTex = Particle2D.TexZzz;
				this.gravity = -0.01f;
				this.shrink = true;
			}
			this.liveTime = this.totalLiveTime;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0001790A File Offset: 0x00015B0A
		public Particle2D()
		{
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00017934 File Offset: 0x00015B34
		public virtual void Update(float elapsed)
		{
			this.liveTime -= elapsed;
			if (this.liveTime <= 0f || this.Position.Y < 0f)
			{
				this.DEAD = true;
				return;
			}
			if (this.shrink)
			{
				this.scale = this.liveTime / this.totalLiveTime;
			}
			this.Velocity.Y = this.Velocity.Y - this.gravity;
			this.Position.X = this.Position.X + this.Velocity.X * elapsed;
			this.Position.Y = this.Position.Y + this.Velocity.Y * elapsed;
			this.Position.Z = this.Position.Z + this.Velocity.Z * elapsed;
			this.GetScreenPosition();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00017A00 File Offset: 0x00015C00
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.myTex, this.ScreenPos, null, Color.White, 0f, new Vector2((float)(this.myTex.Width / 2), (float)(this.myTex.Height / 2)), this.scale, SpriteEffects.None, 0f);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00017A60 File Offset: 0x00015C60
		private void GetScreenPosition()
		{
			Vector3 vector = Global.GraphicsDevice.Viewport.Project(Vector3.Add(this.Position, new Vector3(0f, 0f, 0f)), Global.Projection, Global.View, Matrix.Identity);
			this.ScreenPos.X = vector.X - (float)(this.myTex.Width / 2);
			this.ScreenPos.Y = vector.Y;
		}

		// Token: 0x040002EA RID: 746
		private Vector3 Position;

		// Token: 0x040002EB RID: 747
		private Vector3 Velocity;

		// Token: 0x040002EC RID: 748
		public Vector2 ScreenPos;

		// Token: 0x040002ED RID: 749
		private float liveTime;

		// Token: 0x040002EE RID: 750
		private float gravity = 0.1f;

		// Token: 0x040002EF RID: 751
		private string Type;

		// Token: 0x040002F0 RID: 752
		public bool DEAD;

		// Token: 0x040002F1 RID: 753
		private Texture2D myTex;

		// Token: 0x040002F2 RID: 754
		private bool shrink;

		// Token: 0x040002F3 RID: 755
		private float scale = 1f;

		// Token: 0x040002F4 RID: 756
		public static Texture2D TexMunch;

		// Token: 0x040002F5 RID: 757
		public static Texture2D TexZzz;

		// Token: 0x040002F6 RID: 758
		private float totalLiveTime = 3f;
	}
}
