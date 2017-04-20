var hebeiFilters = angular.module('hebeiFilters',['hebeiCtrls']);
hebeiFilters.filter("pushsetting",function(){
	return function(input){
		var out = "";
		if (input == 1) 
			{
				out = "短信推送";
			};
		if (input == 2) 
			{
				out = "水推送"
			};
		if (input == 3) 
			{
				out = "废气推送"
			};
		if (input == 4) 
			{
				out = "预警决策"
			};
		if (input == 5) 
			{
				out = "设备"
			};
		if (input == 6) 
			{
				out = "报警信息"
			};
		return out;
	}
});
hebeiFilters.filter("accounttype",function(){
	return function(input){
		var out = "";
		if (input == 1) 
			{
				out = "管理员"
			};
		if (input == 2) 
			{
				out = "园区领导"
			};
		if (input == 3) 
			{
				out = "企业领导"
			};
		if (input == 4) 
			{
				out = "园区管理员"
			};
		if (input == 5) 
			{
				out = "企业一般管理员"
			};
		return out;
	}
});
hebeiFilters.filter("sex",function(){
	return function(input){
		var out = "";
		if (input == 1) 
			{
				out = "男"
			};
		if (input == 2)
			{
				out = "女"
			};
		return out;
	}
});