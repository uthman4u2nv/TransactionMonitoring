var app = angular.module("Header", []);
app.controller("HeaderCtrl", ['$scope', '$http', function ($scope, $http) {
GetUser();
function GetUser() {

    $http({
        method: 'POST',
        //url: 'https://localhost:44373/Home/GetLoggedUser',
        url: 'https://tpms.ipsl.co.ke:4433/TransactionMonitoring/Home/GetLoggedUser',
        //data: $.param(users),
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        withCredentials: false,
    }).then(function successCallback(response) {
        
        $scope.email = response.data.Email;
        $scope.mybankCode = response.data.bankCode;
        $scope.bankAdminRole=response.data.bankAdmin;
        $scope.search = response.data.bankCode;
       alert($scope.bankAdminRole);




    }, function errorCallback(response) {
        console.log(response.responseText);
    });
}
}])