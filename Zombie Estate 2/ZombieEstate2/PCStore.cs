using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000CB RID: 203
	internal class PCStore : PCItemScreenController
	{
		// Token: 0x0600052C RID: 1324 RVA: 0x00027B8C File Offset: 0x00025D8C
		public PCStore(Player player, int width, int height)
		{
			this.Player = player;
			this.LoadStoreBackground();
			this.CalculateStoreBGPosition();
			this.ITEMSCREENPOS = new Rectangle(366, 128, 816, 284);
			this.itemScreen = new PCItemScreen(width, height, new Rectangle(366, 128, 816, 284), this, this.storeBackgroundPosition);
			this.storeElements = new List<StoreElement>();
			this.AddStoreElements();
			this.AddItems();
			this.AddEmptyItems();
			this.title = "NO TITLE";
			this.width = width;
			this.height = height;
			ToolTip.Init(this.StoreOffsetX, this.StoreOffsetY);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00027C54 File Offset: 0x00025E54
		public PCStore(Player player, int width, int height, Rectangle itemScreenPos, bool verticle)
		{
			this.ITEMSCREENPOS = itemScreenPos;
			this.Player = player;
			this.LoadStoreBackground();
			this.CalculateStoreBGPosition();
			this.itemScreen = new PCItemScreen(width, height, itemScreenPos, this, this.storeBackgroundPosition);
			this.storeElements = new List<StoreElement>();
			this.AddStoreElements();
			this.AddItems();
			this.AddEmptyItems();
			this.title = "NO TITLE";
			this.width = width;
			this.height = height;
			ToolTip.Init(this.StoreOffsetX, this.StoreOffsetY);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00027CF0 File Offset: 0x00025EF0
		public PCStore(Player player, int width, int height, int screenWidth)
		{
			this.Player = player;
			this.LoadStoreBackground();
			this.CalculateStoreBGPosition();
			this.ITEMSCREENPOS = new Rectangle(366, 128, screenWidth, 284);
			this.itemScreen = new PCItemScreen(width, height, new Rectangle(366, 128, screenWidth, 284), this, this.storeBackgroundPosition);
			this.storeElements = new List<StoreElement>();
			this.AddStoreElements();
			this.AddItems();
			this.AddEmptyItems();
			this.title = "NO TITLE";
			this.width = width;
			this.height = height;
			ToolTip.Init(this.StoreOffsetX, this.StoreOffsetY);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00027DB4 File Offset: 0x00025FB4
		public void ReInit()
		{
			PCItem currentItem = this.CurrentItem;
			this.itemScreen = new PCItemScreen(this.width, this.height, this.ITEMSCREENPOS, this, this.storeBackgroundPosition);
			this.AddItems();
			this.AddEmptyItems();
			if (currentItem != null)
			{
				this.ItemScreen.SelectName(currentItem.Title);
			}
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00023560 File Offset: 0x00021760
		public virtual void LoadStoreBackground()
		{
			this.StoreBackground = Global.Content.Load<Texture2D>("Store\\PCStore\\CharacterSelect");
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void AddItems()
		{
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void AddStoreElements()
		{
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00027E0C File Offset: 0x0002600C
		public virtual void AddEmptyItems()
		{
			this.ItemScreen.FillEmptyItems();
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00027E1C File Offset: 0x0002601C
		public void UpdateStore()
		{
			if (!this.MessageShown)
			{
				this.itemScreen.Update();
				for (int i = 0; i < this.storeElements.Count; i++)
				{
					this.storeElements[i].Update();
				}
			}
			if (this.MessageShown && this.dialog != null && !this.dialog.CLOSED)
			{
				this.dialog.Update();
			}
			ToolTip.Update();
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00027E90 File Offset: 0x00026090
		public void DrawStore(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.StoreBackground, this.storeBackgroundPosition, this.Player.HUDColor);
			this.ItemScreen.Draw(spriteBatch, this.Player.HUDColor);
			if (this.DrawTitle)
			{
				Shadow.DrawString(this.Title, Global.StoreFontBig, VerchickMath.CenterText(Global.StoreFontBig, new Vector2(762f + this.storeBackgroundPosition.X, 48f + this.storeBackgroundPosition.Y), this.Title), 2, Color.White, spriteBatch);
			}
			foreach (StoreElement storeElement in this.storeElements)
			{
				storeElement.Draw(spriteBatch);
			}
			this.DrawMisc(spriteBatch);
			if (this.DrawToolTip)
			{
				ToolTip.Draw(spriteBatch);
			}
			if (this.MessageShown && this.dialog != null)
			{
				this.dialog.Draw(spriteBatch);
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void DrawMisc(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00027F9C File Offset: 0x0002619C
		private void CalculateStoreBGPosition()
		{
			this.storeBackgroundPosition = new Vector2((float)((Global.ScreenRect.Width - this.StoreBackground.Width) / 2), (float)((Global.ScreenRect.Height - this.StoreBackground.Height) / 2));
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00027FDB File Offset: 0x000261DB
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x00027FE3 File Offset: 0x000261E3
		public PCItemScreen ItemScreen
		{
			get
			{
				return this.itemScreen;
			}
			set
			{
				this.itemScreen = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x00027FEC File Offset: 0x000261EC
		public int StoreOffsetX
		{
			get
			{
				return (int)this.storeBackgroundPosition.X;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00027FFA File Offset: 0x000261FA
		public int StoreOffsetY
		{
			get
			{
				return (int)this.storeBackgroundPosition.Y;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x00028008 File Offset: 0x00026208
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x00028010 File Offset: 0x00026210
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00028019 File Offset: 0x00026219
		public void AddStoreElement(StoreElement element)
		{
			this.storeElements.Add(element);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00028027 File Offset: 0x00026227
		public void RemoveStoreElement(StoreElement element)
		{
			this.storeElements.Remove(element);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00028038 File Offset: 0x00026238
		public void ShowMessage(string message, bool yesOrNo, PCButton.PressedDelegate Yes, PCButton.PressedDelegate No)
		{
			if (this.MessageShown)
			{
				return;
			}
			this.MessageShown = true;
			this.dialog = new StoreDialog(message, yesOrNo);
			this.dialog.YesButton.Pressed = Yes;
			if (yesOrNo)
			{
				this.dialog.NoButton.Pressed = No;
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00028088 File Offset: 0x00026288
		public void CloseMessage()
		{
			if (this.MessageShown && this.dialog != null)
			{
				this.dialog.CloseDialog();
				this.dialog = null;
				this.MessageShown = false;
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000280B3 File Offset: 0x000262B3
		public void FilterChanged()
		{
			this.ReInit();
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x000280BC File Offset: 0x000262BC
		public void DrawToolTipX(SpriteBatch spriteBatch, string toolTip)
		{
			int num = 320;
			int num2 = 64;
			int num3 = Math.Max(0, InputManager.GetMousePosition().Y + 32);
			int num4 = Math.Max(0, InputManager.GetMousePosition().X + 32);
			num3 = Math.Min(Global.ScreenRect.Height - num2 - 4, num3);
			num4 = Math.Min(Global.ScreenRect.Width - num - 4, num4);
			Rectangle destinationRectangle = new Rectangle(num4 - 2, num3 - 2, num + 4, num2 + 4);
			Rectangle destinationRectangle2 = new Rectangle(num4, num3, num, num2);
			spriteBatch.Draw(Global.Pixel, destinationRectangle, Color.White);
			spriteBatch.Draw(Global.Pixel, destinationRectangle2, Color.Black);
			Vector2 pos = new Vector2((float)(num4 + 2), (float)(num3 + 4));
			Shadow.DrawString(VerchickMath.WordWrapWidth(toolTip, num, Global.StoreFont), Global.StoreFont, pos, 1, Color.LightGray, spriteBatch);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00028194 File Offset: 0x00026394
		public virtual void SelectedObject(PCItem selectedItem)
		{
			this.CurrentItem = selectedItem;
			foreach (StoreElement storeElement in this.storeElements)
			{
				storeElement.ItemSelected(selectedItem);
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x000281EC File Offset: 0x000263EC
		public virtual void SelectionChange(PCItem highlight)
		{
			this.CurrentItem = highlight;
			foreach (StoreElement storeElement in this.storeElements)
			{
				storeElement.ItemSelected(highlight);
			}
		}

		// Token: 0x04000541 RID: 1345
		private PCItemScreen itemScreen;

		// Token: 0x04000542 RID: 1346
		private string title;

		// Token: 0x04000543 RID: 1347
		public Texture2D StoreBackground;

		// Token: 0x04000544 RID: 1348
		private Vector2 storeBackgroundPosition;

		// Token: 0x04000545 RID: 1349
		private List<StoreElement> storeElements;

		// Token: 0x04000546 RID: 1350
		public Player Player;

		// Token: 0x04000547 RID: 1351
		public PCItem CurrentItem;

		// Token: 0x04000548 RID: 1352
		public bool DrawTitle = true;

		// Token: 0x04000549 RID: 1353
		public bool MessageShown;

		// Token: 0x0400054A RID: 1354
		private StoreDialog dialog;

		// Token: 0x0400054B RID: 1355
		private int width;

		// Token: 0x0400054C RID: 1356
		private int height;

		// Token: 0x0400054D RID: 1357
		public bool DrawToolTip = true;

		// Token: 0x0400054E RID: 1358
		private Rectangle ITEMSCREENPOS;
	}
}
