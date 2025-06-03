using System;
using System.Collections.Generic;
using System.Linq;

namespace ZombieEstate2
{
	// Token: 0x02000074 RID: 116
	internal class PriorityQueue<VALUE>
	{
		// Token: 0x060002CA RID: 714 RVA: 0x00016A44 File Offset: 0x00014C44
		public void Enqueue(float key, VALUE value)
		{
			if (this.list.ContainsKey(key))
			{
				int index = this.list.IndexOfKey(key);
				this.list.Values[index].Push(value);
				return;
			}
			Stack<VALUE> stack = new Stack<VALUE>();
			stack.Push(value);
			this.list.Add(key, stack);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00016AA0 File Offset: 0x00014CA0
		public VALUE Dequeue()
		{
			KeyValuePair<float, Stack<VALUE>> keyValuePair = this.list.First<KeyValuePair<float, Stack<VALUE>>>();
			Stack<VALUE> value = keyValuePair.Value;
			VALUE result = value.Pop();
			if (value.Count == 0)
			{
				this.list.Remove(keyValuePair.Key);
			}
			return result;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00016AE2 File Offset: 0x00014CE2
		public bool Empty()
		{
			return this.list.Count == 0;
		}

		// Token: 0x040002B2 RID: 690
		private SortedList<float, Stack<VALUE>> list = new SortedList<float, Stack<VALUE>>();
	}
}
