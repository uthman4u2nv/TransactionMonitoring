//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TransMonAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Aggregate_Daily_Transaction
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> TRANSDATE { get; set; }
        public Nullable<int> YEARQUARTER { get; set; }
        public Nullable<int> TRANSACTION_TYPE { get; set; }
        public Nullable<int> TRANLEG { get; set; }
        public Nullable<int> TRANSACTIONCOUNT { get; set; }
        public Nullable<decimal> TRANSACTIONVALUE { get; set; }
        public string RESPONSECODE { get; set; }
        public Nullable<int> TRANSACTION_MODE { get; set; }
        public string TRANSACTION_DESCRIPTION { get; set; }
        public string RECEIVERS_SORT_CODE { get; set; }
        public string SENDERS_SORT_CODE { get; set; }
        public string TRANSACTION_SOURCE { get; set; }
    }
}