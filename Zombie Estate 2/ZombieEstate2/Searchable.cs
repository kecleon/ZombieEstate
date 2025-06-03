using System;
using System.Collections.Generic;

namespace ZombieEstate2
{
	// Token: 0x02000075 RID: 117
	internal abstract class Searchable<VALUE>
	{
		// Token: 0x060002CE RID: 718
		public abstract bool IsGoal();

		// Token: 0x060002CF RID: 719
		public abstract List<Searchable<VALUE>> nextStates();

		// Token: 0x060002D0 RID: 720
		public abstract float PathCost();

		// Token: 0x060002D1 RID: 721
		public abstract float Compare(Searchable<VALUE> second);

		// Token: 0x060002D2 RID: 722 RVA: 0x00016B05 File Offset: 0x00014D05
		public Searchable(Searchable<VALUE> previous, VALUE value)
		{
			this.Previous = previous;
			this.Value = value;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00016B1C File Offset: 0x00014D1C
		public Searchable<VALUE> FindGoal()
		{
			PriorityQueue<Searchable<VALUE>> priorityQueue = new PriorityQueue<Searchable<VALUE>>();
			List<Searchable<VALUE>> list = new List<Searchable<VALUE>>();
			foreach (Searchable<VALUE> searchable in this.nextStates())
			{
				priorityQueue.Enqueue(searchable.PathCost(), searchable);
			}
			int num = 0;
			while (!priorityQueue.Empty())
			{
				num++;
				Searchable<VALUE> searchable2 = priorityQueue.Dequeue();
				if (searchable2.IsGoal())
				{
					return searchable2;
				}
				foreach (Searchable<VALUE> searchable3 in searchable2.nextStates())
				{
					if (!this.ListContainsEquivalent(list, searchable3))
					{
						priorityQueue.Enqueue(searchable3.PathCost(), searchable3);
						list.Add(searchable3);
					}
				}
			}
			return null;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00016C08 File Offset: 0x00014E08
		private bool ListContainsEquivalent(List<Searchable<VALUE>> list, Searchable<VALUE> state)
		{
			using (List<Searchable<VALUE>>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Compare(state) == 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00016C64 File Offset: 0x00014E64
		public Queue<VALUE> FindPath()
		{
			Searchable<VALUE> searchable = this.FindGoal();
			Stack<VALUE> stack = new Stack<VALUE>();
			Queue<VALUE> queue = new Queue<VALUE>();
			while (searchable != null)
			{
				stack.Push(searchable.Value);
				searchable = searchable.Previous;
			}
			while (stack.Count > 0)
			{
				queue.Enqueue(stack.Pop());
			}
			return queue;
		}

		// Token: 0x040002B3 RID: 691
		public VALUE Value;

		// Token: 0x040002B4 RID: 692
		public Searchable<VALUE> Previous;
	}
}
