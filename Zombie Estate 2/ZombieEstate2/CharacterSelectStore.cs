using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000DF RID: 223
	internal class CharacterSelectStore : Store
	{
		// Token: 0x060005D1 RID: 1489 RVA: 0x0002C106 File Offset: 0x0002A306
		public CharacterSelectStore(Player parent, StoreManager manager) : base(parent, manager)
		{
			base.FlashColorBG(Color.Black, 1f);
			this.CurrentBGColor = Color.Black;
			this.WaitingForData = false;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0002C134 File Offset: 0x0002A334
		public override void PurchaseItem(Item selectedItem)
		{
			if (this.WaitingForData)
			{
				return;
			}
			CharacterSettings settings = PlayerStatKeeper.GetSettings(selectedItem.label);
			this.parent.TextureCoord = settings.texCoord;
			this.parent.StartTextureCoord = settings.texCoord;
			this.parent.Stats = new PlayerStats(this.Stats[settings], settings, this.parent);
			this.parent.Stats.CharSettings = settings;
			this.parent.InitPlayer(this.Stats[settings], settings, 0);
			this.screen.Locked = true;
			base.FlashColorBG(Color.White, 1f);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0002C1E1 File Offset: 0x0002A3E1
		public override void AddItems()
		{
			this.LoadCharacters();
			this.FinishAddingItems();
			this.LoadGamerData();
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0002C1F8 File Offset: 0x0002A3F8
		private void FinishAddingItems()
		{
			for (int i = 0; i < PlayerStatKeeper.CharacterSettings.Count; i++)
			{
				CharacterSettings characterSettings = PlayerStatKeeper.CharacterSettings[i];
				Item item = new Item(characterSettings.texCoord, 0, characterSettings.name, characterSettings.description);
				this.screen.AddItem(item);
			}
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00002EF9 File Offset: 0x000010F9
		public override void DrawDataNextToTitle(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0002C24C File Offset: 0x0002A44C
		public override void DrawPlayerData(SpriteBatch spriteBatch)
		{
			if (base.GetCurrentItem() == null)
			{
				return;
			}
			Vector2 pos = new Vector2(this.Position.X + 822f - 82f, this.Position.Y + 10f);
			new StatLine("Player: ", this.PlayerData.GamerName, pos, spriteBatch, Color.White, 150, 28, true);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0002C2B6 File Offset: 0x0002A4B6
		public override void DrawItemIcons(SpriteBatch spriteBatch)
		{
			if (this.Locked)
			{
				base.DrawItemIcons(spriteBatch);
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void LoadCharacters()
		{
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0002C2C8 File Offset: 0x0002A4C8
		private void LoadChar(string name)
		{
			CharacterSettings item;
			XMLSaverLoader.LoadObject<CharacterSettings>("Characters\\" + name + ".chr", out item);
			CharacterSelectStore.characters.Add(item);
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0002C2F8 File Offset: 0x0002A4F8
		public bool Locked
		{
			get
			{
				return this.screen.Locked;
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0002C305 File Offset: 0x0002A505
		private void LoadGamerData()
		{
			this.WaitingForData = false;
			this.LoadGamerDataPC();
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0002C314 File Offset: 0x0002A514
		private void LoadGamerDataXbox()
		{
			this.xboxLoader = new XboxSaveLoad(this.parent, true, new SaveLoadCompleted(this.LoadComplete), null);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0002C338 File Offset: 0x0002A538
		private void LoadGamerDataPC()
		{
			GamerData data;
			if (XMLSaverLoader.LoadObject<GamerData>("Players\\" + this.parent.GamerName + "_Data.xml", out data))
			{
				this.LoadComplete("OK", data);
				return;
			}
			this.LoadComplete("FILE NOT FOUND", data);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0002C384 File Offset: 0x0002A584
		private void LoadComplete(string status, GamerData data)
		{
			if (status == "FILE NOT FOUND" || status == "ERROR" || data == null)
			{
				this.PlayerData = new GamerData();
				this.PlayerData.parent = this.parent;
				this.PlayerData.GamerName = this.parent.GamerName;
				this.WaitingForData = false;
				this.parent.PlayerData = this.PlayerData;
				return;
			}
			this.PlayerData = data;
			this.PlayerData.parent = this.parent;
			this.PlayerData.GamerName = this.parent.GamerName;
			this.WaitingForData = false;
			this.parent.PlayerData = this.PlayerData;
		}

		// Token: 0x040005C3 RID: 1475
		private static List<CharacterSettings> characters = new List<CharacterSettings>();

		// Token: 0x040005C4 RID: 1476
		private Dictionary<CharacterSettings, CharacterStats> Stats;

		// Token: 0x040005C5 RID: 1477
		private XboxSaveLoad xboxLoader;

		// Token: 0x040005C6 RID: 1478
		private GamerData PlayerData;
	}
}
