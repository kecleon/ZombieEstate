using System;
using System.Collections.Generic;
using WG;
using ZombieEstate2.Wave;

namespace ZombieEstate2
{
	// Token: 0x020000F6 RID: 246
	public class WaveGenerator
	{
		// Token: 0x0600066F RID: 1647 RVA: 0x0003000C File Offset: 0x0002E20C
		public WaveGenerator(string levelName, Random rand)
		{
			ZombieWrapperGen zombieWrapperGen = new ZombieWrapperGen(rand.Next());
			this.mRandom = rand;
			this.pg = new PercentGenerator(zombieWrapperGen.GetWrapper());
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00030050 File Offset: 0x0002E250
		public List<WaveBase> Generate(int count, float startPercent)
		{
			this.DifficultyMod = 0.9f + (float)(Global.PlayerList.Count - 1);
			if (this.DifficultyMod < 0.9f)
			{
				this.DifficultyMod = 0.9f;
			}
			Terminal.WriteMessage(string.Concat(new object[]
			{
				"Generating ",
				count,
				" waves with Difficulty of ",
				this.DifficultyMod
			}), MessageType.IMPORTANTEVENT);
			Terminal.WriteMessage(++WaveGenerator.RAND_TEST_NUM + " | RAND TEST: " + this.mRandom.Next(), MessageType.IMPORTANTEVENT);
			this.Percent = startPercent;
			List<WaveBase> list = new List<WaveBase>();
			WaveBase item = new WaveBase();
			for (int i = 0; i < count; i++)
			{
				if (!Global.UnlimitedMode && i == count - 1)
				{
					this.Percent += 0.1f;
				}
				string wave = this.GetWave(this.Percent);
				if (wave == "KillWave")
				{
					item = this.GetKillWave(this.Percent);
				}
				if (wave == "AirDropWave")
				{
					item = this.GetAirDropWave(this.Percent);
				}
				if (wave == "RescueWave")
				{
					item = this.GetRescueWave(this.Percent);
				}
				list.Add(item);
				this.Percent += 0.05f;
			}
			foreach (WaveBase waveBase in list)
			{
				foreach (ZombieType key in waveBase.Weights.Keys)
				{
					Terminal.WriteMessage(key.ToString() + ": " + waveBase.Weights[key].ToString());
				}
				Terminal.WriteMessage("------------");
			}
			return list;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00030270 File Offset: 0x0002E470
		private KillWave GetKillWave(float percent)
		{
			KillWave killWave = new KillWave();
			this.GetWaveWeights(killWave, this.Percent);
			int num = (int)((percent * 800f + 25f) * this.DifficultyMod);
			float diffModForTypes = this.GetDiffModForTypes(killWave);
			killWave.KillsToWin = (int)((float)num * diffModForTypes);
			this.UpdateNormalZombieStuff(killWave, percent);
			return killWave;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x000302C4 File Offset: 0x0002E4C4
		private AirDropWave GetAirDropWave(float percent)
		{
			AirDropWave airDropWave = new AirDropWave();
			this.GetWaveWeights(airDropWave, this.Percent);
			airDropWave.Seconds = 120 + (int)(180f * percent);
			airDropWave.TotalDropsNeeded = (int)(20f * percent) + 5;
			this.UpdateNormalZombieStuff(airDropWave, percent);
			return airDropWave;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00030310 File Offset: 0x0002E510
		private RescueWave GetRescueWave(float percent)
		{
			RescueWave rescueWave = new RescueWave();
			this.GetWaveWeights(rescueWave, this.Percent);
			rescueWave.Seconds = 120 + (int)(180f * percent);
			rescueWave.TotalToRescue = (int)(10f * percent) + 3;
			this.UpdateNormalZombieStuff(rescueWave, percent);
			return rescueWave;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0003035C File Offset: 0x0002E55C
		private void UpdateNormalZombieStuff(WaveBase wave, float percent)
		{
			wave.ZombiesPerSecond = Global.PlayerList.Count + 1 + (int)(percent * 8f);
			wave.ZombiesPerSecond = Math.Min(wave.ZombiesPerSecond, 12);
			int num = (int)(percent * 400f + 15f);
			if (num > 440)
			{
				num = 440;
			}
			float num2 = 1f;
			num2 += 0.25f * (float)(Global.PlayerList.Count - 1);
			if (Global.PlayerList.Count == 1)
			{
				num2 = 1f;
			}
			float diffModForTypes = this.GetDiffModForTypes(wave);
			wave.MaxNumberOfZombiesOnScreen = (int)((float)num * diffModForTypes * this.DifficultyMod * num2);
			Terminal.WriteMessage(string.Concat(new object[]
			{
				"%",
				percent * 100f,
				": NumZombiesOnScreen:",
				wave.MaxNumberOfZombiesOnScreen,
				" | Mod: ",
				diffModForTypes
			}));
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00030450 File Offset: 0x0002E650
		private float GetDiffModForTypes(WaveBase wave)
		{
			float num = 0f;
			foreach (ZombieType key in wave.Weights.Keys)
			{
				if (Zombie.PercentMod.ContainsKey(key))
				{
					num += Zombie.PercentMod[key] * ((float)wave.Weights[key] / 100f);
				}
			}
			if (num < 0.15f)
			{
				num = 0.15f;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x000304F4 File Offset: 0x0002E6F4
		private void GetWaveWeights(WaveBase wave, float percent)
		{
			Dictionary<string, double> dictionary = new Dictionary<string, double>();
			dictionary = this.pg.calculateWave((double)percent, this.mRandom);
			Console.WriteLine("Percent: " + percent.ToString());
			foreach (string text in dictionary.Keys)
			{
				Console.WriteLine(text + " : " + dictionary[text]);
			}
			Console.WriteLine("-----------");
			Dictionary<ZombieType, int> dictionary2 = new Dictionary<ZombieType, int>();
			int num = 0;
			ZombieType zombieType = ZombieType.NOTHING;
			foreach (string text2 in dictionary.Keys)
			{
				ZombieType zombieType2 = (ZombieType)Enum.Parse(typeof(ZombieType), text2, true);
				if (zombieType == ZombieType.NOTHING)
				{
					zombieType = zombieType2;
				}
				int num2 = (int)dictionary[text2];
				dictionary2.Add(zombieType2, num2);
				num += num2;
			}
			int num3 = 100 - num;
			Dictionary<ZombieType, int> dictionary3 = dictionary2;
			ZombieType key = zombieType;
			dictionary3[key] += num3;
			wave.Weights = dictionary2;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00030648 File Offset: 0x0002E848
		private string GetWave(float percent)
		{
			return "KillWave";
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0003064F File Offset: 0x0002E84F
		private List<string> GetValidWaveTypes()
		{
			return new List<string>
			{
				"KillWave",
				"AirDropWave",
				"RescueWave"
			};
		}

		// Token: 0x04000651 RID: 1617
		private float DifficultyMod = 1f;

		// Token: 0x04000652 RID: 1618
		private float Percent;

		// Token: 0x04000653 RID: 1619
		private int OtherCount;

		// Token: 0x04000654 RID: 1620
		private bool FirstBossAdded;

		// Token: 0x04000655 RID: 1621
		private PercentGenerator pg;

		// Token: 0x04000656 RID: 1622
		private static int RAND_TEST_NUM;

		// Token: 0x04000657 RID: 1623
		private Random mRandom;
	}
}
