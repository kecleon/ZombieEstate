using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000098 RID: 152
	public static class TalentManager
	{
		// Token: 0x060003F7 RID: 1015 RVA: 0x0001C6EC File Offset: 0x0001A8EC
		public static List<Talent> GetTalents(CharacterSettings settings)
		{
			List<Talent> list = new List<Talent>();
			foreach (Talent item in TalentManager.GenericTalents)
			{
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001C748 File Offset: 0x0001A948
		public static void ApplyTalent(Talent talent, Player parent)
		{
			if (talent.CurrentLevel >= 3)
			{
				Terminal.WriteMessage("ERROR: TALENT FULLY LEVELED!", MessageType.ERROR);
				return;
			}
			talent.CurrentLevel++;
			int num = talent.CurrentLevel - 1;
			if (talent.AbilityClassName != null && talent.AbilityClassName != string.Empty)
			{
				Ability ability = (Ability)Activator.CreateInstance(Type.GetType(talent.AbilityClassName), new object[]
				{
					parent,
					num
				});
				parent.Ability = ability;
				return;
			}
			if (talent.Attribute == null || !(talent.Attribute != string.Empty))
			{
				return;
			}
			if (talent.Attribute == "Health")
			{
				return;
			}
			if (talent.Attribute == "Speed")
			{
				return;
			}
			if (talent.Attribute == "Reload Speed")
			{
				parent.ReloadSpeedMod = talent.Modifiers[num];
				return;
			}
			if (talent.Attribute == "Money Boost")
			{
				parent.ZombieKillMoneyMod = talent.Modifiers[num];
				return;
			}
			if (talent.Attribute == "Assault Storage")
			{
				parent.Stats.AddMaxAmmo(AmmoType.ASSAULT, (int)talent.Modifiers[num]);
				return;
			}
			if (talent.Attribute == "Heavy Storage")
			{
				parent.Stats.AddMaxAmmo(AmmoType.HEAVY, (int)talent.Modifiers[num]);
				return;
			}
			if (talent.Attribute == "Explosive Storage")
			{
				parent.Stats.AddMaxAmmo(AmmoType.EXPLOSIVE, (int)talent.Modifiers[num]);
				return;
			}
			if (talent.Attribute == "Shells Storage")
			{
				parent.Stats.AddMaxAmmo(AmmoType.SHELLS, (int)talent.Modifiers[num]);
				return;
			}
			if (talent.Attribute == "MinionCount")
			{
				parent.MinionCount++;
				return;
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001C918 File Offset: 0x0001AB18
		public static void AddGenericTalents()
		{
			TalentManager.GenericTalents.Clear();
			Talent talent = new Talent();
			talent.Attribute = "Health";
			talent.CurrentLevel = 0;
			talent.Description = new string[3];
			talent.Description[0] = "Increase the player's total health by 20.";
			talent.Description[1] = "Increase the player's total health by another 30.";
			talent.Description[2] = "Increase the player's total health by another 50.";
			talent.Modifiers = new float[3];
			talent.Modifiers[0] = 20f;
			talent.Modifiers[1] = 30f;
			talent.Modifiers[2] = 50f;
			talent.Name = "Health Boost";
			talent.TexCoord = new Point(62, 49);
			TalentManager.GenericTalents.Add(talent);
			talent = new Talent();
			talent.Attribute = "Speed";
			talent.CurrentLevel = 0;
			talent.Description = new string[3];
			talent.Description[0] = "Increase the player's speed by 2.5%.";
			talent.Description[1] = "Increase the player's speed by 5%.";
			talent.Description[2] = "Increase the player's speed by 10%.";
			talent.Modifiers = new float[3];
			talent.Modifiers[0] = 1.025f;
			talent.Modifiers[1] = 1.05f;
			talent.Modifiers[2] = 1.1f;
			talent.Name = "Walk Speed";
			talent.TexCoord = new Point(62, 48);
			TalentManager.GenericTalents.Add(talent);
			talent = new Talent();
			talent.Attribute = "Reload Speed";
			talent.CurrentLevel = 0;
			talent.Description = new string[3];
			talent.Description[0] = "Allows the player to reload 10% faster.";
			talent.Description[1] = "Allows the player to reload 20% faster.";
			talent.Description[2] = "Allows the player to reload 30% faster.";
			talent.Modifiers = new float[3];
			talent.Modifiers[0] = 0.9f;
			talent.Modifiers[1] = 0.8f;
			talent.Modifiers[2] = 0.7f;
			talent.Name = "Reload Speed";
			talent.TexCoord = new Point(62, 50);
			TalentManager.GenericTalents.Add(talent);
			talent = new Talent();
			talent.Attribute = "Money Boost";
			talent.CurrentLevel = 0;
			talent.Description = new string[3];
			talent.Description[0] = "Gain an extra 5% money from zombie kills.";
			talent.Description[1] = "Gain an extra 10% money from zombie kills.";
			talent.Description[2] = "Gain an extra 15% money from zombie kills.";
			talent.Modifiers = new float[3];
			talent.Modifiers[0] = 1.05f;
			talent.Modifiers[1] = 1.1f;
			talent.Modifiers[2] = 1.15f;
			talent.Name = "Money Boost";
			talent.TexCoord = new Point(62, 51);
			TalentManager.GenericTalents.Add(talent);
			talent = new Talent();
			talent.Attribute = "Assault Storage";
			talent.CurrentLevel = 0;
			talent.Description = new string[3];
			talent.Description[0] = "Hold an additional 75 Assault Ammo.";
			talent.Description[1] = "Hold an additional 150 Assault Ammo.";
			talent.Description[2] = "Hold an additional 300 Assault Ammo.";
			talent.Modifiers = new float[3];
			talent.Modifiers[0] = 75f;
			talent.Modifiers[1] = 75f;
			talent.Modifiers[2] = 150f;
			talent.Name = "Assault Storage";
			talent.TexCoord = new Point(62, 52);
			TalentManager.GenericTalents.Add(talent);
			talent = new Talent();
			talent.Attribute = "Shells Storage";
			talent.CurrentLevel = 0;
			talent.Description = new string[3];
			talent.Description[0] = "Hold an additional 20 Shells Ammo.";
			talent.Description[1] = "Hold an additional 40 Shells Ammo.";
			talent.Description[2] = "Hold an additional 80 Shells Ammo.";
			talent.Modifiers = new float[3];
			talent.Modifiers[0] = 20f;
			talent.Modifiers[1] = 20f;
			talent.Modifiers[2] = 40f;
			talent.Name = "Shells Storage";
			talent.TexCoord = new Point(62, 53);
			TalentManager.GenericTalents.Add(talent);
			talent = new Talent();
			talent.Attribute = "Heavy Storage";
			talent.CurrentLevel = 0;
			talent.Description = new string[3];
			talent.Description[0] = "Hold an additional 100 Heavy Ammo.";
			talent.Description[1] = "Hold an additional 200 Heavy Ammo.";
			talent.Description[2] = "Hold an additional 400 Heavy Ammo.";
			talent.Modifiers = new float[3];
			talent.Modifiers[0] = 100f;
			talent.Modifiers[1] = 100f;
			talent.Modifiers[2] = 200f;
			talent.Name = "Heavy Storage";
			talent.TexCoord = new Point(63, 52);
			TalentManager.GenericTalents.Add(talent);
			talent = new Talent();
			talent.Attribute = "Explosive Storage";
			talent.CurrentLevel = 0;
			talent.Description = new string[3];
			talent.Description[0] = "Hold an additional 4 Explosive Ammo.";
			talent.Description[1] = "Hold an additional 8 Explosive Ammo.";
			talent.Description[2] = "Hold an additional 16 Explosive Ammo.";
			talent.Modifiers = new float[3];
			talent.Modifiers[0] = 4f;
			talent.Modifiers[1] = 4f;
			talent.Modifiers[2] = 8f;
			talent.Name = "Explosive Storage";
			talent.TexCoord = new Point(63, 53);
			TalentManager.GenericTalents.Add(talent);
			talent = new Talent();
			talent.Attribute = "MinionCount";
			talent.CurrentLevel = 0;
			talent.Description = new string[3];
			talent.Description[0] = "Drop 1 additional minion/turret. Note: You can not drop more than 1 of each type.";
			talent.Description[1] = "Drop 2 additional minions/turrets. Note: You can not drop more than 1 of each type.";
			talent.Description[2] = "Drop 3 additional minions/turrets. Note: You can not drop more than 1 of each type.";
			talent.Modifiers = new float[3];
			talent.Modifiers[0] = 4f;
			talent.Modifiers[1] = 4f;
			talent.Modifiers[2] = 8f;
			talent.Name = "Minion Master";
			talent.TexCoord = new Point(63, 51);
			TalentManager.GenericTalents.Add(talent);
		}

		// Token: 0x040003C1 RID: 961
		private static List<Talent> GenericTalents = new List<Talent>();
	}
}
