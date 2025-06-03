using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieEstate2
{
	// Token: 0x02000135 RID: 309
	internal class VerchickMath
	{
		// Token: 0x060008D2 RID: 2258 RVA: 0x0004A360 File Offset: 0x00048560
		public static bool WithinDistance(Vector2 source, Vector2 target, float distance)
		{
			float num = distance * distance;
			return (target - source).LengthSquared() <= num;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0004A388 File Offset: 0x00048588
		public static Vector2 DirectionToVector2(Vector2 source, Vector2 target)
		{
			Vector2 result = Vector2.Subtract(target, source);
			result.Normalize();
			return result;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0004A3A5 File Offset: 0x000485A5
		public static Vector2 CenterText(SpriteFont font, Vector2 position, string text)
		{
			return new Vector2(position.X - font.MeasureString(text).X / 2f, position.Y - font.MeasureString(text).Y / 2f);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0004A3E0 File Offset: 0x000485E0
		public static string WordWrap(string text, int width)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			string str = "";
			int num = 0;
			foreach (string text2 in text.Split(new char[]
			{
				' '
			}))
			{
				if (text2.Length + num >= width - 1)
				{
					str += "\n";
					num = 0;
				}
				num += text2.Length;
				str = str + text2 + " ";
			}
			return str + "\n";
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0004A469 File Offset: 0x00048669
		public static float AngleFromVector2(Vector2 dir)
		{
			return (float)Math.Atan2((double)dir.Y, (double)dir.X);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0004A480 File Offset: 0x00048680
		public static float AngleFromVectors(Vector2 source, Vector2 target)
		{
			Vector2 vector = VerchickMath.DirectionToVector2(source, target);
			if (vector.X == float.NaN)
			{
				vector = Vector2.Zero;
			}
			return VerchickMath.AngleFromVector2(vector);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0004A4B0 File Offset: 0x000486B0
		public static string WordWrapWidth(string text, int width, SpriteFont font)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			text = text.Replace('\n', ' ');
			string str = "";
			int num = 0;
			foreach (string text2 in text.Split(new char[]
			{
				' '
			}))
			{
				if ((int)font.MeasureString(text2).X + num >= width)
				{
					str += "\n";
					num = 0;
				}
				num += (int)font.MeasureString(text2).X + (int)font.MeasureString(" ").X;
				str = str + text2 + " ";
			}
			return str + "\n";
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0004A564 File Offset: 0x00048764
		public static string WrapTextCHAT(SpriteFont font, string text, float maxLineWidth)
		{
			string[] array = text.Split(new char[]
			{
				' '
			});
			StringBuilder stringBuilder = new StringBuilder();
			float num = 0f;
			float x = font.MeasureString(" ").X;
			foreach (string text2 in array)
			{
				Vector2 vector = font.MeasureString(text2);
				if (num + vector.X < maxLineWidth)
				{
					stringBuilder.Append(text2 + " ");
					num += vector.X + x;
				}
				else if (vector.X > maxLineWidth)
				{
					if (stringBuilder.ToString() == "")
					{
						stringBuilder.Append(VerchickMath.WrapTextCHAT(font, text2.Insert(text2.Length / 2, " ") + " ", maxLineWidth));
					}
					else
					{
						stringBuilder.Append("\n" + VerchickMath.WrapTextCHAT(font, text2.Insert(text2.Length / 2, " ") + " ", maxLineWidth));
					}
				}
				else
				{
					stringBuilder.Append("\n" + text2 + " ");
					num = vector.X + x;
				}
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0004A6AC File Offset: 0x000488AC
		public static Vector3 GetRandomPosition(Vector3 startPos, float maxDistance)
		{
			Vector3 result = new Vector3(startPos.X, startPos.Y, startPos.Z);
			result.X += Global.RandomFloat(0f, maxDistance * 2f) - maxDistance;
			result.Y += Global.RandomFloat(0f, maxDistance * 2f) - maxDistance;
			result.Z += Global.RandomFloat(0f, maxDistance * 2f) - maxDistance;
			return result;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0004A730 File Offset: 0x00048930
		public static Vector3 GetRandomPosition(Random rand, Vector3 startPos, float maxDistance)
		{
			Vector3 result = new Vector3(startPos.X, startPos.Y, startPos.Z);
			result.X += Global.RandomFloat(rand, 0f, maxDistance * 2f) - maxDistance;
			result.Y += Global.RandomFloat(rand, 0f, maxDistance * 2f) - maxDistance;
			result.Z += Global.RandomFloat(rand, 0f, maxDistance * 2f) - maxDistance;
			return result;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0004A7B4 File Offset: 0x000489B4
		public static string AddCommas(int num)
		{
			return string.Format("{0:#,###0}", num);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0004A7C8 File Offset: 0x000489C8
		public static Vector2 GetScreenPosition(Vector3 Position)
		{
			Vector3 vector = Global.GraphicsDevice.Viewport.Project(Vector3.Add(Position, new Vector3(0f, 0f, 0f)), Global.Projection, Global.View, Matrix.Identity);
			return new Vector2(vector.X, vector.Y);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0004A822 File Offset: 0x00048A22
		public static Vector2 AngleToVector(float angle)
		{
			return new Vector2((float)Math.Cos((double)angle), (float)Math.Sin((double)angle));
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0004A469 File Offset: 0x00048669
		public static float VectorToAngle(Vector2 vector)
		{
			return (float)Math.Atan2((double)vector.Y, (double)vector.X);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0004A839 File Offset: 0x00048A39
		public static int Wrap(int i, int max)
		{
			if (i < 0)
			{
				return max - 1;
			}
			if (i >= max)
			{
				return 0;
			}
			return i;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0004A84A File Offset: 0x00048A4A
		public static bool IsMouseInRect(Rectangle rect)
		{
			return rect.Contains(InputManager.GetMousePosition());
		}
	}
}
