using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;
using ZombieEstate2.Wave;

namespace ZombieEstate2
{
	// Token: 0x020000FB RID: 251
	public class WaveMaster
	{
		// Token: 0x0600069F RID: 1695 RVA: 0x00030FAC File Offset: 0x0002F1AC
		public WaveMaster(Sector level, int waveGenSeed)
		{
			this.mWaveGenRandom = new Random(waveGenSeed);
			this.Level = level;
			ZombieSpawner.sector = level;
			this.SpawnTiles = this.Level.SpawningTileList();
			this.CarePackageTiles = this.Level.CarePackageSpawnList();
			WaveGenerator waveGenerator = new WaveGenerator(GameManager.LevelName, this.mWaveGenRandom);
			this.WaveList = waveGenerator.Generate(Global.WaveCount, 0f);
			this.lastPercent = 2.5f;
			this.mHUD = new XboxWaveHUD();
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0003109C File Offset: 0x0002F29C
		public void GameStart()
		{
			if (Global.UnlimitedMode)
			{
				DeathMenu.CONTINUES = 0;
			}
			else
			{
				DeathMenu.CONTINUES = 3;
			}
			MusicEngine.Play(ZE2Songs.Wave);
			Camera.SetPos();
			foreach (Player player in Global.PlayerList)
			{
				player.LockInput.Start();
			}
			this.SetState(WaveState.BetweenWave, false);
			if (!Global.UnlimitedMode)
			{
				this.WaveList[this.WaveList.Count - 1].SET_GAME_COMPLETE();
			}
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00031140 File Offset: 0x0002F340
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.State == WaveState.BetweenWave)
			{
				if (Global.Paused)
				{
					return;
				}
				if (this.StatsShowing && this.mWaveStats != null)
				{
					foreach (XboxWaveStats xboxWaveStats in this.mWaveStats)
					{
						xboxWaveStats.Draw(spriteBatch);
					}
				}
				this.DrawMidWaveData(spriteBatch);
				if (this.WaveIndex != -1 && this.CurrentWave != null)
				{
					if (this.Timer_BetweenWave.SecondsLeft() < 4)
					{
						this.CurrentWave.AboutToCycleToNextWave();
					}
					this.CurrentWave.DrawAfterWaveComplete(spriteBatch);
				}
			}
			if (this.State == WaveState.WaveRunning)
			{
				if (Global.Paused)
				{
					return;
				}
				Indicator2D.DrawInd(spriteBatch);
				this.mHUD.Draw(spriteBatch, this.CurrentWave.Kills, this.CurrentWave.KillsToWin, this.WaveIndex + 1);
			}
			if (this.State == WaveState.WaveComplete)
			{
				this.CurrentWave.DrawComplete(spriteBatch);
				foreach (XboxWaveStats xboxWaveStats2 in this.mWaveStats)
				{
					xboxWaveStats2.Draw(spriteBatch);
				}
			}
			if (this.State == WaveState.WaveCompleteDelay)
			{
				if (Global.Paused)
				{
					return;
				}
				this.CurrentWave.DrawPreCompleteDelay(spriteBatch);
			}
			if (this.State == WaveState.PreWaveDelay)
			{
				if (Global.Paused)
				{
					return;
				}
				this.CurrentWave.DrawPreWaveDelay(spriteBatch);
				this.mHUD.Draw(spriteBatch, this.CurrentWave.Kills, this.CurrentWave.KillsToWin, this.WaveIndex + 1);
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x000312EC File Offset: 0x0002F4EC
		private void DrawMidWaveData(SpriteBatch spriteBatch)
		{
			int num = this.Timer_BetweenWave.SecondsLeft();
			string timeString = Global.GetTimeString(num);
			Vector2 position = VerchickMath.CenterText(Global.StoreFontXtraLarge, new Vector2((float)(Global.ScreenRect.Width / 2), 64f), this.nextString);
			if (num < 10)
			{
				Shadow.DrawOutlinedString(spriteBatch, Global.StoreFontXtraLarge, this.nextString, Color.DarkRed, Color.Red, 1f, 0f, position);
			}
			else
			{
				Shadow.DrawOutlinedString(spriteBatch, Global.StoreFontXtraLarge, this.nextString, Color.Black, Color.White, 1f, 0f, position);
			}
			position = new Vector2((float)(Global.ScreenRect.Width / 2 - 56), 64f);
			if (num < 10)
			{
				Shadow.DrawOutlinedString(spriteBatch, Global.StoreFontXtraLarge, timeString, Color.DarkRed, Color.Red, 1f, 0f, position);
				return;
			}
			Shadow.DrawOutlinedString(spriteBatch, Global.StoreFontXtraLarge, timeString, Color.Black, Color.White, 1f, 0f, position);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x000313EC File Offset: 0x0002F5EC
		private void StartWave()
		{
			foreach (Player player in Global.PlayerList)
			{
				player.Talleys.Clear();
			}
			this.CurrentWave.StartWaveRunning();
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0003144C File Offset: 0x0002F64C
		public void ResetWave()
		{
			Global.KillAllZombies();
			Global.KillAllAirDrops();
			Global.KillAllDrops();
			if (this.CurrentWave != null)
			{
				this.CurrentWave.ResetWave();
				this.WaveIndex--;
			}
			this.SetState(WaveState.BetweenWave, false);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00031488 File Offset: 0x0002F688
		public void EndWave()
		{
			if (this.State == WaveState.WaveCompleteDelay)
			{
				return;
			}
			SteamHelper.PushStats();
			if (this.WaveIndex == Global.WaveCount - 1 && !Global.UnlimitedMode)
			{
				MusicEngine.Play(ZE2Songs.Victory);
			}
			else
			{
				MusicEngine.Play(ZE2Songs.WinWave);
			}
			Global.KillAllZombies();
			this.CurrentWave.StartPreWaveCompleteDelay();
			this.SetState(WaveState.WaveCompleteDelay, false);
			this.mWaveStats.Clear();
			Global.EndListsDone = 0;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x000314F4 File Offset: 0x0002F6F4
		public void Update(float elapsed)
		{
			if (Global.CHEAT && InputManager.ButtonHeld(Keys.LeftControl, 0) && InputManager.ButtonPressed(Keys.E, 0))
			{
				this.SetState(WaveState.WaveJustEnded, false);
			}
			if (this.WON && this.mWinTimer != null && this.mWinTimer.Expired())
			{
				this.WON = false;
				Terminal.WriteMessage("Win timer expired.", MessageType.IMPORTANTEVENT);
				Global.WavesCompleted = Math.Max(0, this.WaveIndex + 1);
				MasterStore.Deactivate();
				Global.VictoryScreen = new XboxVictoryScreen(false);
				SoundEngine.TEMP_NO_SOUND_TIMER = 2f;
				NetworkManager.REJECT_INCOMING_MSGS = true;
				ScreenFader.Fade(delegate()
				{
					Global.GameState = GameState.Victory;
					NetworkManager.CloseAllConnections();
					SoundEngine.TEMP_NO_SOUND_TIMER = 2f;
				});
			}
			switch (this.State)
			{
			case WaveState.BetweenWave:
				this.UpdateBetween(elapsed);
				return;
			case WaveState.PreWaveDelay:
				this.UpdatePreWaveDelay(elapsed);
				return;
			case WaveState.WaveRunning:
				this.UpdateWaveRunning(elapsed);
				return;
			case WaveState.WaveCompleteDelay:
				this.UpdateWaveCompleteDelay(elapsed);
				return;
			case WaveState.WaveComplete:
				this.UpdateComplete(elapsed);
				return;
			case WaveState.WaveJustEnded:
				break;
			case WaveState.WaveFailed:
				this.UpdateFailed(elapsed);
				break;
			default:
				return;
			}
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00031608 File Offset: 0x0002F808
		private void UpdateBetween(float elapsed)
		{
			if (this.WaveIndex != -1 && this.CurrentWave != null)
			{
				this.CurrentWave.UpdateAfterWaveComplete();
			}
			bool flag = true;
			using (List<Player>.Enumerator enumerator = Global.PlayerList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.READY)
					{
						flag = false;
					}
				}
			}
			if (this.Timer_BetweenWave.Expired() || flag)
			{
				foreach (Player player in Global.PlayerList)
				{
					player.READY = false;
				}
				this.SetState(WaveState.PreWaveDelay, false);
				return;
			}
			if (this.Timer_BetweenWave.Expired())
			{
				this.SetState(WaveState.PreWaveDelay, false);
				return;
			}
			this.WAVE_STATE_SYNC_TIME -= elapsed;
			if (this.WAVE_STATE_SYNC_TIME < 0f && NetworkManager.AmIHost)
			{
				this.WAVE_STATE_SYNC_TIME = 1f;
				this.SendNetWaveState();
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0003171C File Offset: 0x0002F91C
		private void UpdatePreWaveDelay(float elapsed)
		{
			if (this.CurrentWave.UpdatePreWaveDelay(elapsed))
			{
				this.SetState(WaveState.WaveRunning, false);
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00031734 File Offset: 0x0002F934
		private void UpdateWaveRunning(float elapsed)
		{
			WaveCompletionState waveCompletionState = this.CurrentWave.UpdateWaveRunning(elapsed);
			if (waveCompletionState == WaveCompletionState.Running)
			{
				return;
			}
			if (waveCompletionState == WaveCompletionState.Completed)
			{
				this.SetState(WaveState.WaveJustEnded, false);
				return;
			}
			if (waveCompletionState == WaveCompletionState.Failed)
			{
				this.SetState(WaveState.WaveFailed, false);
				return;
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0003176C File Offset: 0x0002F96C
		private void UpdateWaveCompleteDelay(float elapsed)
		{
			bool flag = this.CurrentWave.UpdatePreCompleteDelay(elapsed);
			if (Global.CHEAT && InputManager.BackPressed(0))
			{
				this.SetState(WaveState.WaveComplete, false);
			}
			if (flag)
			{
				this.SetState(WaveState.WaveComplete, false);
			}
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0003179C File Offset: 0x0002F99C
		private void UpdateComplete(float elapsed)
		{
			foreach (XboxWaveStats xboxWaveStats in this.mWaveStats)
			{
				xboxWaveStats.Update();
			}
			if (this.mWaveStatsTimer.Expired())
			{
				this.SetState(WaveState.BetweenWave, false);
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00031804 File Offset: 0x0002FA04
		private void UpdateFailed(float elapsed)
		{
			if (this.CurrentWave.UpdateWaveFailed(elapsed))
			{
				this.SetState(WaveState.BetweenWave, false);
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0003181C File Offset: 0x0002FA1C
		private void SetState(WaveState state, bool FromNet = false)
		{
			if (!NetworkManager.AmIHost && !FromNet)
			{
				return;
			}
			if (this.State == state)
			{
				return;
			}
			this.State = state;
			if (NetworkManager.AmIHost)
			{
				this.SendNetWaveState();
			}
			if (this.State == WaveState.BetweenWave)
			{
				Global.MasterCache.gameObjectCaches[0].RefreshVertexBuffer();
				this.Timer_BetweenWave.Reset();
				this.Timer_BetweenWave.Start();
				if (!this.FinalWaveComplete && !MasterStore.Active)
				{
					MusicEngine.Play(ZE2Songs.Wave);
				}
				if (Global.UnlimitedMode)
				{
					Global.ZombieHealthMod = 1f + Math.Max(1f, (float)(Math.Pow((double)this.WaveIndex, 2.0) / 100.0));
				}
				if (Global.UnlimitedMode)
				{
					Global.DifficultyModeMod = 1f + (float)Math.Max(0, this.WaveIndex - 10) / 8f;
				}
				if (this.WaveIndex >= Global.WaveCount - 1)
				{
					if (Global.UnlimitedMode)
					{
						List<WaveBase> collection = new WaveGenerator(GameManager.LevelName, this.mWaveGenRandom).Generate(50, this.lastPercent);
						this.lastPercent += 2.5f;
						this.WaveList.AddRange(collection);
						Global.WaveCount += 50;
						return;
					}
					Global.WavesCompleted = Math.Max(0, this.WaveIndex + 1);
					MasterStore.Deactivate();
					Global.VictoryScreen = new XboxVictoryScreen(false);
					SoundEngine.TEMP_NO_SOUND_TIMER = 2f;
					NetworkManager.REJECT_INCOMING_MSGS = true;
					ScreenFader.Fade(delegate()
					{
						Global.GameState = GameState.Victory;
						NetworkManager.CloseAllConnections();
						SoundEngine.TEMP_NO_SOUND_TIMER = 2f;
					});
					return;
				}
			}
			if (this.State == WaveState.WaveRunning)
			{
				this.Timer_BetweenWave.Reset();
				this.StartWave();
			}
			if (state == WaveState.PreWaveDelay)
			{
				this.StatsShowing = false;
				MasterStore.Deactivate();
				MusicEngine.Play(Global.LevelSong);
				this.WaveIndex++;
				this.CurrentWave = this.WaveList[this.WaveIndex];
				this.CurrentWave.StartPreWaveDelay();
				this.PopulatePreWaveData();
				foreach (Player player in Global.PlayerList)
				{
					player.READY = false;
				}
			}
			if (state == WaveState.WaveCompleteDelay)
			{
				Global.KillAllZombies();
				foreach (Player player2 in Global.PlayerList)
				{
					if (player2.IAmOwnedByLocalPlayer)
					{
						player2.SendWaveStats();
					}
				}
				this.RespawnAllPlayers();
				this.PopulatePostWaveData();
				this.CurrentWave.GivePreAndPostStats(this.PreData, this.PostData);
				this.StatsShowing = true;
				Terminal.WriteMessage("WAVE " + (this.WaveIndex + 1) + " COMPLETE!", MessageType.IMPORTANTEVENT);
				if (this.WaveIndex >= Global.WaveCount - 1 && !Global.UnlimitedMode)
				{
					SteamHelper.mGamesWon++;
					SteamHelper.PushStats();
					this.WinAnimation();
				}
			}
			if (state == WaveState.WaveJustEnded)
			{
				this.EndWave();
			}
			if (state == WaveState.WaveComplete)
			{
				int amount = this.WaveIndex * 25 + 125;
				this.CalculateBadges();
				foreach (Player player3 in Global.PlayerList)
				{
					player3.InvincibilityTimer.Reset();
					player3.InvincibilityTimer.Start();
					player3.DataStrings.Clear();
					player3.DataStrings.Add(new DataString(4, 44, Vector2.Zero, Color.LightGreen, "+" + amount.ToString(), false, -1, -1, player3));
					player3.DataStrings.Add(new DataString(6, 37, Vector2.Zero, Color.LightYellow, "+2 Upgrade Tokens", false, -1, -1, player3));
					if (player3.GameTalleys.EarnedDmgThisWave)
					{
						player3.DataStrings.Add(new DataString(72, 46, Vector2.Zero, Color.White, "Dmg: " + VerchickMath.AddCommas((int)player3.Talleys.DamageDealt), true, 66, 46, player3));
					}
					else
					{
						player3.DataStrings.Add(new DataString(72, 46, Vector2.Zero, Color.White, "Dmg: " + VerchickMath.AddCommas((int)player3.Talleys.DamageDealt), true, -1, -1, player3));
					}
					if (player3.GameTalleys.EarnedHealThisWave)
					{
						player3.DataStrings.Add(new DataString(72, 47, Vector2.Zero, Color.White, "Heal: " + VerchickMath.AddCommas((int)player3.Talleys.HealingDone), true, 66, 47, player3));
					}
					else
					{
						player3.DataStrings.Add(new DataString(72, 47, Vector2.Zero, Color.White, "Heal: " + VerchickMath.AddCommas((int)player3.Talleys.HealingDone), true, -1, -1, player3));
					}
					if (player3.GameTalleys.EarnedTankThisWave)
					{
						player3.DataStrings.Add(new DataString(73, 47, Vector2.Zero, Color.White, "DmgTaken: " + VerchickMath.AddCommas((int)player3.Talleys.DamageTaken), true, 67, 47, player3));
					}
					else
					{
						player3.DataStrings.Add(new DataString(73, 47, Vector2.Zero, Color.White, "DmgTaken: " + VerchickMath.AddCommas((int)player3.Talleys.DamageTaken), true, -1, -1, player3));
					}
					if (player3.GameTalleys.EarnedMinionThisWave)
					{
						player3.DataStrings.Add(new DataString(73, 46, Vector2.Zero, Color.White, "MinionDmg: " + VerchickMath.AddCommas((int)player3.Talleys.MinionDamageDealt), true, 67, 46, player3));
					}
					else
					{
						player3.DataStrings.Add(new DataString(73, 46, Vector2.Zero, Color.White, "MinionDmg: " + VerchickMath.AddCommas((int)player3.Talleys.MinionDamageDealt), true, -1, -1, player3));
					}
				}
				foreach (Player player4 in Global.PlayerList)
				{
					player4.Stats.AddMoney(amount);
					player4.Stats.UpgradeTokens += 2;
					this.mWaveStats.Add(new XboxWaveStats(player4));
				}
				if (!this.mWaveStatsTimer.Running())
				{
					this.mWaveStatsTimer.IndependentOfTime = true;
					this.mWaveStatsTimer.Reset();
					this.mWaveStatsTimer.Start();
				}
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00031F34 File Offset: 0x00030134
		private void WinAnimation()
		{
			this.WON = true;
			this.mWinTimer = new Timer(15f);
			this.mWinTimer.IndependentOfTime = true;
			this.mWinTimer.Start();
			ScreenFader.Fade(delegate()
			{
				PingManager.DISABLE_PINGS = true;
				foreach (Player player in Global.PlayerList)
				{
					if (player.Index < 4)
					{
						player.Position = GameManager.level.PlayerSpawns[player.Index];
						player.mMovement = Vector2.Zero;
						player.Velocity = Vector3.Zero;
						int num = this.mWaveGenRandom.Next(0, 3);
						player.SetDanceState((byte)num);
						player.PlayerInputLocked = true;
						player.FaceDown();
					}
				}
				Camera.InitWinAnim();
			}, 0.025f);
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x00031F85 File Offset: 0x00030185
		public bool WaveActive
		{
			get
			{
				return this.State == WaveState.WaveRunning || this.State == WaveState.WaveCompleteDelay;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00031F9B File Offset: 0x0003019B
		public bool WaveRUNNING
		{
			get
			{
				return this.State == WaveState.WaveRunning;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00031FA6 File Offset: 0x000301A6
		public bool PreWaveActive
		{
			get
			{
				return this.State == WaveState.PreWaveDelay;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00031FB1 File Offset: 0x000301B1
		public int WaveNumber
		{
			get
			{
				return this.WaveIndex + 1;
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00031FBC File Offset: 0x000301BC
		public void RespawnAllPlayers()
		{
			foreach (Player player in Global.PlayerList)
			{
				if (player.DEAD)
				{
					player.Revive(true);
				}
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00032018 File Offset: 0x00030218
		private void PopulatePreWaveData()
		{
			this.PreData = new List<PlayerDataBeforeWave>();
			foreach (Player player in Global.PlayerList)
			{
				PlayerDataBeforeWave playerDataBeforeWave = new PlayerDataBeforeWave();
				playerDataBeforeWave.Player = player;
				playerDataBeforeWave.TotalKills = player.Stats.TotalZombiesKilled();
				this.PreData.Add(playerDataBeforeWave);
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00032098 File Offset: 0x00030298
		private void PopulatePostWaveData()
		{
			this.PostData = new List<PlayerDataBeforeWave>();
			foreach (Player player in Global.PlayerList)
			{
				PlayerDataBeforeWave playerDataBeforeWave = new PlayerDataBeforeWave();
				playerDataBeforeWave.Player = player;
				playerDataBeforeWave.TotalKills = player.Stats.TotalZombiesKilled();
				this.PostData.Add(playerDataBeforeWave);
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00032118 File Offset: 0x00030318
		private void CalculateBadges()
		{
			if (Global.PlayerList.Count == 1)
			{
				return;
			}
			Player player = null;
			Player player2 = null;
			Player player3 = null;
			Player player4 = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			foreach (Player player5 in Global.PlayerList)
			{
				player5.GameTalleys.EarnedDmgThisWave = false;
				player5.GameTalleys.EarnedHealThisWave = false;
				player5.GameTalleys.EarnedMinionThisWave = false;
				player5.GameTalleys.EarnedTankThisWave = false;
			}
			foreach (Player player6 in Global.PlayerList)
			{
				if (player == null)
				{
					player = player6;
				}
				if (player2 == null)
				{
					player2 = player6;
				}
				if (player3 == null)
				{
					player3 = player6;
				}
				if (player4 == null)
				{
					player4 = player6;
				}
				if (player6.Talleys.DamageDealt > player.Talleys.DamageDealt)
				{
					player = player6;
				}
				if (player6.Talleys.MinionDamageDealt > player2.Talleys.MinionDamageDealt)
				{
					player2 = player6;
				}
				if (player6.Talleys.HealingDone > player3.Talleys.HealingDone)
				{
					player3 = player6;
				}
				if (player6.Talleys.DamageTaken > player4.Talleys.DamageTaken)
				{
					player4 = player6;
				}
			}
			foreach (Player player7 in Global.PlayerList)
			{
				if (player != player7 && player.Talleys.DamageDealt == player7.Talleys.DamageDealt)
				{
					flag = true;
				}
				if (player3 != player7 && player3.Talleys.HealingDone == player7.Talleys.HealingDone)
				{
					flag3 = true;
				}
				if (player4 != player7 && player4.Talleys.DamageTaken == player7.Talleys.DamageTaken)
				{
					flag4 = true;
				}
				if (player2 != player7 && player2.Talleys.MinionDamageDealt == player7.Talleys.MinionDamageDealt)
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				player2.GameTalleys.MinionDmgBadges++;
				player2.GameTalleys.EarnedMinionThisWave = true;
			}
			if (!flag)
			{
				player.GameTalleys.DmgBadges++;
				player.GameTalleys.EarnedDmgThisWave = true;
			}
			if (!flag3)
			{
				player3.GameTalleys.HealBadges++;
				player3.GameTalleys.EarnedHealThisWave = true;
			}
			if (!flag4)
			{
				player4.GameTalleys.TankBadges++;
				player4.GameTalleys.EarnedTankThisWave = true;
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x000323D0 File Offset: 0x000305D0
		public void EndGameDefeat()
		{
			if (this.State == WaveState.WaveRunning || this.State == WaveState.PreWaveDelay)
			{
				Global.WavesCompleted = Math.Max(0, this.WaveIndex);
			}
			else
			{
				Global.WavesCompleted = Math.Max(0, this.WaveIndex + 1);
			}
			MasterStore.Deactivate();
			Global.VictoryScreen = new XboxVictoryScreen(true);
			Global.GameState = GameState.Victory;
			NetworkManager.CloseAllConnections();
			NetworkManager.REJECT_INCOMING_MSGS = true;
			SoundEngine.TEMP_NO_SOUND_TIMER = 2f;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00032444 File Offset: 0x00030644
		private void SendNetWaveState()
		{
			NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.WaveStateUpdate);
			Msg_WaveStateUpdate msg_WaveStateUpdate = new Msg_WaveStateUpdate();
			msg_WaveStateUpdate.WaveNumber = this.WaveIndex;
			msg_WaveStateUpdate.WaveState = (byte)this.State;
			if (this.State == WaveState.BetweenWave)
			{
				msg_WaveStateUpdate.PreSecondsLeft = this.Timer_BetweenWave.SecondsLeft();
			}
			netMessage.Payload = msg_WaveStateUpdate;
			NetworkManager.SendMessage(netMessage, SendType.Reliable);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0003249C File Offset: 0x0003069C
		public void ReceiveNetWaveState(Msg_WaveStateUpdate payload)
		{
			this.WaveIndex = payload.WaveNumber;
			this.SetState((WaveState)payload.WaveState, true);
			if (this.State == WaveState.BetweenWave && this.Timer_BetweenWave.SecondsLeft() != payload.PreSecondsLeft)
			{
				this.Timer_BetweenWave.SetSecondsLeft(payload.PreSecondsLeft);
			}
			if (this.State == WaveState.PreWaveDelay)
			{
				this.Timer_BetweenWave.Reset();
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00032502 File Offset: 0x00030702
		public bool FinalWaveComplete
		{
			get
			{
				return !Global.UnlimitedMode && this.WaveIndex == Global.WaveCount - 1;
			}
		}

		// Token: 0x0400066A RID: 1642
		private Sector Level;

		// Token: 0x0400066B RID: 1643
		public List<Tile> SpawnTiles;

		// Token: 0x0400066C RID: 1644
		public List<Tile> CarePackageTiles;

		// Token: 0x0400066D RID: 1645
		private List<WaveBase> WaveList;

		// Token: 0x0400066E RID: 1646
		private WaveState State = WaveState.NONE;

		// Token: 0x0400066F RID: 1647
		private int WaveIndex = -1;

		// Token: 0x04000670 RID: 1648
		public WaveBase CurrentWave;

		// Token: 0x04000671 RID: 1649
		private Timer mWaveStatsTimer = new Timer(10f);

		// Token: 0x04000672 RID: 1650
		private Timer Timer_BetweenWave = new Timer(180f);

		// Token: 0x04000673 RID: 1651
		public List<PlayerDataBeforeWave> PreData;

		// Token: 0x04000674 RID: 1652
		public List<PlayerDataBeforeWave> PostData;

		// Token: 0x04000675 RID: 1653
		public List<XboxWaveStats> mWaveStats = new List<XboxWaveStats>();

		// Token: 0x04000676 RID: 1654
		public bool StatsShowing;

		// Token: 0x04000677 RID: 1655
		private XboxWaveHUD mHUD;

		// Token: 0x04000678 RID: 1656
		private float lastPercent;

		// Token: 0x04000679 RID: 1657
		private float WAVE_STATE_SYNC_TIME = 1f;

		// Token: 0x0400067A RID: 1658
		private bool DUMMY;

		// Token: 0x0400067B RID: 1659
		private Random mWaveGenRandom;

		// Token: 0x0400067C RID: 1660
		private string nextWaveString = "Press [ENTER] to start the next wave!";

		// Token: 0x0400067D RID: 1661
		private string readyString = "READY";

		// Token: 0x0400067E RID: 1662
		private string nextString = "Next";

		// Token: 0x0400067F RID: 1663
		private bool WON;

		// Token: 0x04000680 RID: 1664
		private Timer mWinTimer;
	}
}
