using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200006F RID: 111
	public class Level
	{
		// Token: 0x06000298 RID: 664 RVA: 0x0001482C File Offset: 0x00012A2C
		public Level(string filename)
		{
			this.game = Global.Game;
			this.FileName = filename;
			this.LoadGlobals();
			if (GameManager.LOADFLATFIELD)
			{
				this.Sectors = new List<Sector>();
				this.Sectors.Add(new Sector(this.game, 0, this.FileName));
				this.Sectors.Add(new Sector(this.game, 1, this.FileName));
				this.Sectors.Add(new Sector(this.game, 2, this.FileName));
				Global.Level = this.MainSector;
				DateTime now = DateTime.Now;
				Terminal.WriteMessage("Loading adj lists...");
				for (int i = 0; i < 32; i++)
				{
					for (int j = 0; j < 32; j++)
					{
						Global.Level.GetTile(i, j).BuildAdjacentList();
						if (Global.Level.GetTile(i, j).TileProperties.Contains(TilePropertyType.ShopKeep))
						{
							Level.ShopKeepLocation = new Vector3((float)i + 0.5f, 0f, (float)j + 0.8f);
						}
						if (Global.Level.GetTile(i, j).TileProperties.Contains(TilePropertyType.PlayerOneSpawn))
						{
							this.PlayerSpawns[0] = new Vector3((float)i + 0.5f, 0f, (float)j + 0.5f);
						}
						if (Global.Level.GetTile(i, j).TileProperties.Contains(TilePropertyType.PlayerTwoSpawn))
						{
							this.PlayerSpawns[1] = new Vector3((float)i + 0.5f, 0f, (float)j + 0.5f);
						}
						if (Global.Level.GetTile(i, j).TileProperties.Contains(TilePropertyType.PlayerThreeSpawn))
						{
							this.PlayerSpawns[2] = new Vector3((float)i + 0.5f, 0f, (float)j + 0.5f);
						}
						if (Global.Level.GetTile(i, j).TileProperties.Contains(TilePropertyType.PlayerFourSpawn))
						{
							this.PlayerSpawns[3] = new Vector3((float)i + 0.5f, 0f, (float)j + 0.5f);
						}
						if (Global.Level.GetTile(i, j).TileProperties.Contains(TilePropertyType.DemonFire))
						{
							this.FireTiles.Add(Global.Level.GetTile(i, j));
						}
						if (Global.Level.GetTile(i, j).TileProperties.Contains(TilePropertyType.BossArea))
						{
							this.BossArea.Add(Global.Level.GetTile(i, j));
						}
					}
				}
				Terminal.WriteMessage("Loaded adj lists in " + (DateTime.Now - now) + "seconds?");
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00014AFC File Offset: 0x00012CFC
		private void LoadGlobals()
		{
			if (this.FileName == "Estate")
			{
				Global.MainOnTop = false;
				Global.DarkMod = 1f;
				Global.CurrentLevelLightTex = Global.Content.Load<Texture2D>("Estate_Shadow");
			}
			if (this.FileName == "Mall")
			{
				Global.MainOnTop = true;
				Global.DarkMod = 1.5f;
				Global.CurrentLevelLightTex = Global.Content.Load<Texture2D>("Mall_Shadow");
			}
			if (this.FileName == "Skyscraper")
			{
				Global.MainOnTop = true;
				Global.DarkMod = 1f;
				Global.CurrentLevelLightTex = Global.Content.Load<Texture2D>("Skyscraper_Shadow");
			}
			if (this.FileName == "School")
			{
				Global.MainOnTop = false;
				Global.DarkMod = 1f;
				Global.CurrentLevelLightTex = Global.Content.Load<Texture2D>("School_Shadow");
			}
			if (this.FileName == "DesertTown")
			{
				Global.MainOnTop = false;
				Global.DarkMod = 1f;
				Global.CurrentLevelLightTex = Global.Content.Load<Texture2D>("DesertTown_Shadow");
			}
			if (this.FileName == "Office")
			{
				Global.MainOnTop = true;
				Global.DarkMod = 1f;
				Global.CurrentLevelLightTex = Global.Content.Load<Texture2D>("Office_Shadow");
			}
			if (this.FileName == "Farm")
			{
				Global.MainOnTop = false;
				Global.DarkMod = 1f;
				Global.CurrentLevelLightTex = Global.Content.Load<Texture2D>("Farm_Shadow");
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00014C84 File Offset: 0x00012E84
		public void DrawLevel()
		{
			for (int i = 0; i < this.Sectors.Count; i++)
			{
				this.Sectors[i].DrawSector(i == this.mainSectorIndex);
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00014CC4 File Offset: 0x00012EC4
		public void DrawLevel(bool TopView, int sectorIndex)
		{
			for (int i = 0; i < this.Sectors.Count; i++)
			{
				if (TopView)
				{
					if (i <= sectorIndex)
					{
						this.Sectors[i].DrawSector(i == this.mainSectorIndex);
					}
				}
				else if (Editor.DrawOnlyCurrentSector)
				{
					if (Global.MainOnTop)
					{
						if (i >= sectorIndex)
						{
							this.Sectors[i].DrawSector(i == this.mainSectorIndex);
						}
					}
					else if (i <= sectorIndex)
					{
						this.Sectors[i].DrawSector(i == this.mainSectorIndex);
					}
				}
				else
				{
					this.Sectors[i].DrawSector(i == this.mainSectorIndex);
				}
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00014D7C File Offset: 0x00012F7C
		public void SaveLevel()
		{
			Terminal.WriteMessage("*****Saving Level*****");
			for (int i = 0; i < this.Sectors.Count; i++)
			{
				SectorSaver.SaveSector(this.Sectors[i], this.FileName + i + ".xml");
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00014DD0 File Offset: 0x00012FD0
		public void LoadLevel()
		{
			Terminal.WriteMessage("*****Loading Level*****");
			if (GameManager.PLANONEDITING)
			{
				Console.WriteLine("***Loading level on main thread.");
				this.loadThread = null;
				this.ThreadLoad();
				return;
			}
			this.loadThread = new Thread(new ThreadStart(this.ThreadLoad));
			this.loadThread.Start();
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00014E28 File Offset: 0x00013028
		public Sector MainSector
		{
			get
			{
				return this.Sectors[this.mainSectorIndex];
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00014E3B File Offset: 0x0001303B
		public Sector GetSector(int index)
		{
			return this.Sectors[index];
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00014E49 File Offset: 0x00013049
		public int SectorCount()
		{
			return this.Sectors.Count;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00014E58 File Offset: 0x00013058
		private void ThreadLoad()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Stopwatch stopwatch = Stopwatch.StartNew();
			Terminal.WriteMessage("------------Initializing level...");
			this.Sectors = new List<Sector>();
			this.Sectors.Add(new Sector(this.game, 0, this.FileName));
			this.Sectors.Add(new Sector(this.game, 1, this.FileName));
			if (this.FileName == "Mall" || this.FileName == "Skyscraper" || this.FileName == "Office" || this.FileName == "Farm")
			{
				this.Sectors.Add(new Sector(this.game, 2, this.FileName));
			}
			Global.Level = this.MainSector;
			Level.TilesDoneLoading = 0;
			Terminal.WriteMessage("------------Initialized level in " + stopwatch.Elapsed.TotalSeconds);
			stopwatch.Reset();
			stopwatch.Start();
			Terminal.WriteMessage("------------Loading level...");
			this.doneLoading = false;
			for (int i = 0; i < this.Sectors.Count; i++)
			{
				if (i == this.mainSectorIndex || GameManager.PLANONEDITING)
				{
					SectorSaver.LoadSector(this.Sectors[i], string.Concat(new object[]
					{
						"Data//Levels//",
						this.FileName,
						i,
						".xml"
					}));
				}
				if (!GameManager.PLANONEDITING)
				{
					this.Sectors[i].LoadSectorVertices("Data//Levels//" + this.FileName + i, true);
				}
				else
				{
					this.Sectors[i].LoadSectorVertices("Data//Levels//" + this.FileName + i, false);
				}
				if (i == this.mainSectorIndex)
				{
					this.Sectors[i].BuildAdjacentLists();
				}
				if (!GameManager.PLANONEDITING && i != this.mainSectorIndex)
				{
					this.Sectors[i].CLEARTILES();
				}
			}
			if (!GameManager.PLANONEDITING)
			{
				for (int j = 0; j < this.Sectors.Count; j++)
				{
					if (j != this.mainSectorIndex)
					{
						this.Sectors[j].CLEARTILES();
					}
				}
			}
			Level.ShopKeepLocation = new Vector3(15.5f, 0f, 10f);
			for (int k = 0; k < 32; k++)
			{
				for (int l = 0; l < 32; l++)
				{
					if (Global.Level.GetTile(k, l).TileProperties.Contains(TilePropertyType.ShopKeep))
					{
						Level.ShopKeepLocation = new Vector3((float)k + 0.5f, 0f, (float)l + 0.8f);
					}
					if (Global.Level.GetTile(k, l).TileProperties.Contains(TilePropertyType.PlayerOneSpawn))
					{
						this.PlayerSpawns[0] = new Vector3((float)k + 0.5f, 0f, (float)l + 0.5f);
					}
					if (Global.Level.GetTile(k, l).TileProperties.Contains(TilePropertyType.PlayerTwoSpawn))
					{
						this.PlayerSpawns[1] = new Vector3((float)k + 0.5f, 0f, (float)l + 0.5f);
					}
					if (Global.Level.GetTile(k, l).TileProperties.Contains(TilePropertyType.PlayerThreeSpawn))
					{
						this.PlayerSpawns[2] = new Vector3((float)k + 0.5f, 0f, (float)l + 0.5f);
					}
					if (Global.Level.GetTile(k, l).TileProperties.Contains(TilePropertyType.PlayerFourSpawn))
					{
						this.PlayerSpawns[3] = new Vector3((float)k + 0.5f, 0f, (float)l + 0.5f);
					}
					if (Global.Level.GetTile(k, l).TileProperties.Contains(TilePropertyType.DemonFire))
					{
						this.FireTiles.Add(Global.Level.GetTile(k, l));
					}
					if (Global.Level.GetTile(k, l).TileProperties.Contains(TilePropertyType.BossArea))
					{
						this.BossArea.Add(Global.Level.GetTile(k, l));
					}
				}
			}
			if (this.FileName == "Skyscraper")
			{
				Level.ShopKeepLocation = Vector3.Subtract(Level.ShopKeepLocation, new Vector3(-1f, 0f, 1f));
			}
			Global.WaveMaster = new WaveMaster(this.MainSector, Global.WAVE_GEN_SEED);
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			Thread.Sleep(4000);
			this.doneLoading = true;
			Terminal.WriteMessage("------------Loaded level in " + stopwatch.Elapsed.TotalSeconds);
			Terminal.WriteMessage("*****Loading of level complete!*****");
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00015344 File Offset: 0x00013544
		public void ComputerShadows()
		{
			SpriteBatch spriteBatch = new SpriteBatch(Global.GraphicsDevice);
			RenderTarget2D renderTarget2D = new RenderTarget2D(Global.GraphicsDevice, 16 * Sector.width, 16 * Sector.height);
			Global.GraphicsDevice.SetRenderTarget(renderTarget2D);
			Global.GraphicsDevice.Clear(Color.Transparent);
			spriteBatch.Begin();
			for (int i = 0; i < this.Sectors.Count; i++)
			{
				this.Sectors[i].ComputeShadows(spriteBatch, i == this.mainSectorIndex);
			}
			spriteBatch.End();
			Global.GraphicsDevice.SetRenderTarget(null);
			FileStream fileStream = new FileStream("Something.png", FileMode.Create);
			renderTarget2D.SaveAsPng(fileStream, 16 * Sector.width, 16 * Sector.height);
			fileStream.Close();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00015404 File Offset: 0x00013604
		public void SaveLevelVertices()
		{
			for (int i = 0; i < this.Sectors.Count; i++)
			{
				string fileName = this.FileName + i;
				this.Sectors[i].SaveSectorVertices(fileName, false);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0001544C File Offset: 0x0001364C
		public void LoadLevelVertices()
		{
			for (int i = 0; i < this.Sectors.Count; i++)
			{
				string fileName = this.FileName + i;
				this.Sectors[i].LoadSectorVertices(fileName, false);
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00015494 File Offset: 0x00013694
		public void SaveLevelVerticesOptimized()
		{
			for (int i = 0; i < this.Sectors.Count; i++)
			{
				string fileName = this.FileName + i;
				this.Sectors[i].SaveSectorVertices(fileName, true);
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000154DC File Offset: 0x000136DC
		public void LoadLevelVerticesOptimized()
		{
			for (int i = 0; i < this.Sectors.Count; i++)
			{
				string fileName = this.FileName + i;
				this.Sectors[i].LoadSectorVertices(fileName, true);
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00015524 File Offset: 0x00013724
		public void BossActivated()
		{
			foreach (Tile tile in this.FireTiles)
			{
				AOE aoe = new AOE(AOEType.DemonFire, 1f, 1f, new Vector3((float)tile.x, 0f, (float)tile.y), -1f, null, null, true);
				this.BossEffects.Add(aoe);
				Global.MasterCache.CreateObject(aoe);
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000155BC File Offset: 0x000137BC
		public void BossDeactivated()
		{
			foreach (AOE aoe in this.BossEffects)
			{
				aoe.DestroyObject();
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0001560C File Offset: 0x0001380C
		public void ClearLevel()
		{
			for (int i = 0; i < this.Sectors.Count; i++)
			{
				if (this.Sectors[i] != null)
				{
					this.Sectors[i].ClearWallsAndFloors();
					this.Sectors[i].BuildLevel();
				}
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00015660 File Offset: 0x00013860
		public void ClearTPIs()
		{
			for (int i = 0; i < this.Sectors.Count; i++)
			{
				if (this.Sectors[i] != null)
				{
					this.Sectors[i].ClearTPIs();
				}
			}
		}

		// Token: 0x04000294 RID: 660
		private List<Sector> Sectors;

		// Token: 0x04000295 RID: 661
		private string FileName;

		// Token: 0x04000296 RID: 662
		private int mainSectorIndex;

		// Token: 0x04000297 RID: 663
		public bool doneLoading;

		// Token: 0x04000298 RID: 664
		public static int TOTAL_TILES = 3072;

		// Token: 0x04000299 RID: 665
		public static int TilesDoneLoading = 0;

		// Token: 0x0400029A RID: 666
		private Thread loadThread;

		// Token: 0x0400029B RID: 667
		private Game game;

		// Token: 0x0400029C RID: 668
		public static Vector3 ShopKeepLocation = new Vector3(15.5f, 0f, 10f);

		// Token: 0x0400029D RID: 669
		public Vector3[] PlayerSpawns = new Vector3[4];

		// Token: 0x0400029E RID: 670
		public List<Tile> FireTiles = new List<Tile>();

		// Token: 0x0400029F RID: 671
		public List<Tile> BossArea = new List<Tile>();

		// Token: 0x040002A0 RID: 672
		public List<AOE> BossEffects = new List<AOE>();
	}
}
