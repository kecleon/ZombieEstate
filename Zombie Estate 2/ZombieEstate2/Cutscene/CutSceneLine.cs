using System;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.Cutscene
{
	// Token: 0x020001D8 RID: 472
	public class CutSceneLine
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000C91 RID: 3217 RVA: 0x00067EE4 File Offset: 0x000660E4
		// (remove) Token: 0x06000C92 RID: 3218 RVA: 0x00067F1C File Offset: 0x0006611C
		public event EventHandler OnFinish;

		// Token: 0x06000C94 RID: 3220 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Run()
		{
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00067F51 File Offset: 0x00066151
		public virtual void Update(float elapsed)
		{
			if (this.Duration != null && this.Duration.Expired() && this.OnFinish != null)
			{
				this.OnFinish(this, null);
			}
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Draw(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void SetArguments(object[] args)
		{
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0002036B File Offset: 0x0001E56B
		public virtual object[] GetArguments()
		{
			return null;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0002036B File Offset: 0x0001E56B
		public virtual string[] GetArgumentDescriptions()
		{
			return null;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x00067F7D File Offset: 0x0006617D
		public void TerminateLine()
		{
			if (this.OnFinish != null)
			{
				this.OnFinish(this, null);
			}
		}

		// Token: 0x04000D5B RID: 3419
		public Timer Duration;

		// Token: 0x04000D5D RID: 3421
		public bool BlockingLine;
	}
}
