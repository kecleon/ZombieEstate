using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000011 RID: 17
	public static class AnimationSetUp
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00002F28 File Offset: 0x00001128
		public static void SetUpZombieAnims(ZombieType zombieType, AnimationHandler handler, float speed)
		{
			if (zombieType == ZombieType.NormalZombie)
			{
			}
			if (zombieType == ZombieType.Mummy)
			{
			}
			int x = 56;
			int num = 17;
			for (int i = 0; i < 5; i++)
			{
				handler.AddLineAnimation(new Point(x, num + 4 * i), 4, "Walk_Down_" + i, speed);
				handler.AddLineAnimation(new Point(x, num + 1 + 4 * i), 4, "Walk_Right_" + i, speed);
				handler.AddLineAnimation(new Point(x, num + 2 + 4 * i), 4, "Walk_Left_" + i, speed);
				handler.AddLineAnimation(new Point(x, num + 3 + 4 * i), 4, "Walk_Up_" + i, speed);
			}
		}
	}
}
