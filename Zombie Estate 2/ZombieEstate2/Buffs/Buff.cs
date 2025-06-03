using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.Buffs
{
	// Token: 0x020001E4 RID: 484
	public class Buff
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0001D37A File Offset: 0x0001B57A
		public virtual string Description
		{
			get
			{
				return "NONE";
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0006977D File Offset: 0x0006797D
		public Buff()
		{
			Buff.BGSrc = Global.GetTexRectange(1, 40);
			Buff.BGOutlineSrc = Global.GetTexRectange(2, 40);
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x000697BC File Offset: 0x000679BC
		public virtual void Init(Shootable parent, Shootable attacker)
		{
			this.Parent = parent;
			this.Attacker = attacker;
			this.Src = Global.GetTexRectange(0, 0);
			this.tick = this.TickLength;
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Arguments(List<string> args)
		{
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x000697E8 File Offset: 0x000679E8
		public void GiveArguments(string arg)
		{
			if (string.IsNullOrEmpty(arg))
			{
				return;
			}
			string[] array = arg.Split(new char[]
			{
				','
			});
			List<string> list = new List<string>();
			foreach (string item in array)
			{
				list.Add(item);
			}
			this.Arguments(list);
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00069836 File Offset: 0x00067A36
		public virtual void Update(float elapsed)
		{
			this.TimeAlive += elapsed;
			this.tick -= elapsed;
			if (this.tick <= 0f)
			{
				this.tick = this.TickLength;
				this.Tick();
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Tick()
		{
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Applied()
		{
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void Removed()
		{
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00069873 File Offset: 0x00067A73
		public virtual void Destroyed()
		{
			if (this.Effect != null)
			{
				this.Effect.DestroyObject();
				this.Effect = null;
			}
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void TakenDamage(Shootable attacker)
		{
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void DeltDamage(Shootable target)
		{
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00002EF9 File Offset: 0x000010F9
		public virtual void KilledATarget(Shootable target)
		{
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0006988F File Offset: 0x00067A8F
		public virtual float ApplyDamageMod(float start)
		{
			return start;
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0006988F File Offset: 0x00067A8F
		public virtual float ApplyHurtMod(float start)
		{
			return start;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0006988F File Offset: 0x00067A8F
		public virtual float ApplySpeedMod(float start)
		{
			return start;
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0000B472 File Offset: 0x00009672
		public virtual bool UpdateSpec(ref SpecialProperties spec)
		{
			return false;
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00069894 File Offset: 0x00067A94
		public bool Draw(SpriteBatch spriteBatch, int index, int origX, int origY, Color hudColor)
		{
			Rectangle destinationRectangle = new Rectangle(origX + index * 18 * Buff.Scale, origY, 16 * Buff.Scale, 16 * Buff.Scale);
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(Buff.BGSrc), this.Positive ? new Color(0.3f, 0.8f, 0.4f) : new Color(0.8f, 0.3f, 0.3f));
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(Buff.BGOutlineSrc), this.Positive ? Color.LightGreen : Color.Pink);
			spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(this.Src), Color.White);
			if (this.Time != 0f)
			{
				Rectangle texRectange = Global.GetTexRectange((int)(10f * this.DeltaTime), 41);
				spriteBatch.Draw(Global.MasterTexture, destinationRectangle, new Rectangle?(texRectange), Color.Black * 0.7f);
			}
			return false;
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x00069998 File Offset: 0x00067B98
		public bool Expired
		{
			get
			{
				return this.Time != 0f && this.TimeAlive >= this.Time;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x000699BA File Offset: 0x00067BBA
		public float DeltaTime
		{
			get
			{
				if (this.Time == 0f)
				{
					return 0f;
				}
				return this.TimeAlive / this.Time;
			}
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x000699DC File Offset: 0x00067BDC
		public void TopOfParent(out Vector3 pos)
		{
			pos = default(Vector3);
			pos.X = this.Parent.Position.X;
			pos.Y = this.Parent.Position.Y + this.Parent.scale * 1.1f;
			pos.Z = this.Parent.Position.Z + 0.01f;
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00069A4C File Offset: 0x00067C4C
		public void FrontOfParent(out Vector3 pos)
		{
			pos = default(Vector3);
			pos.X = this.Parent.Position.X;
			pos.Y = this.Parent.Position.Y;
			pos.Z = this.Parent.Position.Z + 0.08f;
		}

		// Token: 0x04000D96 RID: 3478
		public Shootable Parent;

		// Token: 0x04000D97 RID: 3479
		public Shootable Attacker;

		// Token: 0x04000D98 RID: 3480
		private float TimeAlive;

		// Token: 0x04000D99 RID: 3481
		public float Time;

		// Token: 0x04000D9A RID: 3482
		public Rectangle Src;

		// Token: 0x04000D9B RID: 3483
		public bool CanNotBeOverwritten;

		// Token: 0x04000D9C RID: 3484
		public GameObject Effect;

		// Token: 0x04000D9D RID: 3485
		public string Name = "NONE";

		// Token: 0x04000D9E RID: 3486
		private static Rectangle BGSrc;

		// Token: 0x04000D9F RID: 3487
		private static Rectangle BGOutlineSrc;

		// Token: 0x04000DA0 RID: 3488
		private static int Scale = 2;

		// Token: 0x04000DA1 RID: 3489
		public bool Drawable = true;

		// Token: 0x04000DA2 RID: 3490
		public bool Positive;

		// Token: 0x04000DA3 RID: 3491
		public float TickLength = 1f;

		// Token: 0x04000DA4 RID: 3492
		private float tick;
	}
}
