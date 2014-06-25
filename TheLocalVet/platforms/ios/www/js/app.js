// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
// 'starter.services' is found in services.js
// 'starter.controllers' is found in controllers.js
angular.module('thelocalvet', ['ionic', 'thelocalvet.controllers', 'thelocalvet.services', 'azure-mobile-service.module'])

.config(function($stateProvider, $urlRouterProvider) {

  // Ionic uses AngularUI Router which uses the concept of states
  // Learn more here: https://github.com/angular-ui/ui-router
  // Set up the various states which the app can be in.
  // Each state's controller can be found in controllers.js
  $stateProvider
  
  .state('main', {
    url: '/main',
    templateUrl: 'templates/main.html',
    controller: 'MainCtrl'
  })
  
  .state('MyLoc', {
    url: '/mylocation',
    templateUrl: 'templates/mylocation.html',
    controller: 'MyLocCtrl'
  })
  
  .state('Details', {
    url: '/details/:vetid',
    templateUrl: 'templates/details.html',
    controller: 'DetailCtrl'
  })
  
  .state('PlaceSearch', {
    url: '/searchPlace',
    templateUrl: 'templates/placesearch.html',
    controller: 'PlaceSearchCtrl'
  })

  // if none of the above states are matched, use this as the fallback
  $urlRouterProvider.otherwise('/main');
});