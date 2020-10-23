using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransactionMonitoring.Models
{
    public class Utilities
    {
    }
    public class BankData
    {
        public string BankName { get; set; }
        public double InFailed { get; set; }
        public double OutFailed { get; set; }
    }
}