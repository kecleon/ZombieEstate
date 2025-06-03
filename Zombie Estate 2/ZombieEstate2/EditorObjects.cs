using System;
using System.Collections.Generic;

namespace ZombieEstate2
{
	// Token: 0x0200006B RID: 107
	internal class EditorObjects
	{
		// Token: 0x0600028E RID: 654 RVA: 0x000144C0 File Offset: 0x000126C0
		public EditorObjects()
		{
			this.EditorObjs = new List<GameObject>();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000144D3 File Offset: 0x000126D3
		public void AddObject(GameObject obj)
		{
			this.EditorObjs.Add(obj);
			Editor.editorObjects.CreateObject(obj);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000144F0 File Offset: 0x000126F0
		public void ToggleShown()
		{
			this.Shown = !this.Shown;
			if (this.Shown)
			{
				using (List<GameObject>.Enumerator enumerator = this.EditorObjs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject obj = enumerator.Current;
						Editor.editorObjects.CreateObject(obj);
					}
					return;
				}
			}
			foreach (GameObject gameObject in this.EditorObjs)
			{
				gameObject.DestroyObject();
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000145A0 File Offset: 0x000127A0
		public void RemoveObject(GameObject obj)
		{
			this.EditorObjs.Remove(obj);
			obj.DestroyObject();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000145B8 File Offset: 0x000127B8
		public void ClearObjects()
		{
			for (int i = 0; i < this.EditorObjs.Count; i++)
			{
				this.EditorObjs[i].DestroyObject();
			}
			this.EditorObjs.Clear();
		}

		// Token: 0x04000282 RID: 642
		private List<GameObject> EditorObjs;

		// Token: 0x04000283 RID: 643
		public bool Shown;
	}
}
