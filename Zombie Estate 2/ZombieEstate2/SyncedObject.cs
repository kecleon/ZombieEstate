using System;
using ZombieEstate2.Networking;
using ZombieEstate2.Networking.Messages;

namespace ZombieEstate2
{
	// Token: 0x02000078 RID: 120
	public class SyncedObject
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00017158 File Offset: 0x00015358
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00017160 File Offset: 0x00015360
		public ushort UID
		{
			get
			{
				return this.mUID;
			}
			set
			{
				if (this.mUIDSet)
				{
					NetworkManager.RemoveNetObject(this);
				}
				if (value == 65535)
				{
					return;
				}
				this.SetUID(value);
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00017180 File Offset: 0x00015380
		private void SetUID(ushort uid)
		{
			this.mUID = uid;
			this.mUIDSet = true;
			NetworkManager.AddNetObject(this);
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x00017196 File Offset: 0x00015396
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0001719E File Offset: 0x0001539E
		public int OwnerIndex
		{
			get
			{
				return this.mOwner;
			}
			set
			{
				this.mOwner = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x000171A7 File Offset: 0x000153A7
		public bool IAmOwnedByLocalPlayer
		{
			get
			{
				return PlayerManager.IsIndexLocal(this.OwnerIndex, this);
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000171BA File Offset: 0x000153BA
		public SyncedObject()
		{
			this.mUID = ushort.MaxValue;
			this.SetSyncTimes();
			this.RandomizeSyncTimes();
		}

		// Token: 0x060002EB RID: 747 RVA: 0x000171FA File Offset: 0x000153FA
		public SyncedObject(ushort uid)
		{
			this.SetUID(uid);
			this.SetSyncTimes();
			this.RandomizeSyncTimes();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00017238 File Offset: 0x00015438
		public void UpdateNet(float elapsed)
		{
			if (!this.mDoISync)
			{
				return;
			}
			this.mTimeSinceLastSentSync += elapsed;
			this.mTimeSinceLastReceivedSync += elapsed;
			this.mTimeUntilSync -= elapsed;
			if (this.mTimeUntilSync <= 0f)
			{
				this.Sync();
			}
			this.mTimeUntilFullSync -= elapsed;
			if (this.mTimeUntilFullSync <= 0f)
			{
				this.FullSync();
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000172AC File Offset: 0x000154AC
		protected void Sync()
		{
			NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.Data);
			netMessage.UID = this.mUID;
			this.SendSync(netMessage);
			this.mTimeSinceLastSentSync = 0f;
			NetworkManager.SendMessage(netMessage, SendType.Unreliable);
			this.mTimeUntilSync = this.mTotalSyncTime;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000172F4 File Offset: 0x000154F4
		public void FullSync()
		{
			NetMessage netMessage = NetMessage.GetNetMessage(NetMessageType.Data);
			netMessage.UID = this.mUID;
			this.SendFullSync(netMessage);
			this.mTimeSinceLastSentSync = 0f;
			NetworkManager.SendMessage(netMessage, SendType.ReliableBuffered);
			this.mTimeUntilFullSync = this.mTotalFullSyncTime;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00017339 File Offset: 0x00015539
		public void CLEAR_UID()
		{
			if (this.mDoISync || this.mUIDSet)
			{
				NetworkManager.RemoveNetObject(this);
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00017351 File Offset: 0x00015551
		public void ReceiveSyncMsg(NetPayload incoming)
		{
			this.mTimeSinceLastReceivedSync = 0f;
			this.ReceiveSync(incoming);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00002EF9 File Offset: 0x000010F9
		protected virtual void SendSync(NetMessage msg)
		{
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00002EF9 File Offset: 0x000010F9
		protected virtual void SendFullSync(NetMessage msg)
		{
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00002EF9 File Offset: 0x000010F9
		protected virtual void ReceiveSync(NetPayload incoming)
		{
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00017365 File Offset: 0x00015565
		protected virtual void SetSyncTimes()
		{
			this.mTotalSyncTime = 1f;
			this.mTotalFullSyncTime = 5f;
			this.mDoISync = false;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00017384 File Offset: 0x00015584
		private void RandomizeSyncTimes()
		{
			this.mTimeUntilSync = Global.RandomFloat(0f, this.mTimeUntilSync);
			this.mTimeUntilFullSync = Global.RandomFloat(0f, this.mTotalFullSyncTime);
		}

		// Token: 0x040002C6 RID: 710
		private int mOwner;

		// Token: 0x040002C7 RID: 711
		protected float mTotalSyncTime = 1f;

		// Token: 0x040002C8 RID: 712
		private float mTimeUntilSync;

		// Token: 0x040002C9 RID: 713
		protected float mTotalFullSyncTime = 5f;

		// Token: 0x040002CA RID: 714
		private float mTimeUntilFullSync;

		// Token: 0x040002CB RID: 715
		protected float mTimeSinceLastReceivedSync;

		// Token: 0x040002CC RID: 716
		protected float mTimeSinceLastSentSync;

		// Token: 0x040002CD RID: 717
		private float TIME_INTERVAL = 0.016666668f;

		// Token: 0x040002CE RID: 718
		protected bool mDoISync;

		// Token: 0x040002CF RID: 719
		private ushort mUID;

		// Token: 0x040002D0 RID: 720
		private bool mUIDSet;
	}
}
