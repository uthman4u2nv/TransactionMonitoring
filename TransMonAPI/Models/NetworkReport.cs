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
    
    public partial class NetworkReport
    {
        public long ID { get; set; }
        public string bankID { get; set; }
        public string bankname { get; set; }
        public Nullable<int> bankcode { get; set; }
        public Nullable<decimal> LinkAvailability { get; set; }
        public string entitytype { get; set; }
        public Nullable<System.DateTime> ENDDATE { get; set; }
    }
}