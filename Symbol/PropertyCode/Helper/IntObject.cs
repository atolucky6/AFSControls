using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCode
{
    [Serializable]
    public class IntObject
    {
        private string value;
        private int visible;

        public string Value { get => value; set => this.value = value; }
        public int ControlValue { get => visible; set => visible = value; }
    }
}
