using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000114 RID: 276
	public class GameObjectCache
	{
		// Token: 0x06000787 RID: 1927 RVA: 0x0003B4C0 File Offset: 0x000396C0
		public GameObjectCache(Game game, int numObjs, bool init)
		{
			this.device = game.GraphicsDevice;
			this.gameObjects = new List<GameObject>();
			this.objCount = numObjs;
			this.texNumbers = new Vector2[this.objCount];
			for (int i = 0; i < this.objCount; i++)
			{
				this.gameObjects.Add(new GameObject());
			}
			if (Global.instancedModel == null)
			{
				Global.instancedModel = game.Content.Load<Model>("Plane");
			}
			this.instanceTransforms = new Matrix[this.objCount];
			this.instancedModelBones = new Matrix[Global.instancedModel.Bones.Count];
			Global.instancedModel.CopyAbsoluteBoneTransformsTo(this.instancedModelBones);
			this.rasterizerState1 = new RasterizerState();
			this.rasterizerState1.CullMode = CullMode.None;
			Global.GraphicsDevice.RasterizerState = this.rasterizerState1;
			this.SetupVertexBuffers(this.instanceTransforms, Global.GraphicsDevice);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0003B5BD File Offset: 0x000397BD
		public void DrawObjects()
		{
			this.Draw();
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0003B5C8 File Offset: 0x000397C8
		protected void Draw()
		{
			Global.GraphicsDevice.RasterizerState = this.rasterizerState1;
			Array.Resize<Matrix>(ref this.instanceTransforms, this.objCount);
			this.activeCount = 0;
			for (int i = 0; i < this.objCount; i++)
			{
				this.instanceTransforms[i] = this.gameObjects[i].GetTransform();
				this.texNumbers[i] = this.gameObjects[i].GetTextureNumber();
				if (this.gameObjects[i].Active)
				{
					this.activeCount++;
				}
			}
			this.DrawModelHardwareInstancing(Global.instancedModel, this.instancedModelBones, this.instanceTransforms, Global.View, Global.Projection);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0003B68C File Offset: 0x0003988C
		public void SET_DATA()
		{
			GraphicsDevice graphicsDevice = Global.GraphicsDevice;
			if (this.instanceVertexBuffer == null || this.instanceTransforms.Length > this.instanceVertexBuffer.VertexCount)
			{
				this.SetupVertexBuffers(this.instanceTransforms, graphicsDevice);
			}
			if ((this.instanceVertexBuffer != null && this.instanceVertexBuffer.IsContentLost) || (this.instanceTexBuffer != null && this.instanceTexBuffer.IsContentLost))
			{
				Terminal.WriteMessage("ERROR - ContentLost on instanceVertexBuffer (or instanceTexBuffer)", MessageType.ERROR);
				this.SetupVertexBuffers(this.instanceTransforms, graphicsDevice);
			}
			else if (this.mClearVertBuffer)
			{
				Terminal.WriteMessage("Refreshing buffers...");
				this.mClearVertBuffer = false;
				this.SetupVertexBuffers(this.instanceTransforms, graphicsDevice);
			}
			this.instanceVertexBuffer.SetData<Matrix>(this.instanceTransforms, 0, this.instanceTransforms.Length, SetDataOptions.Discard);
			this.instanceTexBuffer.SetData<Vector2>(this.texNumbers, 0, this.instanceTransforms.Length, SetDataOptions.Discard);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0003B76C File Offset: 0x0003996C
		private void DrawModelHardwareInstancing(Model model, Matrix[] modelBones, Matrix[] instances, Matrix view, Matrix projection)
		{
			GraphicsDevice graphicsDevice = Global.GraphicsDevice;
			if (instances.Length == 0)
			{
				return;
			}
			foreach (ModelMesh modelMesh in model.Meshes)
			{
				foreach (ModelMeshPart modelMeshPart in modelMesh.MeshParts)
				{
					graphicsDevice.SetVertexBuffers(new VertexBufferBinding[]
					{
						new VertexBufferBinding(modelMeshPart.VertexBuffer, modelMeshPart.VertexOffset, 0),
						new VertexBufferBinding(this.instanceVertexBuffer, 0, 1),
						new VertexBufferBinding(this.instanceTexBuffer, 0, 1)
					});
					graphicsDevice.Indices = modelMeshPart.IndexBuffer;
					Effect effect = modelMeshPart.Effect;
					effect.CurrentTechnique = effect.Techniques[GameObjectCache.Technique];
					effect.Parameters[GameObjectCache.mWorld].SetValue(modelBones[modelMesh.ParentBone.Index]);
					effect.Parameters[GameObjectCache.mView].SetValue(view);
					effect.Parameters[GameObjectCache.mProj].SetValue(projection);
					effect.Parameters[GameObjectCache.mTexture].SetValue(Global.MasterTexture);
					foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
					{
						effectPass.Apply();
						graphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, modelMeshPart.NumVertices, modelMeshPart.StartIndex, modelMeshPart.PrimitiveCount, instances.Length);
					}
				}
			}
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0003B97C File Offset: 0x00039B7C
		private void SetupVertexBuffers(Matrix[] instances, GraphicsDevice GraphicsDevice)
		{
			if (this.instanceVertexBuffer != null)
			{
				this.instanceVertexBuffer.Dispose();
			}
			if (this.instanceTexBuffer != null)
			{
				this.instanceTexBuffer.Dispose();
			}
			Terminal.WriteMessage("Initializing instanceVertexBuffer and instanceTexBuffer.", MessageType.IMPORTANTEVENT);
			this.instanceVertexBuffer = new DynamicVertexBuffer(GraphicsDevice, GameObjectCache.instanceVertexDeclaration, instances.Length, BufferUsage.WriteOnly);
			this.instanceTexBuffer = new DynamicVertexBuffer(GraphicsDevice, GameObjectCache.instanceTEXVertDeclaration, instances.Length, BufferUsage.WriteOnly);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0003B9E4 File Offset: 0x00039BE4
		public void RefreshVertexBuffer()
		{
			this.mClearVertBuffer = true;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void CREATETESTOBJECTS()
		{
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0003B9F0 File Offset: 0x00039BF0
		private void nextOpenIndex()
		{
			int num = 0;
			int num2 = 700;
			this.openIndex++;
			if (this.openIndex >= this.objCount - num2)
			{
				this.openIndex = 0;
			}
			if (this.openIndex != -1)
			{
				while (this.gameObjects[this.openIndex].Active && num < this.objCount - num2)
				{
					this.openIndex++;
					if (this.openIndex >= this.objCount - num2)
					{
						this.openIndex = 0;
					}
					num++;
				}
			}
			if (num >= this.objCount - num2)
			{
				this.openIndex = -1;
			}
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0003BA94 File Offset: 0x00039C94
		public bool CreateObject(GameObject obj)
		{
			this.nextOpenIndex();
			if (this.openIndex == -1)
			{
				Terminal.WriteMessage("ERROR: Ran out of objects!!! <-> CreateObject() in GameObjectCache", MessageType.ERROR);
				Console.WriteLine("ERROR: Ran out of objects!!! <-> CreateObject() in GameObjectCache");
				this.nextOpenIndex();
				obj = null;
				return false;
			}
			this.gameObjects[this.openIndex] = obj;
			if (obj is Shootable)
			{
				DynamicShadows.Shootables.Add(obj as Shootable);
			}
			return true;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0003BAFC File Offset: 0x00039CFC
		public void ClearObjects()
		{
			for (int i = 0; i < this.objCount; i++)
			{
				this.gameObjects[i].DestroyObject();
			}
		}

		// Token: 0x040007A8 RID: 1960
		public List<GameObject> gameObjects;

		// Token: 0x040007A9 RID: 1961
		private Matrix[] instanceTransforms;

		// Token: 0x040007AA RID: 1962
		private Vector2[] texNumbers;

		// Token: 0x040007AB RID: 1963
		public int activeCount;

		// Token: 0x040007AC RID: 1964
		private GraphicsDevice device;

		// Token: 0x040007AD RID: 1965
		private int objCount = 1500;

		// Token: 0x040007AE RID: 1966
		private int openIndex;

		// Token: 0x040007AF RID: 1967
		private MultiThreadUpdater up;

		// Token: 0x040007B0 RID: 1968
		private Matrix[] instancedModelBones;

		// Token: 0x040007B1 RID: 1969
		private DynamicVertexBuffer instanceVertexBuffer;

		// Token: 0x040007B2 RID: 1970
		private DynamicVertexBuffer instanceTexBuffer;

		// Token: 0x040007B3 RID: 1971
		public static string Technique = "HardwareInstancing";

		// Token: 0x040007B4 RID: 1972
		private RasterizerState rasterizerState1;

		// Token: 0x040007B5 RID: 1973
		private static VertexDeclaration instanceVertexDeclaration = new VertexDeclaration(new VertexElement[]
		{
			new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
			new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
			new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
			new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3)
		});

		// Token: 0x040007B6 RID: 1974
		private static VertexDeclaration instanceTEXVertDeclaration = new VertexDeclaration(new VertexElement[]
		{
			new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
		});

		// Token: 0x040007B7 RID: 1975
		private static string mWorld = "World";

		// Token: 0x040007B8 RID: 1976
		private static string mProj = "Projection";

		// Token: 0x040007B9 RID: 1977
		private static string mView = "View";

		// Token: 0x040007BA RID: 1978
		private static string mTexture = "Texture";

		// Token: 0x040007BB RID: 1979
		private bool mClearVertBuffer;
	}
}
