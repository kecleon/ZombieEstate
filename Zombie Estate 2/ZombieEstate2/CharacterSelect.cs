using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Talents;

namespace ZombieEstate2
{
	// Token: 0x020000DE RID: 222
	internal class CharacterSelect : SyncedObject
	{
		// Token: 0x060005C6 RID: 1478 RVA: 0x0002BE2C File Offset: 0x0002A02C
		public CharacterSelect()
		{
			PlayerStatKeeper.InitCharSettings();
			this.Players = new List<Player>();
			this.parent = new Player();
			this.parent.Index = 0;
			this.parent.PositionIndex = this.Players.Count;
			this.parent.GamerName = "TestPC";
			this.AddPlayer(this.parent);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0002BEAE File Offset: 0x0002A0AE
		public void Update()
		{
			bool active = ScreenFader.Active;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void Draw(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0002BEB6 File Offset: 0x0002A0B6
		private void AddPlayer(Player player)
		{
			this.Players.Add(player);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0002BEC4 File Offset: 0x0002A0C4
		private void RemovePlayer(Player player)
		{
			this.Players.Remove(player);
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0002BED3 File Offset: 0x0002A0D3
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x0002BEDB File Offset: 0x0002A0DB
		public bool OnHats
		{
			get
			{
				return this.onHats;
			}
			set
			{
				this.onHats = value;
				bool flag = this.onHats;
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0002BEEC File Offset: 0x0002A0EC
		public void SetupGame()
		{
			int num = 10;
			Global.PlayerList = new List<Player>();
			this.Players = new List<Player>();
			foreach (Selections selections in this.PlayerSelections)
			{
				string charName = selections.CharName;
				if (string.IsNullOrEmpty(charName))
				{
					num++;
				}
				else
				{
					Player player = new Player();
					CharacterSettings settings = PlayerStatKeeper.GetSettings(charName);
					player.TextureCoord = settings.texCoord;
					player.StartTextureCoord = settings.texCoord;
					player.Stats = new PlayerStats(PCCharacterSelect.Stats[settings], settings, player);
					player.Stats.CharSettings = settings;
					player.InitPlayer(PCCharacterSelect.Stats[settings], settings, num);
					player.Index = num - 10;
					player.AccessoryName = selections.AccessoryName;
					player.AccessoryTexCoord = selections.AccessoryTex;
					player.GamerName = selections.PlayerName;
					player.SomethingChanged = true;
					if (Global.Player == null)
					{
						Global.Player = player;
					}
					player.Health = player.SpecialProperties.MaxHealth;
					Global.PlayerList.Add(player);
					num++;
				}
			}
			this.StartGame();
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void SetupGameLOCAL()
		{
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0002C058 File Offset: 0x0002A258
		public void StartGame()
		{
			ScreenFader.Fade(delegate()
			{
				Global.Paused = false;
				Global.GameManager.StartGame(GameManager.LevelName);
			});
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0002C080 File Offset: 0x0002A280
		public void StartGameDEBUG()
		{
			this.PlayerSelections = new List<Selections>();
			Selections selections = new Selections();
			selections.AccessoryName = "None";
			selections.AccessoryTex = new Point(63, 63);
			selections.CharName = "Colonel Popcorn";
			selections.Index = 0;
			selections.PlayerName = "DEBUG";
			TalentPage talentPage = new TalentPage(1, Vector2.Zero, 0);
			talentPage.Load();
			selections.TalentsData = talentPage.BuildString();
			this.PlayerSelections.Add(selections);
			this.SetupGame();
		}

		// Token: 0x040005BA RID: 1466
		private const byte MESSAGE_CHAR_SELECTED = 0;

		// Token: 0x040005BB RID: 1467
		private const byte MESSAGE_GAME_START = 1;

		// Token: 0x040005BC RID: 1468
		private List<Player> Players;

		// Token: 0x040005BD RID: 1469
		private PCHatStore HatStore;

		// Token: 0x040005BE RID: 1470
		private bool onHats;

		// Token: 0x040005BF RID: 1471
		private bool AI;

		// Token: 0x040005C0 RID: 1472
		public List<Selections> PlayerSelections = new List<Selections>();

		// Token: 0x040005C1 RID: 1473
		private string prevSelected = "";

		// Token: 0x040005C2 RID: 1474
		private Player parent;
	}
}
