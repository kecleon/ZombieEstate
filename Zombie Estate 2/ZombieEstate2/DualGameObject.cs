using System;
using Microsoft.Xna.Framework;

namespace ZombieEstate2
{
	// Token: 0x02000112 RID: 274
	public class DualGameObject : GameObject
	{
		// Token: 0x06000767 RID: 1895 RVA: 0x0003A6E8 File Offset: 0x000388E8
		public DualGameObject(Point vertTexCoord, Point horizTexCoord, Vector3 pos, bool DONTCREATE)
		{
			if (DONTCREATE)
			{
				return;
			}
			this.secondObject = new GameObject();
			if (!Global.MasterCache.CreateObject(this.secondObject))
			{
				this.DestroyObject();
				return;
			}
			this.secondObject.TextureCoord = horizTexCoord;
			this.TextureCoord = vertTexCoord;
			this.secondObject.XRotation = -1.5707964f;
			this.Active = true;
			this.secondObject.Active = true;
			this.Position = pos;
			this.secondObject.Position = pos;
			this.secondObject.secondaryObject = true;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0003A77C File Offset: 0x0003897C
		public void InitSecondary(Point vertTexCoord, Point horizTexCoord, Vector3 pos)
		{
			if (this.secondObject != null && this.secondObject.Active)
			{
				this.secondObject.DestroyObject();
			}
			this.secondObject = new GameObject();
			if (!Global.MasterCache.CreateObject(this.secondObject))
			{
				this.DestroyObject();
				return;
			}
			this.secondObject.TextureCoord = horizTexCoord;
			this.TextureCoord = vertTexCoord;
			this.secondObject.XRotation = -1.5707964f;
			this.Position = pos;
			this.secondObject.Position = pos;
			this.secondObject.secondaryObject = true;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0003A80F File Offset: 0x00038A0F
		public override void Update(float elapsed)
		{
			base.Update(elapsed);
			this.secondObject.scale = this.scale;
			this.secondObject.Position = this.Position;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0003A83A File Offset: 0x00038A3A
		public void SetHorizantleTextureCoord(Point texCoord)
		{
			this.secondObject.TextureCoord = texCoord;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0003A848 File Offset: 0x00038A48
		public void SetHorizantleRotation(float YRot)
		{
			this.secondObject.YRotation = YRot;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0003A856 File Offset: 0x00038A56
		public override void DestroyObject()
		{
			this.Active = false;
			if (this.secondObject != null)
			{
				this.secondObject.DestroyObject();
			}
			base.DestroyObject();
		}

		// Token: 0x04000777 RID: 1911
		public GameObject secondObject;
	}
}
