using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000132 RID: 306
	public static class TileBuilder
	{
		// Token: 0x060008BD RID: 2237 RVA: 0x000494C4 File Offset: 0x000476C4
		public static CustomLevelVertexDec[] BuildFloor(int x, int y, int xCoord, int yCoord, float height, int lightX, int lightY)
		{
			CustomLevelVertexDec[] array = new CustomLevelVertexDec[6];
			float scale = Tile.SCALE;
			float num = (float)xCoord;
			float num2 = (float)yCoord;
			if (height != 0f)
			{
				height += 0.025f;
			}
			else
			{
				height += 0.015f;
			}
			float num3 = (float)lightX;
			float num4 = (float)lightY;
			Vector3 pos = new Vector3((float)x, height, (float)y);
			array[0] = new CustomLevelVertexDec(pos, new Vector2(num * TileBuilder.TileSize / TileBuilder.TileSetSize, num2 * TileBuilder.TileSize / TileBuilder.TileSetSize), new Vector2(num3 * TileBuilder.TileSize / TileBuilder.TileSetSize, num4 * TileBuilder.TileSize / TileBuilder.TileSetSize));
			pos = new Vector3((float)x + scale, height, (float)y);
			array[1] = new CustomLevelVertexDec(pos, new Vector2((num * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize, num2 * TileBuilder.TileSize / TileBuilder.TileSetSize), new Vector2((num3 * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize, num4 * TileBuilder.TileSize / TileBuilder.TileSetSize));
			pos = new Vector3((float)x + scale, height, (float)y + scale);
			array[2] = new CustomLevelVertexDec(pos, new Vector2((num * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize, (num2 * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize), new Vector2((num3 * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize, (num4 * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize));
			pos = new Vector3((float)x, height, (float)y);
			array[3] = new CustomLevelVertexDec(pos, new Vector2(num * TileBuilder.TileSize / TileBuilder.TileSetSize, num2 * TileBuilder.TileSize / TileBuilder.TileSetSize), new Vector2(num3 * TileBuilder.TileSize / TileBuilder.TileSetSize, num4 * TileBuilder.TileSize / TileBuilder.TileSetSize));
			pos = new Vector3((float)x + scale, height, (float)y + scale);
			array[4] = new CustomLevelVertexDec(pos, new Vector2((num * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize, (num2 * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize), new Vector2((num3 * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize, (num4 * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize));
			pos = new Vector3((float)x, height, (float)y + scale);
			array[5] = new CustomLevelVertexDec(pos, new Vector2(num * TileBuilder.TileSize / TileBuilder.TileSetSize, (num2 * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize), new Vector2(num3 * TileBuilder.TileSize / TileBuilder.TileSetSize, (num4 * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize));
			return array;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0004977C File Offset: 0x0004797C
		public static CustomLevelVertexDec[] AddWall(int x, int y, int xTexCoord, int yTexCoord, float rotation, float height, Point[] wallLights)
		{
			CustomLevelVertexDec[] array = new CustomLevelVertexDec[36];
			float scale = Tile.SCALE;
			float num = 0f;
			if (rotation == 0f || rotation == 3.1415927f)
			{
				num = 0.001f;
			}
			Vector3 vector = new Vector3(0f, 0f + scale + num, scale * TileBuilder.wallThicknessModifier + 0f);
			Vector3 vector2 = new Vector3(0f, 0f, scale * TileBuilder.wallThicknessModifier + 0f);
			Vector3 vector3 = new Vector3(0f + scale, 0f + scale + num, scale * TileBuilder.wallThicknessModifier + 0f);
			Vector3 vector4 = new Vector3(0f + scale, 0f, scale * TileBuilder.wallThicknessModifier + 0f);
			Vector3 vector5 = new Vector3(0f, 0f + scale + num, 0f);
			Vector3 vector6 = new Vector3(0f + scale, 0f + scale + num, 0f);
			Vector3 vector7 = new Vector3(0f, 0f, 0f);
			Vector3 vector8 = new Vector3(0f + scale, 0f, 0f);
			int num2 = 0;
			Vector3 position = new Vector3((float)x, height, (float)y);
			if (rotation == 3.1415927f)
			{
				position = new Vector3((float)(x + 1), height, (float)(y + 1));
				num2 = 2;
			}
			if (rotation == 1.5707964f)
			{
				position = new Vector3((float)x, height, (float)(y + 1));
				num2 = 1;
			}
			if (rotation == 4.712389f)
			{
				position = new Vector3((float)(x + 1), height, (float)y);
				num2 = 3;
			}
			int x2 = wallLights[num2].X;
			int y2 = wallLights[num2].Y;
			vector = Vector3.Transform(vector, Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position));
			vector2 = Vector3.Transform(vector2, Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position));
			vector3 = Vector3.Transform(vector3, Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position));
			vector4 = Vector3.Transform(vector4, Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position));
			vector5 = Vector3.Transform(vector5, Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position));
			vector6 = Vector3.Transform(vector6, Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position));
			vector7 = Vector3.Transform(vector7, Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position));
			vector8 = Vector3.Transform(vector8, Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position));
			float num3 = (float)xTexCoord * TileBuilder.TileSize / TileBuilder.TileSetSize;
			float num4 = (float)yTexCoord * TileBuilder.TileSize / TileBuilder.TileSetSize;
			float x3 = ((float)xTexCoord * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize;
			float y3 = ((float)yTexCoord * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize;
			float x4 = (float)x2 * TileBuilder.TileSize / TileBuilder.TileSetSize;
			float y4 = (float)y2 * TileBuilder.TileSize / TileBuilder.TileSetSize;
			float x5 = ((float)x2 * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize;
			float y5 = ((float)y2 * TileBuilder.TileSize + TileBuilder.TileSize) / TileBuilder.TileSetSize;
			array[0] = new CustomLevelVertexDec(vector, new Vector2(num3, num4), new Vector2(x4, y4));
			array[1] = new CustomLevelVertexDec(vector2, new Vector2(num3, y3), new Vector2(x4, y5));
			array[2] = new CustomLevelVertexDec(vector3, new Vector2(x3, num4), new Vector2(x5, y4));
			array[3] = new CustomLevelVertexDec(vector2, new Vector2(num3, y3), new Vector2(x4, y5));
			array[4] = new CustomLevelVertexDec(vector4, new Vector2(x3, y3), new Vector2(x5, y5));
			array[5] = new CustomLevelVertexDec(vector3, new Vector2(x3, num4), new Vector2(x5, y4));
			array[6] = new CustomLevelVertexDec(vector5, new Vector2(num3, num4), new Vector2(x4, y4));
			array[7] = new CustomLevelVertexDec(vector6, new Vector2(x3, num4), new Vector2(x5, y4));
			array[8] = new CustomLevelVertexDec(vector7, new Vector2(num3, y3), new Vector2(x4, y5));
			array[9] = new CustomLevelVertexDec(vector7, new Vector2(num3, y3), new Vector2(x4, y5));
			array[10] = new CustomLevelVertexDec(vector6, new Vector2(x3, num4), new Vector2(x5, y4));
			array[11] = new CustomLevelVertexDec(vector8, new Vector2(x3, y3), new Vector2(x5, y5));
			array[12] = new CustomLevelVertexDec(vector, new Vector2(num3, num4), new Vector2(-1f, 0f));
			array[13] = new CustomLevelVertexDec(vector6, new Vector2(num3 + 1f / TileBuilder.TileSetSize, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			array[14] = new CustomLevelVertexDec(vector5, new Vector2(num3, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			array[15] = new CustomLevelVertexDec(vector, new Vector2(num3, num4), new Vector2(-1f, 0f));
			array[16] = new CustomLevelVertexDec(vector3, new Vector2(num3 + 1f / TileBuilder.TileSetSize, num4), new Vector2(-1f, 0f));
			array[17] = new CustomLevelVertexDec(vector6, new Vector2(num3, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			array[18] = new CustomLevelVertexDec(vector2, new Vector2(num3, num4), new Vector2(-1f, 0f));
			array[19] = new CustomLevelVertexDec(vector7, new Vector2(num3, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			array[20] = new CustomLevelVertexDec(vector8, new Vector2(num3 + 1f / TileBuilder.TileSetSize, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			array[21] = new CustomLevelVertexDec(vector2, new Vector2(num3, num4), new Vector2(-1f, 0f));
			array[22] = new CustomLevelVertexDec(vector8, new Vector2(num3 + 1f / TileBuilder.TileSetSize, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			array[23] = new CustomLevelVertexDec(vector4, new Vector2(num3 + 1f / TileBuilder.TileSetSize, num4), new Vector2(-1f, 0f));
			array[24] = new CustomLevelVertexDec(vector, new Vector2(num3, num4), new Vector2(-1f, 0f));
			array[25] = new CustomLevelVertexDec(vector7, new Vector2(num3 + 1f / TileBuilder.TileSetSize, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			array[26] = new CustomLevelVertexDec(vector2, new Vector2(num3, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			array[27] = new CustomLevelVertexDec(vector5, new Vector2(num3 + 1f / TileBuilder.TileSetSize, num4), new Vector2(-1f, 0f));
			array[28] = new CustomLevelVertexDec(vector7, new Vector2(num3 + 1f / TileBuilder.TileSetSize, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			array[29] = new CustomLevelVertexDec(vector, new Vector2(num3, num4), new Vector2(-1f, 0f));
			array[30] = new CustomLevelVertexDec(vector3, new Vector2(num3, num4), new Vector2(-1f, 0f));
			array[31] = new CustomLevelVertexDec(vector4, new Vector2(num3, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			array[32] = new CustomLevelVertexDec(vector8, new Vector2(num3 + 1f / TileBuilder.TileSetSize, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			array[33] = new CustomLevelVertexDec(vector6, new Vector2(num3 + 1f / TileBuilder.TileSetSize, num4), new Vector2(-1f, 0f));
			array[34] = new CustomLevelVertexDec(vector3, new Vector2(num3, num4), new Vector2(-1f, 0f));
			array[35] = new CustomLevelVertexDec(vector8, new Vector2(num3 + 1f / TileBuilder.TileSetSize, num4 + 1f / TileBuilder.TileSetSize), new Vector2(-1f, 0f));
			return array;
		}

		// Token: 0x04000985 RID: 2437
		public static float TileSetSize = 512f;

		// Token: 0x04000986 RID: 2438
		public static float TileSize = 16f;

		// Token: 0x04000987 RID: 2439
		public static float wallThicknessModifier = 0.075f;
	}
}
