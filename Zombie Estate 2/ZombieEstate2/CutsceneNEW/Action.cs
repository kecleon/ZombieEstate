using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.CutsceneNEW
{
	// Token: 0x020001E0 RID: 480
	internal class Action : IComparable
	{
		// Token: 0x06000CBE RID: 3262 RVA: 0x00068ABD File Offset: 0x00066CBD
		public Action(float start, ActorObject parent)
		{
			this.StartTime = start;
			this.EndTime = start + 1f;
			this.MatchActor(parent);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00068AF4 File Offset: 0x00066CF4
		public bool UpdateActorObject(ActorObject parent)
		{
			float runTime = CutSceneController.RunTime;
			return this.UpdateActorObject(parent, runTime);
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00068B10 File Offset: 0x00066D10
		public bool UpdateActorObject(ActorObject parent, float runTime)
		{
			if (!this.WithinRunTime())
			{
				if (this.Active)
				{
					this.Active = false;
					if (runTime >= this.EndTime)
					{
						this.ApplyUpdates(parent, 1f);
					}
				}
				return false;
			}
			this.Active = true;
			if (this.EndTime - this.StartTime == 0f)
			{
				Terminal.WriteMessage("ERROR: DIVIDE BY ZERO IN UPDATEACTOROBJECT!", MessageType.ERROR);
				return false;
			}
			float delta = (runTime - this.StartTime) / (this.EndTime - this.StartTime);
			this.ApplyUpdates(parent, delta);
			return true;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00068B95 File Offset: 0x00066D95
		private void ApplyUpdates(ActorObject parent, float delta)
		{
			parent.Position = Vector3.Lerp(this.StartState.Position, this.EndState.Position, delta);
			parent.TextureCoord = this.TextureCoord;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00068BC5 File Offset: 0x00066DC5
		public void ApplyLatestUpdate(ActorObject parent)
		{
			this.ApplyUpdates(parent, 1f);
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00068BD3 File Offset: 0x00066DD3
		public void ApplyEarliestUpdate(ActorObject parent)
		{
			this.ApplyUpdates(parent, 0f);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00068BE4 File Offset: 0x00066DE4
		public int CompareTo(object obj)
		{
			Action action = obj as Action;
			if (action.EndTime < this.EndTime)
			{
				return 1;
			}
			if (action.EndTime > this.EndTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00068C19 File Offset: 0x00066E19
		public bool WithinRunTime()
		{
			return this.WithinTime(CutSceneController.RunTime);
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00068C28 File Offset: 0x00066E28
		public bool WithinTime(float time)
		{
			return this.StartTime < time && time <= this.EndTime;
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00068C4E File Offset: 0x00066E4E
		public void MatchStartEnd(Action action)
		{
			this.FromActionState(ref action.EndState, ref this.StartState);
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00068C62 File Offset: 0x00066E62
		private void MatchActor(ActorObject actor)
		{
			this.StartState = actor.GetCurrentState();
			this.EndState = actor.GetCurrentState();
			this.TextureCoord = actor.TextureCoord;
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00068C88 File Offset: 0x00066E88
		private void FromActionState(ref ActionState fromState, ref ActionState toState)
		{
			toState.Position = fromState.Position;
		}

		// Token: 0x04000D7E RID: 3454
		public float StartTime;

		// Token: 0x04000D7F RID: 3455
		public float EndTime;

		// Token: 0x04000D80 RID: 3456
		public bool Selected;

		// Token: 0x04000D81 RID: 3457
		public ActionState StartState;

		// Token: 0x04000D82 RID: 3458
		public ActionState EndState;

		// Token: 0x04000D83 RID: 3459
		public Point TextureCoord = new Point(0, 0);

		// Token: 0x04000D84 RID: 3460
		public bool Walking;

		// Token: 0x04000D85 RID: 3461
		public ParticleType SpawnParticleType;

		// Token: 0x04000D86 RID: 3462
		public int SpawnCount = 1;

		// Token: 0x04000D87 RID: 3463
		private bool Active;
	}
}
