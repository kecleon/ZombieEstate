using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000055 RID: 85
	public static class BloodEffect
	{
		// Token: 0x060001F2 RID: 498 RVA: 0x0000E3C2 File Offset: 0x0000C5C2
		public static void Update()
		{
			BloodEffect.CurrentAlpha -= 0.015f;
			if (BloodEffect.CurrentAlpha < 0f)
			{
				BloodEffect.CurrentAlpha = 0f;
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000E3EA File Offset: 0x0000C5EA
		public static void FireBloodEffect(float delta)
		{
			BloodEffect.CurrentAlpha = delta;
			if (BloodEffect.CurrentAlpha > 1f)
			{
				BloodEffect.CurrentAlpha = 1f;
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000E408 File Offset: 0x0000C608
		public static void Draw(SpriteBatch spriteBatch)
		{
			if (BloodEffect.CurrentAlpha > 0f)
			{
				spriteBatch.Draw(Global.BloodEffect, Global.ScreenRect, Color.White * BloodEffect.CurrentAlpha);
			}
		}

		// Token: 0x0400017C RID: 380
		private static float CurrentAlpha;
	}
}
