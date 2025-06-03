using System;
using System.Collections.Generic;

namespace WG
{
	// Token: 0x02000004 RID: 4
	public static class Serializer
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002540 File Offset: 0x00000740
		public static void Serialize()
		{
			ZombieWrapper zombieWrapper = new ZombieWrapper();
			zombieWrapper.Name = "Normal";
			zombieWrapper.Start = 0;
			zombieWrapper.Standard = 40;
			zombieWrapper.Variation = 20;
			zombieWrapper.Minimum = 10;
			ZombieWrapper zombieWrapper2 = new ZombieWrapper();
			zombieWrapper2.Name = "A";
			zombieWrapper2.Start = 0;
			zombieWrapper2.Standard = 20;
			zombieWrapper2.Variation = 20;
			zombieWrapper2.Minimum = 20;
			ZombieWrapper zombieWrapper3 = new ZombieWrapper();
			zombieWrapper3.Name = "B";
			zombieWrapper3.Start = 20;
			zombieWrapper3.Standard = 10;
			zombieWrapper3.Variation = 10;
			zombieWrapper3.Minimum = 20;
			ZombieWrapper zombieWrapper4 = new ZombieWrapper();
			zombieWrapper4.Name = "C";
			zombieWrapper4.Start = 30;
			zombieWrapper4.Standard = 15;
			zombieWrapper4.Variation = 20;
			zombieWrapper4.Minimum = 5;
			ZombieWrapper zombieWrapper5 = new ZombieWrapper();
			zombieWrapper5.Name = "D";
			zombieWrapper5.Start = 40;
			zombieWrapper5.Standard = 10;
			zombieWrapper5.Variation = 5;
			zombieWrapper5.Minimum = 10;
			ZombieWrapper zombieWrapper6 = new ZombieWrapper();
			zombieWrapper6.Name = "E";
			zombieWrapper6.Start = 50;
			zombieWrapper6.Standard = 5;
			zombieWrapper6.Variation = 0;
			zombieWrapper6.Minimum = 1;
			List<ZombieWrapper> list = new List<ZombieWrapper>();
			list.Add(zombieWrapper);
			list.Add(zombieWrapper2);
			list.Add(zombieWrapper3);
			list.Add(zombieWrapper4);
			list.Add(zombieWrapper5);
			list.Add(zombieWrapper6);
		}
	}
}
