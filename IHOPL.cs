using System;
using System.Runtime.InteropServices;

namespace PTools
{
    [Guid("6C15F51C-B206-4DEC-A89D-931EB4EB15E1")]
    [ComVisible(true)]
    public interface IHOPL
    {
        bool Debug(bool debug);
        string GenerateGenericTableFromRst64(string rst64_tabs_crlf);
        string VarsToJson(string caret_terminated_bracketed_name_and_value);
        string Rst64ToJson(string rst64_tabs_crlf);
        string RstToJson(string rst_tabs_crlf);
        string SayYear(string rst_tabs_crlf, string which, string vars);
        string SaySammet(string rst_tabs_crlf, string which, string vars);
        string SayNode(string rst_tabs_crlf, string which, string vars);
        string SayCountry(string rst_tabs_crlf, string which, string vars);

    }
}

