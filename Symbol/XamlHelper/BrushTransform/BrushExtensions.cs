

// Hmi.Helper.BrushExtensions

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace XamlHelper
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
}
