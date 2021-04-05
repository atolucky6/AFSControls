

// Hmi.Helper.GeometryExtensions
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Media;
namespace XamlHelper
{
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

		public static List<Point> ListPoint(this Geometry geo, float ScaleX, float ScaleY, float OffsetX, float OffsetY)
		{
			List<Point> list = new List<Point>();
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
				list.Add(Point.Round(pointF));
				foreach (PathSegment segment in figure.Segments)
				{
					pointF = segment.AddToPath(pointF, graphicsPath, ScaleX, ScaleY, OffsetX, OffsetY);
					list.Add(Point.Round(pointF));
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
				return (FillMode)System.Drawing.Drawing2D.FillMode.Alternate;
			}
			return (FillMode)System.Drawing.Drawing2D.FillMode.Winding;
		}
	}
}