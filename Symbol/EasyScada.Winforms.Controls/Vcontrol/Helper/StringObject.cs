using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyScada.Winforms.Controls
{
    [Serializable]
    public class StringObject
    {
        private string value;
        private string text;

        public string Value { get => value; set => this.value = value; }
        public string Text { get => text; set => text = value; }
    }
}
