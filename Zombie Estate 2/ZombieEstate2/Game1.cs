using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using EventInputNamespace;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Steamworks;
using ZombieEstate2.Networking;
using ZombieEstate2.UI;
using ZombieEstate2.XboxSaving;

namespace ZombieEstate2
{
	// Token: 0x02000122 RID: 290
	public class Game1 : Game
	{
		// Token: 0x0600081E RID: 2078 RVA: 0x000439A4 File Offset: 0x00041BA4
		public Game1()
		{
			Console.WriteLine("-------------- ZOMBIE ESTATE 2 --------------");
			Global.Game = this;
			if (SteamAPI.RestartAppIfNecessary(this.mAppID))
			{
				Console.WriteLine("RestartAppIfNecessary returned true.");
				base.Exit();
				this.mExiting = true;
				return;
			}
			this.mSteamInitialized = SteamAPI.Init();
			if (!this.mSteamInitialized)
			{
				Terminal.WriteMessage("SteamAPI failed to init.", MessageType.ERROR);
				MessageBox.Show("Error - Could not connect to Steam Client. Ensure Steam is running, or restart it or your computer and try again.");
				base.Exit();
				this.mExiting = true;
				return;
			}
			if (!DllCheck.Test())
			{
				Terminal.WriteMessage("SteamAPI DLL TEST FAILED", MessageType.ERROR);
			}
			if (!Packsize.Test())
			{
				Terminal.WriteMessage("SteamAPI PACKSIZE TEST FAILED", MessageType.ERROR);
			}
			NetworkManager.Init(0, true);
			SteamHelper.Init();
			EventInput.Initialize(base.Window);
			this.graphics = new GraphicsDeviceManager(this);
			base.Content.RootDirectory = "Content";
			Global.Graphics = this.graphics;
			Global.ScreenRect = new Rectangle(0, 0, Config.Instance.ScreenWidth, Config.Instance.ScreenHeight);
			this.graphics.PreferredBackBufferWidth = Config.Instance.ScreenWidth;
			this.graphics.PreferredBackBufferHeight = Config.Instance.ScreenHeight;
			NetworkManager.DrawDiag = Config.Instance.DrawNetDiag;
			if (false)
			{
				this.graphics.GraphicsProfile = GraphicsProfile.Reach;
			}
			else if (GraphicsAdapter.DefaultAdapter.IsProfileSupported(GraphicsProfile.HiDef))
			{
				this.graphics.GraphicsProfile = GraphicsProfile.HiDef;
			}
			else
			{
				this.graphics.GraphicsProfile = GraphicsProfile.Reach;
			}
			this.graphics.IsFullScreen = (Config.Instance.ScreenMode == ScreenMode.FullScreen);
			base.IsFixedTimeStep = false;
			base.InactiveSleepTime = TimeSpan.Zero;
			this.graphics.SynchronizeWithVerticalRetrace = Config.Instance.VSync;
			this.graphics.ApplyChanges();
			base.Exiting += this.Game1_Exiting;
			if (!Global.CHEAT)
			{
				Game1.EnableMenuItem(Game1.GetSystemMenu(base.Window.Handle, false), 61536U, 1U);
			}
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00043BB1 File Offset: 0x00041DB1
		protected override void Initialize()
		{
			base.Initialize();
			this.mForm = (Form)Control.FromHandle(base.Window.Handle);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00043BD4 File Offset: 0x00041DD4
		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch(base.GraphicsDevice);
			Global.MasterEnvTex = base.Content.Load<Texture2D>("MasterWall_Test_Desert");
			Global.MasterLightTex = base.Content.Load<Texture2D>("MasterWall_LIGHT");
			Global.MasterLightTexTransparent = base.Content.Load<Texture2D>("MasterWall_LIGHT_Transparent");
			PCButton.LOAD(base.Content);
			Global.CurrentLevelLightTex = base.Content.Load<Texture2D>("Estate_Shadow");
			Global.MasterTexture = base.Content.Load<Texture2D>("MasterGrid16x16");
			Global.MenuBG = base.Content.Load<Texture2D>("MenuBG");
			Global.Font = base.Content.Load<SpriteFont>("Font");
			Global.BloodGutterLarge = base.Content.Load<SpriteFont>("BloodGutter");
			Global.BloodGutterSmall = base.Content.Load<SpriteFont>("BloodGutterSmall");
			Global.BloodGutterXtraLarge = base.Content.Load<SpriteFont>("BloodGutterXtraLarge");
			Global.StoreFont = base.Content.Load<SpriteFont>("StoreFont");
			Global.StoreFontSmall = base.Content.Load<SpriteFont>("StoreFontSmall");
			Global.StoreFontBig = base.Content.Load<SpriteFont>("StoreFontBig");
			Global.StoreFontXtraLarge = base.Content.Load<SpriteFont>("StoreFontHuge");
			Global.EquationFont = base.Content.Load<SpriteFont>("EquationFont");
			Global.EquationFontSmall = base.Content.Load<SpriteFont>("EquationFontSmall");
			Global.SpeechBubble = base.Content.Load<Texture2D>("SpeechBubble");
			Global.instancedEffect = base.Content.Load<Effect>("InstancedModel");
			Global.levelEffect = base.Content.Load<Effect>("LevelShader");
			Global.WaveHUD = base.Content.Load<Texture2D>("Misc\\Timer");
			Global.BloodGutterLarge.DefaultCharacter = new char?(' ');
			Global.BloodGutterSmall.DefaultCharacter = new char?(' ');
			Global.BloodGutterXtraLarge.DefaultCharacter = new char?(' ');
			Global.StoreFont.DefaultCharacter = new char?(' ');
			Global.StoreFontSmall.DefaultCharacter = new char?(' ');
			Global.StoreFontBig.DefaultCharacter = new char?(' ');
			Global.StoreFontXtraLarge.DefaultCharacter = new char?(' ');
			Global.EquationFont.DefaultCharacter = new char?(' ');
			Global.EquationFontSmall.DefaultCharacter = new char?(' ');
			Global.Font.DefaultCharacter = new char?(' ');
			HUDWaveObjective.Init();
			MouseHandler.MouseTex = base.Content.Load<Texture2D>("Mouse");
			MouseHandler.src = Global.GetTexRectange(0, 8);
			ZEButton.Init(base.Content);
			PCItem.Background = base.Content.Load<Texture2D>("Store\\PCStore\\StoreIcon");
			PCItem.SelectedBackground = base.Content.Load<Texture2D>("Store\\PCStore\\StoreIconSelected");
			Global.BloodEffect = base.Content.Load<Texture2D>("Blood");
			Particle2D.TexMunch = base.Content.Load<Texture2D>("Misc\\Munch");
			Particle2D.TexZzz = base.Content.Load<Texture2D>("Misc\\Zzz");
			Global.Pixel = base.Content.Load<Texture2D>("Pixel");
			Global.Content = base.Content;
			MusicEngine.Init();
			Zombie.InitWorths();
			this.termScreen = new TerminalScreen(this, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
			if (Terminal.WRITETOFILE)
			{
				Terminal.Initialize("Zombie Estate 2");
			}
			if (!Directory.Exists(XboxSaverLoader.FOLDER))
			{
				try
				{
					Directory.CreateDirectory(XboxSaverLoader.FOLDER);
				}
				catch (Exception ex)
				{
					Terminal.WriteMessage("Directory could not be created! " + XboxSaverLoader.FOLDER, MessageType.ERROR);
					Terminal.WriteMessage(ex.ToString(), MessageType.ERROR);
					Terminal.WriteMessage(ex.StackTrace.ToString(), MessageType.ERROR);
				}
			}
			Global.GraphicsDevice = base.GraphicsDevice;
			Global.basicEffect = new BasicEffect(base.GraphicsDevice);
			base.IsMouseVisible = false;
			EditorSounds.LoadSounds(this);
			new List<Point>();
			FPSTracker.Initialize(this);
			this.rect = new Rectangle(0, 0, 16, 16);
			this.GameManager = new GameManager();
			Global.GameManager = this.GameManager;
			this.GameManager.Init();
			InputManager.Init();
			XboxInputManager.Init();
			MasterStore.Init();
			SoundEngine.Init();
			DynamicShadows.Init();
			Game1.TESTState = new SamplerState
			{
				AddressU = TextureAddressMode.Clamp,
				AddressV = TextureAddressMode.Clamp,
				Filter = TextureFilter.Point
			};
			this.SetupConfig();
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00044044 File Offset: 0x00042244
		private void SetupConfig()
		{
			Global.Config = new Config();
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00002EF9 File Offset: 0x000010F9
		protected override void UnloadContent()
		{
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00044050 File Offset: 0x00042250
		protected override void Update(GameTime gameTime)
		{
			if (this.mExiting)
			{
				return;
			}
			if (this.mSteamInitialized)
			{
				SteamAPI.RunCallbacks();
			}
			this.ClipMouse();
			InputManager.UpdateCurrent();
			XboxInputManager.Update();
			Vibrate.Update();
			MusicEngine.Update();
			SteamHelper.Update();
			if (Global.CHEAT && InputManager.ButtonHeld(Microsoft.Xna.Framework.Input.Keys.LeftControl, 0) && InputManager.ButtonHeld(Microsoft.Xna.Framework.Input.Keys.LeftAlt, 0) && InputManager.ButtonHeld(Microsoft.Xna.Framework.Input.Keys.K, 0))
			{
				Global.GameEnded = true;
				base.Exit();
			}
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			Global.REAL_GAME_TIME = num;
			num *= Global.SpeedMod;
			if (!Global.Slowed && Global.SpeedMod != 1f)
			{
				Global.SpeedMod = MathHelper.Lerp(Global.SpeedMod, 1f, 0.01f);
			}
			this.GameManager.UpdateGame(num);
			MasterStore.Update();
			this.termScreen.Update();
			ScreenTransition.Update();
			InputManager.UpdatePrevs();
			XboxInputManager.EndUpdate();
			ScreenFader.Update(num);
			SoundEngine.Update();
			NetworkManager.Update(num);
			base.Update(gameTime);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00044158 File Offset: 0x00042358
		protected override void Draw(GameTime gameTime)
		{
			Global.GraphicsDevice.Clear(Color.Black);
			FPSTracker.Update(gameTime);
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			this.PulseUpdate(elapsed);
			base.GraphicsDevice.BlendState = BlendState.AlphaBlend;
			base.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			Viewport viewport = base.GraphicsDevice.Viewport;
			Global.AspectRatio = (float)viewport.Width / (float)viewport.Height;
			Global.View = Matrix.CreateLookAt(Global.CameraPosition, Global.CameraLookAt, Vector3.Up);
			if (!Global.Editor.TopView)
			{
				Global.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), Global.AspectRatio, 1f, 10000f);
			}
			else
			{
				Global.Projection = Matrix.CreateOrthographic(17.777f * this.editorZoom, 10f * this.editorZoom, 0.1f, 1000f);
			}
			base.GraphicsDevice.SamplerStates[0] = Game1.TESTState;
			this.spriteBatch.Begin();
			this.GameManager.DrawGame(this.spriteBatch);
			this.spriteBatch.End();
			this.spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null);
			FPSTracker.Draw(this.spriteBatch);
			MasterStore.Draw(this.spriteBatch);
			NetworkManager.Draw(this.spriteBatch);
			ScreenFader.Draw(this.spriteBatch);
			this.termScreen.Draw(this.spriteBatch);
			MouseHandler.DrawMouse(this.spriteBatch);
			this.spriteBatch.End();
			base.Draw(gameTime);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x000442F4 File Offset: 0x000424F4
		private void PulseUpdate(float elapsed)
		{
			if (this.mPulseUp)
			{
				Global.Pulse += 1.1999999f * elapsed;
				if (Global.Pulse >= 1f)
				{
					this.mPulseUp = false;
					return;
				}
			}
			else
			{
				Global.Pulse -= 1.1999999f * elapsed;
				if (Global.Pulse <= 0f)
				{
					this.mPulseUp = true;
				}
			}
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00044354 File Offset: 0x00042554
		private void Game1_Exiting(object sender, EventArgs e)
		{
			Terminal.TearDown();
			Global.GameEnded = true;
			NetworkManager.CloseAllConnections();
			SteamAPI.Shutdown();
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0004436B File Offset: 0x0004256B
		public void SetGameManager(GameManager manager)
		{
			this.GameManager = manager;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00044374 File Offset: 0x00042574
		public void ResetRenderStates()
		{
			base.GraphicsDevice.BlendState = BlendState.Additive;
			base.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00044398 File Offset: 0x00042598
		public static string CreateMd5ForFolder(string path)
		{
			List<string> list = (from p in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
			orderby p
			select p).ToList<string>();
			MD5 md = MD5.Create();
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i];
				if (!text.EndsWith("txt") && !text.EndsWith("chr"))
				{
					string text2 = text.Substring(path.Length + 1);
					byte[] bytes = Encoding.UTF8.GetBytes(text2.ToLower());
					md.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
					byte[] array = File.ReadAllBytes(text);
					if (i == list.Count - 1)
					{
						md.TransformFinalBlock(array, 0, array.Length);
					}
					else
					{
						md.TransformBlock(array, 0, array.Length, array, 0);
					}
				}
			}
			return BitConverter.ToString(md.Hash).Replace("-", "").ToLower();
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x000444A4 File Offset: 0x000426A4
		private void BUILD_CHECKSUM()
		{
			Stopwatch.StartNew();
			if (Game1.CreateMd5ForFolder("Data") != "ccfa35f31420bfd791aa4b7b91fc6e42")
			{
				Terminal.WriteMessage("INVALID DATA DETECTED", MessageType.ERROR);
				MessageBox.Show("Invalid game data detected! Try reinstalling!", "Error");
				SteamApps.MarkContentCorrupt(false);
				throw new Exception("Invalid data encountered.");
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x000444FA File Offset: 0x000426FA
		public bool GameMinimized
		{
			get
			{
				if (this.graphics.IsFullScreen)
				{
					return !base.IsActive;
				}
				return this.mForm.WindowState == FormWindowState.Minimized;
			}
		}

		// Token: 0x0600082C RID: 2092
		[DllImport("user32.dll")]
		private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

		// Token: 0x0600082D RID: 2093
		[DllImport("user32.dll")]
		private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		// Token: 0x0600082E RID: 2094
		[DllImport("user32.dll")]
		private static extern void ClipCursor(ref Rectangle rect);

		// Token: 0x0600082F RID: 2095 RVA: 0x00044524 File Offset: 0x00042724
		protected void ClipMouse()
		{
			if (base.IsActive && !this.GameMinimized && this.graphics.IsFullScreen)
			{
				Rectangle clientBounds = base.Window.ClientBounds;
				Game1.ClipCursor(ref clientBounds);
			}
		}

		// Token: 0x040008D9 RID: 2265
		private GraphicsDeviceManager graphics;

		// Token: 0x040008DA RID: 2266
		private SpriteBatch spriteBatch;

		// Token: 0x040008DB RID: 2267
		private TerminalScreen termScreen;

		// Token: 0x040008DC RID: 2268
		private Rectangle rect;

		// Token: 0x040008DD RID: 2269
		private float editorZoom = 1f;

		// Token: 0x040008DE RID: 2270
		public GameManager GameManager;

		// Token: 0x040008DF RID: 2271
		private bool mExiting;

		// Token: 0x040008E0 RID: 2272
		private AppId_t mAppID = new AppId_t(512490U);

		// Token: 0x040008E1 RID: 2273
		private bool mSteamInitialized;

		// Token: 0x040008E2 RID: 2274
		private Form mForm;

		// Token: 0x040008E3 RID: 2275
		public static SamplerState TESTState;

		// Token: 0x040008E4 RID: 2276
		private bool mPulseUp;

		// Token: 0x040008E5 RID: 2277
		internal const uint SC_CLOSE = 61536U;

		// Token: 0x040008E6 RID: 2278
		internal const uint MF_ENABLED = 0U;

		// Token: 0x040008E7 RID: 2279
		internal const uint MF_GRAYED = 1U;

		// Token: 0x040008E8 RID: 2280
		internal const uint MF_DISABLED = 2U;

		// Token: 0x040008E9 RID: 2281
		internal const uint MF_BYCOMMAND = 0U;
	}
}
