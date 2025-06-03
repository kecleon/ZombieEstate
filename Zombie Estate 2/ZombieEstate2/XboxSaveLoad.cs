using System;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;

namespace ZombieEstate2
{
	// Token: 0x02000050 RID: 80
	internal class XboxSaveLoad
	{
		// Token: 0x060001DB RID: 475 RVA: 0x0000D5BC File Offset: 0x0000B7BC
		public XboxSaveLoad(Player player, bool load, SaveLoadCompleted completedDel, GamerData stats)
		{
			this.fileName = player.GamerName + "_Data.xml";
			this.Stats = stats;
			this.Completed = completedDel;
			this.loading = load;
			this.GetIndex(player);
			if (load)
			{
				this.State = XboxSaveLoad.SavingState.ReadyToLoad;
			}
			else
			{
				this.State = XboxSaveLoad.SavingState.ReadyToSave;
			}
			this.thread = new Thread(new ThreadStart(this.SaveThread));
			this.thread.Start();
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000D638 File Offset: 0x0000B838
		private void GetIndex(Player player)
		{
			switch (player.Index)
			{
			case 0:
				this.index = PlayerIndex.One;
				return;
			case 1:
				this.index = PlayerIndex.Two;
				return;
			case 2:
				this.index = PlayerIndex.Three;
				return;
			case 3:
				this.index = PlayerIndex.Four;
				return;
			default:
				return;
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000D684 File Offset: 0x0000B884
		private void SaveThread()
		{
			bool flag = false;
			while (!flag)
			{
				switch (this.State)
				{
				case XboxSaveLoad.SavingState.ReadyToSelectStorageDevice:
					this.asyncResult = StorageDevice.BeginShowSelector(this.index, null, null);
					this.State = XboxSaveLoad.SavingState.SelectingStorageDevice;
					continue;
				case XboxSaveLoad.SavingState.SelectingStorageDevice:
					if (this.asyncResult.IsCompleted)
					{
						XboxSaveLoad.StorageDevice = StorageDevice.EndShowSelector(this.asyncResult);
						this.State = XboxSaveLoad.SavingState.ReadyToOpenStorageContainer;
						continue;
					}
					continue;
				case XboxSaveLoad.SavingState.ReadyToOpenStorageContainer:
					if (XboxSaveLoad.StorageDevice == null || !XboxSaveLoad.StorageDevice.IsConnected)
					{
						this.State = XboxSaveLoad.SavingState.ReadyToSelectStorageDevice;
						continue;
					}
					this.asyncResult = XboxSaveLoad.StorageDevice.BeginOpenContainer("Zombie Estate 2 _ Player " + this.index.ToString(), null, null);
					this.State = XboxSaveLoad.SavingState.OpeningStorageContainer;
					continue;
				case XboxSaveLoad.SavingState.OpeningStorageContainer:
					if (!this.asyncResult.IsCompleted)
					{
						continue;
					}
					this.StoreContainer = XboxSaveLoad.StorageDevice.EndOpenContainer(this.asyncResult);
					if (this.loading)
					{
						this.State = XboxSaveLoad.SavingState.ReadyToSave;
						continue;
					}
					this.State = XboxSaveLoad.SavingState.ReadyToLoad;
					continue;
				case XboxSaveLoad.SavingState.ReadyToSave:
					if (this.StoreContainer == null)
					{
						this.State = XboxSaveLoad.SavingState.ReadyToOpenStorageContainer;
						continue;
					}
					try
					{
						this.DeleteExisting();
						this.Save();
						if (this.Completed != null)
						{
							this.Completed("OK", this.Stats);
						}
						continue;
					}
					catch (IOException ex)
					{
						Terminal.WriteMessage("ERROR SAVING!!!!! " + ex.Message, MessageType.ERROR);
						if (this.Completed != null)
						{
							this.Completed("ERROR", this.Stats);
						}
						continue;
					}
					finally
					{
						this.StoreContainer.Dispose();
						this.StoreContainer = null;
						this.State = XboxSaveLoad.SavingState.Nothing;
						flag = true;
					}
					break;
				case XboxSaveLoad.SavingState.ReadyToLoad:
					break;
				default:
					continue;
				}
				if (this.StoreContainer == null)
				{
					this.State = XboxSaveLoad.SavingState.ReadyToOpenStorageContainer;
				}
				else
				{
					try
					{
						this.Load();
						if (this.Completed != null)
						{
							this.Completed("OK", this.Stats);
						}
					}
					catch (IOException ex2)
					{
						if (ex2 is FileNotFoundException)
						{
							Terminal.WriteMessage("File not found." + ex2.Message, MessageType.ERROR);
							if (this.Completed != null)
							{
								this.Completed("FILE NOT FOUND", this.Stats);
							}
						}
						else
						{
							Terminal.WriteMessage("ERROR LOADING!!!!! " + ex2.Message, MessageType.ERROR);
							if (this.Completed != null)
							{
								this.Completed("ERROR", this.Stats);
							}
						}
					}
					finally
					{
						this.StoreContainer.Dispose();
						this.StoreContainer = null;
						this.State = XboxSaveLoad.SavingState.Nothing;
						flag = true;
					}
				}
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000D950 File Offset: 0x0000BB50
		private void DeleteExisting()
		{
			if (this.StoreContainer.FileExists(this.fileName))
			{
				this.StoreContainer.DeleteFile(this.fileName);
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000D978 File Offset: 0x0000BB78
		private void Save()
		{
			using (Stream stream = this.StoreContainer.CreateFile(this.fileName))
			{
				new XmlSerializer(typeof(GamerData)).Serialize(stream, this.Stats);
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000D9D0 File Offset: 0x0000BBD0
		private void Load()
		{
			using (Stream stream = this.StoreContainer.OpenFile(this.fileName, FileMode.Open))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(GamerData));
				this.Stats = (GamerData)xmlSerializer.Deserialize(stream);
			}
		}

		// Token: 0x04000153 RID: 339
		private string fileName;

		// Token: 0x04000154 RID: 340
		private XboxSaveLoad.SavingState State;

		// Token: 0x04000155 RID: 341
		private Thread thread;

		// Token: 0x04000156 RID: 342
		private SaveLoadCompleted Completed;

		// Token: 0x04000157 RID: 343
		private StorageContainer StoreContainer;

		// Token: 0x04000158 RID: 344
		private static StorageDevice StorageDevice;

		// Token: 0x04000159 RID: 345
		private IAsyncResult asyncResult;

		// Token: 0x0400015A RID: 346
		private bool loading;

		// Token: 0x0400015B RID: 347
		private PlayerIndex index;

		// Token: 0x0400015C RID: 348
		private GamerData Stats;

		// Token: 0x0200020B RID: 523
		private enum SavingState
		{
			// Token: 0x04000E05 RID: 3589
			Nothing,
			// Token: 0x04000E06 RID: 3590
			ReadyToSelectStorageDevice,
			// Token: 0x04000E07 RID: 3591
			SelectingStorageDevice,
			// Token: 0x04000E08 RID: 3592
			ReadyToOpenStorageContainer,
			// Token: 0x04000E09 RID: 3593
			OpeningStorageContainer,
			// Token: 0x04000E0A RID: 3594
			ReadyToSave,
			// Token: 0x04000E0B RID: 3595
			ReadyToLoad
		}
	}
}
