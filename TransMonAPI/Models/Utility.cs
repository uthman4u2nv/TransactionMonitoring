using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Web;
using Newtonsoft.Json.Serialization;

namespace TransMonAPI.Models
{
    public class Utility
    {
    }
    public class BankData
    {
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public double InFailed { get; set; }
        public float OutFailed { get; set; }
        public int inVolume { get; set; }
        public int outVolume { get; set; }
        public double IndInRate { get; set; }
        public double IndOutRate { get; set; }
        
        public int IndInVolume { get; set; }
        public int IndOutVolume { get; set; }

        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public float failureRate { get; set; }
    }

    public class IndustryStats
    {
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public float failureRate { get; set; }
    }

    public class Statistics
    {
        public int Up { get; set; }
        public int Down { get; set; }
        public int Warning { get; set; }
    }

    public class Users
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }


    public class AddUserResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public AddUserData data { get; set; }
    }

    public class AddUserData
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string bankCode { get; set; }
        public bool bankAdmin { get; set; }
        public bool bankUser { get; set; }
    }


    public class AddUserRequest
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string bankCode { get; set; }
        public int bankAdmin { get; set; }
        public int bankUser { get; set; }
        public int ipslAdmin { get; set; }
    }

    public class AuthResp
    {
        public bool Status { get; set; }
    }

    public class ListOfUserRequest
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }



    }
    public class UList
    {
        public string status { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public U[] data { get; set; }
    }

    public class U
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string bankCode { get; set; }
        public bool bankAdmin { get; set; }
        public bool bankUser { get; set; }
        public bool ipslAdmin { get; set; }
    }
    public class ChangePasswordRequest
    {
        public string email { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmPassword { get; set; }

    }
    public class NewBankDataRequest
    {
        public string MyData { get; set; }
    }
    public class GetDashboardAnalyticsRequest
    {
        public string bankCode { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
    public class LoginResp
    {
        public string status { get; set; }
        public string message { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string bankCode { get; set; }
        public bool bankAdmin { get; set; }
        public bool bankUser { get; set; }
        public bool ipslAdmin { get; set; }
        public bool firsttimeuser { get; set; }
    }

    public class LoginResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public LoginResponseData data { get; set; }
    }

    public class LoginResponseData
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string bankCode { get; set; }
        public bool bankAdmin { get; set; }
        public bool bankUser { get; set; }
        public bool ipslAdmin { get; set; }
        public bool firsttime { get; set; }
    }
    public class LoginDetailsResponse
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string bankCode { get; set; }
        public bool bankAdmin { get; set; }
        public bool bankUser { get; set; }
        public string status { get; set; }
        public bool firsttime { get; set; }
    }
    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class ResetUserRequest
    {
        public string email { get; set; }
        public int ipslAdmin { get; set; }
    }
    public class ListOfBankRequest
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
    public class BankListDetails
    {
        public string status { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public BList[] data { get; set; }
    }

    public class AddBankRequest
    {
        public string name { get; set; }
        public string code { get; set; }
        public string alias { get; set; }
        public string sortCode { get; set; }
        public int volumeThreshold { get; set; }
        public double failureThreshold { get; set; }
        public List<string> email { get; set; }

    }
    public class UpdateBankRequest
    {
        public string name { get; set; }
        public string code { get; set; }
        public string alias { get; set; }
        public int active { get; set; }
        public string sortCode { get; set; }
        public int volumeThreshold { get; set; }
        public double failureThreshold { get; set; }
        public List<string> email { get; set; }

    }
    public class BankDetailsRequest
    {
        public string bankCode { get; set; }
    }
    public class BList
    {
        public string name { get; set; }
        public string code { get; set; }
        public string alias { get; set; }
        //public DateTime dateCreated { get; set; }
        public string date_created { get; set; }
        public string sortCode { get; set; }
        public int volumeThreshold { get; set; }
        public double failureThreshold { get; set; }
        public string email { get; set; }
    }


    public class Banks
    {
        public string name { get; set; }
        public string code { get; set; }
        public DateTime date_created { get; set; }
        public string alias { get; set; }
        public string dateCreated { get; set; }
        public int active { get; set; }
        public string sortCode { get; set; }
        public int volumeThreshold { get; set; }
        public double failureThreshold { get; set; }
        public string email { get; set; }
    }

    public class BankDetailsResponse
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public string bankName { get; set; }
        public string bankCode { get; set; }
        public string bankAlias { get; set; }
        public int active { get; set; }
        public string sortCode { get; set; }
        public int volumeThreshold { get; set; }
        public double failureThreshold { get; set; }
        public string email { get; set; }
    }

    public class Response
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
    }

    public class BankList
    {
        public int bankID { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public DateTime date_created { get; set; }
    }

    public class Userss
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string bankCode { get; set; }
        public int roleID { get; set; }
    }

    public class UserLists
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string bankName { get; set; }
        public string roleID { get; set; }
        public int role { get; set; }
        public string bankID { get; set; }
    }

    public class LoginReq
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class Login
    {
        public string status { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public LoginData data { get; set; }
    }

    public class LoginData
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string bankCode { get; set; }
        public int bankAdmin { get; set; }
        public int bankUser { get; set; }
    }


    public class TransAnalyticResponse
    {
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public int failureRate { get; set; }
        public Analytic[] analytics { get; set; }
    }

    public class Analytic
    {
        public string bankCode { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public float failureRate { get; set; }

        public RespCodeTransAnalysis responseCode { get; set; }
    }

    public class RespCodeTransAnalysis
    {
        public float _000 { get; set; }
        public int _102 { get; set; }
        public float _912 { get; set; }
    }

    public class AnalyticalRequest
    {
        public string bankCode { get; set; }
    }

    public class NewAnalyticalRequest
    {
        public string bankCode { get; set; }
        public string bankData { get; set; }
        public string dailyData { get; set; }
       

        [OnError]
        internal void OnError(StreamingContext context, ErrorContext errorContext)
        {
            errorContext.Handled = true;
        }
    }

    public class Analytical
    {
        public string responseCode { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public float failureRate { get; set; }
        public float success { get; set; }
        public int error102 { get; set; }
        public float error912 { get; set; }
        public int outerror102 { get; set; }
        public int outerror912 { get; set; }
        public int outfailureRate { get; set; }
        public int outsuccess { get; set; }
        public int outtotalFailure { get; set; }
        public int outtotalVolume { get; set; }

        public List<ChartData> FailureRateList { get; set; }
        public List<ChartData> FailureRateListIn { get; set; }
        public int totalInDaily { get; set; }
        public int totalOutdaily { get; set; }
        public int totalDailyFailedIn { get; set; }
        public int totalDailyFailedOut { get; set; }
        public double dailyFailureRateIn { get; set; }
        public double dailyFailureRateOut { get; set; }
        public double dailySuccessRateIn { get; set; }
        public double dailySuccessRateOut { get; set; }
        public int dailySuccessVolumeIn { get; set; }
        public int dailySuccessVolumeOut { get; set; }

        public string TransactionPeriod { get; set; }
    }
    //---Start of TransAnalytical--


    public class tAnalytic
    {
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public float failureRate { get; set; }
        public Analytics[] analytics { get; set; }
    }

    public class Analytics
    {
        public string bankCode { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public float failureRate { get; set; }
        public Responsecode responseCode { get; set; }
    }

    public class Responsecode
    {
        public float _000 { get; set; }
        public float _911 { get; set; }
        public float _400 { get; set; }
        public float _912 { get; set; }
        public int _102 { get; set; }
    }



    //--End TransAnalytical



    public class NewTransAnalytics
    {
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public float failureRate { get; set; }
        public RespCode responseCode { get; set; }
        public Inwardsummary[] inwardSummary { get; set; }
        public Outwardsummary[] outwardSummary { get; set; }

        public bIndustryinward industryInward { get; set; }
        public bIndustryoutward industryOutward { get; set; }
    }

    public class bIndustryinward
    {
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public double inFailed { get; set; }
    }
    public class bIndustryoutward
    {
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public double outFailed { get; set; }
    }

    public class RespCode
    {
        public float _102 { get; set; }
        public float _000 { get; set; }
    }

    public class Inwardsummary
    {
        public string bankCode { get; set; }
        public string bankName { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public float inFailed { get; set; }
    }

    public class Outwardsummary
    {
        public string bankCode { get; set; }
        public string bankName { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public float outFailed { get; set; }
        public string sortCode { get; set; }
    }
    public class ChartData
    {
        public long Date { get; set; }
        public int FailureRate { get; set; }
        


    }
    public class ChartDataOut
    {
        public double FailureRate { get; set; }
        public long Date { get; set; }
    }
    public class ChatDataRequest
    {
        public string BankCode { get; set; }
    }

    public class ChatDataResponse
    {
        public List<ChartData> FailureRate { get; set; }
    }

    /***NEW ADDITION BEGINS***/


    public class LatestTransAnalytics
    {
        public string status { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public int count { get; set; }
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string bank_code { get; set; }
        public DateTime date_created { get; set; }
        public float in_failure_rate { get; set; }
        public float out_failure_rate { get; set; }
        public string total_in_failed { get; set; }
        public string total_in_volume { get; set; }
        public string total_out_failed { get; set; }
        public string total_out_volume { get; set; }
    }


    /***28052020**/

    public class Lytics
    {
        public string status { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public int count { get; set; }
        public Datums[] data { get; set; }
    }

    public class Datums
    {
        public string id { get; set; }
        public string bank_code { get; set; }
        public DateTime date_created { get; set; }
        public float in_failure_rate { get; set; }
        public int out_failure_rate { get; set; }
        public string total_in_failed { get; set; }
        public string total_in_volume { get; set; }
        public string total_out_failed { get; set; }
        public string total_out_volume { get; set; }
    }



    public class FailedLogin
    {
        public string status { get; set; }
        public string message { get; set; }
        public string description { get; set; }
    }

    public class LastFiveMinutesAnalyticsResp
    {
        public string responseCode { get; set; }
        public string transactionperiod { get; set; }
        public List<ChartData> FailureRateList { get; set; }
        public List<ChartData> FailureRateListIn { get; set; }
        public List<ChartData> FailureRateListVolumeOut { get; set; }
        public List<ChartData> FailureRateListVolumeIn { get; set; }
        public double outfailureRate { get; set; }
        public int outtotalFailure { get; set; }
        public int outtotalVolume { get; set; }
        public double outsuccess { get; set; }
        public int outsuccessvolume { get; set; }
        public int outerror102 { get; set; }
        public int outerror912 { get; set; }
        public int outerror100 { get; set; }
        public int outerror114 { get; set; }
        public int outerror908 { get; set; }
        public int outerror111 { get; set; }
        public int outerror400 { get; set; }
        public int outerror909 { get; set; }
        public int outerror107 { get; set; }
        public int outerror116 { get; set; }
        public int outerror118 { get; set; }
        public int outerror119 { get; set; }
        public int outerror121 { get; set; }
        public int outerror910 { get; set; }
        public int outerror911 { get; set; }

        public int outerror101 { get; set; }
        public int outerror103 { get; set; }
        public int outerror104 { get; set; }
        public int outerror105 { get; set; }
        public int outerror106 { get; set; }
        public int outerror109 { get; set; }
        public int outerror110 { get; set; }
        public int outerror112 { get; set; }
        public int outerror115 { get; set; }
        public int outerror117 { get; set; }
        public int outerror120 { get; set; }
        public int outerror125 { get; set; }
        public int outerror200 { get; set; }
        public int outerror201 { get; set; }
        public int outerror202 { get; set; }
        public int outerror203 { get; set; }
        public int outerror204 { get; set; }
        public int outerror904 { get; set; }
        public int outerror913 { get; set; }
        public int outerror914 { get; set; }
        public int outerror923 { get; set; }

        public double outerror102Rate { get; set; }
        public double outerror912Rate { get; set; }
        public double outerror100Rate { get; set; }
        public double outerror114Rate { get; set; }
        public double outerror908Rate { get; set; }
        public double outerror111Rate { get; set; }
        public double outerror400Rate { get; set; }
        public double outerror909Rate { get; set; }
        public double outerror107Rate { get; set; }
        public double outerror116Rate { get; set; }
        public double outerror118Rate { get; set; }
        public double outerror119Rate { get; set; }
        public double outerror121Rate { get; set; }
        public double outerror910Rate { get; set; }
        public double outerror911Rate { get; set; }
        public double outerror101Rate { get; set; }
        public double outerror103Rate { get; set; }
        public double outerror104Rate { get; set; }
        public double outerror105Rate { get; set; }
        public double outerror106Rate { get; set; }
        public double outerror109Rate { get; set; }
        public double outerror110Rate { get; set; }
        public double outerror112Rate { get; set; }
        public double outerror115Rate { get; set; }
        public double outerror117Rate { get; set; }
        public double outerror120Rate { get; set; }
        public double outerror125Rate { get; set; }
        public double outerror200Rate { get; set; }
        public double outerror201Rate { get; set; }
        public double outerror202Rate { get; set; }
        public double outerror203Rate { get; set; }
        public double outerror204Rate { get; set; }
        public double outerror904Rate { get; set; }
        public double outerror913Rate { get; set; }
        public double outerror914Rate { get; set; }
        public double outerror923Rate { get; set; }

        public double infailureRate { get; set; }
        public double intotalFailure { get; set; }
        public int intotalVolume { get; set; }
        public double insuccess { get; set; }
        public int insuccessvolume { get; set; }
        public int inerror102 { get; set; }
        public int inerror912 { get; set; }
        public int inerror100 { get; set; }
        public int inerror114 { get; set; }
        public int inerror117 { get; set; }
        public int inerror204 { get; set; }
        
        public int inerror908 { get; set; }
        public int inerror111 { get; set; }
        public int inerror400 { get; set; }
        public int inerror909 { get; set; }

        public int inerror107 { get; set; }
        public int interror116 { get; set; }
        public int inerror118 { get; set; }
        public int inerror119 { get; set; }
        public int inerror121 { get; set; }
        public int inerror910 { get; set; }
        public int inerror911 { get; set; }

        public int inerror101 { get; set; }
        public int inerror103 { get; set; }
        public int inerror104 { get; set; }
        public int inerror105 { get; set; }
        public int inerror106 { get; set; }
        public int inerror109 { get; set; }
        public int inerror110 { get; set; }
        public int inerror112 { get; set; }

        public int inerror115 { get; set; }
        public int interror117 { get; set; }
        public int inerror120 { get; set; }
        public int inerror125 { get; set; }
        public int inerror200 { get; set; }
        public int inerror201 { get; set; }
        public int inerror202 { get; set; }

        public int inerror203 { get; set; }
        public int interror204 { get; set; }
        public int inerror904 { get; set; }
        public int inerror913 { get; set; }
        public int inerror914 { get; set; }
        public int inerror923 { get; set; }

        public double inerror102Rate { get; set; }
        public double inerror912Rate { get; set; }
        public double inerror100Rate { get; set; }
        public double inerror114Rate { get; set; }
        public double inerror908Rate { get; set; }
        public double inerror111Rate { get; set; }
        public double inerror400Rate { get; set; }
        public double inerror909Rate { get; set; }

        public double inerror107Rate { get; set; }
        public double interror116Rate { get; set; }
        public double inerror118Rate { get; set; }
        public double inerror119Rate { get; set; }
        public double inerror121Rate { get; set; }
        public double inerror910Rate { get; set; }
        public double inerror911Rate { get; set; }

        public double inerror101Rate { get; set; }
        public double inerror103Rate { get; set; }
        public double inerror104Rate { get; set; }
        public double inerror105Rate { get; set; }
        public double inerror106Rate { get; set; }
        public double inerror109Rate { get; set; }
        public double inerror110Rate { get; set; }
        public double inerror112Rate { get; set; }
        public double inerror115Rate { get; set; }
        public double inerror117Rate { get; set; }
        public double inerror120Rate { get; set; }
        public double inerror125Rate { get; set; }
        public double inerror200Rate { get; set; }
        public double inerror201Rate { get; set; }
        public double inerror202Rate { get; set; }
        public double inerror203Rate { get; set; }
        public double inerror204Rate { get; set; }
        public double inerror904Rate { get; set; }
        public double inerror913Rate { get; set; }
        public double inerror914Rate { get; set; }
        public double inerror923Rate { get; set; }

        public double IndInRate { get; set; }
        public double IndOutRate { get; set; }

        /**DAILY RESP **/
        public int dailyTxnVolumeIn { get; set; }
        public int volumeFailedTxnIn { get; set; }
        public float dailyFailureRateIn { get; set; }
        public int dailySuccessVolumeIn { get; set; }
        public float dailySuccessRateIn { get; set; }
        public int inDaily_100 { get; set; }
        public int inDaily_107 { get; set; }
        public int inDaily_111 { get; set; }
        public int inDaily_114 { get; set; }
        public int inDaily_116 { get; set; }
        public int inDaily_118 { get; set; }
        public int inDaily_119 { get; set; }
        public int inDaily_121 { get; set; }
        public int inDaily_400 { get; set; }
        public int inDaily_908 { get; set; }
        public int inDaily_909 { get; set; }
        public int inDaily_910 { get; set; }
        public int inDaily_911 { get; set; }
        public int inDaily_912 { get; set; }
        public int inDaily_000 { get; set; }

        public int inDaily_101 { get; set; }
        public int inDaily_103 { get; set; }
        public int inDaily_104 { get; set; }
        public int inDaily_105 { get; set; }
        public int inDaily_106 { get; set; }
        public int inDaily_109 { get; set; }
        public int inDaily_110 { get; set; }
        public int inDaily_112 { get; set; }
        public int inDaily_115 { get; set; }
        public int inDaily_117 { get; set; }
        public int inDaily_120 { get; set; }
        public int inDaily_125 { get; set; }
        public int inDaily_200 { get; set; }
        public int inDaily_201 { get; set; }
        public int inDaily_202 { get; set; }

        public int inDaily_203 { get; set; }
        public int inDaily_204 { get; set; }
        public int inDaily_904 { get; set; }
        public int inDaily_913 { get; set; }
        public int inDaily_914 { get; set; }
        public int inDaily_923 { get; set; }
        public double inDailyRate_100 { get; set; }
        public double inDailyRate_107 { get; set; }
        public double inDailyRate_111 { get; set; }
        public double inDailyRate_114 { get; set; }
        public double inDailyRate_116 { get; set; }
        public double inDailyRate_118 { get; set; }
        public double inDailyRate_119 { get; set; }
        public double inDailyRate_121 { get; set; }
        public double inDailyRate_400 { get; set; }
        public double inDailyRate_908 { get; set; }
        public double inDailyRate_909 { get; set; }
        public double inDailyRate_910 { get; set; }
        public double inDailyRate_911 { get; set; }
        public double inDailyRate_912 { get; set; }
        public double inDailyRate_000 { get; set; }
        public double inDailyRate_101 { get; set; }
        public double inDailyRate_103 { get; set; }
        public double inDailyRate_104 { get; set; }
        public double inDailyRate_105 { get; set; }
        public double inDailyRate_106 { get; set; }
        public double inDailyRate_109 { get; set; }
        public double inDailyRate_110 { get; set; }
        public double inDailyRate_112 { get; set; }
        public double inDailyRate_115 { get; set; }
        public double inDailyRate_117 { get; set; }
        public double inDailyRate_120 { get; set; }
        public double inDailyRate_125 { get; set; }
        public double inDailyRate_200 { get; set; }
        public double inDailyRate_201 { get; set; }
        public double inDailyRate_202 { get; set; }
        public double inDailyRate_203 { get; set; }
        public double inDailyRate_204 { get; set; }
        public double inDailyRate_904 { get; set; }
        public double inDailyRate_913 { get; set; }
        public double inDailyRate_914 { get; set; }
        public double inDailyRate_923 { get; set; }

        public int dailyTxnVolumeOut { get; set; }
        public int volumeFailedTxnOut { get; set; }
        public float dailyFailureRateOut { get; set; }
        public int dailySuccessVolumeOut { get; set; }
        public float dailySuccessRateOut { get; set; }
        public int outDaily_100 { get; set; }
        public int outDaily_107 { get; set; }
        public int outDaily_111 { get; set; }
        public int outDaily_114 { get; set; }
        public int outDaily_116 { get; set; }
        public int outDaily_118 { get; set; }
        public int outDaily_119 { get; set; }
        public int outDaily_121 { get; set; }
        public int outDaily_400 { get; set; }
        public int outDaily_908 { get; set; }
        public int outDaily_909 { get; set; }
        public int outDaily_910 { get; set; }
        public int outDaily_911 { get; set; }
        public int outDaily_912 { get; set; }
        public int outDaily_000 { get; set; }
        public int outDaily_101 { get; set; }
        public int outDaily_103 { get; set; }
        public int outDaily_104 { get; set; }
        public int outDaily_105 { get; set; }
        public int outDaily_106 { get; set; }
        public int outDaily_109 { get; set; }
        public int outDaily_110 { get; set; }
        public int outDaily_112 { get; set; }
        public int outDaily_115 { get; set; }
        public int outDaily_117 { get; set; }
        public int outDaily_120 { get; set; }
        public int outDaily_125 { get; set; }
        public int outDaily_200 { get; set; }
        public int outDaily_201 { get; set; }
        public int outDaily_202 { get; set; }

        public int outDaily_203 { get; set; }
        public int outDaily_204 { get; set; }
        public int outDaily_904 { get; set; }
        public int outDaily_913 { get; set; }
        public int outDaily_914 { get; set; }
        public int outDaily_923 { get; set; }

        public double outDailyRate_100 { get; set; }
        public double outDailyRate_107 { get; set; }
        public double outDailyRate_111 { get; set; }
        public double outDailyRate_114 { get; set; }
        public double outDailyRate_116 { get; set; }
        public double outDailyRate_118 { get; set; }
        public double outDailyRate_119 { get; set; }
        public double outDailyRate_121 { get; set; }
        public double outDailyRate_400 { get; set; }
        public double outDailyRate_908 { get; set; }
        public double outDailyRate_909 { get; set; }
        public double outDailyRate_910 { get; set; }
        public double outDailyRate_911 { get; set; }
        public double outDailyRate_912 { get; set; }
        public double outDailyRate_000 { get; set; }

        public double outDailyRate_101 { get; set; }
        public double outDailyRate_103 { get; set; }
        public double outDailyRate_104 { get; set; }
        public double outDailyRate_105 { get; set; }
        public double outDailyRate_106 { get; set; }
        public double outDailyRate_109 { get; set; }
        public double outDailyRate_110 { get; set; }
        public double outDailyRate_112 { get; set; }
        public double outDailyRate_115 { get; set; }
        public double outDailyRate_117 { get; set; }
        public double outDailyRate_120 { get; set; }
        public double outDailyRate_125 { get; set; }
        public double outDailyRate_200 { get; set; }
        public double outDailyRate_201 { get; set; }
        public double outDailyRate_202 { get; set; }
        public double outDailyRate_203 { get; set; }
        public double outDailyRate_204 { get; set; }
        public double outDailyRate_904 { get; set; }
        public double outDailyRate_913 { get; set; }
        public double outDailyRate_914 { get; set; }
        public double outDailyRate_923 { get; set; }
    }

    /**MODIFIED TRANSACTION ANALYTICS**/

    public class LastFiveMinutesAnalytics
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public float failureRate { get; set; }
        public NewResponsecode responseCode { get; set; }
        public NewInwardsummary[] inwardSummary { get; set; }
        public NewOutwardsummary[] outwardSummary { get; set; }
    }

    public class NewResponsecode
    {
        public float _100 { get; set; }
        public float _102 { get; set; }
        public float _111 { get; set; }
        public float _114 { get; set; }
        public float _400 { get; set; }
        public float _908 { get; set; }
        public float _000 { get; set; }
        public float _909 { get; set; }
    }

    public class NewInwardsummary
    {
        public string bankCode { get; set; }
        public string bankName { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public Bankresponsecode bankResponseCode { get; set; }
        public float inFailed { get; set; }
    }

    public class Bankresponsecode
    {
        public float _000 { get; set; }
        public float _102 { get; set; }
        public int _100 { get; set; }
        public float _111 { get; set; }
        public float _114 { get; set; }
        public int _908 { get; set; }
        public float _912 { get; set; }
        public float _400 { get; set; }
        public float _909 { get; set; }
    }

    public class NewOutwardsummary
    {
        public string bankCode { get; set; }
        public string bankName { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public Bankresponsecode1 bankResponseCode { get; set; }
        public float outFailed { get; set; }
    }

    public class Bankresponsecode1
    {
        public float _000 { get; set; }
        public float _102 { get; set; }
        public float _100 { get; set; }
        public float _114 { get; set; }
        public float _908 { get; set; }
        public float _912 { get; set; }
        public float _111 { get; set; }
        public float _400 { get; set; }
        public float _909 { get; set; }
    }


    /**DAILY TRANSACTION ANALYTIC */

    public class DailyTransAnalytic
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public Industryinward industryInward { get; set; }
        public Industryoutward industryOutward { get; set; }
        public DailyInwardsummary[] inwardSummary { get; set; }
        public DailyOutwardsummary[] outwardSummary { get; set; }
    }

    public class Industryinward
    {
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public DailyBankresponsecode bankResponseCode { get; set; }
        public Bankresponsecodecount bankResponseCodeCount { get; set; }
        public float inFailed { get; set; }
    }

    public class DailyBankresponsecode
    {
        public float _100 { get; set; }
        public float _107 { get; set; }
        public float _111 { get; set; }
        public float _114 { get; set; }
        public float _116 { get; set; }
        public float _118 { get; set; }
        public float _119 { get; set; }
        public float _121 { get; set; }
        public float _400 { get; set; }
        public float _908 { get; set; }
        public float _909 { get; set; }
        public float _910 { get; set; }
        public float _911 { get; set; }
        public float _912 { get; set; }

        public float _101 { get; set; }
        public float _103 { get; set; }
        public float _104 { get; set; }
        public float _105 { get; set; }
        public float _106 { get; set; }
        public float _109 { get; set; }
        public float _110 { get; set; }
        public float _112 { get; set; }
        public float _115 { get; set; }
        public float _117 { get; set; }
        public float _120 { get; set; }
        public float _125 { get; set; }
        public float _200 { get; set; }
        public float _201 { get; set; }
        public float _202 { get; set; }
        public float _203 { get; set; }
        public float _204 { get; set; }
        public float _904 { get; set; }
        public float _913 { get; set; }
        public float _923 { get; set; }

        public float _000 { get; set; }
    }

    public class Bankresponsecodecount
    {
        public int _100 { get; set; }
        public int _107 { get; set; }
        public int _111 { get; set; }
        public int _114 { get; set; }
        public int _116 { get; set; }
        public int _118 { get; set; }
        public int _119 { get; set; }
        public int _121 { get; set; }
        public int _400 { get; set; }
        public int _908 { get; set; }
        public int _909 { get; set; }
        public int _910 { get; set; }
        public int _911 { get; set; }
        public int _912 { get; set; }
        public int _101 { get; set; }
        public int _103 { get; set; }
        public int _104 { get; set; }
        public int _105 { get; set; }
        public int _106 { get; set; }
        public int _109 { get; set; }
        public int _110 { get; set; }
        public int _112 { get; set; }
        public int _115 { get; set; }
        public int _117 { get; set; }
        public int _120 { get; set; }
        public int _125 { get; set; }
        public int _200 { get; set; }
        public int _201 { get; set; }
        public int _202 { get; set; }
        public int _203 { get; set; }
        public int _204 { get; set; }
        public int _904 { get; set; }
        public int _913 { get; set; }
        public int _914 { get; set; }
        public int _923 { get; set; }
        public int _000 { get; set; }
    }

    public class Industryoutward
    {
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public DailyBankresponsecode1 bankResponseCode { get; set; }
        public Bankresponsecodecount1 bankResponseCodeCount { get; set; }
        public float outFailed { get; set; }
    }

    public class DailyBankresponsecode1
    {
        public float _100 { get; set; }
        public float _102 { get; set; }
        public float _107 { get; set; }
        public float _111 { get; set; }
        public float _114 { get; set; }
        public float _116 { get; set; }
        public float _118 { get; set; }
        public float _119 { get; set; }
        public float _120 { get; set; }
        public float _121 { get; set; }
        public float _400 { get; set; }
        public float _908 { get; set; }
        public float _909 { get; set; }
        public float _910 { get; set; }
        public float _911 { get; set; }
        public float _912 { get; set; }
        public float _914 { get; set; }
        public float _000 { get; set; }
        public float _101 { get; set; }
        public float _103 { get; set; }
        public float _104 { get; set; }
        public float _105 { get; set; }
        public float _106 { get; set; }
        public float _109 { get; set; }
        public float _110 { get; set; }
        public float _112 { get; set; }
        public float _115 { get; set; }
        public float _117 { get; set; }
       
        public float _125 { get; set; }
        public float _200 { get; set; }
        public float _201 { get; set; }
        public float _202 { get; set; }
        public float _203 { get; set; }
        public float _204 { get; set; }
        public float _904 { get; set; }
        public float _913 { get; set; }
        public int _923 { get; set; }
    }

    public class Bankresponsecodecount1
    {
        public int _100 { get; set; }
        public int _102 { get; set; }
        public int _107 { get; set; }
        public int _111 { get; set; }
        public int _114 { get; set; }
        public int _116 { get; set; }
        public int _118 { get; set; }
        public int _119 { get; set; }
        public int _120 { get; set; }
        public int _121 { get; set; }
        public int _400 { get; set; }
        public int _908 { get; set; }
        public int _909 { get; set; }
        public int _910 { get; set; }
        public int _911 { get; set; }
        public int _912 { get; set; }
        public int _914 { get; set; }
        public int _000 { get; set; }

        public int _101 { get; set; }
        public int _103 { get; set; }
        public int _104 { get; set; }
        public int _105 { get; set; }
        public int _106 { get; set; }
        public int _109 { get; set; }
        public int _110 { get; set; }
        public int _112 { get; set; }
        public int _115 { get; set; }
        public int _117 { get; set; }

        public int _125 { get; set; }
        public int _200 { get; set; }
        public int _201 { get; set; }
        public int _202 { get; set; }
        public int _203 { get; set; }
        public int _204 { get; set; }
        public int _904 { get; set; }
        public int _913 { get; set; }
        public int _923 { get; set; }


    }

    public class DailyInwardsummary
    {
        public string bankCode { get; set; }
        public string bankName { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public Bankresponsecode2 bankResponseCode { get; set; }
        public Bankresponsecodecount2 bankResponseCodeCount { get; set; }
        public float inFailed { get; set; }
    }

    public class Bankresponsecode2
    {
        public float _000 { get; set; }
        public float _910 { get; set; }
        public float _400 { get; set; }
        public float _909 { get; set; }
        public float _912 { get; set; }
        public float _100 { get; set; }
        public float _114 { get; set; }
        public float _911 { get; set; }
        public float _111 { get; set; }
        public float _119 { get; set; }
        public float _118 { get; set; }
        public float _121 { get; set; }
        public float _107 { get; set; }
        public float _908 { get; set; }
        public float _116 { get; set; }
        public float _101 { get; set; }
        public float _103 { get; set; }
        public float _104 { get; set; }
        public float _105 { get; set; }
        public float _106 { get; set; }
        public float _109 { get; set; }
        public float _110 { get; set; }
        public float _112 { get; set; }
        public float _115 { get; set; }
        public float _117 { get; set; }

        public float _125 { get; set; }
        public float _200 { get; set; }
        public float _201 { get; set; }
        public float _202 { get; set; }
        public float _203 { get; set; }
        public float _204 { get; set; }
        public float _904 { get; set; }
        public float _913 { get; set; }
        public float _923 { get; set; }
        public float _120 { get; set; }
    }

    public class Bankresponsecodecount2
    {
        public int _000 { get; set; }
        public int _910 { get; set; }
        public int _400 { get; set; }
        public int _909 { get; set; }
        public int _912 { get; set; }
        public int _100 { get; set; }
        public int _114 { get; set; }
        public int _911 { get; set; }
        public int _111 { get; set; }
        public int _119 { get; set; }
        public int _118 { get; set; }
        public int _121 { get; set; }
        public int _107 { get; set; }
        public int _908 { get; set; }
        public int _116 { get; set; }

        public float _101 { get; set; }
        public float _103 { get; set; }
        public float _104 { get; set; }
        public float _105 { get; set; }
        public float _106 { get; set; }
        public float _109 { get; set; }
        public float _110 { get; set; }
        public float _112 { get; set; }
        public float _115 { get; set; }
        public float _117 { get; set; }

        public float _125 { get; set; }
        public float _200 { get; set; }
        public float _201 { get; set; }
        public float _202 { get; set; }
        public float _203 { get; set; }
        public float _204 { get; set; }
        public float _904 { get; set; }
        public float _913 { get; set; }
        public float _923 { get; set; }
        public float _120 { get; set; }

    }

    public class DailyOutwardsummary
    {
        public string bankCode { get; set; }
        public string bankName { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public Bankresponsecode3 bankResponseCode { get; set; }
        public Bankresponsecodecount3 bankResponseCodeCount { get; set; }
        public float outFailed { get; set; }
    }

    public class Bankresponsecode3
    {
        public float _102 { get; set; }
        public float _114 { get; set; }
        public float _000 { get; set; }
        public float _111 { get; set; }
        public float _118 { get; set; }
        public float _912 { get; set; }
        public float _100 { get; set; }
        public float _908 { get; set; }
        public float _107 { get; set; }
        public float _120 { get; set; }
        public float _121 { get; set; }
        public float _909 { get; set; }
        public float _910 { get; set; }
        public float _911 { get; set; }
        public float _119 { get; set; }
        public float _116 { get; set; }
        public float _400 { get; set; }
        public float _914 { get; set; }
        public float _101 { get; set; }
        public float _103 { get; set; }
        public float _104 { get; set; }
        public float _105 { get; set; }
        public float _106 { get; set; }
        public float _109 { get; set; }
        public float _110 { get; set; }
        public float _112 { get; set; }
        public float _115 { get; set; }
        public float _117 { get; set; }

        public float _125 { get; set; }
        public float _200 { get; set; }
        public float _201 { get; set; }
        public float _202 { get; set; }
        public float _203 { get; set; }
        public float _204 { get; set; }
        public float _904 { get; set; }
        public float _913 { get; set; }
        public float _923 { get; set; }
    }

    public class Bankresponsecodecount3
    {
        public int _102 { get; set; }
        public int _114 { get; set; }
        public int _000 { get; set; }
        public int _111 { get; set; }
        public int _118 { get; set; }
        public int _912 { get; set; }
        public int _100 { get; set; }
        public int _908 { get; set; }
        public int _107 { get; set; }
        public int _120 { get; set; }
        public int _121 { get; set; }
        public int _909 { get; set; }
        public int _910 { get; set; }
        public int _911 { get; set; }
        public int _119 { get; set; }
        public int _116 { get; set; }
        public int _400 { get; set; }
        public int _914 { get; set; }
        public int _101 { get; set; }
        public int _103 { get; set; }
        public int _104 { get; set; }
        public int _105 { get; set; }
        public int _106 { get; set; }
        public int _109 { get; set; }
        public int _110 { get; set; }
        public int _112 { get; set; }
        public int _115 { get; set; }
        public int _117 { get; set; }

        public int _125 { get; set; }
        public int _200 { get; set; }
        public int _201 { get; set; }
        public int _202 { get; set; }
        public int _203 { get; set; }
        public int _204 { get; set; }
        public int _904 { get; set; }
        public int _913 { get; set; }
        public int _923 { get; set; }
    }

    /*** MODIFIED LAST FIVE MINUTES***/

    public class NLastFiveMinutes
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public NIndustryinward industryInward { get; set; }
        public NIndustryoutward industryOutward { get; set; }
        public NInwardsummary[] inwardSummary { get; set; }
        public NOutwardsummary[] outwardSummary { get; set; }
    }

    public class NIndustryinward
    {
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public NBankresponsecode bankResponseCode { get; set; }
        public NBankresponsecodecount bankResponseCodeCount { get; set; }
        public double inFailed { get; set; }
    }

    public class NBankresponsecode
    {
        public double _000 { get; set; }
        public double _102 { get; set; }
        public double _114 { get; set; }
        public double _111 { get; set; }
        public double _118 { get; set; }
        public double _912 { get; set; }
        public double _100 { get; set; }
        public double _908 { get; set; }
        public double _107 { get; set; }
        public double _120 { get; set; }
        public double _121 { get; set; }
        public double _909 { get; set; }
        public double _910 { get; set; }
        public double _911 { get; set; }
        public double _119 { get; set; }
        public double _116 { get; set; }
        public double _400 { get; set; }
        public double _914 { get; set; }
        public double _101 { get; set; }
        public double _103 { get; set; }
        public double _104 { get; set; }
        public double _105 { get; set; }
        public double _106 { get; set; }
        public double _109 { get; set; }
        public double _110 { get; set; }
        public double _112 { get; set; }
        public double _115 { get; set; }
        public double _117 { get; set; }
        public double _125 { get; set; }
        public double _200 { get; set; }
        public double _201 { get; set; }
        public double _202 { get; set; }
        public double _203 { get; set; }
        public double _204 { get; set; }
        public double _904 { get; set; }
        public double _913 { get; set; }
        public double _923 { get; set; }
        
    }

    public class NBankresponsecodecount
    {
        public int _000 { get; set; }
        public int _102 { get; set; }
        public int _114 { get; set; }
        public int _111 { get; set; }
        public int _118 { get; set; }
        public int _912 { get; set; }
        public int _100 { get; set; }
        public int _908 { get; set; }
        public int _107 { get; set; }
        public int _120 { get; set; }
        public int _121 { get; set; }
        public int _909 { get; set; }
        public int _910 { get; set; }
        public int _911 { get; set; }
        public int _119 { get; set; }
        public int _116 { get; set; }
        public int _400 { get; set; }
        public int _914 { get; set; }
        public int _101 { get; set; }
        public int _103 { get; set; }
        public int _104 { get; set; }
        public int _105 { get; set; }
        public int _106 { get; set; }
        public int _109 { get; set; }
        public int _110 { get; set; }
        public int _112 { get; set; }
        public int _115 { get; set; }
        public int _117 { get; set; }
        public int _125 { get; set; }
        public int _200 { get; set; }
        public int _201 { get; set; }
        public int _202 { get; set; }
        public int _203 { get; set; }
        public int _204 { get; set; }
        public int _904 { get; set; }
        public int _913 { get; set; }
        public int _923 { get; set; }
    }

    public class NIndustryoutward
    {
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public NBankresponsecode1 bankResponseCode { get; set; }
        public NBankresponsecodecount1 bankResponseCodeCount { get; set; }
        public float outFailed { get; set; }
    }

    public class NBankresponsecode1
    {
        public double _102 { get; set; }
        public double _000 { get; set; }
       public double _114 { get; set; }      
        public double _111 { get; set; }
        public double _118 { get; set; }
        public double _912 { get; set; }
        public double _100 { get; set; }
        public double _908 { get; set; }
        public double _107 { get; set; }
        public double _120 { get; set; }
        public double _121 { get; set; }
        public double _909 { get; set; }
        public double _910 { get; set; }
        public double _911 { get; set; }
        public double _119 { get; set; }
        public double _116 { get; set; }
        public double _400 { get; set; }
        public double _914 { get; set; }
        public double _101 { get; set; }
        public double _103 { get; set; }
        public double _104 { get; set; }
        public double _105 { get; set; }
        public double _106 { get; set; }
        public double _109 { get; set; }
        public double _110 { get; set; }
        public double _112 { get; set; }
        public double _115 { get; set; }
        public double _117 { get; set; }
        public double _125 { get; set; }
        public double _200 { get; set; }
        public double _201 { get; set; }
        public double _202 { get; set; }
        public double _203 { get; set; }
        public double _204 { get; set; }
        public double _904 { get; set; }
        public double _913 { get; set; }
        public double _923 { get; set; }
    }

    public class NBankresponsecodecount1
    {
        public int _102 { get; set; }
        public int _000 { get; set; }        
        public int _114 { get; set; }        
        public int _111 { get; set; }
        public int _118 { get; set; }
        public int _912 { get; set; }
        public int _100 { get; set; }
        public int _908 { get; set; }
        public int _107 { get; set; }
        public int _120 { get; set; }
        public int _121 { get; set; }
        public int _909 { get; set; }
        public int _910 { get; set; }
        public int _911 { get; set; }
        public int _119 { get; set; }
        public int _116 { get; set; }
        public int _400 { get; set; }
        public int _914 { get; set; }
        public int _101 { get; set; }
        public int _103 { get; set; }
        public int _104 { get; set; }
        public int _105 { get; set; }
        public int _106 { get; set; }
        public int _109 { get; set; }
        public int _110 { get; set; }
        public int _112 { get; set; }
        public int _115 { get; set; }
        public int _117 { get; set; }
        public int _125 { get; set; }
        public int _200 { get; set; }
        public int _201 { get; set; }
        public int _202 { get; set; }
        public int _203 { get; set; }
        public int _204 { get; set; }
        public int _904 { get; set; }
        public int _913 { get; set; }
        public int _923 { get; set; }
    }

    public class NInwardsummary
    {
        public string bankCode { get; set; }
        public string bankName { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public NBankresponsecode2 bankResponseCode { get; set; }
        public NBankresponsecodecount2 bankResponseCodeCount { get; set; }
        public double inFailed { get; set; }
    }

    public class NBankresponsecode2
    {
        public double _000 { get; set; }
        public double _102 { get; set; }
        public double _114 { get; set; }
        public double _111 { get; set; }
        public double _118 { get; set; }
        public double _912 { get; set; }
        public double _100 { get; set; }
        public double _908 { get; set; }
        public double _107 { get; set; }
        public double _120 { get; set; }
        public double _121 { get; set; }
        public double _909 { get; set; }
        public double _910 { get; set; }
        public double _911 { get; set; }
        public double _119 { get; set; }
        public double _116 { get; set; }
        public double _400 { get; set; }
        public double _914 { get; set; }
        public double _101 { get; set; }
        public double _103 { get; set; }
        public double _104 { get; set; }
        public double _105 { get; set; }
        public double _106 { get; set; }
        public double _109 { get; set; }
        public double _110 { get; set; }
        public double _112 { get; set; }
        public double _115 { get; set; }
        public double _117 { get; set; }
        public double _125 { get; set; }
        public double _200 { get; set; }
        public double _201 { get; set; }
        public double _202 { get; set; }
        public double _203 { get; set; }
        public double _204 { get; set; }
        public double _904 { get; set; }
        public double _913 { get; set; }
        public double _923 { get; set; }
    }

    public class NBankresponsecodecount2
    {
        public int _102 { get; set; }
        public int _114 { get; set; }
        public int _000 { get; set; }
        public int _111 { get; set; }
        public int _118 { get; set; }
        public int _912 { get; set; }
        public int _100 { get; set; }
        public int _908 { get; set; }
        public int _107 { get; set; }
        public int _120 { get; set; }
        public int _121 { get; set; }
        public int _909 { get; set; }
        public int _910 { get; set; }
        public int _911 { get; set; }
        public int _119 { get; set; }
        public int _116 { get; set; }
        public int _400 { get; set; }
        public int _914 { get; set; }
        public int _101 { get; set; }
        public int _103 { get; set; }
        public int _104 { get; set; }
        public int _105 { get; set; }
        public int _106 { get; set; }
        public int _109 { get; set; }
        public int _110 { get; set; }
        public int _112 { get; set; }
        public int _115 { get; set; }
        public int _117 { get; set; }
        public int _125 { get; set; }
        public int _200 { get; set; }
        public int _201 { get; set; }
        public int _202 { get; set; }
        public int _203 { get; set; }
        public int _204 { get; set; }
        public int _904 { get; set; }
        public int _913 { get; set; }
        public int _923 { get; set; }
    }

    public class NOutwardsummary
    {
        public string bankCode { get; set; }
        public string bankName { get; set; }
        public int totalVolume { get; set; }
        public int totalFailure { get; set; }
        public NBankresponsecode3 bankResponseCode { get; set; }
        public NBankresponsecodecount3 bankResponseCodeCount { get; set; }
        public float outFailed { get; set; }
    }

    public class NBankresponsecode3
    {
        public double _000 { get; set; }
        public double _102 { get; set; }
        
        public double _114 { get; set; }
        
        public double _111 { get; set; }
        public double _118 { get; set; }
        public double _912 { get; set; }
        public double _100 { get; set; }
        public double _908 { get; set; }
        public double _107 { get; set; }
        public double _120 { get; set; }
        public double _121 { get; set; }
        public double _909 { get; set; }
        public double _910 { get; set; }
        public double _911 { get; set; }
        public double _119 { get; set; }
        public double _116 { get; set; }
        public double _400 { get; set; }
        public double _914 { get; set; }
        public double _101 { get; set; }
        public double _103 { get; set; }
        public double _104 { get; set; }
        public double _105 { get; set; }
        public double _106 { get; set; }
        public double _109 { get; set; }
        public double _110 { get; set; }
        public double _112 { get; set; }
        public double _115 { get; set; }
        public double _117 { get; set; }
        public double _125 { get; set; }
        public double _200 { get; set; }
        public double _201 { get; set; }
        public double _202 { get; set; }
        public double _203 { get; set; }
        public double _204 { get; set; }
        public double _904 { get; set; }
        public double _913 { get; set; }
        public double _923 { get; set; }
    }

    public class NBankresponsecodecount3
    {
        public int _000 { get; set; }
        public int _102 { get; set; }
        public int _114 { get; set; }
        public int _111 { get; set; }
        public int _118 { get; set; }
        public int _912 { get; set; }
        public int _100 { get; set; }
        public int _908 { get; set; }
        public int _107 { get; set; }
        public int _120 { get; set; }
        public int _121 { get; set; }
        public int _909 { get; set; }
        public int _910 { get; set; }
        public int _911 { get; set; }
        public int _119 { get; set; }
        public int _116 { get; set; }
        public int _400 { get; set; }
        public int _914 { get; set; }
        public int _101 { get; set; }
        public int _103 { get; set; }
        public int _104 { get; set; }
        public int _105 { get; set; }
        public int _106 { get; set; }
        public int _109 { get; set; }
        public int _110 { get; set; }
        public int _112 { get; set; }
        public int _115 { get; set; }
        public int _117 { get; set; }
        public int _125 { get; set; }
        public int _200 { get; set; }
        public int _201 { get; set; }
        public int _202 { get; set; }
        public int _203 { get; set; }
        public int _204 { get; set; }
        public int _904 { get; set; }
        public int _913 { get; set; }
        public int _923 { get; set; }
    }

    public class BankReportRequest
    {
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
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



        /**END WEEKLY RANKING */


        /**END WEEKLY OUT VALUES **/
        public decimal In911bbw { get; set; }
        public decimal In912bbw { get; set; }
        public decimal Out121bbw { get; set; }
        public decimal Out909bbw { get; set; }

        public decimal link { get; set; }
        public string linkr { get; set; }
        public decimal linkbb { get; set; }
        public decimal linkw { get; set; }
        public string linkwr { get; set; }
        public decimal linkbbw { get; set; }

        public decimal intf { get; set; }
        public string intfr { get; set; }
        public decimal intfbb { get; set; }
        public decimal intfw { get; set; }
        public string intfwr { get; set; }
        public decimal intfbbw { get; set; }

        public decimal tf { get; set; }
        public decimal nu { get; set; }
        public decimal iu { get; set; }
        public string tfreq { get; set; }
        public string nureq { get; set; }
        public string iureq { get; set; }





        public string WeeklyDate { get; set; }
        public string headingDate { get; set; }
    }
    public class Link
    {
        public decimal link { get; set; }
        public int linkr { get; set; }
        public decimal linkbb { get; set; }
        public decimal linkw { get; set; }
        public int linkwr { get; set; }
        public decimal linkbbw { get; set; }
    }
    public class IntFace
    {
        public decimal intf { get; set; }
        public int intfr { get; set; }
        public decimal intfbb { get; set; }
        public decimal intfw { get; set; }
        public int intfwr { get; set; }
        public decimal intfbbw { get; set; }
    }
    
    public class ErrorResponse
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
    }

    public static class NumberPosition
    {
        public static string ToOrdinal(this long number)
        {
            if (number < 0) return number.ToString();
            long rem = number % 100;
            if (rem >= 11 && rem <= 13) return number + "th";

            switch (number % 10)
            {
                case 1:
                    return number + "st";
                case 2:
                    return number + "nd";
                case 3:
                    return number + "rd";
                default:
                    return number + "th";
            }
        }

        public static string ToOrdinal(this int number)
        {
            return ((long)number).ToOrdinal();
        }
    }






}