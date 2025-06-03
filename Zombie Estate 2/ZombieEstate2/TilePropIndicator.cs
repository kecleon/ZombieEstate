using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x0200006E RID: 110
	internal class TilePropIndicator : GameObject
	{
		// Token: 0x06000296 RID: 662 RVA: 0x000146F0 File Offset: 0x000128F0
		public TilePropIndicator(TilePropertyType type)
		{
			this.type = type;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00014700 File Offset: 0x00012900
		public void ActivateIndicator(Tile tile, int num)
		{
			Vector3 pos = new Vector3((float)tile.x + 0.5f, ((float)num + 0.2f) / 4f, (float)tile.y + 0.5f);
			Point texCoord = new Point(0, 0);
			this.XRotation = -1.5707964f;
			this.scale = 0.2f;
			this.AffectedByGravity = false;
			switch (this.type)
			{
			case TilePropertyType.NoPath:
				texCoord = new Point(62, 63);
				break;
			case TilePropertyType.CarePackageSpawn:
				texCoord = new Point(4, 46);
				break;
			case TilePropertyType.ShopKeep:
				texCoord = new Point(4, 4);
				break;
			case TilePropertyType.PlayerOneSpawn:
				texCoord = new Point(0, 8);
				break;
			case TilePropertyType.PlayerTwoSpawn:
				texCoord = new Point(1, 8);
				break;
			case TilePropertyType.PlayerThreeSpawn:
				texCoord = new Point(2, 8);
				break;
			case TilePropertyType.PlayerFourSpawn:
				texCoord = new Point(3, 8);
				break;
			case TilePropertyType.MainOnTop:
				texCoord = new Point(0, 47);
				break;
			case TilePropertyType.NoSpawn:
				texCoord = new Point(61, 63);
				break;
			case TilePropertyType.BossArea:
				texCoord = new Point(66, 62);
				break;
			case TilePropertyType.DemonFire:
				texCoord = new Point(11, 29);
				break;
			}
			base.ActivateObject(pos, texCoord);
		}

		// Token: 0x04000293 RID: 659
		private TilePropertyType type;
	}
}
