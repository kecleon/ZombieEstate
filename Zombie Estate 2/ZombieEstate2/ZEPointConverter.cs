using System;
using System.ComponentModel;
using System.Globalization;

namespace ZombieEstate2
{
	// Token: 0x02000139 RID: 313
	public class ZEPointConverter : TypeConverter
	{
		// Token: 0x0600094A RID: 2378 RVA: 0x0004AC50 File Offset: 0x00048E50
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0004AC6E File Offset: 0x00048E6E
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0004AC8C File Offset: 0x00048E8C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			object result;
			try
			{
				if (value is string)
				{
					string[] array = (value as string).Split(new char[]
					{
						','
					});
					result = new ZEPoint
					{
						X = int.Parse(array[0]),
						Y = int.Parse(array[1])
					};
				}
				else
				{
					result = base.ConvertFrom(context, culture, value);
				}
			}
			catch
			{
				result = base.ConvertFrom(context, culture, value);
			}
			return result;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0004AD08 File Offset: 0x00048F08
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(destinationType == typeof(string)))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			ZEPoint zepoint = value as ZEPoint;
			if (zepoint == null)
			{
				return "";
			}
			return zepoint.X.ToString() + ", " + zepoint.Y.ToString();
		}
	}
}
