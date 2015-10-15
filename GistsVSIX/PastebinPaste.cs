using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Extensions.Clipper
{
    public class PastebinPaste
    {
        public string Key { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public int Size { get; set; }
        public int Private { get; set; }
        public Uri Url { get; set; }
        public int Hits { get; set; }
    }
}
