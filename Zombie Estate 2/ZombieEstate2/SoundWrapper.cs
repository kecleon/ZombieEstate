using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ZombieEstate2
{
	// Token: 0x020000A8 RID: 168
	public class SoundWrapper
	{
		// Token: 0x0600044E RID: 1102 RVA: 0x0001FCF0 File Offset: 0x0001DEF0
		public SoundWrapper(string fileName)
		{
			this.Name = fileName;
			string assetName = "Sounds\\" + fileName;
			this.SoundEffect = Global.Content.Load<SoundEffect>(assetName);
			this.listener = new AudioListener();
			this.emitter = new AudioEmitter();
			if (SoundWrapper.Instances == null)
			{
				SoundWrapper.Instances = new List<SoundEffectInstance>();
				for (int i = 0; i < SoundWrapper.COUNT; i++)
				{
					SoundWrapper.Instances.Add(null);
				}
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000026B9 File Offset: 0x000008B9
		public SoundWrapper()
		{
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001FD69 File Offset: 0x0001DF69
		public void PlaySound(Vector3 emitPos)
		{
			this.PlaySound(emitPos, 0f, 0f);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001FD7C File Offset: 0x0001DF7C
		public void PlaySound(Vector3 emitPos, float low, float high)
		{
			this.PlaySound(emitPos, low, high, 1f);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0001FD8C File Offset: 0x0001DF8C
		public void PlaySound(Vector3 emitPos, float low, float high, float volMod)
		{
			float num = 0f;
			if (low != 0f || high != 0f)
			{
				num = Global.RandomFloat(low, high);
			}
			num *= Global.SpeedMod;
			SoundEffectInstance nextSound = this.GetNextSound();
			nextSound.Volume = 0.25f * ((float)Config.Instance.SfxVolume / 100f) * volMod;
			num = MathHelper.Clamp(num, -1f, 1f);
			nextSound.Pitch = num;
			this.emitter.Position = emitPos;
			this.listener.Position = Global.CameraPosition;
			nextSound.Play();
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0001FE1E File Offset: 0x0001E01E
		public void PlaySound(Vector3 emitPos, float volMod)
		{
			this.PlaySound(emitPos, 0f, 0f, volMod);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0001FE32 File Offset: 0x0001E032
		private static void Next()
		{
			SoundWrapper.current++;
			if (SoundWrapper.current >= SoundWrapper.COUNT)
			{
				SoundWrapper.current = 0;
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001FE54 File Offset: 0x0001E054
		private SoundEffectInstance GetNextSound()
		{
			SoundWrapper.Next();
			if (SoundWrapper.Instances[SoundWrapper.current] != null)
			{
				SoundWrapper.Instances[SoundWrapper.current].Dispose();
			}
			SoundWrapper.Instances[SoundWrapper.current] = this.SoundEffect.CreateInstance();
			return SoundWrapper.Instances[SoundWrapper.current];
		}

		// Token: 0x04000429 RID: 1065
		private SoundEffect SoundEffect;

		// Token: 0x0400042A RID: 1066
		private static List<SoundEffectInstance> Instances;

		// Token: 0x0400042B RID: 1067
		private AudioListener listener;

		// Token: 0x0400042C RID: 1068
		private AudioEmitter emitter;

		// Token: 0x0400042D RID: 1069
		private static int current = 0;

		// Token: 0x0400042E RID: 1070
		private static int COUNT = 280;

		// Token: 0x0400042F RID: 1071
		public string Name;
	}
}
