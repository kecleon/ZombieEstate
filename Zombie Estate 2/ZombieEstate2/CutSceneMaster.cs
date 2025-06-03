using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Cutscene;

namespace ZombieEstate2
{
	// Token: 0x02000018 RID: 24
	public static class CutSceneMaster
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00005482 File Offset: 0x00003682
		public static void Init(string fileName)
		{
			ScreenFader.Fade(new ScreenFader.FadeDone(CutSceneMaster.Start), 0.025f);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000549C File Offset: 0x0000369C
		private static void Start()
		{
			CutSceneMaster.Reset();
			CutSceneMaster.Lines = new List<CutSceneLine>();
			CutSceneMaster.ExecutingLines = new List<CutSceneLine>();
			CutSceneMaster.LinesToRemove = new List<CutSceneLine>();
			if (CutSceneMaster.CutSceneCache == null)
			{
				CutSceneMaster.CutSceneCache = new MasterCache(Global.Game, 1, 2500, false, "CutSceneMaster_MasterCache");
			}
			Camera.StartCine();
			CutSceneMaster.Blocked = false;
			CutSceneMaster.CurrentIndex = 0;
			CutSceneMaster.CutSceneCache.ClearObjects();
			CutSceneMaster.init = true;
			CutSceneMaster.Lines = new ScriptParser().ParseFile();
			CutSceneMaster.Active = true;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005524 File Offset: 0x00003724
		private static void Reset()
		{
			if (CutSceneMaster.Lines != null)
			{
				if (CutSceneMaster.CurrentIndex < CutSceneMaster.Lines.Count && CutSceneMaster.Lines[CutSceneMaster.CurrentIndex] != null)
				{
					CutSceneMaster.Lines[CutSceneMaster.CurrentIndex].OnFinish -= CutSceneMaster.LineFinished;
				}
				CutSceneMaster.Lines.Clear();
			}
			if (CutSceneMaster.ExecutingLines != null)
			{
				CutSceneMaster.ExecutingLines.Clear();
			}
			if (CutSceneMaster.LinesToRemove != null)
			{
				CutSceneMaster.LinesToRemove.Clear();
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000055A8 File Offset: 0x000037A8
		public static void Update(float elapsed)
		{
			if (!CutSceneMaster.init)
			{
				return;
			}
			if (CutSceneMaster.CurrentIndex < CutSceneMaster.Lines.Count && !CutSceneMaster.Blocked)
			{
				CutSceneMaster.ExecuteLine();
				CutSceneMaster.CurrentIndex++;
			}
			foreach (CutSceneLine cutSceneLine in CutSceneMaster.ExecutingLines)
			{
				cutSceneLine.Update(elapsed);
			}
			foreach (CutSceneLine item in CutSceneMaster.LinesToRemove)
			{
				CutSceneMaster.ExecutingLines.Remove(item);
			}
			CutSceneMaster.CutSceneCache.UpdateCaches(elapsed);
			if (CutSceneMaster.CurrentIndex == CutSceneMaster.Lines.Count && !CutSceneMaster.Blocked)
			{
				CutSceneMaster.EndCutScene();
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005698 File Offset: 0x00003898
		public static void Draw(SpriteBatch spriteBatch)
		{
			if (!CutSceneMaster.init)
			{
				return;
			}
			foreach (CutSceneLine cutSceneLine in CutSceneMaster.ExecutingLines)
			{
				cutSceneLine.Draw(spriteBatch);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000056F0 File Offset: 0x000038F0
		public static void DrawObjects()
		{
			if (!CutSceneMaster.init)
			{
				return;
			}
			CutSceneMaster.CutSceneCache.DrawObjects();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005704 File Offset: 0x00003904
		private static void ExecuteLine()
		{
			CutSceneMaster.ExecutingLines.Add(CutSceneMaster.Lines[CutSceneMaster.CurrentIndex]);
			CutSceneMaster.Lines[CutSceneMaster.CurrentIndex].Run();
			if (CutSceneMaster.Lines[CutSceneMaster.CurrentIndex].BlockingLine)
			{
				CutSceneMaster.Blocked = true;
			}
			CutSceneMaster.Lines[CutSceneMaster.CurrentIndex].OnFinish += CutSceneMaster.LineFinished;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000577C File Offset: 0x0000397C
		private static void LineFinished(object sender, EventArgs e)
		{
			CutSceneLine cutSceneLine = sender as CutSceneLine;
			cutSceneLine.OnFinish -= CutSceneMaster.LineFinished;
			if (cutSceneLine.BlockingLine)
			{
				CutSceneMaster.Blocked = false;
			}
			CutSceneMaster.LinesToRemove.Add(cutSceneLine);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000057BB File Offset: 0x000039BB
		public static void EndCutScene()
		{
			CutSceneMaster.Reset();
			Camera.EndCine();
			CutSceneMaster.Active = false;
			CutSceneMaster.CutSceneCache.ClearObjects();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000057D8 File Offset: 0x000039D8
		public static CineObject GetCineObjectByID(string id)
		{
			foreach (GameObject gameObject in CutSceneMaster.CutSceneCache.gameObjectCaches[0].gameObjects)
			{
				CineObject cineObject = gameObject as CineObject;
				if (cineObject != null && cineObject.ObjectID.ToUpper() == id.ToUpper())
				{
					return cineObject;
				}
			}
			Terminal.WriteMessage("ERROR: Did not find CineObject with id: " + id, MessageType.ERROR);
			return null;
		}

		// Token: 0x04000070 RID: 112
		private static int CurrentIndex;

		// Token: 0x04000071 RID: 113
		public static List<CutSceneLine> Lines;

		// Token: 0x04000072 RID: 114
		public static List<CutSceneLine> ExecutingLines;

		// Token: 0x04000073 RID: 115
		public static List<CutSceneLine> LinesToRemove;

		// Token: 0x04000074 RID: 116
		private static bool init;

		// Token: 0x04000075 RID: 117
		private static bool Blocked;

		// Token: 0x04000076 RID: 118
		public static MasterCache CutSceneCache;

		// Token: 0x04000077 RID: 119
		public static bool Active;
	}
}
