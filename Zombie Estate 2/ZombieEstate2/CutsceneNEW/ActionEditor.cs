using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieEstate2.CutsceneNEW
{
	// Token: 0x020001E1 RID: 481
	internal class ActionEditor
	{
		// Token: 0x06000CCA RID: 3274 RVA: 0x00068C98 File Offset: 0x00066E98
		public ActionEditor(Action parent)
		{
			this.picker = new TexturePicker(Global.Game, Global.MasterTexture);
			this.picker.SetPoint(parent.TextureCoord);
			this.action = parent;
			this.InitButtons();
			if (this.action.Walking)
			{
				this.WalkButton.SetValue("Walk On");
			}
			else
			{
				this.WalkButton.SetValue("Walk Off");
			}
			this.ParticleType.SetValue(this.action.SpawnParticleType.ToString());
			this.SpawnCount.SetValue(this.action.SpawnCount.ToString());
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00068D4C File Offset: 0x00066F4C
		public void Update(MouseState prev)
		{
			this.picker.Update();
			this.action.TextureCoord = this.picker.GetPoint();
			this.WalkButton.Update(prev);
			this.ParticleType.Update(prev);
			this.SpawnCount.Update(prev);
			if (this.WalkButton.CurrentValue() == "Walk On")
			{
				this.action.Walking = true;
			}
			else
			{
				this.action.Walking = false;
			}
			this.action.SpawnParticleType = (ParticleType)Enum.Parse(typeof(ParticleType), this.ParticleType.CurrentValue());
			this.action.SpawnCount = int.Parse(this.SpawnCount.CurrentValue());
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00068E14 File Offset: 0x00067014
		public void Draw(SpriteBatch spriteBatch)
		{
			this.picker.Draw(spriteBatch);
			this.WalkButton.Draw(spriteBatch);
			this.ParticleType.Draw(spriteBatch);
			this.SpawnCount.Draw(spriteBatch);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00068E48 File Offset: 0x00067048
		private void InitButtons()
		{
			List<string> list = new List<string>();
			list.Add("Walk Off");
			list.Add("Walk On");
			this.WalkButton = new Button(new Vector2(1152f, 320f), list);
			list.Clear();
			foreach (string item in Enum.GetNames(typeof(ParticleType)))
			{
				list.Add(item);
			}
			this.ParticleType = new Button(new Vector2(1152f, 128f), list);
			list.Clear();
			for (int j = 1; j < 11; j++)
			{
				list.Add(j.ToString());
			}
			this.SpawnCount = new Button(new Vector2(1152f, 164f), list);
			list.Clear();
		}

		// Token: 0x04000D88 RID: 3464
		private TexturePicker picker;

		// Token: 0x04000D89 RID: 3465
		private Action action;

		// Token: 0x04000D8A RID: 3466
		private Button WalkButton;

		// Token: 0x04000D8B RID: 3467
		private Button ParticleType;

		// Token: 0x04000D8C RID: 3468
		private Button SpawnCount;
	}
}
