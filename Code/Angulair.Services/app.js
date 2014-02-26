var angulair = angular.module('angulair', ['signalrModule']);
signalrModule.value('signalrServer', '');
angulair.controller('questionController', [
    'signalrGenericHubProxy', '$scope', '$http',
    function (hubProxy, $scope,$http) {
        $scope.question = "//do you like angular?";
        var total = function() {
            return $scope.question.voteUp + $scope.question.voteDown;
        };

        $scope.yes = function() {
            return ($scope.question.voteUp * 100) /total();
        };
        $scope.no = function() {
            return ($scope.question.voteDown * 100) / total();
        };

        $http({ method: 'GET', url: '/api/question/default' })
            .success(function (data) {
                $scope.question = data;
            });

        $scope.results = {
            total: $scope.total,
            yes: $scope.yes,
            no: $scope.no
        };
    var voteHub = hubProxy(hubProxy.defaultServer, 'votes', { logging: true });
    
    voteHub.on('updateQuestionText', function (content) {
        $scope.question.content = content;
    });
    voteHub.on('updateQuestionVoteUp', function (id,current) {
        $scope.question.voteUp = current;
        
    });

    voteHub.on('updateQuestionVoteDown', function (id, current) {
        $scope.question.voteDown = current;
    });

        $scope.up = function() {
            voteHub.call('VoteUp', ['default'], function (err) {
                console.log(err);
            });
        };
        $scope.down = function () {
            voteHub.call('VoteDown', ['default'], function (err) {
                console.log(err);
            });
        };
    }]);