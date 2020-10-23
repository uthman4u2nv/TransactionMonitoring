$(document).ready(function () {
    $('#bankTable').DataTable();
});
var app = angular.module("Bank", []);
app.controller("BankData", ['$scope', '$http', function ($scope, $http) {
    $scope.Page = "Banks";
    $scope.success = false;
    $scope.failed = false;
    $scope.btnAdd = "Add Bank";
    $scope.btnUpdate = "Update Bank";
    $scope.message = "";
    $scope.display = "show";
    

    function GetUser() {

        $http({
            method: 'POST',
            //url: 'https://localhost:44373/Home/GetLoggedUser',
            url: 'https://tpms.ipsl.co.ke:4433/TransactionMonitoring/Home/GetLoggedUser',
            //data: $.param(users),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            withCredentials: false,
        }).then(function successCallback(response) {
           //alert(response.data.bankAdmin);
            $scope.email = response.data.Email;
            $scope.bankCode = response.data.bankCode;
            $scope.bankAdminRole=response.data.bankAdmin;
            $scope.search = response.data.bankCode;
         
        }, function errorCallback(response) {
           // alert("Error:"+response.responseText);
            console.log(response.responseText);
        });
    }
    
    

      
        var d = {
           // bankCode: "BANK51",
            bankCode: $scope.bankCode

        };
        $http({
            method: 'POST',
                url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/BankDetails',
            //url: 'https://localhost:4433/TransMonAPI/api/BankDetails',
            data: $.param(d),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            withCredentials: false,
        }).then(function successCallback(response) {
            // alert(response.data.responseCode);
            if (response.data.responseCode == "00") {
                $scope.bankName = response.data.bankName;
                $scope.bankCode = response.data.bankCode;
                $scope.bankAlias = response.data.bankAlias;
                $scope.sortCode = response.data.sortCode;
                $scope.volumeThreshold = response.data.volumeThreshold;
                $scope.failureThreshold = response.data.failureThreshold;
                $scope.updatebankStatus = 1;
                //alert("T:"+response.data.bankCode);
                //$scope.success = true;
                //$scope.failed = false;

            } else {
                $scope.success = false;
                $scope.failed = true;

            }
            

        }, function errorCallback(error) {
            $scope.failed = true;
            $scope.message = error.responseText;
            
        });
    
   
    

    $scope.UpdateBank = function () {
        $scope.blackout = true;
        $scope.success = false;
        $scope.failed = false;
        $btnUpdate="Updating...";
        ////$('#loadingModal').modal('show');

        var details = {
            name: $scope.bankName,
            code: $scope.bankCode,
            alias: $scope.bankAlias,
            active: 1,
            sortCode:$scope.sortCode,
            failureThreshold:$scope.failureThreshold,
            volumeThreshold:$scope.volumeThreshold
        };


        $http({
            method: 'POST',
            //url: 'http://localhost:57868/api/AddBank',
            url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/UpdateBank',
            data: $.param(details),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            withCredentials: false,
        }).then(function successCallback(response) {
            // alert(response.data.responseCode);
            if (response.data.responseCode == "00") {
                $scope.success = true;
                $scope.failed = false;
                $scope.message = response.data.responseMessage;
                $scope.btnUpdate = "Update Bank";
                $('#loadingModal').modal('hide');

            } else {
                $scope.success = false;
                $scope.failed = true;
                $scope.message = response.data.responseMessage;
                $scope.btnUpdate = "Update Bank";
                Data.bankCode = "";
                Data.bankName = "";
                $('#loadingModal').modal('hide');
            }
            loadBanks();
            $scope.display = "show";
            $('#loadingModal').modal('hide');
            $('#detailsView').modal('hide');

        }, function errorCallback(response) {
            $scope.failed = true;
            $scope.message = error.responseText;
            loadBanks();
        });




    }
    GetUser();
}])
