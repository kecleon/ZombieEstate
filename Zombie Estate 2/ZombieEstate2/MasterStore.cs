using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.StoreScreen.XboxStore;

namespace ZombieEstate2
{
	// Token: 0x020000B0 RID: 176
	public static class MasterStore
	{
		// Token: 0x0600047F RID: 1151 RVA: 0x0002144C File Offset: 0x0001F64C
		public static void Init()
		{
			MasterStore.mStores = new XboxStore[4];
			Point topLeft = new Point(0, 0);
			float num = 6f;
			topLeft.X = (int)Global.GetScreenCenter().X - (int)(num / 2f * 16f);
			topLeft.Y = (int)Global.GetScreenCenter().Y - (int)(num / 2f * 16f);
			MasterStore.Bob = new BobbingPortrait(new Point(10, 6), new Point(9, 6), num, topLeft, 0.6f);
			MasterStore.BuildStores();
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x000214DC File Offset: 0x0001F6DC
		private static void BuildStores()
		{
			foreach (Player player in Global.PlayerList)
			{
				if (player.IAmOwnedByLocalPlayer)
				{
					MasterStore.mStores[player.Index] = new XboxStore(player);
				}
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00021544 File Offset: 0x0001F744
		public static void Update()
		{
			if (!MasterStore.Active)
			{
				return;
			}
			MasterStore.Bob.Update();
			bool flag = true;
			for (int i = 0; i < 4; i++)
			{
				if (MasterStore.mStores[i] != null)
				{
					MasterStore.mStores[i].Update();
					if (MasterStore.mStores[i].Player.IAmOwnedByLocalPlayer && MasterStore.mStores[i].State != XboxStore.StoreState.Closed)
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				MasterStore.Deactivate();
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000215B4 File Offset: 0x0001F7B4
		public static void Draw(SpriteBatch spriteBatch)
		{
			if (!MasterStore.Active)
			{
				return;
			}
			spriteBatch.Draw(Global.Pixel, Global.ScreenRect, Color.Black * 0.5f);
			for (int i = 0; i < 4; i++)
			{
				if (MasterStore.mStores[i] != null)
				{
					MasterStore.mStores[i].Draw(spriteBatch);
				}
			}
			MasterStore.Bob.Draw(spriteBatch);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00021618 File Offset: 0x0001F818
		public static void Activate()
		{
			MasterStore.Active = true;
			MasterStore.BuildStores();
			MasterStore.StoreTerm("Activated");
			foreach (Player player in Global.PlayerList)
			{
				if (player.IAmOwnedByLocalPlayer)
				{
					player.mMovement = Vector2.Zero;
				}
			}
			MusicEngine.Play(ZE2Songs.Store);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00021694 File Offset: 0x0001F894
		public static void Deactivate()
		{
			if (!MasterStore.Active)
			{
				return;
			}
			MasterStore.Active = false;
			for (int i = 0; i < 4; i++)
			{
				if (MasterStore.mStores[i] != null)
				{
					MasterStore.mStores[i].Close();
				}
			}
			MasterStore.StoreTerm("Deactivated");
			Global.GameManager.StoreClosing = true;
			MusicEngine.Play(ZE2Songs.Wave);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000216EC File Offset: 0x0001F8EC
		private static void StoreTerm(string message)
		{
			Terminal.WriteMessage("Store: " + message, MessageType.DEBUG);
		}

		// Token: 0x0400046E RID: 1134
		private static XboxStore[] mStores;

		// Token: 0x0400046F RID: 1135
		private static BobbingPortrait Bob;

		// Token: 0x04000470 RID: 1136
		public static bool Active;
	}
}
