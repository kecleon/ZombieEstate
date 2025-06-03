using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.StoreScreen.XboxStore
{
	// Token: 0x02000144 RID: 324
	internal class XboxStatsDialog : XboxStoreDialog
	{
		// Token: 0x060009C9 RID: 2505 RVA: 0x0004EFA8 File Offset: 0x0004D1A8
		public XboxStatsDialog(string text, Vector2 topLeft, Player player, GunStats stats, int level) : base(text, topLeft, player, XboxStoreDialog.XboxDialogType.Ok)
		{
			this.mComp = new GunStatsComparer(player, null, stats.SpecialProperties[level], stats.GunProperties[level], new Rectangle((int)topLeft.X + 5, (int)topLeft.Y + 5, 490, 270), null, null);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0004F00C File Offset: 0x0004D20C
		public XboxStatsDialog(string text, Vector2 topLeft, Player player, GunStats stats1, GunStats stats2, int level) : base(text, topLeft, player, XboxStoreDialog.XboxDialogType.Ok)
		{
			this.mComp = new GunStatsComparer(player, null, stats1.SpecialProperties[level], stats1.GunProperties[level], new Rectangle((int)topLeft.X + 5, (int)topLeft.Y + 5, 490, 270), stats2.SpecialProperties[level + 1], stats2.GunProperties[level + 1]);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0004F08D File Offset: 0x0004D28D
		public override void Update()
		{
			this.mComp.Update();
			base.Update();
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0004F0A0 File Offset: 0x0004D2A0
		public override void DrawExtra(SpriteBatch spriteBatch)
		{
			this.mComp.Draw(spriteBatch);
			base.DrawExtra(spriteBatch);
		}

		// Token: 0x04000A41 RID: 2625
		private GunStatsComparer mComp;
	}
}
