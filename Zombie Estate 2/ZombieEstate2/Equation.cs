using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000059 RID: 89
	public class Equation
	{
		// Token: 0x06000206 RID: 518 RVA: 0x0000ECE4 File Offset: 0x0000CEE4
		public Equation(EquationOperation op, int left, int right, int result, WaveObjectiveType leftType, WaveObjectiveType rightType, WaveObjectiveType resultType, string header, Vector2 topLeft) : this(op, left, right, result, leftType, rightType, resultType, header, topLeft, false, new Color(60, 60, 60))
		{
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000ED14 File Offset: 0x0000CF14
		public Equation(EquationOperation op, int left, int right, int result, WaveObjectiveType leftType, WaveObjectiveType rightType, WaveObjectiveType resultType, string header, Vector2 topLeft, bool linked, Color bg)
		{
			this.mResultType = resultType;
			this.mResultAmount = result;
			this.operation = op;
			this.Position = new Vector2(topLeft.X, topLeft.Y);
			this.bgColor = bg;
			if (Equation.BG == null)
			{
				Equation.BG = Global.Content.Load<Texture2D>("Equation");
				Equation.ArrowDown = Global.Content.Load<Texture2D>("ArrowDown");
			}
			if (this.font == null)
			{
				this.font = Global.EquationFont;
			}
			this.headerFont = Global.EquationFont;
			this.header = header;
			if (Global.EquationFont.MeasureString(header).X > 270f)
			{
				this.headerFont = Global.EquationFontSmall;
			}
			this.headerPos = VerchickMath.CenterText(this.headerFont, new Vector2(topLeft.X + 140f, topLeft.Y + 24f), header);
			this.opPos = new Vector2(topLeft.X + 32f, topLeft.Y + 80f);
			this.GetOpText(op);
			this.equalsLine = new Rectangle((int)topLeft.X + 10, (int)topLeft.Y + 116, 260, 2);
			this.equalsShadow = new Rectangle(this.equalsLine.X + 1, this.equalsLine.Y + 1, this.equalsLine.Width, this.equalsLine.Height);
			this.elements = new List<EquationElement>();
			this.elements.Add(new EquationElement(leftType, left, EquationPosition.Left, new Vector2(topLeft.X + 100f, topLeft.Y + 48f)));
			this.elements.Add(new EquationElement(rightType, right, EquationPosition.Right, new Vector2(topLeft.X + 100f, topLeft.Y + 80f)));
			this.elements.Add(new EquationElement(resultType, result, EquationPosition.Result, new Vector2(topLeft.X + 100f, topLeft.Y + 118f)));
			this.Linked = linked;
			this.LinkPos = new Vector2(topLeft.X + 140f - (float)(Equation.ArrowDown.Width / 2), topLeft.Y - (float)(Equation.ArrowDown.Height / 2) + 12f);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000EFAC File Offset: 0x0000D1AC
		private void GetOpText(EquationOperation op)
		{
			switch (op)
			{
			case EquationOperation.Addition:
				this.opText = "+";
				return;
			case EquationOperation.Subtraction:
				this.opText = "-";
				return;
			case EquationOperation.Multiplication:
				this.opText = "x";
				return;
			case EquationOperation.Division:
				this.opText = "/";
				return;
			default:
				return;
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000F000 File Offset: 0x0000D200
		public void Start()
		{
			this.totalTime = 5f;
			this.timer = new Timer(this.totalTime);
			this.timer.IndependentOfTime = true;
			this.timer.DeltaDelegate = new Timer.TimerDelegate(this.Delta);
			this.timer.Start();
			this.index = 0;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000F060 File Offset: 0x0000D260
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Equation.BG, this.Position, this.bgColor);
			if (this.Linked)
			{
				spriteBatch.Draw(Equation.ArrowDown, this.LinkPos, Color.White);
			}
			Shadow.DrawString(this.header, this.headerFont, this.headerPos, 1, Color.White, spriteBatch);
			Shadow.DrawString(this.opText, this.font, this.opPos, 1, Color.White, spriteBatch);
			spriteBatch.Draw(Global.Pixel, this.equalsShadow, Color.Black);
			spriteBatch.Draw(Global.Pixel, this.equalsLine, Color.White);
			foreach (EquationElement equationElement in this.elements)
			{
				equationElement.Draw(spriteBatch);
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000F150 File Offset: 0x0000D350
		private void Delta(float d)
		{
			float num = 0.2f;
			float num2 = 0.4f;
			if (d < num)
			{
				this.index = 0;
			}
			if (d > num && d < num2)
			{
				this.index = 1;
			}
			if (d > num2)
			{
				this.index = 2;
			}
			if (!this.elements[this.index].Started)
			{
				this.elements[this.index].Start(this.totalTime / 3f);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000F1C8 File Offset: 0x0000D3C8
		public WaveObjectiveType ResultType
		{
			get
			{
				return this.mResultType;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000F1D0 File Offset: 0x0000D3D0
		public int ResultAmount
		{
			get
			{
				return this.mResultAmount;
			}
		}

		// Token: 0x040001A1 RID: 417
		private static Texture2D BG;

		// Token: 0x040001A2 RID: 418
		private static Texture2D ArrowDown;

		// Token: 0x040001A3 RID: 419
		private EquationOperation operation;

		// Token: 0x040001A4 RID: 420
		private List<EquationElement> elements;

		// Token: 0x040001A5 RID: 421
		private Timer timer;

		// Token: 0x040001A6 RID: 422
		private int index;

		// Token: 0x040001A7 RID: 423
		private float totalTime;

		// Token: 0x040001A8 RID: 424
		private string header = "";

		// Token: 0x040001A9 RID: 425
		private Vector2 headerPos;

		// Token: 0x040001AA RID: 426
		private string opText = "+";

		// Token: 0x040001AB RID: 427
		private Vector2 opPos;

		// Token: 0x040001AC RID: 428
		private SpriteFont font;

		// Token: 0x040001AD RID: 429
		private SpriteFont headerFont;

		// Token: 0x040001AE RID: 430
		private Rectangle equalsLine;

		// Token: 0x040001AF RID: 431
		private Rectangle equalsShadow;

		// Token: 0x040001B0 RID: 432
		private Vector2 Position;

		// Token: 0x040001B1 RID: 433
		private bool Linked;

		// Token: 0x040001B2 RID: 434
		private Vector2 LinkPos;

		// Token: 0x040001B3 RID: 435
		private Color bgColor = new Color(60, 60, 60);

		// Token: 0x040001B4 RID: 436
		private WaveObjectiveType mResultType;

		// Token: 0x040001B5 RID: 437
		private int mResultAmount;
	}
}
