using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000129 RID: 297
	internal class SectorSaver
	{
		// Token: 0x06000860 RID: 2144 RVA: 0x00045E0C File Offset: 0x0004400C
		public static void SaveSector(Sector sector, string fileName)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TileInfo>));
			List<TileInfo> tileInfo = sector.GetTileInfo();
			FileStream fileStream = new FileStream("Levels\\" + fileName, FileMode.Create);
			xmlSerializer.Serialize(fileStream, tileInfo);
			Terminal.WriteMessage("Saved Level: " + fileName, MessageType.SAVELOAD);
			fileStream.Close();
			if (SectorSaver.SAVEPATH)
			{
				sector.SavePathing();
			}
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00045E6C File Offset: 0x0004406C
		public static void LoadSector(Sector sector, string fileName)
		{
			object @lock = SectorSaver.LOCK;
			lock (@lock)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				Terminal.WriteMessage("Loading sector info...");
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TileInfo>));
				List<TileInfo> info = new List<TileInfo>();
				StreamReader streamReader = new StreamReader(fileName);
				info = (List<TileInfo>)xmlSerializer.Deserialize(streamReader);
				sector.LoadTilesFromInfo(info);
				Terminal.WriteMessage("Loaded Level: " + fileName, MessageType.SAVELOAD);
				streamReader.Close();
				stopwatch.Stop();
				Terminal.WriteMessage("Loaded sector info in " + stopwatch.Elapsed.TotalSeconds);
			}
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00045F28 File Offset: 0x00044128
		public static void SaveVertexData(VertexPositionTexture[] wallVerts, VertexPositionTexture[] groundVerts, string fileName)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(VertexPositionTexture[]));
			FileStream fileStream = new FileStream(fileName + "walls.xml", FileMode.Create);
			xmlSerializer.Serialize(fileStream, wallVerts);
			Terminal.WriteMessage("Saved Walls in Level: " + fileName + "walls.xml", MessageType.SAVELOAD);
			fileStream.Close();
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(VertexPositionTexture[]));
			fileStream = new FileStream(fileName + "ground.xml", FileMode.Create);
			xmlSerializer2.Serialize(fileStream, groundVerts);
			Terminal.WriteMessage("Saved Ground in Level: " + fileName + "ground.xml", MessageType.SAVELOAD);
			fileStream.Close();
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00045FC0 File Offset: 0x000441C0
		public static void LoadWallVertexData(out VertexPositionTexture[] wallVerts, string fileName)
		{
			wallVerts = null;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(VertexPositionTexture[]));
			FileStream fileStream = new FileStream(fileName + "walls.xml", FileMode.Open);
			wallVerts = (VertexPositionTexture[])xmlSerializer.Deserialize(fileStream);
			Terminal.WriteMessage("Loaded Level: " + fileName, MessageType.SAVELOAD);
			fileStream.Close();
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00046018 File Offset: 0x00044218
		public static void LoadGroundVertexData(out VertexPositionTexture[] groundVerts, string fileName)
		{
			groundVerts = null;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(VertexPositionTexture[]));
			FileStream fileStream = new FileStream(fileName + "ground.xml", FileMode.Open);
			groundVerts = (VertexPositionTexture[])xmlSerializer.Deserialize(fileStream);
			Terminal.WriteMessage("Loaded Level: " + fileName, MessageType.SAVELOAD);
			fileStream.Close();
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00002EF9 File Offset: 0x000010F9
		public static void threadLoad()
		{
		}

		// Token: 0x04000909 RID: 2313
		private static bool SAVEPATH = false;

		// Token: 0x0400090A RID: 2314
		private static object LOCK = new object();
	}
}
