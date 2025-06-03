using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2.CutsceneNEW
{
	// Token: 0x020001E3 RID: 483
	internal class DragableObject : GameObject
	{
		// Token: 0x06000CDC RID: 3292 RVA: 0x000693E4 File Offset: 0x000675E4
		public DragableObject()
		{
			this.AffectedByGravity = false;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0006940C File Offset: 0x0006760C
		public override void Update(float elapsed)
		{
			if (this.LockedToMouse)
			{
				if (Mouse.GetState().RightButton == ButtonState.Released)
				{
					Vector3 pickedPosition = MouseHandler.GetPickedPosition();
					this.LockedToMouse = false;
					this.Position.X = pickedPosition.X;
					this.Position.Z = pickedPosition.Z;
					CutSceneController.Clicking = false;
					this.Placed();
				}
				else
				{
					Vector3 pickedPosition2 = MouseHandler.GetPickedPosition();
					this.Position.X = pickedPosition2.X;
					this.Position.Z = pickedPosition2.Z;
					DragableObject.Selection.Position.X = this.Position.X;
					DragableObject.Selection.Position.Z = this.Position.Z;
					this.UpdateUpAndDown();
				}
			}
			if (this.Selected && DragableObject.Selection != null)
			{
				DragableObject.Selection.Position.X = this.Position.X;
				DragableObject.Selection.Position.Z = this.Position.Z;
			}
			base.Update(elapsed);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0006951C File Offset: 0x0006771C
		public bool CheckClicked(Vector3 mousePos)
		{
			if (!this.Selected)
			{
				return false;
			}
			this.Bounds.X = (int)this.Position.X - this.Bounds.Width / 2;
			this.Bounds.Y = (int)this.Position.Z - this.Bounds.Height / 2;
			if (this.Bounds.Contains((int)mousePos.X, (int)mousePos.Z))
			{
				this.Clicked();
				return true;
			}
			return false;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x000695A4 File Offset: 0x000677A4
		public bool CheckSelected(Vector3 mousePos)
		{
			this.Bounds.X = (int)this.Position.X - this.Bounds.Width / 2;
			this.Bounds.Y = (int)this.Position.Z - this.Bounds.Height / 2;
			if (this.Bounds.Contains((int)mousePos.X, (int)mousePos.Z))
			{
				this.Selected = true;
				this.createSelection();
				return true;
			}
			return false;
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00069626 File Offset: 0x00067826
		public virtual void Clicked()
		{
			if (!this.Selected)
			{
				return;
			}
			this.createSelection();
			this.Selected = true;
			this.LockedToMouse = true;
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00069648 File Offset: 0x00067848
		private void createSelection()
		{
			if (DragableObject.Selection == null)
			{
				DragableObject.Selection = new GameObject();
				DragableObject.Selection.TextureCoord = new Point(61, 63);
				DragableObject.Selection.AffectedByGravity = false;
				DragableObject.Selection.XRotation = -1.5707964f;
				Vector3 pos = new Vector3(this.Position.X, 0.2f, this.Position.Z);
				DragableObject.Selection.ActivateObject(pos, new Point(61, 63));
				CutSceneController.CutSceneCache.CreateObject(DragableObject.Selection);
			}
			DragableObject.Selection.Position.X = this.Position.X;
			DragableObject.Selection.Position.Z = this.Position.Z;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Placed()
		{
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00069710 File Offset: 0x00067910
		public void UpdateUpAndDown()
		{
			if (!this.Liftable)
			{
				return;
			}
			if (InputManager.ButtonHeld(Keys.Q, 0))
			{
				this.Position.Y = this.Position.Y - 0.05f;
			}
			if (InputManager.ButtonHeld(Keys.E, 0))
			{
				this.Position.Y = this.Position.Y + 0.05f;
			}
			if (InputManager.ButtonPressed(Keys.G, 0))
			{
				this.Position.Y = this.floorHeight;
			}
		}

		// Token: 0x04000D90 RID: 3472
		public Rectangle Bounds = new Rectangle(0, 0, 2, 2);

		// Token: 0x04000D91 RID: 3473
		public static GameObject Selection;

		// Token: 0x04000D92 RID: 3474
		public bool Selected;

		// Token: 0x04000D93 RID: 3475
		private bool WasSelected;

		// Token: 0x04000D94 RID: 3476
		public bool LockedToMouse;

		// Token: 0x04000D95 RID: 3477
		private bool Liftable = true;
	}
}
