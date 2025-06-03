using System;

namespace ZombieEstate2
{
	// Token: 0x020000A2 RID: 162
	internal class XboxWaveSelect : MenuWithDescription
	{
		// Token: 0x06000433 RID: 1075 RVA: 0x0001F353 File Offset: 0x0001D553
		public XboxWaveSelect() : base(true, -100, false)
		{
			this.MenuBG = Global.MenuBG;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001F36C File Offset: 0x0001D56C
		public override void Setup()
		{
			this.DrawBGPixel = false;
			base.AddToMenu("Casual", new MenuItem.SelectedDelegate(this.Casual), "Start a 15 wave length casual game. [3 Continues]");
			base.AddToMenu("Hard", new MenuItem.SelectedDelegate(this.Hard), "Start a 15 wave length hard game where zombies have more health. Having a balanced team is recommended. [3 Continues]");
			base.AddToMenu("Unlimited", new MenuItem.SelectedDelegate(this.Unlimited), "Zombies gain health as the game progresses. Survive as long as you can! Having a balanced team is required. [0 Continues]");
			this.title = "Mode Select";
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001F3DF File Offset: 0x0001D5DF
		public override void UpdateMenu()
		{
			base.UpdateMenu();
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0001F3E8 File Offset: 0x0001D5E8
		private void Start()
		{
			if (InputManager.LeftMouseClicked())
			{
				Terminal.WriteMessage("Player using keyboard.", MessageType.IMPORTANTEVENT);
				Global.GameManager.INITIATED_CHAR_SEL_WITH_MOUSE = true;
			}
			else
			{
				Terminal.WriteMessage("Player using Gamepad.", MessageType.IMPORTANTEVENT);
				Global.GameManager.INITIATED_CHAR_SEL_WITH_MOUSE = false;
				Global.GameManager.INITIATED_CONTROL_INDEX = this.LastControllerUsedToPressMenuItem;
			}
			ScreenFader.Fade(delegate()
			{
				Global.WaveCount = this.count;
				MenuManager.CLOSEALL();
				Global.GameManager.GotoCharSelect();
			});
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0001F44B File Offset: 0x0001D64B
		private void Casual()
		{
			this.count = 15;
			Global.UnlimitedMode = false;
			Global.ZombieHealthMod = 1f;
			Global.DifficultyModeMod = 1f;
			Global.DIFFICULTY_LEVEL = 1;
			this.Start();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001F47B File Offset: 0x0001D67B
		private void Hard()
		{
			this.count = 15;
			Global.UnlimitedMode = false;
			Global.ZombieHealthMod = 2f;
			Global.DifficultyModeMod = 1.5f;
			Global.DIFFICULTY_LEVEL = 2;
			this.Start();
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0001F4AB File Offset: 0x0001D6AB
		private void Thirty()
		{
			this.count = 15;
			Global.UnlimitedMode = false;
			Global.ZombieHealthMod = 2f;
			Global.DifficultyModeMod = 2f;
			this.Start();
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0001F4D5 File Offset: 0x0001D6D5
		private void Unlimited()
		{
			this.count = 50;
			Global.UnlimitedMode = true;
			Global.ZombieHealthMod = 1f;
			Global.DifficultyModeMod = 1f;
			Global.DIFFICULTY_LEVEL = 3;
			this.Start();
		}

		// Token: 0x04000414 RID: 1044
		private int count;
	}
}
