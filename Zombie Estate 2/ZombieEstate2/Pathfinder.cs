using System;
using System.Collections.Generic;

namespace ZombieEstate2
{
	// Token: 0x02000073 RID: 115
	internal class Pathfinder : Searchable<Tile>
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0001687C File Offset: 0x00014A7C
		private Pathfinder(Pathfinder previous, Tile Pos) : base(previous, Pos)
		{
			this.prev = previous;
			this.goal = previous.goal;
			this.world = previous.world;
			this.weight = previous.weight + Pos.weight;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000168B8 File Offset: 0x00014AB8
		public Pathfinder(Sector World, Tile Pos, Tile Goal) : base(null, Pos)
		{
			this.goal = Goal;
			this.world = World;
			this.weight = 1f;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000168DB File Offset: 0x00014ADB
		public override bool IsGoal()
		{
			return this.Value.x == this.goal.x && this.Value.y == this.goal.y;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00016910 File Offset: 0x00014B10
		public override List<Searchable<Tile>> nextStates()
		{
			List<Searchable<Tile>> list = new List<Searchable<Tile>>();
			for (int i = 0; i < 8; i++)
			{
				Tile tile;
				if ((tile = this.world.Neighbor(this.Value, i)) != null && (this.prev == null || this.prev.Value != tile))
				{
					list.Add(new Pathfinder(this, tile));
				}
			}
			return list;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0001696C File Offset: 0x00014B6C
		public override float Compare(Searchable<Tile> second)
		{
			Pathfinder.compare_count++;
			if (second.Value.x != this.Value.x)
			{
				return (float)(second.Value.x - this.Value.x);
			}
			if (second.Value.y != this.Value.y)
			{
				return (float)(second.Value.y - this.Value.y);
			}
			return 0f;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000169EC File Offset: 0x00014BEC
		public override float PathCost()
		{
			return 1f * (this.weight + (float)Math.Abs(this.Value.x - this.goal.x) + (float)Math.Abs(this.Value.y - this.goal.y));
		}

		// Token: 0x040002AD RID: 685
		private float weight;

		// Token: 0x040002AE RID: 686
		private Sector world;

		// Token: 0x040002AF RID: 687
		private Tile goal;

		// Token: 0x040002B0 RID: 688
		private Pathfinder prev;

		// Token: 0x040002B1 RID: 689
		public static int compare_count;
	}
}
