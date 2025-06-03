using System;
using System.IO;
using System.Xml.Serialization;
using ProtoBuf;

namespace ZombieEstate2
{
	// Token: 0x02000051 RID: 81
	public static class XMLSaverLoader
	{
		// Token: 0x060001E1 RID: 481 RVA: 0x0000DA30 File Offset: 0x0000BC30
		public static bool SaveObject<T>(string fileName, T obj)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			StreamWriter streamWriter = new StreamWriter(fileName);
			xmlSerializer.Serialize(streamWriter, obj);
			streamWriter.Close();
			return true;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000DA68 File Offset: 0x0000BC68
		public static bool LoadObject<T>(string fileName, out T obj)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			StreamReader streamReader = new StreamReader(fileName);
			obj = (T)((object)xmlSerializer.Deserialize(streamReader));
			streamReader.Close();
			return true;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000DAA8 File Offset: 0x0000BCA8
		public static void SaveProto<T>(string fileName, T obj)
		{
			using (FileStream fileStream = File.Create(fileName + ".bin"))
			{
				Serializer.Serialize<T>(fileStream, obj);
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000DAEC File Offset: 0x0000BCEC
		public static void LoadProto<T>(string fileName, out T obj)
		{
			using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read))
			{
				obj = Serializer.Deserialize<T>(fileStream);
			}
		}
	}
}
