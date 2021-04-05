

// XamlHelper.KeyImg
using System;
using System.Collections.Generic;
using System.Drawing;
using XamlHelper;
namespace XamlHelper
{
	[Serializable]
	public class KeyImg
	{
		public Color TargetColor;

		public string ObjectName;

		public Size ImageSize;

		public List<ElementSetting> ListElementSetting = new List<ElementSetting>();

		public FlipType flipType;

		public Rotate Rotation;

		public FillMode mode;
	}
}