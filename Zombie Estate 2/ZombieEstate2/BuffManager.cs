using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Buffs;

namespace ZombieEstate2
{
	// Token: 0x02000014 RID: 20
	public class BuffManager
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00003585 File Offset: 0x00001785
		public BuffManager(GameObject parent)
		{
			this.Parent = parent;
			this.Buffs = new List<Buff>();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000035A0 File Offset: 0x000017A0
		public void Update(float elapsed)
		{
			for (int i = 0; i < this.Buffs.Count; i++)
			{
				this.Buffs[i].Update(elapsed);
				if (this.Buffs.Count == 0)
				{
					break;
				}
				if (this.Buffs[i].Expired)
				{
					this.RemoveBuff(this.Buffs[i]);
				}
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003608 File Offset: 0x00001808
		public void ParentKilled()
		{
			foreach (Buff buff in this.Buffs)
			{
				buff.Destroyed();
			}
			this.Buffs.Clear();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Draw(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003664 File Offset: 0x00001864
		public void TakenDamage(Shootable attacker)
		{
			foreach (Buff buff in this.Buffs)
			{
				buff.TakenDamage(attacker);
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000036B8 File Offset: 0x000018B8
		public void DeltDamage(Shootable target)
		{
			foreach (Buff buff in this.Buffs)
			{
				buff.DeltDamage(target);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000370C File Offset: 0x0000190C
		public void KilledTarget(Shootable target)
		{
			foreach (Buff buff in this.Buffs)
			{
				buff.KilledATarget(target);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003760 File Offset: 0x00001960
		public float DamageMod()
		{
			float num = 1f;
			foreach (Buff buff in this.Buffs)
			{
				num = buff.ApplyDamageMod(num);
			}
			return num;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000037BC File Offset: 0x000019BC
		public float HurtMod()
		{
			float num = 1f;
			foreach (Buff buff in this.Buffs)
			{
				num = buff.ApplyHurtMod(num);
			}
			return num;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003818 File Offset: 0x00001A18
		public float SpeedMod()
		{
			float num = 1f;
			foreach (Buff buff in this.Buffs)
			{
				num = buff.ApplySpeedMod(num);
			}
			return num;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003874 File Offset: 0x00001A74
		public bool UpdateSpec(ref SpecialProperties spec)
		{
			bool result = false;
			SpecialProperties.ClearProps(ref spec);
			using (List<Buff>.Enumerator enumerator = this.Buffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.UpdateSpec(ref spec))
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000038D4 File Offset: 0x00001AD4
		public void AddBuff(string name, Shootable attacker, string args)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			Buff buff = (Buff)Activator.CreateInstance(Type.GetType("ZombieEstate2.Buffs.Types." + name));
			if (buff == null)
			{
				Console.WriteLine("ERROR: No buff name found for '" + name + "'");
			}
			buff.Init(this.Parent as Shootable, attacker);
			buff.GiveArguments(args);
			this.AddBuff(buff);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003940 File Offset: 0x00001B40
		public void AddBuff(Buff buff)
		{
			if (this.Buffs.Count >= BuffManager.MAX)
			{
				return;
			}
			Buff buff2 = this.FindBuff(buff.Name);
			if (buff2 != null)
			{
				if (buff2.CanNotBeOverwritten)
				{
					buff.Destroyed();
					return;
				}
				int index = this.Buffs.IndexOf(buff2);
				this.Buffs.Remove(buff2);
				buff2.Destroyed();
				this.Buffs.Insert(index, buff);
			}
			else
			{
				this.Buffs.Add(buff);
			}
			Shootable shootable = this.Parent as Shootable;
			if (shootable != null)
			{
				shootable.SomethingChanged = true;
			}
			buff.Applied();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000039D8 File Offset: 0x00001BD8
		public void ClearBuffs()
		{
			this.Buffs.Clear();
			Shootable shootable = this.Parent as Shootable;
			if (shootable != null)
			{
				shootable.SomethingChanged = true;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003A08 File Offset: 0x00001C08
		public void RemoveBuff(Buff buff)
		{
			if (buff == null)
			{
				Console.WriteLine("ERROR: buff was null...");
				return;
			}
			Buff buff2 = this.FindBuff(buff.Name);
			if (buff2 != null)
			{
				buff2.Destroyed();
				buff2.Removed();
				this.Buffs.Remove(buff2);
				Shootable shootable = this.Parent as Shootable;
				if (shootable != null)
				{
					shootable.SomethingChanged = true;
				}
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003A62 File Offset: 0x00001C62
		public void RemoveBuff(string name)
		{
			this.RemoveBuff(this.FindBuff(name));
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003A74 File Offset: 0x00001C74
		public Buff FindBuff(string name)
		{
			foreach (Buff buff in this.Buffs)
			{
				if (buff.Name == name)
				{
					return buff;
				}
			}
			return null;
		}

		// Token: 0x04000035 RID: 53
		public GameObject Parent;

		// Token: 0x04000036 RID: 54
		public List<Buff> Buffs;

		// Token: 0x04000037 RID: 55
		private static int MAX = 10;
	}
}
