var signalrModule = angular.module('signalrModule', []);
signalrModule.value('signalrServer', '');
signalrModule.factory('signalrGenericHubProxy', ['$rootScope', 'signalrServer', function ($rootScope, signalrServer) {
    function signalrGenericHubProxy(serverUrl, hubName, startOptions) {
        var connection = $.hubConnection(signalrServer);
        var proxy = connection.createHubProxy(hubName);
        connection.start(startOptions).done(function () { });

        return {
            on: function (eventName, callback) {
                if (proxy.connection.state !== $.signalR.connectionState.connected) {
                    if (callback) {
                        callback({ message: "disconnected" });
                    }
                }
                proxy.on(eventName, function (result, param2) {
                    $rootScope.$apply(function () {
                        if (callback) {
                            callback(result, param2);
                        }
                    });
                });
            },
            off: function (eventName, callback) {
                if (proxy.connection.state !== $.signalR.connectionState.connected) {
                    if (callback) {
                        callback({ message: "disconnected" });
                    }
                }
                proxy.off(eventName, function (result) {
                    $rootScope.$apply(function () {
                        if (callback) {
                            callback(result);
                        }
                    });
                });
            },
            invoke: function (methodName, callback) {
                if (proxy.connection.state !== $.signalR.connectionState.connected) {
                    if (callback) {
                        callback({ message: "disconnected" });
                    }
                }
                proxy.invoke(methodName)
                    .done(function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
            },

            call: function (methodName, args, callback) {
                if (proxy.connection.state !== $.signalR.connectionState.connected) {
                    if (callback) {
                        callback({ message: "disconnected" });
                    }
                }
                proxy.invoke.apply(proxy, $.merge([methodName], args))
                    .done(function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
            },
            connection: connection
        };
    };

    return signalrGenericHubProxy;
}]);

