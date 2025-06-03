using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200001F RID: 31
	public class DroppingAirDrop : GameObject
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00005FA0 File Offset: 0x000041A0
		public DroppingAirDrop(Vector3 pos)
		{
			this.TextureCoord.X = 4;
			this.TextureCoord.Y = Global.rand.Next(45, 48);
			this.Parachute = new GameObject();
			this.Parachute.TextureCoord = new Point(8, 45);
			this.Parachute.scale = 1f;
			this.Parachute.ActivateObject(new Vector3(pos.X, pos.Y + 1f, pos.Z - 0.01f), this.Parachute.TextureCoord);
			this.ActivateObject(pos, this.TextureCoord);
			Global.MasterCache.CreateObject(this);
			Global.MasterCache.CreateObject(this.Parachute);
			this.Parachute.AffectedByGravity = false;
			this.AffectedByGravity = false;
			this.Shadow = new AOE(AOEType.Shadow, 0f, 0.8f, new Vector3(this.Position.X, 0f, this.Position.Z), this.Position.Y / this.fallSpeed, null, null);
			Global.MasterCache.CreateObject(this.Shadow);
			Global.Level.GetTileAtLocation(this.Position).HasObjective = true;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000060F8 File Offset: 0x000042F8
		public override void Update(float elapsed)
		{
			this.Position.Y = this.Position.Y - this.fallSpeed * elapsed;
			this.Parachute.Position.Y = this.Position.Y + 1f;
			if (this.Position.Y <= 0.4f)
			{
				(Global.WaveMaster.CurrentWave as AirDropWave).DroppingList.Remove(this);
				this.DestroyObject();
			}
			base.Update(elapsed);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006178 File Offset: 0x00004378
		public override void DestroyObject()
		{
			Vector3 position = this.Position;
			GameObject gameObject = new CarePackage(Global.WaveMaster.CurrentWave, position, this.TextureCoord.Y);
			this.Parachute.DestroyObject();
			this.Shadow.DestroyObject();
			gameObject.SquishMe(1.4f);
			base.DestroyObject();
		}

		// Token: 0x04000091 RID: 145
		private GameObject Parachute;

		// Token: 0x04000092 RID: 146
		private float fallSpeed = 2f;

		// Token: 0x04000093 RID: 147
		private AOE Shadow;
	}
}
