using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2
{
	// Token: 0x02000016 RID: 22
	internal class Camera
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003DAC File Offset: 0x00001FAC
		public static bool ZoomedOut
		{
			get
			{
				return Camera.mDefZoom >= MathHelper.Lerp(Camera.ORIGZoom, Camera.MAXZoom, 0.175f);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003DCC File Offset: 0x00001FCC
		public static void UpdateCamera(float elapsed)
		{
			if (Camera.mWinAnim)
			{
				Camera.UpdateWin(elapsed);
				return;
			}
			if (Camera.test)
			{
				Camera.UpdateMovingCam(elapsed);
				return;
			}
			if (Camera.CINE)
			{
				Camera.UpdateCineCamera(elapsed);
				return;
			}
			if (Camera.shaking)
			{
				Global.CameraPosition += Camera.shakeDir;
				Global.CameraLookAt += Camera.shakeDir;
				if (Camera.shakeTimer.Expired())
				{
					Camera.shaking = false;
					Camera.shakeTimer = null;
				}
				if (Camera.shakeChangeDir.Expired())
				{
					Camera.ChangeShakeDir();
				}
			}
			if (Global.PlayerList == null || Global.Player == null)
			{
				return;
			}
			Vector3 vector;
			Vector3 value = vector = Camera.AverageLocalPlayerPos();
			float num = Camera.LongestDistance();
			Camera.mDefZoom = MathHelper.Lerp(Camera.ORIGZoom, Camera.MAXZoom, num / 15f - 0.25f);
			Camera.mDefZoom = MathHelper.Clamp(Camera.mDefZoom, Camera.ORIGZoom, Camera.MAXZoom);
			value.X += 0.1f;
			value.Y = 9.01f * Camera.mZoom;
			value.Z += 9.01f * Camera.mZoom + Camera.zOffset;
			Global.CameraPosition = Vector3.Lerp(Global.CameraPosition, value, Camera.LerpSpeed * elapsed);
			Global.CameraLookAt = Vector3.Lerp(Global.CameraLookAt, new Vector3(vector.X, 0f, vector.Z + Camera.zOffset), Camera.LerpSpeed * elapsed);
			if (Global.Slowed && Camera.slowTimer != null && Camera.slowTimer.Expired())
			{
				Global.Slowed = false;
				Camera.slowTimer = null;
			}
			if (Camera.ZoomTimer != null && Camera.ZoomTimer.Expired())
			{
				Camera.mZoom = Camera.mDefZoom;
				Camera.ZoomTimer = null;
				return;
			}
			Camera.mZoom = MathHelper.Lerp(Camera.mZoom, Camera.mDefZoom, 0.025f * elapsed);
			Camera.LerpSpeed = MathHelper.Lerp(Camera.LerpSpeed, Camera.DefLerpSpeed, 0.59999996f * elapsed);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003FB9 File Offset: 0x000021B9
		public static void SlowTime(float seconds)
		{
			Camera.SlowTime(seconds, 0.16666667f);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003FC8 File Offset: 0x000021C8
		public static void SetPos()
		{
			Vector3 vector;
			Vector3 cameraPosition = vector = Camera.AverageLocalPlayerPos();
			if (Global.BossActive)
			{
				cameraPosition = Global.Boss.Position + Global.Boss.CameraOffset;
				vector = Global.Boss.Position;
				Camera.mZoom = 1f;
			}
			cameraPosition.X += 0.1f;
			cameraPosition.Y = 9.01f * Camera.mZoom;
			cameraPosition.Z += 9.01f * Camera.mZoom + Camera.zOffset;
			Global.CameraPosition = cameraPosition;
			Global.CameraLookAt = new Vector3(vector.X, 0f, vector.Z + Camera.zOffset);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004078 File Offset: 0x00002278
		public static void SlowTime(float seconds, float amount)
		{
			Camera.slowTimer = new Timer(seconds);
			Camera.slowTimer.IndependentOfTime = true;
			Camera.slowTimer.Start();
			Global.Slowed = true;
			Global.SpeedMod = amount;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000040A6 File Offset: 0x000022A6
		public static void ShakeCamera(float seconds, float intensity)
		{
			if (!Config.Instance.ScreenShakeEnabled)
			{
				return;
			}
			Camera.shakeTimer = new Timer(seconds);
			Camera.shakeTimer.IndependentOfTime = true;
			Camera.shakeTimer.Start();
			Camera.shakeIntensity = intensity;
			Camera.ChangeShakeDir();
			Camera.shaking = true;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000040E8 File Offset: 0x000022E8
		private static void ChangeShakeDir()
		{
			Camera.shakeDir = default(Vector3);
			Camera.shakeDir.X = Global.RandomFloat(0f, Camera.shakeIntensity) - Camera.shakeIntensity / 2f;
			Camera.shakeDir.Y = Global.RandomFloat(0f, Camera.shakeIntensity) - Camera.shakeIntensity / 2f;
			Camera.shakeDir.Z = Global.RandomFloat(0f, Camera.shakeIntensity) - Camera.shakeIntensity / 2f;
			if (Camera.shakeChangeDir != null)
			{
				Camera.shakeChangeDir.Stop();
				Camera.shakeChangeDir = null;
			}
			Camera.shakeChangeDir = new Timer(0.025f);
			Camera.shakeChangeDir.IndependentOfTime = true;
			Camera.shakeChangeDir.Start();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000041AA File Offset: 0x000023AA
		public static void ZoomOut(float seconds, float amount)
		{
			Camera.LerpSpeed = 0.48000002f;
			Camera.ZoomTimer = new Timer(seconds);
			Camera.ZoomTimer.IndependentOfTime = true;
			Camera.ZoomTimer.Start();
			Camera.mZoom = amount;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000041DC File Offset: 0x000023DC
		private static void UpdateCineCamera(float elapsed)
		{
			if (Camera.CineMovingToPosition)
			{
				Global.CameraPosition = Camera.CineStartPos + Camera.CineMoveLerp * Camera.CineMoveTimer.Delta();
				Global.CameraLookAt = Camera.CineStartLookAt + Camera.CineLookAtLerp * Camera.CineMoveTimer.Delta();
				if (Camera.CineMoveTimer.Expired())
				{
					Camera.CineMoveTimer.Reset();
					Camera.CineMovingToPosition = false;
				}
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004254 File Offset: 0x00002454
		public static void MoveTo(Vector3 pos, Vector3 lookAt, float seconds)
		{
			if (seconds == 0f)
			{
				Global.CameraPosition = pos;
				Global.CameraLookAt = lookAt;
				return;
			}
			Camera.CineStartPos = Global.CameraPosition;
			Camera.CineMoveTimer = new Timer(seconds);
			Camera.CineMoveTimer.Start();
			Camera.CineMovingToPosition = true;
			Camera.CineMoveToPosition = pos;
			Camera.CineLookAtPosition = lookAt;
			Camera.CineStartLookAt = Global.CameraLookAt;
			Camera.CineMoveLerp = pos - Camera.CineStartPos;
			Camera.CineLookAtLerp = lookAt - Camera.CineStartLookAt;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000042D1 File Offset: 0x000024D1
		public static void StartCine()
		{
			Camera.CINE = true;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000042D9 File Offset: 0x000024D9
		public static void EndCine()
		{
			Camera.CINE = false;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000042E1 File Offset: 0x000024E1
		public static bool CineMoving()
		{
			return Camera.CineMovingToPosition;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000042E8 File Offset: 0x000024E8
		private static void UpdateMovingCam(float elapsed)
		{
			Global.Paused = true;
			Vector3 cameraPosition = Global.CameraPosition;
			Vector3 cameraLookAt = Global.CameraLookAt;
			float num = 6f * elapsed;
			if (InputManager.UpHeld(0))
			{
				cameraPosition.Z -= num;
				cameraLookAt.Z -= num;
			}
			if (InputManager.DownHeld(0))
			{
				cameraPosition.Z += num;
				cameraLookAt.Z += num;
			}
			if (InputManager.LeftHeld(0))
			{
				cameraPosition.X -= num;
				cameraLookAt.X -= num;
			}
			if (InputManager.RightHeld(0))
			{
				cameraPosition.X += num;
				cameraLookAt.X += num;
			}
			if (InputManager.ButtonHeld(Keys.Q, 0))
			{
				cameraPosition.Y -= num;
			}
			if (InputManager.ButtonHeld(Keys.E, 0))
			{
				cameraPosition.Y += num;
			}
			Global.CameraPosition = cameraPosition;
			Global.CameraLookAt = cameraLookAt;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000043C8 File Offset: 0x000025C8
		public static Vector3 AveragePlayerPos()
		{
			bool flag = Global.AllPlayersDead();
			int num = Global.PlayerList.Count;
			Vector3 vector = new Vector3(0f, 0f, 0f);
			foreach (Player player in Global.PlayerList)
			{
				if (player.DEAD && !flag)
				{
					num--;
				}
				else
				{
					vector += player.Position;
				}
			}
			if (Global.AllPlayersDead())
			{
				num = Global.PlayerList.Count;
				foreach (Player player2 in Global.PlayerList)
				{
					vector += player2.Position;
				}
			}
			vector.Y = 0f;
			vector.X /= (float)num;
			vector.Z /= (float)num;
			return vector;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000044E4 File Offset: 0x000026E4
		public static Vector3 AverageLocalPlayerPos()
		{
			if (Global.AllLocalPlayersDead())
			{
				return Camera.AveragePlayerPos();
			}
			int num = Global.PlayerList.Count;
			Vector3 vector = new Vector3(0f, 0f, 0f);
			if (Global.AllPlayersDead())
			{
				num = Global.PlayerList.Count;
				using (List<Player>.Enumerator enumerator = Global.PlayerList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Player player = enumerator.Current;
						vector += player.Position;
					}
					goto IL_D0;
				}
			}
			foreach (Player player2 in Global.PlayerList)
			{
				if (player2.DEAD || !player2.IAmOwnedByLocalPlayer)
				{
					num--;
				}
				else
				{
					vector += player2.Position;
				}
			}
			IL_D0:
			vector.Y = 0f;
			vector.X /= (float)num;
			vector.Z /= (float)num;
			return vector;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000460C File Offset: 0x0000280C
		private static float LongestDistance()
		{
			Vector3 value = Camera.AverageLocalPlayerPos();
			float num = 0f;
			foreach (Player player in Global.PlayerList)
			{
				if (!player.DEAD && player.IAmOwnedByLocalPlayer)
				{
					float num2 = Vector3.Distance(player.Position, value);
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000468C File Offset: 0x0000288C
		public static void InitWinAnim()
		{
			Camera.mWinAnim = true;
			Camera.mAnimLerp = 0f;
			Vector3 vector = new Vector3(0f, 0f, 0f);
			for (int i = 0; i < 4; i++)
			{
				vector += GameManager.level.PlayerSpawns[i];
			}
			vector /= 4f;
			Camera.mCenterSpawn = vector;
			Camera.mTargetLookAtLocation = vector;
			Camera.mStartLookAtLocation = vector + new Vector3(-10f, 10f, 0f);
			Camera.mTargetLocation = Camera.mTargetLookAtLocation + new Vector3(1f, 3f, 5f);
			Camera.mStartLocation = Camera.mStartLookAtLocation + new Vector3(1f, 3f, 5f);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004760 File Offset: 0x00002960
		public static void UpdateWin(float elapsed)
		{
			Global.CameraPosition = Vector3.SmoothStep(Camera.mStartLocation, Camera.mTargetLocation, Camera.mAnimLerp);
			Global.CameraLookAt = Vector3.SmoothStep(Camera.mStartLookAtLocation, Camera.mTargetLookAtLocation, Camera.mAnimLerp);
			if (Camera.mAnimLerp < 1f)
			{
				Camera.mAnimLerp += elapsed * 0.1f;
			}
			else
			{
				Camera.mAnimLerp = 1f;
			}
			foreach (Player player in Global.PlayerList)
			{
				if (player.Index < 4)
				{
					player.Position.X = GameManager.level.PlayerSpawns[player.Index].X;
					player.Position.Z = GameManager.level.PlayerSpawns[player.Index].Z;
					player.FaceDown();
				}
			}
			Camera.mConfettiSpawnTimer += elapsed;
			if (Camera.mConfettiSpawnTimer >= 0.016666668f)
			{
				for (int i = 0; i < 1; i++)
				{
					float num = (float)(Global.rand.NextDouble() * 3.141592653589793 * 2.0);
					float num2 = (float)Math.Sqrt(Global.rand.NextDouble()) * 6f;
					Vector3 pos = new Vector3((float)Math.Cos((double)num) * num2, 0f, (float)Math.Sin((double)num) * num2) + Camera.mCenterSpawn;
					pos.Y = (float)Global.rand.Next(1, 10);
					Global.MasterCache.CreateParticle(ParticleType.Confetti2, pos, new Vector3(0f, -1f, 0f));
				}
				Camera.mConfettiSpawnTimer = 0f;
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004934 File Offset: 0x00002B34
		public static void RESET_ANIM()
		{
			Camera.mWinAnim = false;
		}

		// Token: 0x0400003B RID: 59
		private static Timer slowTimer;

		// Token: 0x0400003C RID: 60
		private static Timer shakeTimer;

		// Token: 0x0400003D RID: 61
		private static Timer shakeChangeDir;

		// Token: 0x0400003E RID: 62
		public static Vector3 shakeDir;

		// Token: 0x0400003F RID: 63
		private static bool shaking;

		// Token: 0x04000040 RID: 64
		private static float shakeIntensity;

		// Token: 0x04000041 RID: 65
		private static float mZoom = 0.75f;

		// Token: 0x04000042 RID: 66
		private static float mDefZoom = 0.75f;

		// Token: 0x04000043 RID: 67
		private static float ORIGZoom = 0.75f;

		// Token: 0x04000044 RID: 68
		private static float MAXZoom = 1.25f;

		// Token: 0x04000045 RID: 69
		private static float zOffset = 2f;

		// Token: 0x04000046 RID: 70
		private static float LerpSpeed = 3f;

		// Token: 0x04000047 RID: 71
		private static float DefLerpSpeed = 3f;

		// Token: 0x04000048 RID: 72
		private static Timer ZoomTimer;

		// Token: 0x04000049 RID: 73
		private static bool CINE = false;

		// Token: 0x0400004A RID: 74
		private static bool CineMovingToPosition = false;

		// Token: 0x0400004B RID: 75
		private static Vector3 CineMoveToPosition;

		// Token: 0x0400004C RID: 76
		private static Vector3 CineLookAtPosition;

		// Token: 0x0400004D RID: 77
		private static Timer CineMoveTimer;

		// Token: 0x0400004E RID: 78
		private static Vector3 CineStartPos;

		// Token: 0x0400004F RID: 79
		private static Vector3 CineMoveLerp;

		// Token: 0x04000050 RID: 80
		private static Vector3 CineStartLookAt;

		// Token: 0x04000051 RID: 81
		private static Vector3 CineLookAtLerp;

		// Token: 0x04000052 RID: 82
		public static bool test = false;

		// Token: 0x04000053 RID: 83
		private static Vector3 mTargetLookAtLocation;

		// Token: 0x04000054 RID: 84
		private static Vector3 mStartLookAtLocation;

		// Token: 0x04000055 RID: 85
		private static Vector3 mTargetLocation;

		// Token: 0x04000056 RID: 86
		private static Vector3 mStartLocation;

		// Token: 0x04000057 RID: 87
		private static float mAnimLerp = 0f;

		// Token: 0x04000058 RID: 88
		private static bool mWinAnim = false;

		// Token: 0x04000059 RID: 89
		private static Vector3 mCenterSpawn;

		// Token: 0x0400005A RID: 90
		private static float mConfettiSpawnTimer = 0f;
	}
}
