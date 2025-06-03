using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.XboxSaving;

namespace ZombieEstate2.StoreScreen.XboxStore
{
	// Token: 0x02000143 RID: 323
	internal class XboxItemScreen
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060009B4 RID: 2484 RVA: 0x0004DFB4 File Offset: 0x0004C1B4
		// (remove) Token: 0x060009B5 RID: 2485 RVA: 0x0004DFEC File Offset: 0x0004C1EC
		public event XboxItemScreen.ItemEvent ItemSelected;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060009B6 RID: 2486 RVA: 0x0004E024 File Offset: 0x0004C224
		// (remove) Token: 0x060009B7 RID: 2487 RVA: 0x0004E05C File Offset: 0x0004C25C
		public event XboxItemScreen.ItemEvent ItemHighlighted;

		// Token: 0x060009B8 RID: 2488 RVA: 0x0004E094 File Offset: 0x0004C294
		public XboxItemScreen(int w, int h, int shownW, int shownH, Vector2 topLeft, Player player)
		{
			this.mPlayerIndex = player.Index;
			this.mWidth = w;
			this.mHeight = h;
			this.mWindowWidth = shownW;
			this.mWindowHeight = shownH;
			this.mTopLeft = topLeft;
			this.mPlayer = player;
			this.mSelSrc = Global.GetTexRectange(2, 40);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0004E0F0 File Offset: 0x0004C2F0
		public XboxItemScreen(int w, int h, int shownW, int shownH, Vector2 topLeft, int index)
		{
			this.mPlayerIndex = index;
			this.mWidth = w;
			this.mHeight = h;
			this.mWindowWidth = shownW;
			this.mWindowHeight = shownH;
			this.mTopLeft = topLeft;
			this.mSelSrc = Global.GetTexRectange(2, 40);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0004E140 File Offset: 0x0004C340
		public void Update()
		{
			if (!this.mFirstLoop)
			{
				if (this.ItemHighlighted != null)
				{
					this.ItemHighlighted(this.mItems[this.mCurrentX, this.mCurrentY]);
				}
				this.mFirstLoop = true;
			}
			int num = this.mCurrentX;
			int num2 = this.mCurrentY;
			this.SelectionInputs();
			if ((num != this.mCurrentX || num2 != this.mCurrentY) && this.ItemHighlighted != null)
			{
				this.ItemHighlighted(this.mItems[this.mCurrentX, this.mCurrentY]);
			}
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0004E1D4 File Offset: 0x0004C3D4
		private void SelectionInputs()
		{
			int num = this.mCurrentX;
			int num2 = this.mCurrentY;
			if (VerchickMath.IsMouseInRect(new Rectangle((int)this.mTopLeft.X, (int)this.mTopLeft.Y, this.mWindowWidth * 32, this.mWindowHeight * 32)) && InputManager.LeftMouseClicked())
			{
				int num3 = InputManager.GetMousePosition().X - (int)this.mTopLeft.X;
				int num4 = InputManager.GetMousePosition().Y - (int)this.mTopLeft.Y;
				num3 /= 34;
				num4 /= 34;
				this.mCurrentX = num3 + this.mWindowX;
				this.mCurrentY = num4 + this.mWindowY;
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveNorth, this.mPlayerIndex, false))
			{
				this.mCurrentY--;
				this.UpdateWindowSel();
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveSouth, this.mPlayerIndex, false))
			{
				this.mCurrentY++;
				this.UpdateWindowSel();
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveWest, this.mPlayerIndex, false))
			{
				this.mCurrentX--;
				this.UpdateWindowSel();
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveEast, this.mPlayerIndex, false))
			{
				this.mCurrentX++;
				this.UpdateWindowSel();
			}
			if (this.mCurrentY < 0)
			{
				this.mCurrentY = 0;
			}
			if (this.mCurrentY >= this.mHeight)
			{
				this.mCurrentY = this.mHeight - 1;
			}
			if (this.mCurrentX < 0)
			{
				this.mCurrentX = 0;
			}
			if (this.mCurrentX >= this.mWidth)
			{
				this.mCurrentX = this.mWidth - 1;
			}
			if (this.INVENTORY && (this.mCurrentX == 1 || this.mCurrentX == 2) && this.mCurrentY == this.mHeight - 1)
			{
				this.mCurrentX = 0;
			}
			if (num != this.mCurrentX || num2 != this.mCurrentY)
			{
				SoundEngine.PlaySound("ze2_menunav", 0.25f);
			}
			this.UpdateScrollbar();
			this.UpdateWindow();
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0004E3C8 File Offset: 0x0004C5C8
		private void UpdateWindowSel()
		{
			if (this.mCurrentX + 1 > this.mWindowX + this.mWindowWidth)
			{
				this.mWindowX++;
			}
			if (this.mCurrentY + 1 > this.mWindowY + this.mWindowHeight)
			{
				this.mWindowY++;
			}
			if (this.mCurrentX < this.mWindowX)
			{
				this.mWindowX--;
			}
			if (this.mCurrentY < this.mWindowY)
			{
				this.mWindowY--;
			}
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0004E458 File Offset: 0x0004C658
		private void UpdateWindow()
		{
			Rectangle rectangle = new Rectangle((int)this.mTopLeft.X, (int)this.mTopLeft.Y, 34 * this.mWindowWidth, 34 * this.mWindowHeight);
			if (InputManager.MouseWheelUp() && rectangle.Contains(InputManager.GetMousePosition()))
			{
				this.mWindowY--;
			}
			if (InputManager.MouseWheelDown() && rectangle.Contains(InputManager.GetMousePosition()))
			{
				this.mWindowY++;
			}
			this.mWindowX = (int)MathHelper.Clamp((float)this.mWindowX, 0f, (float)(this.mWidth - this.mWindowWidth));
			this.mWindowY = (int)MathHelper.Clamp((float)this.mWindowY, 0f, (float)(this.mHeight - this.mWindowHeight));
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0004E528 File Offset: 0x0004C728
		public void Draw(SpriteBatch spriteBatch)
		{
			Rectangle pos = new Rectangle((int)this.mTopLeft.X, (int)this.mTopLeft.Y, 32, 32);
			for (int i = this.mWindowX; i < this.mWindowX + this.mWindowWidth; i++)
			{
				for (int j = this.mWindowY; j < this.mWindowY + this.mWindowHeight; j++)
				{
					pos.X = (i - this.mWindowX) * 34 + (int)this.mTopLeft.X;
					pos.Y = (j - this.mWindowY) * 34 + (int)this.mTopLeft.Y;
					if (this.mItems[i, j] != null)
					{
						this.mItems[i, j].Draw(spriteBatch, pos);
					}
				}
			}
			int num = (this.mCurrentX - this.mWindowX) * 34;
			int num2 = (this.mCurrentY - this.mWindowY) * 34;
			if (this.mCurrentY >= this.mWindowY && this.mCurrentY < this.mWindowY + this.mWindowHeight)
			{
				Rectangle destinationRectangle = new Rectangle(num + (int)this.mTopLeft.X, num2 + (int)this.mTopLeft.Y, 32, 32);
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(this.mSelSrc), Color.White);
			}
			this.DrawScrollbar(spriteBatch);
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0004E690 File Offset: 0x0004C890
		public void PopulateGuns(List<GunStats> guns, AmmoType type)
		{
			this.mItems = new XboxItem[this.mWidth, this.mHeight];
			int num = 0;
			int num2 = 0;
			foreach (GunStats gunStats in guns)
			{
				if (gunStats.Cost != -1 && gunStats.AmmoType == type)
				{
					this.mItems[num, num2] = new XboxItem(gunStats.GunXCoord, gunStats.GunYCoord + 1, gunStats, false, true);
					num++;
					if (num >= this.mWidth)
					{
						num = 0;
						num2++;
					}
				}
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0004E73C File Offset: 0x0004C93C
		public void PopulateGuns(List<GunStats> guns)
		{
			this.mItems = new XboxItem[this.mWidth, this.mHeight];
			int num = 0;
			int num2 = 0;
			XboxAmmoItem xboxAmmoItem = new XboxAmmoItem(AmmoType.ASSAULT, 40, 20, this.mPlayer);
			this.mItems[num, num2] = xboxAmmoItem;
			num++;
			xboxAmmoItem = new XboxAmmoItem(AmmoType.SHELLS, 12, 20, this.mPlayer);
			this.mItems[num, num2] = xboxAmmoItem;
			num++;
			xboxAmmoItem = new XboxAmmoItem(AmmoType.HEAVY, 100, 30, this.mPlayer);
			this.mItems[num, num2] = xboxAmmoItem;
			num++;
			xboxAmmoItem = new XboxAmmoItem(AmmoType.EXPLOSIVE, 4, 30, this.mPlayer);
			this.mItems[num, num2] = xboxAmmoItem;
			num++;
			foreach (GunStats gunStats in guns)
			{
				if (gunStats.Cost != -1 && gunStats.AmmoType != AmmoType.INFINITE && !(gunStats.GunName == "Cyber Baton"))
				{
					this.mItems[num, num2] = new XboxItem(gunStats.GunXCoord, gunStats.GunYCoord + 1, gunStats, false, true);
					num++;
					if (num >= this.mWidth)
					{
						num = 0;
						num2++;
					}
				}
			}
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0004E88C File Offset: 0x0004CA8C
		public void PopulateGuns(Gun[] guns)
		{
			this.mItems = new XboxItem[this.mWidth, this.mHeight];
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < Player.MAXGUNS; i++)
			{
				Gun gun = guns[i];
				if (gun != null)
				{
					GunStats stats = gun.stats;
					this.mItems[num, num2] = new XboxItem(stats.GunXCoord + 2 * gun.GetLevel(), stats.GunYCoord + 1, stats, false, true);
					num++;
					if (num >= this.mWidth)
					{
						num = 0;
						num2++;
					}
				}
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0004E918 File Offset: 0x0004CB18
		public void PopulateCharacters(List<CharacterSettings> chars, XboxGamerStats mGamerStats)
		{
			this.mItems = new XboxItem[this.mWidth, this.mHeight];
			int num = 0;
			int num2 = 0;
			foreach (CharacterSettings characterSettings in chars)
			{
				this.mItems[num, num2] = new XboxItem(characterSettings.texCoord.X, characterSettings.texCoord.Y, characterSettings, false, true);
				this.mItems[num, num2].ID = characterSettings.name;
				if (characterSettings.PointsToUnlock > 0 && (mGamerStats == null || !mGamerStats.UnlockedCharacters.Contains(characterSettings.name)))
				{
					this.mItems[num, num2].Locked = true;
				}
				num++;
				if (num >= this.mWidth)
				{
					num = 0;
					num2++;
				}
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0004EA10 File Offset: 0x0004CC10
		public void PopulateHats(List<Hat> hats, XboxGamerStats mGamerStats)
		{
			this.mItems = new XboxItem[this.mWidth, this.mHeight];
			int num = 0;
			int num2 = 0;
			foreach (Hat hat in hats)
			{
				this.mItems[num, num2] = new XboxItem(hat.Tex.X, hat.Tex.Y, hat, false, true);
				this.mItems[num, num2].ID = hat.Name;
				if (hat.ZHCost > 0 && (mGamerStats == null || !mGamerStats.UnlockedHats.Contains(hat.Name)))
				{
					this.mItems[num, num2].Locked = true;
				}
				num++;
				if (num >= this.mWidth)
				{
					num = 0;
					num2++;
				}
			}
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0004EB08 File Offset: 0x0004CD08
		public void ReselectItem(string name)
		{
			for (int i = 0; i < this.mWidth; i++)
			{
				for (int j = 0; j < this.mHeight; j++)
				{
					XboxItem xboxItem = this.mItems[i, j];
					if (xboxItem != null && xboxItem.ID == name)
					{
						this.mCurrentX = i;
						this.mCurrentY = j;
						if (this.ItemHighlighted != null)
						{
							this.ItemHighlighted(this.mItems[this.mCurrentX, this.mCurrentY]);
						}
						return;
					}
				}
			}
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0004EB90 File Offset: 0x0004CD90
		public void UpdateGunListOwnAfford()
		{
			for (int i = 0; i < this.mWidth; i++)
			{
				for (int j = 0; j < this.mHeight; j++)
				{
					XboxItem xboxItem = this.mItems[i, j];
					if (xboxItem != null)
					{
						if (xboxItem is XboxAmmoItem)
						{
							XboxAmmoItem xboxAmmoItem = xboxItem as XboxAmmoItem;
							bool flag = this.mPlayer.Stats.GetMoney() < xboxAmmoItem.mCost;
							xboxItem.mAfford = !flag;
						}
						else
						{
							GunStats gunStats = xboxItem.Tag as GunStats;
							bool flag2 = this.mPlayer.OwnsGun(gunStats);
							bool flag3 = this.mPlayer.Stats.GetMoney() < gunStats.Cost;
							if (flag2)
							{
								flag3 = false;
							}
							xboxItem.mAfford = !flag3;
							xboxItem.mOwn = flag2;
						}
					}
				}
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0004EC66 File Offset: 0x0004CE66
		public void RefireEvent(bool resetSelection = true)
		{
			if (resetSelection)
			{
				this.mCurrentX = 0;
				this.mCurrentY = 0;
			}
			if (this.ItemHighlighted != null)
			{
				this.ItemHighlighted(this.mItems[this.mCurrentX, this.mCurrentY]);
			}
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0004ECA4 File Offset: 0x0004CEA4
		private void UpdateScrollbar()
		{
			Rectangle rectangle = new Rectangle((int)this.mTopLeft.X + 34 * this.mWidth - 4, (int)this.mTopLeft.Y - 8, 16, 16);
			Rectangle rectangle2 = new Rectangle((int)this.mTopLeft.X + 34 * this.mWidth - 4, (int)this.mTopLeft.Y - 8 + this.mWindowHeight * 34, 16, 16);
			if (InputManager.LeftMouseClicked())
			{
				if (rectangle.Contains(InputManager.GetMousePosition()))
				{
					this.mWindowY--;
					return;
				}
				if (rectangle2.Contains(InputManager.GetMousePosition()))
				{
					this.mWindowY++;
				}
			}
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0004ED60 File Offset: 0x0004CF60
		private void DrawScrollbar(SpriteBatch spriteBatch)
		{
			if (this.mWindowHeight == this.mHeight)
			{
				return;
			}
			float num = (float)this.mWindowHeight / (float)this.mHeight;
			float num2 = (float)this.mWindowY / (float)(this.mHeight - this.mWindowHeight);
			num2 = Math.Min(num2, 1f);
			Color color = new Color(0.35f, 0.35f, 0.35f);
			Rectangle destinationRectangle = new Rectangle((int)this.mTopLeft.X + 34 * this.mWidth + 2, (int)this.mTopLeft.Y + 2, 4, this.mWindowHeight * 34 - 4);
			spriteBatch.Draw(Global.Pixel, destinationRectangle, color);
			int num3 = (int)this.mTopLeft.Y + 2 + (int)(34f * num2 * (float)this.mWindowHeight);
			num3 = Math.Min(destinationRectangle.Bottom - (int)(num * 34f), num3);
			Rectangle destinationRectangle2 = new Rectangle((int)this.mTopLeft.X + 34 * this.mWidth + 2, num3, 4, (int)(num * 34f));
			spriteBatch.Draw(Global.Pixel, destinationRectangle2, Color.White);
			Rectangle destinationRectangle3 = new Rectangle((int)this.mTopLeft.X + 34 * this.mWidth - 4, (int)this.mTopLeft.Y - 8, 16, 16);
			if (destinationRectangle3.Contains(InputManager.GetMousePosition()))
			{
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle3, new Rectangle?(Global.GetTexRectange(5, 21)), Color.Lerp(Color.Gray, Color.White, Global.Pulse));
			}
			else
			{
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle3, new Rectangle?(Global.GetTexRectange(5, 21)), Color.Gray);
			}
			Rectangle destinationRectangle4 = new Rectangle((int)this.mTopLeft.X + 34 * this.mWidth - 4, (int)this.mTopLeft.Y - 8 + this.mWindowHeight * 34, 16, 16);
			if (destinationRectangle4.Contains(InputManager.GetMousePosition()))
			{
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle4, new Rectangle?(Global.GetTexRectange(4, 21)), Color.Lerp(Color.Gray, Color.White, Global.Pulse));
				return;
			}
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle4, new Rectangle?(Global.GetTexRectange(4, 21)), Color.Gray);
		}

		// Token: 0x04000A2F RID: 2607
		private XboxItem[,] mItems;

		// Token: 0x04000A30 RID: 2608
		private Vector2 mTopLeft;

		// Token: 0x04000A31 RID: 2609
		public bool INVENTORY;

		// Token: 0x04000A32 RID: 2610
		private int mWidth;

		// Token: 0x04000A33 RID: 2611
		private int mHeight;

		// Token: 0x04000A34 RID: 2612
		private int mWindowWidth;

		// Token: 0x04000A35 RID: 2613
		private int mWindowHeight;

		// Token: 0x04000A36 RID: 2614
		private int mWindowX;

		// Token: 0x04000A37 RID: 2615
		private int mWindowY;

		// Token: 0x04000A38 RID: 2616
		private int mCurrentX;

		// Token: 0x04000A39 RID: 2617
		private int mCurrentY;

		// Token: 0x04000A3A RID: 2618
		private Rectangle mSelSrc;

		// Token: 0x04000A3D RID: 2621
		private Player mPlayer;

		// Token: 0x04000A3E RID: 2622
		private int mPlayerIndex;

		// Token: 0x04000A3F RID: 2623
		private bool mMouseMode;

		// Token: 0x04000A40 RID: 2624
		private bool mFirstLoop;

		// Token: 0x02000220 RID: 544
		// (Invoke) Token: 0x06000DF7 RID: 3575
		public delegate void ItemEvent(XboxItem item);
	}
}
