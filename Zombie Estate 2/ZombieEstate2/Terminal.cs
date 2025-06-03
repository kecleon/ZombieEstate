using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200012D RID: 301
	public static class Terminal
	{
		// Token: 0x0600086D RID: 2157 RVA: 0x000462A4 File Offset: 0x000444A4
		public static void Initialize(string projName)
		{
			Terminal.ACTIVE = Config.Instance.Log;
			if (!Terminal.WRITETOFILE)
			{
				return;
			}
			DateTime now = DateTime.Now;
			Terminal.Messages = new List<Message>();
			Terminal.ProjectName = projName;
			Terminal.StartDate = now.ToLongDateString();
			Terminal.StartTime = now.ToLongTimeString();
			Terminal.FileName = "Log_" + projName;
			Terminal.Dir = string.Concat(new object[]
			{
				"Logs/",
				now.Date.Month,
				"_",
				now.Date.Day,
				"_LogFiles/"
			});
			Terminal.SetUpFile();
			Terminal.WriteHeader();
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00046368 File Offset: 0x00044568
		private static void SetUpFile()
		{
			try
			{
				Directory.CreateDirectory(Terminal.Dir);
			}
			catch (Exception)
			{
			}
			int num = 0;
			while (File.Exists(string.Concat(new object[]
			{
				Terminal.Dir,
				Terminal.FileName,
				"_",
				num,
				".html"
			})))
			{
				num++;
			}
			Terminal.writer = new StreamWriter(string.Concat(new object[]
			{
				Terminal.Dir,
				Terminal.FileName,
				"_",
				num,
				".html"
			}));
			Terminal.writer.AutoFlush = true;
			Terminal.WriterOpened = true;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00046428 File Offset: 0x00044628
		private static void WriteHeader()
		{
			Terminal.writer.WriteLine("<html> <head> <style type=\"text/css\">");
			Terminal.writer.WriteLine("body {background-color: black} body {color: white} body {font-family:\"Lucida Console\", Times, serif;} ");
			Terminal.writer.WriteLine("p {line-height: 0.25}");
			Terminal.writer.WriteLine("</style></head><body>");
			Terminal.WriteMessage("------------------------Log File------------------------", MessageType.TERMINAL);
			Terminal.WriteMessage("Project Name: " + Terminal.ProjectName, MessageType.TERMINAL);
			Terminal.WriteMessage("Author: " + Terminal.Author, MessageType.TERMINAL);
			Terminal.WriteMessage("--------------------------------------------------------", MessageType.TERMINAL);
			Terminal.WriteMessage("Log Date: " + Terminal.StartDate, MessageType.TERMINAL);
			Terminal.WriteMessage("Log Time: " + Terminal.StartTime, MessageType.TERMINAL);
			Terminal.WriteMessage("--------------------------------------------------------", MessageType.TERMINAL);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x000464E6 File Offset: 0x000446E6
		public static void TearDown()
		{
			if (!Terminal.WRITETOFILE)
			{
				return;
			}
			Terminal.WriteMessage("Game Exited Normally", MessageType.TERMINAL);
			Terminal.WriteMessage("----------------------Log Finished----------------------", MessageType.TERMINAL);
			Terminal.writer.WriteLine("</body></html>");
			Terminal.writer.Close();
			Terminal.WriterOpened = false;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00046528 File Offset: 0x00044728
		private static string getPrefix(MessageType type)
		{
			if (type == MessageType.TERMINAL)
			{
				return "<p style=\"color: #aaccff; \">";
			}
			if (type == MessageType.ERROR)
			{
				return "<p style=\"color: red; \">";
			}
			if (type == MessageType.DEBUG)
			{
				return "<p style=\"color: yellow; \">";
			}
			if (type == MessageType.SAVELOAD)
			{
				return "<p style=\"color: gray; \">";
			}
			if (type == MessageType.IMPORTANTEVENT)
			{
				return "<p style=\"color: green; \">";
			}
			if (type == MessageType.COMMAND)
			{
				return "<p style=\"color: white; \">";
			}
			if (type == MessageType.AI)
			{
				return "<p style=\"color: cyan; \">";
			}
			return "<p style=\"color: #444444; \">";
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0004657F File Offset: 0x0004477F
		private static string getSuffix(MessageType type)
		{
			return "</p>";
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00046588 File Offset: 0x00044788
		public static void WriteMessage(string txt, MessageType type)
		{
			if (!Terminal.ACTIVE)
			{
				return;
			}
			Console.WriteLine(txt);
			if (!Terminal.WRITETOFILE)
			{
				return;
			}
			if (Terminal.writer == null || !Terminal.WriterOpened)
			{
				return;
			}
			string text = "[" + DateTime.Now.ToLongTimeString() + "]: ";
			Terminal.writer.WriteLine(Terminal.getPrefix(type));
			Terminal.writer.WriteLine(text);
			Terminal.writer.WriteLine(txt);
			Terminal.writer.WriteLine(Terminal.getSuffix(type));
			Terminal.AddLine(text + txt, type);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0004661A File Offset: 0x0004481A
		private static void AddLine(string txt, MessageType type)
		{
			Terminal.Messages.Add(Terminal.getMessage(txt, type));
			if (Terminal.Messages.Count > Terminal.NumMessages)
			{
				Terminal.Messages.RemoveAt(0);
			}
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0004664C File Offset: 0x0004484C
		private static Message getMessage(string txt, MessageType type)
		{
			Message result = default(Message);
			if (type == MessageType.TERMINAL)
			{
				result.text = txt;
				result.type = type;
				result.color = new Color(0.4f, 0.5f, 1f);
			}
			if (type == MessageType.ERROR)
			{
				result.text = txt;
				result.type = type;
				result.color = Color.Red;
			}
			if (type == MessageType.DEBUG)
			{
				result.text = txt;
				result.type = type;
				result.color = Color.Yellow;
			}
			if (type == MessageType.SAVELOAD)
			{
				result.text = txt;
				result.type = type;
				result.color = Color.Gray;
			}
			if (type == MessageType.IMPORTANTEVENT)
			{
				result.text = txt;
				result.type = type;
				result.color = Color.LightGreen;
			}
			if (type == MessageType.COMMAND)
			{
				result.text = txt;
				result.type = type;
				result.color = Color.White;
			}
			if (type == MessageType.AI)
			{
				result.text = txt;
				result.type = type;
				result.color = Color.Cyan;
			}
			return result;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00046750 File Offset: 0x00044950
		public static List<Message> RecentMessages()
		{
			return Terminal.Messages;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00046757 File Offset: 0x00044957
		public static void WriteMessage(string p)
		{
			Terminal.WriteMessage(p, MessageType.DEBUG);
		}

		// Token: 0x04000916 RID: 2326
		public static bool ACTIVE = true;

		// Token: 0x04000917 RID: 2327
		private static string FileName;

		// Token: 0x04000918 RID: 2328
		private static string StartDate;

		// Token: 0x04000919 RID: 2329
		private static string StartTime;

		// Token: 0x0400091A RID: 2330
		private static string Author = "Jeremy Verchick";

		// Token: 0x0400091B RID: 2331
		private static StreamWriter writer;

		// Token: 0x0400091C RID: 2332
		private static string ProjectName;

		// Token: 0x0400091D RID: 2333
		private static string Dir;

		// Token: 0x0400091E RID: 2334
		private static bool WriterOpened = false;

		// Token: 0x0400091F RID: 2335
		public static bool WRITETOFILE = true;

		// Token: 0x04000920 RID: 2336
		private static int NumMessages = 200;

		// Token: 0x04000921 RID: 2337
		private static List<Message> Messages;
	}
}
