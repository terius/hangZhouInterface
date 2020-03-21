using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TxtFile
    {
        public IList<TxtFileItem> shipments { get; set; }
    }

    public class TxtFileItem
    {
        public string[] mps { get; set; }
        public string hawb { get; set; }
        public string station { get; set; }
    }
}
