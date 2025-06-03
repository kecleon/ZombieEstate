using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000173 RID: 371
	internal class TalentPage
	{
		// Token: 0x06000B34 RID: 2868 RVA: 0x0005C90C File Offset: 0x0005AB0C
		public TalentPage(int scale, Vector2 topLeft, int index)
		{
			this.INDEX = index;
			this.TopLeft = topLeft;
			this.SCALE = scale;
			this.DefaultTrees();
			this.Load();
			this.MainPanel = new Rectangle((int)topLeft.X - 8 * this.SCALE, (int)topLeft.Y - 19 * this.SCALE, this.SCALE * 200, this.SCALE * 200);
			this.Border = new Rectangle(this.MainPanel.X - 2 * this.SCALE, this.MainPanel.Y - 2 * this.SCALE, this.MainPanel.Width + 4 * this.SCALE, this.MainPanel.Height + 4 * this.SCALE);
			this.SmallPanel = new Rectangle(this.MainPanel.X + this.MainPanel.Width - 2 * this.SCALE, this.MainPanel.Y + 16 * this.SCALE, 64 * this.SCALE, this.MainPanel.Height - 32 * this.SCALE);
			this.SmallBorder = new Rectangle(this.SmallPanel.X - 2 * this.SCALE, this.SmallPanel.Y - 2 * this.SCALE, this.SmallPanel.Width + 4 * this.SCALE, this.SmallPanel.Height + 4 * this.SCALE);
			this.SaveButton = new PCButton("Save", Color.LightBlue, new Vector2((float)(this.SmallPanel.X + this.SmallPanel.Width / 2 - PCButton.ButtonTex.Width / 2), (float)(this.SmallPanel.Y + 16 * this.SCALE)));
			this.ResetButton = new PCButton("Revert", Color.LightBlue, new Vector2((float)(this.SmallPanel.X + this.SmallPanel.Width / 2 - PCButton.ButtonTex.Width / 2), (float)(this.SmallPanel.Y + 38 * this.SCALE)));
			this.ExitButton = new PCButton("Back", Color.Gray, new Vector2((float)(this.SmallPanel.X + this.SmallPanel.Width / 2 - PCButton.ButtonTex.Width / 2), (float)(this.SmallPanel.Y + this.SmallPanel.Height - 32 * this.SCALE)));
			this.SaveButton.Pressed = new PCButton.PressedDelegate(this.Save);
			this.ResetButton.Pressed = new PCButton.PressedDelegate(this.Load);
			this.ExitButton.Pressed = new PCButton.PressedDelegate(this.Exit);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0005CBF8 File Offset: 0x0005ADF8
		private void DefaultTrees()
		{
			this.Trees = new List<TalentTree>();
			this.Trees.Add(new OffenseTree(new Point((int)this.TopLeft.X, (int)this.TopLeft.Y), this.SCALE));
			this.Trees.Add(new DefenseTree(new Point((int)this.TopLeft.X + this.SCALE * (TalentTree.TalentBG.Width + 4), (int)this.TopLeft.Y), this.SCALE));
			this.Trees.Add(new MedicTree(new Point((int)this.TopLeft.X + 2 * (this.SCALE * (TalentTree.TalentBG.Width + 4)), (int)this.TopLeft.Y), this.SCALE));
			this.Trees.Add(new MinionTree(new Point((int)this.TopLeft.X, (int)this.TopLeft.Y + this.SCALE * (TalentTree.TalentBG.Height + 4)), this.SCALE));
			this.Trees.Add(new GunSpecTree(new Point((int)this.TopLeft.X + this.SCALE * (TalentTree.TalentBG.Width + 4), (int)this.TopLeft.Y + this.SCALE * (TalentTree.TalentBG.Height + 4)), this.SCALE));
			this.Trees.Add(new UtilityTree(new Point((int)this.TopLeft.X + 2 * (this.SCALE * (TalentTree.TalentBG.Width + 4)), (int)this.TopLeft.Y + this.SCALE * (TalentTree.TalentBG.Height + 4)), this.SCALE));
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0005CDD4 File Offset: 0x0005AFD4
		public void Update()
		{
			int num = this.TotalPoints;
			foreach (TalentTree talentTree in this.Trees)
			{
				num -= talentTree.PointsSpent;
			}
			foreach (TalentTree talentTree2 in this.Trees)
			{
				talentTree2.Update(num);
			}
			if (this.SCALE >= 2)
			{
				this.SaveButton.Update();
				this.ResetButton.Update();
				this.ExitButton.Update();
			}
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0005CE9C File Offset: 0x0005B09C
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.SCALE >= 2)
			{
				spriteBatch.Draw(Global.Pixel, Global.ScreenRect, Color.Black);
			}
			spriteBatch.Draw(Global.Pixel, this.Border, Color.LightGray);
			spriteBatch.Draw(Global.Pixel, this.MainPanel, Color.Black);
			if (this.SCALE >= 2)
			{
				spriteBatch.Draw(Global.Pixel, this.SmallBorder, Color.LightGray);
				spriteBatch.Draw(Global.Pixel, this.SmallPanel, new Color(0.1f, 0.1f, 0.1f));
				this.SaveButton.Draw(spriteBatch);
				this.ResetButton.Draw(spriteBatch);
				this.ExitButton.Draw(spriteBatch);
				int num = this.TotalPoints;
				foreach (TalentTree talentTree in this.Trees)
				{
					num -= talentTree.PointsSpent;
				}
				Vector2 position = new Vector2((float)(this.SmallPanel.X + this.SmallPanel.Width / 2), (float)(this.SmallPanel.Y + this.SmallPanel.Height / 2));
				position = VerchickMath.CenterText(Global.StoreFontBig, position, "Points: " + num);
				spriteBatch.DrawString(Global.StoreFontBig, "Points: " + num, position, Color.White);
			}
			this.DrawNodes(spriteBatch);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0005D02C File Offset: 0x0005B22C
		private void DrawNodes(SpriteBatch spriteBatch)
		{
			TalentNode talentNode = null;
			foreach (TalentTree talentTree in this.Trees)
			{
				talentTree.Draw(spriteBatch);
				if (talentTree.HighlightedNode != null)
				{
					talentNode = talentTree.HighlightedNode;
				}
			}
			if (talentNode != null)
			{
				this.DrawToolTip(spriteBatch, talentNode);
			}
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0005D09C File Offset: 0x0005B29C
		private void DrawToolTip(SpriteBatch spriteBatch, TalentNode high)
		{
			int num = 320;
			int num2 = 360;
			int num3 = Math.Max(0, InputManager.GetMousePosition().Y + 32);
			int num4 = Math.Max(0, InputManager.GetMousePosition().X + 32);
			num3 = Math.Min(Global.ScreenRect.Height - num2 - 4, num3);
			num4 = Math.Min(Global.ScreenRect.Width - num - 4, num4);
			Rectangle destinationRectangle = new Rectangle(num4 - 2, num3 - 2, num + 4, num2 + 4);
			Rectangle destinationRectangle2 = new Rectangle(num4, num3, num, num2);
			spriteBatch.Draw(Global.Pixel, destinationRectangle, Color.White);
			spriteBatch.Draw(Global.Pixel, destinationRectangle2, Color.Black);
			string name = high.Name;
			string text = high.Descriptions[Math.Max(0, high.Level - 1)];
			Vector2 vector = new Vector2((float)(num4 + num / 2), (float)(num3 + 20));
			vector = VerchickMath.CenterText(Global.StoreFontBig, vector, name);
			Shadow.DrawString(name, Global.StoreFontBig, vector, 1, Color.White, spriteBatch);
			Vector2 vector2 = new Vector2((float)(num4 + 2), (float)(num3 + 40));
			text = VerchickMath.WordWrapWidth(text, num, Global.StoreFont);
			if (high.Empty)
			{
				Shadow.DrawString(text, Global.StoreFont, vector2, 1, Color.Gray, spriteBatch);
			}
			else
			{
				Shadow.DrawString(text, Global.StoreFont, vector2, 1, Color.LightGray, spriteBatch);
			}
			Vector2 pos = new Vector2(vector2.X, vector2.Y + 140f);
			if (!high.Filled && !high.Empty)
			{
				Shadow.DrawString("Next rank:", Global.StoreFont, pos, 1, Color.Yellow, spriteBatch);
				pos.Y += 30f;
				text = high.Descriptions[high.Level];
				text = VerchickMath.WordWrapWidth(text, num, Global.StoreFont);
				Shadow.DrawString(text, Global.StoreFont, pos, 1, Color.Gray, spriteBatch);
				return;
			}
			if (high.State == TalentNode.TalentState.NotEnoughPoints)
			{
				Shadow.DrawString(VerchickMath.WordWrapWidth("Need more points in tree.", num, Global.StoreFont), Global.StoreFont, pos, 1, Color.Red, spriteBatch);
				return;
			}
			if (high.State == TalentNode.TalentState.DependantMissing)
			{
				Shadow.DrawString(VerchickMath.WordWrapWidth(string.Format("Requires {0} to be filled.", high.ParentNode.Name), num, Global.StoreFont), Global.StoreFont, pos, 1, Color.Red, spriteBatch);
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0005D2EC File Offset: 0x0005B4EC
		public void ApplyTalents(Player p)
		{
			foreach (TalentTree talentTree in this.Trees)
			{
				talentTree.ApplyTalents(p);
			}
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0005D340 File Offset: 0x0005B540
		private void Exit()
		{
			this.EXITED = true;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0005D34C File Offset: 0x0005B54C
		private void Save()
		{
			List<int[]> list = new List<int[]>();
			foreach (TalentTree talentTree in this.Trees)
			{
				list.Add(talentTree.GetSave());
			}
			XMLSaverLoader.SaveObject<List<int[]>>(string.Format("Talents{0}.xml", this.INDEX), list);
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0005D3C8 File Offset: 0x0005B5C8
		public void Load()
		{
			if (!File.Exists(string.Format("Talents{0}.xml", this.INDEX)))
			{
				this.DefaultTrees();
				return;
			}
			List<int[]> list;
			XMLSaverLoader.LoadObject<List<int[]>>(string.Format("Talents{0}.xml", this.INDEX), out list);
			int num = 0;
			foreach (TalentTree talentTree in this.Trees)
			{
				talentTree.Load(list[num]);
				num++;
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0005D468 File Offset: 0x0005B668
		public string BuildString()
		{
			string text = "";
			foreach (TalentTree talentTree in this.Trees)
			{
				foreach (int num in talentTree.GetSave())
				{
					if (num == -1)
					{
						num = 0;
					}
					text += num.ToString();
				}
				text += ",";
			}
			Console.WriteLine(text);
			return text;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0005D4FC File Offset: 0x0005B6FC
		public void BuildFromString(string s)
		{
			try
			{
				string[] array = s.Split(new char[]
				{
					','
				});
				for (int i = 0; i < this.Trees.Count; i++)
				{
					int[] array2 = new int[array[i].Length];
					for (int j = 0; j < array[i].Length; j++)
					{
						int num = int.Parse(array[i][j].ToString());
						array2[j] = num;
					}
					this.Trees[i].Load(array2);
				}
			}
			catch
			{
				Console.WriteLine("Build from string failed, could just be empty data.. no biggy.");
			}
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void TransferToNet()
		{
		}

		// Token: 0x04000BFE RID: 3070
		private Vector2 TopLeft;

		// Token: 0x04000BFF RID: 3071
		private int SCALE;

		// Token: 0x04000C00 RID: 3072
		private List<TalentTree> Trees;

		// Token: 0x04000C01 RID: 3073
		private int TotalPoints = 20;

		// Token: 0x04000C02 RID: 3074
		private int INDEX;

		// Token: 0x04000C03 RID: 3075
		public bool EXITED;

		// Token: 0x04000C04 RID: 3076
		private Rectangle Border;

		// Token: 0x04000C05 RID: 3077
		private Rectangle MainPanel;

		// Token: 0x04000C06 RID: 3078
		private Rectangle SmallBorder;

		// Token: 0x04000C07 RID: 3079
		private Rectangle SmallPanel;

		// Token: 0x04000C08 RID: 3080
		private PCButton SaveButton;

		// Token: 0x04000C09 RID: 3081
		private PCButton ResetButton;

		// Token: 0x04000C0A RID: 3082
		private PCButton ExitButton;

		// Token: 0x04000C0B RID: 3083
		private string TEST;
	}
}
