using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x020000E4 RID: 228
	internal class MainMenuObjectCache
	{
		// Token: 0x06000600 RID: 1536 RVA: 0x0002CCF4 File Offset: 0x0002AEF4
		public MainMenuObjectCache(Game game)
		{
			this.cache = new MasterCache(game, 1, 6000, false, "Main_Menu_Cache");
			this.obj = new MenuGameObject(new Point(66, 54), new Vector3(14f, 0.48f, 21f));
			this.obj.YRotation = 1.32f;
			this.cache.CreateObject(this.obj);
			this.obj = new MenuGameObject(new Point(70, 54), new Vector3(17f, 0.48f, 19f));
			this.obj.YRotation = 0f;
			this.cache.CreateObject(this.obj);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0002CDB3 File Offset: 0x0002AFB3
		public void Update(float elapsed)
		{
			this.MouseStuff();
			this.cache.UpdateCaches(elapsed);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0002CDC7 File Offset: 0x0002AFC7
		public void Draw()
		{
			this.cache.DrawObjects();
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0002CDD4 File Offset: 0x0002AFD4
		private void MouseStuff()
		{
			MouseState state = Mouse.GetState();
			int x = state.X;
			int y = state.Y;
			Vector3 source = new Vector3((float)x, (float)y, 0f);
			Vector3 source2 = new Vector3((float)x, (float)y, 1f);
			Matrix world = Matrix.CreateTranslation(0f, 0f, 0f);
			Vector3 vector = Global.GraphicsDevice.Viewport.Unproject(source, Global.Projection, Global.View, world);
			Vector3 direction = Global.GraphicsDevice.Viewport.Unproject(source2, Global.Projection, Global.View, world) - vector;
			direction.Normalize();
			Ray ray = new Ray(vector, direction);
			foreach (GameObject gameObject in this.cache.gameObjectCaches[0].gameObjects)
			{
				MenuGameObject menuGameObject = gameObject as MenuGameObject;
				if (menuGameObject != null)
				{
					menuGameObject.Highlighted = false;
					float? num;
					if (menuGameObject.MouseOver(ray, out num))
					{
						menuGameObject.Highlighted = true;
					}
				}
			}
		}

		// Token: 0x040005D7 RID: 1495
		private MasterCache cache;

		// Token: 0x040005D8 RID: 1496
		private MenuGameObject obj;
	}
}
