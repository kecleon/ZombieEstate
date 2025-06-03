using System;
using System.ComponentModel;
using ProtoBuf;

namespace ZombieEstate2
{
	// Token: 0x02000137 RID: 311
	[ProtoContract]
	public class GunProperties
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x0004A9D6 File Offset: 0x00048BD6
		// (set) Token: 0x06000900 RID: 2304 RVA: 0x0004A9DE File Offset: 0x00048BDE
		[Category("Damage")]
		[ProtoMember(1)]
		public int MinDamage { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x0004A9E7 File Offset: 0x00048BE7
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x0004A9EF File Offset: 0x00048BEF
		[Category("Damage")]
		[ProtoMember(2)]
		public int MaxDamage { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x0004A9F8 File Offset: 0x00048BF8
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x0004AA00 File Offset: 0x00048C00
		[Category("Damage")]
		[ProtoMember(3)]
		public float PushBack { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x0004AA09 File Offset: 0x00048C09
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x0004AA11 File Offset: 0x00048C11
		[Category("Damage")]
		[ProtoMember(4)]
		public DamageType DamageType { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x0004AA1A File Offset: 0x00048C1A
		// (set) Token: 0x06000908 RID: 2312 RVA: 0x0004AA22 File Offset: 0x00048C22
		[ProtoMember(5)]
		public string LevelDescription { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x0004AA2B File Offset: 0x00048C2B
		// (set) Token: 0x0600090A RID: 2314 RVA: 0x0004AA33 File Offset: 0x00048C33
		[Category("Bullet")]
		[ProtoMember(6)]
		public string BulletType { get; set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x0004AA3C File Offset: 0x00048C3C
		// (set) Token: 0x0600090C RID: 2316 RVA: 0x0004AA44 File Offset: 0x00048C44
		[Category("Minion")]
		[ProtoMember(7)]
		public string MinionGunStats { get; set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x0004AA4D File Offset: 0x00048C4D
		// (set) Token: 0x0600090E RID: 2318 RVA: 0x0004AA55 File Offset: 0x00048C55
		[Category("Minion")]
		[ProtoMember(8)]
		public ZEPoint MinionTexture { get; set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x0004AA5E File Offset: 0x00048C5E
		// (set) Token: 0x06000910 RID: 2320 RVA: 0x0004AA66 File Offset: 0x00048C66
		[Category("Minion")]
		[ProtoMember(9)]
		public float MinionMoveSpeed { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x0004AA6F File Offset: 0x00048C6F
		// (set) Token: 0x06000912 RID: 2322 RVA: 0x0004AA77 File Offset: 0x00048C77
		[Category("Minion")]
		[ProtoMember(10)]
		public int MinionAmmo { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x0004AA80 File Offset: 0x00048C80
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x0004AA88 File Offset: 0x00048C88
		[Category("Minion")]
		[ProtoMember(11)]
		public bool MinionMuzzleFlash { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0004AA91 File Offset: 0x00048C91
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x0004AA99 File Offset: 0x00048C99
		[Category("Minion")]
		[ProtoMember(12)]
		public int MinionHealth_AmmoCount { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x0004AAA2 File Offset: 0x00048CA2
		// (set) Token: 0x06000918 RID: 2328 RVA: 0x0004AAAA File Offset: 0x00048CAA
		[Category("Minion")]
		[ProtoMember(13)]
		public string MinionBuff { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x0004AAB3 File Offset: 0x00048CB3
		// (set) Token: 0x0600091A RID: 2330 RVA: 0x0004AABB File Offset: 0x00048CBB
		[Category("Minion")]
		[ProtoMember(14)]
		public string MinionBuffArgs { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x0004AAC4 File Offset: 0x00048CC4
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x0004AACC File Offset: 0x00048CCC
		[Category("Properties")]
		[ProtoMember(15)]
		public float ReloadTime { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x0004AAD5 File Offset: 0x00048CD5
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x0004AADD File Offset: 0x00048CDD
		[Category("Properties")]
		[ProtoMember(16)]
		public float ShotTime { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x0004AAE6 File Offset: 0x00048CE6
		// (set) Token: 0x06000920 RID: 2336 RVA: 0x0004AAEE File Offset: 0x00048CEE
		[Category("Properties")]
		[ProtoMember(17)]
		public float Accuracy { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x0004AAF7 File Offset: 0x00048CF7
		// (set) Token: 0x06000922 RID: 2338 RVA: 0x0004AAFF File Offset: 0x00048CFF
		[Category("Properties")]
		[ProtoMember(18)]
		public int BulletsInClip { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x0004AB08 File Offset: 0x00048D08
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x0004AB10 File Offset: 0x00048D10
		[Category("Properties")]
		[ProtoMember(19)]
		public int BulletsCostToFire { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x0004AB19 File Offset: 0x00048D19
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x0004AB21 File Offset: 0x00048D21
		[Category("Properties")]
		[ProtoMember(20)]
		public int NumberOfBulletsFired { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x0004AB2A File Offset: 0x00048D2A
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x0004AB32 File Offset: 0x00048D32
		[Category("Bullet")]
		[ProtoMember(21)]
		public int BounceCount { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x0004AB3B File Offset: 0x00048D3B
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x0004AB43 File Offset: 0x00048D43
		[Category("Bullet")]
		[ProtoMember(22)]
		public int BounceChance { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x0004AB4C File Offset: 0x00048D4C
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x0004AB54 File Offset: 0x00048D54
		[Category("Bullet")]
		[ProtoMember(23)]
		public int PenetrationChance { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x0004AB5D File Offset: 0x00048D5D
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x0004AB65 File Offset: 0x00048D65
		[Category("Sound")]
		[ProtoMember(24)]
		public string SoundName { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0004AB6E File Offset: 0x00048D6E
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x0004AB76 File Offset: 0x00048D76
		[Category("Sound")]
		[ProtoMember(25)]
		public float PitchModHigh { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x0004AB7F File Offset: 0x00048D7F
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x0004AB87 File Offset: 0x00048D87
		[Category("Sound")]
		[ProtoMember(26)]
		public float PitchModLow { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x0004AB90 File Offset: 0x00048D90
		// (set) Token: 0x06000934 RID: 2356 RVA: 0x0004AB98 File Offset: 0x00048D98
		[Category("Melee")]
		[ProtoMember(27)]
		public int MeleeTexX { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x0004ABA1 File Offset: 0x00048DA1
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x0004ABA9 File Offset: 0x00048DA9
		[Category("Melee")]
		[ProtoMember(28)]
		public int MeleeTexY { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x0004ABB2 File Offset: 0x00048DB2
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x0004ABBA File Offset: 0x00048DBA
		[Category("OnHit")]
		[ProtoMember(29)]
		public string Buff { get; set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0004ABC3 File Offset: 0x00048DC3
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x0004ABCB File Offset: 0x00048DCB
		[Category("OnHit")]
		[ProtoMember(30)]
		public string BuffArgs { get; set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0004ABD4 File Offset: 0x00048DD4
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x0004ABDC File Offset: 0x00048DDC
		[Category("OnHit")]
		[ProtoMember(31)]
		public string Explosion { get; set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x0004ABE5 File Offset: 0x00048DE5
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x0004ABED File Offset: 0x00048DED
		[Category("OnHit")]
		[ProtoMember(32)]
		public float ExplosionRadius { get; set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x0004ABF6 File Offset: 0x00048DF6
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x0004ABFE File Offset: 0x00048DFE
		[Category("OnHit")]
		[ProtoMember(33)]
		public int ExplosionDamage { get; set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x0004AC07 File Offset: 0x00048E07
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x0004AC0F File Offset: 0x00048E0F
		[Category("OnHit")]
		[ProtoMember(34)]
		public int ExplosionPushBack { get; set; }
	}
}
