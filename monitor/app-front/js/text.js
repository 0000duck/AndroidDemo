var app = angular.module("test",[]);
app.controller("test1",function($scope){
	$scope.example1 = [
	"haha",
	"gege",
	"gaga",
	"hehe",
	]
});
app.directive("hello",function(){
	//
	return {
		restrict:"E",
		template:"<h1>自定义指令</h1>"
	};
});
app.directive("hello1",function(){
	return {
		restrict:"A",
		template:"<h1>自定义指令1</h1>"
	};
});
app.directive("hello2",function(){
	return {
		restrict:"C",
		template:"<h1>自定义指令2</h1>"
	};
});
