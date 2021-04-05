using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyScada.Winforms.Controls
{
    [Serializable]
    public class ColorSetting
    {
        public string Expression;

        public List<ColorObject> ColorObjects = new List<ColorObject>();

    }
}
