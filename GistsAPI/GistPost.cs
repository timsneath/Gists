using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.Extensions.Gists
{
    [DataContract]
    class GistPost
    {
        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "public")]
        public bool IsPublic;

        [DataMember(Name = "files")]
        public Dictionary<String, Dictionary<String, String>> Files;
    }
}
