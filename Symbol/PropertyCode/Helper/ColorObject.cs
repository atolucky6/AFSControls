using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCode
{
    [Serializable]
    public class ColorObject
    {
        private string value;
        private Color color;

        public string Value { get => value; set => this.value = value; }
        public Color ControlValue { get => color; set => color = value; }
    }
}
