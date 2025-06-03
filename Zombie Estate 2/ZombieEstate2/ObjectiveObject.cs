using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000F5 RID: 245
	public class ObjectiveObject : GameObject
	{
		// Token: 0x0600066A RID: 1642 RVA: 0x0002FC84 File Offset: 0x0002DE84
		public ObjectiveObject(List<Point> animation, Vector3 pos, float liveTime, GameObject changeTo)
		{
			this.Position = pos;
			this.TextureCoord = animation[0];
			this.Animation = animation;
			this.ChangeTo = changeTo;
			this.LiveTime = liveTime;
			this.TotalLiveTime = liveTime;
			this.ActivateObject(this.Position, this.TextureCoord);
			this.scale = 0.4f;
			this.Floating = new FloatingIndicator(this, "Rescue");
			Global.MasterCache.CreateObject(this.Floating);
			if (ObjectiveObject.HoldRightTip == null)
			{
				ObjectiveObject.HoldRightTip = Global.Content.Load<Texture2D>("Tips\\RescueControls");
			}
			Global.Level.GetTileAtLocation(this.Position).HasObjective = true;
			base.GiveIndicator();
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0002FD50 File Offset: 0x0002DF50
		public override void Update(float elapsed)
		{
			this.AnimTime -= elapsed;
			this.LiveTime -= elapsed;
			this.Floating.UpdateData(1f - this.LiveTime / this.TotalLiveTime);
			if (this.AnimTime <= 0f)
			{
				this.AnimTime = ObjectiveObject.AnimationStep;
				this.AnimIndex++;
				if (this.AnimIndex == this.Animation.Count)
				{
					this.AnimIndex = 0;
				}
				this.TextureCoord = this.Animation[this.AnimIndex];
				if (this.SquishesWhenLow && this.LiveTime <= 2f)
				{
					base.SquishMe(1.01f);
				}
			}
			this.CheckForSaving(elapsed);
			if (this.LiveTime <= 0f)
			{
				this.ChangeTo.Position = this.Position;
				this.ChangeTo.ActivateObject(this.ChangeTo.Position, this.ChangeTo.TextureCoord);
				Global.MasterCache.CreateObject(this.ChangeTo);
				Global.WaveMaster.CurrentWave.FailedAnObjective(this);
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0002FE84 File Offset: 0x0002E084
		private void CheckForSaving(float elapsed)
		{
			if (VerchickMath.WithinDistance(Global.Player.TwoDPosition(), base.TwoDPosition(), 1f) && InputManager.RightMouseHeld())
			{
				if (Global.Probability(10))
				{
					for (int i = 0; i < 3; i++)
					{
						Vector3 pos = default(Vector3);
						pos.X = this.Position.X + Global.RandomFloat(0f, 1f) - 0.5f;
						pos.Z = this.Position.Z + Global.RandomFloat(0f, 1f) - 0.5f;
						Global.MasterCache.CreateParticle(ParticleType.Heal, pos, Vector3.Zero);
					}
				}
				this.LiveTime += elapsed * 6f;
				if (this.LiveTime > this.TotalLiveTime)
				{
					HappyRescue happyRescue = new HappyRescue();
					happyRescue.ActivateObject(this.Position, new Point(48, 22));
					Global.MasterCache.CreateObject(happyRescue);
					this.DestroyObject();
					Vector3 position = this.Position;
					position.Y = 0.1f;
					RadialParticle obj = new RadialParticle(new Point(7, 32), 0.05f, 1.4f, 0.1f, position, this);
					Global.MasterCache.CreateObject(obj);
					Global.WaveMaster.CurrentWave.CompletedAnObjective();
				}
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0002FFD7 File Offset: 0x0002E1D7
		public override void DestroyObject()
		{
			Global.Level.GetTileAtLocation(this.Position).HasObjective = false;
			this.Floating.DestroyObject();
			base.DestroyObject();
		}

		// Token: 0x04000646 RID: 1606
		private List<Point> Animation;

		// Token: 0x04000647 RID: 1607
		private GameObject ChangeTo;

		// Token: 0x04000648 RID: 1608
		private float LiveTime;

		// Token: 0x04000649 RID: 1609
		private float AnimTime = ObjectiveObject.AnimationStep;

		// Token: 0x0400064A RID: 1610
		private int AnimIndex;

		// Token: 0x0400064B RID: 1611
		private float TotalLiveTime;

		// Token: 0x0400064C RID: 1612
		private static float AnimationStep = 0.2f;

		// Token: 0x0400064D RID: 1613
		public bool SquishesWhenLow = true;

		// Token: 0x0400064E RID: 1614
		private FloatingIndicator Floating;

		// Token: 0x0400064F RID: 1615
		private static Texture2D HoldRightTip;

		// Token: 0x04000650 RID: 1616
		public int ZombiesAttacking;
	}
}
