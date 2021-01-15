using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
using TransMonAPI.Models;

namespace TransMonAPI.Controllers
{
    [EnableCors(origins: "https://localhost", headers: "*", methods: "*")]
    public class MonitoringController : ApiController
    {
        public TransactionMonitoringEntities dbase = new TransactionMonitoringEntities();
        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
        [HttpPost]
        [Route("api/Authenticate")]
        public async Task<HttpResponseMessage> Authenticate(LoginRequest rrx)
        {
            LoginDetailsResponse resp = new LoginDetailsResponse();
            LoginReq a = new LoginReq();
            var output = "";
            int userID = 0;
            try
            {


                var rs = JsonConvert.SerializeObject(rrx);

                var url = ConfigurationManager.AppSettings["LoginURL"];
                var content = new StringContent(rs, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                HttpResponseMessage result = null;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new
              RemoteCertificateValidationCallback
              (
                 delegate { return true; }
              );
                result = client.PostAsync(url, content).Result;
                var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
                var rr = JsonConvert.DeserializeObject<LoginResponse>(r);



                if (rr.status == "00")
                {
                    resp.responseCode = "00";
                    resp.responseMessage = rr.message;
                    resp.bankAdmin = rr.data.bankAdmin;
                    resp.bankCode = rr.data.bankCode;
                    resp.bankUser = rr.data.bankUser;
                    resp.email = rr.data.email;
                    resp.firstname = rr.data.firstname;
                    resp.lastname = rr.data.lastname;
                }
                else
                {
                    resp.responseCode = rr.status;
                    resp.responseMessage = rr.message;
                }
            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };

        }

        [HttpPost]
        [Route("api/ChartData")]
        public async Task<HttpResponseMessage> ChartData(ChatDataRequest r)
        {
            var output = "";
            List<ChatDataResponse> resp = new List<ChatDataResponse>();
            using(TransactionMonitoringEntities db =new TransactionMonitoringEntities())
            {
                var i = db.MyAnalytics.Where(a => a.BankCode == r.BankCode).ToList();
                output = JsonConvert.SerializeObject(i);
            }
            
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        [Route("api/Authenticate2")]
        public async Task<HttpResponseMessage> Authenticate2(LoginRequest rrx)
        {
            LoginDetailsResponse resp = new LoginDetailsResponse();
            LoginReq a = new LoginReq();
            var output = "";
            int userID = 0;
            int count = 0;
            try
            {


                var status = "00";
                using (TransactionMonitoringEntities db = new TransactionMonitoringEntities())
                {
                    count = db.Users.Count(aa => aa.email == rrx.email && aa.password == rrx.password);
                }



                if (count > 0)
                {
                    var details = dbase.Users.FirstOrDefault(x => x.email == rrx.email && x.password == rrx.password);
                    resp.responseCode = "00";
                    resp.responseMessage = "Successful";
                    resp.bankAdmin = true;
                    resp.bankCode = details.bankCode;
                    resp.bankUser = false;
                    resp.email = details.email;
                    resp.firstname = details.lastname;
                    resp.lastname = details.firstname;
                    resp.status = "00";
                    resp.firsttime = false;
                }
                else
                {
                    resp.responseCode = "96";
                    resp.responseMessage = "Failed";
                }
            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        [HttpPost]
        [Route("api/DisplayStatistics")]
        public async Task<HttpResponseMessage> DisplayStatistics()
        {
            Statistics s = new Statistics();
            s.Up = 25;
            s.Down = 0;
            s.Warning = 0;
            string output = JsonConvert.SerializeObject(s);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };

        }
        public string GetListOfBanks()
        {
            var url = ConfigurationManager.AppSettings["listOfBanksURL"];
            ListOfBankRequest l = new ListOfBankRequest();
            l.pageNumber = 1;
            l.pageSize = 100;
            var rs = JsonConvert.SerializeObject(l);

            var content = new StringContent(rs, Encoding.UTF8, "application/json");
            var client = new HttpClient();

            HttpResponseMessage result = null;
            //client.DefaultRequestHeaders.Add("Username", username);
            //client.DefaultRequestHeaders.Add("Password", password);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new
          RemoteCertificateValidationCallback
          (
             delegate { return true; }
          );
            result = client.PostAsync(url, content).Result;
            //var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1] { '"' });
            var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");



            return r;
        }

        public string GetListOfUsers222()
        {
            var url = "https://tpms.ipsl.co.ke:4433/TransMonAPI/api/ListOfUsers";

            ListOfUserRequest l = new ListOfUserRequest();

            l.startDate = "2020-01-01";
            l.endDate = "2030-12-31";
            l.pageNumber = 1;
            l.pageSize = 100;
            var rs = JsonConvert.SerializeObject(l);


            var content = new StringContent(rs, Encoding.UTF8, "application/json");
            var client = new HttpClient();

            HttpResponseMessage result = null;
            
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new
          RemoteCertificateValidationCallback
          (
             delegate { return true; }
          );
            result = client.PostAsync(url, content).Result;
            
            var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");



            return r;
        }
        public string GetListOfUsers()
        {
            var url = ConfigurationManager.AppSettings["listOfUsersURL"];

            ListOfUserRequest l = new ListOfUserRequest();

            l.startDate = "2020-01-01";
            l.endDate = "2030-12-31";
            l.pageNumber = 1;
            l.pageSize = 100;
            var rs = JsonConvert.SerializeObject(l);


            var content = new StringContent(rs, Encoding.UTF8, "application/json");
            var client = new HttpClient();

            HttpResponseMessage result = null;
            //client.DefaultRequestHeaders.Add("Username", username);
            //client.DefaultRequestHeaders.Add("Password", password);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new
          RemoteCertificateValidationCallback
          (
             delegate { return true; }
          );
            result = client.PostAsync(url, content).Result;
            //var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1] { '"' });
            var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");



            return r;
        }

        public string GetTransAnalytics()
        {
            var url = ConfigurationManager.AppSettings["TransAnalyticsURL"];

            var client = new HttpClient();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new
          RemoteCertificateValidationCallback
          (
             delegate { return true; }
          );
            var result = client.GetAsync(url).Result;
            //var result= "{\"totalVolume\": 5, \"totalFailure\": 2, \"failureRate\": 40, \"analytics\": [{\"bankCode\": \"BANK57\",\"totalVolume\": 1,\"totalFailure\": 0,\"failureRate\": 0, \"responseCode\": {\"000\": 100}},{\"bankCode\": \"BANK68\",\"totalVolume\": 1,\"totalFailure\": 1,\"failureRate\": 100,\"responseCode\": {\"102\": 100}},{\"bankCode\": \"4040059900\",\"totalVolume\": 3,\"totalFailure\": 1,\"failureRate\": 33.33,\"responseCode\": {\"912\": 33.33,\"000\": 66.67}}]}";

            // var result = "{\"totalVolume\":23,\"totalFailure\":4,\"failureRate\":17.39,\"analytics\":[{\"bankCode\":\"BANK63\",\"totalVolume\":1,\"totalFailure\":0,\"failureRate\":0.00,\"responseCode\":{\"000\":100.00}},{\"bankCode\":\"BANK11\",\"totalVolume\":1,\"totalFailure\":0,\"failureRate\":0.00,\"responseCode\":{\"000\":100.00}},{\"bankCode\":\"BANK01\",\"totalVolume\":2,\"totalFailure\":1,\"failureRate\":50.00,\"responseCode\":{\"000\":50.00,\"911\":50.00}},{\"bankCode\":\"BANK68\",\"totalVolume\":2,\"totalFailure\":0,\"failureRate\":0.00,\"responseCode\":{\"000\":100.00}},{\"bankCode\":\"BANK02\",\"totalVolume\":1,\"totalFailure\":0,\"failureRate\":0.00,\"responseCode\":{\"000\":100.00}},{\"bankCode\":\"4040059900\",\"totalVolume\":16,\"totalFailure\":3,\"failureRate\":18.75,\"responseCode\":{\"000\":81.25,\"400\":12.50,\"911\":6.25}}]}";


            var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
            //var r = result.Replace("\\", "");



            return r;
        }
        [HttpPost]
        [Route("api/BankDetails")]
        public async Task<HttpResponseMessage> BankDetails(BankDetailsRequest r)
        {
            var output = "";
            var bankCode = "";
            var bankName = "";
            var bankAlias = "";
            var sortCode = "";
            var email = "";
            int volumeThreshold = 0;
            double failureThreshold = 0;
            int active = 0;
            List<Banks> banks = new List<Banks>();
            try
            {

                var rr = JsonConvert.DeserializeObject<BankListDetails>(GetListOfBanks());
                //deserialize the bank list
                //var b = JsonConvert.DeserializeObject<BankList>(rr.data.ToString());
                if (rr.data.Length == 0)
                {
                    Banks bb = new Banks();
                    bb.code = "NA";
                    bb.date_created = DateTime.Now;
                    bb.name = "NA";
                    output = JsonConvert.SerializeObject(bb);
                }
                else
                {
                    foreach (var i in rr.data)
                    {
                        if (i.code == r.bankCode)
                        {
                            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToDouble(i.date_created) / 1000d)).ToLocalTime();
                            //Console.WriteLine(dt); // Prints: 6/24/2013 10:07:04 AM
                            Banks bb = new Banks();

                            bankCode = i.code;
                            bankName = i.name;
                            bankAlias = i.alias;
                            sortCode = i.sortCode;
                            volumeThreshold = i.volumeThreshold;
                            failureThreshold = i.failureThreshold;
                            email = i.email;
                           
                            active = 1;

                            //banks.Add(bb);
                        }
                    }
                    BankDetailsResponse resp = new BankDetailsResponse();
                    resp.bankAlias = bankAlias;
                    resp.bankCode = bankCode;
                    resp.bankName = bankName;
                    resp.active = active;
                    resp.responseCode = "00";
                    resp.responseMessage = "Successful";
                    resp.sortCode = sortCode;
                    resp.volumeThreshold = volumeThreshold;
                    resp.failureThreshold = failureThreshold;
                    resp.email = email;
                    output = JsonConvert.SerializeObject(resp);
                }
                

            }
            catch (Exception ex)
            {
                output = JsonConvert.SerializeObject(ex.Message);
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        [HttpPost]
        [Route("api/ListOfBanks")]
        public async Task<HttpResponseMessage> ListOfBanks()
        {
            var output = "";
            List<Banks> banks = new List<Banks>();
            try
            {

                var rr = JsonConvert.DeserializeObject<BankListDetails>(GetListOfBanks());
                //deserialize the bank list
                //var b = JsonConvert.DeserializeObject<BankList>(rr.data.ToString());
                if (rr.data.Length == 0)
                {
                    Banks bb = new Banks();
                    bb.code = "NA";
                    bb.date_created = DateTime.Now;
                    bb.name = "NA";
                    output = JsonConvert.SerializeObject(bb);
                }
                else
                {
                    foreach (var i in rr.data)
                    {
                        var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToDouble(i.date_created) / 1000d)).ToLocalTime();
                        //Console.WriteLine(dt); // Prints: 6/24/2013 10:07:04 AM
                        Banks bb = new Banks();
                        
                        bb.code = i.code;
                        bb.alias = i.alias;
                        bb.dateCreated = dt.ToString();
                        bb.date_created = Convert.ToDateTime(i.date_created);
                        bb.name = i.name;
                        bb.sortCode = i.sortCode.ToString();
                        bb.volumeThreshold = i.volumeThreshold;
                        bb.failureThreshold = i.failureThreshold;
                        bb.email = i.email;
                        banks.Add(bb);
                    }
                    output = JsonConvert.SerializeObject(banks);
                }
                //using (TransactionMonitoringEntities db=new TransactionMonitoringEntities())
                //{
                //    var b = db.Banks.ToList();
                //    foreach(var i in b)
                //    {
                //        BankList bb = new BankList();
                //        bb.code = i.code;
                //        bb.date_created = i.date_created;
                //        bb.name = i.name;
                //        banks.Add(bb);
                //    }
                //}

            }
            catch (Exception ex)
            {
                output = JsonConvert.SerializeObject(ex.Message);
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

       
        [HttpPost]
        [Route("api/ListOfBanks2")]
        public async Task<HttpResponseMessage> ListOfBanks2()
        {
            var output = "";
            List<BankList> banks = new List<BankList>();
            try
            {

                //var rr = JsonConvert.DeserializeObject<BankListDetails>(GetListOfBanks());
                //deserialize the bank list
                //var b = JsonConvert.DeserializeObject<BankList>(rr.data.ToString());
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var b = db.Bank_Description.Where(a=>a.STATUS==1).OrderBy(x=>x.BANK_NAME).ToList();
                    foreach (var i in b)
                    {
                        BankList bb = new BankList();
                        bb.bankID = i.ID;
                        bb.code = i.SORT_CODE;
                        bb.date_created = DateTime.Now;
                        bb.name = i.BANK_NAME.ToUpper();
                        banks.Add(bb);
                    }
                }
                output = JsonConvert.SerializeObject(banks);
            }
            catch (Exception ex)
            {
                output = JsonConvert.SerializeObject(ex.Message+" Inner Exception:"+ex.InnerException);
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

        //[HttpPost]
        //[Route("api/CountAdminUsers")]
        //public async Task<HttpResponseMessage> CountBankAdmin()
        //{
        //    var output = "";
        //    List<U> u = new List<U>();
        //    var BankCode = "BANK53";
        //    int count = 0;

        //    try
        //    {
        //        var rr = GetListOfUsers222(); 
        //        //deserialize the bank list
        //        List<string> b = JsonConvert.DeserializeObject<ListOfAdminUsersCount>(rr);

        //        foreach (var i in b.Property1)

        //        {
        //            if(i.bankCode==BankCode && i.bankAdmin == true)
        //            {
        //                count++;
        //            }
                    
                    
        //        }
        //        CountAdminResponse bb = new CountAdminResponse();
        //        bb.Count = count;
        //        output = JsonConvert.SerializeObject(bb);

        //    }
        //    catch (Exception ex)
        //    {

        //        output = JsonConvert.SerializeObject(ex.Message);
        //    }

        //    return new HttpResponseMessage()
        //    {
        //        Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
        //    };
        //}

        [HttpPost]
        [Route("api/ListOfUsers")]
        public async Task<HttpResponseMessage> ListOfUsers()
        {
            var output = "";
            List<U> u = new List<U>();

            try
            {
                var rr = JsonConvert.DeserializeObject<UList>(GetListOfUsers());
                //deserialize the bank list
                //var b = JsonConvert.DeserializeObject<U>(rr.data.ToString());

                foreach (var i in rr.data)

                {

                    U bb = new U();

                    bb.firstname = i.firstname;
                    bb.lastname = i.lastname;
                    bb.bankCode = i.bankCode;
                    bb.bankAdmin = i.bankAdmin;
                    bb.bankUser = i.bankUser;
                    bb.email = i.email;
                    bb.ipslAdmin = i.ipslAdmin;

                    u.Add(bb);
                }
                output = JsonConvert.SerializeObject(u);
                
            }
            catch (Exception ex)
            {

                output = JsonConvert.SerializeObject(ex.Message);
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        //public List<ChartData> ReturnFailureRates22(List<LatestTransAnalytics> rx)
        //{
        //    List<ChartData> rr = new List<ChartData>();
        //    foreach (var i in rx)
        //    {
        //        ChartData r = new ChartData();
        //        r.Date = i.data.;
        //        r.FailureRate = v.FailureRateOut;

        //        rr.Add(r);
        //    }
            
        //    DateTime startDateTime = DateTime.Today; //Today at 00:00:00
        //    DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59

        //    //List<LatestTransAnalytics> r = new List<LatestTransAnalytics>();
        //    JsonConvert.DeserializeObject<LatestTransAnalytics>(rx.ToList());
        //    List<string> rrr = rx;
        //    foreach (var u in rrr)
        //    {

        //    }

        //    using (TransactionMonitoringEntities db = new TransactionMonitoringEntities())
        //    {
        //        var i = db.MyAnalytics.Where(a => a.BankCode == bankCode && a.AnalyticsDate >= startDateTime && a.AnalyticsDate <= endDateTime).ToList();
        //        foreach (var v in i)
        //        {
        //            ChartData r = new ChartData();
        //            r.Date = v.AnalyticsDate;
        //            r.FailureRate = v.FailureRateOut;

        //            rr.Add(r);
        //        }

        //    }
        //    return rr;
        //}
        //public List<ChartData> ReturnFailureRates(string bankCode)
        //{
        //    List<ChartData> rr = new List<ChartData>();
        //    DateTime startDateTime = DateTime.Today; //Today at 00:00:00
        //    DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59



        //    using (TransactionMonitoringEntities db = new TransactionMonitoringEntities())
        //    {
        //        var i = db.MyAnalytics.Where(a => a.BankCode == bankCode && a.AnalyticsDate>=startDateTime && a.AnalyticsDate <=endDateTime).ToList();
        //        foreach(var v in i)
        //        {
        //            ChartData r = new ChartData();
        //            r.Date = v.AnalyticsDate;
        //            r.FailureRate = v.FailureRateOut;
                    
        //            rr.Add(r);
        //        }
               
        //    }
        //    return rr;
        //}
        //public List<ChartData> ReturnFailureRatesIn(string bankCode)
        //{
        //    List<ChartData> rr = new List<ChartData>();
        //    DateTime startDateTime = DateTime.Today; //Today at 00:00:00
        //    DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59

        //    using (TransactionMonitoringEntities db = new TransactionMonitoringEntities())
        //    {
        //        var i = db.MyAnalytics.Where(a => a.BankCode == bankCode && a.AnalyticsDate >= startDateTime && a.AnalyticsDate <= endDateTime).ToList();
        //        foreach (var v in i)
        //        {
        //            ChartData r = new ChartData();
        //            r.Date = v.AnalyticsDate;
        //            r.FailureRate = v.FailureRateIn;

        //            rr.Add(r);
        //        }

        //    }
        //    return rr;
        //}
        public static long ConvertDatetimeToUnixTimeStamp(DateTime date)
        {
            DateTime originDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - originDate;
            return (long)Math.Floor(diff.TotalSeconds);
        }
        [HttpPost]
        [Route("api/TransAnalytics")]
        public async Task<HttpResponseMessage> TransAnalytics(AnalyticalRequest req)
        {
            var output = "";
            

            Analytical resp = new Analytical();
            float failureRate = 0;
            
            int totalFailure = 0;
            int totalVolume = 0;
            float success = 0;
            int error102 = 0;
            float error912 = 0;

            float outfailureRate = 0;
            int outtotalFailure = 0;
            int outtotalVolume = 0;
            float outsuccess = 0;
            int outerror102 = 0;
            float outerror912 = 0;
            try
            {

                //var rr = JsonConvert.DeserializeObject<tAnalytic>(GetTransAnalytics());
                var startdate = DateTime.Now.AddMinutes(-5).ToString();
                var enddate = DateTime.Now.ToString();
                var transactionperiod = startdate + " - " + enddate;

                 var rr = JsonConvert.DeserializeObject<LatestTransAnalytics>(GetDashboardAnalytics(req.bankCode));
                //get the 
                int totalDailyVolumeIn = 0;
                int totalDailyVolumeOut = 0;
                int totalDailyFailedIn = 0;
                int totalDailyFailedOut = 0;
                double dailyFailureRateIn = 0;
                double dailyFailureRateOut = 0;
                double dailySuccessRateIn = 0;
                double dailySuccessRateOut = 0;
                List<ChartData> x = new List<ChartData>();
                List<ChartData> y = new List<ChartData>();
                foreach (var i in rr.data)
                {
                    ChartData d = new ChartData();
                    ChartData c = new ChartData();
                    d.FailureRate = Convert.ToInt16(i.in_failure_rate);
                    d.Date = ConvertDatetimeToUnixTimeStamp(i.date_created)*1000;                    
                    x.Add(d);
                    c.FailureRate = Convert.ToInt16(i.out_failure_rate);
                    c.Date = ConvertDatetimeToUnixTimeStamp(i.date_created)*1000;                    
                    y.Add(c);
                   // totalDailyVolumeIn += Convert.ToInt16(i.total_in_volume);
                    //totalDailyVolumeOut+= Convert.ToInt16(i.total_out_volume);
                   // totalDailyFailedIn += Convert.ToInt16(i.total_in_failed);
                   // totalDailyFailedOut += Convert.ToInt16(i.total_out_failed);

                    
                }
                //var item = items[items.Count - 2];
                
                var items=rr.data.AsEnumerable().Reverse().Skip(1).FirstOrDefault();
                resp.responseCode = "00";
                resp.error102 = 0;
                resp.error912 = 0;
                //resp.totalInDaily = rr.count;
                //calcuate failure rate for in and out
                dailyFailureRateIn = totalDailyFailedIn / totalDailyVolumeIn * 100;
                dailyFailureRateOut = totalDailyFailedOut / totalDailyVolumeIn * 100;
                dailySuccessRateIn = 100 - dailyFailureRateIn;
                dailySuccessRateOut = 100 - dailyFailureRateOut;

                int sIn = ((totalDailyVolumeIn - totalDailyFailedIn) / totalDailyVolumeIn) * 100;
                int sOut = ((totalDailyVolumeOut - totalDailyFailedOut) / totalDailyVolumeOut) * 100;
                resp.totalInDaily = totalDailyVolumeIn;
                resp.totalOutdaily = totalDailyVolumeOut;
                resp.totalDailyFailedIn = totalDailyFailedIn;
                resp.totalDailyFailedOut = totalDailyFailedOut;
                resp.failureRate = items.in_failure_rate;
                resp.dailyFailureRateIn = dailyFailureRateIn;
                resp.dailyFailureRateOut = dailyFailureRateOut;
                resp.dailySuccessRateIn = Convert.ToDouble(sIn);
                resp.dailySuccessRateOut = Convert.ToDouble(sOut);
                resp.dailySuccessVolumeIn = totalDailyVolumeIn - totalDailyFailedIn;
                resp.dailySuccessVolumeOut = totalDailyVolumeOut - totalDailyFailedOut;
                if (items.in_failure_rate < 1)
                {
                    resp.success = 0;
                }
                else
                {
                    resp.success = 100 - items.in_failure_rate;
                }
                
                resp.totalFailure = Convert.ToInt16(items.total_in_failed);
                resp.totalVolume = Convert.ToInt16(items.total_in_volume);
                resp.outerror102 = 0;
                resp.outerror912 = 0;
                resp.outfailureRate =Convert.ToInt16(items.out_failure_rate);
                if(items.out_failure_rate < 1)
                {
                    resp.outsuccess = 0;
                }
                else
                {
                    resp.outsuccess = 100 - Convert.ToInt16(items.out_failure_rate);
                }
                
                resp.outtotalFailure = Convert.ToInt16(items.total_out_failed);
                resp.outtotalVolume = Convert.ToInt16(items.total_out_volume);
                //resp.FailureRateList = ReturnFailureRates(req.bankCode);
                //resp.FailureRateListIn = ReturnFailureRatesIn(req.bankCode);
                //resp.FailureRateList = ReturnFailureRates22(rr);

                resp.FailureRateList = y;
                resp.FailureRateListIn = x;
                resp.TransactionPeriod = transactionperiod;


                //foreach (var i in rr.inwardSummary)

                //{
                //    if (i.bankCode == req.bankCode)
                //    {

                //        //failureRate = (i.totalFailure/i.totalVolume)*100;
                //        failureRate = i.inFailed;
                //        totalFailure = i.totalFailure;
                //        totalVolume = i.totalVolume;

                //        //var rrx = JsonConvert.DeserializeObject<RespCodeTransAnalysis>(i.responseCode.ToString());
                //        //success = (i.totalVolume-i.totalFailure)/i.totalVolume*100;
                //        success = 100 - failureRate;
                //        error102 = 0;
                //        error912 = 0;

                //    }




                //}
                //foreach (var ii in rr.outwardSummary)

                //{
                //    if (ii.bankCode == req.bankCode)
                //    {

                //        outfailureRate = ii.outFailed;
                //        outtotalFailure = ii.totalFailure;
                //        outtotalVolume = ii.totalVolume;

                //        //var rrx = JsonConvert.DeserializeObject<RespCodeTransAnalysis>(i.responseCode.ToString());
                //        //outsuccess = (ii.totalVolume - ii.totalFailure) / ii.totalVolume * 100;
                //        outsuccess = 100 - outfailureRate;
                //        outerror102 = 0;
                //        outerror912 = 0;

                //    }




                //}
                //resp.responseCode = "00";
                //resp.error102 = error102;
                //resp.error912 = error912;
                //resp.failureRate = failureRate;
                //resp.success = success;
                //resp.totalFailure = totalFailure;
                //resp.totalVolume = totalVolume;
                //resp.outerror102 = outerror102;
                //resp.outerror912 = Convert.ToInt32(outerror912);
                //resp.outfailureRate = Convert.ToInt32(outfailureRate);
                //resp.outsuccess = Convert.ToInt32(outsuccess);
                //resp.outtotalFailure = outtotalFailure;
                //resp.outtotalVolume = outtotalVolume;
                //resp.FailureRateList = ReturnFailureRates(req.bankCode);
                //resp.FailureRateListIn= ReturnFailureRatesIn(req.bankCode);




                output = JsonConvert.SerializeObject(resp);




            }
            catch (Exception ex)
            {
                resp.responseCode = "96";
                resp.error102 = 0;
                resp.error912 = 0;
                resp.failureRate = 0;
                resp.success = 0;
                resp.totalFailure = 0;
                resp.totalVolume = 0;

                output = JsonConvert.SerializeObject(ex.Message);
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        [Route("api/NewTransAnalytics")]
        public async Task<HttpResponseMessage> NewTransAnalytics(NewAnalyticalRequest req)
        {
            var output = "";

            //JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //};
            
            LastFiveMinutesAnalyticsResp resp = new LastFiveMinutesAnalyticsResp();


            double outfailureRate = 0;
            int outtotalFailure = 0;
            int outtotalVolume = 0;
            double outsuccess = 0;
            int outsuccessvolume = 0;
            int outerror102 = 0; int outerror912 = 0; int outerror100 = 0;int outerror114 = 0;
            int outerror908 = 0;int outerror111 = 0; int outerror400 = 0; int outerror909 = 0;

            int outerror107 = 0;int outerror116 = 0;int outerror118 = 0;int outerror119 = 0;
            int outerror121 = 0;int outerror910 = 0;int outerror911 = 0;

            double outerror107Rate = 0; double outerror116Rate = 0; double outerror118Rate = 0; double outerror119Rate = 0;
            double outerror121Rate = 0; double outerror910Rate = 0; double outerror911Rate = 0;

            /** NEW PARAMETERS **/
            int outerror101 = 0; int outerror103 = 0;int outerror104 = 0;  int outerror105 = 0;  int outerror106 = 0; 
            int outerror109 = 0; int outerror110 = 0;int outerror112 = 0; int outerror115 = 0; int outerror117 = 0; 
            int outerror120 = 0;  int outerror125 = 0; int outerror200 = 0; int outerror201 = 0; int outerror202 = 0;
            int outerror203 = 0;  int outerror204 = 0; int outerror904 = 0; int outerror913 = 0; int outerror914 = 0;
            int outerror923 = 0; 

            int inerror101 = 0; int inerror103 = 0; int inerror104 = 0; int inerror105 = 0; int inerror106 = 0;
            int inerror109 = 0; int inerror110 = 0; int inerror112 = 0; int inerror115 = 0; int inerror117 = 0;
            int inerror120 = 0; int inerror125 = 0; int inerror200 = 0; int inerror201 = 0; int inerror202 = 0;
            int inerror203 = 0; int inerror204 = 0; int inerror904 = 0; int inerror913 = 0; int inerror914 = 0;
            int inerror923 = 0;

            double outerror101Rate=0; double outerror103Rate = 0; double outerror104Rate = 0; double outerror105Rate = 0; double outerror106Rate = 0;
            double outerror109Rate = 0; double outerror110Rate = 0; double outerror112Rate = 0; double outerror115Rate = 0; double outerror117Rate = 0;
            double outerror120Rate = 0; double outerror125Rate = 0; double outerror200Rate = 0; double outerror201Rate = 0; double outerror202Rate = 0;
            double outerror203Rate = 0; double outerror204Rate = 0; double outerror904Rate = 0; double outerror913Rate = 0; double outerror914Rate = 0;
            double outerror923Rate = 0;

            double inerror101Rate = 0; double inerror103Rate = 0; double inerror104Rate = 0; double inerror105Rate = 0; double inerror106Rate = 0;
            double inerror109Rate = 0; double inerror110Rate = 0; double inerror112Rate = 0; double inerror115Rate = 0; double inerror117Rate = 0;
            double inerror120Rate = 0; double inerror125Rate = 0; double inerror200Rate = 0; double inerror201Rate = 0; double inerror202Rate = 0;
            double inerror203Rate = 0; double inerror204Rate = 0; double inerror904Rate = 0; double inerror913Rate = 0; double inerror914Rate = 0;
            double inerror923Rate = 0;


            /**START NEW INDUSTRY RATE**/
            int indouterror101 = 0; int indouterror103 = 0; int indouterror104 = 0; int indouterror105 = 0; int indouterror106 = 0;
            int indouterror109 = 0; int indouterror110 = 0; int indouterror112 = 0; int indouterror115 = 0; int indouterror117 = 0;
            int indouterror120 = 0; int indouterror125 = 0; int indouterror200 = 0; int indouterror201 = 0; int indouterror202 = 0;
            int indouterror203 = 0; int indouterror204 = 0; int indouterror904 = 0; int indouterror913 = 0; int indouterror914 = 0;
            int indouterror923 = 0;

            int indinerror101 = 0; int indinerror103 = 0; int indinerror104 = 0; int indinerror105 = 0; int indinerror106 = 0;
            int indinerror109 = 0; int indinerror110 = 0; int indinerror112 = 0; int indinerror115 = 0; int indinerror117 = 0;
            int indinerror120 = 0; int indinerror125 = 0; int indinerror200 = 0; int indinerror201 = 0; int indinerror202 = 0;
            int indinerror203 = 0; int indinerror204 = 0; int indinerror904 = 0; int indinerror913 = 0; int indinerror914 = 0;
            int indinerror923 = 0;

            double indouterror101Rate; double indouterror103Rate; double indouterror104Rate; double indouterror105Rate; double indouterror106Rate;
            double indouterror109Rate; double indouterror110Rate; double indouterror112Rate; double indouterror115Rate; double indouterror117Rate;
            double indouterror120Rate; double indouterror125Rate; double indouterror200Rate; double indouterror201Rate; double indouterror202Rate;
            double indouterror203Rate; double indouterror204Rate; double indouterror904Rate; double indouterror913Rate; double indouterror914Rate;
            double indouterror923Rate;

            double indinerror101Rate; double indinerror103Rate; double indinerror104Rate; double indinerror105Rate; double indinerror106Rate;
            double indinerror109Rate; double indinerror110Rate; double indinerror112Rate; double indinerror115Rate; double indinerror117Rate;
            double indinerror120Rate; double indinerror125Rate; double indinerror200Rate; double indinerror201Rate; double indinerror202Rate;
            double indinerror203Rate; double indinerror204Rate; double indinerror904Rate; double indinerror913Rate; double indinerror914Rate;
            double indinerror923Rate;
            /** END NEW INDUSTRY RATE**/








            /**END ADDITION */

            double IndInRate = 0; double IndOutRate;

            double outerror102Rate = 0; double outerror912Rate = 0; double outerror100Rate = 0;
            double outerror114Rate = 0;double outerror908Rate = 0; double outerror111Rate = 0;
            double outerror400Rate = 0; double outerror909Rate = 0; double infailureRate = 0;


            
            int intotalFailure = 0; int intotalVolume = 0; double insuccess = 0; int insuccessvolume = 0;
            int inerror102 = 0; int inerror912 = 0; int inerror100 = 0;
            int inerror114 = 0; int inerror908 = 0; int inerror111 = 0;
            int inerror400 = 0; int inerror909 = 0; int inerror107 = 0; int inerror116 = 0; int inerror118 = 0; int inerror119 = 0;
            int inerror121 = 0; int inerror910 = 0; int inerror911 = 0;


            double inerror102Rate = 0; double inerror912Rate = 0;double inerror100Rate = 0;
            double inerror114Rate = 0; double inerror908Rate = 0;double inerror111Rate = 0;
            double inerror400Rate = 0; double inerror909Rate = 0;

            double inerror107Rate;double inerror116Rate;double inerror118Rate;
            double inerror119Rate;double inerror121Rate; double inerror910Rate;
            double inerror911Rate;

            


            /** daily values**/
            int dailyTxnVolumeOut = 0;
            int volumeFailedTxnOut = 0;
            float dailyFailureRateOut = 0;
            int dailySuccessVolumeOut = 0;
            float dailySuccessRateOut = 0;

            int dailyTxnVolumeIn = 0;
            int volumeFailedTxnIn = 0;
            float dailyFailureRateIn = 0;
            int dailySuccessVolumeIn = 0;
            float dailySuccessRateIn = 0;

            int outDaily_100 = 0; int outDaily_107 = 0; int outDaily_111 = 0; 
            int outDaily_114 = 0; int outDaily_116 = 0; int outDaily_118 = 0; 
            int outDaily_119 = 0; int outDaily_121 = 0; int outDaily_400 = 0; 
            int outDaily_908 = 0; 
            
            int outDaily_909 = 0; int outDaily_910 = 0; int outDaily_911 = 0; 
            int outDaily_912 = 0; int outDaily_000 = 0;

            int outDaily_101 = 0; int outDaily_103 = 0; int outDaily_104 = 0;
            int outDaily_105 = 0; int outDaily_106 = 0; int outDaily_109 = 0;
            int outDaily_110 = 0; int outDaily_112 = 0; int outDaily_115 = 0;
            int outDaily_117 = 0; int outDaily_120 = 0; int outDaily_125 = 0; int outDaily_200 = 0;
            int outDaily_201 = 0;

            int outDaily_202 = 0; int outDaily_203 = 0; int outDaily_204 = 0;
            int outDaily_904 = 0; int outDaily_913 = 0; int outDaily_923 = 0;

            double outDailyRate_100 = 0; double outDailyRate_107 = 0; double outDailyRate_111 = 0;
            double outDailyRate_114 = 0; double outDailyRate_116 = 0; double outDailyRate_118 = 0;
            double outDailyRate_119 = 0; double outDailyRate_121 = 0; double outDailyRate_400 = 0;
            double outDailyRate_908 = 0; double outDailyRate_909 = 0; double outDailyRate_910 = 0;
            double outDailyRate_911 = 0; double outDailyRate_912 = 0; double outDailyRate_000 = 0;

            double outDailyRate_101 = 0; double outDailyRate_103 = 0; double outDailyRate_104 = 0;
            double outDailyRate_105 = 0; double outDailyRate_106 = 0; double outDailyRate_109 = 0;
            double outDailyRate_110 = 0; double outDailyRate_112 = 0; double outDailyRate_115 = 0;
            double outDailyRate_117 = 0; double outDailyRate_120 = 0; double outDailyRate_125 = 0;
            double outDailyRate_200 = 0; double outDailyRate_201 = 0; double outDailyRate_202 = 0;
            double outDailyRate_203 = 0; double outDailyRate_204 = 0; double outDailyRate_904 = 0;
            double outDailyRate_913 = 0; double outDailyRate_923 = 0;

            int inDaily_100 = 0; int inDaily_107 = 0; int inDaily_111 = 0; int inDaily_114 = 0; 
            int inDaily_116 = 0; int inDaily_118 = 0; int inDaily_119 = 0; int inDaily_121 = 0; 
            int inDaily_400 = 0; int inDaily_908 = 0; int inDaily_909 = 0; int inDaily_910 = 0; 
            int inDaily_911 = 0; int inDaily_912 = 0; int inDaily_000 = 0;
            int inDaily_101 = 0; int inDaily_103 = 0; int inDaily_104 = 0; int inDaily_105 = 0;
            int inDaily_106 = 0; int inDaily_109 = 0; int inDaily_110 = 0; int inDaily_112 = 0;
            int inDaily_115 = 0; int inDaily_117 = 0; int inDaily_120 = 0; int inDaily_125 = 0;
            int inDaily_200 = 0; int inDaily_201 = 0; int inDaily_202 = 0;
            int inDaily_203 = 0; int inDaily_204 = 0; int inDaily_904 = 0; int inDaily_913 = 0;
            int inDaily_914 = 0; int inDaily_923 = 0; 

            double inDailyRate_100 = 0; double inDailyRate_107 = 0; double inDailyRate_111 = 0;
            double inDailyRate_114 = 0; double inDailyRate_116 = 0; double inDailyRate_118 = 0;
            double inDailyRate_119 = 0; double inDailyRate_121 = 0; double inDailyRate_400 = 0;
            double inDailyRate_908 = 0; double inDailyRate_909 = 0; double inDailyRate_910 = 0;
            double inDailyRate_911 = 0; double inDailyRate_912 = 0; double inDailyRate_000 = 0;

            double inDailyRate_101 = 0; double inDailyRate_103 = 0; double inDailyRate_104 = 0;
            double inDailyRate_105 = 0; double inDailyRate_106 = 0; double inDailyRate_109 = 0;
            double inDailyRate_110 = 0; double inDailyRate_112 = 0; double inDailyRate_115 = 0;
            double inDailyRate_117 = 0; double inDailyRate_120 = 0; double inDailyRate_125 = 0;
            double inDailyRate_200 = 0; double inDailyRate_201 = 0; double inDailyRate_202 = 0;
            double inDailyRate_203 = 0; double inDailyRate_204 = 0; double inDailyRate_904 = 0;
            double inDailyRate_913 = 0; double inDailyRate_914 = 0; double inDailyRate_923 = 0;


            try
            {
                if (req.bankCode != "INDUSTRY") { 
                    //var rr = JsonConvert.DeserializeObject<tAnalytic>(GetTransAnalytics());
                    var startdate = DateTime.Now.AddMinutes(-5).ToString();
                var enddate = DateTime.Now.ToString();
                    //var request="";
                    //var dailyrequest = "";
                    
                        var request = req.bankData.Replace("\\", "");
                    
                        var dailyrequest = req.dailyData.Replace("\\", "");
                    
                   // NLastFiveMinutes rr = null;
                    //DailyTransAnalytic yy = null;
                    //var yy = "";
                   
                        var rr = JsonConvert.DeserializeObject<NLastFiveMinutes>(request);
                    
                    
                        var yy = JsonConvert.DeserializeObject<DailyTransAnalytic>(dailyrequest);
                   
                    var xx = JsonConvert.DeserializeObject<LatestTransAnalytics>(GetDashboardAnalytics(req.bankCode));
                    List<ChartData> x = new List<ChartData>();
                    List<ChartData> y = new List<ChartData>();
                    List<ChartData> xxx = new List<ChartData>();
                    List<ChartData> yyy = new List<ChartData>();
                    foreach (var i in xx.data)
                    {
                        
                        ChartData d = new ChartData();
                        ChartData c = new ChartData();
                        ChartData e = new ChartData();
                        ChartData f = new ChartData();
                        d.FailureRate = Convert.ToInt16(i.in_failure_rate);
                        d.Date = ConvertDatetimeToUnixTimeStamp(i.date_created);
                        x.Add(d);
                        c.FailureRate = Convert.ToInt16(i.out_failure_rate);
                        c.Date = ConvertDatetimeToUnixTimeStamp(i.date_created);
                        y.Add(c);
                        
                       e.FailureRate = Convert.ToInt16(i.total_in_volume);
                       e.Date = ConvertDatetimeToUnixTimeStamp(i.date_created);
                       xxx.Add(e);
                       f.FailureRate = Convert.ToInt16(i.total_out_volume);
                       f.Date = ConvertDatetimeToUnixTimeStamp(i.date_created);
                       yyy.Add(f);
                       



                    }
                    //get the 

                    var transactionperiod = rr.startDate + " - " + rr.endDate;

                foreach (var j in rr.outwardSummary)
                {
                    if (j.bankCode == req.bankCode)
                    {
                        outtotalVolume = j.totalVolume;
                        outtotalFailure = j.totalFailure;
                        outfailureRate = j.outFailed;
                        outsuccess = 100 - j.outFailed;
                        outsuccessvolume = Convert.ToInt16(j.bankResponseCode._000);
                        /*outerror102= Convert.ToInt16(j.bankResponseCode._102);
                        outerror908 = Convert.ToInt16(j.bankResponseCode._908);
                        outerror100 = Convert.ToInt16(j.bankResponseCode._100);
                        outerror912 = Convert.ToInt16(j.bankResponseCode._912);
                        outerror114 = Convert.ToInt16(j.bankResponseCode._114);
                        outerror111 = Convert.ToInt16(j.bankResponseCode._111);
                        outerror400 = Convert.ToInt16(j.bankResponseCode._400);*/
                        outerror102 = j.bankResponseCodeCount._102; outerror908 = j.bankResponseCodeCount._908;
                        outerror100 = j.bankResponseCodeCount._100; outerror912 = j.bankResponseCodeCount._912;
                        outerror114 = j.bankResponseCodeCount._114; outerror111 = j.bankResponseCodeCount._111;
                        outerror400 = j.bankResponseCodeCount._400; outerror107 = j.bankResponseCodeCount._107;
                        outerror116 = j.bankResponseCodeCount._116; outerror118 = j.bankResponseCodeCount._118;
                        outerror119 = j.bankResponseCodeCount._119;outerror121 = j.bankResponseCodeCount._121;
                        outerror910 = j.bankResponseCodeCount._910;outerror911 = j.bankResponseCodeCount._911;

                            outerror101 = j.bankResponseCodeCount._101; outerror112 = j.bankResponseCodeCount._112;
                            outerror103 = j.bankResponseCodeCount._103; outerror115 = j.bankResponseCodeCount._115;
                            outerror104 = j.bankResponseCodeCount._104; outerror117 = j.bankResponseCodeCount._117;
                            outerror105 = j.bankResponseCodeCount._105; outerror120 = j.bankResponseCodeCount._120;
                            outerror106 = j.bankResponseCodeCount._106; outerror125 = j.bankResponseCodeCount._125;
                            outerror109 = j.bankResponseCodeCount._109; outerror200 = j.bankResponseCodeCount._200;
                            outerror110 = j.bankResponseCodeCount._110; outerror201 = j.bankResponseCodeCount._201;
                            outerror202 = j.bankResponseCodeCount._202; outerror204 = j.bankResponseCodeCount._204;
                            outerror203 = j.bankResponseCodeCount._203; outerror904 = j.bankResponseCodeCount._904;
                            outerror913 = j.bankResponseCodeCount._110; outerror923 = j.bankResponseCodeCount._201;

                            outerror101Rate = j.bankResponseCode._101; outerror115Rate = j.bankResponseCode._115; outerror203Rate = j.bankResponseCode._203;
                            outerror103Rate = j.bankResponseCode._103; outerror117Rate = j.bankResponseCode._117; outerror204Rate = j.bankResponseCode._204;
                            outerror104Rate = j.bankResponseCode._104; outerror120Rate = j.bankResponseCode._120; outerror904Rate = j.bankResponseCode._904;
                            outerror105Rate = j.bankResponseCode._105; outerror125Rate = j.bankResponseCode._125; outerror913Rate = j.bankResponseCode._913;

                            outerror106Rate = j.bankResponseCode._106; outerror200Rate = j.bankResponseCode._200; outerror923Rate = j.bankResponseCode._923;
                            outerror109Rate = j.bankResponseCode._109; outerror201Rate = j.bankResponseCode._201;
                            outerror110Rate = j.bankResponseCode._110; outerror202Rate = j.bankResponseCode._202;
                            outerror112Rate = j.bankResponseCode._112; outerror203Rate = j.bankResponseCode._203;


                            outerror102Rate = j.bankResponseCode._102; outerror908Rate = j.bankResponseCode._908;
                        outerror100Rate = j.bankResponseCode._100; outerror912Rate = j.bankResponseCode._912;
                        outerror114Rate = j.bankResponseCode._114; outerror111Rate = j.bankResponseCode._111;
                        outerror400Rate = j.bankResponseCode._400; outerror909Rate = j.bankResponseCode._909;

                        outerror107Rate = j.bankResponseCode._107; outerror116Rate = j.bankResponseCode._116;
                        outerror118Rate = j.bankResponseCode._118; outerror119Rate = j.bankResponseCode._119;
                        outerror121Rate = j.bankResponseCode._121; outerror910Rate = j.bankResponseCode._910;
                        outerror911Rate = j.bankResponseCode._911;
                    }
                }
                foreach (var k in rr.inwardSummary)
                {
                    if (k.bankCode == req.bankCode)
                    {
                        intotalVolume = k.totalVolume;
                        intotalFailure = k.totalFailure;
                        infailureRate = k.inFailed;
                        insuccess = 100 - k.inFailed;
                        insuccessvolume = Convert.ToInt16(k.bankResponseCode._000);
                        /*inerror102 = Convert.ToInt16(k.bankResponseCode._102);
                        inerror908 = Convert.ToInt16(k.bankResponseCode._908);
                        inerror100 = Convert.ToInt16(k.bankResponseCode._100);
                        inerror912 = Convert.ToInt16(k.bankResponseCode._912);
                        inerror114 = Convert.ToInt16(k.bankResponseCode._114);
                        inerror111 = Convert.ToInt16(k.bankResponseCode._111);
                        inerror400 = Convert.ToInt16(k.bankResponseCode._400);*/
                        inerror102 = Convert.ToInt16(k.bankResponseCodeCount._102);
                        inerror908 = Convert.ToInt16(k.bankResponseCodeCount._908);
                        inerror100 = Convert.ToInt16(k.bankResponseCodeCount._100);
                        inerror912 = Convert.ToInt16(k.bankResponseCodeCount._912);
                        inerror114 = Convert.ToInt16(k.bankResponseCodeCount._114);
                        inerror111 = Convert.ToInt16(k.bankResponseCodeCount._111);
                        inerror400 = Convert.ToInt16(k.bankResponseCodeCount._400);

                        inerror107 = Convert.ToInt16(k.bankResponseCodeCount._107);
                        inerror116 = Convert.ToInt16(k.bankResponseCodeCount._116);
                        inerror118 = Convert.ToInt16(k.bankResponseCodeCount._118);
                        inerror119 = Convert.ToInt16(k.bankResponseCodeCount._119);
                        inerror121 = Convert.ToInt16(k.bankResponseCodeCount._121);
                        inerror910 = Convert.ToInt16(k.bankResponseCodeCount._910);
                        inerror911 = Convert.ToInt16(k.bankResponseCodeCount._911);
                        inerror909 = Convert.ToInt16(k.bankResponseCodeCount._909);

                        inerror102Rate = Convert.ToDouble(k.bankResponseCode._102);
                        inerror908Rate = Convert.ToDouble(k.bankResponseCode._908);
                        inerror100Rate = Convert.ToDouble(k.bankResponseCode._100);
                        inerror912Rate = Convert.ToDouble(k.bankResponseCode._912);
                        inerror114Rate = Convert.ToDouble(k.bankResponseCode._114);
                        inerror111Rate = Convert.ToDouble(k.bankResponseCode._111);
                        inerror400Rate = Convert.ToDouble(k.bankResponseCode._400);
                        inerror909Rate = Convert.ToDouble(k.bankResponseCode._909);

                        inerror107Rate = Convert.ToDouble(k.bankResponseCode._107);
                        inerror116Rate = Convert.ToDouble(k.bankResponseCode._116);
                        inerror118Rate = Convert.ToDouble(k.bankResponseCode._118);
                        inerror119Rate = Convert.ToDouble(k.bankResponseCode._119);
                        inerror121Rate = Convert.ToDouble(k.bankResponseCode._121);
                        inerror910Rate = Convert.ToDouble(k.bankResponseCode._910);
                        inerror911Rate = Convert.ToDouble(k.bankResponseCode._911);

                            inerror101 = k.bankResponseCodeCount._101; inerror112 = k.bankResponseCodeCount._112;
                            inerror103 = k.bankResponseCodeCount._103; inerror115 = k.bankResponseCodeCount._115;
                            inerror104 = k.bankResponseCodeCount._104; inerror117 = k.bankResponseCodeCount._117;
                            inerror105 = k.bankResponseCodeCount._105; inerror120 = k.bankResponseCodeCount._120;
                            inerror106 = k.bankResponseCodeCount._106; inerror125 = k.bankResponseCodeCount._125;
                            inerror109 = k.bankResponseCodeCount._109; inerror200 = k.bankResponseCodeCount._200;
                            inerror110 = k.bankResponseCodeCount._110; inerror201 = k.bankResponseCodeCount._201;
                            inerror202 = k.bankResponseCodeCount._202; inerror204 = k.bankResponseCodeCount._204;
                            inerror203 = k.bankResponseCodeCount._203; inerror904 = k.bankResponseCodeCount._904;
                            inerror913 = k.bankResponseCodeCount._110; inerror923 = k.bankResponseCodeCount._923;

                            inerror101Rate = k.bankResponseCode._101; inerror115Rate = k.bankResponseCode._115; inerror203Rate = k.bankResponseCode._203;
                            inerror103Rate = k.bankResponseCode._103; inerror117Rate = k.bankResponseCode._117; inerror204Rate = k.bankResponseCode._204;
                            inerror104Rate = k.bankResponseCode._104; inerror120Rate = k.bankResponseCode._120; inerror904Rate = k.bankResponseCode._904;
                            inerror105Rate = k.bankResponseCode._105; inerror125Rate = k.bankResponseCode._125; inerror913Rate = k.bankResponseCode._913;

                            inerror106Rate = k.bankResponseCode._106; inerror200Rate = k.bankResponseCode._200; inerror923Rate = k.bankResponseCode._923;
                            inerror109Rate = k.bankResponseCode._109; inerror201Rate = k.bankResponseCode._201;
                            inerror110Rate = k.bankResponseCode._110; inerror202Rate = k.bankResponseCode._202;
                            inerror112Rate = k.bankResponseCode._112; inerror203Rate = k.bankResponseCode._203;

                        }
                }
                foreach (var l in yy.outwardSummary)
                {
                    if (l.bankCode == req.bankCode)
                    {
                        dailyTxnVolumeOut = l.totalVolume;

                        volumeFailedTxnOut = l.totalFailure;
                        dailyFailureRateOut = l.outFailed;
                        dailySuccessVolumeOut = l.bankResponseCodeCount._000;
                        dailySuccessRateOut = l.bankResponseCode._000;
                        outDaily_100 = l.bankResponseCodeCount._100;
                        outDaily_107 = l.bankResponseCodeCount._107;
                        outDaily_111 = l.bankResponseCodeCount._111;
                        outDaily_114 = l.bankResponseCodeCount._114;
                        outDaily_116 = l.bankResponseCodeCount._116;
                        outDaily_118 = l.bankResponseCodeCount._118;
                        outDaily_119 = l.bankResponseCodeCount._119;
                        outDaily_121 = l.bankResponseCodeCount._121;
                        outDaily_400 = l.bankResponseCodeCount._400;
                        outDaily_908 = l.bankResponseCodeCount._908;
                        outDaily_909 = l.bankResponseCodeCount._909;
                        outDaily_910 = l.bankResponseCodeCount._910;
                        outDaily_911 = l.bankResponseCodeCount._911;
                        outDaily_912 = l.bankResponseCodeCount._912;

                            outDaily_101 = l.bankResponseCodeCount._101;
                            outDaily_103 = l.bankResponseCodeCount._103;
                            outDaily_111 = l.bankResponseCodeCount._111;
                            outDaily_114 = l.bankResponseCodeCount._114;
                            outDaily_116 = l.bankResponseCodeCount._116;
                            outDaily_118 = l.bankResponseCodeCount._118;
                            outDaily_119 = l.bankResponseCodeCount._119;
                            outDaily_121 = l.bankResponseCodeCount._121;
                            outDaily_400 = l.bankResponseCodeCount._400;
                            outDaily_908 = l.bankResponseCodeCount._908;
                            outDaily_909 = l.bankResponseCodeCount._909;
                            outDaily_910 = l.bankResponseCodeCount._910;
                            outDaily_911 = l.bankResponseCodeCount._911;
                            outDaily_912 = l.bankResponseCodeCount._912;

                            outDaily_000 = l.bankResponseCodeCount._000;
                        outDailyRate_100 = l.bankResponseCode._100;
                        outDailyRate_107 = l.bankResponseCode._107;
                        outDailyRate_111 = l.bankResponseCode._111;
                        outDailyRate_114 = l.bankResponseCode._114;
                        outDailyRate_116 = l.bankResponseCode._116;
                        outDailyRate_118 = l.bankResponseCode._118;
                        outDailyRate_119 = l.bankResponseCode._119;
                        outDailyRate_121 = l.bankResponseCode._121;
                        outDailyRate_400 = l.bankResponseCode._400;
                        outDailyRate_908 = l.bankResponseCode._908;
                        outDailyRate_909 = l.bankResponseCode._909;
                        outDailyRate_910 = l.bankResponseCode._910;
                        outDailyRate_911 = l.bankResponseCode._911;
                        outDailyRate_912 = l.bankResponseCode._912;

                            outDailyRate_101 = l.bankResponseCode._101;
                            outDailyRate_103 = l.bankResponseCode._103;
                            outDailyRate_104 = l.bankResponseCode._104;
                            outDailyRate_105 = l.bankResponseCode._105;
                            outDailyRate_106 = l.bankResponseCode._106;
                            outDailyRate_109 = l.bankResponseCode._109;
                            outDailyRate_110 = l.bankResponseCode._110;
                            outDailyRate_112 = l.bankResponseCode._112;
                            outDailyRate_115 = l.bankResponseCode._115;
                            outDailyRate_117 = l.bankResponseCode._117;
                            outDailyRate_120 = l.bankResponseCode._120;
                            outDailyRate_125 = l.bankResponseCode._125;
                            outDailyRate_200 = l.bankResponseCode._200;
                            outDailyRate_201 = l.bankResponseCode._201;
                            outDailyRate_202 = l.bankResponseCode._202;
                            outDailyRate_203 = l.bankResponseCode._203;
                            outDailyRate_204 = l.bankResponseCode._204;
                            outDailyRate_904 = l.bankResponseCode._904;
                            outDailyRate_913 = l.bankResponseCode._913;
                            outDailyRate_923 = l.bankResponseCode._923;

                            outDailyRate_000 = l.bankResponseCode._000;



                    }

                }
                foreach (var m in yy.inwardSummary)
                {
                    if (m.bankCode == req.bankCode)
                    {
                        dailyTxnVolumeIn = m.totalVolume;

                        volumeFailedTxnIn = m.totalFailure;
                        dailyFailureRateIn = m.inFailed;
                        dailySuccessVolumeIn = m.bankResponseCodeCount._000;
                        dailySuccessRateIn = m.bankResponseCode._000;
                        inDaily_100 = m.bankResponseCodeCount._100;
                        inDaily_107 = m.bankResponseCodeCount._107;
                        inDaily_111 = m.bankResponseCodeCount._111;
                        inDaily_114 = m.bankResponseCodeCount._114;
                        inDaily_116 = m.bankResponseCodeCount._116;
                        inDaily_118 = m.bankResponseCodeCount._118;
                        inDaily_119 = m.bankResponseCodeCount._119;
                        inDaily_121 = m.bankResponseCodeCount._121;
                        inDaily_400 = m.bankResponseCodeCount._400;
                        inDaily_908 = m.bankResponseCodeCount._908;
                        inDaily_909 = m.bankResponseCodeCount._909;
                        inDaily_910 = m.bankResponseCodeCount._910;
                        inDaily_911 = m.bankResponseCodeCount._911;
                        inDaily_912 = m.bankResponseCodeCount._912;
                        inDaily_000 = m.bankResponseCodeCount._000;

                            inDaily_101 = Convert.ToInt16(m.bankResponseCodeCount._101);
                            inDaily_103 = Convert.ToInt16(m.bankResponseCodeCount._103);
                            inDaily_104 = Convert.ToInt16(m.bankResponseCodeCount._104);
                            inDaily_105 = Convert.ToInt16(m.bankResponseCodeCount._105);
                            inDaily_106 = Convert.ToInt16(m.bankResponseCodeCount._106);
                            inDaily_109 = Convert.ToInt16(m.bankResponseCodeCount._109);
                            inDaily_110 = Convert.ToInt16(m.bankResponseCodeCount._110);
                            inDaily_112 = Convert.ToInt16(m.bankResponseCodeCount._112);
                            inDaily_115 = Convert.ToInt16(m.bankResponseCodeCount._115);
                            inDaily_117 = Convert.ToInt16(m.bankResponseCodeCount._117);
                            inDaily_120 = Convert.ToInt16(m.bankResponseCodeCount._120);
                            inDaily_125 = Convert.ToInt16(m.bankResponseCodeCount._125);
                            inDaily_200 = Convert.ToInt16(m.bankResponseCodeCount._200);
                            inDaily_201 = Convert.ToInt16(m.bankResponseCodeCount._201);
                            inDaily_202 = Convert.ToInt16(m.bankResponseCodeCount._202);
                            inDaily_203 = Convert.ToInt16(m.bankResponseCodeCount._203);
                            inDaily_204 = Convert.ToInt16(m.bankResponseCodeCount._204);
                            inDaily_904 = Convert.ToInt16(m.bankResponseCodeCount._904);
                            inDaily_913 = Convert.ToInt16(m.bankResponseCodeCount._913);
                            inDaily_923 = Convert.ToInt16(m.bankResponseCodeCount._923);





                            inDailyRate_100 = m.bankResponseCode._100;
                        inDailyRate_107 = m.bankResponseCode._107;
                        inDailyRate_111 = m.bankResponseCode._111;
                        inDailyRate_114 = m.bankResponseCode._114;
                        inDailyRate_116 = m.bankResponseCode._116;
                        inDailyRate_118 = m.bankResponseCode._118;
                        inDailyRate_119 = m.bankResponseCode._119;
                        inDailyRate_121 = m.bankResponseCode._121;
                        inDailyRate_400 = m.bankResponseCode._400;
                        inDailyRate_908 = m.bankResponseCode._908;
                        inDailyRate_909 = m.bankResponseCode._909;
                        inDailyRate_910 = m.bankResponseCode._910;
                        inDailyRate_911 = m.bankResponseCode._911;
                        inDailyRate_912 = m.bankResponseCode._912;
                        inDailyRate_000 = m.bankResponseCode._000;

                            inDailyRate_101 = m.bankResponseCode._101;
                            inDailyRate_103 = m.bankResponseCode._103;
                            inDailyRate_104 = m.bankResponseCode._104;
                            inDailyRate_105 = m.bankResponseCode._105;
                            inDailyRate_106 = m.bankResponseCode._106;
                            inDailyRate_109 = m.bankResponseCode._109;
                            inDailyRate_110 = m.bankResponseCode._110;
                            inDailyRate_112 = m.bankResponseCode._112;
                            inDailyRate_115 = m.bankResponseCode._115;
                            inDailyRate_117 = m.bankResponseCode._117;
                            inDailyRate_120 = m.bankResponseCode._120;
                            inDailyRate_125 = m.bankResponseCode._125;
                            inDailyRate_200 = m.bankResponseCode._200;
                            inDailyRate_201 = m.bankResponseCode._201;
                            inDailyRate_202 = m.bankResponseCode._202;
                            inDailyRate_203 = m.bankResponseCode._203;
                            inDailyRate_204 = m.bankResponseCode._204;
                            inDailyRate_904 = m.bankResponseCode._904;
                            inDailyRate_913 = m.bankResponseCode._913;
                            
                            inDailyRate_923 = m.bankResponseCode._923;



                        }

                }

                resp.responseCode = "00";
                resp.transactionperiod = transactionperiod;
                resp.FailureRateList = y;
                resp.FailureRateListIn = x;
                resp.FailureRateListVolumeIn = xxx;
                resp.FailureRateListVolumeOut = yyy;
                resp.IndInRate = rr.industryInward.inFailed;
                resp.IndOutRate = rr.industryOutward.outFailed;
                resp.outfailureRate = outfailureRate;
                resp.outtotalFailure = outtotalFailure;
                resp.outtotalVolume = outtotalVolume;
                resp.outsuccess = outsuccess;
                resp.outsuccessvolume = outsuccessvolume;
                resp.outerror102 = outerror102;
                resp.outerror912 = outerror912;
                resp.outerror100 = outerror100;
                resp.outerror114 = outerror114;
                resp.outerror908 = outerror908;
                resp.outerror111 = outerror111;
                resp.outerror400 = outerror400;
                resp.outerror909 = outerror909;
                    //new addition
                    resp.outerror101 = outerror101;
                    resp.outerror103 = outerror103;
                    resp.outerror104 = outerror104;
                    resp.outerror105 = outerror105;
                    resp.outerror106 = outerror106;
                    resp.outerror109 = outerror109;
                    resp.outerror110 = outerror110;
                    resp.outerror112 = outerror112;
                    resp.outerror115 = outerror115;
                    resp.outerror117 = outerror117;
                    resp.outerror120 = outerror120;
                    resp.outerror125 = outerror125;
                    resp.outerror200 = outerror200;
                    resp.outerror201 = outerror201;
                    resp.outerror202 = outerror202;
                    resp.outerror203 = outerror203;
                    resp.outerror204 = outerror204;
                    resp.outerror904 = outerror904;
                    resp.outerror913 = outerror913;
                    resp.outerror914 = outerror914;
                    resp.outerror923 = outerror923;

                    resp.outerror101Rate = outerror101Rate;
                    resp.outerror103Rate = outerror103Rate;
                    resp.outerror104Rate = outerror104Rate;
                    resp.outerror105Rate = outerror105Rate;
                    resp.outerror106Rate = outerror106Rate;
                    resp.outerror109Rate = outerror109Rate;
                    resp.outerror110Rate = outerror110Rate;
                    resp.outerror112Rate = outerror112Rate;
                    resp.outerror115Rate = outerror115Rate;
                    resp.outerror117Rate = outerror117Rate;
                    resp.outerror120Rate = outerror120Rate;
                    resp.outerror125Rate = outerror125Rate;
                    resp.outerror200Rate = outerror200Rate;
                    resp.outerror201Rate = outerror201Rate;
                    resp.outerror202Rate = outerror202Rate;
                    resp.outerror203Rate = outerror203Rate;
                    resp.outerror204Rate = outerror204Rate;
                    resp.outerror904Rate = outerror904Rate;
                    resp.outerror913Rate = outerror913Rate;
                    resp.outerror914Rate = outerror914Rate;
                    resp.outerror923Rate = outerror923Rate;

                    resp.inerror101 = inerror101;
                    resp.inerror103 = inerror103;
                    resp.inerror104 = inerror104;
                    resp.inerror105 = inerror105;
                    resp.inerror106 = inerror106;
                    resp.inerror109 = inerror109;
                    resp.inerror110 = inerror110;
                    resp.inerror112 = inerror112;
                    resp.inerror115 = inerror115;
                    resp.inerror117 = inerror117;
                    resp.inerror120 = inerror120;
                    resp.inerror125 = inerror125;
                    resp.inerror200 = inerror200;
                    resp.inerror201 = inerror201;
                    resp.inerror202 = inerror202;
                    resp.inerror203 = inerror203;
                    resp.inerror204 = inerror204;
                    resp.inerror200 = inerror200;
                    resp.inerror201 = inerror201;
                    resp.inerror202 = inerror202;
                    resp.inerror203 = inerror203;
                    resp.inerror204 = inerror204;
                    resp.inerror904 = inerror904;
                    resp.inerror913 = inerror913;
                    resp.inerror914 = inerror914;
                    resp.inerror923 = inerror923;

                    resp.inerror101Rate = inerror101Rate;
                    resp.inerror103Rate = inerror103Rate;
                    resp.inerror104Rate = inerror104Rate;
                    resp.inerror105Rate = inerror105Rate;
                    resp.inerror106Rate = inerror106Rate;
                    resp.inerror109Rate = inerror109Rate;
                    resp.inerror110Rate = inerror110Rate;
                    resp.inerror112Rate = inerror112Rate;                  
                    resp.inerror115Rate = inerror115Rate;
                    resp.inerror117Rate = inerror117Rate;
                    resp.inerror120Rate = inerror120Rate;
                    resp.inerror125Rate = inerror125Rate;
                    resp.inerror200Rate = inerror200Rate;
                    resp.inerror201Rate = inerror201Rate;
                    resp.inerror202Rate = inerror202Rate;
                    resp.inerror203Rate = inerror203Rate;
                    resp.inerror204Rate = inerror204Rate;
                    resp.inerror904Rate = inerror904Rate;
                    resp.inerror913Rate = inerror913Rate;
                    resp.inerror914Rate = inerror914Rate;
                    resp.inerror923Rate = inerror923Rate;

                    resp.inDaily_101 = inDaily_101;
                    resp.inDaily_103 = inDaily_103;
                    resp.inDaily_104 = inDaily_104;
                    resp.inDaily_105 = inDaily_105;
                    resp.inDaily_106 = inDaily_106;
                    resp.inDaily_109 = inDaily_109;
                    resp.inDaily_110 = inDaily_110;
                    resp.inDaily_112 = inDaily_112;
                    resp.inDaily_115 = inDaily_115;
                    resp.inDaily_117 = inDaily_117;
                    resp.inDaily_120 = inDaily_120;
                    resp.inDaily_125 = inDaily_125;
                    resp.inDaily_200 = inDaily_200;
                    resp.inDaily_201 = inDaily_201;
                    resp.inDaily_202 = inDaily_202;
                    resp.inDaily_203 = inDaily_203;
                    resp.inDaily_204 = inDaily_204;
                    resp.inDaily_904 = inDaily_904;
                    resp.inDaily_913 = inDaily_913;
                    resp.inDaily_914 = inDaily_914;
                    resp.inDaily_923 = inDaily_923;
                    resp.inDailyRate_101 = inDailyRate_101;
                    resp.inDailyRate_103 = inDailyRate_103;
                    resp.inDailyRate_104 = inDailyRate_104;
                    resp.inDailyRate_105 = inDailyRate_105;
                    resp.inDailyRate_106 = inDailyRate_106;
                    resp.inDailyRate_109 = inDailyRate_109;
                    resp.inDailyRate_110 = inDailyRate_110;
                    resp.inDailyRate_112 = inDailyRate_112;
                    resp.inDailyRate_115 = inDailyRate_115;
                    resp.inDailyRate_117 = inDailyRate_117;
                    resp.inDailyRate_120 = inDailyRate_120;
                    resp.inDailyRate_125 = inDailyRate_125;
                    resp.inDailyRate_200 = inDailyRate_200;
                    resp.inDailyRate_203 = inDailyRate_203;
                    resp.inDailyRate_204 = inDailyRate_204;
                    resp.inDailyRate_904 = inDailyRate_904;
                    resp.inDailyRate_913 = inDailyRate_913;
                    resp.inDailyRate_914 = inDailyRate_914;
                    resp.inDailyRate_923 = inDailyRate_923;

                    resp.outDaily_101 = outDaily_101;
                    resp.outDaily_103 = outDaily_103;
                    resp.outDaily_104 = outDaily_104;
                    resp.outDaily_105 = outDaily_105;
                    resp.outDaily_106 = outDaily_106;
                    resp.outDaily_109 = outDaily_109;
                    resp.outDaily_110 = outDaily_110;
                    resp.outDaily_112 = outDaily_112;
                    resp.outDaily_115 = outDaily_115;
                    resp.outDaily_117 = outDaily_117;
                    resp.outDaily_120 = outDaily_120;
                    resp.outDaily_125 = outDaily_125;
                    resp.outDaily_200 = outDaily_200;
                    resp.outDaily_201 = outDaily_201;
                    resp.outDaily_202 = outDaily_202;
                    resp.outDaily_203 = outDaily_203;
                    resp.outDaily_204 = outDaily_204;
                    resp.outDaily_904 = outDaily_904;
                    resp.outDaily_913 = outDaily_913;
                    //resp.outDaily_914 = outDaily_914;
                    resp.outDaily_923 = outDaily_923;



                    resp.outDailyRate_101 = Convert.ToDouble(outDailyRate_101);
                    resp.outDailyRate_103 = Convert.ToDouble(outDailyRate_103);
                    resp.outDailyRate_104 = Convert.ToDouble(outDailyRate_104);
                    resp.outDailyRate_105 = Convert.ToDouble(outDailyRate_105);
                    resp.outDailyRate_106 = Convert.ToDouble(outDailyRate_106);
                    resp.outDailyRate_109 = Convert.ToDouble(outDailyRate_109);
                    resp.outDailyRate_110 = Convert.ToDouble(outDailyRate_110);
                    resp.outDailyRate_112 = Convert.ToDouble(outDailyRate_112);
                    resp.outDailyRate_115 = Convert.ToDouble(outDailyRate_115);
                    resp.outDailyRate_117 = Convert.ToDouble(outDailyRate_117);
                    resp.outDailyRate_120 = Convert.ToDouble(outDailyRate_120);
                    resp.outDailyRate_125 = Convert.ToDouble(outDailyRate_125);
                    resp.outDailyRate_200 = Convert.ToDouble(outDailyRate_200);
                    resp.outDailyRate_201 = Convert.ToDouble(outDailyRate_201);
                    resp.outDailyRate_202 = Convert.ToDouble(outDailyRate_202);
                    resp.outDailyRate_203 = Convert.ToDouble(outDailyRate_203);
                    resp.outDailyRate_204 = Convert.ToDouble(outDailyRate_204);
                    resp.outDailyRate_904 = Convert.ToDouble(outDailyRate_904);
                    resp.outDailyRate_913 = Convert.ToDouble(outDailyRate_913);
                    //resp.outDailyRate_914 = Convert.ToDouble(outDailyRate_914);
                    resp.outDailyRate_923 = Convert.ToDouble(outDailyRate_923);




                    //end new addition
                    resp.outerror102Rate = outerror102Rate;
                resp.outerror912Rate = outerror912Rate;
                resp.outerror100Rate = outerror100Rate;
                resp.outerror114Rate = outerror114Rate;
                resp.outerror908Rate = outerror908Rate;
                resp.outerror111Rate = outerror111Rate;
                resp.outerror400Rate = outerror400Rate;
                resp.outerror909Rate = outerror909Rate;

                resp.infailureRate = infailureRate;
                resp.intotalFailure = intotalFailure;
                resp.intotalVolume = intotalVolume;
                resp.insuccess = insuccess;
                resp.insuccessvolume = insuccessvolume;
                resp.inerror102 = inerror102;
                resp.inerror912 = inerror912;
                resp.inerror100 = inerror100;
                resp.inerror114 = inerror114;
                resp.inerror908 = inerror908;
                resp.inerror111 = inerror111;
                resp.inerror400 = inerror400;
                resp.inerror909 = inerror909;
                resp.inerror102Rate = inerror102Rate;
                resp.inerror912Rate = inerror912Rate;
                resp.inerror100Rate = inerror100Rate;
                resp.inerror114Rate = inerror114Rate;
                resp.inerror908Rate = inerror908Rate;
                resp.inerror111Rate = inerror111Rate;
                resp.inerror400Rate = inerror400Rate;
                resp.inerror909Rate = inerror909Rate;

                /**DAILY STATS **/
                resp.dailyTxnVolumeIn = dailyTxnVolumeIn;
                resp.volumeFailedTxnIn = volumeFailedTxnIn;
                resp.dailyFailureRateIn = dailyFailureRateIn;
                resp.dailySuccessVolumeIn = dailySuccessVolumeIn;
                resp.dailySuccessRateIn = dailySuccessRateIn;
                resp.inDaily_100 = inDaily_100;
                resp.inDaily_107 = inDaily_107;
                resp.inDaily_111 = inDaily_111;
                resp.inDaily_114 = inDaily_114;
                resp.inDaily_116 = inDaily_116;
                resp.inDaily_118 = inDaily_118;
                resp.inDaily_119 = inDaily_119;
                resp.inDaily_121 = inDaily_121;
                resp.inDaily_400 = inDaily_400;
                resp.inDaily_908 = inDaily_908;
                resp.inDaily_909 = inDaily_909;
                resp.inDaily_910 = inDaily_910;
                resp.inDaily_911 = inDaily_911;
                resp.inDaily_912 = inDaily_912;
                resp.inDaily_000 = inDaily_000;
                resp.inDailyRate_100 = inDailyRate_100;
                resp.inDailyRate_107 = inDailyRate_107;
                resp.inDailyRate_111 = inDailyRate_111;
                resp.inDailyRate_114 = inDailyRate_114;
                resp.inDailyRate_116 = inDailyRate_116;
                resp.inDailyRate_118 = inDailyRate_118;
                resp.inDailyRate_119 = inDailyRate_119;
                resp.inDailyRate_121 = inDailyRate_121;
                resp.inDailyRate_400 = inDailyRate_400;
                resp.inDailyRate_908 = inDailyRate_908;
                resp.inDailyRate_909 = inDailyRate_909;
                resp.inDailyRate_910 = inDailyRate_910;
                resp.inDailyRate_911 = inDailyRate_911;
                resp.inDailyRate_912 = inDailyRate_912;
                resp.inDailyRate_000 = inDailyRate_000;

                resp.dailyTxnVolumeOut = dailyTxnVolumeOut;
                resp.volumeFailedTxnOut = volumeFailedTxnOut;
                resp.dailyFailureRateOut = dailyFailureRateOut;
                resp.dailySuccessVolumeOut = dailySuccessVolumeOut;
                resp.dailySuccessRateOut = dailySuccessRateOut;
                resp.outDaily_100 = outDaily_100;
                resp.outDaily_107 = outDaily_107;
                resp.outDaily_111 = outDaily_111;
                resp.outDaily_114 = outDaily_114;
                resp.outDaily_116 = outDaily_116;
                resp.outDaily_118 = outDaily_118;
                resp.outDaily_119 = outDaily_119;
                resp.outDaily_121 = outDaily_121;
                resp.outDaily_400 = outDaily_400;
                resp.outDaily_908 = outDaily_908;
                resp.outDaily_909 = outDaily_909;
                resp.outDaily_910 = outDaily_910;
                resp.outDaily_911 = outDaily_911;
                resp.outDaily_912 = outDaily_912;
                resp.outDaily_000 = outDaily_000;

                   

                    resp.outDailyRate_100 = Convert.ToDouble(outDailyRate_100);
                resp.outDailyRate_107 = Convert.ToDouble(outDailyRate_107);
                resp.outDailyRate_111 = Convert.ToDouble(outDailyRate_111);
                resp.outDailyRate_114 = Convert.ToDouble(outDailyRate_114);
                resp.outDailyRate_116 = Convert.ToDouble(outDailyRate_116);
                resp.outDailyRate_118 = Convert.ToDouble(outDailyRate_118);
                resp.outDailyRate_119 = Convert.ToDouble(outDailyRate_119);
                resp.outDailyRate_121 = Convert.ToDouble(outDailyRate_121);
                resp.outDailyRate_400 = Convert.ToDouble(outDailyRate_400);
                resp.outDailyRate_908 = Convert.ToDouble(outDailyRate_908);
                resp.outDailyRate_909 = Convert.ToDouble(outDailyRate_909);
                resp.outDailyRate_910 = Convert.ToDouble(outDailyRate_910);
                resp.outDailyRate_911 = Convert.ToDouble(outDailyRate_911);
                resp.outDailyRate_912 = Convert.ToDouble(outDailyRate_912);
                resp.outDailyRate_000 = Convert.ToDouble(outDailyRate_000);


                output = JsonConvert.SerializeObject(resp);
                }
                else
                {
                    var startdate = DateTime.Now.AddMinutes(-5).ToString();
                    var enddate = DateTime.Now.ToString();


                    var request = req.bankData.Replace("\\", "");
                    var dailyrequest = req.dailyData.Replace("\\", "");

                    var rr = JsonConvert.DeserializeObject<NLastFiveMinutes>(request);
                    var yy = JsonConvert.DeserializeObject<DailyTransAnalytic>(dailyrequest);
                    var xx = JsonConvert.DeserializeObject<LatestTransAnalytics>(GetIndustryDashboardAnalytics());
                    List<ChartData> x = new List<ChartData>();
                    List<ChartData> y = new List<ChartData>();
                    List<ChartData> xxx = new List<ChartData>();
                    List<ChartData> yyy = new List<ChartData>();
                    foreach (var i in xx.data)
                    {
                        
                        ChartData d = new ChartData();
                        ChartData c = new ChartData();
                        ChartData e = new ChartData();
                        ChartData f = new ChartData();
                        if (i.in_failure_rate > 0)
                        {
                            d.FailureRate = Convert.ToInt16(i.in_failure_rate);
                            d.Date = ConvertDatetimeToUnixTimeStamp(i.date_created);

                            x.Add(d);
                        }
                        if (i.out_failure_rate > 0)
                        {
                            c.FailureRate = Convert.ToInt16(i.out_failure_rate);
                            c.Date = ConvertDatetimeToUnixTimeStamp(i.date_created);

                            y.Add(c);
                        }


                        if (Convert.ToInt16(i.total_in_volume) > 0)
                        {
                            e.FailureRate = Convert.ToInt16(i.total_in_volume);
                        e.Date = ConvertDatetimeToUnixTimeStamp(i.date_created);
                        
                            xxx.Add(e);
                        }
                        if (Convert.ToInt16(i.total_out_volume) > 0)
                        {
                            f.FailureRate = Convert.ToInt16(i.total_out_volume);
                        f.Date = ConvertDatetimeToUnixTimeStamp(i.date_created);
                       
                            yyy.Add(f);
                        }
                    }

                    //get the 

                    var transactionperiod = rr.startDate + " - " + rr.endDate;
                    
                   
                        
                            outtotalVolume =rr.industryOutward.totalVolume;
                            outtotalFailure = rr.industryOutward.totalFailure;
                            outfailureRate = rr.industryOutward.outFailed;
                            outsuccess = 100-rr.industryOutward.outFailed;
                            outsuccessvolume = Convert.ToInt16(rr.industryOutward.bankResponseCode._000);
                           
                            outerror102 = rr.industryOutward.bankResponseCodeCount._102; outerror908 = rr.industryOutward.bankResponseCodeCount._908;
                            outerror100 = rr.industryOutward.bankResponseCodeCount._100; outerror912 = rr.industryOutward.bankResponseCodeCount._912;
                            outerror114 = rr.industryOutward.bankResponseCodeCount._114; outerror111 = rr.industryOutward.bankResponseCodeCount._111;
                            outerror400 = rr.industryOutward.bankResponseCodeCount._400; outerror107 = rr.industryOutward.bankResponseCodeCount._107; 
                            outerror116 = rr.industryOutward.bankResponseCodeCount._116;
                            outerror118 = rr.industryOutward.bankResponseCodeCount._118; outerror119 = rr.industryOutward.bankResponseCodeCount._119;
                            outerror121 = rr.industryOutward.bankResponseCodeCount._121; outerror910 = rr.industryOutward.bankResponseCodeCount._910;
                            outerror911 = rr.industryOutward.bankResponseCodeCount._911;

                    outerror101 = rr.industryOutward.bankResponseCodeCount._101; outerror115 = rr.industryOutward.bankResponseCodeCount._115;
                    outerror103 = rr.industryOutward.bankResponseCodeCount._103; outerror117 = rr.industryOutward.bankResponseCodeCount._117;
                    outerror104 = rr.industryOutward.bankResponseCodeCount._104; outerror120 = rr.industryOutward.bankResponseCodeCount._120;
                    outerror105 = rr.industryOutward.bankResponseCodeCount._105; outerror125 = rr.industryOutward.bankResponseCodeCount._125;
                    outerror106 = rr.industryOutward.bankResponseCodeCount._106;
                    outerror109 = rr.industryOutward.bankResponseCodeCount._109; outerror200 = rr.industryOutward.bankResponseCodeCount._200;
                    outerror110 = rr.industryOutward.bankResponseCodeCount._110; outerror201 = rr.industryOutward.bankResponseCodeCount._201;
                    outerror112 = rr.industryOutward.bankResponseCodeCount._112; outerror202 = rr.industryOutward.bankResponseCodeCount._202;
                    outerror913 = rr.industryOutward.bankResponseCodeCount._913; outerror203 = rr.industryOutward.bankResponseCodeCount._203;
                    outerror914 = rr.industryOutward.bankResponseCodeCount._914; outerror204 = rr.industryOutward.bankResponseCodeCount._204;
                    outerror923 = rr.industryOutward.bankResponseCodeCount._923; outerror904 = rr.industryOutward.bankResponseCodeCount._904;


                    outerror102Rate = rr.industryOutward.bankResponseCode._102; outerror908Rate = rr.industryOutward.bankResponseCode._908;
                            outerror100Rate = rr.industryOutward.bankResponseCode._100; outerror912Rate = rr.industryOutward.bankResponseCode._912;
                            outerror114Rate = rr.industryOutward.bankResponseCode._114; outerror111Rate = rr.industryOutward.bankResponseCode._111;
                            outerror400Rate = rr.industryOutward.bankResponseCode._400; outerror909Rate = rr.industryOutward.bankResponseCode._909;

                            outerror107Rate = rr.industryOutward.bankResponseCode._107; outerror116Rate = rr.industryOutward.bankResponseCode._116;
                            outerror118Rate = rr.industryOutward.bankResponseCode._118; outerror119Rate = rr.industryOutward.bankResponseCode._119;
                            outerror121Rate = rr.industryOutward.bankResponseCode._121; outerror910Rate = rr.industryOutward.bankResponseCode._910;
                            outerror911Rate = rr.industryOutward.bankResponseCode._911;

                    outerror101Rate = rr.industryOutward.bankResponseCode._101; outerror115Rate = rr.industryOutward.bankResponseCode._115;
                    outerror103Rate = rr.industryOutward.bankResponseCode._103; outerror117Rate = rr.industryOutward.bankResponseCode._117;
                    outerror104Rate = rr.industryOutward.bankResponseCode._104; outerror120Rate = rr.industryOutward.bankResponseCode._120;
                    outerror105Rate = rr.industryOutward.bankResponseCode._105; outerror125Rate = rr.industryOutward.bankResponseCode._125;
                    outerror106Rate = rr.industryOutward.bankResponseCode._106; outerror200Rate = rr.industryOutward.bankResponseCode._200;
                    outerror109Rate = rr.industryOutward.bankResponseCode._109; outerror201Rate = rr.industryOutward.bankResponseCode._201;
                    outerror110Rate = rr.industryOutward.bankResponseCode._110; outerror202Rate = rr.industryOutward.bankResponseCode._202;
                    outerror112Rate = rr.industryOutward.bankResponseCode._112; outerror204Rate = rr.industryOutward.bankResponseCode._204;
                    outerror203Rate = rr.industryOutward.bankResponseCode._203; outerror914Rate = rr.industryOutward.bankResponseCode._914;
                    outerror904Rate = rr.industryOutward.bankResponseCode._904; outerror923Rate = rr.industryOutward.bankResponseCode._923;
                    outerror913Rate = rr.industryOutward.bankResponseCode._913; 
                    




                    intotalVolume = rr.industryInward.totalVolume;
                            intotalFailure = rr.industryInward.totalFailure;
                            infailureRate = rr.industryInward.inFailed;
                            insuccess = 100- rr.industryInward.inFailed;
                            insuccessvolume = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._000);
                          
                            inerror102 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._102);
                            inerror908 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._908);
                            inerror100 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._100);
                            inerror912 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._912);
                            inerror114 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._114);
                            inerror111 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._111);
                            inerror400 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._400);

                            inerror107 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._107);
                            inerror116 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._116);
                            inerror118 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._118);
                            inerror119 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._119);
                            inerror121 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._121);
                            inerror910 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._910);
                            inerror911 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._911);
                            inerror909 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._909);


                    inerror101 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._101);
                    inerror103 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._103);
                    inerror104 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._104);
                    inerror105 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._105);
                    inerror106 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._106);
                    inerror109 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._109);
                    inerror110 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._110);

                    inerror112 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._112);
                    inerror115 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._115);
                    inerror117 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._117);
                    inerror120 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._120);
                    inerror125 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._125);
                    inerror200 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._200);
                    inerror201 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._201);
                    inerror202 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._202);
                    inerror203 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._203);
                    inerror204 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._204);
                    inerror904 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._904);
                    inerror913 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._913);
                    inerror914 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._914);
                    inerror923 = Convert.ToInt16(rr.industryInward.bankResponseCodeCount._923);

                            inerror102Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._102);
                            inerror908Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._908);
                            inerror100Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._100);
                            inerror912Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._912);
                            inerror114Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._114);
                            inerror111Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._111);
                            inerror400Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._400);
                            inerror909Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._909);
                            inerror107Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._107);
                            inerror116Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._116);
                            inerror118Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._118);
                            inerror119Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._119);
                            inerror121Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._121);
                            inerror910Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._910);
                            inerror911Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._911);

                    inerror101Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._101);
                    inerror103Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._103);
                    inerror104Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._104);
                    inerror105Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._105);
                    inerror106Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._106);
                    inerror109Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._109);
                    inerror110Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._110);
                    inerror112Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._112);
                    inerror115Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._115);
                    inerror117Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._117);
                    inerror120Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._120);
                    inerror125Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._125);
                    inerror200Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._200);
                    inerror201Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._201);
                    inerror202Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._202);
                    inerror203Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._203);
                    inerror204Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._204);
                    inerror904Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._904);
                    inerror913Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._913);
                    inerror914Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._914);
                    inerror923Rate = Convert.ToDouble(rr.industryInward.bankResponseCode._923);




                    dailyTxnVolumeOut = yy.industryOutward.totalVolume;

                            volumeFailedTxnOut = yy.industryOutward.totalFailure;
                            dailyFailureRateOut = yy.industryOutward.outFailed;
                            dailySuccessVolumeOut = yy.industryOutward.bankResponseCodeCount._000;
                            dailySuccessRateOut = yy.industryOutward.bankResponseCode._000;
                            outDaily_100 = yy.industryOutward.bankResponseCodeCount._100;
                            outDaily_107 = yy.industryOutward.bankResponseCodeCount._107;
                            outDaily_111 = yy.industryOutward.bankResponseCodeCount._111;
                            outDaily_114 = yy.industryOutward.bankResponseCodeCount._114;
                            outDaily_116 = yy.industryOutward.bankResponseCodeCount._116;
                            outDaily_118 = yy.industryOutward.bankResponseCodeCount._118;
                            outDaily_119 = yy.industryOutward.bankResponseCodeCount._119;
                            outDaily_121 = yy.industryOutward.bankResponseCodeCount._121;
                            outDaily_400 = yy.industryOutward.bankResponseCodeCount._400;
                            outDaily_908 = yy.industryOutward.bankResponseCodeCount._908;
                            outDaily_909 = yy.industryOutward.bankResponseCodeCount._909;
                            outDaily_910 = yy.industryOutward.bankResponseCodeCount._910;
                            outDaily_911 = yy.industryOutward.bankResponseCodeCount._911;
                            outDaily_912 = yy.industryOutward.bankResponseCodeCount._912;
                            outDaily_000 = yy.industryOutward.bankResponseCodeCount._000;

                    outDaily_101 = yy.industryOutward.bankResponseCodeCount._101;
                    outDaily_103 = yy.industryOutward.bankResponseCodeCount._103;
                    outDaily_104 = yy.industryOutward.bankResponseCodeCount._104;
                    outDaily_105 = yy.industryOutward.bankResponseCodeCount._105;
                    outDaily_106 = yy.industryOutward.bankResponseCodeCount._106;
                    outDaily_109 = yy.industryOutward.bankResponseCodeCount._109;
                    outDaily_110 = yy.industryOutward.bankResponseCodeCount._110;
                    outDaily_112 = yy.industryOutward.bankResponseCodeCount._112;
                    outDaily_115 = yy.industryOutward.bankResponseCodeCount._115;
                    outDaily_117 = yy.industryOutward.bankResponseCodeCount._117;
                    outDaily_120 = yy.industryOutward.bankResponseCodeCount._120;
                    outDaily_125 = yy.industryOutward.bankResponseCodeCount._125;
                    outDaily_200 = yy.industryOutward.bankResponseCodeCount._200;
                    outDaily_201 = yy.industryOutward.bankResponseCodeCount._201;
                    outDaily_202 = yy.industryOutward.bankResponseCodeCount._202;
                    outDaily_203 = yy.industryOutward.bankResponseCodeCount._203;
                    outDaily_204 = yy.industryOutward.bankResponseCodeCount._204;
                    outDaily_904 = yy.industryOutward.bankResponseCodeCount._904;
                    outDaily_913 = yy.industryOutward.bankResponseCodeCount._913;
                    //outDaily_914 = yy.industryOutward.bankResponseCodeCount._912;
                    outDaily_923 = yy.industryOutward.bankResponseCodeCount._923;



                    outDailyRate_100 = yy.industryOutward.bankResponseCode._100;
                            outDailyRate_107 = yy.industryOutward.bankResponseCode._107;
                            outDailyRate_111 = yy.industryOutward.bankResponseCode._111;
                            outDailyRate_114 = yy.industryOutward.bankResponseCode._114;
                            outDailyRate_116 = yy.industryOutward.bankResponseCode._116;
                            outDailyRate_118 = yy.industryOutward.bankResponseCode._118;
                            outDailyRate_119 = yy.industryOutward.bankResponseCode._119;
                            outDailyRate_121 = yy.industryOutward.bankResponseCode._121;
                            outDailyRate_400 = yy.industryOutward.bankResponseCode._400;
                            outDailyRate_908 = yy.industryOutward.bankResponseCode._908;
                            outDailyRate_909 = yy.industryOutward.bankResponseCode._909;
                            outDailyRate_910 = yy.industryOutward.bankResponseCode._910;
                            outDailyRate_911 = yy.industryOutward.bankResponseCode._911;
                            outDailyRate_912 = yy.industryOutward.bankResponseCode._912;
                            outDailyRate_000 = yy.industryOutward.bankResponseCode._000;

                    outDailyRate_101 = yy.industryOutward.bankResponseCode._101;
                    outDailyRate_103 = yy.industryOutward.bankResponseCode._103;
                    outDailyRate_104 = yy.industryOutward.bankResponseCode._104;
                    outDailyRate_105 = yy.industryOutward.bankResponseCode._105;
                    outDailyRate_106 = yy.industryOutward.bankResponseCode._106;
                    outDailyRate_109 = yy.industryOutward.bankResponseCode._109;
                    outDailyRate_110 = yy.industryOutward.bankResponseCode._110;
                    outDailyRate_112 = yy.industryOutward.bankResponseCode._112;
                    outDailyRate_115 = yy.industryOutward.bankResponseCode._115;
                    outDailyRate_117 = yy.industryOutward.bankResponseCode._117;
                    outDailyRate_120 = yy.industryOutward.bankResponseCode._120;
                    outDailyRate_125 = yy.industryOutward.bankResponseCode._125;
                    outDailyRate_200 = yy.industryOutward.bankResponseCode._200;
                    outDailyRate_201 = yy.industryOutward.bankResponseCode._201;
                    outDailyRate_202 = yy.industryOutward.bankResponseCode._202;
                    outDailyRate_203 = yy.industryOutward.bankResponseCode._203;
                    outDailyRate_204 = yy.industryOutward.bankResponseCode._204;
                    outDailyRate_904 = yy.industryOutward.bankResponseCode._904;
                    outDailyRate_913 = yy.industryOutward.bankResponseCode._913;
                    //outDailyRate_914 = yy.industryOutward.bankResponseCode._912;
                    outDailyRate_923 = yy.industryOutward.bankResponseCode._923;







                    dailyTxnVolumeIn = yy.industryInward.totalVolume;

                            volumeFailedTxnIn = yy.industryInward.totalFailure;
                            dailyFailureRateIn = yy.industryInward.inFailed;
                            dailySuccessVolumeIn = yy.industryInward.bankResponseCodeCount._000;
                            dailySuccessRateIn = yy.industryInward.bankResponseCode._000;
                            inDaily_100 = yy.industryInward.bankResponseCodeCount._100;
                            inDaily_107 = yy.industryInward.bankResponseCodeCount._107;
                            inDaily_111 = yy.industryInward.bankResponseCodeCount._111;
                            inDaily_114 = yy.industryInward.bankResponseCodeCount._114;
                            inDaily_116 = yy.industryInward.bankResponseCodeCount._116;
                            inDaily_118 = yy.industryInward.bankResponseCodeCount._118;
                            inDaily_119 = yy.industryInward.bankResponseCodeCount._119;
                            inDaily_121 = yy.industryInward.bankResponseCodeCount._121;
                            inDaily_400 = yy.industryInward.bankResponseCodeCount._400;
                            inDaily_908 = yy.industryInward.bankResponseCodeCount._908;
                            inDaily_909 = yy.industryInward.bankResponseCodeCount._909;
                            inDaily_910 = yy.industryInward.bankResponseCodeCount._910;
                            inDaily_911 = yy.industryInward.bankResponseCodeCount._911;
                            inDaily_912 = yy.industryInward.bankResponseCodeCount._912;
                            inDaily_000 = yy.industryInward.bankResponseCodeCount._000;

                    inDaily_101 = yy.industryInward.bankResponseCodeCount._101;
                    inDaily_103 = yy.industryInward.bankResponseCodeCount._103;
                    inDaily_104 = yy.industryInward.bankResponseCodeCount._104;
                    inDaily_105 = yy.industryInward.bankResponseCodeCount._105;
                    inDaily_106 = yy.industryInward.bankResponseCodeCount._106;
                    inDaily_109 = yy.industryInward.bankResponseCodeCount._109;
                    inDaily_110 = yy.industryInward.bankResponseCodeCount._110;
                    inDaily_112 = yy.industryInward.bankResponseCodeCount._112;
                    inDaily_115 = yy.industryInward.bankResponseCodeCount._115;
                    inDaily_117 = yy.industryInward.bankResponseCodeCount._117;
                    inDaily_120 = yy.industryInward.bankResponseCodeCount._120;
                    inDaily_125 = yy.industryInward.bankResponseCodeCount._125;
                    inDaily_200 = yy.industryInward.bankResponseCodeCount._200;
                    inDaily_201 = yy.industryInward.bankResponseCodeCount._201;
                    inDaily_202 = yy.industryInward.bankResponseCodeCount._202;
                    inDaily_203 = yy.industryInward.bankResponseCodeCount._203;
                    inDaily_204 = yy.industryInward.bankResponseCodeCount._204;
                    inDaily_904 = yy.industryInward.bankResponseCodeCount._904;
                    inDaily_913 = yy.industryInward.bankResponseCodeCount._913;
                    inDaily_914 = yy.industryInward.bankResponseCodeCount._914;
                    inDaily_923 = yy.industryInward.bankResponseCodeCount._923;


                        inDailyRate_100 = yy.industryInward.bankResponseCode._100;
                                inDailyRate_107 = yy.industryInward.bankResponseCode._107;
                                inDailyRate_111 = yy.industryInward.bankResponseCode._111;
                                inDailyRate_114 = yy.industryInward.bankResponseCode._114;
                                inDailyRate_116 = yy.industryInward.bankResponseCode._116;
                                inDailyRate_118 = yy.industryInward.bankResponseCode._118;
                                inDailyRate_119 = yy.industryInward.bankResponseCode._119;
                                inDailyRate_121 = yy.industryInward.bankResponseCode._121;
                                inDailyRate_400 = yy.industryInward.bankResponseCode._400;
                                inDailyRate_908 = yy.industryInward.bankResponseCode._908;
                                inDailyRate_909 = yy.industryInward.bankResponseCode._909;
                                inDailyRate_910 = yy.industryInward.bankResponseCode._910;
                                inDailyRate_911 = yy.industryInward.bankResponseCode._911;
                                inDailyRate_912 = yy.industryInward.bankResponseCode._912;
                                inDailyRate_000 = yy.industryInward.bankResponseCode._000;

                    inDailyRate_101 = yy.industryInward.bankResponseCode._101;
                    inDailyRate_103 = yy.industryInward.bankResponseCode._103;
                    inDailyRate_104 = yy.industryInward.bankResponseCode._104;
                    inDailyRate_105 = yy.industryInward.bankResponseCode._105;
                    inDailyRate_106 = yy.industryInward.bankResponseCode._106;
                    inDailyRate_109 = yy.industryInward.bankResponseCode._109;
                    inDailyRate_110 = yy.industryInward.bankResponseCode._110;
                    inDailyRate_112 = yy.industryInward.bankResponseCode._112;
                    inDailyRate_115 = yy.industryInward.bankResponseCode._115;
                    inDailyRate_117 = yy.industryInward.bankResponseCode._117;
                    inDailyRate_120 = yy.industryInward.bankResponseCode._120;
                    inDailyRate_125 = yy.industryInward.bankResponseCode._125;
                    inDailyRate_200 = yy.industryInward.bankResponseCode._200;
                    inDailyRate_201 = yy.industryInward.bankResponseCode._201;
                    inDailyRate_202 = yy.industryInward.bankResponseCode._202;
                    inDailyRate_203 = yy.industryInward.bankResponseCode._203;
                    inDailyRate_204 = yy.industryInward.bankResponseCode._204;
                    inDailyRate_904 = yy.industryInward.bankResponseCode._904;
                    inDailyRate_913 = yy.industryInward.bankResponseCode._913;
                    //inDailyRate_914 = yy.industryInward.bankResponseCode._914;
                    inDailyRate_923 = yy.industryInward.bankResponseCode._923;





                    resp.responseCode = "00";
                    resp.transactionperiod = transactionperiod;
                    resp.FailureRateList = y;
                    resp.FailureRateListIn = x;
                    resp.FailureRateListVolumeIn = xxx;
                    resp.FailureRateListVolumeOut = yyy;
                    resp.IndInRate = rr.industryInward.inFailed;
                    resp.IndOutRate = rr.industryOutward.outFailed;

                    resp.outfailureRate = outfailureRate;
                    resp.outtotalFailure = outtotalFailure;
                    resp.outtotalVolume = outtotalVolume;
                    resp.outsuccess = outsuccess;
                    resp.outsuccessvolume = outsuccessvolume;
                    resp.outerror102 = outerror102;
                    resp.outerror912 = outerror912;
                    resp.outerror100 = outerror100;
                    resp.outerror114 = outerror114;
                    resp.outerror908 = outerror908;
                    resp.outerror111 = outerror111;
                    resp.outerror400 = outerror400;
                    resp.outerror909 = outerror909;
                    resp.outerror107 = outerror107;
                    resp.outerror116 = outerror116;
                    resp.outerror118 = outerror118;
                    resp.outerror119 = outerror119;
                    resp.outerror121 = outerror121;
                    resp.outerror910 = outerror910;
                    resp.outerror911 = outerror911;

                    resp.outerror101 = outerror101;
                    resp.outerror103 = outerror103;
                    resp.outerror104 = outerror104;
                    resp.outerror105 = outerror105;
                    resp.outerror106 = outerror106;
                    resp.outerror109 = outerror109;
                    resp.outerror110 = outerror110;
                    resp.outerror112 = outerror112;
                    resp.outerror115 = outerror115;
                    resp.outerror117 = outerror117;
                    resp.outerror120 = outerror120;
                    resp.outerror125 = outerror125;
                    resp.outerror200 = outerror200;
                    resp.outerror201 = outerror201;
                    resp.outerror202 = outerror202;
                    resp.outerror203 = outerror203;
                    resp.outerror204 = outerror204;
                    resp.outerror904 = outerror904;
                    resp.outerror913 = outerror913;
                    resp.outerror914 = outerror914;
                    resp.outerror923 = outerror923;


                    resp.outerror102Rate = outerror102Rate;
                    resp.outerror912Rate = outerror912Rate;
                    resp.outerror100Rate = outerror100Rate;
                    resp.outerror114Rate = outerror114Rate;
                    resp.outerror908Rate = outerror908Rate;
                    resp.outerror111Rate = outerror111Rate;
                    resp.outerror400Rate = outerror400Rate;
                    resp.outerror909Rate = outerror909Rate;
                    resp.outerror107Rate = outerror107Rate;
                    resp.outerror116Rate = outerror116Rate;
                    resp.outerror118Rate = outerror118Rate;
                    resp.outerror119Rate = outerror119Rate;
                    resp.outerror121Rate = outerror121Rate;
                    resp.outerror910Rate = outerror910Rate;
                    resp.outerror911Rate = outerror911Rate;

                    resp.outerror101Rate = outerror101Rate;
                    resp.outerror103Rate = outerror103Rate;
                    resp.outerror104Rate = outerror104Rate;
                    resp.outerror105Rate = outerror105Rate;
                    resp.outerror106Rate = outerror106Rate;
                    resp.outerror109Rate = outerror109Rate;
                    resp.outerror110Rate = outerror110Rate;
                    resp.outerror112Rate = outerror112Rate;
                    resp.outerror115Rate = outerror115Rate;
                    resp.outerror117Rate = outerror117Rate;
                    resp.outerror120Rate = outerror120Rate;
                    resp.outerror125Rate = outerror125Rate;
                    resp.outerror200Rate = outerror200Rate;
                    resp.outerror201Rate = outerror201Rate;
                    resp.outerror202Rate = outerror202Rate;
                    resp.outerror203Rate = outerror203Rate;
                    resp.outerror204Rate = outerror204Rate;
                    resp.outerror904Rate = outerror904Rate;
                    resp.outerror913Rate = outerror913Rate;
                    resp.outerror914Rate = outerror914Rate;
                    resp.outerror923Rate = outerror923Rate;


                    resp.infailureRate = infailureRate;
                    resp.intotalFailure = intotalFailure;
                    resp.intotalVolume = intotalVolume;
                    resp.insuccess = insuccess;
                    resp.insuccessvolume = insuccessvolume;
                    resp.inerror102 = inerror102;
                    resp.inerror912 = inerror912;
                    resp.inerror100 = inerror100;
                    resp.inerror114 = inerror114;
                    resp.inerror908 = inerror908;
                    resp.inerror111 = inerror111;
                    resp.inerror400 = inerror400;
                    resp.inerror909 = inerror909;
                    resp.inerror107 = inerror107;                    
                    resp.inerror119 = inerror119;
                    resp.inerror121 = inerror121;
                    resp.inerror910 = inerror910;
                    resp.inerror911 = inerror911;

                    resp.inerror101 = inerror101;
                    resp.inerror103 = inerror103;
                    resp.inerror104 = inerror104;
                    resp.inerror105 = inerror105;
                    resp.inerror106 = inerror106;
                    resp.inerror109 = inerror109;
                    resp.inerror110 = inerror110;
                    resp.inerror112 = inerror112;
                    resp.inerror115 = inerror115;
                    resp.inerror117 = inerror117;
                    resp.inerror120 = inerror120;
                    resp.inerror125 = inerror125;
                    resp.inerror200 = inerror200;
                    resp.inerror201 = inerror201;
                    resp.inerror202 = inerror202;
                    resp.inerror203 = inerror203;
                    resp.inerror204 = inerror204;
                    resp.inerror904 = inerror904;
                    resp.inerror913 = inerror913;
                    resp.inerror914 = inerror914;
                    resp.inerror923 = inerror923;



                    resp.inerror102Rate = inerror102Rate;
                    resp.inerror912Rate = inerror912Rate;
                    resp.inerror100Rate = inerror100Rate;
                    resp.inerror114Rate = inerror114Rate;
                    resp.inerror908Rate = inerror908Rate;
                    resp.inerror111Rate = inerror111Rate;
                    resp.inerror400Rate = inerror400Rate;
                    resp.inerror909Rate = inerror909Rate;
                    resp.inerror107Rate = inerror107Rate;
                    resp.inerror118Rate = inerror118Rate;
                    resp.inerror119Rate = inerror119Rate;
                    resp.inerror121Rate = inerror121Rate;
                    resp.inerror910Rate = inerror910Rate;
                    resp.inerror911Rate = inerror911Rate;

                    resp.inerror101Rate = inerror101Rate;
                    resp.inerror103Rate = inerror103Rate;
                    resp.inerror104Rate = inerror104Rate;
                    resp.inerror105Rate = inerror105Rate;
                    resp.inerror106Rate = inerror106Rate;
                    resp.inerror109Rate = inerror109Rate;
                    resp.inerror110Rate = inerror110Rate;
                    resp.inerror112Rate = inerror112Rate;
                    resp.inerror115Rate = inerror115Rate;
                    resp.inerror117Rate = inerror117Rate;
                    resp.inerror120Rate = inerror120Rate;
                    resp.inerror125Rate = inerror125Rate;
                    resp.inerror200Rate = inerror200Rate;
                    resp.inerror201Rate = inerror201Rate;
                    resp.inerror202Rate = inerror202Rate;
                    resp.inerror203Rate = inerror203Rate;
                    resp.inerror204Rate = inerror204Rate;
                    resp.inerror904Rate = inerror904Rate;
                    resp.inerror913Rate = inerror913Rate;
                    resp.inerror914Rate = inerror914Rate;
                    resp.inerror923Rate = inerror923Rate;


                    /**DAILY STATS **/
                    resp.dailyTxnVolumeIn = dailyTxnVolumeIn;
                    resp.volumeFailedTxnIn = volumeFailedTxnIn;
                    resp.dailyFailureRateIn = dailyFailureRateIn;
                    resp.dailySuccessVolumeIn = dailySuccessVolumeIn;
                    resp.dailySuccessRateIn = dailySuccessRateIn;
                    
                    resp.inDaily_100 = inDaily_100;
                    resp.inDaily_107 = inDaily_107;
                    resp.inDaily_111 = inDaily_111;
                    resp.inDaily_114 = inDaily_114;
                    resp.inDaily_116 = inDaily_116;
                    resp.inDaily_118 = inDaily_118;
                    resp.inDaily_119 = inDaily_119;
                    resp.inDaily_121 = inDaily_121;
                    resp.inDaily_400 = inDaily_400;
                    resp.inDaily_908 = inDaily_908;
                    resp.inDaily_909 = inDaily_909;
                    resp.inDaily_910 = inDaily_910;
                    resp.inDaily_911 = inDaily_911;
                    resp.inDaily_912 = inDaily_912;
                    resp.inDaily_000 = inDaily_000;

                    resp.inDaily_101 = inDaily_101;
                    resp.inDaily_103 = inDaily_103;
                    resp.inDaily_104 = inDaily_104;
                    resp.inDaily_105 = inDaily_105;
                    resp.inDaily_106 = inDaily_106;
                    resp.inDaily_109 = inDaily_109;
                    resp.inDaily_110 = inDaily_110;
                    resp.inDaily_112 = inDaily_112;
                    resp.inDaily_115 = inDaily_115;
                    resp.inDaily_117 = inDaily_117;
                    resp.inDaily_120 = inDaily_120;
                    resp.inDaily_125 = inDaily_125;
                    resp.inDaily_200 = inDaily_200;
                    resp.inDaily_201 = inDaily_201;
                    resp.inDaily_202 = inDaily_202;
                    resp.inDaily_203 = inDaily_203;
                    resp.inDaily_204 = inDaily_204;
                    resp.inDaily_904 = inDaily_904;
                    resp.inDaily_913 = inDaily_913;
                    resp.inDaily_914 = inDaily_914;
                    resp.inDaily_923 = inDaily_923;


                    resp.inDailyRate_100 = inDailyRate_100;
                    resp.inDailyRate_107 = inDailyRate_107;
                    resp.inDailyRate_111 = inDailyRate_111;
                    resp.inDailyRate_114 = inDailyRate_114;
                    resp.inDailyRate_116 = inDailyRate_116;
                    resp.inDailyRate_118 = inDailyRate_118;
                    resp.inDailyRate_119 = inDailyRate_119;
                    resp.inDailyRate_121 = inDailyRate_121;
                    resp.inDailyRate_400 = inDailyRate_400;
                    resp.inDailyRate_908 = inDailyRate_908;
                    resp.inDailyRate_909 = inDailyRate_909;
                    resp.inDailyRate_910 = inDailyRate_910;
                    resp.inDailyRate_911 = inDailyRate_911;
                    resp.inDailyRate_912 = inDailyRate_912;
                    resp.inDailyRate_000 = inDailyRate_000;

                    resp.inDailyRate_101 = inDailyRate_101;
                    resp.inDailyRate_103 = inDailyRate_103;
                    resp.inDailyRate_104 = inDailyRate_104;
                    resp.inDailyRate_105 = inDailyRate_105;
                    resp.inDailyRate_106 = inDailyRate_106;
                    resp.inDailyRate_109 = inDailyRate_109;
                    resp.inDailyRate_110 = inDailyRate_110;
                    resp.inDailyRate_112 = inDailyRate_112;
                    resp.inDailyRate_115 = inDailyRate_115;
                    resp.inDailyRate_117 = inDailyRate_117;
                    resp.inDailyRate_120 = inDailyRate_120;
                    resp.inDailyRate_125 = inDailyRate_125;
                    resp.inDailyRate_200 = inDailyRate_200;
                    resp.inDailyRate_201 = inDailyRate_201;
                    resp.inDailyRate_202 = inDailyRate_202;
                    resp.inDailyRate_203 = inDailyRate_203;
                    resp.inDailyRate_204 = inDailyRate_204;
                    resp.inDailyRate_904 = inDailyRate_904;
                    resp.inDailyRate_913 = inDailyRate_913;
                    resp.inDailyRate_914 = inDailyRate_914;
                    resp.inDailyRate_923 = inDailyRate_923;

                    resp.dailyTxnVolumeOut = dailyTxnVolumeOut;
                    resp.volumeFailedTxnOut = volumeFailedTxnOut;
                    resp.dailyFailureRateOut = dailyFailureRateOut;
                    resp.dailySuccessVolumeOut = dailySuccessVolumeOut;
                    resp.dailySuccessRateOut = dailySuccessRateOut;
                    resp.outDaily_100 = outDaily_100;
                    resp.outDaily_107 = outDaily_107;
                    resp.outDaily_111 = outDaily_111;
                    resp.outDaily_114 = outDaily_114;
                    resp.outDaily_116 = outDaily_116;
                    resp.outDaily_118 = outDaily_118;
                    resp.outDaily_119 = outDaily_119;
                    resp.outDaily_121 = outDaily_121;
                    resp.outDaily_400 = outDaily_400;
                    resp.outDaily_908 = outDaily_908;
                    resp.outDaily_909 = outDaily_909;
                    resp.outDaily_910 = outDaily_910;
                    resp.outDaily_911 = outDaily_911;
                    resp.outDaily_912 = outDaily_912;
                    resp.outDaily_000 = outDaily_000;

                    resp.outDaily_101 = outDaily_101;
                    resp.outDaily_103 = outDaily_103;
                    resp.outDaily_104 = outDaily_104;
                    resp.outDaily_105 = outDaily_105;
                    resp.outDaily_106 = outDaily_106;
                    resp.outDaily_109 = outDaily_109;
                    resp.outDaily_110 = outDaily_110;
                    resp.outDaily_112 = outDaily_112;
                    resp.outDaily_115 = outDaily_115;
                    resp.outDaily_117 = outDaily_117;
                    resp.outDaily_120 = outDaily_120;
                    resp.outDaily_125 = outDaily_125;
                    resp.outDaily_200 = outDaily_200;
                    resp.outDaily_201 = outDaily_201;
                    resp.outDaily_202 = outDaily_202;
                    resp.outDaily_203 = outDaily_203;
                    resp.outDaily_204 = outDaily_204;
                    resp.outDaily_904 = outDaily_904;
                    resp.outDaily_913 = outDaily_913;
                    //resp.outDaily_914 = outDaily_914;
                    resp.outDaily_923 = outDaily_923;

                    resp.outDailyRate_100 = Convert.ToDouble(outDailyRate_100);
                    resp.outDailyRate_107 = Convert.ToDouble(outDailyRate_107);
                    resp.outDailyRate_111 = Convert.ToDouble(outDailyRate_111);
                    resp.outDailyRate_114 = Convert.ToDouble(outDailyRate_114);
                    resp.outDailyRate_116 = Convert.ToDouble(outDailyRate_116);
                    resp.outDailyRate_118 = Convert.ToDouble(outDailyRate_118);
                    resp.outDailyRate_119 = Convert.ToDouble(outDailyRate_119);
                    resp.outDailyRate_121 = Convert.ToDouble(outDailyRate_121);
                    resp.outDailyRate_400 = Convert.ToDouble(outDailyRate_400);
                    resp.outDailyRate_908 = Convert.ToDouble(outDailyRate_908);
                    resp.outDailyRate_909 = Convert.ToDouble(outDailyRate_909);
                    resp.outDailyRate_910 = Convert.ToDouble(outDailyRate_910);
                    resp.outDailyRate_911 = Convert.ToDouble(outDailyRate_911);
                    resp.outDailyRate_912 = Convert.ToDouble(outDailyRate_912);
                    resp.outDailyRate_000 = Convert.ToDouble(outDailyRate_000);

                    resp.outDailyRate_101 = Convert.ToDouble(outDailyRate_101);
                    resp.outDailyRate_103 = Convert.ToDouble(outDailyRate_103);
                    resp.outDailyRate_104 = Convert.ToDouble(outDailyRate_104);
                    resp.outDailyRate_105 = Convert.ToDouble(outDailyRate_105);
                    resp.outDailyRate_106 = Convert.ToDouble(outDailyRate_106);
                    resp.outDailyRate_109 = Convert.ToDouble(outDailyRate_109);
                    resp.outDailyRate_110 = Convert.ToDouble(outDailyRate_110);
                    resp.outDailyRate_112 = Convert.ToDouble(outDailyRate_112);
                    resp.outDailyRate_115 = Convert.ToDouble(outDailyRate_115);
                    resp.outDailyRate_117 = Convert.ToDouble(outDailyRate_117);
                    resp.outDailyRate_120 = Convert.ToDouble(outDailyRate_120);
                    resp.outDailyRate_125 = Convert.ToDouble(outDailyRate_125);
                    resp.outDailyRate_200 = Convert.ToDouble(outDailyRate_200);
                    resp.outDailyRate_201 = Convert.ToDouble(outDailyRate_201);
                    resp.outDailyRate_202 = Convert.ToDouble(outDailyRate_202);

                    resp.outDailyRate_203 = Convert.ToDouble(outDailyRate_203);
                    resp.outDailyRate_204 = Convert.ToDouble(outDailyRate_204);
                    resp.outDailyRate_904 = Convert.ToDouble(outDailyRate_904);
                    resp.outDailyRate_913 = Convert.ToDouble(outDailyRate_913);
                   // resp.outDailyRate_914 = Convert.ToDouble(outDailyRate_914);
                    resp.outDailyRate_923 = Convert.ToDouble(outDailyRate_923);


                    output = JsonConvert.SerializeObject(resp);
                }




            }
            catch (Exception ex)
            {
                resp.responseCode = "96";
                

                output = JsonConvert.SerializeObject(ex.Message);
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        [Route("api/ListOfUsers2")]
        public async Task<HttpResponseMessage> ListOfUsers2()
        {
            var output = "";
            List<UserLists> u = new List<UserLists>();

            try
            {
                //var rr = JsonConvert.DeserializeObject<UList>(GetListOfUsers());
                ////deserialize the bank list
                ////var b = JsonConvert.DeserializeObject<U>(rr.data.ToString());

                //foreach (var i in rr.data)

                //{

                //    U bb = new U();

                //    bb.firstname = i.firstname;
                //    bb.lastname = i.lastname;
                //    bb.bankCode = i.bankCode;
                //    bb.bankAdmin = i.bankAdmin;
                //    bb.bankUser = i.bankUser;
                //    bb.email = i.email;

                //    u.Add(bb);
                //}

                using (TransactionMonitoringEntities db = new TransactionMonitoringEntities())
                {
                    var b = db.View_Users.ToList();
                    foreach (var i in b)
                    {
                        UserLists bb = new UserLists();
                        bb.email = i.email;
                        bb.firstname = i.firstname;
                        bb.lastname = i.lastname;
                        bb.bankName = i.name;
                        bb.roleID = i.roleName;
                        bb.role = i.role;
                        bb.bankID = i.bankCode;
                        u.Add(bb);
                    }
                }
                output = JsonConvert.SerializeObject(u);

            }
            catch (Exception ex)
            {

                output = JsonConvert.SerializeObject(ex.Message);
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        [Route("api/AddBank")]
        public async Task<HttpResponseMessage> AddBank(Banks b)
        {
            Response resp = new Response();
            AddBankRequest a = new AddBankRequest();
            var output = "";
            List<string> mail = new List<string>();

            try
            {
                a.name = b.name;
                a.code = b.code;
                a.alias = b.alias;
                a.sortCode = b.sortCode;
                a.failureThreshold = b.failureThreshold;
                a.volumeThreshold = b.volumeThreshold;

                var eM = b.email;
                char[] charsToTrim1 = { '|', ',' };
                eM.TrimEnd(charsToTrim1);
                string[] mailList = eM.Split(new string[] { "|" }, StringSplitOptions.None);
                foreach (string m in mailList)
                {
                    mail.Add(m);
                }
                   
                a.email = mail;

                var rs = JsonConvert.SerializeObject(a);

                var url = ConfigurationManager.AppSettings["AddBankURL"];
                var content = new StringContent(rs, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                HttpResponseMessage result = null;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new
              RemoteCertificateValidationCallback
              (
                 delegate { return true; }
              );
                result = client.PostAsync(url, content).Result;
                var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
                var rr = JsonConvert.DeserializeObject<AddUserResponse>(r);


                if (rr.status == "00")
                {
                    resp.responseCode = "00";
                    resp.responseMessage = rr.message;
                }
                else
                {
                    resp.responseCode = rr.status;
                    resp.responseMessage = rr.message;
                }

            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        [HttpPost]
        [Route("api/UpdateBank")]
        public async Task<HttpResponseMessage> UpdateBank(Banks b)
        {
            Response resp = new Response();
            UpdateBankRequest a = new UpdateBankRequest();
            var output = "";
            List<string> mail = new List<string>();
            try
            {
                a.name = b.name;
                a.code = b.code;
                a.alias = b.alias;
                a.active = b.active;
                a.sortCode = b.sortCode;
                a.failureThreshold = b.failureThreshold;
                a.volumeThreshold = b.volumeThreshold;

                var eM = b.email;
                char[] charsToTrim1 = { '|', ','};
                eM.TrimEnd(charsToTrim1);

                //string[] mailList = b.email.Split(new string[] { "|" }, StringSplitOptions.None);
                string[] mailList = eM.Split(new string[] { "|" }, StringSplitOptions.None);
                foreach (string m in mailList)
                {
                    mail.Add(m);
                }

                a.email = mail;

                //a.email = b.email;

                var rs = JsonConvert.SerializeObject(a);

                var url = ConfigurationManager.AppSettings["UpdateBankURL"];
                var content = new StringContent(rs, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                HttpResponseMessage result = null;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new
              RemoteCertificateValidationCallback
              (
                 delegate { return true; }
              );
                result = client.PostAsync(url, content).Result;
                var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
                var rr = JsonConvert.DeserializeObject<AddUserResponse>(r);


                if (rr.status == "00")
                {
                    resp.responseCode = "00";
                    resp.responseMessage = rr.message;
                }
                else
                {
                    resp.responseCode = rr.status;
                    resp.responseMessage = rr.message;
                }

            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        [Route("api/AddUser2")]
        public async Task<HttpResponseMessage> AddUser2(Userss b)
        {
            Response resp = new Response();
            AddUserRequest a = new AddUserRequest();
            var output = "";
            int userID = 0;
            try
            {
                a.bankAdmin = 1;
                a.bankUser = 1;
                a.bankCode = b.bankCode;
                a.email = b.email;
                a.firstname = b.firstname;
                a.lastname = b.lastname;



                using (TransactionMonitoringEntities db = new TransactionMonitoringEntities())
                {
                    User u = new User();
                    //u.bankCode = b.bankCode;
                    u.bankCode = b.bankCode;
                    u.email = b.email;
                    u.firstname = b.firstname;
                    u.lastname = b.lastname;
                    u.password = "password";
                    u.role = b.roleID;
                    //u.role = 1;

                    db.Users.Add(u);
                    db.SaveChanges();

                    userID = u.userID;
                }
                if (userID > 0)
                {
                    resp.responseCode = "00";
                    resp.responseMessage = "User Added Successfully";
                }
                else
                {
                    resp.responseCode = "96";
                    resp.responseMessage = "Error Adding User";
                }
            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        [Route("api/AddUser")]
        public async Task<HttpResponseMessage> AddUser(Userss b)
        {
            Response resp = new Response();
            AddUserRequest a = new AddUserRequest();
            var output = "";
            int userID = 0;
            try
            {
                if (b.roleID == 1)
                {
                    a.bankAdmin = 0;
                    a.bankUser = 1;
                    a.ipslAdmin = 0;
                    a.bankCode = b.bankCode;
                }
                else if (b.roleID == 2)
                {
                    a.bankAdmin = 1;
                    a.bankUser = 0;
                    a.ipslAdmin = 0;
                    a.bankCode = b.bankCode;
                }
                else if (b.roleID == 3)
                {
                a.bankAdmin = 0;
                a.bankUser = 0;
                    a.ipslAdmin = 1;
                    a.bankCode = "001";
                }
                else if (b.roleID == 4)
                {
                    a.bankAdmin = 0;
                    a.bankUser = 0;
                    a.ipslAdmin = 0;
                    a.bankCode = "001";
                }
                

                //a.bankAdmin = 1;
                //a.bankUser = 1;

                a.email = b.email;
                a.firstname = b.firstname;
                a.lastname = b.lastname;
                var rs = JsonConvert.SerializeObject(a);

                var url = ConfigurationManager.AppSettings["AddUsersURL"];
                var content = new StringContent(rs, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                HttpResponseMessage result = null;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new
              RemoteCertificateValidationCallback
              (
                 delegate { return true; }
              );
                result = client.PostAsync(url, content).Result;
                var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
                var rr = JsonConvert.DeserializeObject<AddUserResponse>(r);


                //using (TransactionMonitoringEntities db = new TransactionMonitoringEntities())
                //{
                //    User u = new User();
                //    //u.bankCode = b.bankCode;
                //    u.bankCode = "1";
                //    u.email = b.email;
                //    u.firstname = b.firstname;
                //    u.lastname = b.lastname;
                //    u.password = "password";
                //    // u.role = b.roleID;
                //    u.role = 1;

                //    db.Users.Add(u);
                //    db.SaveChanges();

                //    userID =u.userID;
                //}
                if (rr.status == "00")
                {
                    resp.responseCode = "00";
                    resp.responseMessage = rr.message;
                }
                else
                {
                    resp.responseCode = rr.status;
                    resp.responseMessage = rr.message;
                }
            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        [HttpPost]
        [Route("api/ResetUser")]
        public async Task<HttpResponseMessage> ResetUser(ResetUserRequest b)
        {
            Response resp = new Response();
            ResetUserRequest a = new ResetUserRequest();
            var output = "";

            try
            {
                a.email = b.email;
                a.ipslAdmin = 0;
                var rs = JsonConvert.SerializeObject(a);

                var url = ConfigurationManager.AppSettings["ResetUsersURL"];
                var content = new StringContent(rs, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                HttpResponseMessage result = null;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new
              RemoteCertificateValidationCallback
              (
                 delegate { return true; }
              );
                result = client.PostAsync(url, content).Result;
                var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
                var rr = JsonConvert.DeserializeObject<AddUserResponse>(r);



                if (rr.status == "00")
                {
                    resp.responseCode = "00";
                    resp.responseMessage = rr.message;
                }
                else
                {
                    resp.responseCode = rr.status;
                    resp.responseMessage = rr.message;
                }
            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        [Route("api/ResetUser2")]
        public async Task<HttpResponseMessage> ResetUser2(ResetUserRequest b)
        {
            Response resp = new Response();
            ResetUserRequest a = new ResetUserRequest();
            var output = "";
            var status = "00";

            try
            {
                a.email = b.email;




                if (status == "00")
                {
                    resp.responseCode = "00";
                    resp.responseMessage = "User reset successful";
                }
                else
                {
                    resp.responseCode = "96";
                    resp.responseMessage = "User reset failed!";
                }
            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        [Route("api/ChangePassword")]
        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordRequest b)
        {
            Response resp = new Response();

            var output = "";

            try
            {

                var rs = JsonConvert.SerializeObject(b);

                var url = ConfigurationManager.AppSettings["ChangePasswordURL"];
                var content = new StringContent(rs, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                HttpResponseMessage result = null;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new
              RemoteCertificateValidationCallback
              (
                 delegate { return true; }
              );
                result = client.PostAsync(url, content).Result;
                var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
                var rr = JsonConvert.DeserializeObject<AddUserResponse>(r);



                if (rr.status == "00")
                {
                    resp.responseCode = "00";
                    resp.responseMessage = rr.message;
                }
                else
                {
                    resp.responseCode = rr.status;
                    resp.responseMessage = rr.message;
                }
            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        [Route("api/ChangePassword2")]
        public async Task<HttpResponseMessage> ChangePassword2(ChangePasswordRequest b)
        {
            Response resp = new Response();

            var output = "";

            try
            {

                var status = "00";


                if (status == "00")
                {
                    resp.responseCode = "00";
                    resp.responseMessage = "Password changed successfully";
                }
                else
                {
                    resp.responseCode = "96";
                    resp.responseMessage = "Error changing password";
                }
            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        /*public int ReturnPosition(string respCode,string dateFrom,string dateTo,string sortCode)
        {
            //var dF=dateFrom.ToString("yyyy-mm-DD");
            var c = ConfigurationManager.AppSettings["cString"];
            var index = 0;
            decimal ccc = 0;
            decimal ddd = 0;
            using (SqlConnection connection = new SqlConnection(c))

            {
                var q = $"SELECT SUM([TRANSACTIONCOUNT]) cnt,RECEIVERS_SORT_CODE from [TransactionMonitoring].[dbo].[Report] WHERE TRANSDATE BETWEEN '{dateFrom}' AND '{dateTo}' AND RESPONSECODE='{respCode}' GROUP BY RECEIVERS_SORT_CODE ORDER BY cnt DESC";
                var reports = connection.Query<Report>(q);
                List<Decimal> termsList = new List<Decimal>();
                decimal[] terms;
                foreach (var rr in reports)
                {
                    ccc = Convert.ToDecimal(rr.TRANSACTIONCOUNT);
                    ddd = ccc / rr.cnt * 100;
                    termsList.Add(ddd);
                    terms = termsList.ToArray();

                }
               // Array.Sort(terms);
                //Array.Reverse(values);
                index = Array.FindIndex(reports.ToArray(), row => row.RECEIVERS_SORT_CODE == sortCode);




            }
            return index;
        }*/
        public int ReturnRank(string ErrorCode,string sortCode,DateTime dtTo)
        {
            int rank =0;
            try
            {
                using(BIDATABASEEntities1 db=new BIDATABASEEntities1())
                {
                    var details = db.banksefficiencies.FirstOrDefault(b => b.BANK == sortCode && b.RESPONSECODE == ErrorCode && b.ENDDATE ==dtTo && b.ORIENTATION=="INFLOW");
                    if (details != null)
                    {
                        rank = details.WEEKLYRANK;
                    }
                    else
                    {
                        rank = 0;
                    }
                }
            }catch(Exception ex)
            {

            }
            return rank;
        }

        public decimal WeeklyRatingIn(string ErrorCode, DateTime dtTo,string sortCode)
        {
            decimal rating = 0;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var details = db.banksefficiencies.FirstOrDefault(b => b.RESPONSECODE == ErrorCode && b.ENDDATE == dtTo && b.ORIENTATION == "INFLOW" && b.BANK==sortCode);
                    if (details != null)
                    {
                        rating = details.PCTEFF;
                    }
                    else
                    {
                        rating = 0;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return rating;
        }

        public decimal WeeklyRatingOut(string ErrorCode, DateTime dtTo, string sortCode)
        {
            decimal rating = 0;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var details = db.banksefficiencies.FirstOrDefault(b => b.RESPONSECODE == ErrorCode && b.ENDDATE == dtTo && b.ORIENTATION == "OUTFLOW" && b.BANK == sortCode);
                    if (details != null)
                    {
                        rating = details.PCTEFF;
                    }
                    else
                    {
                        rating = 0;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return rating;
        }


        public decimal BestBankRatingIn(string ErrorCode,DateTime dtTo)
        {
            decimal rating = 0;
            try
            {
                using(BIDATABASEEntities1 db=new BIDATABASEEntities1())
                {
                    var details = db.banksefficiencies.FirstOrDefault(b => b.RESPONSECODE == ErrorCode && b.WEEKLYRANK == 1 && b.ENDDATE==dtTo && b.ORIENTATION=="INFLOW");
                    if(details != null)
                    {
                        rating = details.PCTEFF;
                    }
                    else
                    {
                        rating = 0;
                    }
                }
            }catch(Exception ex)
            {

            }
            return rating;
        }

        public decimal BestBankRatingOut(string ErrorCode, DateTime dtTo)
        {
            decimal rating = 0;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var details = db.banksefficiencies.FirstOrDefault(b => b.RESPONSECODE == ErrorCode && b.WEEKLYRANK == 1 && b.ENDDATE == dtTo && b.ORIENTATION == "OUTFLOW");
                    if (details != null)
                    {
                        rating = details.PCTEFF;
                    }
                    else
                    {
                        rating = 0;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return rating;
        }

        public int WeeklyRankIn(string ErrorCode, DateTime dtTo,string sortCode)
        {
            int ranking = 0;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var details = db.banksefficiencies.FirstOrDefault(b => b.RESPONSECODE == ErrorCode && b.BANK==sortCode && b.ENDDATE == dtTo && b.ORIENTATION == "INFLOW");
                    if (details != null)
                    {
                        ranking = details.WEEKLYRANK;
                    }
                    else
                    {
                        ranking = 0;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ranking;
        }
        public int WeeklyRankOut(string ErrorCode, DateTime dtTo, string sortCode)
        {
            int ranking = 0;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var details = db.banksefficiencies.FirstOrDefault(b => b.RESPONSECODE == ErrorCode && b.BANK == sortCode && b.ENDDATE == dtTo && b.ORIENTATION == "OUTFLOW");
                    if (details != null)
                    {
                        ranking = details.WEEKLYRANK;
                    }
                    else
                    {
                        ranking = 0;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ranking;
        }

        public int ReturnRank2(string ErrorCode, string sortCode, DateTime dtTo)
        {
            int rank = 0;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var details = db.banksefficiencies.FirstOrDefault(b => b.BANK == sortCode && b.RESPONSECODE == ErrorCode && b.ORIENTATION == "OUTFLOW" && b.ENDDATE==dtTo);
                    if (details != null)
                    {
                        rank = details.WEEKLYRANK;
                    }
                    else
                    {
                        rank = 0;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return rank;
        }

        public string GetBankName(string sortCode)
        {
            var name = "";
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var details = db.Bank_Description.FirstOrDefault(a => a.SORT_CODE == sortCode);
                    if (details != null)
                    {
                        name = details.BANK_NAME;
                    }
                    else
                    {
                        name = "";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return name;
        }


        public string ReturnPosition(int position)
        {
            var pp = "";
           var p = position.ToOrdinal();
            if (p == "0th")
            {
                //pp = "1st";
                pp = "--";
            }
            else
            {
                pp = p;
            }
            
            return pp;
        }
        [HttpPost]
        [Route("api/GetReport")]
        public async Task<HttpResponseMessage> GetReport(BankReportRequest req)
        {
            var output = "";
            DateTime dtFrom = Convert.ToDateTime(req.dateTo);
            DateTime dtTo = Convert.ToDateTime(req.dateTo);

            DateTime lastweekTo = req.dateTo.AddDays(-1);
            DateTime lastweekFrom = req.dateTo.AddDays(-7);

            DateTime lastweekToDisp = Convert.ToDateTime(req.dateTo.AddDays(-0));
            DateTime lastweekFromDispl = Convert.ToDateTime(req.dateTo.AddDays(-7));

            //var p = ReturnPosition(21);
            decimal s911 = 0;decimal s912 = 0;decimal slink = 0;decimal sInt = 0;decimal s121 = 0;decimal s909 = 0;


            decimal totalTransIn = 0;
            int totalTransOut = 0;

            decimal totalTransInWeek = 0;
            int totalTransOutWeek = 0;

            decimal totaltrans114 = 0;
            decimal totaltrans911 = 0;
            decimal totaltrans111 = 0;
            decimal totaltrans400 = 0;
            decimal totaltrans912 = 0;
            decimal totaltrans102 = 0;
            decimal totaltrans100 = 0;
            decimal totaltrans120 = 0;
            decimal totaltrans118 = 0;

            decimal totaltrans114w = 0;
            decimal totaltrans911w = 0;
            decimal totaltrans111w = 0;
            decimal totaltrans400w = 0;
            decimal totaltrans912w = 0;
            decimal totaltrans102w= 0;
            decimal totaltrans100w = 0;
            decimal totaltrans120w = 0;
            decimal totaltrans118w = 0;

            decimal totaltransOut114 = 0;
            decimal totaltransOut911 = 0;
            decimal totaltransOut111 = 0;
            decimal totaltransOut400 = 0;
            decimal totaltransOut912 = 0;
            decimal totaltransOut102 = 0;
            decimal totaltransOut100 = 0;
            decimal totaltransOut120 = 0;
            decimal totaltransOut118 = 0;
            decimal totaltransOut121 = 0;
            decimal totaltransOut909 = 0;

            decimal totaltransOut114w = 0;
            decimal totaltransOut911w = 0;
            decimal totaltransOut111w = 0;
            decimal totaltransOut400w = 0;
            decimal totaltransOut912w = 0;
            decimal totaltransOut102w = 0;
            decimal totaltransOut100w = 0;
            decimal totaltransOut120w = 0;
            decimal totaltransOut118w = 0;
            decimal totaltransOut121w = 0;
            decimal totaltransOut909w = 0;

            int In114P = 0;


            decimal In114=0;decimal In111 = 0; decimal In400 = 0; decimal In912 = 0;
            decimal In911 = 0; decimal In102 = 0; decimal In100 = 0; decimal In120 = 0; decimal In118 = 0;
            decimal Out114 = 0; decimal Out111 = 0; decimal Out400 = 0; decimal Out912 = 0;
            decimal Out911 = 0; decimal Out102 = 0; decimal Out100 = 0; decimal Out120 = 0; decimal Out118 = 0;decimal Out121 = 0;decimal Out909 = 0;

            decimal In114w = 0; decimal In111w = 0; decimal In400w = 0; decimal In912w = 0;
            decimal In911w = 0; decimal In102w = 0; decimal In100w = 0; decimal In120w = 0; decimal In118w = 0;
            decimal Out114w = 0; decimal Out111w = 0; decimal Out400w = 0; decimal Out912w = 0;
            decimal Out911w = 0; decimal Out102w = 0; decimal Out100w = 0; decimal Out120w = 0; decimal Out118w = 0; decimal Out121w = 0; decimal Out909w = 0;

            var In114r="";var In111r = ""; var In400r = ""; var In912r = ""; var In911r = "";
            var In102r = ""; var In100r = ""; var In120r = ""; var In118r = "";
            var Out114r = ""; var Out111r = ""; var Out400r = ""; var Out912r = ""; var Out911r = "";
            var Out102r = ""; var Out100r = ""; var Out120r = ""; var Out118r = ""; var Out121r = ""; var Out909r = "";

            decimal In114bb = 0; decimal In111bb = 0; decimal In400bb = 0; decimal In912bb = 0; decimal In911bb = 0;
            decimal In102bb = 0; decimal In100bb = 0; decimal In120bb = 0; decimal In118bb = 0;
            decimal Out114bb = 0; decimal Out111bb = 0; decimal Out400bb = 0; decimal Out912bb = 0; decimal Out911bb = 0;
            decimal Out102bb = 0; decimal Out100bb = 0; decimal Out120bb = 0; decimal Out118bb = 0; decimal Out121bb = 0; decimal Out909bb = 0;

            int Out114wRank = 0; int Out911wRank = 0;int Out111wRank = 0;
            int Out400wRank = 0; int Out912wRank = 0;  int Out102wRank = 0;
            int Out100wRank = 0; int Out120wRank = 0;int Out118wRank = 0;
            int Out121wRank = 0; int Out909wRank = 0;

            int In114wRank = 0; int In911wRank = 0; int In111wRank = 0;
            int In400wRank = 0; int In912wRank = 0; int In102wRank = 0;
            int In100wRank = 0; int In120wRank = 0; int In118wRank = 0;

            BankReportResponse resp = new BankReportResponse();
            ErrorResponse r = new ErrorResponse();
            try
            {
                using(BIDATABASEEntities1 db=new BIDATABASEEntities1())
                {
                    
                    var In = db.banksefficiencies.Where(a => a.ENDDATE >= req.dateFrom && a.ENDDATE <= req.dateTo && a.BANK == req.sortCode && a.RESPONSECODE !="0" && a.ORIENTATION=="INFLOW");
                    var Out= db.banksefficiencies.Where(a => a.ENDDATE >= req.dateFrom && a.ENDDATE <= req.dateTo && a.BANK == req.sortCode && a.RESPONSECODE != "0" && a.ORIENTATION=="OUTFLOW");

                    var Inw = db.banksefficiencies.Where(a => a.ENDDATE == lastweekFrom && a.BANK == req.sortCode && a.RESPONSECODE != "0" && a.ORIENTATION=="INFLOW");
                    var Outw = db.banksefficiencies.Where(a => a.ENDDATE == lastweekFrom  && a.BANK == req.sortCode && a.RESPONSECODE != "0" && a.ORIENTATION == "OUTFLOW");
                    if (In.Count()> 0)
                    {
                        foreach (var i in In)
                        {
                            //totalTransIn += Convert.ToInt16(i.TRANSACTIONCOUNT);
                            if (i.RESPONSECODE == "114")
                            {
                                In114 = i.PCTEFF;
                               
                            }
                            if (i.RESPONSECODE == "911")
                            {
                                In911 = i.PCTEFF;

                            }
                            if (i.RESPONSECODE == "111")
                            {
                                In111 = i.PCTEFF;

                            }
                            if (i.RESPONSECODE == "400")
                            {
                                In400 = i.PCTEFF;

                            }
                            if (i.RESPONSECODE == "912")
                            {
                                In912 = i.PCTEFF;

                            }
                            if (i.RESPONSECODE == "102")
                            {
                                In102 = i.PCTEFF;

                            }
                            if (i.RESPONSECODE == "100")
                            {
                                In100 = i.PCTEFF; ;
                                
                            }
                            if (i.RESPONSECODE == "120")
                            {
                                In120 = i.PCTEFF;

                            }
                            if (i.RESPONSECODE == "118")
                            {
                                In118 = i.PCTEFF;

                            }

                        }
                       // In114 = (totaltrans114 / totalTransIn) * 100;
                       //// In114P = ReturnPosition("114", dtFrom.ToString("yyyy-MM-dd"), dtTo.ToString("yyyy-MM-dd"), req.sortCode);
                       // In911 = (totaltrans911 / totalTransIn) * 100;
                       // In111 = (totaltrans111 / totalTransIn) * 100;
                       // In400 = (totaltrans400 / totalTransIn) * 100;
                       // In912 = (totaltrans912 / totalTransIn) * 100;
                       // In102 = (totaltrans102 / totalTransIn) * 100;
                       // In100 = (totaltrans100 / totalTransIn) * 100;
                       // In120 = (totaltrans120 / totalTransIn) * 100;
                       // In118 = (totaltrans118 / totalTransIn) * 100;
                    }
                    else
                    {
                        In114 = 0;
                        
                        In911 = 0;
                        In111 = 0;
                        In400 = 0;
                        In912 = 0;
                        In102 = 0;
                        In100 = 0;
                        In120 = 0;
                        In118 = 0;
                    }
                    
                    if (Inw.Count() > 0)
                    {
                        int In114wCount = 0;
                        int In911wCount = 0;
                        int In111wCount = 0;
                        int In400wCount = 0;
                        int In912wCount = 0;
                        int In102wCount = 0;
                        int In100wCount = 0;
                        int In120wCount = 0;
                        int In118wCount = 0;
                        foreach (var j in Inw)
                        {
                            totalTransInWeek += Convert.ToInt16(j.PCTEFF);
                            if (j.RESPONSECODE == "114")
                            {
                                In114w = j.PCTEFF;
                               //totaltrans114w += j.PCTEFF;
                                In114wCount++;
                                In114wRank = j.WEEKLYRANK;

                            }
                            if (j.RESPONSECODE == "911")
                            {
                                In911w = j.PCTEFF;
                               // totaltrans911w += j.PCTEFF;
                               In911wCount++;
                               In911wRank = j.WEEKLYRANK;
                            }
                            if (j.RESPONSECODE == "111")
                            {
                                In111w = j.PCTEFF;
                                //totaltrans111w += j.PCTEFF;
                                In111wCount++;
                                In111wRank = j.WEEKLYRANK;
                            }
                            if (j.RESPONSECODE == "400")
                            {
                                In400w = j.PCTEFF;
                                ///totaltrans400w += j.PCTEFF;
                                In400wCount++;
                                In400wRank = j.WEEKLYRANK;
                            }
                            if (j.RESPONSECODE == "912")
                            {
                                In912w = j.PCTEFF;
                                //totaltrans912w += j.PCTEFF;
                                In912wCount++;
                                In912wRank = j.WEEKLYRANK;
                            }
                            if (j.RESPONSECODE == "102")
                            {
                                In102w = j.PCTEFF;
                                //totaltrans102w += j.PCTEFF;
                                In102wCount++;
                                In102wRank = j.WEEKLYRANK;
                            }
                            if (j.RESPONSECODE == "100")
                            {
                                In100w = j.PCTEFF;
                                //totaltrans100w += j.PCTEFF;
                                In100wCount++;
                                In100wRank = j.WEEKLYRANK;
                            }
                            if (j.RESPONSECODE == "120")
                            {
                                In120w = j.PCTEFF;
                                //totaltrans120w += j.PCTEFF;
                                In120wCount++;
                                In120wRank = j.WEEKLYRANK;
                            }
                            if (j.RESPONSECODE == "118")
                            {
                                In118w = j.PCTEFF;
                                //totaltrans118w += j.PCTEFF;
                                In118wCount++;
                                In118wRank = j.WEEKLYRANK;
                            }

                        }
                        if (In114wCount > 0)
                        {
                            In114w = In114w/1;
                        }
                        else
                        {
                            In114w = 0;
                        }
                        if (In911wCount > 0)
                        {
                            In911w = In911w / 1; 
                        }
                        else
                        {
                            In911w = 0;
                        }
                        if (In111wCount > 0)
                        {
                            In111w = In111w / 1; 
                        }
                        else
                        {
                            In111w = 0;
                        }
                        if (In400wCount > 0)
                        {
                            In400w = In400w / 1; 
                        }
                        else
                        {
                            In400w = 0;
                        }
                        if (In912wCount > 0)
                        {
                            In912w = In912w / 1; 
                        }
                        else
                        {
                            In912w = 0;
                        }
                        if (In102wCount > 0)
                        {
                            In102w = In102w / 1; 
                        }
                        else
                        {
                            In102w = 0;
                        }
                        if (In100wCount > 0)
                        {
                            In100w = In100w / 1; 
                        }
                        else
                        {
                            In100w = 0;
                        }
                        if (In120wCount > 0)
                        {
                            In120w = In120w / 1; 
                        }
                        else
                        {
                            In120w = 0;
                        }
                        if (In118wCount > 0)
                        {
                            In118w = In118w / 1; 
                        }
                        else
                        {
                            In118w = 0;
                        }
                    }
                    else
                    {
                        In114w = 0;

                        In911w = 0;
                        In111w = 0;
                        In400w = 0;
                        In912w = 0;
                        In102w = 0;
                        In100w = 0;
                        In120w = 0;
                        In118w = 0;
                        In114wRank = 0;
                        In911wRank = 0;
                        In111wRank = 0;
                        In400wRank = 0;
                        In912wRank = 0;
                        In102wRank = 0;
                        In100wRank = 0;
                        In120wRank = 0;
                        In118wRank = 0;
                    }


                    if (Out.Count() > 0)
                    {

                        foreach (var o in Out)
                        {
                            //totalTransOut += Convert.ToInt16(o.TRANSACTIONCOUNT);
                            if (o.RESPONSECODE == "114")
                            {
                                Out114 = o.PCTEFF;
                                //totaltransOut114 = Convert.ToInt16(o.TRANSACTIONCOUNT);
                            }
                            if (o.RESPONSECODE == "118")
                            {
                                Out118 = o.PCTEFF;
                            }

                            if (o.RESPONSECODE == "911")
                            {
                                Out911 = o.PCTEFF;
                            }
                            if (o.RESPONSECODE == "111")
                            {
                                Out111 = o.PCTEFF;
                            }
                            if (o.RESPONSECODE == "400")
                            {
                                Out400 = o.PCTEFF;
                            }
                            if (o.RESPONSECODE == "912")
                            {
                                Out912 = o.PCTEFF;
                            }
                            if (o.RESPONSECODE == "102")
                            {
                                Out102 = o.PCTEFF;
                            }
                            if (o.RESPONSECODE == "100")
                            {
                                Out100 = o.PCTEFF;
                            }
                            if (o.RESPONSECODE == "120")
                            {
                                Out120 = o.PCTEFF;
                            }
                            if (o.RESPONSECODE == "121")
                            {
                                Out121 = o.PCTEFF;
                            }
                            if (o.RESPONSECODE == "909")
                            {
                                Out909 = o.PCTEFF;
                            }


                        }
                        //Out114 = (totaltransOut114 / totalTransOut) * 100;
                        //Out911 = (totaltransOut911 / totalTransOut) * 100;
                        //Out111 = (totaltransOut111 / totalTransOut) * 100;
                        //Out400 = (totaltransOut400 / totalTransOut) * 100;
                        //Out912 = (totaltransOut912 / totalTransOut) * 100;
                        //Out102 = (totaltransOut102 / totalTransOut) * 100;
                        //Out100 = (totaltransOut100 / totalTransOut) * 100;
                        //Out120 = (totaltransOut120 / totalTransOut) * 100;
                        //Out118 = (totaltransOut118 / totalTransOut) * 100;
                        //Out121 = (totaltransOut121 / totalTransOut) * 100;
                        //Out909 = (totaltransOut909 / totalTransOut) * 100;
                    }
                    else
                    {
                        Out114 = 0;
                        Out911 = 0;
                        Out111 = 0;
                        Out400 = 0;
                        Out912 = 0;
                        Out102 = 0;
                        Out100 = 0;
                        Out120 = 0;
                        Out118 = 0;
                        Out121 = 0;
                        Out909 = 0;
                    }
                    if (Outw.Count() > 0)
                    {
                        int Out114wCount = 0;
                        int Out911wCount = 0;
                        int Out111wCount = 0;
                        int Out400wCount = 0;
                        int Out912wCount = 0;
                        int Out102wCount = 0;
                        int Out100wCount = 0;
                        int Out120wCount = 0;
                        int Out118wCount = 0;
                        int Out121wCount = 0;
                        int Out909wCount = 0;


                        foreach (var k in Outw)
                        {
                            totalTransOutWeek += Convert.ToInt16(k.PCTEFF);
                            if (k.RESPONSECODE == "114")
                            {
                                Out111w = k.PCTEFF;
                                //totaltransOut114w += k.PCTEFF;
                                Out114wCount++;
                                Out114wRank = k.WEEKLYRANK;
                                //Out114wCount=1;
                            }
                            if (k.RESPONSECODE == "118")
                            {
                                Out118w = k.PCTEFF;
                                //totaltransOut118w += k.PCTEFF;
                                Out118wCount++;
                                Out118wRank = k.WEEKLYRANK;
                            }

                            if (k.RESPONSECODE == "911")
                            {
                                Out911w = k.PCTEFF;
                                //totaltransOut911w += k.PCTEFF;
                                Out911wCount++;
                                Out911wRank = k.WEEKLYRANK;
                            }
                            if (k.RESPONSECODE == "111")
                            {
                                Out111w = k.PCTEFF;
                                // totaltransOut111w += k.PCTEFF;
                                Out111wCount++;
                                Out111wRank = k.WEEKLYRANK;
                            }
                            if (k.RESPONSECODE == "400")
                            {
                                Out400w = k.PCTEFF;
                                //totaltransOut400w += k.PCTEFF;
                                Out400wCount++;
                                Out400wRank = k.WEEKLYRANK;
                            }
                            if (k.RESPONSECODE == "912")
                            {
                                Out912w = k.PCTEFF;
                                //totaltransOut912w += k.PCTEFF;
                                Out912wCount++;
                                Out912wRank = k.WEEKLYRANK;
                            }
                            if (k.RESPONSECODE == "102")
                            {
                                Out102w = k.PCTEFF;
                                //totaltransOut102w += k.PCTEFF;
                                Out102wCount++;
                                Out102wRank = k.WEEKLYRANK;
                            }
                            if (k.RESPONSECODE == "100")
                            {
                                Out100w = k.PCTEFF;
                                // totaltransOut100w += k.PCTEFF;
                                Out100wCount++;
                                Out100wRank = k.WEEKLYRANK;
                            }
                            if (k.RESPONSECODE == "120")
                            {
                                Out120w = k.PCTEFF;
                                //totaltransOut120w += k.PCTEFF;
                                Out120wCount++;
                                Out120wRank = k.WEEKLYRANK;

                            }
                            if (k.RESPONSECODE == "121")
                            {
                                Out121w = k.PCTEFF;
                                //totaltransOut121w += Convert.ToInt16(k.PCTEFF);
                                Out121wCount++;
                                Out121wRank = k.WEEKLYRANK;
                            }
                            if (k.RESPONSECODE == "909")
                            {
                                Out909w = k.PCTEFF;
                                //totaltransOut909w += Convert.ToInt16(k.PCTEFF);
                                Out909wCount++;
                                Out909wRank = k.WEEKLYRANK;
                            }


                        }
                        if (Out114wCount > 0)
                        {
                            Out114w = Out114w/1;
                        }
                        else
                        {
                            Out114w = 0;
                        }
                        if (Out911wCount > 0)
                        {
                            Out911w = Out911w/1;
                        }
                        else
                        {
                            Out911w = 0;
                        }
                        if (Out111wCount > 0)
                        {
                            Out111w = Out111w/1;
                        }
                        else
                        {
                            Out111w = 0;
                        }
                        if (Out400wCount > 0)
                        {
                            Out400w = Out400w/1;
                        }
                        else
                        {
                            Out400w = 0;
                        }
                        if (Out912wCount > 0)
                        {
                            Out912w = Out912w/1;
                        }
                        else
                        {
                            Out912w = 0;
                        }
                        if (Out102wCount > 0)
                        {
                            Out102w = Out102w/1;
                        }
                        else
                        {
                            Out102w = 0;
                        }
                        if (Out100wCount > 0)
                        {
                            Out100w = Out100w/1;
                        }
                        else
                        {
                            Out100w = 0;
                        }
                        if (Out120wCount > 0)
                        {
                            Out120w = Out120w/1;
                            //Out120wRank = k.WEEKLYRANK;
                        }
                        else
                        {
                            Out120w = 0;
                            //Out120wRank = 0;
                        }
                        if (Out118wCount > 0)
                        {
                            Out118w = Out118w/1;
                        }
                        else
                        {
                            Out118w = 0;
                        }

                        if (Out121wCount > 0)
                        {
                            Out121w = Out121w/1;
                        }
                        else
                        {
                            Out121w = 0;
                        }

                        if (Out909wCount > 0)
                        {
                            Out909w = Out909w/1;
                        }
                        else
                        {
                            Out909w = 0;
                        }
                    }
                    else
                    {
                        Out114w = 0;
                        Out911w = 0;
                        Out111w = 0;
                        Out400w = 0;
                        Out912w = 0;
                        Out102w = 0;
                        Out100w = 0;
                        Out120w = 0;
                        Out118w = 0;
                        Out121w = 0;
                        Out909w = 0;
                        Out114wRank = 0;
                        Out911wRank = 0;
                        Out111wRank = 0;
                        Out400wRank = 0;
                        Out912wRank = 0;
                        Out102wRank = 0;
                        Out100wRank = 0;
                        Out120wRank = 0;
                        Out118wRank = 0;
                        Out121wRank = 0;
                        Out909wRank = 0;
                    }

                    /** RANKING **/
                    In114r = ReturnPosition(ReturnRank("114", req.sortCode,dtTo));
                    In911r = ReturnPosition(ReturnRank("911", req.sortCode, dtTo));
                    In111r = ReturnPosition(ReturnRank("111", req.sortCode, dtTo));
                    In400r = ReturnPosition(ReturnRank("400", req.sortCode, dtTo));
                    In912r = ReturnPosition(ReturnRank("912", req.sortCode, dtTo));
                    In102r = ReturnPosition(ReturnRank("102", req.sortCode, dtTo));
                    In100r = ReturnPosition(ReturnRank("100", req.sortCode, dtTo));
                    In120r = ReturnPosition(ReturnRank("120", req.sortCode, dtTo));
                    In118r = ReturnPosition(ReturnRank("118", req.sortCode, dtTo));

                    Out114r = ReturnPosition(ReturnRank2("114", req.sortCode,dtTo));
                    Out911r = ReturnPosition(ReturnRank2("911", req.sortCode, dtTo));
                    Out111r = ReturnPosition(ReturnRank2("111", req.sortCode, dtTo));
                    Out400r = ReturnPosition(ReturnRank2("400", req.sortCode, dtTo));
                    Out912r = ReturnPosition(ReturnRank2("912", req.sortCode, dtTo));
                    Out102r = ReturnPosition(ReturnRank2("102", req.sortCode, dtTo));
                    Out100r = ReturnPosition(ReturnRank2("100", req.sortCode, dtTo));
                    Out120r = ReturnPosition(ReturnRank2("120", req.sortCode, dtTo));
                    Out118r = ReturnPosition(ReturnRank2("118", req.sortCode, dtTo));
                    Out121r = ReturnPosition(ReturnRank2("121", req.sortCode, dtTo));
                    Out909r = ReturnPosition(ReturnRank2("909", req.sortCode, dtTo));
                    /** END RANKING **/
                    decimal In911bbw; decimal In912bbw; decimal Out121bbw; decimal Out909bbw;
                    if (BestBankRatingIn("911", lastweekFrom) > 0)
                    {
                        In911bbw = BestBankRatingIn("911", lastweekFrom);
                    }
                    else
                    {
                        In911bbw = 0;
                    }
                    if (BestBankRatingIn("912", lastweekFrom) > 0)
                    {
                        In912bbw = BestBankRatingIn("912", lastweekFrom);
                    }
                    else 
                    {
                        In912bbw = 0;
                    }
                    if (BestBankRatingIn("121", lastweekFrom) > 0)
                    {
                        Out121bbw = BestBankRatingOut("909", lastweekFrom);
                    }
                    else 
                    {
                        Out121bbw = 0;
                    }
                    if (BestBankRatingIn("909", lastweekFrom) > 0)
                    {
                        Out909bbw = BestBankRatingOut("909", lastweekFrom);
                    }
                    else 
                    {
                        Out909bbw = 0;
                    }


                    //check if the banks position exists
                    int cnt = CheckRankAvailability(dtTo);
                    if(cnt < 1)
                    {
                        //process the ranking
                        using(BIDATABASEEntities1 dbb=new BIDATABASEEntities1())
                        {
                            var det = db.Bank_Description.Where(a => a.STATUS == 1);
                            foreach(var dd in det)
                            {
                                ProcessRanking(dtTo, dd.SORT_CODE, lastweekFrom);
                            }
                        }
                    }
                    else
                    {
                        //get the rank from the db;
                        ProcessRankingUpdate(dtTo, req.sortCode, lastweekFrom);
                    }


                    resp.dateFrom = req.dateTo;
                    resp.dateTo = req.dateTo;
                    resp.responseCode = "00";
                    resp.In114 = Math.Round(In114,2);
                    resp.In118 = Math.Round(In118, 2);
                    
                    resp.In111 = Math.Round(In111, 2);
                    resp.In400 = Math.Round(In400, 2);
                    
                    resp.In102 = Math.Round(In102, 2);
                    resp.In100 = Math.Round(In100, 2);
                    resp.In120 = Math.Round(In120, 2);

                    resp.In114w = Math.Round(In114w, 2);
                    resp.In118w = Math.Round(In118w, 2);
                    resp.In911w = Math.Round(In911w, 2);
                    resp.In111w = Math.Round(In111w, 2);
                    resp.In400w = Math.Round(In400w, 2);
                    resp.In912w = Math.Round(In912w, 2);
                    resp.In102w = Math.Round(In102w, 2);
                    resp.In100w = Math.Round(In100w, 2);
                    resp.In120w = Math.Round(In120w, 2);

                    resp.In114r = In114r;
                    resp.In118r = In118r;
                    resp.In911r = ReturnPosition(WeeklyRankIn("911",dtTo,req.sortCode));
                    resp.In111r = In111r;
                    resp.In400r = In400r;
                    resp.In912r = ReturnPosition(WeeklyRankIn("912", dtTo, req.sortCode));
                    resp.In102r = In102r;
                    resp.In100r = In100r;
                    resp.In120r = In120r;

                    resp.Out114r = Out114r;
                    resp.Out118r = Out118r;
                    resp.Out911r = Out911r;
                    resp.Out111r = Out111r;
                    resp.Out400r = Out400r;
                    resp.Out912r = Out912r;
                    resp.Out102r = Out102r;
                    resp.Out100r = Out100r;
                    resp.Out120r = Out120r;
                    resp.Out121r = ReturnPosition(WeeklyRankOut("121", dtTo, req.sortCode));
                    resp.Out909r = ReturnPosition(WeeklyRankOut("909", dtTo, req.sortCode)); ;

                    resp.In114bb = BestBankRatingIn("114",dtTo);
                    resp.In118bb = BestBankRatingIn("118", dtTo);
                    resp.In911bb = BestBankRatingIn("911", dtTo);
                    resp.In111bb = BestBankRatingIn("111", dtTo);
                    resp.In400bb = BestBankRatingIn("400", dtTo);
                    resp.In912bb = BestBankRatingIn("912", dtTo);
                    resp.In102bb = BestBankRatingIn("102", dtTo);
                    resp.In100bb = BestBankRatingIn("100", dtTo);
                    resp.In120bb = BestBankRatingIn("120", dtTo);

                    resp.Out114bb = BestBankRatingOut("114",dtTo);
                    resp.Out118bb = BestBankRatingOut("118", dtTo);
                    resp.Out911bb = BestBankRatingOut("911", dtTo);
                    resp.Out111bb = BestBankRatingOut("111", dtTo);
                    resp.Out400bb = BestBankRatingOut("400", dtTo);
                    resp.Out912bb = BestBankRatingOut("912", dtTo);
                    resp.Out102bb = BestBankRatingOut("102", dtTo);
                    resp.Out100bb = BestBankRatingOut("100", dtTo);
                    resp.Out120bb = BestBankRatingOut("120", dtTo);
                    resp.Out121bb = BestBankRatingOut("121", dtTo);
                    resp.Out909bb = BestBankRatingOut("909", dtTo);

                    resp.In911bbw= In911bbw;
                    resp.In912bbw = In912bbw;
                    resp.Out121bbw = Out121bbw;
                    resp.Out909bbw = Out909bbw;




                    resp.Out114 = Math.Round(Out114, 2);
                    resp.Out118 = Math.Round(Out118, 2);
                    resp.Out911 = Math.Round(Out911, 2);
                    resp.Out111 = Math.Round(Out111, 2);
                    resp.Out400 = Math.Round(Out400, 2);
                    resp.Out912 = Math.Round(Out912, 2);
                    resp.Out102 = Math.Round(Out102, 2);
                    resp.Out100 = Math.Round(Out100, 2);
                    resp.Out120 = Math.Round(Out120, 2);

                    

                    resp.Out114w = Math.Round(Out114w, 2);
                    resp.Out118w = Math.Round(Out118w, 2);
                    resp.Out911w = Math.Round(Out911w, 2);
                    resp.Out111w = Math.Round(Out111w, 2);
                    resp.Out400w = Math.Round(Out400w, 2);
                    resp.Out912w = Math.Round(Out912w, 2);
                    resp.Out102w = Math.Round(Out102w, 2);
                    resp.Out100w = Math.Round(Out100w, 2);
                    resp.Out120w = Math.Round(Out120w, 2);
                    resp.Out121w = Math.Round(Out121w, 2);
                    resp.Out909w = Math.Round(Out909w, 2);

                    resp.Out114wr = ReturnPosition(Out114wRank);
                    resp.Out118wr = ReturnPosition(Out118wRank);
                    resp.Out911wr = ReturnPosition(Out911wRank);
                    resp.Out111wr = ReturnPosition(Out111wRank);
                    resp.Out400wr = ReturnPosition(Out400wRank);
                    resp.Out912wr = ReturnPosition(Out912wRank);
                    resp.Out102wr = ReturnPosition(Out102wRank);
                    resp.Out100wr = ReturnPosition(Out100wRank);
                    resp.Out120wr = ReturnPosition(Out120wRank);
                    resp.Out121wr = ReturnPosition(Out121wRank);
                    resp.Out909wr = ReturnPosition(Out909wRank);

                    resp.In114wr = ReturnPosition(In114wRank);
                    resp.In118wr = ReturnPosition(In118wRank);
                    resp.In911wr = ReturnPosition(In911wRank);
                    resp.In111wr = ReturnPosition(In111wRank);
                    resp.In400wr = ReturnPosition(In400wRank);
                    resp.In912wr = ReturnPosition(In912wRank);
                    resp.In102wr = ReturnPosition(In102wRank);
                    resp.In100wr = ReturnPosition(In100wRank);
                    resp.In120wr = ReturnPosition(In120wRank);

                    Link l = ReturnLink(dtTo,req.sortCode,lastweekFrom);
                    IntFace ii = ReturnInterface(dtTo, req.sortCode, lastweekFrom);

                    s911 = (In911 / 100) * 10;
                    s912 = (In912 / 100) * 10;
                    s121 = (Out121 / 100) * 10;
                    s909 = (Out909 / 100) * 10;

                    //calculate the link uptime and network int
                    slink = (l.link/ 100) * 60;
                    decimal slink2 = l.link;
                    ///sInt = (ii.intf / 100) * 30;
                    sInt = 0;
                    decimal tq = Convert.ToDecimal(ConfigurationManager.AppSettings["tfreq"]);
                    decimal nq = Convert.ToDecimal(ConfigurationManager.AppSettings["nureq"]);
                    decimal lq = Convert.ToDecimal(ConfigurationManager.AppSettings["lureq"]);
                    decimal aq = Convert.ToDecimal(ConfigurationManager.AppSettings["aggreq"]);
                    

                    string tfreq = "No";
                    string nureq = "No";
                    string lureq = "No";
                    string aggreq = "No";

                    decimal tFailure = ((100 - s911) + (100 - s912) + (100 - s121) + (100 - s909)) / 400 * 100;

                    //if (tq >= (s911+s912+s121+s909))
                    if (tq <= tFailure)
                    {
                        tfreq = "Yes";
                    }
                    if (nq<=slink)
                    {
                        nureq = "Yes";
                    }
                    if (lq<=sInt)
                    {
                        lureq = "Yes";
                    }
                    if(ReturnAggre(req.sortCode, dtTo) >= aq)
                    {
                        aggreq = "Yes";
                    }


                    //check for the bank position
                    
                    resp.tf = Math.Round(tFailure,2);
                    resp.nu = Math.Round(slink2,2);
                    resp.iu = sInt;
                    resp.tfreq = tfreq;
                    resp.nureq = nureq;
                    resp.iureq = lureq;
                    resp.aggreq = aggreq;


                    resp.In911 = WeeklyRatingIn("911",dtTo,req.sortCode);
                    resp.In912 = WeeklyRatingIn("912", dtTo, req.sortCode);
                    resp.Out121 = WeeklyRatingOut("121", dtTo, req.sortCode);
                    resp.Out909 = WeeklyRatingOut("909", dtTo, req.sortCode);



                    resp.link = l.link;
                    resp.linkr = ReturnPosition(l.linkr);
                    resp.linkbb = l.linkbb;
                    resp.linkw = l.linkw;
                    resp.linkwr = ReturnPosition(l.linkwr);
                    resp.linkbbw = l.linkbbw;

                    resp.intf = ii.intf;
                    resp.intfr = ReturnPosition(ii.intfr);
                    resp.intfbb = ii.intfbb;
                    resp.intfw = ii.intfw;
                    resp.intfwr = ReturnPosition(ii.intfwr);
                    resp.intfbbw = ii.intfbbw;
                    resp.aggr = ReturnAggre(req.sortCode, dtTo);
                    resp.weekposition = ReturnPosition(ReturnBankIndustryPosition(ReturnAggre(req.sortCode, dtTo), dtTo));


                    resp.BankName = GetBankName(req.sortCode).ToUpper();
                    resp.WeeklyDate = req.dateTo.AddDays(-13).ToString("MMMM dd, yyyy") + " to " + req.dateTo.AddDays(-7).ToString("MMMM dd, yyyy");
                    resp.headingDate = req.dateTo.AddDays(-6).ToString("MMMM dd, yyyy") + " to " + req.dateTo.ToString("MMMM dd, yyyy");
                    output = JsonConvert.SerializeObject(resp);
                }
            }catch(Exception ex)
            {
                r.responseCode = "99";
                r.responseMessage = ex.Message+" Inner Exception"+ex.InnerException;
                output = JsonConvert.SerializeObject(r);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        public class NumberRank
        {
            public decimal Number { get; set; }
            public int Rank { get; set; }

            public NumberRank(decimal number, int rank)
            {
                Number = number;
                Rank = rank;
            }
        }

        public int ReturnLinkPosition(decimal aggr, DateTime dt)
        {
            int r = 0;

            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var details = db.NetworkReports.Where(a => a.ENDDATE == dt).OrderByDescending(x => x.LinkAvailability).ToList();


                    for (int i = 0; i < details.Count; i++)
                    {
                        if (details[i].LinkAvailability == aggr)
                        {
                            if (aggr == 100)
                            {
                                r = 1;
                            }
                            else
                            {
                                r = i;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return r;
        }


        public int ReturnBankIndustryPosition(decimal aggr,DateTime dt)
        {
            int r = 0;

            try
            {
                using(BIDATABASEEntities1 db=new BIDATABASEEntities1())
                {
                    var details = db.WEEKLY_POSITION.Where(a => a.END_DATE == dt).OrderByDescending(x=>x.RATE).ToList();

                    
                    for (int i = 0; i < details.Count; i++)
                    {
                        if (details[i].RATE==aggr)
                        {
                            if (aggr == 100)
                            {
                                r = 1;
                            }
                            else
                            {
                                r = i;
                            }
                        }
                    }
                    
                }
            }
            catch(Exception ex)
            {

            }
            return r;
        }
        public decimal ReturnAggre(string sortCode,DateTime dt)
        {
            decimal aggr = 0;
            try
            {
                using(BIDATABASEEntities1 db=new BIDATABASEEntities1())
                {
                    var details = db.WEEKLY_POSITION.FirstOrDefault(a => a.BANK == sortCode && a.END_DATE == dt);
                    aggr = Convert.ToDecimal(details.RATE);

                }
            }catch(Exception ex)
            {

            }
            return aggr;
        }
        public int ProcessRanking(DateTime dt,string sortCode,DateTime lastweekFrom)
        {
            int process = 0;
           
            Link l = ReturnLink(dt, sortCode, lastweekFrom);
            IntFace ii = ReturnInterface(dt, sortCode, lastweekFrom);
            decimal In911=WeeklyRatingIn("911", dt, sortCode);
            decimal In912 = WeeklyRatingIn("912", dt, sortCode);
            decimal Out121 = WeeklyRatingOut("121", dt, sortCode);
            decimal Out909 = WeeklyRatingOut("909", dt, sortCode);
            decimal link = l.link;
            decimal intf = ii.intf;

            decimal tf = (((100-In911) + (100-In912) + (100-Out121) + (100-Out909)) / 400) * 40;
            decimal lnk = link / 100 * 60;
            decimal intface = intf / 100 * 30;
            //decimal aggr = tf + lnk + intface;
            decimal aggr = tf + lnk;
            try
            {
                using(BIDATABASEEntities1 db=new BIDATABASEEntities1())
                {
                    WEEKLY_POSITION w = new WEEKLY_POSITION();
                    w.BANK = sortCode;
                    w.END_DATE = dt;
                    w.RATE = aggr;
                    db.WEEKLY_POSITION.Add(w);
                    db.SaveChanges();
                    process = w.ID;
                }
                
            }catch(Exception ex)
            {

            }
            return process;
        }
        public int ProcessRankingUpdate(DateTime dt, string sortCode, DateTime lastweekFrom)
        {
            int process = 0;

            Link l = ReturnLink(dt, sortCode, lastweekFrom);
            IntFace ii = ReturnInterface(dt, sortCode, lastweekFrom);
            decimal In911 = WeeklyRatingIn("911", dt, sortCode);
            decimal In912 = WeeklyRatingIn("912", dt, sortCode);
            decimal Out121 = WeeklyRatingOut("121", dt, sortCode);
            decimal Out909 = WeeklyRatingOut("909", dt, sortCode);
            decimal link = l.link;
            decimal intf = ii.intf;

            decimal tf = (((100 - In911) + (100 - In912) + (100 - Out121) + (100 - Out909)) / 400) * 40;
            decimal lnk = link / 100 * 60;
            decimal intface = intf / 100 * 30;
            //decimal aggr = tf + lnk + intface;
            decimal aggr = tf + lnk;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    WEEKLY_POSITION w = new WEEKLY_POSITION();
                    var details = db.WEEKLY_POSITION.FirstOrDefault(a => a.END_DATE == dt && a.BANK == sortCode);
                    if(details == null)
                    {
                        //if not available add it
                        w.BANK = sortCode;
                        w.END_DATE = dt;
                        w.RATE = aggr;
                        db.WEEKLY_POSITION.Add(w);
                        db.SaveChanges();
                        process = w.ID;
                    }
                    else
                    {
                        //update the weekly  details
                        
                        details.RATE = aggr;
                        db.SaveChanges();
                        process = details.ID;
                    }
                    
                }

            }
            catch (Exception ex)
            {

            }
            return process;
        }

        public int CheckRankAvailability(DateTime dt)
        {
            int count = 0;
            try
            {
                using(BIDATABASEEntities1 db=new BIDATABASEEntities1())
                {
                    count = db.WEEKLY_POSITION.Count(a => a.END_DATE == dt);
                }
            }catch(Exception ex)
            {

            }
            return count;
        }
        public decimal GetBestBankLinkRate(DateTime dt)
        {
            decimal bb = 0;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var d = db.NetworkReports.Where(a => a.ENDDATE == dt).OrderByDescending(x => x.LinkAvailability).FirstOrDefault();
                    //var d = db.LINK_AVAILABILITY.FirstOrDefault(a => a.END_DATE == dt && a.RANK == 1);
                    if (d != null)
                    {
                        bb = Convert.ToDecimal(d.LinkAvailability);
                    }
                    else
                    {
                        bb = 0;
                    }
                }
            }catch(Exception ex)
            {
               bb=0;
            }
            return bb;
        }

        public decimal GetBestBankIntfaceRate(DateTime dt)
        {
            decimal bb = 0;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var d = db.INTERFACE_UPTIME.FirstOrDefault(a => a.END_DATE == dt && a.RANK == 1);
                    if (d != null)
                    {
                        bb = Convert.ToDecimal(d.RATE);
                    }
                    else
                    {
                        bb = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                bb = 0;
            }
            return bb;
        }

        public decimal GetLastWeekLinkRate(string sortCode,DateTime lw)
        {
            decimal r = 0;
            int sCode = Convert.ToInt32(sortCode);
            try
            {
                using(BIDATABASEEntities1 db=new BIDATABASEEntities1())
                {
                    var d = db.NetworkReports.FirstOrDefault(a => a.bankcode == sCode && a.ENDDATE == lw);
                    if (d != null)
                    {
                        r = Convert.ToDecimal(d.LinkAvailability);
                    }
                    else
                    {
                        r = 0;
                    }
                }
            }catch(Exception ex)
            {
                r = 0;
            }
            return r;
        }
        public decimal GetLastWeekIntfaceRate(string sortCode, DateTime lw)
        {
            decimal r = 0;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var d = db.INTERFACE_UPTIME.FirstOrDefault(a => a.BANK_CODE == sortCode && a.END_DATE == lw);
                    if (d != null)
                    {
                        r = Convert.ToDecimal(d.RATE);
                    }
                    else
                    {
                        r = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                r = 0;
            }
            return r;
        }
        public int GetLastWeekLinkRank(string sortCode, DateTime lw)
        {
            int r = 0;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var d = db.LINK_AVAILABILITY.FirstOrDefault(a => a.BANK_CODE == sortCode && a.END_DATE == lw);
                    if(d != null)
                    {
                        r = Convert.ToInt16(d.RANK);
                    }
                    else
                    {
                        r = 0;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                r = 0;
            }
            return r;
        }
        public int GetLastWeekIntfaceRank(string sortCode, DateTime lw)
        {
            int r = 0;
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var d = db.INTERFACE_UPTIME.FirstOrDefault(a => a.BANK_CODE == sortCode && a.END_DATE == lw);
                    if (d != null)
                    {
                        r = Convert.ToInt16(d.RANK);
                    }
                    else
                    {
                        r = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                r = 0;
            }
            return r;
        }

        public Link ReturnLink(DateTime dt,string sortCode,DateTime lw)
        {
            decimal link = 0;
            Link l = new Link();
            int sCode = Convert.ToInt32(sortCode);
            try
            {
                
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var d = db.NetworkReports.FirstOrDefault(a => a.bankcode == sCode && a.ENDDATE == dt);
                    if (d != null)
                    {
                        l.link = Convert.ToDecimal(d.LinkAvailability);
                        l.linkr = ReturnLinkPosition(Convert.ToDecimal(d.LinkAvailability),dt);
                        l.linkbb = GetBestBankLinkRate(dt);
                        l.linkw = GetLastWeekLinkRate(sortCode, lw);
                        l.linkwr = ReturnLinkPosition(GetLastWeekLinkRate(sortCode, lw), lw);
                            //GetLastWeekLinkRank(sortCode, lw);
                        l.linkbbw = GetBestBankLinkRate(lw);
                    }
                    else
                    {
                        l.link =0;
                        l.linkr = 0;
                        //l.linkbb = 0;
                        l.linkbb = GetBestBankLinkRate(dt);
                        l.linkw = 0;
                        l.linkwr = 0;
                        //l.linkbbw = 0;
                        l.linkbbw = GetBestBankLinkRate(lw);
                    }

                }
            }catch(Exception ex)
            {

            }
            return l;
        }
        public IntFace ReturnInterface(DateTime dt, string sortCode, DateTime lw)
        {
            decimal intf = 0;
            IntFace l = new IntFace();
            try
            {
                using (BIDATABASEEntities1 db = new BIDATABASEEntities1())
                {
                    var d = db.INTERFACE_UPTIME.FirstOrDefault(a => a.BANK_CODE == sortCode && a.END_DATE == dt);
                    if (d != null)
                    {
                        l.intf = Convert.ToDecimal(d.RATE);
                        l.intfr = Convert.ToInt16(d.RANK);
                        l.intfbb = GetBestBankIntfaceRate(dt);
                        l.intfw = GetLastWeekIntfaceRate(sortCode, lw);
                        l.intfwr = GetLastWeekIntfaceRank(sortCode, lw);
                        l.intfbbw = GetBestBankIntfaceRate(lw);
                    }
                    else
                    {
                        l.intf = 0;
                        l.intfr =0;
                        l.intfbb = 0;
                        l.intfw = 0;
                        l.intfwr = 0;
                        l.intfbbw = 0;
                    }


                }
            }catch(Exception ex)
            {

            }
            return l;
        }

        [HttpPost]
        [Route("api/Login")]
        public async Task<HttpResponseMessage> Login(LoginRequest b)
        {
            LoginResp resp = new LoginResp();

            var output = "";

            try
            {

                var rs = JsonConvert.SerializeObject(b);

                var url = ConfigurationManager.AppSettings["LoginURL"];
                var content = new StringContent(rs, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                HttpResponseMessage result = null;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new
              RemoteCertificateValidationCallback
              (
                 delegate { return true; }
              );
                result = client.PostAsync(url, content).Result;
                var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
                var rr = JsonConvert.DeserializeObject<LoginResponse>(r);



                if (rr.status == "00")
                {
                    resp.status = rr.status;
                    resp.message = rr.message;
                    resp.firstname = rr.data.firstname;
                    resp.bankUser = rr.data.bankUser;
                    resp.bankAdmin = rr.data.bankAdmin;
                    resp.ipslAdmin = rr.data.ipslAdmin;
                    resp.lastname = rr.data.lastname;
                    resp.email = rr.data.email;
                    resp.bankCode = rr.data.bankCode;
                    resp.firsttimeuser = rr.data.firsttime;
                }
                else
                {
                    var ss = JsonConvert.DeserializeObject<FailedLogin>(r);
                    
                    if (ss.status == "54")
                    {
                        resp.status = ss.status;
                        resp.message = ss.message;
                        resp.email = b.email;
                    }
                    else
                    {
                        resp.status = ss.status;
                        resp.message = ss.message;
                    }
                    


                }
                
                
            }
            catch (Exception ex)
            {
                resp.status = "99";
                resp.message = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        [Route("api/UpdateUser")]
        public async Task<HttpResponseMessage> UpdateUser(Userss b)
        {
            Response resp = new Response();
            AddUserRequest a = new AddUserRequest();
            var output = "";
            int userID = 0;
            try
            {
               // a.bankAdmin = 1;
                //a.bankUser = 1;
                if (b.roleID == 1)
                {
                    a.bankAdmin = 0;
                    a.bankUser = 1;
                    a.ipslAdmin = 0;
                    a.bankCode = b.bankCode;
                }
                else if (b.roleID == 2)
                {
                    a.bankAdmin = 1;
                    a.bankUser = 0;
                    a.ipslAdmin = 0;
                    a.bankCode = b.bankCode;
                }
                else if (b.roleID == 3)
                {
                    a.bankAdmin = 0;
                    a.bankUser = 0;
                    a.ipslAdmin = 1;
                    a.bankCode = "001";
                }
                else if (b.roleID == 4)
                {
                    a.bankAdmin = 0;
                    a.bankUser = 0;
                    a.ipslAdmin = 0;
                    a.bankCode = "001";
                }





                a.email = b.email;
                a.firstname = b.firstname;
                a.lastname = b.lastname;
                var rs = JsonConvert.SerializeObject(a);

                var url = ConfigurationManager.AppSettings["UpdateUsersURL"];
                var content = new StringContent(rs, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                HttpResponseMessage result = null;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new
              RemoteCertificateValidationCallback
              (
                 delegate { return true; }
              );
                result = client.PostAsync(url, content).Result;
                var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
                var rr = JsonConvert.DeserializeObject<AddUserResponse>(r);

                if (rr.status == "00")
                {
                    resp.responseCode = "00";
                    resp.responseMessage = rr.message;
                }
                else
                {
                    resp.responseCode = rr.status;
                    resp.responseMessage = rr.message;
                }
            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        [HttpPost]
        [Route("api/UpdateUser2")]
        public async Task<HttpResponseMessage> UpdateUser2(Userss b)
        {
            Response resp = new Response();
            AddUserRequest a = new AddUserRequest();
            var output = "";
            int userID = 0;
            try
            {
                using (TransactionMonitoringEntities db = new TransactionMonitoringEntities())
                {
                    var details = db.Users.FirstOrDefault(aa => aa.email == b.email);
                    details.firstname = b.firstname;
                    details.lastname = b.lastname;
                    details.bankCode = b.bankCode;
                    details.email = b.email;
                    details.role = b.roleID;
                    db.SaveChanges();
                    userID = details.userID;
                }



                if (userID > 0)
                {
                    resp.responseCode = "00";
                    resp.responseMessage = "User Successfully updated";
                }
                else
                {
                    resp.responseCode = "96";
                    resp.responseMessage = "Error updating user";
                }
            }
            catch (Exception ex)
            {
                resp.responseCode = "99";
                resp.responseMessage = ex.Message;
            }
            output = JsonConvert.SerializeObject(resp);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        public string GetDashboardAnalytics2()
        {
            var url = ConfigurationManager.AppSettings["TransAnalyticsURL"];

            var client = new HttpClient();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new
          RemoteCertificateValidationCallback
          (
             delegate { return true; }
          );
            var result = client.GetAsync(url).Result;
            //var result = "{\"totalVolume\": 149,\"totalFailure\": 4,\"failureRate\": 2.6839939939,\"responseCode\": {\"102\": 2.68,\"000\": 97.32},\"inwardSummary\": [{\"bankCode\": \"BANK63\",\"bankName\": \"No Name\",\"totalVolume\": 2,\"totalFailure\": 2,\"inFailed\": 21.908},{\"bankCode\": \"BANK11\",\"bankName\": \"No Name\",\"totalVolume\": 7,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK12\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK01\",\"bankName\": \"No Name\",\"totalVolume\": 14,\"totalFailure\": 0,\"inFailed\": 3},{\"bankCode\": \"BANK02\",\"bankName\": \"No Name\",\"totalVolume\": 4,\"totalFailure\": 0,\"inFailed\": 4},{\"bankCode\": \"BANK68\",\"bankName\": \"No Name\",\"totalVolume\": 12,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK03\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK07\",\"bankName\": \"No Name\",\"totalVolume\": 3,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK70\",\"bankName\": \"No Name\",\"totalVolume\": 2,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK72\",\"bankName\": \"No Name\",\"totalVolume\": 2,\"totalFailure\": 0,\"inFailed\": 0}],\"outwardSummary\": [{\"bankCode\": \"BANK63\",\"bankName\": \"No Name\",\"totalVolume\": 3,\"totalFailure\": 0,\"outFailed\": 45},{\"bankCode\": \"BANK31\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK00\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK11\",\"bankName\": \"No Name\",\"totalVolume\": 17,\"totalFailure\": 2,\"outFailed\": 11.76},{\"bankCode\": \"BANK01\",\"bankName\": \"No Name\",\"totalVolume\": 4,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK57\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK68\",\"bankName\": \"No Name\",\"totalVolume\": 15,\"totalFailure\": 1,\"outFailed\": 6.67},{\"bankCode\": \"BANK03\",\"bankName\": \"No Name\",\"totalVolume\": 4,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK07\",\"bankName\": \"No Name\",\"totalVolume\": 3,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"4040059900\",\"bankName\": \"No Name\",\"totalVolume\": 97,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK70\",\"bankName\": \"No Name\",\"totalVolume\": 2,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK72\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 1,\"outFailed\": 100.0002928}]}";

            var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
            //var result = "{\"totalVolume\":116,\"totalFailure\":12,\"failureRate\":10.34,\"responseCode\":{\"000\":89.66,\"908\":1.72,\"114\":5.17,\"400\":3.45},\"inwardSummary\":[{\"bankCode\":\"BANK74\",\"bankName\":\"No Name\",\"totalVolume\":1,\"totalFailure\":0,\"inFailed\":0.00},{\"bankCode\":\"BANK11\",\"bankName\":\"No Name\",\"totalVolume\":9,\"totalFailure\":1,\"inFailed\":11.11},{\"bankCode\":\"BANK66\",\"bankName\":\"No Name\",\"totalVolume\":3,\"totalFailure\":0,\"inFailed\":0.00},{\"bankCode\":\"BANK01\",\"bankName\":\"No Name\",\"totalVolume\":8,\"totalFailure\":0,\"inFailed\":0.00},{\"bankCode\":\"BANK02\",\"bankName\":\"No Name\",\"totalVolume\":4,\"totalFailure\":0,\"inFailed\":0.00},{\"bankCode\":\"BANK68\",\"bankName\":\"No Name\",\"totalVolume\":5,\"totalFailure\":1,\"inFailed\":20.00},{\"bankCode\":\"BANK03\",\"bankName\":\"No Name\",\"totalVolume\":4,\"totalFailure\":2,\"inFailed\":50.00},{\"bankCode\":\"BANK07\",\"bankName\":\"No Name\",\"totalVolume\":4,\"totalFailure\":0,\"inFailed\":0.00},{\"bankCode\":\"BANK70\",\"bankName\":\"No Name\",\"totalVolume\":1,\"totalFailure\":0,\"inFailed\":0.00}],\"outwardSummary\":[{\"bankCode\":\"BANK11\",\"bankName\":\"No Name\",\"totalVolume\":4,\"totalFailure\":0,\"outFailed\":0.00},{\"bankCode\":\"BANK66\",\"bankName\":\"No Name\",\"totalVolume\":2,\"totalFailure\":0,\"outFailed\":0.00},{\"bankCode\":\"BANK01\",\"bankName\":\"No Name\",\"totalVolume\":2,\"totalFailure\":0,\"outFailed\":0.00},{\"bankCode\":\"BANK68\",\"bankName\":\"No Name\",\"totalVolume\":18,\"totalFailure\":3,\"outFailed\":16.67},{\"bankCode\":\"BANK03\",\"bankName\":\"No Name\",\"totalVolume\":3,\"totalFailure\":0,\"outFailed\":0.00},{\"bankCode\":\"BANK07\",\"bankName\":\"No Name\",\"totalVolume\":7,\"totalFailure\":1,\"outFailed\":14.29},{\"bankCode\":\"4040059900\",\"bankName\":\"No Name\",\"totalVolume\":79,\"totalFailure\":8,\"outFailed\":10.13},{\"bankCode\":\"BANK70\",\"bankName\":\"No Name\",\"totalVolume\":1,\"totalFailure\":0,\"outFailed\":0.00}]}";
            //var r = result.Replace("\\", "");



            return r;
        }

        public string GetDashboardAnalytics(string bankCode)
        {
            var url = ConfigurationManager.AppSettings["TransAnalyticsURL"];
            GetDashboardAnalyticsRequest req = new GetDashboardAnalyticsRequest();
            req.bankCode = bankCode;
            req.startDate = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 08:00:00";
            req.endDate = DateTime.Now.ToString();
            //req.startDate = startDate;
            //req.endDate = enddate;

            var rs = JsonConvert.SerializeObject(req);

            
            var content = new StringContent(rs, Encoding.UTF8, "application/json");
            var client = new HttpClient();

            HttpResponseMessage result = null;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new
          RemoteCertificateValidationCallback
          (
             delegate { return true; }
          );
            result = client.PostAsync(url, content).Result;
            var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
            //var rr = JsonConvert.DeserializeObject<LoginResponse>(r);

            //  var client = new HttpClient();

            //  ServicePointManager.Expect100Continue = true;
            //  ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //  ServicePointManager.ServerCertificateValidationCallback = new
            //RemoteCertificateValidationCallback
            //(
            //   delegate { return true; }
            //);
            //  var result = client.GetAsync(url).Result;
            //var result = "{\"totalVolume\": 149,\"totalFailure\": 4,\"failureRate\": 2.6839939939,\"responseCode\": {\"102\": 2.68,\"000\": 97.32},\"inwardSummary\": [{\"bankCode\": \"BANK63\",\"bankName\": \"No Name\",\"totalVolume\": 2,\"totalFailure\": 2,\"inFailed\": 21.908},{\"bankCode\": \"BANK11\",\"bankName\": \"No Name\",\"totalVolume\": 7,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK12\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK01\",\"bankName\": \"No Name\",\"totalVolume\": 14,\"totalFailure\": 0,\"inFailed\": 3},{\"bankCode\": \"BANK02\",\"bankName\": \"No Name\",\"totalVolume\": 4,\"totalFailure\": 0,\"inFailed\": 4},{\"bankCode\": \"BANK68\",\"bankName\": \"No Name\",\"totalVolume\": 12,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK03\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK07\",\"bankName\": \"No Name\",\"totalVolume\": 3,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK70\",\"bankName\": \"No Name\",\"totalVolume\": 2,\"totalFailure\": 0,\"inFailed\": 0},{\"bankCode\": \"BANK72\",\"bankName\": \"No Name\",\"totalVolume\": 2,\"totalFailure\": 0,\"inFailed\": 0}],\"outwardSummary\": [{\"bankCode\": \"BANK63\",\"bankName\": \"No Name\",\"totalVolume\": 3,\"totalFailure\": 0,\"outFailed\": 45},{\"bankCode\": \"BANK31\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK00\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK11\",\"bankName\": \"No Name\",\"totalVolume\": 17,\"totalFailure\": 2,\"outFailed\": 11.76},{\"bankCode\": \"BANK01\",\"bankName\": \"No Name\",\"totalVolume\": 4,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK57\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK68\",\"bankName\": \"No Name\",\"totalVolume\": 15,\"totalFailure\": 1,\"outFailed\": 6.67},{\"bankCode\": \"BANK03\",\"bankName\": \"No Name\",\"totalVolume\": 4,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK07\",\"bankName\": \"No Name\",\"totalVolume\": 3,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"4040059900\",\"bankName\": \"No Name\",\"totalVolume\": 97,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK70\",\"bankName\": \"No Name\",\"totalVolume\": 2,\"totalFailure\": 0,\"outFailed\": 0},{\"bankCode\": \"BANK72\",\"bankName\": \"No Name\",\"totalVolume\": 1,\"totalFailure\": 1,\"outFailed\": 100.0002928}]}";

           // var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
            //var result = "{\"totalVolume\":116,\"totalFailure\":12,\"failureRate\":10.34,\"responseCode\":{\"000\":89.66,\"908\":1.72,\"114\":5.17,\"400\":3.45},\"inwardSummary\":[{\"bankCode\":\"BANK74\",\"bankName\":\"No Name\",\"totalVolume\":1,\"totalFailure\":0,\"inFailed\":0.00},{\"bankCode\":\"BANK11\",\"bankName\":\"No Name\",\"totalVolume\":9,\"totalFailure\":1,\"inFailed\":11.11},{\"bankCode\":\"BANK66\",\"bankName\":\"No Name\",\"totalVolume\":3,\"totalFailure\":0,\"inFailed\":0.00},{\"bankCode\":\"BANK01\",\"bankName\":\"No Name\",\"totalVolume\":8,\"totalFailure\":0,\"inFailed\":0.00},{\"bankCode\":\"BANK02\",\"bankName\":\"No Name\",\"totalVolume\":4,\"totalFailure\":0,\"inFailed\":0.00},{\"bankCode\":\"BANK68\",\"bankName\":\"No Name\",\"totalVolume\":5,\"totalFailure\":1,\"inFailed\":20.00},{\"bankCode\":\"BANK03\",\"bankName\":\"No Name\",\"totalVolume\":4,\"totalFailure\":2,\"inFailed\":50.00},{\"bankCode\":\"BANK07\",\"bankName\":\"No Name\",\"totalVolume\":4,\"totalFailure\":0,\"inFailed\":0.00},{\"bankCode\":\"BANK70\",\"bankName\":\"No Name\",\"totalVolume\":1,\"totalFailure\":0,\"inFailed\":0.00}],\"outwardSummary\":[{\"bankCode\":\"BANK11\",\"bankName\":\"No Name\",\"totalVolume\":4,\"totalFailure\":0,\"outFailed\":0.00},{\"bankCode\":\"BANK66\",\"bankName\":\"No Name\",\"totalVolume\":2,\"totalFailure\":0,\"outFailed\":0.00},{\"bankCode\":\"BANK01\",\"bankName\":\"No Name\",\"totalVolume\":2,\"totalFailure\":0,\"outFailed\":0.00},{\"bankCode\":\"BANK68\",\"bankName\":\"No Name\",\"totalVolume\":18,\"totalFailure\":3,\"outFailed\":16.67},{\"bankCode\":\"BANK03\",\"bankName\":\"No Name\",\"totalVolume\":3,\"totalFailure\":0,\"outFailed\":0.00},{\"bankCode\":\"BANK07\",\"bankName\":\"No Name\",\"totalVolume\":7,\"totalFailure\":1,\"outFailed\":14.29},{\"bankCode\":\"4040059900\",\"bankName\":\"No Name\",\"totalVolume\":79,\"totalFailure\":8,\"outFailed\":10.13},{\"bankCode\":\"BANK70\",\"bankName\":\"No Name\",\"totalVolume\":1,\"totalFailure\":0,\"outFailed\":0.00}]}";
            //var r = result.Replace("\\", "");



            return r;
        }
        public string GetIndustryDashboardAnalytics()
        {
            var url = ConfigurationManager.AppSettings["IndustryTransAnalyticsURL"];
                      
            var client = new HttpClient();

            HttpResponseMessage result = null;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new
          RemoteCertificateValidationCallback
          (
             delegate { return true; }
          );
            result = client.GetAsync(url).Result;
            var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
          
            


            return r;
        }

        public float ReturnOutFailed(List<Outwardsummary> Bdata, string bankCode)
        {
            float r = 0;
            foreach (var i in Bdata)
            {
                if (i.bankCode == bankCode)
                {
                    r = i.outFailed;
                }
                else
                {
                    r = 0;
                }
            }
            return r;
        }

        public float ReturnIn(List<Inwardsummary>InData,string BankCode)
        {
            float r = 0;
            foreach(var i in InData)
            {
                if (BankCode == i.bankCode)
                {
                    r = i.inFailed;
                }
            }
            return r;
        }
        public int ReturnInVolume(List<Inwardsummary> InData, string BankCode)
        {
            int r = 0;
            foreach (var i in InData)
            {
                if (BankCode == i.bankCode)
                {
                    r = i.totalVolume;
                }
            }
            return r;
        }
        public float ReturnOut(List<Outwardsummary> outData, string BankCode)
        {
            float r = 0;
            foreach (var i in outData)
            {
                
                if (BankCode == i.bankCode)
                //if(SortCode==i)
                {
                    r = i.outFailed;
                }
            }
            return r;
        }
        public int ReturnOutVolume(List<Outwardsummary> outData, string BankCode)
        {
            int r = 0;
            foreach (var i in outData)
            {
                
                if (BankCode == i.bankCode)
                {
                    r = i.totalVolume;
                }
            }
            return r;
        }
        public int InsertAnalytics(string BankCode, float oFailed, float iFailed, int oVolume, int iVolume)
        {
            int ID = 0;
            try
            {
                using(TransactionMonitoringEntities db=new TransactionMonitoringEntities())
                {
                    MyAnalytic a = new MyAnalytic();
                    a.AnalyticsDate = DateTime.Now;
                    a.FailureRateIn = iFailed;
                    a.FailureRateOut = oFailed;
                    a.VolumeIn = iVolume;
                    a.VolumeOut = oVolume;
                    a.BankCode = BankCode;
                    db.MyAnalytics.Add(a);
                    db.SaveChanges();
                    ID = a.AnalyticsID;
                }
            }catch(Exception ex)
            {

            }
            return ID;
        }
        [HttpPost]
        [Route("api/PageLoadBankData")]
        public async Task<HttpResponseMessage> PageLoadBankData()
        {
            List<BankData> BData = new List<BankData>();
            //var r = req.MyData.Replace("\\", "");

            //var rr = JsonConvert.DeserializeObject<NewTransAnalytics>(r);
            float oFailed = 0;
            float iFailed = 0;
            int oVolume = 0;
            int iVolume = 0;
            //var bbanks = MyBanks();
            var bbanks = MyBanksFromAPI();
            foreach (var bb in bbanks)
            {
                oFailed = 0;
                iFailed = 0;
                oVolume = 0;
                iVolume = 0;


                BData.Add(new BankData { BankName = bb.BankName, InFailed = Math.Round(iFailed, 2), OutFailed = Convert.ToSingle(Math.Round(oFailed, 2)), BankCode = bb.BankCode, inVolume = iVolume, outVolume = oVolume, IndInVolume = 0, IndOutVolume = 0 }); ;

            }

            string output = JsonConvert.SerializeObject(BData);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        [HttpPost]
        [Route("api/PageLoadBankData2")]
        public async Task<HttpResponseMessage> PageLoadBankData2()
        {
            List<BankData> BData = new List<BankData>();
            //var r = req.MyData.Replace("\\", "");

            //var rr = JsonConvert.DeserializeObject<NewTransAnalytics>(r);
            float oFailed = 0;
            float iFailed = 0;
            int oVolume = 0;
            int iVolume = 0;
            //var bbanks = MyBanks();
            var bbanks = MyBanksFromAPI();
            foreach (var bb in bbanks)
            {
                oFailed = 0;
                iFailed = 0;
                oVolume = 0;
                iVolume = 0;


                BData.Add(new BankData { BankName = bb.BankName, InFailed = Math.Round(iFailed, 2), OutFailed = Convert.ToSingle(Math.Round(oFailed, 2)), BankCode = bb.BankCode, inVolume = iVolume, outVolume = oVolume, IndInVolume = 0, IndOutVolume = 0 }); ;

            }

            string output = JsonConvert.SerializeObject(BData);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        [HttpPost]
        [Route("api/NewBankData")]
        public async Task<HttpResponseMessage> NewBanksData(NewBankDataRequest req)
        {
            List<BankData> BData = new List<BankData>();
            var r = req.MyData.Replace("\\", "");
           
            var rr = JsonConvert.DeserializeObject<NewTransAnalytics>(r);
            float oFailed = 0;
            float iFailed = 0;
            int oVolume = 0;
            int iVolume = 0;
            //var bbanks = MyBanks();
            var bbanks = MyBanksFromAPI();

            foreach (var bb in bbanks)
            {

                oFailed = ReturnOut(rr.outwardSummary.ToList(), bb.BankCode);
                iFailed = ReturnIn(rr.inwardSummary.ToList(), bb.BankCode);
                oVolume = ReturnOutVolume(rr.outwardSummary.ToList(), bb.BankCode);
                iVolume = ReturnInVolume(rr.inwardSummary.ToList(), bb.BankCode);

                //oFailed = ReturnOut(rr.outwardSummary.ToList(), bb.BankCode,bb.SortCode);
                //iFailed = ReturnIn(rr.inwardSummary.ToList(), bb.BankCode,bb.SortCode);
                //oVolume = ReturnOutVolume(rr.outwardSummary.ToList(), bb.BankCode,bb.SortCode);
                //iVolume = ReturnInVolume(rr.inwardSummary.ToList(), bb.BankCode,bb.SortCode);


                // BData.Add(new BankData { BankName = i.bankCode, InFailed = Math.Round(i.inFailed, 2), OutFailed = Convert.ToSingle(Math.Round(oFailed, 2)), BankCode = i.bankCode });
                BData.Add(new BankData { BankName = bb.BankName, InFailed = Math.Round(iFailed, 2), OutFailed = Convert.ToSingle(Math.Round(oFailed, 2)), BankCode = bb.BankCode,inVolume=iVolume,outVolume=oVolume,  totalFailure =rr.totalFailure,totalVolume=rr.totalVolume,failureRate=rr.failureRate, IndOutRate = rr.industryOutward.outFailed, IndInRate = rr.industryInward.inFailed,IndInVolume=rr.industryInward.totalVolume,IndOutVolume=rr.industryOutward.totalVolume });

            }

            string output = JsonConvert.SerializeObject(BData);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        [HttpPost]
        [Route("api/BankData")]
        public async Task<HttpResponseMessage> BanksData()
        {
            List<BankData> BData = new List<BankData>();
            //BData.Add(new BankData { BankName = "KCB", InFailed = 3.71, OutFailed = 3.53,BankCode= "4040059900" });
            //BData.Add(new BankData { BankName = "STANCHART", InFailed = 3.5, OutFailed = 3.05, BankCode = "BANK57" });
            //BData.Add(new BankData { BankName = "BARCLAYS", InFailed = 3.42, OutFailed = 3.4, BankCode = "BANK68" });
            //BData.Add(new BankData { BankName = "NCBA", InFailed = 0.0, OutFailed = 2.94, BankCode = "BANK57" });
            //BData.Add(new BankData { BankName = "PRIME BANK", InFailed = 0.0, OutFailed = 0.57, BankCode = "BANK68" });
            //BData.Add(new BankData { BankName = "COOP BANK", InFailed = 3.3, OutFailed = 3.24, BankCode = "BANK57" });
            //BData.Add(new BankData { BankName = "NBK", InFailed =12.99, OutFailed = 2.16, BankCode = "4040059900" });
            //BData.Add(new BankData { BankName = "CITI BANK", InFailed = 0.0, OutFailed = 0.0, BankCode = "BANK57" });
            //BData.Add(new BankData { BankName = "MEB", InFailed = 3.42, OutFailed = 3.44, BankCode = "BANK68" });
            //BData.Add(new BankData { BankName = "BOA", InFailed = 3.42, OutFailed = 3.44, BankCode = "4040059900" });
            //BData.Add(new BankData { BankName = "CREDIT BANK", InFailed = 3.42, OutFailed = 18.5, BankCode = "BANK57" });
            //BData.Add(new BankData { BankName = "STANBIC", InFailed = 6.42, OutFailed = 0.44, BankCode = "BANK68" });
            //BData.Add(new BankData { BankName = "PARAMOUNT", InFailed = 5.42, OutFailed = 3.44, BankCode = "BANK57" });
            //BData.Add(new BankData { BankName = "JAMII BORA", InFailed = 3.42, OutFailed = 7.44, BankCode = "4040059900" });
            //BData.Add(new BankData { BankName = "GUARDIAN BANK", InFailed = 31.62, OutFailed = 3.44, BankCode = "BANK68" });
            //BData.Add(new BankData { BankName = "GT BANK", InFailed = 12.42, OutFailed = 2.44, BankCode = "BANK57" });
            //BData.Add(new BankData { BankName = "I&M BANK", InFailed = 2.42, OutFailed = 6.44, BankCode = "4040059900" });
            //BData.Add(new BankData { BankName = "DTB", InFailed = 3.42, OutFailed = 3.44, BankCode = "BANK68" });
            //BData.Add(new BankData { BankName = "EQUITY BANK", InFailed = 3.42, OutFailed = 0.44, BankCode = "BANK57" });
            //BData.Add(new BankData { BankName = "FAMILY BANK", InFailed = 3.42, OutFailed = 0.44, BankCode = "4040059900" });
            //BData.Add(new BankData { BankName = "GAB", InFailed = 7.42, OutFailed = 5.44, BankCode = "BANK68" });
            //BData.Add(new BankData { BankName = "FCB", InFailed = 0.42, OutFailed = 2.44, BankCode = "BANK57" });
            //BData.Add(new BankData { BankName = "KWFT", InFailed = 2.42, OutFailed = 1.44, BankCode = "4040059900" });
            //BData.Add(new BankData { BankName = "SIDIAN", InFailed = 1.42, OutFailed = 8.44, BankCode = "BANK57" });
            //BData.Add(new BankData { BankName = "UBA", InFailed = 1.42, OutFailed = 3.44, BankCode = "BANK68" });
      
            var rr = JsonConvert.DeserializeObject<NewTransAnalytics>(GetDashboardAnalytics2());
            float oFailed = 0;
            float iFailed = 0;
            int oVolume = 0;
            int iVolume = 0;
            var bbanks = MyBanks();
            double IndInRate=rr.industryInward.inFailed;
            double IndOutRate = rr.industryOutward.outFailed;
            foreach (var bb in bbanks)
            {
                oFailed = ReturnOut(rr.outwardSummary.ToList(), bb.BankCode);
                iFailed = ReturnIn(rr.inwardSummary.ToList(), bb.BankCode);
                oVolume = ReturnOutVolume(rr.outwardSummary.ToList(), bb.BankCode);
                iVolume= ReturnInVolume(rr.inwardSummary.ToList(), bb.BankCode);

                //insert into analytics database
                //InsertAnalytics(bb.BankCode, oFailed, iFailed, oVolume, iVolume);
                //foreach (var i in rr.inwardSummary)
                //{
                //    if (bb.BankCode == i.bankCode)
                //    {
                //        iFailed = i.inFailed;
                //    }
                //}
                //BankData BData = new BankData();
                // oFailed = ReturnOutFailed(rr.outwardSummary.ToList(), i.bankCode);
                //foreach (var b in rr.outwardSummary)
                //{
                //    if (bb.BankCode == b.bankCode)
                //    {
                //        oFailed = b.outFailed;
                //    }
                //}

                // BData.Add(new BankData { BankName = i.bankCode, InFailed = Math.Round(i.inFailed, 2), OutFailed = Convert.ToSingle(Math.Round(oFailed, 2)), BankCode = i.bankCode });
                BData.Add(new BankData { BankName =bb.BankName, InFailed = Math.Round(iFailed, 2), OutFailed = Convert.ToSingle(Math.Round(oFailed, 2)), BankCode = bb.BankCode, IndOutRate= rr.industryOutward.outFailed,IndInRate=rr.industryInward.inFailed });

            }

            string output = JsonConvert.SerializeObject(BData);
            return new HttpResponseMessage()
            {
                Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
            };
        }
        public class MyBank{
            public string BankName { get; set; }
            public string BankCode { get; set; }
            public string SortCode { get; set; }
        }

        public List<MyBank> MyBanksFromAPI()
        {
            List<MyBank> BData = new List<MyBank>();
            var rr = JsonConvert.DeserializeObject<BankListDetails>(GetListOfBanks());
            foreach (var i in rr.data)
            {
                
                BData.Add(new MyBank { BankName = i.name, BankCode = i.code,SortCode=i.sortCode });
            }

                return BData;
        }

        public List<MyBank> MyBanks()
        {
            List<MyBank> BData = new List<MyBank>();
            BData.Add(new MyBank { BankName = "KCB", BankCode = "BANK51" });
            BData.Add(new MyBank { BankName = "STANCHART", BankCode = "BANK02" });
            BData.Add(new MyBank { BankName = "ABSA BANK KENYA PLC",  BankCode = "BANK03" });
            BData.Add(new MyBank { BankName = "NCBA",  BankCode = "BANK07" });
            BData.Add(new MyBank { BankName = "PRIME BANK",  BankCode = "BANK10" });
            BData.Add(new MyBank { BankName = "COOP BANK", BankCode = "BANK11" });
            BData.Add(new MyBank { BankName = "NBK",  BankCode = "BANK12" });
            BData.Add(new MyBank { BankName = "CITI BANK",  BankCode = "BANK16" });
            BData.Add(new MyBank { BankName = "MEB",  BankCode = "BANK18" });
            BData.Add(new MyBank { BankName = "BOA", BankCode = "BANK19" });
            BData.Add(new MyBank { BankName = "CREDIT BANK",  BankCode = "BANK25" });
            BData.Add(new MyBank { BankName = "STANBIC",  BankCode = "BANK31" });
            BData.Add(new MyBank { BankName = "PARAMOUNT",  BankCode = "BANK50" });
            BData.Add(new MyBank { BankName = "JAMII BORA",  BankCode = "BANK51" });
            BData.Add(new MyBank { BankName = "GUARDIAN BANK",  BankCode = "BANK55" });
            BData.Add(new MyBank { BankName = "GT BANK",  BankCode = "BANK53" });
            BData.Add(new MyBank { BankName = "I&M BANK",  BankCode = "BANK57" });
            BData.Add(new MyBank { BankName = "DTB",  BankCode = "BANK63" });
            BData.Add(new MyBank { BankName = "EQUITY BANK", BankCode = "BANK68" });
            BData.Add(new MyBank { BankName = "FAMILY BANK",  BankCode = "BANK70" });
            BData.Add(new MyBank { BankName = "GAB",  BankCode = "BANK72" });
            BData.Add(new MyBank { BankName = "FCB",  BankCode = "BANK74" });
            BData.Add(new MyBank { BankName = "KWFT",  BankCode = "BANK78" });
            BData.Add(new MyBank { BankName = "SIDIAN",  BankCode = "BANK66" });
            BData.Add(new MyBank { BankName = "UBA",  BankCode = "BANK08" });





            return BData;
          
        }
    }
}
