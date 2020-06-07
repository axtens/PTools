using System.Collections.Generic;
using System.Data;
using System.Security.Policy;

namespace PTools
{
    public class SuccessListDictionaryBlock
    {
        public string Error { get; set; }
        public List<Dictionary<string,string>> Cargo { get; set; }
    }
    public class SuccessDictionaryBlock
    {
        public string Error { get; set; }
        public Dictionary<string, string> Cargo { get; set; }
    }
    public class SuccessStringBlock
    {
        public string Error { get; set; }
        public string Cargo { get; set; }
    }
    public class FailureBlock
    {
        public string Error { get; set; }
        public string Cargo { get; set; }
    }
    public class SuccessDataTableBlock
    {
        public string Error { get; set; }
        public DataTable Cargo { get; set; }
    }
    internal class SuccessDoubleBlock
    {
        public object Error { get; set; }
        public double Cargo { get; set; }
    }
    internal class SuccessIntBlock
    {
        public object Error { get; set; }
        public int Cargo { get; set; }
    }
    public class SuccessLongBlock
    {
        public object Error { get; set; }
        public long Cargo { get; set; }
    }
}
