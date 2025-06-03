using System;
using System.Collections.Generic;

namespace ZombieEstate2
{
	// Token: 0x020000AB RID: 171
	public static class StatusEffectLoader
	{
		// Token: 0x0600045F RID: 1119 RVA: 0x00002EF9 File Offset: 0x000010F9
		public static void LoadStatusEffects()
		{
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0002036B File Offset: 0x0001E56B
		public static StatusEffect GetStatusEffect(string type, GameObject parent, StatusEffect[] effects, Bullet bul, GameObject attacker)
		{
			return null;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00020370 File Offset: 0x0001E570
		private static StatusEffect LoadEff(string EffName)
		{
			StatusEffect result;
			XMLSaverLoader.LoadObject<StatusEffect>(".\\Data\\StatusEffects\\" + EffName + ".eff", out result);
			Terminal.WriteMessage("STAT: |" + EffName + "| Loaded.", MessageType.SAVELOAD);
			return result;
		}

		// Token: 0x0400044E RID: 1102
		private static Dictionary<string, StatusEffect> statusEffects = new Dictionary<string, StatusEffect>();
	}
}
