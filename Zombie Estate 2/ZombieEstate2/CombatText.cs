using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000079 RID: 121
	internal class CombatText
	{
		// Token: 0x060002F6 RID: 758 RVA: 0x000173B2 File Offset: 0x000155B2
		public CombatText()
		{
			this.DEAD = true;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x000173E4 File Offset: 0x000155E4
		public void Setup(CombatTextType type, string text, Vector3 pos)
		{
			this.Type = type;
			this.Text = text;
			this.DEAD = false;
			this.Position = VerchickMath.GetScreenPosition(VerchickMath.GetRandomPosition(pos, 0.5f));
			this.Lived = 0f;
			this.Pos3d = pos;
			switch (type)
			{
			case CombatTextType.Damage:
				this.Scale = 0.5f;
				this.Color = Color.Red;
				this.Font = Global.StoreFontBig;
				this.LiveTime = 1.2f;
				break;
			case CombatTextType.Heal:
				this.Scale = 1.5f;
				this.Color = Color.LightGreen;
				this.Font = Global.StoreFontBig;
				this.LiveTime = 1f;
				break;
			case CombatTextType.HealCrit:
				this.Scale = 1f;
				this.Color = Color.White;
				this.LiveTime = 1.2f;
				this.Font = Global.StoreFontBig;
				break;
			}
			this.Position = VerchickMath.CenterText(this.Font, this.Position, text);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x000174E8 File Offset: 0x000156E8
		public void Update(float elapsed)
		{
			this.Lived += elapsed;
			if (this.Lived >= this.LiveTime)
			{
				this.DEAD = true;
			}
			this.Pos3d.Y = this.Pos3d.Y + 0.4f * elapsed;
			this.Position = VerchickMath.GetScreenPosition(this.Pos3d);
			this.Position = VerchickMath.CenterText(this.Font, this.Position, this.Text);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0001755C File Offset: 0x0001575C
		public void Draw(SpriteBatch spriteBatch)
		{
			float scale = 1f;
			float num = 1f - this.Lived / this.LiveTime;
			if (num < 0.5f)
			{
				scale = 1f * (num / 0.5f);
			}
			Shadow.DrawString(this.Text, this.Font, this.Position, 1, this.Color * scale, Color.Black * scale, spriteBatch);
		}

		// Token: 0x040002D1 RID: 721
		public bool DEAD;

		// Token: 0x040002D2 RID: 722
		private CombatTextType Type;

		// Token: 0x040002D3 RID: 723
		private string Text;

		// Token: 0x040002D4 RID: 724
		private SpriteFont Font = Global.StoreFont;

		// Token: 0x040002D5 RID: 725
		private float Scale = 1f;

		// Token: 0x040002D6 RID: 726
		private Color Color = Color.Red;

		// Token: 0x040002D7 RID: 727
		private Vector2 Position;

		// Token: 0x040002D8 RID: 728
		private Vector3 Pos3d;

		// Token: 0x040002D9 RID: 729
		private float LiveTime;

		// Token: 0x040002DA RID: 730
		private float Lived;
	}
}
