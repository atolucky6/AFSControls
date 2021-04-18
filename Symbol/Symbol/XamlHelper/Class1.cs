using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xaml;
using Microsoft.VisualBasic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Windows.Media.Converters;
using System.Windows.Media.Effects;
using System.Diagnostics;
using System.Globalization;

namespace vSymbolfactory.XamlHelper
{

	public static class BrushExtensions
	{
		private class BrushTransform
		{
			internal readonly System.Windows.Media.Matrix ToAbsolute;

			internal readonly System.Windows.Media.Matrix ToBrush;

			internal readonly System.Drawing.Brush DegenerateBrush;

			internal BrushTransform(System.Windows.Media.Brush brush, Rect bounds)
			{
				ToAbsolute = System.Windows.Media.Matrix.Identity;
				ToAbsolute.Scale(bounds.Width, bounds.Height);
				ToAbsolute.Translate(bounds.X, bounds.Y);
				System.Windows.Media.Matrix toAbsolute = ToAbsolute;
				toAbsolute.Invert();
				ToBrush = toAbsolute * brush.RelativeTransform.Value * ToAbsolute * brush.Transform.Value;
				if (!ToBrush.HasInverse)
				{
					DrawingVisual drawingVisual = new DrawingVisual();
					using (DrawingContext drawingContext = drawingVisual.RenderOpen())
					{
						drawingContext.DrawRectangle(brush, null, new Rect(0.0, 0.0, 1.0, 1.0));
					}
					RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(1, 1, 0.0, 0.0, PixelFormats.Pbgra32);
					renderTargetBitmap.Render(drawingVisual);
					byte[] array = new byte[4];
					renderTargetBitmap.CopyPixels(array, 4, 0);
					DegenerateBrush = new SolidBrush(System.Drawing.Color.FromArgb(array[3], array[2], array[1], array[0]));
				}
			}
		}

		public static System.Drawing.Brush ToGdiPlus(this System.Windows.Media.Brush brush, Rect bounds, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			SolidColorBrush brush2;
			if ((brush2 = (brush as SolidColorBrush)) != null)
			{
				return brush2.ToGdiPlus(bounds, ScaleX, ScaleY, OffsetX, OffsetY);
			}
			System.Windows.Media.LinearGradientBrush brush3;
			if ((brush3 = (brush as System.Windows.Media.LinearGradientBrush)) != null)
			{
				return brush3.ToGdiPlus(bounds, ScaleX, ScaleY, OffsetX, OffsetY);
			}
			RadialGradientBrush brush4;
			if ((brush4 = (brush as RadialGradientBrush)) != null)
			{
				return brush4.ToGdiPlus(bounds, ScaleX, ScaleY, OffsetX, OffsetY);
			}
			ImageBrush brush5;
			if ((brush5 = (brush as ImageBrush)) != null)
			{
				return brush5.ToGdiPlus(bounds);
			}
			DrawingBrush brush6;
			if ((brush6 = (brush as DrawingBrush)) != null)
			{
				return brush6.ToGdiPlus(bounds);
			}
			VisualBrush brush7;
			if ((brush7 = (brush as VisualBrush)) != null)
			{
				return brush7.ToGdiPlus(bounds);
			}
			throw new ArgumentOutOfRangeException("brush", brush.GetType().ToString());
		}

		public static System.Drawing.Brush ToGdiPlus(this SolidColorBrush brush, Rect bounds, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			return new SolidBrush(brush.Color.ToGdiPlus(brush.Opacity));
		}

		public static System.Drawing.Brush ToGdiPlus(this DrawingBrush brush, Rect bounds)
		{
			Utility.Warning("Ignoring {0} at {1}", brush.GetType(), bounds);
			return new SolidBrush(System.Drawing.Color.FromArgb(0, 255, 255, 255));
		}

		public static System.Drawing.Brush ToGdiPlus(this VisualBrush brush, Rect bounds)
		{
			Utility.Warning("Ignoring {0} at {1}", brush.GetType(), bounds);
			return new SolidBrush(System.Drawing.Color.FromArgb(0, 255, 255, 255));
		}

		public static System.Drawing.Color ToGdiPlus(this System.Windows.Media.Color color, double opacity)
		{
			return System.Drawing.Color.FromArgb((int)Math.Round(opacity * (double)(int)color.A), color.R, color.G, color.B);
		}

		public static WrapMode ToGdiPlus(this GradientSpreadMethod me)
		{
			switch (me)
			{
				case GradientSpreadMethod.Reflect:
					return WrapMode.TileFlipXY;
				case GradientSpreadMethod.Repeat:
					return WrapMode.Tile;
				default:
					return WrapMode.Clamp;
			}
		}

		public static WrapMode ToGdiPlus(this TileMode me)
		{
			switch (me)
			{
				case TileMode.Tile:
					return WrapMode.Tile;
				case TileMode.FlipX:
					return WrapMode.TileFlipX;
				case TileMode.FlipY:
					return WrapMode.TileFlipY;
				case TileMode.FlipXY:
					return WrapMode.TileFlipXY;
				default:
					return WrapMode.Clamp;
			}
		}

		public static Image ToGdiPlus(this ImageSource me)
		{
			if (Uri.TryCreate(me.ToString(), UriKind.Absolute, out Uri result))
			{
				if (result.IsFile)
				{
					try
					{
						return Image.FromFile(result.LocalPath);
					}
					catch (OutOfMemoryException ex)
					{
						Utility.Warning("Unsupported image format: {0}", ex.Message);
					}
					catch (FileNotFoundException ex2)
					{
						Utility.Warning("Image file not found: {0}", ex2.Message);
					}
				}
				else
				{
					Utility.Warning("Unable to access image: {0}", result);
				}
			}
			else
			{
				Utility.Warning("Unable to resolve image: {0}", me);
			}
			return null;
		}

		private static System.Drawing.Brush CheckDegenerate(GradientBrush brush)
		{
			switch (brush.GradientStops.Count)
			{
				case 0:
					return new SolidBrush(System.Drawing.Color.FromArgb(0, 255, 255, 255));
				case 1:
					return new SolidBrush(brush.GradientStops[0].Color.ToGdiPlus(brush.Opacity));
				default:
					return null;
			}
		}

		private static ColorBlend ConvertGradient(GradientBrush brush)
		{
			List<GradientStop> list = new List<GradientStop>(brush.GradientStops);
			list.Sort((GradientStop a, GradientStop b) => a.Offset.CompareTo(b.Offset));
			if (list[0].Offset > 0.0)
			{
				list.Insert(0, new GradientStop(list[0].Color, 0.0));
			}
			if (list[list.Count - 1].Offset < 1.0)
			{
				list.Add(new GradientStop(list[list.Count - 1].Color, 1.0));
			}
			double offset = list[0].Offset;
			if (offset < 0.0)
			{
				foreach (GradientStop item in list)
				{
					item.Offset -= offset;
				}
			}
			double offset2 = list[list.Count - 1].Offset;
			if (offset2 > 1.0)
			{
				foreach (GradientStop item2 in list)
				{
					item2.Offset /= offset2;
				}
			}
			ColorBlend colorBlend = new ColorBlend(list.Count);
			bool flag = brush is RadialGradientBrush;
			for (int i = 0; i < list.Count; i++)
			{
				colorBlend.Positions[i] = (float)(flag ? (1.0 - list[i].Offset) : list[i].Offset);
				colorBlend.Colors[i] = list[i].Color.ToGdiPlus(brush.Opacity);
			}
			if (flag)
			{
				Array.Reverse(colorBlend.Positions);
				Array.Reverse(colorBlend.Colors);
			}
			return colorBlend;
		}

		public static System.Drawing.Brush ToGdiPlus(this System.Windows.Media.LinearGradientBrush brush, Rect bounds, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			System.Drawing.Brush brush2 = CheckDegenerate(brush);
			if (brush2 != null)
			{
				return brush2;
			}
			BrushTransform brushTransform = new BrushTransform(brush, bounds);
			if (brushTransform.DegenerateBrush != null)
			{
				return brushTransform.DegenerateBrush;
			}
			System.Windows.Point point = brush.StartPoint;
			System.Windows.Point point2 = brush.EndPoint;
			if (brush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
			{
				point = brushTransform.ToAbsolute.Transform(point);
				point2 = brushTransform.ToAbsolute.Transform(point2);
			}
			WrapMode wrapMode = brush.SpreadMethod.ToGdiPlus();
			if (wrapMode == WrapMode.Clamp)
			{
				wrapMode = WrapMode.TileFlipX;
				double num = (bounds.BottomRight - bounds.TopLeft).Length / (brushTransform.ToBrush.Transform(point2) - brushTransform.ToBrush.Transform(point)).Length;
				Vector vector = num * (point2 - point);
				point -= vector;
				point2 += vector;
				brush = brush.Clone();
				GradientStopCollection gradientStops = brush.GradientStops;
				gradientStops.Insert(0, new GradientStop(gradientStops[0].Color, 0.0 - num));
				gradientStops.Add(new GradientStop(gradientStops[gradientStops.Count - 1].Color, num + 1.0));
			}
			System.Drawing.Drawing2D.LinearGradientBrush linearGradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(point.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY), point2.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY), System.Drawing.Color.Black, System.Drawing.Color.White);
			linearGradientBrush.InterpolationColors = ConvertGradient(brush);
			linearGradientBrush.WrapMode = wrapMode;
			linearGradientBrush.MultiplyTransform(brushTransform.ToBrush.ToGdiPlus(), MatrixOrder.Append);
			return linearGradientBrush;
		}

		public static System.Drawing.Brush ToGdiPlus(this RadialGradientBrush brush, Rect bounds, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			System.Drawing.Brush brush2 = CheckDegenerate(brush);
			if (brush2 != null)
			{
				return brush2;
			}
			BrushTransform brushTransform = new BrushTransform(brush, bounds);
			if (brushTransform.DegenerateBrush != null)
			{
				return brushTransform.DegenerateBrush;
			}
			System.Windows.Point point = brush.Center;
			System.Windows.Point point2 = brush.GradientOrigin;
			Vector vector = new Vector(brush.RadiusX, brush.RadiusY);
			if (brush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
			{
				point = brushTransform.ToAbsolute.Transform(point);
				point2 = brushTransform.ToAbsolute.Transform(point2);
				vector = brushTransform.ToAbsolute.Transform(vector);
			}
			Vector vector2 = brushTransform.ToBrush.Transform(vector);
			int num = (int)Math.Ceiling(4.0 * (bounds.BottomRight - bounds.TopLeft).Length / Math.Min(Math.Abs(vector2.X), Math.Abs(vector2.Y)));
			vector *= num;
			point += (num - 1) * (point - point2);
			brush = brush.Clone();
			GradientStopCollection gradientStops = brush.GradientStops;
			int num2 = gradientStops.Count - 1;
			double num3 = 1.00000001;
			switch (brush.SpreadMethod)
			{
				case GradientSpreadMethod.Pad:
					gradientStops.Add(new GradientStop(gradientStops[num2].Color, num));
					break;
				case GradientSpreadMethod.Repeat:
					for (int l = 0; l < num; l++)
					{
						for (int m = 0; m <= num2; m++)
						{
							gradientStops.Add(new GradientStop(gradientStops[m].Color, (double)l + gradientStops[m].Offset + ((m == num2) ? 1.0 : num3)));
						}
					}
					break;
				case GradientSpreadMethod.Reflect:
					for (int i = 0; i < num; i++)
					{
						if (i % 2 == 0)
						{
							for (int j = 0; j <= num2; j++)
							{
								gradientStops.Add(new GradientStop(gradientStops[j].Color, (double)i + (1.0 - gradientStops[j].Offset) + ((j == 0) ? 1.0 : num3)));
							}
						}
						else
						{
							for (int k = 0; k <= num2; k++)
							{
								gradientStops.Add(new GradientStop(gradientStops[k].Color, (double)i + gradientStops[k].Offset + ((k == num2) ? 1.0 : num3)));
							}
						}
					}
					break;
			}
			PathGradientBrush pathGradientBrush = new PathGradientBrush(new EllipseGeometry(point, vector.X, vector.Y).ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY));
			pathGradientBrush.CenterPoint = point2.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY);
			pathGradientBrush.InterpolationColors = ConvertGradient(brush);
			pathGradientBrush.WrapMode = brush.SpreadMethod.ToGdiPlus();
			pathGradientBrush.MultiplyTransform(brushTransform.ToBrush.ToGdiPlus(), MatrixOrder.Append);
			return pathGradientBrush;
		}

		public static System.Drawing.Brush ToGdiPlus(this ImageBrush brush, Rect bounds)
		{
			ImageSource imageSource = brush.ImageSource;
			BrushTransform brushTransform = new BrushTransform(brush, bounds);
			if (brushTransform.DegenerateBrush != null)
			{
				return brushTransform.DegenerateBrush;
			}
			Rect viewbox = brush.Viewbox;
			if (brush.ViewboxUnits == BrushMappingMode.RelativeToBoundingBox)
			{
				viewbox.Scale(imageSource.Width, imageSource.Height);
			}
			Rect viewport = brush.Viewport;
			if (brush.ViewportUnits == BrushMappingMode.RelativeToBoundingBox)
			{
				viewport.Transform(brushTransform.ToAbsolute);
			}
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetColorMatrix(new ColorMatrix
			{
				Matrix33 = (float)brush.Opacity
			});
			TextureBrush textureBrush = new TextureBrush(imageSource.ToGdiPlus(), viewbox.ToGdiPlus(), imageAttributes);
			textureBrush.WrapMode = brush.TileMode.ToGdiPlus();
			textureBrush.TranslateTransform((float)viewport.X, (float)viewport.Y);
			textureBrush.ScaleTransform((float)(viewport.Width / viewbox.Width), (float)(viewport.Height / viewbox.Height));
			textureBrush.MultiplyTransform(brushTransform.ToBrush.ToGdiPlus(), MatrixOrder.Append);
			return textureBrush;
		}
	}
	public static class EllipseExtensions
	{
		public static GraphicsPath EToGdiPlus(this Rect rec, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.StartFigure();
			RectangleF rect = new RectangleF((float)rec.X * ScaleX, (float)rec.Y * ScaleY, (float)rec.Width * ScaleX, (float)rec.Height * ScaleY);
			graphicsPath.AddEllipse(rect);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}
	}


	public static class GeometryExtensions
	{
		public static GraphicsPath ToGdiPlus(this Geometry geo, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			PathGeometry pathGeometry = PathGeometry.CreateFromGeometry(geo);
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.FillMode = (System.Drawing.Drawing2D.FillMode)pathGeometry.FillRule.ToGdiPlus();
			foreach (PathFigure figure in pathGeometry.Figures)
			{
				if (!figure.IsFilled)
				{
					Utility.Warning("Unfilled path figures not supported, use null brush instead.");
				}
				graphicsPath.StartFigure();
				PointF startPoint = figure.StartPoint.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY);
				foreach (PathSegment segment in figure.Segments)
				{
					startPoint = segment.AddToPath(startPoint, graphicsPath, ScaleX, ScaleY, OffsetX, OffsetY);
				}
				if (figure.IsClosed)
				{
					graphicsPath.CloseFigure();
				}
			}
			if (pathGeometry.Transform != null && !pathGeometry.Transform.Value.IsIdentity)
			{
				graphicsPath.Transform(pathGeometry.Transform.Value.ToGdiPlus());
			}
			return graphicsPath;
		}

		public static List<System.Drawing.Point> ListPoint(this Geometry geo, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			List<System.Drawing.Point> list = new List<System.Drawing.Point>();
			PathGeometry pathGeometry = PathGeometry.CreateFromGeometry(geo);
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.FillMode = (System.Drawing.Drawing2D.FillMode)pathGeometry.FillRule.ToGdiPlus();
			foreach (PathFigure figure in pathGeometry.Figures)
			{
				if (!figure.IsFilled)
				{
					Utility.Warning("Unfilled path figures not supported, use null brush instead.");
				}
				graphicsPath.StartFigure();
				PointF pointF = figure.StartPoint.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY);
				list.Add(System.Drawing.Point.Round(pointF));
				foreach (PathSegment segment in figure.Segments)
				{
					pointF = segment.AddToPath(pointF, graphicsPath, ScaleX, ScaleY, OffsetX, OffsetY);
					list.Add(System.Drawing.Point.Round(pointF));
				}
				if (figure.IsClosed)
				{
					graphicsPath.CloseFigure();
				}
			}
			return list;
		}

        public static FillMode ToGdiPlus(this FillRule me)
        {
            if (me == FillRule.EvenOdd)
            {
                return FillMode.Alternate;
            }
            return FillMode.Winding;
        }
    }
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
	public static class RectangleExtensions
	{
		public static GraphicsPath RToGdiPlus(this Rect rec, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.StartFigure();
			RectangleF rect = new RectangleF((float)rec.X * ScaleX, (float)rec.Y * ScaleY, (float)rec.Width * ScaleX, (float)rec.Height * ScaleY);
			graphicsPath.AddRectangle(rect);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}
	}
	public static class SegmentExtensions
	{
		public static PointF AddToPath(this PathSegment segment, PointF startPoint, GraphicsPath path, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			if (!segment.IsStroked)
			{
				Utility.Warning("Unstroked path segments not supported, use null pen instead.");
			}
			ArcSegment segment2;
			BezierSegment segment3;
			LineSegment segment4;
			PolyBezierSegment segment5;
			PolyLineSegment segment6;
			PolyQuadraticBezierSegment segment7;
			if ((segment2 = (segment as ArcSegment)) != null)
			{
				startPoint = segment2.AddToPath(startPoint, path, ScaleX, ScaleY, OffsetX, OffsetY);
			}
			else if ((segment3 = (segment as BezierSegment)) != null)
			{
				startPoint = segment3.AddToPath(startPoint, path, ScaleX, ScaleY, OffsetX, OffsetY);
			}
			else if ((segment4 = (segment as LineSegment)) != null)
			{
				startPoint = segment4.AddToPath(startPoint, path, ScaleX, ScaleY, OffsetX, OffsetY);
			}
			else if ((segment5 = (segment as PolyBezierSegment)) != null)
			{
				startPoint = segment5.AddToPath(startPoint, path, ScaleX, ScaleY, OffsetX, OffsetY);
			}
			else if ((segment6 = (segment as PolyLineSegment)) != null)
			{
				startPoint = segment6.AddToPath(startPoint, path, ScaleX, ScaleY, OffsetX, OffsetY);
			}
			else if ((segment7 = (segment as PolyQuadraticBezierSegment)) != null)
			{
				startPoint = segment7.AddToPath(startPoint, path, ScaleX, ScaleY, OffsetX, OffsetY);
			}
			else
			{
				QuadraticBezierSegment segment8;
				if ((segment8 = (segment as QuadraticBezierSegment)) == null)
				{
					throw new ArgumentOutOfRangeException("segment", segment.GetType().ToString());
				}
				startPoint = segment8.AddToPath(startPoint, path, ScaleX, ScaleY, OffsetX, OffsetY);
			}
			return startPoint;
		}

		private static PointF AddToPath(this LineSegment segment, PointF startPoint, GraphicsPath path, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			PointF pointF = segment.Point.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY);
			path.AddLine(startPoint, pointF);
			return pointF;
		}

		private static PointF AddToPath(this PolyLineSegment segment, PointF startPoint, GraphicsPath path, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			PointF[] array = new PointF[segment.Points.Count + 1];
			int num = 0;
			array[num++] = startPoint;
			foreach (System.Windows.Point point in segment.Points)
			{
				array[num++] = point.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY);
			}
			path.AddLines(array);
			return array[num - 1];
		}

		private static PointF AddToPath(this BezierSegment segment, PointF startPoint, GraphicsPath path, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			PointF pointF = segment.Point3.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY);
			path.AddBezier(startPoint, segment.Point1.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY), segment.Point2.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY), pointF);
			return pointF;
		}

		private static PointF AddToPath(this PolyBezierSegment segment, PointF startPoint, GraphicsPath path, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			PointF[] array = new PointF[segment.Points.Count + 1];
			int num = 0;
			array[num++] = startPoint;
			foreach (System.Windows.Point point in segment.Points)
			{
				array[num++] = point.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY);
			}
			path.AddBeziers(array);
			return array[array.Length - 1];
		}

		private static PointF AddToPath(this QuadraticBezierSegment segment, PointF startPoint, GraphicsPath path, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			PointF[] array = new PointF[3];
			QuadraticToCubic(startPoint, segment.Point1.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY), segment.Point2.ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY), array, 0);
			path.AddBezier(startPoint, array[0], array[1], array[2]);
			return array[2];
		}

		private static PointF AddToPath(this PolyQuadraticBezierSegment segment, PointF startPoint, GraphicsPath path, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			PointF[] array = new PointF[3 * segment.Points.Count / 2 + 1];
			int num = 0;
			array[num++] = startPoint;
			for (int i = 0; i < segment.Points.Count; i += 2)
			{
				QuadraticToCubic(array[num - 1], segment.Points[i].ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY), segment.Points[i + 1].ToGdiPlus(ScaleX, ScaleY, OffsetX, OffsetY), array, num);
				num += 3;
			}
			path.AddBeziers(array);
			return array[array.Length - 1];
		}

		private static void QuadraticToCubic(PointF q0, PointF q1, PointF q2, PointF[] c, int index)
		{
			c[index].X = q0.X + 2f * (q1.X - q0.X) / 3f;
			c[index].Y = q0.Y + 2f * (q1.Y - q0.Y) / 3f;
			c[index + 1].X = q1.X + (q2.X - q1.X) / 3f;
			c[index + 1].Y = q1.Y + (q2.Y - q1.Y) / 3f;
			c[index + 2].X = q2.X;
			c[index + 2].Y = q2.Y;
		}

		private static PointF AddToPath(this ArcSegment segment, PointF startPoint, GraphicsPath path, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			PathGeometry pathGeometry = new PathGeometry
			{
				Figures = new PathFigureCollection
			{
				new PathFigure
				{
					IsFilled = true,
					IsClosed = true,
					StartPoint = new System.Windows.Point((startPoint.X - OffsetX) * ScaleX, (startPoint.Y - OffsetY) * ScaleY),
					Segments = new PathSegmentCollection
					{
						segment
					}
				}
			}
			};
			Rect bounds = pathGeometry.Bounds;
			bounds.Inflate(1.0, 1.0);
			PathGeometry pathGeometry2 = Geometry.Combine(new RectangleGeometry(bounds), pathGeometry, GeometryCombineMode.Intersect, Transform.Identity);
			if (pathGeometry2.Figures.Count != 1)
			{
				throw new InvalidOperationException("Geometry.Combine produced too many figures.");
			}
			PathFigure pathFigure = pathGeometry2.Figures[0];
			if (!(pathFigure.Segments[0] is LineSegment))
			{
				throw new InvalidOperationException("Geometry.Combine didn't start with a line");
			}
			PointF pointF = startPoint;
			for (int i = 1; i < pathFigure.Segments.Count; i++)
			{
				if (pathFigure.Segments[i] is ArcSegment)
				{
					throw new InvalidOperationException("Geometry.Combine produced an ArcSegment - oops, bad hack");
				}
				pointF = pathFigure.Segments[i].AddToPath(pointF, path, ScaleX, ScaleY, OffsetX, OffsetY);
			}
			return pointF;
		}

		public static PointF ToGdiPlus(this System.Windows.Point point, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			return new PointF(((float)point.X + OffsetX) * ScaleX, ((float)point.Y + OffsetY) * ScaleY);
		}

		public static RectangleF ToGdiPlus(this Rect rect)
		{
			return new RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
		}

		public static System.Drawing.Drawing2D.Matrix ToGdiPlus(this System.Windows.Media.Matrix matrix)
		{
			return new System.Drawing.Drawing2D.Matrix((float)matrix.M11, (float)matrix.M12, (float)matrix.M21, (float)matrix.M22, (float)matrix.OffsetX, (float)matrix.OffsetY);
		}
	}
	public static class Utility
	{
		public static object LoadXamlFromFile(string fileName)
		{
			using (Stream stream = File.OpenRead(fileName))
			{
				return System.Windows.Markup.XamlReader.Load(stream, new ParserContext
				{
					BaseUri = new Uri(Path.GetFullPath(fileName), UriKind.Absolute)
				});
			}
		}

		public static void RealizeFrameworkElement(FrameworkElement fe)
		{
			System.Windows.Size availableSize = new System.Windows.Size(double.MaxValue, double.MaxValue);
			if (fe.Width > 0.0 && fe.Height > 0.0)
			{
				availableSize = new System.Windows.Size(fe.Width, fe.Height);
			}
			fe.Measure(availableSize);
			fe.Arrange(new Rect(default(System.Windows.Point), fe.DesiredSize));
			fe.UpdateLayout();
		}

		public static Drawing GetDrawingFromXaml(object xaml)
		{
			Drawing drawing = FindDrawing(xaml);
			if (drawing != null)
			{
				return drawing;
			}
			FrameworkElement frameworkElement = xaml as FrameworkElement;
			if (frameworkElement != null)
			{
				RealizeFrameworkElement(frameworkElement);
				drawing = WalkVisual(frameworkElement);
			}
			return drawing;
		}

		public static void MakeDrawingSerializable(Drawing drawing)
		{
			InternalMakeDrawingSerializable(drawing, new GeometryValueSerializer());
		}

		public static Graphics CreateEmf(string fileName, Rect bounds)
		{
			if (bounds.Width == 0.0 || bounds.Height == 0.0)
			{
				bounds = new Rect(0.0, 0.0, 1.0, 1.0);
			}
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
			using (Graphics graphics = Graphics.FromImage(new Bitmap(1, 1)))
			{
				return Graphics.FromImage(new Metafile(File.Create(fileName), graphics.GetHdc(), bounds.ToGdiPlus(), MetafileFrameUnit.Pixel, EmfType.EmfPlusDual));
			}
		}

		public static void SetGraphicsQuality(Graphics graphics)
		{
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
		}

		private static void InternalMakeDrawingSerializable(Drawing drawing, GeometryValueSerializer gvs)
		{
			DrawingGroup drawingGroup = drawing as DrawingGroup;
			if (drawingGroup != null)
			{
				for (int i = 0; i < drawingGroup.Children.Count; i++)
				{
					InternalMakeDrawingSerializable(drawingGroup.Children[i], gvs);
				}
			}
			else
			{
				GeometryDrawing geometryDrawing = drawing as GeometryDrawing;
			}
		}

		public static Drawing FindDrawing(object xaml)
		{
			Drawing drawing = xaml as Drawing;
			if (drawing != null)
			{
				return drawing;
			}
			DrawingBrush drawingBrush = xaml as DrawingBrush;
			if (drawingBrush != null)
			{
				return drawingBrush.Drawing;
			}
			DrawingImage drawingImage = xaml as DrawingImage;
			if (drawingImage != null)
			{
				return drawingImage.Drawing;
			}
			DrawingVisual drawingVisual = xaml as DrawingVisual;
			if (drawingVisual != null)
			{
				return drawingVisual.Drawing;
			}
			ResourceDictionary resourceDictionary = xaml as ResourceDictionary;
			if (resourceDictionary != null)
			{
				foreach (object value in resourceDictionary.Values)
				{
					Drawing drawing2 = FindDrawing(value);
					if (drawing2 != null)
					{
						if (drawing != null)
						{
							throw new ArgumentException("Multiple Drawings found in ResourceDictionary", "xaml");
						}
						drawing = drawing2;
					}
				}
				if (drawing != null)
				{
					return drawing;
				}
			}
			return null;
		}

		private static DrawingGroup WalkVisual(Visual visual)
		{
			DrawingGroup drawing = VisualTreeHelper.GetDrawing(visual);
			BitmapEffect bitmapEffect = VisualTreeHelper.GetBitmapEffect(visual);
			BitmapEffectInput bitmapEffectInput = VisualTreeHelper.GetBitmapEffectInput(visual);
			Geometry clip = VisualTreeHelper.GetClip(visual);
			double opacity = VisualTreeHelper.GetOpacity(visual);
			System.Windows.Media.Brush opacityMask = VisualTreeHelper.GetOpacityMask(visual);
			GuidelineSet guidelines = GetGuidelines(visual);
			Transform transform = GetTransform(visual);
			DrawingGroup drawingGroup = null;
			if (bitmapEffect == null && clip == null && opacityMask == null && guidelines == null && transform == null && IsZero(opacity - 1.0))
			{
				drawingGroup = (drawing ?? new DrawingGroup());
			}
			else
			{
				drawingGroup = new DrawingGroup();
				if (bitmapEffect != null)
				{
					drawingGroup.BitmapEffect = bitmapEffect;
				}
				if (bitmapEffectInput != null)
				{
					drawingGroup.BitmapEffectInput = bitmapEffectInput;
				}
				if (clip != null)
				{
					drawingGroup.ClipGeometry = clip;
				}
				if (!IsZero(opacity - 1.0))
				{
					drawingGroup.Opacity = opacity;
				}
				if (opacityMask != null)
				{
					drawingGroup.OpacityMask = opacityMask;
				}
				if (guidelines != null)
				{
					drawingGroup.GuidelineSet = guidelines;
				}
				if (transform != null)
				{
					drawingGroup.Transform = transform;
				}
				if (drawing != null)
				{
					drawingGroup.Children.Add(drawing);
				}
			}
			int childrenCount = VisualTreeHelper.GetChildrenCount(visual);
			for (int i = 0; i < childrenCount; i++)
			{
				drawingGroup.Children.Add(WalkVisual(GetChild(visual, i)));
			}
			return drawingGroup;
		}

		private static GuidelineSet GetGuidelines(Visual visual)
		{
			DoubleCollection xSnappingGuidelines = VisualTreeHelper.GetXSnappingGuidelines(visual);
			DoubleCollection ySnappingGuidelines = VisualTreeHelper.GetYSnappingGuidelines(visual);
			if (xSnappingGuidelines == null && ySnappingGuidelines == null)
			{
				return null;
			}
			GuidelineSet guidelineSet = new GuidelineSet();
			if (xSnappingGuidelines != null)
			{
				guidelineSet.GuidelinesX = xSnappingGuidelines;
			}
			if (ySnappingGuidelines != null)
			{
				guidelineSet.GuidelinesY = ySnappingGuidelines;
			}
			return guidelineSet;
		}

		private static Transform GetTransform(Visual visual)
		{
			Transform transform = VisualTreeHelper.GetTransform(visual);
			Vector offset = VisualTreeHelper.GetOffset(visual);
			if (IsZero(offset.X) && IsZero(offset.Y))
			{
				if (!IsIdentity(transform))
				{
					return transform;
				}
				return null;
			}
			if (IsIdentity(transform))
			{
				return new TranslateTransform(offset.X, offset.Y);
			}
			System.Windows.Media.Matrix value = transform.Value;
			value.Translate(offset.X, offset.Y);
			return new MatrixTransform(value);
		}

		private static bool IsIdentity(Transform t)
		{
			return t?.Value.IsIdentity ?? true;
		}

		private static Visual GetChild(Visual visual, int index)
		{
			DependencyObject child = VisualTreeHelper.GetChild(visual, index);
			Visual visual2 = child as Visual;
			if (visual2 == null)
			{
				throw new NotImplementedException("Visual3D not implemented");
			}
			return visual2;
		}

		internal static bool IsZero(double d)
		{
			return Math.Abs(d) < 2E-05;
		}

		internal static void Warning(string message, params object[] args)
		{
			Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, message, args));
		}
	}

}
