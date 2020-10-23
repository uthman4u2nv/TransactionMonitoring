var app = angular.module("Users", []);
    app.controller("UsersData", ['$scope', '$http', function ($scope, $http) {
        $scope.blackout = false;
        $scope.userole = ["Bank User", "Bank Admin","Industry"];
        $scope.Page = "Banks";
        $scope.success = false;
        $scope.failed = false;
        $scope.btnAdd = "Add User";
        $scope.message = "";
        $scope.display = "show";
        $scope.btnReset = "Reset User";
        $scope.btnUpdate = "Update User";
        loadUsers()
        loadBanks();
        GetUser();
        $scope.openUpdateModal = function (email,firstname,lastname,bank,role) {
            //alert(role);
            $scope.updatefirstname = firstname;
            $scope.updatelastname = lastname;
            $scope.updateemail = email;
            $scope.updatebank = bank;
            $scope.updaterole = role;
            $scope.updatesuccess = false;
            $scope.updatefailed = false;

        }
        $scope.openModal = function (email) {
            //alert(email);
            $scope.resetemail = email;

        }
         function loadBanks () {
             $http.post('https://tpms.ipsl.co.ke:4433/TransMonAPI/api/ListOfBanks').then(function (response) {
             //$http.post('http://localhost:57868/api/ListOfBanks2').then(function (response) {

                console.log(response);
                $scope.myData = response.data;
                $scope.noOfBanks = response.data.length;
                $scope.show = false;
            }, function (error) {
                $scope.show = false;
                $scope.myWelcome = error.responseText;

            })
        }
        function loadUsers() {
            //$http.post('http://localhost:57868/api/ListOfUsers2').then(function (response) {
            $http.post('https://tpms.ipsl.co.ke:4433/TransMonAPI/api/ListOfUsers').then(function (response) {

                console.log(response);
                $scope.myUsers = response.data;
                $scope.noOfBanks = response.data.length;
                $scope.show = false;
            }, function (error) {
                $scope.show = false;
                $scope.myWelcome = error.responseText;

            })
        }
        $scope.UpdateUser = function () {
            $scope.blackout = true;
            $scope.updatesuccess = false;
            $scope.updatefailed = false;
            $scope.btnUpdate = "Updating..";



            var user = {
                firstname: $scope.updatefirstname,
                lastname: $scope.updatelastname,
                //bankCode: $scope.updatebankcode,
                bankCode:"BANK53",
                roleID: $scope.updaterole,
                email: $scope.updateemail

            };
alert("FirstName:"+user.firstname+"Last Name:"+user.lastname+"Bank Code"+user.bankCode+"role id:"+user.roleID+"Email:"+user.email);
            $http({
                method: 'POST',
                //url: 'http://localhost:57868/api/UpdateUser2',
                url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/UpdateUser',
                data: $.param(user),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                withCredentials: false,
            }).then(function successCallback(response) {
                // alert(response.data.responseCode);
                if (response.data.responseCode == "00") {
                    $scope.updatesuccess = true;
                    $scope.updatefailed = false;
                    $scope.updatemessage = response.data.responseMessage;
                    $scope.btnUpdate = "Update User";


                } else {
                    //alert(response.data.responseMessage);
                    $scope.updatesuccess = false;
                    $scope.updatefailed = true;
                    $scope.updatemessage = response.data.responseMessage;
                    $scope.btnUpdate = "Update User";


                }
                loadBanks();
                loadUsers();
                $scope.display = "show";
                $scope.blackout = false;







            }, function errorCallback(response) {
                $scope.updatefailed = true;
                $scope.updatemessage = error.responseText;
                loadBanks();
                loadUsers();
            });


            $scope.blackout = false;

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
        $scope.UpdateUser2 = function () {
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
                url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/AddUser',
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
        $scope.AddUser = function () {
            $scope.blackout = true;
            $scope.success = false;
            $scope.failed = false;



            var user = {
                firstname: $scope.addfirstname,
                lastname: $scope.addlastname,
                email: $scope.addemail,
                bankCode: $scope.addbank,
                roleID:$scope.addrole
            };


            $http({
                method: 'POST',
                //url: 'http://localhost:57868/api/AddUser2',
                url: 'https://tpms.ipsl.co.ke:4433/TransMonAPI/api/AddUser',
                data: $.param(user),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                withCredentials: false,
            }).then(function successCallback(response) {
               // alert(response.data.responseCode);
                if (response.data.responseCode == "00") {
                    $scope.success = true;
                    $scope.failed = false;
                    $scope.message = response.data.responseMessage;
                    $scope.btnAdd = "Add User";
                    $scope.addfirstname = "";
                    $scope.addlastname = "";
                    $scope.addemail = "";
                    $scope.addbank = "";
                    $scope.addrole = "";

                } else {
                    $scope.success = false;
                    $scope.failed = true;
                    $scope.message = response.data.responseMessage;
                    $scope.btnAdd = "Add User";
                    Data.bankCode = "";
                    Data.bankName = "";

                }
                loadBanks();
                loadUsers();
                $scope.display = "show";
                $scope.blackout = false;







            }, function errorCallback(error) {
                $scope.failed = true;
                    $scope.message = error.responseText;
                    loadBanks();
                    loadUsers();
            });


            $scope.blackout = false;

        }
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
                $scope.bankCode = response.data.bankCode;
                $scope.search = response.data.bankCode;
                //alert($scope.search);




            }, function errorCallback(response) {
                console.log(response.responseText);
            });
        }
    }])
