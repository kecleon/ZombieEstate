using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.UI;

namespace ZombieEstate2.Wave
{
	// Token: 0x02000158 RID: 344
	public class XboxWaveStats
	{
		// Token: 0x06000A69 RID: 2665 RVA: 0x00054720 File Offset: 0x00052920
		public XboxWaveStats(Player player)
		{
			this.mPlayer = player;
			this.mEndList = new EndWaveList(this.mPlayer.DataStrings, this.mPlayer.Index);
			this.SetUpTopLeft();
			this.mLocLabel = Vector2.Add(new Vector2(158f, 64f), this.mTopLeft);
			this.mLocWave = Vector2.Add(new Vector2(318f, 24f), this.mTopLeft);
			this.mLocDmgBadge = Vector2.Add(new Vector2(29f, 210f), this.mTopLeft);
			this.mLocMinBadge = Vector2.Add(new Vector2(112f, 210f), this.mTopLeft);
			this.mLocHealBadge = Vector2.Add(new Vector2(29f, 240f), this.mTopLeft);
			this.mLocTankBadge = Vector2.Add(new Vector2(112f, 240f), this.mTopLeft);
			this.mLocWave = VerchickMath.CenterText(Global.StoreFontBig, this.mLocWave, XboxWaveStats.mWaveStats);
			this.mPlayerRect = new Rectangle((int)this.mTopLeft.X + 9, (int)this.mTopLeft.Y + 9, 128, 128);
			this.mPlayerSrc = Global.GetTexRectange(this.mPlayer.StartTextureCoord.X, this.mPlayer.StartTextureCoord.Y);
			XboxWaveStats.LoadGfx();
			this.mDamageDealt = this.mPlayer.Talleys.DamageDealt.ToString("0");
			this.mDamageTaken = this.mPlayer.Talleys.DamageTaken.ToString("0");
			this.mHealingDone = this.mPlayer.Talleys.HealingDone.ToString("0");
			this.mMinionDamageDealt = this.mPlayer.Talleys.MinionDamageDealt.ToString("0");
			this.mDmgBadge = this.mPlayer.GameTalleys.DmgBadges.ToString();
			this.mHealBadge = this.mPlayer.GameTalleys.HealBadges.ToString();
			this.mMinBadge = this.mPlayer.GameTalleys.MinionDmgBadges.ToString();
			this.mTankBadge = this.mPlayer.GameTalleys.TankBadges.ToString();
			this.mContinueButton = new ZEButton(new Rectangle((int)this.mTopLeft.X + 221, (int)this.mTopLeft.Y + 231, 200, 40), "Continue", ButtonPress.Affirmative, this.mPlayer.Index, ZEButton.POSITIVE_COLOR);
			this.mContinueButton.OnPressed += delegate(object s, EventArgs e)
			{
				if (this.mEnded)
				{
					this.Active = !this.Active;
					this.mPlayer.LockInput.Reset();
					this.mPlayer.LockInput.Start();
				}
			};
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00054A10 File Offset: 0x00052C10
		public static void LoadGfx()
		{
			if (XboxWaveStats.mBG == null)
			{
				XboxWaveStats.mBG = Global.Content.Load<Texture2D>("XboxStore//WaveStats");
				XboxWaveStats.mIcons = Global.Content.Load<Texture2D>("XboxStore//WaveIcons");
				XboxWaveStats.mReady = Global.Content.Load<Texture2D>("XboxStore//WaveReady");
				XboxWaveStats.mList = Global.Content.Load<Texture2D>("XboxStore//WaveList");
			}
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00054A74 File Offset: 0x00052C74
		public void Update()
		{
			if (this.mEndList.Running)
			{
				this.mEndList.Update();
				return;
			}
			if (!this.mEnded)
			{
				if (!Global.WaveMaster.FinalWaveComplete && !MasterStore.Active)
				{
					MusicEngine.Play(ZE2Songs.Wave);
				}
				this.mEnded = true;
			}
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00054AC3 File Offset: 0x00052CC3
		public void Draw(SpriteBatch spriteBatch)
		{
			this.mEndList.Draw(spriteBatch);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00054AD4 File Offset: 0x00052CD4
		private void SetUpTopLeft()
		{
			Rectangle safeScreenArea = Global.GetSafeScreenArea();
			if (this.mPlayer.Index == 0)
			{
				this.mTopLeft = new Vector2((float)safeScreenArea.X, (float)safeScreenArea.Y);
				return;
			}
			if (this.mPlayer.Index == 1)
			{
				this.mTopLeft = new Vector2((float)(safeScreenArea.X + safeScreenArea.Width - 500), (float)safeScreenArea.Y);
				return;
			}
			if (this.mPlayer.Index == 2)
			{
				this.mTopLeft = new Vector2((float)safeScreenArea.X, (float)(safeScreenArea.Y + safeScreenArea.Height - 280));
				return;
			}
			if (this.mPlayer.Index == 3)
			{
				this.mTopLeft = new Vector2((float)(safeScreenArea.X + safeScreenArea.Width - 500), (float)(safeScreenArea.Y + safeScreenArea.Height - 280));
			}
		}

		// Token: 0x04000B06 RID: 2822
		private Vector2 mTopLeft = new Vector2(320f, 320f);

		// Token: 0x04000B07 RID: 2823
		private Player mPlayer;

		// Token: 0x04000B08 RID: 2824
		private static Texture2D mBG;

		// Token: 0x04000B09 RID: 2825
		private static Texture2D mIcons;

		// Token: 0x04000B0A RID: 2826
		private static Texture2D mList;

		// Token: 0x04000B0B RID: 2827
		private static Texture2D mReady;

		// Token: 0x04000B0C RID: 2828
		private Vector2 mLocLabel;

		// Token: 0x04000B0D RID: 2829
		private Vector2 mLocWave;

		// Token: 0x04000B0E RID: 2830
		private Rectangle mPlayerRect;

		// Token: 0x04000B0F RID: 2831
		private Rectangle mPlayerSrc;

		// Token: 0x04000B10 RID: 2832
		private Vector2 mLocDmgBadge;

		// Token: 0x04000B11 RID: 2833
		private Vector2 mLocMinBadge;

		// Token: 0x04000B12 RID: 2834
		private Vector2 mLocHealBadge;

		// Token: 0x04000B13 RID: 2835
		private Vector2 mLocTankBadge;

		// Token: 0x04000B14 RID: 2836
		private EndWaveList mEndList;

		// Token: 0x04000B15 RID: 2837
		private ZEButton mContinueButton;

		// Token: 0x04000B16 RID: 2838
		private static string mLabelDamageDealt = "Damage Dealt:";

		// Token: 0x04000B17 RID: 2839
		private static string mLabelMinionDamageDealt = "Minion Damage:";

		// Token: 0x04000B18 RID: 2840
		private static string mLabelHealingDone = "Healing Done:";

		// Token: 0x04000B19 RID: 2841
		private static string mLabelDamageTaken = "Damage Taken:";

		// Token: 0x04000B1A RID: 2842
		private static string mWaveStats = "Wave Stats";

		// Token: 0x04000B1B RID: 2843
		private string mDamageDealt;

		// Token: 0x04000B1C RID: 2844
		private string mMinionDamageDealt;

		// Token: 0x04000B1D RID: 2845
		private string mHealingDone;

		// Token: 0x04000B1E RID: 2846
		private string mDamageTaken;

		// Token: 0x04000B1F RID: 2847
		private string mHealBadge;

		// Token: 0x04000B20 RID: 2848
		private string mMinBadge;

		// Token: 0x04000B21 RID: 2849
		private string mDmgBadge;

		// Token: 0x04000B22 RID: 2850
		private string mTankBadge;

		// Token: 0x04000B23 RID: 2851
		public bool Active = true;

		// Token: 0x04000B24 RID: 2852
		private bool mEnded;
	}
}
