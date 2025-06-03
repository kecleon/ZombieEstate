using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200000F RID: 15
	internal class Animation
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00002BAE File Offset: 0x00000DAE
		public Animation(List<Point> points, string label)
		{
			this.frames = points;
			this.label = label;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002BCF File Offset: 0x00000DCF
		public Animation(List<Point> points, string label, float speed)
		{
			this.speed = speed;
			this.frames = points;
			this.label = label;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002BF7 File Offset: 0x00000DF7
		public void UpdateAnimation(float gameTime, AnimatedObjectIF parent)
		{
			this.timeElapsed += gameTime;
			if (this.timeElapsed >= this.speed)
			{
				this.NextFrame(parent);
				this.timeElapsed = 0f;
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002C27 File Offset: 0x00000E27
		public void ResetAnimation()
		{
			this.currentFrame = 0;
			this.Paused = false;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002C37 File Offset: 0x00000E37
		private void NextFrame(AnimatedObjectIF parent)
		{
			this.currentFrame++;
			if (this.currentFrame >= this.frames.Count)
			{
				this.currentFrame = 0;
				parent.AnimationFinished(this.label);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002C70 File Offset: 0x00000E70
		public Point GetCurrentFrame()
		{
			if (this.Paused)
			{
				return this.frames[0];
			}
			if (this.currentFrame >= this.frames.Count)
			{
				return this.frames[this.frames.Count - 1];
			}
			return this.frames[this.currentFrame];
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002CCF File Offset: 0x00000ECF
		public string GetLabel()
		{
			return this.label;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002CD7 File Offset: 0x00000ED7
		public void SetLabel(string value)
		{
			this.label = value;
		}

		// Token: 0x04000024 RID: 36
		private string label;

		// Token: 0x04000025 RID: 37
		public List<Point> frames;

		// Token: 0x04000026 RID: 38
		private float timeElapsed;

		// Token: 0x04000027 RID: 39
		private float speed = 0.2f;

		// Token: 0x04000028 RID: 40
		public int currentFrame;

		// Token: 0x04000029 RID: 41
		public bool Paused;
	}
}
