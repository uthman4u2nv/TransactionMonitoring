var app = angular.module("Dashboard", ['ngTouch', 'ngAnimate', 'ui.bootstrap']);
    //app.controller("DashboardData", function ($scope, $http, $modal, ) {
    app.controller("DashboardData", ['$scope', '$uibModal', '$http', '$interval', '$timeout', function ($scope, $uibModal, $http, $interval, $timeout) {
        $scope.blackout = false;
        //PageloadStarting();
        $scope.setName = function (name) {
            // a//lert(name);
            $scope.bName = name;
        }
    
//
        $scope.Page = "Dashboard";
        $scope.openModalRefreshdata=function(data){
            $scope.nameb = $scope.bName;
            //$scope.bCode=code;
            var details = {
                bankCode: $scope.bCode,
                bankData:$scope.bankData,
                dailyData:$scope.dailyData

            };

            $http({
                method: 'POST',
                //url: 'http://localhost:57868/api/TransAnalytics',
                //url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/TransAnalytics',
                //url: 'https://localhost:4433/TransMonAPI/api/TransAnalytics',
                url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/NewTransAnalytics',
                data: $.param(details),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                withCredentials: false,
            }).then(function successCallback(response) {
                if (response.data.responseCode == "00") {
                    $scope.totalVolume = response.data.outtotalVolume;
                    $scope.totalFailure = response.data.outtotalFailure;
                    $scope.failureRate = response.data.outfailureRate;
                    $scope.successrate = response.data.outsuccess;
                    $scope.error102 = response.data.outerror102Rate;
                    $scope.error912 = response.data.outerror912Rate;
                    $scope.error100=response.data.outerror100Rate;
                    $scope.error114=response.data.outerror114Rate;
                    $scope.error908=response.data.outerror908Rate;
                    $scope.error111=response.data.outerror111Rate;
                    $scope.error400=response.data.outerror400Rate;
                     $scope.error909=response.data.outerror909Rate;
                   

                    $scope.IntotalVolume = response.data.intotalVolume;
                    $scope.IntotalFailure = response.data.intotalFailure;
                    $scope.InfailureRate = response.data.infailureRate;
                    $scope.Insuccessrate = response.data.insuccess;

                    $scope.Inerror102 = response.data.inerror102;
                    $scope.Inerror912 = response.data.inerror912;
                    $scope.Inerror100=response.data.inerror100Rate;
                    $scope.Inerror114=response.data.inerror114Rate;
                    $scope.Inerror908=response.data.inerror908Rate;
                    $scope.Inerror111=response.data.inerror111Rate;
                    $scope.Inerror400=response.data.inerror400Rate;
                    $scope.Inerror909=response.data.inerror909Rate;


                    $scope.TransactionPeriod=response.data.transactionperiod;

                    $scope.volumeFailedTxnIn=response.data.volumeFailedTxnIn;
                    
                    $scope.totalOutdaily=response.data.totalOutdaily;

                    $scope.totalInDaily=response.data.dailyTxnVolumeIn;
                    $scope.totalDailyFailedIn=response.data.totalDailyFailedIn;
                    $scope.totalDailyFailedOut=response.data.totalDailyFailedOut;
                    $scope.dailySuccessVolumeOut=response.data.dailySuccessVolumeOut;
                    $scope.dailySuccessVolumeIn=response.data.dailySuccessVolumeIn;
                    $scope.dailySuccessRateIn=response.data.dailySuccessRateIn;

                    //dailyout
                    $scope.totalOutdaily=response.data.dailyTxnVolumeOut;
                    $scope.totalDailyFailedOut=response.data.volumeFailedTxnOut;
                    $scope.dailyFailureRateOut=response.data.dailyFailureRateOut;

                    //dailyout status
                    $scope.dailySuccessRateOut=response.data.dailySuccessRateOut;
                   $scope.dailySuccessVolumeOut=response.data.dailySuccessVolumeOut;
                   $scope.outDaily_100=response.data.outDaily_100;
						$scope.outDaily_107=response.data.outDaily_107;
						$scope.outDaily_111=response.data.outDaily_111;
						$scope.outDaily_114=response.data.outDaily_114;
						$scope.outDaily_116=response.data.outDaily_116;
						$scope.outDaily_118=response.data.outDaily_118;
						$scope.outDaily_119=response.data.outDaily_119;
						$scope.outDaily_121=response.data.outDaily_121;
						$scope.outDaily_400=response.data.outDaily_400;
						$scope.outDaily_908=response.data.outDaily_908;
						$scope.outDaily_909=response.data.outDaily_909;
						$scope.outDaily_910=response.data.outDaily_910;
						$scope.outDaily_911=response.data.outDaily_911;
						$scope.outDaily_912=response.data.outDaily_912;						
						$scope.outDailyRate_100=response.data.outDailyRate_100;
						$scope.outDailyRate_107=response.data.outDailyRate_107;
						$scope.outDailyRate_111=response.data.outDailyRate_111;
						$scope.outDailyRate_114=response.data.outDailyRate_114;
						$scope.outDailyRate_116=response.data.outDailyRate_116;
						$scope.outDailyRate_118=response.data.outDailyRate_118;
						$scope.outDailyRate_119=response.data.outDailyRate_119;
						$scope.outDailyRate_121=response.data.outDailyRate_121;
						$scope.outDailyRate_400=response.data.outDailyRate_400;
						$scope.outDailyRate_908=response.data.outDailyRate_908;
						$scope.outDailyRate_909=response.data.outDailyRate_909;
						$scope.outDailyRate_910=response.data.outDailyRate_910;
						$scope.outDailyRate_911=response.data.outDailyRate_911;
						$scope.outDailyRate_912=response.data.outDailyRate_912;

                        //daily in status
                        $scope.inDaily_100=response.data.inDaily_100;
						$scope.inDaily_107=response.data.inDaily_107;
						$scope.inDaily_111=response.data.inDaily_111;
						$scope.inDaily_114=response.data.inDaily_114;
						$scope.inDaily_116=response.data.inDaily_116;
						$scope.inDaily_118=response.data.inDaily_118;
						$scope.inDaily_119=response.data.inDaily_119;
						$scope.inDaily_121=response.data.inDaily_121;
						$scope.inDaily_400=response.data.inDaily_400;
						$scope.inDaily_908=response.data.inDaily_908;
						$scope.inDaily_909=response.data.inDaily_909;
						$scope.inDaily_910=response.data.inDaily_910;
						$scope.inDaily_911=response.data.inDaily_911;
						$scope.inDaily_912=response.data.inDaily_912;
						
						$scope.inDailyRate_100=response.data.inDailyRate_100;
						$scope.inDailyRate_107=response.data.inDailyRate_107;
						$scope.inDailyRate_111=response.data.inDailyRate_111;
						$scope.inDailyRate_114=response.data.inDailyRate_114;
						$scope.inDailyRate_116=response.data.inDailyRate_116;
						$scope.inDailyRate_118=response.data.inDailyRate_118;
						$scope.inDailyRate_119=response.data.inDailyRate_119;
						$scope.inDailyRate_121=response.data.inDailyRate_121;
						$scope.inDailyRate_400=response.data.inDailyRate_400;
						$scope.inDailyRate_908=response.data.inDailyRate_908;
						$scope.inDailyRate_909=response.data.inDailyRate_909;
						$scope.inDailyRate_910=response.data.inDailyRate_910;
						$scope.inDailyRate_911=response.data.inDailyRate_911;
						$scope.inDailyRate_912=response.data.inDailyRate_912;

                        $scope.inerror912Rate=response.data.inerror912Rate;
						$scope.inerror100Rate=response.data.inerror100Rate;
						$scope.inerror114Rate=response.data.inerror114Rate;
						$scope.inerror102Rate=response.data.inerror102Rate;
						$scope.inerror908Rate=response.data.inerror908Rate;
						$scope.inerror111Rate=response.data.inerror111Rate;
						$scope.inerror400Rate=response.data.inerror400Rate;
						$scope.inerror909Rate=response.data.inerror909Rate;
						$scope.inerror107Rate=response.data.inerror107Rate;
						$scope.inerror116Rate=response.data.inerror116Rate;
						$scope.inerror118Rate=response.data.inerror118Rate;
						$scope.inerror119Rate=response.data.inerror119Rate;
						$scope.inerror121Rate=response.data.inerror121Rate;
						$scope.inerror910Rate=response.data.inerror910Rate;
						$scope.inerror911Rate=response.data.inerror911Rate;

                        /** LATEST ADDTION */
                        $scope.outerror102=response.data.outerror102;
                        $scope.outerror912=response.data.outerror912;
                        $scope.outerror100=response.data.outerror100;
                        $scope.outerror114=response.data.outerror114;
                        $scope.outerror908=response.data.outerror908;
                        $scope.outerror111=response.data.outerror111;
                        $scope.outerror400=response.data.outerror400;
                        $scope.outerror909=response.data.outerror909;

                        $scope.outerror107=response.data.outerror107;
                        $scope.outerror116=response.data.outerror116;
                        $scope.outerror118=response.data.outerror118;
                        $scope.outerror119=response.data.outerror119;
                        $scope.outerror121=response.data.outerror121;
                        $scope.outerror910=response.data.outerror910;
                        $scope.outerror911=response.data.outerror911;

                        $scope.outerror107Rate=response.data.outerror107Rate;
                        $scope.outerror116Rate=response.data.outerror116Rate;
                        $scope.outerror118Rate=response.data.outerror118Rate;
                        $scope.outerror119Rate=response.data.outerror119Rate;
                        $scope.outerror121Rate=response.data.outerror121Rate;
                        $scope.outerror910Rate=response.data.outerror910Rate;
                        $scope.outerror911Rate=response.data.outerror911Rate;

                        /** NEW ADDITION (MORE ERROR CODES**/
                        $scope.outerror101=response.data.outerror101;
                        $scope.outerror103=response.data.outerror103;
                        $scope.outerror104=response.data.outerror104;
                        $scope.outerror105=response.data.outerror105;
                        $scope.outerror106=response.data.outerror106;
                        $scope.outerror109=response.data.outerror109;
                        $scope.outerror110=response.data.outerror110;
                        $scope.outerror112=response.data.outerror112;
                        $scope.outerror115=response.data.outerror115;
                        $scope.outerror117=response.data.outerror117;
                        $scope.outerror120=response.data.outerror120;
                        $scope.outerror125=response.data.outerror125;
                        $scope.outerror200=response.data.outerror200;
                        $scope.outerror201=response.data.outerror201;
                        $scope.outerror202=response.data.outerror202;
                        $scope.outerror203=response.data.outerror203;
                        $scope.outerror204=response.data.outerror204;
                        $scope.outerror904=response.data.outerror904;
                        $scope.outerror913=response.data.outerror913;
                        $scope.outerror914=response.data.outerror914;
                        $scope.outerror923=response.data.outerror923;
                        /** END NEW ADDITION**/
                    
                }else{
                    $scope.totalVolume = "NA";
                    $scope.totalFailure = "NA";
                    $scope.failureRate = "NA";
                    $scope.successrate = "NA";
                    $scope.error102 = "NA";
                    $scope.error912 = "NA"; 
                }
            }, function errorCallback(response) {
               // $windows.alert(response.responseText)
            });
        }
        $scope.openModal = function (name, code) {
            //alert($scope.dailyData);
            $scope.nameb = name;
            $scope.bName=name;
            $scope.bCode=code;
            var details = {
                bankCode: code,
                bankData:$scope.bankData,
                dailyData:$scope.dailyData

            };

            $http({
                method: 'POST',
                //url: 'http://localhost:57868/api/TransAnalytics',
                //url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/TransAnalytics',
                //url: 'https://localhost:4433/TransMonAPI/api/TransAnalytics',
                url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/NewTransAnalytics',
                data: $.param(details),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                withCredentials: false,
            }).then(function successCallback(response) {

                //alert("Response: "+response.data.responseCode);

                if (response.data.responseCode == "00") {
                    $scope.totalVolume = response.data.outtotalVolume;
                    $scope.totalFailure = response.data.outtotalFailure;
                    $scope.failureRate = response.data.outfailureRate;
                    $scope.successrate = response.data.outsuccess;
                    $scope.error102 = response.data.outerror102;
                    $scope.error912 = response.data.outerror912;
                    $scope.error100=response.data.outerror100Rate;
                    $scope.error114=response.data.outerror114Rate;
                    $scope.error908=response.data.outerror908Rate;
                    $scope.error111=response.data.outerror111Rate;
                    $scope.error400=response.data.outerror400Rate;
                     $scope.error909=response.data.outerror909Rate;
                    $scope.error909=response.data.outerror909Rate;

                    $scope.IntotalVolume = response.data.intotalVolume;
                    $scope.IntotalFailure = response.data.intotalFailure;
                    $scope.InfailureRate = response.data.infailureRate;
                    $scope.Insuccessrate = response.data.insuccess;
                    $scope.Inerror102 = response.data.inerror102;
                    $scope.Inerror912 = response.data.inerror912;
                    $scope.Inerror100=response.data.inerror100Rate;
                    $scope.Inerror114=response.data.inerror114Rate;
                    $scope.Inerror908=response.data.inerror908Rate;
                    $scope.Inerror111=response.data.inerror111Rate;
                    $scope.Inerror400=response.data.inerror400Rate;
                     $scope.Inerror909=response.data.inerror909Rate;
                    $scope.TransactionPeriod=response.data.transactionperiod;
                    $scope.volumeFailedTxnIn=response.data.volumeFailedTxnIn;
                    $scope.dailyFailureRateIn=response.data.dailyFailureRateIn;
                    $scope.totalOutdaily=response.data.totalOutdaily;

                    $scope.totalInDaily=response.data.dailyTxnVolumeIn;
                    $scope.totalDailyFailedIn=response.data.totalDailyFailedIn;
                    $scope.totalDailyFailedOut=response.data.totalDailyFailedOut;
                    $scope.dailySuccessVolumeOut=response.data.dailySuccessVolumeOut;
                    $scope.dailySuccessVolumeIn=response.data.dailySuccessVolumeIn;
                    $scope.dailySuccessRateIn=response.data.dailySuccessRateIn;

                    //dailyout
                    $scope.totalOutdaily=response.data.dailyTxnVolumeOut;
                    $scope.totalDailyFailedOut=response.data.volumeFailedTxnOut;
                    $scope.dailyFailureRateOut=response.data.dailyFailureRateOut;

                    //dailyout status
                    $scope.dailySuccessRateOut=response.data.dailySuccessRateOut;
                   $scope.dailySuccessVolumeOut=response.data.dailySuccessVolumeOut;
                   $scope.outDaily_100=response.data.outDaily_100;
						$scope.outDaily_107=response.data.outDaily_107;
						$scope.outDaily_111=response.data.outDaily_111;
						$scope.outDaily_114=response.data.outDaily_114;
						$scope.outDaily_116=response.data.outDaily_116;
						$scope.outDaily_118=response.data.outDaily_118;
						$scope.outDaily_119=response.data.outDaily_119;
						$scope.outDaily_121=response.data.outDaily_121;
						$scope.outDaily_400=response.data.outDaily_400;
						$scope.outDaily_908=response.data.outDaily_908;
						$scope.outDaily_909=response.data.outDaily_909;
						$scope.outDaily_910=response.data.outDaily_910;
						$scope.outDaily_911=response.data.outDaily_911;
						$scope.outDaily_912=response.data.outDaily_912;						
						$scope.outDailyRate_100=response.data.outDailyRate_100;
						$scope.outDailyRate_107=response.data.outDailyRate_107;
						$scope.outDailyRate_111=response.data.outDailyRate_111;
						$scope.outDailyRate_114=response.data.outDailyRate_114;
						$scope.outDailyRate_116=response.data.outDailyRate_116;
						$scope.outDailyRate_118=response.data.outDailyRate_118;
						$scope.outDailyRate_119=response.data.outDailyRate_119;
						$scope.outDailyRate_121=response.data.outDailyRate_121;
						$scope.outDailyRate_400=response.data.outDailyRate_400;
						$scope.outDailyRate_908=response.data.outDailyRate_908;
						$scope.outDailyRate_909=response.data.outDailyRate_909;
						$scope.outDailyRate_910=response.data.outDailyRate_910;
						$scope.outDailyRate_911=response.data.outDailyRate_911;
						$scope.outDailyRate_912=response.data.outDailyRate_912;

                        //daily in status
                        $scope.inDaily_100=response.data.inDaily_100;
						$scope.inDaily_107=response.data.inDaily_107;
						$scope.inDaily_111=response.data.inDaily_111;
						$scope.inDaily_114=response.data.inDaily_114;
						$scope.inDaily_116=response.data.inDaily_116;
						$scope.inDaily_118=response.data.inDaily_118;
						$scope.inDaily_119=response.data.inDaily_119;
						$scope.inDaily_121=response.data.inDaily_121;
						$scope.inDaily_400=response.data.inDaily_400;
						$scope.inDaily_908=response.data.inDaily_908;
						$scope.inDaily_909=response.data.inDaily_909;
						$scope.inDaily_910=response.data.inDaily_910;
						$scope.inDaily_911=response.data.inDaily_911;
						$scope.inDaily_912=response.data.inDaily_912;
						
						$scope.inDailyRate_100=response.data.inDailyRate_100;
						$scope.inDailyRate_107=response.data.inDailyRate_107;
						$scope.inDailyRate_111=response.data.inDailyRate_111;
						$scope.inDailyRate_114=response.data.inDailyRate_114;
						$scope.inDailyRate_116=response.data.inDailyRate_116;
						$scope.inDailyRate_118=response.data.inDailyRate_118;
						$scope.inDailyRate_119=response.data.inDailyRate_119;
						$scope.inDailyRate_121=response.data.inDailyRate_121;
						$scope.inDailyRate_400=response.data.inDailyRate_400;
						$scope.inDailyRate_908=response.data.inDailyRate_908;
						$scope.inDailyRate_909=response.data.inDailyRate_909;
						$scope.inDailyRate_910=response.data.inDailyRate_910;
						$scope.inDailyRate_911=response.data.inDailyRate_911;
						$scope.inDailyRate_912=response.data.inDailyRate_912;

                        $scope.inerror912Rate=response.data.inerror912Rate;
						$scope.inerror100Rate=response.data.inerror100Rate;
						$scope.inerror114Rate=response.data.inerror114Rate;
						$scope.inerror102Rate=response.data.inerror102Rate;
						$scope.inerror908Rate=response.data.inerror908Rate;
						$scope.inerror111Rate=response.data.inerror111Rate;
						$scope.inerror400Rate=response.data.inerror400Rate;
						$scope.inerror909Rate=response.data.inerror909Rate;
						$scope.inerror107Rate=response.data.inerror107Rate;
						$scope.inerror116Rate=response.data.inerror116Rate;
						$scope.inerror118Rate=response.data.inerror118Rate;
						$scope.inerror119Rate=response.data.inerror119Rate;
						$scope.inerror121Rate=response.data.inerror121Rate;
						$scope.inerror910Rate=response.data.inerror910Rate;
						$scope.inerror911Rate=response.data.inerror911Rate;

                        /**LATEST ADDITION**/
                        $scope.outerror102=response.data.outerror102;
                        $scope.outerror912=response.data.outerror912;
                        $scope.outerror100=response.data.outerror100;
                        $scope.outerror114=response.data.outerror114;
                        $scope.outerror908=response.data.outerror908;
                        $scope.outerror111=response.data.outerror111;
                        $scope.outerror400=response.data.outerror400;
                        $scope.outerror909=response.data.outerror909;

                        $scope.outerror107=response.data.outerror107;
                        $scope.outerror116=response.data.outerror116;
                        $scope.outerror118=response.data.outerror118;
                        $scope.outerror119=response.data.outerror119;
                        $scope.outerror121=response.data.outerror121;
                        $scope.outerror910=response.data.outerror910;
                        $scope.outerror911=response.data.outerror911;

                        $scope.outerror107Rate=response.data.outerror107Rate;
                        $scope.outerror116Rate=response.data.outerror116Rate;
                        $scope.outerror118Rate=response.data.outerror118Rate;
                        $scope.outerror119Rate=response.data.outerror119Rate;
                        $scope.outerror121Rate=response.data.outerror121Rate;
                        $scope.outerror910Rate=response.data.outerror910Rate;
                        $scope.outerror911Rate=response.data.outerror911Rate;

                        /** NEW ADDITION (MORE ERROR CODES**/
                        $scope.outerror101=response.data.outerror101;
                        $scope.outerror103=response.data.outerror103;
                        $scope.outerror104=response.data.outerror104;
                        $scope.outerror105=response.data.outerror105;
                        $scope.outerror106=response.data.outerror106;
                        $scope.outerror109=response.data.outerror109;
                        $scope.outerror110=response.data.outerror110;
                        $scope.outerror112=response.data.outerror112;
                        $scope.outerror115=response.data.outerror115;
                        $scope.outerror117=response.data.outerror117;
                        $scope.outerror120=response.data.outerror120;
                        $scope.outerror125=response.data.outerror125;
                        $scope.outerror200=response.data.outerror200;
                        $scope.outerror201=response.data.outerror201;
                        $scope.outerror202=response.data.outerror202;
                        $scope.outerror203=response.data.outerror203;
                        $scope.outerror204=response.data.outerror204;
                        $scope.outerror904=response.data.outerror904;
                        $scope.outerror913=response.data.outerror913;
                        $scope.outerror914=response.data.outerror914;
                        $scope.outerror923=response.data.outerror923;
                        /** END NEW ADDITION**/
                        /** GOOGLE CHARt BEGIN **/
                       //drawBackgroundColor(response.data.FailureRateList[0]);
                        /** GOOGLE CHARt END **/
                /** HIGHCHART BEGIN **/
                
                   
                   /* var log = [];
                    var log2 = [];
                    // alert(key + ': ' + value);
                    //alert("length" + response.data.FailureRateList.length);
                    for (var i = 0; i < response.data.FailureRateList.length; i++) {
                        angular.forEach(response.data.FailureRateList[i], function (value, key) {
                        //alert(value);
                           this.push(value);
                        }, log);
                    }
                    for (var i = 0; i < response.data.FailureRateListIn.length; i++) {
                        angular.forEach(response.data.FailureRateListIn[i], function (value,key) {
                             //alert(key + ': ' + value*1000);
                             //alert("x:"+value,"y:"+key);
                             //{x:1320070400, y:1},
                             //objects.push([date.getMonth(), v.qnt.toNum()]);
                            this.push(value)
                        }, log2);
                    }*/

                    $scope.jsonArray = [];
            for(var i=0;i<response.data.FailureRateList.length;i++)
            {
                $scope.jsonArray[i]={
                    name:response.data.FailureRateList[i].Date*1000,
                    categories:response.data.FailureRateList[i].Date*1000,
                    y: response.data.FailureRateList[i].FailureRate,

                }
               
            }
         $scope.jsonArray2 = [];
            for(var i=0;i<response.data.FailureRateListIn.length;i++)
            {
                $scope.jsonArray2[i]={
                    name:response.data.FailureRateListIn[i].Date*1000,
                    categories:response.data.FailureRateListIn[i].Date*1000,
                    y: response.data.FailureRateListIn[i].FailureRate,

                }
             
            }


             
                    var a = new Date();
                    var currentDateString = (a.getMonth() + 1) + '/' + a.getDate() + '/' + a.getFullYear();
                    Highcharts.chart('container', {
                        title: {
                            text: 'Transaction Failure Rate For ' + currentDateString
                        },
                        
                        yAxis: {
                            title: {
                                text: 'Failure Rate (%)'
                            }
                        },
                       navigator: {
        enabled: false
      },
                        xAxis: {
                          
                            type: 'datetime',
                             showLastLabel:true,
                            //tickInterval:2,
                             //step: 24,
                             //minTickInterval: 24,
                             //tickPositions: [0, 1, 2, 4, 8],
           
                             //minTickInterval: moment.duration(1, 'hour').asMiliseconds(),
                  //'%H:%M',
                           // labels: {
            //format: '{value:%d-%b-%Y}',
            //format: '{value:%M}',
            //rotation:-45,
        //},
       /* labels: {
                format: '{value:%e %b %H:%M:%S}'
            }*/
                          
                        },
                        
                        legend: {
                            layout: 'vertical',
                            align: 'right',
                            verticalAlign: 'middle'
                        },

                        plotOptions: {
                            series: {
                                label: {
                                    connectorAllowed: true
                                },
                                pointStart: 0
                            }
                        },

                        

                        series: [{
                            name: "Outgoing Failure Rate",
                            data: $scope.jsonArray,
                            //data: [3, 2, 7, 0, 1, 9, 4, 1, 3, 5, 0, 0, 0, 2, 4, 5, 7, 1, 2, 3, 1, 7],
                            pointStart: Date.UTC(a.getFullYear(), a.getMonth(), a.getDate()),
                            turboThreshold:0,
                            //pointInterval: 3600 * 3000, // one hour
                            marker: {
                                enabled: false // uncomment to hide markers
                            },
                            //step: 24,
                            //tickInterval: 3600000,
                               
                        }, {
                            name: "Incoming Failure Rate",
                            data:  $scope.jsonArray2,
                           // data: [2, 5, 7, 0, 1, 9, 4, 10, 3, 5, 15, 1, 10, 12, 20, 13, 8, 1, 2, 3, 1, 7],
                            pointStart: Date.UTC(a.getFullYear(), a.getMonth(), a.getDate()),
                            turboThreshold:0,
                            //pointInterval: 3600 *3000, // one hour
                            marker: {
                                enabled: false // uncomment to hide markers
                            },
                            // step: 24,
                            //tickInterval: 3600000,
                               
                        }],

                        responsive: {
                            rules: [{
                                condition: {
                                    maxWidth: 1200
                                },
                                chartOptions: {
                                    legend: {
                                        layout: 'horizontal',
                                        align: 'center',
                                        verticalAlign: 'bottom'
                                    }
                                }
                            }]
                        }
                    });
                   
                /** CHART END **/
                /** START TRANSACTION VOLUME CHART **/
                $scope.jsonArrayV1 = [];
            for(var i=0;i<response.data.FailureRateListVolumeIn.length;i++)
            {
                $scope.jsonArrayV1[i]={
                    name:response.data.FailureRateListVolumeIn[i].Date*1000,
                    y: response.data.FailureRateListVolumeIn[i].FailureRate,

                }
               
            }
         $scope.jsonArrayV2 = [];
            for(var i=0;i<response.data.FailureRateListVolumeOut.length;i++)
            {
                $scope.jsonArrayV2[i]={
                    name:response.data.FailureRateListVolumeOut[i].Date*1000,
                    y: response.data.FailureRateListVolumeOut[i].FailureRate,

                }
             
            }


             
                    var a = new Date();
                    var currentDateString = (a.getMonth() + 1) + '/' + a.getDate() + '/' + a.getFullYear();
                    Highcharts.chart('containervolume', {
                        title: {
                            text: 'Transaction Volume For ' + currentDateString
                        },
                        yAxis: {
                            title: {
                                text: 'Transaction Volume'
                            }
                        },
                        xAxis: {
                           
                            type: 'datetime',
                            showLastLabel:true,
                            
                          
                        },

                        legend: {
                            layout: 'vertical',
                            align: 'right',
                            verticalAlign: 'middle'
                        },

                        plotOptions: {
                            series: {
                                label: {
                                    connectorAllowed: true
                                },
                                pointStart: 0
                            }
                        },

                        

                        series: [{
                           name: "Outgoing Volume",
                            data: $scope.jsonArrayV1,
                            //data: [3, 2, 7, 0, 1, 9, 4, 1, 3, 5, 0, 0, 0, 2, 4, 5, 7, 1, 2, 3, 1, 7],
                            pointStart: Date.UTC(a.getFullYear(), a.getMonth(), a.getDate()),
                            turboThreshold:0,
                            //pointInterval: 3600 * 1000, // one hour
                           // marker: {
                                //enabled: false // uncomment to hide markers
                            //}
                        }, {
                            name: "Incoming Volume",
                            data:  $scope.jsonArrayV2,
                           // data: [2, 5, 7, 0, 1, 9, 4, 10, 3, 5, 15, 1, 10, 12, 20, 13, 8, 1, 2, 3, 1, 7],
                            pointStart: Date.UTC(a.getFullYear(), a.getMonth(), a.getDate()),
                            turboThreshold:0,
                            //pointInterval: 3600 * 1000, // one hour
                            //marker: {
                                //enabled: false // uncomment to hide markers
                            //}
                        }],

                        responsive: {
                            rules: [{
                                condition: {
                                    maxWidth: 1200
                                },
                                chartOptions: {
                                    legend: {
                                        layout: 'horizontal',
                                        align: 'center',
                                        verticalAlign: 'bottom'
                                    }
                                }
                            }]
                        }
                    });
                /** END TRANSACTION VOLUME CHART **/

                        

                    
                } else {
                    $scope.totalVolume = "NA";
                    $scope.totalFailure = "NA";
                    $scope.failureRate = "NA";
                    $scope.successrate = "NA";
                    $scope.error102 = "NA";
                    $scope.error912 = "NA";


                }

            }, function errorCallback(response) {
               // $windows.alert(response.responseText)
            });

        }

        $scope.NewloadPage = function (myData) {
            //$scope.blackout = false;
            $scope.blackout = false;
            $scope.bankData=myData;
            var l = {
                MyData: myData
            }
            $http({
                method: 'POST',
               // url: 'https://localhost:4433/TransMonAPI/api/NewBankData',
                url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/NewBankData',
                data: $.param(l),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                withCredentials: false,
            }).then(function successCallback(response) {
                $('#loadingModal').modal('hide');
                $scope.blackout = false;
                $scope.myData = response.data;
                $scope.noOfBanks = response.data.length;
                $scope.IndtotalVolume=response.data.IndtotalVolume;
                $scope.IndtotalFailure=response.data.IndtotalFailure;
                $scope.IndfailureRate=response.data.IndfailureRate;
                $scope.IndOutVolume=response.data[0].IndOutVolume;
                $scope.IndInVolume=response.data[0].IndInVolume;
                //alert(response.data.IndOutVolume);
                console.log(response.data);
               //alert(response.data[0].IndInRate)

                $scope.IndOutFailed=response.data[0].IndOutRate;
                $scope.IndInFailed=response.data[0].IndInRate;
                //alert("Ind Fail Rate:"+response.data.IndInRate);
                $scope.show = false;
                $scope.openModalRefreshdata(myData);
            }, function errorCallback(response) {
                console.log(response.responseText);
                $scope.show = false;
                $scope.myWelcome = response.responseText;
            });
$scope.blackout = false;
        }

$scope.NewloadPage2 = function (myData) {           
            $scope.dailyData=myData;            
            

        }

 function PageloadStarting() {           
            $scope.blackout = false;
            
            $http({
                method: 'POST',               
                url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/PageLoadBankData2',                
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                withCredentials: false,
            }).then(function successCallback(response) {
                $scope.blackout = false;
                $scope.myData = response.data;
                $scope.noOfBanks = response.data.length;
                $scope.IndOutFailed=0;
                $scope.IndInFailed=0;
                $scope.IndInVolume=0;
                $scope.IndOutVolume=0;
                $scope.show = false;
                $('#loadingModal').modal('show');
            }, function errorCallback(response) {
                console.log(response.responseText);
                $scope.show = false;
                $scope.myWelcome = response.responseText;
            });
        }



PageloadStarting();
    }]);

    
    function dailyconnect() {
        var socket = new SockJS('https://tpms.ipsl.co.ke:6443/ipsl-endpoint');
        //var socket = new SockJS('https://localhost:6443/ipsl-endpoint');
        //var socket = new SockJS('https://169.254.213.151:6500/ipsl-endpoint');
        var client = Stomp.over(socket);
        
        console.log("About establishing connection for Daily transactions...");
        
        client.connect({}, function (frame) {
            client.subscribe('/ipsl-broker/today/client', function (data) {
            //angular.element(document.querySelector('.mySelector'));
                angular.element(document.querySelector('#loadPageData')).scope().NewloadPage2(data.body);
               // console.log(JSON.stringify(data.body));
            });
            console.info("Connected for daily transactions...");
        });
    }

    function connect() {
        var socket = new SockJS('https://tpms.ipsl.co.ke:6443/ipsl-endpoint');
        //var socket = new SockJS('https://localhost:6443/ipsl-endpoint');
        //var socket = new SockJS('https://169.254.213.151:6500/ipsl-endpoint');
        var client = Stomp.over(socket);
        
        console.log("About establishing connection for 5minutes transactions...");
        
        client.connect({}, function (frame) {
            client.subscribe('/ipsl-broker/client', function (data) {
            //angular.element(document.querySelector('.mySelector'));
                angular.element(document.querySelector('#loadPageData')).scope().NewloadPage(data.body);
                console.log(JSON.stringify(data.body));
            });
            console.info("Connected...");
        });
    }

   
    $(document).ready(function () {
        
        //start web socket conneciton
        dailyconnect();
        connect();
        
    });

