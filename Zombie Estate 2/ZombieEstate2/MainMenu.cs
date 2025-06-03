using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using ZombieEstate2.Networking;
using ZombieEstate2.UI.Menus;

namespace ZombieEstate2
{
	// Token: 0x020000E3 RID: 227
	internal class MainMenu : MenuWithDescription
	{
		// Token: 0x060005F2 RID: 1522 RVA: 0x0002CA75 File Offset: 0x0002AC75
		public MainMenu() : base(false, 18, false)
		{
			this.MenuBG = Global.Content.Load<Texture2D>("Menus\\MainMenuBG_NoTitle");
			this.BGScale = 2;
			MouseHandler.MouseVisible = true;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0002CAA4 File Offset: 0x0002ACA4
		public override void Setup()
		{
			base.AddToMenu("Play!", new MenuItem.SelectedDelegate(this.StartHostGame), "Start a private lobby, then invite or search for other gamers online to join, or play alone!");
			base.AddToMenu("Lobby Browser", new MenuItem.SelectedDelegate(this.StartLobbyBrowser), "Find an existing lobby to join!");
			base.AddToMenu("Options", new MenuItem.SelectedDelegate(this.Options), "Set various options for the game.");
			base.AddToMenu("Credits", new MenuItem.SelectedDelegate(this.Credits), "View the credits screen.");
			base.AddToMenu("Exit Game", new MenuItem.SelectedDelegate(this.End), "Exit the game.");
			this.title = "Zombie Estate II";
			PingManager.DISABLE_PINGS = false;
			MusicEngine.Play(ZE2Songs.Intro);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0002CB54 File Offset: 0x0002AD54
		public override void UpdateMenu()
		{
			this.loaded = true;
			base.UpdateMenu();
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0002CB64 File Offset: 0x0002AD64
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
			Vector2 pos = new Vector2(4f, (float)(Global.ScreenRect.Height - 24));
			Shadow.DrawString("Version: " + Global.VersionNumber, Global.StoreFontSmall, pos, 1, Color.White, spriteBatch);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0002CBB3 File Offset: 0x0002ADB3
		private void NetTest()
		{
			ScreenFader.Fade(delegate()
			{
				MenuManager.PushMenu(new NetTestMenu());
			});
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0002CBD9 File Offset: 0x0002ADD9
		private void StartFindGame()
		{
			MainMenu.HOSTING_GAME = false;
			ScreenFader.Fade(delegate()
			{
				MenuManager.PushMenu(new XboxLevelSelect());
			});
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0002CC05 File Offset: 0x0002AE05
		private void StartHostGame()
		{
			MainMenu.HOSTING_GAME = true;
			ScreenFader.Fade(delegate()
			{
				MenuManager.PushMenu(new XboxLevelSelect());
			});
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0002CC31 File Offset: 0x0002AE31
		private void StartLobbyBrowser()
		{
			MainMenu.HOSTING_GAME = false;
			ScreenFader.Fade(delegate()
			{
				MenuManager.PushMenu(new BrowseLobbiesMenu());
			});
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0002CC5D File Offset: 0x0002AE5D
		private void Options()
		{
			ScreenFader.Fade(delegate()
			{
				MenuManager.PushMenu(new OptionsMenu());
			});
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0002CC83 File Offset: 0x0002AE83
		private void HelpMenu()
		{
			ScreenFader.Fade(delegate()
			{
				MenuManager.PushMenu(new HelpMenu());
			});
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0002CCA9 File Offset: 0x0002AEA9
		private void Credits()
		{
			ScreenFader.Fade(delegate()
			{
				MenuManager.PushMenu(new Credits());
			});
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void JoinGame()
		{
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0002CCCF File Offset: 0x0002AECF
		private void End()
		{
			Global.GameEnded = true;
			Global.Game.Exit();
		}

		// Token: 0x040005D4 RID: 1492
		private bool loaded;

		// Token: 0x040005D5 RID: 1493
		public static bool HOSTING_GAME = false;

		// Token: 0x040005D6 RID: 1494
		public static CSteamID LOBBY_TO_JOIN = CSteamID.Nil;
	}
}
