using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieEstate2.Talents;

namespace ZombieEstate2
{
	// Token: 0x02000099 RID: 153
	internal class TalentTree
	{
		// Token: 0x060003FB RID: 1019 RVA: 0x0001CEF8 File Offset: 0x0001B0F8
		public TalentTree(Point pos, int scale)
		{
			if (TalentTree.TalentBG == null)
			{
				TalentTree.TalentBG = Global.Content.Load<Texture2D>("HUD\\TalentTree");
			}
			this.TopLeft = pos;
			this.SCALE = scale;
			this.Nodes = new TalentNode[this.Width, this.Height];
			this.AddNodes(scale);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001CF68 File Offset: 0x0001B168
		public virtual void AddNodes(int scale)
		{
			this.Nodes[0, 0] = new Talent_HealthI(new Point(0, 0), this.TopLeft, 0, scale);
			this.Nodes[1, 0] = new Talent_MinionMasterI(new Point(1, 0), this.TopLeft, 0, scale);
			this.Nodes[2, 0] = new Talent_ReloadSpeed(new Point(2, 0), this.TopLeft, 0, scale);
			this.Nodes[0, 1] = new Talent_MoveSpeed(new Point(0, 1), this.TopLeft, 0, scale);
			this.Nodes[1, 1] = new Talent_MinionMasterII(new Point(1, 1), this.TopLeft, 0, scale);
			this.Nodes[1, 2] = new Talent_MinionMasterIII(new Point(1, 2), this.TopLeft, 0, scale);
			this.Nodes[1, 3] = new Talent_MinionMasterIII(new Point(1, 3), this.TopLeft, 0, scale);
			TalentNode.LinkNodes(this.Nodes[0, 0], this.Nodes[0, 1]);
			TalentNode.LinkNodes(this.Nodes[1, 0], this.Nodes[1, 1]);
			TalentNode.LinkNodes(this.Nodes[1, 1], this.Nodes[1, 2]);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0001D0BC File Offset: 0x0001B2BC
		public void Update(int ptsRemaining)
		{
			this.HighlightedNode = null;
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = 0; j < this.Height; j++)
				{
					TalentNode talentNode = this.Nodes[i, j];
					if (talentNode != null && talentNode.DestRect.Contains(InputManager.GetMousePosition()))
					{
						this.HighlightedNode = talentNode;
						if (InputManager.LeftMouseClicked())
						{
							if (ptsRemaining <= 0)
							{
								return;
							}
							if (talentNode.LeftClicked(this.PointsSpent))
							{
								this.PointsSpent++;
							}
						}
						if (InputManager.RightMouseClicked() && (!this.AnyNodesFilledBelow(j) || this.PointsSpent - 1 > this.DeepestNode() * 3) && talentNode.RightClicked(this.PointsSpent))
						{
							this.PointsSpent--;
						}
					}
				}
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0001D190 File Offset: 0x0001B390
		private bool AnyNodesFilledBelow(int y)
		{
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = y + 1; j < this.Height; j++)
				{
					if (this.Nodes[i, j] != null && this.Nodes[i, j].PartialOrFilled)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001D1E8 File Offset: 0x0001B3E8
		private int DeepestNode()
		{
			int num = 0;
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = 0; j < this.Height; j++)
				{
					if (this.Nodes[i, j] != null && this.Nodes[i, j].PartialOrFilled && num < j)
					{
						num = j;
					}
				}
			}
			return num;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001D244 File Offset: 0x0001B444
		public void Draw(SpriteBatch spriteBatch)
		{
			Rectangle destinationRectangle = new Rectangle(this.TopLeft.X - 2 * this.SCALE, this.TopLeft.Y - 15 * this.SCALE, TalentTree.TalentBG.Width * this.SCALE, TalentTree.TalentBG.Height * this.SCALE);
			spriteBatch.Draw(TalentTree.TalentBG, destinationRectangle, this.BGColor);
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = 0; j < this.Height; j++)
				{
					TalentNode talentNode = this.Nodes[i, j];
					if (talentNode != null)
					{
						talentNode.Draw(spriteBatch, this.PointsSpent);
					}
				}
			}
			this.DrawName(spriteBatch);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001D2FC File Offset: 0x0001B4FC
		private void DrawName(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2((float)(this.TopLeft.X + (TalentTree.TalentBG.Width / 2 - 2) * this.SCALE), (float)(this.TopLeft.Y - 8 * this.SCALE));
			vector = VerchickMath.CenterText(Global.StoreFont, vector, this.Name);
			if (this.SCALE != 1)
			{
				Shadow.DrawString(this.Name, Global.StoreFont, vector, 1, Color.White, spriteBatch);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0001D37A File Offset: 0x0001B57A
		public virtual string Name
		{
			get
			{
				return "NONE";
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0001D381 File Offset: 0x0001B581
		public virtual Color BGColor
		{
			get
			{
				return Color.White;
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001D388 File Offset: 0x0001B588
		public void Load(int[] saves)
		{
			this.PointsSpent = 0;
			int num = 0;
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = 0; j < this.Height; j++)
				{
					TalentNode talentNode = this.Nodes[i, j];
					if (talentNode != null)
					{
						talentNode.Level = saves[j * this.Width + i];
						this.PointsSpent += talentNode.Level;
					}
					num++;
				}
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001D3FC File Offset: 0x0001B5FC
		public int[] GetSave()
		{
			int[] array = new int[this.Width * this.Height];
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = 0; j < this.Height; j++)
				{
					array[j * this.Width + i] = -1;
					TalentNode talentNode = this.Nodes[i, j];
					if (talentNode != null)
					{
						array[j * this.Width + i] = talentNode.Level;
					}
				}
			}
			return array;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001D470 File Offset: 0x0001B670
		public void ApplyTalents(Player p)
		{
			TalentNode[,] nodes = this.Nodes;
			int upperBound = nodes.GetUpperBound(0);
			int upperBound2 = nodes.GetUpperBound(1);
			for (int i = nodes.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = nodes.GetLowerBound(1); j <= upperBound2; j++)
				{
					TalentNode talentNode = nodes[i, j];
					if (talentNode != null)
					{
						talentNode.Apply(p);
					}
				}
			}
		}

		// Token: 0x040003C2 RID: 962
		public Point TopLeft;

		// Token: 0x040003C3 RID: 963
		public TalentNode[,] Nodes;

		// Token: 0x040003C4 RID: 964
		private int Width = 3;

		// Token: 0x040003C5 RID: 965
		private int Height = 4;

		// Token: 0x040003C6 RID: 966
		public int PointsSpent;

		// Token: 0x040003C7 RID: 967
		private int SCALE = 1;

		// Token: 0x040003C8 RID: 968
		public static Texture2D TalentBG;

		// Token: 0x040003C9 RID: 969
		public TalentNode HighlightedNode;
	}
}
