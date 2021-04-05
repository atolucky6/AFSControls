

// Hmi.Helper.Utility
#define TRACE

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Effects;
namespace XamlHelper
{
	public static class Utility
	{
		public static object LoadXamlFromFile(string fileName)
		{
			using (Stream stream = File.OpenRead(fileName))
			{
				return XamlReader.Load(stream, new ParserContext
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