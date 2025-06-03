using System;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200001D RID: 29
	internal class DialogMaster
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x00005D42 File Offset: 0x00003F42
		public static void ShowDialogs(BlockingDialog dialog)
		{
			DialogMaster.ActiveDialog = dialog;
			Global.Paused = true;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005D50 File Offset: 0x00003F50
		public static void UpdateDialogs()
		{
			if (DialogMaster.ActiveDialog != null)
			{
				DialogMaster.ActiveDialog.Update();
				if (DialogMaster.ActiveDialog.DoneShowing())
				{
					DialogMaster.ActiveDialog = null;
					Global.Paused = false;
				}
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005D7B File Offset: 0x00003F7B
		public static void DrawDialogs(SpriteBatch spriteBatch)
		{
			if (DialogMaster.ActiveDialog != null)
			{
				DialogMaster.ActiveDialog.Draw(spriteBatch);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005D8F File Offset: 0x00003F8F
		public static bool DialogsActive
		{
			get
			{
				return DialogMaster.ActiveDialog != null && DialogMaster.ActiveDialog.DoneShowing();
			}
		}

		// Token: 0x04000090 RID: 144
		private static BlockingDialog ActiveDialog;
	}
}
