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
    loadBanks();
    $scope.openModal = function (name,code) {

        $scope.updatebankName = name;
        $scope.blackout = true;
        $scope.success = false;
        $scope.failed = false;
        var d = {
            //bankCode: $scope.bankCode,
            bankCode:code

        };
        $http({
            method: 'POST',
            url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/BankDetails',
            data: $.param(d),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            withCredentials: false,
        }).then(function successCallback(response) {
            // alert(response.data.responseCode);
            if (response.data.responseCode == "00") {
                $scope.updatebankCode = response.data.bankCode;
                $scope.updatebankAlias = response.data.bankAlias;
                $scope.updateSortCode = response.data.sortCode;
                $scope.updateVolumeThreshold = response.data.volumeThreshold;
                $scope.updateFailureThreshold = response.data.failureThreshold;
                $scope.updatebankStatus = response.data.active;
                //alert("T:"+response.data.bankCode);
                //$scope.success = true;
                //$scope.failed = false;

            } else {
                $scope.success = false;
                $scope.failed = true;

            }
            loadBanks();

        }, function errorCallback(error) {
            $scope.failed = true;
            $scope.message = error.responseText;
            loadBanks();
        });
    }
    function loadBanks() {
        //$http.post('http://localhost:57868/api/ListOfBanks2').then(function (response) {
        $http.post('https://tpms.ipsl.co.ke:4433/TransMonAPI/api/ListOfBanks').then(function (response) {

            console.log(response);
            $scope.myData = response.data;
            $scope.noOfBanks = response.data.length;
            $scope.show = false;
        }, function (error) {
            $scope.show = false;
            $scope.myWelcome = error.responseText;

        })
    }
    $scope.AddBank = function (Data) {
        $scope.blackout = true;
        $scope.success = false;
        $scope.failed = false;
        $('#loadingModal').modal('show');


        var user = {
            name: Data.bankName,
            code: Data.bankCode,
            alias:Data.bankAlias,
            sortCode:$scope.addSortCode,
            failureThreshold:$scope.addFailureThreshold,
            volumeThreshold:$scope.addVolumeThreshold
        };


        $http({
            method: 'POST',
            //url: 'http://localhost:57868/api/AddBank',
            url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/AddBank',
            data: $.param(user),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            withCredentials: false,
        }).then(function successCallback(response) {
            // alert(response.data.responseCode);
            if (response.data.responseCode == "00") {
                $scope.success = true;
                $scope.failed = false;
                $scope.message = response.data.responseMessage;
                $scope.btnAdd = "Add Bank";
                Data.bankCode = "";
                Data.bankName = "";
            } else {
                $scope.success = false;
                $scope.failed = true;
                $scope.message = response.data.responseMessage;
                $scope.btnAdd = "Add Bank";
                Data.bankCode = "";
                Data.bankName = "";
            }
            loadBanks();
            $scope.display = "show";
            $('#loadingModal').modal('hide');







        }, function errorCallback(response) {
            $scope.failed = true;
            $scope.message = error.responseText;
            loadBanks();
        });




    }
    
    $scope.UpdateBank = function () {
        $scope.blackout = true;
        $scope.success = false;
        $scope.failed = false;
        $('#loadingModal').modal('show');

        var details = {
            name: $scope.updatebankName,
            code: $scope.updatebankCode,
            alias: $scope.updatebankAlias,
            active: $scope.updatebankStatus,
            sortCode:$scope.updateSortCode,
            failureThreshold:$scope.updateFailureThreshold,
            volumeThreshold:$scope.updateVolumeThreshold
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
                
            } else {
                $scope.success = false;
                $scope.failed = true;
                $scope.message = response.data.responseMessage;
                $scope.btnUpdate = "Update Bank";
                Data.bankCode = "";
                Data.bankName = "";
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
}])
