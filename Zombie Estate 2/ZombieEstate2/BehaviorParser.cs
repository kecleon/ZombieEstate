using System;
using System.Collections.Generic;
using System.Globalization;

namespace ZombieEstate2
{
	// Token: 0x02000012 RID: 18
	public static class BehaviorParser
	{
		// Token: 0x06000058 RID: 88 RVA: 0x00002FF4 File Offset: 0x000011F4
		public static List<string> GetList(string s)
		{
			List<string> list = new List<string>();
			string[] array = s.Split(new char[]
			{
				'\n'
			});
			new Behavior();
			foreach (string text in array)
			{
				if (!(text == string.Empty) && !(text == "\n"))
				{
					list.Add(text);
				}
			}
			return list;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003054 File Offset: 0x00001254
		public static BulletBehavior GetBehavior(string s)
		{
			if (BehaviorParser.KnownBehaviors == null)
			{
				BehaviorParser.BuildList();
			}
			BulletBehavior result;
			try
			{
				s = s.Trim();
				BulletBehavior bulletBehavior;
				BehaviorParser.KnownBehaviors.TryGetValue(s.ToUpper(), out bulletBehavior);
				result = bulletBehavior;
			}
			catch
			{
				Console.WriteLine("ERROR: Behavior | " + s + " | not found!");
				Terminal.WriteMessage("ERROR: Behavior | " + s + " | not found!", MessageType.ERROR);
				BulletBehavior bulletBehavior2;
				BehaviorParser.KnownBehaviors.TryGetValue("Straight", out bulletBehavior2);
				result = bulletBehavior2;
			}
			return result;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000030E0 File Offset: 0x000012E0
		private static void BuildList()
		{
			BehaviorParser.KnownBehaviors = new Dictionary<string, BulletBehavior>();
			BehaviorParser.KnownBehaviors.Add("STRAIGHT", BulletBehavior.Straight);
			BehaviorParser.KnownBehaviors.Add("WIGGLE", BulletBehavior.Wiggle);
			BehaviorParser.KnownBehaviors.Add("LAUNCH", BulletBehavior.Launch);
			BehaviorParser.KnownBehaviors.Add("ANGLESIDE", BulletBehavior.AngleSide);
			BehaviorParser.KnownBehaviors.Add("SUCK", BulletBehavior.Suck);
			BehaviorParser.KnownBehaviors.Add("WAVE", BulletBehavior.Wave);
			BehaviorParser.KnownBehaviors.Add("DROPMISSLE", BulletBehavior.DropMissle);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003168 File Offset: 0x00001368
		public static Behavior LoadBehaviorFromStringList(string[] list)
		{
			Behavior behavior = new Behavior();
			foreach (string text in list)
			{
				if (!string.IsNullOrEmpty(text) && !(text == "\n") && !(text == Environment.NewLine))
				{
					text.Replace('\n', ' ');
					string[] array = text.Split(new char[]
					{
						','
					});
					BulletBehavior behavior2 = BehaviorParser.GetBehavior(array[0]);
					behavior.AddBehavior(behavior2);
					behavior.ChangePercent(behavior2, float.Parse(array[1], CultureInfo.InvariantCulture));
					behavior.ChangeOverTimePercent(behavior2, float.Parse(array[2], CultureInfo.InvariantCulture));
					behavior.SetMinMaxPercent(behavior2, float.Parse(array[3], CultureInfo.InvariantCulture), float.Parse(array[4], CultureInfo.InvariantCulture));
				}
			}
			return behavior;
		}

		// Token: 0x0400002E RID: 46
		private static Dictionary<string, BulletBehavior> KnownBehaviors;
	}
}
