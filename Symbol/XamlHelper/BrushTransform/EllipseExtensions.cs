

// Hmi.Helper.EllipseExtensions
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
namespace XamlHelper
{
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
}