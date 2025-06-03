using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x020000EB RID: 235
	internal class QuitAreYouSureMenu : Menu
	{
		// Token: 0x06000644 RID: 1604 RVA: 0x0002E831 File Offset: 0x0002CA31
		public QuitAreYouSureMenu(bool blackout) : base(false, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2)))
		{
			this.DrawBGPixel = blackout;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0002E860 File Offset: 0x0002CA60
		public override void Setup()
		{
			base.AddToMenu("No", new MenuItem.SelectedDelegate(this.No), false);
			base.AddToMenu("Yes", new MenuItem.SelectedDelegate(this.Yes), false);
			this.title = "Are you sure you want to quit?";
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0002E8A0 File Offset: 0x0002CAA0
		public void Yes()
		{
			MenuManager.CLOSEALL();
			SoundEngine.TEMP_NO_SOUND_TIMER = 2f;
			PlayerInfo player = PlayerManager.GetPlayer(SteamHelper.GetLocalID());
			if (player != null)
			{
				NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.PlayerLeft);
				netMessage.Payload = new Msg_PlayerLeft
				{
					Index = player.Index
				};
				NetworkManager.SendMessage(netMessage, SendType.Reliable);
			}
			Global.WaveMaster.EndGameDefeat();
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001D9C2 File Offset: 0x0001BBC2
		public void No()
		{
			MenuManager.MenuClosed();
		}
	}
}
