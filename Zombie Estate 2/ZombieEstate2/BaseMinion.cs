using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000087 RID: 135
	internal class BaseMinion : Minion
	{
		// Token: 0x0600032C RID: 812 RVA: 0x00018E0C File Offset: 0x0001700C
		public BaseMinion(Shootable owner, string gun, int ammo, float movespeed, Vector3 pos, Point tex, bool muzzleFlash, int lvl, int health_count, string buff, string buffArgs, ushort startUID) : base(owner, pos, gun, ammo, movespeed, tex, muzzleFlash, lvl, health_count, buff, buffArgs, startUID)
		{
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00018E34 File Offset: 0x00017034
		public override void Init()
		{
			base.Init();
		}
	}
}
