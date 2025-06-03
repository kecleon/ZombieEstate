using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace ZombieEstate2
{
	// Token: 0x02000071 RID: 113
	internal class MasterPathfinder
	{
		// Token: 0x060002B5 RID: 693 RVA: 0x00015C14 File Offset: 0x00013E14
		public MasterPathfinder(Sector sector, Tile[,] tiles, int w, int h, bool pathFind, string levelName)
		{
			this.sector = sector;
			this.tiles = tiles;
			this.width = w;
			this.height = h;
			if (!pathFind)
			{
				this.Message("Pathfinding Skipped.");
				this.LoadPath(tiles, levelName);
				return;
			}
			Stopwatch stopwatch = Stopwatch.StartNew();
			Thread thread = new Thread(new ThreadStart(this.UpdateProgess));
			thread.Start();
			this.ToCompleteTiles = w * h * w * h;
			this.Message("Initializing Tile Directions...");
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					tiles[i, j].Directions = new int[this.width, this.height];
					for (int k = 0; k < this.width; k++)
					{
						for (int l = 0; l < this.height; l++)
						{
							tiles[i, j].Directions[k, l] = -1;
						}
					}
				}
			}
			this.Message("Tile Directions Initialized");
			Thread thread2 = new Thread(new ParameterizedThreadStart(this.PathFind));
			Thread thread3 = new Thread(new ParameterizedThreadStart(this.PathFind2));
			Thread thread4 = new Thread(new ParameterizedThreadStart(this.PathFind3));
			thread2.Start(stopwatch);
			thread3.Start(stopwatch);
			thread4.Start(stopwatch);
			while (!this.oneDone || !this.twoDone || !this.threeDone)
			{
			}
			this.Message("Saving...");
			StreamWriter streamWriter = new StreamWriter("TestPathFind.txt");
			for (int m = 0; m < this.width; m++)
			{
				for (int n = 0; n < this.height; n++)
				{
					for (int num = 0; num < this.width; num++)
					{
						for (int num2 = 0; num2 < this.height; num2++)
						{
							streamWriter.WriteLine(tiles[m, n].Directions[num, num2].ToString());
						}
					}
				}
			}
			streamWriter.Close();
			this.Message("Saved.");
			this.Message("------Complete------");
			stopwatch.Stop();
			this.Message("Completed in " + stopwatch.Elapsed.Minutes + " minutes.");
			thread.Abort();
			Global.Game.Exit();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00015E7C File Offset: 0x0001407C
		public void LoadPath(Tile[,] tiles, string levelName)
		{
			Terminal.WriteMessage("Initializing Tile Directions...");
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					tiles[i, j].Directions = new int[this.width, this.height];
				}
			}
			Terminal.WriteMessage("Tile Directions Initialized");
			this.Message("Loading...");
			Stopwatch stopwatch = Stopwatch.StartNew();
			Terminal.WriteMessage("Pathfinder: Loading...");
			StreamReader streamReader;
			try
			{
				streamReader = new StreamReader("Data//Levels//" + levelName + "_Path.txt");
			}
			catch
			{
				streamReader = new StreamReader("Data//Levels//Estate_Path.txt");
				Terminal.WriteMessage("ERROR: No path map found, loading estate's!", MessageType.ERROR);
			}
			for (int k = 0; k < this.width; k++)
			{
				for (int l = 0; l < this.height; l++)
				{
					string[] array = streamReader.ReadLine().Split(new char[]
					{
						','
					});
					int num = 0;
					for (int m = 0; m < this.width; m++)
					{
						for (int n = 0; n < this.height; n++)
						{
							tiles[k, l].Directions[m, n] = int.Parse(array[num]);
							num++;
						}
					}
				}
			}
			streamReader.Close();
			this.Message("Done Loading");
			stopwatch.Stop();
			Terminal.WriteMessage("Pathfinder: Finished loading in " + stopwatch.Elapsed.TotalSeconds);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00016010 File Offset: 0x00014210
		private void PathFind(object sw)
		{
			Stopwatch stopwatch = sw as Stopwatch;
			this.Message("1>Beginning Pathfinding...");
			for (int i = 0; i < this.width / 3; i++)
			{
				this.Message("1>%" + (float)i / 31f * 100f);
				this.Message("1>So far, " + this.skipped + " tiles have been skipped.");
				this.Message("1>Elapsed time: " + stopwatch.ElapsedMilliseconds / 1000L + " Seconds");
				for (int j = 0; j < this.height; j++)
				{
					for (int k = 0; k < this.width; k++)
					{
						for (int l = 0; l < this.height; l++)
						{
							this.PathFindTile(i, j, k, l);
							this.CompletedTiles++;
						}
					}
				}
			}
			this.Message("1>End Pathfinding ON THREAD 1");
			this.oneDone = true;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00016118 File Offset: 0x00014318
		private void PathFind2(object sw)
		{
			Stopwatch stopwatch = sw as Stopwatch;
			this.Message("2>Beginning Pathfinding...");
			for (int i = this.width / 3; i < 2 * (this.width / 3); i++)
			{
				this.Message("2>%" + (float)i / 31f * 100f);
				this.Message("2>So far, " + this.skipped + " tiles have been skipped.");
				this.Message("2>Elapsed time: " + stopwatch.ElapsedMilliseconds / 1000L + " Seconds");
				for (int j = 0; j < this.height; j++)
				{
					for (int k = 0; k < this.width; k++)
					{
						for (int l = 0; l < this.height; l++)
						{
							this.PathFindTile(i, j, k, l);
							this.CompletedTiles++;
						}
					}
				}
			}
			this.Message("2>End Pathfinding ON THREAD 2");
			this.twoDone = true;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00016228 File Offset: 0x00014428
		private void PathFind3(object sw)
		{
			Stopwatch stopwatch = sw as Stopwatch;
			this.Message("3>Beginning Pathfinding...");
			for (int i = 2 * (this.width / 3); i < this.width; i++)
			{
				this.Message("3>%" + (float)i / 31f * 100f);
				this.Message("3>So far, " + this.skipped + " tiles have been skipped.");
				this.Message("3>Elapsed time: " + stopwatch.ElapsedMilliseconds / 1000L + " Seconds");
				for (int j = 0; j < this.height; j++)
				{
					for (int k = 0; k < this.width; k++)
					{
						for (int l = 0; l < this.height; l++)
						{
							this.PathFindTile(i, j, k, l);
							this.CompletedTiles++;
						}
					}
				}
			}
			this.Message("3>End Pathfinding ON THREAD 3");
			this.threeDone = true;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00016334 File Offset: 0x00014534
		private void PathFindTile(int i, int j, int targI, int targJ)
		{
			if (this.tiles[i, j].Directions[targI, targJ] != -1)
			{
				this.skipped++;
				return;
			}
			if (this.tiles[i, j].TileProperties.Contains(TilePropertyType.NoPath))
			{
				this.skipped++;
				return;
			}
			if (this.tiles[targI, targJ].TileProperties.Contains(TilePropertyType.NoPath))
			{
				this.skipped++;
				return;
			}
			Queue<Tile> queue = new Pathfinder(this.sector, this.tiles[i, j], this.tiles[targI, targJ]).FindPath();
			if (queue == null || queue.Count == 0)
			{
				return;
			}
			queue.Dequeue();
			this.tiles[i, j].Directions[targI, targJ] = this.getAngle(i, j, queue.Peek());
			List<Tile> list = queue.ToList<Tile>();
			while (queue != null && queue.Count != 0)
			{
				Tile tile = queue.Dequeue();
				list.Remove(tile);
				if (queue.Count == 0)
				{
					tile.Directions[targI, targJ] = 10;
				}
				else
				{
					tile.Directions[targI, targJ] = this.getAngle(tile.x, tile.y, queue.Peek());
				}
				if (queue.Count != 0)
				{
					int angle = this.getAngle(tile.x, tile.y, queue.Peek());
					for (int k = 1; k < list.Count; k++)
					{
						Tile tile2 = list[k];
						if (tile.Directions[tile2.x, tile2.y] == -1)
						{
							tile.Directions[tile2.x, tile2.y] = angle;
						}
					}
				}
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00016508 File Offset: 0x00014708
		private int getAngle(int x, int y, Tile nextTile)
		{
			int result = -1;
			if (nextTile.x == x + 1 && nextTile.y == y)
			{
				result = 0;
			}
			if (nextTile.x == x + 1 && nextTile.y == y + 1)
			{
				result = 7;
			}
			if (nextTile.x == x && nextTile.y == y + 1)
			{
				result = 6;
			}
			if (nextTile.x == x - 1 && nextTile.y == y + 1)
			{
				result = 5;
			}
			if (nextTile.x == x - 1 && nextTile.y == y)
			{
				result = 4;
			}
			if (nextTile.x == x - 1 && nextTile.y == y - 1)
			{
				result = 3;
			}
			if (nextTile.x == x && nextTile.y == y - 1)
			{
				result = 2;
			}
			if (nextTile.x == x + 1 && nextTile.y == y - 1)
			{
				result = 1;
			}
			if (nextTile.x == x && nextTile.y == y)
			{
				result = 10;
			}
			return result;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x000165E8 File Offset: 0x000147E8
		private int setAngle(Tile tile, Tile nextTile)
		{
			if (tile.Directions[nextTile.x, nextTile.y] != -1)
			{
				return tile.Directions[nextTile.x, nextTile.y];
			}
			return tile.Directions[nextTile.x, nextTile.y] = this.getAngle(tile.x, tile.y, nextTile);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00016654 File Offset: 0x00014854
		private float PercentComplete()
		{
			return (float)this.CompletedTiles / (float)this.ToCompleteTiles;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00016665 File Offset: 0x00014865
		private void Message(string s)
		{
			Console.WriteLine("Pathfinder: " + s);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00016678 File Offset: 0x00014878
		private void NextTile(List<Tile> tilesToAdd)
		{
			if (tilesToAdd.Count == 0)
			{
				return;
			}
			for (int i = 0; i < tilesToAdd.Count - 1; i++)
			{
				int num = this.setAngle(tilesToAdd[i], tilesToAdd[i + 1]);
				for (int j = i + 1; j < tilesToAdd.Count; j++)
				{
					if (tilesToAdd[i].Directions[tilesToAdd[j].x, tilesToAdd[j].y] != -1)
					{
						tilesToAdd[i].Directions[tilesToAdd[j].x, tilesToAdd[j].y] = num;
					}
				}
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00016728 File Offset: 0x00014928
		private void UpdateProgess()
		{
			int num = 1048576;
			int millisecondsTimeout = 5000;
			for (;;)
			{
				int completedTiles = this.CompletedTiles;
				Thread.Sleep(millisecondsTimeout);
				Console.WriteLine("Completed tiles: " + this.CompletedTiles);
				Console.WriteLine("About %" + (float)this.CompletedTiles / (float)num * 100f + " completed...");
				float num2 = (float)(this.CompletedTiles - completedTiles);
				num2 /= 5f;
				float num3 = (float)(num - this.CompletedTiles) / num2;
				Console.WriteLine("About " + num3 / 60f + " minutes to go...");
			}
		}

		// Token: 0x040002A1 RID: 673
		private bool PATHFIND;

		// Token: 0x040002A2 RID: 674
		private Tile[,] tiles;

		// Token: 0x040002A3 RID: 675
		private int width;

		// Token: 0x040002A4 RID: 676
		private int height;

		// Token: 0x040002A5 RID: 677
		private Sector sector;

		// Token: 0x040002A6 RID: 678
		private int CompletedTiles;

		// Token: 0x040002A7 RID: 679
		private int ToCompleteTiles;

		// Token: 0x040002A8 RID: 680
		private int skipped;

		// Token: 0x040002A9 RID: 681
		private bool oneDone;

		// Token: 0x040002AA RID: 682
		private bool twoDone;

		// Token: 0x040002AB RID: 683
		private bool threeDone;
	}
}
