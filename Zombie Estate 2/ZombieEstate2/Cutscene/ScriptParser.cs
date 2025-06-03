using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Cutscene
{
	// Token: 0x020001DE RID: 478
	internal class ScriptParser
	{
		// Token: 0x06000CB6 RID: 3254 RVA: 0x000685B8 File Offset: 0x000667B8
		public List<CutSceneLine> ParseFile()
		{
			StreamReader streamReader = new StreamReader(ScriptParser.FOLDER + "TestScript" + ScriptParser.EXT);
			List<CutSceneLine> list = new List<CutSceneLine>();
			while (!streamReader.EndOfStream)
			{
				string text = streamReader.ReadLine();
				if (!text.StartsWith("//"))
				{
					list.Add(this.ParseLine(text));
				}
			}
			streamReader.Close();
			return list;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00068618 File Offset: 0x00066818
		private Vector2 ParseVector2(string args)
		{
			Vector2 result = new Vector2(0f, 0f);
			string[] array = args.Split(new char[]
			{
				','
			});
			result.X = float.Parse(array[0]);
			result.Y = float.Parse(array[1]);
			return result;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00068668 File Offset: 0x00066868
		private Vector3 ParseVector3(string args)
		{
			Vector3 result = new Vector3(0f, 0f, 0f);
			string[] array = args.Split(new char[]
			{
				','
			});
			result.X = float.Parse(array[0]);
			result.Y = float.Parse(array[1]);
			result.Z = float.Parse(array[2]);
			return result;
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x000686CC File Offset: 0x000668CC
		private Point ParsePoint(string args)
		{
			Point result = new Point(0, 0);
			string[] array = args.Split(new char[]
			{
				','
			});
			result.X = int.Parse(array[0]);
			result.Y = int.Parse(array[1]);
			return result;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00068714 File Offset: 0x00066914
		private Color ParseColor(string args)
		{
			Color black = Color.Black;
			if (args.ToUpper() == "BLUE")
			{
				black = new Color(76, 118, 169);
			}
			if (args.ToUpper() == "PURPLE")
			{
				black = new Color(120, 40, 100);
			}
			if (args.ToUpper() == "RED")
			{
				black = new Color(255, 140, 140);
			}
			return black;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00068794 File Offset: 0x00066994
		private CutSceneLine ParseLine(string line)
		{
			string[] split = line.Split(new char[]
			{
				'|'
			});
			return this.ParseCommand(split);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x000687BC File Offset: 0x000669BC
		private CutSceneLine ParseCommand(string[] split)
		{
			CutSceneLine cutSceneLine = new CutSceneLine();
			string text = split[0].ToUpper().Trim();
			for (int i = 0; i < split.Length; i++)
			{
				split[i] = split[i].Trim();
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1315103586U)
			{
				if (num <= 483875832U)
				{
					if (num != 13676631U)
					{
						if (num == 483875832U)
						{
							if (text == "SPAWN")
							{
								if (split.Length == 6)
								{
									cutSceneLine = new SpawnCineObject(split[1], split[2], this.ParseVector3(split[3]), this.ParsePoint(split[4]), this.ParseColor(split[5]));
								}
								else
								{
									cutSceneLine = new SpawnCineObject(split[1], split[2], this.ParseVector3(split[3]), this.ParsePoint(split[4]));
								}
								cutSceneLine.BlockingLine = true;
							}
						}
					}
					else if (text == "SPEECH")
					{
						cutSceneLine = new SpeechBubble(split[1], split[2]);
					}
				}
				else if (num != 657999708U)
				{
					if (num == 1315103586U)
					{
						if (text == "BLOCK_WALK")
						{
							cutSceneLine = new WalkLine(split[1], this.ParseVector2(split[2]), float.Parse(split[3]));
							cutSceneLine.BlockingLine = true;
						}
					}
				}
				else if (text == "BLOCK_SPAWNPARTICLE")
				{
					cutSceneLine = new ParticleLine(split[1], this.ParseVector3(split[2]), float.Parse(split[3]), int.Parse(split[4]), float.Parse(split[5]));
					cutSceneLine.BlockingLine = true;
				}
			}
			else if (num <= 2309786035U)
			{
				if (num != 2040339085U)
				{
					if (num == 2309786035U)
					{
						if (text == "MOVECAMERA")
						{
							cutSceneLine = new MoveCamera(this.ParseVector3(split[1]), split[2], float.Parse(split[3]));
						}
					}
				}
				else if (text == "BLOCK_MOVECAMERA")
				{
					cutSceneLine = new MoveCamera(this.ParseVector3(split[1]), split[2], float.Parse(split[3]));
					cutSceneLine.BlockingLine = true;
				}
			}
			else if (num != 2779206416U)
			{
				if (num != 2809800897U)
				{
					if (num == 3641250354U)
					{
						if (text == "SPAWNPARTICLE")
						{
							cutSceneLine = new ParticleLine(split[1], this.ParseVector3(split[2]), float.Parse(split[3]), int.Parse(split[4]), float.Parse(split[5]));
						}
					}
				}
				else if (text == "BLOCK_SPEECH")
				{
					cutSceneLine = new SpeechBubble(split[1], split[2]);
					cutSceneLine.BlockingLine = true;
				}
			}
			else if (text == "WALK")
			{
				cutSceneLine = new WalkLine(split[1], this.ParseVector2(split[2]), float.Parse(split[3]));
			}
			return cutSceneLine;
		}

		// Token: 0x04000D7B RID: 3451
		private static string EXT = ".zes";

		// Token: 0x04000D7C RID: 3452
		private static string FOLDER = "Scripts\\";
	}
}
