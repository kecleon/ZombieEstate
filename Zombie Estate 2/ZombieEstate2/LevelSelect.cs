using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000E1 RID: 225
	internal class LevelSelect : MenuWithDescription
	{
		// Token: 0x060005E8 RID: 1512 RVA: 0x0002C680 File Offset: 0x0002A880
		public LevelSelect() : base(true, true)
		{
			this.WaveCount = new UIArrowControl("Wave Count", new List<string>
			{
				"15",
				"25",
				"50",
				"10"
			}, new Vector2(Global.GetScreenCenter().X - 210f, 520f), false);
			this.LevelSel = new UIArrowControl("Level", new List<string>
			{
				"The Estate",
				"Zombie High School",
				"Mall",
				"Skyscraper",
				"Desert Town"
			}, new Vector2(Global.GetScreenCenter().X - 210f, 320f), true);
			this.Go = new MenuItem(new Vector2(Global.GetScreenCenter().X, (float)(Global.ScreenRect.Height - 320)), "Start", 0, false);
			this.Go.SelectedFunction = new MenuItem.SelectedDelegate(this.GoPressed);
			base.AddToMenu(this.Go);
			this.Back = new MenuItem(new Vector2(Global.GetScreenCenter().X, (float)(Global.ScreenRect.Height - 256)), "Back", 1, false);
			this.Back.SelectedFunction = new MenuItem.SelectedDelegate(this.BackPressed);
			base.AddToMenu(this.Back);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0002C812 File Offset: 0x0002AA12
		public override void Setup()
		{
			this.title = "Level Select";
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0002C81F File Offset: 0x0002AA1F
		public override void UpdateMenu()
		{
			this.WaveCount.Update();
			this.LevelSel.Update();
			base.UpdateMenu();
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0002C83D File Offset: 0x0002AA3D
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			this.Back.Draw(spriteBatch);
			this.Go.Draw(spriteBatch);
			this.WaveCount.Draw(spriteBatch);
			this.LevelSel.Draw(spriteBatch);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0002C86F File Offset: 0x0002AA6F
		public override void BackPressed()
		{
			ScreenFader.Fade(delegate()
			{
				Global.GameState = GameState.CharacterSelect;
			});
			base.BackPressed();
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0002C89C File Offset: 0x0002AA9C
		private LevelDetails SelectedDetails
		{
			get
			{
				foreach (LevelDetails levelDetails in this.Levels)
				{
					if (base.SelectedItem != null && levelDetails.levelDesc == base.SelectedItem.Description)
					{
						return levelDetails;
					}
				}
				return default(LevelDetails);
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void SetupLevelDetails()
		{
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0002C918 File Offset: 0x0002AB18
		private void ItemSelected(string name)
		{
			ScreenFader.Fade(delegate()
			{
				MenuManager.MenuClosed();
				Global.GameManager.StartGame(name);
			});
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0002C938 File Offset: 0x0002AB38
		private void GoPressed()
		{
			Global.WaveCount = Convert.ToInt32(this.WaveCount.Value);
			if (this.LevelSel.Value == "The Estate")
			{
				this.ItemSelected("Estate");
			}
			if (this.LevelSel.Value == "Zombie High School")
			{
				this.ItemSelected("School");
			}
			if (this.LevelSel.Value == "Mall")
			{
				this.ItemSelected("Mall");
			}
			if (this.LevelSel.Value == "Skyscraper")
			{
				this.ItemSelected("Skyscraper");
			}
			if (this.LevelSel.Value == "Desert Town")
			{
				this.ItemSelected("DesertTown");
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0002CA04 File Offset: 0x0002AC04
		public static string MapToLevel(string s)
		{
			if (s == "The Estate")
			{
				return "Estate";
			}
			if (s == "Zombie High School")
			{
				return "School";
			}
			if (s == "Mall")
			{
				return "Mall";
			}
			if (s == "Skyscraper")
			{
				return "Skyscraper";
			}
			if (s == "Desert Town")
			{
				return "DesertTown";
			}
			return "NONE";
		}

		// Token: 0x040005CA RID: 1482
		private List<LevelDetails> Levels = new List<LevelDetails>();

		// Token: 0x040005CB RID: 1483
		private Rectangle iconRect;

		// Token: 0x040005CC RID: 1484
		private UIArrowControl WaveCount;

		// Token: 0x040005CD RID: 1485
		private UIArrowControl LevelSel;

		// Token: 0x040005CE RID: 1486
		private MenuItem Back;

		// Token: 0x040005CF RID: 1487
		private MenuItem Go;
	}
}
