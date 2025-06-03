using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.CutsceneNEW
{
	// Token: 0x020001E2 RID: 482
	internal class ActorObject : DragableObject
	{
		// Token: 0x06000CCE RID: 3278 RVA: 0x00068F1C File Offset: 0x0006711C
		public ActorObject()
		{
			this.TextureCoord = new Point(0, 0);
			this.scale = 0.4f;
			this.Position = new Vector3(10f, this.floorHeight, 10f);
			this.ActivateObject(this.Position, this.TextureCoord);
			this.Actions = new List<Action>();
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00068F80 File Offset: 0x00067180
		public override void Update(float elapsed)
		{
			if (!this.LockedToMouse)
			{
				this.UpdateActions();
			}
			this.UpdateBouncing();
			this.UpdateWaypoints();
			Action currentAction = this.GetCurrentAction();
			if (currentAction != null)
			{
				this.TextureCoord = currentAction.TextureCoord;
				if (this.TextureCoord == new Point(63, 63) && CutSceneController.EDITING)
				{
					this.TextureCoord = new Point(62, 63);
				}
			}
			this.SpawnParticles(elapsed);
			base.Update(elapsed);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00068FF8 File Offset: 0x000671F8
		private void UpdateBouncing()
		{
			if (!CutSceneController.EDITING)
			{
				Action currentAction = this.GetCurrentAction();
				if (currentAction != null)
				{
					if (currentAction.Walking)
					{
						this.BounceEnabled = true;
						return;
					}
					this.BounceEnabled = false;
					return;
				}
				else
				{
					this.BounceEnabled = false;
				}
			}
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00069038 File Offset: 0x00067238
		private void UpdateActions()
		{
			bool flag = false;
			using (List<Action>.Enumerator enumerator = this.Actions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.UpdateActorObject(this))
					{
						flag = true;
					}
				}
			}
			if (!flag && this.Actions.Count > 0)
			{
				Action latestAction = this.GetLatestAction(CutSceneController.RunTime);
				if (latestAction == null)
				{
					this.Actions[0].ApplyEarliestUpdate(this);
					return;
				}
				latestAction.ApplyLatestUpdate(this);
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x000690CC File Offset: 0x000672CC
		private Action GetLatestAction(float currentTime)
		{
			Action result = null;
			this.Actions.Sort();
			foreach (Action action in this.Actions)
			{
				if (action.EndTime <= currentTime)
				{
					result = action;
				}
			}
			return result;
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00069134 File Offset: 0x00067334
		public override void Placed()
		{
			Action currentAction = this.GetCurrentAction();
			if (currentAction == null)
			{
				return;
			}
			currentAction.EndState.Position = this.Position;
			CutSceneController.RunTime = currentAction.EndTime;
			currentAction.ApplyLatestUpdate(this);
			base.Placed();
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00069178 File Offset: 0x00067378
		private Action GetCurrentAction()
		{
			foreach (Action action in this.Actions)
			{
				if (action.WithinRunTime())
				{
					return action;
				}
			}
			return null;
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x000691D4 File Offset: 0x000673D4
		public void MatchStartPos(Action action)
		{
			int num = this.Actions.IndexOf(action);
			if (num == 0)
			{
				return;
			}
			action.MatchStartEnd(this.Actions[num - 1]);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00069208 File Offset: 0x00067408
		public ActionState GetCurrentState()
		{
			return new ActionState
			{
				Position = this.Position
			};
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0006922C File Offset: 0x0006742C
		private void UpdateWaypoints()
		{
			ActorObject.ClearWaypoints();
			if (!this.Selected)
			{
				return;
			}
			foreach (Action action in this.Actions)
			{
				this.CreateWaypoint(action.StartState.Position);
				this.CreateWaypoint(action.EndState.Position);
			}
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x000692A8 File Offset: 0x000674A8
		private void CreateWaypoint(Vector3 pos)
		{
			GameObject gameObject = new GameObject();
			gameObject.ActivateObject(pos, new Point(7, 32));
			gameObject.TextureCoord = new Point(7, 32);
			gameObject.scale = 0.25f;
			gameObject.XRotation = -1.5707964f;
			CutSceneController.CutSceneCache.CreateObject(gameObject);
			ActorObject.WayPoints.Add(gameObject);
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00069308 File Offset: 0x00067508
		public static void ClearWaypoints()
		{
			foreach (GameObject gameObject in ActorObject.WayPoints)
			{
				gameObject.DestroyObject();
			}
			ActorObject.WayPoints.Clear();
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00069364 File Offset: 0x00067564
		private void SpawnParticles(float elapsed)
		{
			if (CutSceneController.EDITING)
			{
				return;
			}
			Action currentAction = this.GetCurrentAction();
			if (currentAction == null)
			{
				return;
			}
			if (currentAction.SpawnParticleType == ParticleType.None)
			{
				return;
			}
			this.particleSpawnTime += elapsed;
			if (this.particleSpawnTime >= 1f / (float)(currentAction.SpawnCount * 4))
			{
				this.particleSpawnTime = 0f;
				CutSceneController.CutSceneCache.CreateParticle(currentAction.SpawnParticleType, this.Position, Vector3.Zero);
			}
		}

		// Token: 0x04000D8D RID: 3469
		public List<Action> Actions;

		// Token: 0x04000D8E RID: 3470
		public static List<GameObject> WayPoints = new List<GameObject>();

		// Token: 0x04000D8F RID: 3471
		private float particleSpawnTime;
	}
}
