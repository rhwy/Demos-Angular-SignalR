Demos-Angular-SignalR
=====================

Presenting SPA with AngularJs and 3 way binding with the SignalR real-time framework.

First presentation a French MS Techdays 2014 

It shows:
* a simple angular base app
*	how to send simple signals with signalR
* how to setup a modern .net web stack with OWIN components: static files, NancyFX, SinalR here for the different parts
* angular bi-directional binding (admin page)
* 3 way binding with votes values in home page
* a signalR directive to make this simple and possible
* dependency injection on signalR and NancyFX (with a simple in-memory implementation)
* bits of Nancy framework for all api related stuff in a simple way
* works from command line with OwinHost
* cors with signalR : setup cors in startup.cs file, host the service online somewhere and try to access signalr/nancy services from a jsbin.com sandbox.

for questions : demos@rui.fr, @rhwy

Notes:
* there is more to come!
* angulair directive will be available as a separate library with a lot more features
