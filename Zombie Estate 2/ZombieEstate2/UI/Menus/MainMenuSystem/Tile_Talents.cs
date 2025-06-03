using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Talents;

namespace ZombieEstate2.UI.Menus.MainMenuSystem
{
	// Token: 0x0200016F RID: 367
	internal class Tile_Talents : MenuTile
	{
		// Token: 0x06000B20 RID: 2848 RVA: 0x0005BE6C File Offset: 0x0005A06C
		public Tile_Talents(Vector2 pos, Vector2 menuPos) : base("Talents", pos, menuPos)
		{
			this.A = new PCButton("A", Color.Blue, base.GetMenuPos(new Vector2(-160f, 0f)), ButtonType.Talent);
			this.B = new PCButton("B", Color.Red, base.GetMenuPos(new Vector2(-32f, 0f)), ButtonType.Talent);
			this.C = new PCButton("C", Color.Green, base.GetMenuPos(new Vector2(96f, 0f)), ButtonType.Talent);
			this.A.Pressed = new PCButton.PressedDelegate(this.AClicked);
			this.B.Pressed = new PCButton.PressedDelegate(this.BClicked);
			this.C.Pressed = new PCButton.PressedDelegate(this.CClicked);
			this.talentPos = new Vector2(menuPos.X + 490f, menuPos.Y + 64f);
			this.ToolTip = "View or edit your 3 talent trees.";
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0005BF7C File Offset: 0x0005A17C
		public override void UpdateMenu()
		{
			if (!this.BigActive)
			{
				this.A.Update();
				this.B.Update();
				this.C.Update();
			}
			if (this.Page != null && this.BigActive)
			{
				this.Page.Update();
				if (this.Page.EXITED)
				{
					this.BigActive = false;
					this.Page = null;
					MenuTileManager.SubMenuActive = false;
				}
			}
			this.UpdateMiniTrees();
			base.UpdateMenu();
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0005BFFC File Offset: 0x0005A1FC
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			this.A.Draw(spriteBatch);
			this.B.Draw(spriteBatch);
			this.C.Draw(spriteBatch);
			base.DrawCenteredTitle("Talents", spriteBatch, Global.StoreFontXtraLarge);
			if (this.Page != null)
			{
				this.Page.Draw(spriteBatch);
			}
			base.DrawMenu(spriteBatch);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0005C05C File Offset: 0x0005A25C
		private void UpdateMiniTrees()
		{
			if (this.A.Highlighted)
			{
				if (this.Page == null)
				{
					this.Page = new TalentPage(1, this.talentPos, 0);
					return;
				}
			}
			else if (this.B.Highlighted)
			{
				if (this.Page == null)
				{
					this.Page = new TalentPage(1, this.talentPos, 1);
					return;
				}
			}
			else if (this.C.Highlighted)
			{
				if (this.Page == null)
				{
					this.Page = new TalentPage(1, this.talentPos, 2);
					return;
				}
			}
			else
			{
				this.Page = null;
			}
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0005C0EB File Offset: 0x0005A2EB
		private void AClicked()
		{
			this.BigActive = true;
			this.Page = new TalentPage(4, MenuTileManager.TopLeft, 0);
			MenuTileManager.SubMenuActive = true;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0005C10C File Offset: 0x0005A30C
		private void BClicked()
		{
			this.BigActive = true;
			this.Page = new TalentPage(4, MenuTileManager.TopLeft, 1);
			MenuTileManager.SubMenuActive = true;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0005C12D File Offset: 0x0005A32D
		private void CClicked()
		{
			this.BigActive = true;
			this.Page = new TalentPage(4, MenuTileManager.TopLeft, 2);
			MenuTileManager.SubMenuActive = true;
		}

		// Token: 0x04000BE6 RID: 3046
		private PCButton A;

		// Token: 0x04000BE7 RID: 3047
		private PCButton B;

		// Token: 0x04000BE8 RID: 3048
		private PCButton C;

		// Token: 0x04000BE9 RID: 3049
		private TalentPage Page;

		// Token: 0x04000BEA RID: 3050
		private Vector2 talentPos;

		// Token: 0x04000BEB RID: 3051
		private bool BigActive;
	}
}
