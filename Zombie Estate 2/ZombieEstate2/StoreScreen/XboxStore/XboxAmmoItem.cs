using System;

namespace ZombieEstate2.StoreScreen.XboxStore
{
	// Token: 0x02000142 RID: 322
	public class XboxAmmoItem : XboxItem
	{
		// Token: 0x060009B2 RID: 2482 RVA: 0x0004DF44 File Offset: 0x0004C144
		public XboxAmmoItem(AmmoType ammoType, int amount, int cost, Player p) : base(PCItem.GetAmmoRect(ammoType).X / 16, PCItem.GetAmmoRect(ammoType).Y / 16, null, false, p.Stats.GetMoney() > cost)
		{
			this.mType = ammoType;
			this.mAmount = amount;
			this.mCost = cost;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0004DF99 File Offset: 0x0004C199
		public void Purchased(Player p)
		{
			p.Stats.GiveAmmo(this.mType, this.mAmount);
		}

		// Token: 0x04000A2C RID: 2604
		public AmmoType mType;

		// Token: 0x04000A2D RID: 2605
		public int mAmount;

		// Token: 0x04000A2E RID: 2606
		public int mCost;
	}
}
