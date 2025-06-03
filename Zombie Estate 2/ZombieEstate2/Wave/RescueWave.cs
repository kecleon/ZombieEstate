using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2.Wave
{
	// Token: 0x02000153 RID: 339
	internal class RescueWave : TimedWave
	{
		// Token: 0x06000A3C RID: 2620 RVA: 0x00053D6C File Offset: 0x00051F6C
		public RescueWave()
		{
			this.anim.Add(new Point(51, 18));
			this.anim.Add(new Point(51, 19));
			this.anim.Add(new Point(51, 20));
			this.anim.Add(new Point(51, 21));
			this.Objectives = new List<ObjectiveObject>();
			this.Tip = new WaveTip("RescueWave");
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00053DFC File Offset: 0x00051FFC
		public override void ResetWave()
		{
			foreach (ObjectiveObject objectiveObject in this.Objectives)
			{
				objectiveObject.DestroyObject();
			}
			this.RescuedCount = 0;
			this.Objectives.Clear();
			base.ResetWave();
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00053E64 File Offset: 0x00052064
		public override void StartPreWaveCompleteDelay()
		{
			foreach (ObjectiveObject objectiveObject in this.Objectives)
			{
				objectiveObject.DestroyObject();
			}
			this.Objectives.Clear();
			base.StartPreWaveCompleteDelay();
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00053EC8 File Offset: 0x000520C8
		public override WaveCompletionState UpdateWaveRunning(float elapsed)
		{
			this.SpawnRescue();
			if (this.RescuedCount >= this.TotalToRescue)
			{
				return WaveCompletionState.Completed;
			}
			return base.UpdateWaveRunning(elapsed);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00053EE8 File Offset: 0x000520E8
		private void SpawnRescue()
		{
			if (Global.WaveMaster.CarePackageTiles.Count == 0)
			{
				return;
			}
			if (this.active < 4)
			{
				int index = Global.rand.Next(0, Global.WaveMaster.CarePackageTiles.Count);
				Vector3 pos = new Vector3((float)Global.WaveMaster.CarePackageTiles[index].x + 0.5f, Global.RandomFloat(15f, 20f), (float)Global.WaveMaster.CarePackageTiles[index].y + 0.5f);
				if (Global.WaveMaster.CarePackageTiles[index].HasObjective)
				{
					return;
				}
				ObjectiveObject objectiveObject = new ObjectiveObject(this.anim, pos, 40f, new FailedRescueZombie());
				Global.MasterCache.CreateObject(objectiveObject);
				this.Objectives.Add(objectiveObject);
				this.active++;
			}
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00053FD3 File Offset: 0x000521D3
		public override int GetHUDActual()
		{
			return this.RescuedCount;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00053FDB File Offset: 0x000521DB
		public override int GetHUDTotal()
		{
			return this.TotalToRescue;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00053FE3 File Offset: 0x000521E3
		public override ObjectiveHUD GetHUD()
		{
			return new ObjectiveHUD(string.Format("Rescue {0} survivers!", this.TotalToRescue), 51, 20, WaveBase.HUDLocation, Global.Player.HUDColor, Color.LimeGreen);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00054018 File Offset: 0x00052218
		public override void CompletedAnObjective()
		{
			foreach (Player player in Global.PlayerList)
			{
				float amount = Global.GetMoneyGained(50f) * player.ZombieKillMoneyMod * (float)Global.WaveMaster.WaveNumber;
				Global.MasterCache.particleSystem.AddPaticle2D(new Part2D_Money(player.Position, amount, true, player));
			}
			this.RescuedCount++;
			this.active--;
			base.CompletedAnObjective();
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x000540C0 File Offset: 0x000522C0
		public override void FailedAnObjective(GameObject obj)
		{
			this.Objectives.Remove(obj as ObjectiveObject);
			this.active--;
		}

		// Token: 0x04000AD5 RID: 2773
		private List<Point> anim = new List<Point>();

		// Token: 0x04000AD6 RID: 2774
		public int RescuedCount;

		// Token: 0x04000AD7 RID: 2775
		public int TotalToRescue = 5;

		// Token: 0x04000AD8 RID: 2776
		private int active;
	}
}
