using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200002B RID: 43
	internal class AmmoDrop : Drop
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x00007100 File Offset: 0x00005300
		public AmmoDrop(Vector3 pos, AmmoType type)
		{
			this.ammoType = type;
			switch (this.ammoType)
			{
			case AmmoType.ASSAULT:
				this.startTex = new Point(0, 43);
				break;
			case AmmoType.HEAVY:
				this.startTex = new Point(0, 44);
				break;
			case AmmoType.SHELLS:
				this.startTex = new Point(0, 45);
				break;
			case AmmoType.EXPLOSIVE:
				this.startTex = new Point(0, 46);
				break;
			case AmmoType.SPECIAL:
				this.startTex = new Point(0, 47);
				break;
			}
			this.TextureCoord = this.startTex;
			pos.Y = 0.25f;
			this.ActivateObject(pos, this.TextureCoord);
			Global.MasterCache.CreateObject(this);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000071BC File Offset: 0x000053BC
		public override bool GiveDrop(Player player)
		{
			bool flag = false;
			if (player.IAmOwnedByLocalPlayer)
			{
				switch (this.ammoType)
				{
				case AmmoType.ASSAULT:
					flag = player.Stats.GiveAmmo(this.ammoType, AmmoDrop.AssaultAmount);
					break;
				case AmmoType.HEAVY:
					flag = player.Stats.GiveAmmo(this.ammoType, AmmoDrop.HeavyAmount);
					break;
				case AmmoType.SHELLS:
					flag = player.Stats.GiveAmmo(this.ammoType, AmmoDrop.ShellsAmount);
					break;
				case AmmoType.EXPLOSIVE:
					flag = player.Stats.GiveAmmo(this.ammoType, AmmoDrop.ExplosiveAmount);
					break;
				case AmmoType.SPECIAL:
					flag = player.Stats.GiveAmmo(this.ammoType, AmmoDrop.SpecialAmount);
					break;
				}
			}
			if (flag || !player.IAmOwnedByLocalPlayer)
			{
				for (int i = 0; i < 6; i++)
				{
					Global.MasterCache.CreateParticle(ParticleType.AmmoPickup, this.Position, Vector3.Zero);
				}
				SoundEngine.PlaySound("ze2_ammo", this.Position);
				base.GiveDrop(player);
			}
			return false;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000072BB File Offset: 0x000054BB
		public override bool CanPickUpObject(Player player)
		{
			return player.Stats.GetAmmo(this.ammoType) < player.Stats.GetMaxAmmo(this.ammoType);
		}

		// Token: 0x040000A1 RID: 161
		public AmmoType ammoType;

		// Token: 0x040000A2 RID: 162
		private static int AssaultAmount = 60;

		// Token: 0x040000A3 RID: 163
		private static int HeavyAmount = 60;

		// Token: 0x040000A4 RID: 164
		private static int ShellsAmount = 14;

		// Token: 0x040000A5 RID: 165
		private static int ExplosiveAmount = 2;

		// Token: 0x040000A6 RID: 166
		private static int SpecialAmount = 1;
	}
}
