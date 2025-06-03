using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x020000D5 RID: 213
	internal class StoreKeep : GameObject
	{
		// Token: 0x06000590 RID: 1424 RVA: 0x0002A65C File Offset: 0x0002885C
		public StoreKeep(Vector3 pos)
		{
			this.ActivateObject(pos, new Point(4, 4));
			Global.MasterCache.CreateObject(this);
			this.scale = 0.4f;
			this.AffectedByGravity = false;
			this.Position.Y = 0.35f;
			this.mInd = new GameObject();
			this.mInd.ActivateObject(new Vector3(pos.X, pos.Y + 1.4f, pos.Z), new Point(6, 38));
			this.mInd.scale = 0.3f;
			this.mInd.AffectedByGravity = false;
			this.mInd.TexScale = 2f;
			Global.MasterCache.CreateObject(this.mInd);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0002A724 File Offset: 0x00028924
		public override void Update(float elapsed)
		{
			if (Global.Paused || MasterStore.Active)
			{
				return;
			}
			if (Global.WaveMaster.WaveActive || Global.WaveMaster.PreWaveActive)
			{
				this.TextureCoord.X = 63;
				this.TextureCoord.Y = 63;
				this.mInd.TextureCoord.X = 63;
				this.mInd.TextureCoord.Y = 63;
				return;
			}
			this.TextureCoord = new Point(4, 4);
			bool flag = false;
			bool flag2 = false;
			foreach (Player player in Global.PlayerList)
			{
				if (player.HUMAN && player.IAmOwnedByLocalPlayer && VerchickMath.WithinDistance(base.TwoDPosition(), player.TwoDPosition(), 1.5f))
				{
					if (player.PlayerInfo != null && !player.PlayerInfo.UsingController)
					{
						flag2 = true;
					}
					flag = true;
					if (InputManager.ButtonPressed(ButtonPress.OpenStore, player.Index, false))
					{
						this.mInd.TextureCoord.X = 63;
						this.mInd.TextureCoord.Y = 63;
						MasterStore.Activate();
						player.mMovement = Vector2.Zero;
					}
				}
			}
			if (flag)
			{
				this.mInd.Invisible = false;
				if (flag2)
				{
					this.mInd.TextureCoord.X = 8;
					this.mInd.TextureCoord.Y = 38;
				}
				else
				{
					this.mInd.TextureCoord.X = 6;
					this.mInd.TextureCoord.Y = 38;
				}
			}
			else
			{
				this.mInd.TextureCoord.X = 63;
				this.mInd.TextureCoord.Y = 63;
				this.mInd.Invisible = true;
			}
			base.Update(elapsed);
		}

		// Token: 0x0400058C RID: 1420
		private static Texture2D SpaceControls;

		// Token: 0x0400058D RID: 1421
		private GameObject mInd;
	}
}
