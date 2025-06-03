using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000B4 RID: 180
	internal class BobbingPortrait : StoreElement
	{
		// Token: 0x06000492 RID: 1170 RVA: 0x00021BD4 File Offset: 0x0001FDD4
		public BobbingPortrait(Point headPoint, Point bodyPoint, float scale, Point topLeft, float secondsPerOneBeat)
		{
			this.HeadRect = Global.GetTexRectange(headPoint.X, headPoint.Y);
			this.BodyRect = Global.GetTexRectange(bodyPoint.X, bodyPoint.Y);
			this.scale = scale;
			int num = (int)(scale * 16f);
			this.HeadDest = new Rectangle(topLeft.X, topLeft.Y, num, num);
			this.BodyDest = new Rectangle(topLeft.X, topLeft.Y, num, num);
			this.StoreRect = new Rectangle(16, 240, 16, 16);
			this.StoreDest = new Rectangle(topLeft.X, topLeft.Y, (int)(16f * scale), (int)(16f * scale));
			this.originX = this.HeadDest.X;
			this.originY = this.HeadDest.Y;
			this.UpDownTimer = new Timer(secondsPerOneBeat / 2f);
			this.LeftRightTimer = new Timer(secondsPerOneBeat * 2f);
			this.UpDownTimer.DeltaDelegate = new Timer.TimerDelegate(this.HeadBob);
			this.UpDownTimer.IndependentOfTime = true;
			this.UpDownTimer.Start();
			this.LeftRightTimer.DeltaDelegate = new Timer.TimerDelegate(this.HeadLeftRight);
			this.LeftRightTimer.IndependentOfTime = true;
			this.LeftRightTimer.Start();
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00021ADB File Offset: 0x0001FCDB
		public override void Update()
		{
			base.Update();
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00021D48 File Offset: 0x0001FF48
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Global.MasterTexture, this.BodyDest, new Rectangle?(this.BodyRect), Color.White);
			spriteBatch.Draw(Global.MasterTexture, this.HeadDest, new Rectangle?(this.HeadRect), Color.White);
			spriteBatch.Draw(Global.MasterEnvTex, this.StoreDest, new Rectangle?(this.StoreRect), Color.White);
			base.Draw(spriteBatch);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00021DBF File Offset: 0x0001FFBF
		public override void Destroy()
		{
			this.UpDownTimer.Stop();
			this.LeftRightTimer.Stop();
			this.UpDownTimer.DeltaDelegate = null;
			this.LeftRightTimer.DeltaDelegate = null;
			base.Destroy();
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00021DF8 File Offset: 0x0001FFF8
		private void HeadBob(float delta)
		{
			if (this.down)
			{
				this.HeadDest.Y = this.originY + (int)(delta * (float)BobbingPortrait.Offset * this.scale);
			}
			else
			{
				this.HeadDest.Y = this.originY + ((int)((float)BobbingPortrait.Offset * this.scale) - (int)(delta * (float)BobbingPortrait.Offset * this.scale));
			}
			if (delta >= 1f)
			{
				this.UpDownTimer.Reset();
				this.UpDownTimer.Start();
				this.down = !this.down;
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00021E90 File Offset: 0x00020090
		private void HeadLeftRight(float delta)
		{
			this.HeadDest.X = (int)((Math.Abs(delta - 0.5f) * 2f - 0.5f) * this.scale) + this.originX;
			if (delta >= 1f)
			{
				this.LeftRightTimer.Reset();
				this.LeftRightTimer.Start();
			}
		}

		// Token: 0x04000485 RID: 1157
		private Rectangle HeadRect;

		// Token: 0x04000486 RID: 1158
		private Rectangle BodyRect;

		// Token: 0x04000487 RID: 1159
		private Rectangle StoreRect;

		// Token: 0x04000488 RID: 1160
		private Rectangle HeadDest;

		// Token: 0x04000489 RID: 1161
		private Rectangle BodyDest;

		// Token: 0x0400048A RID: 1162
		private Rectangle StoreDest;

		// Token: 0x0400048B RID: 1163
		private int originX;

		// Token: 0x0400048C RID: 1164
		private int originY;

		// Token: 0x0400048D RID: 1165
		private Timer UpDownTimer;

		// Token: 0x0400048E RID: 1166
		private Timer LeftRightTimer;

		// Token: 0x0400048F RID: 1167
		private float scale;

		// Token: 0x04000490 RID: 1168
		private static int Offset = 1;

		// Token: 0x04000491 RID: 1169
		private bool down = true;
	}
}
