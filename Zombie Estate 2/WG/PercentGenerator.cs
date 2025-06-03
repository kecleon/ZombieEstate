using System;
using System.Collections.Generic;
using System.Linq;

namespace WG
{
	// Token: 0x02000003 RID: 3
	public class PercentGenerator
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		public PercentGenerator(List<ZombieWrapper> zombieWrappersArgument)
		{
			this.mZombieWrappers = zombieWrappersArgument;
			this.mZombiesSeen = new List<string>();
			this.mReminderList = new List<ZombieWrapper>();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000207C File Offset: 0x0000027C
		private List<ZombieWrapper> getAvailableZombiesForWave(double percentComplete)
		{
			List<ZombieWrapper> list = new List<ZombieWrapper>();
			foreach (ZombieWrapper zombieWrapper in this.mZombieWrappers)
			{
				if ((double)zombieWrapper.Start <= percentComplete * 100.0)
				{
					list.Add(zombieWrapper);
				}
			}
			if (list.Count<ZombieWrapper>() == 0)
			{
				throw new WGException("No Available Zombies.");
			}
			return list;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002100 File Offset: 0x00000300
		private Dictionary<string, double> amortizeDictionary(Dictionary<string, double> innerDictionary, double goal)
		{
			Dictionary<string, double> dictionary = new Dictionary<string, double>();
			double num = 0.0;
			foreach (string key in innerDictionary.Keys)
			{
				num += innerDictionary[key];
			}
			foreach (string key2 in innerDictionary.Keys)
			{
				dictionary.Add(key2, innerDictionary[key2] * goal / num);
			}
			return dictionary;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021B8 File Offset: 0x000003B8
		private Dictionary<string, double> calculateIntroWave(ZombieWrapper introZombieWrapper)
		{
			return new Dictionary<string, double>
			{
				{
					introZombieWrapper.Name,
					100.0
				}
			};
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021D4 File Offset: 0x000003D4
		private Dictionary<string, double> calculateNormalWave(List<ZombieWrapper> zombiesToAppear, Random random)
		{
			Dictionary<string, double> dictionary = new Dictionary<string, double>();
			int num = 0;
			int num2 = random.Next(2, 5);
			if (num2 > zombiesToAppear.Count)
			{
				num2 = zombiesToAppear.Count;
			}
			List<ZombieWrapper> list = new List<ZombieWrapper>();
			for (int i = 0; i < num2; i++)
			{
				int index = random.Next(0, zombiesToAppear.Count<ZombieWrapper>());
				list.Add(zombiesToAppear[index]);
				zombiesToAppear.RemoveAt(index);
			}
			foreach (ZombieWrapper zombieWrapper in list)
			{
				num += zombieWrapper.Minimum;
			}
			foreach (ZombieWrapper zombieWrapper2 in list)
			{
				dictionary.Add(zombieWrapper2.Name, (double)random.Next(Math.Max(zombieWrapper2.Standard - zombieWrapper2.Variation, zombieWrapper2.Minimum), zombieWrapper2.Standard + zombieWrapper2.Variation + 1));
			}
			return this.amortizeDictionary(dictionary, 100.0);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000230C File Offset: 0x0000050C
		private Dictionary<string, double> calculateCrazyWave(List<ZombieWrapper> availableZombies, Random random)
		{
			int num = availableZombies.Count<ZombieWrapper>();
			int num2 = random.Next(1, num + 1);
			int num3 = random.Next(1, num + 1);
			int num4 = num2 + num3 - num;
			if (num4 <= 0)
			{
				num4 *= -1;
				num4 += 2;
			}
			num4 = Math.Min(num4, 4);
			List<ZombieWrapper> list = new List<ZombieWrapper>();
			int num5 = 0;
			for (int i = 0; i < num4; i++)
			{
				int index = random.Next(0, availableZombies.Count<ZombieWrapper>());
				list.Add(availableZombies[index]);
				num5 += availableZombies[index].Minimum;
				availableZombies.RemoveAt(index);
			}
			int num6 = 100;
			int num7 = 0;
			Dictionary<string, double> dictionary = new Dictionary<string, double>();
			foreach (ZombieWrapper zombieWrapper in list)
			{
				num5 -= zombieWrapper.Minimum;
				int num8 = random.Next(zombieWrapper.Minimum, num6 - num7 - num5);
				num7 += num8;
				dictionary.Add(zombieWrapper.Name, (double)num8);
			}
			return this.amortizeDictionary(dictionary, (double)num6);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002430 File Offset: 0x00000630
		private ZombieWrapper fetchIntroZombie(Random random)
		{
			int index = random.Next(0, this.mReminderList.Count<ZombieWrapper>());
			ZombieWrapper zombieWrapper = this.mReminderList[index];
			this.mReminderList.RemoveAt(index);
			this.mZombiesSeen.Add(zombieWrapper.Name);
			return zombieWrapper;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000247C File Offset: 0x0000067C
		public Dictionary<string, double> calculateWave(double percentComplete, Random random)
		{
			if (this.mReminderList.Count != 0)
			{
				return this.calculateIntroWave(this.fetchIntroZombie(random));
			}
			List<ZombieWrapper> availableZombiesForWave = this.getAvailableZombiesForWave(percentComplete);
			foreach (ZombieWrapper zombieWrapper in availableZombiesForWave)
			{
				if (!this.mZombiesSeen.Contains(zombieWrapper.Name))
				{
					this.mReminderList.Add(zombieWrapper);
				}
			}
			if (this.mReminderList.Count != 0)
			{
				return this.calculateIntroWave(this.fetchIntroZombie(random));
			}
			if (random.Next(0, 100) >= 35)
			{
				return this.calculateNormalWave(availableZombiesForWave, random);
			}
			return this.calculateCrazyWave(availableZombiesForWave, random);
		}

		// Token: 0x04000001 RID: 1
		private List<ZombieWrapper> mZombieWrappers;

		// Token: 0x04000002 RID: 2
		private List<string> mZombiesSeen;

		// Token: 0x04000003 RID: 3
		private List<ZombieWrapper> mReminderList;
	}
}
