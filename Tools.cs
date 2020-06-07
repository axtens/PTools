using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTools
{
    public static class Tools
    {
        public static string TagValue(string tag, string value)
        {
            return $"<{tag}>{value}</{tag}>";
        }
        public static string TagAttrValue(string tag, string attr, string value)
        {
            return $"<{tag} {attr}>{value}</{tag}>";
        }
    }
}
