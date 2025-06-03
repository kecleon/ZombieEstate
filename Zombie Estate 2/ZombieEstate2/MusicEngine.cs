using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Media;

namespace ZombieEstate2
{
	// Token: 0x0200009B RID: 155
	public static class MusicEngine
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x0001D4D4 File Offset: 0x0001B6D4
		public static void Init()
		{
			try
			{
				MusicEngine.mSongs = new Dictionary<ZE2Songs, Song>();
				MusicEngine.mSongs.Add(ZE2Songs.AllDead, Global.Content.Load<Song>("Music\\ze2_alldead"));
				MusicEngine.mSongs.Add(ZE2Songs.Wave, Global.Content.Load<Song>("Music\\ze2_betweenwave"));
				MusicEngine.mSongs.Add(ZE2Songs.Victory, Global.Content.Load<Song>("Music\\ze2_credits"));
				MusicEngine.mSongs.Add(ZE2Songs.Estate, Global.Content.Load<Song>("Music\\ze2_estate"));
				MusicEngine.mSongs.Add(ZE2Songs.Office, Global.Content.Load<Song>("Music\\ze2_office"));
				MusicEngine.mSongs.Add(ZE2Songs.Desert, Global.Content.Load<Song>("Music\\ze2_desert"));
				MusicEngine.mSongs.Add(ZE2Songs.Skyscraper, Global.Content.Load<Song>("Music\\ze2_skyscraper"));
				MusicEngine.mSongs.Add(ZE2Songs.Mall, Global.Content.Load<Song>("Music\\ze2_mall"));
				MusicEngine.mSongs.Add(ZE2Songs.School, Global.Content.Load<Song>("Music\\ze2_highschool"));
				MusicEngine.mSongs.Add(ZE2Songs.Intro, Global.Content.Load<Song>("Music\\ze2_intro"));
				MusicEngine.mSongs.Add(ZE2Songs.Store, Global.Content.Load<Song>("Music\\ze2_store"));
				MusicEngine.mSongs.Add(ZE2Songs.WinWave, Global.Content.Load<Song>("Music\\ze2_beatwave"));
				MediaPlayer.IsRepeating = true;
				MusicEngine.Play(ZE2Songs.Intro);
			}
			catch (Exception)
			{
				MusicEngine.CRASH_MUSIC_DISABLED = true;
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001D65C File Offset: 0x0001B85C
		public static void LowerVol()
		{
			MusicEngine.mVolume = MusicEngine.MINVOLUME;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001D668 File Offset: 0x0001B868
		public static void RaiseVol()
		{
			MusicEngine.mVolume = MusicEngine.VOLUME;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001D674 File Offset: 0x0001B874
		public static void Play(ZE2Songs song)
		{
			if (song != MusicEngine.mCurrentSong)
			{
				MusicEngine.mState = MusicEngine.State.FadingOut;
				MusicEngine.mNextSong = MusicEngine.mSongs[song];
				MusicEngine.mCurrentSong = song;
			}
			if (song == ZE2Songs.WinWave)
			{
				MediaPlayer.IsRepeating = false;
				return;
			}
			MediaPlayer.IsRepeating = true;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001D6AC File Offset: 0x0001B8AC
		public static void Update()
		{
			if (MusicEngine.CRASH_MUSIC_DISABLED)
			{
				return;
			}
			switch (MusicEngine.mState)
			{
			case MusicEngine.State.FadingIn:
				MusicEngine.mFade += MusicEngine.FadeRate;
				if (MusicEngine.mFade >= 1f)
				{
					MusicEngine.mState = MusicEngine.State.Playing;
					MusicEngine.mFade = 1f;
					goto IL_E8;
				}
				goto IL_E8;
			case MusicEngine.State.FadingOut:
				MusicEngine.mFade -= MusicEngine.FadeRate;
				if (MusicEngine.mFade > 0f)
				{
					goto IL_E8;
				}
				MusicEngine.mState = MusicEngine.State.FadingIn;
				MusicEngine.mFade = 0f;
				try
				{
					MediaPlayer.Play(MusicEngine.mNextSong);
					goto IL_E8;
				}
				catch (Exception)
				{
					bool isFullScreen = Global.Graphics.IsFullScreen;
					if (isFullScreen)
					{
						Global.Graphics.ToggleFullScreen();
					}
					if (MessageBox.Show("An error occurred playing Zombie Estate 2's music. Please ensure Windows Media Player is enabled. \nClick start, type \"Programs and Features\", then on the left should be \"Turn Windows Features On or Off\", click that, then check to see if there is a check mark under \"Media Features -> Windows Media Player\".\n\nSee the Steam Discussions Board for more details. This will be cleaned up in a future update.\n\nClick OK to play with music disabled or Cancel to quit.", "Error", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						MusicEngine.CRASH_MUSIC_DISABLED = true;
						if (isFullScreen)
						{
							Global.Graphics.ToggleFullScreen();
						}
						return;
					}
					Global.Game.Exit();
					goto IL_E8;
				}
				break;
			case MusicEngine.State.Playing:
				break;
			default:
				goto IL_E8;
			}
			MusicEngine.mFade = 1f;
			IL_E8:
			if (MusicEngine.MUSIC_MUTED)
			{
				MediaPlayer.Volume = 0f;
				return;
			}
			MediaPlayer.Volume = MusicEngine.mFade * 0.5f * ((float)Config.Instance.MusicVolume / 100f);
		}

		// Token: 0x040003D7 RID: 983
		private static Dictionary<ZE2Songs, Song> mSongs;

		// Token: 0x040003D8 RID: 984
		private static float mFade = 0f;

		// Token: 0x040003D9 RID: 985
		private static float mVolume = 0.5f;

		// Token: 0x040003DA RID: 986
		private static MusicEngine.State mState = MusicEngine.State.FadingIn;

		// Token: 0x040003DB RID: 987
		private static ZE2Songs mCurrentSong = ZE2Songs.Wave;

		// Token: 0x040003DC RID: 988
		private static Song mNextSong;

		// Token: 0x040003DD RID: 989
		private static float VOLUME = 0.5f;

		// Token: 0x040003DE RID: 990
		private static float MINVOLUME = 0.5f;

		// Token: 0x040003DF RID: 991
		public static bool MUSIC_MUTED = false;

		// Token: 0x040003E0 RID: 992
		private static bool CRASH_MUSIC_DISABLED = false;

		// Token: 0x040003E1 RID: 993
		private static float FadeRate = 0.01f;

		// Token: 0x0200020C RID: 524
		private enum State
		{
			// Token: 0x04000E0D RID: 3597
			FadingIn,
			// Token: 0x04000E0E RID: 3598
			FadingOut,
			// Token: 0x04000E0F RID: 3599
			Playing
		}
	}
}
