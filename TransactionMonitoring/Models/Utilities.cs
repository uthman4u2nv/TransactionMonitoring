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

    public class BankReportRequest
    {
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string sortCode { get; set; }
    }
    public class BankReportResponse
    {
        public string BankName { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public string responseCode { get; set; }
        public decimal In114 { get; set; }
        public decimal In111 { get; set; }
        public decimal In400 { get; set; }
        public decimal In912 { get; set; }
        public decimal In911 { get; set; }
        public decimal In102 { get; set; }
        public decimal In100 { get; set; }
        public decimal In120 { get; set; }
        public decimal In118 { get; set; }

        /** RANKING INWARDS **/
        public string In114r { get; set; }
        public string In111r { get; set; }
        public string In400r { get; set; }
        public string In912r { get; set; }
        public string In911r { get; set; }
        public string In102r { get; set; }
        public string In100r { get; set; }
        public string In120r { get; set; }
        public string In118r { get; set; }

        public string Out114r { get; set; }
        public string Out111r { get; set; }
        public string Out400r { get; set; }
        public string Out912r { get; set; }
        public string Out911r { get; set; }
        public string Out102r { get; set; }
        public string Out100r { get; set; }
        public string Out120r { get; set; }
        public string Out118r { get; set; }

        public string Out121r { get; set; }
        public string Out909r { get; set; }


        /** END RANKING OUTWARDS **/
        /** BEST BANK RATING **/
        public decimal In114bb { get; set; }
        public decimal In111bb { get; set; }
        public decimal In400bb { get; set; }
        public decimal In912bb { get; set; }
        public decimal In911bb { get; set; }
        public decimal In102bb { get; set; }
        public decimal In100bb { get; set; }
        public decimal In120bb { get; set; }
        public decimal In118bb { get; set; }

        public decimal Out114bb { get; set; }
        public decimal Out111bb { get; set; }
        public decimal Out400bb { get; set; }
        public decimal Out912bb { get; set; }
        public decimal Out911bb { get; set; }
        public decimal Out102bb { get; set; }
        public decimal Out100bb { get; set; }
        public decimal Out120bb { get; set; }
        public decimal Out118bb { get; set; }
        public decimal Out121bb { get; set; }
        public decimal Out909bb { get; set; }

        /** END BEST BANK RATING **/

        /** WEEKLY IN VALUES **/
        public decimal In114w { get; set; }
        public decimal In111w { get; set; }
        public decimal In400w { get; set; }
        public decimal In912w { get; set; }
        public decimal In911w { get; set; }
        public decimal In102w { get; set; }
        public decimal In100w { get; set; }
        public decimal In120w { get; set; }
        public decimal In118w { get; set; }
        /** END WEEKLY IN VALUES **/

        public decimal Out114 { get; set; }
        public decimal Out111 { get; set; }
        public decimal Out400 { get; set; }
        public decimal Out912 { get; set; }
        public decimal Out911 { get; set; }
        public decimal Out102 { get; set; }
        public decimal Out100 { get; set; }
        public decimal Out120 { get; set; }
        public decimal Out118 { get; set; }
        public decimal Out121 { get; set; }
        public decimal Out909 { get; set; }

        /**WEEKLY OUT VALUES **/
        public decimal Out114w { get; set; }
        public decimal Out111w { get; set; }
        public decimal Out400w { get; set; }
        public decimal Out912w { get; set; }
        public decimal Out911w { get; set; }
        public decimal Out102w { get; set; }
        public decimal Out100w { get; set; }
        public decimal Out120w { get; set; }
        public decimal Out118w { get; set; }
        public decimal Out121w { get; set; }
        public decimal Out909w { get; set; }
        /**END WEEKLY OUT VALUES **/

        /** WEEKLY RANKING **/
        public string Out114wr { get; set; }
        public string Out111wr { get; set; }
        public string Out400wr { get; set; }
        public string Out912wr { get; set; }
        public string Out911wr { get; set; }
        public string Out102wr { get; set; }
        public string Out100wr { get; set; }
        public string Out120wr { get; set; }
        public string Out118wr { get; set; }
        public string Out121wr { get; set; }
        public string Out909wr { get; set; }

        public string In114wr { get; set; }
        public string In111wr { get; set; }
        public string In400wr { get; set; }
        public string In912wr { get; set; }
        public string In911wr { get; set; }
        public string In102wr { get; set; }
        public string In100wr { get; set; }
        public string In120wr { get; set; }
        public string In118wr { get; set; }

        public decimal In911bbw { get; set; }
        public decimal In912bbw { get; set; }
        public decimal Out121bbw { get; set; }
        public decimal Out909bbw { get; set; }

        /**END WEEKLY RANKING */
        public decimal tf { get; set; }
        public decimal nu { get; set; }
        public decimal iu { get; set; }
        public string tfreq { get; set; }
        public string nureq { get; set; }
        public string iureq { get; set; }
        public string aggreq { get; set; }
        public decimal aggr { get; set; }
        public string weekposition { get; set; }

        public string headingDate { get; set; }
        public string WeeklyDate { get; set; }
    }

}