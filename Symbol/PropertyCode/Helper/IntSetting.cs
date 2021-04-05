using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCode
{
    [Serializable]
    public class IntSetting
    {
        public string Expression;

        public List<IntObject> IntObjects = new List<IntObject>();

    }
}
