using System;
using ProtoBuf;

namespace ZombieEstate2.Networking.Messages
{
	// Token: 0x020001B9 RID: 441
	[ProtoContract]
	public class Msg_GunListUpdate : NetPayload
	{
		// Token: 0x06000C3B RID: 3131 RVA: 0x000644C4 File Offset: 0x000626C4
		public void FillIn(Gun[] guns, int selectedIndex)
		{
			this.GunUIDs = new short[Player.MAXGUNS];
			this.GunLvls = new byte[Player.MAXGUNS];
			for (int i = 0; i < Player.MAXGUNS; i++)
			{
				Gun gun = guns[i];
				if (gun != null)
				{
					this.GunUIDs[i] = GunStatsLoader.UID_GunsRev[gun.Name];
					this.GunLvls[i] = (byte)gun.GetLevel();
				}
			}
			this.SelectedIndex = (byte)selectedIndex;
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00064538 File Offset: 0x00062738
		public void Retrieve(Player p)
		{
			for (int i = 0; i < Player.MAXGUNS; i++)
			{
				if (this.GunUIDs[i] == GunStatsLoader.INVALID_UID)
				{
					if (p.Guns[i] != null)
					{
						p.RemoveGun(p.Guns[i].Name);
					}
				}
				else if (p.Guns[i] == null)
				{
					p.Guns[i] = new Gun(p, GunStatsLoader.UID_Guns[this.GunUIDs[i]]);
					p.Guns[i].LevelGunToNum((int)this.GunLvls[i]);
				}
				else if (p.Guns[i].Name == GunStatsLoader.UID_Guns[this.GunUIDs[i]])
				{
					p.Guns[i].LevelGunToNum((int)this.GunLvls[i]);
				}
				else
				{
					p.Guns[i] = new Gun(p, GunStatsLoader.UID_Guns[this.GunUIDs[i]]);
					p.Guns[i].LevelGunToNum((int)this.GunLvls[i]);
				}
			}
			if ((int)this.SelectedIndex >= p.GetGunCount())
			{
				Terminal.WriteMessage(string.Concat(new object[]
				{
					"Weapon selected index from net was higher than count: Index: ",
					this.SelectedIndex,
					" Count: ",
					p.GetGunCount()
				}), MessageType.ERROR);
				p.SwitchGun(p.Guns[0].Name);
				return;
			}
			p.SwitchGun(p.Guns[(int)this.SelectedIndex].Name);
		}

		// Token: 0x04000CA0 RID: 3232
		[ProtoMember(1)]
		public short[] GunUIDs;

		// Token: 0x04000CA1 RID: 3233
		[ProtoMember(2)]
		public byte[] GunLvls;

		// Token: 0x04000CA2 RID: 3234
		[ProtoMember(3)]
		public byte SelectedIndex;

		// Token: 0x04000CA3 RID: 3235
		[ProtoMember(4)]
		public byte PlayerIndex;
	}
}
