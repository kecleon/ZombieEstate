using System;
using System.Collections.Generic;
using WG;

namespace ZombieEstate2.Wave
{
	// Token: 0x0200015A RID: 346
	internal class ZombieWrapperGen
	{
		// Token: 0x06000A79 RID: 2681 RVA: 0x0005513C File Offset: 0x0005333C
		public List<ZombieWrapper> GetWrapper()
		{
			this.mWrappers = new List<ZombieWrapper>();
			this.mWrappers.Add(this.PickWrapper(this.mNormWrappers));
			int num = 4;
			if (Global.UnlimitedMode)
			{
				num = this.mMasterList.Keys.Count - 3;
			}
			for (int i = 0; i < num; i++)
			{
				ZombieType key = this.PickType();
				ZombieWrapper zombieWrapper = this.PickWrapper(this.mMasterList[key]);
				zombieWrapper.Start = (i + 1) * 15;
				this.mWrappers.Add(zombieWrapper);
			}
			return this.mWrappers;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x000551CC File Offset: 0x000533CC
		public ZombieWrapperGen(int seed)
		{
			this.mRandom = new Random(seed);
			this.mMasterList = new Dictionary<ZombieType, List<ZombieWrapper>>();
			this.BuildListOfTypes();
			this.mNormWrappers.Add(this.BuildWrapper(ZombieType.NormalZombie, 50, 40, 0, 5));
			this.mNormWrappers.Add(this.BuildWrapper(ZombieType.NormalZombie, 40, 20, 0, 10));
			this.mNormWrappers.Add(this.BuildWrapper(ZombieType.NormalZombie, 60, 30, 0, 5));
			List<ZombieWrapper> list = new List<ZombieWrapper>();
			list.Add(this.BuildWrapper(ZombieType.Skeleton, 20, 10, 0, 5));
			list.Add(this.BuildWrapper(ZombieType.Skeleton, 10, 5, 0, 5));
			list.Add(this.BuildWrapper(ZombieType.Skeleton, 30, 10, 0, 10));
			this.mMasterList.Add(ZombieType.Skeleton, list);
			List<ZombieWrapper> list2 = new List<ZombieWrapper>();
			list2.Add(this.BuildWrapper(ZombieType.Hazmat, 20, 20, 0, 5));
			list2.Add(this.BuildWrapper(ZombieType.Hazmat, 40, 30, 0, 10));
			list2.Add(this.BuildWrapper(ZombieType.Hazmat, 40, 20, 0, 5));
			this.mMasterList.Add(ZombieType.Hazmat, list2);
			List<ZombieWrapper> list3 = new List<ZombieWrapper>();
			list3.Add(this.BuildWrapper(ZombieType.Banshee, 20, 20, 0, 5));
			list3.Add(this.BuildWrapper(ZombieType.Banshee, 40, 30, 0, 10));
			list3.Add(this.BuildWrapper(ZombieType.Banshee, 40, 20, 0, 5));
			this.mMasterList.Add(ZombieType.Banshee, list3);
			List<ZombieWrapper> list4 = new List<ZombieWrapper>();
			list4.Add(this.BuildWrapper(ZombieType.Goliath, 10, 20, 0, 1));
			list4.Add(this.BuildWrapper(ZombieType.Goliath, 20, 10, 0, 1));
			list4.Add(this.BuildWrapper(ZombieType.Goliath, 10, 10, 0, 1));
			this.mMasterList.Add(ZombieType.Goliath, list4);
			List<ZombieWrapper> list5 = new List<ZombieWrapper>();
			list5.Add(this.BuildWrapper(ZombieType.DrZombie, 40, 20, 0, 5));
			list5.Add(this.BuildWrapper(ZombieType.DrZombie, 20, 10, 0, 10));
			list5.Add(this.BuildWrapper(ZombieType.DrZombie, 30, 10, 0, 5));
			this.mMasterList.Add(ZombieType.DrZombie, list5);
			List<ZombieWrapper> list6 = new List<ZombieWrapper>();
			list6.Add(this.BuildWrapper(ZombieType.FireWitch, 20, 10, 0, 5));
			list6.Add(this.BuildWrapper(ZombieType.FireWitch, 20, 10, 0, 10));
			list6.Add(this.BuildWrapper(ZombieType.FireWitch, 10, 10, 0, 5));
			this.mMasterList.Add(ZombieType.FireWitch, list6);
			List<ZombieWrapper> list7 = new List<ZombieWrapper>();
			list7.Add(this.BuildWrapper(ZombieType.IceGolem, 20, 10, 0, 5));
			list7.Add(this.BuildWrapper(ZombieType.IceGolem, 10, 10, 0, 10));
			list7.Add(this.BuildWrapper(ZombieType.IceGolem, 20, 10, 0, 5));
			this.mMasterList.Add(ZombieType.IceGolem, list7);
			List<ZombieWrapper> list8 = new List<ZombieWrapper>();
			list8.Add(this.BuildWrapper(ZombieType.Glooper, 30, 10, 0, 5));
			list8.Add(this.BuildWrapper(ZombieType.Glooper, 20, 20, 0, 10));
			list8.Add(this.BuildWrapper(ZombieType.Glooper, 20, 10, 0, 5));
			this.mMasterList.Add(ZombieType.Glooper, list8);
			List<ZombieWrapper> list9 = new List<ZombieWrapper>();
			list9.Add(this.BuildWrapper(ZombieType.Clown, 20, 10, 0, 5));
			list9.Add(this.BuildWrapper(ZombieType.Clown, 10, 10, 0, 10));
			list9.Add(this.BuildWrapper(ZombieType.Clown, 10, 10, 0, 5));
			this.mMasterList.Add(ZombieType.Clown, list9);
			List<ZombieWrapper> list10 = new List<ZombieWrapper>();
			list10.Add(this.BuildWrapper(ZombieType.BrainZombie, 20, 20, 0, 5));
			list10.Add(this.BuildWrapper(ZombieType.BrainZombie, 20, 10, 0, 10));
			list10.Add(this.BuildWrapper(ZombieType.BrainZombie, 10, 20, 0, 5));
			this.mMasterList.Add(ZombieType.BrainZombie, list10);
			List<ZombieWrapper> list11 = new List<ZombieWrapper>();
			list11.Add(this.BuildWrapper(ZombieType.Gardner, 10, 10, 0, 5));
			list11.Add(this.BuildWrapper(ZombieType.Gardner, 5, 10, 0, 5));
			list11.Add(this.BuildWrapper(ZombieType.Gardner, 10, 10, 0, 5));
			this.mMasterList.Add(ZombieType.Gardner, list11);
			List<ZombieWrapper> list12 = new List<ZombieWrapper>();
			list12.Add(this.BuildWrapper(ZombieType.RobBurglar, 10, 10, 0, 5));
			list12.Add(this.BuildWrapper(ZombieType.RobBurglar, 5, 20, 0, 5));
			list12.Add(this.BuildWrapper(ZombieType.RobBurglar, 10, 10, 0, 5));
			this.mMasterList.Add(ZombieType.RobBurglar, list12);
			List<ZombieWrapper> list13 = new List<ZombieWrapper>();
			list13.Add(this.BuildWrapper(ZombieType.Blob, 30, 10, 0, 5));
			list13.Add(this.BuildWrapper(ZombieType.Blob, 25, 20, 0, 5));
			list13.Add(this.BuildWrapper(ZombieType.Blob, 20, 10, 0, 5));
			this.mMasterList.Add(ZombieType.Blob, list13);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0005567F File Offset: 0x0005387F
		private ZombieWrapper BuildWrapper(ZombieType name, int stand, int vari, int start, int min)
		{
			return new ZombieWrapper
			{
				Name = name.ToString(),
				Standard = stand,
				Start = start,
				Variation = vari,
				Minimum = min
			};
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x000556B8 File Offset: 0x000538B8
		private ZombieWrapper PickWrapper(List<ZombieWrapper> list)
		{
			int index = this.mRandom.Next(0, list.Count);
			return list[index];
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x000556E0 File Offset: 0x000538E0
		private ZombieType PickType()
		{
			int count = this.mAllTypes.Count;
			int index = this.mRandom.Next(0, count);
			ZombieType zombieType = this.mAllTypes[index];
			this.mAllTypes.Remove(zombieType);
			return zombieType;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00055724 File Offset: 0x00053924
		private void BuildListOfTypes()
		{
			this.mAllTypes = new List<ZombieType>();
			this.mAllTypes.Add(ZombieType.Skeleton);
			this.mAllTypes.Add(ZombieType.Hazmat);
			this.mAllTypes.Add(ZombieType.Goliath);
			this.mAllTypes.Add(ZombieType.DrZombie);
			this.mAllTypes.Add(ZombieType.Banshee);
			this.mAllTypes.Add(ZombieType.FireWitch);
			this.mAllTypes.Add(ZombieType.IceGolem);
			this.mAllTypes.Add(ZombieType.Glooper);
			this.mAllTypes.Add(ZombieType.Clown);
			this.mAllTypes.Add(ZombieType.BrainZombie);
			this.mAllTypes.Add(ZombieType.Gardner);
			this.mAllTypes.Add(ZombieType.RobBurglar);
			this.mAllTypes.Add(ZombieType.Blob);
		}

		// Token: 0x04000B2A RID: 2858
		private List<ZombieWrapper> mWrappers;

		// Token: 0x04000B2B RID: 2859
		private Dictionary<ZombieType, List<ZombieWrapper>> mMasterList;

		// Token: 0x04000B2C RID: 2860
		private List<ZombieWrapper> mNormWrappers = new List<ZombieWrapper>();

		// Token: 0x04000B2D RID: 2861
		private List<ZombieType> mAllTypes;

		// Token: 0x04000B2E RID: 2862
		private Random mRandom;
	}
}
