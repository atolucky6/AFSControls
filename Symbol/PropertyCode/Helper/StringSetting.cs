using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCode
{
    [Serializable]
    public class StringSetting
    {
        public string Expression;

        public List<StringObject> StringObjects = new List<StringObject>();

    }
}
