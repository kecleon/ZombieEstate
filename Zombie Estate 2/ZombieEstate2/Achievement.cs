using System;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200000C RID: 12
	public class Achievement
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002A22 File Offset: 0x00000C22
		public void Init()
		{
			if (this.assetName != "NULL")
			{
				this.icon = Global.Content.Load<Texture2D>("AchievementIcons//" + this.assetName);
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002A56 File Offset: 0x00000C56
		public bool IncrementPoints()
		{
			this.currentPoints++;
			if (this.currentPoints >= this.goalPoints)
			{
				this.completed = true;
				this.currentPoints = this.goalPoints;
			}
			return this.completed;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002A8D File Offset: 0x00000C8D
		public void Reset()
		{
			this.currentPoints = 0;
			this.completed = false;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002A9D File Offset: 0x00000C9D
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002AA5 File Offset: 0x00000CA5
		public bool Completed
		{
			get
			{
				return this.completed;
			}
			set
			{
				this.completed = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002AAE File Offset: 0x00000CAE
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002AB6 File Offset: 0x00000CB6
		public int GoalPoints
		{
			get
			{
				return this.goalPoints;
			}
			set
			{
				this.goalPoints = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002ABF File Offset: 0x00000CBF
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002AC7 File Offset: 0x00000CC7
		public int CurrentPoints
		{
			get
			{
				return this.currentPoints;
			}
			set
			{
				this.currentPoints = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002AD0 File Offset: 0x00000CD0
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public string DisplayName
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002AE1 File Offset: 0x00000CE1
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002AE9 File Offset: 0x00000CE9
		public string AssetName
		{
			get
			{
				return this.assetName;
			}
			set
			{
				this.assetName = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002AF2 File Offset: 0x00000CF2
		public string DisplayProgress
		{
			get
			{
				if (this.goalPoints == 1)
				{
					return " ";
				}
				return string.Format("{0} / {1}", this.CurrentPoints, this.GoalPoints);
			}
		}

		// Token: 0x0400001C RID: 28
		private int goalPoints = 1;

		// Token: 0x0400001D RID: 29
		private int currentPoints;

		// Token: 0x0400001E RID: 30
		private string name = "NULL";

		// Token: 0x0400001F RID: 31
		private bool completed;

		// Token: 0x04000020 RID: 32
		private string assetName = "NULL";

		// Token: 0x04000021 RID: 33
		private Texture2D icon;
	}
}
