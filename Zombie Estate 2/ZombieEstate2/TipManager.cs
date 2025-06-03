using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000A0 RID: 160
	public static class TipManager
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x0001E84C File Offset: 0x0001CA4C
		public static void Init()
		{
			if (TipManager.mTips == null)
			{
				TipManager.BuildList();
			}
			int index = Global.rand.Next(0, TipManager.mKeys.Count);
			string text = TipManager.mKeys[index];
			TipManager.mCurrent = new ScrollBox(text, new Rectangle((int)Global.GetScreenCenter().X - 240, Global.ScreenRect.Height - 200, 600, 200), Global.EquationFontSmall, null, Color.White);
			TipManager.mTex = TipManager.mTips[text];
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001E8E0 File Offset: 0x0001CAE0
		private static void BuildList()
		{
			TipManager.mTips = new Dictionary<string, Point>();
			TipManager.mTips.Add("Doctor Zombies heal others around them. Focus them first!", new Point(68, 12));
			TipManager.mTips.Add("Hazmat Zombies will explode when they get close to you, dealing Earth damage.", new Point(56, 12));
			TipManager.mTips.Add("Fire Witches deal powerful fire damage! Dispatch them quickly or you may get overwhelmed.", new Point(56, 24));
			TipManager.mTips.Add("Armor reduces the amount of physical damage your character receives.", new Point(69, 43));
			TipManager.mTips.Add("Water, Earth, and Fire resists reduce incoming elemental damage.", new Point(68, 47));
			TipManager.mTips.Add("Goliaths are enormous, powerful zombies. Use high damage weapons to kill them such as sniper rifles or explosives.", new Point(56, 18));
			TipManager.mTips.Add("Some characters and guns have a Critical Hit Chance. Critical hits do double damage. Explosives can not crit.", new Point(71, 42));
			TipManager.mTips.Add("Minion Damage bonuses mean all minions under your control deal more damage.", new Point(70, 44));
			TipManager.mTips.Add("Tanky characters should use melee weapons with high armor to get in the fray and draw zombies' attentions away from more vulnerable allies.", new Point(68, 42));
			TipManager.mTips.Add("Some characters excel at using certain elemental types. Buy weapons that make use of these bonuses.", new Point(70, 46));
			TipManager.mTips.Add("Some characters are better at melee than ranged weapons.", new Point(69, 44));
			TipManager.mTips.Add("Characters with bonus to healing should purchase healing weapons to help keep their team alive.", new Point(70, 42));
			TipManager.mTips.Add("Using a weapon with Life Steal, some characters can stay alive by dealing massive damage.", new Point(70, 43));
			TipManager.mTips.Add("Ice Golems can slow down your team. Stay at a distance or risk being stuck in a mob of zombies.", new Point(56, 30));
			TipManager.mTips.Add("Characters with high minion damage and speed bonuses should make use of minions while their team mates handle other guns.", new Point(21, 37));
			TipManager.mTips.Add("You can have up to 2 minions out at once. Dropping a third will replace the least healthiest minion.", new Point(20, 45));
			TipManager.mTips.Add("Non-stationary minions can be healed! Keep them alive to get their ammo-cost worth before they die.", new Point(16, 53));
			TipManager.mTips.Add("Guns can be updated 3 times using Upgrade Tokens. Check the stats on the inventory screen to see what they upgrade at a glance.", new Point(6, 37));
			TipManager.mTips.Add("Earning Badges when there are more than one player will grant Zombie Point bonuses.", new Point(66, 46));
			TipManager.mTips.Add("Having a damage dealer, a tank, a healer, and a minion expert will spread out Badges earned giving each player even Point bonuses.", new Point(66, 47));
			TipManager.mTips.Add("If you get low on health, use a health pack. (Default [F])", new Point(0, 42));
			TipManager.mTips.Add("Money drops are shared among all players! Cover your teammates while they grab them!", new Point(4, 44));
			TipManager.mTips.Add("Zombie points are earned at the end of each game. Use them to unlock new characters and hats!", new Point(0, 58));
			TipManager.mTips.Add("Visit the shop keep between each wave to purchase new weapons and access your inventory to upgrade or sell guns!", new Point(4, 4));
			TipManager.mTips.Add("Clowns fire confetti dealing physical damage. Unless you have high armor, keep your distance.", new Point(52, 0));
			TipManager.mTips.Add("On death, Gloopers drop sludge on the ground slowing players who walk over it.", new Point(60, 12));
			TipManager.mTips.Add("Every time you play, different zombie types will show up!", new Point(73, 0));
			TipManager.mTips.Add("Gardner zombies will spawn thorns dealing Earth damage and trapping those who are hit.", new Point(76, 13));
			TipManager.mTips.Add("RobBurglars will mind their own business, munching on their burgers... unless you make them mad.", new Point(68, 4));
			TipManager.mTips.Add("Brain zombies spawn mini brains when they are killed! Don't get swarmed!", new Point(60, 18));
			TipManager.mTips.Add("Blobs are weak but drop acid on the floor dealing Water damage to those who walk over it.", new Point(68, 24));
			TipManager.mTips.Add("You can view your character's stats mid game! (Default [Shift])", new Point(12, 1));
			TipManager.mTips.Add("Speed Boost Power Up increases your character's speed!", new Point(67, 41));
			TipManager.mTips.Add("Damage Boost Power Up increases all damage dealt by you and your minions!", new Point(67, 37));
			TipManager.mTips.Add("Crit Power Up makes every shot a critical strike!", new Point(67, 40));
			TipManager.mTips.Add("Guard Power Up greatly reduces damage dealt to you by all sources!", new Point(67, 38));
			TipManager.mKeys = new List<string>();
			foreach (string item in TipManager.mTips.Keys)
			{
				TipManager.mKeys.Add(item);
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001ECA8 File Offset: 0x0001CEA8
		public static void Draw(SpriteBatch spriteBatch)
		{
			Rectangle texRectange = Global.GetTexRectange(TipManager.mTex.X, TipManager.mTex.Y);
			Rectangle rectangle = new Rectangle((int)Global.GetScreenCenter().X - 348, Global.ScreenRect.Height - 200, 96, 96);
			Rectangle destinationRectangle = new Rectangle(rectangle.X - 2, rectangle.Y - 2, rectangle.Width + 4, rectangle.Height + 4);
			Rectangle destinationRectangle2 = new Rectangle(rectangle.X - 4, rectangle.Y - 4, rectangle.Width + 8, rectangle.Height + 8);
			spriteBatch.Draw(Global.Pixel, destinationRectangle2, Color.White);
			spriteBatch.Draw(Global.Pixel, destinationRectangle, new Color(0.2f, 0.2f, 0.2f));
			spriteBatch.Draw(Global.MasterTexture, rectangle, new Rectangle?(texRectange), Color.White);
			TipManager.mCurrent.Draw(spriteBatch);
		}

		// Token: 0x04000403 RID: 1027
		private static List<string> mKeys;

		// Token: 0x04000404 RID: 1028
		private static ScrollBox mCurrent;

		// Token: 0x04000405 RID: 1029
		private static Point mTex;

		// Token: 0x04000406 RID: 1030
		private static Dictionary<string, Point> mTips;
	}
}
