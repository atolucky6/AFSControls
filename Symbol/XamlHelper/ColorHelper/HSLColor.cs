

// XamlHelper.HSLColor
using System.Drawing;
using XamlHelper;

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
		Color color = this;
		return $"R: {color.R:#0.##} G: {color.G:#0.##} B: {color.B:#0.##}";
	}

	public static implicit operator Color(HSLColor hslColor)
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
		return Color.FromArgb((int)(255.0 * num), (int)(255.0 * num2), (int)(255.0 * num3));
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

	public static implicit operator HSLColor(Color color)
	{
		HSLColor hSLColor = new HSLColor();
		hSLColor.hue = (double)color.GetHue() / 360.0;
		hSLColor.luminosity = color.GetBrightness();
		hSLColor.saturation = color.GetSaturation();
		return hSLColor;
	}

	public void SetRGB(int red, int green, int blue)
	{
		HSLColor hSLColor = Color.FromArgb(red, green, blue);
		hue = hSLColor.hue;
		saturation = hSLColor.saturation;
		luminosity = hSLColor.luminosity;
	}

	public HSLColor()
	{
	}

	public HSLColor(Color color)
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
