using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZombieEstate2.Wave;

namespace ZombieEstate2
{
	// Token: 0x0200002C RID: 44
	internal class CarePackage : Drop
	{
		// Token: 0x060000FA RID: 250 RVA: 0x00007308 File Offset: 0x00005508
		public CarePackage(WaveBase wave, Vector3 pos, int yTex)
		{
			CarePackage.CarePackages.Add(this);
			this.Duration = 300f;
			this.parentWave = wave;
			pos.Y = 0.5f;
			this.TextureCoord.X = 4;
			this.TextureCoord.Y = yTex;
			this.scale = 0.5f;
			this.ActivateObject(pos, this.TextureCoord);
			Global.MasterCache.CreateObject(this);
			this.Drag = false;
			base.GiveIndicator();
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00007390 File Offset: 0x00005590
		public override bool GiveDrop(Player player)
		{
			if (!Global.WaveMaster.WaveActive)
			{
				return false;
			}
			Global.WaveMaster.CurrentWave.CompletedAnObjective();
			CarePackage.CarePackages.Remove(this);
			for (int i = 0; i < 6; i++)
			{
				Global.MasterCache.CreateParticle(ParticleType.AmmoPickup, this.Position, Vector3.Zero);
			}
			Global.Level.GetTileAtLocation(this.Position).HasObjective = false;
			return base.GiveDrop(player);
		}

		// Token: 0x040000A7 RID: 167
		private static int BaseXP = 10;

		// Token: 0x040000A8 RID: 168
		private WaveBase parentWave;

		// Token: 0x040000A9 RID: 169
		public static List<CarePackage> CarePackages = new List<CarePackage>();
	}
}
