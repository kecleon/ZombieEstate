using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Networking;
using ZombieEstate2.UI.Chat;
using ZombieEstate2.XboxSaving;

namespace ZombieEstate2
{
	// Token: 0x02000116 RID: 278
	public static class Global
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0003BBDE File Offset: 0x00039DDE
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x0003BBE5 File Offset: 0x00039DE5
		public static bool Paused
		{
			get
			{
				return Global.mPaused;
			}
			set
			{
				Global.mPaused = value;
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0003BBF0 File Offset: 0x00039DF0
		public static void RESET_GAMEPLAY()
		{
			NetworkManager.CloseAllConnections();
			NetworkManager.ClearAllObjects();
			PlayerManager.ClearPlayers();
			Global.GameManager = new GameManager();
			Global.Game.SetGameManager(Global.GameManager);
			Global.GameManager.Init();
			Global.GameManager.GotoMainMenu();
			NetworkManager.REJECT_INCOMING_MSGS = false;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0003BC3F File Offset: 0x00039E3F
		public static float RandomFloat(float min, float max)
		{
			return MathHelper.Lerp(min, max, (float)Global.rand.NextDouble());
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0003BC53 File Offset: 0x00039E53
		public static float RandomFloat(Random rnd, float min, float max)
		{
			return MathHelper.Lerp(min, max, (float)rnd.NextDouble());
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0003BC63 File Offset: 0x00039E63
		public static bool Probability(int percent)
		{
			return Global.rand.Next(0, 100) < percent;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0003BC78 File Offset: 0x00039E78
		public static bool Probability(Random rnd, int percent)
		{
			return rnd.Next(0, 100) < percent;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0003BC8C File Offset: 0x00039E8C
		public static void KillAllZombies()
		{
			int count = Global.ZombieList.Count;
			List<Zombie> list = new List<Zombie>();
			for (int i = 0; i < count; i++)
			{
				list.Add(Global.ZombieList[i]);
			}
			for (int j = 0; j < count; j++)
			{
				list[j].BaseDestroyObject();
			}
			Global.ZombieList.Clear();
			Terminal.WriteMessage("Zombies Destroyed: " + count, MessageType.IMPORTANTEVENT);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0003BD00 File Offset: 0x00039F00
		public static void KillAllDrops()
		{
			foreach (Drop drop in Global.DropsList)
			{
				if (!(drop is SuperWeaponDrop))
				{
					drop.BaseDestroyObject();
				}
			}
			Global.DropsList.Clear();
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0003BD64 File Offset: 0x00039F64
		public static void KillAllAirDrops()
		{
			List<Drop> list = new List<Drop>();
			foreach (Drop drop in Global.DropsList)
			{
				if (drop is CarePackage)
				{
					drop.BaseDestroyObject();
					list.Add(drop);
				}
			}
			foreach (Drop drop2 in list)
			{
				CarePackage item = (CarePackage)drop2;
				Global.DropsList.Remove(item);
			}
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0003BE14 File Offset: 0x0003A014
		public static bool OutsideMap(Vector3 pos)
		{
			return pos.X < 0f || pos.X > 32f || (pos.Y < 0f || pos.Y > 32f);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0003BE4F File Offset: 0x0003A04F
		public static Rectangle GetTexRectange(int x, int y)
		{
			return new Rectangle(x * 16, y * 16, 16, 16);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0003BE64 File Offset: 0x0003A064
		public static void DrawTextWithIcon(string text, int iconX, int iconY, int posX, int posY, SpriteFont font, SpriteBatch spriteBatch, int size)
		{
			float y = font.MeasureString(text).Y;
			Rectangle texRectange = Global.GetTexRectange(iconX, iconY);
			int num;
			if ((float)size < y)
			{
				num = -(int)((y - (float)size) / 2f);
			}
			else
			{
				num = (int)(((float)size - y) / 2f);
			}
			Rectangle destinationRectangle = new Rectangle(posX, posY - num, size, size);
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(texRectange), Color.White);
			Shadow.DrawString(text, font, new Vector2((float)(posX + size + 8), (float)posY), 1, Color.White, spriteBatch);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0003BEF4 File Offset: 0x0003A0F4
		public static bool AllPlayersDead()
		{
			bool result = true;
			using (List<Player>.Enumerator enumerator = Global.PlayerList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.DEAD)
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0003BF4C File Offset: 0x0003A14C
		public static bool AllLocalPlayersDead()
		{
			bool result = true;
			foreach (Player player in Global.PlayerList)
			{
				if (!player.DEAD && player.IAmOwnedByLocalPlayer)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0003BFAC File Offset: 0x0003A1AC
		public static Rectangle GetAspectRatioRect(Rectangle destination, Texture2D img)
		{
			if (destination.Width < img.Width)
			{
				throw new Exception("Destination smaller than source");
			}
			Rectangle rectangle = new Rectangle(0, 0, img.Width, img.Height);
			rectangle.X = (destination.Width - rectangle.Width) / 2;
			rectangle.Y = (destination.Height - rectangle.Height) / 2;
			return rectangle;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0003C014 File Offset: 0x0003A214
		public static float GetMoneyGained(float amount)
		{
			return amount;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0003C017 File Offset: 0x0003A217
		public static Vector2 GetScreenCenter()
		{
			return new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2));
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0003C038 File Offset: 0x0003A238
		public static Rectangle GetSafeScreenArea()
		{
			float num = 0.02f;
			int num2 = (int)((float)Global.ScreenRect.Width * num);
			int num3 = (int)((float)Global.ScreenRect.Height * num);
			return new Rectangle(num2, num3, Global.ScreenRect.Width - 2 * num2, Global.ScreenRect.Height - 2 * num3);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0003C08C File Offset: 0x0003A28C
		public static string GetTimeString(int time)
		{
			int num = time / 60;
			int num2 = time % 60;
			string str = num.ToString();
			string str2;
			if (num2 >= 10)
			{
				str2 = num2.ToString();
			}
			else
			{
				str2 = "0" + num2.ToString();
			}
			return str + ":" + str2;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0003C0D8 File Offset: 0x0003A2D8
		public static Color GetHUDColor(int i)
		{
			switch (i)
			{
			case 0:
				return new Color(136, 178, 229);
			case 1:
				return new Color(255, 140, 140);
			case 2:
				return new Color(136, 229, 178);
			case 3:
				return new Color(229, 229, 136);
			default:
				return Color.White;
			}
		}

		// Token: 0x040007CC RID: 1996
		public static string VersionNumber = "2.1.3";

		// Token: 0x040007CD RID: 1997
		public static bool CHEAT = false;

		// Token: 0x040007CE RID: 1998
		public static GraphicsDevice GraphicsDevice;

		// Token: 0x040007CF RID: 1999
		public static BasicEffect basicEffect;

		// Token: 0x040007D0 RID: 2000
		public static Effect instancedEffect;

		// Token: 0x040007D1 RID: 2001
		public static Effect levelEffect;

		// Token: 0x040007D2 RID: 2002
		public static ContentManager Content;

		// Token: 0x040007D3 RID: 2003
		public static ChatUI GameChat = null;

		// Token: 0x040007D4 RID: 2004
		public static float AspectRatio = 0f;

		// Token: 0x040007D5 RID: 2005
		public static Vector3 CameraPosition = new Vector3(10.01f, 8f, 18f);

		// Token: 0x040007D6 RID: 2006
		public static Vector3 CameraLookAt = new Vector3(10f, 0f, 10f);

		// Token: 0x040007D7 RID: 2007
		public static Random rand = new Random();

		// Token: 0x040007D8 RID: 2008
		public static Matrix View;

		// Token: 0x040007D9 RID: 2009
		public static Matrix Projection;

		// Token: 0x040007DA RID: 2010
		public static SpriteFont Font;

		// Token: 0x040007DB RID: 2011
		public static SpriteFont BloodGutterXtraLarge;

		// Token: 0x040007DC RID: 2012
		public static SpriteFont BloodGutterLarge;

		// Token: 0x040007DD RID: 2013
		public static SpriteFont BloodGutterSmall;

		// Token: 0x040007DE RID: 2014
		public static SpriteFont StoreFontSmall;

		// Token: 0x040007DF RID: 2015
		public static SpriteFont StoreFont;

		// Token: 0x040007E0 RID: 2016
		public static SpriteFont StoreFontBig;

		// Token: 0x040007E1 RID: 2017
		public static SpriteFont StoreFontXtraLarge;

		// Token: 0x040007E2 RID: 2018
		public static SpriteFont EquationFont;

		// Token: 0x040007E3 RID: 2019
		public static SpriteFont EquationFontSmall;

		// Token: 0x040007E4 RID: 2020
		public static bool ROBMODE = false;

		// Token: 0x040007E5 RID: 2021
		public static XboxVictoryScreen VictoryScreen;

		// Token: 0x040007E6 RID: 2022
		public static int WavesCompleted = 0;

		// Token: 0x040007E7 RID: 2023
		public static bool UnlimitedMode = false;

		// Token: 0x040007E8 RID: 2024
		public static int DIFFICULTY_LEVEL = 0;

		// Token: 0x040007E9 RID: 2025
		public static GamerServicesComponent GamerServices;

		// Token: 0x040007EA RID: 2026
		public static ZE2Songs LevelSong = ZE2Songs.Estate;

		// Token: 0x040007EB RID: 2027
		public static Texture2D BloodEffect;

		// Token: 0x040007EC RID: 2028
		public static List<Zombie> ZombieList = new List<Zombie>();

		// Token: 0x040007ED RID: 2029
		public static List<Player> PlayerList = new List<Player>();

		// Token: 0x040007EE RID: 2030
		public static List<Drop> DropsList = new List<Drop>();

		// Token: 0x040007EF RID: 2031
		public static GameState GameState = GameState.Playing;

		// Token: 0x040007F0 RID: 2032
		public static GameManager GameManager;

		// Token: 0x040007F1 RID: 2033
		public static Texture2D MenuBG;

		// Token: 0x040007F2 RID: 2034
		public static XboxGamerStats GamerStats = new XboxGamerStats();

		// Token: 0x040007F3 RID: 2035
		public static GraphicsDeviceManager Graphics;

		// Token: 0x040007F4 RID: 2036
		public static Rectangle ScreenRect = new Rectangle(0, 0, 1650, 920);

		// Token: 0x040007F5 RID: 2037
		public static bool NETWORKED = false;

		// Token: 0x040007F6 RID: 2038
		public static float ZombieHealthMod = 1f;

		// Token: 0x040007F7 RID: 2039
		public static int UPPERGUNLIMIT = 9000;

		// Token: 0x040007F8 RID: 2040
		public static WaveMaster WaveMaster;

		// Token: 0x040007F9 RID: 2041
		public static Config Config;

		// Token: 0x040007FA RID: 2042
		public static Texture2D MasterEnvTex;

		// Token: 0x040007FB RID: 2043
		public static Texture2D MasterLightTex;

		// Token: 0x040007FC RID: 2044
		public static Texture2D MasterLightTexTransparent;

		// Token: 0x040007FD RID: 2045
		public static Texture2D Alphabet;

		// Token: 0x040007FE RID: 2046
		public static Texture2D Alphabet2;

		// Token: 0x040007FF RID: 2047
		public static Texture2D SplashScreen;

		// Token: 0x04000800 RID: 2048
		public static Texture2D Credits;

		// Token: 0x04000801 RID: 2049
		public static bool AOEnabled = true;

		// Token: 0x04000802 RID: 2050
		public static Texture2D CurrentLevelLightTex;

		// Token: 0x04000803 RID: 2051
		public static Texture2D MasterTexture;

		// Token: 0x04000804 RID: 2052
		public static Texture2D SpeechBubble;

		// Token: 0x04000805 RID: 2053
		public static Texture2D WaveHUD;

		// Token: 0x04000806 RID: 2054
		public static Editor Editor;

		// Token: 0x04000807 RID: 2055
		public static int EndListsDone = 0;

		// Token: 0x04000808 RID: 2056
		public static float Gravity = 28f;

		// Token: 0x04000809 RID: 2057
		public static Player Player;

		// Token: 0x0400080A RID: 2058
		public static Game1 Game;

		// Token: 0x0400080B RID: 2059
		public static MasterCache MasterCache;

		// Token: 0x0400080C RID: 2060
		public static Sector Level;

		// Token: 0x0400080D RID: 2061
		public static int WaveCount = 15;

		// Token: 0x0400080E RID: 2062
		public static float DifficultyModeMod = 1f;

		// Token: 0x0400080F RID: 2063
		public static bool GameEnded = false;

		// Token: 0x04000810 RID: 2064
		public static bool ReadyToLoadTransforms = false;

		// Token: 0x04000811 RID: 2065
		public static bool ReadyToLoadTransformsTwo = false;

		// Token: 0x04000812 RID: 2066
		public static Rectangle RedX = Global.GetTexRectange(62, 63);

		// Token: 0x04000813 RID: 2067
		public static Texture2D Pixel;

		// Token: 0x04000814 RID: 2068
		private static bool mPaused = false;

		// Token: 0x04000815 RID: 2069
		public static Model instancedModel;

		// Token: 0x04000816 RID: 2070
		public static bool Slowed = false;

		// Token: 0x04000817 RID: 2071
		public static float SpeedMod = 1f;

		// Token: 0x04000818 RID: 2072
		public static float Pulse = 0f;

		// Token: 0x04000819 RID: 2073
		public static float REAL_GAME_TIME = 0f;

		// Token: 0x0400081A RID: 2074
		public static StoreManager StoreManager;

		// Token: 0x0400081B RID: 2075
		public static int PartCount = 0;

		// Token: 0x0400081C RID: 2076
		public static bool MainOnTop = false;

		// Token: 0x0400081D RID: 2077
		public static bool BossActive;

		// Token: 0x0400081E RID: 2078
		public static Boss Boss;

		// Token: 0x0400081F RID: 2079
		public static int WAVE_GEN_SEED = Global.rand.Next();

		// Token: 0x04000820 RID: 2080
		public static float DarkMod = 1f;
	}
}
