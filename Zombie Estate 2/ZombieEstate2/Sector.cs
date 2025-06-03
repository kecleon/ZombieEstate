using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000123 RID: 291
	public class Sector
	{
		// Token: 0x06000830 RID: 2096 RVA: 0x00044564 File Offset: 0x00042764
		public Sector(Game game, int level, string name)
		{
			this.SectorLevel = level;
			this.LevelName = name;
			this.TESTWALL = game.Content.Load<Texture2D>("MasterWall_Test_Desert");
			this.tiles = new Tile[Sector.width, Sector.height];
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					this.tiles[j, i] = new Tile(j, i, (float)this.SectorLevel, GameManager.PLANONEDITING);
				}
			}
			if (GameManager.PLANONEDITING)
			{
				this.BuildSurrondingWalls();
				this.BuildLevel();
			}
			if (level == 0)
			{
				new MasterPathfinder(this, this.tiles, Sector.width, Sector.height, false, this.LevelName);
			}
			this.stateFloor = new RasterizerState();
			this.stateFloor.CullMode = CullMode.None;
			this.stateWalls = new RasterizerState();
			this.stateWalls.CullMode = CullMode.CullClockwiseFace;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00044652 File Offset: 0x00042852
		public void BuildLevel()
		{
			this.buildFloor();
			this.buildWalls();
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00044660 File Offset: 0x00042860
		private void buildFloor()
		{
			if (this.groundVertices != null && (Global.Editor == null || !Global.Editor.EDITING))
			{
				return;
			}
			this.groundVerticeCount = this.getFloorVerticeCount();
			this.groundVertices = new CustomLevelVertexDec[this.groundVerticeCount];
			int num = this.groundVerticeCount;
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					num -= this.tiles[j, i].NumberOfFloorVerts();
					this.tiles[j, i].AddGroundVertices(this.groundVertices, num);
				}
			}
			this.groundVertexBuffer = new VertexBuffer(Global.GraphicsDevice, typeof(CustomLevelVertexDec), this.groundVertices.Length, BufferUsage.WriteOnly);
			this.groundVertexBuffer.SetData<CustomLevelVertexDec>(this.groundVertices);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00044730 File Offset: 0x00042930
		private void buildWalls()
		{
			this.wallVerticeCount = this.getWallVerticeCount();
			if (this.wallVerticeCount == 0)
			{
				return;
			}
			this.wallVertices = new CustomLevelVertexDec[this.wallVerticeCount];
			int num = this.wallVerticeCount;
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					num -= this.tiles[j, i].NumberOfWallVerts();
					this.tiles[j, i].AddWallVertices(this.wallVertices, num);
				}
			}
			this.wallVertexBuffer = new VertexBuffer(Global.GraphicsDevice, typeof(CustomLevelVertexDec), this.wallVertices.Length, BufferUsage.WriteOnly);
			this.wallVertexBuffer.SetData<CustomLevelVertexDec>(this.wallVertices);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x000447ED File Offset: 0x000429ED
		public void UpdateVertBuffers()
		{
			this.wallVertexBuffer.SetData<CustomLevelVertexDec>(this.wallVertices);
			this.groundVertexBuffer.SetData<CustomLevelVertexDec>(this.groundVertices);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void buildGrid()
		{
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00044814 File Offset: 0x00042A14
		public void DrawSector(bool main)
		{
			Effect levelEffect = Global.levelEffect;
			levelEffect.Parameters["World"].SetValue(Matrix.Identity);
			levelEffect.Parameters["View"].SetValue(Global.View);
			levelEffect.Parameters["Projection"].SetValue(Global.Projection);
			levelEffect.Parameters["Texture"].SetValue(Global.MasterEnvTex);
			levelEffect.Parameters["FadeMod"].SetValue(Global.DarkMod);
			if (Global.AllLocalPlayersDead())
			{
				levelEffect.Parameters["BW"].SetValue(true);
			}
			else
			{
				levelEffect.Parameters["BW"].SetValue(false);
			}
			if (main)
			{
				levelEffect.Parameters["AO_Enabled"].SetValue(Global.AOEnabled);
			}
			else
			{
				levelEffect.Parameters["AO_Enabled"].SetValue(false);
			}
			levelEffect.Parameters["LightTexture"].SetValue(Global.CurrentLevelLightTex);
			levelEffect.Parameters["DynamicShadowTexture"].SetValue(DynamicShadows.mRT);
			levelEffect.Parameters["DynamicShadows"].SetValue(true);
			this.DrawFloor();
			levelEffect.Parameters["AO_Enabled"].SetValue(Global.AOEnabled);
			levelEffect.Parameters["DynamicShadows"].SetValue(false);
			levelEffect.Parameters["LightTexture"].SetValue(Global.MasterLightTex);
			this.DrawWalls();
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x000449B4 File Offset: 0x00042BB4
		private void DrawFloor()
		{
			Effect levelEffect = Global.levelEffect;
			levelEffect.CurrentTechnique.Passes[0].Apply();
			Global.GraphicsDevice.RasterizerState = this.stateFloor;
			foreach (EffectPass effectPass in levelEffect.CurrentTechnique.Passes)
			{
				Global.GraphicsDevice.SetVertexBuffer(this.groundVertexBuffer);
				int primitiveCount = this.groundVertexBuffer.VertexCount / 3;
				Global.GraphicsDevice.DrawUserPrimitives<CustomLevelVertexDec>(PrimitiveType.TriangleList, this.groundVertices, 0, primitiveCount);
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00044A60 File Offset: 0x00042C60
		private void DrawWalls()
		{
			Effect levelEffect = Global.levelEffect;
			levelEffect.CurrentTechnique.Passes[0].Apply();
			Global.GraphicsDevice.RasterizerState = this.stateWalls;
			foreach (EffectPass effectPass in levelEffect.CurrentTechnique.Passes)
			{
				if (!this.XBOX)
				{
					Global.GraphicsDevice.SetVertexBuffer(this.wallVertexBuffer);
					int primitiveCount = this.wallVertexBuffer.VertexCount / 3;
					Global.GraphicsDevice.DrawUserPrimitives<CustomLevelVertexDec>(PrimitiveType.TriangleList, this.wallVertices, 0, primitiveCount);
				}
				else
				{
					for (int i = 0; i < this.wallVertexBuffers.Count; i++)
					{
						Global.GraphicsDevice.SetVertexBuffer(this.wallVertexBuffers[i]);
						int primitiveCount2 = this.wallVertexBuffers[i].VertexCount / 3;
						Global.GraphicsDevice.DrawUserPrimitives<CustomLevelVertexDec>(PrimitiveType.TriangleList, this.splitList[i], 0, primitiveCount2);
					}
				}
			}
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00044B78 File Offset: 0x00042D78
		private int getFloorVerticeCount()
		{
			int num = 0;
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					num += this.tiles[j, i].NumberOfFloorVerts();
				}
			}
			return num;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00044BC0 File Offset: 0x00042DC0
		private int getWallVerticeCount()
		{
			int num = 0;
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					num += this.tiles[j, i].NumberOfWallVerts();
				}
			}
			return num;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00044C08 File Offset: 0x00042E08
		public Tile GetTileAtLocation(Vector3 loc)
		{
			int num = (int)loc.X;
			int num2 = (int)loc.Z;
			if (this.InBounds(num, num2))
			{
				return this.tiles[num, num2];
			}
			return null;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x00044C3E File Offset: 0x00042E3E
		public bool InBounds(int x, int y)
		{
			return x >= 0 && x < Sector.width && y >= 0 && y < Sector.height;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00044C5C File Offset: 0x00042E5C
		public List<TileInfo> GetTileInfo()
		{
			List<TileInfo> list = new List<TileInfo>();
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					list.Add(this.tiles[j, i].GetTileInfo());
				}
			}
			Terminal.WriteMessage("Tile count: " + list.Count, MessageType.DEBUG);
			return list;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00044CC4 File Offset: 0x00042EC4
		public void LoadTilesFromInfo(List<TileInfo> info)
		{
			int num = 0;
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					this.tiles[j, i].BuildTileFromInfo(this, info[num]);
					num++;
					Level.TilesDoneLoading++;
				}
			}
			if (GameManager.PLANONEDITING)
			{
				this.BuildSurrondingWalls();
			}
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00044D2C File Offset: 0x00042F2C
		public void BuildAdjacentLists()
		{
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					this.tiles[j, i].BuildAdjacentList();
				}
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00044D6C File Offset: 0x00042F6C
		public void ToggleGrid()
		{
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					this.tiles[j, i].ToggleGrid();
					this.tiles[j, i].RebuildTile(this);
				}
			}
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00044DBE File Offset: 0x00042FBE
		public Tile GetTile(int x, int y)
		{
			if (x < 0 || x >= Sector.width || y < 0 || y >= Sector.height)
			{
				return null;
			}
			return this.tiles[x, y];
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00044DE7 File Offset: 0x00042FE7
		public void CLEARTILES()
		{
			this.tiles = null;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00044DF0 File Offset: 0x00042FF0
		public Tile Neighbor(Tile center, int angle)
		{
			switch (angle)
			{
			case 0:
				if (!center.HasTopWall())
				{
					return this.tiles[center.x, center.y - 1];
				}
				break;
			case 1:
				if (!center.HasTopWall() && !this.tiles[center.x, center.y - 1].HasRightWall() && !center.HasRightWall() && !this.tiles[center.x + 1, center.y].HasTopWall())
				{
					return this.tiles[center.x + 1, center.y - 1];
				}
				break;
			case 2:
				if (!center.HasRightWall())
				{
					return this.tiles[center.x + 1, center.y];
				}
				break;
			case 3:
				if (!center.HasBottomWall() && !this.tiles[center.x, center.y + 1].HasRightWall() && !center.HasRightWall() && !this.tiles[center.x + 1, center.y].HasBottomWall())
				{
					return this.tiles[center.x + 1, center.y + 1];
				}
				break;
			case 4:
				if (!center.HasBottomWall())
				{
					return this.tiles[center.x, center.y + 1];
				}
				break;
			case 5:
				if (!center.HasBottomWall() && !this.tiles[center.x, center.y + 1].HasLeftWall() && !center.HasLeftWall() && !this.tiles[center.x - 1, center.y].HasBottomWall())
				{
					return this.tiles[center.x - 1, center.y + 1];
				}
				break;
			case 6:
				if (!center.HasLeftWall())
				{
					return this.tiles[center.x - 1, center.y];
				}
				break;
			case 7:
				if (!center.HasTopWall() && !this.tiles[center.x, center.y - 1].HasLeftWall() && !center.HasLeftWall() && !this.tiles[center.x - 1, center.y].HasTopWall())
				{
					return this.tiles[center.x - 1, center.y - 1];
				}
				break;
			}
			return null;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00045094 File Offset: 0x00043294
		public void BuildSurrondingWalls()
		{
			if (GameManager.PLANONEDITING)
			{
				return;
			}
			Point selectedTexCoords = Editor.SelectedTexCoords;
			Editor.SelectedTexCoords = new Point(63, 63);
			for (int i = 0; i < Sector.width; i++)
			{
				this.tiles[i, 0].ToggleWall(3, this);
				this.tiles[i, Sector.height - 1].ToggleWall(1, this);
				this.tiles[0, i].ToggleWall(2, this);
				this.tiles[Sector.width - 1, i].ToggleWall(0, this);
			}
			Editor.SelectedTexCoords = selectedTexCoords;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0004512F File Offset: 0x0004332F
		public void SavePathing()
		{
			if (this.SectorLevel == 0)
			{
				new MasterPathfinder(this, this.tiles, Sector.width, Sector.height, true, this.LevelName);
			}
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00045158 File Offset: 0x00043358
		public void AddBlankWalls()
		{
			Terminal.WriteMessage("Adding blank walls....");
			int num = 0;
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					if (j > 0 && this.tiles[j, i].HasLeftWall() && !this.tiles[j - 1, i].HasRightWall())
					{
						this.tiles[j - 1, i].wallTextureCoords[0].X = 31;
						this.tiles[j - 1, i].wallTextureCoords[0].Y = 31;
						this.tiles[j - 1, i].ToggleWallNoTex(0, this);
						num++;
					}
					if (j < Sector.width - 1 && this.tiles[j, i].HasRightWall() && !this.tiles[j + 1, i].HasLeftWall())
					{
						this.tiles[j + 1, i].wallTextureCoords[2].X = 31;
						this.tiles[j + 1, i].wallTextureCoords[2].Y = 31;
						this.tiles[j + 1, i].ToggleWallNoTex(2, this);
						num++;
					}
					if (i > 0 && this.tiles[j, i].HasTopWall() && !this.tiles[j, i - 1].HasBottomWall())
					{
						this.tiles[j, i - 1].wallTextureCoords[1].X = 31;
						this.tiles[j, i - 1].wallTextureCoords[1].Y = 31;
						this.tiles[j, i - 1].ToggleWallNoTex(1, this);
						num++;
					}
					if (i < Sector.height - 1 && this.tiles[j, i].HasBottomWall() && !this.tiles[j, i + 1].HasTopWall())
					{
						this.tiles[j, i + 1].wallTextureCoords[3].X = 31;
						this.tiles[j, i + 1].wallTextureCoords[3].Y = 31;
						this.tiles[j, i + 1].ToggleWallNoTex(3, this);
						num++;
					}
				}
			}
			Terminal.WriteMessage("Added " + num + " walls.");
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000453F8 File Offset: 0x000435F8
		public void NoPathOnEmptyFloors()
		{
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					if ((this.tiles[j, i].GetGroundTexCoords() == new Point(31, 31) || this.tiles[j, i].GetGroundTexCoords() == new Point(63, 63)) && !this.tiles[j, i].TileProperties.Contains(TilePropertyType.NoPath))
					{
						this.tiles[j, i].TileProperties.Add(TilePropertyType.NoPath);
					}
				}
			}
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x000454A4 File Offset: 0x000436A4
		public List<Tile> SpawningTileList()
		{
			List<Tile> list = new List<Tile>();
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					if (!this.tiles[j, i].TileProperties.Contains(TilePropertyType.NoPath) && !this.tiles[j, i].TileProperties.Contains(TilePropertyType.NoSpawn))
					{
						list.Add(this.tiles[j, i]);
					}
				}
			}
			return list;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00045520 File Offset: 0x00043720
		public List<Tile> CarePackageSpawnList()
		{
			List<Tile> list = new List<Tile>();
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					if (this.tiles[j, i].TileProperties.Contains(TilePropertyType.CarePackageSpawn))
					{
						list.Add(this.tiles[j, i]);
					}
				}
			}
			return list;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00045584 File Offset: 0x00043784
		public bool InLineOfSight(Vector3 startPos, Vector3 endPos)
		{
			Vector3 vector = endPos - startPos;
			Vector3 vector2 = startPos;
			vector.Normalize();
			Tile tileAtLocation = this.GetTileAtLocation(vector2);
			if (this.GetTileAtLocation(endPos) == null)
			{
				return true;
			}
			while (tileAtLocation != null && tileAtLocation != this.GetTileAtLocation(endPos))
			{
				if (tileAtLocation.CollidedBullet(startPos, new Vector2(vector.X, vector.Z), 10f, 1f) != -1)
				{
					return false;
				}
				vector2 += vector;
				tileAtLocation = this.GetTileAtLocation(vector2);
			}
			return true;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x000455FC File Offset: 0x000437FC
		public bool InLineOfSightMelee(Vector3 startPos, Vector3 endPos)
		{
			Vector3 vector = endPos - startPos;
			Vector3 vector2 = startPos;
			vector.Normalize();
			vector *= 0.1f;
			Tile tileAtLocation = this.GetTileAtLocation(vector2);
			if (this.GetTileAtLocation(endPos) == null)
			{
				return true;
			}
			while (tileAtLocation != null && tileAtLocation != this.GetTileAtLocation(endPos))
			{
				if (tileAtLocation.CollidedBullet(startPos, new Vector2(vector.X, vector.Z), 20f, 1f) != -1)
				{
					return false;
				}
				vector2 += vector;
				tileAtLocation = this.GetTileAtLocation(vector2);
			}
			return true;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00045680 File Offset: 0x00043880
		public List<Tile> ClearADDED()
		{
			List<Tile> list = new List<Tile>(1024);
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					this.tiles[j, i].ADDED = false;
					this.tiles[j, i].parent = null;
					this.tiles[j, i].cost = 2.1474836E+09f;
					if (!this.tiles[j, i].TileProperties.Contains(TilePropertyType.NoPath))
					{
						list.Add(this.tiles[j, i]);
					}
				}
			}
			return list;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00045728 File Offset: 0x00043928
		public void SaveSectorVertices(string fileName, bool optimized = false)
		{
			if (optimized)
			{
				string fileName2 = fileName + "_Ground";
				string fileName3 = fileName + "_Walls";
				List<VertexWrapper> obj = this.CreateWrapper(this.groundVertices);
				List<VertexWrapper> obj2 = this.CreateWrapper(this.wallVertices);
				XMLSaverLoader.SaveProto<List<VertexWrapper>>(fileName2, obj);
				XMLSaverLoader.SaveProto<List<VertexWrapper>>(fileName3, obj2);
				return;
			}
			string fileName4 = fileName + "_Ground.xml";
			string fileName5 = fileName + "_Walls.xml";
			XMLSaverLoader.SaveObject<CustomLevelVertexDec[]>(fileName4, this.groundVertices);
			XMLSaverLoader.SaveObject<CustomLevelVertexDec[]>(fileName5, this.wallVertices);
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000457A8 File Offset: 0x000439A8
		public void LoadSectorVertices(string fileName, bool optimized = false)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			if (optimized)
			{
				string fileName2 = fileName + "_Ground.bin";
				string fileName3 = fileName + "_Walls.bin";
				List<VertexWrapper> list;
				XMLSaverLoader.LoadProto<List<VertexWrapper>>(fileName2, out list);
				List<VertexWrapper> list2;
				XMLSaverLoader.LoadProto<List<VertexWrapper>>(fileName3, out list2);
				this.groundVertices = this.UnWrap(list);
				this.wallVertices = this.UnWrap(list2);
			}
			else
			{
				string fileName4 = fileName + "_Ground.xml";
				string fileName5 = fileName + "_Walls.xml";
				XMLSaverLoader.LoadObject<CustomLevelVertexDec[]>(fileName4, out this.groundVertices);
				XMLSaverLoader.LoadObject<CustomLevelVertexDec[]>(fileName5, out this.wallVertices);
			}
			this.BuildLevel();
			Terminal.WriteMessage(fileName + " loaded in " + stopwatch.Elapsed.TotalSeconds);
			stopwatch.Reset();
			stopwatch.Start();
			this.groundVertexBuffer = new VertexBuffer(Global.GraphicsDevice, typeof(CustomLevelVertexDec), this.groundVertices.Length, BufferUsage.WriteOnly);
			this.groundVertexBuffer.SetData<CustomLevelVertexDec>(this.groundVertices);
			if (!this.XBOX)
			{
				this.wallVertexBuffer = new VertexBuffer(Global.GraphicsDevice, typeof(CustomLevelVertexDec), this.wallVertices.Length, BufferUsage.WriteOnly);
				this.wallVertexBuffer.SetData<CustomLevelVertexDec>(this.wallVertices);
			}
			else
			{
				this.wallVertexBuffers = new List<VertexBuffer>();
				this.splitList = new List<CustomLevelVertexDec[]>();
				for (int i = 0; i < 4; i++)
				{
					CustomLevelVertexDec[] array = new CustomLevelVertexDec[this.wallVertices.Length / 4];
					Array.Copy(this.wallVertices, i * (this.wallVertices.Length / 4), array, 0, this.wallVertices.Length / 4);
					this.splitList.Add(array);
				}
				for (int j = 0; j < this.splitList.Count; j++)
				{
					this.wallVertexBuffers.Add(new VertexBuffer(Global.GraphicsDevice, typeof(CustomLevelVertexDec), this.splitList[j].Length, BufferUsage.WriteOnly));
					this.wallVertexBuffers[j].SetData<CustomLevelVertexDec>(this.splitList[j]);
				}
			}
			stopwatch.Stop();
			Terminal.WriteMessage(fileName + " built vertices in " + stopwatch.Elapsed.TotalSeconds);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x000459D8 File Offset: 0x00043BD8
		private List<VertexWrapper> CreateWrapper(CustomLevelVertexDec[] list)
		{
			List<VertexWrapper> list2 = new List<VertexWrapper>();
			for (int i = 0; i < list.Length; i++)
			{
				Vector3Wrapper vertexPosition = default(Vector3Wrapper);
				vertexPosition.X = list[i].Position.X;
				vertexPosition.Y = list[i].Position.Y;
				vertexPosition.Z = list[i].Position.Z;
				Vector2Wrapper vertexTexCoord = default(Vector2Wrapper);
				vertexTexCoord.X = list[i].TextureCoordinate.X;
				vertexTexCoord.Y = list[i].TextureCoordinate.Y;
				Vector2Wrapper vertexLightTexCoord = default(Vector2Wrapper);
				vertexLightTexCoord.X = list[i].LightTextureCoordinate.X;
				vertexLightTexCoord.Y = list[i].LightTextureCoordinate.Y;
				list2.Add(new VertexWrapper
				{
					vertexPosition = vertexPosition,
					vertexTexCoord = vertexTexCoord,
					vertexLightTexCoord = vertexLightTexCoord
				});
			}
			return list2;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00045AEC File Offset: 0x00043CEC
		private CustomLevelVertexDec[] UnWrap(List<VertexWrapper> list)
		{
			CustomLevelVertexDec[] array = new CustomLevelVertexDec[list.Count];
			int num = 0;
			foreach (VertexWrapper vertexWrapper in list)
			{
				Vector3 pos = new Vector3(vertexWrapper.vertexPosition.X, vertexWrapper.vertexPosition.Y, vertexWrapper.vertexPosition.Z);
				Vector2 tex = new Vector2(vertexWrapper.vertexTexCoord.X, vertexWrapper.vertexTexCoord.Y);
				Vector2 lightTex = new Vector2(vertexWrapper.vertexLightTexCoord.X, vertexWrapper.vertexLightTexCoord.Y);
				CustomLevelVertexDec customLevelVertexDec = new CustomLevelVertexDec(pos, tex, lightTex);
				array[num] = customLevelVertexDec;
				num++;
			}
			return array;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00045BC4 File Offset: 0x00043DC4
		public void ComputeShadows(SpriteBatch spriteBatch, bool main)
		{
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					this.tiles[j, i].ComputeShadows();
					this.tiles[j, i].RebuildTile(this);
					if (main)
					{
						this.tiles[j, i].RenderFloorShadows(spriteBatch);
					}
				}
			}
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00045C2C File Offset: 0x00043E2C
		public void ClearWallsAndFloors()
		{
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					this.tiles[j, i].ClearWallsAndFloor();
					int sectorLevel = this.SectorLevel;
					this.tiles[j, i].RebuildTile(this);
				}
			}
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00045C88 File Offset: 0x00043E88
		public void ClearTPIs()
		{
			for (int i = 0; i < Sector.height; i++)
			{
				for (int j = 0; j < Sector.width; j++)
				{
					this.tiles[j, i].TileProperties.Clear();
				}
			}
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00045CCC File Offset: 0x00043ECC
		public void CreatePartAtEveryTile(ParticleType type, bool rand)
		{
			for (int i = 0; i < Sector.width; i++)
			{
				for (int j = 0; j < Sector.height; j++)
				{
					Vector3 pos = Vector3.Zero;
					if (rand)
					{
						pos = VerchickMath.GetRandomPosition(new Vector3((float)i, 0.25f, (float)j), 1f);
					}
					else
					{
						pos = new Vector3((float)i, 0.25f, (float)j);
					}
					Global.MasterCache.CreateParticle(type, pos, Vector3.Zero);
				}
			}
		}

		// Token: 0x040008EA RID: 2282
		private Tile[,] tiles;

		// Token: 0x040008EB RID: 2283
		public static int width = 32;

		// Token: 0x040008EC RID: 2284
		public static int height = 32;

		// Token: 0x040008ED RID: 2285
		public CustomLevelVertexDec[] groundVertices;

		// Token: 0x040008EE RID: 2286
		private VertexBuffer groundVertexBuffer;

		// Token: 0x040008EF RID: 2287
		public CustomLevelVertexDec[] wallVertices;

		// Token: 0x040008F0 RID: 2288
		private VertexBuffer wallVertexBuffer;

		// Token: 0x040008F1 RID: 2289
		private List<VertexBuffer> wallVertexBuffers;

		// Token: 0x040008F2 RID: 2290
		private int groundVerticeCount;

		// Token: 0x040008F3 RID: 2291
		private int wallVerticeCount;

		// Token: 0x040008F4 RID: 2292
		private Texture2D TESTWALL;

		// Token: 0x040008F5 RID: 2293
		public int SectorLevel;

		// Token: 0x040008F6 RID: 2294
		private string LevelName;

		// Token: 0x040008F7 RID: 2295
		private AlphaTestEffect alph;

		// Token: 0x040008F8 RID: 2296
		private bool XBOX;

		// Token: 0x040008F9 RID: 2297
		private static bool BUILDEMPTYFLOOR = false;

		// Token: 0x040008FA RID: 2298
		private RasterizerState stateFloor;

		// Token: 0x040008FB RID: 2299
		private RasterizerState stateWalls;

		// Token: 0x040008FC RID: 2300
		private List<CustomLevelVertexDec[]> splitList;
	}
}
