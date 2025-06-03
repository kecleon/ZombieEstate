using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000063 RID: 99
	public class PlayerHUD
	{
		// Token: 0x06000226 RID: 550 RVA: 0x0001009C File Offset: 0x0000E29C
		public PlayerHUD(Player player, Vector2 pos)
		{
			this.parent = player;
			this.Position = pos;
			this.HUD = Global.Content.Load<Texture2D>("HUD2");
			this.HUDRect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.HUD.Width * 2, this.HUD.Height * 2);
			this.pixel = Global.Content.Load<Texture2D>("Pixel");
			this.HUDAbility = Global.Content.Load<Texture2D>("HUD_Ability");
			this.HUD_Left = Global.Content.Load<Texture2D>("HUD\\HUD_Left");
			this.HUD_Right = Global.Content.Load<Texture2D>("HUD\\HUD_Right");
			this.HealthBG = Global.Content.Load<Texture2D>("HUD\\HUD_Health_BG");
			this.HealthFG = Global.Content.Load<Texture2D>("HUD\\HUD_Health_FG");
			PlayerHUD.HUD_Left_Pos = new Vector2(0f, (float)(Global.ScreenRect.Height - this.HUD_Left.Height));
			PlayerHUD.HUD_Right_Pos = new Vector2((float)(Global.ScreenRect.Width - this.HUD_Right.Width), (float)(Global.ScreenRect.Height - this.HUD_Right.Height));
			this.HUD_HealthPos = new Vector2(4f, (float)(Global.ScreenRect.Height - 4 - this.HealthBG.Height));
			int num = (int)PlayerHUD.HUD_Left_Pos.X;
			int num2 = (int)PlayerHUD.HUD_Left_Pos.Y;
			this.bgClipBar = Color.Lerp(this.parent.HUDColor, Color.Black, 0.5f);
			this.AmmoBarFull = new Rectangle(num + 239, num2 + 123, 172, 30);
			this.HealthPackRect = new Rectangle(num + 35, num2 + 96, 32, 32);
			this.HealthPackSrc = Global.GetTexRectange(0, 42);
			int num3 = 198;
			this.AssaultMeter = new AmmoMeter(new Point((int)PlayerHUD.HUD_Left_Pos.X + num3, (int)PlayerHUD.HUD_Left_Pos.Y + 78), this.parent.HUDColor, new Point(0, 43));
			this.HeavyMeter = new AmmoMeter(new Point((int)PlayerHUD.HUD_Left_Pos.X + num3, (int)PlayerHUD.HUD_Left_Pos.Y + 78 + 38), this.parent.HUDColor, new Point(0, 44));
			this.ShellsMeter = new AmmoMeter(new Point((int)PlayerHUD.HUD_Left_Pos.X + num3, (int)PlayerHUD.HUD_Left_Pos.Y + 78 + 76), this.parent.HUDColor, new Point(0, 45));
			this.ExplosiveMeter = new AmmoMeter(new Point((int)PlayerHUD.HUD_Left_Pos.X + num3, (int)PlayerHUD.HUD_Left_Pos.Y + 78 + 114), this.parent.HUDColor, new Point(0, 46));
			this.SpecialMeter = new AmmoMeter(new Point((int)PlayerHUD.HUD_Left_Pos.X + num3, (int)PlayerHUD.HUD_Left_Pos.Y + 78 + 152), this.parent.HUDColor, new Point(0, 47));
			this.CurrWepDest = new Rectangle(Global.ScreenRect.Width / 2 - 32, Global.ScreenRect.Height / 2 + 64, 64, 64);
			if (PlayerHUD.levelUp == null)
			{
				PlayerHUD.levelUp = Global.Content.Load<Texture2D>("LevelUp");
			}
			this.currentMessage.priority = 4;
			this.currentMessage.message = "NONE";
			this.hudMessageTimer.DeltaDelegate = new Timer.TimerDelegate(this.MessageTimer);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void UpdateHUD()
		{
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000104D8 File Offset: 0x0000E6D8
		public void DrawHUD(SpriteBatch spriteBatch)
		{
			if (DialogMaster.DialogsActive)
			{
				return;
			}
			spriteBatch.Draw(this.HUD_Left, PlayerHUD.HUD_Left_Pos, Color.White);
			spriteBatch.Draw(this.HUD_Right, PlayerHUD.HUD_Right_Pos, Color.White);
			this.DrawHealthMeter(spriteBatch);
			this.DrawMeters(spriteBatch);
			this.DrawClipBar(spriteBatch);
			this.DrawWep(spriteBatch);
			this.DrawMessage(spriteBatch);
			Vector2 vector = new Vector2(PlayerHUD.HUD_Left_Pos.X + 321f, PlayerHUD.HUD_Left_Pos.Y + 104f);
			vector = VerchickMath.CenterText(Global.BloodGutterSmall, vector, "$" + this.parent.Stats.GetMoney().ToString());
			Shadow.DrawString("$" + this.parent.Stats.GetMoney().ToString(), Global.BloodGutterSmall, vector, 2, Color.LightGreen, spriteBatch);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000105C8 File Offset: 0x0000E7C8
		private void DrawHealthMeter(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.HealthBG, this.HUD_HealthPos, Color.White);
			Rectangle destinationRectangle = new Rectangle(18 + (int)this.HUD_HealthPos.X, 19 + (int)this.HUD_HealthPos.Y, 100, 100);
			int num = (int)(this.parent.Health / this.parent.SpecialProperties.MaxHealth * 100f);
			destinationRectangle.Height = num;
			destinationRectangle.Y += 100 - num;
			spriteBatch.Draw(Global.Pixel, destinationRectangle, Color.Red);
			Vector2 vector = new Vector2(this.HUD_HealthPos.X + (float)(this.HealthBG.Width / 2), this.HUD_HealthPos.Y + (float)(this.HealthBG.Height / 2) + 4f);
			vector = VerchickMath.CenterText(Global.BloodGutterLarge, vector, this.parent.Health.ToString());
			Shadow.DrawString(this.parent.Health.ToString(), Global.BloodGutterLarge, vector, 1, Color.White, spriteBatch);
			spriteBatch.Draw(this.HealthFG, this.HUD_HealthPos, Color.White);
			spriteBatch.Draw(Global.MasterTexture, this.HealthPackRect, new Rectangle?(this.HealthPackSrc), Color.White);
			vector = new Vector2((float)(this.HealthPackRect.X + 34), (float)this.HealthPackRect.Y);
			Shadow.DrawString("x " + this.parent.Stats.HealthPacks.ToString(), Global.StoreFontBig, vector, 1, Color.White, spriteBatch);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00010770 File Offset: 0x0000E970
		private void DrawMeters(SpriteBatch spriteBatch)
		{
			this.AssaultMeter.DrawMeter(spriteBatch, this.parent.Stats.GetAmmo(AmmoType.ASSAULT), this.parent.Stats.GetMaxAmmo(AmmoType.ASSAULT), this.parent.mGun.ammoType == AmmoType.ASSAULT);
			this.HeavyMeter.DrawMeter(spriteBatch, this.parent.Stats.GetAmmo(AmmoType.HEAVY), this.parent.Stats.GetMaxAmmo(AmmoType.HEAVY), this.parent.mGun.ammoType == AmmoType.HEAVY);
			this.ShellsMeter.DrawMeter(spriteBatch, this.parent.Stats.GetAmmo(AmmoType.SHELLS), this.parent.Stats.GetMaxAmmo(AmmoType.SHELLS), this.parent.mGun.ammoType == AmmoType.SHELLS);
			this.ExplosiveMeter.DrawMeter(spriteBatch, this.parent.Stats.GetAmmo(AmmoType.EXPLOSIVE), this.parent.Stats.GetMaxAmmo(AmmoType.EXPLOSIVE), this.parent.mGun.ammoType == AmmoType.EXPLOSIVE);
			this.SpecialMeter.DrawMeter(spriteBatch, this.parent.Stats.GetAmmo(AmmoType.SPECIAL), this.parent.Stats.GetMaxAmmo(AmmoType.SPECIAL), this.parent.mGun.ammoType == AmmoType.SPECIAL);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000108C4 File Offset: 0x0000EAC4
		private void DrawStats(SpriteBatch spriteBatch)
		{
			SpriteFont font = Global.Font;
			Rectangle destinationRectangle = new Rectangle(this.HUDRect.X + 6, this.HUDRect.Y + 18 - 2, 16, 16);
			Rectangle texRectange = Global.GetTexRectange(0, 42);
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(texRectange), Color.White);
			destinationRectangle.Y = this.HUDRect.Y + 54;
			texRectange = Global.GetTexRectange(6, 37);
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(texRectange), Color.White);
			destinationRectangle.Y = this.HUDRect.Y + 90;
			texRectange = Global.GetTexRectange(8, 37);
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(texRectange), Color.White);
			Vector2 pos = VerchickMath.CenterText(font, new Vector2((float)(this.HUDRect.X + 40), (float)(this.HUDRect.Y + 26)), "0");
			Shadow.DrawString(this.parent.Stats.HealthPacks.ToString(), font, pos, 1, Color.White, spriteBatch);
			Vector2 pos2 = VerchickMath.CenterText(font, new Vector2((float)(this.HUDRect.X + 40), (float)(this.HUDRect.Y + 62)), this.parent.Stats.UpgradeTokens.ToString());
			Shadow.DrawString(this.parent.Stats.UpgradeTokens.ToString(), font, pos2, 1, Color.LightYellow, spriteBatch);
			Vector2 pos3 = VerchickMath.CenterText(font, new Vector2((float)(this.HUDRect.X + 40), (float)(this.HUDRect.Y + 98)), this.parent.Stats.GetTalentPoints().ToString());
			Shadow.DrawString(this.parent.Stats.GetTalentPoints().ToString(), font, pos3, 1, Color.Pink, spriteBatch);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00010AB8 File Offset: 0x0000ECB8
		private void DrawMessage(SpriteBatch spriteBatch)
		{
			if (this.currentMessage.message == "NONE" || this.hudMessageTimer.Ready())
			{
				return;
			}
			float num = 1.5f * (1f - this.hudMessageTimer.Delta());
			num = Math.Min(1f, num);
			Vector2 vector = new Vector2((float)(Global.ScreenRect.Width / 2), 64f);
			vector = VerchickMath.CenterText(Global.StoreFontXtraLarge, vector, this.currentMessage.message);
			Color.Black.A = (byte)num;
			Shadow.DrawString(this.currentMessage.message, Global.StoreFontXtraLarge, vector, 3, this.currentMessage.color * num, Color.Black * num, spriteBatch);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00010B84 File Offset: 0x0000ED84
		private void DrawAbilityMeter(SpriteBatch spriteBatch)
		{
			int num = 0;
			num += this.HUDRect.Height - 20;
			Rectangle rectangle = new Rectangle(this.HUDRect.X, this.HUDRect.Y + num, 256, 64);
			spriteBatch.Draw(this.HUDAbility, new Rectangle(rectangle.X + 2, rectangle.Y + 2, 256, 64), new Color(40, 40, 40, 100));
			spriteBatch.Draw(this.HUDAbility, rectangle, this.parent.HUDColor);
			int num2 = (int)(this.parent.Ability.AbilityPercent * 104f);
			Rectangle rectangle2 = new Rectangle(76 + rectangle.X, 18 + rectangle.Y, num2, 28);
			if (num2 > 2)
			{
				spriteBatch.Draw(Global.Pixel, rectangle2, Color.LightGray);
			}
			int num3 = (int)(1f / this.parent.Ability.PercentPerActivation);
			if (num3 > 1)
			{
				for (int i = 0; i < num3; i++)
				{
					int num4 = (int)(this.parent.Ability.PercentPerActivation * 52f * 2f) * i;
					if (this.parent.Ability.PercentPerActivation * (float)i > this.parent.Ability.AbilityPercent)
					{
						spriteBatch.Draw(Global.Pixel, new Rectangle(rectangle2.X + num4, rectangle2.Y, 2, rectangle2.Height), Color.Black);
					}
					else
					{
						spriteBatch.Draw(Global.Pixel, new Rectangle(rectangle2.X + num4, rectangle2.Y, 2, rectangle2.Height), Color.DarkGray);
					}
				}
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00010D38 File Offset: 0x0000EF38
		private void DrawWep(SpriteBatch spriteBatch)
		{
			if (this.hudWepTimer.Running())
			{
				float num = 1f - this.hudWepTimer.Delta();
				if (num > 0.5f)
				{
					num = 1f;
				}
				else
				{
					num /= 0.5f;
				}
				spriteBatch.Draw(Global.MasterTexture, this.CurrWepDest, new Rectangle?(this.WepBG), this.parent.HUDColor * num);
				spriteBatch.Draw(Global.MasterTexture, this.CurrWepDest, new Rectangle?(this.CurrWep), Color.White * num);
				Vector2 vector = new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2 + 138));
				vector = VerchickMath.CenterText(Global.StoreFontBig, vector, this.WepName);
				Shadow.DrawString(this.WepName, Global.StoreFontBig, vector, 2, Color.White * num, Color.Black * num, spriteBatch);
				if (this.parent.mGun.ammoType != AmmoType.INFINITE && this.parent.Stats.GetAmmo(this.parent.mGun.ammoType) <= 0 && this.parent.mGun.bulletsInClip <= 0)
				{
					vector = new Vector2((float)(Global.ScreenRect.Width / 2), (float)(Global.ScreenRect.Height / 2 + 54));
					vector = VerchickMath.CenterText(Global.StoreFontBig, vector, "Out of Ammo!");
					Shadow.DrawString("Out of Ammo!", Global.StoreFontBig, vector, 2, Color.White * num, Color.Red * num, spriteBatch);
				}
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00010EE0 File Offset: 0x0000F0E0
		private void DrawClipBar(SpriteBatch spriteBatch)
		{
			Rectangle destinationRectangle = new Rectangle(this.AmmoBarFull.X - 2, this.AmmoBarFull.Y - 2, this.AmmoBarFull.Width + 4, this.AmmoBarFull.Height + 4);
			spriteBatch.Draw(this.pixel, destinationRectangle, this.bgClipBar);
			spriteBatch.Draw(this.pixel, this.AmmoBarFull, new Color(22, 22, 22));
			int width = (int)((float)this.AmmoBarFull.Width * this.parent.DeltaClip());
			Rectangle destinationRectangle2 = new Rectangle(this.AmmoBarFull.X, this.AmmoBarFull.Y, width, this.AmmoBarFull.Height);
			Color color = new Color(173, 168, 103);
			spriteBatch.Draw(this.pixel, destinationRectangle2, color);
			string text = this.parent.mGun.bulletsInClip.ToString();
			if (this.parent.mGun.Reloading)
			{
				text = "Reloading...";
			}
			Vector2 vector = new Vector2((float)(this.AmmoBarFull.X + this.AmmoBarFull.Width / 2), (float)(this.AmmoBarFull.Y + this.AmmoBarFull.Height / 2));
			vector = VerchickMath.CenterText(Global.BloodGutterSmall, vector, text);
			Shadow.DrawString(text, Global.BloodGutterSmall, vector, 2, Color.White, spriteBatch);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0001104C File Offset: 0x0000F24C
		public void LevelUp()
		{
			this.SendMessage("Level up!", 1, this.parent.HUDColor, 3f);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0001106C File Offset: 0x0000F26C
		public void SendMessage(string message, int priority, Color color, float time)
		{
			if (this.currentMessage.priority >= priority)
			{
				this.hudMessageTimer.mTotalTime = time;
				this.hudMessageTimer.Reset();
				this.hudMessageTimer.Start();
				this.currentMessage.message = message;
				this.currentMessage.color = color;
				this.currentMessage.priority = priority;
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000110CE File Offset: 0x0000F2CE
		private void MessageTimer(float delta)
		{
			if (delta >= 1f)
			{
				this.currentMessage.priority = 4;
				this.currentMessage.message = "NONE";
				this.hudMessageTimer.Stop();
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000110FF File Offset: 0x0000F2FF
		public void SwitchWep(int texX, int texY, string name)
		{
			this.CurrWep.X = texX * 16;
			this.CurrWep.Y = texY * 16;
			this.hudWepTimer.Reset();
			this.hudWepTimer.Start();
			this.WepName = name;
		}

		// Token: 0x040001FE RID: 510
		public Vector2 Position = new Vector2(100f, 464f);

		// Token: 0x040001FF RID: 511
		private Player parent;

		// Token: 0x04000200 RID: 512
		private Texture2D HUD;

		// Token: 0x04000201 RID: 513
		private Texture2D HUD_Left;

		// Token: 0x04000202 RID: 514
		private Texture2D HUD_Right;

		// Token: 0x04000203 RID: 515
		public static Vector2 HUD_Left_Pos;

		// Token: 0x04000204 RID: 516
		public static Vector2 HUD_Right_Pos;

		// Token: 0x04000205 RID: 517
		private Vector2 HUD_HealthPos;

		// Token: 0x04000206 RID: 518
		private Rectangle HUDRect;

		// Token: 0x04000207 RID: 519
		private Texture2D pixel;

		// Token: 0x04000208 RID: 520
		private Texture2D HUDAbility;

		// Token: 0x04000209 RID: 521
		private Texture2D HealthBG;

		// Token: 0x0400020A RID: 522
		private Texture2D HealthFG;

		// Token: 0x0400020B RID: 523
		private Rectangle AmmoBarFull;

		// Token: 0x0400020C RID: 524
		private AmmoMeter AssaultMeter;

		// Token: 0x0400020D RID: 525
		private AmmoMeter HeavyMeter;

		// Token: 0x0400020E RID: 526
		private AmmoMeter ShellsMeter;

		// Token: 0x0400020F RID: 527
		private AmmoMeter ExplosiveMeter;

		// Token: 0x04000210 RID: 528
		private AmmoMeter SpecialMeter;

		// Token: 0x04000211 RID: 529
		private Rectangle HealthPackRect;

		// Token: 0x04000212 RID: 530
		private Rectangle HealthPackSrc;

		// Token: 0x04000213 RID: 531
		private Rectangle CurrWep = Global.GetTexRectange(63, 63);

		// Token: 0x04000214 RID: 532
		private Rectangle CurrWepDest;

		// Token: 0x04000215 RID: 533
		private Rectangle WepBG = Global.GetTexRectange(0, 37);

		// Token: 0x04000216 RID: 534
		private string WepName = "";

		// Token: 0x04000217 RID: 535
		private HUDMessage currentMessage;

		// Token: 0x04000218 RID: 536
		private Timer hudMessageTimer = new Timer(1f);

		// Token: 0x04000219 RID: 537
		private Timer levelUpTimer = new Timer(3.5f);

		// Token: 0x0400021A RID: 538
		private Timer hudWepTimer = new Timer(1f);

		// Token: 0x0400021B RID: 539
		private static Texture2D levelUp;

		// Token: 0x0400021C RID: 540
		private Vector2 TESTPOS = new Vector2(0f, 320f);

		// Token: 0x0400021D RID: 541
		private Color bgClipBar;
	}
}
