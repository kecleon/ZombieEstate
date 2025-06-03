using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000E5 RID: 229
	public class Menu
	{
		// Token: 0x06000604 RID: 1540 RVA: 0x0002CF04 File Offset: 0x0002B104
		public Menu(bool backEnabled, Vector2 center)
		{
			this.Items = new List<MenuItem>();
			this.Center = center;
			this.cooldown.IndependentOfTime = true;
			this.Setup();
			if (backEnabled)
			{
				MenuItem menuItem = new MenuItem(new Vector2(center.X, center.Y + 48f), "Back", this.Items.Count, false);
				menuItem.SelectedFunction = new MenuItem.SelectedDelegate(this.BackPressed);
				this.Items.Add(menuItem);
			}
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0002CFB0 File Offset: 0x0002B1B0
		public Menu() : this(true, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2)))
		{
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0002CFD8 File Offset: 0x0002B1D8
		public Menu(bool backEnabled) : this(backEnabled, new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2)))
		{
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0002D000 File Offset: 0x0002B200
		public void RESET()
		{
			this.Items.Clear();
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0002D00D File Offset: 0x0002B20D
		public virtual void Setup()
		{
			this.title = "NONE";
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0002D01C File Offset: 0x0002B21C
		public virtual void UpdateMenu()
		{
			int num = this.mSelectedIndex;
			int num2 = 4;
			if (this.mFirstPass)
			{
				if (this.Items.Count != 0)
				{
					this.Items[this.mSelectedIndex].Highlight();
				}
				this.mFirstPass = false;
			}
			for (int i = 0; i < num2; i++)
			{
				if (this.Items.Count != 0)
				{
					if (this.mSelectedIndex != -1 && this.Items[this.mSelectedIndex].Picker && !ScreenFader.Active)
					{
						if (InputManager.ButtonPressed(ButtonPress.MoveWest, i, true))
						{
							SoundEngine.PlaySound("ze2_navup", 0.2f);
							this.Items[this.mSelectedIndex].Selected_Pick(false);
						}
						else if (InputManager.ButtonPressed(ButtonPress.MoveEast, i, true))
						{
							SoundEngine.PlaySound("ze2_navup", 0.2f);
							this.Items[this.mSelectedIndex].Selected_Pick(true);
						}
					}
					else if (InputManager.ButtonPressed(ButtonPress.Affirmative, i, true) && !ScreenFader.Active)
					{
						this.LastControllerUsedToPressMenuItem = i;
						if (this.mSelectedIndex == -1)
						{
							this.mSelectedIndex = 0;
						}
						this.Items[this.mSelectedIndex].Selected();
						SoundEngine.PlaySound("ze2_navup", 0.3f);
					}
					if (InputManager.ButtonPressed(ButtonPress.MoveSouth, i, true))
					{
						SoundEngine.PlaySound("ze2_menunav", 0.3f);
						this.mSelectedIndex++;
						if (this.mSelectedIndex < 0)
						{
							this.mSelectedIndex = this.Items.Count - 1;
						}
						if (this.mSelectedIndex >= this.Items.Count)
						{
							this.mSelectedIndex = 0;
						}
					}
					if (InputManager.ButtonPressed(ButtonPress.MoveNorth, i, true))
					{
						SoundEngine.PlaySound("ze2_menunav", 0.3f);
						this.mSelectedIndex--;
						if (this.mSelectedIndex < 0)
						{
							this.mSelectedIndex = this.Items.Count - 1;
						}
						if (this.mSelectedIndex >= this.Items.Count)
						{
							this.mSelectedIndex = 0;
						}
					}
				}
			}
			if (!ScreenFader.Active && !Global.ROBMODE)
			{
				this.MouseSelection();
			}
			if (num != this.mSelectedIndex)
			{
				foreach (MenuItem menuItem in this.Items)
				{
					menuItem.Unhighlight();
				}
				if (this.mSelectedIndex != -1)
				{
					this.Items[this.mSelectedIndex].Highlight();
				}
			}
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0002D2A4 File Offset: 0x0002B4A4
		private void MouseSelection()
		{
			Point mousePosition = InputManager.GetMousePosition();
			if (InputManager.HasMouseMoved())
			{
				this.mSelectedIndex = -1;
				foreach (MenuItem menuItem in this.Items)
				{
					if (menuItem.Collision.Contains(mousePosition))
					{
						this.mSelectedIndex = this.Items.IndexOf(menuItem);
					}
				}
			}
			if (InputManager.LeftMouseClicked() && this.mSelectedIndex != -1 && this.Items.Count > 0)
			{
				if (this.Items[this.mSelectedIndex].Picker)
				{
					Rectangle collision = this.Items[this.mSelectedIndex].Collision;
					if (collision.X + (int)((float)collision.Width / 2f) > InputManager.GetMousePosition().X)
					{
						this.Items[this.mSelectedIndex].SelectedFunction_Picker(false);
					}
					else
					{
						this.Items[this.mSelectedIndex].SelectedFunction_Picker(true);
					}
					SoundEngine.PlaySound("ze2_navup", 0.2f);
				}
				else
				{
					this.Items[this.mSelectedIndex].Selected();
					SoundEngine.PlaySound("ze2_navup", 0.3f);
				}
			}
			this.prevMouse = mousePosition;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0002D418 File Offset: 0x0002B618
		public virtual void DrawMenu(SpriteBatch spriteBatch)
		{
			if (this.DrawBGPixel)
			{
				spriteBatch.Draw(Global.Pixel, Global.ScreenRect, Color.Black);
			}
			if (this.MenuBG != null)
			{
				if (this.ScaleBG)
				{
					this.MenuBGRect = Global.GetAspectRatioRect(Global.ScreenRect, this.MenuBG);
				}
				else
				{
					this.MenuBGRect = Global.ScreenRect;
				}
				spriteBatch.Draw(this.MenuBG, this.MenuBGRect, Color.White);
			}
			this.DrawTitle(spriteBatch);
			foreach (MenuItem menuItem in this.Items)
			{
				menuItem.Draw(spriteBatch);
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Draw3DMenu()
		{
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0002D4D8 File Offset: 0x0002B6D8
		public void DrawTitle(SpriteBatch spriteBatch)
		{
			if (this.title == null)
			{
				this.title = "NULL";
			}
			int num = (int)((float)Global.ScreenRect.Height * 0.1f);
			Vector2 pos = VerchickMath.CenterText(Global.BloodGutterXtraLarge, new Vector2((float)(Global.ScreenRect.Width / 2), (float)num), this.title);
			Shadow.DrawString(this.title, Global.BloodGutterXtraLarge, pos, MenuManager.ShadowDistance, Color.Red, spriteBatch);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0002D54C File Offset: 0x0002B74C
		public MenuItem AddToMenu(string text, MenuItem.SelectedDelegate del, bool picker = false)
		{
			MenuItem menuItem = new MenuItem(this.Center, text, this.Items.Count, picker);
			menuItem.SelectedFunction = del;
			this.Items.Add(menuItem);
			return menuItem;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0002D588 File Offset: 0x0002B788
		public void AddToMenu(string text, MenuItem.SelectedDelegate del, string description)
		{
			MenuItem menuItem = new MenuItem(this.Center, text, this.Items.Count, false);
			menuItem.SelectedFunction = del;
			menuItem.Description = description;
			this.Items.Add(menuItem);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0002D5C8 File Offset: 0x0002B7C8
		public void AddToMenu(MenuItem item)
		{
			this.Items.Add(item);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0001D9C2 File Offset: 0x0001BBC2
		public virtual void BackPressed()
		{
			MenuManager.MenuClosed();
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0002D5D6 File Offset: 0x0002B7D6
		public MenuItem SelectedItem
		{
			get
			{
				if (this.mSelectedIndex == -1)
				{
					return null;
				}
				return this.Items[this.mSelectedIndex];
			}
		}

		// Token: 0x040005D9 RID: 1497
		private List<MenuItem> Items;

		// Token: 0x040005DA RID: 1498
		private int mSelectedIndex;

		// Token: 0x040005DB RID: 1499
		private Timer cooldown = new Timer(0.25f);

		// Token: 0x040005DC RID: 1500
		public string title;

		// Token: 0x040005DD RID: 1501
		public Vector2 Center;

		// Token: 0x040005DE RID: 1502
		public Texture2D MenuBG;

		// Token: 0x040005DF RID: 1503
		public Rectangle MenuBGRect;

		// Token: 0x040005E0 RID: 1504
		public bool ScaleBG;

		// Token: 0x040005E1 RID: 1505
		public bool DrawBGPixel;

		// Token: 0x040005E2 RID: 1506
		public Timer Delay;

		// Token: 0x040005E3 RID: 1507
		private bool mFirstPass = true;

		// Token: 0x040005E4 RID: 1508
		public int BGScale = 1;

		// Token: 0x040005E5 RID: 1509
		public int LastControllerUsedToPressMenuItem = -1;

		// Token: 0x040005E6 RID: 1510
		private Point prevMouse;
	}
}
