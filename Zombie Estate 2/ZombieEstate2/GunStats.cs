using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using ProtoBuf;

namespace ZombieEstate2
{
	// Token: 0x02000136 RID: 310
	[ProtoContract]
	public class GunStats : IComparable
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x0004A85D File Offset: 0x00048A5D
		// (set) Token: 0x060008E4 RID: 2276 RVA: 0x0004A865 File Offset: 0x00048A65
		[Category("0")]
		[ProtoMember(1)]
		public string GunName { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0004A86E File Offset: 0x00048A6E
		// (set) Token: 0x060008E6 RID: 2278 RVA: 0x0004A876 File Offset: 0x00048A76
		[Category("0")]
		[ProtoMember(2)]
		public string StoreDescription { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0004A87F File Offset: 0x00048A7F
		// (set) Token: 0x060008E8 RID: 2280 RVA: 0x0004A887 File Offset: 0x00048A87
		[Category("0")]
		[ProtoMember(3)]
		public int Cost { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0004A890 File Offset: 0x00048A90
		// (set) Token: 0x060008EA RID: 2282 RVA: 0x0004A898 File Offset: 0x00048A98
		[Category("0")]
		[ProtoMember(4)]
		public bool Minion { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0004A8A1 File Offset: 0x00048AA1
		// (set) Token: 0x060008EC RID: 2284 RVA: 0x0004A8A9 File Offset: 0x00048AA9
		[Category("Ammo")]
		[ProtoMember(5)]
		public AmmoType AmmoType { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x0004A8B2 File Offset: 0x00048AB2
		// (set) Token: 0x060008EE RID: 2286 RVA: 0x0004A8BA File Offset: 0x00048ABA
		[Category("Appearance")]
		[ProtoMember(6)]
		public int GunXCoord { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x0004A8C3 File Offset: 0x00048AC3
		// (set) Token: 0x060008F0 RID: 2288 RVA: 0x0004A8CB File Offset: 0x00048ACB
		[Category("Appearance")]
		[ProtoMember(7)]
		public int GunYCoord { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x0004A8D4 File Offset: 0x00048AD4
		// (set) Token: 0x060008F2 RID: 2290 RVA: 0x0004A8DC File Offset: 0x00048ADC
		[Category("Properties")]
		[ProtoMember(8)]
		public List<GunProperties> GunProperties { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x0004A8E5 File Offset: 0x00048AE5
		// (set) Token: 0x060008F4 RID: 2292 RVA: 0x0004A8ED File Offset: 0x00048AED
		[Category("Properties")]
		[ProtoMember(9)]
		public List<SpecialProperties> SpecialProperties { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0004A8F6 File Offset: 0x00048AF6
		// (set) Token: 0x060008F6 RID: 2294 RVA: 0x0004A8FE File Offset: 0x00048AFE
		[Category("Properties")]
		[ProtoMember(10)]
		public bool FireEvenly { get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x0004A907 File Offset: 0x00048B07
		// (set) Token: 0x060008F8 RID: 2296 RVA: 0x0004A90F File Offset: 0x00048B0F
		[Category("Appearance")]
		[ProtoMember(11)]
		public string ParticleOnFire { get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x0004A918 File Offset: 0x00048B18
		// (set) Token: 0x060008FA RID: 2298 RVA: 0x0004A920 File Offset: 0x00048B20
		[Category("Appearance")]
		[ProtoMember(12)]
		public int ParticleCountOnFire { get; set; }

		// Token: 0x060008FC RID: 2300 RVA: 0x0004A92C File Offset: 0x00048B2C
		public void InitEditer()
		{
			this.GunName = "Name";
			this.StoreDescription = "Store Description...";
			this.AmmoType = AmmoType.ASSAULT;
			this.GunProperties = new List<GunProperties>();
			this.SpecialProperties = new List<SpecialProperties>();
			for (int i = 0; i < 4; i++)
			{
				this.GunProperties.Add(new GunProperties());
				this.SpecialProperties.Add(new SpecialProperties());
			}
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0004A998 File Offset: 0x00048B98
		public Point GetOrigin(int level)
		{
			return new Point(this.GunXCoord + 2 * level, this.GunYCoord);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0004A9B0 File Offset: 0x00048BB0
		public int CompareTo(object obj)
		{
			return this.Cost.CompareTo((obj as GunStats).Cost);
		}
	}
}
