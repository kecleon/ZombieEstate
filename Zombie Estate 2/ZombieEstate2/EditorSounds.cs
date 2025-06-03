using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ZombieEstate2
{
	// Token: 0x02000107 RID: 263
	public static class EditorSounds
	{
		// Token: 0x06000725 RID: 1829 RVA: 0x00036C0B File Offset: 0x00034E0B
		public static void LoadSounds(Game game)
		{
			EditorSounds.DeepClick = game.Content.Load<SoundEffect>("ammo");
		}

		// Token: 0x0400071F RID: 1823
		public static SoundEffect DeepClick;
	}
}
