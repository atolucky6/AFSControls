//using Hmi.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using XamlHelper;

namespace vGraphic
{
	//[Serializable]
	//public class KeyImg
	//{
	//	public System.Drawing.Color TargetColor;

	//	public string ObjectName;

	//	public System.Drawing.Size ImageSize;

	//	public List<ElementSetting> ListElementSetting = new List<ElementSetting>();

	//	public FlipType flipType;

	//	public Rotate Rotation;

	//	public XamlHelper.FillMode mode;
	//}
	//public enum XamlHelper.FillMode
	//{
	//	Original = 0,
	//	Shaded = 1,
	//	Solid = 2

	//}

	class Class1
	{

		// this.Region = XamlHelper.XamlHelper.BitmapShade(e.Graphics, objectSelect, this.Size, Helper.getdata(objectSelect), flip, rotate, XamlHelper.FillMode, fillColor);
		public static Region BitmapShade(Graphics grp, string objectName, System.Drawing.Size osize, string xaml, FlipType flipType, Rotate Rotation, XamlHelper.FillMode mode, System.Drawing.Color newColor)
		{
			System.Drawing.Size size = new System.Drawing.Size(1, 1);
			size = ((Rotation == Rotate.Rot_90 || Rotation == Rotate.Rot_270) ? new System.Drawing.Size(osize.Height, osize.Width) : new System.Drawing.Size(osize.Width, osize.Height));
			double num = 1.0;
			double num2 = 1.0;
			KeyImg value = new KeyImg
			{
				ObjectName = objectName,
				ImageSize = size,
				flipType = flipType,
				Rotation = Rotation,
				TargetColor = newColor,
				mode = mode
			};
			string keys = JsonConvert.SerializeObject(value).Replace("[", "").Replace("]", "")
				.Replace(",", "")
				.Replace(":", "")
				.Replace("\"", "")
				.Replace("{", "")
				.Replace("}", "");
			if (Root.dicFact.Any((Img p) => p.key == keys))
			{
				Img img = (from p in Root.dicFact
						   where p.key == keys
						   select p).FirstOrDefault();
				grp.SmoothingMode = SmoothingMode.AntiAlias;
				grp.DrawImage(rect: new RectangleF(0f, 0f, osize.Width, osize.Height), image: img.Bmp);
				return new Region(img.Region);
			}
			List<string> list = new List<string>();
			Bitmap bitmap = new Bitmap(size.Width, size.Height);
			Bitmap bitmap2 = new Bitmap(size.Width, size.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			Graphics graphics2 = Graphics.FromImage(bitmap2);
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics2.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.Clear(System.Drawing.Color.Transparent);
			graphics2.Clear(System.Drawing.Color.Magenta);
			StringReader input = new StringReader(xaml);
			XmlReader reader = XmlReader.Create(input);
			Canvas canvas = (Canvas)XamlReader.Load(reader);
			double width = canvas.Width;
			double height = canvas.Height;
			num = (double)size.Width / width;
			num2 = (double)size.Height / height;
			foreach (UIElement child in canvas.Children)
			{
				if (child is Ellipse)
				{
					Ellipse ellipse = child as Ellipse;
					if (!string.IsNullOrEmpty(ellipse.Name))
					{
						list.Add(ellipse.Name);
					}
					double y = (double)ellipse.GetValue(Canvas.TopProperty);
					double x = (double)ellipse.GetValue(Canvas.LeftProperty);
					double width2 = ellipse.Width;
					double height2 = ellipse.Height;
					Rect rect2 = new Rect(x, y, width2, height2);
					GraphicsPath path = rect2.EToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (ellipse.Fill != null && ellipse.Fill is SolidColorBrush)
					{
						System.Windows.Media.Color color = (ellipse.Fill as SolidColorBrush).Color;
						if (mode == XamlHelper.FillMode.Shaded)
						{
						//	color = ColorHelper.GdiToMedia(ColorHelper.GrayscaleToColorGradient(ColorHelper.MediaToGdi(color), newColor));
							color = ColorHelper.GdiToMedia( newColor);

						}
						SolidColorBrush brush = new SolidColorBrush(color);
						System.Drawing.Brush brush2 = brush.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush2, path);
						graphics2.FillPath(brush2, path);
					}
					if (ellipse.Stroke != null)
					{
						System.Windows.Media.Pen pen = new System.Windows.Media.Pen(ellipse.Stroke, ellipse.StrokeThickness);
						System.Drawing.Pen pen2 = pen.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						pen2.Width = (float)ellipse.StrokeThickness;
						graphics.DrawPath(pen2, path);
						graphics2.DrawPath(pen2, path);
					}
				}
				if (child is System.Windows.Shapes.Rectangle)
				{
					System.Windows.Shapes.Rectangle rectangle = child as System.Windows.Shapes.Rectangle;
					if (!string.IsNullOrEmpty(rectangle.Name))
					{
						list.Add(rectangle.Name);
					}
					double y2 = (double)rectangle.GetValue(Canvas.TopProperty);
					double x2 = (double)rectangle.GetValue(Canvas.LeftProperty);
					double width3 = rectangle.Width;
					double height3 = rectangle.Height;
					Rect rect3 = new Rect(x2, y2, width3, height3);
					GraphicsPath path2 = rect3.RToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (rectangle.Fill != null && rectangle.Fill is SolidColorBrush)
					{
						System.Windows.Media.Color color2 = (rectangle.Fill as SolidColorBrush).Color;
						if (mode == XamlHelper.FillMode.Shaded)
						{
						//	color2 = ColorHelper.GdiToMedia(ColorHelper.GrayscaleToColorGradient(ColorHelper.MediaToGdi(color2), newColor));
							color2 = ColorHelper.GdiToMedia(newColor);

						}
						SolidColorBrush brush3 = new SolidColorBrush(color2);
						System.Drawing.Brush brush4 = brush3.ToGdiPlus(rect3, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush4, path2);
						graphics2.FillPath(brush4, path2);
					}
					if (rectangle.Stroke != null)
					{
						System.Windows.Media.Pen pen3 = new System.Windows.Media.Pen(rectangle.Stroke, rectangle.StrokeThickness);
						System.Drawing.Pen pen4 = pen3.ToGdiPlus(rect3, (float)num, (float)num2, 0f, 0f);
						pen4.Width = (float)rectangle.StrokeThickness;
						graphics.DrawPath(pen4, path2);
						graphics2.DrawPath(pen4, path2);
					}
				}
				if (child is System.Windows.Shapes.Path)
				{
					System.Windows.Shapes.Path path3 = child as System.Windows.Shapes.Path;
					if (!string.IsNullOrEmpty(path3.Name))
					{
						list.Add(path3.Name);
					}
					Geometry data = path3.Data;
					GraphicsPath path4 = data.ToGdiPlus((float)num, (float)num2, 0f, 0f);
					double y3 = (double)path3.GetValue(Canvas.TopProperty);
					double x3 = (double)path3.GetValue(Canvas.LeftProperty);
					double width4 = path3.Width;
					double height4 = path3.Height;
					Rect bounds = new Rect(x3, y3, width4, height4);
					if (path3.Fill != null && path3.Fill is SolidColorBrush)
					{
						System.Windows.Media.Color color3 = (path3.Fill as SolidColorBrush).Color;
						if (mode == XamlHelper.FillMode.Shaded)
						{
						//	color3 = ColorHelper.GdiToMedia(ColorHelper.GrayscaleToColorGradient(ColorHelper.MediaToGdi(color3), newColor));
							color3 = ColorHelper.GdiToMedia(newColor);

						}
						SolidColorBrush brush5 = new SolidColorBrush(color3);
						System.Drawing.Brush brush6 = brush5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush6, path4);
						graphics2.FillPath(brush6, path4);
					}
					if (path3.Stroke != null)
					{
						System.Windows.Media.Pen pen5 = new System.Windows.Media.Pen(path3.Stroke, path3.StrokeThickness);
						System.Drawing.Pen pen6 = pen5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
						pen6.Width = (float)path3.StrokeThickness;
						graphics.DrawPath(pen6, path4);
						graphics2.DrawPath(pen6, path4);
					}
				}
			}
			grp.SmoothingMode = SmoothingMode.AntiAlias;
			FlipBitmap(bitmap, flipType);
			FlipBitmap(bitmap2, flipType);
			RotateBitmap(bitmap, Rotation);
			RotateBitmap(bitmap2, Rotation);
			RectangleF rect4 = new RectangleF(0f, 0f, osize.Width, osize.Height);
			grp.DrawImage(bitmap, rect4);
			Region region = CreateRegion(bitmap2, System.Drawing.Color.Magenta);
			Root.dicFact.Add(new Img
			{
				key = keys,
				Bmp = bitmap,
				Region = region.GetRegionData()
			});
			return region;
		}

		public static Region CreateRegion(Bitmap bmp, System.Drawing.Color rmvColor)
		{
			System.Drawing.Color right = System.Drawing.Color.FromArgb(rmvColor.R, rmvColor.G, rmvColor.B);
			Region region = new Region();
			region.MakeEmpty();
			System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 0, 0);
			bool flag = false;
			int height = bmp.Height;
			int width = bmp.Width;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (!flag)
					{
						if (bmp.GetPixel(j, i) != right)
						{
							flag = true;
							rect.X = j;
							rect.Y = i;
							rect.Height = 1;
						}
					}
					else if (bmp.GetPixel(j, i) == right)
					{
						flag = false;
						rect.Width = j - rect.X;
						region.Union(rect);
					}
				}
				if (flag)
				{
					flag = false;
					rect.Width = bmp.Width - rect.X;
					region.Union(rect);
				}
			}
			return region;
		}

		public static void FlipBitmap(Bitmap bmp, FlipType flipType)
		{
			switch (flipType)
			{
				case FlipType.Horizontal:
					bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
					break;
				case FlipType.Vertical:
					bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
					break;
				case FlipType.Both:
					bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
					break;
				default:
					bmp.RotateFlip(RotateFlipType.RotateNoneFlipNone);
					break;
			}
		}

		public static void RotateBitmap(Bitmap bmp, Rotate Rotation)
		{
			switch (Rotation)
			{
				case Rotate.Rot_90:
					bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
					break;
				case Rotate.Rot_180:
					bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
					break;
				default:
					bmp.RotateFlip(RotateFlipType.RotateNoneFlipNone);
					break;
				case Rotate.Rot_270:
					bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
					break;
			}
		}
	}
}
