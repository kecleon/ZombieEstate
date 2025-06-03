using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200007C RID: 124
	public class Part2D_Money : Particle2D
	{
		// Token: 0x060002FC RID: 764 RVA: 0x000176C8 File Offset: 0x000158C8
		public Part2D_Money(Vector3 pos, float amount, bool small, Player player)
		{
			this.amount = amount;
			if (small)
			{
				this.font = Global.StoreFontBig;
			}
			else
			{
				this.font = Global.StoreFontXtraLarge;
			}
			this.text = "$" + string.Format("{0}", (int)amount);
			this.ScreenPos = VerchickMath.GetScreenPosition(pos);
			this.ScreenPos = VerchickMath.CenterText(this.font, this.ScreenPos, this.text);
			this.parent = player;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00017768 File Offset: 0x00015968
		public override void Update(float elapsed)
		{
			if (this.DEAD)
			{
				return;
			}
			if (this.hangTime > 0f)
			{
				this.hangTime -= 0.016666668f;
			}
			else
			{
				this.time += 0.016666668f;
			}
			float num = this.time / this.totalFlyTime;
			this.ScreenPos = Vector2.SmoothStep(this.ScreenPos, PlayerHUDNew.MoneyLoc, num);
			if (VerchickMath.WithinDistance(this.ScreenPos, PlayerHUDNew.MoneyLoc, 10f))
			{
				this.parent.Stats.AddMoney(this.amount);
				this.DEAD = true;
				return;
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void Draw(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x040002E3 RID: 739
		private string text;

		// Token: 0x040002E4 RID: 740
		private SpriteFont font;

		// Token: 0x040002E5 RID: 741
		private float amount;

		// Token: 0x040002E6 RID: 742
		private float time;

		// Token: 0x040002E7 RID: 743
		private float hangTime = 0.5f;

		// Token: 0x040002E8 RID: 744
		private float totalFlyTime = 1f;

		// Token: 0x040002E9 RID: 745
		private Player parent;
	}
}
