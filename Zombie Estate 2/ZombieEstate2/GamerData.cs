using System;
using System.Xml.Serialization;

namespace ZombieEstate2
{
	// Token: 0x0200008B RID: 139
	public class GamerData
	{
		// Token: 0x0600039A RID: 922 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void LoadData()
		{
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void SaveData()
		{
		}

		// Token: 0x0400035D RID: 861
		public string GamerName;

		// Token: 0x0400035E RID: 862
		[XmlIgnore]
		public Player parent;
	}
}
