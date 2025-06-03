using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieEstate2.UI.Chat;
using ZombieEstate2.UI.Menus;
using ZombieEstate2.UI.Xbox;
using ZombieEstate2.XboxSaving;

namespace ZombieEstate2
{
	// Token: 0x0200004C RID: 76
	public class GameManager
	{
		// Token: 0x060001BF RID: 447 RVA: 0x0000C678 File Offset: 0x0000A878
		public GameManager()
		{
			this.mStartLoadScreen = new StartLoadScreen();
			Global.KillAllZombies();
			Global.MasterCache = null;
			Global.ZombieList = new List<Zombie>();
			Global.MainOnTop = false;
			Global.BossActive = false;
			Global.Boss = null;
			if (GameManager.PLANONEDITING)
			{
				Global.AOEnabled = false;
			}
			Rectangle pos = new Rectangle((int)Global.GetScreenCenter().X - 32, (int)Global.GetScreenCenter().Y - 32, 64, 64);
			this.mLevelLoadIcon = new LoadIcon(pos);
			if (GameManager.LOADFLATFIELD)
			{
				this.editor = new Editor(Global.Game);
				Global.Editor = this.editor;
				this.GotoMainMenu();
				return;
			}
			if (!GameManager.STARTFROMMAINMENU)
			{
				Global.GameState = GameState.MainLoadScreen;
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000C758 File Offset: 0x0000A958
		public void GotoMainMenu()
		{
			if (this.mGameChat != null)
			{
				this.mGameChat.Destroy();
				this.mGameChat = null;
			}
			MouseHandler.TargetReticule = false;
			Global.GameState = GameState.MainMenu;
			MenuManager.PushMenu(new MainMenu());
			Camera.RESET_ANIM();
			this.Reloadloadload();
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000C798 File Offset: 0x0000A998
		public void GotoCharSelect()
		{
			Terminal.WriteMessage("GotoCharSelect");
			if (this.CharSelect != null)
			{
				this.CharSelect.Destroy();
			}
			this.CharSelect = new XboxMasterCharacterSelect();
			Global.GameState = GameState.CharacterSelect;
			TipManager.Init();
			if (DynamicShadows.Shootables != null)
			{
				DynamicShadows.Shootables.Clear();
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000C7EC File Offset: 0x0000A9EC
		public void Init()
		{
			if (!GameManager.LOADFLATFIELD)
			{
				this.editor = new Editor(Global.Game);
				Global.Editor = this.editor;
			}
			if (!SteamHelper.GetLocalID().IsValid())
			{
				Terminal.WriteMessage("Invalid steam id - can not load player data.", MessageType.ERROR);
				return;
			}
			XboxSaverLoader.LoadData();
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000C83C File Offset: 0x0000AA3C
		public void Reloadloadload()
		{
			Rectangle pos = new Rectangle((int)Global.GetScreenCenter().X - 32, (int)Global.GetScreenCenter().Y - 32, 64, 64);
			this.mLevelLoadIcon = new LoadIcon(pos);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000C87C File Offset: 0x0000AA7C
		public void UpdateGame(float elapsed)
		{
			DialogMaster.UpdateDialogs();
			MenuManager.Update();
			this.MouseVisibility();
			if (DialogMaster.DialogsActive)
			{
				TimerHandler.Update(0f);
				return;
			}
			if (Global.Paused)
			{
				elapsed = 0f;
			}
			TimerHandler.Update(elapsed);
			if (Global.CHEAT && InputManager.ButtonHeld(Keys.K, 0))
			{
				Camera.SlowTime(0.1f);
			}
			GameState gameState = Global.GameState;
			if (gameState != GameState.Playing)
			{
				switch (gameState)
				{
				case GameState.Editor:
					MouseHandler.TargetReticule = false;
					this.CameraStuff(elapsed);
					this.editor.UpdateEditor(elapsed);
					return;
				case GameState.Paused:
				case GameState.CutSceneEditor:
					break;
				case GameState.LevelLoading:
					MouseHandler.TargetReticule = false;
					if (GameManager.level != null && GameManager.level.doneLoading && !this.DoneLoading && !Global.Game.GameMinimized)
					{
						ScreenFader.Fade(new ScreenFader.FadeDone(this.DoneLoadingLevel));
						this.DoneLoading = true;
					}
					this.mLevelLoadIcon.Update(elapsed);
					this.x += elapsed;
					return;
				case GameState.CharacterSelect:
					MouseHandler.TargetReticule = false;
					this.CharSelect.Update(elapsed);
					return;
				default:
					switch (gameState)
					{
					case GameState.WaitingForOtherPlayers:
					{
						MouseHandler.TargetReticule = false;
						bool flag = true;
						foreach (int key in this.Readys.Keys)
						{
							if (!this.Readys[key])
							{
								flag = false;
							}
						}
						if (flag)
						{
							Global.GameState = GameState.Playing;
							return;
						}
						break;
					}
					case GameState.MainLoadScreen:
						MouseHandler.TargetReticule = false;
						this.mStartLoadScreen.Update(elapsed);
						if (StartLoadScreen.DONE && !this.mTransitioningFromLoad)
						{
							this.mTransitioningFromLoad = true;
							ScreenFader.Fade(delegate()
							{
								if (Global.CHEAT)
								{
									MenuManager.PushMenu(new PressStart());
								}
								else
								{
									MenuManager.PushMenu(new LogoMenu());
								}
								Global.GameState = GameState.PressStart;
							});
							return;
						}
						break;
					case GameState.PressStart:
						break;
					case GameState.Victory:
						Global.VictoryScreen.Update();
						break;
					default:
						return;
					}
					break;
				}
				return;
			}
			this.CameraStuff(elapsed);
			if (this.StoreClosing)
			{
				foreach (Player player in Global.PlayerList)
				{
					if (player.IAmOwnedByLocalPlayer)
					{
						player.LockInput.Reset();
						player.LockInput.Start();
					}
				}
				this.StoreClosing = false;
				Global.Paused = false;
				return;
			}
			Global.WaveMaster.Update(elapsed);
			Global.MasterCache.UpdateCaches(elapsed);
			if (MenuManager.MenuOpen() || MasterStore.Active)
			{
				MouseHandler.TargetReticule = false;
			}
			else
			{
				MouseHandler.TargetReticule = true;
			}
			Global.MasterCache.particleSystem.Update(elapsed);
			this.mGameChat.Update(elapsed);
			Global.MasterCache.LATE_UPDATE();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000CB44 File Offset: 0x0000AD44
		private void MouseVisibility()
		{
			if (Global.GameState != GameState.Playing)
			{
				MouseHandler.MouseVisible = true;
				return;
			}
			bool mouseVisible = false;
			for (int i = 0; i < 4; i++)
			{
				PlayerInfo player = PlayerManager.GetPlayer(i);
				if (player != null && !player.UsingController)
				{
					mouseVisible = true;
					break;
				}
			}
			MouseHandler.MouseVisible = mouseVisible;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000CB8C File Offset: 0x0000AD8C
		public void DrawGame(SpriteBatch spriteBatch)
		{
			switch (Global.GameState)
			{
			case GameState.Playing:
				DynamicShadows.DrawShadows(spriteBatch);
				GameManager.level.DrawLevel();
				Global.MasterCache.DrawObjects();
				Global.MasterCache.particleSystem.Draw(spriteBatch);
				this.DrawHUDStuff(spriteBatch);
				break;
			case GameState.Editor:
				this.editor.DrawSectorAndObjects();
				this.editor.DrawHUD(spriteBatch);
				break;
			case GameState.LevelLoading:
				this.DrawLoading(spriteBatch);
				break;
			case GameState.CharacterSelect:
				this.CharSelect.Draw(spriteBatch);
				break;
			case GameState.MainMenu:
				spriteBatch.Draw(Global.Pixel, Global.ScreenRect, new Color(0.3f, 0.3f, 0.3f));
				break;
			case GameState.WaitingForOtherPlayers:
				spriteBatch.Draw(Global.Pixel, Global.ScreenRect, Color.Black);
				this.DrawLoading(spriteBatch);
				break;
			case GameState.MainLoadScreen:
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null);
				this.mStartLoadScreen.Draw(spriteBatch);
				spriteBatch.End();
				spriteBatch.Begin();
				break;
			case GameState.Victory:
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null);
				Global.VictoryScreen.Draw(spriteBatch);
				spriteBatch.End();
				spriteBatch.Begin();
				break;
			}
			MenuManager.Draw(spriteBatch);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000CCF8 File Offset: 0x0000AEF8
		private void DrawLoading(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null);
			if (Global.GameState == GameState.LevelLoading)
			{
				spriteBatch.Draw(Global.MenuBG, Global.ScreenRect, Color.White);
				MouseHandler.TargetReticule = true;
				this.mLevelLoadIcon.Draw(spriteBatch);
				Vector2 pos = VerchickMath.CenterText(Global.StoreFontXtraLarge, Global.GetScreenCenter(), this.mLoadString);
				pos.Y -= 96f;
				Shadow.DrawString(this.mLoadString, Global.StoreFontXtraLarge, pos, 3, Color.White, Color.DarkGray * 0.5f, spriteBatch);
				TipManager.Draw(spriteBatch);
			}
			else if (Global.GameState == GameState.WaitingForOtherPlayers)
			{
				MouseHandler.TargetReticule = true;
				Vector2 position = VerchickMath.CenterText(Global.StoreFontBig, Global.GetScreenCenter(), "Waiting for other players...");
				spriteBatch.DrawString(Global.StoreFontBig, "Waiting for other players...", position, Color.White);
			}
			spriteBatch.End();
			spriteBatch.Begin();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000CDE4 File Offset: 0x0000AFE4
		private void DrawHUDStuff(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null);
			Global.WaveMaster.Draw(spriteBatch);
			if (Global.MasterCache != null && !Global.Editor.EDITING && !CutSceneMaster.Active)
			{
				foreach (Player player in Global.PlayerList)
				{
					player.Draw(spriteBatch);
				}
			}
			DialogMaster.DrawDialogs(spriteBatch);
			this.mGameChat.Draw(spriteBatch);
			spriteBatch.End();
			spriteBatch.Begin();
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000CE8C File Offset: 0x0000B08C
		public void CameraStuff(float elapsed)
		{
			if (!Global.Editor.EDITING)
			{
				Camera.UpdateCamera(elapsed);
				return;
			}
			if (Global.Editor.TopView)
			{
				Global.CameraPosition.Z = Global.CameraLookAt.Z + 0.25f;
			}
			float num = 0.25f;
			if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
			{
				num = 0.025f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.S))
			{
				Global.CameraPosition.Z = Global.CameraPosition.Z + num;
				Global.CameraLookAt.Z = Global.CameraLookAt.Z + num;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.D))
			{
				Global.CameraPosition.X = Global.CameraPosition.X + num;
				Global.CameraLookAt.X = Global.CameraLookAt.X + num;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.W))
			{
				Global.CameraPosition.Z = Global.CameraPosition.Z - num;
				Global.CameraLookAt.Z = Global.CameraLookAt.Z - num;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.A))
			{
				Global.CameraPosition.X = Global.CameraPosition.X - num;
				Global.CameraLookAt.X = Global.CameraLookAt.X - num;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.E))
			{
				if (!Global.Editor.TopView)
				{
					Global.CameraPosition.Y = Global.CameraPosition.Y - num;
					Global.CameraPosition.Z = Global.CameraPosition.Z - num;
				}
				else
				{
					this.editorZoom += 0.05f;
					this.editorZoom = MathHelper.Clamp(this.editorZoom, 0.25f, 4f);
				}
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Q))
			{
				if (!Global.Editor.TopView)
				{
					Global.CameraPosition.Y = Global.CameraPosition.Y + num;
					Global.CameraPosition.Z = Global.CameraPosition.Z + num;
					return;
				}
				this.editorZoom -= 0.05f;
				this.editorZoom = MathHelper.Clamp(this.editorZoom, 0.25f, 4f);
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000D084 File Offset: 0x0000B284
		public void StartGame(string levelName)
		{
			GameManager.LevelName = levelName;
			this.LoadLevel();
			if (GameManager.LOADFLATFIELD)
			{
				this.DoneLoadingLevel();
			}
			int width = 400;
			int num = 400;
			int bottomHeight = 48;
			this.mGameChat = new ChatUI(new Rectangle(16, (int)(Global.GetScreenCenter().Y - (float)num / 2f), width, num), false, Color.Black, bottomHeight);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000D0E8 File Offset: 0x0000B2E8
		public void GotoLevelSelect()
		{
			MenuManager.PushMenu(new LevelSelect());
			Global.GameState = GameState.LevelSelect;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000D0FC File Offset: 0x0000B2FC
		private void LoadLevel()
		{
			Terminal.WriteMessage("#####################################");
			this.testSW = Stopwatch.StartNew();
			Global.GameState = GameState.LevelLoading;
			GameManager.level = new Level(GameManager.LevelName);
			if (GameManager.LOADFLATFIELD)
			{
				GameManager.level.doneLoading = true;
				return;
			}
			GameManager.level.LoadLevel();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000D150 File Offset: 0x0000B350
		private void DoneLoadingLevel()
		{
			Terminal.WriteMessage("################### Done in " + this.testSW.Elapsed.TotalSeconds);
			Global.GameState = GameState.Playing;
			this.editor.level = GameManager.level;
			Global.Level = GameManager.level.MainSector;
			this.editor.Init(Global.Game, GameManager.level);
			if (Global.MasterCache != null)
			{
				Global.MasterCache.ClearObjects();
			}
			else
			{
				Global.MasterCache = new MasterCache(Global.Game, 1, 6000, true, "Master_MasterCache");
			}
			foreach (Player player in Global.PlayerList)
			{
				Global.MasterCache.CreateObject(player);
				player.Position = GameManager.level.PlayerSpawns[player.Index];
			}
			Global.Player = Global.PlayerList[0];
			new StoreKeep(Level.ShopKeepLocation);
			Global.Player.LOADED = true;
			int index = Global.Player.Index;
			this.Readys[index] = true;
			Global.WaveMaster.GameStart();
			this.MouseCursorColor();
			MasterStore.Init();
			if (GameManager.LevelName == "Estate")
			{
				Global.LevelSong = ZE2Songs.Estate;
			}
			if (GameManager.LevelName == "School")
			{
				Global.LevelSong = ZE2Songs.School;
			}
			if (GameManager.LevelName == "Skyscraper")
			{
				Global.LevelSong = ZE2Songs.Skyscraper;
			}
			if (GameManager.LevelName == "Mall")
			{
				Global.LevelSong = ZE2Songs.Mall;
			}
			if (GameManager.LevelName == "DesertTown")
			{
				Global.LevelSong = ZE2Songs.Desert;
			}
			if (GameManager.LevelName == "Office")
			{
				Global.LevelSong = ZE2Songs.Office;
			}
			if (GameManager.LevelName == "Farm")
			{
				Global.LevelSong = ZE2Songs.Estate;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000D348 File Offset: 0x0000B548
		private void StoreThreadFn()
		{
			DateTime now = DateTime.Now;
			Global.Paused = true;
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			float num = (float)(DateTime.Now - now).Milliseconds;
			Thread.Sleep((int)Math.Max(0f, 1000f - num));
			Global.Paused = false;
			this.StoreClosing = false;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000D3AC File Offset: 0x0000B5AC
		private void MouseCursorColor()
		{
			MouseHandler.src = Global.GetTexRectange(0, 8);
			for (int i = 0; i < 4; i++)
			{
				PlayerInfo player = PlayerManager.GetPlayer(i);
				if (player != null && player.Local && player.ControllerIndex == -1)
				{
					MouseHandler.src = Global.GetTexRectange(player.Index, 8);
					return;
				}
			}
		}

		// Token: 0x04000131 RID: 305
		public static bool PLANONEDITING = false;

		// Token: 0x04000132 RID: 306
		public static bool LOADFLATFIELD = false;

		// Token: 0x04000133 RID: 307
		public static bool STARTFROMMAINMENU = false;

		// Token: 0x04000134 RID: 308
		public static Level level;

		// Token: 0x04000135 RID: 309
		public static string LevelName = "Estate";

		// Token: 0x04000136 RID: 310
		private Editor editor;

		// Token: 0x04000137 RID: 311
		private float editorZoom;

		// Token: 0x04000138 RID: 312
		private bool DoneLoading;

		// Token: 0x04000139 RID: 313
		private float x;

		// Token: 0x0400013A RID: 314
		private Dictionary<int, bool> Readys = new Dictionary<int, bool>();

		// Token: 0x0400013B RID: 315
		private XboxMasterCharacterSelect CharSelect;

		// Token: 0x0400013C RID: 316
		private StartLoadScreen mStartLoadScreen;

		// Token: 0x0400013D RID: 317
		private LoadIcon mLevelLoadIcon;

		// Token: 0x0400013E RID: 318
		public bool StoreClosing;

		// Token: 0x0400013F RID: 319
		private Thread mStoreThread;

		// Token: 0x04000140 RID: 320
		private ChatUI mGameChat;

		// Token: 0x04000141 RID: 321
		private bool mTransitioningFromLoad;

		// Token: 0x04000142 RID: 322
		public bool INITIATED_CHAR_SEL_WITH_MOUSE = true;

		// Token: 0x04000143 RID: 323
		public int INITIATED_CONTROL_INDEX = -1;

		// Token: 0x04000144 RID: 324
		private string mLoadString = "Loading...";

		// Token: 0x04000145 RID: 325
		private Stopwatch testSW;
	}
}
