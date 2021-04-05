

// Hmi.Helper.SegmentExtensions

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Media;
namespace XamlHelper
{
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
}