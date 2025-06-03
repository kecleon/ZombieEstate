using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000B2 RID: 178
	internal class AmmoDisplay : StoreElement
	{
		// Token: 0x0600048B RID: 1163 RVA: 0x0002187C File Offset: 0x0001FA7C
		public AmmoDisplay(Vector2 pos, Player player)
		{
			this.Player = player;
			this.AmmoAssault = new AmmoMeter(new Point(4 + (int)pos.X, (int)pos.Y), Color.White, new Point(0, 43));
			this.AmmoShells = new AmmoMeter(new Point(4 + (int)pos.X, (int)pos.Y + 38), Color.White, new Point(0, 45));
			this.AmmoHeavy = new AmmoMeter(new Point(4 + (int)pos.X, (int)pos.Y + 76), Color.White, new Point(0, 44));
			this.AmmoExplosive = new AmmoMeter(new Point(4 + (int)pos.X, (int)pos.Y + 114), Color.White, new Point(0, 46));
			this.AmmoSpecial = new AmmoMeter(new Point(4 + (int)pos.X, (int)pos.Y + 152), Color.White, new Point(0, 47));
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00021988 File Offset: 0x0001FB88
		public override void Draw(SpriteBatch spriteBatch)
		{
			this.AmmoAssault.DrawMeterWithText(spriteBatch, this.Player.Stats.GetAmmo(AmmoType.ASSAULT), this.Player.Stats.GetMaxAmmo(AmmoType.ASSAULT), false);
			this.AmmoHeavy.DrawMeterWithText(spriteBatch, this.Player.Stats.GetAmmo(AmmoType.HEAVY), this.Player.Stats.GetMaxAmmo(AmmoType.HEAVY), false);
			this.AmmoShells.DrawMeterWithText(spriteBatch, this.Player.Stats.GetAmmo(AmmoType.SHELLS), this.Player.Stats.GetMaxAmmo(AmmoType.SHELLS), false);
			this.AmmoExplosive.DrawMeterWithText(spriteBatch, this.Player.Stats.GetAmmo(AmmoType.EXPLOSIVE), this.Player.Stats.GetMaxAmmo(AmmoType.EXPLOSIVE), false);
			this.AmmoSpecial.DrawMeterWithText(spriteBatch, this.Player.Stats.GetAmmo(AmmoType.SPECIAL), this.Player.Stats.GetMaxAmmo(AmmoType.SPECIAL), false);
			base.Draw(spriteBatch);
		}

		// Token: 0x04000478 RID: 1144
		private AmmoMeter AmmoAssault;

		// Token: 0x04000479 RID: 1145
		private AmmoMeter AmmoHeavy;

		// Token: 0x0400047A RID: 1146
		private AmmoMeter AmmoExplosive;

		// Token: 0x0400047B RID: 1147
		private AmmoMeter AmmoShells;

		// Token: 0x0400047C RID: 1148
		private AmmoMeter AmmoSpecial;

		// Token: 0x0400047D RID: 1149
		private Player Player;
	}
}
