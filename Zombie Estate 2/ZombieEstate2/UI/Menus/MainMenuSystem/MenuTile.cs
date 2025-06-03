using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.UI.Menus.MainMenuSystem
{
	// Token: 0x0200016A RID: 362
	internal class MenuTile
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0005AED4 File Offset: 0x000590D4
		public bool Idle
		{
			get
			{
				return this.SM.CurrentState == MenuTile.TileState.Idle;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0005AEE4 File Offset: 0x000590E4
		public bool Active
		{
			get
			{
				return this.SM.CurrentState == MenuTile.TileState.Active;
			}
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0005AEF4 File Offset: 0x000590F4
		public MenuTile(string assetName, Vector2 pos, Vector2 menuPos)
		{
			this.Tex = Global.Content.Load<Texture2D>("MenuTiles\\" + assetName);
			if (MenuTile.SelTex == null)
			{
				MenuTile.SelTex = Global.Content.Load<Texture2D>("MenuTiles\\MenuTile_Selected");
				MenuTile.EmptyTex = Global.Content.Load<Texture2D>("MenuTiles\\MenuTile_Empty");
			}
			this.StartPos = pos;
			this.CurrentPos = pos;
			this.MenuPos = menuPos;
			float num = Vector2.Distance(this.StartPos, this.MenuPos) / 1500f;
			this.SM = new StateMachineEngine<MenuTile.TileState>();
			this.SM.AddState(MenuTile.TileState.Idle, -1f, MenuTile.TileState.Fired, -1, -1, false, null);
			this.SM.AddState(MenuTile.TileState.Fired, 0.4f, MenuTile.TileState.Idle, -1, -1, false, new UpdateStateDelegate(this.UpdateFired));
			this.SM.CurrentState = MenuTile.TileState.Idle;
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void UpdateFired(float d)
		{
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0005AFE0 File Offset: 0x000591E0
		public void Update()
		{
			if (this.Rotation > -0.01f && this.Rotation < 0.01f && this.RotationPeek < 0.025f)
			{
				this.Rotation = 0f;
			}
			if (this.Rotation != 0f)
			{
				this.Rotation += this.RotationRate * (float)this.RotateMod;
				if (((float)this.RotateMod == -1f && this.Rotation < this.RotationPeek * (float)this.RotateMod) || ((float)this.RotateMod == 1f && this.Rotation > this.RotationPeek * (float)this.RotateMod))
				{
					this.RotateMod *= -1;
					this.RotationPeek *= 0.85f;
					this.RotationRate = Math.Max(0.005f, this.RotationRate - 0.004f);
				}
			}
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void UpdateMenu()
		{
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0005B0D0 File Offset: 0x000592D0
		public void DrawTile(SpriteBatch spriteBatch, bool highlighted)
		{
			spriteBatch.Draw(this.Tex, this.CurrentPos + MenuTileManager.ShakeVectorOffset + new Vector2(0f, 2f), null, Color.Black * 0.4f, this.Rotation, new Vector2(64f, 4f), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(this.Tex, this.CurrentPos + MenuTileManager.ShakeVectorOffset, null, Color.White, this.Rotation, new Vector2(64f, 4f), 1f, SpriteEffects.None, 0f);
			if (highlighted)
			{
				spriteBatch.Draw(MenuTile.SelTex, this.CurrentPos + MenuTileManager.ShakeVectorOffset, null, Color.White, this.Rotation, new Vector2(64f, 4f), 1f, SpriteEffects.None, 0f);
			}
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void DrawMenu(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0005B1DC File Offset: 0x000593DC
		public void Fire(float intensity)
		{
			this.RotationRate = Global.RandomFloat(0.038f * intensity, 0.062f * intensity);
			this.RotationPeek = Global.RandomFloat(0.2f * intensity, 0.4f * intensity);
			if (Global.Probability(50))
			{
				this.RotateMod = -1;
			}
			else
			{
				this.RotateMod = 1;
			}
			this.Rotation += this.RotationRate * (float)this.RotateMod;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x0005B24F File Offset: 0x0005944F
		public Rectangle Bounds
		{
			get
			{
				return new Rectangle((int)this.CurrentPos.X - 64, (int)this.CurrentPos.Y - 4, this.Tex.Width, this.Tex.Height);
			}
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0005B289 File Offset: 0x00059489
		public Vector2 GetMenuPos(Vector2 offFromCenter)
		{
			return new Vector2(offFromCenter.X + 306f - 74f + this.MenuPos.X, offFromCenter.Y + 153f + this.MenuPos.Y);
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0005B2C8 File Offset: 0x000594C8
		public void DrawCenteredTitle(string s, SpriteBatch spriteBatch, SpriteFont font)
		{
			Vector2 position = this.GetMenuPos(new Vector2(0f, -104f));
			position = VerchickMath.CenterText(font, position, s);
			Shadow.DrawOutlinedString(spriteBatch, font, s, Color.Black, Color.White, 1f, 0f, position);
		}

		// Token: 0x04000BBC RID: 3004
		private Texture2D Tex;

		// Token: 0x04000BBD RID: 3005
		private static Texture2D SelTex;

		// Token: 0x04000BBE RID: 3006
		private static Texture2D EmptyTex;

		// Token: 0x04000BBF RID: 3007
		private Vector2 StartPos;

		// Token: 0x04000BC0 RID: 3008
		private Vector2 CurrentPos;

		// Token: 0x04000BC1 RID: 3009
		private Vector2 MenuPos;

		// Token: 0x04000BC2 RID: 3010
		private Vector2 MIDPos;

		// Token: 0x04000BC3 RID: 3011
		private StateMachineEngine<MenuTile.TileState> SM;

		// Token: 0x04000BC4 RID: 3012
		private float Rotation;

		// Token: 0x04000BC5 RID: 3013
		private float RotationRate;

		// Token: 0x04000BC6 RID: 3014
		private float RotationPeek;

		// Token: 0x04000BC7 RID: 3015
		private int RotateMod = 1;

		// Token: 0x04000BC8 RID: 3016
		public string ToolTip = "";

		// Token: 0x02000229 RID: 553
		private enum TileState
		{
			// Token: 0x04000E6E RID: 3694
			Idle,
			// Token: 0x04000E6F RID: 3695
			Fired,
			// Token: 0x04000E70 RID: 3696
			Moving,
			// Token: 0x04000E71 RID: 3697
			Enlarging,
			// Token: 0x04000E72 RID: 3698
			Active,
			// Token: 0x04000E73 RID: 3699
			Retracting
		}
	}
}
