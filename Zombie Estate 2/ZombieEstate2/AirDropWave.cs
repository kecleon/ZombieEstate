using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZombieEstate2.Wave;

namespace ZombieEstate2
{
	// Token: 0x020000F8 RID: 248
	internal class AirDropWave : TimedWave
	{
		// Token: 0x06000680 RID: 1664 RVA: 0x00030879 File Offset: 0x0002EA79
		public AirDropWave()
		{
			this.Tip = new WaveTip("AirDropWave");
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x000308B3 File Offset: 0x0002EAB3
		public override void StartPreWaveDelay()
		{
			Camera.ZoomOut(10f, 2f);
			this.CutsceneDelay.Reset();
			this.CutsceneDelay.Start();
			base.StartPreWaveDelay();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x000308E0 File Offset: 0x0002EAE0
		public override bool UpdatePreWaveDelay(float elapsed)
		{
			if (this.CutsceneDelay.Expired())
			{
				this.CutsceneDelay.Reset();
				this.SpawnAirDrop();
				this.SpawnAirDrop();
				this.SpawnAirDrop();
			}
			return base.UpdatePreWaveDelay(elapsed);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00030913 File Offset: 0x0002EB13
		public override WaveCompletionState UpdateWaveRunning(float elapsed)
		{
			this.SpawnAirDrop();
			if (this.DropsPickedUp >= this.TotalDropsNeeded)
			{
				return WaveCompletionState.Completed;
			}
			return base.UpdateWaveRunning(elapsed);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00030934 File Offset: 0x0002EB34
		public override void CompletedAnObjective()
		{
			foreach (Player player in Global.PlayerList)
			{
				float amount = Global.GetMoneyGained(25f) * player.ZombieKillMoneyMod * (float)Global.WaveMaster.WaveNumber;
				Global.MasterCache.particleSystem.AddPaticle2D(new Part2D_Money(player.Position, amount, true, player));
			}
			this.activeDrops--;
			this.DropsPickedUp++;
			base.CompletedAnObjective();
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000309DC File Offset: 0x0002EBDC
		public override void StartPreWaveCompleteDelay()
		{
			foreach (DroppingAirDrop droppingAirDrop in this.DroppingList)
			{
				droppingAirDrop.DestroyObject();
			}
			this.DroppingList.Clear();
			Global.KillAllAirDrops();
			base.StartPreWaveCompleteDelay();
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00030A44 File Offset: 0x0002EC44
		public override void ResetWave()
		{
			foreach (DroppingAirDrop droppingAirDrop in this.DroppingList)
			{
				droppingAirDrop.DestroyObject();
			}
			Global.KillAllAirDrops();
			this.DropsPickedUp = 0;
			this.activeDrops = 0;
			this.DroppingList.Clear();
			base.ResetWave();
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00030AB8 File Offset: 0x0002ECB8
		private void SpawnAirDrop()
		{
			if (Global.WaveMaster.CarePackageTiles.Count == 0)
			{
				return;
			}
			if (this.activeDrops < 3)
			{
				int index = Global.rand.Next(0, Global.WaveMaster.CarePackageTiles.Count);
				if (Global.WaveMaster.CarePackageTiles[index].HasObjective)
				{
					return;
				}
				DroppingAirDrop item = new DroppingAirDrop(new Vector3((float)Global.WaveMaster.CarePackageTiles[index].x + 0.5f, Global.RandomFloat(15f, 20f), (float)Global.WaveMaster.CarePackageTiles[index].y + 0.5f));
				this.activeDrops++;
				this.DroppingList.Add(item);
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00030B84 File Offset: 0x0002ED84
		public override int GetHUDActual()
		{
			return this.DropsPickedUp;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00030B8C File Offset: 0x0002ED8C
		public override int GetHUDTotal()
		{
			return this.TotalDropsNeeded;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00030B94 File Offset: 0x0002ED94
		public override ObjectiveHUD GetHUD()
		{
			return new ObjectiveHUD(string.Format("Pick up {0} Air Drops!", this.TotalDropsNeeded), 5, 46, WaveBase.HUDLocation, Global.Player.HUDColor, Color.CornflowerBlue);
		}

		// Token: 0x0400065F RID: 1631
		private Timer CutsceneDelay = new Timer(0.2f);

		// Token: 0x04000660 RID: 1632
		private int activeDrops;

		// Token: 0x04000661 RID: 1633
		public int TotalDropsNeeded = 5;

		// Token: 0x04000662 RID: 1634
		public int DropsPickedUp;

		// Token: 0x04000663 RID: 1635
		public List<DroppingAirDrop> DroppingList = new List<DroppingAirDrop>();
	}
}
