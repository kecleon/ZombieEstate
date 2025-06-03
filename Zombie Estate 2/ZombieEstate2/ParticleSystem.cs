using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x0200011C RID: 284
	public class ParticleSystem
	{
		// Token: 0x060007BD RID: 1981 RVA: 0x0003FC54 File Offset: 0x0003DE54
		public ParticleSystem(MasterCache master)
		{
			this.objList = master.gameObjectCaches[0].gameObjects;
			this.startIndex = this.objList.Count - this.partCount;
			this.particles = new Queue<GameObject>();
			this.particles2D = new List<Particle2D>();
			this.combat = new Queue<CombatText>();
			for (int i = this.startIndex; i < this.startIndex + this.partCount; i++)
			{
				this.objList[i] = new Particle(ParticleType.None, new Vector3(0f, 0f, 0f));
				this.objList[i].DestroyObject();
				this.particles.Enqueue(this.objList[i]);
			}
			for (int j = 0; j < 50; j++)
			{
				this.combat.Enqueue(new CombatText());
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0003FD4C File Offset: 0x0003DF4C
		public void Update(float elapsed)
		{
			for (int i = 0; i < this.particles2D.Count; i++)
			{
				this.particles2D[i].Update(elapsed);
				if (this.particles2D[i].DEAD)
				{
					this.particles2D.RemoveAt(i);
				}
			}
			foreach (CombatText combatText in this.combat)
			{
				if (!combatText.DEAD)
				{
					combatText.Update(elapsed);
				}
			}
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0003FDF0 File Offset: 0x0003DFF0
		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < this.particles2D.Count; i++)
			{
				this.particles2D[i].Draw(spriteBatch);
			}
			foreach (CombatText combatText in this.combat)
			{
				if (!combatText.DEAD)
				{
					combatText.Draw(spriteBatch);
				}
			}
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0003FE74 File Offset: 0x0003E074
		public void AddParticle(ParticleType type, Vector3 pos, Vector3 vel)
		{
			if (type == ParticleType.None)
			{
				return;
			}
			Particle particle = (Particle)this.particles.Dequeue();
			particle.SetUp(type, pos, vel);
			this.particles.Enqueue(particle);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0003FEAC File Offset: 0x0003E0AC
		public void AddPaticle2D(string type, Vector3 pos)
		{
			Particle2D item = new Particle2D(pos, type);
			this.particles2D.Add(item);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0003FECD File Offset: 0x0003E0CD
		public void AddPaticle2D(Particle2D part)
		{
			this.particles2D.Add(part);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0003FEDC File Offset: 0x0003E0DC
		public void AddCombatText(CombatTextType type, string text, Vector3 pos)
		{
			CombatText combatText = this.combat.Dequeue();
			combatText.Setup(type, text, pos);
			this.combat.Enqueue(combatText);
		}

		// Token: 0x04000874 RID: 2164
		private int partCount = 250;

		// Token: 0x04000875 RID: 2165
		private int startIndex;

		// Token: 0x04000876 RID: 2166
		private Queue<GameObject> particles;

		// Token: 0x04000877 RID: 2167
		private List<GameObject> objList;

		// Token: 0x04000878 RID: 2168
		private List<Particle2D> particles2D;

		// Token: 0x04000879 RID: 2169
		private Queue<CombatText> combat;
	}
}
