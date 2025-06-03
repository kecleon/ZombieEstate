using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000130 RID: 304
	public struct TileInfo
	{
		// Token: 0x04000948 RID: 2376
		public bool leftWall;

		// Token: 0x04000949 RID: 2377
		public bool rightWall;

		// Token: 0x0400094A RID: 2378
		public bool topWall;

		// Token: 0x0400094B RID: 2379
		public bool bottomWall;

		// Token: 0x0400094C RID: 2380
		public bool floor;

		// Token: 0x0400094D RID: 2381
		public bool leftWallFake;

		// Token: 0x0400094E RID: 2382
		public bool rightWallFake;

		// Token: 0x0400094F RID: 2383
		public bool topWallFake;

		// Token: 0x04000950 RID: 2384
		public bool bottomWallFake;

		// Token: 0x04000951 RID: 2385
		public bool leftWallHalf;

		// Token: 0x04000952 RID: 2386
		public bool rightWallHalf;

		// Token: 0x04000953 RID: 2387
		public bool topWallHalf;

		// Token: 0x04000954 RID: 2388
		public bool bottomWallHalf;

		// Token: 0x04000955 RID: 2389
		public bool leftWallWindow;

		// Token: 0x04000956 RID: 2390
		public bool rightWallWindow;

		// Token: 0x04000957 RID: 2391
		public bool topWallWindow;

		// Token: 0x04000958 RID: 2392
		public bool bottomWallWindow;

		// Token: 0x04000959 RID: 2393
		public Point leftWallTex;

		// Token: 0x0400095A RID: 2394
		public Point rightWallTex;

		// Token: 0x0400095B RID: 2395
		public Point topWallTex;

		// Token: 0x0400095C RID: 2396
		public Point bottomWallTex;

		// Token: 0x0400095D RID: 2397
		public Point floorTex;

		// Token: 0x0400095E RID: 2398
		public List<TilePropertyType> tileProps;
	}
}
