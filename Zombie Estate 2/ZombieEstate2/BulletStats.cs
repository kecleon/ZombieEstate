using System;
using System.ComponentModel;
using ProtoBuf;

namespace ZombieEstate2
{
	// Token: 0x02000104 RID: 260
	[ProtoContract]
	public class BulletStats
	{
		// Token: 0x060006D4 RID: 1748 RVA: 0x00033B64 File Offset: 0x00031D64
		public BulletStats()
		{
			this.BehaviorsStrings = new string[4];
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00033B84 File Offset: 0x00031D84
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00033B8C File Offset: 0x00031D8C
		[Category("Particles")]
		[ProtoMember(1)]
		public string DeathParticleType { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00033B95 File Offset: 0x00031D95
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x00033B9D File Offset: 0x00031D9D
		[Category("0")]
		[ProtoMember(2)]
		public string Name { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00033BA6 File Offset: 0x00031DA6
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x00033BAE File Offset: 0x00031DAE
		[Category("Appearance")]
		[ProtoMember(3)]
		public ZEPoint VertTexCoord { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00033BB7 File Offset: 0x00031DB7
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x00033BBF File Offset: 0x00031DBF
		[Category("Appearance")]
		[ProtoMember(4)]
		public ZEPoint HorizTexCoord { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00033BC8 File Offset: 0x00031DC8
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x00033BD0 File Offset: 0x00031DD0
		[Category("Movement")]
		[ProtoMember(5)]
		public float Speed { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00033BDC File Offset: 0x00031DDC
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x00033C14 File Offset: 0x00031E14
		[Category("Movement")]
		[ProtoMember(6)]
		public string[] BehaviorsStrings
		{
			get
			{
				for (int i = 0; i < 4; i++)
				{
					if (this.mBehave[i] == null)
					{
						this.mBehave[i] = "";
					}
				}
				return this.mBehave;
			}
			set
			{
				this.mBehave = value;
				if (this.mBehave.Length != 4)
				{
					string[] array = new string[4];
					int num = 0;
					for (int i = 0; i < this.mBehave.Length; i++)
					{
						if (this.mBehave[i] != null && this.mBehave[i] != "" && this.mBehave[i] != "\n")
						{
							array[num] = this.mBehave[i];
							num++;
						}
					}
					this.mBehave = array;
				}
				for (int j = 0; j < 4; j++)
				{
					if (this.mBehave[j] == null)
					{
						this.mBehave[j] = "";
					}
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00033CBA File Offset: 0x00031EBA
		// (set) Token: 0x060006E2 RID: 1762 RVA: 0x00033CC2 File Offset: 0x00031EC2
		[Category("Life")]
		[ProtoMember(7)]
		public bool PersistOnDeath { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x00033CCB File Offset: 0x00031ECB
		// (set) Token: 0x060006E4 RID: 1764 RVA: 0x00033CD3 File Offset: 0x00031ED3
		[Category("Life")]
		[ProtoMember(8)]
		public float PersistTime { get; set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x00033CDC File Offset: 0x00031EDC
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x00033CE4 File Offset: 0x00031EE4
		[Category("Rotation")]
		[ProtoMember(9)]
		public bool RandomZRotation { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00033CED File Offset: 0x00031EED
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x00033CF5 File Offset: 0x00031EF5
		[Category("Rotation")]
		[ProtoMember(10)]
		public bool RotatesInDirection { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00033CFE File Offset: 0x00031EFE
		// (set) Token: 0x060006EA RID: 1770 RVA: 0x00033D06 File Offset: 0x00031F06
		[Category("Rotation")]
		[ProtoMember(11)]
		public float XRotationOverTime { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x00033D0F File Offset: 0x00031F0F
		// (set) Token: 0x060006EC RID: 1772 RVA: 0x00033D17 File Offset: 0x00031F17
		[Category("Rotation")]
		[ProtoMember(12)]
		public float YRotationOverTime { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00033D20 File Offset: 0x00031F20
		// (set) Token: 0x060006EE RID: 1774 RVA: 0x00033D28 File Offset: 0x00031F28
		[Category("Rotation")]
		[ProtoMember(13)]
		public float ZRotationOverTime { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x00033D31 File Offset: 0x00031F31
		// (set) Token: 0x060006F0 RID: 1776 RVA: 0x00033D39 File Offset: 0x00031F39
		[Category("Movement")]
		[ProtoMember(14)]
		public int RandomSpeedPercent { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x00033D42 File Offset: 0x00031F42
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x00033D4A File Offset: 0x00031F4A
		[Category("Appearance")]
		[ProtoMember(15)]
		public bool RandomFour { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x00033D53 File Offset: 0x00031F53
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x00033D5B File Offset: 0x00031F5B
		[ProtoMember(16)]
		public bool NoGore { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00033D64 File Offset: 0x00031F64
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x00033D6C File Offset: 0x00031F6C
		[Category("Life")]
		[ProtoMember(17)]
		public float LiveTime { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00033D75 File Offset: 0x00031F75
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x00033D7D File Offset: 0x00031F7D
		[Category("Particles")]
		[ProtoMember(18)]
		public string EmitPartType { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00033D86 File Offset: 0x00031F86
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x00033D8E File Offset: 0x00031F8E
		[Category("Particles")]
		[ProtoMember(19)]
		public float EmitPartTime { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x00033D97 File Offset: 0x00031F97
		// (set) Token: 0x060006FC RID: 1788 RVA: 0x00033D9F File Offset: 0x00031F9F
		[Category("Appearance")]
		[ProtoMember(20)]
		public float Scale { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x00033DA8 File Offset: 0x00031FA8
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x00033DB0 File Offset: 0x00031FB0
		[ProtoMember(21)]
		public bool IgnoreEnemies { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x00033DB9 File Offset: 0x00031FB9
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x00033DC1 File Offset: 0x00031FC1
		[ProtoMember(22)]
		public bool HurtsPlayers { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x00033DCA File Offset: 0x00031FCA
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x00033DD2 File Offset: 0x00031FD2
		[Category("Life")]
		[ProtoMember(23)]
		public bool LandOnFloor { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x00033DDB File Offset: 0x00031FDB
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x00033DE3 File Offset: 0x00031FE3
		[Category("Life")]
		[ProtoMember(24)]
		public bool StickToOwner { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x00033DEC File Offset: 0x00031FEC
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x00033DF4 File Offset: 0x00031FF4
		[Category("Life")]
		[ProtoMember(25)]
		public float StickToOwnerXScale { get; set; }

		// Token: 0x040006D8 RID: 1752
		private string[] mBehave = new string[4];
	}
}
