using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.XboxSaving;

namespace ZombieEstate2.UI.Menus
{
	// Token: 0x02000168 RID: 360
	internal class DeleteDataMenu : Menu
	{
		// Token: 0x06000AF7 RID: 2807 RVA: 0x0005ACAC File Offset: 0x00058EAC
		public DeleteDataMenu() : base(false, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2)))
		{
			this.sb = new ScrollBox("Are you absolutely sure you want to clear all data? This will delete all characters and hats unlocked!", new Rectangle((int)Global.GetScreenCenter().X - 400, 200, 800, 400), Global.StoreFontBig, null, Color.Red);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0005AD20 File Offset: 0x00058F20
		public override void Setup()
		{
			base.AddToMenu("Cancel", new MenuItem.SelectedDelegate(this.Cancel), false);
			base.AddToMenu("Cancel", new MenuItem.SelectedDelegate(this.Cancel), false);
			base.AddToMenu("DELETE ALL DATA", new MenuItem.SelectedDelegate(this.Delete), false);
			base.AddToMenu("Cancel", new MenuItem.SelectedDelegate(this.Cancel), false);
			base.AddToMenu("Cancel", new MenuItem.SelectedDelegate(this.Cancel), false);
			this.title = "Delete Saved Data";
			this.MenuBG = Global.MenuBG;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0005ADC0 File Offset: 0x00058FC0
		public override void DrawMenu(SpriteBatch spriteBatch)
		{
			base.DrawMenu(spriteBatch);
			this.sb.Draw(spriteBatch);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00059D26 File Offset: 0x00057F26
		private void Cancel()
		{
			this.BackPressed();
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0005ADD8 File Offset: 0x00058FD8
		private void Delete()
		{
			try
			{
				XboxSaverLoader.ClearAllData();
				XboxSaverLoader.LoadData();
				SoundEngine.PlaySound("ze2_death");
			}
			catch
			{
			}
			this.BackPressed();
		}

		// Token: 0x04000BBA RID: 3002
		private ScrollBox sb;
	}
}
