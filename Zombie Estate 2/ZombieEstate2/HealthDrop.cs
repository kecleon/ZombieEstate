using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200002E RID: 46
	internal class HealthDrop : Drop
	{
		// Token: 0x06000108 RID: 264 RVA: 0x000078FC File Offset: 0x00005AFC
		public HealthDrop(Vector3 pos)
		{
			this.Indicator = false;
			this.TextureCoord = new Point(0, 42);
			pos.Y = 0.25f;
			this.ActivateObject(pos, this.TextureCoord);
			Global.MasterCache.CreateObject(this);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000794C File Offset: 0x00005B4C
		public override bool GiveDrop(Player player)
		{
			if (player.Stats.HealthPacks < 3)
			{
				if (player.IAmOwnedByLocalPlayer)
				{
					PlayerStats stats = player.Stats;
					int healthPacks = stats.HealthPacks;
					stats.HealthPacks = healthPacks + 1;
				}
				base.GiveDrop(player);
				player.HealthPackTip();
				for (int i = 0; i < 6; i++)
				{
					Global.MasterCache.CreateParticle(ParticleType.AmmoPickup, this.Position, Vector3.Zero);
				}
				SoundEngine.PlaySound("ze2_ammo", this.Position);
				return true;
			}
			return false;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000079C8 File Offset: 0x00005BC8
		public override bool CanPickUpObject(Player player)
		{
			return player.Stats.HealthPacks < 3;
		}

		// Token: 0x040000B7 RID: 183
		private static int amount = 25;
	}
}
