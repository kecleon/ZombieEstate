using System;
using System.Collections.Generic;

namespace ZombieEstate2
{
	// Token: 0x02000124 RID: 292
	internal static class ChunkExtension
	{
		// Token: 0x06000856 RID: 2134 RVA: 0x00045D54 File Offset: 0x00043F54
		public static IEnumerable<T[]> Chunkify<T>(this IEnumerable<T> source, int size)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (size < 1)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			using (IEnumerator<T> iter = source.GetEnumerator())
			{
				while (iter.MoveNext())
				{
					T[] array = new T[size];
					array[0] = iter.Current;
					int num = 1;
					while (num < size && iter.MoveNext())
					{
						array[num] = iter.Current;
						num++;
					}
					yield return array;
				}
			}
			IEnumerator<T> iter = null;
			yield break;
			yield break;
		}
	}
}
