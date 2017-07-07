var hebei = angular.module('hebei',[
	'ngRoute','ngAnimate','hebeiCtrls','hebeiService','ngCookies','hebeiFilters','hebeiDirectives']);
hebei.config(function($routeProvider){
	$routeProvider.when('/index',{
		templateUrl:'tpls/index.html',
		controller:'body'
	}).when('/water1',{
		templateUrl:'tpls/water1.html',
		controller:'water1'
	}).when('/water2',{
		templateUrl:'tpls/water2.html',
		controller:'water2'
	}).when('/water3',{
		templateUrl:'tpls/water3.html',
		controller:'water3'
	}).when('/water4',{
		templateUrl:'tpls/water4.html',
		controller:'water4'
	}).when('/UserManagement',{
		templateUrl:'tpls/UserManagement.html',
		controller:'UserManagement'
	}).when('/CompanyManagment',{
		templateUrl:'tpls/CompanyManagment.html',
		controller:'CompanyManagment'
	}).when('/EquipmentManagment',{
		templateUrl:'tpls/EquipmentManagment.html',
		controller:'EquipmentManagment'
	}).when('/air1',{
		templateUrl:'tpls/air1.html',
		controller:'air1'
	}).when('/air2',{
		templateUrl:'tpls/air2.html',
		controller:'air2'
	}).when('/air3',{
		templateUrl:'tpls/air3.html',
		controller:'air3'
	}).when('/air4',{
		templateUrl:'tpls/air4.html',
		controller:'air4'
	}).when('/msgPushSetting',{
		templateUrl:'tpls/msgPushSetting.html',
		controller:'msgPushSetting'
	}).when('/mailPushSetting',{
		templateUrl:'tpls/mailPushSetting.html',
		controller:'mailPushSetting'
	}).when('/OverproofAnalysis',{
		templateUrl:'tpls/OverproofAnalysis.html',
		controller:'OverproofAnalysis'
	}).when('/Reporter',{
		templateUrl:'tpls/Reporter.html',
		controller:'Reporter'
	}).when('/PollutionAnalysis',{
		templateUrl:'tpls/PollutionAnalysis.html',
		controller:'PollutionAnalysis'
	}).when('/111',{
		templateUrl:'tpls/chart.html',
		controller:'123'
	}).otherwise({
		redirectTo:'/index'
	})
});



// var hebei = angular.module('hebei', [
//     'ngRoute', 'ngAnimate', 'hebeiCtrls',
//     'hebeiDirectives','ngResource'
// ]);
// hebei.config(function($routeProvider){
// 	$routeProvider.when('/index',{
// 		templateUrl:'tpls/index.html'
// 	}).when('/hello',{
// 		templateUrl:'tpls/hello.html'
// 	}).otherwise({
// 		redirectTo:'/index'
// 	})
// });
// var equpiment = angular.module('equpiment', [
//     'ngRoute', 'ngAnimate', 'hebeiCtrls',
//     'hebeiDirectives'
// ]);
// equpiment.config(function($routeProvider){
// 	$routeProvider.when('/1',{
// 		templateUrl:'tpls/equipment/1.html'
// 		//controller:
// 	}).when('/2',{
// 		templateUrl:'tpls/equipment/2.html'
// 	}).when('/3',{
// 		templateUrl:'tpls/equipment/3.html'
// 	}).when('/4',{
// 		templateUrl:'tpls/equipment/4.html'
// 	}).when('/5',{
// 		templateUrl:'tpls/equipment/5.html'
// 	}).otherwise({
// 		redirectTo:'/1'
// 	})
// });
// var login = angular.module('login', [
//     'ngRoute', 'ngAnimate', 'LoginCtrls',
//     'hebeiDirectives','ngResource'
// ]);