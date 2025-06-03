using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000C6 RID: 198
	public class PCItem
	{
		// Token: 0x060004F4 RID: 1268 RVA: 0x000261C4 File Offset: 0x000243C4
		public PCItem(Point tex)
		{
			this.itemTex = Global.GetTexRectange(tex.X, tex.Y);
			this.itemDest = new Rectangle(0, 0, PCItem.Size, PCItem.Size);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0002622C File Offset: 0x0002442C
		public PCItem(Point tex, bool selectable)
		{
			this.itemTex = Global.GetTexRectange(tex.X, tex.Y);
			this.itemDest = new Rectangle(0, 0, PCItem.Size, PCItem.Size);
			this.Selectable = selectable;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00026298 File Offset: 0x00024498
		public PCItem(Point tex, string title, string desc, int cost)
		{
			this.itemTex = Global.GetTexRectange(tex.X, tex.Y);
			this.itemDest = new Rectangle(0, 0, PCItem.Size, PCItem.Size);
			this.Title = title;
			this.Description = desc;
			this.Cost = cost;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00026314 File Offset: 0x00024514
		public PCItem(Hat hat)
		{
			this.itemTex = Global.GetTexRectange(hat.Tex.X, hat.Tex.Y);
			this.itemDest = new Rectangle(0, 0, PCItem.Size, PCItem.Size);
			this.Title = hat.Name;
			this.Description = "";
			this.PointsToUnlock = hat.ZHCost;
			this.Tag = hat;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000263B4 File Offset: 0x000245B4
		public PCItem(Point tex, string title, string desc, int cost, string overlay)
		{
			this.itemTex = Global.GetTexRectange(tex.X, tex.Y);
			this.itemDest = new Rectangle(0, 0, PCItem.Size, PCItem.Size);
			this.Title = title;
			this.Description = desc;
			this.Cost = cost;
			this.OverlayText = overlay;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00026438 File Offset: 0x00024638
		public PCItem(GunStats gun, Player player) : this(new Point(gun.GunXCoord, gun.GunYCoord + 1))
		{
			this.Title = gun.GunName;
			this.Description = gun.StoreDescription;
			this.Tag = gun;
			this.Cost = gun.Cost;
			this.itemAmmoIcon = PCItem.GetAmmoRect(gun.AmmoType);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0002649C File Offset: 0x0002469C
		public PCItem(GunStats gun, Player player, string overlay) : this(new Point(gun.GunXCoord, gun.GunYCoord + 1))
		{
			this.Title = gun.GunName;
			this.Description = gun.StoreDescription;
			this.Tag = gun;
			this.Cost = PCItem.RoundCost((float)gun.Cost);
			this.OverlayText = overlay;
			this.itemAmmoIcon = PCItem.GetAmmoRect(gun.AmmoType);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0002650C File Offset: 0x0002470C
		public PCItem(GunStats gunStats, Gun gun) : this(new Point(gunStats.GunXCoord + gun.GetLevel() * 2, gunStats.GunYCoord + 1))
		{
			this.Title = gunStats.GunName;
			this.Tag = gun;
			this.Cost = PCItem.RoundCost((float)gunStats.Cost);
			if (gun.GetLevel() >= 3)
			{
				this.itemNextLevelTex = Global.GetTexRectange(4, 37);
				this.Description = "Gun fully upgraded.";
			}
			else
			{
				this.itemNextLevelTex = Global.GetTexRectange(gunStats.GunXCoord + (gun.GetLevel() + 1) * 2, gunStats.GunYCoord + 1);
				this.Description = gunStats.GunProperties[gun.GetLevel() + 1].LevelDescription;
			}
			if (gun.ammoType != AmmoType.SPECIAL)
			{
				this.UpgradeTokens = true;
			}
			this.Level = gun.GetLevel();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x000265E4 File Offset: 0x000247E4
		public PCItem(Talent talent) : this(talent.TexCoord)
		{
			this.Tag = talent;
			this.Title = talent.Name;
			this.Cost = 1;
			if (talent.CurrentLevel >= 3)
			{
				this.Description = "Fully upgraded!\n " + talent.Description[talent.CurrentLevel - 1];
			}
			else
			{
				this.Description = talent.Description[talent.CurrentLevel];
			}
			this.itemOverlay = Global.GetTexRectange(60, 49 + talent.CurrentLevel);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0002666A File Offset: 0x0002486A
		public void Lock()
		{
			this.itemOverlay = Global.GetTexRectange(9, 37);
			this.overlayColor = Color.White * 0.5f;
			this.Locked = true;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00026698 File Offset: 0x00024898
		public static Rectangle GetAmmoRect(AmmoType type)
		{
			Rectangle texRectange = Global.GetTexRectange(63, 63);
			if (type == AmmoType.ASSAULT)
			{
				texRectange = Global.GetTexRectange(0, 43);
			}
			if (type == AmmoType.SHELLS)
			{
				texRectange = Global.GetTexRectange(0, 45);
			}
			if (type == AmmoType.HEAVY)
			{
				texRectange = Global.GetTexRectange(0, 44);
			}
			if (type == AmmoType.EXPLOSIVE)
			{
				texRectange = Global.GetTexRectange(0, 46);
			}
			if (type == AmmoType.MELEE)
			{
				texRectange = Global.GetTexRectange(8, 47);
			}
			return texRectange;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x000266F0 File Offset: 0x000248F0
		public void DrawItem(SpriteBatch spriteBatch, Vector2 position, Color color)
		{
			if (!this.Enabled)
			{
				color = new Color(55, 55, 55);
			}
			Texture2D texture = this.Selected ? PCItem.SelectedBackground : PCItem.Background;
			if (!this.Selectable)
			{
				color = new Color(55, 55, 55);
			}
			spriteBatch.Draw(texture, position, color);
			this.itemDest.X = (int)position.X + 1;
			this.itemDest.Y = (int)position.Y + 1;
			if (this.Enabled)
			{
				spriteBatch.Draw(Global.MasterTexture, this.itemDest, new Rectangle?(this.itemTex), Color.White);
			}
			else
			{
				spriteBatch.Draw(Global.MasterTexture, this.itemDest, new Rectangle?(this.itemTex), Color.White * 0.5f);
			}
			Rectangle rectangle = this.itemOverlay;
			spriteBatch.Draw(Global.MasterTexture, this.itemDest, new Rectangle?(this.itemOverlay), this.overlayColor);
			if (this.OverlayText != "")
			{
				Shadow.DrawString(this.OverlayText, Global.StoreFontBig, new Vector2(position.X, position.Y + 32f), 2, Color.White, spriteBatch);
			}
			if (this.UpgradeTokens)
			{
				Rectangle destinationRectangle = new Rectangle(0, (int)position.Y + 1, 16, 16);
				for (int i = 0; i < 3; i++)
				{
					destinationRectangle.X = i * 16 + (int)position.X;
					spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(PCItem.GrayToken), Color.White);
				}
				for (int j = 0; j < this.Level; j++)
				{
					destinationRectangle.X = j * 16 + (int)position.X;
					spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(PCItem.Token), Color.White);
				}
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x000268C5 File Offset: 0x00024AC5
		public Rectangle CollisionRectangle
		{
			get
			{
				if (!this.Selectable || !this.Enabled)
				{
					return PCItem.NoRect;
				}
				return this.itemDest;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x000268E3 File Offset: 0x00024AE3
		public Rectangle ItemTex
		{
			get
			{
				return this.itemTex;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x000268EB File Offset: 0x00024AEB
		public Rectangle ItemNextLevelTex
		{
			get
			{
				return this.itemNextLevelTex;
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000268F3 File Offset: 0x00024AF3
		public static int RoundCost(float cost)
		{
			return (int)cost;
		}

		// Token: 0x0400050E RID: 1294
		private Rectangle itemTex;

		// Token: 0x0400050F RID: 1295
		private Rectangle itemDest;

		// Token: 0x04000510 RID: 1296
		private Rectangle itemNextLevelTex;

		// Token: 0x04000511 RID: 1297
		private Rectangle itemAmmoIcon;

		// Token: 0x04000512 RID: 1298
		private Rectangle itemAmmoIconDest;

		// Token: 0x04000513 RID: 1299
		public static int Size = 64;

		// Token: 0x04000514 RID: 1300
		public static Texture2D Background;

		// Token: 0x04000515 RID: 1301
		public static Texture2D SelectedBackground;

		// Token: 0x04000516 RID: 1302
		public string Title;

		// Token: 0x04000517 RID: 1303
		public string Description;

		// Token: 0x04000518 RID: 1304
		public string OverlayText = "";

		// Token: 0x04000519 RID: 1305
		public bool UpgradeTokens;

		// Token: 0x0400051A RID: 1306
		public int Level;

		// Token: 0x0400051B RID: 1307
		public bool Selected;

		// Token: 0x0400051C RID: 1308
		public object Tag;

		// Token: 0x0400051D RID: 1309
		public int Cost;

		// Token: 0x0400051E RID: 1310
		private Rectangle itemOverlay;

		// Token: 0x0400051F RID: 1311
		private Color overlayColor = Color.White;

		// Token: 0x04000520 RID: 1312
		public bool Locked;

		// Token: 0x04000521 RID: 1313
		public int PointsToUnlock;

		// Token: 0x04000522 RID: 1314
		private static Rectangle GrayToken = new Rectangle(112, 592, 16, 16);

		// Token: 0x04000523 RID: 1315
		private static Rectangle Token = new Rectangle(96, 592, 16, 16);

		// Token: 0x04000524 RID: 1316
		private bool Selectable = true;

		// Token: 0x04000525 RID: 1317
		public bool Enabled = true;

		// Token: 0x04000526 RID: 1318
		private static Rectangle NoRect = new Rectangle(0, 0, 0, 0);
	}
}
