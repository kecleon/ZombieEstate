using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000A1 RID: 161
	internal class XboxLevelSelect : MenuWithDescription
	{
		// Token: 0x06000422 RID: 1058 RVA: 0x0001ED9C File Offset: 0x0001CF9C
		public static void LoadIcons()
		{
			if (XboxLevelSelect.ICON_Estate == null)
			{
				XboxLevelSelect.ICON_Estate = Global.Content.Load<Texture2D>("LevelIcons//Icon_Estate");
				XboxLevelSelect.ICON_School = Global.Content.Load<Texture2D>("LevelIcons//Icon_ZHS");
				XboxLevelSelect.ICON_Mall = Global.Content.Load<Texture2D>("LevelIcons//Icon_Mall");
				XboxLevelSelect.ICON_Skyscraper = Global.Content.Load<Texture2D>("LevelIcons//Icon_Skyscraper");
				XboxLevelSelect.ICON_DesertTown = Global.Content.Load<Texture2D>("LevelIcons//Icon_DesertTown");
				XboxLevelSelect.ICON_Office = Global.Content.Load<Texture2D>("LevelIcons//Icon_Office");
				XboxLevelSelect.ICON_Farm = Global.Content.Load<Texture2D>("LevelIcons//Icon_Farm");
				XboxLevelSelect.ICON_Estate.Name = "estate";
				XboxLevelSelect.ICON_School.Name = "school";
				XboxLevelSelect.ICON_Mall.Name = "mall";
				XboxLevelSelect.ICON_Skyscraper.Name = "sky";
				XboxLevelSelect.ICON_DesertTown.Name = "desert";
				XboxLevelSelect.ICON_Office.Name = "office";
				XboxLevelSelect.ICON_Farm.Name = "farm";
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001EEA8 File Offset: 0x0001D0A8
		public XboxLevelSelect() : base(true, -100, true)
		{
			this.MenuBG = Global.MenuBG;
			XboxLevelSelect.LoadIcons();
			this.mIconRect = new Rectangle((int)Global.GetScreenCenter().X + 212, (int)Global.GetScreenCenter().Y - 170, 256, 256);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001EF14 File Offset: 0x0001D114
		public override void Setup()
		{
			this.DrawBGPixel = false;
			base.AddToMenu("Estate", new MenuItem.SelectedDelegate(this.Estate), "The abandoned estate has been a well known fortress in the apocalypse.");
			base.AddToMenu("Zombie High", new MenuItem.SelectedDelegate(this.School), "ZHS was the best school in the district. Their band was good too.");
			base.AddToMenu("The Mall", new MenuItem.SelectedDelegate(this.Mall), "This mall was one of the most profitable in the country.");
			base.AddToMenu("Skyscraper", new MenuItem.SelectedDelegate(this.Skyscraper), "People said the rooftops were safe. They were wrong.");
			base.AddToMenu("Desert Town", new MenuItem.SelectedDelegate(this.DesertTown), "An abandoned town at the edge of the desert.");
			base.AddToMenu("Office", new MenuItem.SelectedDelegate(this.Office), "An office located on the 40th floor of a skyscraper.");
			base.AddToMenu("The Farm", new MenuItem.SelectedDelegate(this.Farm), "Despite being abandoned, this farm provides a safe refuge for hungry survivors.");
			this.title = "Level Select";
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001EFF8 File Offset: 0x0001D1F8
		public override void UpdateMenu()
		{
			base.UpdateMenu();
			if (this.mInflateTime > 0f)
			{
				this.mInflateTime -= Global.REAL_GAME_TIME;
			}
			else
			{
				this.mInflateTime = 0f;
			}
			if (this.mCurrIcon != null && this.mCurrIcon.Name != this.mPrevIcon)
			{
				this.mInflateTime = 0.15f;
				this.mPrevIcon = this.mCurrIcon.Name;
			}
			if (base.SelectedItem == null)
			{
				return;
			}
			if (base.SelectedItem.Text == "Estate")
			{
				this.mCurrIcon = XboxLevelSelect.ICON_Estate;
				return;
			}
			if (base.SelectedItem.Text == "Zombie High")
			{
				this.mCurrIcon = XboxLevelSelect.ICON_School;
				return;
			}
			if (base.SelectedItem.Text == "The Mall")
			{
				this.mCurrIcon = XboxLevelSelect.ICON_Mall;
				return;
			}
			if (base.SelectedItem.Text == "Skyscraper")
			{
				this.mCurrIcon = XboxLevelSelect.ICON_Skyscraper;
				return;
			}
			if (base.SelectedItem.Text == "Desert Town")
			{
				this.mCurrIcon = XboxLevelSelect.ICON_DesertTown;
				return;
			}
			if (base.SelectedItem.Text == "Office")
			{
				this.mCurrIcon = XboxLevelSelect.ICON_Office;
				return;
			}
			if (base.SelectedItem.Text == "The Farm")
			{
				this.mCurrIcon = XboxLevelSelect.ICON_Farm;
				return;
			}
			this.mCurrIcon = null;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001F178 File Offset: 0x0001D378
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
			if (this.mCurrIcon != null)
			{
				int num = (int)(30f * this.mInflateTime);
				Rectangle destinationRectangle = new Rectangle(this.mIconRect.X, this.mIconRect.Y, this.mIconRect.Width, this.mIconRect.Height);
				destinationRectangle.Inflate(num, num);
				spriteBatch.Draw(this.mCurrIcon, destinationRectangle, Color.White);
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001F1F0 File Offset: 0x0001D3F0
		private void Start()
		{
			ScreenFader.Fade(delegate()
			{
				GameManager.LevelName = this.name;
				MenuManager.PushMenu(new XboxWaveSelect());
			});
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001F203 File Offset: 0x0001D403
		private void Any()
		{
			this.name = "Any";
			this.Start();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001F218 File Offset: 0x0001D418
		private void Random()
		{
			int index = Global.rand.Next(0, XboxLevelSelect.LevelNames.Count);
			this.name = XboxLevelSelect.LevelNames[index];
			this.Start();
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001F252 File Offset: 0x0001D452
		private void Estate()
		{
			this.name = "Estate";
			this.Start();
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001F265 File Offset: 0x0001D465
		private void School()
		{
			this.name = "School";
			this.Start();
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001F278 File Offset: 0x0001D478
		private void Mall()
		{
			this.name = "Mall";
			this.Start();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001F28B File Offset: 0x0001D48B
		private void Skyscraper()
		{
			this.name = "Skyscraper";
			this.Start();
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001F29E File Offset: 0x0001D49E
		private void DesertTown()
		{
			this.name = "DesertTown";
			this.Start();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001F2B1 File Offset: 0x0001D4B1
		private void Office()
		{
			this.name = "Office";
			this.Start();
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001F2C4 File Offset: 0x0001D4C4
		private void Farm()
		{
			this.name = "Farm";
			this.Start();
		}

		// Token: 0x04000407 RID: 1031
		public static Texture2D ICON_Estate;

		// Token: 0x04000408 RID: 1032
		public static Texture2D ICON_School;

		// Token: 0x04000409 RID: 1033
		public static Texture2D ICON_Mall;

		// Token: 0x0400040A RID: 1034
		public static Texture2D ICON_Skyscraper;

		// Token: 0x0400040B RID: 1035
		public static Texture2D ICON_DesertTown;

		// Token: 0x0400040C RID: 1036
		public static Texture2D ICON_Office;

		// Token: 0x0400040D RID: 1037
		public static Texture2D ICON_Farm;

		// Token: 0x0400040E RID: 1038
		private Rectangle mIconRect;

		// Token: 0x0400040F RID: 1039
		private string name;

		// Token: 0x04000410 RID: 1040
		private Texture2D mCurrIcon;

		// Token: 0x04000411 RID: 1041
		private float mInflateTime;

		// Token: 0x04000412 RID: 1042
		public static List<string> LevelNames = new List<string>
		{
			"Estate",
			"School",
			"Mall",
			"Skyscraper",
			"DesertTown",
			"Office",
			"Farm"
		};

		// Token: 0x04000413 RID: 1043
		private string mPrevIcon = "";
	}
}
