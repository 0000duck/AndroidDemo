var hebeiDirectives = angular.module('hebeiDirectives',['ngCookies','hebeiCtrls']);
hebeiDirectives.directive("hello",function($cookies,$cookieStore){
	var token = $cookies.get("LoginToken");
	var user = $cookies.get("user");
	if (token == null){
		return{
			restrict:'E',
			template:'<li role="presentation"><a href="login.html">登 陆</a>',
			replace:true
		};
	}
	else{
		return{
			restrict:'E',
			templateUrl:'tpls/userlist.html',
			replace:true
		};
	}
});
hebeiDirectives.directive("charta",function(){
	
})
//<li role="presentation"><button class="btn btn-sm btn-warning col-md-12">{{loginame}}</button><button class="btn btn-sm btn-info col-md-6" ng-click="exit(token)">退 出</button><button class="btn btn-sm btn-success col-md-6">注 册</button></li>