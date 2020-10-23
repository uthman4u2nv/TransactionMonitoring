using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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