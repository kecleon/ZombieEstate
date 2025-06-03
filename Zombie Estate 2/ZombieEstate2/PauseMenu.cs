using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.UI.Menus;

namespace ZombieEstate2
{
	// Token: 0x020000EA RID: 234
	internal class PauseMenu : Menu
	{
		// Token: 0x0600063C RID: 1596 RVA: 0x0002E6AC File Offset: 0x0002C8AC
		public PauseMenu() : base(false, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2)))
		{
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0002E6D4 File Offset: 0x0002C8D4
		public override void Setup()
		{
			base.AddToMenu("Resume", new MenuItem.SelectedDelegate(this.Resume), false);
			base.AddToMenu("Options", new MenuItem.SelectedDelegate(this.Options), false);
			base.AddToMenu("Main Menu", new MenuItem.SelectedDelegate(this.QuitGameTemp), false);
			if (PlayerManager.AllLocalPlayers())
			{
				this.title = "PAUSED";
			}
			else
			{
				this.title = "MENU";
			}
			foreach (Player player in Global.PlayerList)
			{
				if (player.PlayerInfo.Local)
				{
					player.PlayerInputLocked = true;
				}
			}
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0002E26C File Offset: 0x0002C46C
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0002E7A0 File Offset: 0x0002C9A0
		private void Resume()
		{
			MenuManager.MenuClosed();
			foreach (Player player in Global.PlayerList)
			{
				if (player.PlayerInfo.Local)
				{
					player.PlayerInputLocked = false;
				}
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0002E804 File Offset: 0x0002CA04
		private void Options()
		{
			MenuManager.PushMenu(new OptionsMenu());
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0002E810 File Offset: 0x0002CA10
		private void QuitGameTemp()
		{
			MenuManager.PushMenu(new QuitAreYouSureMenu(false));
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0002E81D File Offset: 0x0002CA1D
		private void Help()
		{
			MenuManager.PushMenu(new HelpMenu());
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0002E829 File Offset: 0x0002CA29
		public override void UpdateMenu()
		{
			base.UpdateMenu();
		}
	}
}
