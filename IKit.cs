using System;
using System.Runtime.InteropServices;

namespace PTools
{
    [Guid("5E10370D-B1C1-400B-80C0-481A9E2AD499")]
    [ComVisible(true)]
    public interface IKit
    {
        string TagValue(string tag, string value);
        string TagAttrValue(string tag, string attr, string value);
        string EvaluateSQLReturnJSON(string connection, string sql, int timeout = 60);
        string EvaluateSQL(string connection, string sql, int timeout = 60);
        string GetTicks();
        string GetUnixTimestamp();
    }
}

