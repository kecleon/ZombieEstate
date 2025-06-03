using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000023 RID: 35
	internal class SuperWeaponDrop : Drop
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x000065F0 File Offset: 0x000047F0
		public SuperWeaponDrop(Player player, Vector3 pos) : base(player)
		{
			this.Pulse = false;
			this.gunName = player.Stats.CharSettings.superWeapon;
			GunStats stats = GunStatsLoader.GetStats(this.gunName);
			this.startTex = new Point(stats.GunXCoord, stats.GunYCoord + 1);
			this.ActivateObject(pos, this.startTex);
			Global.MasterCache.CreateObject(this);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000665F File Offset: 0x0000485F
		public override void Update(float elapsed)
		{
			this.Duration = 40000f;
			base.Update(elapsed);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006673 File Offset: 0x00004873
		public override bool GiveDrop(Player player)
		{
			player.AddGun(this.gunName, true);
			player.Stats.GiveAmmo(AmmoType.SPECIAL, 1);
			return base.GiveDrop(player);
		}

		// Token: 0x04000098 RID: 152
		private string gunName;
	}
}
