using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x020000A3 RID: 163
	public static class Vibrate
	{
		// Token: 0x0600043C RID: 1084 RVA: 0x0001F524 File Offset: 0x0001D724
		public static void VibrateController(int index, float duration, float strength)
		{
			Vibrate.Vibration item = default(Vibrate.Vibration);
			item.timeLeft = duration;
			item.index = index;
			GamePad.SetVibration((PlayerIndex)index, strength, strength);
			Vibrate.mVibs.Add(item);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001F560 File Offset: 0x0001D760
		public static void Update()
		{
			for (int i = 0; i < Vibrate.mVibs.Count; i++)
			{
				Vibrate.Vibration value = default(Vibrate.Vibration);
				value.timeLeft = Vibrate.mVibs[i].timeLeft - 0.016666668f;
				value.index = Vibrate.mVibs[i].index;
				Vibrate.mVibs[i] = value;
				if (Vibrate.mVibs[i].timeLeft <= 0f)
				{
					GamePad.SetVibration((PlayerIndex)Vibrate.mVibs[i].index, 0f, 0f);
					Vibrate.mVibs.RemoveAt(i);
				}
			}
		}

		// Token: 0x04000415 RID: 1045
		private static List<Vibrate.Vibration> mVibs = new List<Vibrate.Vibration>();

		// Token: 0x0200020D RID: 525
		private struct Vibration
		{
			// Token: 0x04000E10 RID: 3600
			public float timeLeft;

			// Token: 0x04000E11 RID: 3601
			public int index;
		}
	}
}
