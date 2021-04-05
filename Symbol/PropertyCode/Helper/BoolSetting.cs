using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCode
{
    [Serializable]
    public class BoolSetting
    {
        public string Expression;

        public List<BoolObject> BoolObjects = new List<BoolObject>();

    }
}
