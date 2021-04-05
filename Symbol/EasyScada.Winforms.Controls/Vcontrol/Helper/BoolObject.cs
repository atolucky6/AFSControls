using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyScada.Winforms.Controls
{
    [Serializable]
    public class BoolObject
    {
        private string value;
        private bool? visible;

        public string Value { get => value; set => this.value = value; }
        public bool? ControlValue { get => visible; set => visible = value; }
    }
}
