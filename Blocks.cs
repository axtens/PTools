﻿using System.Data;
namespace PTools
{
    public class FailureBlock
    {
        public string Error { get; set; }
        public string Cargo { get; set; }
    }
    public class SuccessBlock
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
