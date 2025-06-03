using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.UI.Menus
{
	// Token: 0x02000167 RID: 359
	internal class Credits : Menu
	{
		// Token: 0x06000AF4 RID: 2804 RVA: 0x0005AC68 File Offset: 0x00058E68
		public Credits() : base(true, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height - 140)))
		{
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0005AC94 File Offset: 0x00058E94
		public override void Setup()
		{
			this.title = "";
			this.MenuBG = Global.Credits;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0002E26C File Offset: 0x0002C46C
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
		}

		// Token: 0x04000BB9 RID: 3001
		public static Texture2D mTex;
	}
}
