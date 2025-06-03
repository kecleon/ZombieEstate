using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.StoreScreen.PCStore.Stats;

namespace ZombieEstate2
{
	// Token: 0x020000CC RID: 204
	internal class CharStatsComparer : StatComparer
	{
		// Token: 0x06000546 RID: 1350 RVA: 0x00028244 File Offset: 0x00026444
		public CharStatsComparer(PCStore store, SpecialProperties sp1, Rectangle loc) : base(loc, store, null)
		{
			CharStatsComparer.DEF = new SpecialProperties();
			CharStatsComparer.DEF.MaxHealth = 100f;
			CharStatsComparer.DEF.Speed = 3.8f;
			this.Spec1 = sp1;
			this.BuildList();
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00028284 File Offset: 0x00026484
		public override void BuildList()
		{
			base.AddCell(SpecStatBuilder.MaxHealth(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.Speed(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.Armor(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.MoneyEarned(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.CritChance(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.LifeSteal(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.BulletDmg(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.ReloadSpeed(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.ShotSpeed(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.MeleeDmg(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.SwingSpeed(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.AssaultAmmo(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.ShellAmmo(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.HeavyAmmo(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.ExplosiveAmmo(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.MinionDmg(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.MinionAmmo(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.MinionRateOfFire(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.HealingDone(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.HealingRecieved(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.HealOverTime(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.FireBonus(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.WaterBonus(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.EarthBonus(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.FireResist(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.WaterResist(this.Spec1, CharStatsComparer.DEF));
			base.AddCell(SpecStatBuilder.EarthResist(this.Spec1, CharStatsComparer.DEF));
			base.BuildList();
		}

		// Token: 0x0400054F RID: 1359
		private SpecialProperties Spec1;

		// Token: 0x04000550 RID: 1360
		public static SpecialProperties DEF = new SpecialProperties();
	}
}
