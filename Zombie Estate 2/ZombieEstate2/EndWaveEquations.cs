using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000057 RID: 87
	internal class EndWaveEquations
	{
		// Token: 0x060001FE RID: 510 RVA: 0x0000E8E4 File Offset: 0x0000CAE4
		public EndWaveEquations()
		{
			this.Grid = new Equation[this.NUM, this.NUM];
			this.xOff = Global.ScreenRect.Width / 2 - 640;
			this.yOff = Global.ScreenRect.Height / 2 - 380;
			this.timer = new Timer(this.TOTALTIME / (float)(this.NUM * this.NUM));
			this.timer.DeltaDelegate = new Timer.TimerDelegate(this.Delta);
			this.timer.IndependentOfTime = true;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000E994 File Offset: 0x0000CB94
		public void AddEquation(int x, int y, EqData data)
		{
			if (this.Grid[x, y] != null)
			{
				throw new Exception("ERROR! Equation slot taken already!");
			}
			Vector2 topLeft = new Vector2((float)(this.xOff + x * 320), (float)(this.yOff + y * 190));
			Equation equation = new Equation(data.op, data.left, data.right, data.result, data.leftType, data.rightType, data.resultType, data.header, topLeft, data.linked, data.color);
			this.Grid[x, y] = equation;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000EA34 File Offset: 0x0000CC34
		public void AddEquation(EqData data)
		{
			for (int i = 0; i < this.NUM; i++)
			{
				for (int j = 0; j < this.NUM - 1; j++)
				{
					if (this.Grid[i, j] == null)
					{
						this.AddEquation(i, j, data);
						return;
					}
				}
			}
			throw new Exception("ERROR! No more equation slots!");
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000EA88 File Offset: 0x0000CC88
		public void AddEquationDual(EqData data, EqData data2)
		{
			for (int i = 0; i < this.NUM; i++)
			{
				if (this.Grid[i, 0] == null && this.Grid[i, 1] == null)
				{
					this.AddEquation(i, 0, data);
					this.AddEquation(i, 1, data2);
					return;
				}
			}
			for (int j = 0; j < this.NUM; j++)
			{
				if (this.Grid[j, 1] == null && this.Grid[j, 2] == null)
				{
					this.AddEquation(j, 1, data);
					this.AddEquation(j, 2, data2);
					return;
				}
			}
			throw new Exception("ERROR! No more equation slots!");
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000EB24 File Offset: 0x0000CD24
		public void Start()
		{
			for (int i = 0; i < this.NUM; i++)
			{
				for (int j = 0; j < this.NUM; j++)
				{
					if (this.Grid[i, j] != null)
					{
						this.Grid[i, j].Start();
					}
				}
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000EB74 File Offset: 0x0000CD74
		private void Delta(float d)
		{
			if (d == 1f)
			{
				this.timer.Reset();
				this.timer.Start();
				if (this.Grid[this.shimX, this.shimY] != null)
				{
					this.Grid[this.shimX, this.shimY].Start();
				}
				this.shimX++;
				if (this.shimX >= this.NUM)
				{
					this.shimY++;
					this.shimX = 0;
					if (this.shimY >= this.NUM)
					{
						this.timer.Stop();
					}
				}
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000EC24 File Offset: 0x0000CE24
		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < this.NUM; i++)
			{
				for (int j = 0; j < this.NUM; j++)
				{
					if (this.Grid[i, j] != null)
					{
						this.Grid[i, j].Draw(spriteBatch);
					}
				}
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000EC78 File Offset: 0x0000CE78
		public int AddUpMoney()
		{
			int num = 0;
			for (int i = 0; i < this.NUM; i++)
			{
				for (int j = 0; j < this.NUM; j++)
				{
					if (this.Grid[i, j] != null && this.Grid[i, j].ResultType == WaveObjectiveType.Money)
					{
						num += this.Grid[i, j].ResultAmount;
					}
				}
			}
			return num;
		}

		// Token: 0x0400018F RID: 399
		private Equation[,] Grid;

		// Token: 0x04000190 RID: 400
		private int xOff;

		// Token: 0x04000191 RID: 401
		private int yOff;

		// Token: 0x04000192 RID: 402
		private int NUM = 4;

		// Token: 0x04000193 RID: 403
		private float TOTALTIME = 10f;

		// Token: 0x04000194 RID: 404
		private Timer timer;

		// Token: 0x04000195 RID: 405
		private int shimX;

		// Token: 0x04000196 RID: 406
		private int shimY;
	}
}
