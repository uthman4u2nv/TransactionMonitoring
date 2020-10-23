var app = angular.module("IPSLLogin", []);
       app.controller("LoginCtrl", function ($scope, $location, $http, $window) {
        $scope.display = false;
           $scope.btnLogin = "Sign In";
           $scope.blackout = false;
           $scope.email = localStorage.getItem("email");
           $scope.btnReset = "Reset Password";
        //$scope.myUrl = $location.absUrl();
           function Go(username, useremail, bAdmin, bUser, bCode) {
               
               var users = {
                   email:useremail,
                   name: username,
                   bankAdmin: bAdmin,
                   bankUser: bUser,
                   bankCode:bCode
               }
              // alert(users.email + " " + users.name + " " + users.bankAdmin + " " + users.bankUser + " " + bankCode);
               $http({
                   method: 'POST',
                   //url: 'https://localhost:4433/TransactionMonitoring/Home/SetUser',
                   url: 'https://tpms.ipsl.co.ke:4433/TransactionMonitoring/Home/SetUser',
                   data: $.param(users),
                   headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                   withCredentials: false,
               }).then(function successCallback(response) {
                  // if (response.data.responseCode == "00") {
                       //localStorage.setItem(response.data.email, "email");
                      // $window.location.href = "../Home/Dashboard";

                  /* } else {
                       $scope.display = true;
                       $scope.message = "Error";
                   }*/
               }, function errorCallback(response) {
                   console.log(response.responseText);
               });
           }
        $scope.LoginClick = function () {
            $scope.blackout = true;
            $scope.btnLogin = "Sign in.....";
            var user = {
                password: $scope.userpassword,
                email: $scope.useremail
            };


            $http({
                method: 'POST',
                url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/Login',
                //url: 'http://localhost:57868/api/Authenticate2',
                data: $.param(user),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                withCredentials: false,
            }).then(function successCallback(response) {
              //alert("Login response:"+response.data.status);
                if (response.data.status == "00" || response.data.status=="54") {

                    $scope.display = false;
                    $scope.message = "";
                    localStorage.setItem(response.data.email, "email");
                    //$scope.email = "";
                    //$scope.password = "";
                   // alert(response.data.bankUser);
                    Go(response.data.firstname + " " + response.data.lastname, response.data.email, response.data.bankAdmin, response.data.bankUser, response.data.bankCode);
                    //alert(response.data.firsttime);
                    //determine where the app goes
                     //$window.location.href = "../Home/Dashboard";
                    if (response.data.status=="54") {
                        $window.location.href = "../Home/ChangePassword";
                    } else {
                        $window.location.href = "../Home/Loading";
                    }
                    //$scope.blackout = false;

                   //go to homepage



                } else {
                    $scope.blackout = false;
                    $scope.display = true;
                    $scope.password = "";
                    $scope.btnLogin = "Sign In";
                    $scope.logout = "";
                    $scope.message = "Invalid Username or Password";
                }
            }, function errorCallback(response) {
                alert("Response:"+response.responseText);
                console.log(response.responseText);
            });



           }
           $scope.ResetUser = function () {
               $scope.blackout = true;
               $scope.resetsuccess = false;
               $scope.resetfailed = false;
               $scope.ResetUser = "Loading..";



               var user = {
                   email: $scope.resetemail

               };
               $http({
                   method: 'POST',
                   //url: 'http://localhost:57868/api/ResetUser2',
                   url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/ResetUser',
                   data: $.param(user),
                   headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                   withCredentials: false,
               }).then(function successCallback(response) {
                   // alert(response.data.responseCode);
                   if (response.data.responseCode == "00") {
                       $scope.resetsuccess = true;
                       $scope.resetfailed = false;
                       $scope.resetmessage = response.data.responseMessage;
                       $scope.ResetUser = "Reset User";
                       $scope.resetemail = "";


                   } else {
                       $scope.resetsuccess = false;
                       $scope.resetfailed = true;
                       $scope.resetmessage = response.data.responseMessage;
                       $scope.ResetUser = "Reset User";


                   }
                   loadBanks();
                   loadUsers();
                   $scope.display = "show";
                   $scope.blackout = false;







               }, function errorCallback(response) {
                   $scope.resetfailed = true;
                   $scope.resetmessage = error.responseText;
                   loadBanks();
                   loadUsers();
               });


               $scope.blackout = false;

           }
    })