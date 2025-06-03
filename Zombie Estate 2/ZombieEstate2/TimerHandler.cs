using System;
using System.Collections.Generic;

namespace ZombieEstate2
{
	// Token: 0x02000134 RID: 308
	public static class TimerHandler
	{
		// Token: 0x060008D0 RID: 2256 RVA: 0x0004A298 File Offset: 0x00048498
		public static void Update(float elapsed)
		{
			for (int i = 0; i < TimerHandler.timers.Count; i++)
			{
				if (TimerHandler.timers[i].IndependentOfTime)
				{
					TimerHandler.timers[i].Update(Global.REAL_GAME_TIME);
				}
				else
				{
					TimerHandler.timers[i].Update(elapsed);
				}
			}
			foreach (Timer item in TimerHandler.removeList)
			{
				TimerHandler.timers.Remove(item);
			}
			TimerHandler.removeList.Clear();
		}

		// Token: 0x0400098D RID: 2445
		public static List<Timer> timers = new List<Timer>();

		// Token: 0x0400098E RID: 2446
		public static List<Timer> removeList = new List<Timer>();
	}
}
