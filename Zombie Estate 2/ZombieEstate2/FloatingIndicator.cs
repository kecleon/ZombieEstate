using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000044 RID: 68
	public class FloatingIndicator : GameObject
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x0000BC1C File Offset: 0x00009E1C
		public FloatingIndicator(GameObject parent, string type)
		{
			if (type == "Reload")
			{
				this.originTex = new Point(0, 35);
			}
			if (type == "Charge")
			{
				this.originTex = new Point(0, 36);
			}
			if (type == "DurationMeter")
			{
				this.originTex = new Point(10, 35);
				this.offset.Y = 0.45f;
			}
			if (type == "HealthMeter")
			{
				this.originTex = new Point(10, 38);
				this.offset.Y = 0.45f;
			}
			if (type == "Rescue")
			{
				this.originTex = new Point(10, 36);
			}
			this.scale = 0.25f;
			this.AffectedByGravity = false;
			this.parent = parent;
			this.ActivateObject(parent.Position + this.offset, this.originTex);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000BD30 File Offset: 0x00009F30
		public FloatingIndicator(GameObject parent, Point tex, float seconds)
		{
			this.originTex = tex;
			this.offset.X = this.offset.X + 0.05f;
			this.offset.Z = this.offset.Z - 0.05f;
			this.seconds = seconds;
			this.timer = new Timer(seconds);
			this.timer.DeltaDelegate = new Timer.TimerDelegate(this.UpdateTimer);
			this.timer.Start();
			this.scale = 0.25f;
			this.AffectedByGravity = false;
			this.parent = parent;
			this.ActivateObject(parent.Position + this.offset, this.originTex);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000BE00 File Offset: 0x0000A000
		public override void Update(float elapsed)
		{
			if (!this.parent.Active)
			{
				this.DestroyObject();
			}
			this.Position = this.parent.Position + this.offset;
			if (this.timer != null && this.timer.Expired())
			{
				this.DestroyObject();
				return;
			}
			if (this.parent is Player && (this.parent as Player).DEAD)
			{
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000BE84 File Offset: 0x0000A084
		public void UpdateData(float data, float maxData)
		{
			if (maxData == 0f)
			{
				return;
			}
			int num = (int)(10f * data / maxData);
			if (num > 10)
			{
				num = 10;
			}
			this.TextureCoord.X = this.originTex.X + num;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000BEC8 File Offset: 0x0000A0C8
		public void UpdateData(float delta)
		{
			delta = Math.Min(1f, delta);
			int num = (int)(9f * delta);
			this.TextureCoord.X = this.originTex.X + num;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000BF04 File Offset: 0x0000A104
		private void UpdateTimer(float delta)
		{
			float num = 1f - delta;
			float num2 = 0.5f;
			if (num > num2)
			{
				num = 1f;
			}
			else
			{
				num = num2 * this.seconds * num + num;
			}
			if (this.Shrinks)
			{
				this.scale = 0.25f * num;
			}
		}

		// Token: 0x0400010C RID: 268
		private GameObject parent;

		// Token: 0x0400010D RID: 269
		private Point originTex;

		// Token: 0x0400010E RID: 270
		private Vector3 offset = new Vector3(0f, 1f, 0.1f);

		// Token: 0x0400010F RID: 271
		public Timer timer;

		// Token: 0x04000110 RID: 272
		private float seconds;

		// Token: 0x04000111 RID: 273
		public bool Shrinks = true;
	}
}
