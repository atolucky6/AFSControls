using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
 
//using WindowsBase;

namespace vSymbolfactory.XamlHelper
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

    [Serializable]
    public class ElementSetting
    {
        public string ElementName
        {
            get;
            set;
        }

        public System.Drawing.Color DrawColor
        {
            get;
            set;
        }

        public System.Drawing.Color FillColor
        {
            get;
            set;
        }

        public bool Visible
        {
            get;
            set;
        }

        public object TagValue
        {
            get;
            set;
        }
    }
    //public enum FillMode
    //{
    //    Original,
    //    Shaded,
    //    Alternate,
    //    Winding
    //}

    public enum FlipType
    {
        Horizontal,
        Vertical,
        Both,
        None
    }
    public class HSLColor
    {
        private double hue = 1.0;

        private double saturation = 1.0;

        private double luminosity = 1.0;

        private const double scale = 240.0;

        public double Hue
        {
            get
            {
                return hue * 240.0;
            }
            set
            {
                hue = CheckRange(value / 240.0);
            }
        }

        public double Saturation
        {
            get
            {
                return saturation * 240.0;
            }
            set
            {
                saturation = CheckRange(value / 240.0);
            }
        }

        public double Luminosity
        {
            get
            {
                return luminosity * 240.0;
            }
            set
            {
                luminosity = CheckRange(value / 240.0);
            }
        }

        private double CheckRange(double value)
        {
            if (value < 0.0)
            {
                value = 0.0;
            }
            else if (value > 1.0)
            {
                value = 1.0;
            }
            return value;
        }

        public override string ToString()
        {
            return $"H: {Hue:#0.##} S: {Saturation:#0.##} L: {Luminosity:#0.##}";
        }

        public string ToRGBString()
        {
            System.Drawing.Color color = this;
            return $"R: {color.R:#0.##} G: {color.G:#0.##} B: {color.B:#0.##}";
        }

        public static implicit operator System.Drawing.Color(HSLColor hslColor)
        {
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            if (hslColor.luminosity != 0.0)
            {
                if (hslColor.saturation == 0.0)
                {
                    num = (num2 = (num3 = hslColor.luminosity));
                }
                else
                {
                    double temp = GetTemp2(hslColor);
                    double temp2 = 2.0 * hslColor.luminosity - temp;
                    num = GetColorComponent(temp2, temp, hslColor.hue + 0.33333333333333331);
                    num2 = GetColorComponent(temp2, temp, hslColor.hue);
                    num3 = GetColorComponent(temp2, temp, hslColor.hue - 0.33333333333333331);
                }
            }
            return System.Drawing.Color.FromArgb((int)(255.0 * num), (int)(255.0 * num2), (int)(255.0 * num3));
        }

        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            temp3 = MoveIntoRange(temp3);
            if (temp3 < 0.16666666666666666)
            {
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            }
            if (temp3 < 0.5)
            {
                return temp2;
            }
            if (temp3 < 2.0 / 3.0)
            {
                return temp1 + (temp2 - temp1) * (2.0 / 3.0 - temp3) * 6.0;
            }
            return temp1;
        }

        private static double MoveIntoRange(double temp3)
        {
            if (temp3 < 0.0)
            {
                temp3 += 1.0;
            }
            else if (temp3 > 1.0)
            {
                temp3 -= 1.0;
            }
            return temp3;
        }

        private static double GetTemp2(HSLColor hslColor)
        {
            if (hslColor.luminosity < 0.5)
            {
                return hslColor.luminosity * (1.0 + hslColor.saturation);
            }
            return hslColor.luminosity + hslColor.saturation - hslColor.luminosity * hslColor.saturation;
        }

        public static implicit operator HSLColor(System.Drawing.Color color)
        {
            HSLColor hSLColor = new HSLColor();
            hSLColor.hue = (double)color.GetHue() / 360.0;
            hSLColor.luminosity = color.GetBrightness();
            hSLColor.saturation = color.GetSaturation();
            return hSLColor;
        }

        public void SetRGB(int red, int green, int blue)
        {
            HSLColor hSLColor = System.Drawing.Color.FromArgb(red, green, blue);
            hue = hSLColor.hue;
            saturation = hSLColor.saturation;
            luminosity = hSLColor.luminosity;
        }

        public HSLColor()
        {
        }

        public HSLColor(System.Drawing.Color color)
        {
            SetRGB(color.R, color.G, color.B);
        }

        public HSLColor(int red, int green, int blue)
        {
            SetRGB(red, green, blue);
        }

        public HSLColor(double hue, double saturation, double luminosity)
        {
            Hue = hue;
            Saturation = saturation;
            Luminosity = luminosity;
        }
    }
    public class Img
    {
        public string key;

        public Bitmap Bmp;

        public RegionData Region;
    }
    public enum FillMode
    {
        Original,
        Shaded,
        Alternate,
        Winding
    }


    [Serializable]
    public class KeyImg
    {
        public System.Drawing.Color TargetColor;

        public string ObjectName;

        public System.Drawing.Size ImageSize;

        public List<ElementSetting> ListElementSetting = new List<ElementSetting>();

        public FlipType flipType;

        public Rotate Rotation;

        public FillMode mode;
    }
    public static class Root
    {
        public static Dictionary<string, string> xamlObject = new Dictionary<string, string>
    {
        {
            "ObjectValve",
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"Document\" Width=\"54.5594\" Height=\"56.19\" Clip=\"F1 M 0,0L 54.5594,0L 54.5594,56.19L 0,56.19L 0,0\">\r\n\t<Path x:Name=\"FbOff\" Width=\"31.9687\" Height=\"33.8333\" Canvas.Left=\"14.476\" Canvas.Top=\"11.2785\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF808080\" Fill=\"#FFE0E0E0\" Data=\"F1 M 16.1596,12.7785L 44.9158,12.7785L 44.9447,27.9323L 15.976,28.0417L 44.9158,27.9167L 44.9158,43.6118L 16.1596,43.6118L 16.1596,12.7785 Z \"/>\r\n\t<Path x:Name=\"FbOffFbOn\" Width=\"31.7748\" Height=\"33.8542\" Canvas.Left=\"14.5502\" Canvas.Top=\"11.4583\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF808080\" Fill=\"#FFE0E0E0\" Data=\"F1 M 16.0502,12.9791L 44.8064,12.9791L 44.825,43.7552L 16.0593,12.9583L 44.8064,43.8125L 16.0502,43.8125L 16.0502,12.9791 Z \"/>\r\n\t<Path x:Name=\"FbOn\" Width=\"31.7562\" Height=\"33.8333\" Canvas.Left=\"14.6743\" Canvas.Top=\"11.2542\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF008000\" Fill=\"#FF00FF00\" Data=\"F1 M 16.1743,12.7542L 44.9305,12.7542L 44.9305,43.5875L 31.093,43.5719L 31.1607,12.7698L 31.0773,43.5719L 16.1743,43.5875L 16.1743,12.7542 Z \"/>\r\n\t<Path x:Name=\"Test\" Width=\"11.275\" Height=\"9.96402\" Canvas.Left=\"25.0173\" Canvas.Top=\"0.0797119\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 29.6829,10.0437L 29.6829,1.18683L 25.0173,1.18683L 25.0173,0.0797119L 36.2924,0.0797119L 36.2924,1.18683L 31.6268,1.18683L 31.6268,10.0437L 29.6829,10.0437 Z \"/>\r\n\t<Path x:Name=\"Maint\" Width=\"13.0497\" Height=\"10.2036\" Canvas.Left=\"24.0096\" Canvas.Top=\"-3.05176e-005\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 24.0096,10.2036L 24.0096,-3.05176e-005L 26.8199,-3.05176e-005L 29.9109,7.181C 30.1966,7.8512 30.4044,8.35239 30.5345,8.68454C 30.6803,8.31549 30.9068,7.77298 31.2141,7.05701L 34.2786,-3.05176e-005L 37.0593,-3.05176e-005L 37.0593,10.2036L 35.1681,10.2036L 35.1681,1.55884L 31.4683,10.2036L 29.5741,10.2036L 25.9009,1.55884L 25.9009,10.2036L 24.0096,10.2036 Z \"/>\r\n\t<Rectangle x:Name=\"Interlocked\" Width=\"1.91841\" Height=\"9.63086\" Canvas.Left=\"29.3951\" Canvas.Top=\"0.209473\" Stretch=\"Fill\" Fill=\"#FF800080\"/>\r\n\t<Path x:Name=\"Out\" Width=\"9.37502\" Height=\"9.375\" Canvas.Left=\"26.3959\" Canvas.Top=\"23.4897\" Stretch=\"Fill\" StrokeThickness=\"3\" StrokeLineJoin=\"Round\" Stroke=\"#FF818C81\" Data=\"F1 M 31.0834,24.9897C 32.8438,24.9897 34.2709,26.4168 34.2709,28.1772C 34.2709,29.9377 32.8438,31.3647 31.0834,31.3647C 29.3229,31.3647 27.8959,29.9377 27.8959,28.1772C 27.8959,26.4168 29.3229,24.9897 31.0834,24.9897 Z \"/>\r\n\t<Path x:Name=\"InMan\" Width=\"16.5739\" Height=\"20.8253\" Canvas.Left=\"7.62939e-006\" Canvas.Top=\"35.3648\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF230FD2\" Data=\"F1 M 3.73941,40.356L 2.56574,39.7847L 1.1198,40.7296L 0.500008,43.2791L 0.676788,51.1004L 3.29461,54.0572L 3.85845,55.69L 10.0225,55.6812L 10.5206,54.2605L 13.4568,51.9544L 16.0739,48.4903L 16.0739,45.9903L 15.1242,45.2542L 12.2355,47.8347L 12.2254,41.3915L 12.1455,39.3485L 10.9255,38.2666L 9.44348,39.1807L 9.4465,46.5548L 9.42126,37.3731L 7.78525,36.0941L 7.03123,35.8648L 6.33652,36.9937L 6.37254,45.0361L 6.34314,38.0902L 5.37879,37.4087L 4.49277,37.8279L 3.78797,38.865L 3.7779,46.2901L 3.73941,40.356 Z \"/>\r\n\t<Path x:Name=\"SimFB\" Width=\"3.03127\" Height=\"15.0032\" Canvas.Left=\"51.5281\" Canvas.Top=\"34.636\" Stretch=\"Fill\" Fill=\"#FFFF0000\" Data=\"F1 M 54.5281,34.6444L 51.5495,34.636L 51.5281,49.6392L 54.5281,49.6392L 54.5594,46.6341L 51.5339,46.6256L 51.5495,45.6411L 54.5437,45.6236\"/>\r\n</Canvas>\r\n\r\n\r\n\r\n"
        },
        {
            "MotorType1",
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"Document\" Width=\"56.8927\" Height=\"65.8567\" Clip=\"F1 M 0,0L 56.8927,0L 56.8927,65.8567L 0,65.8567L 0,0\">\r\n\t<Path x:Name=\"Test\" Width=\"11.2751\" Height=\"9.96399\" Canvas.Left=\"23.934\" Canvas.Top=\"0.079752\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 28.5995,10.0437L 28.5995,1.18687L 23.934,1.18687L 23.934,0.079752L 35.2091,0.079752L 35.2091,1.18687L 30.5436,1.18687L 30.5436,10.0437L 28.5995,10.0437 Z \"/>\r\n\t<Path x:Name=\"Maint\" Width=\"13.0497\" Height=\"10.2036\" Canvas.Left=\"22.9263\" Canvas.Top=\"4.00543e-005\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 22.9263,10.2036L 22.9263,4.00543e-005L 25.7366,4.00543e-005L 28.8276,7.18107C 29.1133,7.85124 29.3212,8.35246 29.4512,8.68461C 29.5969,8.31553 29.8235,7.77299 30.1309,7.05705L 33.1953,4.00543e-005L 35.976,4.00543e-005L 35.976,10.2036L 34.0847,10.2036L 34.0847,1.55888L 30.3849,10.2036L 28.4907,10.2036L 24.8176,1.55888L 24.8176,10.2036L 22.9263,10.2036 Z \"/>\r\n\t<Rectangle x:Name=\"Interlocked\" Width=\"1.9184\" Height=\"9.63086\" Canvas.Left=\"28.3118\" Canvas.Top=\"0.209513\" Stretch=\"Fill\" Fill=\"#FF800080\"/>\r\n\t<Ellipse x:Name=\"Run\" Width=\"44.562\" Height=\"44.9366\" Canvas.Left=\"7.44641\" Canvas.Top=\"11.2618\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF007E00\" Fill=\"#FF00FE00\"/>\r\n\t<Path x:Name=\"InMan\" Width=\"16.5739\" Height=\"20.8253\" Canvas.Left=\"0\" Canvas.Top=\"45.0315\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF230FD2\" Data=\"F1 M 3.73938,50.0227L 2.56573,49.4513L 1.11975,50.3963L 0.5,52.9458L 0.676758,60.7671L 3.29462,63.7239L 3.85846,65.3567L 10.0225,65.3479L 10.5206,63.9272L 13.4568,61.6211L 16.0739,58.157L 16.0739,55.657L 15.1242,54.9208L 12.2355,57.5014L 12.2254,51.0582L 12.1455,49.0152L 10.9255,47.9333L 9.44348,48.8474L 9.44647,56.2216L 9.42126,47.0398L 7.78522,45.7608L 7.03125,45.5315L 6.33655,46.6604L 6.37256,54.7028L 6.34314,47.757L 5.37878,47.0754L 4.49274,47.4947L 3.78796,48.5317L 3.77789,55.9568L 3.73938,50.0227 Z \"/>\r\n\t<Path x:Name=\"SimMode\" Width=\"4.885\" Height=\"23.378\" Canvas.Left=\"52.008\" Canvas.Top=\"22.719\" Stretch=\"Fill\" Fill=\"#FFFF0000\" Data=\"F1 M 56.8615,25.9778L 53.8829,25.9693L 53.8615,40.9726L 56.8615,40.9726L 56.8927,37.9674L 53.8672,37.9589L 53.8829,36.9745L 56.8771,36.9569\"/>\r\n\t<Path x:Name=\"Out\" Width=\"30.2084\" Height=\"28.0417\" Canvas.Left=\"15.0713\" Canvas.Top=\"16.345\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF007E00\" Fill=\"#FF00FE00\" Data=\"F1 M 44.2797,43.3867L 30.0714,17.345L 16.0713,43.345L 44.2797,43.3867 Z \"/>\r\n</Canvas>\r\n"
        },
        {
            "MotorType2",
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"Document\" Width=\"56.8927\" Height=\"65.8567\" Clip=\"F1 M 0,0L 56.8927,0L 56.8927,65.8567L 0,65.8567L 0,0\">\r\n\t<Path x:Name=\"Test\" Width=\"11.2751\" Height=\"9.96405\" Canvas.Left=\"23.934\" Canvas.Top=\"0.0797119\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 28.5995,10.0438L 28.5995,1.18689L 23.934,1.18689L 23.934,0.0797119L 35.2091,0.0797119L 35.2091,1.18689L 30.5436,1.18689L 30.5436,10.0438L 28.5995,10.0438 Z \"/>\r\n\t<Path x:Name=\"Maint\" Width=\"13.0497\" Height=\"10.2036\" Canvas.Left=\"22.9263\" Canvas.Top=\"0\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 22.9263,10.2036L 22.9263,0L 25.7366,0L 28.8276,7.18103C 29.1133,7.85126 29.3212,8.35242 29.4512,8.68457C 29.597,8.31555 29.8235,7.77301 30.1309,7.05701L 33.1953,0L 35.976,0L 35.976,10.2036L 34.0847,10.2036L 34.0847,1.5589L 30.3849,10.2036L 28.4908,10.2036L 24.8176,1.5589L 24.8176,10.2036L 22.9263,10.2036 Z \"/>\r\n\t<Rectangle x:Name=\"Interlocked\" Width=\"1.91837\" Height=\"9.63086\" Canvas.Left=\"28.3118\" Canvas.Top=\"0.209534\" Stretch=\"Fill\" Fill=\"#FF800080\"/>\r\n\t<Ellipse x:Name=\"Run\" Width=\"44.5619\" Height=\"44.9366\" Canvas.Left=\"7.44641\" Canvas.Top=\"11.2618\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF007E00\" Fill=\"#FF00FE00\"/>\r\n\t<Path x:Name=\"InMan\" Width=\"16.5739\" Height=\"20.8252\" Canvas.Left=\"0\" Canvas.Top=\"45.0315\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF230FD2\" Data=\"F1 M 3.73941,50.0228L 2.56573,49.4514L 1.11978,50.3962L 0.5,52.9458L 0.676788,60.7671L 3.29462,63.7239L 3.85846,65.3567L 10.0225,65.3479L 10.5206,63.9272L 13.4568,61.6211L 16.0739,58.157L 16.0739,55.657L 15.1242,54.9208L 12.2355,57.5013L 12.2254,51.0582L 12.1455,49.0152L 10.9255,47.9333L 9.44348,48.8474L 9.44647,56.2216L 9.42126,47.0398L 7.78522,45.7607L 7.03125,45.5315L 6.33655,46.6604L 6.37256,54.7028L 6.34314,47.757L 5.37881,47.0754L 4.49274,47.4946L 3.78796,48.5317L 3.77789,55.9568L 3.73941,50.0228 Z \"/>\r\n\t<Path x:Name=\"SimMode\" Width=\"4.885\" Height=\"23.031\" Canvas.Left=\"52.008\" Canvas.Top=\"23.219\" Stretch=\"Fill\" Fill=\"#FFFF0000\" Data=\"F1 M 56.8615,25.9778L 53.8829,25.9694L 53.8615,40.9727L 56.8615,40.9727L 56.8927,37.9674L 53.8672,37.959L 53.8829,36.9746L 56.8771,36.957\"/>\r\n\t<Path x:Name=\"Out\" Width=\"27.1458\" Height=\"29.8541\" Canvas.Left=\"13.0083\" Canvas.Top=\"18.7609\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF007E00\" Fill=\"#FF00FE00\" Data=\"F1 M 14.0083,33.1567L 39.1541,19.7609L 39.1333,47.615L 14.0083,33.1567 Z \"/>\r\n</Canvas>\r\n"
        },
        {
            "MotorType3",
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"Document\" Width=\"56.8927\" Height=\"65.8567\" Clip=\"F1 M 0,0L 56.8927,0L 56.8927,65.8567L 0,65.8567L 0,0\">\r\n\t<Path x:Name=\"Test\" Width=\"11.2751\" Height=\"9.96405\" Canvas.Left=\"23.9341\" Canvas.Top=\"0.0797329\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 28.5996,10.0438L 28.5996,1.18685L 23.9341,1.18685L 23.9341,0.0797329L 35.2091,0.0797329L 35.2091,1.18685L 30.5436,1.18685L 30.5436,10.0438L 28.5996,10.0438 Z \"/>\r\n\t<Path x:Name=\"Maint\" Width=\"13.0497\" Height=\"10.2036\" Canvas.Left=\"22.9263\" Canvas.Top=\"2.09808e-005\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 22.9263,10.2036L 22.9263,2.09808e-005L 25.7366,2.09808e-005L 28.8277,7.18105C 29.1133,7.85128 29.3212,8.35244 29.4512,8.68459C 29.597,8.31551 29.8235,7.77303 30.1309,7.05703L 33.1953,2.09808e-005L 35.9761,2.09808e-005L 35.9761,10.2036L 34.0848,10.2036L 34.0848,1.55892L 30.385,10.2036L 28.4908,10.2036L 24.8176,1.55892L 24.8176,10.2036L 22.9263,10.2036 Z \"/>\r\n\t<Rectangle x:Name=\"InterLocked\" Width=\"1.9184\" Height=\"9.63086\" Canvas.Left=\"28.3118\" Canvas.Top=\"0.209555\" Stretch=\"Fill\" Fill=\"#FF800080\"/>\r\n\t<Ellipse x:Name=\"Run\" Width=\"44.5619\" Height=\"44.9366\" Canvas.Left=\"7.44644\" Canvas.Top=\"11.2618\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF007E00\" Fill=\"#FF00FE00\"/>\r\n\t<Path x:Name=\"InMan\" Width=\"16.5739\" Height=\"20.8253\" Canvas.Left=\"0\" Canvas.Top=\"45.0315\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF230FD2\" Data=\"F1 M 3.73941,50.0228L 2.56573,49.4514L 1.11978,50.3963L 0.5,52.9458L 0.676788,60.7671L 3.29462,63.7238L 3.85846,65.3567L 10.0225,65.3479L 10.5206,63.9272L 13.4568,61.6211L 16.0739,58.1569L 16.0739,55.6569L 15.1242,54.9209L 12.2355,57.5014L 12.2254,51.0582L 12.1455,49.0152L 10.9255,47.9333L 9.44348,48.8474L 9.44647,56.2216L 9.42126,47.0398L 7.78522,45.7608L 7.03125,45.5315L 6.33655,46.6604L 6.37256,54.7028L 6.34314,47.757L 5.37881,47.0754L 4.49274,47.4947L 3.78796,48.5317L 3.77789,55.9567L 3.73941,50.0228 Z \"/>\r\n\t<Path x:Name=\"SimMode\" Width=\"4.885\" Height=\"23.365\" Canvas.Left=\"52.008\" Canvas.Top=\"21.635\" Stretch=\"Fill\" Fill=\"#FFFF0000\" Data=\"F1 M 56.8615,25.9778L 53.8829,25.9693L 53.8615,40.9726L 56.8615,40.9726L 56.8927,37.9674L 53.8673,37.9589L 53.8829,36.9746L 56.8771,36.957\"/>\r\n\t<Path x:Name=\"Out\" Width=\"24.0856\" Height=\"17.5695\" Canvas.Left=\"18.1126\" Canvas.Top=\"23.302\" Stretch=\"Fill\" Fill=\"#FF007E00\" Data=\"F1 M 42.1982,40.8714L 37.381,40.8714L 37.381,30.3572C 37.381,29.2225 37.4422,27.9688 37.5645,26.5963L 37.4469,26.5963C 37.2023,27.676 36.9812,28.4525 36.7836,28.9257L 31.8442,40.8714L 27.9632,40.8714L 22.9344,29.0473C 22.7964,28.7284 22.5753,27.9113 22.2711,26.5963L 22.1394,26.5963C 22.2648,28.3297 22.3276,29.85 22.3276,31.1572L 22.3276,40.8714L 18.1126,40.8714L 18.1126,23.302L 25.0043,23.302L 29.3086,33.7182C 29.6536,34.5496 29.9045,35.3862 30.0613,36.2281L 30.1507,36.2281C 30.4141,35.2555 30.6932,34.4097 30.988,33.6907L 35.2924,23.302L 42.1982,23.302L 42.1982,40.8714 Z \"/>\r\n</Canvas>\r\n"
        },
        {
            "MotorType4",
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"Document\" Width=\"56.8927\" Height=\"65.8567\" Clip=\"F1 M 0,0L 56.8927,0L 56.8927,65.8567L 0,65.8567L 0,0\">\r\n\t<Path x:Name=\"Test\" Width=\"11.275\" Height=\"9.96405\" Canvas.Left=\"23.9341\" Canvas.Top=\"0.0797119\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 28.5996,10.0438L 28.5996,1.18689L 23.9341,1.18689L 23.9341,0.0797119L 35.2091,0.0797119L 35.2091,1.18689L 30.5436,1.18689L 30.5436,10.0438L 28.5996,10.0438 Z \"/>\r\n\t<Path x:Name=\"Maint\" Width=\"13.0497\" Height=\"10.2036\" Canvas.Left=\"22.9263\" Canvas.Top=\"0\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 22.9263,10.2036L 22.9263,0L 25.7366,0L 28.8276,7.18103C 29.1133,7.85126 29.3212,8.35242 29.4512,8.68457C 29.597,8.31555 29.8235,7.77301 30.1309,7.05701L 33.1953,0L 35.9761,0L 35.9761,10.2036L 34.0848,10.2036L 34.0848,1.5589L 30.385,10.2036L 28.4908,10.2036L 24.8176,1.5589L 24.8176,10.2036L 22.9263,10.2036 Z \"/>\r\n\t<Rectangle x:Name=\"Interlocked\" Width=\"1.9184\" Height=\"9.63086\" Canvas.Left=\"28.3118\" Canvas.Top=\"0.209534\" Stretch=\"Fill\" Fill=\"#FF800080\"/>\r\n\t<Ellipse x:Name=\"Run\" Width=\"44.562\" Height=\"44.9366\" Canvas.Left=\"7.44641\" Canvas.Top=\"11.2617\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF007E00\" Fill=\"#FF00FE00\"/>\r\n\t<Path x:Name=\"InMan\" Width=\"16.574\" Height=\"20.8252\" Canvas.Left=\"0\" Canvas.Top=\"45.0314\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF230FD2\" Data=\"F1 M 3.73944,50.0227L 2.5658,49.4513L 1.11981,50.3962L 0.5,52.9457L 0.676819,60.767L 3.29468,63.7238L 3.85852,65.3567L 10.0225,65.3479L 10.5206,63.9272L 13.4569,61.6211L 16.074,58.1569L 16.074,55.6569L 15.1243,54.9208L 12.2355,57.5013L 12.2255,51.0582L 12.1455,49.0152L 10.9255,47.9333L 9.44348,48.8474L 9.44653,56.2215L 9.42126,47.0398L 7.78528,45.7607L 7.03125,45.5314L 6.33655,46.6603L 6.37256,54.7027L 6.34314,47.7569L 5.37885,47.0753L 4.4928,47.4946L 3.78796,48.5317L 3.77795,55.9567L 3.73944,50.0227 Z \"/>\r\n\t<Path x:Name=\"SimMode\" Width=\"4.031\" Height=\"20.004\" Canvas.Left=\"52.862\" Canvas.Top=\"23.469\" Stretch=\"Fill\" Fill=\"#FFFF0000\" Data=\"F1 M 56.8615,25.9778L 53.8829,25.9693L 53.8615,40.9726L 56.8615,40.9726L 56.8927,37.9674L 53.8673,37.9589L 53.8829,36.9745L 56.8771,36.957\"/>\r\n\t<Path x:Name=\"Out\" Width=\"26.0209\" Height=\"29.8542\" Canvas.Left=\"21.0083\" Canvas.Top=\"18.8858\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF007E00\" Fill=\"#FF00FE00\" Data=\"F1 M 46.0292,33.3858L 22.0292,19.8858L 22.0083,47.74L 46.0292,33.3858 Z \"/>\r\n</Canvas>\r\n"
        },
        {
            "MotorType5",
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"Document\" Width=\"56.8927\" Height=\"65.8567\" Clip=\"F1 M 0,0L 56.8927,0L 56.8927,65.8567L 0,65.8567L 0,0\">\r\n\t<Path x:Name=\"Test\" Width=\"11.2751\" Height=\"9.96405\" Canvas.Left=\"23.9341\" Canvas.Top=\"0.0797119\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 28.5996,10.0438L 28.5996,1.18689L 23.9341,1.18689L 23.9341,0.0797119L 35.2091,0.0797119L 35.2091,1.18689L 30.5436,1.18689L 30.5436,10.0438L 28.5996,10.0438 Z \"/>\r\n\t<Path x:Name=\"Maint\" Width=\"13.0497\" Height=\"10.2036\" Canvas.Left=\"22.9264\" Canvas.Top=\"0\" Stretch=\"Fill\" Fill=\"#FF800080\" Data=\"F1 M 22.9264,10.2036L 22.9264,0L 25.7367,0L 28.8277,7.18103C 29.1134,7.85126 29.3212,8.35242 29.4512,8.68457C 29.597,8.31555 29.8236,7.77301 30.1309,7.05701L 33.1953,0L 35.9761,0L 35.9761,10.2036L 34.0848,10.2036L 34.0848,1.5589L 30.385,10.2036L 28.4908,10.2036L 24.8176,1.5589L 24.8176,10.2036L 22.9264,10.2036 Z \"/>\r\n\t<Rectangle x:Name=\"Interlocked\" Width=\"1.91837\" Height=\"9.63086\" Canvas.Left=\"28.3119\" Canvas.Top=\"0.209534\" Stretch=\"Fill\" Fill=\"#FF800080\"/>\r\n\t<Ellipse x:Name=\"Run\" Width=\"44.562\" Height=\"44.9366\" Canvas.Left=\"7.44644\" Canvas.Top=\"11.2618\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF007E00\" Fill=\"#FF00FE00\"/>\r\n\t<Path x:Name=\"InMan\" Width=\"16.5739\" Height=\"20.8253\" Canvas.Left=\"0\" Canvas.Top=\"45.0315\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF230FD2\" Data=\"F1 M 3.73941,50.0228L 2.56577,49.4514L 1.11978,50.3963L 0.5,52.9458L 0.676788,60.7671L 3.29465,63.7238L 3.85849,65.3567L 10.0225,65.3479L 10.5206,63.9272L 13.4569,61.6211L 16.0739,58.157L 16.0739,55.657L 15.1242,54.9208L 12.2355,57.5014L 12.2254,51.0582L 12.1455,49.0152L 10.9255,47.9333L 9.44348,48.8474L 9.4465,56.2216L 9.42126,47.0398L 7.78525,45.7608L 7.03125,45.5315L 6.33655,46.6604L 6.37256,54.7028L 6.34314,47.757L 5.37881,47.0754L 4.49277,47.4947L 3.78796,48.5317L 3.77792,55.9568L 3.73941,50.0228 Z \"/>\r\n\t<Path x:Name=\"SimMode\" Width=\"4.885\" Height=\"23.155\" Canvas.Left=\"52.008\" Canvas.Top=\"21.845\" Stretch=\"Fill\" Fill=\"#FFFF0000\" Data=\"F1 M 56.8615,25.9778L 53.8829,25.9693L 53.8615,40.9726L 56.8615,40.9726L 56.8927,37.9674L 53.8673,37.9589L 53.8829,36.9745L 56.8771,36.957\"/>\r\n\t<Path x:Name=\"Out\" Width=\"29.0468\" Height=\"27.0872\" Canvas.Left=\"15.4309\" Canvas.Top=\"21.8448\" Stretch=\"Fill\" StrokeThickness=\"2\" StrokeLineJoin=\"Round\" Stroke=\"#FF007E00\" Fill=\"#FF00FE00\" Data=\"F1 M 43.4778,22.8657L 16.4309,22.8448L 30.2712,47.9321L 43.4778,22.8657 Z \"/>\r\n</Canvas>\r\n"
        },
        {
            "DigitalInput",
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"Document\" Width=\"27.1667\" Height=\"20.8333\" Clip=\"F1 M 0,0L 27.1667,0L 27.1667,20.8333L 0,20.8333L 0,0\">\r\n\t<Rectangle x:Name=\"MainRec\" Width=\"19.25\" Height=\"19.5\" Canvas.Left=\"0.854492\" Canvas.Top=\"0.591843\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF000000\" Fill=\"#FFA0A0A4\"/>\r\n\t<Path x:Name=\"LL1\" Width=\"5.15845\" Height=\"10.9235\" Canvas.Left=\"4.8606\" Canvas.Top=\"4.95049\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 10.019,15.8739L 4.8606,15.8739L 4.8606,4.95049L 6.0069,4.95049L 6.0069,14.7816L 10.019,14.7816L 10.019,15.8739 Z \"/>\r\n\t<Path x:Name=\"LL2\" Width=\"5.15845\" Height=\"10.9235\" Canvas.Left=\"11.5952\" Canvas.Top=\"4.95049\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 16.7537,15.8739L 11.5952,15.8739L 11.5952,4.95049L 12.7415,4.95049L 12.7415,14.7816L 16.7537,14.7816L 16.7537,15.8739 Z \"/>\r\n\t<Path x:Name=\"HL1\" Width=\"7.59436\" Height=\"10.9235\" Canvas.Left=\"3.07599\" Canvas.Top=\"5.27678\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 10.6703,16.2003L 9.52405,16.2003L 9.52405,11.2067L 4.22229,11.2067L 4.22229,16.2003L 3.07599,16.2003L 3.07599,5.27678L 4.22229,5.27678L 4.22229,10.1144L 9.52405,10.1144L 9.52405,5.27678L 10.6703,5.27678L 10.6703,16.2003 Z \"/>\r\n\t<Path x:Name=\"HL2\" Width=\"5.15845\" Height=\"10.9235\" Canvas.Left=\"13.2496\" Canvas.Top=\"5.27678\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 18.408,16.2003L 13.2496,16.2003L 13.2496,5.27678L 14.3959,5.27678L 14.3959,15.108L 18.408,15.108L 18.408,16.2003 Z \"/>\r\n\t<Path x:Name=\"PS1\" Width=\"6.1615\" Height=\"10.9235\" Canvas.Left=\"3.0296\" Canvas.Top=\"5.42052\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 4.17596,12.1307L 4.17596,16.344L 3.0296,16.344L 3.0296,5.42052L 5.79694,5.42052C 6.87457,5.42052 7.70966,5.70134 8.30225,6.26299C 8.89484,6.82457 9.1911,7.61662 9.1911,8.63902C 9.1911,9.66313 8.85974,10.5011 8.19702,11.1529C 7.5343,11.8048 6.63947,12.1307 5.51257,12.1307L 4.17596,12.1307 Z M 4.17596,6.51286L 4.17596,11.0384L 5.40509,11.0384C 6.2171,11.0384 6.83612,10.8343 7.26227,10.4263C 7.68842,10.0184 7.90149,9.44206 7.90149,8.69756C 7.90149,7.24107 7.11859,6.51286 5.55286,6.51286L 4.17596,6.51286 Z \"/>\r\n\t<Path x:Name=\"PS2\" Width=\"6.1615\" Height=\"11.2356\" Canvas.Left=\"10.624\" Canvas.Top=\"5.26451\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 10.624,15.8246L 10.624,14.3154C 10.7837,14.4844 10.9747,14.6368 11.1971,14.7726C 11.4196,14.9083 11.6539,15.0225 11.9001,15.1151C 12.1464,15.2078 12.3939,15.2797 12.6423,15.3309C 12.8909,15.3821 13.1204,15.4078 13.3308,15.4078C 14.0577,15.4078 14.6003,15.2577 14.9585,14.9579C 15.3167,14.658 15.4958,14.2267 15.4958,13.6644C 15.4958,13.3717 15.4357,13.1166 15.3156,12.8987C 15.1954,12.681 15.0298,12.4826 14.8185,12.3038C 14.6074,12.1249 14.3574,11.953 14.0685,11.7881C 13.7797,11.6231 13.4681,11.4504 13.1338,11.2699C 12.7816,11.0765 12.4528,10.8815 12.1475,10.6847C 11.8423,10.4881 11.5766,10.2711 11.3505,10.0337C 11.1244,9.79643 10.9468,9.52739 10.8177,9.22667C 10.6885,8.92595 10.624,8.57317 10.624,8.1685C 10.624,7.67271 10.7259,7.2415 10.9296,6.87492C 11.1334,6.50841 11.4009,6.20647 11.7322,5.9691C 12.0636,5.7318 12.4412,5.55504 12.8651,5.43883C 13.289,5.32262 13.7211,5.26451 14.1614,5.26451C 15.1645,5.26451 15.8958,5.40831 16.3556,5.69609L 16.3556,7.13707C 15.76,6.61687 14.9951,6.3568 14.0607,6.3568C 13.8025,6.3568 13.5443,6.38689 13.2861,6.44707C 13.0278,6.50719 12.798,6.60551 12.5965,6.74205C 12.395,6.87859 12.2308,7.05412 12.1039,7.26872C 11.977,7.48332 11.9136,7.74498 11.9136,8.05388C 11.9136,8.33183 11.9617,8.57195 12.058,8.77434C 12.1542,8.97673 12.2964,9.16124 12.4845,9.32793C 12.6725,9.49449 12.9017,9.65617 13.1718,9.8131C 13.442,9.96989 13.7532,10.1419 14.1055,10.3288C 14.4682,10.5206 14.8115,10.7226 15.1354,10.9347C 15.4593,11.1468 15.7436,11.3821 15.9884,11.6405C 16.2332,11.899 16.4272,12.1847 16.5705,12.4977C 16.7138,12.8106 16.7855,13.1694 16.7855,13.5741C 16.7855,14.1105 16.6873,14.5645 16.491,14.9359C 16.2947,15.3073 16.0302,15.6093 15.6973,15.8417C 15.3645,16.0741 14.9805,16.2421 14.5454,16.3453C 14.1103,16.4484 13.6517,16.5001 13.1696,16.5001C 13.0084,16.5001 12.8099,16.485 12.574,16.4549C 12.3382,16.4249 12.0972,16.381 11.8509,16.3233C 11.6046,16.2655 11.3718,16.194 11.1524,16.1087C 10.9329,16.0234 10.7568,15.9288 10.624,15.8246 Z \"/>\r\n\t<Path x:Name=\"FS1\" Width=\"5.15845\" Height=\"10.9235\" Canvas.Left=\"4.24872\" Canvas.Top=\"5.33769\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 9.40717,6.43004L 5.39508,6.43004L 5.39508,10.3312L 9.12061,10.3312L 9.12061,11.4236L 5.39508,11.4236L 5.39508,16.2612L 4.24872,16.2612L 4.24872,5.33769L 9.40717,5.33769L 9.40717,6.43004 Z \"/>\r\n\t<Path x:Name=\"FS2\" Width=\"6.1615\" Height=\"11.2356\" Canvas.Left=\"10.1236\" Canvas.Top=\"5.18169\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 10.1236,15.7418L 10.1236,14.2325C 10.2833,14.4015 10.4744,14.554 10.6968,14.6897C 10.9192,14.8255 11.1535,14.9396 11.3998,15.0323C 11.6461,15.1249 11.8935,15.1969 12.142,15.2481C 12.3905,15.2992 12.62,15.3249 12.8304,15.3249C 13.5573,15.3249 14.0999,15.1749 14.4581,14.875C 14.8163,14.5751 14.9955,14.1439 14.9955,13.5815C 14.9955,13.2889 14.9354,13.0337 14.8152,12.8158C 14.6951,12.5981 14.5294,12.3998 14.3182,12.221C 14.107,12.0421 13.857,11.8702 13.5682,11.7052C 13.2794,11.5402 12.9678,11.3675 12.6334,11.1871C 12.2812,10.9937 11.9525,10.7986 11.6472,10.6019C 11.342,10.4052 11.0762,10.1883 10.8502,9.95085C 10.624,9.71355 10.4464,9.4445 10.3173,9.14384C 10.1882,8.84312 10.1236,8.49034 10.1236,8.08562C 10.1236,7.58989 10.2255,7.15862 10.4293,6.7921C 10.633,6.42558 10.9005,6.12364 11.2319,5.88628C 11.5632,5.64897 11.9409,5.47221 12.3647,5.356C 12.7886,5.23979 13.2208,5.18169 13.6611,5.18169C 14.6641,5.18169 15.3954,5.32549 15.8552,5.6132L 15.8552,7.05424C 15.2596,6.53404 14.4947,6.27397 13.5604,6.27397C 13.3021,6.27397 13.0439,6.30406 12.7857,6.36418C 12.5275,6.42436 12.2976,6.52263 12.0961,6.65923C 11.8946,6.79576 11.7304,6.9713 11.6035,7.1859C 11.4766,7.4005 11.4132,7.66216 11.4132,7.97105C 11.4132,8.24895 11.4614,8.48912 11.5576,8.69151C 11.6539,8.89391 11.7961,9.07841 11.9841,9.24504C 12.1722,9.41167 12.4013,9.57335 12.6714,9.73027C 12.9417,9.88707 13.2529,10.059 13.6051,10.246C 13.9678,10.4378 14.3111,10.6398 14.635,10.8519C 14.9589,11.064 15.2432,11.2992 15.488,11.5577C 15.7328,11.8162 15.9268,12.1019 16.0701,12.4148C 16.2134,12.7277 16.2851,13.0865 16.2851,13.4913C 16.2851,14.0277 16.187,14.4816 15.9907,14.8531C 15.7944,15.2245 15.5298,15.5265 15.197,15.7589C 14.8641,15.9913 14.4802,16.1592 14.045,16.2624C 13.6099,16.3656 13.1514,16.4173 12.6693,16.4173C 12.5081,16.4173 12.3095,16.4022 12.0737,16.3721C 11.8378,16.3421 11.5968,16.2981 11.3505,16.2404C 11.1042,16.1827 10.8714,16.1112 10.652,16.0259C 10.4326,15.9405 10.2565,15.8459 10.1236,15.7418 Z \"/>\r\n\t<Path x:Name=\"PX1\" Width=\"6.1615\" Height=\"10.9235\" Canvas.Left=\"3.42828\" Canvas.Top=\"5.30724\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 4.57465,12.0174L 4.57465,16.2308L 3.42828,16.2308L 3.42828,5.30724L 6.19562,5.30724C 7.27325,5.30724 8.10834,5.58806 8.70093,6.1497C 9.29352,6.71129 9.58978,7.50334 9.58978,8.52574C 9.58978,9.54985 9.25842,10.3878 8.5957,11.0397C 7.93298,11.6915 7.03815,12.0174 5.91125,12.0174L 4.57465,12.0174 Z M 4.57465,6.39958L 4.57465,10.9251L 5.80377,10.9251C 6.61578,10.9251 7.2348,10.7211 7.66095,10.313C 8.0871,9.90508 8.30017,9.32878 8.30017,8.58427C 8.30017,7.12779 7.51727,6.39958 5.95154,6.39958L 4.57465,6.39958 Z \"/>\r\n\t<Path x:Name=\"PX2\" Width=\"8.16748\" Height=\"10.9235\" Canvas.Left=\"9.58978\" Canvas.Top=\"5.30724\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 17.7573,16.2308L 16.3043,16.2308L 13.9646,12.0491C 13.8929,11.9223 13.8146,11.7419 13.7296,11.5077L 13.7026,11.5077C 13.6548,11.6249 13.5743,11.8053 13.4609,12.0491L 11.0496,16.2308L 9.58978,16.2308L 13.0198,10.7398L 9.8764,5.30724L 11.3406,5.30724L 13.425,9.14757C 13.5623,9.40117 13.6832,9.65471 13.7877,9.90831L 13.8168,9.90831C 13.9691,9.57341 14.1019,9.30925 14.2154,9.11583L 16.3848,5.30724L 17.7573,5.30724L 14.549,10.7227L 17.7573,16.2308 Z \"/>\r\n\t<Path x:Name=\"GS1\" Width=\"8.16748\" Height=\"11.2356\" Canvas.Left=\"2.43335\" Canvas.Top=\"4.88579\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 10.6008,15.2508C 9.59186,15.8312 8.47089,16.1214 7.23798,16.1214C 5.80511,16.1214 4.64612,15.621 3.76099,14.6205C 2.87592,13.62 2.43335,12.2957 2.43335,10.6474C 2.43335,8.96495 2.92853,7.58409 3.91888,6.50475C 4.90918,5.4254 6.16412,4.88579 7.68353,4.88579C 8.7851,4.88579 9.70978,5.067 10.4576,5.42949L 10.4576,6.75835C 9.6441,6.23814 8.68134,5.97807 7.5694,5.97807C 6.44397,5.97807 5.52155,6.40276 4.80206,7.25206C 4.08264,8.10143 3.72296,9.20152 3.72296,10.5523C 3.72296,11.9454 4.05542,13.0397 4.7204,13.8355C 5.38531,14.6311 6.28723,15.029 7.42609,15.029C 8.20673,15.029 8.88287,14.8591 9.45453,14.5194L 9.45453,11.4398L 7.30518,11.4398L 7.30518,10.3475L 10.6008,10.3475L 10.6008,15.2508 Z \"/>\r\n\t<Path x:Name=\"GS2\" Width=\"6.1615\" Height=\"11.2356\" Canvas.Left=\"12.4636\" Canvas.Top=\"4.88579\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 12.4636,15.4459L 12.4636,13.9366C 12.6233,14.1056 12.8143,14.2581 13.0367,14.3938C 13.2592,14.5296 13.4935,14.6437 13.7397,14.7364C 13.986,14.829 14.2335,14.901 14.4819,14.9522C 14.7305,15.0033 14.96,15.029 15.1704,15.029C 15.8973,15.029 16.4399,14.879 16.7981,14.5791C 17.1563,14.2792 17.3354,13.848 17.3354,13.2856C 17.3354,12.993 17.2753,12.7378 17.1552,12.5199C 17.035,12.3022 16.8694,12.1039 16.6581,11.9251C 16.447,11.7462 16.197,11.5743 15.9081,11.4093C 15.6193,11.2443 15.3077,11.0716 14.9734,10.8912C 14.6212,10.6978 14.2924,10.5027 13.9871,10.306C 13.6819,10.1093 13.4162,9.89238 13.1901,9.65495C 12.964,9.41765 12.7864,9.14861 12.6573,8.84795C 12.5281,8.54723 12.4636,8.19444 12.4636,7.78972C 12.4636,7.29399 12.5655,6.86272 12.7692,6.4962C 12.973,6.12968 13.2405,5.82774 13.5718,5.59038C 13.9032,5.35307 14.2808,5.17632 14.7047,5.0601C 15.1286,4.94389 15.5607,4.88579 16.001,4.88579C 17.0041,4.88579 17.7354,5.02959 18.1952,5.31731L 18.1952,6.75835C 17.5996,6.23814 16.8347,5.97807 15.9003,5.97807C 15.6421,5.97807 15.3839,6.00816 15.1257,6.06828C 14.8674,6.12846 14.6376,6.22673 14.4361,6.36333C 14.2346,6.49986 14.0704,6.6754 13.9435,6.89C 13.8166,7.1046 13.7532,7.36626 13.7532,7.67516C 13.7532,7.95305 13.8013,8.19322 13.8976,8.39561C 13.9938,8.59801 14.136,8.78252 14.3241,8.94914C 14.5121,9.11577 14.7413,9.27745 15.0114,9.43437C 15.2816,9.59117 15.5928,9.76311 15.9451,9.95006C 16.3078,10.1419 16.6511,10.3439 16.975,10.556C 17.2989,10.7681 17.5832,11.0033 17.828,11.2618C 18.0728,11.5203 18.2668,11.806 18.4101,12.1189C 18.5534,12.4318 18.6251,12.7906 18.6251,13.1954C 18.6251,13.7318 18.5269,14.1857 18.3306,14.5572C 18.1343,14.9286 17.8698,15.2306 17.5369,15.463C 17.2041,15.6954 16.8201,15.8633 16.385,15.9665C 15.9499,16.0697 15.4913,16.1214 15.0092,16.1214C 14.848,16.1214 14.6495,16.1063 14.4136,16.0762C 14.1778,16.0462 13.9368,16.0022 13.6905,15.9445C 13.4442,15.8868 13.2114,15.8153 12.992,15.73C 12.7725,15.6446 12.5964,15.55 12.4636,15.4459 Z \"/>\r\n\t<Path x:Name=\"PB1\" Width=\"6.16144\" Height=\"10.9235\" Canvas.Left=\"3.09918\" Canvas.Top=\"4.95885\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 4.24548,11.669L 4.24548,15.8824L 3.09918,15.8824L 3.09918,4.95885L 5.86646,4.95885C 6.94409,4.95885 7.77924,5.23967 8.37177,5.80132C 8.96436,6.3629 9.26062,7.15495 9.26062,8.17735C 9.26062,9.20146 8.92926,10.0394 8.26654,10.6913C 7.60382,11.3431 6.70905,11.669 5.58209,11.669L 4.24548,11.669 Z M 4.24548,6.05119L 4.24548,10.5767L 5.47467,10.5767C 6.28662,10.5767 6.9057,10.3727 7.33179,9.96465C 7.75793,9.55669 7.97101,8.98039 7.97101,8.23589C 7.97101,6.7794 7.18817,6.05119 5.62238,6.05119L 4.24548,6.05119 Z \"/>\r\n\t<Path x:Name=\"PB2\" Width=\"6.30481\" Height=\"10.9235\" Canvas.Left=\"11.1234\" Canvas.Top=\"4.95885\" Stretch=\"Fill\" Fill=\"#FF000000\" Data=\"F1 M 11.1234,15.8824L 11.1234,4.95885L 14.0026,4.95885C 14.8788,4.95885 15.5732,5.18883 16.0859,5.64885C 16.5986,6.10893 16.855,6.70793 16.855,7.44591C 16.855,8.06194 16.7016,8.59758 16.3948,9.05278C 16.0881,9.50786 15.6653,9.83134 15.1265,10.0231L 15.1265,10.0549C 15.8235,10.141 16.3814,10.4198 16.8001,10.8912C 17.2188,11.3626 17.4282,11.9762 17.4282,12.7321C 17.4282,13.67 17.114,14.4296 16.4855,15.0107C 15.8572,15.5918 15.0646,15.8824 14.1078,15.8824L 11.1234,15.8824 Z M 12.2697,6.05119L 12.2697,9.6403L 13.481,9.6403C 14.1287,9.6403 14.6381,9.46843 15.009,9.12462C 15.3799,8.78087 15.5654,8.29607 15.5654,7.67021C 15.5654,6.59087 14.9198,6.05119 13.6287,6.05119L 12.2697,6.05119 Z M 12.2697,10.7327L 12.2697,14.79L 13.9063,14.79C 14.6138,14.79 15.1627,14.6067 15.553,14.2402C 15.9434,13.8736 16.1385,13.3702 16.1385,12.7297C 16.1385,11.3984 15.3109,10.7327 13.6556,10.7327L 12.2697,10.7327 Z \"/>\r\n\t<Path x:Name=\"SimMode\" Width=\"3.001\" Height=\"11.314\" Canvas.Left=\"21.999\" Canvas.Top=\"4.886\" Stretch=\"Fill\" Fill=\"#FFFF0000\" Data=\"F1 M 26.479,1.66661L 21.9991,1.59892L 22.232,19.6663L 25.9998,19.8124L 26.0624,16.125L 22.1871,16.0372L 22.1889,14.852L 26.1456,14.9166\"/>\r\n</Canvas>\r\n"
        },
        {
            "AOutType1",
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"Document\" Width=\"35.75\" Height=\"63.75\" Clip=\"F1 M 0,0L 35.75,0L 35.75,63.75L 0,63.75L 0,0\">\r\n\t<Path x:Name=\"On\" Width=\"33.5776\" Height=\"61.8365\" Canvas.Left=\"1.2702\" Canvas.Top=\"1.17969\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF008000\" Fill=\"#FF00FF00\" Data=\"F1 M 1.7702,1.67969L 34.3478,1.67969L 34.3219,30.5259L 17.3028,30.5836L 34.3092,30.5412L 34.3304,48.1396L 29.2465,48.1396L 34.3402,48.1448L 34.3478,62.5162L 1.7702,62.5162L 1.77808,48.2589L 17.4131,48.2437L 17.3506,30.5771L 17.2881,48.2438L 1.77808,48.2589L 1.7702,1.67969 Z \"/>\r\n\t<Path x:Name=\"Off\" Width=\"33.5776\" Height=\"61.8365\" Canvas.Left=\"1.31586\" Canvas.Top=\"1.22675\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF000000\" Fill=\"#FFC0C0C0\" Data=\"F1 M 1.81586,1.72675L 34.3934,1.72675L 34.3675,30.5729L 17.3485,30.6307L 34.3548,30.5883L 34.3934,62.5633L 1.81586,62.5633L 1.82374,48.3059L 34.2268,48.2684L 1.82374,48.3059L 1.81586,1.72675 Z \"/>\r\n</Canvas>\r\n"
        },
        {
            "AOutType2",
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"Document\" Width=\"23.3675\" Height=\"43.3514\" Clip=\"F1 M 0,0L 23.3675,0L 23.3675,43.3514L 0,43.3514L 0,0\">\r\n\t<Path x:Name=\"On\" Width=\"23.1694\" Height=\"43.3008\" Canvas.Left=\"0.19809\" Canvas.Top=\"0.0505981\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF008000\" Fill=\"#FF00FF00\" Data=\"F1 M 0.69809,0.550598L 22.8674,0.550598L 22.8498,20.6079L 11.2682,20.6481L 22.8412,20.6186L 22.8674,42.8514L 0.69809,42.8514L 0.703461,32.938L 11.3432,32.9276L 11.3007,20.6436L 11.2582,32.9276L 0.703461,32.938L 0.69809,0.550598 Z \"/>\r\n\t<Path x:Name=\"Off\" Width=\"23.1694\" Height=\"43.3008\" Canvas.Left=\"0\" Canvas.Top=\"0\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF000000\" Fill=\"#FFC0C0C0\" Data=\"F1 M 0.5,0.5L 22.6694,0.5L 22.6517,20.5573L 11.0701,20.5975L 22.6431,20.568L 22.6694,42.8008L 0.5,42.8008L 0.505371,32.8874L 11.6628,32.843L 0.505371,32.8874L 0.5,0.5 Z \"/>\r\n</Canvas>\r\n"
        },
        {
            "AOutType3",
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"Document\" Width=\"32.75\" Height=\"33.2501\" Clip=\"F1 M 0,0L 32.75,0L 32.75,33.2501L 0,33.2501L 0,0\">\r\n\t<Path x:Name=\"P0\" Width=\"29.9895\" Height=\"16.0938\" Canvas.Left=\"1.36694\" Canvas.Top=\"15.4004\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF000000\" Fill=\"#FFE0E0E0\" Data=\"F1 M 30.8148,15.9004L 30.8564,30.9942L 1.86694,15.9734L 1.8877,30.9941L 30.8148,15.9004 Z \"/>\r\n\t<Path x:Name=\"P1\" Width=\"1.02081\" Height=\"15.349\" Canvas.Left=\"15.2007\" Canvas.Top=\"7.89066\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF000000\" Data=\"F1 M 15.7215,8.39066L 15.7007,22.7396\"/>\r\n\t<Path x:Name=\"P2\" Width=\"14.9375\" Height=\"8.27084\" Canvas.Left=\"8.77393\" Canvas.Top=\"0.712494\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF000000\" Fill=\"#FFE0E0E0\" Data=\"F1 M 20.7323,2.99374L 18.5969,1.58743L 16.4822,1.21249L 13.7635,1.67078L 11.7635,3.04578L 10.1906,5.42078L 9.27393,8.48331L 23.2114,8.48334L 22.1698,5.47293L 20.7323,2.99374 Z \"/>\r\n\t<Path x:Name=\"P3\" Width=\"7.125\" Height=\"6.625\" Canvas.Left=\"12.3985\" Canvas.Top=\"19.4642\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF000000\" Fill=\"#FF000000\" Data=\"F1 M 15.961,19.9642C 17.6523,19.9642 19.0235,21.2234 19.0235,22.7767C 19.0235,24.33 17.6523,25.5892 15.961,25.5892C 14.2697,25.5892 12.8985,24.33 12.8985,22.7767C 12.8985,21.2234 14.2697,19.9642 15.961,19.9642 Z \"/>\r\n\t<Path x:Name=\"P4\" Width=\"9.4375\" Height=\"7.09375\" Canvas.Left=\"11.2734\" Canvas.Top=\"22.2136\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF000000\" Data=\"F1 M 15.7111,22.7136L 20.2109,25.7351L 15.6798,28.8073L 11.7734,25.7351L 15.7111,22.7136 Z \"/>\r\n</Canvas>\r\n"
        }
    };

        public static List<Img> dicImg = new List<Img>();

        public static List<Img> dicFact = new List<Img>();
    }
    public enum Rotate
    {
        Rot_90,
        Rot_180,
        Rot_270,
        None
    }
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
            KeyImg value = new KeyImg
            {
                ObjectName = objectName,
                ImageSize = size,
                ListElementSetting = els,
                flipType = flipType,
                Rotation = Rotation
            };
            string keys = JsonConvert.SerializeObject(value).Replace("[", "").Replace("]", "")
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
            Canvas canvas = (Canvas)System.Windows.Markup.XamlReader.Load(reader);
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
            KeyImg value = new KeyImg
            {
                ObjectName = objectName,
                ImageSize = size,
                flipType = flipType,
                Rotation = Rotation
            };
            string keys = JsonConvert.SerializeObject(value).Replace("[", "").Replace("]", "")
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
            Canvas canvas = (Canvas)System.Windows.Markup.XamlReader.Load(reader);
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
            KeyImg value = new KeyImg
            {
                ObjectName = objectName,
                ImageSize = size,
                ListElementSetting = els,
                flipType = flipType,
                Rotation = Rotation
            };
            string keys = JsonConvert.SerializeObject(value).Replace("[", "").Replace("]", "")
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
            Canvas canvas = (Canvas)System.Windows.Markup.XamlReader.Load(reader);
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
            KeyImg value = new KeyImg
            {
                ObjectName = objectName,
                ImageSize = size,
                flipType = flipType,
                Rotation = Rotation,
                TargetColor = targetColor
            };
            string keys = JsonConvert.SerializeObject(value).Replace("[", "").Replace("]", "")
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
            Canvas canvas = (Canvas)System.Windows.Markup.XamlReader.Load(reader);
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
            Canvas canvas = (Canvas)System.Windows.Markup.XamlReader.Load(reader);
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
            Canvas canvas = (Canvas)System.Windows.Markup.XamlReader.Load(reader);
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
                        if (mode != 0)
                        {
                            color = ColorHelper.GdiToMedia(ColorHelper.GrayscaleToColorGradient(ColorHelper.MediaToGdi(color), newColor));
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
                        if (mode != 0)
                        {
                            color2 = ColorHelper.GdiToMedia(ColorHelper.GrayscaleToColorGradient(ColorHelper.MediaToGdi(color2), newColor));
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
                        if (mode != 0)
                        {
                            color3 = ColorHelper.GdiToMedia(ColorHelper.GrayscaleToColorGradient(ColorHelper.MediaToGdi(color3), newColor));
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

}
