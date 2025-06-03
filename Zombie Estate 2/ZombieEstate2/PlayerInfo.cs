using System;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;

namespace ZombieEstate2
{
	// Token: 0x02000089 RID: 137
	public class PlayerInfo
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00019582 File Offset: 0x00017782
		public bool UsingController
		{
			get
			{
				return this.ControllerIndex != -1;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00019590 File Offset: 0x00017790
		public string PersonaName
		{
			get
			{
				if (this.mPersonaName == null)
				{
					this.mPersonaName = SteamFriends.GetFriendPersonaName(this.SteamID);
					int num = PlayerManager.NumberOfTimesNameInUse(this.mPersonaName);
					if (num > 1)
					{
						num--;
						this.mPersonaName = this.mPersonaName + " [" + num.ToString() + "]";
					}
				}
				return this.mPersonaName;
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x000195F4 File Offset: 0x000177F4
		public string GetData()
		{
			return string.Concat(new object[]
			{
				this.CharacterName,
				",",
				this.AccessoryName,
				",",
				this.Guest.ToString(),
				",",
				this.ControllerIndex,
				",",
				this.SteamID.ToString()
			});
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00019678 File Offset: 0x00017878
		public PlayerInfo Clone()
		{
			PlayerInfo playerInfo = new PlayerInfo();
			playerInfo.Index = this.Index;
			playerInfo.PlayerObject = this.PlayerObject;
			playerInfo.Local = this.Local;
			playerInfo.Guest = this.Guest;
			playerInfo.SteamID = this.SteamID;
			playerInfo.CharacterName = this.CharacterName;
			playerInfo.AccessoryName = this.AccessoryName;
			playerInfo.mPersonaName = this.mPersonaName;
			playerInfo.ControllerIndex = this.ControllerIndex;
			playerInfo.GetImage();
			return playerInfo;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000196FC File Offset: 0x000178FC
		public void GetImage()
		{
			try
			{
				Terminal.WriteMessage("Attempting to get avatar for player: " + this.PersonaName);
				this.SteamAvatar = null;
				this.SteamAvatarID = -1;
				this.SteamAvatarID = SteamFriends.GetMediumFriendAvatar(this.SteamID);
				if (this.SteamAvatarID == -1 || this.SteamAvatarID == 0)
				{
					Terminal.WriteMessage("No steam avatar found.");
				}
				else
				{
					uint num;
					uint num2;
					SteamUtils.GetImageSize(this.SteamAvatarID, out num, out num2);
					if (num > 0U && num2 > 0U)
					{
						byte[] array = new byte[num * num2 * 4U];
						if (!SteamUtils.GetImageRGBA(this.SteamAvatarID, array, array.Length))
						{
							Terminal.WriteMessage("Failed to retrieved avatar. API recall returned false.");
							this.SteamAvatarID = -1;
							this.SteamAvatar = null;
						}
						else
						{
							Texture2D texture2D = new Texture2D(Global.GraphicsDevice, (int)num, (int)num2);
							texture2D.SetData<byte>(array);
							this.SteamAvatar = texture2D;
							Terminal.WriteMessage("Successfully retrieved avatar.");
						}
					}
					else
					{
						this.SteamAvatarID = -1;
						Terminal.WriteMessage("Failed to retrieved avatar. Width/Height was 0.");
						this.SteamAvatar = null;
					}
				}
			}
			catch (Exception arg)
			{
				this.SteamAvatarID = -1;
				Terminal.WriteMessage("Failed to retrieved avatar. Exception: " + arg);
				this.SteamAvatar = null;
			}
		}

		// Token: 0x04000323 RID: 803
		public static string NO_SELECTION = "NONE";

		// Token: 0x04000324 RID: 804
		public static string NO_PLAYER = "NO_PLAYER";

		// Token: 0x04000325 RID: 805
		private const int AvatarWidth = 64;

		// Token: 0x04000326 RID: 806
		private const int AvatarHeight = 64;

		// Token: 0x04000327 RID: 807
		public int Index;

		// Token: 0x04000328 RID: 808
		public Player PlayerObject;

		// Token: 0x04000329 RID: 809
		public bool Local;

		// Token: 0x0400032A RID: 810
		public bool Guest;

		// Token: 0x0400032B RID: 811
		public CSteamID SteamID;

		// Token: 0x0400032C RID: 812
		public string CharacterName = PlayerInfo.NO_SELECTION;

		// Token: 0x0400032D RID: 813
		public string AccessoryName = PlayerInfo.NO_SELECTION;

		// Token: 0x0400032E RID: 814
		public int ControllerIndex = -1;

		// Token: 0x0400032F RID: 815
		public float TimeSinceLastPing = -1f;

		// Token: 0x04000330 RID: 816
		public float LastPingTime = -1f;

		// Token: 0x04000331 RID: 817
		public bool WaitingOnPong;

		// Token: 0x04000332 RID: 818
		public bool HaveGottenOnePing;

		// Token: 0x04000333 RID: 819
		public int SteamAvatarID = -1;

		// Token: 0x04000334 RID: 820
		public Texture2D SteamAvatar;

		// Token: 0x04000335 RID: 821
		private string mPersonaName;
	}
}
