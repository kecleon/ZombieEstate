using System;
using System.Globalization;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.HUD.XboxHUD;
using ZombieEstate2.StoreScreen.XboxStore;
using ZombieEstate2.UI.Menus;
using ZombieEstate2.Wave;

namespace ZombieEstate2.UI.Xbox
{
	// Token: 0x0200015E RID: 350
	public class StartLoadScreen
	{
		// Token: 0x06000A94 RID: 2708 RVA: 0x00056198 File Offset: 0x00054398
		public StartLoadScreen()
		{
			Rectangle pos = new Rectangle((int)Global.GetScreenCenter().X - 32, (int)Global.GetScreenCenter().Y - 32, 64, 64);
			this.mTextPos = VerchickMath.CenterText(Global.StoreFontXtraLarge, Global.GetScreenCenter(), this.mText);
			this.mTextPos.Y = this.mTextPos.Y - 96f;
			this.mIcon = new LoadIcon(pos);
			TipManager.Init();
			new Thread(new ThreadStart(this.Load)).Start();
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00056234 File Offset: 0x00054434
		private void Load()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			if (!StartLoadScreen.DONE)
			{
				XboxCharacterSelect.LoadGfx();
				XboxStore.LoadTex();
				XboxStatsHUD.LoadGfx();
				XboxHUD.LoadTex();
				XboxWaveHUD.LoadGfx();
				Global.Alphabet = Global.Content.Load<Texture2D>("Alphabet");
				Global.Alphabet2 = Global.Content.Load<Texture2D>("Alphabet2");
				Global.SplashScreen = Global.Content.Load<Texture2D>("SplashScreen");
				Global.Credits = Global.Content.Load<Texture2D>("Credits");
				XboxWaveStats.LoadGfx();
				HelpMenu.mTex = Global.Content.Load<Texture2D>("Instructions2");
				GunStatsLoader.LoadAllGunsInFolder();
				StatusEffectLoader.LoadStatusEffects();
				BulletCreator.LoadAllBulletsInFolder();
				PlayerStatKeeper.InitCharSettings();
				PCCharacterSelect.LoadCharacters();
			}
			ScreenFader.Fade(delegate()
			{
				StartLoadScreen.DONE = true;
			});
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00056319 File Offset: 0x00054519
		public void Update(float elapsed)
		{
			this.mIcon.Update(elapsed);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00056327 File Offset: 0x00054527
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Global.Pixel, Global.ScreenRect, Color.Black);
		}

		// Token: 0x04000B4E RID: 2894
		private LoadIcon mIcon;

		// Token: 0x04000B4F RID: 2895
		private string mText = "Loading...";

		// Token: 0x04000B50 RID: 2896
		private Vector2 mTextPos;

		// Token: 0x04000B51 RID: 2897
		public static bool DONE;
	}
}
