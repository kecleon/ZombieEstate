using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000125 RID: 293
	public struct CustomLevelVertexDec : IVertexType
	{
		// Token: 0x06000857 RID: 2135 RVA: 0x00045D6B File Offset: 0x00043F6B
		public CustomLevelVertexDec(Vector3 pos, Vector2 tex, Vector2 lightTex)
		{
			this.vertexPosition = pos;
			this.vertexTexCoord = tex;
			this.vertexLightTexCoord = lightTex;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x00045D82 File Offset: 0x00043F82
		// (set) Token: 0x06000859 RID: 2137 RVA: 0x00045D8A File Offset: 0x00043F8A
		public Vector3 Position
		{
			get
			{
				return this.vertexPosition;
			}
			set
			{
				this.vertexPosition = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x00045D93 File Offset: 0x00043F93
		// (set) Token: 0x0600085B RID: 2139 RVA: 0x00045D9B File Offset: 0x00043F9B
		public Vector2 TextureCoordinate
		{
			get
			{
				return this.vertexTexCoord;
			}
			set
			{
				this.vertexTexCoord = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x00045DA4 File Offset: 0x00043FA4
		// (set) Token: 0x0600085D RID: 2141 RVA: 0x00045DAC File Offset: 0x00043FAC
		public Vector2 LightTextureCoordinate
		{
			get
			{
				return this.vertexLightTexCoord;
			}
			set
			{
				this.vertexLightTexCoord = value;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x00045DB5 File Offset: 0x00043FB5
		public VertexDeclaration VertexDeclaration
		{
			get
			{
				return CustomLevelVertexDec.LevelVertexDec;
			}
		}

		// Token: 0x040008FD RID: 2301
		private Vector3 vertexPosition;

		// Token: 0x040008FE RID: 2302
		private Vector2 vertexTexCoord;

		// Token: 0x040008FF RID: 2303
		private Vector2 vertexLightTexCoord;

		// Token: 0x04000900 RID: 2304
		private static VertexDeclaration LevelVertexDec = new VertexDeclaration(new VertexElement[]
		{
			new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
			new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
			new VertexElement(20, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1)
		});
	}
}
