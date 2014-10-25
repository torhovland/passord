var app = angular.module('todo', [
    'oauth'
]);

angular.module('todo').config(function($locationProvider) {
    $locationProvider.html5Mode(true).hashPrefix('!');
});

angular.module('todo')
    .controller('todoController', function($scope, $sce, $timeout, $http, AccessToken, Profile) {
        $scope.$on('oauth:login', function(event, token) {
            $scope.accessToken = token.access_token;
            $scope.decodedToken = $sce.trustAsHtml(decodeJwt(token.access_token));
            $http.defaults.headers.common.Authorization = 'Bearer ' + token.access_token;
        });

        $scope.$on('oauth:logout', function(event) {
            $scope.accessToken = null;
        });

        $scope.getProfile = function() {
            $scope.profile = Profile.get();
        };

        $scope.getTodos = function() {
        	loadTodos();
        };

        $scope.postTodo = function(item, event) {
            console.log("--> Submitting form");
            var dataObject = {
                Text: $scope.todo,
            };

            var responsePromise = $http.post("https://localhost/TodoApi/api/todo", dataObject, {});
            responsePromise.success(function(dataFromServer, status, headers, config) {
                console.log(dataFromServer.title);
                loadTodos();
            });
            responsePromise.error(function(data, status, headers, config) {
                alert("Submitting form failed!");
            });
        }

        function loadTodos() {
            console.log("Loading todos.");

            var responsePromise = $http.get("https://localhost/TodoApi/api/todo");
            responsePromise.success(function(dataFromServer, status, headers, config) {
                console.log("Returned data.")
                $scope.todos = dataFromServer;
            });
            responsePromise.error(function(data, status, headers, config) {
                alert("Getting todos failed!");
            });
        }
    });

  var keyStr = "ABCDEFGHIJKLMNOP" +
               "QRSTUVWXYZabcdef" +
               "ghijklmnopqrstuv" +
               "wxyz0123456789+/" +
               "=";

  function decodeJwt(input) {
    var decoded = "";
    var parts = input.split(".");

    for (var i = 0; i < parts.length - 1; i++) {
        decoded += decode64(parts[i]).replace(/,/g, ",<br>").replace(/\}/g, "}<br>");
    };

    return decoded;
  }

  function decode64(input) {
     var output = "";
     var chr1, chr2, chr3 = "";
     var enc1, enc2, enc3, enc4 = "";
     var i = 0;

     // remove all characters that are not A-Z, a-z, 0-9, +, /, or =
     var base64test = /[^A-Za-z0-9\+\/\=]/g;
     if (base64test.exec(input)) {
        alert("There were invalid base64 characters in the input text.\n" +
              "Valid base64 characters are A-Z, a-z, 0-9, '+', '/',and '='\n" +
              "Expect errors in decoding.");
     }
     input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

     do {
        enc1 = keyStr.indexOf(input.charAt(i++));
        enc2 = keyStr.indexOf(input.charAt(i++));
        enc3 = keyStr.indexOf(input.charAt(i++));
        enc4 = keyStr.indexOf(input.charAt(i++));

        chr1 = (enc1 << 2) | (enc2 >> 4);
        chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
        chr3 = ((enc3 & 3) << 6) | enc4;

        output = output + String.fromCharCode(chr1);

        if (enc3 != 64) {
           output = output + String.fromCharCode(chr2);
        }
        if (enc4 != 64) {
           output = output + String.fromCharCode(chr3);
        }

        chr1 = chr2 = chr3 = "";
        enc1 = enc2 = enc3 = enc4 = "";

     } while (i < input.length);

     return unescape(output);
  }
