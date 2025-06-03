using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000B6 RID: 182
	internal class CharWeapon : StoreElement
	{
		// Token: 0x0600049D RID: 1181 RVA: 0x000220A0 File Offset: 0x000202A0
		public CharWeapon(int xPos, int yPos, int size)
		{
			this.itemDest = new Rectangle(xPos, yPos, size, size);
			this.bgDest = new Rectangle(xPos - 1, yPos - 1, size + 1, size + 1);
			this.itemRect = Global.GetTexRectange(63, 63);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000220F4 File Offset: 0x000202F4
		public override void ItemSelected(PCItem item)
		{
			if (item.Tag == null)
			{
				return;
			}
			CharacterSettings characterSettings = (CharacterSettings)item.Tag;
			GunStats stats = GunStatsLoader.GetStats(characterSettings.startingGun);
			this.name = characterSettings.startingGun;
			this.itemRect.X = stats.GetOrigin(0).X * 16;
			this.itemRect.Y = stats.GetOrigin(0).Y * 16 + 16;
			base.ItemSelected(item);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0002216C File Offset: 0x0002036C
		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2((float)(this.bgDest.X + 32), (float)(this.bgDest.Y - 48));
			vector = VerchickMath.CenterText(Global.StoreFont, vector, "Starting Weapon");
			Shadow.DrawString("Starting Weapon", Global.StoreFont, vector, 1, Color.White, spriteBatch);
			vector = new Vector2((float)(this.bgDest.X + 32), (float)(this.bgDest.Y - 16));
			vector = VerchickMath.CenterText(Global.StoreFont, vector, this.name);
			Shadow.DrawString(this.name, Global.StoreFont, vector, 1, Color.LightYellow, spriteBatch);
			spriteBatch.Draw(PCItem.Background, this.bgDest, Color.White);
			spriteBatch.Draw(Global.MasterTexture, this.itemDest, new Rectangle?(this.itemRect), Color.White);
			base.Draw(spriteBatch);
		}

		// Token: 0x04000497 RID: 1175
		private Rectangle itemRect;

		// Token: 0x04000498 RID: 1176
		private Rectangle itemDest;

		// Token: 0x04000499 RID: 1177
		private Rectangle bgDest;

		// Token: 0x0400049A RID: 1178
		private bool Upgrade;

		// Token: 0x0400049B RID: 1179
		private string name = "";
	}
}
