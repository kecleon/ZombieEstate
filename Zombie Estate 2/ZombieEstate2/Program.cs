using System;
using System.IO;
using System.Text;
using Steamworks;

namespace ZombieEstate2
{
	// Token: 0x02000121 RID: 289
	public static class Program
	{
		// Token: 0x0600081C RID: 2076 RVA: 0x000437DC File Offset: 0x000419DC
		private static void Main(string[] args)
		{
			if (args.Length >= 2)
			{
				try
				{
					string a = args[0];
					string s = args[1];
					if (a == "+connect_lobby")
					{
						Program.JOIN_LOBBY = true;
						Program.LOBBY_ID = new CSteamID(ulong.Parse(s));
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error trying to set lobby data! " + ex.ToString());
					Program.JOIN_LOBBY = false;
				}
			}
			if (!Global.CHEAT)
			{
				try
				{
					using (Game1 game = new Game1())
					{
						game.Run();
					}
					return;
				}
				catch (Exception ex2)
				{
					StringBuilder stringBuilder = new StringBuilder();
					StreamWriter streamWriter = new StreamWriter("ERROR.txt", true);
					stringBuilder.AppendLine("Zombie Estate 2");
					stringBuilder.AppendLine(DateTime.Now.ToShortDateString() + " | " + DateTime.Now.ToShortTimeString());
					stringBuilder.AppendLine("Exception:");
					stringBuilder.AppendLine(ex2.ToString());
					stringBuilder.AppendLine("Message:");
					stringBuilder.AppendLine(ex2.Message.ToString());
					stringBuilder.AppendLine("-------------------------------------------------------------------");
					streamWriter.WriteLine(stringBuilder.ToString());
					streamWriter.Close();
					ErrorForm errorForm = new ErrorForm();
					string path = "";
					try
					{
						path = Path.GetFullPath("ERROR.txt");
					}
					catch
					{
						path = "?";
					}
					errorForm.Init(stringBuilder.ToString(), path);
					errorForm.ShowDialog();
					return;
				}
			}
			using (Game1 game2 = new Game1())
			{
				game2.Run();
			}
		}

		// Token: 0x040008D7 RID: 2263
		public static bool JOIN_LOBBY;

		// Token: 0x040008D8 RID: 2264
		public static CSteamID LOBBY_ID;
	}
}
