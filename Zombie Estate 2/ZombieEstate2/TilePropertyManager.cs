using System;

namespace ZombieEstate2
{
	// Token: 0x0200006C RID: 108
	internal class TilePropertyManager
	{
		// Token: 0x06000293 RID: 659 RVA: 0x000145F7 File Offset: 0x000127F7
		public TilePropertyManager(Sector sector)
		{
			this.sector = sector;
			this.editorObjects = new EditorObjects();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00014611 File Offset: 0x00012811
		public void ToggleIndicators()
		{
			this.Shown = !this.Shown;
			if (!this.Shown)
			{
				this.editorObjects.ClearObjects();
				return;
			}
			this.BuildIndicators();
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0001463C File Offset: 0x0001283C
		public void BuildIndicators()
		{
			this.editorObjects.ClearObjects();
			for (int i = 0; i < Sector.width; i++)
			{
				for (int j = 0; j < Sector.height; j++)
				{
					int num = 0;
					foreach (TilePropertyType type in this.sector.GetTile(i, j).TileProperties)
					{
						TilePropIndicator tilePropIndicator = new TilePropIndicator(type);
						tilePropIndicator.ActivateIndicator(this.sector.GetTile(i, j), num);
						this.editorObjects.AddObject(tilePropIndicator);
						num++;
					}
				}
			}
		}

		// Token: 0x04000284 RID: 644
		private Sector sector;

		// Token: 0x04000285 RID: 645
		private EditorObjects editorObjects;

		// Token: 0x04000286 RID: 646
		public bool Shown;
	}
}
