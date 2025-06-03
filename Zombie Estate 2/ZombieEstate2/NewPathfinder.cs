using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ZombieEstate2
{
	// Token: 0x02000070 RID: 112
	internal class NewPathfinder
	{
		// Token: 0x060002AC RID: 684 RVA: 0x000156D0 File Offset: 0x000138D0
		public NewPathfinder(Sector sector)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					Tile tile = sector.GetTile(i, j);
					this.Dijkstra(tile, sector);
					this.TraverseList(tile, sector);
				}
				Console.WriteLine("Column " + i + " Completed");
			}
			StreamWriter streamWriter = new StreamWriter(GameManager.LevelName + "_PathFind.txt");
			for (int k = 0; k < 32; k++)
			{
				for (int l = 0; l < 32; l++)
				{
					for (int m = 0; m < 32; m++)
					{
						for (int n = 0; n < 32; n++)
						{
							streamWriter.Write(sector.GetTile(k, l).Directions[m, n].ToString() + ",");
						}
					}
					streamWriter.WriteLine();
				}
			}
			streamWriter.Close();
			stopwatch.Stop();
			int num = (int)stopwatch.Elapsed.TotalSeconds;
			Terminal.WriteMessage("DONE");
			Terminal.WriteMessage("Seconds: " + num.ToString());
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00015810 File Offset: 0x00013A10
		private void Dijkstra(Tile src, Sector sector)
		{
			List<Tile> list = sector.ClearADDED();
			List<Tile> list2 = new List<Tile>(1024);
			src.cost = 0f;
			src.ADDED = true;
			while (list.Count != 0)
			{
				Tile tile = this.ExtractMin(list);
				list.Remove(tile);
				list2.Add(tile);
				foreach (Tile tile2 in this.GetNeighbors(sector, tile))
				{
					if (tile2.cost > tile.cost + this.Weight(tile, tile2))
					{
						tile2.cost = tile.cost + this.Weight(tile, tile2);
						tile2.parent = tile;
					}
				}
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000158E4 File Offset: 0x00013AE4
		private float Weight(Tile u, Tile v)
		{
			if (this.getAngle(u.x, u.y, v) % 2 == 1)
			{
				return 1.41f;
			}
			return 1f;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0001590C File Offset: 0x00013B0C
		private Tile ExtractMin(List<Tile> list)
		{
			Tile tile = list[0];
			foreach (Tile tile2 in list)
			{
				if (tile2.cost < tile.cost)
				{
					tile = tile2;
				}
			}
			return tile;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0001596C File Offset: 0x00013B6C
		private List<Tile> GetNeighbors(Sector sector, Tile tile)
		{
			List<Tile> list = new List<Tile>(8);
			for (int i = 0; i < 8; i++)
			{
				Tile tile2 = sector.Neighbor(tile, i);
				if (tile2 != null && !tile2.ADDED)
				{
					list.Add(tile2);
				}
			}
			return list;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000159A8 File Offset: 0x00013BA8
		private void BuildTree(Tile src, Sector sector)
		{
			Queue<Tile> queue = new Queue<Tile>();
			queue.Enqueue(src);
			while (queue.Count > 0)
			{
				Tile tile = queue.Dequeue();
				for (int i = 0; i < 8; i++)
				{
					Tile tile2 = sector.Neighbor(tile, i);
					if (tile2 != null && !tile2.ADDED)
					{
						tile2.ADDED = true;
						tile2.parent = tile;
						queue.Enqueue(tile2);
					}
				}
			}
			this.TraverseList(src, sector);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00015A14 File Offset: 0x00013C14
		private void TraverseList(Tile src, Sector sector)
		{
			int x = src.x;
			int y = src.y;
			src.Directions[x, y] = 10;
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					if (i != x || j != x)
					{
						Tile tile = sector.GetTile(i, j);
						if (tile.parent != null)
						{
							tile.Directions[x, y] = this.getAngle(tile.x, tile.y, tile.parent);
						}
					}
				}
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00015A9C File Offset: 0x00013C9C
		private Queue<Tile> AddNeighbors(Tile tile, Queue<Tile> tiles, Sector sector)
		{
			List<Tile> list = new List<Tile>(8);
			for (int i = 0; i < 8; i++)
			{
				Tile tile2 = sector.Neighbor(tile, i);
				if (tile2 != null && !tile2.ADDED)
				{
					tile2.ADDED = true;
					tile2.parent = tile;
					tiles.Enqueue(tile2);
					list.Add(tile2);
				}
			}
			foreach (Tile tile3 in list)
			{
				this.AddNeighbors(tile3, tiles, sector);
			}
			return tiles;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00015B34 File Offset: 0x00013D34
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
	}
}
