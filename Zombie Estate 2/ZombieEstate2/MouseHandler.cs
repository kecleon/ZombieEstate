using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x02000118 RID: 280
	public static class MouseHandler
	{
		// Token: 0x060007B0 RID: 1968 RVA: 0x0003C448 File Offset: 0x0003A648
		public static Vector3 GetPickedPosition()
		{
			Vector2 value = new Vector2((float)Mouse.GetState().X, (float)Mouse.GetState().Y);
			Vector3 source = new Vector3(value, 0f);
			Vector3 source2 = new Vector3(value, 1f);
			Vector3 vector = Global.GraphicsDevice.Viewport.Unproject(source, Global.Projection, Global.View, Matrix.Identity);
			Vector3 vector2 = Global.GraphicsDevice.Viewport.Unproject(source2, Global.Projection, Global.View, Matrix.Identity) - vector;
			vector2.Normalize();
			Ray ray = new Ray(vector, vector2);
			Vector3 normal = new Vector3(0f, 1f, 0f);
			Plane plane = new Plane(normal, 0f);
			float num = Vector3.Dot(plane.Normal, ray.Direction);
			float scaleFactor = -((Vector3.Dot(plane.Normal, ray.Position) + plane.D) / num);
			return vector + vector2 * scaleFactor;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0003C560 File Offset: 0x0003A760
		public static Vector3 GetPickedPosition(int height)
		{
			Vector2 value = new Vector2((float)Mouse.GetState().X, (float)Mouse.GetState().Y);
			Vector3 source = new Vector3(value, 0f);
			Vector3 source2 = new Vector3(value, 1f);
			Vector3 vector = Global.GraphicsDevice.Viewport.Unproject(source, Global.Projection, Global.View, Matrix.Identity);
			Vector3 vector2 = Global.GraphicsDevice.Viewport.Unproject(source2, Global.Projection, Global.View, Matrix.Identity) - vector;
			vector2.Normalize();
			Ray ray = new Ray(vector, vector2);
			Vector3 normal = new Vector3(0f, 1f, 0f);
			Plane plane = new Plane(normal, (float)height);
			float num = Vector3.Dot(plane.Normal, ray.Direction);
			float scaleFactor = -((Vector3.Dot(plane.Normal, ray.Position) + plane.D) / num);
			return vector + vector2 * scaleFactor;
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0003C674 File Offset: 0x0003A874
		public static Vector3 GetPickedPosition(Vector2 mousePosition)
		{
			Vector3 source = new Vector3(mousePosition, 0f);
			Vector3 source2 = new Vector3(mousePosition, 1f);
			Vector3 vector = Global.GraphicsDevice.Viewport.Unproject(source, Global.Projection, Global.View, Matrix.Identity);
			Vector3 vector2 = Global.GraphicsDevice.Viewport.Unproject(source2, Global.Projection, Global.View, Matrix.Identity) - vector;
			vector2.Normalize();
			Ray ray = new Ray(vector, vector2);
			Vector3 normal = new Vector3(0f, 1f, 0f);
			Plane plane = new Plane(normal, 0f);
			float num = Vector3.Dot(plane.Normal, ray.Direction);
			float scaleFactor = -((Vector3.Dot(plane.Normal, ray.Position) + plane.D) / num);
			return vector + vector2 * scaleFactor;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0003C764 File Offset: 0x0003A964
		public static void DrawMouse(SpriteBatch spriteBatch)
		{
			if (!MouseHandler.MouseVisible)
			{
				return;
			}
			MouseHandler.pos.X = InputManager.GetMousePosition().X;
			MouseHandler.pos.Y = InputManager.GetMousePosition().Y;
			if (MouseHandler.TargetReticule)
			{
				MouseHandler.pos.X = MouseHandler.pos.X - 32;
				MouseHandler.pos.Y = MouseHandler.pos.Y - 32;
				MouseHandler.pos.Width = 64;
				MouseHandler.pos.Height = 64;
				spriteBatch.Draw(Global.MasterTexture, MouseHandler.pos, new Rectangle?(MouseHandler.src), Color.White);
				return;
			}
			MouseHandler.pos.Width = 32;
			MouseHandler.pos.Height = 32;
			spriteBatch.Draw(MouseHandler.MouseTex, MouseHandler.pos, Color.White);
		}

		// Token: 0x04000827 RID: 2087
		public static bool TargetReticule = false;

		// Token: 0x04000828 RID: 2088
		public static Texture2D MouseTex;

		// Token: 0x04000829 RID: 2089
		public static Rectangle pos = new Rectangle(0, 0, 32, 32);

		// Token: 0x0400082A RID: 2090
		public static Rectangle src;

		// Token: 0x0400082B RID: 2091
		public static bool MouseVisible = false;
	}
}
