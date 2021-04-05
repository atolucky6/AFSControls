

// XamlHelper.ElementSetting
using System;
using System.Drawing;
namespace XamlHelper
{
	[Serializable]
	public class ElementSetting
	{
		public string ElementName
		{
			get;
			set;
		}

		public Color DrawColor
		{
			get;
			set;
		}

		public Color FillColor
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
}
