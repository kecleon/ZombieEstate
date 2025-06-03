using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000010 RID: 16
	public class AnimationHandler
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00002CE0 File Offset: 0x00000EE0
		public AnimationHandler(AnimatedObjectIF parentObject)
		{
			this.animations = new List<Animation>();
			this.parent = parentObject;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002CFA File Offset: 0x00000EFA
		public void UpdateAnimations(float elapsed)
		{
			if (this.activeAnimation != null)
			{
				this.activeAnimation.UpdateAnimation(elapsed, this.parent);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002D16 File Offset: 0x00000F16
		public bool Playing()
		{
			return this.activeAnimation != null;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002D21 File Offset: 0x00000F21
		public bool OnLastFrame()
		{
			return this.activeAnimation.currentFrame == this.activeAnimation.frames.Count<Point>() - 1;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002D45 File Offset: 0x00000F45
		public bool AnimationPlaying(string label)
		{
			return this.activeAnimation != null && this.activeAnimation.GetLabel() == label;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002D64 File Offset: 0x00000F64
		public void PlayAnimation(string label)
		{
			if (this.activeAnimation != null && this.activeAnimation.GetLabel() == label)
			{
				return;
			}
			Animation animation = this.getAnimation(label);
			animation.ResetAnimation();
			this.activeAnimation = animation;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002DA2 File Offset: 0x00000FA2
		public void AddAnimation(List<Point> points, string label)
		{
			this.animations.Add(new Animation(points, label));
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002DB6 File Offset: 0x00000FB6
		public void AddAnimation(List<Point> points, string label, float speed)
		{
			this.animations.Add(new Animation(points, label, speed));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002DCC File Offset: 0x00000FCC
		public void AddLineAnimation(Point point, int length, string label, float speed)
		{
			List<Point> list = new List<Point>();
			for (int i = 0; i < length; i++)
			{
				list.Add(new Point(point.X + i, point.Y));
			}
			this.animations.Add(new Animation(list, label, speed));
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002E18 File Offset: 0x00001018
		public void AddLineAnimation(Point point, int length, string label)
		{
			List<Point> list = new List<Point>();
			for (int i = 0; i < length; i++)
			{
				list.Add(new Point(point.X + i, point.Y));
			}
			this.animations.Add(new Animation(list, label));
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002E64 File Offset: 0x00001064
		private Animation getAnimation(string label)
		{
			for (int i = 0; i < this.animations.Count; i++)
			{
				if (this.animations[i].GetLabel() == label)
				{
					return this.animations[i];
				}
			}
			Terminal.WriteMessage("ERROR: getAnimation found no match to label: '" + label + "'", MessageType.ERROR);
			if (AnimationHandler.THROWEXCEPTION)
			{
				throw new Exception("ERROR: getAnimation found no match to label: '" + label + "'");
			}
			return this.animations[0];
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002EEC File Offset: 0x000010EC
		public Point GetTexCoord()
		{
			return this.activeAnimation.GetCurrentFrame();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void ReplayAnimation()
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002EFB File Offset: 0x000010FB
		public void PauseAnimation()
		{
			if (this.activeAnimation != null)
			{
				this.activeAnimation.Paused = true;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002F11 File Offset: 0x00001111
		public void UnPauseAnimation()
		{
			if (this.activeAnimation != null)
			{
				this.activeAnimation.Paused = false;
			}
		}

		// Token: 0x0400002A RID: 42
		private List<Animation> animations;

		// Token: 0x0400002B RID: 43
		private Animation activeAnimation;

		// Token: 0x0400002C RID: 44
		private AnimatedObjectIF parent;

		// Token: 0x0400002D RID: 45
		private static bool THROWEXCEPTION;
	}
}
