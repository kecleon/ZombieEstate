using System;
using Microsoft.Xna.Framework;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x0200002D RID: 45
	public class Drop : GameObject
	{
		// Token: 0x060000FD RID: 253 RVA: 0x0000741C File Offset: 0x0000561C
		public Drop()
		{
			this.AffectedByGravity = false;
			this.scale = 0.25f;
			Global.DropsList.Add(this);
			base.SquishMe(1.5f);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000748C File Offset: 0x0000568C
		public Drop(Player single)
		{
			this.singlePlayer = single;
			this.AffectedByGravity = false;
			this.scale = 0.25f;
			Global.DropsList.Add(this);
			this.colorInd = new GameObject();
			this.colorInd.ActivateObject(this.Position, new Point(6 + single.Index, 33));
			this.colorInd.XRotation = 1.5707964f;
			this.colorInd.AffectedByGravity = false;
			Global.MasterCache.CreateObject(this.colorInd);
			base.SquishMe(1.5f);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00007558 File Offset: 0x00005758
		public override void Update(float elapsed)
		{
			this.elap += elapsed;
			this.totalElapsed += elapsed;
			if (this.totalElapsed >= this.Duration)
			{
				this.DestroyObject();
				return;
			}
			if (this.Pulse && this.elap >= this.speed)
			{
				this.elap = 0f;
				if (this.positive)
				{
					this.TextureCoord.X = this.TextureCoord.X + 1;
				}
				else
				{
					this.TextureCoord.X = this.TextureCoord.X - 1;
				}
				if (this.TextureCoord.X == this.startTex.X + 3)
				{
					this.positive = false;
				}
				if (this.TextureCoord.X == this.startTex.X)
				{
					this.positive = true;
				}
				if (this.totalElapsed > Drop.FlickerTime * this.Duration)
				{
					this.speed = Math.Max(0.05f, this.speed - 0.01f);
				}
			}
			if (this.colorInd != null)
			{
				this.colorInd.Position = this.Position;
			}
			base.UpdateTile();
			this.CheckCollisions(elapsed);
			base.Update(elapsed);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00007684 File Offset: 0x00005884
		public override void ActivateObject(Vector3 pos, Point texCoord)
		{
			this.startTex = texCoord;
			base.ActivateObject(pos, texCoord);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00007698 File Offset: 0x00005898
		private void CheckCollisions(float elapsed)
		{
			if (this.tile == null)
			{
				return;
			}
			foreach (Player player in Global.PlayerList)
			{
				if (this.Check(elapsed, player))
				{
					break;
				}
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000076F8 File Offset: 0x000058F8
		private bool Check(float elapsed, Player player)
		{
			if (this.CanPickUpObject(player))
			{
				float num = Vector2.Distance(base.TwoDPosition(), player.TwoDPosition());
				if (this.Drag && num < Drop.DRAG_DISTANCE * (1f + player.SpecialProperties.MagnetBonus / 100f) && num > 0.4f)
				{
					Vector2 vector = Vector2.Subtract(base.TwoDPosition(), player.TwoDPosition());
					vector.Normalize();
					vector.X *= Math.Max(0.25f, 1f / num * 2f);
					vector.Y *= Math.Max(0.25f, 1f / num * 2f);
					this.Position.X = this.Position.X - vector.X * elapsed;
					this.Position.Z = this.Position.Z - vector.Y * elapsed;
				}
				if (num < 0.5f && this.GiveDrop(player))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00007804 File Offset: 0x00005A04
		public virtual bool GiveDrop(Player player)
		{
			if (player.IAmOwnedByLocalPlayer)
			{
				NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.DropPickedUp);
				netMessage.Payload = new Msg_DropPickedUp
				{
					DropUID = base.UID,
					PickUpperUID = player.UID
				};
				NetworkManager.SendMessage(netMessage, SendType.Reliable);
			}
			player.SendHUDData();
			this.ShowIndicator(player);
			this.DestroyObject();
			return true;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00007860 File Offset: 0x00005A60
		public void ShowIndicator(Player player)
		{
			if (this.Indicator)
			{
				FloatingIndicator obj = new FloatingIndicator(player, this.startTex, 2f);
				Global.MasterCache.CreateObject(obj);
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00007893 File Offset: 0x00005A93
		public override void DestroyObject()
		{
			if (this.colorInd != null)
			{
				this.colorInd.DestroyObject();
			}
			Global.DropsList.Remove(this);
			base.DestroyObject();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000078BA File Offset: 0x00005ABA
		public virtual bool CanPickUpObject(Player player)
		{
			if (this.singlePlayer != null)
			{
				return player.Index == this.singlePlayer.Index;
			}
			return !player.DEAD;
		}

		// Token: 0x040000AA RID: 170
		public Point startTex;

		// Token: 0x040000AB RID: 171
		private bool positive = true;

		// Token: 0x040000AC RID: 172
		private float elap;

		// Token: 0x040000AD RID: 173
		private float speed = 0.2f;

		// Token: 0x040000AE RID: 174
		public float Duration = 20f;

		// Token: 0x040000AF RID: 175
		private float totalElapsed;

		// Token: 0x040000B0 RID: 176
		private static float FlickerTime = 0.8f;

		// Token: 0x040000B1 RID: 177
		public bool Indicator = true;

		// Token: 0x040000B2 RID: 178
		public bool Pulse = true;

		// Token: 0x040000B3 RID: 179
		private Player singlePlayer;

		// Token: 0x040000B4 RID: 180
		private static float DRAG_DISTANCE = 3.5f;

		// Token: 0x040000B5 RID: 181
		private GameObject colorInd;

		// Token: 0x040000B6 RID: 182
		public bool Drag = true;
	}
}
