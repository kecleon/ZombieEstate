using System;
using System.Text;
using Steamworks;

namespace ZombieEstate2
{
	// Token: 0x02000077 RID: 119
	public static class SteamHelper
	{
		// Token: 0x060002DA RID: 730 RVA: 0x00016E48 File Offset: 0x00015048
		public static void Init()
		{
			SteamHelper.mStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(SteamHelper.OnStatsReceived));
			SteamHelper.mStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(SteamHelper.OnStatsStored));
			SteamHelper.mGameOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(SteamHelper.OnGameOverlayActivated));
			SteamUserStats.RequestUserStats(SteamHelper.GetLocalID());
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00016EA2 File Offset: 0x000150A2
		public static CSteamID GetLocalID()
		{
			return SteamUser.GetSteamID();
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00016EA9 File Offset: 0x000150A9
		public static void Update()
		{
			if (SteamHelper.SteamOverlayVisible && SteamHelper.mCooldown > 0f)
			{
				SteamHelper.mCooldown -= Global.REAL_GAME_TIME;
				if (SteamHelper.mCooldown <= 0f)
				{
					SteamHelper.SteamOverlayVisible = false;
				}
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00016EE0 File Offset: 0x000150E0
		private static void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
		{
			bool flag = pCallback.m_bActive == 1;
			if (flag)
			{
				SteamHelper.SteamOverlayVisible = true;
				SteamHelper.mCooldown = 0f;
			}
			else
			{
				SteamHelper.mCooldown = 0.5f;
			}
			Terminal.WriteMessage("Steam Overlay: " + (flag ? "Visible" : "Hidden"), MessageType.IMPORTANTEVENT);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00016F38 File Offset: 0x00015138
		private static void OnStatsReceived(UserStatsReceived_t pCallback)
		{
			SteamHelper.STATS_INIT = true;
			if (pCallback.m_eResult == EResult.k_EResultFail)
			{
				Terminal.WriteMessage("No stats found??");
				return;
			}
			SteamUserStats.GetStat("games_won", out SteamHelper.mGamesWon);
			SteamUserStats.GetStat("max_wave", out SteamHelper.mHighestWave);
			SteamUserStats.GetStat("gun_dmg", out SteamHelper.mGunDmg);
			SteamUserStats.GetStat("healing_done", out SteamHelper.mHeal);
			SteamUserStats.GetStat("minion_dmg", out SteamHelper.mMinionDmg);
			SteamUserStats.GetStat("melee_dmg", out SteamHelper.mMeleeDmg);
			Terminal.WriteMessage("Stats received!");
			SteamHelper.LogStats();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00016FCE File Offset: 0x000151CE
		private static void OnStatsStored(UserStatsStored_t pCallback)
		{
			if (pCallback.m_eResult != EResult.k_EResultOK)
			{
				Terminal.WriteMessage("Error storing stats.", MessageType.ERROR);
				return;
			}
			Terminal.WriteMessage("Stats stored.");
			SteamUserStats.RequestUserStats(SteamHelper.GetLocalID());
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00016FFC File Offset: 0x000151FC
		public static void UpdateAllStats()
		{
			if (!SteamHelper.STATS_INIT)
			{
				return;
			}
			SteamUserStats.SetStat("games_won", SteamHelper.mGamesWon);
			SteamUserStats.SetStat("max_wave", SteamHelper.mHighestWave);
			SteamUserStats.SetStat("gun_dmg", SteamHelper.mGunDmg);
			SteamUserStats.SetStat("healing_done", SteamHelper.mHeal);
			SteamUserStats.SetStat("minion_dmg", SteamHelper.mMinionDmg);
			SteamUserStats.SetStat("melee_dmg", SteamHelper.mMeleeDmg);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00017071 File Offset: 0x00015271
		public static void PushStats()
		{
			if (!SteamHelper.STATS_INIT)
			{
				return;
			}
			SteamHelper.UpdateAllStats();
			Terminal.WriteMessage("Storing stats...");
			SteamUserStats.StoreStats();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00017090 File Offset: 0x00015290
		private static void LogStats()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("     games_won: " + SteamHelper.mGamesWon);
			stringBuilder.AppendLine("     max_wave: " + SteamHelper.mHighestWave);
			stringBuilder.AppendLine("     gun_dmg: " + SteamHelper.mGunDmg);
			stringBuilder.AppendLine("     healing_done: " + SteamHelper.mHeal);
			stringBuilder.AppendLine("     minion_dmg: " + SteamHelper.mMinionDmg);
			stringBuilder.AppendLine("     melee_dmg: " + SteamHelper.mMeleeDmg);
			Terminal.WriteMessage("STATS: ");
			Terminal.WriteMessage(stringBuilder.ToString());
		}

		// Token: 0x040002BA RID: 698
		private static Callback<GameOverlayActivated_t> mGameOverlayActivated;

		// Token: 0x040002BB RID: 699
		public static bool SteamOverlayVisible;

		// Token: 0x040002BC RID: 700
		private static float mCooldown;

		// Token: 0x040002BD RID: 701
		public static int mGunDmg;

		// Token: 0x040002BE RID: 702
		public static int mMeleeDmg;

		// Token: 0x040002BF RID: 703
		public static int mHighestWave;

		// Token: 0x040002C0 RID: 704
		public static int mHeal;

		// Token: 0x040002C1 RID: 705
		public static int mMinionDmg;

		// Token: 0x040002C2 RID: 706
		public static int mGamesWon;

		// Token: 0x040002C3 RID: 707
		private static Callback<UserStatsReceived_t> mStatsReceived;

		// Token: 0x040002C4 RID: 708
		private static Callback<UserStatsStored_t> mStatsStored;

		// Token: 0x040002C5 RID: 709
		private static bool STATS_INIT;
	}
}
