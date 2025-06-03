using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.UI.Menus.MainMenuSystem
{
	// Token: 0x0200016B RID: 363
	internal class MenuTileManager
	{
		// Token: 0x06000B0B RID: 2827 RVA: 0x0005B314 File Offset: 0x00059514
		public MenuTileManager()
		{
			this.BigTile = Global.Content.Load<Texture2D>("MenuTiles//MenuTileBig");
			this.BG = Global.Content.Load<Texture2D>("MenuTiles\\BG_Temp");
			this.Tiles = new List<MenuTile>();
			Rectangle aspectRatioRect = Global.GetAspectRatioRect(Global.ScreenRect, this.BG);
			MenuTileManager.TopLeft = new Vector2((float)aspectRatioRect.X, (float)aspectRatioRect.Y);
			this.MenuPos = new Vector2((float)(aspectRatioRect.X + 508), (float)(aspectRatioRect.Y + this.BG.Height / 2 - this.BigTile.Height / 2));
			this.MenuInPos = new Vector2((float)(aspectRatioRect.X - 100), (float)(aspectRatioRect.Y + this.BG.Height / 2 - this.BigTile.Height / 2));
			this.MenuMain = new Vector2(this.MenuPos.X + 75f, this.MenuPos.Y + 4f);
			this.Bubble = new SpeechBubble(new Vector2((float)(aspectRatioRect.X + 520), (float)(aspectRatioRect.Y + aspectRatioRect.Height - 180)));
			int num = (int)Global.GetScreenCenter().X - 526;
			int num2 = (int)Global.GetScreenCenter().X - 526 + 168 + 8;
			int num3 = (int)Global.GetScreenCenter().Y - 340;
			int num4 = (int)Global.GetScreenCenter().Y - 340 + 168 + 8;
			int num5 = (int)Global.GetScreenCenter().Y - 340 + 352;
			int num6 = (int)Global.GetScreenCenter().Y - 340 + 528;
			this.Tiles.Add(new Tile_Singleplayer(new Vector2((float)num, (float)num3), this.MenuMain));
			this.Tiles.Add(new Tile_Multiplayer(new Vector2((float)num2, (float)num3), this.MenuMain));
			this.Tiles.Add(new Tile_Talents(new Vector2((float)num, (float)num4), this.MenuMain));
			this.Tiles.Add(new MenuTile("SinglePlayer", new Vector2((float)num2, (float)num4), this.MenuMain));
			this.Tiles.Add(new MenuTile("Achievements", new Vector2((float)num, (float)num5), this.MenuMain));
			this.Tiles.Add(new MenuTile("Multiplayer", new Vector2((float)num2, (float)num5), this.MenuMain));
			this.Tiles.Add(new MenuTile("DLC", new Vector2((float)num, (float)num6), this.MenuMain));
			this.Tiles.Add(new MenuTile("Settings", new Vector2((float)num2, (float)num6), this.MenuMain));
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0005B600 File Offset: 0x00059800
		public void Update()
		{
			if (!MenuTileManager.SubMenuActive)
			{
				this.UpdateTiles();
				this.UpdateMenuState();
			}
			if (this.CurrentTile != null)
			{
				this.CurrentTile.UpdateMenu();
			}
			this.Bubble.Update();
			MenuTileManager.ShakeVectorOffset = new Vector2(Camera.shakeDir.X, Camera.shakeDir.Y);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0005B65C File Offset: 0x0005985C
		private void UpdateTiles()
		{
			foreach (MenuTile menuTile in this.Tiles)
			{
				menuTile.Update();
				if (InputManager.LeftMouseClicked() && menuTile.Bounds.Contains(InputManager.GetMousePosition()))
				{
					if (menuTile == this.CurrentTile)
					{
						this.State = MenuTileManager.MenuState.GoingIn;
						this.CurrentTile = null;
					}
					else if (menuTile.Idle)
					{
						menuTile.Fire(1f);
						this.FireMenu();
						this.Bubble.GiveString(menuTile.ToolTip);
						this.CurrentTile = menuTile;
					}
				}
			}
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0005B714 File Offset: 0x00059914
		public void Draw(SpriteBatch spriteBatch)
		{
			this.DrawBG(spriteBatch);
			foreach (MenuTile menuTile in this.Tiles)
			{
				if (menuTile != this.CurrentTile)
				{
					if (menuTile.Bounds.Contains(InputManager.GetMousePosition()))
					{
						menuTile.DrawTile(spriteBatch, true);
					}
					else
					{
						menuTile.DrawTile(spriteBatch, false);
					}
				}
			}
			if (this.CurrentTile != null)
			{
				this.CurrentTile.DrawTile(spriteBatch, true);
				if (this.State == MenuTileManager.MenuState.Out)
				{
					this.CurrentTile.DrawMenu(spriteBatch);
				}
			}
			if (!MenuTileManager.SubMenuActive)
			{
				this.Bubble.Draw(spriteBatch);
			}
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0005B7D4 File Offset: 0x000599D4
		private void DrawBG(SpriteBatch spriteBatch)
		{
			Rectangle aspectRatioRect = Global.GetAspectRatioRect(Global.ScreenRect, this.BG);
			aspectRatioRect.X += (int)MenuTileManager.ShakeVectorOffset.X;
			aspectRatioRect.Y += (int)MenuTileManager.ShakeVectorOffset.Y;
			spriteBatch.Draw(this.BG, aspectRatioRect, Color.White);
			spriteBatch.Draw(this.BigTile, Vector2.Lerp(this.MenuInPos, this.MenuPos, this.MenuDelta) + MenuTileManager.ShakeVectorOffset, Color.White);
			aspectRatioRect.Width = 508;
			spriteBatch.Draw(this.BG, aspectRatioRect, new Rectangle?(new Rectangle(0, 0, 508, this.BG.Height)), Color.White);
			spriteBatch.Draw(Global.Pixel, new Rectangle(0, 0, Global.GetAspectRatioRect(Global.ScreenRect, this.BG).X, Global.ScreenRect.Height), Color.Black);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0005B8D4 File Offset: 0x00059AD4
		private void UpdateMenuState()
		{
			float num = 0.05f;
			switch (this.State)
			{
			case MenuTileManager.MenuState.GoingIn:
				this.MenuDelta -= num;
				if (this.MenuDelta <= 0f)
				{
					this.MenuDelta = 0f;
					this.State = MenuTileManager.MenuState.In;
					if (this.CurrentTile == null)
					{
						this.ShakeAll(0.25f);
						return;
					}
				}
				break;
			case MenuTileManager.MenuState.GoingOut:
				this.MenuDelta += num;
				if (this.MenuDelta >= 1f)
				{
					this.MenuDelta = 1f;
					this.State = MenuTileManager.MenuState.Out;
					this.ShakeAll(0.55f);
				}
				break;
			case MenuTileManager.MenuState.In:
				if (this.CurrentTile != null)
				{
					this.State = MenuTileManager.MenuState.GoingOut;
					return;
				}
				break;
			case MenuTileManager.MenuState.Out:
				break;
			default:
				return;
			}
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0005B991 File Offset: 0x00059B91
		private void FireMenu()
		{
			if (this.State == MenuTileManager.MenuState.GoingIn || this.State == MenuTileManager.MenuState.In)
			{
				this.State = MenuTileManager.MenuState.GoingOut;
				return;
			}
			if (this.State == MenuTileManager.MenuState.GoingOut || this.State == MenuTileManager.MenuState.Out)
			{
				this.State = MenuTileManager.MenuState.GoingIn;
			}
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0005B9C8 File Offset: 0x00059BC8
		private void ShakeAll(float intensity)
		{
			foreach (MenuTile menuTile in this.Tiles)
			{
				menuTile.Fire(intensity);
			}
			Camera.ShakeCamera(0.4f * intensity, 8f * intensity);
		}

		// Token: 0x04000BC9 RID: 3017
		private List<MenuTile> Tiles;

		// Token: 0x04000BCA RID: 3018
		private Texture2D BigTile;

		// Token: 0x04000BCB RID: 3019
		private SpeechBubble Bubble;

		// Token: 0x04000BCC RID: 3020
		private Vector2 MenuMain;

		// Token: 0x04000BCD RID: 3021
		private MenuTile CurrentTile;

		// Token: 0x04000BCE RID: 3022
		private Texture2D BG;

		// Token: 0x04000BCF RID: 3023
		public static Vector2 TopLeft;

		// Token: 0x04000BD0 RID: 3024
		public static bool SubMenuActive = false;

		// Token: 0x04000BD1 RID: 3025
		private Vector2 MenuPos;

		// Token: 0x04000BD2 RID: 3026
		private Vector2 MenuInPos;

		// Token: 0x04000BD3 RID: 3027
		private float MenuDelta;

		// Token: 0x04000BD4 RID: 3028
		private MenuTileManager.MenuState State = MenuTileManager.MenuState.In;

		// Token: 0x04000BD5 RID: 3029
		public static Vector2 ShakeVectorOffset = new Vector2(0f, 0f);

		// Token: 0x0200022A RID: 554
		private enum MenuState
		{
			// Token: 0x04000E75 RID: 3701
			GoingIn,
			// Token: 0x04000E76 RID: 3702
			GoingOut,
			// Token: 0x04000E77 RID: 3703
			In,
			// Token: 0x04000E78 RID: 3704
			Out
		}
	}
}
