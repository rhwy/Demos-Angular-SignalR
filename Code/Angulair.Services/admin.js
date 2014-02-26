var app = angular.module('adminapp', []);

//app.config(['$routeProvider', function ($routeProvider) { 
//    $routeProvider
//      .when('/home', {
//          templateUrl: 'views/home.html',
//          controller: 'HomeController'
//      })
//    .when('/about', {
//        templateUrl: 'views/about.html',
//        controller: 'AboutController'
//    })
//      .otherwise({
//          redirectTo: '/home'
//      });
//}]);

//myapp.controller('HomeController',['$scope',function($scope){
  
//}]);

//myapp.controller('AboutController',['$scope',function($scope){
  
//}]);

app.directive('editQuestion', function () {
    return {
        replace: true,
        restrict: 'AEC',
        scope : {
            question : '='
        },
        templateUrl: '/editQuestion.html',
        link: function(scope, elem, attrs) {
            scope.question = "enter a question";
            
        }
    };
});

app.controller('editQuestionController', [
    '$scope', '$http', function ($scope, $http) {
        
        $http({ method: 'GET', url: '/api/question/default' })
            .success(function (data) {
            $scope.question = data;
            });

        $scope.update = function () {
            
            $http({
                method: 'POST', url: '/api/question', data: $scope.question
            })
                        .success(function (data) {
                $scope.message = "ok";
            });

        };
    }
]);