using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000024 RID: 36
	internal class BigPlant : Zombie
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00006698 File Offset: 0x00004898
		public BigPlant() : base(ZombieType.Plant)
		{
			this.TexScale = 2f;
			this.startingTex = new Point(72, 56);
			this.TextureCoord = new Point(72, 56);
			this.ActivateObject(this.Position, this.TextureCoord);
			base.InitSpeed(0f);
			this.NOLOGIC = true;
			this.NODAMAGE = true;
			this.GibbletChance = 0;
			this.scale = 0.8f;
			this.floorHeight = 0.8f;
			this.Worth = 55f;
			this.SM.AddState(BigPlant.PlantState.Idle, 3f, BigPlant.PlantState.Prepping, 72, 56, false, new UpdateStateDelegate(this.Idle));
			this.SM.AddState(BigPlant.PlantState.Prepping, 1f, BigPlant.PlantState.Fire, 72, 56, false, new UpdateStateDelegate(this.Prep));
			this.SM.AddState(BigPlant.PlantState.Fire, 1f, BigPlant.PlantState.Idle, 72, 58, false, new UpdateStateDelegate(this.Fire));
			this.SM.ActLocal = true;
			if (!Global.NETWORKED)
			{
				this.Rand = Global.rand;
				this.InitiateAllRandomness();
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000067C4 File Offset: 0x000049C4
		protected override void InitiateAllRandomness()
		{
			this.delay = Global.RandomFloat(this.Rand, 1f, 3f);
			Tile tileAtLocation = Global.Level.GetTileAtLocation(new Vector3((float)this.Rand.Next(0, 32), 0f, (float)this.Rand.Next(0, 32)));
			while (tileAtLocation == null || !tileAtLocation.AmIGrass())
			{
				tileAtLocation = Global.Level.GetTileAtLocation(new Vector3((float)this.Rand.Next(0, 32), 0f, (float)this.Rand.Next(0, 32)));
			}
			this.Position.X = (float)tileAtLocation.x + 0.5f;
			this.Position.Z = (float)tileAtLocation.y + 0.5f;
			base.InitiateAllRandomness();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006898 File Offset: 0x00004A98
		public override void Update(float elapsed)
		{
			if (this.delay > 0f)
			{
				this.delay -= elapsed;
			}
			else
			{
				this.SM.Update(elapsed);
			}
			this.Velocity = Vector3.Zero;
			this.TextureCoord = this.SM.CurrentTex;
			base.Update(elapsed);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000068F1 File Offset: 0x00004AF1
		private void Idle(float delta)
		{
			this.fired = false;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000068FA File Offset: 0x00004AFA
		private void Prep(float delta)
		{
			base.SquishMe(1.1f, false);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void Fire(float delta)
		{
		}

		// Token: 0x04000099 RID: 153
		private StateMachineEngine<BigPlant.PlantState> SM = new StateMachineEngine<BigPlant.PlantState>();

		// Token: 0x0400009A RID: 154
		private bool fired;

		// Token: 0x0400009B RID: 155
		private float delay;

		// Token: 0x02000203 RID: 515
		private enum PlantState
		{
			// Token: 0x04000DD3 RID: 3539
			Idle,
			// Token: 0x04000DD4 RID: 3540
			Prepping,
			// Token: 0x04000DD5 RID: 3541
			Fire
		}
	}
}
