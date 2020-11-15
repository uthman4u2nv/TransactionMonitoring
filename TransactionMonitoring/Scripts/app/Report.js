    var app = angular.module("Reports", []);
app.controller("ReportData", function ($scope, $location, $http, $window) {    
    $scope.blackout = false;
       // alert('yes');
        //function($scope) {
            $scope.data = [{ "agence": "CTM", "secteur": "Safi", "statutImp": "operationnel" }];

            $scope.export = function () {
                html2canvas(document.getElementById('PrintThis'), {
                    onrendered: function (canvas) {
                        var data = canvas.toDataURL();
                        var docDefinition = {
                            content: [{
                                image: data,
                                width: 500,
                            }]
                        };
                        pdfMake.createPdf(docDefinition).download("test.pdf");
                    }
                });
            }
        //}
 




        $scope.blackout = false;
        $scope.noresult = "Please select a search criteria";
		  function loadBanks() {
			  
        $http.post('http://localhost:57868/api/ListOfBanks2').then(function (response) {
        //$http.post('https://tpms.ipsl.co.ke:4433/TransMonAPI/api/ListOfBanks').then(function (response) {

            console.log(response);
            $scope.myData = response.data;
            $scope.noOfBanks = response.data.length;
            $scope.show = false;
        }, function (error) {
            $scope.show = false;
            $scope.myWelcome = error.responseText;

        })
        }

        function formatDate(date) {
            var d = new Date(date),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;

            return [year, month, day].join('-');
        }
	loadBanks();
        $scope.SearchReport = function () {    
            //$scope.dFrom = "";
            //$scope.dTo = "";
            //$scope.sCode = "";
           
            //debugger;
            $scope.blackout = true;
            $scope.responseCode = "99";
            $scope.noresult = "Loading Data....";
            var user = {
                dateFrom: formatDate($scope.datefrom),
                dateTo: formatDate($scope.dateto),
                sortCode:$scope.sortCode

            };
            $scope.dFrom = user.dateFrom;
            $scope.dTo = user.dateTo;
            $scope.sCode = user.sortCode;
            //alert(user.dateFrom+" "+user.sortCode);
            //return false;
            $http({
                method: 'POST',
                url: 'http://localhost:57868/api/GetReport',
                
                data: $.param(user),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                withCredentials: false,
            }).then(function successCallback(response) {
                // alert(response.data.responseCode);
                if (response.data.responseCode == "00") {
                    //$scope.headingDate = formatDate($scope.datefrom) + " - " + formatDate($scope.dateto);
                    $scope.headingDate = response.data.headingDate;
                    $scope.responseCode = "00";
                    $scope.In114 = response.data.In114 + "%";
                    $scope.In911 = response.data.In911 + "%";
                    $scope.In111 = response.data.In111 + "%";
                    $scope.In400 = response.data.In400 + "%";
                    $scope.In912 = response.data.In912 + "%";
                    $scope.In102 = response.data.In102 + "%";
                    $scope.In100 = response.data.In100 + "%";
                    $scope.In120 = response.data.In120 + "%";
                    $scope.In100 = response.data.In100 + "%";
                    $scope.In118 = response.data.In118 + "%";

                    $scope.In114w = response.data.In114w + "%";
                    $scope.In911w = response.data.In911w + "%";
                    $scope.In111w = response.data.In111w + "%";
                    $scope.In400w = response.data.In400w + "%";
                    $scope.In912w = response.data.In912w + "%";
                    $scope.In102w = response.data.In102w + "%";
                    $scope.In100w = response.data.In100w + "%";
                    $scope.In120w = response.data.In120w + "%";
                    $scope.In100w = response.data.In100w + "%";
                    $scope.In118w = response.data.In118w + "%";

                    $scope.Out114 = response.data.Out114 + "%";
                    $scope.Out911 = response.data.Out911 + "%";
                    $scope.Out111 = response.data.Out111 + "%";
                    $scope.Out400 = response.data.Out400 + "%";
                    $scope.Out912 = response.data.Out912 + "%";
                    $scope.Out102 = response.data.Out102 + "%";
                    $scope.Out100 = response.data.Out100 + "%";
                    $scope.Out120 = response.data.Out120 + "%";
                    $scope.Out100 = response.data.Out100 + "%";
                    $scope.Out118 = response.data.Out118 + "%";
                    $scope.Out121 = response.data.Out121 + "%";
                    $scope.Out909 = response.data.Out909 + "%";

                    $scope.Out114w = response.data.Out114w + "%";
                    $scope.Out911w = response.data.Out911w + "%";
                    $scope.Out111w = response.data.Out111w + "%";
                    $scope.Out400w = response.data.Out400w + "%";
                    $scope.Out912w = response.data.Out912w + "%";
                    $scope.Out102w = response.data.Out102w + "%";
                    $scope.Out100w = response.data.Out100w + "%";
                    $scope.Out120w = response.data.Out120w + "%";
                    $scope.Out100w = response.data.Out100w + "%";
                    $scope.Out118w = response.data.Out118w + "%";
                    $scope.Out121w = response.data.Out121w + "%";
                    $scope.Out909w = response.data.Out909w + "%";

                    $scope.In114r = response.data.In114r;
                    $scope.In118r = response.data.In118r;
                    $scope.In911r = response.data.In911r;
                    $scope.In111r = response.data.In111r;
                    $scope.In400r = response.data.In400r;
                    $scope.In912r = response.data.In912r;
                    $scope.In102r = response.data.In102r;
                    $scope.In100r = response.data.In100r;
                    $scope.In120r = response.data.In120r;

                    $scope.Out114r = response.data.Out114r;
                    $scope.Out118r = response.data.Out118r;
                    $scope.Out911r = response.data.Out911r;
                    $scope.Out111r = response.data.Out111r;
                    $scope.Out400r = response.data.Out400r;
                    $scope.Out912r = response.data.Out912r;
                    $scope.Out102r = response.data.Out102r;
                    $scope.Out100r = response.data.Out100r;
                    $scope.Out120r = response.data.Out120r;
                    $scope.Out121r = response.data.Out121r;
                    $scope.Out909r = response.data.Out909r;

                    $scope.In114bb = response.data.In114bb;
                    $scope.In118bb = response.data.In118bb;
                    $scope.In911bb = response.data.In911bb;
                    $scope.In111bb = response.data.In111bb;
                    $scope.In400bb = response.data.In400bb;
                    $scope.In912bb = response.data.In912bb;
                    $scope.In102bb = response.data.In102bb;
                    $scope.In100bb = response.data.In100bb;
                    $scope.In120bb = response.data.In120bb;

                    $scope.Out114bb = response.data.Out114bb;
                    $scope.Out118bb = response.data.Out118bb;
                    $scope.Out911bb = response.data.Out911bb;
                    $scope.Out111bb = response.data.Out111bb;
                    $scope.Out400bb = response.data.Out400bb;
                    $scope.Out912bb = response.data.Out912bb;
                    $scope.Out102bb = response.data.Out102bb;
                    $scope.Out100bb = response.data.Out100bb;
                    $scope.Out120bb = response.data.Out120bb;
                    $scope.Out121bb = response.data.Out121bb;
                    $scope.Out909bb = response.data.Out909bb;


                    $scope.Out114wr = response.data.Out114wr;
                    $scope.Out118wr = response.data.Out118wr;
                    $scope.Out911wr = response.data.Out911wr;
                    $scope.Out111wr = response.data.Out111wr;
                    $scope.Out400wr = response.data.Out400wr;
                    $scope.Out912wr = response.data.Out912wr;
                    $scope.Out102wr = response.data.Out102wr;
                    $scope.Out100wr = response.data.Out100wr;
                    $scope.Out120wr = response.data.Out120wr;
                    $scope.Out121wr = response.data.Out121wr;
                    $scope.Out909wr = response.data.Out909wr;

                    $scope.In114wr = response.data.In114wr;
                    $scope.In118wr = response.data.In118wr;
                    $scope.In911wr = response.data.In911wr;
                    $scope.In111wr = response.data.In111wr;
                    $scope.In400wr = response.data.In400wr;
                    $scope.In912wr = response.data.In912wr;
                    $scope.In102wr = response.data.In102wr;
                    $scope.In100wr = response.data.In100wr;
                    $scope.In120wr = response.data.In120wr;
                    $scope.In911bbw = response.data.In911bbw;
                    $scope.In912bbw = response.data.In912bbw;
                    $scope.Out121bbw = response.data.Out121bbw;
                    $scope.Out909bbw = response.data.Out909bbw;

                    $scope.BankName = response.data.BankName;
                    $scope.WeeklyDate = response.data.WeeklyDate;

                    $scope.link = response.data.link;
                    $scope.linkr = response.data.linkr;
                    $scope.linkbb = response.data.linkbb;
                    $scope.linkw = response.data.linkw+"%";
                    $scope.linkwr = response.data.linkwr;
                    $scope.linkbbw = response.data.linkbbw;
                    $scope.intf = response.data.intf;
                    $scope.intfr = response.data.intfr;
                    $scope.intfbb = response.data.intfbb;
                    $scope.intfw = response.data.intfw+"%";
                    $scope.intfwr = response.data.intfwr;
                    $scope.intfbbw = response.data.intfbbw;

                    $scope.cust = "0.00";
                    $scope.custr = "1st";
                    $scope.custbb = "0.00";
                    $scope.custw = "0.00";
                    $scope.custwr = "1st";

                    $scope.tf = response.data.tf;
                    $scope.nu = response.data.nu;
                    $scope.iu = response.data.iu;
                    $scope.tfreq = response.data.tfreq;
                    $scope.nureq = response.data.nureq;
                    $scope.iureq = response.data.iureq;
                    $scope.aggreq = response.data.aggreq;

                    $scope.aggr = response.data.aggr;
                    $scope.weekposition = response.data.weekposition;

                    $scope.bankrank = "1.08%";
                    $scope.bankrankw = "1.49%";

                    $scope.indrank = "1.28%";
                    $scope.indrankw = "1.35%";

                   


                } else {
                    $scope.responseCode = response.data.responseCode;
                    console.log("cons:" + response);
                    $scope.noresult = "No record found,Please refine your search";
                    //alert('something went wrong'+response);
                }
               
                
                $scope.blackout = false;

            }, function errorCallback(response) {
                
                loadBanks();
                
            }); 
                $scope.blackout = false;
            

        }
    
	})
	
   
