

// Hmi.Helper.PenExtensions

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Media;
namespace XamlHelper
{
	public static class PenExtensions
	{
		public static System.Drawing.Pen ToGdiPlus(this System.Windows.Media.Pen pen, Rect bounds, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			SolidColorBrush solidColorBrush = pen.Brush as SolidColorBrush;
			System.Drawing.Pen pen2 = (solidColorBrush == null) ? new System.Drawing.Pen(pen.Brush.ToGdiPlus(bounds, ScaleX, ScaleY, OffsetX, OffsetY), (float)pen.Thickness) : new System.Drawing.Pen(solidColorBrush.Color.ToGdiPlus(solidColorBrush.Opacity), (float)pen.Thickness);
			pen2.LineJoin = LineJoin.Round;
			pen2.MiterLimit = (float)pen.MiterLimit;
			pen2.StartCap = LineCap.Round;
			pen2.EndCap = LineCap.Round;
			System.Windows.Media.DashStyle dashStyle = pen.DashStyle;
			if (dashStyle != DashStyles.Solid)
			{
				List<float> list = new List<float>();
				int num = (pen.DashCap != 0) ? (-1) : 0;
				for (int i = 0; i < dashStyle.Dashes.Count % 2 + 1; i++)
				{
					foreach (double dash in dashStyle.Dashes)
					{
						list.Add((float)dash + (float)(num *= -1));
					}
				}
				bool flag = true;
				int num2 = 0;
				while (num2 < list.Count)
				{
					if (list[num2] == 0f)
					{
						list.RemoveAt(num2);
						if (num2 == 0)
						{
							flag = !flag;
							continue;
						}
						if (num2 > list.Count - 1)
						{
							break;
						}
						List<float> list2 = list;
						int index = num2 - 1;
						list2[index] += list[num2];
						list.RemoveAt(num2);
					}
					else
					{
						num2++;
					}
				}
				if (list.Count < 2)
				{
					if (flag)
					{
						return pen2;
					}
					return new System.Drawing.Pen(System.Drawing.Color.FromArgb(0, 0, 0, 0), (float)pen.Thickness);
				}
				if (!flag)
				{
					float num3 = list[0];
					list.RemoveAt(0);
					list.Add(num3);
					dashStyle.Offset -= num3;
				}
				pen2.DashPattern = list.ToArray();
				pen2.DashOffset = (float)dashStyle.Offset;
				if (pen.DashCap == PenLineCap.Square)
				{
					pen2.DashOffset += 0.5f;
				}
				pen2.DashCap = pen.DashCap.ToDashCap();
			}
			return pen2;
		}

		public static LineJoin ToGdiPlus(this PenLineJoin me)
		{
			switch (me)
			{
				case PenLineJoin.Bevel:
					return LineJoin.Bevel;
				case PenLineJoin.Round:
					return LineJoin.Round;
				default:
					return LineJoin.Miter;
			}
		}

		public static LineCap ToGdiPlus(this PenLineCap me)
		{
			switch (me)
			{
				case PenLineCap.Square:
					return LineCap.Square;
				case PenLineCap.Round:
					return LineCap.Round;
				case PenLineCap.Triangle:
					return LineCap.Triangle;
				default:
					return LineCap.Flat;
			}
		}

		public static DashCap ToDashCap(this PenLineCap me)
		{
			switch (me)
			{
				case PenLineCap.Round:
					return DashCap.Round;
				case PenLineCap.Triangle:
					return DashCap.Triangle;
				default:
					return DashCap.Flat;
			}
		}
	}
}