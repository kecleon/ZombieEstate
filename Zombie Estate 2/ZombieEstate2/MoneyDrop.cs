using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000021 RID: 33
	internal class MoneyDrop : Drop
	{
		// Token: 0x060000CD RID: 205 RVA: 0x00006328 File Offset: 0x00004528
		public MoneyDrop(Vector3 pos)
		{
			this.TextureCoord = new Point(4, 44);
			pos.Y = 0.35f;
			this.scale = 0.2f;
			this.ActivateObject(pos, this.TextureCoord);
			Global.MasterCache.CreateObject(this);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006384 File Offset: 0x00004584
		public override bool GiveDrop(Player player)
		{
			this.amount = 20;
			if (Global.PlayerList.Count == 1)
			{
				this.amount = 20;
			}
			if (Global.PlayerList.Count == 2)
			{
				this.amount = 15;
			}
			if (Global.PlayerList.Count == 3)
			{
				this.amount = 12;
			}
			if (Global.PlayerList.Count == 4)
			{
				this.amount = 10;
			}
			foreach (Player player2 in Global.PlayerList)
			{
				if (player2.IAmOwnedByLocalPlayer)
				{
					player2.Stats.AddMoney(player2.ZombieKillMoneyMod * (float)this.amount);
				}
			}
			SoundEngine.PlaySound("ze2_money", 0.6f);
			base.GiveDrop(player);
			return true;
		}

		// Token: 0x04000095 RID: 149
		private int amount = 20;
	}
}
