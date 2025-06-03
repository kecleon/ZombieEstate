using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000C1 RID: 193
	internal class PCCharacterSelect : PCStore
	{
		// Token: 0x060004C1 RID: 1217 RVA: 0x000231F1 File Offset: 0x000213F1
		public PCCharacterSelect(Player player, CharacterSelect sel) : base(player, 10, 4, new Rectangle(306, 90, 454, 320), true)
		{
			PCCharacterSelect.LoadCharacters();
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void AddItems()
		{
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0002321C File Offset: 0x0002141C
		public override void AddStoreElements()
		{
			this.MainPortrait = new Portrait(new Vector2((float)(32 + base.StoreOffsetX), (float)(38 + base.StoreOffsetY)), 192, true);
			base.AddStoreElement(this.MainPortrait);
			base.AddStoreElement(new ItemPortrait(302 + base.StoreOffsetX, 512 + base.StoreOffsetY - 130, 128));
			base.AddStoreElement(new ItemDescription(this, 450, 420));
			this.GoButton = new PCButton("Go!", Color.LightBlue, new Vector2((float)(base.StoreOffsetX + 1058), (float)(base.StoreOffsetY + 540)));
			this.GoButton.Pressed = new PCButton.PressedDelegate(this.GoDelegate);
			base.AddStoreElement(this.GoButton);
			this.GoButton.Enabled = false;
			this.BackButton = new PCButton("Cancel", Color.Pink, new Vector2((float)(base.StoreOffsetX + 1058), (float)(base.StoreOffsetY + 620)));
			this.BackButton.Pressed = new PCButton.PressedDelegate(this.BackDelegate);
			base.AddStoreElement(this.BackButton);
			base.AddStoreElement(new CharWeapon(base.StoreOffsetX + 362, base.StoreOffsetY + 600, 64));
			this.hat = new CharHat(base.StoreOffsetX + 720, base.StoreOffsetY + 570, 64, this.Player.AccessoryTexCoord);
			base.AddStoreElement(this.hat);
			this.EquipButton = new PCButton("Change", Color.Orange, new Vector2((float)(base.StoreOffsetX + 720 + 72), (float)(base.StoreOffsetY + 570 + 20)), "Equip a hat that you've unlocked!");
			this.EquipButton.Pressed = new PCButton.PressedDelegate(this.EquipDelegate);
			base.AddStoreElement(this.EquipButton);
			base.AddStoreElements();
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00023426 File Offset: 0x00021626
		public static void LoadCharacters()
		{
			PCCharacterSelect.Stats = PlayerStatKeeper.LoadPlayerStatDictionary();
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00023432 File Offset: 0x00021632
		private void GoDelegate()
		{
			if (this.CurrentItem == null)
			{
				return;
			}
			this.select.SetupGameLOCAL();
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00023448 File Offset: 0x00021648
		private void EquipDelegate()
		{
			this.select.OnHats = true;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void StartGame()
		{
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00023456 File Offset: 0x00021656
		private void BackDelegate()
		{
			base.ShowMessage("Are you sure you want to go back to the main menu?", true, new PCButton.PressedDelegate(this.Back), new PCButton.PressedDelegate(base.CloseMessage));
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0002347C File Offset: 0x0002167C
		private void Back()
		{
			ScreenFader.Fade(delegate()
			{
				Global.GameManager.GotoMainMenu();
			});
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x000234A2 File Offset: 0x000216A2
		public override void SelectionChange(PCItem highlight)
		{
			base.SelectionChange(highlight);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000234AB File Offset: 0x000216AB
		public void ReInitHat()
		{
			this.hat.SetHat(this.Player.AccessoryTexCoord);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000234C4 File Offset: 0x000216C4
		public void UpdatePortraits()
		{
			foreach (Selections selections in this.select.PlayerSelections)
			{
				if (selections != null && !string.IsNullOrEmpty(selections.PlayerName) && this.OtherPlayers[selections.Index] != null)
				{
					this.OtherPlayers[selections.Index].SelectCharacter(base.ItemScreen.GetFromName(selections.CharName));
				}
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00023560 File Offset: 0x00021760
		public override void LoadStoreBackground()
		{
			this.StoreBackground = Global.Content.Load<Texture2D>("Store\\PCStore\\CharacterSelect");
		}

		// Token: 0x040004E5 RID: 1253
		private Portrait MainPortrait;

		// Token: 0x040004E6 RID: 1254
		private List<Portrait> OtherPlayers;

		// Token: 0x040004E7 RID: 1255
		public static Dictionary<CharacterSettings, CharacterStats> Stats;

		// Token: 0x040004E8 RID: 1256
		private static List<CharacterSettings> characters;

		// Token: 0x040004E9 RID: 1257
		private PCButton GoButton;

		// Token: 0x040004EA RID: 1258
		private PCButton BackButton;

		// Token: 0x040004EB RID: 1259
		private PCButton EquipButton;

		// Token: 0x040004EC RID: 1260
		private CharacterSelect select;

		// Token: 0x040004ED RID: 1261
		private CharHat hat;

		// Token: 0x040004EE RID: 1262
		private CharStatsComparer Comp;
	}
}
