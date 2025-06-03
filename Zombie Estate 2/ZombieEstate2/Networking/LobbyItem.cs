using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;

namespace ZombieEstate2.Networking
{
	// Token: 0x020001AB RID: 427
	internal class LobbyItem
	{
		// Token: 0x06000BD6 RID: 3030 RVA: 0x00060EF8 File Offset: 0x0005F0F8
		public LobbyItem(CSteamID id, string map, string mode, int slotsAvail, int listIndex)
		{
			this.mLobbyID = id;
			this.mLevel = map;
			this.mMode = mode;
			this.mSlotsAvailable = slotsAvail;
			this.mListIndex = listIndex;
			this.mTitle = string.Format("Lobby {0}:    {1,15}{2, -15}{3, -20}", new object[]
			{
				this.mListIndex + 1,
				this.mLevel,
				this.mSlotsAvailable,
				this.mMode
			});
			if (this.mLevel == "Estate")
			{
				this.mIcon = XboxLevelSelect.ICON_Estate;
				return;
			}
			if (this.mLevel == "School")
			{
				this.mIcon = XboxLevelSelect.ICON_School;
				return;
			}
			if (this.mLevel == "Mall")
			{
				this.mIcon = XboxLevelSelect.ICON_Mall;
				return;
			}
			if (this.mLevel == "Skyscraper")
			{
				this.mIcon = XboxLevelSelect.ICON_Skyscraper;
				return;
			}
			if (this.mLevel == "DesertTown")
			{
				this.mIcon = XboxLevelSelect.ICON_DesertTown;
				return;
			}
			if (this.mLevel == "Office")
			{
				this.mIcon = XboxLevelSelect.ICON_Office;
				return;
			}
			if (this.mLevel == "Farm")
			{
				this.mIcon = XboxLevelSelect.ICON_Farm;
				return;
			}
			this.mIcon = null;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00061050 File Offset: 0x0005F250
		public void Draw(SpriteBatch spritebatch, int index, bool selected)
		{
			Rectangle rect = this.GetRect(index);
			Rectangle destinationRectangle = new Rectangle(rect.X + 2, rect.Y + 2, 60, 60);
			Color color;
			if (this.mListIndex % 2 == 0)
			{
				color = new Color(0.2f, 0.2f, 0.2f);
			}
			else
			{
				color = new Color(0.15f, 0.15f, 0.15f);
			}
			if (selected)
			{
				color = Color.Lerp(color, Color.Gray, Global.Pulse);
			}
			spritebatch.Draw(Global.Pixel, rect, color);
			Vector2 pos = new Vector2((float)(rect.X + 80), (float)(rect.Y + 12));
			Shadow.DrawString("Lobby " + (this.mListIndex + 1).ToString() + ":", Global.StoreFontBig, pos, 1, Color.White, spritebatch);
			pos.X += 140f;
			Shadow.DrawString(this.mLevel, Global.StoreFontBig, pos, 1, Color.White, spritebatch);
			pos.X += 200f;
			Shadow.DrawString(this.mSlotsAvailable.ToString(), Global.StoreFontBig, pos, 1, Color.White, spritebatch);
			pos.X += 100f;
			Shadow.DrawString(this.mMode, Global.StoreFontBig, pos, 1, Color.White, spritebatch);
			if (this.mIcon != null)
			{
				spritebatch.Draw(this.mIcon, destinationRectangle, Color.White);
			}
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x000611C0 File Offset: 0x0005F3C0
		public Rectangle GetRect(int index)
		{
			int x = (int)(Global.GetScreenCenter().X - (float)LobbyItem.WIDTH / 2f);
			int y = (int)((float)(index * LobbyItem.HEIGHT) + ((float)Global.ScreenRect.Height * 0.1f + 180f) + (float)(LobbyItem.PADDING * index));
			return new Rectangle(x, y, LobbyItem.WIDTH, LobbyItem.HEIGHT);
		}

		// Token: 0x04000C30 RID: 3120
		public static int WIDTH = 840;

		// Token: 0x04000C31 RID: 3121
		public static int HEIGHT = 64;

		// Token: 0x04000C32 RID: 3122
		public static int PADDING = 4;

		// Token: 0x04000C33 RID: 3123
		private string mLevel;

		// Token: 0x04000C34 RID: 3124
		public CSteamID mLobbyID;

		// Token: 0x04000C35 RID: 3125
		private int mSlotsAvailable;

		// Token: 0x04000C36 RID: 3126
		private string mMode;

		// Token: 0x04000C37 RID: 3127
		public int mListIndex;

		// Token: 0x04000C38 RID: 3128
		private string mTitle;

		// Token: 0x04000C39 RID: 3129
		private Texture2D mIcon;
	}
}
