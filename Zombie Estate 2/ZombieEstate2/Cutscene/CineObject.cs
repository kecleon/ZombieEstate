using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Cutscene
{
	// Token: 0x020001D7 RID: 471
	public class CineObject : GameObject
	{
		// Token: 0x06000C88 RID: 3208 RVA: 0x00067BE4 File Offset: 0x00065DE4
		public CineObject()
		{
			this.scale = 0.4f;
			this.textColor = Color.DarkRed;
			this.TextureCoord = new Point(0, 1);
			this.startingTex = this.TextureCoord;
			this.ActivateObject(new Vector3(10f, 0f, 10f), this.TextureCoord);
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00067C51 File Offset: 0x00065E51
		public CineObject(string name, Point tex, Vector3 pos) : this(name, tex, pos, Color.Black)
		{
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00067C64 File Offset: 0x00065E64
		public CineObject(string name, Point tex, Vector3 pos, Color textColor)
		{
			this.scale = 0.4f;
			this.textColor = textColor;
			this.ObjectID = name;
			this.startingTex = tex;
			this.TextureCoord = tex;
			this.ActivateObject(pos, tex);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00067CB4 File Offset: 0x00065EB4
		public override void Update(float elapsed)
		{
			if (this.talking && this.talkTimer.Expired())
			{
				this.talkTimer = null;
				this.talking = false;
			}
			if (this.walking)
			{
				this.UpdateFacing();
				this.BounceEnabled = true;
				this.Position.X = this.Position.X + this.movement.X * elapsed * this.walkSpeed;
				this.Position.Z = this.Position.Z + this.movement.Y * elapsed * this.walkSpeed;
				if (this.walkTimer.Expired())
				{
					this.walkTimer = null;
					this.walking = false;
					this.movement.X = 0f;
					this.movement.Y = 0f;
				}
			}
			else
			{
				this.BounceEnabled = false;
			}
			base.Update(elapsed);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00067D8D File Offset: 0x00065F8D
		public void Talk(Timer duration)
		{
			this.talking = true;
			this.talkTimer = duration;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00067D9D File Offset: 0x00065F9D
		public void Walk(Timer duration, Vector2 dir)
		{
			this.walking = true;
			this.movement = dir;
			this.walkTimer = duration;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00067DB4 File Offset: 0x00065FB4
		public Vector3 GetCinePosition()
		{
			return new Vector3(this.Position.X, this.floorHeight, this.Position.Z);
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00067DD7 File Offset: 0x00065FD7
		public override void DestroyObject()
		{
			this.ObjectID = "NULL";
			base.DestroyObject();
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00067DEC File Offset: 0x00065FEC
		private void UpdateFacing()
		{
			if (Math.Abs(this.movement.X) > Math.Abs(this.movement.Y))
			{
				if (this.movement.X < 0f)
				{
					this.facing = Facing.Left;
				}
				else
				{
					this.facing = Facing.Right;
				}
			}
			else if (this.movement.Y > 0f)
			{
				this.facing = Facing.Down;
			}
			else
			{
				this.facing = Facing.Up;
			}
			if (this.facing == Facing.Left)
			{
				this.TextureCoord.X = this.startingTex.X + 2;
			}
			if (this.facing == Facing.Down)
			{
				this.TextureCoord.X = this.startingTex.X;
			}
			if (this.facing == Facing.Up)
			{
				this.TextureCoord.X = this.startingTex.X + 3;
			}
			if (this.facing == Facing.Right)
			{
				this.TextureCoord.X = this.startingTex.X + 1;
			}
		}

		// Token: 0x04000D51 RID: 3409
		private bool talking;

		// Token: 0x04000D52 RID: 3410
		private Timer talkTimer;

		// Token: 0x04000D53 RID: 3411
		private Timer walkTimer;

		// Token: 0x04000D54 RID: 3412
		public Color textColor;

		// Token: 0x04000D55 RID: 3413
		private Facing facing;

		// Token: 0x04000D56 RID: 3414
		private Vector2 movement;

		// Token: 0x04000D57 RID: 3415
		private Point startingTex;

		// Token: 0x04000D58 RID: 3416
		private bool walking;

		// Token: 0x04000D59 RID: 3417
		private float walkSpeed = 2.5f;

		// Token: 0x04000D5A RID: 3418
		public string ObjectID;
	}
}
