using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x020000AA RID: 170
	public class StatusEffect
	{
		// Token: 0x06000458 RID: 1112 RVA: 0x0001FF78 File Offset: 0x0001E178
		public void Update(float elapsed)
		{
			if (this.Type == "None")
			{
				return;
			}
			if (this.Effect != null)
			{
				this.Effect.Position = this.Parent.Position - this.actualOffset;
			}
			Shootable shootable = this.Parent as Shootable;
			if (this.Parent is Player)
			{
				(this.Parent as Player).SlowDown(this.SpeedMod);
			}
			float tickTime = this.TickTime;
			if (this.timer.Expired())
			{
				this.Clear();
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00020010 File Offset: 0x0001E210
		public void Init(Bullet bul, GameObject parent, Gun gun)
		{
			this.timer = new Timer(this.Time);
			this.timer.Start();
			this.attacker = parent;
			this.gun = gun;
			if (this.VisibleEffect)
			{
				this.Effect = new GameObject();
				this.Effect.scale = 0.5f;
				this.Effect.AffectedByGravity = false;
				if (!this.RotOnBulDir || bul == null)
				{
					this.Effect.YRotation = 0f;
				}
				else
				{
					this.Effect.YRotation = bul.YRotation;
				}
				this.Effect.scale = this.Scale;
				if (this.Flat)
				{
					this.Effect.XRotation = 1.5707964f;
				}
				this.actualOffset = Vector3.Transform(-this.OffsetPosition, Matrix.CreateRotationY(this.Effect.YRotation));
				this.Effect.Position += this.actualOffset;
				this.Effect.UpdateTransform();
				this.Effect.ActivateObject(this.Parent.Position, this.EffectTexCoord[0]);
				Global.MasterCache.CreateObject(this.Effect);
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00020150 File Offset: 0x0001E350
		public void Clear()
		{
			if (this.Effect != null)
			{
				this.Effect.DestroyObject();
				this.Effect = null;
			}
			this.Type = "None";
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00020178 File Offset: 0x0001E378
		private void Particles()
		{
			if (this.ParticleName == "None")
			{
				return;
			}
			if (this.ParticleName == "Fire")
			{
				Vector3 randomPosition = this.GetRandomPosition();
				Global.MasterCache.CreateParticle(ParticleType.Fire, randomPosition, this.Parent.Velocity);
				return;
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000201CC File Offset: 0x0001E3CC
		private Vector3 GetRandomPosition()
		{
			Vector3 result = new Vector3(this.Parent.Position.X, this.Parent.Position.Y, this.Parent.Position.Z);
			result.X += Global.RandomFloat(0f, 0.5f) - 0.25f;
			result.Y += Global.RandomFloat(0f, 0.4f);
			result.Z += Global.RandomFloat(0f, 0.5f) - 0.25f;
			result.X += 0.1f;
			return result;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0002027B File Offset: 0x0001E47B
		public void ResetTimer()
		{
			this.timer.Reset();
			this.timer.Start();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00020294 File Offset: 0x0001E494
		public StatusEffect Clone(GameObject parent, Bullet bul, GameObject attacker, Gun gun)
		{
			StatusEffect statusEffect = new StatusEffect();
			statusEffect.Time = this.Time;
			statusEffect.Parent = parent;
			statusEffect.ParticleName = this.ParticleName;
			statusEffect.SpeedMod = this.SpeedMod;
			statusEffect.OffsetPosition = this.OffsetPosition;
			statusEffect.Gore = this.Gore;
			statusEffect.Flat = this.Flat;
			statusEffect.EffectTexCoord = this.EffectTexCoord;
			statusEffect.VisibleEffect = this.VisibleEffect;
			statusEffect.Type = this.Type;
			statusEffect.TickTime = this.TickTime;
			statusEffect.Stackable = this.Stackable;
			statusEffect.DamageOverTime = this.DamageOverTime;
			statusEffect.RotOnBulDir = this.RotOnBulDir;
			statusEffect.Scale = this.Scale;
			statusEffect.Init(bul, attacker, gun);
			statusEffect.DamageDoneMod = this.DamageDoneMod;
			return statusEffect;
		}

		// Token: 0x04000438 RID: 1080
		public string Type = "None";

		// Token: 0x04000439 RID: 1081
		[XmlIgnore]
		public GameObject Parent;

		// Token: 0x0400043A RID: 1082
		[XmlIgnore]
		public GameObject Effect;

		// Token: 0x0400043B RID: 1083
		[XmlIgnore]
		public GameObject attacker;

		// Token: 0x0400043C RID: 1084
		[XmlIgnore]
		public Gun gun;

		// Token: 0x0400043D RID: 1085
		public bool VisibleEffect;

		// Token: 0x0400043E RID: 1086
		public int DamageOverTime;

		// Token: 0x0400043F RID: 1087
		public float TickTime;

		// Token: 0x04000440 RID: 1088
		public Vector3 OffsetPosition = new Vector3(0f, 0f, 0f);

		// Token: 0x04000441 RID: 1089
		public bool Stackable;

		// Token: 0x04000442 RID: 1090
		public string ParticleName = "None";

		// Token: 0x04000443 RID: 1091
		public float SpeedMod = 1f;

		// Token: 0x04000444 RID: 1092
		public Point[] EffectTexCoord = new Point[]
		{
			new Point(0, 0),
			new Point(0, 0),
			new Point(0, 0),
			new Point(0, 0)
		};

		// Token: 0x04000445 RID: 1093
		public bool Flat;

		// Token: 0x04000446 RID: 1094
		public bool Gore = true;

		// Token: 0x04000447 RID: 1095
		public float Time = 1f;

		// Token: 0x04000448 RID: 1096
		public float Scale = 0.4f;

		// Token: 0x04000449 RID: 1097
		public bool RotOnBulDir;

		// Token: 0x0400044A RID: 1098
		public Vector3 actualOffset;

		// Token: 0x0400044B RID: 1099
		private float currentTickTime;

		// Token: 0x0400044C RID: 1100
		private Timer timer;

		// Token: 0x0400044D RID: 1101
		public float DamageDoneMod;
	}
}
