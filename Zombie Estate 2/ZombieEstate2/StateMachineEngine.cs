using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200003F RID: 63
	public class StateMachineEngine<T>
	{
		// Token: 0x0600017E RID: 382 RVA: 0x0000AFBD File Offset: 0x000091BD
		public StateMachineEngine()
		{
			this.States = new Dictionary<T, StateMachineEngine<T>.StateHolder<T>>();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000AFD0 File Offset: 0x000091D0
		public void AddState(T state, float timeInState, T nextState, int texXCoord, int texYCoord, bool invincible, UpdateStateDelegate updateDel)
		{
			StateMachineEngine<T>.StateHolder<T> value = default(StateMachineEngine<T>.StateHolder<T>);
			value.State = state;
			value.NextState = nextState;
			value.Invincible = invincible;
			value.Time = timeInState;
			value.Tex = new Point(texXCoord, texYCoord);
			value.Delegate = updateDel;
			this.States.Add(state, value);
			if (this.Current == null)
			{
				this.Current = state;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000B040 File Offset: 0x00009240
		// (set) Token: 0x06000181 RID: 385 RVA: 0x0000B048 File Offset: 0x00009248
		public T CurrentState
		{
			get
			{
				return this.Current;
			}
			set
			{
				if (!this.Current.Equals(value))
				{
					if (this.Switched != null)
					{
						this.Switched();
					}
					this.Current = value;
					this.time = 0f;
				}
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000B088 File Offset: 0x00009288
		public void Update(float elapsed)
		{
			this.time += elapsed;
			if (this.time >= this.States[this.Current].Time && this.States[this.Current].Time != -1f)
			{
				if (this.States[this.Current].Delegate != null)
				{
					this.States[this.Current].Delegate(1f);
				}
				if (this.States[this.Current].NextState != null)
				{
					if (!Global.NETWORKED || this.ActLocal)
					{
						this.CurrentState = this.States[this.Current].NextState;
						return;
					}
					this.CurrentState = this.States[this.Current].NextState;
					return;
				}
			}
			else if (this.States[this.Current].Delegate != null)
			{
				this.States[this.Current].Delegate(this.CurrentDelta);
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000B1C0 File Offset: 0x000093C0
		public void SetTimeInState(float timeInState, T state)
		{
			StateMachineEngine<T>.StateHolder<T> value = this.States[state];
			value.Time = timeInState;
			this.States.Remove(state);
			this.States.Add(state, value);
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000184 RID: 388 RVA: 0x0000B1FC File Offset: 0x000093FC
		public bool InvInCurrent
		{
			get
			{
				return this.States[this.Current].Invincible;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000B214 File Offset: 0x00009414
		public Point CurrentTex
		{
			get
			{
				return this.States[this.Current].Tex;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000B22C File Offset: 0x0000942C
		public float CurrentDelta
		{
			get
			{
				return Math.Min(this.time / this.States[this.Current].Time, 1f);
			}
		}

		// Token: 0x040000F7 RID: 247
		private Dictionary<T, StateMachineEngine<T>.StateHolder<T>> States;

		// Token: 0x040000F8 RID: 248
		private float time;

		// Token: 0x040000F9 RID: 249
		private T Current;

		// Token: 0x040000FA RID: 250
		public bool ActLocal;

		// Token: 0x040000FB RID: 251
		public StateSwitched Switched;

		// Token: 0x02000207 RID: 519
		private struct StateHolder<T>
		{
			// Token: 0x04000DF8 RID: 3576
			public T State;

			// Token: 0x04000DF9 RID: 3577
			public float Time;

			// Token: 0x04000DFA RID: 3578
			public T NextState;

			// Token: 0x04000DFB RID: 3579
			public bool Invincible;

			// Token: 0x04000DFC RID: 3580
			public Point Tex;

			// Token: 0x04000DFD RID: 3581
			public UpdateStateDelegate Delegate;
		}
	}
}
