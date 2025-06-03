using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.UI;

namespace ZombieEstate2.HUD.XboxHUD
{
	// Token: 0x020001CC RID: 460
	public class XboxStatsHUD
	{
		// Token: 0x06000C60 RID: 3168 RVA: 0x000658E0 File Offset: 0x00063AE0
		public XboxStatsHUD(Player player, Vector2 topLeft, bool forceTopLeft = false)
		{
			XboxStatsHUD.LoadGfx();
			this.mPlayer = player;
			this.mTopLeft = topLeft;
			if (!forceTopLeft)
			{
				Rectangle safeScreenArea = Global.GetSafeScreenArea();
				if (this.mPlayer.Index == 0)
				{
					this.mTopLeft.X = (float)safeScreenArea.X;
					this.mTopLeft.Y = (float)safeScreenArea.Y;
				}
				if (this.mPlayer.Index == 1)
				{
					this.mTopLeft.X = (float)(safeScreenArea.Right - XboxStatsHUD.mTexBG.Width);
					this.mTopLeft.Y = (float)safeScreenArea.Y;
				}
				if (this.mPlayer.Index == 2)
				{
					this.mTopLeft.X = (float)safeScreenArea.X;
					this.mTopLeft.Y = (float)(safeScreenArea.Bottom - XboxStatsHUD.mTexBG.Height);
				}
				if (this.mPlayer.Index == 3)
				{
					this.mTopLeft.X = (float)(safeScreenArea.Right - XboxStatsHUD.mTexBG.Width);
					this.mTopLeft.Y = (float)(safeScreenArea.Bottom - XboxStatsHUD.mTexBG.Height);
				}
			}
			this.mTextPos = new Vector2(this.mTopLeft.X + 233f, this.mTopLeft.Y + 270f);
			this.mPositions = new Vector2[4, 6];
			this.mStats = new XboxStatsDisplay[4, 6];
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					this.mPositions[i, j] = new Vector2(this.mTopLeft.X + (float)(i * 59 * this.scale) + (float)(18 * this.scale), this.mTopLeft.Y + (float)(j * 21 * this.scale) + (float)(2 * this.scale) + 1f);
					this.mStats[i, j] = new XboxStatsDisplay(this.mPositions[i, j]);
				}
			}
			this.mBounds = new Rectangle((int)this.mTopLeft.X, (int)this.mTopLeft.Y, 236 * this.scale + 36, 126 * this.scale + 5);
			int x = (int)this.mTopLeft.X + (this.mBounds.Width - 120) / 2;
			this.mBackButton = new ZEButton(new Rectangle(x, this.mBounds.Bottom + 24, 120, 28), "Back", ButtonPress.Negative, this.mPlayer.Index);
			this.mBackButton.OnPressed += delegate(object e, EventArgs s)
			{
				this.CLOSED = true;
			};
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00065BB0 File Offset: 0x00063DB0
		public static void LoadGfx()
		{
			if (XboxStatsHUD.mTexBG == null)
			{
				XboxStatsHUD.mTexBG = Global.Content.Load<Texture2D>("XboxHUD//Stats");
				XboxStatsHUD.mTexHigh = Global.Content.Load<Texture2D>("XboxHUD//StatsHighlight");
				XboxStatsHUD.mTexIcons = Global.Content.Load<Texture2D>("XboxHUD//StatsIcon");
			}
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00065C00 File Offset: 0x00063E00
		public void Update()
		{
			this.UpdateStats();
			int num = this.mHighX;
			int num2 = this.mHighY;
			if (VerchickMath.IsMouseInRect(this.mBounds))
			{
				int num3 = InputManager.GetMousePosition().X - (int)this.mTopLeft.X;
				int num4 = InputManager.GetMousePosition().Y - (int)this.mTopLeft.Y;
				num3 /= 119;
				num4 /= 43;
				this.mHighX = num3;
				this.mHighY = num4;
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveWest, this.mPlayer.Index, false))
			{
				this.mHighX--;
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveEast, this.mPlayer.Index, false))
			{
				this.mHighX++;
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveNorth, this.mPlayer.Index, false))
			{
				this.mHighY--;
			}
			if (InputManager.ButtonPressed(ButtonPress.MoveSouth, this.mPlayer.Index, false))
			{
				this.mHighY++;
			}
			if (this.mHighX < 0 || this.mHighX > 3)
			{
				this.mHighX = num;
			}
			if (this.mHighY < 0 || this.mHighY > 5)
			{
				this.mHighY = num2;
			}
			this.mHighPos.X = (int)this.mPositions[this.mHighX, this.mHighY].X - 34;
			this.mHighPos.Y = (int)this.mPositions[this.mHighX, this.mHighY].Y - 3;
			this.mBackButton.Update();
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00065D94 File Offset: 0x00063F94
		private void UpdateStats()
		{
			this.mStats[0, 0].Update(this.mPlayer.SpecialProperties.MaxHealth, CharStatsComparer.DEF.MaxHealth, "Max Health", false);
			this.mStats[1, 0].Update(this.mPlayer.SpecialProperties.Speed, CharStatsComparer.DEF.Speed, "Movement Speed", false);
			this.mStats[2, 0].Update(this.mPlayer.SpecialProperties.HealingDoneMod, CharStatsComparer.DEF.HealingDoneMod, "Healing Bonus", true);
			this.mStats[3, 0].Update(this.mPlayer.SpecialProperties.CritChance, CharStatsComparer.DEF.CritChance, "Critical Strike Chance", true);
			this.mStats[0, 1].Update(this.mPlayer.SpecialProperties.ReloadTimeMod, CharStatsComparer.DEF.ReloadTimeMod, "Reload Speed", true);
			this.mStats[1, 1].Update(this.mPlayer.SpecialProperties.Armor, CharStatsComparer.DEF.Armor, "Armor", false);
			this.mStats[2, 1].Update(this.mPlayer.SpecialProperties.LifeStealPercent, CharStatsComparer.DEF.LifeStealPercent, "Life Steal Percent", true);
			this.mStats[3, 1].Update(this.mPlayer.SpecialProperties.ExplosionDamageMod, CharStatsComparer.DEF.ExplosionDamageMod, "Explosive Weapon Bonus", true);
			this.mStats[0, 2].Update(this.mPlayer.SpecialProperties.BulletDamageMod, CharStatsComparer.DEF.BulletDamageMod, "Bonus Gun (Physical) Damage", true);
			this.mStats[1, 2].Update(this.mPlayer.SpecialProperties.MeleeDamageMod, CharStatsComparer.DEF.MeleeDamageMod, "Bonus Melee Damage", true);
			this.mStats[2, 2].Update(this.mPlayer.SpecialProperties.MinionDmgMod, CharStatsComparer.DEF.MinionDmgMod, "Bonus Minion Damage", true);
			this.mStats[3, 2].Update(this.mPlayer.SpecialProperties.Ammo_Assault, CharStatsComparer.DEF.Ammo_Assault, "Bonus Assault Ammo", false);
			this.mStats[0, 3].Update(this.mPlayer.SpecialProperties.ShotTimeMod, CharStatsComparer.DEF.ShotTimeMod, "Bonus Gun Shot Speed", true);
			this.mStats[1, 3].Update(this.mPlayer.SpecialProperties.SwingTimeMod, CharStatsComparer.DEF.SwingTimeMod, "Bonus Melee Swing Speed", true);
			this.mStats[2, 3].Update(this.mPlayer.SpecialProperties.MinionFireRateMod, CharStatsComparer.DEF.MinionFireRateMod, "Bonus Minion Attack Speed", true);
			this.mStats[3, 3].Update(this.mPlayer.SpecialProperties.Ammo_Shells, CharStatsComparer.DEF.Ammo_Shells, "Bonus Shells Ammo", false);
			this.mStats[0, 4].Update(this.mPlayer.SpecialProperties.FireDmg, CharStatsComparer.DEF.FireDmg, "Bonus Fire Damage", true);
			this.mStats[1, 4].Update(this.mPlayer.SpecialProperties.EarthDmg, CharStatsComparer.DEF.EarthDmg, "Bonus Earth Damage", true);
			this.mStats[2, 4].Update(this.mPlayer.SpecialProperties.WaterDmg, CharStatsComparer.DEF.WaterDmg, "Bonus Water Damage", true);
			this.mStats[3, 4].Update(this.mPlayer.SpecialProperties.Ammo_Heavy, CharStatsComparer.DEF.Ammo_Heavy, "Bonus Heavy Ammo", false);
			this.mStats[0, 5].Update(this.mPlayer.SpecialProperties.FireResist, CharStatsComparer.DEF.FireResist, "Fire Resistance", false);
			this.mStats[1, 5].Update(this.mPlayer.SpecialProperties.EarthResist, CharStatsComparer.DEF.EarthResist, "Earth Resistance", false);
			this.mStats[2, 5].Update(this.mPlayer.SpecialProperties.WaterResist, CharStatsComparer.DEF.WaterResist, "Water Resistance", false);
			this.mStats[3, 5].Update(this.mPlayer.SpecialProperties.Ammo_Explosive, CharStatsComparer.DEF.Ammo_Explosive, "Bonus Explosive Ammo", false);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x00066344 File Offset: 0x00064544
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(XboxStatsHUD.mTexBG, this.mTopLeft, this.mPlayer.HUDColor);
			spriteBatch.Draw(XboxStatsHUD.mTexIcons, this.mTopLeft, Color.White);
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					this.mStats[i, j].Draw(spriteBatch);
				}
			}
			spriteBatch.Draw(XboxStatsHUD.mTexHigh, this.mHighPos, Color.White);
			Vector2 position = VerchickMath.CenterText(Global.StoreFontBig, this.mTextPos, this.mStats[this.mHighX, this.mHighY].ToolTip);
			Shadow.DrawOutlinedString(spriteBatch, Global.StoreFontBig, this.mStats[this.mHighX, this.mHighY].ToolTip, Color.Black, Color.White, 1f, 0f, position);
			this.mBackButton.Draw(spriteBatch);
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0006643C File Offset: 0x0006463C
		private void DrawStats(SpriteBatch spriteBatch)
		{
			SpriteFont equationFontSmall = Global.EquationFontSmall;
			Shadow.DrawString(this.mPlayer.SpecialProperties.MaxHealth.ToString(), equationFontSmall, this.mPositions[0, 0], 1, Color.White, spriteBatch);
			Shadow.DrawString(this.mPlayer.SpecialProperties.Speed.ToString(), equationFontSmall, this.mPositions[1, 0], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.HealingDoneMod.ToString(), equationFontSmall, this.mPositions[2, 0], 1, Color.White, spriteBatch);
			Shadow.DrawString("+" + this.mPlayer.SpecialProperties.Ammo_Assault.ToString(), equationFontSmall, this.mPositions[3, 0], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.ReloadTimeMod.ToString(), equationFontSmall, this.mPositions[0, 1], 1, Color.White, spriteBatch);
			Shadow.DrawString(this.mPlayer.SpecialProperties.Armor.ToString(), equationFontSmall, this.mPositions[1, 1], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.LifeStealPercent.ToString(), equationFontSmall, this.mPositions[2, 1], 1, Color.White, spriteBatch);
			Shadow.DrawString("+" + this.mPlayer.SpecialProperties.Ammo_Shells.ToString(), equationFontSmall, this.mPositions[3, 1], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.BulletDamageMod.ToString(), equationFontSmall, this.mPositions[0, 2], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.MeleeDamageMod.ToString(), equationFontSmall, this.mPositions[1, 2], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.MinionDmgMod.ToString(), equationFontSmall, this.mPositions[2, 2], 1, Color.White, spriteBatch);
			Shadow.DrawString("+" + this.mPlayer.SpecialProperties.Ammo_Heavy.ToString(), equationFontSmall, this.mPositions[3, 2], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.ShotTimeMod.ToString(), equationFontSmall, this.mPositions[0, 3], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.SwingTimeMod.ToString(), equationFontSmall, this.mPositions[1, 3], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.MinionFireRateMod.ToString(), equationFontSmall, this.mPositions[2, 3], 1, Color.White, spriteBatch);
			Shadow.DrawString("+" + this.mPlayer.SpecialProperties.Ammo_Explosive.ToString(), equationFontSmall, this.mPositions[3, 3], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.FireDmg.ToString(), equationFontSmall, this.mPositions[0, 4], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.WaterDmg.ToString(), equationFontSmall, this.mPositions[1, 4], 1, Color.White, spriteBatch);
			Shadow.DrawString("%" + this.mPlayer.SpecialProperties.EarthDmg.ToString(), equationFontSmall, this.mPositions[2, 4], 1, Color.White, spriteBatch);
			Shadow.DrawString(this.mPlayer.SpecialProperties.FireResist.ToString(), equationFontSmall, this.mPositions[0, 5], 1, Color.White, spriteBatch);
			Shadow.DrawString(this.mPlayer.SpecialProperties.WaterResist.ToString(), equationFontSmall, this.mPositions[1, 5], 1, Color.White, spriteBatch);
			Shadow.DrawString(this.mPlayer.SpecialProperties.EarthResist.ToString(), equationFontSmall, this.mPositions[2, 5], 1, Color.White, spriteBatch);
		}

		// Token: 0x04000D10 RID: 3344
		private Vector2[,] mPositions;

		// Token: 0x04000D11 RID: 3345
		private Vector2 mTopLeft;

		// Token: 0x04000D12 RID: 3346
		private static Texture2D mTexBG;

		// Token: 0x04000D13 RID: 3347
		private static Texture2D mTexHigh;

		// Token: 0x04000D14 RID: 3348
		private static Texture2D mTexIcons;

		// Token: 0x04000D15 RID: 3349
		private Player mPlayer;

		// Token: 0x04000D16 RID: 3350
		private int scale = 2;

		// Token: 0x04000D17 RID: 3351
		private XboxStatsDisplay[,] mStats;

		// Token: 0x04000D18 RID: 3352
		private Vector2 mTextPos;

		// Token: 0x04000D19 RID: 3353
		private int mHighX;

		// Token: 0x04000D1A RID: 3354
		private int mHighY;

		// Token: 0x04000D1B RID: 3355
		private Rectangle mHighPos = new Rectangle(0, 0, 116, 40);

		// Token: 0x04000D1C RID: 3356
		private Rectangle mBounds;

		// Token: 0x04000D1D RID: 3357
		private ZEButton mBackButton;

		// Token: 0x04000D1E RID: 3358
		public bool CLOSED;
	}
}
