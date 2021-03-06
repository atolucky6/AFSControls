using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyScada.Winforms.Controls
{
    [Serializable]
    public class ColorObject
    {
        private string value;
        private Color color;

        public string Value { get => value; set => this.value = value; }
        public Color Color { get => color; set => color = value; }
    }
}
