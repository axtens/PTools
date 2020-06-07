using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTools
{
    public class SQLStoredProcedureCall
    {
        internal string _sproc { set; get; }
        internal List<string> _param;

        public SQLStoredProcedureCall(string sproc)
        {
            _sproc = sproc;
            _param = new List<string>();
        }

        public SQLStoredProcedureCall StringParam(string sym, string val)
        {
            _param.Add($"{sym}='{val.Replace("'", "''")}'");
            return this;
        }

        public SQLStoredProcedureCall NumericParam(string sym, Object val)
        {
            _param.Add($"{sym}={val}");
            return this;
        }


        public SQLStoredProcedureCall DateParam(string sym, string val)
        {
            _param.Add($"{sym}='{val}'");
            return this;
        }

        public SQLStoredProcedureCall BooleanParam(string sym, bool val)
        {
            _param.Add($"{sym}={(val ? 1 : 0)}");
            return this;
        }

        public override string ToString() => _sproc + " " + string.Join(", ", _param.ToArray());

    }
}
