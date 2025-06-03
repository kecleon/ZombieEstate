using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2.Cutscene
{
	// Token: 0x020001DD RID: 477
	internal class SpeechBubble : CutSceneLine
	{
		// Token: 0x06000CAB RID: 3243 RVA: 0x000682DC File Offset: 0x000664DC
		public SpeechBubble(string speaker, string message)
		{
			this.SpeakerID = speaker;
			this.Text = VerchickMath.WordWrapWidth(message, (int)(SpeechBubble.Width * this.scale - 32f) - 16, Global.StoreFont);
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00068340 File Offset: 0x00066540
		public override void Run()
		{
			this.Speaker = CutSceneMaster.GetCineObjectByID(this.SpeakerID);
			this.textPos = new Vector2(0f, 0f);
			this.GetPosition();
			this.CreateTimer();
			this.Speaker.Talk(this.Duration);
			base.Run();
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00068398 File Offset: 0x00066598
		private void GetPosition()
		{
			Vector3 vector = Global.GraphicsDevice.Viewport.Project(this.Speaker.GetCinePosition(), Global.Projection, Global.View, Matrix.Identity);
			Vector2 vector2 = new Vector2(vector.X - SpeechBubble.Width * this.scale / 2f + SpeechBubble.XOffset, vector.Y + SpeechBubble.YOffset * this.Speaker.scale - SpeechBubble.Width * this.scale);
			this.rectangle = new Rectangle((int)vector2.X, (int)vector2.Y, (int)(SpeechBubble.Width * this.scale), (int)(SpeechBubble.Width * this.scale));
			this.textPos.X = (float)(this.rectangle.X + 22);
			this.textPos.Y = (float)(this.rectangle.Y + 44);
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00068484 File Offset: 0x00066684
		private void CreateTimer()
		{
			float num = SpeechBubble.secondsPerChar * (float)this.Text.Length;
			num = Math.Max(num, SpeechBubble.minTimer);
			this.Duration = new Timer(num);
			this.Duration.IndependentOfTime = true;
			this.Duration.Start();
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x000684D3 File Offset: 0x000666D3
		public override void Update(float elapsed)
		{
			this.GetPosition();
			base.Update(elapsed);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x000684E4 File Offset: 0x000666E4
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Global.SpeechBubble, this.rectangle, Color.White);
			Shadow.DrawString(this.Text, Global.StoreFont, this.textPos, 1, this.Speaker.textColor, spriteBatch);
			base.Draw(spriteBatch);
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00068531 File Offset: 0x00066731
		public override object[] GetArguments()
		{
			return new object[]
			{
				this.SpeakerID,
				this.Text
			};
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0006854B File Offset: 0x0006674B
		public override string[] GetArgumentDescriptions()
		{
			return new string[]
			{
				"Speaker ID",
				"Message Text"
			};
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x00068563 File Offset: 0x00066763
		public override void SetArguments(object[] args)
		{
			this.SpeakerID = (string)args[0];
			this.Text = (string)args[1];
		}

		// Token: 0x04000D70 RID: 3440
		private float scale = 1f;

		// Token: 0x04000D71 RID: 3441
		private static float Width = 256f;

		// Token: 0x04000D72 RID: 3442
		private static float YOffset = 12f;

		// Token: 0x04000D73 RID: 3443
		private static float XOffset = 28f;

		// Token: 0x04000D74 RID: 3444
		private static float secondsPerChar = 0.08f;

		// Token: 0x04000D75 RID: 3445
		private static float minTimer = 3f;

		// Token: 0x04000D76 RID: 3446
		private Rectangle rectangle;

		// Token: 0x04000D77 RID: 3447
		private string Text = "NULL";

		// Token: 0x04000D78 RID: 3448
		private CineObject Speaker;

		// Token: 0x04000D79 RID: 3449
		private Vector2 textPos;

		// Token: 0x04000D7A RID: 3450
		private string SpeakerID = "NULL";
	}
}
