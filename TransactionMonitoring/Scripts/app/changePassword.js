var app = angular.module("ChangePassword", []);
    app.controller("ChangePass", ['$scope', '$http','$window', function ($scope, $http,$window) {
        
        $scope.success = false;
        $scope.failed = false;
        $scope.btnChange = "Change Password";
        $scope.message = "";
        GetUser();

        function GetUser() {
          
            $http({
                method: 'POST',
                url: 'https://tpms.ipsl.co.ke:4433/TransactionMonitoring/Home/GetLoggedUser',
                // url: 'http://localhost/Home/SetUser',
                //data: $.param(users),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                withCredentials: false,
            }).then(function successCallback(response) {
                //alert(response.data.Email);
                    $scope.email = response.data.Email;
                    
                   

                
            }, function errorCallback(response) {
                console.log(response.responseText);
            });
        }
       
        
        $scope.ChangeMyPassword = function () {
           // alert("Changing Password"+$scope.email);
            $scope.blackout = true;
            $scope.success = false;
            $scope.failed = false;
            $scope.btnChange = "Processing";

            if ($scope.newpassword != $scope.confpassword) {
                $scope.message = "Password and confirm password do not match";
                $scope.failed = true;
                $scope.btnChange = "Change Password";
                return false;
            }

            var user = {
                oldPassword: $scope.oldpassword,
                newPassword: $scope.newpassword,
                confirmPassword: $scope.confpassword,
                email:$scope.email,
               
            };


            $http({
                method: 'POST',
                url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/ChangePassword',
                //url: 'http://localhost:57868/api/ChangePassword2',
                data: $.param(user),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                withCredentials: false,
            }).then(function successCallback(response) {
               // alert(response.data.responseCode);
                if (response.data.responseCode == "00") {
                    $scope.oldpassword = "";
                    $scope.newpassword = "";
                    $scope.confpassword = "";
                    $scope.success = true;
                    $scope.failed = false;
                    $scope.message = response.data.responseMessage;
                    $scope.btnChange = "Change Password";
                    $window.location.href = "../Home/Logout";
                } else {
                    $scope.success = false;
                    $scope.failed = true;
                    $scope.message = response.data.responseMessage;
                    $scope.btnChange = "Change Password";
                   
                }
               
                







            }, function errorCallback(response) {
                $scope.failed = true;
                    $scope.message = error.responseText;
                    loadBanks();
                    loadUsers();
            });




        }
    }])

