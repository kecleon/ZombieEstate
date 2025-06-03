using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieEstate2.CutsceneNEW;

namespace ZombieEstate2
{
	// Token: 0x02000017 RID: 23
	internal class CutSceneController
	{
		// Token: 0x06000091 RID: 145 RVA: 0x000049BC File Offset: 0x00002BBC
		public static void Init()
		{
			CutSceneController.RunTime = 0f;
			if (CutSceneController.CutSceneCache == null)
			{
				CutSceneController.CutSceneCache = new MasterCache(Global.Game, 1, 2500, false, "CutSceneController_MasterCache");
			}
			ActorObject obj = new ActorObject();
			CutSceneController.CutSceneCache.CreateObject(obj);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004A08 File Offset: 0x00002C08
		public static void UpdateCutScene(float elapsed)
		{
			if (!CutSceneController.Active)
			{
				return;
			}
			if (!CutSceneController.EDITING)
			{
				CutSceneController.RunTime += elapsed;
			}
			if (InputManager.ButtonPressed(Keys.P, 0))
			{
				if (!CutSceneController.EDITING)
				{
					CutSceneController.EDITING = true;
				}
				else
				{
					CutSceneController.EDITING = false;
					if (!InputManager.ButtonHeld(Keys.LeftShift, 0))
					{
						CutSceneController.RunTime = 0f;
					}
				}
			}
			CutSceneController.CutSceneCache.UpdateCaches(elapsed);
			if (!CutSceneController.OutOfBounds() && !CutSceneController.UpdateMoveTime() && !CutSceneController.Clicking)
			{
				CutSceneController.UpdateStartState();
				CutSceneController.UpdateActiveAction();
				CutSceneController.UpdateSelectionDragCheck();
				CutSceneController.UpdateMoveAction();
				CutSceneController.UpdateAddRemoveAction();
			}
			CutSceneController.UpdateActionEditor();
			CutSceneController.prevMouseState = Mouse.GetState();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004AB0 File Offset: 0x00002CB0
		private static void UpdateSelectionDragCheck()
		{
			if (CutSceneController.Clicking)
			{
				return;
			}
			bool flag = false;
			if (Mouse.GetState().LeftButton == ButtonState.Pressed && CutSceneController.prevMouseState.LeftButton == ButtonState.Released)
			{
				for (int i = 0; i < CutSceneController.CutSceneCache.gameObjectCaches[0].gameObjects.Count; i++)
				{
					DragableObject dragableObject = CutSceneController.CutSceneCache.gameObjectCaches[0].gameObjects[i] as DragableObject;
					if (dragableObject != null)
					{
						flag = dragableObject.CheckSelected(MouseHandler.GetPickedPosition());
						if (flag)
						{
							CutSceneController.SelectedActor = (dragableObject as ActorObject);
						}
					}
				}
				if (!flag)
				{
					if (CutSceneController.SelectedActor != null)
					{
						CutSceneController.SelectedActor.Selected = false;
						ActorObject.ClearWaypoints();
						CutSceneController.SelectedActor = null;
					}
					if (DragableObject.Selection != null)
					{
						DragableObject.Selection.DestroyObject();
						DragableObject.Selection = null;
					}
				}
			}
			if (Mouse.GetState().RightButton == ButtonState.Pressed && !CutSceneController.Clicking)
			{
				foreach (GameObject gameObject in CutSceneController.CutSceneCache.gameObjectCaches[0].gameObjects)
				{
					DragableObject dragableObject2 = gameObject as DragableObject;
					if (dragableObject2 != null && dragableObject2.CheckClicked(MouseHandler.GetPickedPosition()))
					{
						CutSceneController.Clicking = true;
						break;
					}
				}
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004C0C File Offset: 0x00002E0C
		public static void DrawCutScene(SpriteBatch spriteBatch)
		{
			if (!CutSceneController.Active)
			{
				return;
			}
			CutSceneController.CutSceneCache.DrawObjects();
			CutSceneController.DrawTimeLine(spriteBatch);
			if (CutSceneController.actionEditor != null)
			{
				CutSceneController.actionEditor.Draw(spriteBatch);
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004C38 File Offset: 0x00002E38
		private static void DrawTimeLine(SpriteBatch spriteBatch)
		{
			Color darkSlateBlue = Color.DarkSlateBlue;
			spriteBatch.Draw(Global.Pixel, CutSceneController.TimeLineBounds, darkSlateBlue);
			CutSceneController.DrawActions(spriteBatch);
			spriteBatch.Draw(Global.MasterTexture, CutSceneController.ComputeActionLocation(CutSceneController.RunTime), new Rectangle?(CutSceneController.TimePasser), Color.White);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004C88 File Offset: 0x00002E88
		private static void DrawActions(SpriteBatch spriteBatch)
		{
			if (CutSceneController.SelectedActor == null)
			{
				return;
			}
			foreach (ZombieEstate2.CutsceneNEW.Action action in CutSceneController.SelectedActor.Actions)
			{
				if (action.Selected)
				{
					spriteBatch.Draw(Global.MasterTexture, CutSceneController.ComputeActionLocation(action.StartTime), new Rectangle?(CutSceneController.LeftBracketSelected), Color.White);
					spriteBatch.Draw(Global.MasterTexture, CutSceneController.ComputeActionLocation(action.EndTime), new Rectangle?(CutSceneController.RightBracketSelected), Color.White);
				}
				else
				{
					spriteBatch.Draw(Global.MasterTexture, CutSceneController.ComputeActionLocation(action.StartTime), new Rectangle?(CutSceneController.LeftBracket), Color.White);
					spriteBatch.Draw(Global.MasterTexture, CutSceneController.ComputeActionLocation(action.EndTime), new Rectangle?(CutSceneController.RightBracket), Color.White);
				}
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004D84 File Offset: 0x00002F84
		private static Rectangle ComputeActionLocation(float startTime)
		{
			return new Rectangle((int)(startTime / CutSceneController.TotalTime * (float)CutSceneController.WIDTH) + CutSceneController.XOFFSET, CutSceneController.YOFFSET, 16, 16);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004DA9 File Offset: 0x00002FA9
		private static float ComputeDelta(int x)
		{
			x -= CutSceneController.XOFFSET;
			return (float)x / (float)CutSceneController.WIDTH;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004DC0 File Offset: 0x00002FC0
		private static bool UpdateMoveTime()
		{
			if (!CutSceneController.CHANGINGTIME)
			{
				if (Mouse.GetState().LeftButton == ButtonState.Pressed)
				{
					int x = Mouse.GetState().X;
					int y = Mouse.GetState().Y;
					if (CutSceneController.TimeLineBounds.Contains(x, y))
					{
						CutSceneController.RunTime = CutSceneController.ComputeDelta(x) * CutSceneController.TotalTime;
						CutSceneController.RunTime = MathHelper.Clamp(CutSceneController.RunTime, 0f, CutSceneController.TotalTime);
						CutSceneController.CHANGINGTIME = true;
						return true;
					}
				}
				return false;
			}
			if (Mouse.GetState().LeftButton == ButtonState.Released)
			{
				CutSceneController.CHANGINGTIME = false;
				return true;
			}
			CutSceneController.RunTime = CutSceneController.ComputeDelta(Mouse.GetState().X) * CutSceneController.TotalTime;
			CutSceneController.RunTime = MathHelper.Clamp(CutSceneController.RunTime, 0f, CutSceneController.TotalTime);
			return true;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004E90 File Offset: 0x00003090
		private static void UpdateToDelta(float d)
		{
			foreach (GameObject gameObject in CutSceneController.CutSceneCache.gameObjectCaches[0].gameObjects)
			{
				ActorObject actorObject = gameObject as ActorObject;
				if (actorObject != null)
				{
					foreach (ZombieEstate2.CutsceneNEW.Action action in actorObject.Actions)
					{
						action.UpdateActorObject(actorObject, d * CutSceneController.TotalTime);
					}
				}
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004F3C File Offset: 0x0000313C
		private static void UpdateMoveAction()
		{
			if (CutSceneController.SelectedAction == null)
			{
				return;
			}
			if (CutSceneController.CHANGINGACTION)
			{
				if (!InputManager.ButtonHeld(Keys.E, 0) && !InputManager.ButtonHeld(Keys.S, 0))
				{
					CutSceneController.CHANGINGACTION = false;
				}
				if (InputManager.ButtonHeld(Keys.E, 0))
				{
					float mouseTime = CutSceneController.GetMouseTime();
					CutSceneController.SelectedAction.EndTime = mouseTime;
					CutSceneController.ClampBounds();
				}
				if (InputManager.ButtonHeld(Keys.S, 0))
				{
					float mouseTime2 = CutSceneController.GetMouseTime();
					CutSceneController.SelectedAction.StartTime = mouseTime2;
					CutSceneController.ClampBounds();
					return;
				}
			}
			else if (InputManager.ButtonHeld(Keys.E, 0) || InputManager.ButtonHeld(Keys.S, 0))
			{
				if (CutSceneController.SelectedActor == null)
				{
					return;
				}
				if (CutSceneController.SelectedAction == null)
				{
					return;
				}
				CutSceneController.CHANGINGACTION = true;
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004FE0 File Offset: 0x000031E0
		private static void UpdateActiveAction()
		{
			if (CutSceneController.SelectedActor == null)
			{
				if (CutSceneController.SelectedAction != null)
				{
					CutSceneController.SelectedAction.Selected = false;
					CutSceneController.SelectedAction = null;
				}
				return;
			}
			if (Mouse.GetState().RightButton == ButtonState.Pressed)
			{
				if (CutSceneController.SelectedAction != null)
				{
					CutSceneController.SelectedAction.Selected = false;
				}
				int x = Mouse.GetState().X;
				int y = Mouse.GetState().Y;
				if (y <= CutSceneController.YOFFSET || y >= CutSceneController.YOFFSET + 32)
				{
					return;
				}
				float time = CutSceneController.ComputeDelta(x) * CutSceneController.TotalTime;
				bool flag = false;
				foreach (ZombieEstate2.CutsceneNEW.Action action in CutSceneController.SelectedActor.Actions)
				{
					action.Selected = false;
					if (action.WithinTime(time))
					{
						CutSceneController.SelectedAction = action;
						flag = true;
						action.Selected = true;
						CutSceneController.actionEditor = new ActionEditor(action);
						return;
					}
				}
				if (!flag)
				{
					CutSceneController.SelectedAction = null;
				}
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000050F4 File Offset: 0x000032F4
		private static void UpdateStartState()
		{
			if (CutSceneController.SelectedAction == null || CutSceneController.SelectedActor == null)
			{
				return;
			}
			if (InputManager.ButtonHeld(Keys.LeftShift, 0) && InputManager.ButtonPressed(Keys.V, 0))
			{
				CutSceneController.SelectedActor.MatchStartPos(CutSceneController.SelectedAction);
				return;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000512C File Offset: 0x0000332C
		private static void UpdateAddRemoveAction()
		{
			if (InputManager.ButtonPressed(Keys.N, 0))
			{
				if (CutSceneController.SelectedActor == null)
				{
					return;
				}
				using (List<ZombieEstate2.CutsceneNEW.Action>.Enumerator enumerator = CutSceneController.SelectedActor.Actions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.WithinTime(CutSceneController.RunTime))
						{
							return;
						}
					}
				}
				ZombieEstate2.CutsceneNEW.Action action = new ZombieEstate2.CutsceneNEW.Action(CutSceneController.RunTime, CutSceneController.SelectedActor);
				CutSceneController.SelectedActor.Actions.Add(action);
				CutSceneController.SelectedActor.Actions.Sort();
				CutSceneController.SelectedActor.MatchStartPos(action);
			}
			if (InputManager.ButtonPressed(Keys.Delete, 0) && CutSceneController.SelectedAction != null)
			{
				CutSceneController.SelectedActor.Actions.Remove(CutSceneController.SelectedAction);
				CutSceneController.SelectedAction = null;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005204 File Offset: 0x00003404
		private static void UpdateActionEditor()
		{
			if (CutSceneController.SelectedAction == null)
			{
				if (CutSceneController.actionEditor != null)
				{
					CutSceneController.actionEditor = null;
				}
				return;
			}
			if (CutSceneController.actionEditor == null)
			{
				CutSceneController.actionEditor = new ActionEditor(CutSceneController.SelectedAction);
			}
			CutSceneController.actionEditor.Update(CutSceneController.prevMouseState);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005240 File Offset: 0x00003440
		private static float GetMouseTime()
		{
			return CutSceneController.ComputeDelta(Mouse.GetState().X) * CutSceneController.TotalTime;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005268 File Offset: 0x00003468
		private static bool OutOfBounds()
		{
			return Mouse.GetState().X > 1152 && Mouse.GetState().Y < 656;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000052A0 File Offset: 0x000034A0
		private static void ClampBounds()
		{
			CutSceneController.SelectedActor.Actions.Sort();
			List<ZombieEstate2.CutsceneNEW.Action> actions = CutSceneController.SelectedActor.Actions;
			float num = 0.1f;
			for (int i = 0; i < actions.Count; i++)
			{
				float minStart;
				if (i - 1 >= 0)
				{
					minStart = actions[i - 1].EndTime;
				}
				else
				{
					minStart = 0f;
				}
				float maxEnd;
				if (i + 1 < actions.Count)
				{
					maxEnd = actions[i + 1].StartTime;
				}
				else
				{
					maxEnd = CutSceneController.TotalTime;
				}
				float minEnd = actions[i].StartTime + num;
				float maxStart = actions[i].EndTime - num;
				CutSceneController.Clamp(actions[i], minStart, maxStart, minEnd, maxEnd);
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005373 File Offset: 0x00003573
		private static void Clamp(ZombieEstate2.CutsceneNEW.Action action, float minStart, float maxStart, float minEnd, float maxEnd)
		{
			action.EndTime = MathHelper.Clamp(action.EndTime, minEnd, maxEnd);
			action.StartTime = MathHelper.Clamp(action.StartTime, minStart, maxStart);
		}

		// Token: 0x0400005B RID: 91
		public static MasterCache CutSceneCache;

		// Token: 0x0400005C RID: 92
		public static float RunTime;

		// Token: 0x0400005D RID: 93
		public static bool EDITING = true;

		// Token: 0x0400005E RID: 94
		public static bool Clicking = false;

		// Token: 0x0400005F RID: 95
		private static bool CHANGINGTIME = false;

		// Token: 0x04000060 RID: 96
		private static bool CHANGINGACTION = false;

		// Token: 0x04000061 RID: 97
		private static ZombieEstate2.CutsceneNEW.Action SelectedAction;

		// Token: 0x04000062 RID: 98
		private static float TotalTime = 20f;

		// Token: 0x04000063 RID: 99
		private static Rectangle LeftBracket = new Rectangle(944, 1008, 16, 16);

		// Token: 0x04000064 RID: 100
		private static Rectangle RightBracket = new Rectangle(960, 1008, 16, 16);

		// Token: 0x04000065 RID: 101
		private static Rectangle LeftBracketSelected = new Rectangle(944, 992, 16, 16);

		// Token: 0x04000066 RID: 102
		private static Rectangle RightBracketSelected = new Rectangle(960, 992, 16, 16);

		// Token: 0x04000067 RID: 103
		private static Rectangle TimePasser = new Rectangle(944, 976, 16, 16);

		// Token: 0x04000068 RID: 104
		public static ActorObject SelectedActor = null;

		// Token: 0x04000069 RID: 105
		private static int WIDTH = 1152;

		// Token: 0x0400006A RID: 106
		private static int XOFFSET = 64;

		// Token: 0x0400006B RID: 107
		private static int YOFFSET = 656;

		// Token: 0x0400006C RID: 108
		private static Rectangle TimeLineBounds = new Rectangle(64, 656, 1152, 16);

		// Token: 0x0400006D RID: 109
		private static ActionEditor actionEditor;

		// Token: 0x0400006E RID: 110
		private static MouseState prevMouseState;

		// Token: 0x0400006F RID: 111
		public static bool Active = false;
	}
}
