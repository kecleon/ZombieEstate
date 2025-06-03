using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200010A RID: 266
	internal class HealWard : GameObject
	{
		// Token: 0x06000731 RID: 1841 RVA: 0x00036E20 File Offset: 0x00035020
		public HealWard(Player parent, Vector3 pos, int level)
		{
			this.parent = parent;
			this.time = 25f;
			AOE obj = new AOE(AOEType.HealWard, 2f, 3f, pos, this.time, null, parent);
			Global.MasterCache.CreateObject(obj);
			this.ActivateObject(pos, new Point(7, 32));
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00036E7A File Offset: 0x0003507A
		public override void Update(float elapsed)
		{
			this.time -= elapsed;
			if (this.time <= 0f)
			{
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x04000729 RID: 1833
		private Player parent;

		// Token: 0x0400072A RID: 1834
		private float time;
	}
}
