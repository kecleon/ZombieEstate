using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000F0 RID: 240
	public class EndWaveList
	{
		// Token: 0x0600065A RID: 1626 RVA: 0x0002F3A3 File Offset: 0x0002D5A3
		public EndWaveList(PlayerDataBeforeWave pre, PlayerDataBeforeWave post)
		{
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0002F3CC File Offset: 0x0002D5CC
		public EndWaveList(List<DataString> data, int index)
		{
			this.timer.IndependentOfTime = true;
			this.timer.Start();
			this.DataStrings = data;
			PlayerInfo player = PlayerManager.GetPlayer(index);
			if (player != null)
			{
				this.mPlayer = player.PlayerObject;
			}
			int num = 0;
			foreach (DataString dataString in this.DataStrings)
			{
				Vector2 dataPos = this.GetDataPos(index);
				dataPos.Y += (float)(num * 32);
				dataString.SetPosition(dataPos);
				dataString.Visible = false;
				num++;
			}
			if (data.Count == 0)
			{
				this.Running = false;
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0002F4AC File Offset: 0x0002D6AC
		public void Update()
		{
			if (!this.Running)
			{
				return;
			}
			this.Running = (Global.EndListsDone != Global.PlayerList.Count);
			if (this.ImDone)
			{
				return;
			}
			foreach (DataString dataString in this.DataStrings)
			{
				dataString.Update();
			}
			if (this.timer.Expired())
			{
				this.index++;
				if (this.index >= this.DataStrings.Count)
				{
					Global.EndListsDone++;
					this.ImDone = true;
					return;
				}
				this.DataStrings[this.index].Visible = true;
				this.DataStrings[this.index].Flash(Color.White);
				if (this.index == this.DataStrings.Count - 1)
				{
					this.timer.mTotalTime = 2f;
				}
				this.timer.Reset();
				this.timer.Start();
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0002F5DC File Offset: 0x0002D7DC
		public void Draw(SpriteBatch spriteBatch)
		{
			if (MasterStore.Active)
			{
				return;
			}
			if (this.mPlayer != null && this.mPlayer.XboxHUD.mStatsMode)
			{
				return;
			}
			foreach (DataString dataString in this.DataStrings)
			{
				dataString.Draw(spriteBatch);
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0002F650 File Offset: 0x0002D850
		private Vector2 GetDataPos(int index)
		{
			Global.GetSafeScreenArea();
			Player player = this.mPlayer;
			if (player == null)
			{
				return new Vector2(1000f, 1000f);
			}
			if (index == 0 || index == 2)
			{
				return new Vector2(player.XboxHUD.mPosition.X + 200f, player.XboxHUD.mPosition.Y - 32f);
			}
			return new Vector2(player.XboxHUD.mPosition.X - 248f, player.XboxHUD.mPosition.Y - 32f);
		}

		// Token: 0x0400062C RID: 1580
		private List<DataString> DataStrings;

		// Token: 0x0400062D RID: 1581
		private PlayerDataBeforeWave Pre;

		// Token: 0x0400062E RID: 1582
		private PlayerDataBeforeWave Post;

		// Token: 0x0400062F RID: 1583
		private Timer timer = new Timer(0.5f);

		// Token: 0x04000630 RID: 1584
		private int index = -1;

		// Token: 0x04000631 RID: 1585
		public bool Running = true;

		// Token: 0x04000632 RID: 1586
		private bool ImDone;

		// Token: 0x04000633 RID: 1587
		private Player mPlayer;
	}
}
