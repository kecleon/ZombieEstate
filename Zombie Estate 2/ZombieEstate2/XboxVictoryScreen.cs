using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Networking;
using ZombieEstate2.UI;
using ZombieEstate2.XboxSaving;

namespace ZombieEstate2
{
	// Token: 0x0200009F RID: 159
	public class XboxVictoryScreen
	{
		// Token: 0x0600041A RID: 1050 RVA: 0x0001E024 File Offset: 0x0001C224
		public XboxVictoryScreen(bool failed = false)
		{
			if (XboxVictoryScreen.mCont == null)
			{
				XboxVictoryScreen.mCont = Global.Content.Load<Texture2D>("XboxStore\\Continue");
				XboxVictoryScreen.mBG = Global.Content.Load<Texture2D>("Menus\\Victory");
			}
			this.mFailed = failed;
			this.mPlayers = Global.PlayerList;
			this.Init();
			this.mQuit = new ZEButton(new Rectangle((int)Global.GetScreenCenter().X - 50, Global.ScreenRect.Height - 100, 100, 40), "Quit", ButtonPress.Affirmative, -1);
			this.mQuit.OnPressed += this.MQuit_OnPressed;
			if (Global.DIFFICULTY_LEVEL == 2)
			{
				this.mDiffBonus = "Difficulty Bonus: x1.25";
				return;
			}
			if (Global.DIFFICULTY_LEVEL == 3)
			{
				this.mDiffBonus = "Difficulty Bonus: x1.5";
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001E108 File Offset: 0x0001C308
		private void Init()
		{
			if (this.mFailed)
			{
				this.mBounce = new BouncyText(new List<string>
				{
					"DEFEAT"
				}, (int)Global.GetScreenCenter().X, 190, 2);
				MusicEngine.Play(ZE2Songs.AllDead);
				if (Global.UnlimitedMode && Global.WavesCompleted > SteamHelper.mHighestWave)
				{
					SteamHelper.mHighestWave = Global.WavesCompleted;
				}
			}
			else
			{
				this.mBounce = new BouncyText(new List<string>
				{
					"VICTORY"
				}, (int)Global.GetScreenCenter().X, 190, 2);
			}
			SteamHelper.PushStats();
			Global.MasterCache.ClearObjects();
			NetworkManager.ClearMessage();
			NetworkManager.CloseAllConnections();
			NetworkManager.ClearAllObjects();
			this.mItems = new List<VictoryItem>();
			int num = (int)Global.GetScreenCenter().X - 560;
			foreach (Player player in this.mPlayers)
			{
				this.mItems.Add(new VictoryItem(player.GamerName, player.StartTextureCoord, new Vector2((float)(num + 100), 220f), 2f, Global.StoreFontBig, true));
				SpriteFont storeFont = Global.StoreFont;
				int num2 = 90;
				int num3 = (int)Math.Pow((double)Global.WavesCompleted, 1.15);
				this.mItems.Add(new VictoryItem("Waves Complete: " + Global.WavesCompleted.ToString(), new Point(4, 37), new Vector2((float)num, 260f), 3f, storeFont, false));
				this.mItems.Add(new VictoryItem("x" + num3, new Point(0, 58), new Vector2((float)(num + num2), 285f), 4f, Global.StoreFontBig, false));
				this.mItems.Add(new VictoryItem("Damage Badges: " + player.GameTalleys.DmgBadges, new Point(66, 46), new Vector2((float)num, 320f), 5f, storeFont, false));
				this.mItems.Add(new VictoryItem("x" + player.GameTalleys.DmgBadges, new Point(0, 58), new Vector2((float)(num + num2), 345f), 6f, Global.StoreFontBig, false));
				this.mItems.Add(new VictoryItem("Tank Badges: " + player.GameTalleys.TankBadges, new Point(67, 47), new Vector2((float)num, 380f), 7f, storeFont, false));
				this.mItems.Add(new VictoryItem("x" + player.GameTalleys.TankBadges, new Point(0, 58), new Vector2((float)(num + num2), 405f), 8f, Global.StoreFontBig, false));
				this.mItems.Add(new VictoryItem("Heal Badges: " + player.GameTalleys.HealBadges, new Point(66, 47), new Vector2((float)num, 440f), 9f, storeFont, false));
				this.mItems.Add(new VictoryItem("x" + player.GameTalleys.HealBadges, new Point(0, 58), new Vector2((float)(num + num2), 465f), 10f, Global.StoreFontBig, false));
				this.mItems.Add(new VictoryItem("Minion Badges: " + player.GameTalleys.MinionDmgBadges, new Point(67, 46), new Vector2((float)num, 500f), 11f, storeFont, false));
				this.mItems.Add(new VictoryItem("x" + player.GameTalleys.MinionDmgBadges, new Point(0, 58), new Vector2((float)(num + num2), 525f), 12f, Global.StoreFontBig, false));
				int num4 = num3 + player.GameTalleys.DmgBadges + player.GameTalleys.HealBadges + player.GameTalleys.MinionDmgBadges + player.GameTalleys.TankBadges;
				if (Global.DIFFICULTY_LEVEL == 2)
				{
					num4 = (int)((float)num4 * 1.25f);
				}
				if (Global.DIFFICULTY_LEVEL == 3)
				{
					num4 = (int)((float)num4 * 1.5f);
				}
				this.mItems.Add(new VictoryItem("Total: " + num4, new Point(0, 58), new Vector2((float)num, 570f), 13f, Global.StoreFontBig, false));
				XboxGamerStats gamerStats = Global.GamerStats;
				if (gamerStats != null)
				{
					gamerStats.Points += num4;
				}
				if (player.IAmOwnedByLocalPlayer)
				{
					XboxSaverLoader.SaveData();
				}
				num += 280;
			}
			XboxSaverLoader.SaveData();
			Global.WavesCompleted = 0;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001E620 File Offset: 0x0001C820
		public void Update()
		{
			if (this.fading)
			{
				return;
			}
			this.mTotalTime += 0.016666668f;
			if (this.mDiffAlpha < 1f)
			{
				this.mDiffAlpha += Global.REAL_GAME_TIME * 0.5f;
			}
			else
			{
				this.mDiffAlpha = 1f;
			}
			this.mBounce.Update();
			foreach (VictoryItem victoryItem in this.mItems)
			{
				victoryItem.Update();
			}
			if (this.mTotalTime > 3.5f)
			{
				for (int i = 0; i < 4; i++)
				{
					if ((InputManager.ButtonPressed(ButtonPress.Affirmative, i, false) || InputManager.ButtonPressed(ButtonPress.Negative, i, false)) && !this.fading)
					{
						this.fading = true;
						Global.RESET_GAMEPLAY();
					}
				}
				if ((InputManager.ALL_ButtonPressed(ButtonPress.Affirmative) || InputManager.ALL_ButtonPressed(ButtonPress.Negative)) && !this.fading)
				{
					this.fading = true;
					Global.RESET_GAMEPLAY();
				}
				if (!this.fading)
				{
					this.mQuit.Update();
				}
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001E740 File Offset: 0x0001C940
		private void MQuit_OnPressed(object sender, EventArgs e)
		{
			if (!this.fading)
			{
				this.fading = true;
				Global.RESET_GAMEPLAY();
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001E758 File Offset: 0x0001C958
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(XboxVictoryScreen.mBG, Global.ScreenRect, Color.White);
			if (this.mDiffAlpha > 0f)
			{
				Vector2 vector = new Vector2(Global.GetScreenCenter().X, 200f);
				vector = VerchickMath.CenterText(Global.StoreFontBig, vector, this.mDiffBonus);
				Shadow.DrawString(this.mDiffBonus, Global.StoreFontBig, vector, 1, Color.White * this.mDiffAlpha, Color.Black * this.mDiffAlpha, spriteBatch);
			}
			foreach (VictoryItem victoryItem in this.mItems)
			{
				victoryItem.Draw(spriteBatch);
			}
			if (this.mTotalTime > 3.5f)
			{
				this.mQuit.Draw(spriteBatch);
			}
			this.mBounce.Draw(spriteBatch);
		}

		// Token: 0x040003F8 RID: 1016
		private List<Player> mPlayers;

		// Token: 0x040003F9 RID: 1017
		private List<VictoryItem> mItems;

		// Token: 0x040003FA RID: 1018
		private static Texture2D mCont;

		// Token: 0x040003FB RID: 1019
		private static Texture2D mBG;

		// Token: 0x040003FC RID: 1020
		private BouncyText mBounce;

		// Token: 0x040003FD RID: 1021
		private float mTotalTime;

		// Token: 0x040003FE RID: 1022
		private bool fading;

		// Token: 0x040003FF RID: 1023
		private bool mFailed;

		// Token: 0x04000400 RID: 1024
		private ZEButton mQuit;

		// Token: 0x04000401 RID: 1025
		private string mDiffBonus = "Difficulty Bonus: x1.0";

		// Token: 0x04000402 RID: 1026
		private float mDiffAlpha = -1f;
	}
}
