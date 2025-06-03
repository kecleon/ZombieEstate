using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000082 RID: 130
	internal class AbilityTeleport : Ability
	{
		// Token: 0x06000314 RID: 788 RVA: 0x00017EDC File Offset: 0x000160DC
		public AbilityTeleport(Player player, int level) : base(player, level)
		{
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00017EE8 File Offset: 0x000160E8
		public override void SetupCooldownsAndName()
		{
			this.AbilityName = "Teleport";
			this.CooldownType = AbilityCooldownType.Pressed;
			this.SecondsUntilCooldownBegins = 3f;
			this.SecondsToCompleteCooldown = 3f;
			this.MidActivationCooldown = 0.25f;
			this.PercentPerActivation = 0.33f;
			base.SetupCooldownsAndName();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00017F3C File Offset: 0x0001613C
		public override void DoAbility()
		{
			float num = (float)(this.Level + 1) * 1.5f;
			Vector3 vector = default(Vector3);
			vector.X = this.Parent.TwoDPosition().X + this.Parent.mAimDir.X * num;
			vector.Z = this.Parent.TwoDPosition().Y + this.Parent.mAimDir.Y * num;
			vector.Y = this.Parent.Position.Y;
			Tile tileAtLocation = Global.Level.GetTileAtLocation(vector);
			if (tileAtLocation == null)
			{
				this.Failed();
				return;
			}
			if (tileAtLocation.TileProperties.Contains(TilePropertyType.NoPath))
			{
				this.Failed();
				return;
			}
			if (!Global.Level.InLineOfSight(this.Parent.Position, vector))
			{
				this.Failed();
				return;
			}
			Global.MasterCache.CreateParticle(ParticleType.SkwugTele, this.Parent.Position, Vector3.Zero);
			this.Parent.Position = vector;
			Global.MasterCache.CreateParticle(ParticleType.SkwugTele, this.Parent.Position, Vector3.Zero);
			base.DoAbility();
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00002EF9 File Offset: 0x000010F9
		private void Failed()
		{
		}
	}
}
