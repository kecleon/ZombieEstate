using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000131 RID: 305
	public class Tile
	{
		// Token: 0x06000887 RID: 2183 RVA: 0x000476A8 File Offset: 0x000458A8
		public Tile(int i, int j, float level, bool buildFloor = true)
		{
			this.level = level;
			if (Global.MainOnTop)
			{
				this.level = -this.level;
			}
			this.x = i;
			this.y = j;
			this.groundTexCoord = new Point(31, 31);
			this.ContainedObjects = new List<GameObject>();
			this.AdjacentObjects = new List<GameObject>();
			if (buildFloor)
			{
				this.groundVertices = TileBuilder.BuildFloor(this.x, this.y, this.groundTexCoord.X, this.groundTexCoord.Y, level, this.lightTexCoord.X, this.lightTexCoord.Y);
			}
			this.wallLightTexCoord = new Point[4];
			for (int k = 0; k < 4; k++)
			{
				this.wallLightTexCoord[k] = new Point(0, 0);
			}
			this.wallVertices = new CustomLevelVertexDec[0];
			this.walls = new bool[4];
			this.fakeWalls = new bool[4];
			this.halfWalls = new bool[4];
			this.windowWalls = new bool[4];
			this.wallTextureCoords = new Point[4];
			for (int l = 0; l < 4; l++)
			{
				this.wallTextureCoords[l] = new Point(0, 0);
			}
			this.TileProperties = new List<TilePropertyType>();
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00047924 File Offset: 0x00045B24
		public void ClearWallsAndFloor()
		{
			this.walls = new bool[4];
			this.fakeWalls = new bool[4];
			this.halfWalls = new bool[4];
			this.windowWalls = new bool[4];
			this.groundTexCoord = new Point(63, 63);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00047970 File Offset: 0x00045B70
		public void BuildAdjacentList()
		{
			this.AdjacentTiles = new List<Tile>();
			this.TouchingTiles = new List<Tile>();
			this.TouchingTiles.Add(this);
			if (Global.Level.InBounds(this.x - 1, this.y))
			{
				this.AdjacentTiles.Add(Global.Level.GetTile(this.x - 1, this.y));
				if (!this.HasLeftWall())
				{
					this.TouchingTiles.Add(Global.Level.GetTile(this.x - 1, this.y));
				}
			}
			if (Global.Level.InBounds(this.x + 1, this.y))
			{
				this.AdjacentTiles.Add(Global.Level.GetTile(this.x + 1, this.y));
				if (!this.HasRightWall())
				{
					this.TouchingTiles.Add(Global.Level.GetTile(this.x + 1, this.y));
				}
			}
			if (Global.Level.InBounds(this.x - 1, this.y + 1))
			{
				this.AdjacentTiles.Add(Global.Level.GetTile(this.x - 1, this.y + 1));
			}
			if (Global.Level.InBounds(this.x + 1, this.y + 1))
			{
				this.AdjacentTiles.Add(Global.Level.GetTile(this.x + 1, this.y + 1));
			}
			if (Global.Level.InBounds(this.x - 1, this.y - 1))
			{
				this.AdjacentTiles.Add(Global.Level.GetTile(this.x - 1, this.y - 1));
			}
			if (Global.Level.InBounds(this.x + 1, this.y - 1))
			{
				this.AdjacentTiles.Add(Global.Level.GetTile(this.x + 1, this.y - 1));
			}
			if (Global.Level.InBounds(this.x, this.y - 1))
			{
				this.AdjacentTiles.Add(Global.Level.GetTile(this.x, this.y - 1));
				if (!this.HasTopWall())
				{
					this.TouchingTiles.Add(Global.Level.GetTile(this.x, this.y - 1));
				}
			}
			if (Global.Level.InBounds(this.x, this.y + 1))
			{
				this.AdjacentTiles.Add(Global.Level.GetTile(this.x, this.y + 1));
				if (!this.HasBottomWall())
				{
					this.TouchingTiles.Add(Global.Level.GetTile(this.x, this.y + 1));
				}
			}
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00047C44 File Offset: 0x00045E44
		private void BuildAWall(float rotation, int index)
		{
			CustomLevelVertexDec[] array = TileBuilder.AddWall(this.x, this.y, this.wallTextureCoords[index].X, this.wallTextureCoords[index].Y, rotation, this.level, this.wallLightTexCoord);
			int num = this.wallVertices.Length;
			Array.Resize<CustomLevelVertexDec>(ref this.wallVertices, num + 36);
			Array.Copy(array, 0, this.wallVertices, num, array.Length);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00047CBC File Offset: 0x00045EBC
		public bool HasFloor()
		{
			return (this.groundTexCoord.X != 31 || this.groundTexCoord.Y != 31) && (this.groundTexCoord.X != 63 || this.groundTexCoord.Y != 63);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00047D0A File Offset: 0x00045F0A
		public void RemoveFloor(Sector sector)
		{
			this.floor = false;
			this.RebuildTile(sector);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00047D1A File Offset: 0x00045F1A
		public int NumberOfFloorVerts()
		{
			if (this.groundVertices == null)
			{
				return 0;
			}
			return this.groundVertices.Length;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00047D2E File Offset: 0x00045F2E
		public int NumberOfWallVerts()
		{
			if (this.wallVertices == null)
			{
				return 0;
			}
			return this.wallVertices.Length;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00047D42 File Offset: 0x00045F42
		public CustomLevelVertexDec[] GetGroundVertices()
		{
			return this.groundVertices;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00047D4A File Offset: 0x00045F4A
		public CustomLevelVertexDec[] GetWallVertices()
		{
			return this.wallVertices;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00047D52 File Offset: 0x00045F52
		public CustomLevelVertexDec[] AddGroundVertices(CustomLevelVertexDec[] VerticeList, int startIndex)
		{
			this.groundVertices.CopyTo(VerticeList, startIndex);
			return VerticeList;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00047D62 File Offset: 0x00045F62
		public CustomLevelVertexDec[] AddWallVertices(CustomLevelVertexDec[] VerticeList, int startIndex)
		{
			this.wallVertices.CopyTo(VerticeList, startIndex);
			return VerticeList;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00047D74 File Offset: 0x00045F74
		public void RebuildTile(Sector sector)
		{
			this.wallVertices = new CustomLevelVertexDec[0];
			if (this.floor)
			{
				this.groundVertices = TileBuilder.BuildFloor(this.x, this.y, this.groundTexCoord.X, this.groundTexCoord.Y, this.level, this.lightTexCoord.X, this.lightTexCoord.Y);
			}
			if (this.walls[0] || this.fakeWalls[0] || this.halfWalls[0] || this.windowWalls[0])
			{
				this.BuildAWall(4.712389f, 0);
			}
			if (this.walls[1] || this.fakeWalls[1] || this.halfWalls[1] || this.windowWalls[1])
			{
				this.BuildAWall(3.1415927f, 1);
			}
			if (this.walls[2] || this.fakeWalls[2] || this.halfWalls[2] || this.windowWalls[2])
			{
				this.BuildAWall(1.5707964f, 2);
			}
			if (this.walls[3] || this.fakeWalls[3] || this.halfWalls[3] || this.windowWalls[3])
			{
				this.BuildAWall(0f, 3);
			}
			sector.BuildLevel();
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00047EB4 File Offset: 0x000460B4
		public void CheckBuildWallTest(Vector3 mousePos, Sector sector)
		{
			float num = mousePos.X;
			float num2 = mousePos.Z;
			num -= (float)this.x;
			num2 -= (float)this.y;
			float num3 = 0.3f;
			if (num < num3)
			{
				this.walls[2] = !this.walls[2];
				this.RebuildTile(sector);
				return;
			}
			if (num > 1f - num3)
			{
				this.walls[0] = !this.walls[0];
				this.RebuildTile(sector);
				return;
			}
			if (num2 < num3)
			{
				this.walls[3] = !this.walls[3];
				this.RebuildTile(sector);
				return;
			}
			if (num2 > 1f - num3)
			{
				this.walls[1] = !this.walls[1];
				this.RebuildTile(sector);
				return;
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00047F74 File Offset: 0x00046174
		public void ToggleWall(int loc, Sector sector)
		{
			if (Editor.WallType == "Normal Wall")
			{
				this.walls[loc] = !this.walls[loc];
				this.fakeWalls[loc] = false;
				this.halfWalls[loc] = false;
				this.windowWalls[loc] = false;
				this.wallTextureCoords[loc] = new Point(Editor.SelectedTexCoords.X, Editor.SelectedTexCoords.Y);
				this.RebuildTile(sector);
			}
			if (Editor.WallType == "Fake Wall")
			{
				this.walls[loc] = false;
				this.fakeWalls[loc] = !this.fakeWalls[loc];
				this.halfWalls[loc] = false;
				this.windowWalls[loc] = false;
				this.wallTextureCoords[loc] = new Point(Editor.SelectedTexCoords.X, Editor.SelectedTexCoords.Y);
				this.RebuildTile(sector);
			}
			if (Editor.WallType == "Half Wall")
			{
				this.walls[loc] = false;
				this.fakeWalls[loc] = false;
				this.halfWalls[loc] = !this.halfWalls[loc];
				this.windowWalls[loc] = false;
				this.wallTextureCoords[loc] = new Point(Editor.SelectedTexCoords.X, Editor.SelectedTexCoords.Y);
				this.RebuildTile(sector);
			}
			if (Editor.WallType == "Window Wall")
			{
				this.walls[loc] = false;
				this.fakeWalls[loc] = false;
				this.halfWalls[loc] = false;
				this.windowWalls[loc] = !this.windowWalls[loc];
				this.wallTextureCoords[loc] = new Point(Editor.SelectedTexCoords.X, Editor.SelectedTexCoords.Y);
				this.RebuildTile(sector);
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00048130 File Offset: 0x00046330
		public void ToggleWallNoTex(int loc, Sector sector)
		{
			if (Editor.WallType == "Normal Wall")
			{
				this.walls[loc] = !this.walls[loc];
				this.fakeWalls[loc] = false;
				this.halfWalls[loc] = false;
				this.RebuildTile(sector);
			}
			if (Editor.WallType == "Fake Wall")
			{
				this.walls[loc] = false;
				this.fakeWalls[loc] = !this.fakeWalls[loc];
				this.halfWalls[loc] = false;
				this.RebuildTile(sector);
			}
			if (Editor.WallType == "Half Wall")
			{
				this.walls[loc] = false;
				this.fakeWalls[loc] = false;
				this.halfWalls[loc] = !this.halfWalls[loc];
				this.RebuildTile(sector);
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x000481F4 File Offset: 0x000463F4
		public void BuildFloor(Sector sector)
		{
			this.floor = true;
			this.groundTexCoord = new Point(Editor.SelectedTexCoords.X, Editor.SelectedTexCoords.Y);
			this.RebuildTile(sector);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00048223 File Offset: 0x00046423
		public void SetFloor(int x, int y)
		{
			this.floor = true;
			this.groundTexCoord = new Point(x, y);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0004823C File Offset: 0x0004643C
		public TileInfo GetTileInfo()
		{
			return new TileInfo
			{
				rightWall = this.walls[0],
				bottomWall = this.walls[1],
				leftWall = this.walls[2],
				topWall = this.walls[3],
				floor = this.floor,
				rightWallTex = this.wallTextureCoords[0],
				bottomWallTex = this.wallTextureCoords[1],
				leftWallTex = this.wallTextureCoords[2],
				topWallTex = this.wallTextureCoords[3],
				rightWallFake = this.fakeWalls[0],
				bottomWallFake = this.fakeWalls[1],
				leftWallFake = this.fakeWalls[2],
				topWallFake = this.fakeWalls[3],
				rightWallHalf = this.halfWalls[0],
				bottomWallHalf = this.halfWalls[1],
				leftWallHalf = this.halfWalls[2],
				topWallHalf = this.halfWalls[3],
				rightWallWindow = this.windowWalls[0],
				bottomWallWindow = this.windowWalls[1],
				leftWallWindow = this.windowWalls[2],
				topWallWindow = this.windowWalls[3],
				floorTex = this.groundTexCoord,
				tileProps = this.TileProperties
			};
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x000483B8 File Offset: 0x000465B8
		public void BuildTileFromInfo(Sector sector, TileInfo info)
		{
			this.walls[0] = info.rightWall;
			this.walls[1] = info.bottomWall;
			this.walls[2] = info.leftWall;
			this.walls[3] = info.topWall;
			this.fakeWalls[0] = info.rightWallFake;
			this.fakeWalls[1] = info.bottomWallFake;
			this.fakeWalls[2] = info.leftWallFake;
			this.fakeWalls[3] = info.topWallFake;
			this.halfWalls[0] = info.rightWallHalf;
			this.halfWalls[1] = info.bottomWallHalf;
			this.halfWalls[2] = info.leftWallHalf;
			this.halfWalls[3] = info.topWallHalf;
			this.windowWalls[0] = info.rightWallWindow;
			this.windowWalls[1] = info.bottomWallWindow;
			this.windowWalls[2] = info.leftWallWindow;
			this.windowWalls[3] = info.topWallWindow;
			this.floor = info.floor;
			this.wallTextureCoords[0] = info.rightWallTex;
			this.wallTextureCoords[1] = info.bottomWallTex;
			this.wallTextureCoords[2] = info.leftWallTex;
			this.wallTextureCoords[3] = info.topWallTex;
			this.TileProperties = info.tileProps;
			this.groundTexCoord = info.floorTex;
			if (GameManager.PLANONEDITING)
			{
				this.RebuildTile(sector);
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0004851F File Offset: 0x0004671F
		public Point GetGroundTexCoords()
		{
			return this.groundTexCoord;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00048528 File Offset: 0x00046728
		public bool HasAnyWalls()
		{
			return this.walls[0] || this.walls[1] || this.walls[2] || this.walls[3] || this.halfWalls[0] || this.halfWalls[1] || this.halfWalls[2] || this.halfWalls[3] || this.windowWalls[0] || this.windowWalls[1] || this.windowWalls[2] || this.windowWalls[3];
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x000485B0 File Offset: 0x000467B0
		public int CollidedWithWall(Vector3 position, float speed)
		{
			Vector2 vector = new Vector2(position.X - (float)this.x, position.Z - (float)this.y);
			float num = 0.4f;
			if (speed != 1f)
			{
				num = speed;
			}
			if ((this.walls[0] || this.halfWalls[0] || this.windowWalls[0]) && vector.X > 1f - num + 0.25f)
			{
				return 0;
			}
			if ((this.walls[2] || this.halfWalls[2] || this.windowWalls[2]) && vector.X < num - 0.25f)
			{
				return 2;
			}
			if ((this.walls[1] || this.halfWalls[1] || this.windowWalls[1]) && vector.Y > 1f - num + 0.25f)
			{
				return 3;
			}
			if ((this.walls[3] || this.halfWalls[3] || this.windowWalls[3]) && vector.Y < num - 0.25f)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x000486BC File Offset: 0x000468BC
		public int CollidedWithFullOrWindowWall(Vector3 position, float speed)
		{
			Vector2 vector = new Vector2(position.X - (float)this.x, position.Z - (float)this.y);
			float num = 0.4f;
			if (speed != 1f)
			{
				num = speed;
			}
			if ((this.walls[0] || this.windowWalls[0]) && vector.X > 1f - num + 0.25f)
			{
				return 0;
			}
			if ((this.walls[2] || this.windowWalls[2]) && vector.X < num - 0.25f)
			{
				return 2;
			}
			if ((this.walls[1] || this.windowWalls[1]) && vector.Y > 1f - num + 0.25f)
			{
				return 3;
			}
			if ((this.walls[3] || this.windowWalls[3]) && vector.Y < num - 0.25f)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x000487A0 File Offset: 0x000469A0
		public int CollidedWithWallPLAYER(Vector3 position, float speed)
		{
			int num = this.CollidedWithWall(position, speed);
			if (Global.BossActive)
			{
				Vector2 vector = new Vector2(position.X - (float)num, position.Z - (float)this.y);
				float num2 = 0.4f;
				if (speed != 1f)
				{
					num2 = speed;
				}
				if (!this.TileProperties.Contains(TilePropertyType.BossArea))
				{
					if (vector.X > 1f - num2 + 0.25f)
					{
						return 0;
					}
					if (vector.X < num2 - 0.25f)
					{
						return 2;
					}
					if (vector.Y > 1f - num2 + 0.25f)
					{
						return 3;
					}
					if (vector.Y < num2 - 0.25f)
					{
						return 1;
					}
				}
			}
			return num;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00048850 File Offset: 0x00046A50
		public bool CollidedWithHalfWall(Vector3 position, float speed)
		{
			Vector2 vector = new Vector2(position.X - (float)this.x, position.Z - (float)this.y);
			float num = 0.4f;
			if (speed != 1f)
			{
				num = speed;
			}
			return (this.halfWalls[0] && vector.X > 1f - num + 0.25f) || (this.halfWalls[2] && vector.X < num - 0.25f) || (this.halfWalls[1] && vector.Y > 1f - num + 0.25f) || (this.halfWalls[3] && vector.Y < num - 0.25f);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0004890C File Offset: 0x00046B0C
		public int CollidedBullet(Vector3 position, Vector2 dir, float speed, float elapsed)
		{
			if (!this.HasAnyWalls())
			{
				return -1;
			}
			speed = speed * elapsed * 1.33f;
			Vector3 zero = Vector3.Zero;
			zero.X = position.X + dir.X * speed;
			zero.Z = position.Z + dir.Y * speed;
			Tile tileAtLocation = Global.Level.GetTileAtLocation(zero);
			if (tileAtLocation == null)
			{
				if (zero.Z <= 0f)
				{
					return 3;
				}
				if (zero.Z >= 32f)
				{
					return 1;
				}
				if (zero.X <= 0f)
				{
					return 2;
				}
				if (zero.X >= 32f)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (tileAtLocation.x == this.x && tileAtLocation.y == this.y)
				{
					return -1;
				}
				if (tileAtLocation.x < this.x && this.HasLeftWallBullet())
				{
					return 2;
				}
				if (tileAtLocation.x > this.x && this.HasRightWallBullet())
				{
					return 0;
				}
				if (tileAtLocation.y < this.y && this.HasTopWallBullet())
				{
					return 3;
				}
				if (tileAtLocation.y > this.y && this.HasBottomWallBullet())
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00048A30 File Offset: 0x00046C30
		public int ShouldIHaveCollidedBulletPreviously(Vector3 position, Vector2 dir, float speed, float elapsed)
		{
			if (!this.HasAnyWalls())
			{
				return -1;
			}
			speed *= elapsed;
			Vector3 zero = Vector3.Zero;
			zero.X = position.X - dir.X * speed * 1.33f;
			zero.Z = position.Z - dir.Y * speed * 1.33f;
			Tile tileAtLocation = Global.Level.GetTileAtLocation(zero);
			if (tileAtLocation == null)
			{
				return -1;
			}
			if (tileAtLocation.x == this.x && tileAtLocation.y == this.y)
			{
				return -1;
			}
			if (tileAtLocation.x < this.x && this.HasLeftWallBullet())
			{
				return 2;
			}
			if (tileAtLocation.x > this.x && this.HasRightWallBullet())
			{
				return 0;
			}
			if (tileAtLocation.y < this.y && this.HasTopWallBullet())
			{
				return 3;
			}
			if (tileAtLocation.y > this.y && this.HasBottomWallBullet())
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00048B1C File Offset: 0x00046D1C
		public int CollidedFromParent(Vector3 parentPos)
		{
			Tile tileAtLocation = Global.Level.GetTileAtLocation(parentPos);
			if (tileAtLocation == null)
			{
				return -1;
			}
			if (tileAtLocation.x == this.x && tileAtLocation.y == this.y)
			{
				return -1;
			}
			if (this.x < tileAtLocation.x && tileAtLocation.HasLeftWallBullet())
			{
				return 2;
			}
			if (this.x > tileAtLocation.x && tileAtLocation.HasRightWallBullet())
			{
				return 0;
			}
			if (this.y < tileAtLocation.y && tileAtLocation.HasTopWallBullet())
			{
				return 3;
			}
			if (this.y > tileAtLocation.y && tileAtLocation.HasBottomWallBullet())
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00048BBC File Offset: 0x00046DBC
		public void ToggleGrid()
		{
			if (this.groundTexCoord.X == 31 && this.groundTexCoord.Y == 31)
			{
				this.groundTexCoord.X = 30;
				return;
			}
			if (this.groundTexCoord.X == 30 && this.groundTexCoord.Y == 31)
			{
				this.groundTexCoord.X = 31;
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00048C20 File Offset: 0x00046E20
		public List<GameObject> GetAdjacentTileContainedList()
		{
			return this.AdjacentObjects;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00048C28 File Offset: 0x00046E28
		public void AddObjectToContainedList(GameObject obj)
		{
			if (!this.ContainedObjects.Contains(obj))
			{
				this.ContainedObjects.Add(obj);
			}
			foreach (Tile tile in this.AdjacentTiles)
			{
				if (!tile.AdjacentObjects.Contains(obj))
				{
					tile.AdjacentObjects.Add(obj);
				}
			}
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00048CA8 File Offset: 0x00046EA8
		public void RemoveObjectFromContainedList(GameObject obj)
		{
			this.ContainedObjects.Remove(obj);
			foreach (Tile tile in this.AdjacentTiles)
			{
				tile.AdjacentObjects.Remove(obj);
			}
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00048D0C File Offset: 0x00046F0C
		public int GetOutsideLevelCollision()
		{
			if (this.x == 0)
			{
				return 2;
			}
			if (this.x == 31)
			{
				return 0;
			}
			if (this.y == 0)
			{
				return 1;
			}
			if (this.y == 31)
			{
				return 3;
			}
			return -1;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00048D3C File Offset: 0x00046F3C
		public void ComputeShadows()
		{
			this.lightTexCoord = new Point(this.x, this.y);
			for (int i = 0; i < 4; i++)
			{
				if (this.HasFloor())
				{
					this.wallLightTexCoord[i] = new Point(2, 2);
				}
				else
				{
					this.wallLightTexCoord[i] = new Point(0, 0);
				}
			}
			Point point = new Point(9, 5);
			Point point2 = new Point(9, 2);
			Point point3 = new Point(13, 4);
			Point point4 = new Point(9, 4);
			Point point5 = new Point(9, 1);
			Point point6 = new Point(13, 3);
			if (this.HasBottomWallLIGHT() && this.HasRightWallLIGHT())
			{
				if (this.HasFloor())
				{
					this.wallLightTexCoord[2] = point2;
					this.wallLightTexCoord[3] = point;
				}
				else
				{
					this.wallLightTexCoord[2] = point5;
					this.wallLightTexCoord[3] = point4;
				}
			}
			if (this.HasBottomWallLIGHT() && this.HasLeftWallLIGHT())
			{
				if (this.HasFloor())
				{
					this.wallLightTexCoord[2] = point;
					this.wallLightTexCoord[1] = point2;
				}
				else
				{
					this.wallLightTexCoord[2] = point4;
					this.wallLightTexCoord[1] = point5;
				}
			}
			if (this.HasBottomWallLIGHT() && this.HasRightWallLIGHT() && this.HasLeftWallLIGHT())
			{
				if (this.HasFloor())
				{
					this.wallLightTexCoord[2] = point3;
				}
				else
				{
					this.wallLightTexCoord[2] = point6;
				}
			}
			if (this.HasTopWallLIGHT() && this.HasRightWallLIGHT())
			{
				if (this.HasFloor())
				{
					this.wallLightTexCoord[0] = point;
					this.wallLightTexCoord[3] = point2;
				}
				else
				{
					this.wallLightTexCoord[0] = point4;
					this.wallLightTexCoord[3] = point5;
				}
			}
			if (this.HasTopWallLIGHT() && this.HasLeftWallLIGHT())
			{
				if (this.HasFloor())
				{
					this.wallLightTexCoord[0] = point2;
					this.wallLightTexCoord[1] = point;
				}
				else
				{
					this.wallLightTexCoord[0] = point5;
					this.wallLightTexCoord[1] = point4;
				}
			}
			if (this.HasTopWallLIGHT() && this.HasRightWallLIGHT() && this.HasLeftWallLIGHT())
			{
				if (this.HasFloor())
				{
					this.wallLightTexCoord[0] = point3;
					return;
				}
				this.wallLightTexCoord[0] = point6;
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00048F94 File Offset: 0x00047194
		private Color[,] GetTexColors()
		{
			Texture2D masterEnvTex = Global.MasterEnvTex;
			Color[] array = new Color[masterEnvTex.Width * masterEnvTex.Height];
			masterEnvTex.GetData<Color>(array);
			Color[,] array2 = new Color[masterEnvTex.Width, masterEnvTex.Height];
			for (int i = 0; i < masterEnvTex.Width; i++)
			{
				for (int j = 0; j < masterEnvTex.Height; j++)
				{
					array2[i, j] = array[i + j * masterEnvTex.Height];
				}
			}
			return array2;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00049014 File Offset: 0x00047214
		private Color[,] GetWallTexColors(Point coord, Color[,] tex)
		{
			Color[,] array = new Color[16, 16];
			int num = 0;
			if (coord.X == 63)
			{
				coord.X = 31;
			}
			if (coord.Y == 63)
			{
				coord.Y = 31;
			}
			for (int i = coord.X * 16; i < coord.X * 16 + 16; i++)
			{
				int num2 = 0;
				for (int j = coord.Y * 16; j < coord.Y * 16 + 16; j++)
				{
					array[num, num2] = tex[i, j];
					num2++;
				}
				num++;
			}
			return array;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x000490B4 File Offset: 0x000472B4
		private int ShadowSize(Point wallTex, Color[,] tex)
		{
			int num = 15;
			int num2 = 0;
			for (int i = 0; i < 16; i++)
			{
				if (tex[i, num].A > 200)
				{
					num2++;
				}
			}
			if (num2 == 0)
			{
				return -1;
			}
			if (num2 < 10)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x000490FC File Offset: 0x000472FC
		public void RenderFloorShadows(SpriteBatch spriteBatch)
		{
			Rectangle destinationRectangle = new Rectangle(this.x * 16, this.y * 16, 16, 16);
			new Rectangle(16, 32, 16, 16);
			if (this.HasLeftWall())
			{
				int num = this.RenderBigWall(this.wallTextureCoords[2]);
				if (num == 0)
				{
					spriteBatch.Draw(Global.MasterLightTexTransparent, destinationRectangle, new Rectangle?(Tile.ShadowLeft), Color.White);
				}
				else if (num == 1)
				{
					spriteBatch.Draw(Global.MasterLightTexTransparent, destinationRectangle, new Rectangle?(Tile.ShadowLeftSmall), Color.White);
				}
			}
			if (this.HasTopWall())
			{
				int num2 = this.RenderBigWall(this.wallTextureCoords[3]);
				if (num2 == 0)
				{
					spriteBatch.Draw(Global.MasterLightTexTransparent, destinationRectangle, new Rectangle?(Tile.ShadowTop), Color.White);
				}
				else if (num2 == 1)
				{
					spriteBatch.Draw(Global.MasterLightTexTransparent, destinationRectangle, new Rectangle?(Tile.ShadowTopSmall), Color.White);
				}
			}
			if (this.HasRightWall())
			{
				int num3 = this.RenderBigWall(this.wallTextureCoords[0]);
				if (num3 == 0)
				{
					spriteBatch.Draw(Global.MasterLightTexTransparent, destinationRectangle, new Rectangle?(Tile.ShadowRight), Color.White);
				}
				else if (num3 == 1)
				{
					spriteBatch.Draw(Global.MasterLightTexTransparent, destinationRectangle, new Rectangle?(Tile.ShadowRightSmall), Color.White);
				}
			}
			if (this.HasBottomWall())
			{
				int num4 = this.RenderBigWall(this.wallTextureCoords[1]);
				if (num4 == 0)
				{
					spriteBatch.Draw(Global.MasterLightTexTransparent, destinationRectangle, new Rectangle?(Tile.ShadowBottom), Color.White);
					return;
				}
				if (num4 == 1)
				{
					spriteBatch.Draw(Global.MasterLightTexTransparent, destinationRectangle, new Rectangle?(Tile.ShadowBottomSmall), Color.White);
				}
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x000492A0 File Offset: 0x000474A0
		private int RenderBigWall(Point wallCoord)
		{
			if (this.FULL_SHADOW.Contains(wallCoord))
			{
				return 0;
			}
			Color[,] wallTexColors = this.GetWallTexColors(wallCoord, this.GetTexColors());
			return this.ShadowSize(wallCoord, wallTexColors);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x000492D3 File Offset: 0x000474D3
		public bool AmIGrass()
		{
			return this.groundTexCoord.X == 0 && (this.groundTexCoord.Y == 1 || this.groundTexCoord.Y == 2 || this.groundTexCoord.Y == 3);
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00049310 File Offset: 0x00047510
		public bool HasTopWall()
		{
			return this.walls[3] || this.halfWalls[3] || this.windowWalls[3];
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00049330 File Offset: 0x00047530
		public bool HasBottomWall()
		{
			return this.walls[1] || this.halfWalls[1] || this.windowWalls[1];
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00049350 File Offset: 0x00047550
		public bool HasLeftWall()
		{
			return this.walls[2] || this.halfWalls[2] || this.windowWalls[2];
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00049370 File Offset: 0x00047570
		public bool HasRightWall()
		{
			return this.walls[0] || this.halfWalls[0] || this.windowWalls[0];
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00049390 File Offset: 0x00047590
		public bool HasTopWallLIGHT()
		{
			return this.walls[3] || this.windowWalls[3];
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x000493A6 File Offset: 0x000475A6
		public bool HasBottomWallLIGHT()
		{
			return this.walls[1] || this.windowWalls[1];
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x000493BC File Offset: 0x000475BC
		public bool HasLeftWallLIGHT()
		{
			return this.walls[2] || this.windowWalls[2];
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x000493D2 File Offset: 0x000475D2
		public bool HasRightWallLIGHT()
		{
			return this.walls[0] || this.windowWalls[0];
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000493E8 File Offset: 0x000475E8
		public bool HasTopWallBullet()
		{
			return this.walls[3];
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x000493F2 File Offset: 0x000475F2
		public bool HasBottomWallBullet()
		{
			return this.walls[1];
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x000493FC File Offset: 0x000475FC
		public bool HasLeftWallBullet()
		{
			return this.walls[2];
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00049406 File Offset: 0x00047606
		public bool HasRightWallBullet()
		{
			return this.walls[0];
		}

		// Token: 0x0400095F RID: 2399
		public static float SCALE = 1f;

		// Token: 0x04000960 RID: 2400
		private CustomLevelVertexDec[] groundVertices;

		// Token: 0x04000961 RID: 2401
		private CustomLevelVertexDec[] wallVertices;

		// Token: 0x04000962 RID: 2402
		public List<GameObject> ContainedObjects;

		// Token: 0x04000963 RID: 2403
		public List<GameObject> AdjacentObjects;

		// Token: 0x04000964 RID: 2404
		public List<Tile> AdjacentTiles;

		// Token: 0x04000965 RID: 2405
		public List<Tile> TouchingTiles;

		// Token: 0x04000966 RID: 2406
		public float weight = 1f;

		// Token: 0x04000967 RID: 2407
		public int ContainedZombieNumber;

		// Token: 0x04000968 RID: 2408
		public int[,] Directions;

		// Token: 0x04000969 RID: 2409
		public int ZombiePathingToCount;

		// Token: 0x0400096A RID: 2410
		public List<TilePropertyType> TileProperties;

		// Token: 0x0400096B RID: 2411
		public int x;

		// Token: 0x0400096C RID: 2412
		public int y;

		// Token: 0x0400096D RID: 2413
		private Point groundTexCoord;

		// Token: 0x0400096E RID: 2414
		private Point lightTexCoord;

		// Token: 0x0400096F RID: 2415
		private Point[] wallLightTexCoord;

		// Token: 0x04000970 RID: 2416
		public Point[] wallTextureCoords;

		// Token: 0x04000971 RID: 2417
		private bool[] walls;

		// Token: 0x04000972 RID: 2418
		private bool[] fakeWalls;

		// Token: 0x04000973 RID: 2419
		private bool[] halfWalls;

		// Token: 0x04000974 RID: 2420
		private bool[] windowWalls;

		// Token: 0x04000975 RID: 2421
		public bool ADDED;

		// Token: 0x04000976 RID: 2422
		public Tile parent;

		// Token: 0x04000977 RID: 2423
		public float cost;

		// Token: 0x04000978 RID: 2424
		private bool floor = true;

		// Token: 0x04000979 RID: 2425
		private float level;

		// Token: 0x0400097A RID: 2426
		public bool HasObjective;

		// Token: 0x0400097B RID: 2427
		private List<GameObject> AdjacentList = new List<GameObject>();

		// Token: 0x0400097C RID: 2428
		private List<Point> FULL_SHADOW = new List<Point>
		{
			new Point(20, 18),
			new Point(21, 18),
			new Point(22, 20),
			new Point(22, 16),
			new Point(23, 16),
			new Point(24, 16),
			new Point(25, 16),
			new Point(26, 16),
			new Point(27, 16),
			new Point(28, 16),
			new Point(29, 16),
			new Point(23, 17),
			new Point(24, 17),
			new Point(25, 17),
			new Point(26, 17),
			new Point(27, 17),
			new Point(28, 17),
			new Point(29, 17)
		};

		// Token: 0x0400097D RID: 2429
		private static Rectangle ShadowBottom = new Rectangle(32, 32, 16, 16);

		// Token: 0x0400097E RID: 2430
		private static Rectangle ShadowTop = new Rectangle(32, 112, 16, 16);

		// Token: 0x0400097F RID: 2431
		private static Rectangle ShadowLeft = new Rectangle(80, 80, 16, 16);

		// Token: 0x04000980 RID: 2432
		private static Rectangle ShadowRight = new Rectangle(0, 80, 16, 16);

		// Token: 0x04000981 RID: 2433
		private static Rectangle ShadowBottomSmall = new Rectangle(32, 144, 16, 16);

		// Token: 0x04000982 RID: 2434
		private static Rectangle ShadowTopSmall = new Rectangle(32, 160, 16, 16);

		// Token: 0x04000983 RID: 2435
		private static Rectangle ShadowLeftSmall = new Rectangle(48, 160, 16, 16);

		// Token: 0x04000984 RID: 2436
		private static Rectangle ShadowRightSmall = new Rectangle(48, 144, 16, 16);
	}
}
