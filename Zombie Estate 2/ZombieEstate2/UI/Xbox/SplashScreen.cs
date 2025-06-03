using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace ZombieEstate2.UI.Xbox
{
	// Token: 0x0200015D RID: 349
	internal class SplashScreen : Menu
	{
		// Token: 0x06000A8E RID: 2702 RVA: 0x00056057 File Offset: 0x00054257
		public SplashScreen() : base(false, new Vector2(0f, 200f))
		{
			this.MenuBG = Global.SplashScreen;
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0005607A File Offset: 0x0005427A
		public override void Setup()
		{
			this.DrawBGPixel = false;
			this.title = "Buy Now!";
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00056090 File Offset: 0x00054290
		public override void UpdateMenu()
		{
			for (int i = 0; i < 4; i++)
			{
				if (XboxInputManager.APressed(i))
				{
					this.GotoMarket(i);
					return;
				}
				if (XboxInputManager.BPressed(i))
				{
					this.Cont();
					return;
				}
			}
			base.UpdateMenu();
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x000560D0 File Offset: 0x000542D0
		private void GotoMarket(int i)
		{
			try
			{
				if (!Guide.IsTrialMode)
				{
					this.Cont();
				}
				if (Gamer.SignedInGamers[i].Privileges.AllowPurchaseContent)
				{
					Guide.ShowMarketplace((PlayerIndex)i);
				}
				else
				{
					Guide.BeginShowMessageBox((PlayerIndex)i, "Sign In Required", "You must be logged in a XBox Live account to buy this game.", new string[]
					{
						"Ok"
					}, 0, MessageBoxIcon.Warning, new AsyncCallback(this.confirmwarning), null);
				}
				this.Cont();
			}
			catch (Exception)
			{
				Guide.BeginShowMessageBox((PlayerIndex)i, "Sign In Required", "You must be logged in a XBox Live account to buy this game.", new string[]
				{
					"Ok"
				}, 0, MessageBoxIcon.Warning, new AsyncCallback(this.confirmwarning), null);
			}
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00056180 File Offset: 0x00054380
		private void Cont()
		{
			MenuManager.CLOSEALL();
			Global.Paused = false;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0005618D File Offset: 0x0005438D
		private void confirmwarning(IAsyncResult result)
		{
			Guide.EndShowMessageBox(result);
		}
	}
}
