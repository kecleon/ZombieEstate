using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000026 RID: 38
	public class Boss : Zombie
	{
		// Token: 0x060000DF RID: 223 RVA: 0x00006A9D File Offset: 0x00004C9D
		public Boss() : base(ZombieType.NOTHING)
		{
			this.EngageDistance = float.MaxValue;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006AB2 File Offset: 0x00004CB2
		public virtual string GetBossName()
		{
			return "Null";
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006AB9 File Offset: 0x00004CB9
		public virtual string GetBossSubtitle()
		{
			return "Nuller Null";
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006AC0 File Offset: 0x00004CC0
		public virtual Vector3 GetPlayerStartPos(int i)
		{
			return new Vector3(this.Position.X + (float)i, this.Position.Y, this.Position.Z + 8f);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005D3F File Offset: 0x00003F3F
		public virtual bool ActivateBossLevelStuff()
		{
			return true;
		}

		// Token: 0x0400009C RID: 156
		public Vector3 CameraOffset;
	}
}
