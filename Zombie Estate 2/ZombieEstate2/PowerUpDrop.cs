using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.Buffs;
using ZombieEstate2.Buffs.Types;

namespace ZombieEstate2
{
	// Token: 0x02000022 RID: 34
	internal class PowerUpDrop : Drop
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00006464 File Offset: 0x00004664
		public PowerUpDrop(Vector3 pos, int type)
		{
			this.Duration = 60f;
			switch (type)
			{
			case 0:
				this.mType = PowerUpDrop.PowerUp.Speed;
				break;
			case 1:
				this.mType = PowerUpDrop.PowerUp.Crit;
				break;
			case 2:
				this.mType = PowerUpDrop.PowerUp.Defense;
				break;
			case 3:
				this.mType = PowerUpDrop.PowerUp.Power;
				break;
			}
			switch (this.mType)
			{
			case PowerUpDrop.PowerUp.Speed:
				this.startTex = new Point(67, 41);
				break;
			case PowerUpDrop.PowerUp.Crit:
				this.startTex = new Point(67, 40);
				break;
			case PowerUpDrop.PowerUp.Defense:
				this.startTex = new Point(67, 38);
				break;
			case PowerUpDrop.PowerUp.Power:
				this.startTex = new Point(67, 37);
				break;
			}
			this.TextureCoord = this.startTex;
			pos.Y = 0.35f;
			this.scale = 0.25f;
			this.ActivateObject(pos, this.TextureCoord);
			Global.MasterCache.CreateObject(this);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000655C File Offset: 0x0000475C
		public override bool GiveDrop(Player player)
		{
			SoundEngine.PlaySound("ze2_heal", player.Position, -0.8f, -0.7f, 0.6f);
			Buff buff;
			switch (this.mType)
			{
			case PowerUpDrop.PowerUp.Speed:
				buff = new Buff_MoveQuick();
				goto IL_5C;
			case PowerUpDrop.PowerUp.Crit:
				buff = new Buff_Crit();
				goto IL_5C;
			case PowerUpDrop.PowerUp.Defense:
				buff = new Buff_Guard();
				goto IL_5C;
			}
			buff = new Buff_DmgBoost();
			IL_5C:
			buff.Init(player, null);
			player.BuffManager.AddBuff(buff);
			base.GiveDrop(player);
			return true;
		}

		// Token: 0x04000096 RID: 150
		public static float DURATION = 30f;

		// Token: 0x04000097 RID: 151
		private PowerUpDrop.PowerUp mType;

		// Token: 0x02000202 RID: 514
		private enum PowerUp
		{
			// Token: 0x04000DCE RID: 3534
			Speed,
			// Token: 0x04000DCF RID: 3535
			Crit,
			// Token: 0x04000DD0 RID: 3536
			Defense,
			// Token: 0x04000DD1 RID: 3537
			Power
		}
	}
}
