using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.Talents
{
	// Token: 0x02000197 RID: 407
	internal class TalentNode
	{
		// Token: 0x06000B87 RID: 2951 RVA: 0x0005EFC0 File Offset: 0x0005D1C0
		public TalentNode(Point tex, Point loc, Point topLeft, int level, int scale)
		{
			this.SetupGfx();
			this.SCALE = scale;
			this.SrcRect = Global.GetTexRectange(tex.X, tex.Y);
			this.DestRect = new Rectangle(topLeft.X + loc.X * 20 * this.SCALE, topLeft.Y + loc.Y * 20 * this.SCALE, 16 * this.SCALE, 16 * this.SCALE);
			this.NeededPoints = loc.Y * 3;
			this.Level = level;
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0005F080 File Offset: 0x0005D280
		private void SetupGfx()
		{
			if (TalentNode.BGRect == null)
			{
				TalentNode.BGRect = new Dictionary<TalentNode.TalentState, Rectangle>();
				TalentNode.BGRect.Add(TalentNode.TalentState.NotEnoughPoints, Global.GetTexRectange(60, 51));
				TalentNode.BGRect.Add(TalentNode.TalentState.Filled, Global.GetTexRectange(60, 50));
				TalentNode.BGRect.Add(TalentNode.TalentState.Open, Global.GetTexRectange(60, 49));
				TalentNode.BGRect.Add(TalentNode.TalentState.Partial, Global.GetTexRectange(60, 49));
				TalentNode.BGRect.Add(TalentNode.TalentState.DependantMissing, Global.GetTexRectange(60, 48));
				TalentNode.BGRectSel = new Dictionary<TalentNode.TalentState, Rectangle>();
				foreach (TalentNode.TalentState key in TalentNode.BGRect.Keys)
				{
					TalentNode.BGRectSel.Add(key, new Rectangle(TalentNode.BGRect[key].X + 16, TalentNode.BGRect[key].Y, TalentNode.BGRect[key].Width, TalentNode.BGRect[key].Height));
				}
				TalentNode.ArrowRect = Global.GetTexRectange(60, 53);
			}
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00002EF9 File Offset: 0x000010F9
		protected virtual void ApplyTalent(Player player)
		{
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0005F1B4 File Offset: 0x0005D3B4
		public void Apply(Player player)
		{
			if (this.Level > 0)
			{
				this.ApplyTalent(player);
			}
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0005F1C6 File Offset: 0x0005D3C6
		public bool LeftClicked(int spent)
		{
			if (this.State == TalentNode.TalentState.Partial || this.State == TalentNode.TalentState.Open)
			{
				this.AddLevel();
				return true;
			}
			return false;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0005F1E3 File Offset: 0x0005D3E3
		public bool RightClicked(int spent)
		{
			if (this.State != TalentNode.TalentState.Partial && this.State != TalentNode.TalentState.Filled)
			{
				return false;
			}
			if (this.ChildNode != null && this.ChildNode.PartialOrFilled)
			{
				return false;
			}
			this.RemoveLevel();
			return true;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0005F217 File Offset: 0x0005D417
		private void AddLevel()
		{
			if (this.Level < this.MaxLevel)
			{
				this.Level++;
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0005F235 File Offset: 0x0005D435
		private void RemoveLevel()
		{
			if (this.Level > 0)
			{
				this.Level--;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x0005F24E File Offset: 0x0005D44E
		public bool Filled
		{
			get
			{
				return this.MaxLevel == this.Level;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x0005F25E File Offset: 0x0005D45E
		public bool Empty
		{
			get
			{
				return this.Level == 0;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0005F269 File Offset: 0x0005D469
		public bool PartialOrFilled
		{
			get
			{
				return this.Level > 0;
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0005F274 File Offset: 0x0005D474
		public void Draw(SpriteBatch spriteBatch, int pts)
		{
			this.UpdateState(pts);
			if (this.DestRect.Contains(InputManager.GetMousePosition()))
			{
				spriteBatch.Draw(Global.MasterTexture, this.DestRect, new Rectangle?(TalentNode.BGRectSel[this.State]), Color.White);
			}
			else
			{
				spriteBatch.Draw(Global.MasterTexture, this.DestRect, new Rectangle?(TalentNode.BGRect[this.State]), Color.White);
			}
			if (this.State == TalentNode.TalentState.NotEnoughPoints)
			{
				spriteBatch.Draw(Global.MasterTexture, this.DestRect, new Rectangle?(this.SrcRect), new Color(0.2f, 0.2f, 0.2f));
			}
			else
			{
				spriteBatch.Draw(Global.MasterTexture, this.DestRect, new Rectangle?(this.SrcRect), Color.White);
			}
			if (this.SCALE > 1)
			{
				Vector2 position = new Vector2((float)(this.DestRect.X + 2 * this.SCALE), (float)(this.DestRect.Y + 1 * this.SCALE));
				string text = string.Format("{0}/{1}", this.Level, this.MaxLevel);
				Shadow.DrawOutlinedString(spriteBatch, Global.StoreFont, text, Color.Black, Color.White, 0.8f, 0f, position);
			}
			if (this.ParentNode != null)
			{
				Rectangle destinationRectangle = new Rectangle(this.DestRect.X, this.DestRect.Y - 14 * this.SCALE, 16 * this.SCALE, 16 * this.SCALE);
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(TalentNode.ArrowRect), Color.White);
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0005F428 File Offset: 0x0005D628
		private void UpdateState(int pts)
		{
			if (this.SCALE == 1)
			{
				if (this.Filled)
				{
					this.State = TalentNode.TalentState.Filled;
					return;
				}
				if (this.Level > 0)
				{
					this.State = TalentNode.TalentState.Partial;
					return;
				}
				this.State = TalentNode.TalentState.NotEnoughPoints;
				return;
			}
			else
			{
				if (this.NeededPoints > pts)
				{
					this.State = TalentNode.TalentState.NotEnoughPoints;
					return;
				}
				if (this.ParentNode != null && !this.ParentNode.Filled)
				{
					this.State = TalentNode.TalentState.DependantMissing;
					return;
				}
				if (this.Filled)
				{
					this.State = TalentNode.TalentState.Filled;
					return;
				}
				if (this.Level > 0)
				{
					this.State = TalentNode.TalentState.Partial;
					return;
				}
				this.State = TalentNode.TalentState.Open;
				return;
			}
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0005F4BD File Offset: 0x0005D6BD
		public static void LinkNodes(TalentNode parent, TalentNode child)
		{
			parent.ChildNode = child;
			child.ParentNode = parent;
		}

		// Token: 0x04000C0C RID: 3084
		public string Name = "NONE";

		// Token: 0x04000C0D RID: 3085
		public List<string> Descriptions = new List<string>();

		// Token: 0x04000C0E RID: 3086
		public int Level;

		// Token: 0x04000C0F RID: 3087
		public int MaxLevel = 1;

		// Token: 0x04000C10 RID: 3088
		public Rectangle SrcRect;

		// Token: 0x04000C11 RID: 3089
		private static Dictionary<TalentNode.TalentState, Rectangle> BGRect;

		// Token: 0x04000C12 RID: 3090
		private static Dictionary<TalentNode.TalentState, Rectangle> BGRectSel;

		// Token: 0x04000C13 RID: 3091
		private static Rectangle ArrowRect;

		// Token: 0x04000C14 RID: 3092
		public Rectangle DestRect;

		// Token: 0x04000C15 RID: 3093
		public TalentNode.TalentState State;

		// Token: 0x04000C16 RID: 3094
		private int SCALE = 1;

		// Token: 0x04000C17 RID: 3095
		public TalentNode ParentNode;

		// Token: 0x04000C18 RID: 3096
		public TalentNode ChildNode;

		// Token: 0x04000C19 RID: 3097
		private int NeededPoints;

		// Token: 0x0200022B RID: 555
		public enum TalentState
		{
			// Token: 0x04000E7A RID: 3706
			NotEnoughPoints,
			// Token: 0x04000E7B RID: 3707
			DependantMissing,
			// Token: 0x04000E7C RID: 3708
			Open,
			// Token: 0x04000E7D RID: 3709
			Partial,
			// Token: 0x04000E7E RID: 3710
			Filled
		}
	}
}
