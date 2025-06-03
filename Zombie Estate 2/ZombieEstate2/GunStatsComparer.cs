using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.StoreScreen.PCStore.Stats;

namespace ZombieEstate2
{
	// Token: 0x020000CD RID: 205
	internal class GunStatsComparer : StatComparer
	{
		// Token: 0x06000549 RID: 1353 RVA: 0x000284F5 File Offset: 0x000266F5
		public GunStatsComparer(Player player, PCStore store, SpecialProperties sp1, GunProperties gp1, Rectangle loc, SpecialProperties sp2 = null, GunProperties gp2 = null) : base(loc, store, player)
		{
			this.Spec1 = sp1;
			this.Gun1 = gp1;
			this.Spec2 = sp2;
			this.Gun2 = gp2;
			this.BuildList();
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00028528 File Offset: 0x00026728
		public override void BuildList()
		{
			base.AddCell(StatBuilder.LowDamage(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.HighDamage(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.DmgType(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.ShotTime(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.CritChance(this.Spec1, this.Spec2));
			base.AddCell(StatBuilder.Accuracy(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.Penetration(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.Reload(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.ClipSize(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.KnockBack(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.BouncePercent(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.ExplosionDmg(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.ExplosionRange(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.NumBulletsFired(this.Gun1, this.Gun2));
			base.AddCell(StatBuilder.BulletsCostToFire(this.Gun1, this.Gun2));
			base.AddCell(SpecStatBuilder.GUNLifeSteal(this.Spec1, this.Spec2));
			base.AddCell(SpecStatBuilder.GUNArmor(this.Spec1, this.Spec2));
			base.AddCell(SpecStatBuilder.GUNFireResist(this.Spec1, this.Spec2));
			base.AddCell(SpecStatBuilder.GUNWaterResist(this.Spec1, this.Spec2));
			base.AddCell(SpecStatBuilder.GUNEarthResist(this.Spec1, this.Spec2));
			base.BuildList();
		}

		// Token: 0x04000551 RID: 1361
		private SpecialProperties Spec1;

		// Token: 0x04000552 RID: 1362
		private GunProperties Gun1;

		// Token: 0x04000553 RID: 1363
		private SpecialProperties Spec2;

		// Token: 0x04000554 RID: 1364
		private GunProperties Gun2;
	}
}
