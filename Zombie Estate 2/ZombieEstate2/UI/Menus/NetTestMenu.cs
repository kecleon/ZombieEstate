using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;

namespace ZombieEstate2.UI.Menus
{
	// Token: 0x02000164 RID: 356
	internal class NetTestMenu : Menu
	{
		// Token: 0x06000ADF RID: 2783 RVA: 0x0005A12C File Offset: 0x0005832C
		public NetTestMenu() : base(true, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2)))
		{
			this.mOnLobbyMatchList = CallResult<LobbyMatchList_t>.Create(new CallResult<LobbyMatchList_t>.APIDispatchDelegate(this.OnLobbyMatchList));
			this.mOnLobbyEntered = CallResult<LobbyEnter_t>.Create(new CallResult<LobbyEnter_t>.APIDispatchDelegate(this.OnLobbyEntered));
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0005A190 File Offset: 0x00058390
		public override void Setup()
		{
			this.music = base.AddToMenu("Connect to Jess", new MenuItem.SelectedDelegate(this.Jess), false);
			this.checkLobby = base.AddToMenu("Look for lobbies", new MenuItem.SelectedDelegate(this.CheckLobby), false);
			this.title = "Networking Test";
			this.MenuBG = Global.MenuBG;
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void Jess()
		{
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0005A1F0 File Offset: 0x000583F0
		private void CheckLobby()
		{
			SteamAPICall_t hAPICall = SteamMatchmaking.RequestLobbyList();
			this.mOnLobbyMatchList.Set(hAPICall, null);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002E26C File Offset: 0x0002C46C
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0005A210 File Offset: 0x00058410
		private void JoinLobby(CSteamID id)
		{
			SteamAPICall_t hAPICall = SteamMatchmaking.JoinLobby(id);
			this.mOnLobbyEntered.Set(hAPICall, null);
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0005A234 File Offset: 0x00058434
		private void OnLobbyMatchList(LobbyMatchList_t list, bool bIOFailure)
		{
			Terminal.WriteMessage("OnLobbyMatchList");
			Terminal.WriteMessage("     List Count: " + list.m_nLobbiesMatching);
			int num = 0;
			CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(num);
			Terminal.WriteMessage(string.Concat(new object[]
			{
				"     ID of index ",
				num,
				": ",
				lobbyByIndex
			}));
			this.JoinLobby(lobbyByIndex);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0005A2A7 File Offset: 0x000584A7
		private void OnLobbyEntered(LobbyEnter_t lobby, bool bIOFailure)
		{
			Terminal.WriteMessage("OnLobbyEntered");
			Terminal.WriteMessage("     Lobby ID Joined: " + lobby.m_ulSteamIDLobby);
		}

		// Token: 0x04000BA9 RID: 2985
		private MenuItem music;

		// Token: 0x04000BAA RID: 2986
		private MenuItem ping;

		// Token: 0x04000BAB RID: 2987
		private MenuItem checkLobby;

		// Token: 0x04000BAC RID: 2988
		private CallResult<LobbyMatchList_t> mOnLobbyMatchList;

		// Token: 0x04000BAD RID: 2989
		private CallResult<LobbyEnter_t> mOnLobbyEntered;
	}
}
