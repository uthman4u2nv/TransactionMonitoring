using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using TransMonAPI.Models;
using AllowAnonymousAttribute = System.Web.Mvc.AllowAnonymousAttribute;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;
using SelectPdf;

namespace TransactionMonitoring.Controllers
{
    
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View("HomePage");
        }
        [AllowAnonymous]
        public ActionResult HomePage()
        {
            //if (Session["name"] == null || Session["email"] == null)
            //{

            //    Response.Redirect("Index");
            //    return View("InvalidAccess");
            //}
            //else
            //{
            //    return View();
            //}
            return View();
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
            //CheckSessionState();
            if (Session["name"] == null || Session["email"] == null)
            {
                RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Report()
        {
            return View();
        }
        public ActionResult Convert(FormCollection collection)
        {
            // read parameters from the webpage
            //string url = collection["TxtUrl"];
            string dateFrom = Request.Form["dFrom"];
            string dateTo = Request.Form["dTo"];
            string sortCode = Request.Form["sCode"];
            


            //string dateFrom = "2020-01-01";  // dd-MM-yyyy    
            //string dateTo = "2020-10-23";
            //string sortCode = "40402000";

            string url = ConfigurationManager.AppSettings["ReportTemplate"];
            url += $"?dateFrom={dateFrom}&dateTo={dateTo}&sortCode={sortCode}"; 
            // string pdf_page_size = collection["DdlPageSize"];
            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);

            //string pdf_orientation = collection["DdlPageOrientation"];
            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(
                typeof(PdfPageOrientation), pdf_orientation, true);

            int webPageWidth = 1600;
            try
            {
                //webPageWidth = System.Convert.ToInt32(collection["TxtWidth"]);
                webPageWidth = 1024;
            }
            catch { }

            int webPageHeight = 0;
            try
            {
                //webPageHeight = System.Convert.ToInt32(collection["TxtHeight"]);
                webPageHeight = 0;
            }
            catch { }

            // instantiate a html to pdf converter object
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.MarginTop = 10;
            converter.Options.MarginBottom = 10;

            // create a new pdf document converting an url
            SelectPdf.PdfDocument doc = converter.ConvertUrl(url);

            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Document.pdf";
            return fileResult;
        }

        [System.Web.Http.HttpGet]
        public ActionResult ViewReport(string dateFrom, string dateTo,string sortCode)
        {
            
            BankReportRequest req = new BankReportRequest();
            //string dateFrom = Request["dFrom"];  // dd-MM-yyyy    
            //string dateTo= Request["dTo"];
            //string dateFrom = "2020-01-01";  // dd-MM-yyyy    
            //string dateTo = "2020-10-23"; 
            DateTime d;
            DateTime e;
            if (DateTime.TryParseExact(dateFrom, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
            {
                // use d
            }
            if (DateTime.TryParseExact(dateTo, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out e)){}

            req.dateFrom = d;
            req.dateTo = e;
            //req.sortCode = Request["sCode"];
            req.sortCode = sortCode;
            var rs = JsonConvert.SerializeObject(req);

            var url = ConfigurationManager.AppSettings["ReportAPIURL"];
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
            var rr = JsonConvert.DeserializeObject<BankReportResponse>(r);
            ViewBag.dateFrom = req.dateFrom;
            ViewBag.dateTo = req.dateTo;           
            ViewBag.In114 = Math.Round(rr.In114, 2);
            ViewBag.In118 = Math.Round(rr.In118, 2);
            ViewBag.In911 = Math.Round(rr.In911, 2);
            ViewBag.In111 = Math.Round(rr.In111, 2);
            ViewBag.In400 = Math.Round(rr.In400, 2);
            ViewBag.In912 = Math.Round(rr.In912, 2);
            ViewBag.In102 = Math.Round(rr.In102, 2);
            ViewBag.In100 = Math.Round(rr.In100, 2);
            ViewBag.In120 = Math.Round(rr.In120, 2);
            ViewBag.In114w = Math.Round(rr.In114w, 2);
            ViewBag.In118w = Math.Round(rr.In118w, 2);
            ViewBag.In911w = Math.Round(rr.In911w, 2);
            ViewBag.In111w = Math.Round(rr.In111w, 2);
            ViewBag.In400w = Math.Round(rr.In400w, 2);
            ViewBag.In912w = Math.Round(rr.In912w, 2);
            ViewBag.In102w = Math.Round(rr.In102w, 2);
            ViewBag.In100w = Math.Round(rr.In100w, 2);
            ViewBag.In120w = Math.Round(rr.In120w, 2);

            ViewBag.In114r = rr.In114r;
            ViewBag.In118r = rr.In118r;
            ViewBag.In911r = rr.In911r;
            ViewBag.In111r = rr.In111r;
            ViewBag.In400r = rr.In400r;
            ViewBag.In912r = rr.In912r;
            ViewBag.In102r = rr.In102r;
            ViewBag.In100r = rr.In100r;
            ViewBag.In120r = rr.In120r;

            ViewBag.Out114r = rr.Out114r;
            ViewBag.Out118r = rr.Out118r;
            ViewBag.Out911r = rr.Out911r;
            ViewBag.Out111r = rr.Out111r;
            ViewBag.Out400r = rr.Out400r;
            ViewBag.Out912r = rr.Out912r;
            ViewBag.Out102r = rr.Out102r;
            ViewBag.Out100r = rr.Out100r;
            ViewBag.Out120r = rr.Out120r;
            ViewBag.Out121r = rr.Out121r;
            ViewBag.Out909r = rr.Out909r;

            ViewBag.In114bb = rr.In114bb;
            ViewBag.In118bb = rr.In118bb;
            ViewBag.In911bb = rr.In911bb;
            ViewBag.In111bb = rr.In111bb;
            ViewBag.In400bb = rr.In400bb;
            ViewBag.In912bb = rr.In912bb;
            ViewBag.In102bb = rr.In102bb;
            ViewBag.In100bb = rr.In100bb;
            ViewBag.In120bb = rr.In120bb;

            ViewBag.Out114bb = rr.Out114bb;
            ViewBag.Out118bb = rr.Out118bb;
            ViewBag.Out911bb = rr.Out911bb;
            ViewBag.Out111bb = rr.Out111bb;
            ViewBag.Out400bb = rr.Out400bb;
            ViewBag.Out912bb = rr.Out912bb;
            ViewBag.Out102bb = rr.Out102bb;
            ViewBag.Out100bb = rr.Out100bb;
            ViewBag.Out120bb = rr.Out120bb;
            ViewBag.Out121bb = rr.Out121bb;
            ViewBag.Out909bb = rr.Out909bb;
            ViewBag.Out114 = Math.Round(rr.Out114, 2);
            ViewBag.Out118 = Math.Round(rr.Out118, 2);
            ViewBag.Out911 = Math.Round(rr.Out911, 2);
            ViewBag.Out111 = Math.Round(rr.Out111, 2);
            ViewBag.Out400 = Math.Round(rr.Out400, 2);
            ViewBag.Out912 = Math.Round(rr.Out912, 2);
            ViewBag.Out102 = Math.Round(rr.Out102, 2);
            ViewBag.Out100 = Math.Round(rr.Out100, 2);
            ViewBag.Out120 = Math.Round(rr.Out120, 2);
            ViewBag.Out121 = Math.Round(rr.Out121, 2);
            ViewBag.Out909 = Math.Round(rr.Out909, 2);

            ViewBag.Out114w = Math.Round(rr.Out114w, 2);
            ViewBag.Out118w = Math.Round(rr.Out118w, 2);
            ViewBag.Out911w = Math.Round(rr.Out911w, 2);
            ViewBag.Out111w = Math.Round(rr.Out111w, 2);
            ViewBag.Out400w = Math.Round(rr.Out400w, 2);
            ViewBag.Out912w = Math.Round(rr.Out912w, 2);
            ViewBag.Out102w = Math.Round(rr.Out102w, 2);
            ViewBag.Out100w = Math.Round(rr.Out100w, 2);
            ViewBag.Out120w = Math.Round(rr.Out120w, 2);
            ViewBag.Out121w = Math.Round(rr.Out121w, 2);
            ViewBag.Out909w = Math.Round(rr.Out909w, 2);
            ViewBag.BankName = rr.BankName;
            //ViewBag.headingDate = dateFrom + " -  " + dateTo;
            ViewBag.headingDate = rr.headingDate;
            ViewBag.WeeklyDate = rr.WeeklyDate;
            ViewBag.link = 100;
            ViewBag.linkr = "1st";
            ViewBag.linkbb = "100%";
            ViewBag.linkw = "0.00";
            ViewBag.linkwr = "1st";
            ViewBag.intf = "95";
            ViewBag.intfr = "3rd";
            ViewBag.intfbb = "100%";
            ViewBag.intfw = "0.00";
            ViewBag.intfwr = "1st";
            ViewBag.cust = 10;
            ViewBag.custr = "2nd";
            ViewBag.custbb = "5%";
            ViewBag.custw = "0.00";
            ViewBag.custwr = "1st";

            ViewBag.Out114wr = rr.Out114wr;
            ViewBag.Out118wr = rr.Out118wr;
            ViewBag.Out911wr = rr.Out911wr;
            ViewBag.Out111wr = rr.Out111wr;
            ViewBag.Out400wr = rr.Out400wr;
            ViewBag.Out912wr = rr.Out912wr;
            ViewBag.Out102wr = rr.Out102wr;
            ViewBag.Out100wr = rr.Out100wr;
            ViewBag.Out120wr = rr.Out120wr;
            ViewBag.Out121wr = rr.Out121wr;
            ViewBag.Out909wr = rr.Out909wr;

            ViewBag.In114wr = rr.In114wr;
            ViewBag.In118wr = rr.In118wr;
            ViewBag.In911wr = rr.In911wr;
            ViewBag.In111wr = rr.In111wr;
            ViewBag.In400wr = rr.In400wr;
            ViewBag.In912wr = rr.In912wr;
            ViewBag.In102wr = rr.In102wr;
            ViewBag.In100wr = rr.In100wr;
            ViewBag.In120wr = rr.In120wr;

            ViewBag.bankrank = "1.08%";
            ViewBag.bankrankw = "1.49%";

            ViewBag.indrank = "1.28%";
            ViewBag.indrankw = "1.35%";


            return View();
        }

        public void CheckSessionState()
        {
            if (Session["name"] == null || Session["email"]==null)
            {
                RedirectToAction("Login");
            }
        }
        [Authorize]
        public ActionResult Users()
        {
            string bankAdmin = Session["bankAdmin"] as string;
            string bankUser= Session["bankUser"] as string;
            string name=Session["name"] as string;
            string email= Session["email"] as string;
            string bankCode= Session["bankCode"] as string;
            // CheckSessionState();
            if (Session["name"] == null || Session["email"] == null)
            {
                return RedirectToAction("Login");
            }
            if (bankAdmin == "true" && bankCode !="001")
            {
                return View();
            }
            //else if(bankAdmin == "false" && bankUser == "false" )
            else if (bankCode == "001")
            {
                return View("UsersAdmin");
            }
            else
            {
                return View();
            }
            //if (!string.IsNullOrEmpty(Session["bankAdmin"] as string) && Session["bankAdmin"].ToString() == "true")
            //{
            //    return View();
            //}
            //else if (!string.IsNullOrEmpty(Session["bankAdmin"] as string) && Session["bankAdmin"].ToString() == "false" && Session["bankUser"].ToString() == "false")
            //{
            //    return View("UsersAdmin");
            //}
            //else if (string.IsNullOrEmpty(Session["bankUser"] as string) || Session["bankUser"].ToString() == "true")
            //{
            //    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Unauthorised Access" };
            //    //throw new HttpResponseException(msg);
            //    return new HttpUnauthorizedResult();
            //}


            return View();
            
        }

        public ActionResult Test()
        {
            return View();
        }
        public ActionResult Banks()
        {
            return View();
        }
        public class Resp
        {
            public string responseCode { get; set; }
        }
        public class UserDetailsResp
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string bankAdmin { get; set; }
            public string bankUser { get; set; }
            public string ipslAdmin { get; set; }
            public string ipslSuperAdmin { get; set; }
            public string bankCode { get; set; }
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [Authorize]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            ViewBag.Message = "You have logged out";
            return View("Login");
        }
        [System.Web.Http.HttpPost]
        public JsonResult SetUser()
        {
            //Check if he is a valid user
            Session["email"] = Request.Params["email"];
            Session["name"] = Request.Params["name"];
            Session["bankAdmin"] = Request.Params["bankAdmin"];
            Session["bankUser"]= Request.Params["bankUser"];
            Session["ipslAdmin"] = Request.Params["ipslAdmin"];
            Session["bankCode"]=Request.Params["bankCode"];
            Resp r = new Resp();
            r.responseCode = "00";


            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [System.Web.Http.HttpPost]
        public JsonResult GetLoggedUser()
        {
            string email = "";
            string name = "";
            string bankAdmin;
            string bankUser;
            string ipslAdmin;
            string ipslSuperAdmin;
            string bankCode;
            //if (!string.IsNullOrEmpty(Session["email"].ToString()))
            //{
            string bAdmin = Session["bankAdmin"] as string;
            string bUser = Session["bankUser"] as string;
            string iSuperAdmin = Session["ipslAdmin"] as string;
            string username = Session["name"] as string;
            string useremail = Session["email"] as string;
            string bCode= Session["bankCode"] as string;
            // CheckSessionState();

            //email = Session["email"].ToString();
            //    name= Session["name"].ToString();
            //    bankAdmin= Session["bankAdmin"].ToString();
            //    bankUser= Session["bankUser"].ToString();
            //    bankCode = Session["bankCode"].ToString();

            email = useremail;
            name = username;
            bankAdmin = bAdmin;
            ipslSuperAdmin = iSuperAdmin;
            bankUser = bUser;
            bankCode = bCode;
            if (bankAdmin=="false" && bankUser=="false" && ipslSuperAdmin == "false")
            {
                ipslAdmin = "true";
            }
            else
            {
                ipslAdmin = "false";
            }
            
            

            //}
            UserDetailsResp r = new UserDetailsResp();
            r.Email = email;
            r.Name = name;
            r.bankUser = bankUser;
            r.bankCode = bankCode;
            r.bankAdmin = bankAdmin;
            r.ipslAdmin = ipslAdmin;
            r.ipslSuperAdmin = ipslSuperAdmin;



            return Json(r, JsonRequestBehavior.AllowGet);
        }
        //[System.Web.Http.HttpPost]
        //public JsonResult ReturnBankData()
        //{
        //    BankData ac = new BankData();
        //    ac.BankName = "";
        //    string rs = JsonConvert.SerializeObject(ac);

        //    var content = new StringContent(rs, Encoding.UTF8, "text/json");
        //    var client = new HttpClient();

        //    HttpResponseMessage result = null;
        //    //client.DefaultRequestHeaders.Add("Username", username);
        //    //client.DefaultRequestHeaders.Add("Password", password);
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    ServicePointManager.ServerCertificateValidationCallback = new
        //  RemoteCertificateValidationCallback
        //  (
        //     delegate { return true; }
        //  );
        //    var url = "http://localhost:57868/api/BankData";
        //    result = client.PostAsync(url, content).Result;
        //    //var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1] { '"' });
        //    var r = result.Content.ReadAsStringAsync().Result.Replace("\\", "");
        //    //var resp = JsonConvert.DeserializeObject<BankData>(r.ToString());
        //    return Json(r.ToString());
        //}
        public ActionResult Loading()
        {
            return RedirectToAction("Dashboard");
            
        }
        public ActionResult LoadingChangePassword()
        {
            return RedirectToAction("ChangePassword");

        }
        public ActionResult Settings()
        {
            return View();
        }
        [Authorize]
        public ActionResult Dashboard()
        {
            //CheckSessionState();
            string bankAdmin = Session["bankAdmin"] as string;
            string bankUser = Session["bankUser"] as string;
            string name = Session["name"] as string;
            string email = Session["email"] as string;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        

        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}