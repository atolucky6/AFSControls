using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyScada.Winforms.Controls
{
    [Serializable]
    public class StringSetting
    {
        public string Expression;

        public List<StringObject> TextObjects = new List<StringObject>();

    }
}
