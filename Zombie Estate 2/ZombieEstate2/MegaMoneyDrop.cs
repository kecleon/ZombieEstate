using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000020 RID: 32
	internal class MegaMoneyDrop : Drop
	{
		// Token: 0x060000CB RID: 203 RVA: 0x000061D0 File Offset: 0x000043D0
		public MegaMoneyDrop(Vector3 pos)
		{
			this.TextureCoord = new Point(67, 39);
			pos.Y = 0.35f;
			this.scale = 0.25f;
			this.ActivateObject(pos, this.TextureCoord);
			Global.MasterCache.CreateObject(this);
			this.Duration = 30f;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006238 File Offset: 0x00004438
		public override bool GiveDrop(Player player)
		{
			this.amount = 100;
			if (Global.PlayerList.Count == 1)
			{
				this.amount = 100;
			}
			if (Global.PlayerList.Count == 2)
			{
				this.amount = 75;
			}
			if (Global.PlayerList.Count == 3)
			{
				this.amount = 60;
			}
			if (Global.PlayerList.Count == 4)
			{
				this.amount = 55;
			}
			foreach (Player player2 in Global.PlayerList)
			{
				if (player2.IAmOwnedByLocalPlayer)
				{
					player2.Stats.AddMoney(player2.ZombieKillMoneyMod * (float)this.amount);
				}
			}
			SoundEngine.PlaySound("ze2_money", player.Position, -0.2f, -0.2f, 0.6f);
			base.GiveDrop(player);
			return true;
		}

		// Token: 0x04000094 RID: 148
		private int amount = 100;
	}
}
