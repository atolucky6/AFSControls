

// XamlHelper.XamlHelper

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using XamlHelper;
namespace XamlHelper
{
	public static class XamlHelper
	{
		public static Region DrawImage(Graphics grp, System.Drawing.Color backColor, string objectName, System.Drawing.Size osize, List<ElementSetting> els, FlipType flipType, Rotate Rotation)
		{
			System.Drawing.Size size = new System.Drawing.Size(1, 1);
			size = ((Rotation == Rotate.Rot_90 || Rotation == Rotate.Rot_270) ? new System.Drawing.Size(osize.Height, osize.Width) : new System.Drawing.Size(osize.Width, osize.Height));
			double num = 1.0;
			double num2 = 1.0;
			string s = string.Empty;
			if (Root.xamlObject.ContainsKey(objectName))
			{
				s = Root.xamlObject[objectName];
			}
			KeyImg keyImg = new KeyImg
			{
				ObjectName = objectName,
				ImageSize = size,
				ListElementSetting = els,
				flipType = flipType,
				Rotation = Rotation
			};
			string keys = JsonConvert.SerializeObject((object)keyImg).Replace("[", "").Replace("]", "")
				.Replace(",", "")
				.Replace(":", "")
				.Replace("\"", "")
				.Replace("{", "")
				.Replace("}", "");
			if (Root.dicImg.Any((Img p) => p.key == keys))
			{
				Img img = (from p in Root.dicImg
						   where p.key == keys
						   select p).FirstOrDefault();
				grp.SmoothingMode = SmoothingMode.AntiAlias;
				grp.DrawImage(rect: new RectangleF(0f, 0f, osize.Width, osize.Height), image: img.Bmp);
				return new Region(img.Region);
			}
			Bitmap bitmap = new Bitmap(size.Width, size.Height);
			Bitmap bitmap2 = new Bitmap(size.Width, size.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			Graphics graphics2 = Graphics.FromImage(bitmap2);
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics2.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.Clear(backColor);
			graphics2.Clear(System.Drawing.Color.Magenta);
			StringReader input = new StringReader(s);
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
					Ellipse elp = child as Ellipse;
					if (els.Any((ElementSetting p) => p.ElementName == elp.Name && !p.Visible))
					{
						continue;
					}
					double y = (double)elp.GetValue(Canvas.TopProperty);
					double x = (double)elp.GetValue(Canvas.LeftProperty);
					double width2 = elp.Width;
					double height2 = elp.Height;
					Rect rect2 = new Rect(x, y, width2, height2);
					GraphicsPath path2 = rect2.EToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (elp.Fill != null && elp.Fill is SolidColorBrush)
					{
						SolidColorBrush brush = new SolidColorBrush((elp.Fill as SolidColorBrush).Color);
						if (els.Any((ElementSetting p) => p.ElementName == elp.Name && p.FillColor != System.Drawing.Color.Empty))
						{
							ElementSetting elementSetting = (from p in els
															 where p.ElementName == elp.Name
															 select p).FirstOrDefault();
							System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(elementSetting.FillColor.A, elementSetting.FillColor.R, elementSetting.FillColor.G, elementSetting.FillColor.B);
							brush = new SolidColorBrush(color);
						}
						System.Drawing.Brush brush2 = brush.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush2, path2);
						graphics2.FillPath(brush2, path2);
					}
					if (elp.Stroke != null)
					{
						System.Windows.Media.Pen pen = new System.Windows.Media.Pen(elp.Stroke, elp.StrokeThickness * (double)(float)num);
						if (els.Any((ElementSetting p) => p.ElementName == elp.Name && p.DrawColor != System.Drawing.Color.Empty))
						{
							ElementSetting elementSetting2 = (from p in els
															  where p.ElementName == elp.Name
															  select p).FirstOrDefault();
							System.Windows.Media.Color color2 = System.Windows.Media.Color.FromArgb(elementSetting2.DrawColor.A, elementSetting2.DrawColor.R, elementSetting2.DrawColor.G, elementSetting2.DrawColor.B);
							pen = new System.Windows.Media.Pen(new SolidColorBrush(color2), (elp.StrokeThickness * (double)(float)num > 1.0) ? (elp.StrokeThickness * (double)(float)num) : 1.5);
						}
						System.Drawing.Pen pen2 = pen.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen2, path2);
						graphics2.DrawPath(pen2, path2);
					}
				}
				if (child is System.Windows.Shapes.Rectangle)
				{
					System.Windows.Shapes.Rectangle rec = child as System.Windows.Shapes.Rectangle;
					if (els.Any((ElementSetting p) => p.ElementName == rec.Name && !p.Visible))
					{
						continue;
					}
					double y2 = (double)rec.GetValue(Canvas.TopProperty);
					double x2 = (double)rec.GetValue(Canvas.LeftProperty);
					double width3 = rec.Width;
					double height3 = rec.Height;
					Rect rect3 = new Rect(x2, y2, width3, height3);
					GraphicsPath path3 = rect3.RToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (rec.Fill != null && rec.Fill is SolidColorBrush)
					{
						SolidColorBrush brush3 = new SolidColorBrush((rec.Fill as SolidColorBrush).Color);
						if (els.Any((ElementSetting p) => p.ElementName == rec.Name && p.FillColor != System.Drawing.Color.Empty))
						{
							ElementSetting elementSetting3 = (from p in els
															  where p.ElementName == rec.Name
															  select p).FirstOrDefault();
							System.Windows.Media.Color color3 = System.Windows.Media.Color.FromArgb(elementSetting3.FillColor.A, elementSetting3.FillColor.R, elementSetting3.FillColor.G, elementSetting3.FillColor.B);
							brush3 = new SolidColorBrush(color3);
						}
						System.Drawing.Brush brush4 = brush3.ToGdiPlus(rect3, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush4, path3);
						graphics2.FillPath(brush4, path3);
					}
					if (rec.Stroke != null)
					{
						System.Windows.Media.Pen pen3 = new System.Windows.Media.Pen(rec.Stroke, rec.StrokeThickness * (double)(float)num);
						if (els.Any((ElementSetting p) => p.ElementName == rec.Name && p.DrawColor != System.Drawing.Color.Empty))
						{
							ElementSetting elementSetting4 = (from p in els
															  where p.ElementName == rec.Name
															  select p).FirstOrDefault();
							System.Windows.Media.Color color4 = System.Windows.Media.Color.FromArgb(elementSetting4.DrawColor.A, elementSetting4.DrawColor.R, elementSetting4.DrawColor.G, elementSetting4.DrawColor.B);
							pen3 = new System.Windows.Media.Pen(new SolidColorBrush(color4), (rec.StrokeThickness * (double)(float)num > 1.0) ? (rec.StrokeThickness * (double)(float)num) : 1.5);
						}
						System.Drawing.Pen pen4 = pen3.ToGdiPlus(rect3, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen4, path3);
						graphics2.DrawPath(pen4, path3);
					}
				}
				if (child is System.Windows.Shapes.Path)
				{
					System.Windows.Shapes.Path path = child as System.Windows.Shapes.Path;
					if (!els.Any((ElementSetting p) => p.ElementName == path.Name && !p.Visible))
					{
						Geometry data = path.Data;
						GraphicsPath path4 = data.ToGdiPlus((float)num, (float)num2, 0f, 0f);
						double y3 = (double)path.GetValue(Canvas.TopProperty);
						double x3 = (double)path.GetValue(Canvas.LeftProperty);
						double width4 = path.Width;
						double height4 = path.Height;
						Rect bounds = new Rect(x3, y3, width4, height4);
						if (path.Fill != null && path.Fill is SolidColorBrush)
						{
							SolidColorBrush brush5 = new SolidColorBrush((path.Fill as SolidColorBrush).Color);
							if (els.Any((ElementSetting p) => p.ElementName == path.Name && p.FillColor != System.Drawing.Color.Empty))
							{
								ElementSetting elementSetting5 = (from p in els
																  where p.ElementName == path.Name
																  select p).FirstOrDefault();
								System.Windows.Media.Color color5 = System.Windows.Media.Color.FromArgb(elementSetting5.FillColor.A, elementSetting5.FillColor.R, elementSetting5.FillColor.G, elementSetting5.FillColor.B);
								brush5 = new SolidColorBrush(color5);
							}
							System.Drawing.Brush brush6 = brush5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
							graphics.FillPath(brush6, path4);
							graphics2.FillPath(brush6, path4);
						}
						if (path.Stroke != null)
						{
							System.Windows.Media.Pen pen5 = new System.Windows.Media.Pen(path.Stroke, path.StrokeThickness * (double)(float)num);
							if (els.Any((ElementSetting p) => p.ElementName == path.Name && p.DrawColor != System.Drawing.Color.Empty))
							{
								ElementSetting elementSetting6 = (from p in els
																  where p.ElementName == path.Name
																  select p).FirstOrDefault();
								System.Windows.Media.Color color6 = System.Windows.Media.Color.FromArgb(elementSetting6.DrawColor.A, elementSetting6.DrawColor.R, elementSetting6.DrawColor.G, elementSetting6.DrawColor.B);
								pen5 = new System.Windows.Media.Pen(new SolidColorBrush(color6), (path.StrokeThickness * (double)(float)num > 1.0) ? (path.StrokeThickness * (double)(float)num) : 1.5);
							}
							System.Drawing.Pen pen6 = pen5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
							graphics.DrawPath(pen6, path4);
							graphics2.DrawPath(pen6, path4);
						}
					}
				}
			}
			grp.SmoothingMode = SmoothingMode.AntiAlias;
			FlipBitmap(bitmap, flipType);
			FlipBitmap(bitmap2, flipType);
			RotateBitmap(bitmap, Rotation);
			RotateBitmap(bitmap2, Rotation);
			Region region = CreateRegion(bitmap2, System.Drawing.Color.Magenta).Clone();
			RectangleF rect4 = new RectangleF(0f, 0f, osize.Width, osize.Height);
			grp.DrawImage(bitmap, rect4);
			Root.dicImg.Add(new Img
			{
				key = keys,
				Bmp = bitmap,
				Region = region.GetRegionData()
			});
			return region;
		}

		public static Region DrawImage(Graphics grp, string objectName, System.Drawing.Size osize, string xaml, FlipType flipType, Rotate Rotation)
		{
			System.Drawing.Size size = new System.Drawing.Size(1, 1);
			size = ((Rotation == Rotate.Rot_90 || Rotation == Rotate.Rot_270) ? new System.Drawing.Size(osize.Height, osize.Width) : new System.Drawing.Size(osize.Width, osize.Height));
			double num = 1.0;
			double num2 = 1.0;
			KeyImg keyImg = new KeyImg
			{
				ObjectName = objectName,
				ImageSize = size,
				flipType = flipType,
				Rotation = Rotation
			};
			string keys = JsonConvert.SerializeObject((object)keyImg).Replace("[", "").Replace("]", "")
				.Replace(",", "")
				.Replace(":", "")
				.Replace("\"", "")
				.Replace("{", "")
				.Replace("}", "");
			if (Root.dicImg.Any((Img p) => p.key == keys))
			{
				Img img = (from p in Root.dicImg
						   where p.key == keys
						   select p).FirstOrDefault();
				grp.SmoothingMode = SmoothingMode.AntiAlias;
				grp.DrawImage(rect: new RectangleF(0f, 0f, osize.Width, osize.Height), image: img.Bmp);
				return new Region(img.Region);
			}
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
					double y = (double)ellipse.GetValue(Canvas.TopProperty);
					double x = (double)ellipse.GetValue(Canvas.LeftProperty);
					double width2 = ellipse.Width;
					double height2 = ellipse.Height;
					Rect rect2 = new Rect(x, y, width2, height2);
					GraphicsPath path = rect2.EToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (ellipse.Fill != null && ellipse.Fill is SolidColorBrush)
					{
						SolidColorBrush brush = new SolidColorBrush((ellipse.Fill as SolidColorBrush).Color);
						System.Drawing.Brush brush2 = brush.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush2, path);
						graphics2.FillPath(brush2, path);
					}
					if (ellipse.Stroke != null)
					{
						System.Windows.Media.Pen pen = new System.Windows.Media.Pen(ellipse.Stroke, ellipse.StrokeThickness * (double)(float)num);
						System.Drawing.Pen pen2 = pen.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen2, path);
						graphics2.DrawPath(pen2, path);
					}
				}
				if (child is System.Windows.Shapes.Rectangle)
				{
					System.Windows.Shapes.Rectangle rectangle = child as System.Windows.Shapes.Rectangle;
					double y2 = (double)rectangle.GetValue(Canvas.TopProperty);
					double x2 = (double)rectangle.GetValue(Canvas.LeftProperty);
					double width3 = rectangle.Width;
					double height3 = rectangle.Height;
					Rect rect3 = new Rect(x2, y2, width3, height3);
					GraphicsPath path2 = rect3.RToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (rectangle.Fill != null && rectangle.Fill is SolidColorBrush)
					{
						SolidColorBrush brush3 = new SolidColorBrush((rectangle.Fill as SolidColorBrush).Color);
						System.Drawing.Brush brush4 = brush3.ToGdiPlus(rect3, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush4, path2);
						graphics2.FillPath(brush4, path2);
					}
					if (rectangle.Stroke != null)
					{
						System.Windows.Media.Pen pen3 = new System.Windows.Media.Pen(rectangle.Stroke, rectangle.StrokeThickness * (double)(float)num);
						System.Drawing.Pen pen4 = pen3.ToGdiPlus(rect3, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen4, path2);
						graphics2.DrawPath(pen4, path2);
					}
				}
				if (child is System.Windows.Shapes.Path)
				{
					System.Windows.Shapes.Path path3 = child as System.Windows.Shapes.Path;
					Geometry data = path3.Data;
					GraphicsPath path4 = data.ToGdiPlus((float)num, (float)num2, 0f, 0f);
					double y3 = (double)path3.GetValue(Canvas.TopProperty);
					double x3 = (double)path3.GetValue(Canvas.LeftProperty);
					double width4 = path3.Width;
					double height4 = path3.Height;
					Rect bounds = new Rect(x3, y3, width4, height4);
					if (path3.Fill != null && path3.Fill is SolidColorBrush)
					{
						SolidColorBrush brush5 = new SolidColorBrush((path3.Fill as SolidColorBrush).Color);
						System.Drawing.Brush brush6 = brush5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush6, path4);
						graphics2.FillPath(brush6, path4);
					}
					if (path3.Stroke != null)
					{
						System.Windows.Media.Pen pen5 = new System.Windows.Media.Pen(path3.Stroke, path3.StrokeThickness * (double)(float)num);
						System.Drawing.Pen pen6 = pen5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
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
			Root.dicImg.Add(new Img
			{
				key = keys,
				Bmp = bitmap,
				Region = region.GetRegionData()
			});
			return region;
		}

		public static Region DrawImage(Graphics grp, string objectName, System.Drawing.Size osize, string xaml, List<ElementSetting> els, FlipType flipType, Rotate Rotation)
		{
			System.Drawing.Size size = new System.Drawing.Size(1, 1);
			size = ((Rotation == Rotate.Rot_90 || Rotation == Rotate.Rot_270) ? new System.Drawing.Size(osize.Height, osize.Width) : new System.Drawing.Size(osize.Width, osize.Height));
			double num = 1.0;
			double num2 = 1.0;
			KeyImg keyImg = new KeyImg
			{
				ObjectName = objectName,
				ImageSize = size,
				ListElementSetting = els,
				flipType = flipType,
				Rotation = Rotation
			};
			string keys = JsonConvert.SerializeObject((object)keyImg).Replace("[", "").Replace("]", "")
				.Replace(",", "")
				.Replace(":", "")
				.Replace("\"", "")
				.Replace("{", "")
				.Replace("}", "");
			if (Root.dicImg.Any((Img p) => p.key == keys))
			{
				Img img = (from p in Root.dicImg
						   where p.key == keys
						   select p).FirstOrDefault();
				grp.SmoothingMode = SmoothingMode.AntiAlias;
				grp.DrawImage(rect: new RectangleF(0f, 0f, osize.Width, osize.Height), image: img.Bmp);
				return new Region(img.Region);
			}
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
					Ellipse elp = child as Ellipse;
					if (els.Any((ElementSetting p) => p.ElementName == elp.Name && !p.Visible))
					{
						continue;
					}
					double y = (double)elp.GetValue(Canvas.TopProperty);
					double x = (double)elp.GetValue(Canvas.LeftProperty);
					double width2 = elp.Width;
					double height2 = elp.Height;
					Rect rect2 = new Rect(x, y, width2, height2);
					GraphicsPath path2 = rect2.EToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (elp.Fill != null && elp.Fill is SolidColorBrush)
					{
						SolidColorBrush brush = new SolidColorBrush((elp.Fill as SolidColorBrush).Color);
						if (els.Any((ElementSetting p) => p.ElementName == elp.Name && p.FillColor != System.Drawing.Color.Empty))
						{
							ElementSetting elementSetting = (from p in els
															 where p.ElementName == elp.Name
															 select p).FirstOrDefault();
							System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(elementSetting.FillColor.A, elementSetting.FillColor.R, elementSetting.FillColor.G, elementSetting.FillColor.B);
							brush = new SolidColorBrush(color);
						}
						System.Drawing.Brush brush2 = brush.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush2, path2);
						graphics2.FillPath(brush2, path2);
					}
					if (elp.Stroke != null)
					{
						System.Windows.Media.Pen pen = new System.Windows.Media.Pen(elp.Stroke, elp.StrokeThickness * (double)(float)num);
						if (els.Any((ElementSetting p) => p.ElementName == elp.Name && p.DrawColor != System.Drawing.Color.Empty))
						{
							ElementSetting elementSetting2 = (from p in els
															  where p.ElementName == elp.Name
															  select p).FirstOrDefault();
							System.Windows.Media.Color color2 = System.Windows.Media.Color.FromArgb(elementSetting2.DrawColor.A, elementSetting2.DrawColor.R, elementSetting2.DrawColor.G, elementSetting2.DrawColor.B);
							pen = new System.Windows.Media.Pen(new SolidColorBrush(color2), (elp.StrokeThickness * (double)(float)num > 1.0) ? (elp.StrokeThickness * (double)(float)num) : 1.5);
						}
						System.Drawing.Pen pen2 = pen.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen2, path2);
						graphics2.DrawPath(pen2, path2);
					}
				}
				if (child is System.Windows.Shapes.Rectangle)
				{
					System.Windows.Shapes.Rectangle rec = child as System.Windows.Shapes.Rectangle;
					if (els.Any((ElementSetting p) => p.ElementName == rec.Name && !p.Visible))
					{
						continue;
					}
					double y2 = (double)rec.GetValue(Canvas.TopProperty);
					double x2 = (double)rec.GetValue(Canvas.LeftProperty);
					double width3 = rec.Width;
					double height3 = rec.Height;
					Rect rect3 = new Rect(x2, y2, width3, height3);
					GraphicsPath path3 = rect3.RToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (rec.Fill != null && rec.Fill is SolidColorBrush)
					{
						SolidColorBrush brush3 = new SolidColorBrush((rec.Fill as SolidColorBrush).Color);
						if (els.Any((ElementSetting p) => p.ElementName == rec.Name && p.FillColor != System.Drawing.Color.Empty))
						{
							ElementSetting elementSetting3 = (from p in els
															  where p.ElementName == rec.Name
															  select p).FirstOrDefault();
							System.Windows.Media.Color color3 = System.Windows.Media.Color.FromArgb(elementSetting3.FillColor.A, elementSetting3.FillColor.R, elementSetting3.FillColor.G, elementSetting3.FillColor.B);
							brush3 = new SolidColorBrush(color3);
						}
						System.Drawing.Brush brush4 = brush3.ToGdiPlus(rect3, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush4, path3);
						graphics2.FillPath(brush4, path3);
					}
					if (rec.Stroke != null)
					{
						System.Windows.Media.Pen pen3 = new System.Windows.Media.Pen(rec.Stroke, rec.StrokeThickness * (double)(float)num);
						if (els.Any((ElementSetting p) => p.ElementName == rec.Name && p.DrawColor != System.Drawing.Color.Empty))
						{
							ElementSetting elementSetting4 = (from p in els
															  where p.ElementName == rec.Name
															  select p).FirstOrDefault();
							System.Windows.Media.Color color4 = System.Windows.Media.Color.FromArgb(elementSetting4.DrawColor.A, elementSetting4.DrawColor.R, elementSetting4.DrawColor.G, elementSetting4.DrawColor.B);
							pen3 = new System.Windows.Media.Pen(new SolidColorBrush(color4), (rec.StrokeThickness * (double)(float)num > 1.0) ? (rec.StrokeThickness * (double)(float)num) : 1.5);
						}
						System.Drawing.Pen pen4 = pen3.ToGdiPlus(rect3, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen4, path3);
						graphics2.DrawPath(pen4, path3);
					}
				}
				if (child is System.Windows.Shapes.Path)
				{
					System.Windows.Shapes.Path path = child as System.Windows.Shapes.Path;
					if (!els.Any((ElementSetting p) => p.ElementName == path.Name && !p.Visible))
					{
						Geometry data = path.Data;
						GraphicsPath path4 = data.ToGdiPlus((float)num, (float)num2, 0f, 0f);
						double y3 = (double)path.GetValue(Canvas.TopProperty);
						double x3 = (double)path.GetValue(Canvas.LeftProperty);
						double width4 = path.Width;
						double height4 = path.Height;
						Rect bounds = new Rect(x3, y3, width4, height4);
						if (path.Fill != null && path.Fill is SolidColorBrush)
						{
							SolidColorBrush brush5 = new SolidColorBrush((path.Fill as SolidColorBrush).Color);
							if (els.Any((ElementSetting p) => p.ElementName == path.Name && p.FillColor != System.Drawing.Color.Empty))
							{
								ElementSetting elementSetting5 = (from p in els
																  where p.ElementName == path.Name
																  select p).FirstOrDefault();
								System.Windows.Media.Color color5 = System.Windows.Media.Color.FromArgb(elementSetting5.FillColor.A, elementSetting5.FillColor.R, elementSetting5.FillColor.G, elementSetting5.FillColor.B);
								brush5 = new SolidColorBrush(color5);
							}
							System.Drawing.Brush brush6 = brush5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
							graphics.FillPath(brush6, path4);
							graphics2.FillPath(brush6, path4);
						}
						if (path.Stroke != null)
						{
							System.Windows.Media.Pen pen5 = new System.Windows.Media.Pen(path.Stroke, path.StrokeThickness * (double)(float)num);
							if (els.Any((ElementSetting p) => p.ElementName == path.Name && p.DrawColor != System.Drawing.Color.Empty))
							{
								ElementSetting elementSetting6 = (from p in els
																  where p.ElementName == path.Name
																  select p).FirstOrDefault();
								System.Windows.Media.Color color6 = System.Windows.Media.Color.FromArgb(elementSetting6.DrawColor.A, elementSetting6.DrawColor.R, elementSetting6.DrawColor.G, elementSetting6.DrawColor.B);
								pen5 = new System.Windows.Media.Pen(new SolidColorBrush(color6), (path.StrokeThickness * (double)(float)num > 1.0) ? (path.StrokeThickness * (double)(float)num) : 1.5);
							}
							System.Drawing.Pen pen6 = pen5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
							graphics.DrawPath(pen6, path4);
							graphics2.DrawPath(pen6, path4);
						}
					}
				}
			}
			grp.SmoothingMode = SmoothingMode.AntiAlias;
			FlipBitmap(bitmap, flipType);
			FlipBitmap(bitmap2, flipType);
			RotateBitmap(bitmap, Rotation);
			RotateBitmap(bitmap2, Rotation);
			Region region = CreateRegion(bitmap2, System.Drawing.Color.Magenta);
			RectangleF rect4 = new RectangleF(0f, 0f, osize.Width, osize.Height);
			grp.DrawImage(bitmap, rect4);
			Root.dicImg.Add(new Img
			{
				key = keys,
				Bmp = bitmap,
				Region = region.GetRegionData()
			});
			return region;
		}

		public static Region DrawImage(Graphics grp, string objectName, System.Drawing.Size osize, string xaml, System.Drawing.Color targetColor, FlipType flipType, Rotate Rotation)
		{
			System.Drawing.Size size = new System.Drawing.Size(1, 1);
			size = ((Rotation == Rotate.Rot_90 || Rotation == Rotate.Rot_270) ? new System.Drawing.Size(osize.Height, osize.Width) : new System.Drawing.Size(osize.Width, osize.Height));
			double num = 1.0;
			double num2 = 1.0;
			KeyImg keyImg = new KeyImg
			{
				ObjectName = objectName,
				ImageSize = size,
				flipType = flipType,
				Rotation = Rotation,
				TargetColor = targetColor
			};
			string keys = JsonConvert.SerializeObject((object)keyImg).Replace("[", "").Replace("]", "")
				.Replace(",", "")
				.Replace(":", "")
				.Replace("\"", "")
				.Replace("{", "")
				.Replace("}", "");
			if (Root.dicImg.Any((Img p) => p.key == keys))
			{
				Img img = (from p in Root.dicImg
						   where p.key == keys
						   select p).FirstOrDefault();
				grp.SmoothingMode = SmoothingMode.AntiAlias;
				grp.DrawImage(rect: new RectangleF(0f, 0f, osize.Width, osize.Height), image: img.Bmp);
				return new Region(img.Region);
			}
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
					double y = (double)ellipse.GetValue(Canvas.TopProperty);
					double x = (double)ellipse.GetValue(Canvas.LeftProperty);
					double width2 = ellipse.Width;
					double height2 = ellipse.Height;
					Rect rect2 = new Rect(x, y, width2, height2);
					GraphicsPath path = rect2.EToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (ellipse.Fill != null && ellipse.Fill is SolidColorBrush)
					{
						SolidColorBrush brush = new SolidColorBrush((ellipse.Fill as SolidColorBrush).Color);
						System.Drawing.Brush brush2 = brush.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush2, path);
						graphics2.FillPath(brush2, path);
					}
					if (ellipse.Stroke != null)
					{
						System.Windows.Media.Pen pen = new System.Windows.Media.Pen(ellipse.Stroke, ellipse.StrokeThickness * (double)(float)num);
						System.Drawing.Pen pen2 = pen.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen2, path);
						graphics2.DrawPath(pen2, path);
					}
				}
				if (child is System.Windows.Shapes.Rectangle)
				{
					System.Windows.Shapes.Rectangle rectangle = child as System.Windows.Shapes.Rectangle;
					double y2 = (double)rectangle.GetValue(Canvas.TopProperty);
					double x2 = (double)rectangle.GetValue(Canvas.LeftProperty);
					double width3 = rectangle.Width;
					double height3 = rectangle.Height;
					Rect rect3 = new Rect(x2, y2, width3, height3);
					GraphicsPath path2 = rect3.RToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (rectangle.Fill != null && rectangle.Fill is SolidColorBrush)
					{
						SolidColorBrush brush3 = new SolidColorBrush((rectangle.Fill as SolidColorBrush).Color);
						System.Drawing.Brush brush4 = brush3.ToGdiPlus(rect3, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush4, path2);
						graphics2.FillPath(brush4, path2);
					}
					if (rectangle.Stroke != null)
					{
						System.Windows.Media.Pen pen3 = new System.Windows.Media.Pen(rectangle.Stroke, rectangle.StrokeThickness * (double)(float)num);
						System.Drawing.Pen pen4 = pen3.ToGdiPlus(rect3, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen4, path2);
						graphics2.DrawPath(pen4, path2);
					}
				}
				if (child is System.Windows.Shapes.Path)
				{
					System.Windows.Shapes.Path path3 = child as System.Windows.Shapes.Path;
					Geometry data = path3.Data;
					GraphicsPath path4 = data.ToGdiPlus((float)num, (float)num2, 0f, 0f);
					double y3 = (double)path3.GetValue(Canvas.TopProperty);
					double x3 = (double)path3.GetValue(Canvas.LeftProperty);
					double width4 = path3.Width;
					double height4 = path3.Height;
					Rect bounds = new Rect(x3, y3, width4, height4);
					if (path3.Fill != null && path3.Fill is SolidColorBrush)
					{
						SolidColorBrush brush5 = new SolidColorBrush((path3.Fill as SolidColorBrush).Color);
						System.Drawing.Brush brush6 = brush5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush6, path4);
						graphics2.FillPath(brush6, path4);
					}
					if (path3.Stroke != null)
					{
						System.Windows.Media.Pen pen5 = new System.Windows.Media.Pen(path3.Stroke, path3.StrokeThickness * (double)(float)num);
						System.Drawing.Pen pen6 = pen5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen6, path4);
						graphics2.DrawPath(pen6, path4);
					}
				}
			}
			grp.SmoothingMode = SmoothingMode.AntiAlias;
			RectangleF rect4 = new RectangleF(0f, 0f, osize.Width, osize.Height);
			bitmap = bitmap.ColorShade(targetColor);
			FlipBitmap(bitmap, flipType);
			FlipBitmap(bitmap2, flipType);
			RotateBitmap(bitmap, Rotation);
			RotateBitmap(bitmap2, Rotation);
			grp.DrawImage(bitmap, rect4);
			Region region = CreateRegion(bitmap2, System.Drawing.Color.Magenta);
			Root.dicImg.Add(new Img
			{
				key = keys,
				Bmp = bitmap,
				Region = region.GetRegionData()
			});
			return region;
		}

		public static List<string> DrawImg(Graphics grp, string objectName, System.Drawing.Size size, string xaml, string ElementHighLight)
		{
			double num = 1.0;
			double num2 = 1.0;
			List<string> list = new List<string>();
			Bitmap image = new Bitmap(size.Width, size.Height);
			Graphics graphics = Graphics.FromImage(image);
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.Clear(System.Drawing.Color.Transparent);
			StringReader input = new StringReader(xaml);
			XmlReader reader = XmlReader.Create(input);
			Canvas canvas = (Canvas)XamlReader.Load(reader);
			double width = canvas.Width;
			double height = canvas.Height;
			num = (double)size.Width / width;
			num2 = (double)size.Height / height;
			UIElement uIElement = null;
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
					Rect rect = new Rect(x, y, width2, height2);
					GraphicsPath path = rect.EToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (ellipse.Fill != null && ellipse.Fill is SolidColorBrush)
					{
						System.Windows.Media.Color color = (ellipse.Fill as SolidColorBrush).Color;
						color.A = 150;
						SolidColorBrush brush = new SolidColorBrush(color);
						System.Drawing.Brush brush2 = brush.ToGdiPlus(rect, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush2, path);
					}
					if (ellipse.Stroke != null)
					{
						System.Windows.Media.Pen pen = new System.Windows.Media.Pen(ellipse.Stroke, ellipse.StrokeThickness * (double)(float)num);
						System.Drawing.Pen pen2 = pen.ToGdiPlus(rect, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen2, path);
					}
					if (ElementHighLight == ellipse.Name)
					{
						uIElement = child;
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
					Rect rect2 = new Rect(x2, y2, width3, height3);
					GraphicsPath path2 = rect2.RToGdiPlus((float)num, (float)num2, 0f, 0f);
					if (rectangle.Fill != null && rectangle.Fill is SolidColorBrush)
					{
						System.Windows.Media.Color color2 = (rectangle.Fill as SolidColorBrush).Color;
						color2.A = 150;
						SolidColorBrush brush3 = new SolidColorBrush(color2);
						System.Drawing.Brush brush4 = brush3.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush4, path2);
					}
					if (rectangle.Stroke != null)
					{
						System.Windows.Media.Pen pen3 = new System.Windows.Media.Pen(rectangle.Stroke, rectangle.StrokeThickness * (double)(float)num);
						System.Drawing.Pen pen4 = pen3.ToGdiPlus(rect2, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen4, path2);
					}
					if (ElementHighLight == rectangle.Name)
					{
						uIElement = child;
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
						color3.A = 150;
						SolidColorBrush brush5 = new SolidColorBrush(color3);
						System.Drawing.Brush brush6 = brush5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
						graphics.FillPath(brush6, path4);
					}
					if (path3.Stroke != null)
					{
						System.Windows.Media.Pen pen5 = new System.Windows.Media.Pen(path3.Stroke, path3.StrokeThickness * (double)(float)num);
						System.Drawing.Pen pen6 = pen5.ToGdiPlus(bounds, (float)num, (float)num2, 0f, 0f);
						graphics.DrawPath(pen6, path4);
					}
					if (ElementHighLight == path3.Name)
					{
						uIElement = child;
					}
				}
			}
			if (uIElement != null)
			{
				if (uIElement is Ellipse)
				{
					Ellipse ellipse2 = uIElement as Ellipse;
					double y4 = (double)ellipse2.GetValue(Canvas.TopProperty);
					double x4 = (double)ellipse2.GetValue(Canvas.LeftProperty);
					double width5 = ellipse2.Width;
					double height5 = ellipse2.Height;
					Rect rect3 = new Rect(x4, y4, width5, height5);
					GraphicsPath path5 = rect3.EToGdiPlus((float)num, (float)num2, 0f, 0f);
					System.Windows.Media.Pen pen7 = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Red, ellipse2.StrokeThickness * (double)(float)num);
					System.Drawing.Pen pen8 = pen7.ToGdiPlus(rect3, (float)num, (float)num2, 0f, 0f);
					graphics.DrawPath(pen8, path5);
				}
				if (uIElement is System.Windows.Shapes.Rectangle)
				{
					System.Windows.Shapes.Rectangle rectangle2 = uIElement as System.Windows.Shapes.Rectangle;
					double y5 = (double)rectangle2.GetValue(Canvas.TopProperty);
					double x5 = (double)rectangle2.GetValue(Canvas.LeftProperty);
					double width6 = rectangle2.Width;
					double height6 = rectangle2.Height;
					Rect rect4 = new Rect(x5, y5, width6, height6);
					GraphicsPath path6 = rect4.RToGdiPlus((float)num, (float)num2, 0f, 0f);
					System.Windows.Media.Pen pen9 = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Red, rectangle2.StrokeThickness * (double)(float)num);
					System.Drawing.Pen pen10 = pen9.ToGdiPlus(rect4, (float)num, (float)num2, 0f, 0f);
					graphics.DrawPath(pen10, path6);
				}
				if (uIElement is System.Windows.Shapes.Path)
				{
					System.Windows.Shapes.Path path7 = uIElement as System.Windows.Shapes.Path;
					Geometry data2 = path7.Data;
					GraphicsPath path8 = data2.ToGdiPlus((float)num, (float)num2, 0f, 0f);
					double y6 = (double)path7.GetValue(Canvas.TopProperty);
					double x6 = (double)path7.GetValue(Canvas.LeftProperty);
					double width7 = path7.Width;
					double height7 = path7.Height;
					Rect bounds2 = new Rect(x6, y6, width7, height7);
					System.Windows.Media.Pen pen11 = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Red, path7.StrokeThickness * (double)(float)num);
					System.Drawing.Pen pen12 = pen11.ToGdiPlus(bounds2, (float)num, (float)num2, 0f, 0f);
					graphics.DrawPath(pen12, path8);
				}
			}
			grp.SmoothingMode = SmoothingMode.AntiAlias;
			RectangleF rect5 = new RectangleF(0f, 0f, size.Width, size.Height);
			grp.DrawImage(image, rect5);
			return list;
		}

		public static Region BitmapShade(Graphics grp, string objectName, System.Drawing.Size osize, string xaml, FlipType flipType, Rotate Rotation, FillMode mode, System.Drawing.Color newColor)
		{
			System.Drawing.Size size = new System.Drawing.Size(1, 1);
			size = ((Rotation == Rotate.Rot_90 || Rotation == Rotate.Rot_270) ? new System.Drawing.Size(osize.Height, osize.Width) : new System.Drawing.Size(osize.Width, osize.Height));
			double num = 1.0;
			double num2 = 1.0;
			KeyImg keyImg = new KeyImg
			{
				ObjectName = objectName,
				ImageSize = size,
				flipType = flipType,
				Rotation = Rotation,
				TargetColor = newColor,
				mode = mode
			};
			string keys = JsonConvert.SerializeObject((object)keyImg).Replace("[", "").Replace("]", "")
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
						switch (mode)
						{
							case FillMode.Shaded:
								color = ColorHelper.GdiToMedia(ColorHelper.GrayscaleToColorGradient(ColorHelper.MediaToGdi(color), newColor));
								break;
							case FillMode.Solid:
								color = ColorHelper.GdiToMedia(newColor);
								break;
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
						switch (mode)
						{
							case FillMode.Shaded:
								color2 = ColorHelper.GdiToMedia(ColorHelper.GrayscaleToColorGradient(ColorHelper.MediaToGdi(color2), newColor));
								break;
							case FillMode.Solid:
								color2 = ColorHelper.GdiToMedia(newColor);
								break;
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
						switch (mode)
						{
							case FillMode.Shaded:
								color3 = ColorHelper.GdiToMedia(ColorHelper.GrayscaleToColorGradient(ColorHelper.MediaToGdi(color3), newColor));
								break;
							case FillMode.Solid:
								color3 = ColorHelper.GdiToMedia(newColor);
								break;
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

		public static Region CreateRegionExt(Bitmap bmp, System.Drawing.Color rmvColor)
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
					else if (bmp.GetPixel(j, i) != right)
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

		public static Bitmap ColorShade(this Bitmap sourceBitmap, System.Drawing.Color color)
		{
			float num = (int)color.B;
			float num2 = (int)color.G;
			float num3 = (int)color.R;
			BitmapData bitmapData = sourceBitmap.LockBits(new System.Drawing.Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			byte[] array = new byte[bitmapData.Stride * bitmapData.Height];
			Marshal.Copy(bitmapData.Scan0, array, 0, array.Length);
			sourceBitmap.UnlockBits(bitmapData);
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			for (int i = 0; i + 4 < array.Length; i += 4)
			{
				num4 = (float)(int)array[i] * num;
				num5 = (float)(int)array[i + 1] * num2;
				num6 = (float)(int)array[i + 2] * num3;
				if (num4 < 0f)
				{
					num4 = 0f;
				}
				if (num5 < 0f)
				{
					num5 = 0f;
				}
				if (num6 < 0f)
				{
					num6 = 0f;
				}
				array[i] = (byte)num4;
				array[i + 1] = (byte)num5;
				array[i + 2] = (byte)num6;
			}
			Bitmap bitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
			BitmapData bitmapData2 = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			Marshal.Copy(array, 0, bitmapData2.Scan0, array.Length);
			bitmap.UnlockBits(bitmapData2);
			return bitmap;
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
	// XamlHelper.FillMode
	public enum FillMode
	{
		Original,
		Shaded,
		Solid
	}
	// XamlHelper.FlipType
	public enum FlipType
	{
		Horizontal,
		Vertical,
		Both,
		None
	}
	// XamlHelper.Rotate
	public enum Rotate
	{
		Rot_90,
		Rot_180,
		Rot_270,
		None
	}
}
