using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000113 RID: 275
	public class GameObject : SyncedObject
	{
		// Token: 0x0600076D RID: 1901 RVA: 0x0003A878 File Offset: 0x00038A78
		public GameObject()
		{
			this.Position = new Vector3(0f, 0f, 0f);
			this.PrevPosition = new Vector3(0f, 0f, 0f);
			this.TextureCoord = new Point(0, 0);
			this.twoDPosition.X = this.Position.X;
			this.twoDPosition.Y = this.Position.Z;
			this.Active = false;
			this.RotationMatrix = Matrix.CreateRotationZ(this.ZRotation) * Matrix.CreateRotationX(this.XRotation) * Matrix.CreateRotationY(this.YRotation);
			this.ScaleMatrix = Matrix.CreateScale(this.scale);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0003AA60 File Offset: 0x00038C60
		public virtual void Update(float elapsed)
		{
			if (this.secondaryObject)
			{
				return;
			}
			this.AddVelocity(elapsed);
			this.addGravity(elapsed);
			this.twoDPosition.X = this.Position.X;
			this.twoDPosition.Y = this.Position.Z;
			this.BounceCurrentTime += elapsed;
			if (this.BounceEnabled && this.Bouncing && this.BounceSpeed <= this.BounceCurrentTime)
			{
				this.BounceCurrentTime = 0f;
				this.Bouncing = false;
			}
			else if (this.BounceEnabled && !this.Bouncing && this.BounceSpeed <= this.BounceCurrentTime)
			{
				this.BounceCurrentTime = 0f;
				this.Bouncing = true;
			}
			this.UpdateSquish(elapsed);
			this.ApplySquish();
			this.PrevPosition = this.Position;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0003AB3C File Offset: 0x00038D3C
		public void UpdateTile()
		{
			this.tile = Global.Level.GetTileAtLocation(this.Position);
			if (this.tile != null && this.tile != this.prevTile)
			{
				if (this.prevTile != null)
				{
					this.prevTile.RemoveObjectFromContainedList(this);
				}
				this.tile.AddObjectToContainedList(this);
			}
			this.prevTile = this.tile;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0003ABA4 File Offset: 0x00038DA4
		public void UpdateTransform()
		{
			if (!this.Active)
			{
				this.transform = GameObject.InactiveTransform;
				return;
			}
			if (this.BounceEnabled && this.Bouncing)
			{
				this.Position.Y = this.Position.Y + this.BounceHeight;
				this.CalcTransform();
				this.Position.Y = this.Position.Y - this.BounceHeight;
			}
			else
			{
				this.CalcTransform();
			}
			this.preScale = this.scale;
			this.preXScale = this.xScale;
			this.preYScale = this.yScale;
			this.preXRot = this.XRotation;
			this.preYRot = this.YRotation;
			this.preZRot = this.ZRotation;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0003AC58 File Offset: 0x00038E58
		private void CalcTransform()
		{
			if (this.preXRot == this.XRotation && this.preYRot == this.YRotation && this.preZRot == this.ZRotation)
			{
				Matrix rotationMatrix = this.RotationMatrix;
			}
			else
			{
				this.RotationMatrix = Matrix.Multiply(Matrix.Multiply(Matrix.CreateRotationZ(this.ZRotation), Matrix.CreateRotationX(this.XRotation)), Matrix.CreateRotationY(this.YRotation));
			}
			if (this.preScale == this.scale && this.preXScale == this.xScale && this.preYScale == this.yScale)
			{
				Matrix scaleMatrix = this.ScaleMatrix;
			}
			else
			{
				Matrix.CreateScale(this.xScale * this.scale, this.yScale * this.scale, this.scale, out this.ScaleMatrix);
			}
			this.tempTrans.X = this.Position.X;
			this.tempTrans.Y = this.Position.Y - (1f - this.yScale) * this.scale;
			this.tempTrans.Z = this.Position.Z;
			this.transform = Matrix.Multiply(Matrix.Multiply(this.ScaleMatrix, this.RotationMatrix), Matrix.CreateTranslation(this.tempTrans));
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0003ADA4 File Offset: 0x00038FA4
		public virtual void ActivateObject(Vector3 pos, Point texCoord)
		{
			this.Position = pos;
			this.TextureCoord = texCoord;
			this.Active = true;
			this.twoDPosition.X = this.Position.X;
			this.twoDPosition.Y = this.Position.Z;
			this.UpdateTransform();
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0003ADF8 File Offset: 0x00038FF8
		public Matrix GetTransform()
		{
			if (!this.Active)
			{
				return GameObject.InactiveTransform;
			}
			return this.transform;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0003AE10 File Offset: 0x00039010
		public Vector2 GetTextureNumber()
		{
			if (this.Invisible)
			{
				int num = 64512;
				num += 63;
				this.texNumber.X = (float)num;
				this.texNumber.Y = 1f;
				return this.texNumber;
			}
			int x = this.TextureCoord.X;
			int y = this.TextureCoord.Y;
			int num2 = x * 1024;
			num2 += y;
			this.texNumber.X = (float)num2;
			this.texNumber.Y = this.TexScale;
			return this.texNumber;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0003AE9E File Offset: 0x0003909E
		public Vector2 TwoDPosition()
		{
			return this.twoDPosition;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0003AEA8 File Offset: 0x000390A8
		public virtual void DestroyObject()
		{
			if (this.Indicator2D != null)
			{
				Indicator2D.Indicators.Remove(this.Indicator2D);
				this.Indicator2D = null;
			}
			this.tile = Global.Level.GetTileAtLocation(this.Position);
			if (this.tile != null)
			{
				this.tile.RemoveObjectFromContainedList(this);
				if (this.prevTile != null)
				{
					this.prevTile.RemoveObjectFromContainedList(this);
				}
			}
			this.Active = false;
			base.CLEAR_UID();
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0003AF20 File Offset: 0x00039120
		public virtual void BaseDestroyObject()
		{
			if (this.Indicator2D != null)
			{
				Indicator2D.Indicators.Remove(this.Indicator2D);
				this.Indicator2D = null;
			}
			this.tile = Global.Level.GetTileAtLocation(this.Position);
			if (this.tile != null)
			{
				this.tile.RemoveObjectFromContainedList(this);
				if (this.prevTile != null)
				{
					this.prevTile.RemoveObjectFromContainedList(this);
				}
			}
			this.Active = false;
			base.CLEAR_UID();
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0003AF98 File Offset: 0x00039198
		private void addGravity(float elapsed)
		{
			if (this.AffectedByGravity)
			{
				if (this.Velocity.Y > this.maxFallSpeed)
				{
					this.Velocity.Y = this.Velocity.Y - Global.Gravity * elapsed * this.Mass;
				}
				if (this.Velocity.Y < this.maxFallSpeed)
				{
					this.Velocity.Y = this.maxFallSpeed;
				}
				if (this.Position.Y < this.floorHeight)
				{
					if (this.PrevPosition.Y > this.floorHeight)
					{
						this.Landed();
					}
					this.Position.Y = this.floorHeight;
					this.Velocity.Y = 0f;
				}
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0003B051 File Offset: 0x00039251
		public bool OnFloor()
		{
			return this.Position.Y == this.floorHeight;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Landed()
		{
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0003B06C File Offset: 0x0003926C
		public virtual void AddVelocity(float elapsed)
		{
			if (this.Velocity.Length() > 30f)
			{
				this.Velocity.Normalize();
				this.Velocity *= 30f;
			}
			this.Position += this.Velocity * elapsed / this.Mass;
			float num = 0.5f;
			float num2 = this.groundFriction;
			if (!this.OnFloor())
			{
				num2 = this.airFriction;
			}
			if (this.Velocity.X < -num)
			{
				this.Velocity.X = this.Velocity.X + num2 * elapsed;
			}
			else if (this.Velocity.X > num)
			{
				this.Velocity.X = this.Velocity.X - num2 * elapsed;
			}
			else
			{
				this.Velocity.X = 0f;
			}
			if (this.Velocity.Z < -num)
			{
				this.Velocity.Z = this.Velocity.Z + num2 * elapsed;
				return;
			}
			if (this.Velocity.Z > num)
			{
				this.Velocity.Z = this.Velocity.Z - num2 * elapsed;
				return;
			}
			this.Velocity.Z = 0f;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0003B198 File Offset: 0x00039398
		private void UpdateSquish(float elapsed)
		{
			if (this.StateOfSquish == GameObject.SquishState.Idle)
			{
				return;
			}
			if (this.StateOfSquish == GameObject.SquishState.Squishing)
			{
				if (this.Squish >= this.SquishFactor)
				{
					this.Squish = this.SquishFactor;
					this.StateOfSquish = GameObject.SquishState.Rebound;
					return;
				}
				this.Squish += elapsed * 12f * this.Squishyness;
				return;
			}
			else if (this.StateOfSquish == GameObject.SquishState.Rebound)
			{
				if (this.Squish <= 1f - 0.1f * this.SquishFactor)
				{
					this.Squish = 1f - 0.1f * this.SquishFactor;
					this.StateOfSquish = GameObject.SquishState.Setting;
					return;
				}
				this.Squish -= elapsed * 3.5f * this.Squishyness;
				return;
			}
			else if (this.StateOfSquish == GameObject.SquishState.Setting)
			{
				if (this.Squish > 1f)
				{
					this.Squish = 1f;
					this.StateOfSquish = GameObject.SquishState.Idle;
					return;
				}
				this.Squish += elapsed * 1.5f * this.Squishyness;
				return;
			}
			else if (this.StateOfSquish == GameObject.SquishState.Stretch)
			{
				if (this.Squish <= 1f - this.SquishFactor)
				{
					this.Squish = 1f - this.SquishFactor;
					return;
				}
				this.Squish -= elapsed * 2.5f * this.Squishyness;
				return;
			}
			else
			{
				if (this.StateOfSquish != GameObject.SquishState.StretchThenReset)
				{
					return;
				}
				if (this.Squish <= 1f - this.SquishFactor)
				{
					this.Squish = 1f - this.SquishFactor;
					this.StateOfSquish = GameObject.SquishState.Setting;
					return;
				}
				this.Squish -= elapsed * 2.5f * this.Squishyness;
				return;
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0003B338 File Offset: 0x00039538
		private void ApplySquish()
		{
			this.xScale = this.Squish;
			this.yScale = 1f / this.Squish;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0003B358 File Offset: 0x00039558
		public void SquishMe(float amount)
		{
			this.StateOfSquish = GameObject.SquishState.Squishing;
			this.SquishFactor = amount;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0003B368 File Offset: 0x00039568
		public void SquishMe(float amount, bool reset)
		{
			if (!reset && this.StateOfSquish != GameObject.SquishState.Idle)
			{
				return;
			}
			this.SquishMe(amount);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0003B37D File Offset: 0x0003957D
		public void UnSquishMe(float amount)
		{
			this.StateOfSquish = GameObject.SquishState.Stretch;
			this.SquishFactor = amount;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0003B38D File Offset: 0x0003958D
		public void UnSquishMe(float amount, bool reset)
		{
			if (reset && this.StateOfSquish != GameObject.SquishState.Idle)
			{
				return;
			}
			if (this.StateOfSquish == GameObject.SquishState.StretchThenReset)
			{
				return;
			}
			this.StateOfSquish = GameObject.SquishState.StretchThenReset;
			this.SquishFactor = amount;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0003B3B3 File Offset: 0x000395B3
		public void GiveIndicator()
		{
			if (this.Indicator2D != null)
			{
				Indicator2D.Indicators.Remove(this.Indicator2D);
			}
			this.Indicator2D = new Indicator2D(this);
			Indicator2D.Indicators.Add(this.Indicator2D);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0003B3EA File Offset: 0x000395EA
		public Vector2 GetScreenPosCenter()
		{
			return VerchickMath.GetScreenPosition(new Vector3(this.Position.X + 0f, this.Position.Y, this.Position.Z));
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0003B41D File Offset: 0x0003961D
		public Vector2 GetScreenPosBottom()
		{
			return VerchickMath.GetScreenPosition(new Vector3(this.Position.X + 0f, this.Position.Y - this.scale * 1.4f, this.Position.Z));
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0003B45D File Offset: 0x0003965D
		public Vector2 GetScreenPosTop()
		{
			return VerchickMath.GetScreenPosition(new Vector3(this.Position.X + 0f, this.Position.Y + this.scale * 1.4f, this.Position.Z));
		}

		// Token: 0x04000778 RID: 1912
		public Vector3 Position;

		// Token: 0x04000779 RID: 1913
		public Vector3 PrevPosition;

		// Token: 0x0400077A RID: 1914
		public Vector3 Velocity = new Vector3(0f, 0f, 0f);

		// Token: 0x0400077B RID: 1915
		public float XRotation;

		// Token: 0x0400077C RID: 1916
		public float YRotation;

		// Token: 0x0400077D RID: 1917
		public float ZRotation;

		// Token: 0x0400077E RID: 1918
		private Matrix transform;

		// Token: 0x0400077F RID: 1919
		public bool Active;

		// Token: 0x04000780 RID: 1920
		public Point TextureCoord;

		// Token: 0x04000781 RID: 1921
		public float maxFallSpeed = -12f;

		// Token: 0x04000782 RID: 1922
		public float airFriction = 4.5f;

		// Token: 0x04000783 RID: 1923
		public float groundFriction = 10.5f;

		// Token: 0x04000784 RID: 1924
		public float Mass = 1f;

		// Token: 0x04000785 RID: 1925
		public float scale = 0.5f;

		// Token: 0x04000786 RID: 1926
		public float xScale = 1f;

		// Token: 0x04000787 RID: 1927
		public float yScale = 1f;

		// Token: 0x04000788 RID: 1928
		public Tile tile;

		// Token: 0x04000789 RID: 1929
		public Tile prevTile;

		// Token: 0x0400078A RID: 1930
		public bool AffectedByGravity = true;

		// Token: 0x0400078B RID: 1931
		public float floorHeight = 0.424f;

		// Token: 0x0400078C RID: 1932
		public Vector2 twoDPosition = new Vector2(0f, 0f);

		// Token: 0x0400078D RID: 1933
		public bool BounceEnabled;

		// Token: 0x0400078E RID: 1934
		public bool Bouncing;

		// Token: 0x0400078F RID: 1935
		public float BounceHeight = 0.05f;

		// Token: 0x04000790 RID: 1936
		public float BounceSpeed = 0.15f;

		// Token: 0x04000791 RID: 1937
		private float BounceCurrentTime;

		// Token: 0x04000792 RID: 1938
		public bool secondaryObject;

		// Token: 0x04000793 RID: 1939
		public static Matrix InactiveTransform = Matrix.CreateTranslation(new Vector3(0f, -100000f, 0f));

		// Token: 0x04000794 RID: 1940
		private int toggle = Global.rand.Next(0, 2);

		// Token: 0x04000795 RID: 1941
		public bool CollidesWithWalls;

		// Token: 0x04000796 RID: 1942
		private Matrix ScaleMatrix;

		// Token: 0x04000797 RID: 1943
		private Matrix RotationMatrix;

		// Token: 0x04000798 RID: 1944
		private float preXRot;

		// Token: 0x04000799 RID: 1945
		private float preYRot;

		// Token: 0x0400079A RID: 1946
		private float preZRot;

		// Token: 0x0400079B RID: 1947
		private float preScale;

		// Token: 0x0400079C RID: 1948
		private float preXScale;

		// Token: 0x0400079D RID: 1949
		private float preYScale;

		// Token: 0x0400079E RID: 1950
		private Vector2 texNumber = new Vector2(0f, 0f);

		// Token: 0x0400079F RID: 1951
		public float TexScale = 1f;

		// Token: 0x040007A0 RID: 1952
		private float Squish = 1f;

		// Token: 0x040007A1 RID: 1953
		private float SquishFactor = 1.5f;

		// Token: 0x040007A2 RID: 1954
		private float ReboundSquish = 1f;

		// Token: 0x040007A3 RID: 1955
		private GameObject.SquishState StateOfSquish;

		// Token: 0x040007A4 RID: 1956
		private float Squishyness = 0.5f;

		// Token: 0x040007A5 RID: 1957
		public bool Invisible;

		// Token: 0x040007A6 RID: 1958
		private Indicator2D Indicator2D;

		// Token: 0x040007A7 RID: 1959
		private Vector3 tempTrans = new Vector3(0f, 0f, 0f);

		// Token: 0x0200021A RID: 538
		private enum SquishState
		{
			// Token: 0x04000E32 RID: 3634
			Idle,
			// Token: 0x04000E33 RID: 3635
			Squishing,
			// Token: 0x04000E34 RID: 3636
			Rebound,
			// Token: 0x04000E35 RID: 3637
			Setting,
			// Token: 0x04000E36 RID: 3638
			Stretch,
			// Token: 0x04000E37 RID: 3639
			StretchThenReset
		}
	}
}
