

// XamlHelper.ColorHelper
using System;
using System.Drawing;
using System.Windows.Media;
using XamlHelper;
namespace XamlHelper
{
	public static class ColorHelper
	{
		public static string ColorBase = "#FFffffff";

		public static string ToHexColor(System.Drawing.Color color)
		{
			return string.Format("#{0}{1}{2}", color.R.ToString("X2"), color.G.ToString("X2"), color.B.ToString("X2"));
		}

		public static string HexConverter(System.Drawing.Color c)
		{
			return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
		}

		public static System.Drawing.Color ToMediaColor(System.Drawing.Color color)
		{
			return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		public static System.Drawing.Color GrayscaleToColorGradient(System.Drawing.Color sourceColor, System.Drawing.Color targetColor)
		{
			if (sourceColor == System.Drawing.Color.FromArgb(0, 0, 0))
			{
				return System.Drawing.Color.FromArgb(0, 0, 0);
			}
			if (sourceColor == System.Drawing.Color.FromArgb(255, 255, 255))
			{
				return System.Drawing.Color.FromArgb(255, 255, 255);
			}
			byte b = (byte)((sourceColor.R + sourceColor.G + sourceColor.B) / 3);
			if (b < 128)
			{
				return InterpolatedColor(System.Drawing.Color.FromArgb(sourceColor.A, System.Drawing.Color.Black.R, System.Drawing.Color.Black.G, System.Drawing.Color.Black.B), targetColor, 1000 * b / 128);
			}
			return InterpolatedColor(System.Drawing.Color.FromArgb(sourceColor.A, targetColor.R, targetColor.G, targetColor.B), System.Drawing.Color.White, 1000 * (b - 128) / 128);
		}

		public static System.Drawing.Color Darken(System.Drawing.Color color, double darkenAmount)
		{
			HSLColor hSLColor = new HSLColor(color);
			hSLColor.Luminosity *= darkenAmount;
			return hSLColor;
		}

		public static System.Windows.Media.Color GdiToMedia(System.Drawing.Color color)
		{
			return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		public static System.Drawing.Color MediaToGdi(System.Windows.Media.Color mediacolor)
		{
			return System.Drawing.Color.FromArgb(mediacolor.A, mediacolor.R, mediacolor.G, mediacolor.B);
		}

		public static System.Drawing.Color InterpolatedColor(System.Drawing.Color srcColor, System.Drawing.Color dstColor, double amount)
		{
			if (amount == 0.0)
			{
				return srcColor;
			}
			if (amount == 1000.0)
			{
				return dstColor;
			}
			amount = (int)Math.Min(Math.Max(amount, 0.0), 1000.0);
			byte red = (byte)((double)(int)srcColor.R + amount * (double)(dstColor.R - srcColor.R) / 1000.0);
			byte green = (byte)((double)(int)srcColor.G + amount * (double)(dstColor.G - srcColor.G) / 1000.0);
			byte blue = (byte)((double)(int)srcColor.B + amount * (double)(dstColor.B - srcColor.B) / 1000.0);
			return System.Drawing.Color.FromArgb(srcColor.A, red, green, blue);
		}
	}
}
