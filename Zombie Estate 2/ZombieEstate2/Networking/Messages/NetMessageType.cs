using System;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001B0 RID: 432
	public enum NetMessageType : byte
	{
		// Token: 0x04000C6B RID: 3179
		NULL,
		// Token: 0x04000C6C RID: 3180
		Ping,
		// Token: 0x04000C6D RID: 3181
		Pong,
		// Token: 0x04000C6E RID: 3182
		Data,
		// Token: 0x04000C6F RID: 3183
		WeaponList,
		// Token: 0x04000C70 RID: 3184
		WeaponFired,
		// Token: 0x04000C71 RID: 3185
		WaveStateUpdate,
		// Token: 0x04000C72 RID: 3186
		PlayerWaveReadyUpdate,
		// Token: 0x04000C73 RID: 3187
		SpawnZombie,
		// Token: 0x04000C74 RID: 3188
		ZombieKilled,
		// Token: 0x04000C75 RID: 3189
		BulletDestroyed,
		// Token: 0x04000C76 RID: 3190
		ZombieAttack,
		// Token: 0x04000C77 RID: 3191
		PlayerJump,
		// Token: 0x04000C78 RID: 3192
		PlayerWaveStats,
		// Token: 0x04000C79 RID: 3193
		DropPickedUp,
		// Token: 0x04000C7A RID: 3194
		PlayerReloaded,
		// Token: 0x04000C7B RID: 3195
		PlayerHUDUpdate,
		// Token: 0x04000C7C RID: 3196
		SpawnEffect,
		// Token: 0x04000C7D RID: 3197
		ChatMsg,
		// Token: 0x04000C7E RID: 3198
		PlayerLeft,
		// Token: 0x04000C7F RID: 3199
		Dance
	}
}
