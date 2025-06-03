using System;
using System.Collections.Concurrent;

namespace ZombieEstate2.UI.Chat
{
	// Token: 0x02000172 RID: 370
	public class FixedSizedQueue<T> : ConcurrentQueue<T>
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0005C885 File Offset: 0x0005AA85
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x0005C88D File Offset: 0x0005AA8D
		public int Size { get; private set; }

		// Token: 0x06000B32 RID: 2866 RVA: 0x0005C896 File Offset: 0x0005AA96
		public FixedSizedQueue(int size)
		{
			this.Size = size;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0005C8B0 File Offset: 0x0005AAB0
		public new void Enqueue(T obj)
		{
			base.Enqueue(obj);
			object obj2 = this.syncObject;
			lock (obj2)
			{
				while (base.Count > this.Size)
				{
					T t;
					base.TryDequeue(out t);
				}
			}
		}

		// Token: 0x04000BFC RID: 3068
		private readonly object syncObject = new object();
	}
}
