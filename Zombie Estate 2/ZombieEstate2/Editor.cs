using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x02000106 RID: 262
	public class Editor
	{
		// Token: 0x06000719 RID: 1817 RVA: 0x000026B9 File Offset: 0x000008B9
		public Editor(Game game)
		{
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00036024 File Offset: 0x00034224
		public void Init(Game game, Level level)
		{
			this.level = level;
			this.picker = new TexturePicker(game);
			Editor.editorObjects = new MasterCache(game, 1, 6000, false, "Editor_MasterCache");
			List<string> list = new List<string>();
			list.Add("Level 0");
			list.Add("Level 1");
			if (level.SectorCount() > 2)
			{
				list.Add("Level 2");
			}
			this.levelButton = new Button(new Vector2((float)(Global.ScreenRect.Width - 148), 200f), list);
			list = new List<string>();
			list.Add("Wall Editing");
			list.Add("Floor Editing");
			this.wallFloorButton = new Button(new Vector2((float)(Global.ScreenRect.Width - 148), 250f), list);
			list = new List<string>();
			list.Add("Normal Wall");
			list.Add("Half Wall");
			list.Add("Fake Wall");
			list.Add("Window Wall");
			this.wallTypeButton = new Button(new Vector2((float)(Global.ScreenRect.Width - 148), 300f), list);
			list = new List<string>();
			list.Add("TPIs Off");
			list.Add("TPIs On");
			this.tpisToggle = new Button(new Vector2((float)(Global.ScreenRect.Width - 148), 150f), list);
			this.PropertyManager = new TilePropertyManager(level.MainSector);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000361A4 File Offset: 0x000343A4
		public void UpdateEditor(float elapsed)
		{
			if (this.freezeTime)
			{
				elapsed = 0f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.P) && this.prevKeyState.IsKeyUp(Keys.P))
			{
				this.freezeTime = !this.freezeTime;
			}
			this.Buttons();
			if (InputManager.ButtonPressed(Keys.X, 0) && InputManager.ButtonHeld(Keys.LeftControl, 0))
			{
				this.level.ComputerShadows();
				this.level.SaveLevelVertices();
				this.level.SaveLevelVerticesOptimized();
				Terminal.WriteMessage("Saved vertices...", MessageType.SAVELOAD);
			}
			if (InputManager.ButtonPressed(Keys.C, 0))
			{
				Tile tileAtLocation = this.level.GetSector(this.sectorIndex).GetTileAtLocation(MouseHandler.GetPickedPosition());
				Terminal.WriteMessage(string.Format("Current Location: {0}, {1}", tileAtLocation.x, tileAtLocation.y), MessageType.COMMAND);
				Terminal.WriteMessage(string.Format("GROUND: {0}, {1}", tileAtLocation.GetGroundTexCoords().X, tileAtLocation.GetGroundTexCoords().Y, MessageType.COMMAND));
			}
			this.mouse = Mouse.GetState();
			if (this.EDITING)
			{
				Editor.editorObjects.UpdateCaches(elapsed);
				this.TileIndicator();
				if (!this.floorEditing)
				{
					this.UpdatePlacingWalls(this.level.GetSector(this.sectorIndex));
				}
				else
				{
					this.UpdatePlacingFloors(this.level.GetSector(this.sectorIndex));
				}
				this.picker.Update();
				if (InputManager.ButtonPressed(Keys.G, 0))
				{
					this.level.MainSector.AddBlankWalls();
				}
				if (InputManager.ButtonPressed(Keys.I, 0))
				{
					Global.GameState = GameState.Playing;
					this.EDITING = false;
				}
				if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.End) && !this.prevKeyState.IsKeyDown(Keys.End))
				{
					Console.WriteLine("CLEARING LEVEL");
					this.level.ClearLevel();
				}
				if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.M) && !this.prevKeyState.IsKeyDown(Keys.M))
				{
					Console.WriteLine("CLEARING LEVEL TPIs");
					this.level.ClearTPIs();
				}
				if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.S) && !this.prevKeyState.IsKeyDown(Keys.S))
				{
					this.level.SaveLevel();
				}
				if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.P) && !this.prevKeyState.IsKeyDown(Keys.P))
				{
					new NewPathfinder(this.level.MainSector);
				}
				if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.O) && !this.prevKeyState.IsKeyDown(Keys.O))
				{
					this.level.LoadLevel();
					Global.KillAllZombies();
				}
				if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.U) && !this.prevKeyState.IsKeyDown(Keys.U))
				{
					Terminal.WriteMessage("Drawing only current sector: " + Editor.DrawOnlyCurrentSector.ToString());
					Editor.DrawOnlyCurrentSector = !Editor.DrawOnlyCurrentSector;
				}
				this.TileProps();
			}
			this.prevMouse = Mouse.GetState();
			this.prevKeyState = Keyboard.GetState();
			Editor.editorObjects.LATE_UPDATE();
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00036530 File Offset: 0x00034730
		private void TileProps()
		{
			if (InputManager.ButtonHeld(Keys.LeftControl, 0) && InputManager.ButtonPressed(Keys.Y, 0))
			{
				this.level.MainSector.NoPathOnEmptyFloors();
			}
			if (InputManager.ButtonHeld(Keys.RightControl, 0))
			{
				this.InputTileProp(Keys.NumPad1, TilePropertyType.PlayerOneSpawn);
				this.InputTileProp(Keys.NumPad2, TilePropertyType.PlayerTwoSpawn);
				this.InputTileProp(Keys.NumPad3, TilePropertyType.PlayerThreeSpawn);
				this.InputTileProp(Keys.NumPad4, TilePropertyType.PlayerFourSpawn);
				return;
			}
			this.InputTileProp(Keys.Y, TilePropertyType.NoPath);
			this.InputTileProp(Keys.NumPad7, TilePropertyType.NoPath);
			this.InputTileProp(Keys.NumPad8, TilePropertyType.CarePackageSpawn);
			this.InputTileProp(Keys.NumPad9, TilePropertyType.ShopKeep);
			this.InputTileProp(Keys.NumPad4, TilePropertyType.NoSpawn);
			this.InputTileProp(Keys.NumPad6, TilePropertyType.BossArea);
			this.InputTileProp(Keys.NumPad5, TilePropertyType.DemonFire);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x000365D8 File Offset: 0x000347D8
		private void InputTileProp(Keys key, TilePropertyType type)
		{
			if (InputManager.ButtonPressed(key, 0))
			{
				Vector3 pickedPosition = MouseHandler.GetPickedPosition();
				Tile tileAtLocation = this.level.MainSector.GetTileAtLocation(pickedPosition);
				if (tileAtLocation == null)
				{
					return;
				}
				if (tileAtLocation.TileProperties.Contains(type))
				{
					tileAtLocation.TileProperties.Remove(type);
				}
				else
				{
					tileAtLocation.TileProperties.Add(type);
				}
				Console.WriteLine(tileAtLocation.GetGroundTexCoords() + " - Added Prop: " + type.ToString());
				this.PropertyManager.BuildIndicators();
			}
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00036668 File Offset: 0x00034868
		private void Buttons()
		{
			this.levelButton.Update(this.prevMouse);
			this.wallFloorButton.Update(this.prevMouse);
			this.wallTypeButton.Update(this.prevMouse);
			this.tpisToggle.Update(this.prevMouse);
			string a = this.levelButton.CurrentValue();
			if (a == "Level 0")
			{
				this.sectorIndex = 0;
			}
			if (a == "Level 1")
			{
				this.sectorIndex = 1;
			}
			if (a == "Level 2")
			{
				this.sectorIndex = 2;
			}
			if (this.wallFloorButton.CurrentValue() == "Wall Editing")
			{
				this.floorEditing = false;
			}
			else
			{
				this.floorEditing = true;
			}
			Editor.WallType = this.wallTypeButton.CurrentValue();
			string a2 = this.tpisToggle.CurrentValue();
			if (a2 == "TPIs On" && !this.PropertyManager.Shown)
			{
				this.PropertyManager.BuildIndicators();
				this.PropertyManager.ToggleIndicators();
			}
			if (a2 == "TPIs Off" && this.PropertyManager.Shown)
			{
				this.PropertyManager.ToggleIndicators();
			}
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00036798 File Offset: 0x00034998
		public void DrawHUD(SpriteBatch spriteBatch)
		{
			if (this.EDITING)
			{
				Rectangle destinationRectangle = new Rectangle(Global.ScreenRect.Width - 196, 0, 196, Global.ScreenRect.Height);
				spriteBatch.Draw(Global.Pixel, destinationRectangle, Color.Black);
				this.picker.Draw(spriteBatch);
				this.levelButton.Draw(spriteBatch);
				this.wallFloorButton.Draw(spriteBatch);
				this.wallTypeButton.Draw(spriteBatch);
				this.tpisToggle.Draw(spriteBatch);
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00036824 File Offset: 0x00034A24
		private void UpdatePlacingWalls(Sector currentSector)
		{
			float num = 0.11f;
			if (this.mouse.X > Global.ScreenRect.Width - 196)
			{
				return;
			}
			if (this.mouse.LeftButton == ButtonState.Pressed && this.prevMouse.LeftButton == ButtonState.Released)
			{
				this.placingWall = true;
				this.startPos = MouseHandler.GetPickedPosition(this.level.GetSector(this.sectorIndex).SectorLevel);
			}
			if (this.mouse.LeftButton == ButtonState.Released && this.placingWall)
			{
				this.placingWall = false;
				Vector3 pickedPosition = MouseHandler.GetPickedPosition(this.level.GetSector(this.sectorIndex).SectorLevel);
				if (Math.Abs(Vector3.Distance(pickedPosition, this.startPos)) > num)
				{
					Tile tileAtLocation = currentSector.GetTileAtLocation(this.startPos);
					if (tileAtLocation == null)
					{
						return;
					}
					Vector3 vector = new Vector3(pickedPosition.X - this.startPos.X, 0f, pickedPosition.Z - this.startPos.Z);
					vector.Normalize();
					if (Math.Abs(vector.X) > Math.Abs(vector.Z))
					{
						EditorSounds.DeepClick.Play(0.25f, 1f, 0f);
						if (vector.X > 0f)
						{
							tileAtLocation.ToggleWall(0, currentSector);
							return;
						}
						tileAtLocation.ToggleWall(2, currentSector);
						return;
					}
					else
					{
						EditorSounds.DeepClick.Play(0.25f, 1f, 0f);
						if (vector.Z > 0f)
						{
							tileAtLocation.ToggleWall(1, currentSector);
							return;
						}
						tileAtLocation.ToggleWall(3, currentSector);
					}
				}
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000369C0 File Offset: 0x00034BC0
		private void UpdatePlacingFloors(Sector currentSector)
		{
			if (this.mouse.X > Global.ScreenRect.Width - 196)
			{
				return;
			}
			if (this.mouse.LeftButton == ButtonState.Pressed)
			{
				Vector3 pickedPosition = MouseHandler.GetPickedPosition(this.level.GetSector(this.sectorIndex).SectorLevel);
				Tile tileAtLocation = currentSector.GetTileAtLocation(pickedPosition);
				if (tileAtLocation == null)
				{
					return;
				}
				if (tileAtLocation.GetGroundTexCoords() == Editor.SelectedTexCoords)
				{
					return;
				}
				tileAtLocation.BuildFloor(currentSector);
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00036A3C File Offset: 0x00034C3C
		public void DrawSectorAndObjects()
		{
			if (Global.GameState != GameState.Playing && Global.GameState != GameState.Editor)
			{
				return;
			}
			if (!this.TopView)
			{
				this.level.DrawLevel(false, this.sectorIndex);
			}
			else
			{
				this.level.DrawLevel(this.TopView, this.sectorIndex);
			}
			if (this.EDITING)
			{
				Editor.editorObjects.DrawObjects();
			}
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00036AA0 File Offset: 0x00034CA0
		private void TileIndicator()
		{
			if (this.SingleIndicator == null)
			{
				this.SingleIndicator = new GameObject();
				this.SingleIndicator.ActivateObject(new Vector3(0f, 0.01f, 0f), new Point(9, 35));
				this.SingleIndicator.XRotation = 1.5707964f;
				this.SingleIndicator.scale = 0.25f;
				this.SingleIndicator.AffectedByGravity = false;
				Editor.editorObjects.CreateObject(this.SingleIndicator);
			}
			if (Mouse.GetState().LeftButton != ButtonState.Pressed || this.floorEditing)
			{
				Tile tileAtLocation = this.level.MainSector.GetTileAtLocation(MouseHandler.GetPickedPosition(this.level.GetSector(this.sectorIndex).SectorLevel));
				if (tileAtLocation != null)
				{
					this.SingleIndicator.Position.X = (float)tileAtLocation.x + 0.5f;
					this.SingleIndicator.Position.Z = (float)tileAtLocation.y + 0.5f;
					int num = -1;
					if (Global.MainOnTop)
					{
						num = 1;
					}
					this.SingleIndicator.Position.Y = (float)num * MouseHandler.GetPickedPosition(this.level.GetSector(this.sectorIndex).SectorLevel).Y + 0.15f;
				}
			}
		}

		// Token: 0x04000708 RID: 1800
		private MouseState mouse;

		// Token: 0x04000709 RID: 1801
		private MouseState prevMouse;

		// Token: 0x0400070A RID: 1802
		private TexturePicker picker;

		// Token: 0x0400070B RID: 1803
		private bool placingWall;

		// Token: 0x0400070C RID: 1804
		private bool floorEditing;

		// Token: 0x0400070D RID: 1805
		private Vector3 startPos;

		// Token: 0x0400070E RID: 1806
		private KeyboardState prevKeyState;

		// Token: 0x0400070F RID: 1807
		private GameObject[] tileIndicator;

		// Token: 0x04000710 RID: 1808
		private GameObject SingleIndicator;

		// Token: 0x04000711 RID: 1809
		public static MasterCache editorObjects;

		// Token: 0x04000712 RID: 1810
		private TilePropertyManager PropertyManager;

		// Token: 0x04000713 RID: 1811
		private bool freezeTime;

		// Token: 0x04000714 RID: 1812
		public bool TopView;

		// Token: 0x04000715 RID: 1813
		public bool EDITING;

		// Token: 0x04000716 RID: 1814
		public static string WallType = "Normal Wall";

		// Token: 0x04000717 RID: 1815
		private int sectorIndex;

		// Token: 0x04000718 RID: 1816
		private Button levelButton;

		// Token: 0x04000719 RID: 1817
		private Button wallFloorButton;

		// Token: 0x0400071A RID: 1818
		private Button wallTypeButton;

		// Token: 0x0400071B RID: 1819
		private Button tpisToggle;

		// Token: 0x0400071C RID: 1820
		public static bool DrawOnlyCurrentSector = false;

		// Token: 0x0400071D RID: 1821
		public Level level;

		// Token: 0x0400071E RID: 1822
		public static Point SelectedTexCoords = new Point(0, 0);
	}
}
